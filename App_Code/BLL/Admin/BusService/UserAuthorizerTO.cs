using EB_Service.Commons;
using EB_Service.DAL;
using ILSystem.App_Code.Commons;
using IBM.Data.DB2.iSeries;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Web.Configuration;

namespace EB_Service.Admin.BusService
{
    public class UserAuthorizerTO
    {
        //		private string m_Lib = EB_Service.Common.AppConfig.AHPBackupLib;
        private string m_Lib = AppConfig.GNLib;
        private string m_LastError;
        //		private const string m_TableName = "HMTB66";
        private const string m_TableName = "GNTA05";
        private string m_ConnStr;

        private string m_UserID;
        private string m_UserName;
        private int m_RoleID;
        private string m_PortalID;


        private DateTimeFormatInfo m_DThai = new CultureInfo("th-TH", false).DateTimeFormat;

        private string m_UdpDate = DateTime.Now.ToString("yyMMdd");
        private string m_UdpTime = DateTime.Now.ToString("HHmmss");
        private string m_Program = "USERWEB";
        private string m_User;
        private string m_Wrkstn = "";


        private UserInfo m_UserInfo;

        private As400DAL m_da400 = new As400DAL();


        public UserAuthorizerTO()
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

        public string PortalID
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

        #region "Entry DropdownList"

