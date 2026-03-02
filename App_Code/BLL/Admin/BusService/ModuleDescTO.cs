using EB_Service.Commons;
using EB_Service.DAL;
using ILSystem.App_Code.Commons;
using IBM.Data.DB2.iSeries;
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
	public class ModuleDescTO
	{
		private string m_Lib = AppConfig.GNLib;
		private string m_LastError;
		//Peak 05/05/2550 เปลี่ยนไปใช้ Lib GN
		//private string m_Lib = EB_Service.Common.AppConfig.AHPBackupLib;
		//private const string m_TableName = "HMTB63";
		private const string m_TableName = "GNTA02";
		private string m_ConnStr;

		private string m_UserID;
		private string m_UserName;
		private int m_RoleID;


		private DateTimeFormatInfo m_DThai = new CultureInfo("th-TH", false).DateTimeFormat;

		private string m_UdpDate = DateTime.Now.ToString("yyMMdd");
		private string m_UdpTime = DateTime.Now.ToString("HHmmss");
		private string m_Program = "USERWEB";
		private string m_User;
		private string m_Wrkstn = "";
		//Peak 05/05/2550 เปลี่ยนไปใช้ LIb GN
		//		private string m_T63MID ;
		//		private string m_T63ALI ;
		//		private string m_T63NAM ;
		//		private string m_T63SOU ;
		//		private string m_T63PID ;
		private string m_T02OID;
		private string m_T02MID;
		private string m_T02ALI;
		private string m_T02NAM;
		private string m_T02SOU;
		private string m_T02PID;


		private UserInfo m_UserInfo;

		private As400DAL m_da400 = new As400DAL();


		public ModuleDescTO()
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
		public string T02OID
		{
			get { return m_T02OID; }
			set { m_T02OID = value; }
		}
		public string T02MID
		{
			get { return m_T02MID; }
			set { m_T02MID = value; }
		}
		public string T02ALI
		{
			get { return m_T02ALI; }
			set { m_T02ALI = value; }
		}
		public string T02NAM
		{
			get { return m_T02NAM; }
			set { m_T02NAM = value; }
		}
		public string T02SOU
		{
			get { return m_T02SOU; }
			set { m_T02SOU = value; }
		}
		public string T02PID
		{
			get { return m_T02PID; }
			set { m_T02PID = value; }
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
				}
			}


			else
			{

			}
			return DS;
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

		#endregion

		public bool FindObject(string strSQL)
		{
			bool popSuccess = false;

			return false;
			if (WebConfigurationManager.AppSettings["AuthenticationBy"] == "0")  //AS400 Authentication
			{
				iDB2DataReader dr = null;
				IBM.Data.DB2.iSeries.iDB2Connection cnn = null;
				try
				{

					cnn = (iDB2Connection)m_da400.GetiSeriesDbAs400Connection();
					dr = m_da400.ExecuteDataReader(cnn, strSQL);
					//PopulateObject(ref dr);
					if (dr.HasRows)
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
			//return (dr.HasRows && popSuccess);
			return (popSuccess);
		}
		public string findMaxMID(string PortalID)
		{
			string MaxMID = "";
			string selectCmd = "";
			DataSet DS = null;

			if (WebConfigurationManager.AppSettings["AuthenticationBy"] == "0")  //AS400 Authentication
			{
				/*
                selectCmd = " Select Max(T02MID)AS MAX From " +
                                   EB_Service.Commons.AppConfig.GNLib + ".GNTA02 "
                                + " WHERE T02OID = '" + PortalID + "' ";
                */
				selectCmd = " Select Max(T02MID)AS MAX From GNTA02 WHERE T02OID = '" + PortalID + "' ";

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


			//return DS;
			if (DS.Tables[0].Rows.Count > 0 && DS != null)
				MaxMID = DS.Tables[0].Rows[0]["MAX"].ToString();
			else
				MaxMID = "";
			return ("0" + MaxMID);
		}

		public bool Create()
		{
			string ModuleID = "";
			ModuleID = findMaxMID(m_T02OID);
			if (ModuleID != "")
			{
				ModuleID = Convert.ToString(Convert.ToDecimal(ModuleID) + 1);
			}
			m_UdpDate = DateTime.Now.Date.ToString("yyMMdd", m_DThai);
			m_UdpTime = DateTime.Now.ToString("HHmmss", m_DThai);
			int affectedRows = -1;

			if (WebConfigurationManager.AppSettings["AuthenticationBy"] == "0")  //AS400 Authentication
			{
				iDB2Command cmd = new iDB2Command();
				cmd.CommandText = String.Format("INSERT INTO {0}(T02OID,T02MID,T02ALI,T02NAM,T02SOU,T02PID,"
					+ "T02UPD,T02UPT,T02PGM,T02USR,T02WKS) VALUES(@T02OID,@T02MID,@T02ALI,@T02NAM,@T02SOU,"
					+ "@T02PID,@T02UPD,@T02UPT,@T02PGM,@T02USR,@T02WKS)", m_TableName);

				cmd.Parameters.Add("@T02OID", m_T02OID);
				cmd.Parameters.Add("@T02MID", ModuleID);
				cmd.Parameters.Add("@T02ALI", m_T02ALI);
				cmd.Parameters.Add("@T02NAM", m_T02NAM);
				cmd.Parameters.Add("@T02SOU", m_T02SOU);
				cmd.Parameters.Add("@T02PID", m_T02PID);
				cmd.Parameters.Add("@T02UPD", m_UdpDate);
				cmd.Parameters.Add("@T02UPT", m_UdpTime);
				cmd.Parameters.Add("@T02PGM", m_Program);
				cmd.Parameters.Add("@T02USR", m_User);

				string strWrkStn = m_Wrkstn;
				if (m_Wrkstn.Trim().Length > 10)
				{
					strWrkStn = m_Wrkstn.Trim().Substring((m_Wrkstn.Trim().Length - 10), 10);
				}
				cmd.Parameters.Add("@T02WKS", strWrkStn);

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


			if (affectedRows > 0)
			{
				m_T02MID = ModuleID;
			}
			return (affectedRows > 0);
		}

		public bool Update()
		{
			m_UdpDate = DateTime.Now.Date.ToString("yyMMdd");
			m_UdpTime = DateTime.Now.ToString("HHmmss");
			int affectedRows = -1;

			if (WebConfigurationManager.AppSettings["AuthenticationBy"] == "0")  //AS400 Authentication
			{
				iDB2Command cmd = new iDB2Command();
				cmd.CommandText = String.Format("UPDATE GNTA02 SET T02PID=@T02PID, T02ALI=@T02ALI, T02NAM=@T02NAM, "
								+ "T02SOU=@T02SOU,T02UPD=@T02UPD,T02UPT=@T02UPT,T02PGM=@T02PGM,T02USR=@T02USR,"
								+ "T02WKS=@T02WKS WHERE T02MID=@T02MID AND T02OID=@T02OID ");

				cmd.Parameters.Add("@T02PID", this.m_T02PID);
				cmd.Parameters.Add("@T02ALI", this.m_T02ALI);
				cmd.Parameters.Add("@T02NAM", this.m_T02NAM);
				cmd.Parameters.Add("@T02SOU", this.m_T02SOU);
				cmd.Parameters.Add("@T02UPD", m_UdpDate);
				cmd.Parameters.Add("@T02UPT", m_UdpTime);
				cmd.Parameters.Add("@T02PGM", m_Program);
				cmd.Parameters.Add("@T02USR", m_User);

				string strWrkStn = m_Wrkstn;
				if (m_Wrkstn.Trim().Length > 10)
				{
					strWrkStn = m_Wrkstn.Trim().Substring((m_Wrkstn.Trim().Length - 10), 10);
				}
				cmd.Parameters.Add("@T02WKS", strWrkStn);

				cmd.Parameters.Add("@T02MID", this.m_T02MID);
				cmd.Parameters.Add("@T02OID", this.m_T02OID);

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

			//return (affectedRows > 0);
			if (affectedRows > 0)
				return true;
			else
				return false;
		}


	}
}