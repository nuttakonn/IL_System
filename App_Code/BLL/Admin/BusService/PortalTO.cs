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
	public class PortalTO
	{
		private string m_LastError;
		//Peak 05/05/2550 เปลี่ยนไปใช้ Lib GN
		//private string m_Lib = EB_Service.Common.AppConfig.AHPBackupLib;
		//private const string m_TableName = "HMTB63";
		private string m_Lib = AppConfig.GNLib;
		private const string m_TableName = "GNTA03";
		private string m_ConnStr;

		private string m_UserID;
		private string m_UserName;

		private DateTimeFormatInfo m_DThai = new CultureInfo("th-TH", false).DateTimeFormat;

		private Int32 m_UdpDate = Convert.ToInt32(DateTime.Now.ToString("yyMMdd"));
		private Int32 m_UdpTime = Convert.ToInt32(DateTime.Now.ToString("HHmmss"));
		private string m_Program = "USERWEB";
		private string m_User;
		private string m_Wrkstn = "";
		private string m_T03OID;    //PortalID
		private string m_T03ALI;    //Portal Header
		private string m_T03NAM;    //Portal Name
		private string m_T03URL;        //Portal URL (Path)
		private string m_T03PID;        //Portal ParentID
		private string m_T03ISP;        //IsParent
		private string m_T03ORD;        //Order

		private UserInfo m_UserInfo;

		private As400DAL m_da400 = new As400DAL();


		public PortalTO()
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

		public Int32 UdpDate
		{
			get { return m_UdpDate; }
			set { m_UdpDate = value; }
		}
		public Int32 UdpTime
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

		public string T03OID
		{
			get { return m_T03OID; }
			set { m_T03OID = value; }
		}
		public string T03ALI
		{
			get { return m_T03ALI; }
			set { m_T03ALI = value; }
		}
		public string T03NAM
		{
			get { return m_T03NAM; }
			set { m_T03NAM = value; }
		}
		public string T03URL
		{
			get { return m_T03URL; }
			set { m_T03URL = value; }
		}
		public string T03PID
		{
			get { return m_T03PID; }
			set { m_T03PID = value; }
		}
		public string T03ISP
		{
			get { return m_T03ISP; }
			set { m_T03ISP = value; }
		}
		public string T03ORD
		{
			get { return m_T03ORD; }
			set { m_T03ORD = value; }
		}


		#endregion

		#region "Entry DropdownList"

		public DataSet RetriveDDListAsDataSet(string selectCmd)
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
					Debug.WriteLine(ex.Message);
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
		public string findMaxPortalID()
		{
			string MaxPortalID = "";
			string selectCmd = "";
			DataSet DS = null;

			if (WebConfigurationManager.AppSettings["AuthenticationBy"] == "0")  //AS400 Authentication
			{
				selectCmd = " Select Max(T03OID)AS MAX From GNTA03 ";

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

			//return DS;
			if (DS.Tables[0].Rows.Count > 0 && DS != null)
				MaxPortalID = DS.Tables[0].Rows[0]["MAX"].ToString();
			else
				MaxPortalID = "";

			return ("0" + MaxPortalID);
		}

		public string findMaxOrder()
		{
			string MaxOrder = "";
			string selectCmd = "";
			DataSet DS = null;

			if (WebConfigurationManager.AppSettings["AuthenticationBy"] == "0")  //AS400 Authentication
			{
				selectCmd = " Select Max(T03ORD)AS MAX From GNTA03 ";

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
				MaxOrder = DS.Tables[0].Rows[0]["MAX"].ToString();
			else
				MaxOrder = "";
			return ("0" + MaxOrder);
		}

		public bool Create()
		{
			string PortalID = "";
			PortalID = findMaxPortalID();
			if (PortalID.Trim() != "")
			{
				PortalID = Convert.ToString(Convert.ToInt32(PortalID.Trim()) + 1);
			}
			m_UdpDate = Convert.ToInt32(DateTime.Now.Date.ToString("yyMMdd", m_DThai));
			m_UdpTime = Convert.ToInt32(DateTime.Now.ToString("HHmmss", m_DThai));
			int affectedRows = -1;

			if (WebConfigurationManager.AppSettings["AuthenticationBy"] == "0")  //AS400 Authentication
			{
				iDB2Command cmd = new iDB2Command();
				cmd.CommandText = String.Format("INSERT INTO {0}(T03OID,T03ALI,T03NAM,T03URL,T03PID,T03ISP,T03ORD, "
					+ "T03UPD,T03UPT,T03PGM,T03USR,T03WKS) VALUES(@T03OID,@T03ALI,@T03NAM,@T03URL,@T03PID,@T03ISP,@T03ORD, "
					+ "@T03UPD,@T03UPT,@T03PGM,@T03USR,@T03WKS)", m_TableName);

				cmd.Parameters.Add("@T03OID", this.m_T03OID);
				cmd.Parameters.Add("@T03ALI", this.m_T03ALI);
				cmd.Parameters.Add("@T03NAM", this.m_T03NAM);
				cmd.Parameters.Add("@T03URL", this.m_T03URL);
				cmd.Parameters.Add("@T03PID", this.m_T03PID);
				cmd.Parameters.Add("@T03ISP", this.m_T03ISP);
				cmd.Parameters.Add("@T03ORD", this.m_T03ORD);
				cmd.Parameters.Add("@T03UPD", this.m_UdpDate);
				cmd.Parameters.Add("@T03UPT", this.m_UdpTime);
				cmd.Parameters.Add("@T03PGM", this.m_Program);
				cmd.Parameters.Add("@T03USR", this.m_User);
				//cmd.Parameters.Add("@T03WKS", this.m_Wrkstn);                
				string strWrkStn = m_Wrkstn;
				if (m_Wrkstn.Trim().Length > 10)
				{
					strWrkStn = m_Wrkstn.Trim().Substring((m_Wrkstn.Trim().Length - 10), 10);
				}
				cmd.Parameters.Add("@T03WKS", strWrkStn);

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

			if (affectedRows > 0)
			{
				m_T03OID = PortalID;
			}
			return (affectedRows > 0);
		}

		public bool Update()
		{
			m_UdpDate = Convert.ToInt32(DateTime.Now.Date.ToString("yyMMdd"));
			m_UdpTime = Convert.ToInt32(DateTime.Now.ToString("HHmmss"));
			int affectedRows = -1;

			if (WebConfigurationManager.AppSettings["AuthenticationBy"] == "0")  //AS400 Authentication
			{
				iDB2Command cmd = new iDB2Command();
				cmd.CommandText = String.Format("UPDATE GNTA03 SET T03PID=@T03PID, T03ALI=@T03ALI, T03NAM=@T03NAM,T03URL=@T03URL, "
								+ "T03ISP=@T03ISP,T03ORD=@T03ORD,T03UPD=@T03UPD,T03UPT=@T03UPT,T03PGM=@T03PGM,T03USR=@T03USR,"
								+ "T03WKS=@T03WKS WHERE T03OID=@T03OID ");

				cmd.Parameters.Add("@T03PID", this.m_T03PID);
				cmd.Parameters.Add("@T03ALI", this.m_T03ALI);
				cmd.Parameters.Add("@T03NAM", this.m_T03NAM);
				cmd.Parameters.Add("@T03URL", this.m_T03URL);
				cmd.Parameters.Add("@T03ISP", this.m_T03ISP);
				cmd.Parameters.Add("@T03ORD", this.m_T03ORD);
				cmd.Parameters.Add("@T03UPD", this.m_UdpDate);
				cmd.Parameters.Add("@T03UPT", this.m_UdpTime);
				cmd.Parameters.Add("@T03PGM", this.m_Program);
				cmd.Parameters.Add("@T03USR", this.m_User);
				//cmd.Parameters.Add("@T03WKS", this.m_Wrkstn);
				string strWrkStn = m_Wrkstn;
				if (m_Wrkstn.Trim().Length > 10)
				{
					strWrkStn = m_Wrkstn.Trim().Substring((m_Wrkstn.Trim().Length - 10), 10);
				}
				cmd.Parameters.Add("@T03WKS", strWrkStn);

				cmd.Parameters.Add("@T03OID", this.m_T03OID);

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