        public DataSet RetriveDDListAsDataSet(string selectCmd)
        {
            DataSet DS = null;

            if (WebConfigurationManager.AppSettings["AuthenticationBy"] == "0")  //AS400 Authentication
            {
                //iDB2Command cmd = new iDB2Command();
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

            else
            {

            }
            return DS;
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
                cmd.CommandText = String.Format("INSERT INTO {0}(T05OID,T05UID,T05NAM,T05RID,T05UPD,T05UPT,"
                    + "T05PGM,T05USR,T05WKS) VALUES(@T05OID,@T05UID,@T05NAM,@T05RID,@T05UPD,"
                    + "@T05UPT,@T05PGM,@T05USR,@T05WKS)", m_TableName);

                cmd.Parameters.Add("@T05OID", m_PortalID);
                cmd.Parameters.Add("@T05UID", m_UserID);
                cmd.Parameters.Add("@T05NAM", m_UserName);
                cmd.Parameters.Add("@T05RID", m_RoleID);
                cmd.Parameters.Add("@T05UPD", m_UdpDate);
                cmd.Parameters.Add("@T05UPT", m_UdpTime);
                cmd.Parameters.Add("@T05PGM", m_Program);
                cmd.Parameters.Add("@T05USR", m_User);
                //cmd.Parameters.Add("@T05WKS", m_Wrkstn);
                string strWrkStn = m_Wrkstn;
                if (m_Wrkstn.Trim().Length > 10)
                {
                    strWrkStn = m_Wrkstn.Trim().Substring((m_Wrkstn.Trim().Length - 10), 10);
                }
                cmd.Parameters.Add("@T05WKS", strWrkStn);

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
                    cmd.CommandText = String.Format("INSERT INTO {0}(T05OID,T05UID,T05NAM,T05RID,T05UPD,T05UPT,"
                        + "T05PGM,T05USR,T05WKS) VALUES(@T05OID,@T05UID,@T05NAM,@T05RID,@T05UPD,"
                        + "@T05UPT,@T05PGM,@T05USR,@T05WKS)", m_TableName);

                    cmd.Parameters.Add("@T05OID", m_PortalID);
                    cmd.Parameters.Add("@T05UID", m_UserID);
                    cmd.Parameters.Add("@T05NAM", m_UserName);
                    cmd.Parameters.Add("@T05RID", strRoleID);
                    cmd.Parameters.Add("@T05UPD", m_UdpDate);
                    cmd.Parameters.Add("@T05UPT", m_UdpTime);
                    cmd.Parameters.Add("@T05PGM", m_Program);
                    cmd.Parameters.Add("@T05USR", m_User);
                    //cmd.Parameters.Add("@T05WKS", m_Wrkstn);
                    string strWrkStn = m_Wrkstn;
                    if (m_Wrkstn.Trim().Length > 10)
                    {
                        strWrkStn = m_Wrkstn.Trim().Substring((m_Wrkstn.Trim().Length - 10), 10);
                    }
                    cmd.Parameters.Add("@T05WKS", strWrkStn);

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
                deleteCmd.CommandText = String.Format("DELETE FROM {0} WHERE T05UID=@T05UID AND T05OID=@T05OID And T05NAM=@T05NAM ", m_TableName);

                deleteCmd.Parameters.Add("@T05UID", m_UserID);
                deleteCmd.Parameters.Add("@T05OID", m_PortalID);
                deleteCmd.Parameters.Add("@T05NAM", m_UserName);		//Peak 19/07/2550 เพิ่ม UserName มา Where ด้วย ป้องกัน ID ที่ซ้ำกัน					

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
        //public bool FindObject(bool usedStoreProcedure)
        //{
        //    bool popSuccess = false;
        //    if (WebConfigurationManager.AppSettings["AuthenticationBy"] == "0")  //AS400 Authentication
        //    {
        //        iDB2DataReader dr = null;
        //        IBM.Data.DB2.iSeries.iDB2Connection cnn = null;
        //        try
        //        {																																																					//Peak 19/07/2550 เพิ่ม UserName มา Where ด้วย ป้องกัน ID ที่ซ้ำกัน
        //            string findCmd = string.Format("SELECT DISTINCT(T05UID),T05NAM FROM {0} WHERE T05UID='{1}' And T05NAM='{2}' AND T05OID='{3}' ", m_TableName, m_UserID, m_UserName, m_PortalID);

        //            cnn = (iDB2Connection)m_da400.GetiSeriesDbAs400Connection();
        //            dr = m_da400.ExecuteDataReader(cnn, findCmd);
        //            //dr = iSeriesHelper.ExecuteReader(cnn, CommandType.Text, findCmd);  
        //            PopulateObject(ref dr);
        //            popSuccess = true;
        //        }
        //        catch (Exception ex)
        //        {
        //            popSuccess = false;
        //            m_LastError = ex.Message;
        //            Debug.WriteLine(ex.Message);
        //        }
        //        if (dr != null)
        //            dr.Close();

        //        cnn.Close();
        //        cnn.Dispose();
        //    }


        //    return (popSuccess);
        //}

        public bool FindObject(string strSQL)
        {
            bool popSuccess = false;
            if (WebConfigurationManager.AppSettings["AuthenticationBy"] == "0")  //AS400 Authentication
            {
                iDB2DataReader dr = null;
                IBM.Data.DB2.iSeries.iDB2Connection cnn = null;
                try
                {

                    cnn = (iDB2Connection)m_da400.GetiSeriesDbAs400Connection();
                    dr = m_da400.ExecuteDataReader(cnn, strSQL);
                    //dr = iSeriesHelper.ExecuteReader(cnn, CommandType.Text, findCmd);  
                    if (PopulateObject(ref dr) == true)
                        popSuccess = true;
                    else popSuccess = false;
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

            else
            {

            }
            return DS;
        }
        public DataSet RetriveAsDataSet(Hashtable htParam)
        {
            DataSet DS = null;
            if (WebConfigurationManager.AppSettings["AuthenticationBy"] == "0")  //AS400 Authentication
            {
                string strQry = String.Format("SELECT * FROM {0} WHERE T37STS<>'X' AND ", m_TableName);

                iDB2Command cmd = new iDB2Command();
                IDictionaryEnumerator iDEnum = htParam.GetEnumerator();
                while (iDEnum.MoveNext())
                {
                    strQry = strQry + iDEnum.Key.ToString() +
                        (iDEnum.Value.ToString().EndsWith("*") ? " LIKE @" : "=@") + iDEnum.Key.ToString() + " AND ";
                    cmd.Parameters.Add("@" + iDEnum.Key.ToString(),
                        (iDEnum.Value.ToString().EndsWith("*") ? iDEnum.Value.ToString().Replace("*", "%") :
                        iDEnum.Value.ToString()));

                }
                if (strQry.EndsWith(" AND "))
                    strQry = strQry.Remove(strQry.LastIndexOf(" AND "), 5);

                cmd.CommandText = strQry;

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
        public bool PopulateObject(ref iDB2DataReader dataReader)
        {
            if (!dataReader.HasRows)
                return false;

            while (dataReader.Read())
            {
                if (!Convert.IsDBNull(dataReader["T05UID"]))
                    this.UserID = Convert.ToString(dataReader["T05UID"]).Trim();

                if (!Convert.IsDBNull(dataReader["T05NAM"]))
                    this.UserName = Convert.ToString(dataReader["T05NAM"]).Trim();

                //				if(!Convert.IsDBNull(dataReader["T37FCN"]))
                //					this.RoleID  = Convert.ToString(dataReader["T37FCN"]).Trim();
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
            return true;

        }
        //public bool PopulateObject(ref OracleDataReader dataReader)
        //{
        //    if (!dataReader.Read())
        //        return false;

        //    while (dataReader.Read())
        //    {
        //        if (!Convert.IsDBNull(dataReader["T05UID"]))
        //            this.UserID = Convert.ToString(dataReader["T05UID"]).Trim();

        //        if (!Convert.IsDBNull(dataReader["T05NAM"]))
        //            this.UserName = Convert.ToString(dataReader["T05NAM"]).Trim();
        //    }

        //    return true;
        //}
        public bool PopulateObject(ref MySqlDataReader dataReader)
        {
            if (!dataReader.Read())
                return false;

            while (dataReader.Read())
            {
                if (!Convert.IsDBNull(dataReader["T05UID"]))
                    this.UserID = Convert.ToString(dataReader["T05UID"]).Trim();

                if (!Convert.IsDBNull(dataReader["T05NAM"]))
                    this.UserName = Convert.ToString(dataReader["T05NAM"]).Trim();
            }

            return true;
        }

    }
}