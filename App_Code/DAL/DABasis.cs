
using System;
using System.Data;
using IBM.Data.DB2.iSeries;
using MySql.Data.MySqlClient;
using System.Web.Configuration;
using EB_Service.Commons;

namespace EB_Service.DAL
{

    public enum DatabaseTypes { DbOracle = 0, DbAS400, DbDB2, DbMSSQL, DbMySql }
    public enum Providor400Types { PrOle = 0, PriSeries }
    /// <summary>
    /// This is the super class for Data Access Classes
    /// </summary>
    public class DABasis
    {

        protected static string strConnect;
        private string m_strUserName;
        private string m_strPassword;
        private string m_strLib;
        private string m_strSID;
        public bool Assignlib_Flag = false;
        private iDB2Connection m_iDB2Con;
        private iDB2Transaction m_tr;



        protected DatabaseTypes m_DBType = DatabaseTypes.DbAS400;
        // protected Providor400Types m_ProvType = Providor400Types.PriSeries;
        protected Providor400Types m_ProvType = Providor400Types.PrOle;
        public DABasis()
        {

        }
        ~DABasis()
        {
            // CloseConnect(); 
        }

        //			static DABasis(string ConnectionStringKey,DatabaseType _DBType)
        //			{
        //				m_DBType = _DBType;
        //				strConnect=ConfigurationSettings.AppSettings[ConnectionStringKey];
        //			}
        public iDB2Connection iDB2Con
        {
            set { m_iDB2Con = value; }
            get { return m_iDB2Con; }
        }

        public iDB2Transaction _tr
        {
            set { m_tr = value; }
            get { return m_tr; }
        }


        public string UserName
        {
            set { m_strUserName = value; }
            get { return m_strUserName; }
        }
        public string Password
        {
            set { m_strPassword = value; }

        }

        public DatabaseTypes DatabaseType
        {
            set { m_DBType = value; }

        }

        public Providor400Types Providor400Type
        {
            set { m_ProvType = value; }

        }

        public string AS400Lib
        {
            set { m_strLib = value; }
            get { return m_strLib; }
        }
        public string OracleSID
        {
            set { m_strSID = value; }

        }
        /// <summary>
        /// Gets a SqlConnection to the local sqlserver
        /// </summary>
        /// <returns>SqlConnection</returns>
        //public OleDbConnection GetOleDbAs400Connection()
        //{
        //    string strCnn = AppConfig.OleDB400ConnectionString;
        //    strCnn = String.Format(strCnn, WebConfigurationManager.AppSettings["AS400ProHost"],  this.m_strUserName, 
        //        this.m_strPassword, this.m_strLib);
        //    OleDbConnection oConnection = new OleDbConnection(strCnn);
        //    return oConnection;
        //}


        /// <summary>
        /// Gets a SqlConnection to the AS400
        /// </summary>
        /// <returns>SqlConnection</returns>

        public iDB2Connection GetiSeriesDbAs400Connection()
        {
            string strCnn = AppConfig.iSeries400ConnectionString;
            string strlib = EB_Service.Commons.AppConfig.GeneralLib;
            strCnn = String.Format(strCnn, WebConfigurationManager.AppSettings["AS400ProHost"], this.m_strUserName,
                this.m_strPassword, strlib);
            iDB2Connection oConnection = new iDB2Connection(strCnn);

            return oConnection;
        }
        public bool ConnectAs400()
        {
            bool bRet = true;
            return false;
            try
            {
                //if (m_iDB2Con == null || !m_iDB2Con.CheckConnectionOnOpen) // .State. != ConnectionState.Open())
                if ((m_iDB2Con == null) || (m_iDB2Con.State != ConnectionState.Open))
                {
                    Assignlib_Flag = false;

                    string strlib = EB_Service.Commons.AppConfig.GeneralLib;
                    //strCnn = String.Format(strCnn,m_strUserName,m_strPassword,m_strLib);


                    string strCnn = AppConfig.iSeries400ConnectionString;
                    //strCnn = String.Format(strCnn, WebConfigurationManager.AppSettings["AS400ProHost"], this.m_strUserName,
                    //    this.m_strPassword, strlib);

                    m_iDB2Con = new iDB2Connection(strCnn);
                    m_iDB2Con.Open();
                    if (!StartTransactionAs400())
                    {
                        bRet = false;
                    }
                }


            }
            catch (iDB2Exception idb2ex)
            { bRet = false; }
            //oConnection.ConnectionString = "User ID=NUTTHEE;Data Source=192.168.4.3";
            return bRet;
        }

