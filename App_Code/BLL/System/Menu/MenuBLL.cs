using EB_Service.Commons;
using EB_Service.DAL;
using ILSystem.App_Code.Commons;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Configuration;


namespace ILSystem.App_Code.BLL.System.Menu
{
    public class MenuBLL
    {
        private As400DAL m_MenuAdapter400 = null;
        //private OracleDAL m_MenuAdapterOra = null;    
        //private MySqlDAL m_MenuAdapterMySQL = null;
        private UserInfo m_UserInfo = null;

        private string m_UserSchema = EB_Service.Commons.AppConfig.OraAutUser;
        private string m_LastError;
        private const string m_TableName = "";

        private ulong m_UdpDate = Convert.ToUInt32(DateTime.Now.ToString("yyMMdd"));
        private ulong m_UdpTime = Convert.ToUInt32(DateTime.Now.ToString("HHmmss"));
        private string m_Program = "CSManageWEB";
        private string m_User;
        private string m_Wrkstn;

        public MenuBLL()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public UserInfo UserInformation
        {
            set { m_UserInfo = value; }
            get { return m_UserInfo; }
        }

        public DataSet GetDataSystemMenuFromUserID_PortalID()
        {
            DataSet DS = new DataSet();
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
            string strSQL = "SELECT DISTINCT(GNTA01.T01MID),GNTA02.T02ALI,GNTA02.T02SOU"
                    + ", GNTA02.T02NAM FROM GNTA01 RIGHT OUTER JOIN "
                    + "GNTA02 ON GNTA01.T01MID = GNTA02.T02MID  "
                    + " AND GNTA01.T01PID = GNTA02.T02OID "
                    + "WHERE (GNTA01.T01RID IN (SELECT GNTA04.T04RID FROM "
                    + "GNTA04 INNER JOIN GNTA05 ON GNTA04.T04RID = GNTA05.T05RID "
                    + " AND GNTA04.T04PID = GNTA05.T05OID "
                    + "WHERE GNTA05.T05NAM = '{0}' AND GNTA04.T04PID = {1})) "
                    + "AND (GNTA02.T02PID = 0) AND (GNTA02.T02OID={1}) "
                    + " Order By  (GNTA01.T01MID) ";
            /*
            if (WebConfigurationManager.AppSettings["AuthenticationBy"] == "0")
            {
                strSQL = String.Format(strSQL, AppConfig.GNLib, m_UserInfo.Username, m_UserInfo.PortalID);
                m_MenuAdapter400 = new As400DAL(m_UserInfo.Username, m_UserInfo.Password, AppConfig.GNLib, Providor400Types.PriSeries);
                m_MenuAdapter400.ConnectAs400();
                DS = m_MenuAdapter400.ExecuteDataSet(strSQL);
                m_MenuAdapter400.CloseConnect();
            }
            else if (WebConfigurationManager.AppSettings["AuthenticationBy"] == "1")   //Oracle Authentication mode
            {
                strSQL = String.Format(strSQL, AppConfig.OraAutUser, m_UserInfo.Username, m_UserInfo.PortalID);
                m_MenuAdapter = new OracleDAL(m_UserInfo.Username, m_UserInfo.Password);
                DS = m_MenuAdapter.ExecuteDataSet(strSQL);
            }
            else if (WebConfigurationManager.AppSettings["AuthenticationBy"] == "2")   //MySQL Authentication mode
            {
                strSQL = String.Format(strSQL, AppConfig.MySqlAutDbName, m_UserInfo.Username, m_UserInfo.PortalID);
                m_MenuAdapterMySQL = new MySqlDAL(m_UserInfo.Username, m_UserInfo.Password);
                DS = m_MenuAdapterMySQL.ExecuteDataSet(strSQL);
            }
            */

            switch (WebConfigurationManager.AppSettings["AuthenticationBy"].Trim())
            {
                case "0":
                    strSQL = String.Format(strSQL, m_UserInfo.Username, m_UserInfo.PortalID);
                    m_MenuAdapter400 = new As400DAL(m_UserInfo.Username, m_UserInfo.Password, AppConfig.GNLib, Providor400Types.PriSeries);
                    DS = m_MenuAdapter400.ExecuteDataSet(strSQL);
                    break;

            }

            return DS;
        }

        public DataSet GetDataSystemSubMenuByParentID(string strParentID)
        {
            DataSet DS = new DataSet();
            /*
            string strSQL = "SELECT DISTINCT({0}.GNTA01.T01MID),{0}.GNTA02.T02SOU,{0}.GNTA02.T02ALI FROM {0}.GNTA01 INNER JOIN {0}.GNTA02 "
                    + " ON {0}.GNTA01.T01MID = {0}.GNTA02.T02MID AND {0}.GNTA01.T01PID = {0}.GNTA02.T02OID "
                    + " WHERE ({0}.GNTA01.T01PID = {1}) "
                    + " AND ({0}.GNTA02.T02PID = {3}) AND "
                    + " ({0}.GNTA01.T01RID IN (SELECT {0}.GNTA04.T04RID FROM  {0}.GNTA04 INNER JOIN "
                    + " {0}.GNTA05 ON {0}.GNTA04.T04RID = {0}.GNTA05.T05RID AND "
                    + " {0}.GNTA04.T04PID = {0}.GNTA05.T05OID "
                    + " WHERE {0}.GNTA05.T05NAM = '" + m_UserInfo.Username.ToUpper().Trim() + "' AND {0}.GNTA04.T04PID = {1})) "
                    + " Order By ({0}.GNTA01.T01MID) ";
            */
            string strSQL = "SELECT DISTINCT(GNTA01.T01MID),GNTA02.T02SOU,GNTA02.T02ALI FROM GNTA01 INNER JOIN GNTA02 "
                            + " ON GNTA01.T01MID = GNTA02.T02MID AND GNTA01.T01PID = GNTA02.T02OID "
                            + " WHERE (GNTA01.T01PID = {0}) "
                            + " AND (GNTA02.T02PID = {2}) AND "
                            + " (GNTA01.T01RID IN (SELECT GNTA04.T04RID FROM  GNTA04 INNER JOIN "
                            + " GNTA05 ON GNTA04.T04RID = GNTA05.T05RID AND "
                            + " GNTA04.T04PID = GNTA05.T05OID "
                            + " WHERE GNTA05.T05NAM = '" + m_UserInfo.Username.ToUpper().Trim() + "' AND GNTA04.T04PID = {0})) "
                            + " Order By (GNTA01.T01MID) ";

            ArrayList arRoles = m_UserInfo.RolesID;
            string strRole = "";
            //for (int i = 0; i < arRoles.Count; i++)
            //    strRole += Convert.ToString(arRoles[i]) + (arRoles.Count - i > 1 ? "," : "");

            switch (WebConfigurationManager.AppSettings["AuthenticationBy"].Trim())
            {
                case "0":
                    strSQL = String.Format(strSQL, m_UserInfo.PortalID, strRole, strParentID);
                    m_MenuAdapter400 = new As400DAL(m_UserInfo.Username, m_UserInfo.Password, AppConfig.GNLib, Providor400Types.PriSeries);
                    DS = m_MenuAdapter400.ExecuteDataSet(strSQL);
                    break;

            }

            return DS;


        }
    }

}