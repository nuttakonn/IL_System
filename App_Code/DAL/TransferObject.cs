using System;
using System.Collections;
using IBM.Data.DB2.iSeries;

namespace EB_Service.BLLService_TO
{

	/// <summary>
	/// 
	/// </summary>
	/// 

	public abstract class EntityTO
	{
		protected bool m_bUpdatable = false;
		private string m_strTableName = "UNASSIGNED";
		protected string m_strObjectID = "UNASSIGNED";
		protected string m_strFieldOID = "UNASSIGNED";
		protected DateTime m_dtLastUpdDt = DateTime.Now;
		protected string m_strLastUpdBy = "UNASSIGNED";
		protected DateTime m_dtCreatedDt = DateTime.Now;
		protected string m_strCreatedBy = "UNASSIGNED";

		public abstract void PrepareUpdate();
		public abstract void UndoValueChanged();
		public abstract void ResetUpdatable();
		public abstract void Populate(iDB2DataReader dataReader);
		public abstract Hashtable GetFieldsChanged();

		#region Property Methods
		public string ObjectID
		{
			get { return m_strObjectID; }
			set { m_strObjectID = value; }
		}

		public string TableMapping
		{
			get { return m_strTableName; }
			set { m_strTableName = value; }
		}

		public string FiledObjectID
		{
			get { return m_strFieldOID; }
			set { m_strFieldOID = value; }
		}

		public DateTime CreatedDateTime
		{
			get { return m_dtCreatedDt; }
		}

		public string CreatedBy
		{
			get { return m_strCreatedBy; }
		}
		public DateTime LastUpdated
		{
			get { return m_dtLastUpdDt; }
		}

		public string LastUpdatedBy
		{
			get { return m_strLastUpdBy; }
		}

		public bool Updatable
		{
			get { return m_bUpdatable; }
		}

		#endregion Property Methods

		public virtual void CopyObject(EntityTO rhs)
		{
			if (rhs == this)
				return;

			m_bUpdatable = rhs.Updatable;
			m_strTableName = rhs.TableMapping;
			m_strObjectID = rhs.ObjectID;
			m_strFieldOID = rhs.FiledObjectID;
			m_strLastUpdBy = rhs.LastUpdatedBy;
			m_dtLastUpdDt = rhs.LastUpdated;
			m_strCreatedBy = rhs.CreatedBy;
			m_dtCreatedDt = rhs.CreatedDateTime;
		}
	}


	//Peak 25/7/2550 เพิ่ม Class TransferLog เพื่อไว้ใช้ Call เมื่อมีการ Export Data ที่สามารถ Edit ได้ให้กับ User
	public class TransferLog
	{
		private string m_LastError = "";
		private System.Globalization.DateTimeFormatInfo m_DThai = new System.Globalization.CultureInfo("th-TH", false).DateTimeFormat;
		private EB_Service.DAL.As400DAL m_da400 = new EB_Service.DAL.As400DAL();

		private ulong m_UpdDate = Convert.ToUInt32(DateTime.Now.ToString("yyMMdd"));
		private ulong m_UpdTime = Convert.ToUInt32(DateTime.Now.ToString("HHmmss"));

		public string TransferWriteLog(string PgmTransferFile, string TransferFileType, int TotalRecord, string UpdProgram,
			ILSystem.App_Code.Commons.UserInfo m_UserInfo, string LibWriteLog, string FileWriteLog)
		{
			//MTFPGM,MTFTFF,MTFREC,MTFUDT,MTFUTM,MTFUUS,MTFUPG,MTFUWS
			//ProgramTransfer, TransferFile, TotalRecord, UpdDate, UpdTime, UpdUser, UpdProgram, WrkStation
			m_UpdDate = Convert.ToUInt32(DateTime.Now.Date.ToString("yyyyMMdd", m_DThai));
			m_UpdTime = Convert.ToUInt32(DateTime.Now.ToString("HHmmss", m_DThai));

			m_da400.UserName = m_UserInfo.Username;
			m_da400.Password = m_UserInfo.Password;
			m_da400.AS400Lib = LibWriteLog;

			IBM.Data.DB2.iSeries.iDB2Command cmd = new IBM.Data.DB2.iSeries.iDB2Command();

			cmd.CommandText = String.Format("Insert Into {1}(MTFPGM,MTFTFF,MTFREC, "
				+ " MTFUDT,MTFUTM,MTFUUS,MTFUPG,MTFUWS) "
				+ "VALUES('" + PgmTransferFile.ToUpper().Trim() + "','" + TransferFileType.ToUpper().Trim() + "'," + TotalRecord + ", " +
				"" + m_UpdDate + "," + m_UpdTime + ",'" + m_UserInfo.Username + "','" + UpdProgram + "','" + m_UserInfo.LocalClient + "')", LibWriteLog, FileWriteLog);

			int affectedRows = -1;
			try
			{
				affectedRows = m_da400.ExecuteNonQuery(cmd);
			}
			catch (Exception ex)
			{
				m_LastError = ex.Message;
			}
			return (m_LastError);

		}

	}
}
