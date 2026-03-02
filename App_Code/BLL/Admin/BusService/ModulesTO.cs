using EB_Service.Commons;
using EB_Service.DAL;
using ILSystem.App_Code.Commons;
using IBM.Data.DB2.iSeries;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace EB_Service.Admin.BusService
{
    public class ModulesTO
    {
        private string m_Lib = AppConfig.GNLib;
        private string m_LastError;
        //		private const string m_TableName = "HMTB62";
        private const string m_TableName = "GNTA01";
        private string m_ConnStr;

        private string m_UserID;
        private string m_UserName;
        private int m_RoleID;
        private int m_ModuleID;
        private int m_PortalID;
        private string m_AuthorizeUser = "";


        private DateTimeFormatInfo m_DThai = new CultureInfo("th-TH", false).DateTimeFormat;

        private string m_UdpDate = DateTime.Now.ToString("yyMMdd");
        private string m_UdpTime = DateTime.Now.ToString("HHmmss");
        private string m_Program = "MODULEWEB";
        private string m_User;
        private string m_Wrkstn = "";


        private UserInfo m_UserInfo;

        private As400DAL m_da400 = new As400DAL();


        public ModulesTO()
        {
            m_DThai = new CultureInfo("th-TH", false).DateTimeFormat;
            m_da400.AS400Lib = m_Lib;
        }

        #region "Properties"
        public UserInfo UserInfomation
        {
            set
            {
                m_UserInfo = value;
                m_User = m_UserInfo.Username;
                m_Wrkstn = m_UserInfo.LocalClient;
                m_da400.UserName = m_UserInfo.Username;
                m_da400.Password = m_UserInfo.Password;

            }
        }
        public string Library
        {
            get { return m_Lib; }
            set
            {
                m_Lib = value;
                m_da400.AS400Lib = m_Lib;
            }
        }
        public string LastError
        {
            get { return m_LastError; }

        }
        public string TableName
        {
            get { return m_TableName; }

        }
        public string ConnStr
        {
            get { return m_ConnStr; }
            set { m_ConnStr = value; }
        }
        public string UserID
        {
            get { return m_UserID; }
            set { m_UserID = value; }
        }
        public string UserName
        {
            get { return m_UserName; }
            set { m_UserName = value; }
        }

        public int RoleID
        {
            get { return m_RoleID; }
            set { m_RoleID = value; }
        }

        public int ModuleID
        {
            get { return m_ModuleID; }
            set { m_ModuleID = value; }
        }

        public int PortalID
        {
            get { return m_PortalID; }
            set { m_PortalID = value; }
        }

        public string AuthorizeUser
        {
            get { return m_AuthorizeUser; }
            set { m_AuthorizeUser = value; }
        }

        public string UdpDate
        {
            get { return m_UdpDate; }
            set { m_UdpDate = value; }
        }
        public string UdpTime
        {
            get { return m_UdpTime; }
            set { m_UdpTime = value; }
        }
        public string Program
        {
            get { return m_Program; }
            set { m_Program = value; }
        }
        public string User
        {
            get { return m_User; }
            set { m_User = value; }
        }
        public string Wrkstn
        {
            get { return m_Wrkstn; }
            set { m_Wrkstn = value; }
        }

        #endregion

        public bool Create(bool usedStoreProcedure)
        {

            m_UdpDate = DateTime.Now.Date.ToString("yyMMdd", m_DThai);
            m_UdpTime = DateTime.Now.ToString("HHmmss", m_DThai);
            int affectedRows = -1;

            if (WebConfigurationManager.AppSettings["AuthenticationBy"] == "0")  //AS400 Authentication
            {
                iDB2Command cmd = new iDB2Command();
                cmd.CommandText = String.Format("INSERT INTO {0}(T01MID,T01PID,T01RID,T01AUT,T01UPD,T01UPT, "
                    + "T01PGM,T01USR,T01WKS) VALUES(@T01MID,@T01PID,@T01RID,@T01AUT,@T01UPD,@T01UPT, "
                    + "@T01PGM,@T01USR,@T01WKS)", m_TableName);

                cmd.Parameters.Add("@T01MID", m_ModuleID);
                cmd.Parameters.Add("@T01PID", m_PortalID);
                cmd.Parameters.Add("@T01RID", m_RoleID);
                cmd.Parameters.Add("@T01AUT", m_AuthorizeUser);
                cmd.Parameters.Add("@T01UPD", m_UdpDate);
                cmd.Parameters.Add("@T01UPT", m_UdpTime);
                cmd.Parameters.Add("@T01PGM", m_Program);
                cmd.Parameters.Add("@T01USR", m_User);

                string strWrkStn = m_Wrkstn;
                if (m_Wrkstn.Trim().Length > 10)
                {
                    strWrkStn = m_Wrkstn.Trim().Substring((m_Wrkstn.Trim().Length - 10), 10);
                }
                cmd.Parameters.Add("@T01WKS", strWrkStn);

                try
                {
                    affectedRows = m_da400.ExecuteNonQuery(cmd);
                }
                catch (Exception ex)
                {
                    m_LastError = ex.Message;
                    Debug.WriteLine(ex.Message);
                }
            }


            else
            {

            }
            return (affectedRows > 0);
        }

        public bool Create(string[] RolesID)
        {

            m_UdpDate = DateTime.Now.Date.ToString("yyMMdd", m_DThai);
            m_UdpTime = DateTime.Now.ToString("HHmmss", m_DThai);
            int affectedRows = -1;

            foreach (string strRoleID in RolesID)
            {
                if (WebConfigurationManager.AppSettings["AuthenticationBy"] == "0")  //AS400 Authentication
                {
                    iDB2Command cmd = new iDB2Command();
                    cmd.CommandText = String.Format("INSERT INTO {0}(T01MID,T01PID,T01RID,T01AUT,T01UPD,T01UPT, "
                        + "T01PGM,T01USR,T01WKS) VALUES(@T01MID,@T01PID,@T01RID,@T01AUT,@T01UPD,@T01UPT, "
                        + "@T01PGM,@T01USR,@T01WKS)", m_TableName);

                    cmd.Parameters.Add("@T01MID", m_ModuleID);
                    cmd.Parameters.Add("@T01PID", m_PortalID);
                    cmd.Parameters.Add("@T01RID", strRoleID);
                    cmd.Parameters.Add("@T01AUT", m_AuthorizeUser);
                    cmd.Parameters.Add("@T01UPD", m_UdpDate);
                    cmd.Parameters.Add("@T01UPT", m_UdpTime);
                    cmd.Parameters.Add("@T01PGM", m_Program);
                    cmd.Parameters.Add("@T01USR", m_User);
                    //cmd.Parameters.Add("@T01WKS", m_Wrkstn);
                    string strWrkStn = m_Wrkstn;
                    if (m_Wrkstn.Trim().Length > 10)
                    {
                        strWrkStn = m_Wrkstn.Trim().Substring((m_Wrkstn.Trim().Length - 10), 10);
                    }
                    cmd.Parameters.Add("@T01WKS", strWrkStn);

                    try
                    {
                        affectedRows = m_da400.ExecuteNonQuery(cmd);
                    }
                    catch (Exception ex)
                    {
                        m_LastError = ex.Message;
                    }
                }

                else
                {

                }
            }
            return (affectedRows > 0);
        }

        public bool Update(bool usedStoreProcedure)
        {
            return false;
        }
        public bool Delete(bool usedStoreProcedure)
        {
            int affectedRows = -1;
            if (WebConfigurationManager.AppSettings["AuthenticationBy"] == "0")  //AS400 Authentication
            {
                iDB2Command deleteCmd = new iDB2Command();
                deleteCmd.CommandText = String.Format("DELETE FROM {0} WHERE T01MID=@T01MID AND T01PID=@T01PID ", m_TableName);	 //AND T62RID = @T62RID",m_Lib,m_TableName);

                deleteCmd.Parameters.Add("@T01MID", m_ModuleID);
                deleteCmd.Parameters.Add("@T01PID", m_PortalID);
                //deleteCmd.Parameters.Add("@T62RID",m_RoleID);

                try
                {
                    affectedRows = m_da400.ExecuteNonQuery(deleteCmd);
                }
                catch (Exception ex)
                {
                    m_LastError = ex.Message;
                    Debug.WriteLine(ex.Message);
                }
            }


            return (affectedRows > 0);
        }

        public bool Delete(string selectCmd)
        {
            int affectedRows = -1;
            if (WebConfigurationManager.AppSettings["AuthenticationBy"] == "0")  //AS400 Authentication
            {
                iDB2Command cmd = new iDB2Command();
                cmd.CommandText = selectCmd;

                try
                {
                    affectedRows = m_da400.ExecuteNonQuery(selectCmd);
                }
                catch (Exception ex)
                {
                    m_LastError = ex.Message;
                    Debug.WriteLine(ex.Message);
                }
            }

            else
            {

            }
            return (affectedRows > 0);
        }

        public bool FindObject(bool usedStoreProcedure)
        {
            bool popSuccess = false;

            if (WebConfigurationManager.AppSettings["AuthenticationBy"] == "0")  //AS400 Authentication
            {
                iDB2DataReader dr = null;
                IBM.Data.DB2.iSeries.iDB2Connection cnn = null;
                try
                {
                    string findCmd = string.Format("SELECT T01MID, T01PID, T01RID, T01AUT  FROM {0} WHERE T01MID={1} AND T01PID={2} AND T01RID={3} ",
                        m_TableName, m_ModuleID, m_PortalID, m_RoleID);

                    cnn = (iDB2Connection)m_da400.GetiSeriesDbAs400Connection();
                    dr = m_da400.ExecuteDataReader(cnn, findCmd);
                    //dr = iSeriesHelper.ExecuteReader(cnn, CommandType.Text, findCmd);  
                    PopulateObject(ref dr);
                    popSuccess = true;
                }
                catch (Exception ex)
                {
                    popSuccess = false;
                    m_LastError = ex.Message;
                    Debug.WriteLine(ex.Message);
                }
                if (dr != null)
                    dr.Close();

                cnn.Close();
                cnn.Dispose();
            }

            return (popSuccess);

        }
        //
        //		public bool FindObject(string strSQL)
        //		{
        //			bool popSuccess;
        //			iDB2DataReader dr = null; 
        //			IBM.Data.DB2.iSeries.iDB2Connection cnn = null;
        //			try
        //			{
        //							
        //				cnn = (iDB2Connection)m_da400.GetDbAs400Connection(); 
        //				dr = m_da400.ExecuteDataReader(cnn,strSQL); 
        //				//dr = iSeriesHelper.ExecuteReader(cnn, CommandType.Text, findCmd);  
        //				PopulateObject(ref dr);
        //				popSuccess = true;
        //			}
        //			catch(Exception ex)
        //			{
        //				popSuccess = false;
        //				m_LastError = ex.Message;
        //				Debug.WriteLine(ex.Message);
        //			}
        //			if(dr!=null)
        //				dr.Close();
        //
        //			cnn.Close();
        //			cnn.Dispose(); 
        //			return (dr.HasRows && popSuccess);
        //		}

        public DataSet RetriveAsDataSet(string selectCmd)
        {
            DataSet DS = null;
            if (WebConfigurationManager.AppSettings["AuthenticationBy"] == "0")  //AS400 Authentication
            {
                iDB2Command cmd = new iDB2Command();
                cmd.CommandText = selectCmd;

                try
                {
                    DS = m_da400.ExecuteDataSet(cmd);
                }
                catch (Exception ex)
                {
                    m_LastError = ex.Message;
                    Debug.WriteLine(ex.Message);
                }
            }

            return DS;
        }

        public DataSet RetriveAsDataSetByProcedure(string procedureName, iDB2Parameter[] parms)
        {
            return new DataSet();
        }

        public void PopulateObject(ref iDB2DataReader dataReader)
        {
            if (!dataReader.HasRows)
                return;

            while (dataReader.Read())
            {
                if (!Convert.IsDBNull(dataReader["T01MID"]))
                    this.ModuleID = Convert.ToInt32(dataReader["T01MID"]);

                if (!Convert.IsDBNull(dataReader["T01PID"]))
                    this.PortalID = Convert.ToInt32(dataReader["T01PID"]);

                if (!Convert.IsDBNull(dataReader["T01RID"]))
                    this.RoleID = Convert.ToInt32(dataReader["T01RID"]);
                //            
                //				if(!Convert.IsDBNull(dataReader["T37FLG"]))
                //					this.Flag = Convert.ToString(dataReader["T37FLG"]).Trim();
                //            
                //				if(!Convert.IsDBNull(dataReader["T37STS"]))
                //					this.m_Status = Convert.ToString(dataReader["T37STS"]).Trim();
                //            
                //				if(!Convert.IsDBNull(dataReader["T37UPD"]))
                //					this.m_UdpDate = Convert.ToUInt32(dataReader["T37UPD"]);
                //            
                //				if(!Convert.IsDBNull(dataReader["T37UPT"]))
                //					this.m_UdpTime = Convert.ToUInt32(dataReader["T37UPT"]);
                //            
                //				if(!Convert.IsDBNull(dataReader["T37PGM"]))
                //					this.m_Program = Convert.ToString(dataReader["T37PGM"]).Trim();
                //            
                //				if(!Convert.IsDBNull(dataReader["T37USR"]))
                //					this.m_User = Convert.ToString(dataReader["T37USR"]).Trim();
                //            
                //				if(!Convert.IsDBNull(dataReader["T37WKS"]))
                //					this.m_Wrkstn = Convert.ToString(dataReader["T37WKS"]).Trim();
                //
                //				if(!Convert.IsDBNull(dataReader["T37FIL"]))
                //					this.m_Filler = Convert.ToString(dataReader["T37FIL"]).Trim();
            }
        }
        //public void PopulateObject(ref OracleDataReader dataReader)
        //{
        //    if (!dataReader.Read())
        //        return;

        //    while (dataReader.Read())
        //    {
        //        if (!Convert.IsDBNull(dataReader["T01MID"]))
        //            this.ModuleID = Convert.ToInt32(dataReader["T01MID"]);

        //        if (!Convert.IsDBNull(dataReader["T01PID"]))
        //            this.PortalID = Convert.ToInt32(dataReader["T01PID"]);

        //        if (!Convert.IsDBNull(dataReader["T01RID"]))
        //            this.RoleID = Convert.ToInt32(dataReader["T01RID"]);
        //    }
        //}
        public void PopulateObject(ref MySqlDataReader dataReader)
        {
            if (!dataReader.Read())
                return;

            while (dataReader.Read())
            {
                if (!Convert.IsDBNull(dataReader["T01MID"]))
                    this.ModuleID = Convert.ToInt32(dataReader["T01MID"]);

                if (!Convert.IsDBNull(dataReader["T01PID"]))
                    this.PortalID = Convert.ToInt32(dataReader["T01PID"]);

                if (!Convert.IsDBNull(dataReader["T01RID"]))
                    this.RoleID = Convert.ToInt32(dataReader["T01RID"]);
            }
        }

    }
}