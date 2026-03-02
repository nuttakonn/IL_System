using System.Text;
using System.Linq;
using System.Web;
using IBM.Data.DB2.iSeries;
using System.Web.Configuration;
using System.Collections;
using Microsoft.VisualBasic;
using System.Data;
using System.Globalization;
using System;
using System.Diagnostics;
using ILSystem.App_Code.Commons;
using EB_Service.DAL;

namespace ILSystem.App_Code.BLL.DataCenter
{
    public class ILDataCenter
    {
        private string m_LastError;
        private string m_ConnStr;

        private string m_user_name;
        private string m_user_leader;
        private string m_status;

        private DateTimeFormatInfo m_DThai = new CultureInfo("th-TH", false).DateTimeFormat;

        private ulong m_UpdDate = Convert.ToUInt32(DateTime.Now.ToString("yyMMdd"));
        private ulong m_UpdTime = Convert.ToUInt32(DateTime.Now.ToString("HHmmss"));
        private ulong m_UdpD = Convert.ToUInt32(DateTime.Now.ToString("yyyyMMdd"));
        private ulong m_UdpT = Convert.ToUInt32(DateTime.Now.ToString("HHmmss"));
        private string m_UserName;
        private string m_User;
        private string m_Wrkstn = "";
        private string m_Autodial_Usage = "";
        private UserInfo m_UserInfo;
        private As400DAL m_da400 = new As400DAL();

        public string LastError;

        public UserInfo UserInfomation
        {
            set
            {
                m_UserInfo = value;
                m_UserName = m_UserInfo.Username;
                m_da400.UserName = m_UserInfo.Username;
                m_da400.Password = m_UserInfo.Password;
                m_User = m_UserInfo?.Username;
                m_Wrkstn = m_UserInfo?.LocalClient;
                //m_da400.ConnectAs400();
            }
        }
        #region Standard Function
        //public bool Connect400()
        //{
        //    return m_da400.ConnectAs400();
        //}
        public void CloseConnectioDAL()
        {
            m_da400.CloseConnect();
        }
        public bool CommitDAL()
        {
            return m_da400.CommitAs400();
        }
        public bool RollbackDAL()
        {
            return m_da400.RollbackAs400();
        }
        public string ConnStr
        {
            get { return m_ConnStr; }
            set { m_ConnStr = value; }
        }
        public ulong UpdDate
        {
            get { return m_UpdDate; }
            set { m_UpdDate = value; }
        }
        public ulong UpdTime
        {
            get { return m_UpdTime; }
            set { m_UpdTime = value; }
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
        public void OpenConnectioDAL()
        {
            iDB2Connection dConnection = m_da400.GetiSeriesDbAs400Connection();
        }

        public DataSet RetriveAsDataSet(string selectCmd)
        {
            iDB2Command cmd = new iDB2Command();
            cmd.CommandText = selectCmd;

            DataSet DS = null;
            try
            {
                DS = m_da400.ExecuteDataSet(cmd);
            }
            catch (Exception ex)
            {
                m_LastError = ex.Message;
                //Debug.WriteLine(ex.Message);
            }
            return DS;
        }

        public DataSet RetriveAsDataSet(iDB2Command cmd)
        {
            DataSet DS = null;
            try
            {
                DS = m_da400.ExecuteDataSet(cmd);
            }
            catch (Exception ex)
            {
                m_LastError = ex.Message;
                //Debug.WriteLine(ex.Message);
            }
            return DS;
        }

        public DataSet RetriveAsDataSet(StringBuilder stbselectCmd)
        {
            iDB2Command cmd = new iDB2Command();
            cmd.CommandText = stbselectCmd.ToString();
            cmd.CommandTimeout = 120;
            DataSet DS = null;
            try
            {
                DS = m_da400.ExecuteDataSet(cmd);
            }
            catch (Exception ex)
            {
                m_LastError = ex.Message;
                Debug.WriteLine(ex.Message);
            }
            return DS;
        }
        public DataSet RetriveAsDataSet_AS400(string StrCmd)
        {
            DataSet DS = new DataSet();
            iDB2Command cmd = new iDB2Command();
            cmd.CommandText = StrCmd;

            try
            {
                DS = m_da400.ExecuteDataSet(cmd);
            }
            catch (Exception ex)
            {
                m_LastError = ex.Message;
            }
            return DS;
        }
        public bool ExcuteiDB2Command(iDB2Command prmIcmd)
        {
            int affectedRows = -1;
            bool success = false;
            try
            {
                affectedRows = m_da400.ExecuteNonQuery(prmIcmd);
                if (affectedRows > 0)
                    success = true;
            }
            catch (Exception ex)
            {
                m_LastError = ex.Message;
            }
            return (success);
        }
        public int ExecuteNonQuery(iDB2Command db2Command)
        {
            int iRet = -1;
            try
            {
                iRet = m_da400.ExecuteNonQuery(db2Command);
            }
            catch (iDB2Exception iDb2ex)
            {
                iRet = -1;
            }
            return iRet;
        }



        //****  for commit rollback   *****//
        public DataSet RetriveAsDataSetNoConnect(string selectCmd)
        {
            iDB2Command cmd = new iDB2Command();
            cmd.CommandText = selectCmd;

            DataSet DS = null;
            try
            {
                DS = m_da400.ExecuteDataSetNoCloseConnect(cmd);
            }
            catch (Exception ex)
            {
                m_LastError = ex.Message;
            }
            cmd.Dispose();
            return DS;
        }
        public int ExecuteNonQueryNoCommit(iDB2Command db2Command)
        {
            int iRet = -1;
            try
            {
                iRet = m_da400.ExecuteNonQueryNoCommit(db2Command);

            }
            catch (iDB2Exception iDb2ex)
            {
                iRet = -1;

            }
            return iRet;
        }
        #endregion

        #region Call subroutine
        //  CALL  Company name
        public bool CALL_GNSRCONM(string WKLNG, string WKTYP, string WKLEN, ref string WKNME)
        {
            string m_UdpTime = DateTime.Now.ToString("HHmmss");
            m_LastError = "";
            iDB2Command cmd = new iDB2Command();

            cmd.CommandType = CommandType.StoredProcedure;
            //cmd.CommandText = m_CSPGM + ".CSSRW11";
            cmd.CommandText = "GNSRCONM";
            // Parameter In
            cmd.Parameters.Add("WKLNG", iDB2DbType.iDB2Char, 2).Value = WKLNG;
            cmd.Parameters.Add("WKTYP", iDB2DbType.iDB2Decimal, 0).Value = WKTYP;
            cmd.Parameters.Add("WKLEN", iDB2DbType.iDB2Decimal, 0).Value = WKLEN;
            cmd.Parameters["WKLEN"].Precision = 3;
            cmd.Parameters["WKLEN"].Scale = 0;
            cmd.Parameters.Add("WKNME", iDB2DbType.iDB2Char, 100).Direction = ParameterDirection.Output;
            if (m_da400.ExecuteSubRoutine("GNSRCONM", ref cmd))
            {
                WKNME = cmd.Parameters["WKNME"].Value.ToString().Trim();
                return true;
            }
            else
                return false;
        }
        public bool Call_GNSRBLC(string ID_IN, string CSCD_IN, string APPCODE_IN, string SOURCE_IN, string Case_IN,
                                 ref string prmError, ref string prmErrorMsg, string strBizInit, string strBranchNo)
        {
            iDB2Command cmd = new iDB2Command();

            m_da400.AS400Lib = EB_Service.Commons.AppConfig.GNPGMLib;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "GNSRBLC";

            // INPUT
            cmd.Parameters.Add("WKIDNO", iDB2DbType.iDB2Char, 15).Value = ID_IN.ToString().Trim();
            cmd.Parameters.Add("WKCSCD", iDB2DbType.iDB2Decimal, 0).Value = CSCD_IN.ToString().Trim();
            cmd.Parameters["WKCSCD"].Precision = 16;
            cmd.Parameters["WKCSCD"].Scale = 0;
            cmd.Parameters.Add("WKBS", iDB2DbType.iDB2Char, 2).Value = APPCODE_IN.ToString().Trim();
            cmd.Parameters.Add("WKSRC", iDB2DbType.iDB2Char, 3).Value = SOURCE_IN.ToString().Trim();
            cmd.Parameters.Add("WKCAS", iDB2DbType.iDB2Char, 3).Value = Case_IN.ToString().Trim();

            // OUTPUT
            cmd.Parameters.Add("WKERR", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("WKMSG", iDB2DbType.iDB2Char, 50).Direction = ParameterDirection.Output;

            if (m_da400.ExecuteSubRoutineNoConnect("GNSRBLC", ref cmd, strBizInit, strBranchNo))
            {
                prmError = cmd.Parameters["WKERR"].Value.ToString().Trim();
                prmErrorMsg = cmd.Parameters["WKMSG"].Value.ToString().Trim();
                //m_da400.CloseConnect();
                return true;
            }
            else
            {
                //m_da400.CloseConnect();
                return false;
            }
        }

        public DataSet Get_GNSRAM(string prmstrIDNo, string prmstrName, string prmstrSurname, string prmstrBirthDateYMD, string prmstrPROC,
            string strBizInit, string strBranchNo) //[56991] เพิ่ม Parameter IP, Parm สำหรับ Initial Lib -Biz, -Branch No
        {
            iDB2Command cmd = new iDB2Command();
            m_da400.AS400Lib = EB_Service.Commons.AppConfig.GNPGMLib;  //GeneralLib;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "GNSRAM"; //[56991] แก้ไขเรือง Fix Lib //m_da400.AS400Lib.Trim() + ".GNSRAM";

            // INPUT
            cmd.Parameters.Add("PIIDNO", iDB2DbType.iDB2Char, 15).Value = prmstrIDNo.Trim();
            cmd.Parameters.Add("PINAME", iDB2DbType.iDB2Char, 50).Value = prmstrName.Trim();
            cmd.Parameters.Add("PISNME", iDB2DbType.iDB2Char, 50).Value = prmstrSurname.Trim();
            cmd.Parameters.Add("PIBDTE", iDB2DbType.iDB2Char, 8).Value = prmstrBirthDateYMD.Trim();
            cmd.Parameters.Add("PIPROC", iDB2DbType.iDB2Char, 3).Value = prmstrPROC;

            //OUTPUT DATA                                             
            //-	Amlo Group           = POAGRP ( 3A)           
            //-	Amlo Message         = POAFGC (20A)      เอา Parameter นี้ไปแสดง
            //-	Match Type           = POMTYP ( 1A)            
            //-	Flag Error           = POERR ( 1A)       *Blanks: อนุญาตให้ทำรายการ, Y: ไม่อนุญาตให้ทำรายการได้ 
            //-	SETON LR             = WPOLR ( 1A)              
            cmd.Parameters.Add("POAGRP", iDB2DbType.iDB2Char, 3).Direction = ParameterDirection.Output;  //Amlo Group
            cmd.Parameters.Add("POAFGC", iDB2DbType.iDB2Char, 20).Direction = ParameterDirection.Output;  //Amlo Message
            cmd.Parameters.Add("POMTYP", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;  //Match Type
            cmd.Parameters.Add("POERR", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;  //Flag Error
            cmd.Parameters.Add("WPOLR", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;  //SETON LR


            DataSet dsResult = new DataSet();
            DataTable dt = new DataTable();
            dt.Columns.Add("PIIDNO");
            dt.Columns.Add("PINAME");
            dt.Columns.Add("PISNME");
            dt.Columns.Add("PIBDTE");
            dt.Columns.Add("PIPROC");
            dt.Columns.Add("POAGRP");
            dt.Columns.Add("POAFGC");
            dt.Columns.Add("POMTYP");
            dt.Columns.Add("POERR");
            dt.Columns.Add("WPOLR");

            DataRow dr = dt.NewRow();
            dr["PIIDNO"] = prmstrIDNo.Trim();
            dr["PINAME"] = prmstrName.Trim();
            dr["PISNME"] = prmstrSurname.Trim();
            dr["PIBDTE"] = prmstrBirthDateYMD.Trim();
            dr["PIPROC"] = prmstrPROC.Trim();
            dr["POAGRP"] = "";
            dr["POAFGC"] = "";
            dr["POMTYP"] = "";
            dr["POERR"] = "";

            m_da400.ConnectAs400();
            try
            {
                if (m_da400.ExecuteSubRoutine(cmd.CommandText, ref cmd, strBizInit, strBranchNo))  //[56991] เพิ่ม Parameter IP, Parm สำหรับ Initial Lib -Biz, -Branch No
                {
                    if (cmd.Parameters["POAGRP"].Value.ToString().Trim() != "")
                    {
                        dr["POAGRP"] = cmd.Parameters["POAGRP"].Value.ToString().Trim();
                    }

                    if (cmd.Parameters["POAFGC"].Value.ToString().Trim() != "")
                    {
                        dr["POAFGC"] = cmd.Parameters["POAFGC"].Value.ToString().Trim();
                    }

                    if (cmd.Parameters["POMTYP"].Value.ToString().Trim() != "")
                    {
                        dr["POMTYP"] = cmd.Parameters["POMTYP"].Value.ToString().Trim();
                    }

                    if (cmd.Parameters["POERR"].Value.ToString().Trim() != "")
                    {
                        dr["POERR"] = cmd.Parameters["POERR"].Value.ToString().Trim();
                    }

                    if (cmd.Parameters["WPOLR"].Value.ToString().Trim() != "")
                    {
                        dr["WPOLR"] = cmd.Parameters["WPOLR"].Value.ToString().Trim();
                    }
                }
            }
            catch (Exception ex)
            { m_LastError = ex.Message; }

            m_da400.CloseConnect();


            dt.Rows.Add(dr);
            dsResult.Tables.Add(dt);

            return dsResult;
        }

        public bool CALL_CSSRRFAM(string CSN_IN, ref string prmError, ref string prmErrorRes, string strBizInit, string strBranchNo)
        {
            iDB2Command cmd = new iDB2Command();

            m_da400.AS400Lib = EB_Service.Commons.AppConfig.GNPGMLib;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "CSSRRFCM";

            // INPUT
            cmd.Parameters.Add("CSNSTR", iDB2DbType.iDB2Char, 8).Value = CSN_IN.ToString().Trim();

            // OUTPUT
            cmd.Parameters.Add("ERRFLG", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("RESFLG", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;

            if (m_da400.ExecuteSubRoutine("CSSRRFCM", ref cmd, strBizInit, strBranchNo))
            {
                prmError = cmd.Parameters["ERRFLG"].Value.ToString().Trim();
                prmErrorRes = cmd.Parameters["RESFLG"].Value.ToString().Trim();
                //m_da400.CloseConnect();
                return true;
            }
            else
            {
                //m_da400.CloseConnect();
                return false;
            }
        }

        //ถ้าจะ Copy อย่าเอาตัวนี้ไปเพราะตัวนี้จะไม่เปิด Connection เพิ่ม และใช้ ExecuteSubRoutineNoConnect เพราะมันจะอยู่ใน Loop Save ต้อง Commit Rollback
        public bool CALL_CSSR16(string Mode_IN, string Type_IN, string CSN_IN, string RSQ_IN, string SHIPTO_IN, string Teltype_IN, string Telno_IN, string Telext_IN,
                                ref string prmError, string strBizInit, string strBranchNo)
        {
            iDB2Command cmd = new iDB2Command();

            m_da400.AS400Lib = EB_Service.Commons.AppConfig.GNPGMLib;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "CSSR16";

            // INPUT
            cmd.Parameters.Add("PIMODE", iDB2DbType.iDB2Char, 1).Value = Mode_IN.ToString().Trim();
            cmd.Parameters.Add("PITYPE", iDB2DbType.iDB2Char, 1).Value = Type_IN.ToString().Trim();
            cmd.Parameters.Add("PICSNO", iDB2DbType.iDB2Char, 8).Value = CSN_IN.ToString().Trim();
            cmd.Parameters.Add("PIRSQ", iDB2DbType.iDB2Char, 3).Value = RSQ_IN.ToString().Trim();
            cmd.Parameters.Add("PISHTO", iDB2DbType.iDB2Char, 1).Value = SHIPTO_IN.ToString().Trim();
            cmd.Parameters.Add("PITELY", iDB2DbType.iDB2Char, 1).Value = Teltype_IN.ToString().Trim();
            cmd.Parameters.Add("PITELT", iDB2DbType.iDB2Char, 20).Value = Telno_IN.ToString().Trim();
            cmd.Parameters.Add("PIEXTN", iDB2DbType.iDB2Char, 15).Value = Telext_IN.ToString().Trim();

            // OUTPUT
            cmd.Parameters.Add("POERR", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;

            if (m_da400.ExecuteSubRoutineNoConnect("CSSR16", ref cmd, strBizInit, strBranchNo))
            {
                prmError = cmd.Parameters["POERR"].Value.ToString().Trim();
                //m_da400.CloseConnect();
                return true;
            }
            else
            {
                //m_da400.CloseConnect();
                return false;
            }
        }

        public bool CALL_ILSR16(string Mode_IN, string CSN_IN, string BRN_IN, string APPNO_IN, ref string prmError, string strBizInit, string strBranchNo)
        {
            iDB2Command cmd = new iDB2Command();

            m_da400.AS400Lib = EB_Service.Commons.AppConfig.GNPGMLib;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ILSR16";

            // INPUT
            cmd.Parameters.Add("WIMODE", iDB2DbType.iDB2Char, 1).Value = Mode_IN.ToString().Trim();
            cmd.Parameters.Add("WICSN", iDB2DbType.iDB2Char, 8).Value = CSN_IN.ToString().Trim();
            cmd.Parameters.Add("WIBRN", iDB2DbType.iDB2Char, 3).Value = BRN_IN.ToString().Trim();
            cmd.Parameters.Add("WIAPN", iDB2DbType.iDB2Char, 11).Value = APPNO_IN.ToString().Trim();

            // OUTPUT
            cmd.Parameters.Add("WOERR", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;

            if (m_da400.ExecuteSubRoutine("ILSR16", ref cmd, strBizInit, strBranchNo))
            {
                prmError = cmd.Parameters["WOERR"].Value.ToString().Trim();
                //m_da400.CloseConnect();
                return true;
            }
            else
            {
                //m_da400.CloseConnect();
                return false;
            }
        }

        public bool Call_CSSR034(string prm1, string prm2, string prm3, string prm4, string prm5,
                                 string prm6, string prm7, string prm8, string prm9, string prm10,
                                 ref string prmReqno, ref string prmErrMsg,
            string strBizInit, string strBranchNo) //[56991] เพิ่ม Parameter IP, Parm สำหรับ Initial Lib -Biz, -Branch No
        {
            m_LastError = "";
            iDB2Command cmd = new iDB2Command();

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.CommandText = "CSSR034";
            cmd.Parameters.Add("PIMODE", iDB2DbType.iDB2Char, 1).Value = prm1;
            cmd.Parameters.Add("PIAPPL", iDB2DbType.iDB2Char, 1).Value = prm2;
            cmd.Parameters.Add("PIOCCU", iDB2DbType.iDB2Char, 3).Value = prm3;
            cmd.Parameters.Add("PIDOCF", iDB2DbType.iDB2Char, 1).Value = prm4;
            cmd.Parameters.Add("PICSNO", iDB2DbType.iDB2Decimal, 0).Value = prm5;
            cmd.Parameters.Add("PIBUSS", iDB2DbType.iDB2Char, 2).Value = prm6;
            cmd.Parameters.Add("PIBRAN", iDB2DbType.iDB2Decimal, 0).Value = prm7;
            cmd.Parameters.Add("PIAPNO", iDB2DbType.iDB2Decimal, 0).Value = prm8;
            cmd.Parameters.Add("PISDAT", iDB2DbType.iDB2Decimal, 0).Value = prm9;
            cmd.Parameters.Add("PISTIM", iDB2DbType.iDB2Decimal, 0).Value = prm10;
            cmd.Parameters.Add("POERRC", iDB2DbType.iDB2Char, 3).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("POERRM", iDB2DbType.iDB2Char, 64).Direction = ParameterDirection.Output;

            //if (m_da400.ExecuteSubRoutine(m_CSPGM + ".CSSR034", ref cmd))
            if (m_da400.ExecuteSubRoutine("CSSR034", ref cmd, strBizInit, strBranchNo))
            {
                prmReqno = cmd.Parameters["POERRC"].Value.ToString();
                prmErrMsg = cmd.Parameters["POERRM"].Value.ToString();
                return true;
            }
            else
                return false;
        }

        public bool CALL_GNSR86(string prmPPBUSS, string prmPPLNTY, string prmPPBRN, string prmPPAPDT, string prmPPAVDT, string prmPPRANK, string prmPPITCL,
                                ref string PPOTCL, ref string POERR, ref string PERMSG, string strBizInit, string strBranchNo)
        {
            iDB2Command cmd = new iDB2Command();

            m_da400.AS400Lib = EB_Service.Commons.AppConfig.GNPGMLib;  //GeneralLib;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "GNSR86";

            cmd.Parameters.Add("PPBUSS", iDB2DbType.iDB2Char, 2).Value = prmPPBUSS;
            cmd.Parameters.Add("PPLNTY", iDB2DbType.iDB2Char, 2).Value = prmPPLNTY;
            cmd.Parameters.Add("PPBRN", iDB2DbType.iDB2Decimal, 0).Value = prmPPBRN;
            cmd.Parameters["PPBRN"].Precision = 3;
            cmd.Parameters["PPBRN"].Scale = 0;
            cmd.Parameters.Add("PPAPDT", iDB2DbType.iDB2Decimal, 0).Value = prmPPAPDT;
            cmd.Parameters["PPAPDT"].Precision = 8;
            cmd.Parameters["PPAPDT"].Scale = 0;
            cmd.Parameters.Add("PPAVDT", iDB2DbType.iDB2Decimal, 0).Value = prmPPAVDT;
            cmd.Parameters["PPAVDT"].Precision = 8;
            cmd.Parameters["PPAVDT"].Scale = 0;
            cmd.Parameters.Add("PPRANK", iDB2DbType.iDB2Decimal, 0).Value = prmPPRANK;
            cmd.Parameters["PPRANK"].Precision = 3;
            cmd.Parameters["PPRANK"].Scale = 0;
            cmd.Parameters.Add("PPITCL", iDB2DbType.iDB2Decimal, 0).Value = prmPPITCL;
            cmd.Parameters["PPITCL"].Precision = 13;
            cmd.Parameters["PPITCL"].Scale = 2;

            //***   out put ***//
            cmd.Parameters.Add("PPOTCL", iDB2DbType.iDB2Decimal, 0).Direction = ParameterDirection.Output;
            cmd.Parameters["PPOTCL"].Precision = 13;
            cmd.Parameters["PPOTCL"].Scale = 2;
            cmd.Parameters["PPOTCL"].Value = 0;

            cmd.Parameters.Add("POERR", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;
            cmd.Parameters["POERR"].Value = "";

            cmd.Parameters.Add("PERMSG", iDB2DbType.iDB2Char, 64).Direction = ParameterDirection.Output;
            cmd.Parameters["PERMSG"].Value = "";

            if (m_da400.ExecuteSubRoutine("GNSR86", ref cmd, strBizInit, strBranchNo))
            {
                PPOTCL = cmd.Parameters["PPOTCL"].Value.ToString().Trim();
                POERR = cmd.Parameters["POERR"].Value.ToString().Trim();
                PERMSG = cmd.Parameters["PERMSG"].Value.ToString().Trim();
                m_da400.CloseConnect();
                return true;
            }
            else
            {
                m_da400.CloseConnect();
                return false;
            }
        }

        public bool CALL_CSSR31(string prmPICSN, string prmPISAL, string prmPIFLG, string prmPIRANK, string prmPIOCC, string prmBRN, string prmPISINCP, string prmPINCAJ,
                                ref string POSALN, ref string POORNK, ref string POOTMS, ref string POOACL, ref string POARNK, ref string POATMS, ref string POAACL,
                                ref string PORRNK, ref string PORTMS, ref string PORACL, ref string POFTCL, ref string POSTRP, ref string POTOTP, ref string POAFLG,
                                ref string POHAVE, ref string POODGC, ref string POMDL, ref string POPD, ref string POGNO,
                                string strBizInit, string strBranchNo)
        {
            iDB2Command cmd = new iDB2Command();

            m_da400.AS400Lib = EB_Service.Commons.AppConfig.GNPGMLib;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "CSSR31";

            cmd.Parameters.Add("PICSN", iDB2DbType.iDB2Decimal, 0).Value = prmPICSN;
            cmd.Parameters["PICSN"].Precision = 8;
            cmd.Parameters["PICSN"].Scale = 0;

            cmd.Parameters.Add("PISAL", iDB2DbType.iDB2Decimal, 0).Value = prmPISAL;
            cmd.Parameters["PISAL"].Precision = 9;
            cmd.Parameters["PISAL"].Scale = 2;

            cmd.Parameters.Add("PIFLG", iDB2DbType.iDB2Char, 1).Value = prmPIFLG;

            cmd.Parameters.Add("PIRANK", iDB2DbType.iDB2Decimal, 0).Value = prmPIRANK;
            cmd.Parameters["PIRANK"].Precision = 2;
            cmd.Parameters["PIRANK"].Scale = 0;

            cmd.Parameters.Add("PIOCC", iDB2DbType.iDB2Char, 3).Value = prmPIOCC;

            cmd.Parameters.Add("PIBRN", iDB2DbType.iDB2Decimal, 0).Value = prmBRN;
            cmd.Parameters["PIBRN"].Precision = 3;
            cmd.Parameters["PIBRN"].Scale = 0;

            cmd.Parameters.Add("PISINCP", iDB2DbType.iDB2Char, 2).Value = prmPISINCP;

            cmd.Parameters.Add("PINCAJ", iDB2DbType.iDB2Decimal, 0).Value = prmPINCAJ;
            cmd.Parameters["PINCAJ"].Precision = 9;
            cmd.Parameters["PINCAJ"].Scale = 2;

            //***   out put ***//
            cmd.Parameters.Add("POSALN", iDB2DbType.iDB2Decimal, 0).Direction = ParameterDirection.Output;
            cmd.Parameters["POSALN"].Precision = 9;
            cmd.Parameters["POSALN"].Scale = 2;
            cmd.Parameters["POSALN"].Value = 0;

            cmd.Parameters.Add("POORNK", iDB2DbType.iDB2Decimal, 0).Direction = ParameterDirection.Output;
            cmd.Parameters["POORNK"].Precision = 2;
            cmd.Parameters["POORNK"].Scale = 0;
            cmd.Parameters["POORNK"].Value = 0;

            cmd.Parameters.Add("POOTMS", iDB2DbType.iDB2Decimal, 0).Direction = ParameterDirection.Output;
            cmd.Parameters["POOTMS"].Precision = 5;
            cmd.Parameters["POOTMS"].Scale = 3;
            cmd.Parameters["POOTMS"].Value = 0;

            cmd.Parameters.Add("POOACL", iDB2DbType.iDB2Decimal, 0).Direction = ParameterDirection.Output;
            cmd.Parameters["POOACL"].Precision = 13;
            cmd.Parameters["POOACL"].Scale = 2;
            cmd.Parameters["POOACL"].Value = 0;

            cmd.Parameters.Add("POARNK", iDB2DbType.iDB2Decimal, 0).Direction = ParameterDirection.Output;
            cmd.Parameters["POARNK"].Precision = 2;
            cmd.Parameters["POARNK"].Scale = 0;
            cmd.Parameters["POARNK"].Value = 0;

            cmd.Parameters.Add("POATMS", iDB2DbType.iDB2Decimal, 0).Direction = ParameterDirection.Output;
            cmd.Parameters["POATMS"].Precision = 5;
            cmd.Parameters["POATMS"].Scale = 3;
            cmd.Parameters["POATMS"].Value = 0;

            cmd.Parameters.Add("POAACL", iDB2DbType.iDB2Decimal, 0).Direction = ParameterDirection.Output;
            cmd.Parameters["POAACL"].Precision = 13;
            cmd.Parameters["POAACL"].Scale = 2;
            cmd.Parameters["POAACL"].Value = 0;

            cmd.Parameters.Add("PORRNK", iDB2DbType.iDB2Decimal, 0).Direction = ParameterDirection.Output;
            cmd.Parameters["PORRNK"].Precision = 2;
            cmd.Parameters["PORRNK"].Scale = 0;
            cmd.Parameters["PORRNK"].Value = 0;

            cmd.Parameters.Add("PORTMS", iDB2DbType.iDB2Decimal, 0).Direction = ParameterDirection.Output;
            cmd.Parameters["PORTMS"].Precision = 5;
            cmd.Parameters["PORTMS"].Scale = 3;
            cmd.Parameters["PORTMS"].Value = 0;

            cmd.Parameters.Add("PORACL", iDB2DbType.iDB2Decimal, 0).Direction = ParameterDirection.Output;
            cmd.Parameters["PORACL"].Precision = 13;
            cmd.Parameters["PORACL"].Scale = 2;
            cmd.Parameters["PORACL"].Value = 0;

            cmd.Parameters.Add("POFTCL", iDB2DbType.iDB2Decimal, 0).Direction = ParameterDirection.Output;
            cmd.Parameters["POFTCL"].Precision = 13;
            cmd.Parameters["POFTCL"].Scale = 2;
            cmd.Parameters["POFTCL"].Value = 0;

            cmd.Parameters.Add("POSTRP", iDB2DbType.iDB2Decimal, 0).Direction = ParameterDirection.Output;
            cmd.Parameters["POSTRP"].Precision = 2;
            cmd.Parameters["POSTRP"].Scale = 0;
            cmd.Parameters["POSTRP"].Value = 0;

            cmd.Parameters.Add("POTOTP", iDB2DbType.iDB2Decimal, 0).Direction = ParameterDirection.Output;
            cmd.Parameters["POTOTP"].Precision = 2;
            cmd.Parameters["POTOTP"].Scale = 0;
            cmd.Parameters["POTOTP"].Value = 0;

            cmd.Parameters.Add("POAFLG", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;
            cmd.Parameters["POAFLG"].Value = "";

            cmd.Parameters.Add("POHAVE", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;

            cmd.Parameters.Add("POODGC", iDB2DbType.iDB2Decimal, 0).Direction = ParameterDirection.Output;
            cmd.Parameters["POODGC"].Precision = 2;
            cmd.Parameters["POODGC"].Scale = 0;
            cmd.Parameters["POODGC"].Value = 0;

            cmd.Parameters.Add("POMDL", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;

            cmd.Parameters.Add("POPD", iDB2DbType.iDB2Decimal, 0).Direction = ParameterDirection.Output;
            cmd.Parameters["POPD"].Precision = 10;
            cmd.Parameters["POPD"].Scale = 9;
            cmd.Parameters["POPD"].Value = 0;

            cmd.Parameters.Add("POGNO", iDB2DbType.iDB2Decimal, 0).Direction = ParameterDirection.Output;
            cmd.Parameters["POGNO"].Precision = 3;
            cmd.Parameters["POGNO"].Scale = 0;
            cmd.Parameters["POGNO"].Value = 0;

            if (m_da400.ExecuteSubRoutine("CSSR31", ref cmd, strBizInit, strBranchNo))
            {
                //Delphi
                //G_Net_Income   := Call_CSSR31.Value[8];
                //G_Orank        := Call_CSSR31.Value[9];
                //G_Otimes       := Call_CSSR31.Value[10];
                //G_ACL          := Call_CSSR31.Value[11];
                //G_Arank        := Call_CSSR31.Value[12];
                //G_Atimes       := Call_CSSR31.Value[13];
                //G_AACL         := Call_CSSR31.Value[14];
                //G_Rrank        := Call_CSSR31.Value[15];
                //G_Rtimes       := Call_CSSR31.Value[16];
                //G_TCL          := Call_CSSR31.Value[17];
                //G_Final_TCL    := Call_CSSR31.Value[18];
                //G_CSP          := Call_CSSR31.Value[19];
                //G_Total_CSP    := Call_CSSR31.Value[20];
                //G_Up_Down_Flag := Call_CSSR31.Value[21];
                //G_Have_TCL     := Call_CSSR31.Value[22];
                //G_GRACE_Period := Call_CSSR31.Value[23];
                //G_Model        := Call_CSSR31.Value[24];
                //G_PD1          := Call_CSSR31.Value[25];
                //G_GNO          := Call_CSSR31.Value[26];

                POSALN = cmd.Parameters["POSALN"].Value.ToString().Trim();
                POORNK = cmd.Parameters["POORNK"].Value.ToString().Trim();
                POOTMS = cmd.Parameters["POOTMS"].Value.ToString().Trim();
                POOACL = cmd.Parameters["POOACL"].Value.ToString().Trim();
                POARNK = cmd.Parameters["POARNK"].Value.ToString().Trim();
                POATMS = cmd.Parameters["POATMS"].Value.ToString().Trim();
                POAACL = cmd.Parameters["POAACL"].Value.ToString().Trim();
                PORRNK = cmd.Parameters["PORRNK"].Value.ToString().Trim();
                PORTMS = cmd.Parameters["PORTMS"].Value.ToString().Trim();
                PORACL = cmd.Parameters["PORACL"].Value.ToString().Trim();
                POFTCL = cmd.Parameters["POFTCL"].Value.ToString().Trim();
                POSTRP = cmd.Parameters["POSTRP"].Value.ToString().Trim();
                POTOTP = cmd.Parameters["POTOTP"].Value.ToString().Trim();
                POAFLG = cmd.Parameters["POAFLG"].Value.ToString().Trim();
                POHAVE = cmd.Parameters["POHAVE"].Value.ToString().Trim();
                POODGC = cmd.Parameters["POODGC"].Value.ToString().Trim();
                POMDL = cmd.Parameters["POMDL"].Value.ToString().Trim();
                POPD = cmd.Parameters["POPD"].Value.ToString().Trim();
                POGNO = cmd.Parameters["POGNO"].Value.ToString().Trim();

                //-- เพิ่มตอน Request 75972 เนื่องจาก Version source หายตั้งแต่ 08/05/2018
                DataSet DS_RLTB10 = get_RLTB10();
                if (DS_RLTB10 != null)
                {
                    if (DS_RLTB10.Tables[0].Rows.Count > 0)
                    {
                        if (Convert.ToDecimal(prmPISAL) < Convert.ToDecimal(DS_RLTB10.Tables[0].Rows[0]["T10CD2"]))
                        {
                            #region "check  Freeze TCL"
                            DataSet ds = new DataSet();
                            try
                            {
                                string sql = " SELECT M57NTCL FROM csms57l1 WHERE m57csn = " + prmPICSN + " AND m57lkb = 'BOT' ";
                                ds = RetriveAsDataSet(sql);
                                if (ds != null)
                                {
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        decimal FreezeBOT = Convert.ToDecimal(ds.Tables[0].Rows[0]["M57NTCL"].ToString());
                                        if (POFTCL.Trim() == "")
                                        {
                                            POFTCL = "0";
                                        }
                                        decimal FinalTCL = Convert.ToDecimal(POFTCL);
                                        if (FinalTCL > FreezeBOT)
                                        {
                                            POFTCL = FreezeBOT.ToString();
                                            if (POMDL == "3")
                                            {
                                                POMDL = "F";
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                CloseConnectioDAL();
                            }
                            #endregion
                        }
                    }
                }
                //--
                m_da400.CloseConnect();
                return true;
            }
            else
            {
                m_da400.CloseConnect();
                return false;
            }
        }

        public bool CALL_CSSR36(string prmCSN, ref string STS, ref string MSG, ref string FLG,
                                string strBizInit, string strBranchNo)
        {
            iDB2Command cmd = new iDB2Command();

            m_da400.AS400Lib = EB_Service.Commons.AppConfig.GNPGMLib;  //GeneralLib;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "CSSR36";

            cmd.Parameters.Add("W0CSNO", iDB2DbType.iDB2Char, 8).Value = prmCSN;

            //***   out put ***//
            cmd.Parameters.Add("W0PSTS", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;
            cmd.Parameters["W0PSTS"].Value = "";
            cmd.Parameters.Add("W0MSGL", iDB2DbType.iDB2Char, 50).Direction = ParameterDirection.Output;
            cmd.Parameters["W0MSGL"].Value = "";
            cmd.Parameters.Add("W0NFLG", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;
            cmd.Parameters["W0NFLG"].Value = "";

            if (m_da400.ExecuteSubRoutine("CSSR36", ref cmd, strBizInit, strBranchNo))
            {
                STS = cmd.Parameters["W0PSTS"].Value.ToString().Trim();
                MSG = cmd.Parameters["W0MSGL"].Value.ToString().Trim();
                FLG = cmd.Parameters["W0NFLG"].Value.ToString().Trim();
                m_da400.CloseConnect();
                return true;
            }
            else
            {
                m_da400.CloseConnect();
                return false;
            }
        }

        public bool CALL_GNP014(string IDATE, string ITYPDATE, string IBORC, ref string OERRCHK, ref string ONAMED,
                               string strBizInit, string strBranchNo)
        {
            iDB2Command cmd = new iDB2Command();

            m_da400.AS400Lib = EB_Service.Commons.AppConfig.GNPGMLib;  //GeneralLib;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "GNP014";

            cmd.Parameters.Add("IDATE", iDB2DbType.iDB2Char, 6).Value = IDATE;
            cmd.Parameters.Add("ITYPDATE", iDB2DbType.iDB2Char, 3).Value = ITYPDATE;
            cmd.Parameters.Add("IBORC", iDB2DbType.iDB2Char, 1).Value = IBORC;


            //***   out put ***//
            cmd.Parameters.Add("ONAMED", iDB2DbType.iDB2Char, 3).Direction = ParameterDirection.Output;
            cmd.Parameters["ONAMED"].Value = "";
            cmd.Parameters.Add("OERRCHK", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;
            cmd.Parameters["OERRCHK"].Value = "";


            if (m_da400.ExecuteSubRoutine("GNP014", ref cmd, strBizInit, strBranchNo))
            {
                ONAMED = cmd.Parameters["ONAMED"].Value.ToString().Trim();
                OERRCHK = cmd.Parameters["OERRCHK"].Value.ToString().Trim();
                m_da400.CloseConnect();
                return true;
            }
            else
            {
                m_da400.CloseConnect();
                return false;
            }
        }


        public bool CALL_GNP0371(string prmDATE1, string prmDATE2, string prmFMT, string prmBOC, string prmAPT, string prmBUS, string prmCST,
                                     string strBizInit, string strBranchNo, ref string AGE, ref string Error)
        {
            iDB2Command cmd = new iDB2Command();

            m_da400.AS400Lib = EB_Service.Commons.AppConfig.GNPGMLib;  //GeneralLib;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "GNP0371";

            cmd.Parameters.Add("WPIDT", iDB2DbType.iDB2Char, 8).Value = prmDATE1;
            cmd.Parameters.Add("WPIDT2", iDB2DbType.iDB2Char, 8).Value = prmDATE2;
            cmd.Parameters.Add("WPFMT", iDB2DbType.iDB2Char, 3).Value = prmFMT;
            cmd.Parameters.Add("WPBOC", iDB2DbType.iDB2Char, 1).Value = prmBOC;
            cmd.Parameters.Add("WPAPT", iDB2DbType.iDB2Char, 2).Value = prmAPT;
            cmd.Parameters.Add("WPBUS", iDB2DbType.iDB2Char, 2).Value = prmBUS;
            cmd.Parameters.Add("WPCST", iDB2DbType.iDB2Char, 1).Value = prmCST;

            //***   out put ***//
            cmd.Parameters.Add("W#AGE", iDB2DbType.iDB2Char, 5).Direction = ParameterDirection.Output;
            cmd.Parameters["W#AGE"].Value = "";
            cmd.Parameters.Add("WPERR", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;
            cmd.Parameters["WPERR"].Value = "";

            if (m_da400.ExecuteSubRoutine("GNP0371", ref cmd, strBizInit, strBranchNo))
            {
                AGE = cmd.Parameters["W#AGE"].Value.ToString().Trim();
                Error = cmd.Parameters["WPERR"].Value.ToString().Trim();
                //m_da400.CloseConnect();
                return true;
            }
            else
            {
                //m_da400.CloseConnect();
                return false;
            }
        }

        public bool CALL_GNSRGNOC(string prmBUS, string prmBRN, string prmAPPNO, string prmARRAY,
                                  ref string prmOCSNO, ref string OPD, ref string OGNO, ref string ORANK, ref string OINC, ref string OACL,
                                  ref string O2GNO, ref string O2RANK, ref string O2ACL, ref string O2RNK, ref string O21ACL, ref string OTYPE,
                                  string strBizInit, string strBranchNo)
        {
            iDB2Command cmd = new iDB2Command();

            m_da400.AS400Lib = EB_Service.Commons.AppConfig.GNPGMLib;  //GeneralLib;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "GNSRGNOC";

            cmd.Parameters.Add("P#BUS", iDB2DbType.iDB2Char, 2).Value = prmBUS;
            cmd.Parameters.Add("P#BRN", iDB2DbType.iDB2Decimal, 0).Value = prmBRN;
            cmd.Parameters["P#BRN"].Precision = 3;
            cmd.Parameters["P#BRN"].Scale = 0;
            cmd.Parameters.Add("P#APN", iDB2DbType.iDB2Decimal, 0).Value = prmAPPNO;
            cmd.Parameters["P#APN"].Precision = 11;
            cmd.Parameters["P#APN"].Scale = 0;
            cmd.Parameters.Add("I@PARA1", iDB2DbType.iDB2Char, 100).Value = prmARRAY;



            //***   out put ***//
            cmd.Parameters.Add("O@CSNO", iDB2DbType.iDB2Char, 8).Direction = ParameterDirection.Output;
            cmd.Parameters["O@CSNO"].Value = "";

            cmd.Parameters.Add("O@PD", iDB2DbType.iDB2Decimal, 0).Direction = ParameterDirection.Output;
            cmd.Parameters["O@PD"].Precision = 10;
            cmd.Parameters["O@PD"].Scale = 9;
            cmd.Parameters["O@PD"].Value = 0;

            cmd.Parameters.Add("O@GNO", iDB2DbType.iDB2Decimal, 0).Direction = ParameterDirection.Output;
            cmd.Parameters["O@GNO"].Precision = 3;
            cmd.Parameters["O@GNO"].Scale = 0;
            cmd.Parameters["O@GNO"].Value = 0;

            cmd.Parameters.Add("O@RANK", iDB2DbType.iDB2Decimal, 0).Direction = ParameterDirection.Output;
            cmd.Parameters["O@RANK"].Precision = 2;
            cmd.Parameters["O@RANK"].Scale = 0;
            cmd.Parameters["O@RANK"].Value = 0;

            cmd.Parameters.Add("O@INC", iDB2DbType.iDB2Decimal, 0).Direction = ParameterDirection.Output;
            cmd.Parameters["O@INC"].Precision = 13;
            cmd.Parameters["O@INC"].Scale = 2;
            cmd.Parameters["O@INC"].Value = 0;

            cmd.Parameters.Add("O@ACL", iDB2DbType.iDB2Decimal, 0).Direction = ParameterDirection.Output;
            cmd.Parameters["O@ACL"].Precision = 13;
            cmd.Parameters["O@ACL"].Scale = 2;
            cmd.Parameters["O@ACL"].Value = 0;

            cmd.Parameters.Add("O2@GNO", iDB2DbType.iDB2Decimal, 0).Direction = ParameterDirection.Output;
            cmd.Parameters["O2@GNO"].Precision = 3;
            cmd.Parameters["O2@GNO"].Scale = 0;
            cmd.Parameters["O2@GNO"].Value = 0;

            cmd.Parameters.Add("O2@RANK", iDB2DbType.iDB2Decimal, 0).Direction = ParameterDirection.Output;
            cmd.Parameters["O2@RANK"].Precision = 2;
            cmd.Parameters["O2@RANK"].Scale = 0;
            cmd.Parameters["O2@RANK"].Value = 0;

            cmd.Parameters.Add("O2@#ACL", iDB2DbType.iDB2Decimal, 0).Direction = ParameterDirection.Output;
            cmd.Parameters["O2@#ACL"].Precision = 8;
            cmd.Parameters["O2@#ACL"].Scale = 0;
            cmd.Parameters["O2@#ACL"].Value = 0;

            cmd.Parameters.Add("O2@#RNK", iDB2DbType.iDB2Decimal, 0).Direction = ParameterDirection.Output;
            cmd.Parameters["O2@#RNK"].Precision = 2;
            cmd.Parameters["O2@#RNK"].Scale = 0;
            cmd.Parameters["O2@#RNK"].Value = 0;

            cmd.Parameters.Add("O2@#ACL", iDB2DbType.iDB2Decimal, 0).Direction = ParameterDirection.Output;
            cmd.Parameters["O2@#ACL"].Precision = 8;
            cmd.Parameters["O2@#ACL"].Scale = 0;
            cmd.Parameters["O2@#ACL"].Value = 0;

            cmd.Parameters.Add("O@TYPE", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;
            cmd.Parameters["O@TYPE"].Value = "";

            if (m_da400.ExecuteSubRoutine("GNSRGNOC", ref cmd, strBizInit, strBranchNo))
            {
                prmOCSNO = cmd.Parameters["O@CSNO"].Value.ToString().Trim();
                OPD = cmd.Parameters["O@PD"].Value.ToString().Trim();
                OGNO = cmd.Parameters["O@GNO"].Value.ToString().Trim();
                ORANK = cmd.Parameters["O@RANK"].Value.ToString().Trim();
                OINC = cmd.Parameters["O@INC"].Value.ToString().Trim();
                OACL = cmd.Parameters["O@ACL"].Value.ToString().Trim();
                O2GNO = cmd.Parameters["O2@GNO"].Value.ToString().Trim();
                O2RANK = cmd.Parameters["O2@RANK"].Value.ToString().Trim();
                O2ACL = cmd.Parameters["O2@#ACL"].Value.ToString().Trim();
                O2RNK = cmd.Parameters["O2@#RNK"].Value.ToString().Trim();
                O21ACL = cmd.Parameters["O2@#ACL"].Value.ToString().Trim();
                OTYPE = cmd.Parameters["O@TYPE"].Value.ToString().Trim();
                m_da400.CloseConnect();
                return true;
            }
            else
            {
                m_da400.CloseConnect();
                return false;
            }
        }

        //public DataSet CALL_CSE0221A(string prmIDNO, string prmAPPNO, string strBizInit, string strBranchNo)
        //{
        //    iDB2Command cmd = new iDB2Command();
        //    m_da400.AS400Lib = EB_Service.Commons.AppConfig.GNPGMLib;
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "CSE0221A";

        //    cmd.Parameters.Add("IDNO", iDB2DbType.iDB2Char, 15).Value = prmIDNO.ToString().Trim();
        //    cmd.Parameters.Add("PIAPNO", iDB2DbType.iDB2Char, 11).Value = prmAPPNO.ToString().Trim();
        //    //cmd.Parameters.Add("APPNO", iDB2DbType.iDB2Decimal, 0).Value = prmAPPNO.ToString().Trim();
        //    //cmd.Parameters["APPNO"].Precision = 11;
        //    //cmd.Parameters["APPNO"].Scale = 0;

        //    m_da400.AS400Lib = EB_Service.Commons.AppConfig.GNPGMLib;    //GeneralLib;

        //    DataSet dsResult = new DataSet();
        //    m_da400.ConnectAs400();
        //    try
        //    {
        //        dsResult = m_da400.ExecSubRoutine_DS(ref cmd, ref m_LastError, strBizInit, strBranchNo);
        //    }
        //    catch (Exception ex)
        //    { m_LastError = ex.Message; }
        //    m_da400.CloseConnect();
        //    return dsResult;
        //}

        public DataSet CALL_ILE0221(string prmIDNO, string strBizInit, string strBranchNo)
        {
            iDB2Command cmd = new iDB2Command();
            m_da400.AS400Lib = EB_Service.Commons.AppConfig.GNPGMLib;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ILE0221";

            cmd.Parameters.Add("IDNO", iDB2DbType.iDB2Char, 15).Value = prmIDNO.ToString().Trim();
            //cmd.Parameters.Add("PIAPNO", iDB2DbType.iDB2Char, 11).Value = prmAPPNO.ToString().Trim();
            //cmd.Parameters.Add("APPNO", iDB2DbType.iDB2Decimal, 0).Value = prmAPPNO.ToString().Trim();
            //cmd.Parameters["APPNO"].Precision = 11;
            //cmd.Parameters["APPNO"].Scale = 0;

            m_da400.AS400Lib = EB_Service.Commons.AppConfig.GNPGMLib;    //GeneralLib;

            DataSet dsResult = new DataSet();
            m_da400.ConnectAs400();
            try
            {
                dsResult = m_da400.ExecSubRoutine_DS(ref cmd, ref m_LastError, strBizInit, strBranchNo);
            }
            catch (Exception ex)
            { m_LastError = ex.Message; }
            m_da400.CloseConnect();
            return dsResult;
        }

        //public DataSet CALL_CSE0221B(string prmGROUP, string prmTEL, string prmBUS,
        //    string strBizInit, string strBranchNo)
        //{
        //    iDB2Command cmd = new iDB2Command();
        //    m_da400.AS400Lib = EB_Service.Commons.AppConfig.GNPGMLib;
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "CSE0221B";

        //    cmd.Parameters.Add("PIGROP", iDB2DbType.iDB2Char, 5).Value = prmGROUP.ToString().Trim();
        //    cmd.Parameters.Add("PITELN", iDB2DbType.iDB2Char, 50).Value = prmTEL.ToString().Trim();
        //    cmd.Parameters.Add("PIBUS", iDB2DbType.iDB2Char, 2).Value = prmBUS.ToString().Trim();

        //    m_da400.AS400Lib = EB_Service.Commons.AppConfig.GNPGMLib;    //GeneralLib;

        //    DataSet dsResult = new DataSet();
        //    m_da400.ConnectAs400();
        //    try
        //    {
        //        dsResult = m_da400.ExecSubRoutine_DS(ref cmd, ref m_LastError, strBizInit, strBranchNo);
        //    }
        //    catch (Exception ex)
        //    { m_LastError = ex.Message; }
        //    m_da400.CloseConnect();
        //    return dsResult;
        //}

        public DataSet CALL_ILE02211(string prmGROUP, string prmTEL, string prmBUS,
            string strBizInit, string strBranchNo)
        {
            iDB2Command cmd = new iDB2Command();
            m_da400.AS400Lib = EB_Service.Commons.AppConfig.GNPGMLib;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ILE02211";

            cmd.Parameters.Add("PIGROP", iDB2DbType.iDB2Char, 5).Value = prmGROUP.ToString().Trim();
            cmd.Parameters.Add("PITELN", iDB2DbType.iDB2Char, 50).Value = prmTEL.ToString().Trim();
            cmd.Parameters.Add("PIBUS", iDB2DbType.iDB2Char, 2).Value = prmBUS.ToString().Trim();

            m_da400.AS400Lib = EB_Service.Commons.AppConfig.GNPGMLib;    //GeneralLib;

            DataSet dsResult = new DataSet();
            m_da400.ConnectAs400();
            try
            {
                dsResult = m_da400.ExecSubRoutine_DS(ref cmd, ref m_LastError, strBizInit, strBranchNo);
            }
            catch (Exception ex)
            { m_LastError = ex.Message; }
            m_da400.CloseConnect();
            return dsResult;
        }

        //public DataSet CALL_CSE0222A(string prmIDNO, string prmBRN, string prmAPPNO, string strBizInit, string strBranchNo)
        //{
        //    iDB2Command cmd = new iDB2Command();
        //    m_da400.AS400Lib = EB_Service.Commons.AppConfig.GNPGMLib;
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "CSE0222A";

        //    cmd.Parameters.Add("PIIDNO", iDB2DbType.iDB2Char, 15).Value = prmIDNO.ToString().Trim();
        //    cmd.Parameters.Add("PIBRN", iDB2DbType.iDB2Char, 3).Value = prmBRN.ToString().Trim();
        //    cmd.Parameters.Add("PIAPNO", iDB2DbType.iDB2Char, 11).Value = prmAPPNO.ToString().Trim();

        //    m_da400.AS400Lib = EB_Service.Commons.AppConfig.GNPGMLib;    //GeneralLib;

        //    DataSet dsResult = new DataSet();
        //    m_da400.ConnectAs400();
        //    try
        //    {
        //        dsResult = m_da400.ExecSubRoutine_DS(ref cmd, ref m_LastError, strBizInit, strBranchNo);
        //    }
        //    catch (Exception ex)
        //    { m_LastError = ex.Message; }
        //    m_da400.CloseConnect();
        //    return dsResult;
        //}
        public DataSet CALL_ILE0222(string prmIDNO, string strBizInit, string strBranchNo)
        {
            iDB2Command cmd = new iDB2Command();
            m_da400.AS400Lib = EB_Service.Commons.AppConfig.GNPGMLib;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ILE0222";

            cmd.Parameters.Add("PIIDNO", iDB2DbType.iDB2Char, 15).Value = prmIDNO.ToString().Trim();
            //cmd.Parameters.Add("PIBRN", iDB2DbType.iDB2Char, 3).Value = prmBRN.ToString().Trim();
            //cmd.Parameters.Add("PIAPNO", iDB2DbType.iDB2Char, 11).Value = prmAPPNO.ToString().Trim();

            m_da400.AS400Lib = EB_Service.Commons.AppConfig.GNPGMLib;    //GeneralLib;

            DataSet dsResult = new DataSet();
            m_da400.ConnectAs400();
            try
            {
                dsResult = m_da400.ExecSubRoutine_DS(ref cmd, ref m_LastError, strBizInit, strBranchNo);
            }
            catch (Exception ex)
            { m_LastError = ex.Message; }
            m_da400.CloseConnect();
            return dsResult;
        }

        //public DataSet CALL_CSE0222B(string prmIDNO, string prmBRN, string prmAPPNO, string prmGROUP, string prmTEL, string prmBUS, 
        //    string strBizInit, string strBranchNo)
        //{
        //    iDB2Command cmd = new iDB2Command();
        //    m_da400.AS400Lib = EB_Service.Commons.AppConfig.GNPGMLib;
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "CSE0222B";

        //    cmd.Parameters.Add("PIIDNO", iDB2DbType.iDB2Char, 15).Value = prmIDNO.ToString().Trim();
        //    cmd.Parameters.Add("PIBRN", iDB2DbType.iDB2Char, 3).Value = prmBRN.ToString().Trim();
        //    cmd.Parameters.Add("PIAPNO", iDB2DbType.iDB2Char, 11).Value = prmAPPNO.ToString().Trim();
        //    cmd.Parameters.Add("PIGROP", iDB2DbType.iDB2Char, 5).Value = prmGROUP.ToString().Trim();
        //    cmd.Parameters.Add("PITELN", iDB2DbType.iDB2Char, 20).Value = prmTEL.ToString().Trim();
        //    cmd.Parameters.Add("PIBUS", iDB2DbType.iDB2Char, 2).Value = prmBUS.ToString().Trim();

        //    m_da400.AS400Lib = EB_Service.Commons.AppConfig.GNPGMLib;    //GeneralLib;

        //    DataSet dsResult = new DataSet();
        //    m_da400.ConnectAs400();
        //    try
        //    {
        //        dsResult = m_da400.ExecSubRoutine_DS(ref cmd, ref m_LastError, strBizInit, strBranchNo);
        //    }
        //    catch (Exception ex)
        //    { m_LastError = ex.Message; }
        //    m_da400.CloseConnect();
        //    return dsResult;
        //}
        public DataSet CALL_ILE02222(string prmIDNO, string prmGROUP, string prmTEL, string prmBUS,
            string strBizInit, string strBranchNo)
        {
            iDB2Command cmd = new iDB2Command();
            m_da400.AS400Lib = EB_Service.Commons.AppConfig.GNPGMLib;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ILE02222";

            cmd.Parameters.Add("PIIDNO", iDB2DbType.iDB2Char, 15).Value = prmIDNO.ToString().Trim();
            //cmd.Parameters.Add("PIBRN", iDB2DbType.iDB2Char, 3).Value = prmBRN.ToString().Trim();
            //cmd.Parameters.Add("PIAPNO", iDB2DbType.iDB2Char, 11).Value = prmAPPNO.ToString().Trim();
            cmd.Parameters.Add("PIGROP", iDB2DbType.iDB2Char, 5).Value = prmGROUP.ToString().Trim();
            cmd.Parameters.Add("PITELN", iDB2DbType.iDB2Char, 20).Value = prmTEL.ToString().Trim();
            cmd.Parameters.Add("PIBUS", iDB2DbType.iDB2Char, 2).Value = prmBUS.ToString().Trim();

            m_da400.AS400Lib = EB_Service.Commons.AppConfig.GNPGMLib;    //GeneralLib;

            DataSet dsResult = new DataSet();
            m_da400.ConnectAs400();
            try
            {
                dsResult = m_da400.ExecSubRoutine_DS(ref cmd, ref m_LastError, strBizInit, strBranchNo);
            }
            catch (Exception ex)
            { m_LastError = ex.Message; }
            m_da400.CloseConnect();
            return dsResult;
        }

        public bool CALL_CSSRW11(string prmBiz, string prmIDNO, string prmAct, string prmRes, string prmNote1, string prmNote2, ref string prmErrMsg,
            string strBizInit, string strBranchNo)
        {
            m_LastError = "";
            iDB2Command cmd = new iDB2Command();

            cmd.CommandType = CommandType.StoredProcedure;
            //cmd.CommandText = m_CSPGM + ".CSSRW11";
            cmd.CommandText = "CSSRW11";
            // Parameter In
            cmd.Parameters.Add("PIMODE", iDB2DbType.iDB2Char, 1).Value = "C";
            cmd.Parameters.Add("PIBUSS", iDB2DbType.iDB2Char, 2).Value = prmBiz;
            cmd.Parameters.Add("PICSID", iDB2DbType.iDB2Char, 15).Value = prmIDNO;
            cmd.Parameters.Add("PIBRAN", iDB2DbType.iDB2Decimal, 0).Value = '0';
            cmd.Parameters.Add("PICONT", iDB2DbType.iDB2Decimal, 0).Value = '0';
            cmd.Parameters.Add("PIMSG1", iDB2DbType.iDB2Char, 100).Value = prmNote1;
            cmd.Parameters.Add("PIMSG2", iDB2DbType.iDB2Char, 100).Value = prmNote2;
            cmd.Parameters.Add("PIACCD", iDB2DbType.iDB2Char, 6).Value = prmAct;
            cmd.Parameters.Add("PIRSCD", iDB2DbType.iDB2Char, 6).Value = prmRes;
            cmd.Parameters.Add("PIUSER", iDB2DbType.iDB2Char, 10).Value = m_User;
            cmd.Parameters.Add("PITIME", iDB2DbType.iDB2Decimal, 0).Value = m_UpdTime;
            cmd.Parameters.Add("POERRC", iDB2DbType.iDB2Char, 3).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("POEMSG", iDB2DbType.iDB2Char, 80).Direction = ParameterDirection.Output;


            //if (m_da400.ExecuteSubRoutine(m_CSPGM + ".CSSRW11", ref cmd))
            if (m_da400.ExecuteSubRoutine("CSSRW11", ref cmd, strBizInit, strBranchNo))
            {
                prmErrMsg = cmd.Parameters["POEMSG"].Value.ToString().Trim();
                return true;
            }
            else
                return false;
        }

        public bool CALL_CSSRW11_NOCOMMIT(string PIMODE, string prmBiz, string prmIDNO, string prmAct, string prmRes, string prmNote1, string prmNote2, ref string prmErrMsg,
            string strBizInit, string strBranchNo)
        {
            m_LastError = "";
            iDB2Command cmd = new iDB2Command();

            cmd.CommandType = CommandType.StoredProcedure;
            //cmd.CommandText = m_CSPGM + ".CSSRW11";
            cmd.CommandText = "CSSRW11";
            // Parameter In
            cmd.Parameters.Add("PIMODE", iDB2DbType.iDB2Char, 1).Value = PIMODE;
            cmd.Parameters.Add("PIBUSS", iDB2DbType.iDB2Char, 2).Value = prmBiz;
            cmd.Parameters.Add("PICSID", iDB2DbType.iDB2Char, 15).Value = prmIDNO;
            cmd.Parameters.Add("PIBRAN", iDB2DbType.iDB2Decimal, 0).Value = '0';
            cmd.Parameters.Add("PICONT", iDB2DbType.iDB2Decimal, 0).Value = '0';
            cmd.Parameters.Add("PIMSG1", iDB2DbType.iDB2Char, 100).Value = prmNote1;
            cmd.Parameters.Add("PIMSG2", iDB2DbType.iDB2Char, 100).Value = prmNote2;
            cmd.Parameters.Add("PIACCD", iDB2DbType.iDB2Char, 6).Value = prmAct;
            cmd.Parameters.Add("PIRSCD", iDB2DbType.iDB2Char, 6).Value = prmRes;
            cmd.Parameters.Add("PIUSER", iDB2DbType.iDB2Char, 10).Value = m_User;
            cmd.Parameters.Add("PITIME", iDB2DbType.iDB2Decimal, 0).Value = m_UpdTime;
            cmd.Parameters.Add("POERRC", iDB2DbType.iDB2Char, 3).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("POEMSG", iDB2DbType.iDB2Char, 80).Direction = ParameterDirection.Output;


            //if (m_da400.ExecuteSubRoutine(m_CSPGM + ".CSSRW11", ref cmd))
            if (m_da400.ExecuteSubRoutine("CSSRW11", ref cmd, strBizInit, strBranchNo))
            {
                prmErrMsg = cmd.Parameters["POEMSG"].Value.ToString().Trim();
                return true;
            }
            else
                return false;
        }


        public bool Call_GNSRCID(string prmID, ref string prmError,
        string strBizInit, string strBranchNo)
        {
            iDB2Command cmd = new iDB2Command();

            m_da400.AS400Lib = EB_Service.Commons.AppConfig.GNPGMLib;  //GeneralLib;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "GNSRCID";

            cmd.Parameters.Add("WIIDNO", iDB2DbType.iDB2Char, 15).Value = prmID;
            cmd.Parameters.Add("ERROR", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;

            if (m_da400.ExecuteSubRoutine("GNSRCID", ref cmd, strBizInit, strBranchNo))
            {
                prmError = cmd.Parameters["ERROR"].Value.ToString();
                m_da400.CloseConnect();
                return true;
            }
            else
            {
                m_da400.CloseConnect();
                return false;
            }
        }

        public bool Call_GNP0221(string prmDate, ref string prmError,
        string strBizInit, string strBranchNo)
        {
            iDB2Command cmd = new iDB2Command();

            m_da400.AS400Lib = EB_Service.Commons.AppConfig.GNPGMLib;  //GeneralLib;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "GNP0221";

            cmd.Parameters.Add("WPIDT", iDB2DbType.iDB2Char, 8).Value = prmDate;
            cmd.Parameters.Add("WPFMT", iDB2DbType.iDB2Char, 3).Value = "DMY";
            cmd.Parameters.Add("WPBOC", iDB2DbType.iDB2Char, 1).Value = "B";
            cmd.Parameters.Add("WPERR", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;

            if (m_da400.ExecuteSubRoutine("GNP0221", ref cmd, strBizInit, strBranchNo))
            {
                prmError = cmd.Parameters["WPERR"].Value.ToString();
                m_da400.CloseConnect();
                return true;
            }
            else
            {
                m_da400.CloseConnect();
                return false;
            }
        }

        public bool Call_GNP023(string prm1, string prm2, string prm3, string prm4, string prm5, string prm6,
                                ref string prmCalcDate, ref string prmError,
        string strBizInit, string strBranchNo)
        {
            iDB2Command cmd = new iDB2Command();

            m_da400.AS400Lib = EB_Service.Commons.AppConfig.GNPGMLib;  //GeneralLib;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "GNP023";

            cmd.Parameters.Add("WPIDT", iDB2DbType.iDB2Char, 8).Value = prm1;
            cmd.Parameters.Add("WPFMT", iDB2DbType.iDB2Char, 3).Value = prm2;
            cmd.Parameters.Add("WPBOC", iDB2DbType.iDB2Char, 1).Value = prm3;
            cmd.Parameters.Add("WPPS#", iDB2DbType.iDB2Decimal, 0).Value = prm4;
            cmd.Parameters["WPPS#"].Precision = 5;
            cmd.Parameters["WPPS#"].Scale = 0;

            cmd.Parameters.Add("WPPST", iDB2DbType.iDB2Char, 1).Value = prm5;
            cmd.Parameters.Add("WPPOM", iDB2DbType.iDB2Char, 1).Value = prm6;

            cmd.Parameters.Add("WPODT", iDB2DbType.iDB2Char, 8).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("WPERR", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;

            if (m_da400.ExecuteSubRoutine("GNP023", ref cmd, strBizInit, strBranchNo))
            {
                prmError = cmd.Parameters["WPERR"].Value.ToString();
                prmCalcDate = cmd.Parameters["WPODT"].Value.ToString();
                m_da400.CloseConnect();
                return true;
            }
            else
            {
                m_da400.CloseConnect();
                return false;
            }
        }

        public bool Call_ILP036(string prmExpireDate, string prmBirthDate, string prmAppDate, string prmformatdmy, string prmyeartype, ref string prmError,
        string strBizInit, string strBranchNo)
        {
            iDB2Command cmd = new iDB2Command();

            m_da400.AS400Lib = EB_Service.Commons.AppConfig.GNPGMLib;  //GeneralLib;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ILP036";

            cmd.Parameters.Add("WPID1", iDB2DbType.iDB2Char, 8).Value = prmExpireDate;
            cmd.Parameters.Add("WPID2", iDB2DbType.iDB2Char, 8).Value = prmBirthDate;
            cmd.Parameters.Add("WPID3", iDB2DbType.iDB2Char, 8).Value = prmAppDate;
            cmd.Parameters.Add("WPFMT", iDB2DbType.iDB2Char, 3).Value = prmformatdmy;
            cmd.Parameters.Add("WPBOC", iDB2DbType.iDB2Char, 1).Value = prmyeartype;
            cmd.Parameters.Add("WPERR", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;

            if (m_da400.ExecuteSubRoutine("ILP036", ref cmd, strBizInit, strBranchNo))
            {
                prmError = cmd.Parameters["WPERR"].Value.ToString();
                m_da400.CloseConnect();
                return true;
            }
            else
            {
                m_da400.CloseConnect();
                return false;
            }
        }

        //Gen Application No.
        public bool Call_ILSR01(string prmbranch, string prmloantype, ref string prmAppNo, ref string prmError, ref string prmErrorMsg,
        string strBizInit, string strBranchNo)
        {
            iDB2Command cmd = new iDB2Command();

            m_da400.AS400Lib = EB_Service.Commons.AppConfig.GNPGMLib;  //GeneralLib;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ILSR01";

            cmd.Parameters.Add("WBRN", iDB2DbType.iDB2Char, 3).Value = prmbranch;
            cmd.Parameters.Add("WILNTY", iDB2DbType.iDB2Char, 2).Value = prmloantype;
            cmd.Parameters.Add("WAPPNO", iDB2DbType.iDB2Char, 11).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("WOERRF", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("WOEMSG", iDB2DbType.iDB2Char, 60).Direction = ParameterDirection.Output;

            if (m_da400.ExecuteSubRoutine("ILSR01", ref cmd, strBizInit, strBranchNo))
            {
                prmAppNo = cmd.Parameters["WAPPNO"].Value.ToString();
                prmError = cmd.Parameters["WOERRF"].Value.ToString();
                prmErrorMsg = cmd.Parameters["WOEMSG"].Value.ToString();
                //m_da400.CloseConnect();
                return true;
            }
            else
            {
                //m_da400.CloseConnect();
                return false;
            }
        }

        //Gen CSN No.
        public bool Call_CSSR10(string prmID, ref string prmCSN, ref string prmError, ref string prmErrorMsg,
        string strBizInit, string strBranchNo)
        {
            iDB2Command cmd = new iDB2Command();

            m_da400.AS400Lib = EB_Service.Commons.AppConfig.GNPGMLib;  //GeneralLib;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "CSSR10";

            cmd.Parameters.Add("W10IDN", iDB2DbType.iDB2Char, 15).Value = prmID;
            cmd.Parameters.Add("W10CSN", iDB2DbType.iDB2Decimal, 0).Direction = ParameterDirection.Output;
            cmd.Parameters["W10CSN"].Precision = 8;
            cmd.Parameters["W10CSN"].Scale = 0;
            cmd.Parameters.Add("W10FLG", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("W10MSG", iDB2DbType.iDB2Char, 60).Direction = ParameterDirection.Output;

            if (m_da400.ExecuteSubRoutine("CSSR10", ref cmd, strBizInit, strBranchNo))
            {
                prmCSN = cmd.Parameters["W10CSN"].Value.ToString();
                prmError = cmd.Parameters["W10FLG"].Value.ToString();
                prmErrorMsg = cmd.Parameters["W10MSG"].Value.ToString();
                //m_da400.CloseConnect();
                return true;
            }
            else
            {
                m_da400.CloseConnect();
                return false;
            }
        }

        public bool Call_RLSRAUR(string prmID, string prmBirthDate, string prmThainame, string prmThaisurname,
                                 string prmNCB, string prmStatus, string prmOfficeName, string prmOfficeProvince, string prmSalary, string prmMobile, string prmZipcode,
                                 ref string prmWPRJAC, ref string prmWPRJCD, ref string prmWPRJDS, ref string prmError, string prmBusiness,
                                 string strBizInit, string strBranchNo)
        {
            iDB2Command cmd = new iDB2Command();

            m_da400.AS400Lib = EB_Service.Commons.AppConfig.GNPGMLib;  //GeneralLib;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "RLSRAUR";

            cmd.Parameters.Add("WPIDNO", iDB2DbType.iDB2Char, 15).Value = prmID;
            cmd.Parameters.Add("WPBIRT", iDB2DbType.iDB2Char, 8).Value = prmBirthDate;
            cmd.Parameters.Add("WPNAME", iDB2DbType.iDB2Char, 50).Value = prmThainame;
            cmd.Parameters.Add("WPSNAM", iDB2DbType.iDB2Char, 50).Value = prmThaisurname;
            cmd.Parameters.Add("WPNCBF", iDB2DbType.iDB2Char, 1).Value = prmNCB;
            cmd.Parameters.Add("WPHSTS", iDB2DbType.iDB2Char, 1).Value = prmStatus;
            cmd.Parameters.Add("WPOFFN", iDB2DbType.iDB2Char, 50).Value = prmOfficeName;
            cmd.Parameters.Add("WPOPRV", iDB2DbType.iDB2Char, 3).Value = prmOfficeProvince;
            cmd.Parameters.Add("WPSALA", iDB2DbType.iDB2Char, 13).Value = prmSalary;
            cmd.Parameters.Add("WPMOBL", iDB2DbType.iDB2Char, 1).Value = prmMobile;
            cmd.Parameters.Add("WPOZIP", iDB2DbType.iDB2Char, 5).Value = prmZipcode;
            cmd.Parameters.Add("WPRJAC", iDB2DbType.iDB2Char, 6).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("WPRJCD", iDB2DbType.iDB2Char, 6).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("WPRJDS", iDB2DbType.iDB2Char, 100).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("WPERR#", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("WPBUS", iDB2DbType.iDB2Char, 2).Value = prmBusiness;

            if (m_da400.ExecuteSubRoutine("RLSRAUR", ref cmd, strBizInit, strBranchNo))
            {
                prmWPRJAC = cmd.Parameters["WPRJAC"].Value.ToString();
                prmWPRJCD = cmd.Parameters["WPRJCD"].Value.ToString();
                prmWPRJDS = cmd.Parameters["WPRJDS"].Value.ToString();
                prmError = cmd.Parameters["WPERR#"].Value.ToString();
                m_da400.CloseConnect();
                return true;
            }
            else
            {
                m_da400.CloseConnect();
                return false;
            }
        }

        public bool Call_GNSR69(string prmAppcode, string prmBranch, string prmAppno, string prmID, string prmCSN, string prmName, string prmBirthdate, string prmWPPRE,
                                 ref string prmResult, ref string prmResCrReview, ref string prmErrorCode, ref string prmError,
                                 string strBizInit, string strBranchNo)
        {
            iDB2Command cmd = new iDB2Command();

            m_da400.AS400Lib = EB_Service.Commons.AppConfig.GNPGMLib;  //GeneralLib;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "GNSR69";

            cmd.Parameters.Add("WPBUS", iDB2DbType.iDB2Char, 2).Value = prmAppcode;
            cmd.Parameters.Add("WPBRN", iDB2DbType.iDB2Char, 3).Value = prmBranch;
            cmd.Parameters.Add("WPAPNO", iDB2DbType.iDB2Char, 11).Value = prmAppno;
            cmd.Parameters.Add("WPIDNO", iDB2DbType.iDB2Char, 15).Value = prmID;
            cmd.Parameters.Add("WPICSN", iDB2DbType.iDB2Decimal, 0).Value = prmCSN;
            cmd.Parameters["WPICSN"].Precision = 8;
            cmd.Parameters["WPICSN"].Scale = 0;
            cmd.Parameters.Add("WPNAM", iDB2DbType.iDB2Char, 100).Value = prmName;
            cmd.Parameters.Add("WPBDT", iDB2DbType.iDB2Char, 8).Value = prmBirthdate;
            cmd.Parameters.Add("WPPRE", iDB2DbType.iDB2Char, 1).Value = prmWPPRE;

            cmd.Parameters.Add("WPORES", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("WPORCR", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("WPOECDE", iDB2DbType.iDB2Char, 6).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("WPOERR", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;


            if (m_da400.ExecuteSubRoutine("GNSR69", ref cmd, strBizInit, strBranchNo))
            {
                prmResult = cmd.Parameters["WPORES"].Value.ToString();
                prmResCrReview = cmd.Parameters["WPORCR"].Value.ToString();

                m_da400.CloseConnect();
                return true;
            }
            else
            {
                m_da400.CloseConnect();
                return false;
            }
        }

        public bool Call_GNSR48(string prmTel, string prm2, ref string prmError, ref string prmErrorMsg,
        string strBizInit, string strBranchNo, ref string prmOTYPE)
        {
            iDB2Command cmd = new iDB2Command();

            m_da400.AS400Lib = EB_Service.Commons.AppConfig.GNPGMLib;  //GeneralLib;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "GNSR48";

            cmd.Parameters.Add("ITEL#", iDB2DbType.iDB2Char, 12).Value = prmTel;
            cmd.Parameters.Add("IFORM", iDB2DbType.iDB2Char, 1).Value = prm2;
            //cmd.Parameters.Add("OTYPE", iDB2DbType.iDB2Char, 1).Value = "";
            cmd.Parameters.Add("OTYPE", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("OERR", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("OMSG", iDB2DbType.iDB2Char, 40).Direction = ParameterDirection.Output;

            if (m_da400.ExecuteSubRoutine("GNSR48", ref cmd, strBizInit, strBranchNo))
            {
                prmError = cmd.Parameters["OERR"].Value.ToString();
                prmErrorMsg = cmd.Parameters["OMSG"].Value.ToString();
                prmOTYPE = cmd.Parameters["OTYPE"].Value.ToString();
                m_da400.CloseConnect();
                return true;
            }
            else
            {
                m_da400.CloseConnect();
                return false;
            }
        }

        public bool Call_GNSR49(string prmTel, string prmProvince, ref string prmError, string strBizInit, string strBranchNo)
        {
            iDB2Command cmd = new iDB2Command();

            m_da400.AS400Lib = EB_Service.Commons.AppConfig.GNPGMLib;  //GeneralLib;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "GNSR49";

            cmd.Parameters.Add("WPITL", iDB2DbType.iDB2Char, 3).Value = prmTel;
            cmd.Parameters.Add("WPPCD", iDB2DbType.iDB2Char, 3).Value = prmProvince;
            cmd.Parameters.Add("WPERR", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;

            if (m_da400.ExecuteSubRoutine("GNSR49", ref cmd, strBizInit, strBranchNo))
            {
                prmError = cmd.Parameters["WPERR"].Value.ToString();
                m_da400.CloseConnect();
                return true;
            }
            else
            {
                m_da400.CloseConnect();
                return false;
            }
        }

        public bool Call_GNSRNM(string prmName, string prmSurname, string prmLang, ref string prmError, ref string prmErrorMsg,
        string strBizInit, string strBranchNo)
        {
            iDB2Command cmd = new iDB2Command();

            m_da400.AS400Lib = EB_Service.Commons.AppConfig.GNPGMLib;  //GeneralLib;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "GNSRNM";

            cmd.Parameters.Add("NAME", iDB2DbType.iDB2Char, 50).Value = prmName;
            cmd.Parameters.Add("SURNAME", iDB2DbType.iDB2Char, 50).Value = prmSurname;
            cmd.Parameters.Add("LANGUAGE", iDB2DbType.iDB2Char, 1).Value = prmLang;
            cmd.Parameters.Add("ERROR", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("MSG", iDB2DbType.iDB2Char, 50).Direction = ParameterDirection.Output;

            if (m_da400.ExecuteSubRoutine("GNSRNM", ref cmd, strBizInit, strBranchNo))
            {
                prmError = cmd.Parameters["ERROR"].Value.ToString();
                prmErrorMsg = cmd.Parameters["MSG"].Value.ToString();
                m_da400.CloseConnect();
                return true;
            }
            else
            {
                m_da400.CloseConnect();
                return false;
            }
        }



        public DataSet Get_GNCSC07(string prmIDNO, string strBizInit, string strBranchNo) //[56991] เพิ่ม Parameter IP, Parm สำหรับ Initial Lib -Biz, -Branch No
        {
            iDB2Command cmd = new iDB2Command();
            m_da400.AS400Lib = EB_Service.Commons.AppConfig.GNPGMLib;  //GeneralLib;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "GNCSC07"; //[56991] แก้ไขเรือง Fix Lib //m_da400.AS400Lib.Trim()+".GNCSC07";
                                         // INPUT
            cmd.Parameters.Add("WIIDNO", iDB2DbType.iDB2Char, 15).Value = prmIDNO.ToString().Trim();
            // OUTPUT
            cmd.Parameters.Add("WORESL", iDB2DbType.iDB2Char, 2).Direction = ParameterDirection.Output;

            m_da400.AS400Lib = EB_Service.Commons.AppConfig.GNPGMLib;    //GeneralLib;

            DataSet dsResult = new DataSet();
            m_da400.ConnectAs400();
            try
            {
                dsResult = m_da400.ExecSubRoutine_DS(ref cmd, ref m_LastError, strBizInit, strBranchNo); //[56991] เพิ่ม Parameter IP, Parm สำหรับ Initial Lib -Biz, -Branch No

            }
            catch (Exception ex)
            { m_LastError = ex.Message; }
            m_da400.CloseConnect();
            return dsResult;
        }



        public bool getILR75(string PICARD, string strBizInit, string strBranchNo, ref string prmError, ref string prmMsg)
        {
            //*** verify customer for IL USING ***//
            iDB2Command cmd = new iDB2Command();

            m_da400.AS400Lib = EB_Service.Commons.AppConfig.GNPGMLib;  //GeneralLib;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ILSR75";

            // INPUT
            cmd.Parameters.Add("PICARD", iDB2DbType.iDB2Char, 16).Value = PICARD.ToString().Trim();
            cmd.Parameters.Add("PIBRN", iDB2DbType.iDB2Char, 3).Value = strBranchNo.ToString().Trim();

            // OUTPUT
            cmd.Parameters.Add("POERRF", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("POERRM", iDB2DbType.iDB2Char, 100).Direction = ParameterDirection.Output;

            if (m_da400.ExecuteSubRoutine("ILSR75", ref cmd, strBizInit, strBranchNo))
            {
                prmError = cmd.Parameters["POERRF"].Value.ToString().Trim();
                prmMsg = cmd.Parameters["POERRM"].Value.ToString().Trim();
                m_da400.CloseConnect();
                return true;
            }
            else
            {
                m_da400.CloseConnect();
                return false;
            }
        }

        public bool Call_GNSR16(string csn_no, string tel, string ext, ref string prmTelType, ref string prmZip, ref string prmO_TLDS, ref string prmError, string strBizInit, string strBranchNo)
        {

            iDB2Command cmd = new iDB2Command();
            try
            {

                m_da400.AS400Lib = EB_Service.Commons.AppConfig.GNPGMLib;  //GeneralLib;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GNSR16";

                //// INPUT
                cmd.Parameters.Add("I_CSN", iDB2DbType.iDB2Char, 8).Value = csn_no.ToString().Trim();
                cmd.Parameters.Add("H_TEL1", iDB2DbType.iDB2Char, 20).Value = tel.ToString().Trim();
                cmd.Parameters.Add("H_EXT1", iDB2DbType.iDB2Char, 15).Value = ext.ToString().Trim();

                //// OUTPUT
                cmd.Parameters.Add("H_TLTY", iDB2DbType.iDB2Char, 2).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("H_ZIP", iDB2DbType.iDB2Char, 5).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("O_TLDS", iDB2DbType.iDB2Char, 50).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("WKERR", iDB2DbType.iDB2Char, 50).Direction = ParameterDirection.Output;


                if (m_da400.ExecuteSubRoutine("GNSR16", ref cmd, strBizInit, strBranchNo))
                {
                    prmTelType = cmd.Parameters["H_TLTY"].Value.ToString().Trim();
                    prmError = cmd.Parameters["WKERR"].Value.ToString().Trim();
                    prmO_TLDS = cmd.Parameters["O_TLDS"].Value.ToString().Trim();
                    prmZip = cmd.Parameters["H_ZIP"].Value.ToString().Trim();
                    //m_da400.CloseConnect();
                    return true;
                }
                else
                {
                    //m_da400.CloseConnect();
                    return false;
                }

            }
            catch (Exception ex)
            {
                m_da400.CloseConnect();
                return false;
            }
        }

        public bool Call_CSSR07(string prmID_NO, string prmCDTE, ref string salary, ref string date, ref string time, string strBizInit, string strBranchNo)
        {
            try
            {
                iDB2Command cmd = new iDB2Command();
                m_da400.AS400Lib = EB_Service.Commons.AppConfig.GNPGMLib;  //GeneralLib;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "CSSR07";

                //// INPUT
                cmd.Parameters.Add("WIIDNO", iDB2DbType.iDB2Char, 15).Value = prmID_NO.ToString().Trim();
                cmd.Parameters.Add("WICDTE", iDB2DbType.iDB2Decimal, 0).Value = prmCDTE.ToString().Trim();
                cmd.Parameters["WICDTE"].Precision = 8;
                cmd.Parameters["WICDTE"].Scale = 0;

                //// OUTPUT
                cmd.Parameters.Add("WOSDTE", iDB2DbType.iDB2Decimal, 0).Direction = ParameterDirection.Output;
                cmd.Parameters["WOSDTE"].Precision = 8;
                cmd.Parameters["WOSDTE"].Scale = 0;
                cmd.Parameters.Add("WOSTIM", iDB2DbType.iDB2Decimal, 0).Direction = ParameterDirection.Output;
                cmd.Parameters["WOSTIM"].Precision = 6;
                cmd.Parameters["WOSTIM"].Scale = 0;
                cmd.Parameters.Add("WOSALA", iDB2DbType.iDB2Decimal, 0).Direction = ParameterDirection.Output;
                cmd.Parameters["WOSALA"].Precision = 13;
                cmd.Parameters["WOSALA"].Scale = 2;

                if (m_da400.ExecuteSubRoutine("CSSR07", ref cmd, strBizInit, strBranchNo))
                {
                    salary = cmd.Parameters["WOSALA"].Value.ToString().Trim();
                    date = cmd.Parameters["WOSDTE"].Value.ToString().Trim();
                    time = cmd.Parameters["WOSTIM"].Value.ToString().Trim();
                    m_da400.CloseConnect();
                    return true;
                }
                else
                {
                    m_da400.CloseConnect();
                    return false;
                }



            }
            catch (Exception ex)
            {
                m_da400.CloseConnect();
                return false;
            }

        }

        public bool Call_GNSRBL(string prmIID, string prmTNM, string prmTSN, ref string prmPercent, ref string prmError, ref string prmWPOLR, string strBizInit, string strBranchNo)
        {
            DataSet dsResult = new DataSet();
            iDB2Command cmd = new iDB2Command();
            try
            {

                m_da400.AS400Lib = EB_Service.Commons.AppConfig.GNPGMLib;  //GeneralLib;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GNSRBL";

                //// INPUT
                cmd.Parameters.Add("WPIID", iDB2DbType.iDB2Char, 15).Value = prmIID.ToString().Trim();
                cmd.Parameters.Add("WPTNM", iDB2DbType.iDB2Char, 50).Value = prmTNM.ToString().Trim();
                cmd.Parameters.Add("WPTSN", iDB2DbType.iDB2Char, 50).Value = prmTSN.ToString().Trim();

                //// OUTPUT
                cmd.Parameters.Add("WPPER", iDB2DbType.iDB2Char, 3).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("WPERR", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("WPOLR", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;


                if (m_da400.ExecuteSubRoutine("GNSRBL", ref cmd, strBizInit, strBranchNo))
                {
                    prmPercent = cmd.Parameters["WPPER"].Value.ToString().Trim();
                    prmError = cmd.Parameters["WPERR"].Value.ToString().Trim();
                    m_da400.CloseConnect();
                    return true;
                }
                else
                {
                    m_da400.CloseConnect();
                    return false;
                }

            }
            catch (Exception ex)
            {
                m_da400.CloseConnect();
                return false;
            }

        }

        public bool Call_GNSR87(string prmPPBUSS, string prmPPCSNO, string prmPPBRN, string prmPPAPNO, string prmPPAPDT,
                                   string prmPPBHDT, string prmPPCSTY, string prmPPBNGS, string prmPPSALA,
                                   string prmPPTCL, string prmPPPCAM, string prmPPVDID, string prmPPUSOP, string prmPPFOCL,
                                   ref string POBOTL, ref string PONOAP, ref string POCSBL, ref string POCRAV, ref string POAPAM,
                                   ref string POEBCL, ref string POAVAP, ref string POSTS, ref string POOLDC, ref string POVENR,
                                   ref string POPERV, ref string POEBCS, ref string POERR, ref string PERMSG, string strBizInit, string strBranchNo)
        {
            //DataSet dsResult = new DataSet();
            try
            {
                iDB2Command cmd = new iDB2Command();
                m_da400.AS400Lib = EB_Service.Commons.AppConfig.GNPGMLib;  //GeneralLib;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GNSR87";

                //// INPUT
                cmd.Parameters.Add("PPBUSS", iDB2DbType.iDB2Char, 2).Value = prmPPBUSS.Trim(); // 0

                cmd.Parameters.Add("PPCSNO", iDB2DbType.iDB2Decimal, 0).Value = prmPPCSNO.Trim();  // 1
                cmd.Parameters["PPCSNO"].Precision = 8;
                cmd.Parameters["PPCSNO"].Scale = 0;

                cmd.Parameters.Add("PPBRN", iDB2DbType.iDB2Decimal, 0).Value = prmPPBRN.ToString().Trim();  // 2
                cmd.Parameters["PPBRN"].Precision = 3;
                cmd.Parameters["PPBRN"].Scale = 0;

                cmd.Parameters.Add("PPAPNO", iDB2DbType.iDB2Decimal, 0).Value = prmPPAPNO.Trim(); // 3
                cmd.Parameters["PPAPNO"].Precision = 11;
                cmd.Parameters["PPAPNO"].Scale = 0;

                cmd.Parameters.Add("PPAPDT", iDB2DbType.iDB2Decimal, 0).Value = prmPPAPDT.Trim();  // 4
                cmd.Parameters["PPAPDT"].Precision = 8;
                cmd.Parameters["PPAPDT"].Scale = 0;

                cmd.Parameters.Add("PPBHDT", iDB2DbType.iDB2Decimal, 0).Value = prmPPBHDT.Trim();  // 5
                cmd.Parameters["PPBHDT"].Precision = 8;
                cmd.Parameters["PPBHDT"].Scale = 0;

                cmd.Parameters.Add("PPCSTY", iDB2DbType.iDB2Char, 1).Value = prmPPCSTY.Trim();  // 6
                cmd.Parameters.Add("PPBNGS", iDB2DbType.iDB2Char, 1).Value = prmPPBNGS.Trim();  // 7

                cmd.Parameters.Add("PPSALA", iDB2DbType.iDB2Decimal, 0).Value = prmPPSALA.Trim();  // 8
                cmd.Parameters["PPSALA"].Precision = 13;
                cmd.Parameters["PPSALA"].Scale = 2;

                cmd.Parameters.Add("PPTCL", iDB2DbType.iDB2Decimal, 0).Value = prmPPTCL.Trim();  // 9
                cmd.Parameters["PPTCL"].Precision = 13;
                cmd.Parameters["PPTCL"].Scale = 2;

                cmd.Parameters.Add("PPPCAM", iDB2DbType.iDB2Decimal, 0).Value = prmPPPCAM.Trim();  // 10
                cmd.Parameters["PPPCAM"].Precision = 13;
                cmd.Parameters["PPPCAM"].Scale = 2;

                cmd.Parameters.Add("PPVDID#", iDB2DbType.iDB2Char, 12).Value = prmPPVDID.Trim().PadLeft(12, '0');  // 11
                cmd.Parameters.Add("PPUSOP", iDB2DbType.iDB2Char, 1).Value = prmPPUSOP.Trim();   // 12
                cmd.Parameters.Add("PPFOCL", iDB2DbType.iDB2Char, 1).Value = prmPPFOCL.Trim();  // 13


                //// OUTPUT
                cmd.Parameters.Add("POBOTL", iDB2DbType.iDB2Decimal, 0).Direction = ParameterDirection.Output;  // 14
                cmd.Parameters["POBOTL"].Precision = 13;
                cmd.Parameters["POBOTL"].Scale = 2;

                cmd.Parameters.Add("PONOAP", iDB2DbType.iDB2Decimal, 0).Direction = ParameterDirection.Output;  // 15
                cmd.Parameters["PONOAP"].Precision = 2;
                cmd.Parameters["PONOAP"].Scale = 0;

                cmd.Parameters.Add("POCSBL", iDB2DbType.iDB2Decimal, 0).Direction = ParameterDirection.Output;  // 16
                cmd.Parameters["POCSBL"].Precision = 13;
                cmd.Parameters["POCSBL"].Scale = 2;

                cmd.Parameters.Add("POCRAV", iDB2DbType.iDB2Decimal, 0).Direction = ParameterDirection.Output;  // 17
                cmd.Parameters["POCRAV"].Precision = 13;
                cmd.Parameters["POCRAV"].Scale = 2;

                cmd.Parameters.Add("POAPAM", iDB2DbType.iDB2Decimal, 0).Direction = ParameterDirection.Output;  // 18
                cmd.Parameters["POAPAM"].Precision = 13;
                cmd.Parameters["POAPAM"].Scale = 2;

                cmd.Parameters.Add("POEBCL", iDB2DbType.iDB2Decimal, 0).Direction = ParameterDirection.Output;  // 19
                cmd.Parameters["POEBCL"].Precision = 13;
                cmd.Parameters["POEBCL"].Scale = 2;

                cmd.Parameters.Add("POAVAP", iDB2DbType.iDB2Decimal, 0).Direction = ParameterDirection.Output;  // 20
                cmd.Parameters["POAVAP"].Precision = 13;
                cmd.Parameters["POAVAP"].Scale = 2;

                cmd.Parameters.Add("POSTS", iDB2DbType.iDB2Char, 2).Direction = ParameterDirection.Output;   // 21
                cmd.Parameters.Add("POOLDC", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;  // 22
                cmd.Parameters.Add("POVENR", iDB2DbType.iDB2Char, 2).Direction = ParameterDirection.Output;  // 23 

                cmd.Parameters.Add("POPERV", iDB2DbType.iDB2Decimal, 0).Direction = ParameterDirection.Output;  // 24
                cmd.Parameters["POPERV"].Precision = 5;
                cmd.Parameters["POPERV"].Scale = 2;

                cmd.Parameters.Add("POEBCS", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;   // 25
                cmd.Parameters.Add("POERR", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;    // 26
                cmd.Parameters.Add("PERMSG", iDB2DbType.iDB2Char, 64).Direction = ParameterDirection.Output;  // 27

                if (m_da400.ExecuteSubRoutine("GNSR87", ref cmd))
                {
                    POBOTL = cmd.Parameters["POBOTL"].Value.ToString().Trim();
                    PONOAP = cmd.Parameters["PONOAP"].Value.ToString().Trim();
                    POCSBL = cmd.Parameters["POCSBL"].Value.ToString().Trim();
                    POCRAV = cmd.Parameters["POCRAV"].Value.ToString().Trim();
                    POAPAM = cmd.Parameters["POAPAM"].Value.ToString().Trim();
                    POEBCL = cmd.Parameters["POEBCL"].Value.ToString().Trim();
                    POAVAP = cmd.Parameters["POAVAP"].Value.ToString().Trim();
                    POSTS = cmd.Parameters["POSTS"].Value.ToString().Trim();
                    POOLDC = cmd.Parameters["POOLDC"].Value.ToString().Trim();
                    POVENR = cmd.Parameters["POVENR"].Value.ToString().Trim();
                    POPERV = cmd.Parameters["POPERV"].Value.ToString().Trim();
                    POEBCS = cmd.Parameters["POEBCS"].Value.ToString().Trim();
                    POERR = cmd.Parameters["POERR"].Value.ToString().Trim();
                    PERMSG = cmd.Parameters["PERMSG"].Value.ToString().Trim();

                    m_da400.CloseConnect();
                    return true;
                }
                else
                {
                    m_da400.CloseConnect();
                    return false;
                }

                //dsResult = m_da400.ExecSubRoutine_DS(ref cmd, ref m_LastError);
                m_da400.CloseConnect(); ;

            }
            catch (Exception ex)
            {
                return false;
                m_da400.CloseConnect();
            }
            //return dsResult;
        }



        public bool Call_ILSR97(string prmWKCDE, string prmWKFMT, string strBizInit, string strBranchNo, ref string prmWKDTE)
        {
            DataSet dsResult = new DataSet();
            iDB2Command cmd = new iDB2Command();
            try
            {

                //GeneralLib;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "ILSR97";

                //// INPUT
                cmd.Parameters.Add("WKCDE", iDB2DbType.iDB2Char, 2).Value = prmWKCDE.ToString().Trim();
                cmd.Parameters.Add("WKFMT", iDB2DbType.iDB2Char, 3).Value = prmWKFMT.ToString().Trim();

                //// OUTPUT
                cmd.Parameters.Add("WKDTE", iDB2DbType.iDB2Decimal, 0).Direction = ParameterDirection.Output;
                cmd.Parameters["WKDTE"].Precision = 8;
                cmd.Parameters["WKDTE"].Scale = 0;
                //m_da400.AS400Lib = EB_Service.Commons.AppConfig.GNPGMLib;

                if (m_da400.ExecuteSubRoutine("ILSR97", ref cmd, strBizInit, strBranchNo))
                {
                    prmWKDTE = cmd.Parameters["WKDTE"].Value.ToString().Trim();
                    m_da400.CloseConnect();
                    return true;
                }
                else
                {
                    m_da400.CloseConnect();
                    return false;
                }

            }
            catch (Exception ex)
            {
                m_da400.CloseConnect();
                return false;
            }

        }


        public bool Call_GNSR093(string IDNO, string salary, ref string WPERR, ref string WPAYMENT, string strBizInit, string strBranchNo)
        {
            DataSet dsResult = new DataSet();
            iDB2Command cmd = new iDB2Command();
            try
            {

                //GeneralLib;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GNSR093";

                //// INPUT
                cmd.Parameters.Add("WIDNO", iDB2DbType.iDB2Char, 15).Value = IDNO;
                cmd.Parameters.Add("WSALARY", iDB2DbType.iDB2Decimal, 0).Value = salary;
                cmd.Parameters["WSALARY"].Precision = 9;
                cmd.Parameters["WSALARY"].Scale = 2;

                //// OUTPUT
                cmd.Parameters.Add("WPAYMENT", iDB2DbType.iDB2Decimal, 0).Direction = ParameterDirection.Output;
                cmd.Parameters["WPAYMENT"].Precision = 7;
                cmd.Parameters["WPAYMENT"].Scale = 0;

                cmd.Parameters.Add("WPERR", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;
                //m_da400.AS400Lib = EB_Service.Commons.AppConfig.GNPGMLib;

                if (m_da400.ExecuteSubRoutine("GNSR093", ref cmd, strBizInit, strBranchNo))
                {
                    WPAYMENT = cmd.Parameters["WPAYMENT"].Value.ToString().Trim();
                    WPERR = cmd.Parameters["WPERR"].Value.ToString().Trim();
                    m_da400.CloseConnect();
                    return true;
                }
                else
                {
                    m_da400.CloseConnect();
                    return false;
                }

            }
            catch (Exception ex)
            {
                m_da400.CloseConnect();
                return false;
            }

        }

        public bool Call_ILSR39(string WIPCAM, string WIINST, string WIINTR, string WICRUR,
                                ref string WOADJT, ref string WOADJL, ref string WODEDL, ref string WOCONA, ref string WPERR, string strBizInit, string strBranchNo)
        {
            iDB2Command cmd = new iDB2Command();
            try
            {

                //GeneralLib;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "ILSR39";

                //// INPUT

                cmd.Parameters.Add("WIPCAM", iDB2DbType.iDB2Decimal, 0).Value = WIPCAM;
                cmd.Parameters["WIPCAM"].Precision = 13;
                cmd.Parameters["WIPCAM"].Scale = 2;

                cmd.Parameters.Add("WIINST", iDB2DbType.iDB2Decimal, 0).Value = WIINST;
                cmd.Parameters["WIINST"].Precision = 7;
                cmd.Parameters["WIINST"].Scale = 0;

                cmd.Parameters.Add("WIINTR", iDB2DbType.iDB2Decimal, 0).Value = WIINTR;
                cmd.Parameters["WIINTR"].Precision = 4;
                cmd.Parameters["WIINTR"].Scale = 2;

                cmd.Parameters.Add("WICRUR", iDB2DbType.iDB2Decimal, 0).Value = WICRUR;
                cmd.Parameters["WICRUR"].Precision = 4;
                cmd.Parameters["WICRUR"].Scale = 2;



                //// OUTPUT
                cmd.Parameters.Add("WOADJT", iDB2DbType.iDB2Decimal, 0).Direction = ParameterDirection.Output;
                cmd.Parameters["WOADJT"].Precision = 3;
                cmd.Parameters["WOADJT"].Scale = 0;
                cmd.Parameters["WOADJT"].Value = 0;

                cmd.Parameters.Add("WOADJL", iDB2DbType.iDB2Decimal, 0).Direction = ParameterDirection.Output;
                cmd.Parameters["WOADJL"].Precision = 11;
                cmd.Parameters["WOADJL"].Scale = 0;
                cmd.Parameters["WOADJL"].Value = 0;

                cmd.Parameters.Add("WODEDL", iDB2DbType.iDB2Decimal, 0).Direction = ParameterDirection.Output;
                cmd.Parameters["WODEDL"].Precision = 13;
                cmd.Parameters["WODEDL"].Scale = 2;
                cmd.Parameters["WODEDL"].Value = 0;

                cmd.Parameters.Add("WOCONA", iDB2DbType.iDB2Decimal, 0).Direction = ParameterDirection.Output;
                cmd.Parameters["WOCONA"].Precision = 13;
                cmd.Parameters["WOCONA"].Scale = 2;
                cmd.Parameters["WOCONA"].Value = 0;

                cmd.Parameters.Add("WPERR", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;
                cmd.Parameters["WPERR"].Value = "";

                if (m_da400.ExecuteSubRoutine("ILSR39", ref cmd, strBizInit, strBranchNo))
                {
                    WPERR = cmd.Parameters["WPERR"].Value.ToString().Trim();
                    m_da400.CloseConnect();
                    return true;
                }
                else
                {
                    m_da400.CloseConnect();
                    return false;
                }

            }
            catch (Exception ex)
            {
                m_da400.CloseConnect();
                return false;
            }
        }

        public bool Call_ILSYD24D(string PITEXT, string PITERM, string PIINTR, string PICRUR,
                                  ref string POPCAM, ref string POINTR, ref string POCRUR, ref string POINFR,
                                  ref string PODIFR, ref string POINST, ref string POTOAM, ref string PODUTY,
                                  ref string POINTB, ref string POCRUB, ref string POINFB, ref string POCONA,
                                  ref string POFDAT, ref string POAINR, ref string POACRU, ref string PODDAT,
                                  ref string POPPRN, ref string POINSD, ref string POINTD, ref string POCRUD,
                                  ref string POINFD, ref string POCDAT, ref string POINCM, ref string POREBT,
                                  ref string POCLSA, ref string POFLAG, string strBizInit, string strBranchNo
                                  )
        {
            iDB2Command cmd = new iDB2Command();
            try
            {

                //GeneralLib;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "ILSYD24D";

                //// INPUT
                cmd.Parameters.Add("PITEXT", iDB2DbType.iDB2Char, 75).Value = PITEXT;  //0
                cmd.Parameters.Add("PITERM", iDB2DbType.iDB2Char, 297).Value = PITERM;  //1
                cmd.Parameters.Add("PIINTR", iDB2DbType.iDB2Char, 396).Value = PIINTR;  //2
                cmd.Parameters.Add("PICRUR", iDB2DbType.iDB2Char, 396).Value = PICRUR;  //3


                //// OUTPUT

                cmd.Parameters.Add("POPCAM", iDB2DbType.iDB2Char, 1287).Direction = ParameterDirection.Output; //4
                cmd.Parameters["POPCAM"].Value = "";

                cmd.Parameters.Add("POINTR", iDB2DbType.iDB2Char, 1188).Direction = ParameterDirection.Output;  //5
                cmd.Parameters["POINTR"].Value = "";

                cmd.Parameters.Add("POCRUR", iDB2DbType.iDB2Char, 1188).Direction = ParameterDirection.Output;  //6
                cmd.Parameters["POCRUR"].Value = "";

                cmd.Parameters.Add("POINFR", iDB2DbType.iDB2Char, 1188).Direction = ParameterDirection.Output;  //7
                cmd.Parameters["POINFR"].Value = "";

                cmd.Parameters.Add("PODIFR", iDB2DbType.iDB2Char, 495).Direction = ParameterDirection.Output;  //8
                cmd.Parameters["PODIFR"].Value = "";

                cmd.Parameters.Add("POINST", iDB2DbType.iDB2Char, 693).Direction = ParameterDirection.Output;  //9
                cmd.Parameters["POINST"].Value = "";

                cmd.Parameters.Add("POTOAM", iDB2DbType.iDB2Char, 1089).Direction = ParameterDirection.Output;  //10
                cmd.Parameters["POTOAM"].Value = "";

                //cmd.Parameters.Add("PODUTY", iDB2DbType.iDB2Char, 1089).Direction = ParameterDirection.Output;
                //cmd.Parameters["PODUTY"].Value = "";

                cmd.Parameters.Add("PODUTY", iDB2DbType.iDB2Decimal, 0).Direction = ParameterDirection.Output;  //11
                cmd.Parameters["PODUTY"].Precision = 7;
                cmd.Parameters["PODUTY"].Scale = 0;
                cmd.Parameters["PODUTY"].Value = 0;

                cmd.Parameters.Add("POINTB", iDB2DbType.iDB2Decimal, 0).Direction = ParameterDirection.Output;  //12
                cmd.Parameters["POINTB"].Precision = 9;
                cmd.Parameters["POINTB"].Scale = 2;
                cmd.Parameters["POINTB"].Value = 0;

                cmd.Parameters.Add("POCRUB", iDB2DbType.iDB2Decimal, 0).Direction = ParameterDirection.Output;  //13
                cmd.Parameters["POCRUB"].Precision = 9;
                cmd.Parameters["POCRUB"].Scale = 2;
                cmd.Parameters["POCRUB"].Value = 0;

                cmd.Parameters.Add("POINFB", iDB2DbType.iDB2Decimal, 0).Direction = ParameterDirection.Output; //14
                cmd.Parameters["POINFB"].Precision = 9;
                cmd.Parameters["POINFB"].Scale = 2;
                cmd.Parameters["POINFB"].Value = 0;

                cmd.Parameters.Add("POCONA", iDB2DbType.iDB2Decimal, 0).Direction = ParameterDirection.Output;  //15
                cmd.Parameters["POCONA"].Precision = 13;
                cmd.Parameters["POCONA"].Scale = 2;
                cmd.Parameters["POCONA"].Value = 0;



                cmd.Parameters.Add("POFDAT", iDB2DbType.iDB2Char, 8).Direction = ParameterDirection.Output; //16
                cmd.Parameters["POFDAT"].Value = "";

                cmd.Parameters.Add("POAINR", iDB2DbType.iDB2Decimal, 0).Direction = ParameterDirection.Output; //17
                cmd.Parameters["POAINR"].Precision = 5;
                cmd.Parameters["POAINR"].Scale = 2;
                cmd.Parameters["POAINR"].Value = 0;

                cmd.Parameters.Add("POACRU", iDB2DbType.iDB2Decimal, 0).Direction = ParameterDirection.Output; //18
                cmd.Parameters["POACRU"].Precision = 5;
                cmd.Parameters["POACRU"].Scale = 2;
                cmd.Parameters["POACRU"].Value = 0;

                cmd.Parameters.Add("PODDAT", iDB2DbType.iDB2Char, 792).Direction = ParameterDirection.Output; //19
                cmd.Parameters["PODDAT"].Value = "";

                cmd.Parameters.Add("POPPRN", iDB2DbType.iDB2Char, 1089).Direction = ParameterDirection.Output; //20
                cmd.Parameters["POPPRN"].Value = "";

                cmd.Parameters.Add("POINSD", iDB2DbType.iDB2Char, 693).Direction = ParameterDirection.Output; //21
                cmd.Parameters["POINSD"].Value = "";

                cmd.Parameters.Add("POINTD", iDB2DbType.iDB2Char, 1188).Direction = ParameterDirection.Output;  //22
                cmd.Parameters["POINTD"].Value = "";

                cmd.Parameters.Add("POCRUD", iDB2DbType.iDB2Char, 1188).Direction = ParameterDirection.Output; //23
                cmd.Parameters["POCRUD"].Value = "";

                cmd.Parameters.Add("POINFD", iDB2DbType.iDB2Char, 1188).Direction = ParameterDirection.Output;  //24
                cmd.Parameters["POINFD"].Value = "";

                cmd.Parameters.Add("POCDAT", iDB2DbType.iDB2Char, 792).Direction = ParameterDirection.Output;  //25
                cmd.Parameters["POCDAT"].Value = "";

                cmd.Parameters.Add("POINCM", iDB2DbType.iDB2Char, 1188).Direction = ParameterDirection.Output;  //26
                cmd.Parameters["POINCM"].Value = "";

                cmd.Parameters.Add("POREBT", iDB2DbType.iDB2Char, 1188).Direction = ParameterDirection.Output;  //27
                cmd.Parameters["POREBT"].Value = "";

                cmd.Parameters.Add("POCLSA", iDB2DbType.iDB2Char, 1089).Direction = ParameterDirection.Output;  //28
                cmd.Parameters["POCLSA"].Value = "";


                cmd.Parameters.Add("POFLAG", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;  //29
                cmd.Parameters["POFLAG"].Value = "";

                if (m_da400.ExecuteSubRoutine("ILSYD24D", ref cmd, strBizInit, strBranchNo))
                {
                    POPCAM = cmd.Parameters["POPCAM"].Value.ToString().Trim();
                    POINTR = cmd.Parameters["POINTR"].Value.ToString().Trim();
                    POCRUR = cmd.Parameters["POCRUR"].Value.ToString().Trim();
                    POINFR = cmd.Parameters["POINFR"].Value.ToString().Trim();
                    PODIFR = cmd.Parameters["PODIFR"].Value.ToString().Trim();
                    POINST = cmd.Parameters["POINST"].Value.ToString().Trim();
                    POTOAM = cmd.Parameters["POTOAM"].Value.ToString().Trim();
                    PODUTY = cmd.Parameters["PODUTY"].Value.ToString().Trim();
                    POINTB = cmd.Parameters["POINTB"].Value.ToString().Trim();
                    POCRUB = cmd.Parameters["POCRUB"].Value.ToString().Trim();
                    POINFB = cmd.Parameters["POINFB"].Value.ToString().Trim();
                    POCONA = cmd.Parameters["POCONA"].Value.ToString().Trim();
                    POFDAT = cmd.Parameters["POFDAT"].Value.ToString().Trim();
                    POAINR = cmd.Parameters["POAINR"].Value.ToString().Trim();
                    POACRU = cmd.Parameters["POACRU"].Value.ToString().Trim();
                    PODDAT = cmd.Parameters["PODDAT"].Value.ToString().Trim();
                    POPPRN = cmd.Parameters["POPPRN"].Value.ToString().Trim();
                    POINSD = cmd.Parameters["POINSD"].Value.ToString().Trim();
                    POINTD = cmd.Parameters["POINTD"].Value.ToString().Trim();
                    POCRUD = cmd.Parameters["POCRUD"].Value.ToString().Trim();
                    POINFD = cmd.Parameters["POINFD"].Value.ToString().Trim();
                    POCDAT = cmd.Parameters["POCDAT"].Value.ToString().Trim();
                    POINCM = cmd.Parameters["POINCM"].Value.ToString().Trim();
                    POREBT = cmd.Parameters["POREBT"].Value.ToString().Trim();
                    POCLSA = cmd.Parameters["POCLSA"].Value.ToString().Trim();

                    POFLAG = cmd.Parameters["POFLAG"].Value.ToString().Trim();
                    m_da400.CloseConnect();
                    return true;
                }
                else
                {
                    m_da400.CloseConnect();
                    return false;
                }

            }
            catch (Exception ex)
            {
                m_da400.CloseConnect();
                return false;
            }
        }

        public bool Call_ILSREIR(string PITEXT, string PITERM, string PIINTR, string PICRUR,
                                  ref string POPCAM, ref string POINTR, ref string POCRUR, ref string POINFR,
                                  ref string PODIFR, ref string POINST, ref string POTOAM, ref string PODUTY,
                                  ref string POINTB, ref string POCRUB, ref string POINFB, ref string POCONA,
                                  ref string POFDAT, ref string POAINR, ref string POACRU, ref string PODDAT,
                                  ref string POPPRN, ref string POINSD, ref string POINTD, ref string POCRUD,
                                  ref string POINFD, ref string POCDAT, ref string POINCM, ref string POREBT,
                                  ref string POCLSA, ref string POFLAG, string strBizInit, string strBranchNo
                                  )
        {
            iDB2Command cmd = new iDB2Command();
            try
            {

                //GeneralLib;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "ILSREIR";

                //// INPUT
                cmd.Parameters.Add("PITEXT", iDB2DbType.iDB2Char, 75).Value = PITEXT;  //0
                cmd.Parameters.Add("PITERM", iDB2DbType.iDB2Char, 297).Value = PITERM;  //1
                cmd.Parameters.Add("PIINTR", iDB2DbType.iDB2Char, 396).Value = PIINTR;  //2
                cmd.Parameters.Add("PICRUR", iDB2DbType.iDB2Char, 396).Value = PICRUR;  //3


                //// OUTPUT

                cmd.Parameters.Add("POPCAM", iDB2DbType.iDB2Char, 1287).Direction = ParameterDirection.Output; //4
                cmd.Parameters["POPCAM"].Value = "";

                cmd.Parameters.Add("POINTR", iDB2DbType.iDB2Char, 1188).Direction = ParameterDirection.Output;  //5
                cmd.Parameters["POINTR"].Value = "";

                cmd.Parameters.Add("POCRUR", iDB2DbType.iDB2Char, 1188).Direction = ParameterDirection.Output;  //6
                cmd.Parameters["POCRUR"].Value = "";

                cmd.Parameters.Add("POINFR", iDB2DbType.iDB2Char, 1188).Direction = ParameterDirection.Output;  //7
                cmd.Parameters["POINFR"].Value = "";

                cmd.Parameters.Add("PODIFR", iDB2DbType.iDB2Char, 495).Direction = ParameterDirection.Output;  //8
                cmd.Parameters["PODIFR"].Value = "";

                cmd.Parameters.Add("POINST", iDB2DbType.iDB2Char, 693).Direction = ParameterDirection.Output;  //9
                cmd.Parameters["POINST"].Value = "";

                cmd.Parameters.Add("POTOAM", iDB2DbType.iDB2Char, 1089).Direction = ParameterDirection.Output;  //10
                cmd.Parameters["POTOAM"].Value = "";

                //cmd.Parameters.Add("PODUTY", iDB2DbType.iDB2Char, 1089).Direction = ParameterDirection.Output;
                //cmd.Parameters["PODUTY"].Value = "";

                cmd.Parameters.Add("PODUTY", iDB2DbType.iDB2Decimal, 0).Direction = ParameterDirection.Output;  //11
                cmd.Parameters["PODUTY"].Precision = 7;
                cmd.Parameters["PODUTY"].Scale = 0;
                cmd.Parameters["PODUTY"].Value = 0;

                cmd.Parameters.Add("POINTB", iDB2DbType.iDB2Decimal, 0).Direction = ParameterDirection.Output;  //12
                cmd.Parameters["POINTB"].Precision = 9;
                cmd.Parameters["POINTB"].Scale = 2;
                cmd.Parameters["POINTB"].Value = 0;

                cmd.Parameters.Add("POCRUB", iDB2DbType.iDB2Decimal, 0).Direction = ParameterDirection.Output;  //13
                cmd.Parameters["POCRUB"].Precision = 9;
                cmd.Parameters["POCRUB"].Scale = 2;
                cmd.Parameters["POCRUB"].Value = 0;

                cmd.Parameters.Add("POINFB", iDB2DbType.iDB2Decimal, 0).Direction = ParameterDirection.Output; //14
                cmd.Parameters["POINFB"].Precision = 9;
                cmd.Parameters["POINFB"].Scale = 2;
                cmd.Parameters["POINFB"].Value = 0;

                cmd.Parameters.Add("POCONA", iDB2DbType.iDB2Decimal, 0).Direction = ParameterDirection.Output;  //15
                cmd.Parameters["POCONA"].Precision = 13;
                cmd.Parameters["POCONA"].Scale = 2;
                cmd.Parameters["POCONA"].Value = 0;



                cmd.Parameters.Add("POFDAT", iDB2DbType.iDB2Char, 8).Direction = ParameterDirection.Output; //16
                cmd.Parameters["POFDAT"].Value = "";

                cmd.Parameters.Add("POAINR", iDB2DbType.iDB2Decimal, 0).Direction = ParameterDirection.Output; //17
                cmd.Parameters["POAINR"].Precision = 5;
                cmd.Parameters["POAINR"].Scale = 2;
                cmd.Parameters["POAINR"].Value = 0;

                cmd.Parameters.Add("POACRU", iDB2DbType.iDB2Decimal, 0).Direction = ParameterDirection.Output; //18
                cmd.Parameters["POACRU"].Precision = 5;
                cmd.Parameters["POACRU"].Scale = 2;
                cmd.Parameters["POACRU"].Value = 0;

                cmd.Parameters.Add("PODDAT", iDB2DbType.iDB2Char, 792).Direction = ParameterDirection.Output; //19
                cmd.Parameters["PODDAT"].Value = "";

                cmd.Parameters.Add("POPPRN", iDB2DbType.iDB2Char, 1089).Direction = ParameterDirection.Output; //20
                cmd.Parameters["POPPRN"].Value = "";

                cmd.Parameters.Add("POINSD", iDB2DbType.iDB2Char, 693).Direction = ParameterDirection.Output; //21
                cmd.Parameters["POINSD"].Value = "";

                cmd.Parameters.Add("POINTD", iDB2DbType.iDB2Char, 1188).Direction = ParameterDirection.Output;  //22
                cmd.Parameters["POINTD"].Value = "";

                cmd.Parameters.Add("POCRUD", iDB2DbType.iDB2Char, 1188).Direction = ParameterDirection.Output; //23
                cmd.Parameters["POCRUD"].Value = "";

                cmd.Parameters.Add("POINFD", iDB2DbType.iDB2Char, 1188).Direction = ParameterDirection.Output;  //24
                cmd.Parameters["POINFD"].Value = "";

                cmd.Parameters.Add("POCDAT", iDB2DbType.iDB2Char, 792).Direction = ParameterDirection.Output;  //25
                cmd.Parameters["POCDAT"].Value = "";

                cmd.Parameters.Add("POINCM", iDB2DbType.iDB2Char, 1188).Direction = ParameterDirection.Output;  //26
                cmd.Parameters["POINCM"].Value = "";

                cmd.Parameters.Add("POREBT", iDB2DbType.iDB2Char, 1188).Direction = ParameterDirection.Output;  //27
                cmd.Parameters["POREBT"].Value = "";

                cmd.Parameters.Add("POCLSA", iDB2DbType.iDB2Char, 1089).Direction = ParameterDirection.Output;  //28
                cmd.Parameters["POCLSA"].Value = "";


                cmd.Parameters.Add("POFLAG", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;  //29
                cmd.Parameters["POFLAG"].Value = "";

                if (m_da400.ExecuteSubRoutine("ILSREIR", ref cmd, strBizInit, strBranchNo))
                {
                    POPCAM = cmd.Parameters["POPCAM"].Value.ToString().Trim();
                    POINTR = cmd.Parameters["POINTR"].Value.ToString().Trim();
                    POCRUR = cmd.Parameters["POCRUR"].Value.ToString().Trim();
                    POINFR = cmd.Parameters["POINFR"].Value.ToString().Trim();
                    PODIFR = cmd.Parameters["PODIFR"].Value.ToString().Trim();
                    POINST = cmd.Parameters["POINST"].Value.ToString().Trim();
                    POTOAM = cmd.Parameters["POTOAM"].Value.ToString().Trim();
                    PODUTY = cmd.Parameters["PODUTY"].Value.ToString().Trim();
                    POINTB = cmd.Parameters["POINTB"].Value.ToString().Trim();
                    POCRUB = cmd.Parameters["POCRUB"].Value.ToString().Trim();
                    POINFB = cmd.Parameters["POINFB"].Value.ToString().Trim();
                    POCONA = cmd.Parameters["POCONA"].Value.ToString().Trim();
                    POFDAT = cmd.Parameters["POFDAT"].Value.ToString().Trim();
                    POAINR = cmd.Parameters["POAINR"].Value.ToString().Trim();
                    POACRU = cmd.Parameters["POACRU"].Value.ToString().Trim();
                    PODDAT = cmd.Parameters["PODDAT"].Value.ToString().Trim();
                    POPPRN = cmd.Parameters["POPPRN"].Value.ToString().Trim();
                    POINSD = cmd.Parameters["POINSD"].Value.ToString().Trim();
                    POINTD = cmd.Parameters["POINTD"].Value.ToString().Trim();
                    POCRUD = cmd.Parameters["POCRUD"].Value.ToString().Trim();
                    POINFD = cmd.Parameters["POINFD"].Value.ToString().Trim();
                    POCDAT = cmd.Parameters["POCDAT"].Value.ToString().Trim();
                    POINCM = cmd.Parameters["POINCM"].Value.ToString().Trim();
                    POREBT = cmd.Parameters["POREBT"].Value.ToString().Trim();
                    POCLSA = cmd.Parameters["POCLSA"].Value.ToString().Trim();

                    POFLAG = cmd.Parameters["POFLAG"].Value.ToString().Trim();
                    m_da400.CloseConnect();
                    return true;
                }
                else
                {
                    m_da400.CloseConnect();
                    return false;
                }

            }
            catch (Exception ex)
            {
                m_da400.CloseConnect();
                return false;
            }
        }
        public bool Call_CSSR035(string PICSNO, string PIOCCU, string PIDOCF, ref string POERRC, ref string POERRM, string strBizInit, string strBranchNo)
        {
            iDB2Command cmd = new iDB2Command();

            m_da400.AS400Lib = EB_Service.Commons.AppConfig.GNPGMLib;  //GeneralLib;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "CSSR035";

            cmd.Parameters.Add("PICSNO", iDB2DbType.iDB2Decimal, 0).Value = PICSNO;
            cmd.Parameters["PICSNO"].Precision = 8;
            cmd.Parameters["PICSNO"].Scale = 0;

            cmd.Parameters.Add("PIOCCU", iDB2DbType.iDB2Char, 3).Value = PIOCCU;
            cmd.Parameters.Add("PIDOCF", iDB2DbType.iDB2Char, 1).Value = PIDOCF;

            cmd.Parameters.Add("POERRC", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;
            cmd.Parameters["POERRC"].Value = "";


            cmd.Parameters.Add("POERRM", iDB2DbType.iDB2Char, 64).Direction = ParameterDirection.Output;
            cmd.Parameters["POERRM"].Value = "";


            if (m_da400.ExecuteSubRoutine("CSSR035", ref cmd, strBizInit, strBranchNo))
            {
                POERRC = cmd.Parameters["POERRC"].Value.ToString().Trim();
                POERRM = cmd.Parameters["POERRM"].Value.ToString().Trim();

                m_da400.CloseConnect();
                return true;
            }
            else
            {
                m_da400.CloseConnect();
                return false;
            }
        }

        public bool Call_GNSRCBST(string WPNME, ref string WPPER, ref string WPERR, ref string WPOLR, string strBizInit, string strBranchNo)
        {
            iDB2Command cmd = new iDB2Command();

            m_da400.AS400Lib = EB_Service.Commons.AppConfig.GNPGMLib;  //GeneralLib;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "GNSRCB";

            cmd.Parameters.Add("WPNME", iDB2DbType.iDB2Char, 50).Value = WPNME;



            cmd.Parameters.Add("WPPER", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;
            cmd.Parameters["WPPER"].Value = "";


            cmd.Parameters.Add("WPERR", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;
            cmd.Parameters["WPERR"].Value = "";

            cmd.Parameters.Add("WPOLR", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;
            cmd.Parameters["WPOLR"].Value = "";


            if (m_da400.ExecuteSubRoutine("GNSRCB", ref cmd, strBizInit, strBranchNo))
            {
                WPPER = cmd.Parameters["WPPER"].Value.ToString().Trim();
                WPERR = cmd.Parameters["WPERR"].Value.ToString().Trim();
                WPOLR = cmd.Parameters["WPOLR"].Value.ToString().Trim();

                m_da400.CloseConnect();
                return true;
            }
            else
            {
                m_da400.CloseConnect();
                return false;
            }
        }


        public bool Call_ILSR02(string WBRN, string WILNTY, ref string WCONT, ref string WOERRF, ref string WOEMSG, string strBizInit, string strBranchNo)
        {
            iDB2Command cmd = new iDB2Command();

            m_da400.AS400Lib = EB_Service.Commons.AppConfig.GNPGMLib;  //GeneralLib;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ILSR02";

            cmd.Parameters.Add("WBRN", iDB2DbType.iDB2Char, 3).Value = WBRN;
            cmd.Parameters.Add("WILNTY", iDB2DbType.iDB2Char, 2).Value = WILNTY;

            cmd.Parameters.Add("WCONT", iDB2DbType.iDB2Char, 16).Direction = ParameterDirection.Output;
            cmd.Parameters["WCONT"].Value = "";

            cmd.Parameters.Add("WOERRF", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;
            cmd.Parameters["WOERRF"].Value = "";

            cmd.Parameters.Add("WOEMSG", iDB2DbType.iDB2Char, 60).Direction = ParameterDirection.Output;
            cmd.Parameters["WOEMSG"].Value = "";

            if (m_da400.ExecuteSubRoutine("ILSR02", ref cmd, strBizInit, strBranchNo))
            {
                WCONT = cmd.Parameters["WCONT"].Value.ToString().Trim();
                WOERRF = cmd.Parameters["WOERRF"].Value.ToString().Trim();
                WOEMSG = cmd.Parameters["WOEMSG"].Value.ToString().Trim();
                // m_da400.CloseConnect();
                return true;
            }
            else
            {
                //m_da400.CloseConnect();
                return false;
            }

        }
        public bool Call_CSSR06C(ILDataCenter ilObj, string WISYS, string WIIDNO, string WIBRN, string WIAPNO, string WICONT_,
                                string WITCA, string WIRES, string WIAPDT, string WIBHDT, string WICSTY, string WIBNGS, string WISALA,
                                string WITCL, string WIPCAM, string WIVDID, string WIUSOP, ref string WOERR, ref string WOERRM
                                , string strBizInit, string strBranchNo
                                )
        {
            iDB2Command cmd = new iDB2Command();

            m_da400.AS400Lib = EB_Service.Commons.AppConfig.GNPGMLib;  //GeneralLib;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "CSSR06C";

            cmd.Parameters.Add("WISYS", iDB2DbType.iDB2Char, 2).Value = WISYS;  //0

            cmd.Parameters.Add("WIIDNO", iDB2DbType.iDB2Char, 15).Value = WIIDNO; //1

            cmd.Parameters.Add("WIBRN", iDB2DbType.iDB2Decimal, 0).Value = WIBRN; //2
            cmd.Parameters["WIBRN"].Precision = 3;
            cmd.Parameters["WIBRN"].Scale = 0;

            cmd.Parameters.Add("WIAPNO", iDB2DbType.iDB2Decimal, 0).Value = WIAPNO; // 3
            cmd.Parameters["WIAPNO"].Precision = 11;
            cmd.Parameters["WIAPNO"].Scale = 0;

            cmd.Parameters.Add("WICONT#", iDB2DbType.iDB2Char, 16).Value = WICONT_; //4

            cmd.Parameters.Add("WITCA", iDB2DbType.iDB2Decimal, 0).Value = WITCA;  //5
            cmd.Parameters["WITCA"].Precision = 13;
            cmd.Parameters["WITCA"].Scale = 2;

            cmd.Parameters.Add("WIRES", iDB2DbType.iDB2Char, 6).Value = WIRES; //6

            cmd.Parameters.Add("WIAPDT", iDB2DbType.iDB2Decimal, 0).Value = WIAPDT;  //7
            cmd.Parameters["WIAPDT"].Precision = 8;
            cmd.Parameters["WIAPDT"].Scale = 0;


            cmd.Parameters.Add("WIBHDT", iDB2DbType.iDB2Decimal, 0).Value = WIBHDT;  //8
            cmd.Parameters["WIBHDT"].Precision = 8;
            cmd.Parameters["WIBHDT"].Scale = 0;

            cmd.Parameters.Add("WICSTY", iDB2DbType.iDB2Char, 1).Value = WICSTY;  //9
            cmd.Parameters.Add("WIBNGS", iDB2DbType.iDB2Char, 1).Value = WIBNGS;  //10

            cmd.Parameters.Add("WISALA", iDB2DbType.iDB2Decimal, 0).Value = WISALA; //11
            cmd.Parameters["WISALA"].Precision = 13;
            cmd.Parameters["WISALA"].Scale = 2;

            cmd.Parameters.Add("WITCL", iDB2DbType.iDB2Decimal, 0).Value = WITCL;  //12
            cmd.Parameters["WITCL"].Precision = 13;
            cmd.Parameters["WITCL"].Scale = 2;

            cmd.Parameters.Add("WIPCAM", iDB2DbType.iDB2Decimal, 0).Value = WIPCAM; //13
            cmd.Parameters["WIPCAM"].Precision = 13;
            cmd.Parameters["WIPCAM"].Scale = 2;

            cmd.Parameters.Add("WIVDID", iDB2DbType.iDB2Char, 12).Value = WIVDID.PadLeft(12, '0'); //14
            cmd.Parameters.Add("WIUSOP", iDB2DbType.iDB2Char, 1).Value = WIUSOP;  //15

            cmd.Parameters.Add("WOERR", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output; //16
            cmd.Parameters["WOERR"].Value = "";

            cmd.Parameters.Add("WOERRM", iDB2DbType.iDB2Char, 80).Direction = ParameterDirection.Output; //17
            cmd.Parameters["WOERRM"].Value = "";


            if (m_da400.ExecuteSubRoutine("CSSR06C", ref cmd, strBizInit, strBranchNo))
            {
                WOERR = cmd.Parameters["WOERR"].Value.ToString().Trim();
                WOERRM = cmd.Parameters["WOERRM"].Value.ToString().Trim();

                //m_da400.CloseConnect();
                return true;
            }
            else
            {
                //m_da400.CloseConnect();
                return false;
            }
        }

        public bool Call_ILSRSMS(string PIMODE, string PIBUS, string PIBRN, string PIAPNO, string PITEL_, ref string POERRC, ref string POERRM, string strBizInit, string strBranchNo)
        {
            iDB2Command cmd = new iDB2Command();

            m_da400.AS400Lib = EB_Service.Commons.AppConfig.GNPGMLib;  //GeneralLib;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ILSRSMS";

            cmd.Parameters.Add("PIMODE", iDB2DbType.iDB2Char, 1).Value = PIMODE;
            cmd.Parameters.Add("PIBUS", iDB2DbType.iDB2Char, 2).Value = PIBUS;

            cmd.Parameters.Add("PIBRN", iDB2DbType.iDB2Char, 3).Value = PIBRN;

            cmd.Parameters.Add("PIAPNO", iDB2DbType.iDB2Char, 11).Value = PIAPNO;

            cmd.Parameters.Add("PITEL#", iDB2DbType.iDB2Char, 10).Value = PITEL_;

            cmd.Parameters.Add("POERRC", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output; //16
            cmd.Parameters["POERRC"].Value = "";

            cmd.Parameters.Add("POERRM", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output; //16
            cmd.Parameters["POERRM"].Value = "";


            if (m_da400.ExecuteSubRoutine("ILSRSMS", ref cmd, strBizInit, strBranchNo))
            {
                POERRC = cmd.Parameters["POERRC"].Value.ToString().Trim();
                POERRM = cmd.Parameters["POERRM"].Value.ToString().Trim();

                //m_da400.CloseConnect();
                return true;
            }
            else
            {
                //m_da400.CloseConnect();
                return false;
            }
        }


        public bool Call_ILSR73(string CONTNO1, string SGDATE, string FLGERR1, string FLGMSG1, string strBizInit, string strBranchNo)
        {
            iDB2Command cmd = new iDB2Command();

            m_da400.AS400Lib = EB_Service.Commons.AppConfig.GNPGMLib;  //GeneralLib;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ILSR73";

            cmd.Parameters.Add("CONTNO1", iDB2DbType.iDB2Char, 16).Value = CONTNO1;
            cmd.Parameters.Add("SGDATE", iDB2DbType.iDB2Char, 8).Value = SGDATE;


            cmd.Parameters.Add("FLGERR1", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;
            cmd.Parameters["FLGERR1"].Value = "";

            cmd.Parameters.Add("FLGMSG1", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;
            cmd.Parameters["FLGMSG1"].Value = "";


            if (m_da400.ExecuteSubRoutine("ILSR73", ref cmd, strBizInit, strBranchNo))
            {
                FLGERR1 = cmd.Parameters["FLGERR1"].Value.ToString().Trim();
                FLGMSG1 = cmd.Parameters["FLGMSG1"].Value.ToString().Trim();
                m_da400.CloseConnect();
                return true;
            }
            else
            {
                m_da400.CloseConnect();
                return false;
            }
        }

        public bool Call_GNSR45(string IOFNME, string ITITLE, string ITELOF, string IBIZ, string IBRAN, string IAPP, string IAPDTE,
                                ref string OAPP, ref string OMSGA, ref string OTEL, ref string OMSGT, ref string ONER, ref string OMSG
                                , string strBizInit, string strBranchNo
            )
        {
            iDB2Command cmd = new iDB2Command();

            m_da400.AS400Lib = EB_Service.Commons.AppConfig.GNPGMLib;  //GeneralLib;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "GNSR45";

            cmd.Parameters.Add("IOFNME", iDB2DbType.iDB2Char, 50).Value = IOFNME;
            cmd.Parameters.Add("ITITLE", iDB2DbType.iDB2Char, 8).Value = ITITLE;
            cmd.Parameters.Add("ITELOF", iDB2DbType.iDB2Char, 20).Value = ITELOF;
            cmd.Parameters.Add("IBIZ", iDB2DbType.iDB2Char, 2).Value = IBIZ;

            cmd.Parameters.Add("IBRAN", iDB2DbType.iDB2Decimal, 0).Value = IBRAN;  //12
            cmd.Parameters["IBRAN"].Precision = 3;
            cmd.Parameters["IBRAN"].Scale = 0;

            cmd.Parameters.Add("IAPP", iDB2DbType.iDB2Decimal, 0).Value = IAPP;  //12
            cmd.Parameters["IAPP"].Precision = 11;
            cmd.Parameters["IAPP"].Scale = 0;

            cmd.Parameters.Add("IAPDTE", iDB2DbType.iDB2Decimal, 0).Value = IAPDTE;  //12
            cmd.Parameters["IAPDTE"].Precision = 8;
            cmd.Parameters["IAPDTE"].Scale = 0;

            //***   out put ***//

            cmd.Parameters.Add("OAPP", iDB2DbType.iDB2Decimal, 0).Direction = ParameterDirection.Output; //17
            cmd.Parameters["OAPP"].Precision = 3;
            cmd.Parameters["OAPP"].Scale = 0;
            cmd.Parameters["OAPP"].Value = 0;

            cmd.Parameters.Add("OMSGA", iDB2DbType.iDB2Char, 100).Direction = ParameterDirection.Output;
            cmd.Parameters["OMSGA"].Value = "";

            cmd.Parameters.Add("OTEL", iDB2DbType.iDB2Decimal, 0).Direction = ParameterDirection.Output; //17
            cmd.Parameters["OTEL"].Precision = 3;
            cmd.Parameters["OTEL"].Scale = 0;
            cmd.Parameters["OTEL"].Value = 0;

            cmd.Parameters.Add("OMSGT", iDB2DbType.iDB2Char, 100).Direction = ParameterDirection.Output;
            cmd.Parameters["OMSGT"].Value = "";

            cmd.Parameters.Add("ONER", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;
            cmd.Parameters["ONER"].Value = "";

            cmd.Parameters.Add("OMSG", iDB2DbType.iDB2Char, 100).Direction = ParameterDirection.Output;
            cmd.Parameters["OMSG"].Value = "";




            if (m_da400.ExecuteSubRoutine("GNSR45", ref cmd, strBizInit, strBranchNo))
            {
                OAPP = cmd.Parameters["OAPP"].Value.ToString().Trim();
                OMSGA = cmd.Parameters["OMSGA"].Value.ToString().Trim();
                OTEL = cmd.Parameters["OTEL"].Value.ToString().Trim();
                OMSGT = cmd.Parameters["OMSGT"].Value.ToString().Trim();
                ONER = cmd.Parameters["ONER"].Value.ToString().Trim();
                OMSG = cmd.Parameters["OMSG"].Value.ToString().Trim();
                m_da400.CloseConnect();
                return true;
            }
            else
            {
                m_da400.CloseConnect();
                return false;
            }
        }

        public bool Call_GNSRCS(string WPIDNO, string WPAPPL, string WPBRNO, string WPAPNO, string strBizInit, string strBranchNo, ref string WPHSTS,
                                ref string WPERR, ref string WPMSG)
        {
            iDB2Command cmd = new iDB2Command();

            m_da400.AS400Lib = EB_Service.Commons.AppConfig.GNPGMLib;  //GeneralLib;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "GNSRCS";

            cmd.Parameters.Add("WPIDNO", iDB2DbType.iDB2Char, 15).Value = WPIDNO;
            cmd.Parameters.Add("WPAPPL", iDB2DbType.iDB2Char, 2).Value = WPAPPL;

            cmd.Parameters.Add("WPBRNO", iDB2DbType.iDB2Decimal, 0).Value = WPBRNO;
            cmd.Parameters["WPBRNO"].Precision = 3;
            cmd.Parameters["WPBRNO"].Scale = 0;

            cmd.Parameters.Add("WPAPNO", iDB2DbType.iDB2Decimal, 0).Value = WPAPNO;
            cmd.Parameters["WPAPNO"].Precision = 11;
            cmd.Parameters["WPAPNO"].Scale = 0;


            cmd.Parameters.Add("WPHSTS", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;
            cmd.Parameters["WPHSTS"].Value = "";

            cmd.Parameters.Add("WPERR", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;
            cmd.Parameters["WPERR"].Value = "";

            cmd.Parameters.Add("WPMSG", iDB2DbType.iDB2Char, 50).Direction = ParameterDirection.Output;
            cmd.Parameters["WPMSG"].Value = "";


            if (m_da400.ExecuteSubRoutine("GNSRCS", ref cmd, strBizInit, strBranchNo))
            {
                WPHSTS = cmd.Parameters["WPHSTS"].Value.ToString().Trim();
                WPERR = cmd.Parameters["WPERR"].Value.ToString().Trim();
                WPMSG = cmd.Parameters["WPMSG"].Value.ToString().Trim();
                //m_da400.CloseConnect();
                return true;
            }
            else
            {
                //m_da400.CloseConnect();
                return false;
            }
        }

        public bool CALL_ILR0502C2(string WBDATF, string WBDATT, string WBRN, string WAPP_, string WPCODE, string strBizInit, string strBranchNo)
        {
            string m_UdpTime = DateTime.Now.ToString("HHmmss");
            m_LastError = "";
            iDB2Command cmd = new iDB2Command();

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.CommandText = "ILR0502C2";
            // Parameter In

            cmd.Parameters.Add("WBDATF", iDB2DbType.iDB2Decimal, 0).Value = WBDATF;
            cmd.Parameters["WBDATF"].Precision = 8;
            cmd.Parameters["WBDATF"].Scale = 0;

            cmd.Parameters.Add("WBDATT", iDB2DbType.iDB2Decimal, 0).Value = WBDATT;
            cmd.Parameters["WBDATT"].Precision = 8;
            cmd.Parameters["WBDATT"].Scale = 0;

            cmd.Parameters.Add("WBRN", iDB2DbType.iDB2Decimal, 0).Value = WBRN;
            cmd.Parameters["WBRN"].Precision = 3;
            cmd.Parameters["WBRN"].Scale = 0;

            cmd.Parameters.Add("WAPP#", iDB2DbType.iDB2Decimal, 0).Value = WAPP_;
            cmd.Parameters["WAPP#"].Precision = 1;
            cmd.Parameters["WAPP#"].Scale = 0;

            cmd.Parameters.Add("WOPT", iDB2DbType.iDB2Char, 1).Value = WPCODE;

            if (m_da400.ExecuteSubRoutine("ILR0502C2", ref cmd, strBizInit, strBranchNo))
            {
                return true;
            }
            else
                return false;
        }

        public bool Call_CSSR67(string prm1, string prm2, ref string prmErrMsg, string strBizInit, string strBranchNo)
        {
            m_LastError = "";
            iDB2Command cmd = new iDB2Command();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "CSSR67";

            // Parameter In
            cmd.Parameters.Add("I_CSN", iDB2DbType.iDB2Char, 8).Value = prm1;
            cmd.Parameters.Add("I_PGM", iDB2DbType.iDB2Char, 10).Value = prm2;
            cmd.Parameters.Add("O_ERR", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;


            //if (m_da400.ExecuteSubRoutine(m_CSPGM + ".CSSR67", ref cmd))
            if (m_da400.ExecuteSubRoutine("CSSR67", ref cmd, strBizInit, strBranchNo))
            {
                prmErrMsg = cmd.Parameters["O_ERR"].Value.ToString();
                return true;
            }
            else
                return false;
        }

        //CSSR68 Clear Block Contact
        public bool Call_CSSR68(string prm1, string prm2, ref string prmErrMsg,
            string strBizInit, string strBranchNo)
        {
            m_LastError = "";
            iDB2Command cmd = new iDB2Command();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "CSSR68";
            // Parameter In
            cmd.Parameters.Add("PICSNO", iDB2DbType.iDB2Char, 8).Value = prm1;
            cmd.Parameters.Add("PIALL", iDB2DbType.iDB2Char, 1).Value = prm2;
            cmd.Parameters.Add("POERR", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;

            if (m_da400.ExecuteSubRoutine("CSSR68", ref cmd, strBizInit, strBranchNo))
            {
                prmErrMsg = cmd.Parameters["POERR"].Value.ToString();
                return true;
            }
            else
                return false;
        }

        public bool Call_GNSRCIDF(string PIIDN, string PIBDT, ref string POERC, ref string POMSG,
            string strBizInit, string strBranchNo)
        {
            m_LastError = "";
            iDB2Command cmd = new iDB2Command();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "GNSRCIDF";
            // Parameter In
            cmd.Parameters.Add("PIIDN", iDB2DbType.iDB2Char, 15).Value = PIIDN;
            cmd.Parameters.Add("PIBDT", iDB2DbType.iDB2Char, 8).Value = PIBDT;
            cmd.Parameters.Add("POERC", iDB2DbType.iDB2Char, 2).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("POMSG", iDB2DbType.iDB2Char, 50).Direction = ParameterDirection.Output;

            if (m_da400.ExecuteSubRoutine("GNSRCIDF", ref cmd, strBizInit, strBranchNo))
            {
                POERC = cmd.Parameters["POERC"].Value.ToString();
                POMSG = cmd.Parameters["POMSG"].Value.ToString();
                return true;
            }
            else
                return false;
        }






        #endregion

        #region ---  function sql ---

        //***  get customer by card number  ***// 
        public DataSet getCustInfoByCard(string cardNO)
        {
            DataSet ds = new DataSet();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;
            try
            {
                string sql = " SELECT m00csn as CSN, m00idn as IDCard, TRIM(m00tnm) as Name, TRIM(m00tsn) as Surname, m00sex as Gender, SUBSTR(GNB2FL,2,1) as gn_sex ,M00TTL as TitleCode,GNB2TD as TitleName ,   " +
                              " m00bdt as BirthDate, m00eid as ExpireDate, m00mst as MaritalStatus, t44tds as MaritalStatus_Desc, " +
                              " m00rst as TypeofResident, g32tds as TypeofResident_Desc, m00tof as TotalFamily, " +
                              " m00ryr as ResidentYear, m00rmo as ResidentMonth, m00bus as Business, gn22dt as Business_Desc, " +
                              " m00occ as Occupation,m00sal, gn27dt as Occupation_Desc, m00pos as Position, gn28dt as Position_Desc, " +
                              " m00too as TotalEmployee, m00ept as EmployeeType, gn68dt as EmployeeType_Desc, " +
                              " m00wty as TotalWorkYear, m00wtm as TotalWorkMonth, m00sat as SalaryType, gn17dt as SalaryType_Desc,m00oft,m00ofc,M00DSN ," +
                              " m11tel,M11EXT, m11mob, 'CSSR07' as LastUpdateSalary, m11tam as Tambol_H, gn18dt as Tambol_Desc_H, " +
                              " m11amp as Amphur_H, gn19dt as Amphur_Desc_H, m11prv as Province_H, gn20dt as Province_Desc_H, " +
                              " m11zip as Zipcode_H, " +
                              " p1brn , p1apno , p1apdt , p1pram, p1vdid,P1PDGP " +
                              " FROM CSMS00 " +
                              " LEFT JOIN ilms01 on(p1brn =" + m_userInfo.BranchNo + " AND p1apno = 0 ) " +
                              " LEFT JOIN csms11 on(m00csn=m11csn and m11ref='' and m11cde = 'H') " +
                              " LEFT JOIN gntb18 on(m11tam=gn18cd) " +
                              " LEFT JOIN gntb19 on(m11amp=gn19cd) " +
                              " LEFT JOIN gntb20 on(m11prv=gn20cd) " +
                              " LEFT JOIN gntb44 on(m00mst=t44mrt) " +
                              " LEFT JOIN gntb32 on(m00rst=g32cod) " +
                              " LEFT JOIN gntb22 on(m00bus=gn22cd) " +
                              " LEFT JOIN gntb27 on(m00occ=gn27cd) " +
                              " LEFT JOIN gntb28 on(m00pos=gn28cd) " +
                              " LEFT JOIN gntb68 on(m00ept=gn68cd) " +
                              " LEFT JOIN gntb17 on(m00sat=gn17cd) " +
                              " LEFT JOIN gnmb20 on(M00TTL= GNB2TC) " +
                              " WHERE m00ebc = " + cardNO;
                ds = RetriveAsDataSet(sql);
                CloseConnectioDAL();
            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }
            return ds;
        }

        //*** get verify call Detail IL Normal  ***//
        public DataSet getCustVerifyCall(string branch, string appNo)
        {
            DataSet ds = new DataSet();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;
            try
            {

                string sql = " SELECT   " +
                             " W5CSNO,W5BRN, W5APNO,W5CSTY,W5SBCD,W5FVTO,W5FVTH,W5FVTM,W5FVTE,W5CCRD, " +
                             " W5CUPD,W5CUPT,W5CUSR,W5CWRK,W5CACD,W5CAUD,W5CAUT,W5CAUS,W5CAUW,W5VRTO,W5OFC , " +
                             " W5SALT,W5INET,W5SSO ,W5BOL ,W5HOTL,W5HOEX,W5WOTL,W5WOEX,W5MOTL,W5VOOT,W5VONM, " +
                             " W5TONM,W5VOAD,W5TOAD,W5EMST,W5RVTO,W5IPTO,W5TITO,W5TYEM,W5LETN,W5LETP,W5LETD, " +
                             " W5OCRD,W5OUPD,W5OUPT,W5OUSR,W5OWRK,W5OACD,W5OAUD,W5OAUT,W5OAUS,W5OAUW,W5VRTH, " +
                             " W5HTL ,W5HTEX,W5VHHT,W5VHNM,W5THNM,W5VHAD,W5THAD,W5TYTL,W5RVTH,W5TVTH,W5IPTH, " +
                             " W5TITH,W5HCRD,W5HUPD,W5HUPT,W5HUSR,W5HWRK,W5HACD,W5HAUD,W5HAUT,W5HAUS,W5HAUW, " +
                             " W5VRTM,W5MBTL,W5VMTL,W5MCRD,W5MUPD,W5MUPT,W5MUSR,W5MWRK,W5MACD,W5MAUD,W5MAUT, " +
                             " W5MAUS,W5MAUW,W5VRTE,W5ECRD,W5EUPD,W5EUPT,W5EUSR,W5EWRK,W5EACD,W5EAUD,W5EAUT, " +
                             " W5EAUS,W5EAUW,W5CRTD,W5CRTT,W5FIL ,W5UPDT,W5UPTM,W5USER,W5PRGM,W5WRKS, w5cusr as I_Type, " +
                             " case when w5fvto = 'Y' then w5ousr else '' end I_TO, " +
                             " case when w5fvto = 'Y' then w5oaus else '' end K_TO, " +
                             " case when w5fvth = 'Y' then w5husr else '' end I_TH, " +
                             " case when w5fvth = 'Y' then w5haus else '' end K_TH, " +
                             " case when w5fvtm = 'Y' then w5musr else '' end I_TM, " +
                             " case when w5fvtm = 'Y' then w5maus else '' end K_TM, " +
                             " case when w5fvte = 'Y' then w5eusr else '' end I_TE, " +
                             " case when w5fvte = 'Y' then w5eaus else '' end K_TE, " +
                             " W5KTDL, W5KCRD,W5KUPD, W5KUPT, W5KUSR,W5KWRK,W5KACD, W5KAUD,W5KAUT, W5KAUS,W5KAUW " +
                             " FROM ilwk05  " +
                             " WHERE w5brn = " + branch +
                             " AND w5apno  = " + appNo + " with ur";

                ds = RetriveAsDataSet(sql);
                CloseConnectioDAL();

            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }

            return ds;
        }

        // *** Get Marital status *** //
        public DataSet getMaritalStatus(string code)
        {
            DataSet ds = new DataSet();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;
            try
            {
                string condition = "";
                string sql = " SELECT * FROM GNTB44 " +
                             " WHERE exists(SELECT * FROM gnts44 WHERE gntb44.t44mrt=s44mrt)  ";
                if (code.Trim() != "")
                {
                    condition += " AND t44mrt = '" + code + "'";
                }
                string orderBy = " ORDER BY t44mrt WITH ur ";

                sql += condition + orderBy;
                ds = RetriveAsDataSet(sql);
                CloseConnectioDAL();

            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }

            return ds;
        }

        // *** Get sub Marital status *** //
        public DataSet getSubMaritalStatus(string codeMarital, string code)
        {
            DataSet ds = new DataSet();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;
            try
            {
                string condition = "";
                string sql = "";
                if (codeMarital != "")
                {
                    sql = " SELECT * FROM GNTS44 WHERE s44mrt = '" + codeMarital + "' ";

                    if (code.Trim() != "")
                    {
                        condition = " AND s44smt = '" + code + "'";
                    }
                }
                else
                {
                    sql = "SELECT * FROM GNTS44 ";
                }


                string orderBy = " ORDER BY s44mrt,s44smt WITH ur ";
                sql += condition + orderBy;

                ds = RetriveAsDataSet(sql);
                CloseConnectioDAL();

            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }

            return ds;
        }

        // *** Get resident type ***//
        public DataSet getResidentType(string code)
        {
            DataSet ds = new DataSet();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;
            try
            {
                string condition = "";
                string sql = " SELECT * FROM gntb32  ";
                if (code.Trim() != "")
                {
                    condition = " WHERE G32COD = '" + code + "'";
                }
                string orderBy = " ORDER BY g32cod ";
                sql += condition + orderBy;

                ds = RetriveAsDataSet(sql);
                CloseConnectioDAL();

            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }

            return ds;
        }

        // *** Get business type ***//
        public DataSet getBusinessType(string code)
        {
            DataSet ds = new DataSet();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;
            try
            {
                string condition = "";
                string sql = " SELECT * FROM gntb22  ";
                if (code.Trim() != "")
                {
                    condition = " WHERE GN22CD = '" + code + "'";
                }
                string orderBy = " ORDER BY gn22cd ";
                sql += condition + orderBy;

                ds = RetriveAsDataSet(sql);
                CloseConnectioDAL();

            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }

            return ds;
        }

        // *** Get occupation ***//
        public DataSet getOccupation(string code)
        {
            DataSet ds = new DataSet();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;
            try
            {
                string condition = "";
                string sql = " SELECT * FROM gntb27  ";
                if (code.Trim() != "")
                {
                    condition = " WHERE GN27CD = '" + code + "'";
                }

                string orderBy = " ORDER BY gn27cd ";
                sql += condition + orderBy;

                ds = RetriveAsDataSet(sql);
                CloseConnectioDAL();

            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }

            return ds;
        }

        // *** Get Position ***//
        public DataSet getPosition(string code)
        {
            DataSet ds = new DataSet();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;
            try
            {
                string condition = "";
                string sql = " SELECT * FROM gntb28  ";
                if (code.Trim() != "")
                {
                    condition = " WHERE gn28cd = '" + code + "'";
                }
                string orderBy = " ORDER BY gn28cd ";

                sql += condition + orderBy;
                ds = RetriveAsDataSet(sql);
                CloseConnectioDAL();

            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }

            return ds;
        }

        // *** Get Employee type ***//
        public DataSet getEmployeeType(string code)
        {
            DataSet ds = new DataSet();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;
            try
            {
                string condition = "";
                string sql = " SELECT  * FROM  gntb68  ";
                if (code.Trim() != "")
                {
                    condition = " WHERE gn68cd = '" + code + "'";
                }
                string orderBy = " ORDER BY GN68CD ";

                sql += condition + orderBy;
                ds = RetriveAsDataSet(sql);
                CloseConnectioDAL();

            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }

            return ds;
        }

        // *** Get salary type ***//
        public DataSet getSalaryType(string code)
        {
            DataSet ds = new DataSet();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;
            try
            {
                string condition = "";
                string sql = " SELECT * FROM gntb17 ";
                if (code.Trim() != "")
                {
                    condition = " WHERE gn17cd = '" + code + "'";
                }
                string orderBy = " ORDER BY gn17cd ";

                sql += condition + orderBy;
                ds = RetriveAsDataSet(sql);
                CloseConnectioDAL();

            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }

            return ds;
        }

        // *** Get home Telephone  ***//
        public DataSet getTelephoneType(string code)
        {   // call GNSR16
            DataSet ds = new DataSet();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;
            try
            {


            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }

            return ds;
        }

        //*** Get commercial registration ***//
        public DataSet getCommercialRegister(string occupation)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Columns.Add("code");
            dt.Columns.Add("desc");
            //***  get data ***//
            try
            {
                if (occupation == "011" || occupation == "012")
                {
                    dt.Rows.Add("Y", "ได้รับ");
                    dt.Rows.Add("N", "ไม่ได้รับ");
                }
                if (occupation == "")
                {
                    dt.Rows.Add("Y", "ได้รับ");
                    dt.Rows.Add("N", "ไม่ได้รับ");
                }
                ds.Tables.Add(dt);
            }
            catch (Exception ex)
            {

            }
            return ds;
        }
        //***  get TCL by csn ***//
        public DataSet getTCL(string csn)
        {
            DataSet ds = new DataSet();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;
            try
            {
                string sql = " SELECT * FROM csms03 WHERE m3csno = " + csn + " AND m3del = '' ";
                ds = RetriveAsDataSet(sql);
                CloseConnectioDAL();

            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }
            return ds;
        }

        // *** get payment type //
        public DataSet getPaymentType()
        {
            DataSet ds = new DataSet();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;
            try
            {
                string sql = "SELECT gt48tc,gt48td FROM gntb48 WHERE gt48fl = 'IL' ";
                ds = RetriveAsDataSet(sql);
                CloseConnectioDAL();
            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }
            return ds;
        }

        //***  get Account no. by csn ***//
        public DataSet getDebitAccountByCSN(string csn)
        {
            DataSet ds = new DataSet();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;
            try
            {
                string sql = " SELECT  DISTINCT gn13cd,gn13td,gnb31a,gnb31c,gnb31d, " +
                             " gnb30a,gnb30c,p00cis,p00bnk,p00aty,p00bbr,p00sts,p00bac  " +
                             " FROM ilms00 " +
                             " LEFT JOIN gnmb30 ON P00BNK = gnb30a " +
                             " LEFT JOIN gnmb31 ON P00BNK = gnb31a AND P00BBR = gnb31c " +
                             " LEFT JOIN gntb13 ON p00aty = gn13cd " +
                             " WHERE p00cis= " + csn;

                ds = RetriveAsDataSet(sql);
                CloseConnectioDAL();
            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }
            return ds;
        }
        //***  get csms13 ***//
        public DataSet getCSMS13(string appNo, string brn, string csn)
        {
            DataSet ds = new DataSet();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;
            try
            {
                string sql = " SELECT * FROM CSMS13 WHERE M13APN = '" + appNo + "' AND M13BRN = " + brn + " AND  M13APP = 'IL' " +
                             " AND M13CSN =  " + csn;



                ds = RetriveAsDataSet(sql);
                CloseConnectioDAL();
            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }
            return ds;
        }


        //*** get bank code ***//
        public DataSet getBankCode()
        {
            DataSet ds = new DataSet();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;
            try
            {
                string sql = " SELECT g32bnk, gnb30c,g32fil FROM gnmb32 " +
                             " LEFT JOIN gnmb30 on(g32bnk = gnb30a) " +
                             " WHERE g32app='IL' AND g32typ = 'A' ";

                ds = RetriveAsDataSet(sql);
                CloseConnectioDAL();
            }
            catch (Exception ex)
            {
                CloseConnectioDAL();

            }
            return ds;
        }

        //*** get bank branch ***//
        public DataSet getBankBranch(string bankCode)
        {
            DataSet ds = new DataSet();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;
            try
            {
                string sql = " SELECT gnb31c, gnb31d   " +
                             " FROM gnmb31 " +
                             " WHERE gnb31a = '" + bankCode + "' ORDER BY gnb31c ";

                ds = RetriveAsDataSet(sql);
                CloseConnectioDAL();
            }
            catch (Exception ex)
            {
                CloseConnectioDAL();

            }
            return ds;
        }

        //*** get Account type ***//
        public DataSet getAccountType()
        {
            DataSet ds = new DataSet();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;
            try
            {
                string sql = " SELECT gn13cd,gn13td FROM gntb13 ORDER BY gn13cd ";

                ds = RetriveAsDataSet(sql);
                CloseConnectioDAL();
            }
            catch (Exception ex)
            {
                CloseConnectioDAL();

            }
            return ds;
        }

        //***  get province Amphur Tambol ***//
        public DataSet getTambol(string code, string name = "")
        {
            DataSet ds = new DataSet();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;
            try
            {
                string condition = " WHERE 1 = 1";
                string sql = " SELECT gn18cd, gn18dt FROM gntb18 ";

                if (code.Trim() != "")
                {
                    condition += " AND  gn18cd = " + code;
                }
                if (name.Trim() != "")
                {
                    condition += " AND gn18dt = @tambol ";
                }

                sql += condition;
                iDB2Command cmd = new iDB2Command();
                cmd.CommandText = sql;
                cmd.Parameters.Add("@tambol", name.Trim());
                ds = RetriveAsDataSet(cmd);
                CloseConnectioDAL();
            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }
            return ds;
        }

        public DataSet getAmphur(string nameAmphur, string nameTambol, string code = "")
        {
            DataSet ds = new DataSet();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;
            try
            {
                string sql = "";

                iDB2Command cmd = new iDB2Command();
                if (nameTambol.Trim() != "")
                {
                    sql = " SELECT gn19cd, gn19dt FROM gntb21, gntb19 " +
                          " WHERE gn21tm IN (SELECT gn18cd FROM gntb18 WHERE gn18dt = @tambol) " +
                          " AND   gn21am = gn19cd ORDER BY gn19dt ";

                    cmd.CommandText = sql;
                    cmd.Parameters.Add("@tambol", nameTambol.Trim());
                }
                else if (nameAmphur.Trim() != "")
                {
                    sql = " SELECT gn19cd, gn19dt FROM gntb19 " +
                          " WHERE gn19dt = @amphur ";
                    cmd.CommandText = sql;
                    cmd.Parameters.Add("@amphur", nameAmphur.Trim());
                }
                else if (code.Trim() != "")
                {
                    sql = " select gn19cd, gn19dt from gntb19 " +
                          " where gn19cd  = " + code;
                    cmd.CommandText = sql;
                }

                ds = RetriveAsDataSet(cmd);
                CloseConnectioDAL();
            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }
            return ds;
        }

        public DataSet getProvince(string tambol, string amphur, string code = "")
        {
            DataSet ds = new DataSet();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;
            try
            {
                iDB2Command cmd = new iDB2Command();
                string sql = "";
                if (tambol == "" && amphur != "")
                {
                    sql = " SELECT gn20cd, gn20dt, gn21zp, gn18cd, gn18dt " +
                          " FROM gntb21, gntb20, gntb18 " +
                          " WHERE gn21am in (SELECT gn19cd FROM gntb19 WHERE gn19dt = @amphur) " +
                          " AND gn21pr=gn20cd and gn21tm=gn18cd ";
                    cmd.CommandText = sql;
                    cmd.Parameters.Add("@amphur", amphur.Trim());

                }
                else if (tambol != "" && amphur != "")
                {
                    sql = " SELECT gn20cd, gn20dt, gn21zp, gn18cd, gn18dt " +
                          " FROM gntb21, gntb20, gntb18 " +
                          " WHERE gn21tm IN (SELECT gn18cd FROM gntb18 WHERE gn18dt = @tambol) " +
                          " AND gn21am IN (SELECT gn19cd FROM gntb19 WHERE gn19dt = @amphur) " +
                          " AND gn21pr=gn20cd AND gn21tm=gn18cd ";
                    cmd.CommandText = sql;
                    cmd.Parameters.Add("@tambol", tambol.Trim());
                    cmd.Parameters.Add("@amphur", amphur.Trim());
                }
                else if (code != "")
                {
                    sql = " select gn20cd, gn20dt from gntb20 where gn20cd = " + code;
                    cmd.CommandText = sql;
                }
                ds = RetriveAsDataSet(cmd);
                CloseConnectioDAL();
            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }
            return ds;
        }

        public DataSet getAddress(string desc)
        {
            DataSet ds = new DataSet();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;
            try
            {
                iDB2Command cmd = new iDB2Command();
                cmd.Parameters.Clear();

                string condition = "";

                if (desc.Trim() != "")
                {
                    //condition = String.Format(" WHERE gn18dt like '%" + desc + "%' or gn19dt like '%" + desc + "%' or gn20dt like '%" + desc + "%' or gn21zp like '%" + desc + "%' ");
                    condition = " WHERE gn18dt like @desc1 or gn19dt like @desc2 or gn20dt like @desc3 or gn21zp like @desc4 ";
                }

                cmd.CommandText = " SELECT TRIM(ifnull(gn18dt,''))||' - '|| " +
                                  " TRIM(ifnull(gn19dt,''))||' - '|| " +
                                  " TRIM(ifnull(gn20dt,''))||' - '|| " +
                                  " TRIM(gn21zp) as Address, " +
                                  " digits(gn21tm)||'-'||digits(gn21am)||'-'||digits(gn21pr)||'-'||gn21zp as Code " +
                                  " FROM gntb21 " +
                                  " LEFT JOIN gntb18 on(gn21tm = gn18cd) " +
                                  " LEFT JOIN gntb19 on(gn21am = gn19cd) " +
                                  " LEFT JOIN gntb20 on(gn21pr = gn20cd) " + condition;
                cmd.Parameters.Add("@desc1", '%' + desc + '%');
                cmd.Parameters.Add("@desc2", '%' + desc + '%');
                cmd.Parameters.Add("@desc3", '%' + desc + '%');
                cmd.Parameters.Add("@desc4", '%' + desc + '%');

                ds = RetriveAsDataSet(cmd);
                CloseConnectioDAL();
            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }
            return ds;
        }
        //*** get vendor ***//
        public DataSet getVendor(string vendor = "", string brn = "")
        {
            DataSet ds = new DataSet();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;


            try
            {
                string sql = "";
                if (vendor == "")
                {
                    sql = " SELECT distinct(digits(p10ven)) as p10ven, p10nam, p10fi1,p10grd,P16RNK,p10spc FROM ilms10 " +
                          " LEFT JOIN ilms16 ON (p10VEN=P16VEN and (" + m_UdpD.ToString() + " BETWEEN P16STD AND P16END )) " +
                          " WHERE p10del = '' AND " + m_UdpD.ToString().Trim() +
                          " BETWEEN p10fjd    AND p10edt " +
                          //" AND  (" + m_UdpD.ToString() + " BETWEEN P16STD AND P16END )" +
                          " AND exists (SELECT * FROM ilmd10 WHERE ilms10.p10ven=d10ven and d10apt='01') " +
                          " AND P10BRN = " + brn +
                          " ORDER BY p10nam, p10ven ";
                }
                else
                {
                    sql = " SELECT digits(p10ven) as p10ven, p10nam, p10fi1,p10grd,P16RNK,p10spc FROM ilms10 " +
                          " LEFT JOIN ilms16 ON (p10VEN=P16VEN and (" + m_UdpD.ToString() + " BETWEEN P16STD AND P16END )) " +
                          " WHERE p10del = '' AND " + m_UdpD.ToString().Trim() +
                          " BETWEEN p10fjd    AND p10edt " +
                          //" AND  (" + m_UdpD.ToString() + " BETWEEN P16STD AND P16END )" +
                          " AND p10ven = " + vendor +
                          " AND exists (SELECT * FROM ilmd10 WHERE ilms10.p10ven=d10ven and d10apt='01') " +
                          " AND P10BRN = " + brn +
                          " ORDER BY p10nam, p10ven ";
                }
                ds = RetriveAsDataSet(sql);
                CloseConnectioDAL();
            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }
            return ds;
        }

        // ***  get for check ncb ***//
        public DataSet getNCB(string appdate)
        {
            DataSet ds = new DataSet();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;
            try
            {
                string sql = " SELECT * FROM gntb100 " +
                             " WHERE g00app = 'IL' and G00CDE = 'NCB' " +
                             " AND g00efd <= " + appdate +
                             " ORDER BY G00EFD ";
                ds = RetriveAsDataSet(sql);
                CloseConnectioDAL();
            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }
            return ds;
        }

        public string GetCust_NCB(string s_appno, string s_brn)
        {
            DataSet ds = new DataSet();
            string s_cust_ncb = "";
            try
            {
                string sql = " SELECT substr(P1FILL, 21, 1) AS Cust_NCB" +
                             " FROM   ILMS01" +
                             " WHERE  (P1BRN = '" + s_brn + "') AND (P1APNO = '" + s_appno + "')";
                ds = RetriveAsDataSet(sql);
                CloseConnectioDAL();

                if (ds == null) { return ""; }
                if (ds.Tables[0].Rows[0]["Cust_NCB"] == null) { return ""; }

                s_cust_ncb = ds.Tables[0].Rows[0]["Cust_NCB"].ToString().Trim();
            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }
            return s_cust_ncb;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="brn"></param>
        /// <param name="vendorCode"></param>
        /// <returns></returns>
        public DataSet getCampaign(string vendorCode, string totalTerm, string campCode = "", string campSeq = "", string camRsq = "", string appdate = "")
        {
            DataSet ds = new DataSet();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;

            try
            {
                string sql = "";
                if (campCode == "")
                {
                    sql = " SELECT c01cmp, c01cnm,c01fin,c02ifr, c02csq,c02rsq ,C01CTY,C02AIR,C02ACR,c02tot,c02fmt " +
                                 " FROM ilcp08 " +
                                 " LEFT JOIN ilcp01 on (c08cmp=c01cmp and C01EDT >= " + appdate.Trim() + ") " +
                                 " LEFT JOIN ilcp02 ON (c08cmp=c02cmp) " +
                                 " LEFT JOIN ilcp06 ON (c08cmp=c06cmp) " +
                                 " LEFT JOIN ilms10 ON (c08ven=p10ven) " +
                                 " LEFT JOIN ilcp09 ON (c08cmp=c09cmp) " +
                                 " WHERE c09brn = " + m_UserInfo.BranchApp + " AND c01cst='A' " +
                                 " AND ((c08ven = " + vendorCode + ") OR (c08ven=0))  AND c08rst='' " +
                                 " AND c06apt='01' AND " + appdate.Trim() + " <= C01CAD AND c01cty='R' " +
                                 " and c02ttm = " + totalTerm + " and c02tot = " + totalTerm + " " +
                                 " AND not exists " +
                                 " (SELECT * FROM ilcp081 WHERE ilcp08.c08cmp = c81cmp and c81ven= " + vendorCode +
                                 " AND c81end <= " + appdate.Trim() + ") " +
                                 " ORDER BY c08cmp,c02csq,c02rsq ";
                }
                else if (campCode != "" && campSeq != "" && camRsq == "")
                {
                    sql = " SELECT c01nxd,c01fin,c01mkc,c02rsq,c02ttm,c02ifr,c02inr,c02crr,C01CTY,C02AIR,C02ACR, " +
                          " c02tot,c02fmt FROM ilcp02 " +
                          " LEFT JOIN ilcp01 ON c02cmp=c01cmp " +
                          " WHERE c02cmp = " + campCode +
                          " AND c02csq = " + campSeq;

                }
                else if (campCode != "" && campSeq != "" && camRsq != "")
                {
                    sql = " SELECT c01nxd,c01fin,c01mkc,c02rsq,c02ttm,c02ifr,c02inr,c02crr,C01CTY,C02AIR,C02ACR, " +
                          " c02tot,c02fmt FROM ilcp02 " +
                          " LEFT JOIN ilcp01 ON c02cmp=c01cmp " +
                          " WHERE c02cmp = " + campCode +
                          " AND c02csq = " + campSeq +
                          " AND C02RSQ = " + camRsq;

                }
                ds = RetriveAsDataSet(sql);
                CloseConnectioDAL();
            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }
            return ds;
        }


        public DataSet getILTB40(string desc)
        {
            DataSet ds = new DataSet();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;
            try
            {
                string sql = " SELECT * FROM ILTB40 LEFT JOIN  ILTB00 ON T40LTY=T00LTY WHERE t40del = '' ";
                string cond = "";
                if (desc != "")
                {
                    cond = " AND UPPER(T40TYP)  like '" + desc.ToUpper() + "%'  OR UPPER(T40DES) like '" + desc.ToUpper() + "%'";
                }

                sql += cond;
                ds = RetriveAsDataSet(sql);
                CloseConnectioDAL();
            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }
            return ds;
        }

        public DataSet getILTB41(string desc, string prodType)
        {
            DataSet ds = new DataSet();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;
            try
            {
                string sql = " SELECT * FROM ILTB41 WHERE T41TYP = " + prodType + "  AND  T41DEL =  '' ";
                string cond = "";
                if (desc != "")
                {
                    cond = " AND (UPPER(T41COD)  like '" + desc.ToUpper() + "%'  OR UPPER(T41DES) like '" + desc.ToUpper() + "%') ";
                }

                sql += cond;
                ds = RetriveAsDataSet(sql);
                CloseConnectioDAL();
            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }
            return ds;
        }

        public DataSet getILTB41_ALL(string desc, string code = "")
        {
            DataSet ds = new DataSet();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;
            try
            {
                string sql = " SELECT * FROM ILTB41 WHERE  T41DEL =  '' ";
                string cond = "";
                if (desc != "")
                {
                    cond = " AND (UPPER(T41COD)  like '" + desc.ToUpper() + "%'  OR UPPER(T41DES) like '" + desc.ToUpper() + "%') ";
                }

                if (desc == "" && code != "")
                {
                    cond = " AND T41COD = " + code;
                }


                sql += cond;
                ds = RetriveAsDataSet(sql);
                CloseConnectioDAL();
            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }
            return ds;
        }
        public DataSet getILTB42(string desc)
        {
            DataSet ds = new DataSet();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;
            try
            {
                string sql = " SELECT * FROM ILTB42 ";
                string cond = "";
                if (desc != "")
                {
                    cond = " WHERE  UPPER(T42BRD)  like '" + desc.ToUpper() + "%'  OR UPPER(T42DES) like '" + desc.ToUpper() + "%'";
                }

                sql += cond;

                ds = RetriveAsDataSet(sql);
                CloseConnectioDAL();
            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }
            return ds;
        }
        public DataSet getGNAT07()
        {
            DataSet ds = new DataSet();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;
            try
            {
                string sql = " SELECT * FROM gnat07 ";

                ds = RetriveAsDataSet(sql);
                CloseConnectioDAL();
            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }
            return ds;
        }



        public DataSet getILTB43(string desc, string prodType = "", string brand = "", string code = "", string modelcode = "")
        {
            DataSet ds = new DataSet();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;
            try
            {
                string sql = " SELECT * FROM ILTB43 WHERE  T43DEL = '' ";
                string cond = "";
                if (desc != "" && modelcode == "")
                {
                    cond = " AND T43MDL  like '%" + desc + "%'  OR UPPER(T43DES) like '%" + desc.ToUpper() + "%'";
                }
                else if (desc == "" && code != "" && modelcode != "" && brand != "" && prodType != "")
                {
                    cond = " AND T43MDL = " + modelcode +
                           " AND T43BRD = " + brand +
                           " AND T43COD = " + code +
                           " AND T43TYP = " + prodType;

                }

                sql = sql + cond;
                ds = RetriveAsDataSet(sql);
                CloseConnectioDAL();
            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }
            return ds;
        }

        public DataSet getProduct_(string vendorCode, string campCode, string campSeq, string productType = "", string productBrand = "", string productCode = "", string productModel = "")
        {
            DataSet ds = new DataSet();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;
            try
            {
                string sqlChk = " SELECT c07pit FROM ilcp07 " +
                                 " WHERE c07cmp = " + campCode +
                                 " AND c07csq = " + campSeq;
                DataSet dsChk = RetriveAsDataSet(sqlChk);
                CloseConnectioDAL();
                string all_product = "";
                if (check_dataset(dsChk))
                {
                    foreach (DataRow dr in dsChk.Tables[0].Rows)
                    {
                        if (dr["c07pit"].ToString().Trim() == "0")
                        {
                            all_product = "Y";
                            break;
                        }
                    }
                }
                string sql = "";
                string condition = "";

                if (productType != "")
                {
                    condition += " AND ((T40TYP) = '" + productType.ToUpper() + "' " +
                                " )";
                    //  " OR Upper(T40DES) like '%" + productType.ToUpper() + "%' ) ";
                }
                if (productBrand != "")
                {
                    condition += " AND ((T42BRD) = '" + productBrand.ToUpper() + "' " +
                                " )";
                    //   " OR Upper(T42DES) like '%" + productBrand.ToUpper() + "%' ) ";
                }
                if (productCode != "")
                {
                    condition += " AND ((T41COD) = '" + productCode.ToUpper() + "' " +
                                " )";
                    //  " OR Upper(T41DES) like '%" + productCode.ToUpper() + "%' ) ";
                }
                if (productModel != "")
                {
                    condition += " AND ((t44itm) = '" + productModel.ToUpper() + "' " +
                                " )";
                    //  " OR Upper(t44des) like '%" + productModel.ToUpper() + "%' ) ";
                }

                if (all_product == "Y")
                {
                    sql = " SELECT C07CMP,C07CSQ,C07PRC,C07DOW,c07pit,c07min,c07max,t44itm, t44des,t44cod,T44PGP,T44TYP,T44BRD,T44MDL from ilcp07 " +
                            " LEFT JOIN ilcp01 on (c07cmp=c01cmp) " +
                            " LEFT JOIN ilcp09 on (c09cmp=c01cmp) " +
                            " LEFT JOIN iltb44 on (1=1) " +
                            " LEFT JOIN iltb40 on (t44typ=t40typ) " +
                            " LEFT JOIN iltb42 on (t44brd=t42brd) " +
                            " LEFT JOIN iltb41 on (t44typ=t41typ and t44cod=t41cod) " +
                            " LEFT JOIN iltb43 on (t44typ=t43typ and t44brd=t43brd and t44cod=t43cod and t44mdl=t43mdl) " +
                            " Where c07cmp=" + campCode + " and c07csq=" + campSeq + " and t44del='' and c09BRN=001 and c01cst='A' " +
                            " and c07cmp=" + campCode + " and c07csq=" + campSeq + " and t44del='' " +
                            " and exists (select * from ilms13 where p13ven=" + vendorCode + " and t44typ = p13typ and p13del = '') " +
                            " and not exists (select * from ilcp04 where (c04cmp=ilcp07.c07cmp and c04pty=iltb44.t44typ and c04pcd=0) " +
                            " or c04cmp=ilcp07.c07cmp and c04pty=iltb44.t44typ and c04pcd=iltb44.t44cod) " + condition;
                }
                else
                {
                    sql = "  select C07CMP,C07CSQ,C07PRC,C07DOW,c07pit,c07min,c07max,t44itm, t44des,t44cod,T44PGP,T44TYP,T44BRD,T44MDL from ilcp07 " +
                            "  left join ilcp09 on (c09cmp=c07cmp) " +
                            "  left join ilcp04 on (c07cmp=c04cmp) " +
                            "  left join ilcp01 on (c07cmp=c01cmp) " +
                            "  left join iltb44 on (t44itm=c07pit) " +
                            "  left join iltb40 on (t44typ=t40typ) " +
                            "  left join iltb42 on (t44brd=t42brd) " +
                            "  left join iltb41 on (t44typ=t41typ and t44cod=t41cod) " +
                            "  left join iltb43 on (t44typ=t43typ and t44brd=t43brd and t44cod=t43cod and t44mdl=t43mdl) " +
                            "  Where c07cmp=" + campCode + " and c07csq= " + campSeq + " and t44del='' and c09BRN=001 and c01cst='A' " +
                            "  and (c04cmp is null or (t44typ <> c04pty and t44cod<>c04pcd)) " +
                            "  and exists (select * from ilms13 where p13ven=" + vendorCode + " and t44typ = p13typ and p13del = '') " +
                            condition +
                            "  order by c07cmp,c07csq ";
                }

                ds = RetriveAsDataSet(sql);
                CloseConnectioDAL();
            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }
            return ds;
        }
        public DataSet getProduct(string vendorCode, string campCode, string campSeq, string product = "")
        {
            DataSet ds = new DataSet();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;
            try
            {
                string sqlChk = " SELECT c07pit FROM ilcp07 " +
                                 " WHERE c07cmp = " + campCode +
                                 " AND c07csq = " + campSeq;
                DataSet dsChk = RetriveAsDataSet(sqlChk);
                CloseConnectioDAL();
                string all_product = "";
                if (check_dataset(dsChk))
                {
                    foreach (DataRow dr in dsChk.Tables[0].Rows)
                    {
                        if (dr["c07pit"].ToString().Trim() == "0")
                        {
                            all_product = "Y";
                            break;
                        }
                    }
                }
                string sql = "";
                string condition = "";
                if (product != "")
                {
                    condition = " AND (Upper(t44itm) like '%" + product.ToUpper() + "%' " +
                                " OR Upper(t44des) like '%" + product.ToUpper() + "%' ) ";
                }
                if (all_product == "Y")
                {
                    sql = " SELECT c07pit,t44itm, t44des,t44cod,c07min,c07max,T44PGP,t40des from ilcp07 " +
                            " LEFT JOIN ilcp01 on (c07cmp=c01cmp) " +
                            " LEFT JOIN ilcp09 on (c09cmp=c01cmp) " +
                            " LEFT JOIN iltb44 on (1=1) " +
                            " LEFT JOIN iltb40 on (t44typ=t40typ) " +
                            " LEFT JOIN iltb42 on (t44brd=t42brd) " +
                            " LEFT JOIN iltb41 on (t44typ=t41typ and t44cod=t41cod) " +
                            " LEFT JOIN iltb43 on (t44typ=t43typ and t44brd=t43brd and t44cod=t43cod and t44mdl=t43mdl) " +
                            " Where c07cmp=" + campCode + " and c07csq=" + campSeq + " and t44del='' and c09BRN=001 and c01cst='A' " +
                            " and c07cmp=" + campCode + " and c07csq=" + campSeq + " and t44del='' " +
                            " and exists (select * from ilms13 where p13ven=" + vendorCode + " and t44typ = p13typ and p13del = '') " +
                            " and not exists (select * from ilcp04 where (c04cmp=ilcp07.c07cmp and c04pty=iltb44.t44typ and c04pcd=0) " +
                            " or c04cmp=ilcp07.c07cmp and c04pty=iltb44.t44typ and c04pcd=iltb44.t44cod) " + condition;
                }
                else
                {
                    sql = "  select c07pit,t44itm, t44des,t44cod,c07min,c07max,T44PGP,t40des from ilcp07 " +
                            "  left join ilcp09 on (c09cmp=c07cmp) " +
                            "  left join ilcp04 on (c07cmp=c04cmp) " +
                            "  left join ilcp01 on (c07cmp=c01cmp) " +
                            "  left join iltb44 on (t44itm=c07pit) " +
                            "  left join iltb40 on (t44typ=t40typ) " +
                            "  left join iltb42 on (t44brd=t42brd) " +
                            "  left join iltb41 on (t44typ=t41typ and t44cod=t41cod) " +
                            "  left join iltb43 on (t44typ=t43typ and t44brd=t43brd and t44cod=t43cod and t44mdl=t43mdl) " +
                            "  Where c07cmp=" + campCode + " and c07csq= " + campSeq + " and t44del='' and c09BRN=001 and c01cst='A' " +
                            "  and (c04cmp is null or (t44typ <> c04pty and t44cod<>c04pcd)) " +
                            "  and exists (select * from ilms13 where p13ven=" + vendorCode + " and t44typ = p13typ and p13del = '') " +
                            condition +
                            "  order by c07cmp,c07csq ";
                }

                ds = RetriveAsDataSet(sql);
                CloseConnectioDAL();
            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }
            return ds;
        }
        //**  get rltb71 **//
        public DataSet getRLtb71(string code_Rcd, string code_cd1, string code_cd2 = "", string t71syr = "")
        {
            DataSet ds = new DataSet();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;
            try
            {
                string orderBy = "";
                string condition = "";
                string sql = " SELECT t71cd1,t71cd2, t71dst FROM rltb71 WHERE t71del= '' ";
                if (code_Rcd.Trim() != "")
                {
                    condition += " AND t71rcd = '" + code_Rcd + "' ";
                }
                if (code_cd1.Trim() != "")
                {
                    condition += " AND t71cd1 = '" + code_cd1 + "' ";
                }
                if (code_cd2.Trim() == "Y")
                {
                    condition += " AND t71cd2 <> '' ";
                    orderBy = "ORDER BY t71cd2  ";
                }
                else if (code_cd2.Trim() == "N")
                {
                    condition += " AND t71cd2 = '' ";
                    orderBy = "ORDER BY t71cd2  ";
                }
                if (t71syr.Trim() == "Y")
                {
                    condition += " AND t71syr = 'Y' ";
                    orderBy = "ORDER BY t71cd1  ";
                }

                sql += condition + orderBy;
                ds = RetriveAsDataSet(sql);
                CloseConnectioDAL();
            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }
            return ds;
        }

        //**  get rltb70 **//
        public DataSet getRLtb70(string code, string sub_code)
        {
            DataSet ds = new DataSet();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;
            try
            {

                string sql = " SELECT * FROM  RLTB70 WHERE T70HST = '" + code + "' " +
                             " AND T70SBC = '" + sub_code + "' " +
                             " AND T70DEL <> 'X' ";

                ds = RetriveAsDataSet(sql);
                CloseConnectioDAL();
            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }
            return ds;
        }

        //**  get ILTB01 **//
        public DataSet getILTB01(string code = "")
        {
            DataSet ds = new DataSet();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;
            try
            {
                string condition = "";
                string sql = @"SELECT T1BRN, T1BNME FROM ILTB01";
                if (code.Trim() != "")
                {
                    condition += " WHERE T1BRN = " + code;
                }

                sql += condition;
                ds = RetriveAsDataSet(sql);
            }
            catch (Exception ex)
            {
            }
            finally
            {
                CloseConnectioDAL();
            }

            return ds;
        }
        public DataSet getGNTB262()
        {
            DataSet ds = new DataSet();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;
            try
            {
                string sql = "SELECT * FROM GNTB262L1 WHERE G62SEL = 'Y' AND G62ILF = 'Y' ";
                ds = RetriveAsDataSet(sql);
            }
            catch (Exception ex)
            {
            }
            finally
            {
                CloseConnectioDAL();
            }
            return ds;

        }


        public DataSet getApplyType()
        {
            DataSet ds = new DataSet();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;
            try
            {
                string sql = " SELECT GN61CD, GN61DT FROM GNTB61L0 ORDER BY GN61CD ";
                ds = RetriveAsDataSet(sql);
            }
            catch (Exception ex)
            {
            }
            finally
            {
                CloseConnectioDAL();
            }
            return ds;
        }

        public DataSet getApplyVia()
        {
            DataSet ds = new DataSet();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;
            try
            {
                string sql = " SELECT GN16CD, GN16DT FROM GNTB16L4 ORDER BY GN16CD ";
                ds = RetriveAsDataSet(sql);
            }
            catch (Exception ex)
            {
            }
            finally
            {
                CloseConnectioDAL();
            }
            return ds;
        }

        public DataSet getApplyChannel()
        {
            DataSet ds = new DataSet();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;
            try
            {
                string sql = " SELECT GS16CD, GS16DT FROM GNTS16 ORDER BY GS16CD ";
                ds = RetriveAsDataSet(sql);
            }
            catch (Exception ex) { }
            finally
            {
                CloseConnectioDAL();
            }
            return ds;
        }

        public DataSet getGNTS18(string gs18sc = "")
        {
            DataSet ds = new DataSet();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;
            try
            {
                string condition = "";
                string sql = "";
                if (gs18sc.Trim() == "")
                {
                    sql = " SELECT gs18sc, gs18dt FROM gnts18 WHERE gs18dl = '' ORDER BY gs18sc ";
                }
                else
                {
                    sql = " SELECT * FROM gnts18 WHERE gs18sc = " + gs18sc + " AND gs18dl = '' ORDER BY gs18sc ";
                }
                ds = RetriveAsDataSet(sql);
                CloseConnectioDAL();
            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }
            return ds;
        }

        public DataSet getSlipDocType()
        {
            DataSet ds = new DataSet();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;
            try
            {
                string condition = "";
                string sql = " SELECT GS26CD, GS26DT, gs25de " +
                             " FROM gnts26 " +
                             " LEFT JOIN gnts25 ON(gs26sc=gs25cd) ";

                ds = RetriveAsDataSet(sql);
                CloseConnectioDAL();
            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }
            return ds;
        }

        public DataSet getILTB06()
        {
            DataSet ds = new DataSet();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;
            try
            {
                string condition = "";
                string sql = " SELECT * FROM ILTB06 ";
                ds = RetriveAsDataSet(sql);
                CloseConnectioDAL();
            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }
            return ds;
        }
        public DataSet getILMS10(string vendorCode)
        {
            DataSet ds = new DataSet();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;
            try
            {
                string sql = " SELECT * FROM ilms10 WHERE p10ven = " + vendorCode +
                             " AND p10del = ''  AND p10ats = 'Y' ";
                ds = RetriveAsDataSet(sql);
                CloseConnectioDAL();
            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }
            return ds;
        }



        public DataSet getGNMX01(string appdate)
        {
            DataSet ds = new DataSet();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;
            try
            {
                string sql = " SELECT * FROM gnmx01 " +
                             " WHERE mx1app = 'IL' " +
                             " AND " + appdate + ">= MX1STD " +
                             " AND " + appdate + "<= MX1END ";
                ds = RetriveAsDataSet(sql);
                CloseConnectioDAL();
            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }
            return ds;

        }
        public DataSet getGNTB67(bool tel, bool ext)
        {
            DataSet ds = new DataSet();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;
            try
            {
                string code = "";
                if (tel && !ext)
                {
                    code = "01";
                }
                else if (tel && ext)
                {
                    code = "02";
                }
                else if (!tel && !ext)
                {
                    code = "03";
                }

                string sql = "SELECT * FROM gntb67 WHERE GN67CD = '" + code + "'";
                ds = RetriveAsDataSet(sql);
                CloseConnectioDAL();
            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }
            return ds;
        }

        public DataSet getILTB09l1()
        {
            DataSet ds = new DataSet();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;
            try
            {
                string sql = "SELECT * FROM iltb09l1 WHERE T09CDE= '01'";

                ds = RetriveAsDataSet(sql);
                CloseConnectioDAL();
            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }
            return ds;

        }

        public DataSet getcsmh00(string csn)
        {
            DataSet ds = new DataSet();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;
            try
            {
                string sql = " SELECT h00sst, h00saj, h00cal FROM csmh00 WHERE h00csn = " + csn;

                ds = RetriveAsDataSet(sql);
                CloseConnectioDAL();
            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }
            return ds;

        }
        public DataSet getGNTB14()
        {
            DataSet ds = new DataSet();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;
            try
            {
                string sql = " select gn14cd,gn14td from gntb14 order by gn14cd with ur ";

                ds = RetriveAsDataSet(sql);
                CloseConnectioDAL();
            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }
            return ds;

        }

        //*** get sub Apply ***//
        public DataSet getSubChannel(string channel)
        {
            DataSet ds = new DataSet();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;
            try
            {
                string sql = " SELECT gs17sc, gs17dt FROM gnts17 WHERE gs17cd = '" + channel + "'  ORDER BY gs17sc";

                ds = RetriveAsDataSet(sql);
                CloseConnectioDAL();
            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }
            return ds;
        }

        public DataSet getGNMB20(string code)
        {
            DataSet ds = new DataSet();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;
            try
            {
                string sql = " SELECT gnb2tc,gnb2td, substr(gnb2fl,1,1)  FROM gnmb20  " +
                             " WHERE TRIM(GNB2FL) = '" + code + "'" +
                             " ORDER BY gnb2tc  ";
                ds = RetriveAsDataSet(sql);
                CloseConnectioDAL();
            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }
            return ds;
        }

        public DataSet getGNMB20_titleName(string code)
        {
            DataSet ds = new DataSet();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;
            try
            {
                string sql = " SELECT gnb2tc,gnb2td, substr(gnb2fl,2,1) fl_20 FROM gnmb20  " +
                             " WHERE GNB2TC = '" + code + "'" +
                             " ORDER BY gnb2tc  ";
                ds = RetriveAsDataSet(sql);
                CloseConnectioDAL();
            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }
            return ds;
        }
        public DataSet getILWK12(string appNo, string branch, string csn)
        {
            DataSet ds = new DataSet();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;
            try
            {
                DataSet ds_12 = new DataSet();
                //string sql = " SELECT   W12RSQ  Seq, W12REL Relation,W12NME Name,W12SNM SurName,  " +
                //             " substr(W12HTL,1,9) Tel_1,substr(W12HTL,11,4)  To_1, W12HEX Ext_1, " +
                //             " substr(W12OTL,1,9) Tel_2,substr(W12OTL,11,4)  To_2, W12OEX Ext_2, " +
                //             " W12MOB Mobile,W12VTE Ver " +
                //             " FROM ILWK12 where W12APN = " + appNo + " AND W12BRN = " + branch +
                //             " ORDER BY W12RSQ ";

                string sql = " SELECT   W12RSQ  Seq, W12REL Relation,W12NME Name,W12SNM SurName, " +
                             " substr(W12HTL,1,9) Tel_1,substr(W12HTL,11,4)  To_1, W12HEX Ext_1, " +
                             " substr(W12OTL,1,9) Tel_2,substr(W12OTL,11,4)  To_2, W12OEX Ext_2, " +
                             " W12MOB Mobile,W12VTE Ver,GN14TD Rel_DES " +
                             " FROM ILWK12 join gntb14  on  W12REL = GN14CD  where W12APN = " + appNo + " AND W12BRN = " + branch +
                             " ORDER BY W12RSQ ";

                ds_12 = RetriveAsDataSet(sql);

                if (check_dataset(ds_12))
                {
                    ds = ds_12;
                }
                else
                {
                    //sql = " SELECT m01seq Seq, m01rel Relation, m01tnm Name, m01tsn SurName, " +
                    //      " substr(m11tel,1,9) Tel_1,substr(m11tel,11,4)  To_1, m11ext Ext_1, " +
                    //      " substr(m11tl2,1,9) Tel_2,substr(m11tl2,11,4)  To_2, m11ex2 Ext_2, " +
                    //      " m11mob Mobile,'N' Ver " +
                    //      " FROM csms01 JOIN " +
                    //      " csms11  ON  (m01csn = m11csn and m01ref = m11ref and m01seq = m11rsq) " +
                    //      " WHERE m01csn = " + csn + " and m01ref = '1'";
                    sql = " SELECT Seq,Relation,Name,SurName,Tel_1,To_1,Ext_1,Tel_2,To_2,Ext_2,Mobile,Ver,GN14TD Rel_DES  " +
                           " FROM (SELECT m01seq Seq, m01rel Relation, m01tnm Name, m01tsn SurName,substr(m11tel,1,9) Tel_1,substr(m11tel,11,4)  To_1, m11ext Ext_1, " +
                           " substr(m11tl2,1,9) Tel_2,substr(m11tl2,11,4)  To_2, m11ex2 Ext_2, m11mob Mobile,'N' Ver " +
                           " FROM csms01 JOIN  csms11  ON  (m01csn = m11csn and m01ref = m11ref and m01seq = m11rsq) WHERE m01csn = " + csn + " AND  m01ref = '1'  )  a " +
                           " left  join gntb14  on  a.Relation = GN14CD   ";

                    ds = RetriveAsDataSet(sql);
                }


                CloseConnectioDAL();
            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }
            return ds;
        }

        public DataSet getCSMS00(string csn)
        {
            DataSet ds = new DataSet();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;
            try
            {
                string sql = " SELECT * FROM CSMS00 WHERE m00csn = " + csn;
                ds = RetriveAsDataSet(sql);
                CloseConnectioDAL();
                return ds;

            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
                return ds;
            }
        }

        public DataSet getCSMS11(string csn)
        {
            DataSet ds = new DataSet();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;
            try
            {
                string sql = " SELECT * FROM CSMS11 WHERE M11CSN = " + csn + " AND M11REF = '' ";
                ds = RetriveAsDataSet(sql);
                CloseConnectioDAL();
                return ds;

            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
                return ds;
            }
        }
        public DataSet getGNTB71()
        {
            DataSet ds = new DataSet();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;
            try
            {
                string sql = " SELECT gn71cd, gn71dt FROM gntb71 ";
                ds = RetriveAsDataSet(sql);
                CloseConnectioDAL();

            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }
            return ds;
        }

        public DataSet getGNTB70()
        {
            DataSet ds = new DataSet();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;
            try
            {
                string sql = " SELECT gn70cd, gn70dt FROM gntb70 ";
                ds = RetriveAsDataSet(sql);
                CloseConnectioDAL();

            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }
            return ds;
        }

        public DataSet getActionCode(string code = "")
        {
            DataSet ds = new DataSet();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;
            try
            {
                string condition = "";
                string sql = " SELECT G24ACD,G24DES FROM GNTB24 ";
                if (code.Trim() != "")
                {
                    condition = " WHERE G24ACD = '" + code.ToUpper() + "'";
                }
                sql += condition;
                ds = RetriveAsDataSet(sql);
                CloseConnectioDAL();
            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }
            return ds;
        }

        public DataSet getResultCode()
        {
            DataSet ds = new DataSet();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;
            try
            {
                string sql = " SELECT g25rcd, g25des FROM gntb25 ORDER BY g25rcd WITH UR ";

                ds = RetriveAsDataSet(sql);
                CloseConnectioDAL();
            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }
            return ds;
        }

        public DataSet getResultCodeWithCode(string ResultCode)
        {
            DataSet ds = new DataSet();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;
            try
            {
                string sql = " SELECT g25rcd, g25des FROM gntb25 WHERE G25RCD = '" + ResultCode + "'";
                ds = RetriveAsDataSet(sql);
                CloseConnectioDAL();
            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }
            return ds;
        }

        public DataSet getNote(string csn)
        {
            DataSet ds = new DataSet();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;
            try
            {
                if (csn.Trim() != "")
                {
                    //string sql = " SELECT SUBSTR(DIGITS(M38DAT),7,2)||'/'||SUBSTR(DIGITS(M38DAT),5,2)||'/'||SUBSTR(DIGITS(M38DAT),1,4) AS M38DAT_, " +
                    //             " SUBSTR(DIGITS(M38TIM),1,2)||':'||SUBSTR(DIGITS(M38TIM),3,2)||':'||SUBSTR(DIGITS(M38TIM),5,2) AS M38TIM, " +
                    //             " M38ACD ,M38RCD,M38USR,M38DES " +

                    //             " FROM csms38    " +
                    //             " WHERE M38CSN = " +csn+
                    //             " ORDER BY M38DAT DESC  WITH UR ";

                    string sql = " select substr(p38dat,7,2)||'/'||substr(p38dat,5,2)||'/'||substr(p38dat,1,4) as M38DAT_, " +
                                 " SUBSTR(DIGITS(P38TIM),1,2)||':'||SUBSTR(DIGITS(P38TIM),3,2)||':'||SUBSTR(DIGITS(P38TIM),5,2) AS M38TIM, " +
                                 " p38acd as M38ACD, p38rcd as M38RCD, p38usr as M38USR, p38de1||p38de2 as M38DES " +
                                 " FROM ilms38    " +
                                 " WHERE P38CSN = " + csn +
                                 " ORDER BY p38dat DESC  WITH UR ";

                    ds = RetriveAsDataSet(sql);
                    CloseConnectioDAL();
                }
            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }
            return ds;
        }




        public DataSet checkLockPending(string appNo, string brn)
        {
            DataSet ds = new DataSet();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;
            try
            {
                string sql = "";
                if (appNo.Trim() != "" && brn.Trim() != "")
                {
                    sql = " SELECT p1aprj, p1auth, substr(p1avdt,7,2)||'/'||substr(p1avdt,5,2)||'/'||substr(p1avdt,1,4) as p1avdt " +
                                 " FROM ilms01 WHERE p1apno = " + appNo + " AND p1brn = " + brn + " with ur ";
                }

                ds = RetriveAsDataSet(sql);
                CloseConnectioDAL();
            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }
            return ds;
        }
        public DataSet get_GNTB106(string provCode)
        {
            DataSet ds = new DataSet();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;
            try
            {
                string sql = " SELECT * FROM gntb106 WHERE g06cde = " + provCode + " AND g06del = '' with ur ";
                ds = RetriveAsDataSet(sql);
                CloseConnectioDAL();
            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }
            return ds;

        }

        public DataSet get_CSTB05()
        {
            DataSet ds = new DataSet();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;
            try
            {
                string mode = "";
                try
                {
                    mode = WebConfigurationManager.AppSettings["Mode"].ToString().Trim();
                }
                catch
                {
                    mode = "O";
                }
                string sql = " SELECT t05inc FROM cstb05 WHERE t05brn = " + m_userInfo.BranchApp +
                             " AND t05app = 'IL' " +
                             " AND t05ven = 0 " +
                             " AND t05opr = '" + mode + "'" + " WITH UR ";
                ds = RetriveAsDataSet(sql);
                CloseConnectioDAL();
            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }
            return ds;

        }
        public DataSet get_ilms01(string P1BRN, string P1APNO)
        {
            DataSet ds = new DataSet();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;
            try
            {

                string sql = " SELECT *  FROM  ILMS01  WHERE P1BRN = " + P1BRN +
                             " AND P1APNO = " + P1APNO;

                ds = RetriveAsDataSet(sql);
                CloseConnectioDAL();
            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }
            return ds;

        }
        public DataSet get_csms30(string csn)
        {
            DataSet ds = new DataSet();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;
            try
            {

                string sql = " SELECT *  FROM  CSMS30  WHERE m30csn = " + csn + " with ur";


                ds = RetriveAsDataSet(sql);
                CloseConnectioDAL();
            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }
            return ds;

        }




        public bool checkApproveCriteria(ILDataCenter ilObj_, string productCode, string vendorCode, string brn, string appNo, string appDate, string appType, string csn, string idno, string date_97, string program_name, bool nothave_th, string BizInit, string BranchNo, string username, string LocalClient, ref string t22cod, ref string t22seq, ref string sqlRet, string ver_contact = "")
        {

            try
            {
                sqlRet = "";
                string appType_con = "";
                if (appType == "01")
                {
                    appType_con = "t20at1 = '01' ";
                }
                else if (appType == "02")
                {
                    appType_con = "t20at2 = '02' ";
                }
                else if (appType == "03")
                {
                    appType_con = "t20at3 = '03' ";
                }
                else if (appType == "04")
                {
                    appType_con = "t20at4 = '04' ";
                }
                else if (appType == "05")
                {
                    appType_con = "t20at5 = '05' ";
                }

                DataSet ds_13 = new DataSet();
                //ILDataCenter ilObj_ = new ILDataCenter();
                //UserInfo m_userInfo_ = (UserInfo)HttpContext.Current.Session["UserInfo"];
                //UserInfomation = m_userInfo_;

                string sql = " SELECT m13bdt,m13mrt,m13sex,m13res,m13con,m13lyr*12+m13lmt as m13tre,m13but,m13occ, " +
                                 " m13pos,m13off,m13emp,m13wky*12+m13wkm as m13len,m13slt,m13net,m13htl,m13mtl,m13#rk,t44typ " +
                                 " FROM csms13 " +
                                 " LEFT JOIN iltb44 ON ( " + productCode + " = " + " t44itm) " +
                                 " WHERE m13app = 'IL' " +
                                 " AND m13brn = " + brn +
                                 " AND m13apn = " + appNo + " with ur ";
                ds_13 = ilObj_.RetriveAsDataSetNoConnect(sql);//ilObj_.RetriveAsDataSet(sql);
                                                              //ilObj_.CloseConnectioDAL();

                if (!ilObj_.check_dataset(ds_13))
                {
                    return true;
                }

                DataRow dr_13 = ds_13.Tables[0].Rows[0];
                //**  call GNP0371 **//
                string in_AGE = "", error = "";

                ilObj_.CALL_GNP0371(dr_13["m13bdt"].ToString().PadLeft(8, '0'), "", "YMD", "B", "", "IL", "",
                                    BizInit, BranchNo,
                                    ref in_AGE, ref error);
                //ilObj_.CloseConnectioDAL();

                if (in_AGE.Trim() == "")
                {
                    in_AGE = "0";
                }
                // ***  check criteria iltb21 ***//

                string sql_20 = " SELECT t20cod,t22ag1,t22ag2,t22ag3,t22mrt,t22sex,t22res,t22tof,t22tre,t22bus,t22occ,t22pos, " +
                              " t22toe,t22emt,t22len,t22tys,t22sa1,t22sa2,t22sa3,t22teh,t22mob,t22cod,t22seq,t20des,t22vto,t22rnk " +
                              " FROM iltb20 " +
                              " LEFT JOIN iltb21 on(t21cod=t20cod) " +
                              " LEFT JOIN iltb22 on(t22cod=t20cod) " +
                              " WHERE  t20std<= " + appDate + " AND " + " t20end >= " + appDate +
                              " AND " + appType_con +
                              " AND t20sts = 'A' AND t20del=  '' " +
                              " AND ((t21ven = " + vendorCode +
                              " AND (t21pty = " + dr_13["t44typ"].ToString() + " OR t21pty = 0 )  " +
                              " AND t21del = '' ) OR ( t21ven =  0)) " +
                              " AND t22del = '' with ur  ";
                //" AND t21ven = " + vendorCode +
                //" AND (t21pty = " + dr_13["t44typ"].ToString() + " OR t21pty = 0) AND " +
                //" t21del = '' AND t22del = '' with ur ";

                DataSet ds_20 = ilObj_.RetriveAsDataSetNoConnect(sql_20);
                //ilObj_.CloseConnectioDAL();
                if (!ilObj_.check_dataset(ds_20))
                {
                    return true;
                }

                foreach (DataRow dr_20 in ds_20.Tables[0].Rows)
                {
                    string condition = " WHERE t22cod = " + dr_20["t20cod"].ToString() + " AND t22del = '' ";

                    t22cod = dr_20["t22cod"].ToString();
                    t22seq = dr_20["t22seq"].ToString() + "(" + dr_20["t20des"].ToString().Trim() + ")";

                    if (dr_20["t22ag1"].ToString().Trim() != "0")
                    {
                        if (dr_20["t22ag2"].ToString().Trim() == "0")
                        {
                            condition += " AND " + int.Parse(in_AGE).ToString() + dr_20["t22ag1"].ToString();
                        }
                        else
                        {
                            condition += " AND ( " + int.Parse(in_AGE).ToString() + dr_20["t22ag1"].ToString() + dr_20["t22ag2"].ToString() +
                                         int.Parse(in_AGE).ToString() + dr_20["t22ag3"].ToString() + " )";
                        }
                    }
                    if (dr_20["t22mrt"].ToString().Trim() != "0")
                    {
                        condition += " AND " + dr_13["m13mrt"].ToString().Trim() + " IN (" + dr_20["t22mrt"].ToString().Trim() + " )";
                    }
                    if (dr_20["t22sex"].ToString().Trim() != "0")
                    {
                        condition += " AND '" + dr_13["m13sex"].ToString().Trim() + "' = '" + dr_20["t22sex"].ToString().Trim() + "' ";
                    }
                    if (dr_20["t22res"].ToString().Trim() != "0")
                    {
                        condition += " AND " + dr_13["m13res"].ToString().Trim() + " IN (" + dr_20["t22res"].ToString().Trim() + ") ";
                    }
                    if (dr_20["t22tof"].ToString().Trim() != "0")
                    {
                        condition += " AND " + dr_13["m13con"].ToString().Trim() + dr_20["t22tof"].ToString().Trim();
                    }
                    if (dr_20["t22tre"].ToString().Trim() != "0")
                    {
                        condition += " AND " + dr_13["m13tre"].ToString().Trim() + dr_20["t22tre"].ToString().Trim();
                    }
                    if (dr_20["t22bus"].ToString().Trim() != "0")
                    {
                        condition += " AND " + dr_13["m13but"].ToString().Trim() + " IN (" + dr_20["t22bus"].ToString().Trim() + " )";
                    }
                    if (dr_20["t22occ"].ToString().Trim() != "0")
                    {
                        condition += " AND " + dr_13["m13occ"].ToString().Trim() + " IN (" + dr_20["t22occ"].ToString().Trim() + " )";
                    }
                    if (dr_20["t22pos"].ToString().Trim() != "0")
                    {
                        condition += " AND " + dr_13["m13pos"].ToString().Trim() + " IN (" + dr_20["t22pos"].ToString().Trim() + " )";
                    }
                    if (dr_20["t22toe"].ToString().Trim() != "0")
                    {
                        condition += " AND " + dr_13["m13off"].ToString().Trim() + dr_20["t22toe"].ToString().Trim();
                    }
                    if (dr_20["t22emt"].ToString().Trim() != "0")
                    {
                        condition += " AND " + dr_13["t22emt"].ToString().Trim() + " IN (" + dr_20["t22emt"].ToString().Trim() + " )";
                    }
                    if (dr_20["t22len"].ToString().Trim() != "0")
                    {
                        condition += " AND " + dr_13["m13len"].ToString().Trim() + dr_20["t22len"].ToString().Trim();
                    }
                    if (dr_20["t22tys"].ToString().Trim() != "0")
                    {
                        condition += " AND " + dr_13["m13slt"].ToString().Trim() + " IN (" + dr_20["t22tys"].ToString().Trim() + " )";
                    }
                    if (dr_20["t22sa1"].ToString().Trim() != "0")
                    {
                        if (dr_20["t22sa2"].ToString().Trim() == "0")
                        {
                            condition += " AND " + dr_13["m13net"].ToString() + dr_20["t22sa1"].ToString();
                        }
                        else
                        {
                            condition += " AND ( " + dr_13["m13net"].ToString().Trim() +
                                         dr_20["t22sa1"].ToString() + " " +
                                         dr_20["t22sa2"].ToString() + " " +
                                         dr_13["m13net"].ToString().Trim() +
                                         dr_20["t22sa3"].ToString().Trim();
                        }
                    }
                    if (dr_20["t22teh"].ToString().Trim() != "0")
                    {
                        if (dr_20["t22teh"].ToString() == "Y")
                        {
                            condition += " AND " + dr_13["m13htl"].ToString() + "'<>'";
                        }
                        if (dr_20["t22teh"].ToString().Trim() == "N")
                        {
                            condition += " AND '" + dr_13["m13htl"].ToString().Trim() + "'='";
                        }
                    }
                    if (dr_20["t22mob"].ToString().Trim() != "0")
                    {
                        if (dr_20["t22mob"].ToString().Trim() == "Y")
                        {
                            condition += " AND " + dr_13["m13mtl"].ToString() + "'<>'";
                        }
                        if (dr_20["t22mob"].ToString().Trim() == "N")
                        {
                            condition += " AND " + dr_13["m13mtl"].ToString() + "'='";
                        }
                    }
                    if (appType == "01")
                    {
                        if (dr_20["t22vto"].ToString().Trim() != "0")
                        {
                            condition += " AND " + ver_contact + " IN (" + dr_20["t22vto"].ToString().Trim() + ") ";
                        }
                        if (dr_20["t22rnk"].ToString().Trim() != "0")
                        {
                            condition += " AND " + dr_13["m13#rk"].ToString() + dr_20["t22rnk"].ToString() + " ";
                        }
                    }
                    string sql_22 = " SELECT * FROM iltb22 " + condition;
                    DataSet ds_22 = ilObj_.RetriveAsDataSetNoConnect(sql_22);
                    //ilObj_.CloseConnectioDAL();
                    if (ilObj_.check_dataset(ds_22))
                    {
                        foreach (DataRow dr_22 in ds_22.Tables[0].Rows)
                        {
                            string sql_23 = " SELECT * FROM iltb23 " +
                                            " WHERE t23cod = " + dr_20["t22cod"].ToString() +
                                            " AND t23seq = " + dr_20["t22seq"].ToString() +
                                            " AND t23brn = " + brn +
                                            " AND t23apn = " + appNo +
                                            " AND t23csn = " + csn + " with ur ";

                            DataSet ds_23 = ilObj_.RetriveAsDataSetNoConnect(sql_23);
                            //ilObj_.CloseConnectioDAL();
                            if (!ilObj_.check_dataset(ds_23))
                            {
                                // ***  insert into iltb23  ***//
                                //iDB2Command cmd = new iDB2Command();
                                string sqlInsert = " INSERT INTO iltb23(t23cod,t23seq,t23brn,t23apn,t23csn,t23apt,t23upd,t23upt,t23upg,t23usr,t23uws) " +
                                                " VALUES (" +
                                                   dr_20["t22cod"].ToString() + "," +
                                                   dr_20["t22seq"].ToString() + "," +
                                                   brn + "," +
                                                   appNo + "," +
                                                   csn + "," +
                                                   "'01'," +
                                                   date_97 + "," +
                                                   m_UdpT.ToString() + "," +
                                                   "'" + program_name + "'," +
                                                   "'" + username + "'," +
                                                   "'" + LocalClient + "'" +
                                                   ")";
                                sqlRet = sqlInsert;
                                if (appType == "02")
                                {
                                    return false;
                                }

                            }

                            //***  for IL Normal ***//
                            if (appType == "01")
                            {
                                if (dr_20["t22cod"].ToString() == "570124001")
                                {
                                    string WPHSTS = "";
                                    string WPERR = "";
                                    string WPMSG = "";
                                    bool res_Call_GNSRCS = ilObj_.Call_GNSRCS(idno, "IL", brn, appNo, BizInit, BranchNo, ref WPHSTS, ref WPERR, ref WPMSG);
                                    //ilObj_.CloseConnectioDAL();
                                    if (res_Call_GNSRCS || WPERR.ToString() != "Y")
                                    {
                                        if (WPHSTS == "N")
                                        {
                                            if (nothave_th)
                                            {
                                                return false;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    return false;
                                }
                            }
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }




        public string insertILMS01(string p1brn, string p1apno, string p1ltyp, string p1appt, string p1apvs,
                                string p1apdt, string p1vdid, string p1mkid, string p1camp, string p1cmsq,
                                string p1item, string p1pdgp, string p1pric, string p1qty, string p1purc,
                                string p1vatr, string p1vata, string p1down, string p1term, string p1rang,
                                string p1ndue, string p1lndr, string p1dutr, string p1infr, string p1intr,
                                string p1crur, string p1pram, string p1inta, string p1crua, string p1infa,
                                string p1duty, string p1diff, string p1coam, string p1fdam, string p1frtm,
                                string p1frdt, string p1fram, string p1aprj, string p1stdt, string p1sttm,
                                string p1fdue, string p1csno, string p1loca, string p1crcd, string P1RESN,
                                string p1kusr, string p1kdte, string p1ktim, string p1avdt, string p1avtm,
                                string p1fill, string p1updt, string p1uptm, string p1upus, string p1prog,
                                string p1wsid, string p1rsts, string P1AUTH, string p1cont, string p1cndt,
                                string p1payt, string p1pbcd, string p1pbrn, string p1paty, string p1pano)
        {
            // int res = 0;
            string sql = "";
            try
            {
                sql = " INSERT INTO ilms01 ( " +
                             " p1brn , p1apno , p1ltyp , p1appt , p1apvs , " +
                             " p1apdt, p1vdid , p1mkid , p1camp , p1cmsq , " +
                             " p1item, p1pdgp , p1pric , p1qty  , p1purc , " +
                             " p1vatr, p1vata , p1down , p1term , p1rang , " +
                             " p1ndue, p1lndr , p1dutr , p1infr , p1intr , " +
                             " p1crur, p1pram , p1inta , p1crua , p1infa , " +
                             " p1duty, p1diff , p1coam , p1fdam , p1frtm , " +
                             " p1frdt, p1fram , p1aprj , p1stdt , p1sttm , " +
                             " p1fdue, p1csno , p1loca , p1crcd , P1RESN , " +
                             " p1kusr, p1kdte , p1ktim , p1avdt , p1avtm , " +
                             " p1fill, p1updt , p1uptm , p1upus , p1prog , " +
                             " p1wsid, p1rsts , P1AUTH , p1cont , p1cndt,p1payt , " +
                             " p1pbcd, p1pbrn , p1paty , p1pano ) VALUES ( " +
                               p1brn + "," +
                               p1apno + "," +
                              "'" + p1ltyp + "'," +//"'01'" + "," + 
                              "'" + p1appt + "'," + //"'02'" + "," + 
                              "'" + p1apvs + "'," +//"'1'"  + "," +
                               p1apdt + "," +
                               p1vdid + "," +
                               p1mkid + "," +
                               p1camp + "," +
                               p1cmsq + "," +
                               p1item + "," +
                               "'" + p1pdgp + "'," +
                               p1pric + "," +
                               p1qty + "," +
                               p1purc + "," +
                               p1vatr + "," +
                               p1vata + "," +
                               p1down + "," +
                               p1term + "," +
                               p1rang + "," +
                               p1ndue + "," +
                               p1lndr + "," +
                               p1dutr + "," +
                               p1infr + "," +
                               p1intr + "," +
                               p1crur + "," +
                               p1pram + "," +
                               p1inta + "," +
                               p1crua + "," +
                               p1infa + "," +
                               p1duty + "," +
                               p1diff + "," +
                               p1coam + "," +
                               p1fdam + "," +
                               p1frtm + "," +
                               p1frdt + "," +
                               p1fram + "," +
                               "'" + p1aprj + "'," +
                               p1stdt + "," +
                               p1sttm + "," +
                               p1fdue + "," +
                               p1csno + "," +
                               "'" + p1loca + "'," +
                               "'" + p1crcd + "'," +
                               "'" + P1RESN + "'," +
                               "'" + p1kusr + "'," +
                               p1kdte + "," +
                               p1ktim + "," +
                               p1avdt + "," +
                               p1avtm + "," +
                               "'" + p1fill + "'," +
                               p1updt + "," +
                               p1uptm + "," +
                               "'" + p1upus + "'," +
                               "'" + p1prog + "'," +
                               "'" + p1wsid + "'," +
                               "'" + p1rsts + "'," +
                               "'" + P1AUTH + "'," +
                               p1cont + "," +
                               p1cndt + "," +
                               "'" + p1payt + "'," +
                               "'" + p1pbcd + "'," +
                               "'" + p1pbrn + "'," +
                               "'" + p1paty + "'," +
                               "'" + p1pano + "'" +
                               " )";


                //iDB2Command cmd = new iDB2Command();
                //cmd.CommandText = sql;
                //res = ilobj.ExecuteNonQueryNoCommit(cmd);

            }
            catch (Exception ex)
            {

            }
            return sql;
        }



        public string insertILMS02(
                                string p2brn, string p2cont, string p2lnty, string p2csno, string p2apno,
                                string p2appt, string p2crcd, string p2atcd, string p2loca, string p2vdid,
                                string p2mkid, string p2camp, string p2cmsq, string p2cmct, string p2item,
                                string p2pric, string p2qty, string p2purc, string p2vatr, string p2vata,
                                string p2down, string p2term, string p2rang, string p2ndue, string p2dte1,
                                string p2cndt, string p2bkdt, string p2lndr, string p2dutr, string p2infr,
                                string p2intr, string p2crur, string p2toam, string p2osam, string p2pcam,
                                string p2pcbl, string p21due, string p2fdam, string p2diff, string p2difb,
                                string p2frtm, string p2frdt, string p2fram, string p2duty, string p2dutb,
                                string p2fee, string p2feeb, string p2ufeb, string p2feib, string p2crua,
                                string p2crub, string p2ucrb, string p2ucib, string p2uida, string p2intb,
                                string p2ubas, string p2uidb, string p2resn, string p2updt, string p2uptm,
                                string p2prog, string p2user, string p2ddsp
                               )
        {
            string sql = "";
            //int res = 0;
            try
            {

                sql = " INSERT INTO ilms02( " +
                             " p2brn,p2cont,p2lnty,p2csno,p2apno,  " +
                             " p2appt,p2crcd,p2atcd,p2loca,p2vdid, " +
                             " p2mkid,p2camp,p2cmsq,p2cmct,p2item, " +
                             " p2pric,p2qty,p2purc,p2vatr,p2vata,  " +
                             " p2down,p2term,p2rang,p2ndue,p2dte1, " +
                             " p2cndt,p2bkdt,p2lndr,p2dutr,p2infr, " +
                             " p2intr,p2crur,p2toam,p2osam,p2pcam, " +
                             " p2pcbl,p21due,p2fdam,p2diff,p2difb, " +
                             " p2frtm,p2frdt,p2fram,p2duty,p2dutb, " +
                             " p2fee,p2feeb,p2ufeb,p2feib,p2crua,  " +
                             " p2crub,p2ucrb,p2ucib,p2uida,p2intb, " +
                             " p2ubas,p2uidb,p2resn,p2updt,p2uptm, " +
                             " p2prog,p2user,p2ddsp ) " +
                             " VALUES ( " +
                               p2brn + "," +
                               p2cont + "," +
                               "'02'" + "," +
                               p2csno + "," +
                               p2apno + "," +
                               "'02'" + "," +
                               "'" + p2crcd + "'" + "," +
                               "'" + p2atcd + "'" + "," +
                               "'250'" + "," +
                               p2vdid + "," +
                               p2mkid + "," +
                               p2camp + "," +
                               p2cmsq + "," +
                               "'R'" + "," +
                               p2item + "," +
                               p2pric + "," +
                               p2qty + "," +
                               p2purc + "," +
                               p2vatr + "," +
                               p2vata + "," +
                               p2down + "," +
                               p2term + "," +
                               p2rang + "," +
                               p2ndue + "," +
                               "2" + "," +
                               p2cndt + "," +
                               p2bkdt + "," +
                               p2lndr + "," +
                               p2dutr + "," +
                               p2infr + "," +
                               p2intr + "," +
                               p2crur + "," +
                               p2toam + "," +
                               p2osam + "," +
                               p2pcam + "," +
                               p2pcbl + "," +
                               p21due + "," +
                               p2fdam + "," +
                               p2diff + "," +
                               p2difb + "," +
                               p2frtm + "," +
                               p2frdt + "," +
                               p2fram + "," +
                               p2duty + "," +
                               p2dutb + "," +
                               p2fee + "," +
                               p2feeb + "," +
                               p2ufeb + "," +
                               p2feib + "," +
                               p2crua + "," +
                               p2crub + "," +
                               p2ucrb + "," +
                               p2ucib + "," +
                               p2uida + "," +
                               p2intb + "," +
                               p2ubas + "," +
                               p2uidb + "," +
                               "'" + p2resn + "'," +
                               p2updt + "," +
                               p2uptm + "," +
                               "'" + p2prog + "'," +
                               "'" + p2user + "'," +
                               "'" + p2ddsp + "'" +
                               ")";

                //iDB2Command cmd = new iDB2Command();
                //cmd.CommandText = sql;
                //res = ilObj.ExecuteNonQueryNoCommit(cmd);

                //return res;
            }
            catch (Exception ex)
            {
                // return res;
            }
            return sql;
        }

        public string InsertILMD012(string d012br, string d012ap, string d012sq, string d012lt, string d012ct, string d012tt
                               , string d012fm, string d012to, string d012ir, string d012cr, string d012fr
                               , string d012pa, string d012ia, string d012ca, string d012fa, string d012in
                               , string d012df, string d012ud, string d012ut, string d012us, string d012pg
                               , string d012ws)
        {
            //int res = 0;
            string sql = "";
            try
            {
                sql = " INSERT INTO ilmd012 ( " +
                            " d012br,d012ap,d012sq,d012lt,d012ct,d012tt, " +
                            " d012fm,d012to,d012ir,d012cr,d012fr, " +
                            " d012pa,d012ia,d012ca,d012fa,d012in, " +
                            " d012df,d012ud,d012ut,d012us,d012pg, " +
                            " d012ws)  VALUES ( " +
                            d012br + "," +
                            d012ap + "," +
                            d012sq + "," +
                            "'" + d012lt + "'," +
                            d012ct + "," +
                            d012tt + "," +
                            d012fm + "," +
                            d012to + "," +
                            d012ir + "," +
                            d012cr + "," +
                            d012fr + "," +
                            d012pa + "," +
                            d012ia + "," +
                            d012ca + "," +
                            d012fa + "," +
                            d012in + "," +
                            d012df + "," +
                            d012ud + "," +
                            d012ut + "," +
                            "'" + d012us + "'," +
                            "'" + d012pg + "'," +
                            "'" + d012ws + "'" +
                            ")";

                //iDB2Command cmd = new iDB2Command();
                //cmd.CommandText = sql;
                //res = ilObj.ExecuteNonQueryNoCommit(cmd);
                //return res;

            }
            catch (Exception ex)
            {
                //return res;
            }
            return sql;
        }

        public string InsertCSMS13(
                                string m13app, string m13csn, string m13brn, string m13apn, string m13apt
                               , string m13apv, string m13sex, string m13mrt, string m13smt, string m13but
                               , string m13occ, string m13pos, string m13off, string m13lna, string m13res
                               , string m13con, string m13ttl, string m13htl, string m13hex, string m13mtl
                               , string m13wky, string m13wkm, string m13slt, string m13net, string m13cmp
                               , string m13csq, string m13trm, string m13tcl, string m13tca, string m13gol
                               , string m13bdt, string m13chl, string m13hzp, string m13htm, string m13ham
                               , string m13hpv, string m13lyr, string m13lmt, string m13fdt, string m13mob
                               , string m131id, string m13gno, string m13acl, string m13bot, string m13cbl
                               , string M13_RK, string M13_GN, string m13_ac, string m13cru, string m13cud
                               , string M13SAD, string M13SAT, string M13OSL, string M13OSD, string M13OST
                               , string M13EMP, string M13FIL, string m13sst, string m13saj, string m13cal
                               , string m13upg, string m13udt, string m13utm, string m13usr, string m13wks
                               , string m13aus, string m13aud
            )
        {
            //int res = 0;
            string sql = "";
            try
            {
                sql = " INSERT INTO csms13 ( " +
                             " m13app,m13csn,m13brn,m13apn,m13apt, " +
                             " m13apv,m13sex,m13mrt,m13smt,m13but, " +
                             " m13occ,m13pos,m13off,m13lna,m13res, " +
                             " m13con,m13ttl,m13htl,m13hex,m13mtl, " +
                             " m13wky,m13wkm,m13slt,m13net,m13cmp, " +
                             " m13csq,m13trm,m13tcl,m13tca,m13gol, " +
                             " m13bdt,m13chl,m13hzp,m13htm,m13ham, " +
                             " m13hpv,m13lyr,m13lmt,m13fdt,m13mob, " +
                             " m131id,m13gno,m13acl,m13bot,m13cbl, " +
                             " M13#RK,M13#GN,m13#ac,m13cru,m13cud, " +
                             " M13SAD,M13SAT,M13OSL,M13OSD,M13OST, " +
                             " M13EMP,M13FIL,m13sst,m13saj,m13cal, " +
                             " m13upg,m13udt,m13utm, m13usr,m13wks, " +
                             " m13aus,m13aud) VALUES (" +
                             "'" + m13app + "'," +
                             "'" + m13csn + "'," +
                             m13brn + "," +
                             m13apn + "," +
                             "'" + m13apt + "'," +
                             "'" + m13apv + "'," +
                             "'" + m13sex + "'," +
                             "'" + m13mrt + "'," +
                             "'" + m13smt + "'," +
                             "'" + m13but + "'," +
                             "'" + m13occ + "'," +
                             "'" + m13pos + "'," +
                             "'" + m13off + "'," +
                             m13lna + "," +
                             "'" + m13res + "'," +
                             m13con + "," +
                             "'" + m13ttl + "'," +
                             "'" + m13htl + "'," +
                             "@m13hex ," +
                             //"'" + m13hex + "'," +
                             "'" + m13mtl + "'," +
                             m13wky + "," +
                             m13wkm + "," +
                             "'" + m13slt + "'," +
                             m13net + "," +
                             "'" + m13cmp + "'," +
                             m13csq + "," +
                             m13trm + "," +
                             m13tcl + "," +
                             m13tca + "," +
                             m13gol + "," +
                             m13bdt + "," +
                             m13chl + "," +
                             m13hzp + "," +
                             m13htm + "," +
                             m13ham + "," +
                             m13hpv + "," +
                             m13lyr + "," +
                             m13lmt + "," +
                             m13fdt + "," +
                             "'" + m13mob + "'," +
                             m131id + "," +
                             m13gno + "," +
                             m13acl + "," +
                             m13bot + "," +
                             m13cbl + "," +
                             M13_RK + "," +
                             M13_GN + "," +
                             m13_ac + "," +
                             "'" + m13cru + "'," +
                             m13cud + "," +
                             M13SAD + "," +
                             M13SAT + "," +
                             M13OSL + "," +
                             M13OSD + "," +
                             M13OST + "," +
                             "'" + M13EMP + "'," +
                             "'" + M13FIL + "'," +
                             "'" + m13sst + "'," +
                             m13saj + "," +
                             "'" + m13cal + "'," +
                             "'" + m13upg + "'," +
                             m13udt + "," +
                             m13utm + "," +
                             "'" + m13usr + "'," +
                             "'" + m13wks + "'," +
                             "'" + m13aus + "'," +
                             m13aud + "" +
                             " ) ";

                //iDB2Command cmd = new iDB2Command();
                //cmd.CommandText = sql;
                //res = ilObj.ExecuteNonQueryNoCommit(cmd);
                //return res;

            }
            catch (Exception ex)
            {
                //return res;
            }
            return sql;
        }
        public string INSERT_CSMS13_JUDG(string m13app, string m13csn, string m13brn, string m13apn, string m13apt, string m13apv, string m13cha, string m13sch, string m13bdt, string m13sex, string m13mrt, string m13smt, string m13but, string m13occ, string m13pos, string
                        m13off, string m13res, string m13con, string m13ttl, string m13htl, string m13hex, string m13mtl, string m13wky, string m13wkm, string m13slt, string m13sld, string m13net, string m13cmp, string m13csq, string m13trm, string
                        m13tcl, string m13tca, string m13gol, string m13chl, string m13hzp, string m13htm, string m13ham, string m13hpv, string m13ozp, string m13otm, string m13oam, string m13opv, string m13lyr, string m13lmt, string m13fdt, string
                        m13mob, string m13emp, string m13lna, string m13pbl, string m13fil, string m131id, string m13gno, string m13acl, string m13bot, string m13cbl, string m13_rk, string m13_gn, string m13_ac, string m13cru, string m13cud, string
                        m13aus, string m13aud, string m13rtl, string m13sad, string m13sat, string m13osl, string m13osd, string m13ost, string M13SST, string M13SAJ, string M13CAL, string M13DOC, string m13upg, string m13udt, string m13utm, string
                        m13usr, string m13wks, string M13OTL, string M13OEX, string M13CMP, string M13CSQ, string M13TRM, string m13izp, string m13itm, string m13iam, string m13ipv
            )
        {
            string sql = "";
            try
            {
                sql = " INSERT INTO csms13 ( " +
                      " m13app,m13csn,m13brn,m13apn,m13apt,m13apv,m13cha,m13sch,m13bdt,m13sex,m13mrt,m13smt,m13but,m13occ,m13pos," +
                      " m13off,m13res,m13con,m13ttl,m13htl,m13hex,m13mtl,m13wky,m13wkm,m13slt,m13sld,m13net,m13cmp,m13csq,m13trm," +
                      " m13tcl,m13tca,m13gol,m13chl,m13hzp,m13htm,m13ham,m13hpv,m13ozp,m13otm,m13oam,m13opv,m13lyr,m13lmt,m13fdt," +
                      " m13mob,m13emp,m13lna,m13pbl,m13fil,m131id,m13gno,m13acl,m13bot,m13cbl,m13#rk,m13#gn,m13#ac,m13cru,m13cud," +
                      " m13aus,m13aud,m13rtl,m13sad,m13sat,m13osl,m13osd,m13ost,M13SST,M13SAJ,M13CAL,M13DOC,m13upg,m13udt,m13utm,m13usr," +
                      " m13wks,m13OTL,M13OEX,M13CMP,M13CSQ,M13TRM,m13izp,m13itm,m13iam,m13ipv ) VALUES (" +
                       "'" + m13app + "',"
                          + m13csn + ","
                          + m13brn + ","
                          + m13apn + ","
                       + "'" + m13apt + "',"
                       + "'" + m13apv + "',"
                       + "'" + m13cha + "',"
                       + "'" + m13sch + "',"
                       + "'" + m13bdt + "',"
                       + "'" + m13sex + "',"
                       + "'" + m13mrt + "',"
                       + "'" + m13smt + "',"
                       + "'" + m13but + "',"
                       + "'" + m13occ + "',"
                       + "'" + m13pos + "',"
                       + "'" + m13off + "',"
                       + "'" + m13res + "',"
                          + m13con + ","
                       + "'" + m13ttl + "',"
                       + "'" + m13htl + "',"
                       + " @m13hex  ,"
                       //+"'"+ m13hex + "',"
                       + "'" + m13mtl + "',"
                          + m13wky + ","
                          + m13wkm + ","
                       + "'" + m13slt + "',"
                       + "'" + m13sld + "',"
                          + m13net + ","
                       + "'" + m13cmp + "',"
                          + m13csq + ","
                          + m13trm + ","
                          + m13tcl + ","
                          + m13tca + ","
                          + m13gol + ","
                          + m13chl + ","
                          + m13hzp + ","
                          + m13htm + ","
                          + m13ham + ","
                          + m13hpv + ","
                          + m13ozp + ","
                          + m13otm + ","
                          + m13oam + ","
                          + m13opv + ","
                          + m13lyr + ","
                          + m13lmt + ","
                          + m13fdt + ","
                    + "'" + m13mob + "',"
                          + m13emp + ","
                          + m13lna + ","
                          + m13pbl + ","
                    + "'" + m13fil + "',"
                          + m131id + ","
                          + m13gno + ","
                          + m13acl + ","
                          + m13bot + ","
                          + m13cbl + ","
                          + m13_rk + ","
                          + m13_gn + ","
                          + m13_ac + ","
                    + "'" + m13cru + "',"
                          + m13cud + ","
                    + "'" + m13aus + "',"
                          + m13aud + ","
                    + "'" + m13rtl + "',"
                          + m13sad + ","
                          + m13sat + ","
                          + m13osl + ","
                          + m13osd + ","
                          + m13ost + ","
                    + "'" + M13SST + "',"
                          + M13SAJ + ","
                    + "'" + M13CAL + "',"
                    + "'" + M13DOC + "',"
                    + "'" + m13upg + "',"
                          + m13udt + ","
                          + m13utm + ","
                    + "'" + m13usr + "',"
                    + "'" + m13wks + "',"
                    + "'" + M13OTL + "',"
                    + "   @M13OEX   ,"
                    + "'" + M13CMP + "',"
                    + M13CSQ + ","
                    + M13TRM + ""
                     + m13izp + ","
                          + m13itm + ","
                          + m13iam + ","
                          + m13ipv + "," +
                         ")";
            }
            catch (Exception ex)
            {
            }
            return sql;
        }

        public string UPDATE_CSMS13(string m13apt, string m13apv, string m13cha, string m13sch, string m13bdt, string m13sex,
                                    string m13mrt, string m13smt, string m13but, string m13occ, string m13pos, string m13off,
                                    string m13res, string m13con, string m13ttl, string m13htl, string m13hex, string m13mtl,
                                    string m13wky, string m13wkm, string m13slt, string m13sld, string m13net, string m13cmp,
                                    string m13csq, string m13trm, string m13tcl, string m13tca, string m13gol, string m13chl,
                                    string m13hzp, string m13htm, string m13ham, string m13hpv, string m13ozp, string m13otm,
                                    string m13oam, string m13opv, string m13lyr, string m13lmt, string m13fdt, string m13mob,
                                    string m13emp, string m13lna, string m13pbl, string M13FIL, string m131id, string m13gno,
                                    string m13acl, string m13bot, string m13cbl, string m13_rk, string m13_gn, string m13_ac,
                                    string M13RTL, string m13sad, string m13sat, string m13osl, string m13osd, string m13ost,
                                    string M13SST, string M13SAJ, string M13CAL, string M13DOC, string m13upg, string m13udt,
                                    string m13utm, string m13usr, string m13wks, string m13brn, string m13app, string m13apn,
                                    string m13aus, string m13aud, string m13cru, string m13cud, string M13OTL, string M13OEX
                                    )
        {
            string sql = "";
            try
            {
                sql = " UPDATE csms13 SET " +
                      " m13apt = '" + m13apt + "'," +
                      " m13apv = '" + m13apv + "'," +
                      " m13cha = '" + m13cha + "'," +
                      " m13sch = '" + m13sch + "'," +
                      " m13bdt = '" + m13bdt + "'," +
                      " m13sex = '" + m13sex + "'," +
                      " m13mrt = '" + m13mrt + "'," +
                      " m13smt = '" + m13smt + "'," +
                      " m13but = '" + m13but + "'," +
                      " m13occ = '" + m13occ + "'," +
                      " m13pos = '" + m13pos + "'," +
                      " m13off = '" + m13off.PadLeft(5, '0') + "'," +
                      " m13res = '" + m13res + "'," +
                      " m13con = " + m13con + "," +
                      " m13ttl = '" + m13ttl + "'," +
                      " m13htl = '" + m13htl + "'," +
                      " m13hex =  @m13hex  ," +
                      //" m13hex = '"+m13hex+"',"+
                      " m13mtl = '" + m13mtl + "'," +
                      " m13wky = " + m13wky + "," +
                      " m13wkm = " + m13wkm + "," +
                      " m13slt = '" + m13slt + "'," +
                      " m13sld = '" + m13sld + "'," +
                      " m13net = " + m13net + "," +
                      " m13cmp = '" + m13cmp + "'," +
                      " m13csq = " + m13csq + "," +
                      " m13trm = " + m13trm + "," +
                      " m13tcl = " + m13tcl + "," +
                      " m13tca = " + m13tca + "," +
                      " m13gol = " + m13gol + "," +
                      " m13chl = " + m13chl + "," +
                      " m13hzp = " + m13hzp + "," +
                      " m13htm = " + m13htm + "," +
                      " m13ham = " + m13ham + "," +
                      " m13hpv = " + m13hpv + "," +
                      " m13ozp = " + m13ozp + "," +
                      " m13otm = " + m13otm + "," +
                      " m13oam = " + m13oam + "," +
                      " m13opv = " + m13opv + "," +
                      " m13lyr = " + m13lyr + "," +
                      " m13lmt = " + m13lmt + "," +
                      " m13fdt = " + m13fdt + "," +
                      " m13mob = '" + m13mob + " '," +
                      " m13emp = '" + m13emp + " '," +
                      " m13lna = " + m13lna + "," +
                      " m13pbl = " + m13pbl + "," +
                      " M13FIL = " + M13FIL + "," +
                      " m131id = " + m131id + "," +
                      " m13gno = " + m13gno + "," +
                      " m13acl = " + m13acl + " ," +
                      " m13bot = " + m13bot + " ," +
                      " m13cbl = " + m13cbl + " ," +
                      " m13#rk = " + m13_rk + " ," +
                      " m13#gn = " + m13_gn + " ," +
                      " m13#ac = " + m13_ac + " ," +
                      " M13RTL = '" + M13RTL + "'," +
                      " m13sad = " + m13sad + " ," +
                      " m13sat = " + m13sat + " ," +
                      " m13osl = " + m13osl + " ," +
                      " m13osd = " + m13osd + " ," +
                      " m13ost = " + m13ost + " ," +
                      " M13SST = '" + M13SST + "'," +
                      " M13SAJ = " + M13SAJ + " ," +
                      " M13CAL = '" + M13CAL + "'," +
                      " M13DOC = '" + M13DOC + "'," +
                      " m13upg = '" + m13upg + "'," +
                      " m13udt = " + m13udt + "," +
                      " m13utm = " + m13utm + "," +
                      " m13usr = '" + m13usr + "'," +
                      " m13wks = '" + m13wks + "'," +
                      " m13aus = " + m13aus + "," +
                      " m13aud = " + m13aud + "," +
                      " m13cru = " + m13cru + "," +
                      " m13cud = " + m13cud + "," +
                      " M13OTL = '" + M13OTL + "'," +
                      " M13OEX =  @M13OEX  " +
                      //" M13OEX = '"+M13OEX+"'"+
                      " WHERE m13brn = " + m13brn +
                      " AND   m13app = '" + m13app + "'" +
                      " AND   m13apn = " + m13apn;


            }
            catch (Exception ex)
            {
            }
            return sql;
        }
        public string UPDATE_CSMS13POSTID(string m13brn, string m13app, string m13apn, string m13izp, string m13itm, string m13iam, string m13ipv, string m13rkf)
        {
            string sql = "";
            try
            {
                sql = " UPDATE csms13 SET " +
                      " m13izp = " + m13izp + "," +
                      " m13itm = " + m13itm + "," +
                      " m13iam = " + m13iam + "," +
                      " m13ipv = " + m13ipv + "," +
                      " m13rkf = '" + m13rkf + "'" +
                      " WHERE m13brn = " + m13brn +
                      " AND   m13app = '" + m13app + "'" +
                      " AND   m13apn = " + m13apn;
            }
            catch (Exception ex)
            {

            }
            return sql;
        }
        public string UPDATE_CSMS13SEFTDEC(string m13brn, string m13app, string m13apn, string m13sfd)
        {
            string sql = "";
            try
            {
                sql = " UPDATE csms13 SET " +
                      " m13sfd = '" + m13sfd + "'" +
                      " WHERE m13brn = " + m13brn +
                      " AND   m13app = '" + m13app + "'" +
                      " AND   m13apn = " + m13apn;
            }
            catch (Exception ex)
            {

            }
            return sql;
        }
        public string UPDATE_MS13CRU(string m13brn, string m13apn)
        {
            string sql = "";
            try
            {
                sql = " UPDATE CSMS13 SET M13CRU = '' " +
                       " WHERE m13brn = " + m13brn +
                       " AND   m13apn = " + m13apn;
            }
            catch (Exception ex)
            {

            }
            return sql;
        }

        public string UPDATE_MS13CRRW_MD(string m13brn, string m13apn, string crrw, string crmd)
        {
            string sql = "";
            try
            {
                if (crrw == "")
                {
                    sql = "Update CSMS13 set " +
                                       "M13FIL = substr(M13FIL,1,28) || '" + crmd + "' || substr(M13FIL, 30, LENGTH(M13FIL)) " +
                                       "where M13APP='IL' and m13brn = " + m13brn + " AND   m13apn = " + m13apn;
                }
                else
                {
                    sql = "Update CSMS13 set " +
                                       "M13FIL = substr(M13FIL,1,27) || '" + crrw + crmd + "' || substr(M13FIL, 30, LENGTH(M13FIL)) " +
                                       "where M13APP='IL' and m13brn = " + m13brn + " AND   m13apn = " + m13apn;
                }
            }
            catch (Exception ex)
            {

            }
            return sql;
        }

        public string INSERT_CSMS13HS(string brn, string AppCode, string appNo)
        {
            string sql = "";
            try
            {
                sql = " INSERT INTO csms13hs (SELECT * FROM csms13 " +
                      " WHERE m13brn = " + brn +
                      " AND m13app = '" + AppCode + "'" +
                      " AND m13apn = " + appNo + ")";
            }
            catch (Exception ex)
            {
            }
            return sql;
        }

        public string INSERT_ILMS86(
                                string P86RST, string P86BRN, string P86CON, string P86PRI, string P86ACT, string P86RUN, string P86AMT, string P86BAL,
                                string P86GDT, string P86DUE, string P86RDT, string P86FIL, string P86PGM, string P86USR, string P86UDT, string P86UTM
                                )
        {
            string sql = "";
            //int res = 0;
            try
            {
                sql = " INSERT INTO ilms86 ( " +
                         " P86RST,P86BRN,P86CON,P86PRI,P86ACT,P86RUN,P86AMT,P86BAL, " +
                         " P86GDT,P86DUE,P86RDT,P86FIL,P86PGM,P86USR,P86UDT,P86UTM  " +
                         ")  VALUES ( " +
                          "'" + P86RST + "'," +
                          P86BRN + "," +
                          P86CON + "," +
                          P86PRI + "," +
                          "'" + P86ACT + "'," +
                          P86RUN + "," +
                          P86AMT + "," +
                          P86BAL + "," +
                          P86GDT + "," +
                          P86DUE + "," +
                          P86RDT + "," +
                         "'" + P86FIL + "'," +
                         "'" + P86PGM + "'," +
                         "'" + P86USR + "'," +
                          P86UDT + "," +
                          P86UTM +
                         ")";

                //iDB2Command cmd = new iDB2Command();
                //cmd.CommandText = sql;
                //res = ilObj.ExecuteNonQueryNoCommit(cmd);
                //return res;

            }
            catch (Exception ex)
            {
                //return res;
            }
            return sql;
        }






        public string INSERT_ILWK05(string w5csno, string w5brn, string w5apno, string w5csty, string w5sbcd,
                                    string w5cupd, string w5cupt, string w5cusr, string w5cwrk, string w5inet,
                                    string w5crtd, string w5crtt, string w5updt, string w5uptm, string w5user,
                                    string w5prgm, string w5wrks, string w5ktdl, string w5kcrd, string w5kupd,
                                    string w5kupt, string w5kusr, string w5kwrk, string w5kacd, string w5kaud, string w5kaut, string w5kaus, string w5kauw
                                    )
        {
            string sql = "";
            try
            {
                sql = " INSERT INTO ilwk05 (w5csno, w5brn, w5apno, w5csty, w5sbcd, " +
                      " w5cupd, w5cupt, w5cusr, w5cwrk, w5inet, w5crtd, w5crtt, " +
                      " w5updt, w5uptm, w5user, w5prgm, w5wrks,  w5ktdl, w5kcrd,w5kupd, w5kupt, w5kusr,w5kwrk,w5kacd, w5kaud,w5kaut, w5kaus,w5kauw ) values ( " +
                        w5csno + "," +
                        w5brn + "," +
                        w5apno + "," +
                        "'" + w5csty + "'," +
                        "'" + w5sbcd + "'," +
                        w5cupd + "," +
                        w5cupt + "," +
                        "'" + w5cusr + "'," +
                        "'" + w5cwrk + "'," +
                        w5inet + "," +
                        w5crtd + "," +
                        w5crtt + "," +
                        w5updt + "," +
                        w5uptm + "," +
                        "'" + w5user + "'," +
                        "'" + w5prgm + "'," +
                        "'" + w5wrks + "'," +
                        "'" + w5ktdl + "'," +
                        "'" + w5kcrd + "'," +
                        w5kupd + "," +
                        w5kupt + "," +
                        "'" + w5kusr + "'," +
                        "'" + w5kwrk + "'," +
                        "'" + w5kacd + "'," +
                        w5kaud + "," +
                        w5kaut + "," +
                        "'" + w5kaus + "'," +
                        "'" + w5kauw + "'" +
                        " )";
            }
            catch (Exception ex)
            {

            }
            return sql;
        }
        public string UPDATE_ILWK05_Cust_INT(string w5csty, string w5sbcd, string w5cupd, string w5cupt,
                                             string w5cusr, string w5cwrk, string w5crtd, string w5crtt,
                                             string w5updt, string w5uptm, string w5user, string w5prgm,
                                             string w5wrks, string w5brn, string w5apno,
                                            string w5ktdl, string w5kcrd, string w5kupd, string w5kupt, string w5kusr, string w5kwrk, string w5kacd, string w5kaud, string w5kaut, string w5kaus, string w5kauw)
        {
            string sql = "";
            try
            {
                string condition = "";
                if (w5brn.Trim() != "" && w5apno.Trim() != "")
                {
                    condition = " w5fvto = 'Y', w5fvth = 'Y', w5fvtm = 'Y', w5fvte = 'Y' ";
                    if (w5csty == "S" || w5csty == "Z")
                    {
                        condition = " w5fvto = '' , w5fvth = '', w5fvtm = '', w5fvte = '' ";
                    }
                    else if (w5csty == "O")
                    {
                        if (w5sbcd == "03")
                        {
                            condition = " w5fvto = '' , w5fvth = '', w5fvtm = 'Y', w5fvte = 'Y' ";
                        }
                        else if (w5sbcd == "04")
                        {
                            condition = " w5fvto = 'Y' , w5fvth = '', w5fvtm = 'Y', w5fvte = 'Y' ";
                        }
                        else if (w5sbcd == "05")
                        {
                            condition = " w5fvto = '' , w5fvth = 'Y', w5fvtm = 'Y', w5fvte = 'Y' ";
                        }
                    }


                    sql = " UPDATE ilwk05 SET " + condition + "," +
                          " w5csty =  '" + w5csty + "'," +
                          " w5sbcd =  '" + w5sbcd + "'," +
                          " w5cupd =  " + w5cupd + "," +
                          " w5cupt =  " + w5cupt + "," +
                          " w5cusr =  '" + w5cusr + "'," +
                          " w5cwrk =  '" + w5cwrk + "'," +
                          " w5crtd =  " + w5crtd + "," +
                          " w5crtt =  " + w5crtt + "," +
                          " w5updt =  " + w5updt + "," +
                          " w5uptm =  " + w5uptm + "," +
                          " w5user =  '" + w5user + "'," +
                          " w5prgm =  '" + w5prgm + "'," +
                          " w5wrks =  '" + w5wrks + "', " +
                          " w5ktdl = '" + w5ktdl + "'," +
                          " w5kcrd = '" + w5kcrd + "'," +
                          " w5kupd = " + w5kupd + "," +
                          " w5kupt = " + w5kupt + "," +
                          " w5kusr = '" + w5kusr + "'," +
                          " w5kwrk = '" + w5kwrk + "'," +
                          " w5kacd = '" + w5kacd + "'" +
                          " WHERE w5brn = " + w5brn + " AND w5apno = " + w5apno;
                }
            }
            catch (Exception ex)
            {

            }

            return sql;
        }


        public string UPDATE_ILWK05_Cust_KESSAI(string w5caud, string w5caut, string w5caus,
                                         string w5cauw, string w5updt, string w5uptm,
                                         string w5user, string w5prgm,
                                         string w5wrks, string w5brn, string w5apno,
                                          string w5ktdl, string w5kcrd, string w5kupd, string w5kupt, string w5kusr, string w5kwrk, string w5kacd, string w5kaud, string w5kaut, string w5kaus, string w5kauw)
        {
            string sql = "";
            try
            {
                if (w5brn.Trim() != "" && w5apno.Trim() != "")
                {
                    sql = " UPDATE ilwk05 SET " +
                          " w5caud =  " + w5caud + "," +
                          " w5caut =  " + w5caut + "," +
                          " w5caus =  '" + w5caus + "'," +
                          " w5cauw =  '" + w5cauw + "'," +
                          " w5updt =  " + w5updt + "," +
                          " w5uptm =  " + w5uptm + "," +
                          " w5user =  '" + w5user + "'," +
                          " w5prgm =  '" + w5prgm + "'," +
                          " w5wrks =  '" + w5wrks + "', " +
                          " w5ktdl = '" + w5ktdl + "'," +
                          " w5kcrd = '" + w5kcrd + "'," +
                          " w5kacd = '" + w5kacd + "'," +
                          " w5kaud = " + w5kaud + "," +
                          " w5kaut = " + w5kaut + "," +
                          " w5kaus = '" + w5kaus + "'," +
                          " w5kauw = '" + w5kauw + "'" +
                          " WHERE w5brn = " + w5brn + " AND w5apno = " + w5apno;
                }
            }
            catch (Exception ex)
            {

            }

            return sql;
        }

        //******** TO ********//
        public string UPDATE_ILWK05_TO_INT(string w5vrto, string w5fvto, string w5salt, string w5inet,
                                           string w5sso, string w5bol, string w5hotl, string w5hoex,
                                           string w5wotl, string w5woex, string w5motl, string w5voot,
                                           string w5vonm, string w5tonm, string w5voad, string w5toad,
                                           string w5emst, string w5rvto, string w5ipto, string w5tito,
                                           string w5tyem, string w5letn, string w5letp, string w5letd,
                                           string w5oupd, string w5oupt, string w5ousr, string w5owrk,
                                           string w5oaud, string w5oaut, string w5oaus, string w5oauw,
                                           string w5updt, string w5uptm, string w5user, string w5prgm,
                                           string w5wrks, string w5brn, string w5apno)
        {
            string sql = "";

            try
            {

                sql = " UPDATE ilwk05 SET " +
                                   " w5vrto = '" + w5vrto + "'," +
                                   " w5fvto = '" + w5fvto + "'," +
                                   " w5salt = '" + w5salt + "'," +
                                   " w5inet = " + w5inet + "," +
                                   " w5sso  = '" + w5sso + "'," +
                                   " w5bol  = '" + w5bol + "'," +
                                   " w5hotl = '" + w5hotl + "'," +
                                   " w5hoex =   @w5hoex  , " +
                                   //" w5hoex = '" + w5hoex + "', " +
                                   " w5wotl = '" + w5wotl + "', " +
                                   " w5woex =  @w5woex  , " +
                                   //" w5woex = '" + w5woex + "', " +
                                   " w5motl = '" + w5motl + "'," +
                                   " w5voot = '" + w5voot + "'," +
                                   " w5vonm = '" + w5vonm + "'," +
                                   " w5tonm = @w5tonm," +
                                   " w5voad = '" + w5voad + "'," +
                                   " w5toad = @w5toad," +
                                   " w5emst = '" + w5emst + "'," +
                                   " w5rvto = '" + w5rvto + "'," +
                                   " w5ipto = '" + w5ipto + "'," +
                                   " w5tito = @w5tito," +
                                   " w5tyem = '" + w5tyem + "', " +
                                   " w5letn = @w5letn," +
                                   " w5letp = @w5letp, " +
                                   " w5letd = @w5letd, " +
                                   " w5oupd = " + w5oupd + ", " +
                                   " w5oupt = " + w5oupt + ", " +
                                   " w5ousr = '" + w5ousr + "'," +
                                   " w5owrk = '" + w5owrk + "', " +
                                   " w5oaud = " + w5oaud + "," +
                                   " w5oaut = " + w5oaut + "," +
                                   " w5oaus = '" + w5oaus + "'," +
                                   " w5oauw = '" + w5oauw + "'," +
                                   " w5updt = " + w5updt + ", " +
                                   " w5uptm = " + w5uptm + ", " +
                                   " w5user = '" + w5user + "', " +
                                   " w5prgm = '" + w5prgm + "'," +
                                   " w5wrks = '" + w5wrks + "' " +
                                   " WHERE w5brn = " + w5brn + " and w5apno = " + w5apno;




            }
            catch (Exception ex)
            {
            }

            return sql;
        }

        public string UPDATE_ILWK05_TO_KESSAI(string w5vrto, string w5oaud, string w5oaut,
                                       string w5oaus, string w5oauw, string w5updt,
                                       string w5uptm, string w5user, string w5prgm,
                                       string w5wrks, string w5brn, string w5apno)
        {
            string sql = "";
            try
            {
                if (w5brn.Trim() != "" && w5apno.Trim() != "")
                {
                    sql = " UPDATE ilwk05 SET " +
                          " w5vrto =  '" + w5vrto + "'," +
                          " w5oaud =  " + w5oaud + "," +
                          " w5oaut =  " + w5oaut + "," +
                          " w5oaus =  '" + w5oaus + "'," +
                          " w5oauw =  '" + w5oauw + "'," +
                          " w5updt =  " + w5updt + "," +
                          " w5uptm =  " + w5uptm + "," +
                          " w5user =  '" + w5user + "'," +
                          " w5prgm =  '" + w5prgm + "'," +
                          " w5wrks =  '" + w5wrks + "' " +
                          " WHERE w5brn = " + w5brn + " AND w5apno = " + w5apno;
                }
            }
            catch (Exception ex)
            {

            }

            return sql;
        }

        //******** TH ********//
        public string UPDATE_ILWK05_TH_INT(string w5vrto, string w5fvth, string w5vrth, string w5htl,
                                           string w5htex, string w5vhht, string w5vhnm, string w5thnm,
                                           string w5vhad, string w5thad, string w5tytl, string w5rvth,
                                           string w5ipth, string w5tith, string w5hupd, string w5hupt,
                                           string w5husr, string w5hwrk, string w5haud, string w5haut,
                                           string w5haus, string w5hauw, string w5updt, string w5uptm,
                                           string w5user, string w5prgm, string w5wrks, string w5brn, string w5apno)
        {
            //int res = -1;
            string sql = "";
            try
            {

                if (w5brn != "" && w5apno != "")
                {
                    //iDB2Command cmd = new iDB2Command();
                    //cmd.Parameters.Clear();
                    //cmd.CommandText =
                    sql = " update ilwk05 set " +
                                         " w5vrto = '" + w5vrto + "'," +
                                         " w5fvth = '" + w5fvth + "'," +
                                         " w5vrth = '" + w5vrth + "'," +
                                         " w5htl  = '" + w5htl + "', " +
                                         " w5htex =  @w5htex   , " +
                                         //" w5htex = '"+w5htex+"', "+
                                         " w5vhht = '" + w5vhht + "', " +
                                         " w5vhnm = '" + w5vhnm + "', " +
                                         " w5thnm = @w5thnm, " +
                                         " w5vhad = '" + w5vhad + "', " +
                                         " w5thad = @w5thad, " +
                                         " w5tytl = '" + w5tytl + "', " +
                                         " w5rvth = '" + w5rvth + "', " +
                                         " w5ipth = '" + w5ipth + "', " +
                                         " w5tith = @w5tith, " +
                                         " w5hupd =  " + w5hupd + ", " +
                                         " w5hupt =  " + w5hupt + ", " +
                                         " w5husr = '" + w5husr + "', " +
                                         " w5hwrk = '" + w5hwrk + "', " +
                                         " w5haud = " + w5haud + ",  " +
                                         " w5haut = " + w5haut + ",  " +
                                         " w5haus = '" + w5haus + "', " +
                                         " w5hauw = '" + w5hauw + "', " +
                                         " w5updt = " + w5updt + ", " +
                                         " w5uptm = " + w5uptm + ", " +
                                         " w5user = '" + w5user + "', " +
                                         " w5prgm = '" + w5prgm + "', " +
                                         " w5wrks = '" + w5wrks + "' " +
                                         " WHERE w5brn = " + w5brn + " AND w5apno = " + w5apno;

                    //cmd.Parameters.Add("@w5thnm", w5thnm);
                    //cmd.Parameters.Add("@w5thad", w5thad);
                    //cmd.Parameters.Add("@w5tith", w5tith);
                    //res = ilObj.ExecuteNonQuery(cmd);
                }

            }
            catch (Exception ex)
            {
            }

            return sql;
        }

        public string UPDATE_ILWK05_TH_KESSAI(string w5vrto, string w5haud, string w5haut, string w5haus,
                                       string w5hauw, string w5updt, string w5uptm, string w5user,
                                       string w5prgm, string w5wrks, string w5brn, string w5apno)
        {
            string sql = "";
            try
            {
                if (w5brn.Trim() != "" && w5apno.Trim() != "")
                {
                    sql = " UPDATE ilwk05 SET " +
                          " w5vrto = '" + w5vrto + "'," +
                          " w5haud = " + w5haud + "," +
                          " w5haut = " + w5haut + "," +
                          " w5haus = '" + w5haus + "'," +
                          " w5hauw = '" + w5hauw + "'," +
                          " w5updt =  " + w5updt + "," +
                          " w5uptm =  " + w5uptm + "," +
                          " w5user =  '" + w5user + "'," +
                          " w5prgm =  '" + w5prgm + "'," +
                          " w5wrks =  '" + w5wrks + "' " +
                          " WHERE w5brn = " + w5brn + " AND w5apno = " + w5apno;
                }
            }
            catch (Exception ex)
            {

            }
            return sql;
        }


        //******** TM ********//
        public string UPDATE_ILWK05_TM_INT(string w5vrto, string w5fvtm, string w5vrtm, string w5mbtl, string w5vmtl,
                                            string w5mupd, string w5mupt, string w5musr, string w5mwrk, string w5maud,
                                            string w5maut, string w5maus, string w5mauw, string w5updt, string w5uptm,
                                            string w5user, string w5prgm, string w5wrks, string w5brn, string w5apno)
        {
            string sql = "";
            try
            {
                if (w5brn != "" && w5apno != "")
                {
                    sql = " update ilwk05 set " +
                          " w5vrto = '" + w5vrto + "'," +
                          " w5fvtm = '" + w5fvtm + "', " +
                          " w5vrtm = '" + w5vrtm + "', " +
                          " w5mbtl = '" + w5mbtl + "', " +
                          " w5vmtl = '" + w5vmtl + "', " +
                          " w5mupd = " + w5mupd + ", " +
                          " w5mupt = " + w5mupt + ", " +
                          " w5musr = '" + w5musr + "', " +
                          " w5mwrk = '" + w5mwrk + "', " +
                          " w5maud = " + w5maud + ",  " +
                          " w5maut = " + w5maut + ",  " +
                          " w5maus = '" + w5maus + "'," +
                          " w5mauw = '" + w5mauw + "'," +
                          " w5updt = " + w5updt + ", " +
                          " w5uptm = " + w5uptm + ", " +
                          " w5user = '" + w5user + "', " +
                          " w5prgm = '" + w5prgm + "', " +
                          " w5wrks = '" + w5wrks + "' " +
                          " WHERE  w5brn = " + w5brn + " and w5apno = " + w5apno;
                }

            }
            catch (Exception ex)
            {
            }

            return sql;
        }

        public string UPDATE_ILWK05_TM_KESSAI(string w5vrto, string w5maud, string w5maut, string w5maus,
                                       string w5mauw, string w5updt, string w5uptm, string w5user,
                                       string w5prgm, string w5wrks, string w5brn, string w5apno)
        {
            string sql = "";
            try
            {
                if (w5brn.Trim() != "" && w5apno.Trim() != "")
                {
                    sql = " UPDATE ilwk05 SET " +
                          " w5vrto = '" + w5vrto + "'," +
                          " w5maud = " + w5maud + "," +
                          " w5maut = " + w5maut + "," +
                          " w5maus = '" + w5maus + "'," +
                          " w5mauw = '" + w5mauw + "'," +
                          " w5updt =  " + w5updt + "," +
                          " w5uptm =  " + w5uptm + "," +
                          " w5user =  '" + w5user + "'," +
                          " w5prgm =  '" + w5prgm + "'," +
                          " w5wrks =  '" + w5wrks + "' " +
                          " WHERE w5brn = " + w5brn + " AND w5apno = " + w5apno;
                }


            }
            catch (Exception ex)
            {

            }
            return sql;
        }


        //******** TE ********//
        public string UPDATE_ILWK05_TE_INT(string w5vrto, string w5fvte, string w5eupd, string w5eupt,
                                           string w5eusr, string w5ewrk, string w5eaud, string w5eaut, string w5eaus,
                                           string w5eauw, string w5updt, string w5uptm, string w5user,
                                           string w5prgm, string w5wrks, string w5brn, string w5apno)
        {
            string sql = "";
            try
            {

                if (w5brn.Trim() != "" && w5apno.Trim() != "")
                {
                    sql = " UPDATE ilwk05 SET " +
                          " w5vrto = '" + w5vrto + "'," +
                          " w5fvte = '" + w5fvte + "'," +
                          " w5eupd = " + w5eupd + "," +
                          " w5eupt = " + w5eupt + "," +
                          " w5eusr = '" + w5eusr + "'," +
                          " w5ewrk = '" + w5ewrk + "'," +
                          " w5eaud = " + w5eaud + "," +
                          " w5eaut = " + w5eaut + "," +
                          " w5eaus = '" + w5eaus + "'," +
                          " w5eauw = '" + w5eauw + "'," +
                          " w5updt =  " + w5updt + "," +
                          " w5uptm =  " + w5uptm + "," +
                          " w5user = '" + w5user + "'," +
                          " w5prgm = '" + w5prgm + "'," +
                          " w5wrks = '" + w5wrks + "' " +
                          " WHERE w5brn = " + w5brn + " AND w5apno = " + w5apno;
                }


            }
            catch (Exception ex)
            {

            }
            return sql;
        }

        public string UPDATE_ILWK05_TE_KESSAI(string w5vrto, string w5eaud, string w5eaut, string w5eaus,
                                       string w5eauw, string w5updt, string w5uptm, string w5user,
                                       string w5prgm, string w5wrks, string w5brn, string w5apno)
        {
            string sql = "";
            try
            {
                if (w5brn.Trim() != "" && w5apno.Trim() != "")
                {
                    sql = " UPDATE ilwk05 SET " +
                          " w5vrto = '" + w5vrto + "'," +
                          " w5eaud = " + w5eaud + "," +
                          " w5eaut = " + w5eaut + "," +
                          " w5eaus = '" + w5eaus + "'," +
                          " w5eauw = '" + w5eauw + "'," +
                          " w5updt =  " + w5updt + "," +
                          " w5uptm =  " + w5uptm + "," +
                          " w5user =  '" + w5user + "'," +
                          " w5prgm =  '" + w5prgm + "'," +
                          " w5wrks =  '" + w5wrks + "' " +
                          " WHERE w5brn = " + w5brn + " AND w5apno = " + w5apno;
                }


            }
            catch (Exception ex)
            {

            }
            return sql;
        }

        public string INSERT_ILWK12(string W12CSN, string W12BRN, string W12APN, string W12RSQ, string W12REL, string W12TTL,
                                     string W12NME, string W12SNM, string W12HTL, string W12HEX, string W12OTL, string W12OEX,
                                     string W12MOB, string W12VTE, string W12UDT, string W12UTM, string W12UUS, string W12UPG, string W12UWS)
        {
            string sql = "";
            try
            {
                sql = " INSERT INTO ilwk12 ( " +
                      " W12CSN,W12BRN,W12APN,W12RSQ,W12REL,W12TTL, " +
                      " W12NME,W12SNM,W12HTL,W12HEX,W12OTL,W12OEX, " +
                      " W12MOB,W12VTE,W12UDT,W12UTM,W12UUS,W12UPG,W12UWS ) VALUES (" +
                        W12CSN + "," +
                        W12BRN + "," +
                        W12APN + "," +
                        W12RSQ + "," +
                      "'" + W12REL + "'," +
                      "'" + W12TTL + "'," +
                      "@W12NME," +
                      "@W12SNM," +
                      "'" + W12HTL + "'," +
                      "@W12HEX ," +
                      //"'" + W12HEX + "'," +
                      "'" + W12OTL + "'," +
                      "@W12OEX ," +
                      //"'" + W12OEX + "'," +
                      "'" + W12MOB + "'," +
                      "'" + W12VTE + "'," +
                        W12UDT + "," +
                        W12UTM + "," +
                      "'" + W12UUS + "'," +
                      "'" + W12UPG + "'," +
                      "'" + W12UWS + "')";
            }
            catch (Exception ex)
            {

            }
            return sql;
        }
        public string INSERT_ILWK05HS(string brn, string appNo, string userName, string wrkStn)
        {
            string sql = "";
            try
            {
                if (brn.Trim() != "" && appNo.Trim() != "")
                {
                    sql = " INSERT INTO ilwk05hs  (SELECT 'C', " + m_UdpD.ToString() + "," + m_UdpT.ToString() + "," +
                          "'" + userName + "','" + wrkStn + "'," + "W5CSNO,W5BRN,W5APNO,W5CSTY,W5SBCD,W5FVTO,W5FVTH,W5FVTM," +
                          " W5FVTE,W5CCRD,W5CUPD,W5CUPT,W5CUSR,W5CWRK,W5CACD,W5CAUD,W5CAUT,W5CAUS,W5CAUW,W5VRTO,W5OFC,W5SALT,W5INET, " +
                          " W5SSO,W5BOL,W5HOTL,W5HOEX,W5WOTL,W5WOEX,W5MOTL,W5VOOT,W5VONM,W5TONM,W5VOAD,W5TOAD,W5EMST,W5RVTO,W5IPTO, " +
                          " W5TITO,W5TYEM,W5LETN,W5LETP,W5LETD,W5OCRD,W5OUPD,W5OUPT,W5OUSR,W5OWRK,W5OACD,W5OAUD,W5OAUT,W5OAUS,W5OAUW," +
                          " W5VRTH,W5HTL,W5HTEX,W5VHHT,W5VHNM,W5THNM,W5VHAD,W5THAD,W5TYTL,W5RVTH,W5TVTH,W5IPTH,W5TITH,W5HCRD,W5HUPD," +
                          " W5HUPT,W5HUSR,W5HWRK,W5HACD,W5HAUD,W5HAUT,W5HAUS,W5HAUW,W5VRTM,W5MBTL,W5VMTL,W5MCRD,W5MUPD,W5MUPT,W5MUSR," +
                          " W5MWRK,W5MACD,W5MAUD,W5MAUT,W5MAUS,W5MAUW,W5VRTE,W5ECRD,W5EUPD,W5EUPT,W5EUSR,W5EWRK,W5EACD,W5EAUD, " +
                          " W5EAUT,W5EAUS,W5EAUW,W5FIL,W5UPDT,W5UPTM,W5USER,W5PRGM,W5WRKS , W5KTDL, W5KCRD,W5KUPD, W5KUPT, W5KUSR,W5KWRK,W5KACD, W5KAUD,W5KAUT, W5KAUS,W5KAUW  " +
                          " FROM ilwk05 WHERE w5brn = " + brn + " AND w5apno = " + appNo + " )";
                }

            }
            catch (Exception ex)
            {

            }
            return sql;

        }

        public string INSERT_ILWK12HS(string brn, string appNo, string userName, string wrkStn)
        {
            string sql = "";
            try
            {
                if (brn.Trim() != "" && appNo.Trim() != "")
                {
                    sql = " INSERT INTO ilwk12hs (select 'C', " + m_UdpD.ToString() + "," + m_UdpT.ToString() + "," + "'" + userName + "','" + wrkStn + "'," +
                          " W12CSN,W12BRN,W12APN,W12RSQ,W12REL,W12TTL,W12NME,W12SNM,W12HTL,W12HEX,W12OTL, " +
                          " W12OEX,W12MOB,W12VTE,W12FIL,W12CRD,W12UDT,W12UTM,W12UUS,W12UPG,W12UWS,W12STS " +
                          " FROM ilwk12 WHERE w12brn = " + brn + " AND w12apn = " + appNo + " )";
                }

            }
            catch (Exception ex)
            {

            }
            return sql;

        }


        public string DELETE_ILWK12(string brn, string appNo)
        {
            string sql = "";
            try
            {
                if (brn != "" && appNo != "")
                {
                    sql = " DELETE FROM ILWK12 WHERE W12BRN = " + brn + " AND W12apn = " + appNo;
                }
            }
            catch (Exception ex)
            {

            }
            return sql;
        }

        public string UPDATE_CSMS00(string csn, string M00UDT, string M00UTM, string M00UUS, string M00UPG, string M00UWS)
        {
            string sql = "";
            try
            {
                sql = " UPDATE CSMS00 SET " +
                      " M00OFT = @M00OFT ," +
                      " M00OFC = @M00OFC , " +
                      " M00UDT = " + M00UDT + ", " +
                      " M00UTM = " + M00UTM + ", " +
                      " M00UUS = '" + M00UUS + "'," +
                      " M00UPG = '" + M00UPG + "'," +
                      " M00UWS = '" + M00UWS + "'" +
                      " WHERE M00CSN =" + csn;
            }
            catch (Exception ex)
            {
            }
            return sql;
        }
        public bool SavePendingStatus(string curdate, string appNo, string appDate, string reason, string Username, string LocalClient, string BranchApp)
        {
            try
            {
                //UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
                //UserInfomation = m_userInfo;
                string sqlInsert = " INSERT INTO ilms01hs  " +
                                   " SELECT " +
                                    curdate + "," +
                                    m_UpdTime + "," +
                                    " '" + Username + "'," +
                                    " '" + LocalClient + "'," +
                                    " ilms01.* " +
                                    " FROM ilms01 WHERE p1brn = " + BranchApp +
                                    " AND  p1apno = " + appNo;


                iDB2Command cmd = new iDB2Command();
                cmd.CommandText = sqlInsert;
                int resInsert = ExecuteNonQuery(cmd);
                if (resInsert == -1)
                {
                    RollbackDAL();
                    CloseConnectioDAL();
                    return false;
                }
                string sqlUpd = " UPDATE ilms01 SET " +
                                " p1aprj = 'PD', " +
                                " p1loca = '150', " +
                                " p1auth = '" + m_UserInfo.Username + "'," +
                                " p1avdt = " + appDate + "," +
                                " p1avtm = " + m_UdpT + "," +
                                " p1resn = '" + reason + "'," +
                                " p1updt = " + curdate + "," +
                                " p1uptm = " + m_UdpT + "," +
                                " p1upus = '" + Username + "'," +
                                " p1wsid = '" + LocalClient + "'" +
                                " WHERE p1apno = " + appNo +
                                " AND p1brn = " + BranchApp;
                cmd.Parameters.Clear();
                cmd.CommandText = sqlUpd;
                int resUdp = ExecuteNonQuery(cmd);
                if (resUdp == -1)
                {
                    RollbackDAL();
                    CloseConnectioDAL();
                    return false;
                }
                //CommitDAL();
                //CloseConnectioDAL();
                return true;


            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool SaveStepCustomer(ILDataCenter ilObj, string curdate, string appNo, string appDate, string step, string programName, string Username, string LocalClient, string BranchApp, string p1aprj)
        {
            try
            {
                //UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
                //UserInfomation = m_userInfo;
                string sqlInsert = " INSERT INTO ilms01hs  " +
                                   " SELECT " +
                                    curdate + "," +
                                    m_UpdTime + "," +
                                    " '" + Username + "'," +
                                    " '" + LocalClient + "'," +
                                    " ilms01.* " +
                                    " FROM ilms01 WHERE p1brn = " + BranchApp +
                                    " AND  p1apno = " + appNo;


                iDB2Command cmd = new iDB2Command();
                cmd.CommandText = sqlInsert;
                int resInsert = ilObj.ExecuteNonQueryNoCommit(cmd);
                if (resInsert == -1)
                {
                    ilObj.RollbackDAL();
                    ilObj.CloseConnectioDAL();
                    return false;
                }
                string sqlUpd = "";

                if (step == "2")
                {
                    sqlUpd = " UPDATE ilms01 SET " +
                                " p1fill = substr(p1fill,1,24)||'" + step + "'||substr(p1fill,26,2)||'  '||substr(p1fill,30,9)," +
                                " p1aprj = " + p1aprj + ", " +
                                //" p1resn = ''," +       // แก้ไข เรื่อง PENDING CODE  10/02/2558 
                                " p1updt = " + curdate + "," +
                                " p1uptm = " + m_UdpT + "," +
                                " p1upus = '" + Username + "'," +
                                " p1prog = '" + programName + "', " +
                                " p1wsid = '" + LocalClient + "'" +
                                " WHERE p1apno = " + appNo +
                                " AND p1brn = " + BranchApp;
                }
                else if (step == "3")
                {
                    sqlUpd = " UPDATE ilms01 SET " +
                                " p1fill = substr(p1fill,1,24)||'" + step + "'||substr(p1fill,26,2)||'PI'||substr(p1fill,30,9)," +
                                " p1aprj = " + p1aprj + ", " +
                                //" p1resn = ''," +              // แก้ไข เรื่อง PENDING CODE  10/02/2558        
                                " p1updt = " + curdate + "," +
                                " p1uptm = " + m_UdpT + "," +
                                " p1upus = '" + Username + "'," +
                                " p1prog = '" + programName + "', " +
                                " p1wsid = '" + LocalClient + "'" +
                                " WHERE p1apno = " + appNo +
                                " AND p1brn = " + BranchApp;
                }
                else
                {
                    sqlUpd = " UPDATE ilms01 SET " +
                             " p1fill = substr(p1fill,1,24)||'" + step + "'||substr(p1fill,26,13)," +
                             " p1aprj = " + p1aprj + ", " +
                             " p1resn = p1resn," +     // แก้ไข เรื่อง PENDING CODE  10/02/2558 
                             " p1updt = " + curdate + "," +
                             " p1uptm = " + m_UdpT + "," +
                             " p1upus = '" + Username + "'," +
                             " p1prog = '" + programName + "', " +
                             " p1wsid = '" + LocalClient + "'" +
                             " WHERE p1apno = " + appNo +
                             " AND p1brn = " + BranchApp;
                }
                cmd.Parameters.Clear();
                cmd.CommandText = sqlUpd;
                int resUdp = ilObj.ExecuteNonQuery(cmd);
                if (resUdp == -1)
                {
                    ilObj.RollbackDAL();
                    ilObj.CloseConnectioDAL();
                    return false;
                }
                //CommitDAL();
                //CloseConnectioDAL();
                return true;


            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool SaveRejectStatus(ILDataCenter ilObj, string curdate, string appNo, string appDate, string reason, string p1aprj, string p1loca, string step, string programName, string Username, string LocalClient, string BranchApp, string csn, string idNO)
        {
            try
            {
                //UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
                //UserInfomation = m_userInfo;
                string sqlInsert = " INSERT INTO ilms01hs  " +
                                   " SELECT " +
                                    curdate + "," +
                                    m_UpdTime + "," +
                                    " '" + Username + "'," +
                                    " '" + LocalClient + "'," +
                                    " ilms01.* " +
                                    " FROM ilms01 WHERE p1brn = " + BranchApp +
                                    " AND  p1apno = " + appNo;


                iDB2Command cmd = new iDB2Command();
                cmd.CommandText = sqlInsert;
                int resInsert = ilObj.ExecuteNonQuery(cmd);
                if (resInsert == -1)
                {
                    ilObj.RollbackDAL();
                    ilObj.CloseConnectioDAL();
                    return false;
                }
                string sqlUpd = " UPDATE ilms01 SET " +
                                " p1fill = substr(p1fill,1,24)||'" + step + "'||substr(p1fill,26,13)," +
                                " p1stdt = " + curdate + ", " + //  เพิ่มเติมวันที่ Update เมื่อ Auto Reject 
                                " P1STTM = " + m_UdpT + " ," +
                                " p1aprj = '" + p1aprj + "', " +
                                " p1loca = '" + p1loca + "', " +
                                " p1crcd = '" + Username + "'," +
                                " p1avdt = " + curdate + "," +
                                " p1avtm = " + m_UdpT + "," +
                                // " p1auth = '" + m_UserInfo.Username + "'," +
                                // " p1avdt = " + appDate + "," +
                                // " p1avtm = " + m_UdpT + "," +
                                " p1resn = '" + reason + "'," +
                                " p1updt = " + curdate + "," +
                                " p1uptm = " + m_UdpT + "," +
                                " p1upus = '" + Username + "'," +
                                " p1wsid = '" + LocalClient + "'," +
                                " p1prog = '" + programName + "' " +
                                " WHERE p1apno = " + appNo +
                                " AND p1brn = " + BranchApp;
                cmd.Parameters.Clear();
                cmd.CommandText = sqlUpd;
                int resUdp = ilObj.ExecuteNonQuery(cmd);
                if (resUdp == -1)
                {
                    ilObj.RollbackDAL();
                    ilObj.CloseConnectioDAL();
                    return false;
                }

                string NoteDesc_ILMS38 = "";
                DataSet ds_note = ilObj.RetriveAsDataSetNoConnect("select g25des from gntb25 where g25rcd = '" + reason + "' ");
                if (ds_note != null)
                {
                    foreach (DataRow dr in ds_note.Tables[0].Rows)
                    {
                        NoteDesc_ILMS38 = dr["g25des"].ToString().Trim();
                    }
                    ds_note.Clear();
                }

                // เพิ่ม ให้ update document completed เมื่อ Auto reject ตาม Req No.  10/02/2558
                cmd.Parameters.Clear();
                cmd.CommandText = "update ilmd01 set d1dccm = 'Y' " +
                                  "where d1idno = '" + idNO + "' and d1srno = '" + appNo.PadLeft(11, '0') + "' ";
                int res_ilmd01 = ilObj.ExecuteNonQuery(cmd);
                if (res_ilmd01 == -1)
                {
                    ilObj.RollbackDAL();
                    ilObj.CloseConnectioDAL();
                    return false;
                }


                cmd.Parameters.Clear();
                cmd.CommandText = "Insert into ilms38 (P38CSN,P38CNT,P38ACD,P38RCD,P38DE1,P38DAT,P38TIM,P38USR) " +
                                          "Values (" + csn + ",0,'ADD','" + reason + "',@note38, " +
                                           curdate + ", " + m_UpdTime + ", '" + Username + "') ";
                cmd.Parameters.Add("@note38", NoteDesc_ILMS38.ToString().Trim());
                int affectedRows = ilObj.ExecuteNonQuery(cmd);
                if (affectedRows == -1)
                {
                    ilObj.RollbackDAL();
                    ilObj.CloseConnectioDAL();
                    return false;
                }

                return true;


            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public string UPDATE_CSMS11(ILDataCenter ilObj, string userName, string wkStation, string csn, string addrCode, string tel, string tel_ext, string mobile, string curdate, string programName)
        {
            string sqlRet = "";
            try
            {
                DataSet ds_11 = new DataSet();

                // ***  check data in csms11 ***// 
                string sql = " SELECT * FROM CSMS11 " +
                             " WHERE M11CSN = " + csn +
                             " AND M11CDE = '" + addrCode + "' " +
                             " AND M11REF = '' ";
                ds_11 = ilObj.RetriveAsDataSetNoConnect(sql);

                if (check_dataset(ds_11))
                {
                    sqlRet = " UPDATE CSMS11 SET M11TEL = '" + tel + "'," +
                             " M11EXT = @M11EXT ," +
                             //" M11EXT = '" + tel_ext + "' ," +
                             " M11UDT = " + curdate + "," +
                             " M11UTM = " + m_UdpT + "," +
                             " M11UUS = '" + userName + "'," +
                             " M11UPG = '" + programName + "'," +
                             " M11UWS = '" + Wrkstn + "'";
                    if (addrCode == "H")
                    {
                        if (mobile.Trim() != "")
                        {
                            sqlRet += " , M11MOB = '" + mobile + "' ";
                        }
                    }
                    sqlRet += " WHERE M11CSN = " + csn +
                              " AND M11CDE = '" + addrCode + "' " +
                              " AND M11REF = '' ";

                }
                else if (ds_11.Tables[0].Rows.Count == 0)
                {
                    sqlRet = " INSERT INTO  CSMS11 ( M11CSN,M11CDE,M11REF,M11TEL,M11EXT,M11MOB,M11UDT,M11UTM,M11UUS,M11UPG,M11UWS ) " +
                             " VALUES ( " +
                             csn + "," +
                             "'" + addrCode + "'," +
                             "''," +
                             "'" + tel + "'," +
                             " @M11EXT  ," +
                             //"'"+tel_ext+"',"+
                             "'" + mobile + "'," +
                             curdate + "," +
                             m_UpdTime + "," +
                             "'" + userName + "'," +
                             "'" + programName + "'," +
                             "'" + Wrkstn + "'" +
                             " )";
                }


            }
            catch (Exception ex)
            {

            }
            return sqlRet;

        }

        public string INSERT_CSMS00HS(string csn)
        {
            string sql = "";
            try
            {
                sql = "INSERT INTO csms00hs (SELECT * FROM CSMS00 WHERE m00csn = " + csn + ") ";
            }
            catch (Exception ex)
            {
            }
            return sql;
        }
        public string INSERT_CSMS11HS(string csn)
        {
            string sql = "";
            try
            {
                sql = " INSERT INTO csms11hs " +
                      " (SELECT * FROM csms11 WHERE m11csn =" + csn +
                      " AND m11ref = '' and m11rsq=0 and m11cde='H')";
            }
            catch (Exception ex)
            {

            }
            return sql;
        }




        #endregion ---  function sql ---

        #region function
        public bool check_dataset(DataSet ds)
        {
            try
            {
                if (ds == null)
                {
                    return false;
                }
                if (ds.Tables == null)
                {
                    return false;
                }
                if (ds.Tables[0].Rows.Count == 0)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public bool checkBirthDate(string birthDate1, string birthDate2, string realBirthDate, ref string err)
        {
            if (birthDate1 != birthDate2)
            {
                err = "วันเกิดไม่ตรงกัน กรุณาระบุใหม่";
                return false;
            }
            else
            {
                if (birthDate2 == realBirthDate)
                {
                    return true;
                }
                else
                {
                    err = "วันเกิดไม่ตรงกัน ไม่ตรงกับวันที่ในระบบ กรุณาระบุใหม่";
                    return false;
                }
            }
        }
        public bool checkMobile(string mobile1, string mobile2, string realMobile, ref string err)
        {
            if (mobile1 != mobile2)
            {
                err = "เบอร์โทรศัพท์มือถือ ไม่ตรงกัน ";
                return false;
            }
            else
            {
                if (mobile2 == realMobile)
                {
                    return true;
                }
                else
                {
                    err = "เบอร์โทรศัพท์ ไม่ตรงกับระบบ";
                    return false;
                }
            }

        }
        public bool checkCommercialRegis(string commerc, string occupation, ref string err)
        {
            if (occupation == "011" || occupation == "012")
            {
                if (commerc == "")
                {
                    err = "ต้องระบุ";
                    return false;
                }
                else
                {
                    return true;
                }

            }
            else
            {
                return true;
            }
        }
        public bool checkSubMarital(string subMarital, int child, int age, ref string err, string marital = "")
        {
            //subMarital = subMarital.ToString().Trim();

            if (marital == "")
            {

                if (subMarital == "04")
                {
                    if (age > 40)
                    {
                        err = "SubMarital status ไม่ถูกต้อง :อายุไม่สัมพันธ์กับสถานะที่เลือก ";
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }

                else if (subMarital == "06")
                {
                    if (age < 40)
                    {
                        err = "SubMarital status ไม่ถูกต้อง : อายุไม่สัมพันธ์กับสถานะที่เลือก";
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else if (subMarital == "01")
                {

                    if (child == 0)
                    {
                        err = "กรุณาระบุจำนวนบุตร";
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else if (subMarital == "02")
                {

                    if (child > 0)
                    {
                        err = "จำนวนบุตรต้องเท่ากับ 0";
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else if (subMarital == "03")
                {
                    if (child == 0)
                    {
                        err = "กรุณาระบุจำนวนบุตร";
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else if (subMarital == "05")
                {
                    if (child > 0)
                    {
                        err = "จำนวนบุตรต้องเท่ากับ 0";
                        return false;
                    }
                    else
                    {
                        return true;
                    }

                }
                else
                {
                    return true;
                }
            }
            else
            {
                if (marital == "1")
                {
                    if (subMarital.Equals("04"))
                    {
                        if (age > 40)
                        {
                            err = "SubMarital status ไม่ถูกต้อง :อายุไม่สัมพันธ์กับสถานะที่เลือก ";
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else if (subMarital.Equals("06"))
                    {
                        if (age < 40)
                        {
                            err = "SubMarital status ไม่ถูกต้อง : อายุไม่สัมพันธ์กับสถานะที่เลือก";
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        err = "สถานภาพไม่สัมพันธ์กับ Sub marital ";
                        return false;
                    }


                    ////if (!subMarital.Equals("04") || !subMarital.Equals("06"))
                    //if (subMarital != "04" && subMarital != "06")
                    //{
                    //    err = "สถานภาพไม่สัมพันธ์กับ Sub marital ";
                    //    return false;
                    //}
                    //return true;
                }
                else if (marital == "2")
                {
                    if (subMarital.Equals("01"))
                    {
                        if (child == 0)
                        {
                            err = "กรุณาระบุจำนวนบุตร";
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else if (subMarital.Equals("02"))
                    {
                        if (child > 0)
                        {
                            err = "จำนวนบุตรต้องเท่ากับ 0";
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        err = "สถานภาพไม่สัมพันธ์กับ Sub marital ";
                        return false;
                    }
                    //if ( subMarital != "01" && subMarital !="02")
                    //{
                    //    err = "สถานภาพไม่สัมพันธ์กับ Sub marital ";
                    //    return false;
                    //}
                    //else if (subMarital == "01") 
                    //{
                    //    if (child == 0)
                    //    {
                    //        err = "กรุณาระบุจำนวนบุตร";
                    //        return false;
                    //    }
                    //    else
                    //    {
                    //        return true;
                    //    }
                    //}
                    //else if (subMarital == "02") 
                    //{
                    //    if (child > 0)
                    //    {
                    //        err = "จำนวนบุตรต้องเท่ากับ 0";
                    //        return false;
                    //    }
                    //    else
                    //    {
                    //        return true;
                    //    }
                    //}
                    //return true;
                }
                else if (marital == "3" || marital == "4")
                {
                    if (subMarital.Equals("03"))
                    {
                        if (child == 0)
                        {
                            err = "กรุณาระบุจำนวนบุตร";
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else if (subMarital.Equals("05"))
                    {
                        if (child > 0)
                        {
                            err = "จำนวนบุตรต้องเท่ากับ 0";
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        err = "สถานภาพไม่สัมพันธ์กับ Sub marital ";
                        return false;
                    }
                    //if (subMarital != "03" &&  subMarital != "05")
                    //{
                    //    err = "สถานภาพไม่สัมพันธ์กับ Sub marital ";
                    //    return false;
                    //}
                    //else if (subMarital == "03")
                    //{
                    //    if (child == 0)
                    //    {
                    //        err = "กรุณาระบุจำนวนบุตร";
                    //        return false;
                    //    }
                    //    else
                    //    {
                    //        return true;
                    //    }
                    //}
                    //else if (subMarital == "05")
                    //{
                    //    if (child > 0)
                    //    {
                    //        err = "จำนวนบุตรต้องเท่ากับ 0";
                    //        return false;
                    //    }
                    //    else
                    //    {
                    //        return true;
                    //    }
                    //}
                    //return true;
                }
                return true;


            }

        }
        public int computeAge(string birthDate)
        {
            try
            {

                DateTime dateNow = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy", m_DThai), m_DThai);
                DateTime birthDate_ = Convert.ToDateTime(birthDate.Substring(6, 2) + "/" + birthDate.Substring(4, 2) + "/" + birthDate.Substring(0, 4), m_DThai);
                //DateTime birthDate_ = Convert.ToDateTime(birthDate.Substring(6, 2) + "/" + birthDate.Substring(4, 2) + "/" + birthDate.Substring(0, 4), m_DThai);
                //DateTime dt_printdate = Convert.ToDateTime("25" + sub_printDate.Substring(4, 2) + "/" + sub_printDate.Substring(2, 2) + "/" + sub_printDate.Substring(0, 2), m_DThai);
                //DateTime dt_bookingDate = Convert.ToDateTime(bookingDate, m_DThai);

                int age = dateNow.Year - birthDate_.Year;
                if (dateNow < birthDate_.AddYears(age))
                {
                    age--;
                }

                return age;
            }
            catch (Exception ex)
            {
                return 0;
            }

        }
        public string getTel(string tel, string tel_to)
        {
            string telAll = "";
            try
            {
                if (tel.Trim() != "" && tel_to.Trim() != "")
                {
                    telAll = tel + "-" + tel_to;
                }
                else if (tel.Trim() != "" && tel_to.Trim() == "")
                {
                    telAll = tel.Trim();
                }
                else if (tel.Trim() == "" && tel_to.Trim() == "")
                {
                    telAll = "";
                }
                else
                {
                    telAll = tel.Trim();
                }
                return telAll;
            }
            catch (Exception ex)
            {
                return telAll;
            }
        }
        public bool checkTelExt(string tel, string ext)
        {
            try
            {
                if (tel.Trim() == "" && ext.Trim() != "")
                {
                    return false;
                }
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public bool calTCL(string idno, string csn, string brn, string appno, string appDate, string Date_97,
            string birthdate, string m13sex, string age, string m13mrt, string m13ttl, string m13mtl, string m13res, string resident_y,
            string workyear, string m13occ, string m13but, string m13slt, string m13off, string m13net, string m13con, string m13chl,
            string m13pos, string m13emp, string pad_three_1, string pad_three_2, string m13apv, string m13hzp, string m13sst, string m13saj,
            string loanReq, string vendor, string M13GOL, string M13CHA,
            ref DataSet ds_result, ref string err, string m13ozp = "")
        {
            ILDataCenter busobj = new ILDataCenter();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            busobj.UserInfomation = m_userInfo;
            DataSet DS = new DataSet();
            DataSet DS_GNSRGNOC = new DataSet();

            DataTable dt = new DataTable();
            dt.Columns.Add("EdtTCL", typeof(string));
            dt.Columns.Add("LblStsTCL", typeof(string));
            dt.Columns.Add("LTCL", typeof(string));
            dt.Columns.Add("EdtACL", typeof(string));
            dt.Columns.Add("G_TCL_13", typeof(string));
            dt.Columns.Add("G_ACL_13", typeof(string));
            dt.Columns.Add("G_PD", typeof(string));
            dt.Columns.Add("E_group", typeof(string));
            dt.Columns.Add("E_rank", typeof(string));
            dt.Columns.Add("G_Rank_for_GNSR031", typeof(string));
            dt.Columns.Add("G_GROUP_ONGOING_Y", typeof(string));
            dt.Columns.Add("G_RANK_ONGOING_Y", typeof(string));
            dt.Columns.Add("G_ACL_ONGOING_Y", typeof(string));
            dt.Columns.Add("P4TYPE", typeof(string));
            dt.Columns.Add("LACL", typeof(string));
            dt.Columns.Add("EdtBOTLoan", typeof(string));
            dt.Columns.Add("G_NewModel_ZR", typeof(string));
            dt.Columns.Add("G_Mem_ACL_Ongoing", typeof(string));
            dt.Columns.Add("G_ACL", typeof(string));
            dt.Columns.Add("G_Mem_TCL_Ongoing", typeof(string));
            dt.Columns.Add("G_Final_TCL", typeof(string));
            dt.Columns.Add("G_Net_Income", typeof(string));
            dt.Columns.Add("G_Orank", typeof(string));
            dt.Columns.Add("G_Otimes", typeof(string));
            dt.Columns.Add("G_Arank", typeof(string));
            dt.Columns.Add("G_Atimes", typeof(string));
            dt.Columns.Add("G_AACL", typeof(string));
            dt.Columns.Add("G_Rrank", typeof(string));
            dt.Columns.Add("G_Rtimes", typeof(string));
            dt.Columns.Add("G_TCL", typeof(string));
            dt.Columns.Add("G_CSP", typeof(string));
            dt.Columns.Add("G_Total_CSP", typeof(string));
            dt.Columns.Add("G_Up_Down_Flag", typeof(string));
            dt.Columns.Add("G_Have_TCL", typeof(string));
            dt.Columns.Add("G_GRACE_Period", typeof(string));
            dt.Columns.Add("G_Model", typeof(string));
            dt.Columns.Add("G_PD1", typeof(string));
            dt.Columns.Add("G_GNO", typeof(string));
            dt.Columns.Add("E_model", typeof(string));
            dt.Columns.Add("G_Have_CSMS03", typeof(bool));
            dt.Columns.Add("G_Ongoing_TCL", typeof(string));
            dt.Columns.Add("EdtNetIncome", typeof(string));
            dt.Columns.Add("EdtCrBal", typeof(string));
            dt.Columns.Add("EAAA", typeof(string));
            dt.Columns.Add("EdtCrLmt", typeof(string));
            dt.Columns.Add("EdtESBLoan", typeof(string));
            dt.Columns.Add("EdtCrAvi", typeof(string));
            dt.Columns.Add("E_Apv_avi", typeof(string));
            dt.Columns.Add("lb_pApproveL", typeof(string));
            dt.Columns.Add("txt_pay_abl", typeof(string));
            //dt.Columns.Add("PERMSG", typeof(string));

            try
            {
                string EdtTCL = "";
                string LblStsTCL = "";
                string LTCL = "";
                string EdtACL = "";
                string G_TCL_13 = "";
                string G_ACL_13 = "";
                string G_PD = "";
                string E_group = "";
                string E_rank = "";
                string G_Rank_for_GNSR031 = "";
                string G_GROUP_ONGOING_Y = "";
                string G_RANK_ONGOING_Y = "";
                string G_ACL_ONGOING_Y = "";
                string P4TYPE = "";
                string LACL = "";
                string EdtBOTLoan = "";
                string G_NewModel_ZR = "";
                string G_Mem_ACL_Ongoing = "";
                string G_ACL = "";
                string G_Mem_TCL_Ongoing = "";
                string G_Final_TCL = "";
                string G_Net_Income = "";
                string G_Orank = "";
                string G_Otimes = "";
                string G_Arank = "";
                string G_Atimes = "";
                string G_AACL = "";
                string G_Rrank = "";
                string G_Rtimes = "";
                string G_TCL = "";
                string G_CSP = "";
                string G_Total_CSP = "";
                string G_Up_Down_Flag = "";
                string G_Have_TCL = "";
                string G_GRACE_Period = "";
                string G_Model = "";
                string G_PD1 = "";
                string G_GNO = "";
                string E_model = "";
                bool G_Have_CSMS03;
                string G_Ongoing_TCL = "";
                string EdtNetIncome = "";
                string EdtCrBal = "";
                string EAAA = "";
                string EdtCrLmt = "";
                string EdtESBLoan = "";
                string EdtCrAvi = "";
                string E_Apv_avi = "";
                string lb_pApproveL = "";
                string txt_pay_abl = "";


                //for test
                //idno = "2557090300007";
                //CSN.Value = "57199876";
                //brn.Value = "001";
                //appno = "1570007767";
                string appMobile = "0";
                if (m13mtl != "")
                {
                    appMobile = "1";
                }

                string Array100 = csn.PadRight(8, '0') +
                                             m13sex.PadLeft(1, '0') +
                                             Convert.ToInt32(age).ToString().PadLeft(3, '0') +
                                             m13mrt.PadLeft(1, '0') +
                                             m13ttl.PadLeft(1, '0') +
                                             appMobile +//m13mtl.PadLeft(1, '0') +
                                             m13res.PadLeft(2, '0') +
                                             resident_y.PadLeft(4, '0') +
                                             workyear.PadLeft(4, '0') +
                                             m13occ.PadLeft(3, '0') +
                                             m13but.PadLeft(2, '0') +
                                             m13slt.PadLeft(2, '0') +
                                             m13off.PadLeft(6, '0') +
                                             m13net.PadLeft(8, '0') +
                                             m13con.PadLeft(2, '0') +
                                             m13chl.PadLeft(2, '0') +
                                             m13pos.PadLeft(3, '0') +
                                             m13emp.PadLeft(2, '0') +
                                             pad_three_1.PadLeft(3, '0') +
                                             Date_97.PadLeft(8, '0') +
                                             pad_three_2.PadRight(3, '0') +
                                             m13apv.PadLeft(1, '0') +
                                             m13hzp.PadLeft(5, '0') +
                                             m13sst.PadLeft(2, ' ') +
                                             m13saj.PadLeft(8, '0') +
                                             M13CHA.PadLeft(3, '0') +
                                             m13ozp.PadLeft(5, '0') +
                                             "OFF";



                DS = busobj.RetriveAsDataSet("select m3csno, m3crlm, m3acam from csms03 where m3csno = " + csn + " and m3del='' ");
                G_Have_CSMS03 = false;
                if (busobj.check_dataset(DS))
                {
                    G_Have_CSMS03 = true;
                }
                if (G_Have_CSMS03 == false)
                {
                    string outCSNO = "", outOPD = "", outOGNO = "", outORANK = "", outOINC = "", outOACL = "";
                    string outO2GNO = "", outO2RANK = "", outO2ACL = "", outO2RNK = "", outO21ACL = "", outOTYPE = "";

                    string in_AGE = "", error = "";

                    busobj.CALL_GNP0371(birthdate, "", "YMD", "B", "", "IL", "",
                                        m_userInfo.BizInit.ToString(), m_userInfo.BranchNo.ToString(),
                                        ref in_AGE, ref error);
                    busobj.CloseConnectioDAL();
                    busobj.CALL_GNSRGNOC("IL", brn, appno, Array100.ToString(),
                                          ref outCSNO, ref outOPD, ref outOGNO, ref outORANK, ref outOINC, ref outOACL,  //4,5,6,7,8,9
                                          ref outO2GNO, ref outO2RANK, ref outO2ACL, ref outO2RNK, ref outO21ACL, ref outOTYPE, //10,11,12,13,14,15
                                          m_userInfo.BizInit.ToString(), m_userInfo.BranchNo.ToString());
                    busobj.CloseConnectioDAL();


                    EdtTCL = "0";
                    LblStsTCL = "TCL";
                    LTCL = "TCL(New Model)";
                    EdtACL = outOACL.ToString();
                    EdtTCL = outOACL.ToString();
                    G_TCL_13 = EdtTCL;
                    if (G_TCL_13 == "")
                    {
                        G_TCL_13 = "0";
                    }
                    G_ACL_13 = outOACL.ToString();
                    if (G_ACL_13 == "")
                    {
                        G_ACL_13 = "0";
                    }
                    if (LTCL == "TCL(New Model)")
                    {
                        LTCL = LTCL + ' ' + outOTYPE.ToString();
                    }

                    G_PD = "";
                    if (outOPD.ToString().Trim() != "")
                    {
                        G_PD = outOPD.Trim().Replace(".", "").PadLeft(12, '0');//(outOPD.ToString().Replace(".", "").Trim()).PadLeft(15, '0');//G_PD = outOPD.ToString().PadLeft(3, '0').Replace(".", ""); //(outOPD.ToString().Replace(".", "").Trim()).PadLeft(12,'0');   // 0 = 4 ตำแหน่ง
                    }
                    E_group = outOGNO.ToString().Trim();
                    E_rank = outORANK.ToString().Trim();
                    G_Rank_for_GNSR031 = outO2RANK.ToString().Trim();

                    G_GROUP_ONGOING_Y = outO2GNO.ToString().Trim();
                    G_RANK_ONGOING_Y = outO2RANK.ToString().Trim();
                    G_ACL_ONGOING_Y = outO2ACL.ToString().Trim();
                    P4TYPE = outOTYPE.ToString().Trim();
                }
                else
                {
                    foreach (DataRow dr in DS.Tables[0].Rows)
                    {
                        EdtTCL = dr["m3crlm"].ToString().Trim();
                        LblStsTCL = "TCL(Existing)";
                        LTCL = "TCL(Existing)";
                        LACL = "ACL(Existing)";
                        EdtACL = "0";
                        //compare with BOT Loan
                        if (Convert.ToDecimal(EdtTCL) > (Convert.ToDecimal(m13net) * 5))
                        {
                            EdtTCL = (((Convert.ToDecimal(m13net) * 5) / 100) * 100).ToString();
                        }
                        G_TCL_13 = EdtTCL;
                        if (G_TCL_13.Trim() == "")
                        {
                            G_TCL_13 = "0";
                        }
                        //ACL
                        // LACL.Visible := False;
                        // EdtACL.Visible := False;
                    }
                    busobj.CloseConnectioDAL();
                }
                EdtBOTLoan = (Convert.ToDecimal(m13net) * 5).ToString();

                string outSTS = "", outMSG = "", outFLG = "";
                busobj.CALL_CSSR36(csn, ref outSTS, ref outMSG, ref outFLG, m_userInfo.BizInit.ToString(), m_userInfo.BranchNo.ToString());
                busobj.CloseConnectioDAL();
                if (outFLG.ToString().Trim() == "Y")
                {
                    G_NewModel_ZR = "Y";
                    string outCSNO = "", outOPD = "", outOGNO = "", outORANK = "", outOINC = "", outOACL = "";
                    string outO2GNO = "", outO2RANK = "", outO2ACL = "", outO2RNK = "", outO21ACL = "", outOTYPE = "";

                    if (busobj.check_dataset(DS))
                    {
                        foreach (DataRow dr in DS.Tables[0].Rows)
                        {
                            string in_AGE = "", error = "";

                            busobj.CALL_GNSRGNOC("IL", brn, appno.ToString(), Array100.ToString(),
                                                  ref outCSNO, ref outOPD, ref outOGNO, ref outORANK, ref outOINC, ref outOACL,  //4,5,6,7,8,9
                                                  ref outO2GNO, ref outO2RANK, ref outO2ACL, ref outO2RNK, ref outO21ACL, ref outOTYPE, //10,11,12,13,14,15
                                                  m_userInfo.BizInit.ToString(), m_userInfo.BranchNo.ToString());
                            busobj.CloseConnectioDAL();
                        }
                        DS.Clear();
                    }
                    else
                    {
                        err = "Pleaes verify judgment before save product";
                        busobj.CloseConnectioDAL();
                        ds_result.Tables.Add(dt);
                        return false;
                    }

                    EdtTCL = "0";
                    LblStsTCL = "TCL";
                    LTCL = "TCL(New Model ZR)";
                    EdtACL = outOACL.ToString();
                    EdtTCL = outOACL.ToString();
                    G_TCL_13 = EdtTCL;
                    if (G_TCL_13 == "")
                    {
                        G_TCL_13 = "0";
                    }
                    G_ACL_13 = outOACL.ToString();
                    if (G_ACL_13 == "")
                    {
                        G_ACL_13 = "0";
                    }
                    if (LTCL == "TCL(New Model ZR)")
                    {
                        LTCL = LTCL + ' ' + outOTYPE.ToString();
                    }

                    G_PD = "";
                    if (outOPD.ToString().Trim() != "")
                    {
                        G_PD = outOPD.Trim().Replace(".", "").PadLeft(12, '0');//outOPD.ToString().Replace(".", "").Trim().PadLeft(15,'0');
                    }
                    E_group = outOGNO.ToString().Trim();
                    E_rank = outORANK.ToString().Trim();
                    G_Rank_for_GNSR031 = outO2RANK.ToString().Trim();

                    G_GROUP_ONGOING_Y = outO2GNO.ToString().Trim();
                    G_RANK_ONGOING_Y = outO2RANK.ToString().Trim();
                    G_ACL_ONGOING_Y = outO2ACL.ToString().Trim();
                    P4TYPE = "Z";
                }
                else
                {
                    if (outSTS.ToString() == "Y")
                    {
                        string outPOSALN = "", outPOORNK = "", outPOOTMS = "", outPOOACL = "", outPOARNK = "", outPOATMS = "", outPOAACL = "";
                        string outPORRNK = "", outPORTMS = "", outPORACL = "", outPOFTCL = "", outPOSTRP = "", outPOTOTP = "", outPOAFLG = "";
                        string outPOHAVE = "", outPOODGC = "", outPOMDL = "", outPOPD = "", outPOGNO = "";
                        foreach (DataRow dr in DS.Tables[0].Rows)
                        {
                            //public bool CALL_CSSR31(string prmPICSN, string prmPISAL, string prmPIFLG, string prmPIRANK, string prmPIOCC, string prmBRN, string prmPISINCP, string prmPINCAJ,
                            //    ref string POSALN, ref string POORNK, ref string POOTMS, ref string POOACL, ref string POARNK, ref string POATMS, ref string POAACL,
                            //    ref string PORRNK, ref string PORTMS, ref string PORACL, ref string POFTCL, ref string POSTRP, ref string POTOTP, ref string POAFLG,
                            //    ref string POHAVE, ref string POODGC, ref string POMDL, ref string POPD, ref string POGNO,
                            //    string strBizInit, string strBranchNo)

                            if (G_Rank_for_GNSR031.ToString() == "")
                            {
                                G_Rank_for_GNSR031 = "0";
                            }
                            busobj.CALL_CSSR31(csn, m13net, "I", G_Rank_for_GNSR031.Trim(),
                                               m13occ, m_userInfo.BranchNo.ToString().Trim(), m13sst, m13saj,
                                              ref outPOSALN, ref outPOORNK, ref outPOOTMS, ref outPOOACL, ref outPOARNK, ref outPOATMS, ref outPOAACL, //8,9,10,11,12,13,14
                                              ref outPORRNK, ref outPORTMS, ref outPORACL, ref outPOFTCL, ref outPOSTRP, ref outPOTOTP, ref outPOAFLG, //15,16,17,18,19,20,21
                                              ref outPOHAVE, ref outPOODGC, ref outPOMDL, ref outPOPD, ref outPOGNO, //22,23,24,25,26
                                              m_userInfo.BizInit.ToString(), m_userInfo.BranchNo.ToString());
                            busobj.CloseConnectioDAL();
                        }
                        G_Mem_ACL_Ongoing = G_ACL;
                        G_Mem_TCL_Ongoing = G_Final_TCL;
                        G_Net_Income = outPOSALN.ToString().Trim();
                        G_Orank = outPOORNK.ToString().Trim();
                        G_Otimes = outPOOTMS.ToString().Trim();
                        G_ACL = outPOOACL.ToString().Trim();
                        G_Arank = outPOARNK.ToString().Trim();
                        G_Atimes = outPOATMS.ToString().Trim();
                        G_AACL = outPOAACL.ToString().Trim();
                        G_Rrank = outPORRNK.ToString().Trim();
                        G_Rtimes = outPORTMS.ToString().Trim();
                        G_TCL = outPORACL.ToString().Trim();
                        G_Final_TCL = outPOFTCL.ToString().Trim();
                        G_CSP = outPOSTRP.ToString().Trim();
                        G_Total_CSP = outPOTOTP.ToString().Trim();
                        G_Up_Down_Flag = outPOAFLG.ToString().Trim();

                        G_Have_TCL = outPOHAVE.ToString().Trim();
                        G_GRACE_Period = outPOODGC.ToString().Trim();
                        G_Model = outPOMDL.ToString().Trim();
                        G_PD1 = outPOPD.ToString().Trim();
                        G_GNO = outPOGNO.ToString().Trim();

                        G_PD1 = G_PD1.PadLeft(11, '0');
                        G_GNO = G_GNO.PadLeft(3, '0');

                        E_model = G_Model.Trim();
                        if (E_model == "3")
                        {
                            E_model = "T";
                        }
                    }
                }

                if (G_NewModel_ZR == "Y")
                {
                    G_Model = P4TYPE.ToString();
                }
                else
                {
                    if (G_Have_TCL.Trim() != "Y")
                    {
                        if (G_Have_CSMS03)
                        {
                            G_Model = "E";
                        }
                        else
                        {
                            G_Model = P4TYPE.Trim();
                        }
                    }
                }

                string POBOTL = "", PONOAP = "", POCSBL = "", POCRAV = "", POAPAM = "", POEBCL = "", POAVAP = "", POSTS = "", POOLDC = "", POVENR = "",
                       POPERV = "", POEBCS = "", POERR = "", PERMSG = "", mode = "";
                try
                {
                    mode = WebConfigurationManager.AppSettings["Mode"].ToString().Trim();
                }
                catch
                {
                    mode = "O";
                }

                if ((G_Have_CSMS03) & (G_NewModel_ZR != "Y"))
                {
                    G_PD = "            ";
                    if (G_Model.Trim() == "")
                    {
                        G_Model = " ";
                    }
                    if (G_Rank_for_GNSR031.Trim() == "")
                    {
                        G_Rank_for_GNSR031 = "  ";
                    }
                }

                if (G_Rank_for_GNSR031.Trim().Length == 0)
                {
                    G_PD = G_PD + "  " + G_Model;
                }
                if (G_Rank_for_GNSR031.Trim().Length == 1)
                {
                    G_PD = G_PD + "0" + G_Rank_for_GNSR031 + G_Model;
                }
                if (G_Rank_for_GNSR031.Trim().Length == 2)
                {
                    G_PD = G_PD + G_Rank_for_GNSR031 + G_Model;
                }

                string GNSR87_9 = "", GNSR87_13 = "";
                if (G_Have_TCL == "Y")
                {
                    LACL = "ACL(TOGO)";
                    LTCL = "TCL(TOGO)";
                    EdtACL = G_ACL;
                    E_rank = G_Orank;
                    //***Find_Old_TCL_Ongoing();

                    if ((G_Mem_TCL_Ongoing != G_Final_TCL) || (G_Mem_ACL_Ongoing != G_ACL))
                    {
                        G_Ongoing_TCL = G_Final_TCL;
                        EdtACL = G_Final_TCL;
                        EdtTCL = G_Final_TCL;
                    }

                    if ((EdtTCL == "0") || (EdtTCL == ""))
                    {
                        G_Ongoing_TCL = G_Final_TCL;
                        EdtACL = G_Final_TCL;
                        EdtTCL = G_Final_TCL;
                    }

                    //M13GOL ตัวนี้จะส่งมา                
                    DS = busobj.RetriveAsDataSet("select * from csms07 " +
                                                 "where c07csn=" + csn + " and " +
                                                 "c07app='IL' and c07apn=" + appno + " and c07brn=" + brn + " and " +
                                                 "c07acl=" + G_ACL + " and c07ftc=" + G_Final_TCL + " ");
                    if (busobj.check_dataset(DS))
                    {
                        foreach (DataRow dr in DS.Tables[0].Rows)
                        {
                            if ((G_Mem_TCL_Ongoing != G_Final_TCL) || (G_Mem_ACL_Ongoing != G_ACL))
                            {
                                if (M13GOL.ToString() == "")
                                {
                                    M13GOL = "0";
                                }

                                G_Ongoing_TCL = M13GOL;
                                EdtACL = G_Final_TCL;
                                EdtTCL = M13GOL;
                            }
                        }
                    }


                    //***
                    GNSR87_9 = G_Final_TCL.Trim();
                    if (G_Final_TCL.Trim() != EdtTCL.Trim())
                    {
                        EdtTCL = G_Final_TCL.Trim();
                    }
                    GNSR87_13 = "Y";
                }

                if (G_NewModel_ZR == "Y")
                {
                    GNSR87_13 = "Y";
                }

                if (GNSR87_9.ToString() == "")
                {
                    GNSR87_9 = EdtTCL.Trim();
                }

                if ((m13net != "0") & (G_Have_TCL != "Y") & (!G_Have_CSMS03))
                {
                    string outPPOTCL = "", outPOERR = "", outPERMSG = "";
                    busobj.CALL_GNSR86("IL", "01", m_userInfo.BranchApp.ToString(), appDate,
                                       Date_97, E_rank.Trim(), EdtACL.Trim(),
                                       ref outPPOTCL, ref outPOERR, ref outPERMSG, m_userInfo.BizInit.ToString(), m_userInfo.BranchNo.ToString());
                    busobj.CloseConnectioDAL();

                    if (outPOERR.ToString() == "Y")
                    {
                        ds_result.Tables.Add(dt);
                        return false;
                    }

                    if (G_TCL == "")
                    {
                        G_TCL_13 = "0";
                    }

                    if ((G_TCL_13.Trim() == EdtTCL.Trim()) || (EdtTCL.Trim() == "0"))
                    {
                        EdtTCL = outPPOTCL.ToString();
                        G_TCL_13 = EdtTCL.Trim();
                    }
                    //E_Apv_avi. = EdtTCL - StrToFloat(callGNSR87.value[16]);
                    GNSR87_9 = EdtTCL.Trim();
                }

                busobj.Call_GNSR87("IL", csn, m_userInfo.BranchApp.ToString(), appno.ToString().Trim(), appDate,
                                   birthdate, "1", "Y", m13net, GNSR87_9.ToString(), loanReq,
                                   vendor, "O", GNSR87_13.ToString(),
                                   ref POBOTL, ref PONOAP, ref POCSBL, ref POCRAV, ref POAPAM, ref POEBCL, ref POAVAP, ref POSTS, ref POOLDC, //14,15,16,17,18,19,20,21,22
                                   ref POVENR, ref POPERV, ref POEBCS, ref POERR, ref PERMSG, //23,24,25,26,27
                                   m_userInfo.BizInit.ToString(), m_userInfo.BranchNo.ToString());
                busobj.CloseConnectioDAL();

                EdtBOTLoan = POBOTL.ToString();
                EdtNetIncome = m13net;
                EdtCrBal = POCSBL.ToString();
                EAAA = POCRAV.ToString();

                //ปัดเศษ เช่น คำนวณ sal*1.5 = 29999*1.5 = 49998.5 ให้ปัดเป็น 44900
                if (Convert.ToDecimal(EdtTCL) > Convert.ToDecimal(EdtBOTLoan))
                {
                    int intTCL = (int)Math.Floor((Convert.ToDecimal(POBOTL.ToString()) / 100));
                    intTCL = (intTCL * 100);
                    EdtTCL = intTCL.ToString();
                }

                if ((G_Have_TCL.Trim() == "Y") || (G_NewModel_ZR.Trim() == "Y"))
                {
                    EAAA = (Convert.ToDecimal(EdtTCL) - Convert.ToDecimal(EdtCrBal)).ToString();
                }

                EdtCrLmt = EdtTCL;
                EdtESBLoan = POEBCL.ToString();
                EdtCrAvi = POAVAP.ToString();
                E_Apv_avi = EdtCrAvi;
                lb_pApproveL = EdtCrAvi;


                string Error_GNSR093 = "";
                string payment_ability = "";

                busobj.Call_GNSR093(idno.ToString(), m13net, ref Error_GNSR093, ref payment_ability, m_userInfo.BizInit, m_userInfo.BranchNo);
                busobj.CloseConnectioDAL();
                txt_pay_abl = payment_ability;

                dt.Rows.Add(EdtTCL, LblStsTCL, LTCL, EdtACL, G_TCL_13, G_ACL_13, G_PD, E_group,
                            E_rank, G_Rank_for_GNSR031, G_GROUP_ONGOING_Y, G_RANK_ONGOING_Y,
                            G_ACL_ONGOING_Y, P4TYPE, LACL, EdtBOTLoan, G_NewModel_ZR, G_Mem_ACL_Ongoing,
                            G_ACL, G_Mem_TCL_Ongoing, G_Final_TCL, G_Net_Income, G_Orank, G_Otimes, G_Arank,
                            G_Atimes, G_AACL, G_Rrank, G_Rtimes, G_TCL, G_CSP, G_Total_CSP, G_Up_Down_Flag,
                            G_Have_TCL, G_GRACE_Period, G_Model, G_PD1, G_GNO, E_model, G_Have_CSMS03, G_Ongoing_TCL,
                            EdtNetIncome, EdtCrBal, EAAA, EdtCrLmt, EdtESBLoan, EdtCrAvi, E_Apv_avi, lb_pApproveL, txt_pay_abl);

                ds_result.Tables.Add(dt);
                return true;
            }//try
            catch (Exception ex)
            {
                err = "Error on Calculate TCL";
                busobj.CloseConnectioDAL();
                ds_result.Tables.Add(dt);
                return false;
            }
        }

        public DataSet getCSMS07(ILDataCenter ilObj, string appNo, string branch_app, string csn)
        {
            DataSet ds = new DataSet();
            try
            {

                //UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
                //UserInfomation = m_userInfo;
                string sql = " SELECT c07csn FROM  csms07 " +
                             " WHERE  c07csn =  " + csn +
                             " AND c07app = 'IL' and c07apn = " + appNo +
                             " and c07brn = " + branch_app;

                ds = RetriveAsDataSetNoConnect(sql);
                //CloseConnectioDAL();



            }
            catch (Exception ex)
            {
                //CloseConnectioDAL();
            }
            return ds;
        }



        #endregion

        #region Phase 2
        //***  sub routine ***//

        public bool CALL_CSSR032(string PPMODE, string PPAPPL, string PPFLAG, string PPCSNO, string PPBUSS, string PPBRAN, string PPAPNO,
                                 string PPSDAT, string PPSTIM, ref string PRTNCD, ref string PERMSG1, string strBizInit, string strBranchNo)
        {
            string m_UdpTime = DateTime.Now.ToString("HHmmss");
            m_LastError = "";
            iDB2Command cmd = new iDB2Command();

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.CommandText = "CSSR032";
            // Parameter In

            cmd.Parameters.Add("PPMODE", iDB2DbType.iDB2Char, 1).Value = PPMODE;


            cmd.Parameters.Add("PPAPPL", iDB2DbType.iDB2Char, 1).Value = PPAPPL;

            cmd.Parameters.Add("PPFLAG", iDB2DbType.iDB2Char, 1).Value = PPFLAG;

            cmd.Parameters.Add("PPCSNO", iDB2DbType.iDB2Decimal, 0).Value = PPCSNO;
            cmd.Parameters["PPCSNO"].Precision = 8;
            cmd.Parameters["PPCSNO"].Scale = 0;

            cmd.Parameters.Add("PPBUSS", iDB2DbType.iDB2Char, 2).Value = PPBUSS;

            cmd.Parameters.Add("PPBRAN", iDB2DbType.iDB2Decimal, 0).Value = PPBRAN;
            cmd.Parameters["PPBRAN"].Precision = 3;
            cmd.Parameters["PPBRAN"].Scale = 0;

            cmd.Parameters.Add("PPAPNO", iDB2DbType.iDB2Decimal, 0).Value = PPAPNO;
            cmd.Parameters["PPAPNO"].Precision = 11;
            cmd.Parameters["PPAPNO"].Scale = 0;

            cmd.Parameters.Add("PPSDAT", iDB2DbType.iDB2Decimal, 0).Value = PPSDAT;
            cmd.Parameters["PPSDAT"].Precision = 8;
            cmd.Parameters["PPSDAT"].Scale = 0;

            cmd.Parameters.Add("PPSTIM", iDB2DbType.iDB2Decimal, 0).Value = PPSTIM;
            cmd.Parameters["PPSTIM"].Precision = 6;
            cmd.Parameters["PPSTIM"].Scale = 0;

            //cmd.Parameters.Add("PRTNCD", iDB2DbType.iDB2Char, 3).Value = PRTNCD;

            // output
            cmd.Parameters.Add("PRTNCD", iDB2DbType.iDB2Char, 3).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("PERMSG1", iDB2DbType.iDB2Char, 64).Direction = ParameterDirection.Output;

            if (m_da400.ExecuteSubRoutine("CSSR032", ref cmd, strBizInit, strBranchNo))
            {
                PRTNCD = cmd.Parameters["PRTNCD"].Value.ToString().Trim();
                PERMSG1 = cmd.Parameters["PERMSG1"].Value.ToString().Trim();

                return true;
            }
            else
                return false;
        }



        //** Set Recieve fax  **//
        public DataSet get_ilmd01_Date(string appDate)
        {
            DataSet ds = new DataSet();
            try
            {

                UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
                UserInfomation = m_userInfo;
                if (appDate != "")
                {
                    string sql = " SELECT SUM(CASE WHEN d1abrn <=1 THEN 1 ELSE 0 end) as SUMHQ , " +
                                 " SUM (CASE WHEN d1abrn > 1 THEN 1 ELSE 0  END) as SUMURT, " +
                                 " SUM (CASE WHEN D1DCCM = 'Y' THEN 1 ELSE 0 END) as SUMCOMP, " +
                                 " SUM (CASE WHEN D1DCCM <> 'Y' THEN 1 ELSE 0 END) as SUMNOTCOMP, " +
                                 " SUM (1) as TOTALCASE " +
                                 " FROM ilmd01 where d1apdt = " + appDate +
                                 " AND d1del=''  ";
                    ds = RetriveAsDataSet(sql);
                    CloseConnectioDAL();

                }

            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }
            return ds;

        }

        // get range  ilmd01
        public DataSet get_ilmd01_Hour(string appDate)
        {
            DataSet ds = new DataSet();
            try
            {

                UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
                UserInfomation = m_userInfo;
                if (appDate != "")
                {
                    string case_time = " CASE WHEN D1FXTM <= 100000 then 1 " +
                                       " WHEN D1FXTM <= 110000 THEN 2 " +
                                       " WHEN D1FXTM <= 120000 THEN 3 " +
                                       " WHEN D1FXTM <= 130000 THEN 4 " +
                                       " WHEN D1FXTM <= 140000 THEN 5 " +
                                       " WHEN D1FXTM <= 150000 THEN 6 " +
                                       " WHEN D1FXTM <= 160000 THEN 7 " +
                                       " WHEN D1FXTM <= 170000 THEN 8 " +
                                       " WHEN D1FXTM <= 180000 THEN 9 " +
                                       " WHEN D1FXTM <= 190000 THEN 10 " +
                                       " WHEN D1FXTM <= 200000 THEN 11 " +
                                       " WHEN D1FXTM  > 200000 THEN 12  END ";

                    string sql = "SELECT    " +
                                 case_time +
                                 " AS FXTM " +
                                 " , COUNT(*) as TOTALCASE " +
                                 " FROM ilmd01 WHERE d1apdt = " + appDate +
                                 " AND d1del='' " +
                                 " GROUP BY " +
                                 case_time +
                                 " ORDER BY " +
                                 case_time +
                                 " WITH UR ";

                    ds = RetriveAsDataSet(sql);
                    CloseConnectioDAL();

                }

            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }
            return ds;

        }

        // get App
        public DataSet get_app_ilmd01(string appDate, string name = "", string appNo = "", string mode = "")
        {
            DataSet ds = new DataSet();
            try
            {

                UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
                UserInfomation = m_userInfo;

                if (mode == "")
                {

                    if (appDate != "")
                    {


                        string sql = " SELECT substr(digits(D1APDT),7,2)||'/'||substr(digits(D1APDT),5,2)||'/'||substr(digits(D1APDT),1,4) D1APDT,D1SRNO,D1IDNO,D1ABRN,digits(D1VNID) as D1VNID , " +
                                     " substr(digits(D1FXTM),1,2)||':'||substr(digits(D1FXTM),3,2) as D1FXTM1, " +
                                     " D1FXTM,D1DCCM,D1TNAM,D1RMAK,D1UPUS " +
                                     " FROM ilmd01 " +
                                     " WHERE d1apdt =  " + appDate +
                                     " AND d1del = '' ORDER BY D1SRNO  , D1FXTM ASC   WITH UR ";


                        ds = RetriveAsDataSet(sql);
                        CloseConnectioDAL();

                    }
                }
                else if (mode == "GET" && appNo.Trim() != "")
                {
                    string sql = " SELECT * " +
                                 " FROM ilmd01 " +
                                 " WHERE D1SRNO =  '" + appNo + "' ORDER BY D1SRNO  , D1FXTM ASC With UR ";
                    ds = RetriveAsDataSet(sql);
                    CloseConnectioDAL();
                }

            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }
            return ds;

        }

        public DataSet get_ilms01_setTimeReceive(string appNo, string Branch)
        {
            DataSet ds = new DataSet();
            try
            {

                UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
                UserInfomation = m_userInfo;
                string sql = " SELECT * FROM ilms01 " +
                                " LEFT JOIN csms00 ON(p1csno=m00csn) " +
                                " WHERE p1brn = " + Branch +
                                " AND p1apno = '" + appNo + "' ";

                ds = RetriveAsDataSet(sql);
                CloseConnectioDAL();


            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }
            return ds;

        }
        // 
        public DataSet getILMS10_setReceive(string vendorDes, string vendorCode = "")
        {
            DataSet ds = new DataSet();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;
            try
            {
                string sql = "";
                if (vendorDes.Trim() != "")
                {
                    sql = " SELECT * FROM ilms10 WHERE DIGITS(p10ven) LIKE '" + vendorDes + "%'" +
                          " AND p10del <> 'X'  OR UPPER(P10TNM) LIKE  '%" + vendorDes.ToUpper() + "%'";

                }
                else if (vendorDes.Trim() == "" && vendorCode.Trim() != "")
                {
                    sql = " SELECT * FROM ilms10 WHERE p10ven = " + vendorCode +
                           " AND p10del <> 'X' ";
                }
                else
                {
                    sql = " SELECT * FROM ilms10 WHERE " +
                                 " p10del <> 'X'   ";
                }
                ds = RetriveAsDataSet(sql);
                CloseConnectioDAL();
            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }
            return ds;
        }

        //**** insert in ILMD01 //
        public string updateILMD01_setTime(string oper, string D1APDT, string D1SRNO, string D1IDNO, string D1ABRN,
                                   string D1VNID, string D1FXTM, string D1DCCM, string D1TNAM, string D1RMAK,
                                   string D1UPDT, string D1UPTM, string D1UPUS, string D1PROG, string D1WSID, string IDNO_OLD = "")
        {
            string sql = "";
            try
            {

                if (oper == "EDIT")
                {
                    sql = " UPDATE ILMD01 SET "
                         + " D1APDT = " + D1APDT + ","
                         + " D1SRNO = '" + D1SRNO + "',"
                         + " D1IDNO = '" + D1IDNO + "',"
                         + " D1ABRN = " + D1ABRN + ","
                         + " D1VNID = " + D1VNID + ","
                         + " D1FXTM = " + D1FXTM + ","
                         + " D1DCCM = '" + D1DCCM + "',"
                         + " D1TNAM =  @D1TNAM,"
                         + " D1RMAK =  @D1RMAK,"
                         //+ " D1TNAM = '" + D1TNAM + "',"
                         //+ " D1RMAK = '" + D1RMAK + "',"
                         + " D1UPDT = " + D1UPDT + ","
                         + " D1UPTM = " + D1UPTM + ","
                         + " D1UPUS = '" + D1UPUS + "',"
                         + " D1PROG = '" + D1PROG + "',"
                         + " D1WSID = '" + D1WSID + "',"
                         + " D1DEL  = '' "
                         + " WHERE D1APDT  = " + D1APDT + ""
                         + " AND D1SRNO = '" + D1SRNO + "'"
                         + " AND D1IDNO = '" + IDNO_OLD + "'"
                         + " AND D1FXTM = " + D1FXTM;
                }
                else if (oper == "INSERT")
                {
                    sql = " INSERT INTO ILMD01 (D1APDT, D1SRNO,D1IDNO,D1ABRN,D1VNID,D1FXTM,D1DCCM,D1TNAM,D1RMAK,D1UPDT,D1UPTM,D1UPUS,D1PROG,D1WSID,D1DEL) "
                          + " VALUES ("
                          + D1APDT + ","
                          + "'" + D1SRNO + "',"
                          + "'" + D1IDNO + "',"
                          + D1ABRN + ","
                          + D1VNID + ","
                          + D1FXTM + ","
                          + "'" + D1DCCM + "',"
                          + "@D1TNAM,"
                          + "@D1RMAK,"
                          //+"'" + D1TNAM +"',"
                          //+"'" + D1RMAK +"',"
                          + "'" + D1UPDT + "',"
                          + "'" + D1UPTM + "',"
                          + "'" + D1UPUS + "',"
                          + "'" + D1PROG + "',"
                          + "'" + D1WSID + "',"
                          + "       ''     )";
                }
                else if (oper == "DELETE")
                {
                    sql = " DELETE FROM ILMD01 "
                        + " WHERE  D1APDT = " + D1APDT
                        + " AND D1SRNO    = '" + D1SRNO + "'"
                        + " AND D1IDNO    = '" + D1IDNO + "'"
                        + " AND D1FXTM    = " + D1FXTM;
                }
            }
            catch (Exception ex)
            {

            }
            return sql;
        }



        // *********    Resend Bureau **********//
        // get resend 
        // edit 10/07/2558  Req:61756 
        // เปลี่ยนเงื่อนไขจาก Resend ได้ภายในวัน เป็น สามารถ Resend ย้อนหลังได้ 6 วัน
        public DataSet get_resend_bureau(string appDate, string Branch)
        {
            DataSet ds = new DataSet();
            try
            {

                UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
                UserInfomation = m_userInfo;
                if (appDate != "")
                {
                    string sql_RLMS97 = " SELECT P97CDT FROM RLMS97 WHERE P97REC = '69' ";
                    ds = RetriveAsDataSet(sql_RLMS97);
                    CloseConnectioDAL();
                    if (check_dataset(ds))
                    {

                        string RLDate97 = ds.Tables[0].Rows[0]["P97CDT"].ToString().Substring(2);


                        int year = int.Parse("25" + appDate.Substring(0, 2)) - 543;
                        DateTime Key_inDate = Convert.ToDateTime(appDate.Substring(4, 2) + "/" + appDate.Substring(2, 2) + "/" + year.ToString());
                        DateTime dateBefore = Key_inDate.AddDays(-6);

                        string[] dateSP = dateBefore.ToString("yyyy/MM/dd").Split('/');
                        string dateStart = ((int.Parse(dateSP[0]) + 543).ToString()).Substring(2) + dateSP[1] + dateSP[2];
                        string dateEnd = appDate;


                        //string sql = " SELECT brname,g76apn,g76idn,m00tnm,m00tsn,g76rdt,g76brn,m00csn,m00bdt,gnb2td,m00ebc, " +
                        //             " substr(g76rdt,5,2)||'/'||substr(g76rdt,3,2)||'/'||'25'||substr(g76rdt,1,2) appdate,BRCODE " +
                        //             " FROM gnms76 JOIN ilms01 ON(g76apn=p1apno and g76brn=p1brn)  " +
                        //             " LEFT JOIN csms00 on(m00idn=g76idn) " +
                        //             " LEFT JOIN gnmb20 on(m00ttl=gnb2tc) " +
                        //             " LEFT JOIN syfbrdes on(brcode=g76brn) " +
                        //             " WHERE g76bus= 'IL' AND g76rdt = " + appDate +
                        //             " AND g76res = 'C'   AND g76sts = '' AND g76brn = " + Branch;


                        string sql = " SELECT brname,g76apn,g76idn,m00tnm,m00tsn,g76rdt,g76brn,m00csn,m00bdt,gnb2td,m00ebc, " +
                                     " substr(g76rdt,5,2)||'/'||substr(g76rdt,3,2)||'/'||'25'||substr(g76rdt,1,2) appdate,BRCODE " +
                                     " FROM gnms76 JOIN ilms01 ON(g76apn=p1apno and g76brn=p1brn)  " +
                                     " LEFT JOIN csms00 on(m00idn=g76idn) " +
                                     " LEFT JOIN gnmb20 on(m00ttl=gnb2tc) " +
                                     " LEFT JOIN syfbrdes on(brcode=g76brn) " +
                                     " WHERE g76bus= 'IL'  " +
                                     " AND g76rdt >=  " + RLDate97 +
                                     " AND g76rdt BETWEEN " + dateStart + " AND " + dateEnd +
                                     " AND g76res = 'C' AND P1APPT = '01' AND g76sts = '' AND g76brn = " + Branch;


                        ds = RetriveAsDataSet(sql);
                        CloseConnectioDAL();
                    }

                }

            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }
            return ds;

        }
        public DataSet checkDataResendBureau(string appNo, string Branch)
        {
            DataSet ds = new DataSet();
            try
            {

                UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
                UserInfomation = m_userInfo;


                string sql = " SELECT m00idn,m00csn,gnb2td,m00tnm,m00tsn,m00bdt,p1appt,P1APNO,P1BRN" +
                                 " FROM ilms01 " +
                                 " LEFT JOIN csms00 ON(p1csno=m00csn) " +
                                 " LEFT JOIN gnmb20 ON(m00ttl=gnb2tc) " +
                                 " WHERE p1brn = " + Branch + " AND p1apno = " + appNo;

                ds = RetriveAsDataSet(sql);
                CloseConnectioDAL();



            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }
            return ds;

        }

        public DataSet getGNMS76(ILDataCenter ilObj, string biz, string appNo, string Branch)
        {
            DataSet ds = new DataSet();
            try
            {

                //UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
                //UserInfomation = m_userInfo;
                //iDB2Command cmd = new iDB2Command();
                //cmd.Parameters.Clear();
                //cmd.CommandText = " SELECT * FROM gnms76 " +
                //                   " WHERE g76bus = @biz  " +
                //                   " AND g76brn = @Branch " +
                //                   " AND g76apn = @appNo  " ;
                //cmd.Parameters.Add("@biz", biz);
                //cmd.Parameters.Add("@Branch", Branch);
                //cmd.Parameters.Add("@appNo", appNo);

                // ds = ilObj.ExecuteDataReader_to_DataSet_iSeries(cmd);
                //iDB2DataReader db2 = cmd.ExecuteReader();
                //DataTable dt = new DataTable();
                //dt.Load(db2);
                //ds.Tables.Add(dt);


                string sql = " SELECT g76bus FROM gnms76 " +
                             " WHERE g76bus = '" + biz + "'" +
                             " AND g76brn = " + Branch +
                             " AND g76apn = " + appNo;


                ds = ilObj.RetriveAsDataSetNoConnect(sql);
                //CloseConnectioDAL();



            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }
            return ds;

        }

        public void resendBureau(ILDataCenter ilObj, string date97, string app, string brn, ref bool resBureau, ref string resNCB, ref string detail)
        {
            try
            {
                if (app != "" && brn != "")
                {
                    //*** check from ilms01 ***//

                    DataSet ds_01 = ilObj.checkDataResendBureau(app, brn);

                    if (ilObj.check_dataset(ds_01))
                    {
                        DataRow dr_01 = ds_01.Tables[0].Rows[0];
                        UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
                        ilObj.UserInfomation = m_userInfo;
                        string bureau_prm = "";
                        string ErrorCode = "";
                        string Error = "";
                        string crreview_prm = "";

                        bool call_GNSR69 = ilObj.Call_GNSR69("IL", brn, dr_01["P1APNO"].ToString().Trim(), dr_01["m00idn"].ToString().Trim(), dr_01["m00csn"].ToString(),
                                                              dr_01["M00TNM"].ToString().Trim() + " " + dr_01["M00TNM"].ToString().Trim(),
                                                              dr_01["M00BDT"].ToString().Trim(), "",
                                                              ref bureau_prm, ref crreview_prm, ref ErrorCode, ref Error, m_userInfo.BizInit, m_userInfo.BranchNo);
                        ilObj.CloseConnectioDAL();

                        resNCB = bureau_prm;
                        string sql_resNCB = " SELECT g101ed FROM gntb101 WHERE g101cd = '" + bureau_prm.Trim() + "'";
                        DataSet ds_reslNCB = ilObj.RetriveAsDataSetNoConnect(sql_resNCB);
                        if (ilObj.check_dataset(ds_reslNCB))
                        {
                            DataRow dr_resNCB = ds_reslNCB.Tables[0].Rows[0];
                            detail = dr_resNCB["g101ed"].ToString().Trim();
                        }

                        bool save_76 = false;



                        DataSet ds_76 = ilObj.getGNMS76(ilObj, "IL", dr_01["P1APNO"].ToString().Trim(), dr_01["P1BRN"].ToString().Trim());
                        if (ilObj.check_dataset(ds_76))
                        {
                            save_76 = true;
                        }
                        //** ilms01hs **//

                        iDB2Command cmd = new iDB2Command();
                        cmd.Parameters.Clear();
                        cmd.CommandText = ilObj.InsertILMS01HS_autoPay(m_UdpD.ToString(), m_UpdTime.ToString(), m_userInfo.Username, m_userInfo.LocalClient, dr_01["P1BRN"].ToString().Trim(), dr_01["P1APNO"].ToString().Trim());

                        int res_01HS = ilObj.ExecuteNonQuery(cmd);
                        if (res_01HS == -1)
                        {
                            ilObj.RollbackDAL();
                            ilObj.CloseConnectioDAL();

                            resBureau = false;
                            return;
                        }
                        if (bureau_prm != "C" && save_76)
                        {
                            // update gnms76
                            cmd.Parameters.Clear();
                            cmd.CommandText = ilObj.updateGNMS76(bureau_prm, "IL", brn, app);
                            int res_gnms76 = ilObj.ExecuteNonQuery(cmd);
                            if (res_gnms76 == -1)
                            {
                                ilObj.RollbackDAL();
                                ilObj.CloseConnectioDAL();

                                resBureau = false;
                                return;
                            }

                        }

                        if (bureau_prm == "B" || bureau_prm == "R" || bureau_prm == "O" || bureau_prm == "S" ||
                            bureau_prm == "L")
                        {
                            // insert note ilms38
                            string res_code = "";
                            if (bureau_prm == "B") { res_code = "BL3"; }
                            if (bureau_prm == "R" || bureau_prm == "O") { res_code = "IL3"; }
                            if (bureau_prm == "S") { res_code = "LL9"; }
                            if (bureau_prm == "L") { res_code = "IL7"; }

                            string sql_reslCode = " SELECT g25des FROM gntb25 WHERE g25rcd = '" + res_code + "'";
                            DataSet ds_reslCode = ilObj.RetriveAsDataSetNoConnect(sql_reslCode);
                            if (!ilObj.check_dataset(ds_reslCode))
                            {
                                ilObj.RollbackDAL();
                                ilObj.CloseConnectioDAL();

                                resBureau = false;
                                return;
                            }
                            DataRow dr_reslCode = ds_reslCode.Tables[0].Rows[0];
                            Connect_NoteAPI noteAPI = new Connect_NoteAPI();

                            string ErrMsgNote = "";
                            //bool AddNote = ilObj.CALL_CSSRW11("IL", dr_01["M00IDN"].ToString().Trim(), "ADD", res_code, dr_reslCode["g25des"].ToString().Trim(), "", ref ErrMsgNote, m_userInfo.BizInit, m_userInfo.BranchNo);
                            var resNote = noteAPI.AddNote(dr_01["M00IDN"].ToString().Trim(), "0", "ADD", res_code, dr_reslCode["g25des"].ToString().Trim(), m_UpdDate.ToString().Trim(), m_UpdTime.ToString().Trim()).Result;

                            if (!resNote.success || ErrMsgNote.Trim() != "")
                            {
                                //L_message.Text = "Cannot Save note : " + ErrMsgNote.ToString().Trim();
                                //ilObj.RollbackDAL();
                                //ilObj.CloseConnectioDAL();
                                resBureau = false;
                                return;
                            }

                            string sql_csms11 = " SELECT m00dsn,m11zip FROM csms00 " +
                                                " LEFT JOIN csms11 ON (m00csn = m11csn AND m00dsn = m11cde AND m11ref = '') " +
                                                " where m00csn = " + dr_01["M00CSN"].ToString().Trim();

                            DataSet ds_csms11 = ilObj.RetriveAsDataSetNoConnect(sql_csms11);
                            string chk_zip = "";
                            string chk_dsn = "";
                            if (ilObj.check_dataset(ds_csms11))
                            {
                                DataRow dr_ms11 = ds_csms11.Tables[0].Rows[0];  //
                                chk_dsn = dr_ms11["m00dsn"].ToString();
                                chk_zip = dr_ms11["m11zip"].ToString();

                            }

                            if (chk_dsn.Trim() != "")
                            {
                                string sql_csms11wk = " SELECT m11idn FROM csms11wk " +
                                                      " WHERE m11idn = '" + dr_01["M00IDN"].ToString().Trim() + "'" +
                                                      " AND m11flg   = 'R' " +
                                                      " AND m11ebc   = " + app;
                                DataSet ds_resCSMS11wk = ilObj.RetriveAsDataSetNoConnect(sql_csms11wk);
                                if (ilObj.check_dataset(ds_resCSMS11wk))
                                {
                                    ilObj.RollbackDAL();
                                    ilObj.CloseConnectioDAL();
                                    resBureau = false;
                                    return;
                                }
                                //DataRow dr_csms11 = ds_resCSMS11wk.Tables[0].Rows[0];
                                //string zipCode = dr_csms11["m11zip"].ToString().Trim();
                                //string dsn = dr_csms11["m00dsn"].ToString().Trim();

                                string insertCSMS11wk = " Insert into csms11wk " +
                                                      " (m11brn,m11idn,m11ebc,m11csn,m11dsn,m11flg,m11zip," +
                                                      " m11udt,m11utm,m11uus,m11uws,m11upg) " +
                                                      " VALUES ( " +
                                                      brn + "," +
                                                      "'" + dr_01["M00IDN"].ToString().Trim() + "'," +
                                                      app + "," +
                                                      dr_01["M00CSN"].ToString().Trim() + "," +
                                                      "'" + chk_dsn + "'," +
                                                      "'R'," +
                                                      chk_zip + "," +
                                                      m_UdpD + "," +
                                                      m_UpdTime + "," +
                                                      "'" + m_userInfo.Username + "'," +
                                                      "'" + m_userInfo.LocalClient + "'," +
                                                      "'IL_RESEND')";

                                cmd.Parameters.Clear();
                                cmd.CommandText = insertCSMS11wk;
                                int res_insertCsms11 = ilObj.ExecuteNonQuery(cmd);
                                if (res_insertCsms11 == -1)
                                {
                                    ilObj.RollbackDAL();
                                    ilObj.CloseConnectioDAL();
                                    resBureau = false;
                                    return;
                                }
                            }
                        }
                        else
                        {
                            // insert csms381//
                            if (bureau_prm != "C")
                            {
                                //resBureau = true;
                                //return;

                                //resBureau = false;
                                string res_code = "";
                                if (bureau_prm == "P" || bureau_prm == "T")
                                {
                                    res_code = "PASS";
                                }
                                if (bureau_prm == "D")
                                {
                                    res_code = "NTE";
                                }
                                if (bureau_prm == "N")
                                {
                                    res_code = "NF";
                                }

                                string desc = "";
                                if (res_code != "NTE")
                                {
                                    string sql_reslCode = " SELECT g25des FROM gntb25 WHERE g25rcd = '" + res_code + "'";
                                    //string sql_reslCode = " SELECT g101ed FROM gntb101 WHERE g101cd = '" + res_code + "'";
                                    DataSet ds_reslCode = ilObj.RetriveAsDataSetNoConnect(sql_reslCode);

                                    if (!ilObj.check_dataset(ds_reslCode))
                                    {
                                        desc = " ไม่ได้ทำการตรวจสอบข้อมูลเครดิตบูโร ";
                                    }
                                    else
                                    {
                                        DataRow dr_reslCode = ds_reslCode.Tables[0].Rows[0];
                                        desc = dr_reslCode["g25des"].ToString().Trim();
                                    }

                                }

                                Connect_NoteAPI noteAPI = new Connect_NoteAPI();
                                string ErrMsgNote = "";
                                bool AddNote = ilObj.CALL_CSSRW11("IL", dr_01["M00IDN"].ToString().Trim(), "NCB", res_code, desc, "", ref ErrMsgNote, m_userInfo.BizInit, m_userInfo.BranchNo);
                                var resNote = noteAPI.AddNote(dr_01["M00IDN"].ToString().Trim(), "0", "ADD", res_code, desc, m_UpdDate.ToString().Trim(), m_UpdTime.ToString().Trim()).Result;

                                if (!resNote.success || ErrMsgNote.Trim() != "")
                                {
                                    
                                    //L_message.Text = "Cannot Save note : " + ErrMsgNote.ToString().Trim();
                                    //ilObj.RollbackDAL();
                                    //ilObj.CloseConnectioDAL();
                                    resBureau = false;
                                    return;
                                }
                            }
                            resBureau = true;
                        }
                        //  save data to ilms01
                        if (bureau_prm == "B")
                        {
                            string Error_GNSRBLC = "";
                            string ErrorMsg_GNSRBLC = "";
                            bool call_GNSRBLC = ilObj.Call_GNSRBLC(dr_01["M00IDN"].ToString().Trim(), dr_01["M00CSN"].ToString().Trim(), "IL", "005", "15",
                                                                    ref Error_GNSRBLC, ref ErrorMsg_GNSRBLC, m_userInfo.BizInit, m_userInfo.BranchNo);
                            if (ErrorMsg_GNSRBLC.ToString().Trim() != "")
                            {
                                ilObj.RollbackDAL();
                                ilObj.CloseConnectioDAL();
                                resBureau = false;
                                return;
                            }

                            string updateILMS01 = " UPDATE ilms01 SET p1loca = '210', p1aprj = 'RJ', p1rsts = 'B', p1resn='BL3', " +
                                                  " p1fill = Substr(p1fill,1,20)||'" + bureau_prm + "'||Substr(p1fill,22,length(p1fill)), " +
                                                  " P1STDT = " + date97 + " ," +  // เพิ่มวันที่ Status date เมื่อทำการ Reject
                                                  " P1STTM = " + m_UdpT + " ," +  // เพิ่มวันที่ Status date เมื่อทำการ Reject
                                                  " p1avdt = " + date97 + ", " +  // เพิ่มวันที่ Approve date เมื่อทำการ Reject
                                                  " p1avtm = " + m_UdpT + ", " +  // เพิ่มวันที่ Approve time เมื่อทำการ Reject
                                                  " p1updt = " + m_UdpD + " ,p1uptm = " + m_UdpT + ",p1upus ='" + m_UserInfo.Username + "'," +
                                                  " p1prog  = 'IL_RESEND' , p1wsid = '" + m_UserInfo.LocalClient + "'" +
                                                  " WHERE   p1brn = " + m_UserInfo.BranchApp + " AND p1apno = " + app;

                            cmd.Parameters.Clear();
                            cmd.CommandText = updateILMS01;
                            int res_updateILMS01 = ilObj.ExecuteNonQuery(cmd);
                            if (res_updateILMS01 == -1)
                            {
                                ilObj.RollbackDAL();
                                ilObj.CloseConnectioDAL();
                                resBureau = false;
                                return;
                            }
                        }

                        if (bureau_prm == "L" || bureau_prm == "R" || bureau_prm == "O" || bureau_prm == "S")
                        {
                            string p1resn = "";
                            if (bureau_prm == "L") { p1resn = "IL7"; }
                            else if (bureau_prm == "R" || bureau_prm == "O") { p1resn = "IL3"; }
                            else if (bureau_prm == "S") { p1resn = "LL9"; }

                            string updateILMS01 = " UPDATE ilms01 SET " +
                                                 " p1loca = '210', p1aprj = 'RJ', p1resn =  '" + p1resn + "' ," +
                                                 " p1fill = Substr(p1fill,1,20)||'" + bureau_prm + "'||Substr(p1fill,22,length(p1fill)) , " +
                                                 " P1STDT = " + date97 + " ," +  // เพิ่มวันที่ Status date เมื่อทำการ Reject
                                                 " P1STTM = " + m_UdpT + " ," +
                                                 " p1avdt = " + date97 + ", " +  // เพิ่มวันที่ Approve date เมื่อทำการ Reject
                                                 " p1avtm = " + m_UdpT + ", " +  // เพิ่มวันที่ Approve time เมื่อทำการ Reject
                                                 " p1updt = " + m_UdpD + " ,p1uptm = " + m_UdpT + ",p1upus ='" + m_UserInfo.Username + "'," +
                                                 " p1prog  = 'IL_RESEND' , p1wsid = '" + m_UserInfo.LocalClient + "'" +
                                                 " WHERE   p1brn = " + m_UserInfo.BranchApp + " AND p1apno = " + app;

                            cmd.Parameters.Clear();
                            cmd.CommandText = updateILMS01;
                            int res_updateILMS01 = ilObj.ExecuteNonQuery(cmd);
                            if (res_updateILMS01 == -1)
                            {
                                ilObj.RollbackDAL();
                                ilObj.CloseConnectioDAL();
                                resBureau = false;
                                return;
                            }
                        }
                        else if (bureau_prm == "P" || bureau_prm == "N" || bureau_prm == "D" || bureau_prm == "C")
                        {
                            string updateILMS01 = " UPDATE ilms01 SET p1fill = Substr(p1fill,1,20)||'" + bureau_prm + "'||Substr(p1fill,22,length(p1fill)) " +
                                                 " ,p1updt = " + m_UdpD + " ,p1uptm = " + m_UdpT + ",p1upus ='" + m_UserInfo.Username + "'," +
                                                 " p1prog  = 'IL_RESEND' , p1wsid = '" + m_UserInfo.LocalClient + "'" +
                                                 " WHERE   p1brn = " + m_UserInfo.BranchApp + " AND p1apno = " + app;

                            cmd.Parameters.Clear();
                            cmd.CommandText = updateILMS01;
                            int res_updateILMS01 = ilObj.ExecuteNonQuery(cmd);
                            if (res_updateILMS01 == -1)
                            {
                                ilObj.RollbackDAL();
                                ilObj.CloseConnectioDAL();
                                resBureau = false;
                                return;
                            }

                        }
                        resBureau = true;

                        ilObj.CommitDAL();
                        ilObj.CloseConnectioDAL();
                        return;

                    }

                }
                ilObj.RollbackDAL();
                ilObj.CloseConnectioDAL();
                resBureau = false;
                return;
            }
            catch (Exception ex)
            {
                ilObj.RollbackDAL();
                ilObj.CloseConnectioDAL();
                resBureau = false;
                return;
            }
        }

        public string updateGNMS76(string res_bur, string biz, string brn, string app_no)
        {
            string sql = "";
            try
            {
                sql = " UPDATE gnms76 SET g76res = '" + res_bur + "' " +
                             " WHERE  g76bus = '" + biz + "' AND g76brn = '" + brn + "'" +
                             " AND    g76apn = " + app_no;

            }
            catch (Exception ex)
            {

            }
            return sql;
        }





        //**************Cancel Unsign***************//
        public DataSet get_cust_cancel_unsign(string s_search, string Branch, string search_by)
        {
            DataSet ds = new DataSet();
            try
            {

                UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
                UserInfomation = m_userInfo;

                string where = " WHERE p2brn= " + Branch + " AND ";
                if (search_by == "1")
                {
                    where += " p2apno = " + s_search;
                }
                else if (search_by == "2")
                {
                    where += " p2cont = " + s_search;

                }

                string sql = " SELECT m00idn,m00tnm,m00tsn,m00sex,m00age, digits(m00ebc) m00ebc,m00bdt, " +
                             " p2brn,p2cont,p2csno,p2apno,digits(p2cont) p2cont1,p3crlm,p3pcam,p2pcam, " +
                             " p2toam,p2item,p2qty,t44des,p2bkdt " +
                             " FROM ilms02 " +
                             " LEFT JOIN csms00 on(p2csno=m00csn) " +
                             " LEFT JOIN ilms03 on(p2csno=p3csno) " +
                             " LEFT JOIN iltb44 on(p2item=t44itm) " +
                             where +
                             " AND  p2loca in('250','255') AND p2del = '' with UR ";



                ds = RetriveAsDataSet(sql);
                CloseConnectioDAL();



            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }
            return ds;

        }

        public DataSet get_csms032(string csn, string brn, string appNo)
        {
            DataSet ds = new DataSet();
            try
            {

                UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
                UserInfomation = m_userInfo;

                string sql = " SELECT d3osal FROM csms032 JOIN csms13  " +
                             " ON  d3csno = m13csn " +
                             " WHERE d3csno=" + csn + " AND d3flag = 'C' " +
                             " AND m13app = 'IL' " +
                             " AND m13brn= " + brn +
                             " AND m13apn = " + appNo +
                             " AND substr(m13fil,20,1) = 'C' ";

                ds = RetriveAsDataSet(sql);
                CloseConnectioDAL();

            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }
            return ds;

        }

        public DataSet get_ilms03(string csn)
        {
            DataSet ds = new DataSet();
            try
            {

                //UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
                //UserInfomation = m_userInfo;

                string sql = " SELECT p3pcam FROM ilms03 WHERE p3csno = " + csn;

                ds = RetriveAsDataSetNoConnect(sql);
                //CloseConnectioDAL();

            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }
            return ds;

        }

        public DataSet get_csms20(string csn)
        {
            DataSet ds = new DataSet();
            try
            {

                //UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
                //UserInfomation = m_userInfo;

                string sql = " SELECT m20avi FROM csms20 WHERE m20csn = " + csn;

                ds = RetriveAsDataSetNoConnect(sql);
                //CloseConnectioDAL();

            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }
            return ds;

        }

        public string insertCSMS20HS(string csn)
        {
            string sql = "";
            try
            {
                sql = " INSERT INTO csms20hs SELECT * FROM csms20 " +
                             " where m20csn = " + csn +
                             " and m20sts = '' ";
                return sql;
            }
            catch (Exception ex)
            {
                return sql;
            }
        }
        public string updateCSMS20(string M20AVI, string csn, string M20UPG, string userName)
        {
            string sql = "";
            try
            {
                //UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
                //UserInfomation = m_userInfo;

                sql = "  UPDATE CSMS20 SET " +
                      "  M20AVI = " + M20AVI + ", " +
                      "  M20UDT = " + m_UdpD + "," +
                      "  M20UTM = " + m_UdpT + "," +
                      "  M20UUS = '" + userName + "'," +
                      "  M20UPG = '" + M20UPG + "' " +
                      "  WHERE m20csn = " + csn + " AND m20sts = '' ";
            }
            catch (Exception ex)
            {

            }
            return sql;
        }
        public string insertILMS03HS(string csn, string UserName, string wrkstn)
        {
            string sql = "";
            try
            {
                //UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
                //UserInfomation = m_userInfo;
                sql = " INSERT INTO  ilms03hs " +
                      " SELECT  " + m_UdpD + "," + m_UpdTime + "," +
                      "'" + UserName + "'," +
                      "'" + wrkstn + "'," +
                      " ilms03.* " +
                      " FROM ilms03 " +
                      " WHERE p3csno = " + csn;

            }
            catch (Exception ex)
            {

            }
            return sql;
        }

        public string UpdateILMS03(string P3PCAM, string csn, string P3PGMN, string UserName, string wrkstn)
        {
            string sql = "";
            try
            {
                //UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
                //UserInfomation = m_userInfo;
                sql = " UPDATE ILMS03 SET " +
                      " P3PCAM = " + P3PCAM + "," +
                      " P3UPDT = " + m_UpdDate + "," +
                      " P3UPTM = " + m_UpdTime + "," +
                      " P3USER = '" + UserName + "'," +
                      " p3ddsp = '" + wrkstn + "', " +
                      " P3PGMN = '" + P3PGMN + "' " +
                      " WHERE  P3CSNO = " + csn;
            }
            catch (Exception ex)
            {

            }
            return sql;
        }

        public string UpdateILMS01_cancelUnsign(string date_User, string brn, string appNo, string p1prog, string UserName, string wrkstn)
        {
            string sql = "";
            try
            {
                //UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
                //UserInfomation = m_userInfo;
                sql = " UPDATE ILMS01 SET " +
                      " p1loca = '300', " +
                      " p1rsts = 'X' ," +
                      " p1fill = '" + date_User + "'||Substr(p1fill,length('" + date_User + "')+1,length(p1fill)), " +
                      " p1updt = " + m_UdpD + "," +
                      " p1upus = '" + UserName + "'," +
                      " p1uptm = " + m_UpdTime + "," +
                      " p1prog = '" + p1prog + "' " + "," +
                      " p1wsid = '" + wrkstn + "'" +
                      " WHERE p1brn = " + brn +
                      " AND p1apno = " + appNo;
            }
            catch (Exception ex)
            {

            }
            return sql;
        }

        public string insertILMS02HS(string brn, string cont)
        {
            string sql = "";
            try
            {
                UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
                UserInfomation = m_userInfo;
                sql = " INSERT INTO  ilms02hs " +
                      " SELECT  " + m_UdpD + "," + m_UpdTime + "," +
                      "'" + m_userInfo.Username + "'," +
                      "'" + m_userInfo.LocalClient + "'," +
                      " ilms02.* " +
                      " FROM ilms02 " +
                      " WHERE  p2brn = " + brn +
                      " AND p2cont = " + cont;
            }
            catch (Exception ex)
            {

            }
            return sql;
        }

        public string updateILMS02(string brn, string cont, string appNo, string p2prog)
        {
            string sql = "";
            try
            {
                UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
                UserInfomation = m_userInfo;
                sql = " UPDATE ilms02 SET " +
                      " p2loca = '300' ," +
                      " p2del  = 'X', " +
                      " p2lmvd = " + m_UdpD + "," +
                      " p2updt = " + m_UdpD + "," +
                      " p2uptm = " + m_UpdTime + "," +
                      " p2user = '" + m_userInfo.Username + "'," +
                      " p2prog = '" + p2prog + "'," +
                      " p2ddsp = '" + m_userInfo.LocalClient + "'" +
                      " WHERE  p2brn = " + brn +
                      " AND    p2cont = " + cont +
                      " AND    p2apno = " + appNo;

            }
            catch (Exception ex)
            {

            }
            return sql;
        }










        //******  Control Document *******//
        public DataSet get_Cust_Control(string Branch)
        {
            DataSet ds = new DataSet();
            try
            {

                UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
                UserInfomation = m_userInfo;

                string sql = " SELECT p2brn,digits(p2cont) p2cont,p2vdid,p2appt,p2csno,p10nam, TRIM(GNB2TD) || TRIM(M00TNM) ||'  '||TRIM(M00TSN) cust_name,M00EBC  " +
                                 " FROM ilms02 " +
                                 " LEFT JOIN ilms10 on(p2vdid=p10ven)  " +
                                 " JOIN CSMS00 ON (p2csno = m00csn) " +
                                 " LEFT JOIN GNMB20 ON (m00ttl = GNB2TC) " +
                                 " WHERE p2brn = " + Branch +
                                 " AND p2stdc= 'N' AND " +
                                 " p2del <> 'X' and p2loca='275' ";

                ds = RetriveAsDataSet(sql);
                CloseConnectioDAL();

                //}

            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }
            return ds;

        }

        public DataSet get_Doc_Control(string appType, string cont)
        {
            DataSet ds = new DataSet();
            try
            {

                UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
                UserInfomation = m_userInfo;


                string sql = " SELECT t18app,t18cod,t05dst,t18upd,t18upt,t18upu,d02del,d02sts " +
                             " FROM iltb18l1 " +
                             " LEFT JOIN iltb05 ON (t18cod=t05cod) " +
                             " LEFT JOIN ilmd02 ON (t18cod=d02cod) " +
                             " WHERE d02con = " + cont +
                             " AND t18app= '" + appType + "' ";

                ds = RetriveAsDataSet(sql);
                CloseConnectioDAL();


            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }
            return ds;

        }
        public string update_ilmd02Del(string user, string localClient, string con, string docCode)
        {
            string sql = "";
            try
            {
                sql = " update ilmd02 set " +
                      " d02del = 'X', " +
                      " d02upd = " + m_UdpD + ", " +
                      " d02upt = " + m_UpdTime + " , " +
                      " d02upu = '" + user + "', " +
                      " d02upw = '" + localClient + "' " +
                      " WHERE d02con = " + con + " AND " +
                      " d02cod= '" + docCode + "'";
            }
            catch (Exception ex)
            {

            }
            return sql;
        }
        public string update_ilmd02(string user, string localClient, string con, string docCode)
        {
            string sql = "";
            try
            {
                sql = " update ilmd02 set " +
                      " d02del = '', " +
                      " d02upd = " + m_UdpD + ", " +
                      " d02upt = " + m_UpdTime + " , " +
                      " d02upu = '" + user + "', " +
                      " d02upw = '" + localClient + "' " +
                      " WHERE d02con = " + con + " AND " +
                      " d02cod= '" + docCode + "'";
            }
            catch (Exception ex)
            {

            }
            return sql;
        }
        public string updateILMS02ControlDoc(string brn, string con)
        {
            string sql = "";
            try
            {
                sql = " UPDATE ilms02 set p2stdc = 'Y' " +
                      " WHERE p2brn = " + brn + " AND " +
                      " p2cont = " + con;
            }
            catch (Exception ex)
            {

            }
            return sql;
        }








        //****** Update Auto pay *********//
        public DataSet get_cust_autoPay(string conType, string ID, string app, string ebc,
                                        string cust, string cont, string bank, string brn,
                                        string dateFrom, string dateTo)
        {
            DataSet ds = new DataSet();
            try
            {

                UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
                UserInfomation = m_userInfo;
                string where = "";
                string cond = "";
                if (ID.Trim() != "")
                {
                    cond += " AND m00idn = '" + ID + "'";
                }
                if (app.Trim() != "")
                {
                    cond += " AND p1apno = '" + app + "'";
                }
                if (ebc.Trim() != "")
                {
                    cond += " AND m00ebc = '" + ebc + "'";
                }
                if (cust.Trim() != "")
                {
                    cond += " AND p1csno = '" + cust + "'";
                }
                if (cont.Trim() != "")
                {
                    cond += " AND p1cont =  " + cont + "";
                }
                if (bank.Trim() != "")
                {
                    cond += " AND p1pbcd = '" + bank + "'";
                }
                if (brn.Trim() != "")
                {
                    cond += " AND p1brn =  " + brn + "";
                }


                if (conType == "CHANGE")
                {
                    where = " WHERE ";
                    if (dateFrom == dateTo)
                    {
                        where += " p1cndt = " + dateFrom;
                    }
                    else if (dateFrom != dateTo)
                    {
                        where += " p1cndt BETWEEN " + dateFrom
                                 + " AND " + dateTo;
                    }
                }
                else if (conType == "SEND")
                {
                    where = " WHERE p1rsts='' ";
                    if (dateFrom == dateTo)
                    {
                        where += " AND p1cndt = " + dateFrom;
                    }
                    else if (dateFrom != dateTo)
                    {
                        where += " AND p1cndt BETWEEN " + dateFrom
                                 + " AND " + dateTo;
                    }
                    where += " AND p1loca = '275' and p1payt = '1' ";
                }
                else if (conType == "READY")
                {
                    where = " WHERE p1rsts='' ";
                    if (dateFrom == dateTo)
                    {
                        where += " AND P00STD = " + dateFrom;
                    }
                    else
                    {
                        where += " AND p00std BETWEEN  " + dateFrom
                               + " AND " + dateTo;
                    }
                    where += " AND p1payt = 'W' ";
                }


                string sql = " SELECT gnb2td ||m00tnm ||'  '|| m00tsn fName ,m00idn,p1apno,p1pram,p1csno,p1vdid, " +
                             " p10nam,p1payt,p1pbcd,p1pbrn,p1paty,p1pano,digits(p1cont) p1cont, " +
                             " p1brn,p00bst,p00doc " +
                             " FROM ilms01 " +
                             " LEFT JOIN ilms00 ON (p1csno=p00cis and p1pbcd=p00bnk AND " +
                             " p1pbrn=p00bbr AND p1pano=p00bac ) " +
                             " LEFT JOIN csms00 ON (p1csno=m00csn) " +
                             " LEFT JOIN ilms10 ON (p1vdid=p10ven) " +
                             " LEFT JOIN gnmb20 ON (m00ttl=gnb2tc) " +
                              where + cond;

                ds = RetriveAsDataSet(sql);
                CloseConnectioDAL();


            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }
            return ds;

        }

        public DataSet get_pamentType_autoPay(string csn)
        {
            DataSet ds = new DataSet();
            try
            {
                UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
                UserInfomation = m_userInfo;

                string sql = " SELECT p00bnk,gnb31d,p00bac,p00bbr,p00aty,p00doc FROM ilms00 " +
                             " lEFT JOIN gnmb31 ON (gnb31a=p00bnk and gnb31c=p00bbr) " +
                             " WHERE p00cis = " + csn +
                             " AND p00sts='A' ";

                ds = RetriveAsDataSet(sql);
                CloseConnectioDAL();





            }
            catch (Exception ex)
            {
            }
            return ds;
        }

        public DataSet get_lims00_sendToBank(string csn, string bankCode, string bankBranch, string bankAcc)
        {
            DataSet ds = new DataSet();
            try
            {
                UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
                UserInfomation = m_userInfo;

                string sql = " SELECT p00cis FROM ilms00 " +
                             " WHERE p00cis= " + csn +
                             " AND p00bnk = '" + bankCode + "'" +
                             " AND p00bbr = '" + bankBranch + "'" +
                             " AND p00bac = '" + bankAcc + "' ";

                ds = RetriveAsDataSet(sql);
                CloseConnectioDAL();

            }
            catch (Exception ex)
            {
            }
            return ds;
        }
        public string InsertILMS00HS_autoPay(string date, string time, string user, string localClient, string p00cis, string p00bnk, string p00bbr, string p00bac)
        {
            string sql = "";
            try
            {
                sql = " INSERT INTO ilms00hs " +
                        " SELECT " + date + "," + time + "," +
                        "'" + user + "','" + localClient + "'," +
                        " ilms00.* " +
                        " FROM ilms00 " +
                        " WHERE p00cis = " + p00cis +
                        " AND p00bnk = '" + p00bnk + "'" +
                        " AND p00bbr = '" + p00bbr + "'" +
                        " AND p00bac = '" + p00bac + "'";

            }
            catch (Exception ex)
            {

            }
            return sql;
        }
        public string InsertILMS01HS_autoPay(string date, string time, string user, string localClient, string brn, string appNo)
        {
            string sql = "";
            try
            {
                sql = " INSERT INTO ilms01hs " +
                        " SELECT " + date + "," + time + "," +
                        "'" + user + "','" + localClient + "'," +
                        " ilms01.* " +
                        " FROM ilms01 " +
                        " WHERE p1brn = " + brn +
                        " AND p1apno = " + appNo;
            }
            catch (Exception ex)
            {

            }
            return sql;
        }
        public string UpdateILMS00_autoPay(string p00cis, string p00sts, string p00std, string p00efd, string p00bnk, string p00bbr, string p00bac, string p00aty, string p00doc, string p00bst, string p00upd, string p00upt, string p00usr, string p00upg, string p00uws, string oper, string sqlSTS, string p00cnt)
        {
            string sql = "";
            try
            {
                // SEND
                if (oper == "SEND")
                {
                    if (sqlSTS == "INSERT")
                    {

                        sql = " INSERT INTO ILMS00 " +
                                     " (p00cis,p00sts,p00std,p00efd,p00bnk,p00bbr,p00bac, " +
                                     " p00aty,p00doc,p00bst, " +
                                     " p00upd,p00upt,p00usr,p00upg,p00uws,p00cnt) " +
                                     " values( " +
                                     p00cis + "," +
                                     "'" + p00sts + "'," +
                                     p00std + "," +
                                     p00efd + "," +
                                     "'" + p00bnk + "'," +
                                     "'" + p00bbr + "'," +
                                     "'" + p00bac + "'," +
                                     "'" + p00aty + "'," +
                                     "'" + p00doc + "'," +
                                     "'" + p00bst + "'," +
                                     p00upd + "," +
                                     p00upt + "," +
                                     "'" + p00usr + "'," +
                                     "'" + p00upg + "'," +
                                     "'" + p00uws + "'," +
                                     "'" + p00cnt + "' " +
                                     " )";


                    }
                    else if (sqlSTS == "UPDATE")
                    {
                        sql = " UPDATE ilms00 SET " +
                              " p00sts = '" + p00sts + "' , " +
                              " p00std = " + p00std +
                              " WHERE p00cis =" + p00cis +
                              " AND p00bnk = '" + p00bnk + "'" +
                              " AND p00bbr = '" + p00bbr + "'" +
                              " AND p00bac = '" + p00bac + "'" +
                              " AND p00cnt = '" + p00cnt + "'";

                    }
                } // Ready
                else if (oper == "READY")
                {
                    if (sqlSTS == "UPDATE")
                    {
                        sql = " UPDATE ilms00 SET " +
                              " p00sts = '" + p00sts + "' , " +
                              " p00bst = '" + p00bst + "'," +
                              " p00std = " + p00std + "," +
                              " p00upd = " + p00upd + "," +
                              " p00upt = " + p00upt + "," +
                              " p00usr = '" + p00usr + "'," +
                              " p00upg = '" + p00upg + "'," +
                              " p00uws = '" + p00uws + "'" +
                              " WHERE p00cis =" + p00cis +
                              " AND p00cnt = '" + p00cnt + "'";
                    }
                }

                else if (oper == "CHANGE")
                {
                    if (sqlSTS == "UPDATE")
                    {
                        sql = " UPDATE ilms00 SET " +
                              " p00sts = '" + p00sts + "' , " +
                              " p00std = " + p00std + "," +
                              " p00upd = " + p00upd + "," +
                              " p00upt = " + p00upt + "," +
                              " p00usr = '" + p00usr + "'," +
                              " p00upg = '" + p00upg + "'," +
                              " p00uws = '" + p00uws + "'" +
                              " WHERE p00cis =" + p00cis +
                              " AND p00cnt = '" + p00cnt + "'";
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return sql;
        }
        public string UpdateILMS01_autoPay(string p1payt, string p1pbcd, string p1pbrn, string p1paty, string p1pano, string p1updt, string p1uptm,
                                           string p1upus, string p1prog, string p1wsid, string p1apno, string p1cont, string oper)
        {
            string sql = "";
            try
            {

                if (oper == "SEND" || oper == "CHANGE")
                {

                    sql = " UPDATE ilms01 SET " +
                                 " p1payt = '" + p1payt + "' , " +
                                 " p1pbcd = '" + p1pbcd + "'," +
                                 " p1pbrn = '" + p1pbrn + "'," +
                                 " p1paty = '" + p1paty + "'," +
                                 " p1pano = '" + p1pano + "'," +
                                 " p1updt =  " + p1updt + "," +
                                 " p1uptm =  " + p1uptm + "," +
                                 " p1upus =  '" + p1upus + "'," +
                                 " p1prog =  '" + p1prog + "'," +
                                 " p1wsid =  '" + p1wsid + "'" +
                                 " WHERE p1apno = " + p1apno +
                                 " AND p1cont =" + p1cont;
                }
                else if (oper == "READY")
                {
                    sql = " UPDATE ilms01 SET " +
                                 " p1payt = '" + p1payt + "', " +
                                 " p1updt =  " + p1updt + "," +
                                 " p1uptm =  " + p1uptm + "," +
                                 " p1upus =  '" + p1upus + "'," +
                                 " p1prog =  '" + p1prog + "'," +
                                 " p1wsid =  '" + p1wsid + "'" +
                                 " WHERE p1apno = " + p1apno +
                                 " AND p1cont =" + p1cont;
                }

            }
            catch (Exception ex)
            {

            }
            return sql;
        }

        public DataSet get_gnmb30_update_auto_pay(string bankCode)
        {
            DataSet ds = new DataSet();
            try
            {
                UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
                UserInfomation = m_userInfo;

                string sql = " SELECT substr(gnb30f,5,2) AS LenAccNo FROM gnmb30  " +
                             " WHERE gnb30a = '" + bankCode + "'";


                ds = RetriveAsDataSet(sql);
                CloseConnectioDAL();

            }
            catch (Exception ex)
            {
            }
            return ds;
        }
        public DataSet get_ilms01_autopay(string appNo, string cont)
        {
            DataSet ds = new DataSet();
            try
            {
                UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
                UserInfomation = m_userInfo;

                string sql = " select p1rsts from ilms01 " +
                             " where p1apno  = " + appNo + " AND p1cont = " + cont;


                ds = RetriveAsDataSet(sql);
                CloseConnectioDAL();

            }
            catch (Exception ex)
            {
            }
            return ds;
        }

        public DataSet get_ilms00_autopay(string csn)
        {
            DataSet ds = new DataSet();
            try
            {
                UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
                UserInfomation = m_userInfo;

                string sql = " select p00cis,p00sts from ilms00 " +
                             " where p00cis= " + csn + " and p00sts = 'A' ";


                ds = RetriveAsDataSet(sql);
                CloseConnectioDAL();

            }
            catch (Exception ex)
            {
            }
            return ds;
        }

        public DataSet get_ilmd012(string cont, string app)
        {
            DataSet ds = new DataSet();
            try
            {
                UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
                UserInfomation = m_userInfo;

                string sql = " SELECT * FROM ilmd012 " +
                             " WHERE d012ap = " + app +
                             " AND   d012ct = " + cont +
                             " ORDER BY d012sq WITH UR ";


                ds = RetriveAsDataSet(sql);
                CloseConnectioDAL();

            }
            catch (Exception ex)
            {

            }
            return ds;
        }

        public DataSet get_contractIL(string app, string brn)
        {
            DataSet ds = new DataSet();
            try
            {
                //azode add " ,m00bdt as BirthDate,m00ebc " + 08-06-2560 [16:32]
                UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
                UserInfomation = m_userInfo;

                string StrWhere = " WHERE p1apno = " + app + " AND  p1brn= " + brn;

                string sql = " SELECT m00idn,gnb2td,m00tnm,m00tsn,p1ltyp,p1csno,p1brn,p1apno, " +
                             " digits(p1cont) p1cont,p2cndt,p1term,p1pram,p1coam,p2osam,p2lndr, " +
                             " p2dutr,p2cndt,p2term,p2rang,p2ndue,p2frtm,p2infr,0,0," +
                             " p1crcd,p1auth,p1apdt,p1vdid,p1payt,p1kusr,p1appt,p2frtm,p2duty," +
                             " p1aprj,p1loca,p1rsts,p2sost,p2odt2,p1item,p2fdam,p2frdt,p2fram, " +
                             " p1camp,p1cmsq,p1avdt,p1purc,p1down,p1stdt,p1crcd,p1auth,p2pcbl, " +
                             " p1qty,P1FILL " +
                             " ,m00bdt as BirthDate,m00ebc " +
                             " FROM ilms01 " +
                             " LEFT JOIN csms00 on(p1csno=m00csn) " +
                             " LEFT JOIN gnmb20 on(m00ttl=gnb2tc) " +
                             " LEFT JOIN ilms02 on(p1csno=p2csno and p1apno=p2apno) " +
                               StrWhere + " with UR ";


                ds = RetriveAsDataSet(sql);
                CloseConnectioDAL();

            }
            catch (Exception ex)
            {

            }
            return ds;
        }
        public DataSet get_ILClosingPayment(string brn, string ID, string contractNo, string appNo, string csn)
        {
            DataSet ds = new DataSet();
            try
            {
                UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
                UserInfomation = m_userInfo;

                string StrWhere = "";
                if (ID != "")
                {
                    StrWhere = " WHERE m00idn = '" + ID + "' ";
                }
                else if (contractNo != "")
                {
                    StrWhere = " WHERE p1cont = " + contractNo;
                }
                else if (appNo != "")
                {
                    StrWhere = " WHERE p1apno = " + appNo;
                }
                else if (csn != "")
                {
                    StrWhere = " WHERE p1csno = " + csn;
                }

                if (brn != "999")
                {
                    StrWhere += " AND  p1brn= " + brn;
                }



                string sql = " SELECT m00idn,gnb2td,m00tnm,m00tsn,TRIM(gnb2td)||' '||TRIM(m00tnm)||' '||TRIM(m00tsn) name_,p1ltyp,p1csno,p1brn,p1apno," +
                             " digits(p1cont) p1cont,p2cndt,p1term,p1pram,p1coam,p2osam,p2lndr," +
                             " p2dutr,p2cndt,p2term,p2rang,p2ndue,p2frtm,p2infr,0,0," +
                             " p1crcd,p1auth,p1apdt,p1vdid,P10TNM,p1payt,p1kusr,p1appt,p2frtm,p2duty," +
                             " p1aprj,p1loca,p1rsts,p2sost,p2odt2,p1item,T44DES,p2fdam,p2frdt,p2fram, " +
                             " p1camp,p1cmsq,p1avdt,p1purc,p1down,p1stdt,p1crcd,p1auth,p2pcbl," +
                             " p1qty ,SUBSTR(p1apdt,7,2)||'/'|| SUBSTR(p1apdt,5,2)||'/'||SUBSTR(p1apdt,1,4) appDate, " +
                             " TRIM(p1payt)||' : '||TRIM(gt48ed) recieve " +
                             " FROM ilms01 " +
                             " LEFT JOIN csms00 on(p1csno=m00csn) " +
                             " LEFT JOIN gnmb20 on(m00ttl=gnb2tc) " +
                             " LEFT JOIN ilms02 on(p1csno=p2csno and p1apno=p2apno) " +
                             " LEFT JOIN ilms10 on(p1vdid = P10VEN) " +
                             " LEFT JOIN iltb44 on(p1item = T44ITM) " +
                             " LEFT JOIN GNTB48 on(p1payt = GT48TC)";


                sql += StrWhere;


                ds = RetriveAsDataSet(sql);
                CloseConnectioDAL();

            }
            catch (Exception ex)
            {

            }
            return ds;
        }



        #endregion


        #region ---------REPORT----------------------------

        public ArrayList RP_Param(string m_program, string dateFrom = "", string dateTo = "", string year = "")
        {
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;
            string company = "";
            try
            {
                CALL_GNSRCONM("E", "F", "0", ref company);
                CloseConnectioDAL();
            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }


            ArrayList aryParm = new ArrayList();

            aryParm.Add(m_userInfo.Username.ToUpper().Trim());
            aryParm.Add(DateTime.Now.ToString("dd/MM/yyyy", m_DThai)); //  date
            aryParm.Add(DateTime.Now.ToString("HH:mm:ss")); // time
            aryParm.Add(m_userInfo.LocalClient.ToString()); // work station
            aryParm.Add(m_program);  // program
            aryParm.Add(company); //  company
            aryParm.Add(m_userInfo.BranchDescEN.ToString()); // branch
            aryParm.Add(dateFrom); // month1
            aryParm.Add(dateTo); // month2
            aryParm.Add(year); // year
            aryParm.Add(m_userInfo.BranchApp.ToString()); // branch

            CloseConnectioDAL();

            return aryParm;
        }

        public DataSet RP_SummaryReject(string WBDATF, string WBDATT, string WBRN, string WAPP_, string WPCODE)
        {
            DataSet ds = new DataSet();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;
            try
            {

                bool Rescall = CALL_ILR0502C2(WBDATF, WBDATT, WBRN, WAPP_, WPCODE, m_UserInfo.BizInit, m_UserInfo.BranchNo);
                if (Rescall)
                {
                    string sql_198 = " SELECT case when (substr(F198,1,1) = 0 OR substr(F198,1,1) = '') THEN substr(F198,2,198)  ELSE '' END   F198  ,case when (substr(F198,1,1) = 0 OR substr(F198,1,1) = '') THEN 0 ELSE '1' END  newpage " +
                                     " FROM qtemp/text198 ";
                    //string sql_198 = " SELECT case   substr(F198,2,198) F198 " +
                    //                 " FROM qtemp/text198 ";

                    ds = RetriveAsDataSet(sql_198);

                }
                CloseConnectioDAL();

            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }

            return ds;
        }

        public DataSet RP_IL_Pending(string RP_Type, string DateF, string DateT, string brn, bool all_PD, string sts_PD)
        {
            DataSet ds = new DataSet();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;
            try
            {
                string sqlWhere = " WHERE p1aprj IN ('MI','PD') and substr(p1fill,25,1) IN ('1','2','3','4') ";
                if (!all_PD)
                {
                    sqlWhere += " AND p1apdt >= " + DateF + " and p1apdt <= " + DateT;
                    if (brn != "")
                    {
                        sqlWhere += " AND p1brn = " + brn;
                    }
                }
                if (sts_PD == "Y")
                {
                    sqlWhere += " AND p1resn <> '' ";
                }
                else if (sts_PD == "N")
                {
                    sqlWhere += " AND p1resn = '' ";
                }


                string sql = "";
                if (RP_Type == "S")
                {
                    sql = " SELECT p1resn as pending_code, trim(g25des) as pending_desc, count(*) as case, " +
                            " ((count(*) * 100.00) / (select count(*) from ilms01 " + sqlWhere + " )) as Percent_Case, " +
                            " sum(p1pram) as Loan_amt, case when sum(p1pram) > 0 then " +
                            " ((sum(p1pram) * 100.00) / (select sum(p1pram) from ilms01 " + sqlWhere + " )) else 0 end as Percent_Loan, " +
                            " sum(p1coam) as Cont_amt, " +
                            " case when sum(p1coam) > 0 then " +
                            " ((sum(p1coam) * 100.00) / (select sum(p1coam) from ilms01 " + sqlWhere + " )) else 0 end as Percent_Cont " +
                            " FROM ilms01 " +
                            " LEFT JOIN gntb25 ON (p1resn=g25rcd) " +
                            sqlWhere +
                            " GROUP BY p1resn, g25des " +
                            " ORDER BY p1resn WITH ur  ";

                    ds = RetriveAsDataSet(sql);
                    CloseConnectioDAL();
                }
                else if (RP_Type == "D")
                {
                    sql = " SELECT substr(p1apdt,7,2)||'/'||substr(p1apdt,5,2)||'/'||substr(p1apdt,1,4) as p1apdt, " +
                           " DIGITS(p1brn) p1brn, DIGITS(p1apno) p1apno, p1pram, p1coam, " +
                           " case when substr(p1fill,25,1) in ('1') then 'KEY-IN STEP1'  " +
                           " when substr(p1fill,25,1) in ('2','4') then 'INTERVIEW' " +
                           " when substr(p1fill,25,1) in ('3') then 'KESSAI' " +
                           " else 'END CASE' end Processing, " +
                           " case when (p1aprj = 'PD' and p1avdt > 0) then substr(p1avdt,7,2)||'/'||substr(p1avdt,5,2)||'/'||substr(p1avdt,1,4) " +
                           " else '' end Pending_Date, " +
                           " case when (p1aprj = 'PD' and p1avtm > 0) then substr(p1avtm,1,2)||':'||substr(p1avtm,3,2)||':'||substr(p1avtm,5,2) " +
                           " else '' end Pending_Time, " +
                           " case when p1aprj = 'PD' then p1auth else '' end Pending_user, " +
                           " case when p1aprj = 'PD' then p1resn else '' end Pending_code, " +
                           " case when p1aprj = 'PD' then g25des else '' end Pending_desc  " +
                           " FROM ilms01 " +
                           " LEFT JOIN gntb25 on (p1resn=g25rcd) " +
                           sqlWhere + " ORDER by substr(p1apdt,7,4)||substr(p1apdt,4,2)||substr(p1apdt,1,2) with ur ";
                    ds = RetriveAsDataSet(sql);
                    CloseConnectioDAL();
                }




            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }
            return ds;
        }

        public DataSet RP_UsingSpeedTime(string WBDATF, string WBDATT, string WBRN, string WUSER, string WDEPT)
        {
            DataSet ds = new DataSet();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;

            string wherebrn = "";
            if (WBRN.ToString().Trim() != "")
            {
                wherebrn = "and p1brn = " + WBRN.ToString().Trim() + " ";
            }

            try
            {
                string condition = " AND exists (SELECT * FROM SYFUSDES " +
                                   " JOIN GNAT04 ON USEMID = AT04EM " +
                                   " JOIN GNAT05 ON AT04JD = AT05JD " +
                                   " JOIN GNAT06 ON AT05DP = AT06DP " +
                                   " WHERE ilms01.p1crcd = uscode and AT06GD in ('" +
                                    WDEPT +
                                   "') and uscode <> '' )";
                if (WUSER != "")
                {
                    condition += " AND  p1crcd = '" + WUSER + "'";

                }


                string sql = " SELECT distinct  m00idn as IDCard,DIGITS(P1APNO) P1APNO, substr(p1apdt,7,2)||'/'||substr(p1apdt,5,2)||'/'||substr(p1apdt,1,4) as AppDate,  " +
                             " trim(m00tnm)||' '||trim(m00tsn) as Name, DIGITS(p1vdid) as Vendor, digits(p1cont) as Contract, " +
                             " substr(digits(a.mlsstr),1,2)||':'||substr(digits(a.mlsstr),3,2)||':'||substr(digits(a.mlsstr),5,2) as StartTime, " +
                             " substr(digits(p1avtm),1,2)||':'||substr(digits(p1avtm),3,2)||':'||substr(digits(p1avtm),5,2) as EndTime,  " +
                             " RIGHT('00'||trim(char(timestampdiff(8,char(timestamp(substr(digits(p1apdt),1,4)||'-'||substr(digits(p1apdt),5,2)||'-'||substr(digits(p1apdt),7,2)||'-'||substr(digits(p1avtm),1,2)||'.'||substr(digits(p1avtm),3,2)||'.'||substr " +
                             " (digits(p1avtm),5,2)) -  timestamp(substr(digits(p1apdt),1,4)||'-'||substr(digits(p1apdt),5,2)||'-'||substr(digits(p1apdt),7,2)||'-'||substr(digits(a.mlsstr),1,2)||'.'||substr(digits(a.mlsstr),3,2)||'.'||substr(digits(a.mlsstr),5,2)))))),2) ||':'|| " +
                             " RIGHT('00'||trim(char(mod(timestampdiff(4,char(timestamp(substr(digits(p1apdt),1,4)||'-'||substr(digits(p1apdt),5,2)||'-'||substr(digits(p1apdt),7,2)||'-'||substr(digits(p1avtm),1,2)||'.'||substr(digits(p1avtm),3,2)||'.'|| " +
                             " substr(digits(p1avtm),5,2)) -  timestamp(substr(digits(p1apdt),1,4)||'-'||substr(digits(p1apdt),5,2)||'-'||substr(digits(p1apdt),7,2)||'-'||substr(digits(a.mlsstr),1,2)||'.'||substr(digits(a.mlsstr),3,2)||'.'||substr(digits(a.mlsstr),5,2)))),60))) ,2) " +
                             " ||':'|| RIGHT('00'||trim(char(mod(timestampdiff(2,char(timestamp(substr(digits(p1apdt),1,4)||'-'||substr(digits(p1apdt),5,2)||'-'||substr(digits(p1apdt),7,2)||'-'||substr(digits(p1avtm),1,2)||'.'||substr(digits(p1avtm),3,2)||'.'|| " +
                             " substr(digits(p1avtm),5,2)) - timestamp(substr(digits(p1apdt),1,4)||'-'||substr(digits(p1apdt),5,2)||'-'||substr(digits(p1apdt),7,2)||'-'||substr(digits(a.mlsstr),1,2)||'.'||substr(digits(a.mlsstr),3,2)||'.'||substr(digits(a.mlsstr),5,2)))),60))) ,2) as SpeedTime, " +
                             " case when (USFNME <> '' AND USFNME IS NOT null) then USFNME else p1crcd end AS p1crcd " +
                             " FROM  ilms01 LEFT JOIN csms00 on(p1csno=m00csn) " +
                             " LEFT JOIN csmsls a on (m00idn=a.mlsidn and a.mlsuus=p1crcd and a.mlsres='OPC003' and a.mlsstr < p1avtm) " +
                             " LEFT JOIN SYFUSDES  on  (USEMID = p1crcd)  " +
                             //" LEFT JOIN GNAT04  on  (usemid = at04em) " +
                             //" LEFT JOIN GNAT05  on  (at04jd = at05jd) " +
                             " WHERE p1appt='02' and (a.mlsstr = (select max(b.mlsstr) FROM csmsls b where csms00.m00idn=b.mlsidn  " +
                             " AND b.mlsuus=ilms01.p1crcd AND b.mlsres='OPC003' AND b.mlsstr < ilms01.p1avtm) or a.mlsstr is null) " +
                             " and p1apdt between " + WBDATF + " and " + WBDATT + " " +
                             " " + wherebrn + " " +
                             condition +
                             " ORDER BY AppDate, StartTime ";

                //string sql = " SELECT distinct  m00idn as IDCard,DIGITS(P1APNO) P1APNO, substr(p1apdt,7,2)||'/'||substr(p1apdt,5,2)||'/'||substr(p1apdt,1,4) as AppDate,  " +
                //             " trim(m00tnm)||' '||trim(m00tsn) as Name, DIGITS(p1vdid) as Vendor, digits(p1cont) as Contract, " +
                //             " substr(digits(a.mlsstr),1,2)||':'||substr(digits(a.mlsstr),3,2)||':'||substr(digits(a.mlsstr),5,2) as StartTime, " +
                //             " substr(digits(p1avtm),1,2)||':'||substr(digits(p1avtm),3,2)||':'||substr(digits(p1avtm),5,2) as EndTime,  " +
                //             " RIGHT('00'||trim(char(timestampdiff(8,char(timestamp(substr(digits(p1apdt),1,4)||'-'||substr(digits(p1apdt),5,2)||'-'||substr(digits(p1apdt),7,2)||'-'||substr(digits(p1avtm),1,2)||'.'||substr(digits(p1avtm),3,2)||'.'||substr " +
                //             " (digits(p1avtm),5,2)) -  timestamp(substr(digits(p1apdt),1,4)||'-'||substr(digits(p1apdt),5,2)||'-'||substr(digits(p1apdt),7,2)||'-'||substr(digits(a.mlsstr),1,2)||'.'||substr(digits(a.mlsstr),3,2)||'.'||substr(digits(a.mlsstr),5,2)))))),2) ||':'|| " +
                //             " RIGHT('00'||trim(char(mod(timestampdiff(4,char(timestamp(substr(digits(p1apdt),1,4)||'-'||substr(digits(p1apdt),5,2)||'-'||substr(digits(p1apdt),7,2)||'-'||substr(digits(p1avtm),1,2)||'.'||substr(digits(p1avtm),3,2)||'.'|| " +
                //             " substr(digits(p1avtm),5,2)) -  timestamp(substr(digits(p1apdt),1,4)||'-'||substr(digits(p1apdt),5,2)||'-'||substr(digits(p1apdt),7,2)||'-'||substr(digits(a.mlsstr),1,2)||'.'||substr(digits(a.mlsstr),3,2)||'.'||substr(digits(a.mlsstr),5,2)))),60))) ,2) " +
                //             " ||':'|| RIGHT('00'||trim(char(mod(timestampdiff(2,char(timestamp(substr(digits(p1apdt),1,4)||'-'||substr(digits(p1apdt),5,2)||'-'||substr(digits(p1apdt),7,2)||'-'||substr(digits(p1avtm),1,2)||'.'||substr(digits(p1avtm),3,2)||'.'|| " +
                //             " substr(digits(p1avtm),5,2)) - timestamp(substr(digits(p1apdt),1,4)||'-'||substr(digits(p1apdt),5,2)||'-'||substr(digits(p1apdt),7,2)||'-'||substr(digits(a.mlsstr),1,2)||'.'||substr(digits(a.mlsstr),3,2)||'.'||substr(digits(a.mlsstr),5,2)))),60))) ,2) as SpeedTime, p1crcd " +
                //             " FROM  ilms01 LEFT JOIN csms00 on(p1csno=m00csn) " +
                //             " LEFT JOIN csmsls a on (m00idn=a.mlsidn and a.mlsuus=p1crcd and a.mlsres='OPC003' and a.mlsstr < p1avtm) " +
                //             " WHERE p1appt='02' and (a.mlsstr = (select max(b.mlsstr) FROM csmsls b where csms00.m00idn=b.mlsidn  " +
                //             " AND b.mlsuus=ilms01.p1crcd AND b.mlsres='OPC003' AND b.mlsstr < ilms01.p1avtm) or a.mlsstr is null) " +
                //             " and p1apdt between " + WBDATF + " and " + WBDATT + " " +
                //             " " + wherebrn + " " +
                //             condition +
                //             " ORDER BY AppDate, StartTime ";





                ds = RetriveAsDataSet(sql);


                CloseConnectioDAL();

            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }

            return ds;
        }



        #region report phaseII

        public DataSet RP_AutoPay(string pay_type, string dateFrom, string dateTo, string debit_bank, string brn)
        {
            DataSet ds = new DataSet();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;
            try
            {


                string where = "";
                string sql = "";
                if (pay_type == "W" || pay_type == "4" || pay_type == "R")
                {

                    if (pay_type == "W")
                    {
                        where = " WHERE p1payt = '" + pay_type + "' AND  p1loca = '275' ";
                    }
                    else if (pay_type == "4")
                    {
                        where = " WHERE p1payt = '" + pay_type + "'";
                    }
                    else if (pay_type == "R")
                    {
                        where = " WHERE p1payt = '" + pay_type + "'";
                    }

                    if (dateFrom == dateTo)
                    {
                        where += " AND p00std= " + dateFrom;
                    }
                    else if (dateFrom != dateTo)
                    {
                        where += " AND p00std >=  " + dateFrom +
                                 " AND p00std <= " + dateTo;
                    }

                    if (debit_bank != "")
                    {
                        where += " AND p1pbcd = '" + debit_bank + "'";
                    }

                    if (brn != "")
                    {
                        where += " AND p1brn = " + brn;
                    }

                    where += " order by p00bnk,p00bst ";



                    /*   sql = " SELECT p00std,p00bst,p00doc,substr(digits(m00ebc),1,4)||'-'||substr(digits(m00ebc),5,4)||'-'||substr(digits(m00ebc),9,4)||'-'||substr(digits(m00ebc),13,4) m00ebc1," +
                                     " TRIM(gnb2td)||TRIM(m00tnm)||' '||TRIM(m00tsn) name_,digits(p1cont) p1cont1,p1pbcd,p1pbrn,p1pano, " +
                                     " p00aty,gn13ed " +
                                     " FROM ilms01 " +
                                     " LEFT JOIN ilms00 ON(p1csno=p00cis and p1pbcd=p00bnk and " +
                                     " p1pbrn=p00bbr and p1pano=p00bac) " +
                                     " LEFT JOIN csms00 on(p1csno=m00csn) " +
                                     " LEFT JOIN gnmb20 on(m00ttl=gnb2tc) " +
                                     " LEFT JOIN gntb13 on(gn13cd=p00aty) " +
                                     where + " with UR "; */


                    sql = " SELECT p00std,p00bst,p00doc,substr(digits(m00ebc),1,4)||'-'||substr(digits(m00ebc),5,4)||'-'||substr(digits(m00ebc),9,4)||'-'||substr(digits(m00ebc),13,4) m00ebc1," +
                                  " TRIM(gnb2td)||TRIM(m00tnm)||' '||TRIM(m00tsn) name_,digits(p1cont) p1cont1,p1pbcd,p1pbrn,p1pano, " +
                                  " p00aty,gn13ed " +
                                  " FROM ilms00  " +
                                  " LEFT JOIN  ilms01 ON ( p00cnt = p1cont AND p00cis = p1csno and p00bnk = p1pbcd and  p00bbr=p1pbrn and p00bac=p1pano ) " +
                                  " LEFT JOIN  csms00 ON ( p00cis = m00csn ) " +
                                  " LEFT JOIN  gnmb20 ON ( m00ttl = gnb2tc ) " +
                                  " LEFT JOIN  gntb13 ON ( gn13cd = p00aty ) " +
                                  where + " with UR ";
                }
                else if (pay_type == "3" || pay_type == "2" || pay_type == "5")
                {
                    if (pay_type == "3" || pay_type == "2" || pay_type == "5")
                    {
                        where = " WHERE p1payt = '" + pay_type + "'";
                    }
                    if (dateFrom == dateTo)
                    {
                        where += " AND P1APDT = " + dateFrom;
                    }
                    else if (dateFrom != dateTo)
                    {
                        where += " AND P1APDT >=  " + dateFrom +
                                 " AND P1APDT <= " + dateTo;
                    }

                    if (brn != "")
                    {
                        where += " AND p1brn = " + brn;
                    }

                    /*sql = " SELECT p00std,p00bst,p00doc,substr(digits(m00ebc),1,4)||'-'||substr(digits(m00ebc),5,4)||'-'||substr(digits(m00ebc),9,4)||'-'||substr(digits(m00ebc),13,4) m00ebc1," +
                                 " TRIM(gnb2td)||TRIM(m00tnm)||' '||TRIM(m00tsn) name_,digits(p1cont) p1cont1,p1pbcd,p1pbrn,p1pano, " +
                                 " p00aty,gn13ed " +
                                 " FROM ilms01 " +
                                 " LEFT JOIN ilms00 ON(p1csno=p00cis and p1pbcd=p00bnk and " +
                                 " p1pbrn=p00bbr and p1pano=p00bac) " +
                                 " LEFT JOIN csms00 on(p1csno=m00csn) " +
                                 " LEFT JOIN gnmb20 on(m00ttl=gnb2tc) " +
                                 " LEFT JOIN gntb13 on(gn13cd=p00aty) " +
                                 where + " with UR ";*/

                    sql = " SELECT p00std,p00bst,p00doc,substr(digits(m00ebc),1,4)||'-'||substr(digits(m00ebc),5,4)||'-'||substr(digits(m00ebc),9,4)||'-'||substr(digits(m00ebc),13,4) m00ebc1," +
                                  " TRIM(gnb2td)||TRIM(m00tnm)||' '||TRIM(m00tsn) name_,digits(p1cont) p1cont1,p1pbcd,p1pbrn,p1pano, " +
                                  " p00aty,gn13ed " +
                                  " FROM ilms00 " +
                                  " LEFT JOIN ilms01 ON( p00cnt = p1cont AND p00cis = p1csno and p1pbcd=p00bnk and p1pbrn=p00bbr and p1pano=p00bac) " +
                                  " LEFT JOIN csms00 on( p1csno = m00csn ) " +
                                  " LEFT JOIN gnmb20 on( m00ttl = gnb2tc ) " +
                                  " LEFT JOIN gntb13 on( gn13cd = p00aty ) " +
                                  where + " with UR ";



                }

                ds = RetriveAsDataSet(sql);
                CloseConnectioDAL();

            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }

            return ds;
        }
        // report รายการนำส่งหนังสือขอให้หักบัญชี
        public DataSet RP_Document_Debit(string dateFrom, string dateTo, string debit_bank, string brn)
        {
            DataSet ds = new DataSet();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;
            try
            {
                string where = " Where p1payt = 'W' and p1loca = '275' ";
                if (dateFrom == dateTo)
                {
                    where += " AND p00std= " + dateFrom;
                }
                else if (dateFrom != dateTo)
                {
                    where += " AND p00std >=  " + dateFrom +
                             " AND p00std <= " + dateTo;
                }

                if (debit_bank != "")
                {
                    where += " AND p1pbcd = '" + debit_bank + "'";
                }

                if (brn != "")
                {
                    where += " AND p1brn = " + brn;
                }

                where += " ORDER by p00bnk,p00std ";



                /*  string sql = " SELECT p00bnk,p00std,p1pano,'' as bank_,substr(digits(m00ebc),1,4)||'-'||substr(digits(m00ebc),5,4)||'-'||substr(digits(m00ebc),9,4)||'-'||substr(digits(m00ebc),13,4)  m00ebc1, " +
                               " TRIM(gnb2td)||TRIM(m00tnm)||' '||TRIM(m00tsn) name_ ,digits(p1cont) p1cont1,p1pbrn,gnb31d, ' ' as remark_ " +
                               " FROM ilms01 " +
                               " LEFT JOIN ilms00 ON (p1csno=p00cis AND p1pbcd=p00bnk AND " +
                               " p1pbrn=p00bbr and p1pano=p00bac) " +
                               " LEFT JOIN csms00 ON (p1csno=m00csn) " +
                               " LEFT JOIN gnmb20 ON(m00ttl=gnb2tc) " +
                               " LEFT JOIN gnmb31 ON(gnb31a=p1pbcd and gnb31c=p1pbrn) " +
                                where + " with UR "; */


                string sql = " SELECT p00bnk,p00std,p1pano,'' as bank_,substr(digits(m00ebc),1,4)||'-'||substr(digits(m00ebc),5,4)||'-'||substr(digits(m00ebc),9,4)||'-'||substr(digits(m00ebc),13,4)  m00ebc1, " +
                               " TRIM(gnb2td)||TRIM(m00tnm)||' '||TRIM(m00tsn) name_ ,digits(p1cont) p1cont1,p1pbrn,gnb31d, ' ' as remark_ " +
                               " FROM ilms00  " +
                               " LEFT JOIN ilms01 ON (P00CNT = P1CONT AND p00cis = p1csno AND p00bnk=p1pbcd AND p00bbr = p1pbrn and p00bac = p1pano)  " +
                               " LEFT JOIN csms00 ON (p00cis = m00csn) " +
                               " LEFT JOIN gnmb20 ON (m00ttl=gnb2tc) " +
                               " LEFT JOIN gnmb31 ON(p1pbcd = gnb31a AND p1pbrn = gnb31c AND  TRIM(p1pbrn) <> '' ) " +
                                where + " with UR ";



                ds = RetriveAsDataSet(sql);
                CloseConnectioDAL();

            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }

            return ds;
        }

        public DataSet RP_Customer_No_Addr(string dateFrom, string dateTo, string brn,
                                           bool no_shipTo, bool no_add_shipto, bool no_add_id, bool no_add_home,
                                           bool no_add_off)
        {
            DataSet ds = new DataSet();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;
            try
            {
                string whereBrn = "";
                string strWhere = "";

                if (brn == "All RBC")
                {
                    whereBrn = " d.p2brn>100 AND ";
                }
                else if (brn != "")
                {
                    whereBrn = " d.p2brn = " + brn + " AND  ";
                }

                strWhere = " WHERE " + whereBrn + " d.p2loca = '275' ";

                if (dateFrom == dateTo)
                {
                    strWhere += " and d.p2bkdt = " + dateFrom;
                }
                else if (dateFrom != dateTo)
                {
                    strWhere += " AND d.p2bkdt >=  " + dateFrom +
                             " and d.p2bkdt<= " + dateTo;
                }


                string strField = " case when (d.p2bkdt=0) or (d.p2bkdt is null) then '' else  " +
                                  " substr(ifnull(digits(d.p2bkdt),0),7,2)||'/'||substr(ifnull(digits(d.p2bkdt),0),5,2)||'/'|| " +
                                  " substr(ifnull(digits(d.p2bkdt),0),1,4) end as p2bkdt, " +
                                  " case when (d.p2cont=0) or (d.p2cont is null) then '' else " +
                                  " substr(ifnull(digits(d.p2cont),''),1,4)||'-'||substr(ifnull(digits(d.p2cont),''),5,3)||'-'|| " +
                                  " substr(ifnull(digits(d.p2cont),''),8,9) end as p2cont, " +
                                  " case when (r.p2bkdt=0) or (r.p2bkdt is null) then '' else " +
                                  " substr(ifnull(digits(r.p2bkdt),0),7,2)||'/'||substr(ifnull(digits(r.p2bkdt),0),5,2)||'/'|| " +
                                  " substr(ifnull(digits(r.p2bkdt),0),1,4) end as rlbkdt, " +
                                  " case when (r.p2cont=0) or (r.p2cont is null) then '' else " +
                                  " substr(ifnull(digits(r.p2cont),''),1,4)||'-'||substr(ifnull(digits(r.p2cont),''),5,3)||'-'|| " +
                                  " substr(ifnull(digits(r.p2cont),''),8,9) end as rlcont, " +
                                  " ifnull(m00idn,'') as id_no, " +
                                  " p1appt,Trim(ifnull(gnb2td,''))||Trim(ifnull(m00tnm,''))||' '||Trim(ifnull(m00tsn,'')) as name, " +
                                  " m00dsn,m00csn, p1brn,t1bnme, " +
                                  " case when d.p2bkdt > 0 then substr(ifnull(digits(d.p2bkdt),0),7,2)||'/'||" +
                                  " substr(ifnull(digits(d.p2bkdt),0),5,2)||'/'||substr(ifnull(digits(d.p2bkdt),0),1,4) " +
                                  " else '00/00/0000' end as p2bkdt, " +
                                  " case when ((select distinct m51cnt from csms151 where m51cnt=d.p2cont) is null) then '' else 'Y' end Full_IL, " +
                                  " case when ((select distinct m51cnt from csms151 where m51cnt=r.p2cont) is null) then '' else 'Y' end Full_RL " +
                                  " from ilms01 as a " +
                                  " left join ilms02 as d on(p1brn=d.p2brn and p1cont=d.p2cont and d.p2loca='275') " +
                                  " left join rlms02 as r on(p1csno=r.p2csno and r.p2del='') " +
                                  " left join csms00 as b on(m00csn=p1csno) " +
                                  " left join iltb01 on (p1brn=t1brn) " +
                                  " left join gnmb20 on(m00ttl=gnb2tc) ";

                string strLastIL = " AND d.p2cont = (select max(p2cont) from ilms02 as c " + strWhere + " AND d.p2csno=c.p2csno) ";
                string strsql_1 = "";
                string strsql_2 = "";
                string strsql_3 = "";
                string strsql_4 = "";
                string strsql_5 = "";
                string strsql_6 = "";
                string strsql_7 = "";
                string strsql_8 = "";
                string sql = "";

                if (no_shipTo)
                {
                    strsql_1 = " SELECT 'No Ship to' as Title,  " + strField + " " + strWhere +
                               " AND exists (select * from csms00 where a.p1csno=m00csn and m00dsn='') " +
                               strLastIL;
                    if (sql != "")
                    {
                        sql += " UNION ";
                    }
                    sql += strsql_1;

                }
                if (no_add_shipto)
                {
                    strsql_2 = " SELECT 'No Address in Ship to' as Title, " + strField + " " + strWhere + " " +
                               " AND not exists (SELECT * FROM csms11 WHERE a.p1csno=m11csn AND m11ref = '' AND m11cde = b.m00dsn) " +
                               strLastIL;

                    if (sql != "")
                    {
                        sql += " UNION ";
                    }
                    sql += strsql_2;
                }
                if (no_add_id)
                {
                    strsql_3 = " SELECT 'No Address, Incomplete in ID Card' as Title, " + strField + " " + strWhere + " " +
                               " AND NOT exists (SELECT * FROM csms11 WHERE a.p1csno = m11csn AND m11ref = '' AND m11cde = 'I') " +
                               strLastIL;

                    strsql_4 = " SELECT 'No Address, Incomplete in ID Card' as Title, " + strField + " " + strWhere + " " +
                               " AND exists (SELECT * FROM csms11 WHERE a.p1csno = m11csn AND m11ref = '' AND m11cde = 'I ' " +
                               " AND (m11adr = '' OR m11tam = 0 OR m11amp = 0 OR m11prv = 0 AND  m11zip = 0))" +
                               strLastIL;
                    if (strsql_3 != "")
                    {
                        if (sql != "")
                        {
                            sql += " UNION ";
                        }
                        sql += strsql_3;
                    }
                    if (strsql_4 != "")
                    {
                        if (sql != "")
                        {
                            sql += " UNION ";
                        }
                        sql += strsql_4;
                    }

                }
                if (no_add_home)
                {
                    strsql_5 = " SELECT 'No Address, Incomplete in Home' as Title, " + strField + " " + strWhere + " " +
                               " AND NOT exists (SELECT * FROM csms11 WHERE a.p1csno=m11csn AND m11ref = '' AND m11cde = 'H') " +
                               strLastIL;

                    strsql_6 = " SELECT 'No Address, Incomplete in Home' as Title, " + strField + " " + strWhere + " " +
                                " AND exists (SELECT * FROM csms11 WHERE a.p1csno = m11csn AND m11ref = '' AND m11cde = 'H' " +
                                " AND (m11adr ='' OR m11tam = 0 OR m11amp = 0 OR m11prv = 0 AND m11zip = 0)) " +
                                strLastIL;


                    if (strsql_5 != "")
                    {
                        if (sql != "")
                        {
                            sql += " UNION ";
                        }
                        sql += strsql_5;
                    }
                    if (strsql_6 != "")
                    {
                        if (sql != "")
                        {
                            sql += " UNION ";
                        }
                        sql += strsql_6;
                    }
                }
                if (no_add_off)
                {
                    strsql_7 = " SELECT 'No Address, Incomplete in Office' as Title, " + strField + " " + strWhere + " " +
                               " AND NOT exists (SELECT * FROM csms11 WHERE a.p1csno = m11csn AND m11ref = '' AND m11cde = 'O')  " +
                               strLastIL;

                    strsql_8 = " SELECT 'No Address, Incomplete in Office' as Title, " + strField + " " + strWhere + " " +
                               " AND exists (SELECT * FROM csms11 WHERE a.p1csno = m11csn AND m11ref = '' AND m11cde = 'O' " +
                               " AND (m11adr = '' OR m11tam = 0 OR m11amp = 0 OR m11prv = 0 AND m11zip = 0)) " +
                               strLastIL;

                    if (strsql_7 != "")
                    {
                        if (sql != "")
                        {
                            sql += " UNION ";
                        }
                        sql += strsql_7;
                    }
                    if (strsql_8 != "")
                    {
                        if (sql != "")
                        {
                            sql += " UNION ";
                        }
                        sql += strsql_8;
                    }

                }


                ds = RetriveAsDataSet(sql);
                CloseConnectioDAL();

            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }

            return ds;
        }


        //***** print sticker ****//
        public bool CALL_ILC021CL1(string WSBRN, string WSSDTE, string WSEDTE, string WSTYPE, string WSOPT1, string WSOPT2, string WSDFMT, string strBizInit, string strBranchNo)
        {
            string m_UdpTime = DateTime.Now.ToString("HHmmss");
            m_LastError = "";
            string WSTEXT = "";
            iDB2Command cmd = new iDB2Command();

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.CommandText = "ILC021CL1";
            // Parameter In

            cmd.Parameters.Add("WSBRN", iDB2DbType.iDB2Decimal, 0).Value = WSBRN;
            cmd.Parameters["WSBRN"].Precision = 3;
            cmd.Parameters["WSBRN"].Scale = 0;

            cmd.Parameters.Add("WSSDTE", iDB2DbType.iDB2Decimal, 0).Value = WSSDTE;
            cmd.Parameters["WSSDTE"].Precision = 8;
            cmd.Parameters["WSSDTE"].Scale = 0;

            cmd.Parameters.Add("WSEDTE", iDB2DbType.iDB2Decimal, 0).Value = WSEDTE;
            cmd.Parameters["WSEDTE"].Precision = 8;
            cmd.Parameters["WSEDTE"].Scale = 0;

            cmd.Parameters.Add("WSTYPE", iDB2DbType.iDB2Char, 1).Value = WSTYPE;
            cmd.Parameters.Add("WSOPT1", iDB2DbType.iDB2Char, 1).Value = WSOPT1;
            cmd.Parameters.Add("WSOPT2", iDB2DbType.iDB2Char, 1).Value = WSOPT2;


            cmd.Parameters.Add("WSTEXT", iDB2DbType.iDB2Char, 150).Direction = ParameterDirection.Output;
            cmd.Parameters["WSTEXT"].Value = "";

            cmd.Parameters.Add("WSDFMT", iDB2DbType.iDB2Char, 1).Value = WSDFMT;


            if (m_da400.ExecuteSubRoutine("ILC021CL1", ref cmd, strBizInit, strBranchNo))
            {
                WSTEXT = cmd.Parameters["WSTEXT"].Value.ToString().Trim();
                return true;
            }
            else
                return false;
        }
        public DataSet RP_PrintSticker(string WSBRN, string WSSDTE, string WSEDTE, string WSTYPE, string WSOPT1, string WSOPT2, string WSDFMT, ref DataSet ds_tb28)
        {
            DataSet ds = new DataSet();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;
            try
            {

                bool Rescall = CALL_ILC021CL1(WSBRN, WSSDTE, WSEDTE, WSTYPE, WSOPT1, WSOPT2, WSDFMT, m_UserInfo.BizInit, m_UserInfo.BranchNo);
                if (Rescall)
                {


                    if (WSOPT2.Trim() == "X")
                    {
                        string sql = " SELECT   '" + WSTYPE + "'||'-'||SUBSTR(M40BDT,3)||'-'||M40CON CONT ,CASE  WHEN M00EBC <> 0 THEN SUBSTR(DIGITS(M00EBC),1,1)||'-'||SUBSTR(DIGITS(M00EBC),2,3)||'-'||SUBSTR(DIGITS(M00EBC),5) ELSE '' END EBC  , " +
                                     " CASE  WHEN  D45RDT <> 0 THEN SUBSTR(D45RDT,7,2)||'/'||SUBSTR(D45RDT,5,2)||'/'||SUBSTR(D45RDT,1,4) ELSE '' END FOL_DATE , " +
                                     " CASE  WHEN  P2TMDT <> 0 THEN SUBSTR(P2TMDT,7,2)||'/'||SUBSTR(P2TMDT,5,2)||'/'||SUBSTR(P2TMDT,1,4) ELSE '' END TER_DATE , " +
                                     " TRIM(GNB2TD)||TRIM(M00TNM)||'  ' ||TRIM(M00TSN) CUSTNM , " +
                                     " SUBSTR(M40BDT,7,2)||'/'||SUBSTR(M40BDT,5,2)||'/'||SUBSTR(M40BDT,1,4) BOOKDATE " +
                                     " FROM ILMS40 " +
                                     " JOIN ILMS02 ON (M40CON = P2CONT AND  M40BRN = P2BRN) " +
                                     " LEFT JOIN ILMD45 ON (M40CON = D45CNT AND  M40BRN = D45BRN) " +
                                     " LEFT JOIN CSMS00 ON (M40CSN = M00CSN) " +
                                     " LEFT JOIN GNMB20 ON (M00TTL = GNB2TC)" +
                                     " WHERE M40BDT BETWEEN " + WSSDTE + " AND " + WSEDTE + " AND " +
                                     " ((M40DEL = '') OR ( M40DEL ='X' AND P2DEL ='X' AND P2LOCA ='301')) " +
                                     " AND M40BRN =  " + WSBRN + " ORDER BY M40BDT ASC";

                        ds = RetriveAsDataSet(sql);

                        //string sql_198 = " SELECT  substr(F198,2,132) F198 ,  " +
                        //             " CASE when (substr(F198,1,1) = '1') THEN '1' ELSE '0' END  newpage " +
                        //             " FROM qtemp/text198 ";

                        //ds = RetriveAsDataSet(sql_198);




                    }

                    if (WSOPT1.Trim() == "X")
                    {
                        string sql_iltb28 = " select CONT from  iltb28 ";


                        ds_tb28 = RetriveAsDataSet(sql_iltb28);
                    }


                }

                CloseConnectioDAL();

            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }

            return ds;
        }
        //***** end print sticker ****//

        //****  Customer inquiry ****//
        public bool CALL_ILSR37(string PIAPRJ, string PILOCA, string PIRSTS, ref string POTEXT, string strBizInit, string strBranchNo)
        {
            string m_UdpTime = DateTime.Now.ToString("HHmmss");
            m_LastError = "";
            string WSTEXT = "";
            iDB2Command cmd = new iDB2Command();

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.CommandText = "ILSR37";
            // Parameter In

            cmd.Parameters.Add("PIAPRJ", iDB2DbType.iDB2Char, 2).Value = PIAPRJ;
            cmd.Parameters.Add("PILOCA", iDB2DbType.iDB2Char, 3).Value = PILOCA;
            cmd.Parameters.Add("PIRSTS", iDB2DbType.iDB2Char, 1).Value = PIRSTS;


            cmd.Parameters.Add("POTEXT", iDB2DbType.iDB2Char, 20).Direction = ParameterDirection.Output;
            cmd.Parameters["POTEXT"].Value = "";


            if (m_da400.ExecuteSubRoutine("ILSR37", ref cmd, strBizInit, strBranchNo))
            {
                POTEXT = cmd.Parameters["POTEXT"].Value.ToString().Trim();
                return true;
            }
            else
                return false;
        }



        //***** report Daily Application analysis ****//
        public bool CALL_ILR033C2(string WKBRN, string WKYMD, string WKOPTS, string strBizInit, string strBranchNo)
        {
            string m_UdpTime = DateTime.Now.ToString("HHmmss");
            m_LastError = "";
            string WSTEXT = "";
            iDB2Command cmd = new iDB2Command();

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.CommandText = "ILR033C2";
            // Parameter In

            cmd.Parameters.Add("WKBRN", iDB2DbType.iDB2Decimal, 0).Value = WKBRN;
            cmd.Parameters["WKBRN"].Precision = 3;
            cmd.Parameters["WKBRN"].Scale = 0;

            cmd.Parameters.Add("WKYMD", iDB2DbType.iDB2Decimal, 0).Value = WKYMD;
            cmd.Parameters["WKYMD"].Precision = 8;
            cmd.Parameters["WKYMD"].Scale = 0;

            cmd.Parameters.Add("WKOPTS", iDB2DbType.iDB2Char, 1).Value = WKOPTS;

            if (m_da400.ExecuteSubRoutine("ILR033C2", ref cmd, strBizInit, strBranchNo))
            {
                return true;
            }
            else
                return false;
        }
        public DataSet RP_DailyApplication_Analysis(string WKBRN, string WKYMD, string WKOPTS)
        {
            DataSet ds = new DataSet();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;
            try
            {

                bool Rescall = CALL_ILR033C2(WKBRN, WKYMD, WKOPTS, m_UserInfo.BizInit, m_UserInfo.BranchNo);
                if (Rescall)
                {

                    string sql_198 = " SELECT  substr(F198,2,198) F198 ,  " +
                                     " CASE when (substr(F198,1,1) = '1') THEN '1' ELSE '0' END  newpage " +
                                     " FROM qtemp/text198 ";

                    ds = RetriveAsDataSet(sql_198);

                }
                CloseConnectioDAL();

            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }

            return ds;
        }

        public DataSet Inquiry_Activity_By_Kessai(string BRN, string dateFrom, string dateTo)
        {
            DataSet ds = new DataSet();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;
            try
            {
                string where = " WHERE p1brn = " + BRN;
                if (dateFrom == dateTo)
                {
                    where += " AND P1STDT = " + dateFrom;
                }
                else if (dateFrom != dateTo)
                {
                    where += " and P1STDT >=  " + dateFrom +
                             " and P1STDT <= " + dateTo;
                }

                string sql = " SELECT SUBSTR(DIGITS(P1STDT),7,2)||'/'||SUBSTR(DIGITS(P1STDT),5,2)||'/'||SUBSTR(DIGITS(P1STDT),1,4) ApproveDate , P1AUTH User_ , " +
                             " (SELECT count(p1apno) FROM ilms01 AS a WHERE P1STDT=ilms01.P1STDT  AND P1AUTH=ilms01.P1AUTH AND TRIM(a.P1AUTH) <> '' AND p1aprj = 'AP') AS Approve, " +
                             " (SELECT count(p1apno) FROM ilms01 AS a WHERE P1STDT=ilms01.P1STDT  AND P1AUTH=ilms01.P1AUTH AND TRIM(a.P1AUTH) <> '' AND p1aprj = 'RJ') AS Reject, " +
                             " (SELECT count(p1apno) FROM ilms01 AS a WHERE P1STDT=ilms01.P1STDT  AND P1AUTH=ilms01.P1AUTH AND TRIM(a.P1AUTH) <> '' AND p1aprj = 'CN') AS Cancel, " +
                             " " +
                             " (SELECT sum(p1pram)   FROM ilms01 AS a WHERE P1STDT=ilms01.P1STDT AND P1AUTH=ilms01.P1AUTH AND TRIM(a.P1AUTH) <> ''  )  AS Prin_Amount  " +
                             " FROM ilms01  " +
                             where + " AND  TRIM(P1AUTH) <> ''   AND p1aprj IN ('AP','RJ','CN') " +
                             " GROUP BY P1STDT,P1AUTH  " +
                             " ORDER BY P1STDT,P1AUTH WITH UR ";



                ds = RetriveAsDataSet(sql);
                CloseConnectioDAL();

            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }

            return ds;
        }

        public DataSet Inquiry_Activity_By_Credit(string BRN, string dateFrom, string dateTo)
        {
            DataSet ds = new DataSet();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;
            try
            {
                string where = " WHERE p1brn = " + BRN;
                if (dateFrom == dateTo)
                {
                    where += " AND p1avdt = " + dateFrom;
                }
                else if (dateFrom != dateTo)
                {
                    where += " and p1avdt >=  " + dateFrom +
                             " and p1avdt <= " + dateTo;
                }

                string sql = " SELECT SUBSTR(DIGITS(p1avdt),7,2)||'/'||SUBSTR(DIGITS(p1avdt),5,2)||'/'||SUBSTR(DIGITS(p1avdt),1,4) ApproveDate , p1crcd User_ ,  " +
                             " (SELECT count(p1apno) FROM ilms01 AS a WHERE p1avdt=ilms01.p1avdt and p1crcd=ilms01.p1crcd AND (p1aprj = 'AP' OR p1aprj = 'AC')) AS Approve, " +
                             " (SELECT count(p1apno) FROM ilms01 AS a WHERE p1avdt=ilms01.p1avdt and p1crcd=ilms01.p1crcd AND p1aprj =  'RJ' ) AS Reject, " +
                             " (SELECT count(p1apno) FROM ilms01 AS a WHERE p1avdt=ilms01.p1avdt and p1crcd=ilms01.p1crcd and p1aprj = 'CN') AS Cancel, " +
                             " " +
                             " (SELECT SUM(p1pram)   FROM ilms01 AS a WHERE p1avdt=ilms01.p1avdt and p1crcd=ilms01.p1crcd)  AS Prin_Amount  " +
                             " FROM ilms01  " +
                             where + " " +
                             " GROUP BY p1avdt,p1crcd " +
                             " ORDER BY p1avdt,p1crcd with UR ";



                ds = RetriveAsDataSet(sql);
                CloseConnectioDAL();

            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }

            return ds;
        }



        //***** report change occupation ****//
        public bool CALL_ILR341C2(string PFRYMD, string PTOYMD, string PDOCTPY, string PCHANEL, string PUSER, string POCCCD, string POPTS, string strBizInit, string strBranchNo)
        {
            string m_UdpTime = DateTime.Now.ToString("HHmmss");
            m_LastError = "";
            string WSTEXT = "";
            iDB2Command cmd = new iDB2Command();

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.CommandText = "ILR341C2";
            // Parameter In

            cmd.Parameters.Add("PFRYMD", iDB2DbType.iDB2Decimal, 0).Value = PFRYMD;
            cmd.Parameters["PFRYMD"].Precision = 8;
            cmd.Parameters["PFRYMD"].Scale = 0;

            cmd.Parameters.Add("PTOYMD", iDB2DbType.iDB2Decimal, 0).Value = PTOYMD;
            cmd.Parameters["PTOYMD"].Precision = 8;
            cmd.Parameters["PTOYMD"].Scale = 0;

            cmd.Parameters.Add("PDOCTPY", iDB2DbType.iDB2Char, 1).Value = PDOCTPY;
            cmd.Parameters.Add("PCHANEL", iDB2DbType.iDB2Char, 1).Value = PCHANEL;
            cmd.Parameters.Add("PUSER", iDB2DbType.iDB2Char, 10).Value = PUSER;
            cmd.Parameters.Add("POCCCD", iDB2DbType.iDB2Char, 3).Value = POCCCD;
            cmd.Parameters.Add("POPTS", iDB2DbType.iDB2Char, 1).Value = POPTS;

            if (m_da400.ExecuteSubRoutine("ILR341C2", ref cmd, strBizInit, strBranchNo))
            {
                return true;
            }
            else
                return false;
        }
        public DataSet RP_Change_Occupation(string PFRYMD, string PTOYMD, string PDOCTPY, string PCHANEL, string PUSER, string POCCCD, string POPTS)
        {
            DataSet ds = new DataSet();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;
            try
            {


                bool Rescall = CALL_ILR341C2(PFRYMD, PTOYMD, PDOCTPY, PCHANEL, PUSER, POCCCD, POPTS, m_UserInfo.BizInit, m_UserInfo.BranchNo);
                if (Rescall)
                {

                    string sql_198 = " SELECT * FROM  (SELECT  substr(F198,2,198) F198 ,  " +
                                     " CASE when (substr(F198,1,1) = '1') THEN '1' ELSE '0' END  newpage " +
                                     " FROM qtemp/text198 ) a WHERE a.newpage <> '1' ";

                    ds = RetriveAsDataSet(sql_198);

                }
                CloseConnectioDAL();

            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }

            return ds;
        }

        public DataSet RP_Change_Occupation_sql(string PFRYMD, string PTOYMD, string PDOCTPY, string PCHANEL, string PUSER, string POCCCD, string POPTS)
        {
            DataSet ds = new DataSet();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;
            try
            {
                string where = "";
                if (PDOCTPY == "1")
                {
                    where += " AND D5FLAG = 'Y' ";
                }
                else if (PDOCTPY == "2")
                {
                    where += " AND D5FLAG = 'N' ";
                }

                if (PCHANEL.Trim() != "")
                {
                    where += " AND TMCHANNEL = '" + PCHANEL.Trim() + "' ";
                }

                if (PUSER.Trim() != "")
                {
                    where += " AND D5FUSR = '" + PUSER.Trim() + "' ";
                }

                if (POCCCD.Trim() != "")
                {
                    where += " AND D5OCCN = '" + POCCCD + "' ";
                }


                string sql = " SELECT * FROM ( " +
                             " SELECT  SUBSTR(DIGITS(A.D5FDAT),7,2) ||'/'||SUBSTR(DIGITS(A.D5FDAT),5,2)||'/'||SUBSTR(DIGITS(A.D5FDAT),1,4)   FDATE, " +
                             " CASE WHEN A.M00EBC <> 0 THEN " +
                             " SUBSTR(DIGITS(A.M00EBC),1,4) ||'-'||SUBSTR(DIGITS(A.M00EBC),5,4)||'-'||SUBSTR(DIGITS(A.M00EBC),9,4)||'-'||SUBSTR(DIGITS(A.M00EBC),13,4) ELSE '' END  EBC_NO, " +
                             " CASE A.D5FBUS  WHEN 'PW' THEN  CASE  WHEN PWMS01.P1CNT# <> 0 THEN DIGITS(PWMS01.P1CNT#) ELSE '' END  " +
                             " WHEN 'RL' THEN  CASE  WHEN RLMS01.P1CNT# <> 0 THEN DIGITS(RLMS01.P1CNT#) ELSE '' END " +
                             " WHEN 'IL' THEN  CASE  WHEN ILMS01.P1CONT <> 0 THEN DIGITS(ILMS01.P1CONT) ELSE '' END " +
                             " ELSE  '' END TMCONT , " +
                             " D5OCCO , " +
                             " D5OCCN, " +
                             " CASE  WHEN (A.D5FLBY = '01' AND ILMS01.P1APPT = '01') THEN '1' " +
                             " WHEN (A.D5FLBY = '01' AND ILMS01.P1APPT = '02') THEN '2' " +
                             " WHEN (A.D5FLBY = '02' ) THEN '3' " +
                             " WHEN (A.D5FLBY = '07' ) THEN '5' " +
                             " ELSE '4' END   TMCHANNEL , " +
                             " CASE  WHEN (A.D5FLBY = '01' AND ILMS01.P1APPT = '01') THEN 'NORMAL'||A.FLAG_RV " +
                             " WHEN (A.D5FLBY = '01' AND ILMS01.P1APPT = '02') THEN 'EBC'||A.FLAG_RV " +
                             " WHEN (A.D5FLBY = '02' ) THEN 'UPD.SALARY'||A.FLAG_RV " +
                             " WHEN (A.D5FLBY = '07' ) THEN 'CHG CUST.'||A.FLAG_RV " +
                             " ELSE 'FULL APP. ' END   TMCHANNEL_name ,D5FUSR , " +
                             " CASE A.D5FBUS   WHEN 'PW' THEN DIGITS(PWMS01.P1APDT) " +
                             " WHEN 'RL' THEN DIGITS(RLMS01.P1APDT) " +
                             " WHEN 'IL' THEN DIGITS(ILMS01.P1APDT) " +
                             " ELSE   '' END TMAPDT , D5FLAG, " +
                             " CASE  WHEN  (A.D5FLAG = 'Y') OR (D5FLAG='N' AND D5QUTA > 0) THEN 'N' " +
                             " ELSE  'Y' END TMDOCR ,  " +
                             " TRIM(M00TNM) || '  '||TRIM(M00TSN)  CUSTNAME , DIGITS(M00CSN) M00CSN " +
                             " FROM " +
                             "( SELECT csms035.*,M00TNM,M00TSN,M00EBC,CSMS032.*,CSMS032HS.*,m00csn, " +
                             " CASE WHEN D5FLBY = '01' THEN CASE  WHEN (D5FBUS = CSMS032.D3FBUS AND D5FAPN = CSMS032.D3FAPN AND CSMS032.D3CBUS = 'RV' ) THEN '(RV1)' ELSE ''  END  " +
                             " WHEN D5FLBY = '02' THEN CASE WHEN (D5FBUS = CSMS032.D3FBUS AND D5FSLD = CSMS032.D3FSLD AND D5FSLT = CSMS032.D3FSLT  AND CSMS032.D3CBUS = 'RV') THEN '(RV1)' ELSE '' END " +
                             " WHEN D5FLBY = '01' THEN CASE WHEN (D5FBUS = CSMS032HS.D3FBUS AND D5FAPN = CSMS032HS.D3FAPN AND CSMS032HS.D3CBUS = 'RV' ) THEN '(RV1)'  ELSE ''  END " +
                             " WHEN D5FLBY = '02' THEN CASE WHEN (D5FBUS = CSMS032HS.D3FBUS AND D5FSLD = CSMS032HS.D3FSLD AND D5FSLT = CSMS032HS.D3FSLT AND CSMS032HS.D3CBUS = 'RV' ) THEN '(RV1)' ELSE ''  END  " +
                             " ELSE '' END FLAG_RV " +
                             " FROM csms035 JOIN " +
                             " CSMS00 ON D5CSNO = M00CSN " +
                             " LEFT JOIN CSMS032  ON  D5CSNO = CSMS032.D3CSNO " +
                             " LEFT JOIN CSMS032HS  ON  D5CSNO = CSMS032HS.D3CSNO " +
                             " WHERE ((D5FBUS IN ('IL','')) OR (D5FLBY IN ('01','02','03','04','07')) ) " +
                             " AND D5FDAT  BETWEEN " + PFRYMD + " AND " + PTOYMD +
                             " AND D5OCCO <> D5OCCN ) A " +
                             " LEFT JOIN  ILMS01 ON  ( A.D5FBRN = ILMS01.P1BRN   AND A.D5FAPN = ILMS01.P1APNO ) " +
                             " LEFT JOIN  PWMS01 ON  ( A.D5FBRN = PWMS01.P1BRN   AND A.D5FAPN = PWMS01.P1APNO ) " +
                             " LEFT JOIN  RLMS01 ON  ( A.D5FBRN = RLMS01.P1BRN   AND A.D5FAPN = RLMS01.P1APNO ) " +
                             " WHERE 1 = 1 " +
                             " ORDER BY A.D5FDAT ,A.M00EBC " +
                             " ) B  WHERE 1 = 1 " + where
                             ;

                ds = RetriveAsDataSet(sql);


                CloseConnectioDAL();

            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }

            return ds;
        }



        //***** report update salary ****//
        public bool CALL_CSR001C2(string WSDTE, string WEDTE, string DUSID, string DUSBUS, string DUSUSR, string DUSRVS, string DUSSTD, string strBizInit, string strBranchNo)
        {
            string m_UdpTime = DateTime.Now.ToString("HHmmss");
            m_LastError = "";
            string WSTEXT = "";
            iDB2Command cmd = new iDB2Command();

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.CommandText = "CSR001C2";
            // Parameter In

            cmd.Parameters.Add("WSDTE", iDB2DbType.iDB2Decimal, 0).Value = WSDTE;
            cmd.Parameters["WSDTE"].Precision = 8;
            cmd.Parameters["WSDTE"].Scale = 0;

            cmd.Parameters.Add("WEDTE", iDB2DbType.iDB2Decimal, 0).Value = WEDTE;
            cmd.Parameters["WEDTE"].Precision = 8;
            cmd.Parameters["WEDTE"].Scale = 0;

            cmd.Parameters.Add("DUSID", iDB2DbType.iDB2Char, 15).Value = DUSID;
            cmd.Parameters.Add("DUSBUS", iDB2DbType.iDB2Char, 3).Value = DUSBUS;
            cmd.Parameters.Add("DUSUSR", iDB2DbType.iDB2Char, 10).Value = DUSRVS;
            cmd.Parameters.Add("DUSRVS", iDB2DbType.iDB2Char, 1).Value = DUSRVS;
            cmd.Parameters.Add("DUSSTD", iDB2DbType.iDB2Char, 1).Value = DUSSTD;

            if (m_da400.ExecuteSubRoutine("CSR001C2", ref cmd, strBizInit, strBranchNo))
            {
                return true;
            }
            else
                return false;
        }
        public DataSet RP_Update_Salary(string WSDTE, string WEDTE, string DUSID, string DUSBUS, string DUSUSR, string DUSRVS, string DUSSTD)
        {
            DataSet ds = new DataSet();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;
            try
            {

                bool Rescall = CALL_CSR001C2(WSDTE, WEDTE, DUSID, DUSBUS, DUSUSR, DUSRVS, DUSSTD, m_UserInfo.BizInit, m_UserInfo.BranchNo);
                if (Rescall)
                {

                    string sql_198 = " SELECT  substr(F198,2,198) F198 ,  " +
                                     " CASE when (substr(F198,1,1) = '1') THEN '1' ELSE '0' END  newpage " +
                                     " FROM qtemp/text198  ";

                    ds = RetriveAsDataSet(sql_198);

                }
                CloseConnectioDAL();

            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }

            return ds;
        }

        //***** report remain update salary  ****//
        public bool CALL_CSR008C2(string WSDTE, string WEDTE, string DUSID, string DBRN, string strBizInit, string strBranchNo)
        {
            string m_UdpTime = DateTime.Now.ToString("HHmmss");
            m_LastError = "";
            string WSTEXT = "";
            iDB2Command cmd = new iDB2Command();

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.CommandText = "CSR008C2";
            // Parameter In

            cmd.Parameters.Add("WSDTE", iDB2DbType.iDB2Decimal, 0).Value = WSDTE;
            cmd.Parameters["WSDTE"].Precision = 8;
            cmd.Parameters["WSDTE"].Scale = 0;

            cmd.Parameters.Add("WEDTE", iDB2DbType.iDB2Decimal, 0).Value = WEDTE;
            cmd.Parameters["WEDTE"].Precision = 8;
            cmd.Parameters["WEDTE"].Scale = 0;

            cmd.Parameters.Add("DUSID", iDB2DbType.iDB2Char, 15).Value = DUSID;

            cmd.Parameters.Add("DBRN", iDB2DbType.iDB2Decimal, 0).Value = DBRN;
            cmd.Parameters["DBRN"].Precision = 3;
            cmd.Parameters["DBRN"].Scale = 0;


            if (m_da400.ExecuteSubRoutine("CSR008C2", ref cmd, strBizInit, strBranchNo))
            {
                return true;
            }
            else
                return false;
        }
        public DataSet RP_REMAIN_UPDATE_SALARY(string WSDTE, string WEDTE, string DUSID, string DBRN)
        {
            DataSet ds = new DataSet();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;
            try
            {

                bool Rescall = CALL_CSR008C2(WSDTE, WEDTE, DUSID, DBRN, m_UserInfo.BizInit, m_UserInfo.BranchNo);
                if (Rescall)
                {

                    string sql_198 = " SELECT  substr(F198,2,198) F198 ,  " +
                                     " CASE when (substr(F198,1,1) = '1') THEN '1' ELSE '0' END  newpage " +
                                     " FROM qtemp/text198  ";

                    ds = RetriveAsDataSet(sql_198);

                }
                CloseConnectioDAL();

            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }

            return ds;
        }


        //***** report recieve doc update salary  ****//
        public bool CALL_CSR009C2(string WSTYP, string WSDTE, string WEDTE, string DUSID, string DBRN, string strBizInit, string strBranchNo)
        {
            string m_UdpTime = DateTime.Now.ToString("HHmmss");
            m_LastError = "";
            string WSTEXT = "";
            iDB2Command cmd = new iDB2Command();

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.CommandText = "CSR009C2";
            // Parameter In

            cmd.Parameters.Add("WSTYP", iDB2DbType.iDB2Char, 1).Value = WSTYP;

            cmd.Parameters.Add("WSDTE", iDB2DbType.iDB2Decimal, 0).Value = WSDTE;
            cmd.Parameters["WSDTE"].Precision = 8;
            cmd.Parameters["WSDTE"].Scale = 0;

            cmd.Parameters.Add("WEDTE", iDB2DbType.iDB2Decimal, 0).Value = WEDTE;
            cmd.Parameters["WEDTE"].Precision = 8;
            cmd.Parameters["WEDTE"].Scale = 0;

            cmd.Parameters.Add("DUSID", iDB2DbType.iDB2Char, 15).Value = DUSID;

            cmd.Parameters.Add("DBRN", iDB2DbType.iDB2Decimal, 0).Value = DBRN;
            cmd.Parameters["DBRN"].Precision = 3;
            cmd.Parameters["DBRN"].Scale = 0;


            if (m_da400.ExecuteSubRoutine("CSR009C2", ref cmd, strBizInit, strBranchNo))
            {
                return true;
            }
            else
                return false;
        }
        public DataSet RP_RECIEVE_DOC_UPDATE_SALARY(string WSTYP, string WSDTE, string WEDTE, string DUSID, string DBRN)
        {
            DataSet ds = new DataSet();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;
            try
            {

                bool Rescall = CALL_CSR009C2(WSTYP, WSDTE, WEDTE, DUSID, DBRN, m_UserInfo.BizInit, m_UserInfo.BranchNo);
                if (Rescall)
                {

                    string sql_198 = " SELECT  substr(F198,2,198) F198 ,  " +
                                     " CASE when (substr(F198,1,1) = '1') THEN '1' ELSE '0' END  newpage " +
                                     " FROM qtemp/text198  ";

                    ds = RetriveAsDataSet(sql_198);

                }
                CloseConnectioDAL();

            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }

            return ds;
        }


        public DataSet RP_Product_code(string type)
        {
            DataSet ds = new DataSet();
            try
            {
                UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
                UserInfomation = m_userInfo;
                string StrWhere = "";

                if (type.Trim() != "")
                {
                    StrWhere = "WHERE  t41typ =" + type + " AND t41del = '' ";
                }

                string sql = " SELECT ifnull(t41typ,0) AS t41typ, ifnull(t40des,'') AS t40des, " +
                             " ifnull(t41cod,0) AS t41cod, ifnull(t41des,'') AS t41des " +
                             " FROM iltb41 " +
                             " LEFT JOIN iltb40 ON(t41typ=t40typ) " +
                             StrWhere + " ORDER BY t41typ,t41cod with UR ";


                ds = RetriveAsDataSet(sql);
                CloseConnectioDAL();

            }
            catch (Exception ex)
            {
            }
            return ds;
        }

        public DataSet RP_Product_Model(string Type, string brand, string code)
        {
            DataSet ds = new DataSet();
            try
            {
                UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
                UserInfomation = m_userInfo;
                string StrWhere = " Where t43del = '' ";


                if (Type.Trim() != "")
                {
                    StrWhere += " AND t43typ = " + Type;
                }
                if (brand.Trim() != "")
                {
                    StrWhere += " AND t43brd = " + brand;
                }
                if (code.Trim() != "")
                {
                    StrWhere += " and t43cod = " + code;
                }

                string sql = " SELECT ifnull(t43typ,0) as t43typ, ifnull(t43brd,0) as t43brd, ifnull(t43cod,0) AS t43cod, " +
                             " ifnull(t43mdl,0) AS t43mdl, ifnull(t43des,'') AS t43des, " +
                             " ifnull(t40des,'') AS t40des, ifnull(t42des,'') AS t42des, ifnull(t41des,'') AS t41des " +
                             " FROM iltb43 " +
                             " LEFT JOIN iltb40 ON (t40typ = t43typ) " +
                             " LEFT JOIN iltb42 ON (t42brd = t43brd) " +
                             " LEFT JOIN iltb41 ON (t43typ = t41typ AND t43cod = t41cod) " +
                             StrWhere + " ORDER BY t43typ,t43brd,t43cod,t43mdl WITH UR ";


                ds = RetriveAsDataSet(sql);
                CloseConnectioDAL();

            }
            catch (Exception ex)
            {
            }
            return ds;
        }

        public DataSet RP_Product_Item(string Type, string brand, string code, string model)
        {
            DataSet ds = new DataSet();
            try
            {
                UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
                UserInfomation = m_userInfo;
                string StrWhere = " Where t44del = '' ";


                if (Type.Trim() != "")
                {
                    StrWhere += " AND t44typ = " + Type;
                }
                if (brand.Trim() != "")
                {
                    StrWhere += " AND t44brd = " + brand;
                }
                if (code.Trim() != "")
                {
                    StrWhere += " AND t44cod = " + code;
                }
                if (model.Trim() != "")
                {
                    StrWhere += " AND t44mdl= " + model;
                }

                string sql = " select t44typ as t44typ, t44brd as t44brd, t44cod as t44cod, " +
                             " t44mdl as t44mdl, t44itm as t44itm, " +
                             " t44des as t44des, t40des as t40des, t42des as t42des, " +
                             " t41des as t41des, t43des as t43des " +
                             " FROM iltb44 " +
                             " LEFT JOIN iltb40 ON (t40typ=t44typ) " +
                             " LEFT JOIN iltb42 ON (t42brd=t44brd) " +
                             " LEFT JOIN iltb41 ON (t44typ=t41typ AND t44cod = t41cod) " +
                             " LEFT JOIN iltb43 ON (t44typ=t43typ AND t44brd=t43brd and " +
                             " t44cod = t43cod AND t44mdl = t43mdl) " +
                              StrWhere + " ORDER BY t44typ,t44brd,t44cod,t44mdl,t44itm with UR ";


                ds = RetriveAsDataSet(sql);
                CloseConnectioDAL();

            }
            catch (Exception ex)
            {
            }
            return ds;
        }

        //****     Clear RS Flag   ****//

        public bool CALL_CSGC216(string PICSNO, string PIIDNO, string strBizInit, string strBranchNo, ref string POERR, ref string POMSG)
        {
            iDB2Command cmd = new iDB2Command();

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.CommandText = "CSGC216";

            // Parameter In

            cmd.Parameters.Add("PIMODE", iDB2DbType.iDB2Char, 1).Value = "C";

            cmd.Parameters.Add("PICSNO", iDB2DbType.iDB2Decimal, 0).Value = PICSNO;
            cmd.Parameters["PICSNO"].Precision = 8;
            cmd.Parameters["PICSNO"].Scale = 0;

            cmd.Parameters.Add("PIIDNO", iDB2DbType.iDB2Char, 15).Value = PIIDNO;

            cmd.Parameters.Add("PIFLAG", iDB2DbType.iDB2Char, 1).Value = " ";

            cmd.Parameters.Add("PIFFRM", iDB2DbType.iDB2Char, 2).Value = "AP";


            cmd.Parameters.Add("POERR", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;
            cmd.Parameters["POERR"].Value = "";

            cmd.Parameters.Add("POMSG", iDB2DbType.iDB2Char, 100).Direction = ParameterDirection.Output;
            cmd.Parameters["POMSG"].Value = "";

            //  parameters out
            if (m_da400.ExecuteSubRoutine("CSGC216", ref cmd, strBizInit, strBranchNo))
            {
                POERR = cmd.Parameters["POERR"].Value.ToString().Trim();
                POMSG = cmd.Parameters["POMSG"].Value.ToString().Trim();
                return true;
            }
            else
                return false;
        }

        #endregion


        //***  MASTER FILE  & PHASE 3 ***//


        public bool Call_ILSR92(string WILNTY, string WIRECD, ref string WORUNN, ref string WOLENG, ref string WOERRF, ref string WOEMSG)
        {
            iDB2Command cmd = new iDB2Command();

            /* m_da400.AS400Lib = EB_Service.Commons.AppConfig.GNPGMLib;  *///GeneralLib;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ILSR92";

            //Input
            cmd.Parameters.Add("WILNTY", iDB2DbType.iDB2Char, 2).Value = WILNTY;
            cmd.Parameters.Add("WIRECD", iDB2DbType.iDB2Char, 3).Value = WIRECD;

            //Output
            cmd.Parameters.Add("WORUNN", iDB2DbType.iDB2Char, 16).Direction = ParameterDirection.Output;
            cmd.Parameters["WORUNN"].Value = "";

            cmd.Parameters.Add("WOLENG", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;
            cmd.Parameters["WOLENG"].Value = "";

            cmd.Parameters.Add("WOERRF", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;
            cmd.Parameters["WOERRF"].Value = "";

            cmd.Parameters.Add("WOEMSG", iDB2DbType.iDB2Char, 60).Direction = ParameterDirection.Output;
            cmd.Parameters["WOEMSG"].Value = "";


            if (m_da400.ExecuteSubRoutine("ILSR92", ref cmd, WILNTY, WIRECD))
            {
                WORUNN = cmd.Parameters["WORUNN"].Value.ToString().Trim();
                WOLENG = cmd.Parameters["WOLENG"].Value.ToString().Trim();
                WOERRF = cmd.Parameters["WOERRF"].Value.ToString().Trim();
                WOEMSG = cmd.Parameters["WOEMSG"].Value.ToString().Trim();

                return true;
            }
            else
            {
                m_da400.CloseConnect();
                return false;
            }
        }


        public bool Call_ILSR10(string WIPMTY, string WIADD, string WIPROV, string WIRUNN, ref string WOCODE, ref string WOERR, ref string WOEMSG)
        {
            iDB2Command cmd = new iDB2Command();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ILSR10";

            //Input
            cmd.Parameters.Add("WIPMTY", iDB2DbType.iDB2Char, 1).Value = WIPMTY;
            cmd.Parameters.Add("WIADD", iDB2DbType.iDB2Char, 1).Value = WIADD;
            cmd.Parameters.Add("WIPROV", iDB2DbType.iDB2Char, 2).Value = WIPROV;
            cmd.Parameters.Add("WIRUNN", iDB2DbType.iDB2Char, 6).Value = WIRUNN;

            //Output
            cmd.Parameters.Add("WOCODE", iDB2DbType.iDB2Char, 12).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("WOERR", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("WOEMSG", iDB2DbType.iDB2Char, 60).Direction = ParameterDirection.Output;

            if (m_da400.ExecuteSubRoutine("ILSR10", ref cmd))
            {
                WOCODE = cmd.Parameters["WOCODE"].Value.ToString().Trim();
                WOERR = cmd.Parameters["WOERR"].Value.ToString().Trim();
                WOEMSG = cmd.Parameters["WOEMSG"].Value.ToString().Trim();
                return true;
            }
            else
            {
                m_da400.CloseConnect();
                return false;
            }
        }

        //** MAKER NOTE ilms15** //
        public DataSet getNote_ILMS15(string vendor)
        {
            DataSet ds = new DataSet();
            try
            {
                UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
                UserInfomation = m_userInfo;

                string sql = " SELECT P15MAK,P15NSQ,P15LSQ,P15NOT, " +
                             " SUBSTR(DIGITS(P15UDT),7,2)||'/'||SUBSTR(DIGITS(P15UDT),5,2)||'/'|| SUBSTR(DIGITS(P15UDT),1,4) P15UDT , " +
                             " SUBSTR(DIGITS(P15UTM),1,2)||':'||SUBSTR(DIGITS(P15UTM),3,2)||':'||SUBSTR(DIGITS(P15UTM),5,2)  P15UTM " +
                             " FROM ilms15 WHERE P15MAK = " + vendor + " ORDER BY P15NSQ , P15LSQ ASC";
                DataTable dt = new DataTable();
                dt.Columns.Add("Seq", typeof(string));
                dt.Columns.Add("NoteDESC", typeof(string));
                dt.Columns.Add("UPD_DATE", typeof(string));
                dt.Columns.Add("UPD_TIME", typeof(string));

                DataSet ds_note = RetriveAsDataSet(sql);
                if (check_dataset(ds_note))
                {
                    string note_seq = ds_note.Tables[0].Rows[0]["P15NSQ"].ToString();
                    int count = 1;
                    foreach (DataRow dr_seq in ds_note.Tables[0].Rows)
                    {
                        string descNote = "";

                        if (note_seq != dr_seq["P15NSQ"].ToString().Trim())
                        {
                            note_seq = dr_seq["P15NSQ"].ToString().Trim();
                            count = 1;
                        }

                        if (count == 1)
                        {
                            DataRow[] noteLine = ds_note.Tables[0].Select("P15NSQ = " + note_seq);
                            foreach (DataRow row in noteLine)
                            {
                                descNote += row["P15NOT"].ToString();

                            }

                            dt.Rows.Add(note_seq, descNote, noteLine[0]["P15UDT"], noteLine[(noteLine.Count() - 1)]["P15UTM"]);
                            count = 0;
                        }


                        // add to datatable //


                    }
                    ds.Tables.Add(dt);

                }
                else
                {
                    ds.Tables.Add(dt);
                }
                CloseConnectioDAL();

            }
            catch (Exception ex)
            {
            }
            return ds;
        }


        //Create ILSR99 date : 2020/10/25

        public bool Call_ILSR99(string WIBRN, string WILNTY, string WIRECD, string strBizInit, string strBranchNo, ref string WORUNN)
        {
            iDB2Command cmd = new iDB2Command();

            /* m_da400.AS400Lib = EB_Service.Commons.AppConfig.GNPGMLib;  *///GeneralLib;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ILSR99";

            //Input
            cmd.Parameters.Add("WIBRN", iDB2DbType.iDB2Char, 3).Value = WIBRN;
            cmd.Parameters.Add("WILNTY", iDB2DbType.iDB2Char, 2).Value = WILNTY;
            cmd.Parameters.Add("WIRECD", iDB2DbType.iDB2Char, 3).Value = WIRECD;

            //Output
            cmd.Parameters.Add("WORUNN", iDB2DbType.iDB2Char, 16).Direction = ParameterDirection.Output;
            cmd.Parameters["WORUNN"].Value = "";


            if (m_da400.ExecuteSubRoutine("ILSR99", ref cmd, strBizInit, strBranchNo))
            {
                WORUNN = cmd.Parameters["WORUNN"].Value.ToString().Trim();
                return true;
            }
            else
            {
                m_da400.CloseConnect();
                return false;
            }
            //return false;
        }

        //**report EDC transaction**//

        public DataSet RP_EDC_TRANSACTION(string strDate, string endDate, string terminal_id, string stan, string TrackNo, string esbCard, string type, string contractNo, string brn, string sts, string vendor, string sortTime, string strTime, string endTime)
        {
            DataSet ds = new DataSet();
            try
            {
                UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
                UserInfomation = m_userInfo;
                string StrWhere = " WHERE p7eidt BETWEEN " + strDate + " AND " + endDate;

                if (type.Trim() != "")
                {
                    if (type.Trim() != "04")
                    {
                        StrWhere += " AND p7type = '" + type + "' ";

                    }
                    else
                    {
                        StrWhere += " AND p7type in ('04','05','06') ";
                    }
                }
                if (terminal_id.Trim() != "")
                {
                    StrWhere += " AND p7tid = '" + terminal_id + "' ";
                }
                if (stan.Trim() != "")
                {
                    StrWhere += " AND p7seq = '" + stan + "' ";
                }
                if (TrackNo.Trim() != "")
                {
                    StrWhere += " AND p7trac = '" + TrackNo + "' ";
                }
                if (vendor.Trim() != "")
                {
                    StrWhere += " AND p7ven = '" + vendor + "' ";
                }
                if (contractNo.Trim() != "")
                {
                    StrWhere += " AND p7cont = '" + contractNo + "' ";
                }
                if (sts.Trim() != "")
                {
                    StrWhere += " AND p7aprj = '" + sts + "' ";
                }
                if (esbCard.Trim() != "")
                {
                    StrWhere += " AND p7ebc = " + esbCard;
                }
                if (brn.Trim() != "")
                {
                    StrWhere += " AND exists(select * from ilms10 where ilms70.p7ven=p10ven and p10brn = " + brn + ") ";
                }

                if (strTime.Trim() != "000000" && endTime.Trim() != "000000")
                {
                    StrWhere += " AND p7eitm BETWEEN " + strTime + " AND " + endTime;
                }
                if (strTime.Trim() != "000000" && endTime.Trim() == "000000")
                {
                    StrWhere += " AND p7eitm >= " + strTime;
                }
                if (strTime.Trim() == "000000" && endTime.Trim() != "000000")
                {
                    StrWhere += " AND p7eitm <= " + endTime;
                }

                string Sort = "";
                if (sortTime.Trim() == "A")
                {
                    Sort = " ASC ";
                }
                else if (sortTime.Trim() == "Z")
                {
                    Sort = " DESC ";
                }



                string sql = " SELECT p7tid, substr(digits(p7eidt),7,2)||'/'||substr(digits(p7eidt),5,2)||'/'||substr(digits(p7eidt),1,4) as p7eidt," +
                             " substr(digits(p7eitm),1,2)||':'||substr(digits(p7eitm),3,2)||':'||substr(digits(p7eitm),5,2) as p7eitm, " +
                             " case when p7type = '01' then 'Chk Avai' " +
                             " when p7type = '02' then 'Approve' " +
                             " when p7type = '03' then 'Void' " +
                             " when p7type = '04' then 'Reversal' " +
                             " when p7type = '40' then 'SAF1' " +
                             " when p7type = '41' then 'SAF2' else '' end as p7type, " +
                             " p7tid, p7trac, p7seq, " +
                             " substr(digits(p7ebc),1,4)||'-'||substr(digits(p7ebc),5,4)||'-'|| " +
                             " substr(digits(p7ebc),9,4)||'-'||substr(digits(p7ebc),13,4) as p7ebc, " +
                             " substr(p7ven,1,2)||'-'||substr(p7ven,3,6)||'-'|| " +
                             " substr(p7ven,9,3)||'-'||substr(p7ven,12,1) as p7ven, " +
                             " p7camp, p7item, " +
                             " p7pric, p7down, p7totm, " +
                             " p7cont, p7aprj, ifnull(t7rjds,'') as t7rjds, Int(p7tcl) as p7tcl, Int(p7tca) as p7tca, " +
                             " Int(p7rca) as p7rca, Int(p7pat) as p7pat  " +
                             " from ilms70 " +
                             " left join iltb77 on(p7rjcd = t7rjcd) " + StrWhere +
                             " ORDER BY p7tid, p7eidt, p7eitm  " + Sort + " WITH UR";

                ds = RetriveAsDataSet(sql);
                CloseConnectioDAL();

            }
            catch (Exception ex)
            {
            }
            return ds;
        }

        //***  Get Campaign for Proposal ***//
        public DataSet GetCampaign_Proposal(string code, string name)
        {
            DataSet ds = new DataSet();
            try
            {
                UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
                UserInfomation = m_userInfo;


                string StrWhere = " ";


                if (code.Trim() != "")
                {
                    StrWhere += " cmp = '" + code + "' ";
                }

                if (name.Trim() != "")
                {
                    StrWhere += " cmp_name = '" + name + "' ";
                }


                string sql = " SELECT * FROM " +
                             " (SELECT c01cmp as cmp, c01cnm as cmp_name FROM ilcp01 " +
                             " WHERE c01brn = " + m_userInfo.BranchApp +
                             " UNION " +
                             " SELECT c09cmp as cmp, c01cnm as brn FROM ilcp09 " +
                             " LEFT JOIN ilcp01 ON(c09cmp=c01cmp) " +
                             " WHERE c09brn = " + m_userInfo.BranchApp + " with ur) " +
                             StrWhere;

                ds = RetriveAsDataSet(sql);
                CloseConnectioDAL();

            }
            catch (Exception ex)
            {
            }
            return ds;
        }

        //*** Get Proposal Campaign ***//
        public DataSet getAllDetail_Proposal(string camp)
        {
            DataSet ds = new DataSet();
            try
            {
                UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
                UserInfomation = m_userInfo;





                string sql_rate = " SELECT ifnull(count(*),0) as Count1, ifnull(max(c02csq),0) as Max1 " +
                             " FROM ilcp02 WHERE c02cmp = " + camp;

                ds = RetriveAsDataSet(sql_rate);

                string sql_desc = " Select c01cmp,c01cnm,c01sty,c01vdc,c01mkc,c01sdt,c01edt, " +
                                  " c01cad,c01cld,c01srt,c01nxd,c01lty,p10nam,t46enm, " +
                                  " user as user_login, " + m_UpdDate;



                CloseConnectioDAL();


            }
            catch (Exception ex)
            {

            }
            return ds;
        }


        //public DataSet getCampaign_Summary(string startCamp,string endCamp,string campCode,string campName,string campSts,string branch,string vendor,string createUser,string createDateF,string createDateT,string closeDate)
        //{
        //    DataSet ds = new DataSet();
        //    try
        //    {
        //        UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
        //        UserInfomation = m_userInfo;

        //        string sql_rate = " SELECT DISTINCT " + " " + " ,DIGITS(C01BRN) C01BRN,C01CTY,C01CNM," +
        //                          " DATEDISPLAY1 , DATEDISPLAY2 , DATEDISPLAY3  " +
        //                          " C01CST,C01LTY,c11uus, " +
        //                          " substr(c11udt,7,2)||'/'||substr(c11udt,5,2)||'/'||substr(c11udt,1,4), " +
        //                          " case when (ilcp01.c01cmp = (select max(b.c08cmp) from ilcp08 b " +
        //                          " WHERE ilcp01.c01cmp=b.c08cmp " +
        //                          " AND b.c08ven = (SELECT d.p10ven FROM ilms10 d " +
        //                          " WHERE b.c08ven=d.p10ven and d.p10del= '' and d.p10edt >= " + Curdate + ")))" +
        //                          " OR (ilcp01.c01cmp = (SELECT max(c.c08cmp) FROM ilcp08 c WHERE ilcp01.c01cmp=c.c08cmp AND c.c08ven=0)) " +
        //                          " then '' else 'Y' end as c01cst " +
        //                          " case when ((ilcp01.c01cmp = (SELECT max(b.c08cmp) FROM ilcp08 b " +
        //                          " WHERE ilcp01.c01cmp=b.c08cmp AND " +
        //                          " c.c08ven=0)))) " +
        //                          " THEN '' ELSE 'Y' END AS c01cst " +
        //                          " FROM ILCP01 " +
        //                          " JOIN ILCP09 ON C01CMP = C09CMP " +
        //                          " LEFT JOIN ilcp11 on(c01cmp=c11cmp AND c11nsq=1 AND c11lst=1) ";

        //        string whereSql = " WHERE ";


        //        if(startCamp.Trim() != "")
        //        {

        //        }

        //        if (endCamp.Trim() != "") 
        //        {

        //        }

        //        if (campCode.Trim() != "") 
        //        {

        //        }

        //        if (campName.Trim() != "") 
        //        {

        //        }

        //        if (campSts.Trim() != "") 
        //        {

        //        }




        //        ds = RetriveAsDataSet(sql_rate);




        //        CloseConnectioDAL();


        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return ds;
        //}







        #endregion

        public DataSet get_RLTB10()
        {
            DataSet ds = new DataSet();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;
            try
            {

                string sql = " SELECT T10CD2 FROM  RLTB10  WHERE T10RCD = '44' AND T10CD1 = '02'";

                ds = RetriveAsDataSet(sql);
                CloseConnectioDAL();
            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }
            return ds;

        }

        #region "Function for IL FAX "
        /* FAX IL (Approve, Reject, Cancel) - Project New FOB */
        public bool Call_ILG002CL(string prmBRN, string prmSDATE, string prmEDATE, string prmSTIM, string prmETIM, string prmUSER, string prmVEND, string prmTFAX, string prmAPP, string prmTYP, string strBizInit, string strBranchNo)
        {
            iDB2Command cmd = new iDB2Command();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ILG002CL";

            //in put
            cmd.Parameters.Add("PBRN", iDB2DbType.iDB2Char, 3).Value = prmBRN;
            cmd.Parameters.Add("PSDATE", iDB2DbType.iDB2Char, 8).Value = prmSDATE;
            cmd.Parameters.Add("PEDATE", iDB2DbType.iDB2Char, 8).Value = prmEDATE;
            cmd.Parameters.Add("PSTIM", iDB2DbType.iDB2Char, 6).Value = prmSTIM;
            cmd.Parameters.Add("PETIM", iDB2DbType.iDB2Char, 6).Value = prmETIM;
            cmd.Parameters.Add("PUUSER", iDB2DbType.iDB2Char, 10).Value = prmUSER.Trim();
            cmd.Parameters.Add("PVEND", iDB2DbType.iDB2Char, 12).Value = prmVEND;
            cmd.Parameters.Add("PTFAX", iDB2DbType.iDB2Char, 2).Value = prmTFAX;
            cmd.Parameters.Add("PAPP", iDB2DbType.iDB2Char, 60).Value = prmAPP.Trim();
            cmd.Parameters.Add("PTYP", iDB2DbType.iDB2Char, 2).Value = prmTYP;

            if (m_da400.ExecuteSubRoutineNoConnect("ILG002CL", ref cmd, strBizInit, strBranchNo))
            {
                return true;
            }
            else
            {
                m_da400.CloseConnect();
                return false;
            }
        }

        public DataSet Get_ILFB01()
        {
            DataSet ds = new DataSet();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;
            try
            {
                string sql = @"
                SELECT F01RST, F01VEN, F01NMT, F01TBL, F01AMP, F01PRO, F01FAX, F01NAM, F01GNM, F01CIS,
                CHAR(BIGINT(F01EBC)) F01EBC, CHAR(BIGINT(F01CNT)) F01CNT, F01APT, F01PRD, F01AMO, F01CBU, F01PUR, F01DOW, 
                F01COA, F01DUT, F01PAY, F01BRN, F01APP, F01PRA, F01INA, F01CRA, F01FPA, F01FIA, F01FCA, F01FDU, F01FBU,      
                F01FOA, P90NAME, P90SURN,F02FRM,F02TO,F02IRT,F02CR,F02PA
                FROM ILFB01
                LEFT JOIN ILMS90 ON(F01BRN=P90BRN AND F01APP=P90APNO) 
                LEFT JOIN ILFB02 ON(F01BRN=F02BRN AND F01APP=F02APP) 
                WHERE F01RST = '0' 
                ORDER BY F01VEN 
            ";
                //F01VEN = '010103520029'
                ds = RetriveAsDataSet(sql);
                ds.Tables[0].TableName = "approveTable";
                sql = @"
                SELECT F01RST, F01VEN, F01NMT, F01TBL, F01AMP, F01PRO, F01FAX, F01NAM, F01APP, 
                F01APT, F01PRD, F01AMO, F01PUR, F01DOW, P90NAME, P90SURN
                FROM ILFB01 
                LEFT JOIN ILMS90 ON(F01BRN=P90BRN AND F01APP=P90APNO) 
                WHERE F01RST = '1' 
                ORDER BY F01VEN
                ";
                DataSet dsReject = RetriveAsDataSet(sql);
                dsReject.Tables[0].TableName = "rejectTable";
                ds.Tables.Add(dsReject.Tables[0].Copy());

                sql = @"
                SELECT F01RST, F01VEN, F01NMT, F01TBL, F01AMP, F01PRO, F01FAX, F01NAM, F01APP,
                F01APT, F01PRD, F01AMO, F01PUR, F01DOW, P90NAME, P90SURN
                FROM ILFB01
                LEFT JOIN ILMS90 ON(F01BRN=P90BRN AND F01APP=P90APNO)
                WHERE F01RST = '2' 
                ORDER BY F01VEN
                ";
                DataSet dsCancel = RetriveAsDataSet(sql);
                dsCancel.Tables[0].TableName = "cancelTable";
                ds.Tables.Add(dsCancel.Tables[0].Copy());
                sql = @"
                SELECT SUM(CASE WHEN F01RST = '0' THEN 1 ELSE 0 END) AS APPROVE
                        ,SUM(CASE WHEN F01RST = '1' THEN 1 ELSE 0 END) AS REJECT
                        ,SUM(CASE WHEN F01RST = '2' THEN 1 ELSE 0 END) AS CANCEL
                        ,F01VEN,F01NMT, F01FAX
                FROM ILFB01 
                LEFT JOIN ILMS90 ON(F01BRN=P90BRN AND F01APP=P90APNO) 
                WHERE F01RST IN('0','1','2')
                GROUP BY F01VEN,F01NMT, F01FAX
                ORDER BY F01VEN
                ";
                DataSet dsSummary = RetriveAsDataSet(sql);
                dsSummary.Tables[0].TableName = "summaryTable";
                ds.Tables.Add(dsSummary.Tables[0].Copy());
                CloseConnectioDAL();
            }
            catch (Exception ex) { CloseConnectioDAL(); }
            return ds;
        }
        public DataSet getFAXProduct(string campaignCode, string vendorCode, ref string errmsg)
        {
            DataSet ds = new DataSet();
            string sql = @"
                    SELECT C01CMP AS CAMPAIGNCODE, C01CNM AS CAMPAIGNNAME, C01SDT CAMPAIGN_START, C01EDT as CAMPAIGN_END, C01CLD AS CAMPAIGN_ENDBILL
                    , T44ITM AS PROITEM ,T41DES AS PRODUCT,T42DES AS BRAND,T43DES as MODEL ,C02INR AS INT ,C02CRR AS CRU,C02FMT PERIODFROM,C02TOT as PERIODTO
                    ,CASE WHEN C07MAX <=0 THEN C07MIN ELSE C07MAX END AS PRICE, P10TNM AS VENDOR
                    FROM ILCP07
                    LEFT JOIN ILMS10 ON P10VEN = {1}
                    LEFT JOIN ILCP01 ON C01CMP = C07CMP
                    LEFT JOIN ILCP02 ON C01CMP = C02CMP
                    LEFT JOIN ILTB44 ON C07LNT = T44LTY AND CASE WHEN C07PIT = 0 THEN T44ITM ELSE C07PIT END = T44ITM
                    LEFT JOIN ILTB41 ON T41LTY = T44LTY AND T41TYP = T44TYP AND T41COD = T44COD
                    LEFT JOIN ILTB42 ON T42BRD = T44BRD
                    LEFT JOIN ILTB43 ON T43LTY = T44LTY AND T43TYP = T44TYP AND T43BRD = T42BRD AND T43COD = T44COD AND T43MDL = T44MDL
                    WHERE C01CMP = '{0}' AND T41DEL = '' AND T42DEL = '' AND T43DEL = '' AND T44DEL = '' 
            ";
            try
            {
                UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
                UserInfomation = m_userInfo;
                ds = RetriveAsDataSet(string.Format(sql, campaignCode, vendorCode));
                CloseConnectioDAL();
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                ds = null;
            }

            return ds;

        }
        public DataSet getFAXCampaign(string prmType, ref string errmsg)
        {
            DataSet ds = new DataSet();
            string strWhere = "";
            try
            {
                UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
                UserInfomation = m_userInfo;

                if ((prmType == "5") || (prmType == "6") || (prmType == "7"))
                {
                    strWhere = " and c01brn = 701 ";
                }

                string sql = "Select c01cmp, c01cnm " +
                             "from ilcp01 " +
                             "where c01sty = '" + prmType + "' and c01edt >= " + m_UdpD +
                             strWhere +
                             " order by c01cmp";

                ds = RetriveAsDataSet(sql);
                CloseConnectioDAL();
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                ds = null;
            }

            return ds;

        }

        public DataSet getFAXVendor(string prmType, string prmCampaign, ref string errmsg)
        {
            DataSet ds = new DataSet();
            string strWhere = "";
            string sql = "";
            try
            {
                UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
                UserInfomation = m_userInfo;

                if ((prmType == "5") || (prmType == "6") || (prmType == "7"))
                {
                    strWhere = " and P10BRN = 701 ";
                }

                sql = "Select c08ven, p10tnm " +
                      "from ilcp08 " +
                      "left join ilms10 on c08ven = P10VEN " +
                      "where c08cmp = " + prmCampaign + strWhere +
                      " and c08ven = p10ven " +
                      "order by c08ven";

                ds = RetriveAsDataSet(sql);
                CloseConnectioDAL();
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                ds = null;
            }

            return ds;
        }

        public DataSet getFAXProduct(string prmType, ref string errmsg)
        {
            DataSet ds = new DataSet();
            string strWhere = "";
            string sql = "";
            try
            {
                UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
                UserInfomation = m_userInfo;

                //if ((prmType == "5") || (prmType == "6") || (prmType == "7"))
                //{
                //    strWhere = " and c01brn = 701 ";
                //}

                //string sql = "Select c01cmp, c01cnm " +
                //             "from ilcp01 " +
                //             "where c01sty = '" + prmType + "' and c01edt >= " + m_UdpD +
                //             strWhere +
                //             " order by c01cmp";

                ds = RetriveAsDataSet(sql);
                CloseConnectioDAL();
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                ds = null;
            }

            return ds;

        }

        #endregion

        #region "New Function Convert rate 20190306 Manop"
        public double EIR_ConvertRate(double oldrate, double loanamt, int term)
        {

            double n_eir = 0;
            double installment = Math.Round(-1 * ((loanamt + (oldrate * loanamt * term)) / term), 0);
            n_eir = (Math.Floor((Financial.Rate(term, installment, loanamt) * 10000)) / 10000) * 100 * 12;
            return n_eir;
        }
        #endregion
        public DataSet get_ILMS98(int day)
        {
            DataSet ds = new DataSet();
            UserInfo m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;
            try
            {

                string sql = string.Format("SELECT * FROM ILMS98 WHERE {0} BETWEEN P98SDT AND P98EDT", day);

                ds = RetriveAsDataSet(sql);
                CloseConnectioDAL();
            }
            catch (Exception ex)
            {
                CloseConnectioDAL();
            }
            return ds;

        }
        //public static implicit operator Financial(ILDataCenter iLDataCenter)
        //{
        //    return iLDataCenter.fina;
        //}

        #region " 76361 IL (Effective Interest Rate) Modify By Manop"
        public string InsertIlMS23(string p23csn, string p23cnt, string p23int, string p23crt, string p23iny, string p23cry, string p23atm, string p23ain,
            string p23acr, string p23ldt, string p23upd, string p23upt, string p23usr, string p23dsp, string p23upg, string p23del
            , string p23ttm, string p23in1, string p23tf2, string p23tt2, string p23in2, string p23in3)
        {
            string sql = "";
            try
            {
                sql = string.Format("INSERT INTO ILMS23 " +
                               "(P23CSN, P23CNT, P23INT, P23CRT, P23INY, P23CRY, P23ATM, P23AIN, P23ACR, P23LDT " +
                               " , P23UPD, P23UPT, P23USR, P23DSP, P23UPG, P23DEL " +
                               " , P23TTM, P23IN1, P23TF2, P23TT2, P23IN2, P23IN3)" +
                               " VALUES({0}, {1},    {2},    {3},  {4},    {5},    {6},    {7},   {8},     {9},   {10},   {11}, '{12}',   '{13}',   '{14}',   '{15}' " +
                               " ,{16}, {17},    {18},    {19},  {20},    {21} ) ",
                                 p23csn, p23cnt, p23int, p23crt, p23iny, p23cry, p23atm, p23ain, p23acr, p23ldt, p23upd, p23upt, p23usr, p23dsp, p23upg, p23del
                                 , p23ttm, p23in1, p23tf2, p23tt2, p23in2, p23in3);
            }
            catch
            {

            }
            return sql;
        }
        #endregion

       
    }


}