        public bool StartTransactionAs400()
        {
            bool bRet = true;
            try
            {
                m_tr = m_iDB2Con.BeginTransaction();

            }
            catch
            {
                bRet = false;
            }

            return bRet;
        }
        public bool CommitAs400()
        {
            bool bRet = true;
            try
            {
                m_tr.Commit();

            }
            catch
            {
                bRet = false;
            }

            return bRet;
        }

        public bool RollbackAs400()
        {
            bool bRet = true;
            try
            {
                m_tr.Rollback();

            }
            catch
            {
                bRet = false;
            }

            return bRet;
        }
        public bool CloseConnect()
        {
            bool bRet = true;
            try
            {
                m_iDB2Con.Close();
                //					m_iDB2Con.Dispose();
                //					m_iDB2Con = null;
            }
            catch
            {
                bRet = false;
            }

            return bRet;

        }

        /// <summary>
        /// Gets a SqlConnection to the Oracle
        /// </summary>
        /// <returns>OraConnection</returns>

        //public OracleConnection GetDbOraConnection()
        //{
        //    string strCnn = AppConfig.OracleConnectionString;
        //    strCnn = String.Format(strCnn, WebConfigurationManager.AppSettings["OraProHost"],
        //        WebConfigurationManager.AppSettings["OraProSID"],WebConfigurationManager.AppSettings["OraProUser"],
        //        WebConfigurationManager.AppSettings["OraProPwd"]);
        //    OracleConnection oraCnn = new OracleConnection(strCnn);

        //    return oraCnn;
        //}

        //public OracleConnection GetDbOraConnectionSelf()
        //{
        //    string strCnn = AppConfig.OracleConnectionString;
        //    strCnn = String.Format(strCnn, WebConfigurationManager.AppSettings["OraSelfHost"],
        //        WebConfigurationManager.AppSettings["OraSelfSID"], WebConfigurationManager.AppSettings["OraSelfUser"],
        //        WebConfigurationManager.AppSettings["OraSelfPwd"]);
        //    OracleConnection oraCnn = new OracleConnection(strCnn);

        //    return oraCnn;
        //}

        //public OracleConnection GetDbOraConnectionAuthentication() //Peak 23/7/2009 เพิ่ม Function Get Connection สำหรับ Check Authen 
        //{
        //    string strCnn = AppConfig.OracleConnectionString;
        //    strCnn = String.Format(strCnn, WebConfigurationManager.AppSettings["OraAutHost"],
        //        WebConfigurationManager.AppSettings["OraAutSID"], WebConfigurationManager.AppSettings["OraAutUser"],
        //        WebConfigurationManager.AppSettings["OraAutPwd"]);
        //    OracleConnection oraCnn = new OracleConnection(strCnn);

        //    return oraCnn;
        //}



        /// <summary>
        /// Gets a SqlConnection to the MySql
        /// </summary>
        /// <returns>MySqlConnection</returns>

        public MySqlConnection GetDbMySqlConnection()
        {
            string strCnn = AppConfig.MySqlConnectionString;
            strCnn = String.Format(strCnn, WebConfigurationManager.AppSettings["MySqlProHost"],
                WebConfigurationManager.AppSettings["MySqlProDbName"], WebConfigurationManager.AppSettings["MySqlProUser"],
                WebConfigurationManager.AppSettings["MySqlProPwd"]);
            MySqlConnection mySqlCnn = new MySqlConnection(strCnn);

            return mySqlCnn;
        }

        public MySqlConnection GetDbMySqlConnectionAuthentication() //Peak 23/7/2009 เพิ่ม Function Get Connection สำหรับ Check Authen 
        {
            string strCnn = AppConfig.MySqlConnectionString;
            strCnn = String.Format(strCnn, WebConfigurationManager.AppSettings["MySqlAutHost"],
                WebConfigurationManager.AppSettings["MySqlAutDbName"], WebConfigurationManager.AppSettings["MySqlAutUser"],
                WebConfigurationManager.AppSettings["MySqlAutPwd"]);
            MySqlConnection mySqlCnn = new MySqlConnection(strCnn);

            return mySqlCnn;
        }
    }
}