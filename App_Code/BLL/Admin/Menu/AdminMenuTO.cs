using System;
using System.Diagnostics;
using System.Data;
using System.Collections;
using System.Globalization;
using IBM.Data.DB2.iSeries;
using MySql.Data.MySqlClient;
using System.Web.Configuration;
using EB_Service.DAL;
using ILSystem.App_Code.Commons;
using EB_Service.Commons;

namespace ILSystem.App_Code.BLL.Admin.Menu
{
    public class AdminMenuTO
    {
        private string m_Lib = EB_Service.Commons.AppConfig.GNLib;
        private string m_LastError;
        private const string m_TableName = "GNTA03";
        private string m_ConnStr;
        private string m_CostCode;
        private string m_EName;
        private string m_TName;
        //private string m_Flag=""; // {""=STAFF,"X"=AUTHRIZE}
        private string m_Status = ""; //{}
        private DateTimeFormatInfo m_DThai = new CultureInfo("th-TH", false).DateTimeFormat;
        private ulong m_UdpDate = Convert.ToUInt32(DateTime.Now.ToString("yyMMdd"));
        private ulong m_UdpTime = Convert.ToUInt32(DateTime.Now.ToString("HHmmss"));
        private string m_Program = "PORTALWEB";
        private string m_User;
        private string m_Wrkstn = "";
        private string m_Filler = "";
        private UserInfo m_UserInfo;
        private As400DAL m_da400 = new As400DAL();


        public AdminMenuTO()
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

        public ulong UdpDate
        {
            get { return m_UdpDate; }
            set { m_UdpDate = value; }
        }
        public ulong UdpTime
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
        public string Filler
        {
            get { return m_Filler; }
            set { m_Filler = value; }
        }
        #endregion

        public DataSet GetDataMenuFromUserID_PortalID()
        {
            /*
            string strSQL = "SELECT DISTINCT({0}.GNTA01.T01MID),{0}.GNTA02.T02ALI,{0}.GNTA02.T02SOU"
                    + ", {0}.GNTA02.T02NAM FROM {0}.GNTA01 RIGHT OUTER JOIN "
                    + "{0}.GNTA02 ON {0}.GNTA01.T01MID = {0}.GNTA02.T02MID  "
                    + " AND {0}.GNTA01.T01PID = {0}.GNTA02.T02OID "
                    + "WHERE ({0}.GNTA01.T01RID IN (SELECT {0}.GNTA04.T04RID FROM "
                    + "{0}.GNTA04 INNER JOIN {0}.GNTA05 ON {0}.GNTA04.T04RID = {0}.GNTA05.T05RID "
                    + " AND {0}.GNTA04.T04PID = {0}.GNTA05.T05OID "
                    + "WHERE {0}.GNTA05.T05NAM = '{1}' AND {0}.GNTA04.T04PID = {2})) "
                    + "AND ({0}.GNTA02.T02PID = 0) AND ({0}.GNTA02.T02OID={2}) "
                    + " Order By  ({0}.GNTA01.T01MID) ";
            */
            string strSQL = "SELECT DISTINCT(GNTA01.T01MID), GNTA02.T02ALI,GNTA02.T02SOU"
                                + ", GNTA02.T02NAM FROM GNTA01 RIGHT OUTER JOIN "
                                + " GNTA02 ON GNTA01.T01MID = GNTA02.T02MID  "
                                + " AND GNTA01.T01PID = GNTA02.T02OID "
                                + "WHERE (GNTA01.T01RID IN (SELECT GNTA04.T04RID FROM "
                                + "GNTA04 INNER JOIN GNTA05 ON GNTA04.T04RID = GNTA05.T05RID "
                                + " AND GNTA04.T04PID = GNTA05.T05OID "
                                + "WHERE GNTA05.T05NAM = '{1}' AND GNTA04.T04PID = {2})) "
                                + "AND (GNTA02.T02PID = 0) AND (GNTA02.T02OID={2}) "
                                + " Order By  (GNTA01.T01MID) ";

            DataSet ds = new DataSet();
            // 0=AS400, 1=Oracle, 2=MySql
            switch (WebConfigurationManager.AppSettings["AuthenticationBy"].Trim())
            {
                case "0":
                    strSQL = String.Format(strSQL, AppConfig.GNLib, m_UserInfo.Username, m_UserInfo.PortalID);
                    m_da400 = new As400DAL(m_UserInfo.Username, m_UserInfo.Password, AppConfig.GNLib, Providor400Types.PriSeries);
                    ds = m_da400.ExecuteDataSet(strSQL);
                    break;

            }
            return ds;

        }
        public DataSet GetDataSubMenuByParentID(string strParentID)
        {
            /*
            string strSQL = "SELECT DISTINCT({0}.GNTA01.T01MID),{0}.GNTA02.T02SOU,{0}.GNTA02.T02ALI FROM {0}.GNTA01 INNER JOIN {0}.GNTA02 "
                    + " ON {0}.GNTA01.T01MID = {0}.GNTA02.T02MID AND {0}.GNTA01.T01PID = {0}.GNTA02.T02OID "
                    + " WHERE ({0}.GNTA01.T01PID = {1}) "
                    + " AND ({0}.GNTA02.T02PID = {3}) AND "
                    + " ({0}.GNTA01.T01RID IN (SELECT {0}.GNTA04.T04RID FROM {0}.GNTA04 INNER JOIN "
                    + " {0}.GNTA05 ON {0}.GNTA04.T04RID = {0}.GNTA05.T05RID AND "
                    + " {0}.GNTA04.T04PID = {0}.GNTA05.T05OID "
                    + " WHERE {0}.GNTA05.T05NAM = '" + m_UserInfo.Username.ToUpper().Trim() + "' AND {0}.GNTA04.T04PID = {1})) "
                    + " Order By ({0}.GNTA01.T01MID) ";
            */
            string strSQL = "SELECT DISTINCT(GNTA01.T01MID),GNTA02.T02SOU,GNTA02.T02ALI FROM GNTA01 INNER JOIN GNTA02 "
                    + " ON GNTA01.T01MID = GNTA02.T02MID AND GNTA01.T01PID = GNTA02.T02OID "
                    + " WHERE (GNTA01.T01PID = {1}) "
                    + " AND (GNTA02.T02PID = {3}) AND "
                    + " (GNTA01.T01RID IN (SELECT GNTA04.T04RID FROM GNTA04 INNER JOIN "
                    + " GNTA05 ON GNTA04.T04RID = GNTA05.T05RID AND "
                    + " GNTA04.T04PID = GNTA05.T05OID "
                    + " WHERE GNTA05.T05NAM = '" + m_UserInfo.Username.ToUpper().Trim() + "' AND GNTA04.T04PID = {1})) "
                    + " Order By (GNTA01.T01MID) ";

            ArrayList arRoles = m_UserInfo.RolesID;
            string strRole = "";
            DataSet ds = new DataSet();
            // 0=AS400, 1=Oracle, 2=MySql
            switch (WebConfigurationManager.AppSettings["AuthenticationBy"].Trim())
            {
                case "0":
                    strSQL = String.Format(strSQL, AppConfig.GNLib, m_UserInfo.PortalID, strRole, strParentID);
                    m_da400 = new As400DAL(m_UserInfo.Username, m_UserInfo.Password, AppConfig.GNLib, Providor400Types.PriSeries);
                    ds = m_da400.ExecuteDataSet(strSQL);
                    break;

            }
            return ds;
        }


