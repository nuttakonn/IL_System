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
    public class RolesTO
    {
        //		private string m_Lib = EB_Service.Common.AppConfig.AHPBackupLib;
        private string m_Lib = AppConfig.GNLib;
        private string m_LastError;
        //		private const string m_TableName = "HMTB65";
        private const string m_TableName = "GNTA04";
        private string m_ConnStr;

        private string m_RoleName;
        private int m_RoleID;
        private int m_PortalID;

        private DateTimeFormatInfo m_DThai = new CultureInfo("th-TH", false).DateTimeFormat;

        private string m_UdpDate = DateTime.Now.ToString("yyMMdd");
        private string m_UdpTime = DateTime.Now.ToString("HHmmss");
        private string m_Program = "ROLESWEB";
        private string m_User;
        private string m_Wrkstn = "";


        private UserInfo m_UserInfo;

        private As400DAL m_da400 = new As400DAL();


        public RolesTO()
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

        public string RoleName
        {
            get { return m_RoleName; }
            set { m_RoleName = value; }
        }

        public int RoleID
        {
            get { return m_RoleID; }
            set { m_RoleID = value; }
        }

        public int PortalID
        {
            get { return m_PortalID; }
            set { m_PortalID = value; }
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
                cmd.CommandText = String.Format("INSERT INTO {0}(T04RID,T04NAM,T04PID,T04UPD,T04UPT, "
                    + "T04PGM,T04USR,T04WKS) VALUES(@T04RID,@T04NAM,@T04PID,@T04UPD,@T04UPT, "
                    + "@T04PGM,@T04USR,@T04WKS)", m_TableName);

                cmd.Parameters.Add("@T04RID", m_RoleID);
                cmd.Parameters.Add("@T04NAM", m_RoleName);
                cmd.Parameters.Add("@T04PID", m_PortalID);
                cmd.Parameters.Add("@T04UPD", m_UdpDate);
                cmd.Parameters.Add("@T04UPT", m_UdpTime);
                cmd.Parameters.Add("@T04PGM", m_Program);
                cmd.Parameters.Add("@T04USR", m_User);
                //cmd.Parameters.Add("@T04WKS", m_Wrkstn);                
                string strWrkStn = m_Wrkstn;
                if (m_Wrkstn.Trim().Length > 10)
                {
                    strWrkStn = m_Wrkstn.Trim().Substring((m_Wrkstn.Trim().Length - 10), 10);
                }
                cmd.Parameters.Add("@T04WKS", strWrkStn);

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
            return (affectedRows > 0);
        }

        public bool Update(bool usedStoreProcedure)
        {
            int affectedRows = -1;

            if (WebConfigurationManager.AppSettings["AuthenticationBy"] == "0")  //AS400 Authentication
            {
                m_UdpDate = DateTime.Now.Date.ToString("yyMMdd");
                m_UdpTime = DateTime.Now.ToString("HHmmss");
                iDB2Command cmd = new iDB2Command();
                cmd.CommandText = String.Format("UPDATE {0} SET T04NAM=@T04NAM,T04UPD=@T04UPD,T04UPT=@T04UPT, "
                                                        + " T04PGM=@T04PGM,T04USR=@T04USR,T04WKS=@T04WKS "
                                                        + " WHERE T04RID=@T04RID AND T04PID=@T04PID ", m_TableName);

                cmd.Parameters.Add("@T04RID", m_RoleID);
                cmd.Parameters.Add("@T04NAM", m_RoleName);
                cmd.Parameters.Add("@T04PID", m_PortalID);
                cmd.Parameters.Add("@T04UPD", m_UdpDate);
                cmd.Parameters.Add("@T04UPT", m_UdpTime);
                cmd.Parameters.Add("@T04PGM", m_Program);
                cmd.Parameters.Add("@T04USR", m_User);
                //cmd.Parameters.Add("@T04WKS", m_Wrkstn);
                string strWrkStn = m_Wrkstn;
                if (m_Wrkstn.Trim().Length > 10)
                {
                    strWrkStn = m_Wrkstn.Trim().Substring((m_Wrkstn.Trim().Length - 10), 10);
                }
                cmd.Parameters.Add("@T04WKS", strWrkStn);

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

            //			return false;
        }
        public bool Delete(bool usedStoreProcedure)
        {
            int affectedRows = -1;
            if (WebConfigurationManager.AppSettings["AuthenticationBy"] == "0")  //AS400 Authentication
            {
                iDB2Command deleteCmd = new iDB2Command();
                deleteCmd.CommandText = String.Format("DELETE FROM {0} WHERE T04RID=@T04RID AND T04PID=@T04PID ", m_TableName);

                deleteCmd.Parameters.Add("@T04MID", m_RoleID);
                deleteCmd.Parameters.Add("@T04PID", m_PortalID);

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

            else
            {

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
                    string findCmd = string.Format("SELECT T04RID, T04NAM, T04PID FROM {0} WHERE T04PID={1} AND T04RID={2} ",
                        m_TableName, m_PortalID, m_RoleID);

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
                }
                if (dr != null)
                    dr.Close();

                cnn.Close();
                cnn.Dispose();
            }

            else
            {

            }
            return (popSuccess);

        }

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
                if (!Convert.IsDBNull(dataReader["T04NAM"]))
                    this.RoleName = (dataReader["T04NAM"].ToString().Trim());

                if (!Convert.IsDBNull(dataReader["T04PID"]))
                    this.PortalID = Convert.ToInt32(dataReader["T04PID"]);

                if (!Convert.IsDBNull(dataReader["T04RID"]))
                    this.RoleID = Convert.ToInt32(dataReader["T04RID"]);
            }
        }

        //public void PopulateObject(ref OracleDataReader dataReader)
        //{
        //    if (!dataReader.Read())
        //        return;

        //    while (dataReader.Read())
        //    {
        //        if (!Convert.IsDBNull(dataReader["T04NAM"]))
        //            this.RoleName = (dataReader["T04NAM"].ToString().Trim());

        //        if (!Convert.IsDBNull(dataReader["T04PID"]))
        //            this.PortalID = Convert.ToInt32(dataReader["T04PID"]);

        //        if (!Convert.IsDBNull(dataReader["T04RID"]))
        //            this.RoleID = Convert.ToInt32(dataReader["T04RID"]);
        //    }
        //}

        public void PopulateObject(ref MySqlDataReader dataReader)
        {
            if (!dataReader.Read())
                return;

            while (dataReader.Read())
            {
                if (!Convert.IsDBNull(dataReader["T04NAM"]))
                    this.RoleName = (dataReader["T04NAM"].ToString().Trim());

                if (!Convert.IsDBNull(dataReader["T04PID"]))
                    this.PortalID = Convert.ToInt32(dataReader["T04PID"]);

                if (!Convert.IsDBNull(dataReader["T04RID"]))
                    this.RoleID = Convert.ToInt32(dataReader["T04RID"]);
            }
        }

    }
}