        public bool Create(bool usedStoreProcedure)
        {
            m_UdpDate = Convert.ToUInt32(DateTime.Now.Date.ToString("yyMMdd", m_DThai));
            m_UdpTime = Convert.ToUInt32(DateTime.Now.ToString("HHmmss", m_DThai));
            int affectedRows = -1;

            if (WebConfigurationManager.AppSettings["AuthenticationBy"] == "0")  //AS400 Authentication
            {
                iDB2Command cmd = new iDB2Command();
                cmd.CommandText = String.Format("INSERT INTO {1}(T70CDE,T70DSE,T70DST,T70STS,T70UDT,T70UTM,"
                                    + "T70USR,T70DSP,T70FIL) VALUES(@T70CDE,@T70DSE,@T70DST,@T70STS,@T37UDT,"
                                    + "@T37UTM,@T70USR,@T70DSP,@T70FIL)", m_Lib, m_TableName);
                cmd.Parameters.Add("@T70CDE", m_CostCode);
                cmd.Parameters.Add("@T70DSE", m_EName);
                cmd.Parameters.Add("@T70DST", m_TName);
                cmd.Parameters.Add("@T70STS", m_Status);
                cmd.Parameters.Add("@T70UDT", m_UdpDate);
                cmd.Parameters.Add("@T70UTM", m_UdpTime);
                cmd.Parameters.Add("@T70USR", m_User);
                cmd.Parameters.Add("@T70DSP", m_Wrkstn);
                cmd.Parameters.Add("@T70FIL", m_Filler);

                try
                {
                    affectedRows = m_da400.ExecuteNonQuery(cmd);
                }
                catch (Exception ex)
                {
                    m_LastError = ex.Message;
                }
            }
            else if (WebConfigurationManager.AppSettings["AuthenticationBy"] == "1")  //Oracle Authentication            
            {
                /*
                OracleConnection OraCnn = new OracleConnection();   //Peak 23/7/2009 เพิ่มการกำหนด Connection ตาม Authentication
                OraCnn = m_daOra.GetDbOraConnectionAuthentication();

                OracleCommand cmd = new OracleCommand();
                cmd.CommandText = String.Format("INSERT INTO {0}(T70CDE,T70DSE,T70DST,T70STS,T70UDT,T70UTM,"
                    + "T70USR,T70DSP,T70FIL) VALUES(:T70CDE,:T70DSE,:T70DST,:T70STS,:T37UDT,"
                    + ":T37UTM,:T70USR,:T70DSP,:T70FIL)", m_TableName);

                cmd.Parameters.Add(":T70CDE", m_CostCode);
                cmd.Parameters.Add(":T70DSE", m_EName);
                cmd.Parameters.Add(":T70DST", m_TName);
                cmd.Parameters.Add(":T70STS", m_Status);
                cmd.Parameters.Add(":T70UDT", m_UdpDate);
                cmd.Parameters.Add(":T70UTM", m_UdpTime);
                cmd.Parameters.Add(":T70USR", m_User);
                cmd.Parameters.Add(":T70DSP", m_Wrkstn);
                cmd.Parameters.Add(":T70FIL", m_Filler);

                try
                {
                    affectedRows = m_daOra.ExecuteNonQuery(OraCnn,cmd);
                }
                catch (Exception ex)
                {
                    m_LastError = ex.Message;
                }
                */
            }


            return (affectedRows > 0);
        }
        public bool Update(bool usedStoreProcedure)
        {
            m_UdpDate = Convert.ToUInt32(DateTime.Now.Date.ToString("yyMMdd"));
            m_UdpTime = Convert.ToUInt32(DateTime.Now.ToString("HHmmss"));
            int affectedRows = -1;

            if (WebConfigurationManager.AppSettings["AuthenticationBy"] == "0")  //AS400 Authentication
            {
                iDB2Command cmd = new iDB2Command();
                cmd.CommandText = String.Format("UPDATE {0} SET T70DSE=@T70DSE,T70DST=@T70DST,"
                    + "T70STS=@T70STS,T70UDT=@T70UDT,T70UTM=@T70UTM,T70USR=@T70USR,"
                    + "T70DSP=@T70DSP,T70FIL=@T70FIL WHERE T70CDE=@T70CDE", m_TableName);

                cmd.Parameters.Add("@T70CDE", m_CostCode);
                cmd.Parameters.Add("@T70DSE", m_EName);
                cmd.Parameters.Add("@T70DST", m_TName);
                cmd.Parameters.Add("@T70STS", m_Status);
                cmd.Parameters.Add("@T70UDT", m_UdpDate);
                cmd.Parameters.Add("@T70UTM", m_UdpTime);
                cmd.Parameters.Add("@T70USR", m_User);
                cmd.Parameters.Add("@T70DSP", m_Wrkstn);
                cmd.Parameters.Add("@T70FIL", m_Filler);

                try
                {
                    affectedRows = m_da400.ExecuteNonQuery(cmd);
                }
                catch (Exception ex)
                {
                    m_LastError = ex.Message;
                }
            }

            return (affectedRows > 0);
        }
        public bool Delete(bool usedStoreProcedure)
        {
            int affectedRows = -1;

            if (WebConfigurationManager.AppSettings["AuthenticationBy"] == "0")  //AS400 Authentication
            {
                iDB2Command deleteCmd = new iDB2Command();
                deleteCmd.CommandText = String.Format("UPDATE {0} SET T70STS=@T70STS,T70UDT=@T70UDT,"
                    + "T70UTM=@T70UTM,T70USR=@T70USR,"
                    + "T70DSP=@T70DSP WHERE T70CDE=@T70CDE", m_TableName);

                deleteCmd.Parameters.Add("@T70CDE", m_CostCode);
                deleteCmd.Parameters.Add("@T70STS", "X");
                deleteCmd.Parameters.Add("@T70UDT", m_UdpDate);
                deleteCmd.Parameters.Add("@T70UTM", m_UdpTime);
                deleteCmd.Parameters.Add("@T70USR", m_User);
                deleteCmd.Parameters.Add("@T70DSP", m_Wrkstn);

                try
                {
                    affectedRows = m_da400.ExecuteNonQuery(deleteCmd);
                }
                catch (Exception ex)
                {
                    m_LastError = ex.Message;
                }
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
                    string findCmd = string.Format("SELECT * FROM {1} WHERE T70CDE='{2}'",
                        m_Lib, m_TableName, m_CostCode);

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
        public DataSet RetriveAsDataSet(Hashtable htParam)
        {
            string strQry = "";
            DataSet DS = null;

            if (WebConfigurationManager.AppSettings["AuthenticationBy"] == "0")  //AS400 Authentication
            {
                strQry = String.Format("SELECT * FROM {0} WHERE T70STS<>'X' AND T70FIL='B' AND ", m_TableName);

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
                if (!Convert.IsDBNull(dataReader["T70CDE"]))
                    this.m_CostCode = Convert.ToString(dataReader["T70CDE"]).Trim();

                if (!Convert.IsDBNull(dataReader["T70DSE"]))
                    this.m_EName = Convert.ToString(dataReader["T70DSE"]).Trim();

                if (!Convert.IsDBNull(dataReader["T70DST"]))
                    this.m_TName = Convert.ToString(dataReader["T70DST"]).Trim();

                if (!Convert.IsDBNull(dataReader["T70STS"]))
                    this.m_Status = Convert.ToString(dataReader["T70STS"]).Trim();

                if (!Convert.IsDBNull(dataReader["T70UDT"]))
                    this.m_UdpDate = Convert.ToUInt32(dataReader["T70UDT"]);

                if (!Convert.IsDBNull(dataReader["T70UTM"]))
                    this.m_UdpTime = Convert.ToUInt32(dataReader["T70UTM"]);

                if (!Convert.IsDBNull(dataReader["T70USR"]))
                    this.m_User = Convert.ToString(dataReader["T70USR"]).Trim();

                if (!Convert.IsDBNull(dataReader["T70DSP"]))
                    this.m_Wrkstn = Convert.ToString(dataReader["T70DSP"]).Trim();

                if (!Convert.IsDBNull(dataReader["T70FIL"]))
                    this.m_Filler = Convert.ToString(dataReader["T70FIL"]).Trim();
            }

        }
        //public void PopulateObject(ref OracleDataReader dataReader)
        //{
        //    if (!dataReader.Read())
        //        return;

        //    while (dataReader.Read())
        //    {
        //        if (!Convert.IsDBNull(dataReader["T70CDE"]))
        //            this.m_CostCode = Convert.ToString(dataReader["T70CDE"]).Trim();

        //        if (!Convert.IsDBNull(dataReader["T70DSE"]))
        //            this.m_EName = Convert.ToString(dataReader["T70DSE"]).Trim();

        //        if (!Convert.IsDBNull(dataReader["T70DST"]))
        //            this.m_TName = Convert.ToString(dataReader["T70DST"]).Trim();

        //        if (!Convert.IsDBNull(dataReader["T70STS"]))
        //            this.m_Status = Convert.ToString(dataReader["T70STS"]).Trim();

        //        if (!Convert.IsDBNull(dataReader["T70UDT"]))
        //            this.m_UdpDate = Convert.ToUInt32(dataReader["T70UDT"]);

        //        if (!Convert.IsDBNull(dataReader["T70UTM"]))
        //            this.m_UdpTime = Convert.ToUInt32(dataReader["T70UTM"]);

        //        if (!Convert.IsDBNull(dataReader["T70USR"]))
        //            this.m_User = Convert.ToString(dataReader["T70USR"]).Trim();

        //        if (!Convert.IsDBNull(dataReader["T70DSP"]))
        //            this.m_Wrkstn = Convert.ToString(dataReader["T70DSP"]).Trim();

        //        if (!Convert.IsDBNull(dataReader["T70FIL"]))
        //            this.m_Filler = Convert.ToString(dataReader["T70FIL"]).Trim();
        //    }

        //}
        public void PopulateObject(ref MySqlDataReader dataReader)
        {
            if (!dataReader.Read())
                return;

            while (dataReader.Read())
            {
                if (!Convert.IsDBNull(dataReader["T70CDE"]))
                    this.m_CostCode = Convert.ToString(dataReader["T70CDE"]).Trim();

                if (!Convert.IsDBNull(dataReader["T70DSE"]))
                    this.m_EName = Convert.ToString(dataReader["T70DSE"]).Trim();

                if (!Convert.IsDBNull(dataReader["T70DST"]))
                    this.m_TName = Convert.ToString(dataReader["T70DST"]).Trim();

                if (!Convert.IsDBNull(dataReader["T70STS"]))
                    this.m_Status = Convert.ToString(dataReader["T70STS"]).Trim();

                if (!Convert.IsDBNull(dataReader["T70UDT"]))
                    this.m_UdpDate = Convert.ToUInt32(dataReader["T70UDT"]);

                if (!Convert.IsDBNull(dataReader["T70UTM"]))
                    this.m_UdpTime = Convert.ToUInt32(dataReader["T70UTM"]);

                if (!Convert.IsDBNull(dataReader["T70USR"]))
                    this.m_User = Convert.ToString(dataReader["T70USR"]).Trim();

                if (!Convert.IsDBNull(dataReader["T70DSP"]))
                    this.m_Wrkstn = Convert.ToString(dataReader["T70DSP"]).Trim();

                if (!Convert.IsDBNull(dataReader["T70FIL"]))
                    this.m_Filler = Convert.ToString(dataReader["T70FIL"]).Trim();
            }

        }
    }
}