using EB_Service.Commons;
using EB_Service.DAL;
using ESB.WebAppl.ILSystem.commons;
using ILSystem.App_Code.BLL.DataCenter;
using ILSystem.App_Code.Commons;
using ILSystem.App_Code.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace ILSystem.App_Code.BLL.Integrate
{

    public class ILSRSMS : UserInfo
    {
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

        public ILSRSMS()
        {

        }

        public UserInfo UserInfomation
        {
            set
            {
                m_UserInfo = value;
                m_UserName = m_UserInfo.Username;
                m_User = m_UserInfo.Username;
                m_Wrkstn = m_UserInfo.LocalClient;
            }
        }
        //public EB_Service.DAL.DataCenter _dataCenter = new EB_Service.DAL.DataCenter();

        public bool Call_ILSRSMS(EB_Service.DAL.DataCenter dataCenter,  string PIMODE, string PIBUS, string PIBRN, string PIAPNO, string PITEL, ref string poerrc, ref string poerrm)
        {
            try
            {
                ILDataCenter ilobj = new ILDataCenter();
                DataSet DS = new DataSet();
                string SNDSMS = "";
                string SMSTYPE = "";
                string SRCDTA = "";
              //  string TXT1 = "jackaphop#s9hmk7#Umay+#";
                string[] ARRRES = { "SL23", "SL27", "SL28", "SL29", "SL30" };
                poerrc = "";
                poerrm = "";
                string WKTEXT = "";
                string WKTEXT2 = "";
                string WK8DTE = DateTime.Now.ToString("yyyyMMdd");
                string WK6DTE = DateTime.Now.ToString("yyMMdd");
                string WKBRN = PIBRN;
                string WKAPNO = PIAPNO;
                string WKTEL = PITEL;
                string WK21AP = PIBUS;
                string WK21FD = WK6DTE;
                string sqlILMS01 = $@"SELECT P1BRN,P1APNO,P1LTYP,P1APPT,P1APVS
                          ,P1SAPC,P1APDT,P1PBCD,P1PBRN,P1PATY
                          ,P1PANO,P1PAYT,P1VDID,P1MKID,P1CAMP
                          ,P1CMSQ,P1ITEM,P1PDGP,P1PRIC,P1QTY
                          ,P1PURC,P1VATR,P1VATA,P1DOWN,P1DISC
                          ,P1TERM,P1RANG,P1NDUE,P1LNDR,P1DUTR
                          ,P1INFR,P1INTR,P1CRUR,P1PRAM,P1INTA
                          ,P1CRUA,P1INFA,P1DUTY,P1DIFF,P1COAM
                          ,P1FDAM,P1FRTM,P1FRDT,P1FRAM,P1APRJ
                          ,P1STDT,P1STTM,P1AVDT,P1AVTM,P1CONT
                          ,P1CNDT,P1FDUE,P1CSNO,P1LOCA,P1CRCD
                          ,P1AUTH,P1RESN,P1DCCD,P1DOCR,P1CONC
                          ,P1COOT,P1KUSR,P1KDTE,P1KTIM,P1FAX
                          ,P1FILL,P1UPDT,P1UPTM,P1UPUS,P1PROG
                          ,P1WSID,P1RSTS
                      FROM AS400DB01.ILOD0001.ILMS01 WITH (NOLOCK)
                        WHERE P1BRN = {WKBRN} AND P1APNO = {WKAPNO} ";
                DS = dataCenter.GetDataset<DataTable>(sqlILMS01, CommandType.Text).Result.data;
                if (!ilobj.check_dataset(DS))
                {
                    poerrc = "Y";
                    poerrm = "Not Found App. No.";
                    return true;
                }
                DataRow dr_ILMS01 = DS.Tables[0].Rows.Count > 0 ? DS.Tables[0].Rows[0] : null;
                string sqlCustomer = $@"SELECT CustomerTypeID,BirthDate,CISNumber,TitleID
                                      ,NameInENG,SurnameInENG,NickName,NameInTHAI,SurnameInTHAI
                                      ,SexID,MaritalStatusID,CardTypeID,IDCard,IDCardIssued
                                      ,IDCardExpiredDate,EmailAddress,ResidentalStatusID
                                      ,ResidentalYear,ResidentalMonth,NoOfChildren,NoOfFamily
                                  FROM CustomerDB01.CustomerInfo.CustomerGeneral cg WITH (NOLOCK)
                                  JOIN GeneralDB01.GeneralInfo.GeneralCenter gc WITH (NOLOCK) ON (CustomerTypeID = gc.ID AND Type = 'CustomerTypeID')
                                  WHERE CISNumber = {dr_ILMS01["P1CSNO"].ToString().Trim()} AND gc.Code = '1' ";
                DS = dataCenter.GetDataset<DataTable>(sqlCustomer, CommandType.Text).Result.data;
                if (!ilobj.check_dataset(DS))
                {
                    poerrc = "Y";
                    poerrm = "The customer was not found or is not an individual customer.";
                    return true;
                }
                #region EXSR      @MOBILE  
                if (!IsValidPhoneNumber(WKTEL))
                {
                    poerrc = "Y";
                    poerrm = "Invalid Mobile No.";
                    return true;
                }
                string MOBTEL = WKTEL;
                #endregion
                #region EXSR      @GENSMS
                string WK21SD = "0";
                #region ** APPROVE (NORMAL) => SMS(A1)
                if (dr_ILMS01["P1APRJ"].ToString().Trim() == "AP" && dr_ILMS01["P1APPT"].ToString().Trim() == "01")
                {
                    WK21SD = "81";
                    if (dr_ILMS01["P1BRN"].ToString().Trim() == "701")
                    {
                        WK21SD = "89";

                    }
                }
                #endregion
                #region **APPROVE (USING CARD) => SMS(A2)
                else if (dr_ILMS01["P1APRJ"].ToString().Trim() == "AP" && dr_ILMS01["P1APPT"].ToString().Trim() == "02")
                {
                    WK21SD = "82";
                    string sql_ILMS10 = $@"SELECT P10VEN,P10TIC,P10TNM,P10NAM,P10ADR,P10VIL,P10BIL,P10BUD,P10ROM,P10FLO
                                      ,P10SOI,P10ROD,P10MOO,P10TMC,P10AMC,P10PVC,P10ZIP,P10AD2,P10VI2,P10BI2
                                      ,P10BU2,P10RM2,P10FL2,P10SO2,P10RD2,P10MO2,P10TM2,P10AM2,P10PV2,P10ZI2
                                      ,P10A31,P10A32,P10A33,P10STS,P10GRD,P10MOU,P10REG,P10TAX,P10TE1,P10TLR
                                      ,P10EXT,P10TE2,P10TR2,P10EX2,P10FX1,P10F1T,P10FX2,P10F2T,P10RES,P10POT
                                      ,P10RDP,P10RE2,P10PO2,P10RP2,P10FJD,P10JDT,P10EDT,P10PVN,P10BPY,P10TXR
                                      ,P10PYE,P10PTY,P10BCD,P10BNO,P10BRG,P10DLV,P10HED,P10DTX,P10SFG,P10CLD
                                      ,P10RF1,P10CRD,P10MKD,P10TAV,P10LTM,P10SAL,P10CTY,P10DTY,P10CPD,P10BRN
                                      ,P10DT1,P10DT2,P10FI1,P10FIL,P10UPD,P10TIM,P10USR,P10PGM,P10DSP,P10DEL
                                      ,P10ATS,P10FIX,P10SPC
                                  FROM AS400DB01.ILOD0001.ILMS10 WITH (NOLOCK)
                                  WHERE P10VEN = {dr_ILMS01["P1VDID"].ToString().Trim()}";
                    DS = dataCenter.GetDataset<DataTable>(sql_ILMS10, CommandType.Text).Result.data;
                    if (ilobj.check_dataset(DS))
                    {
                        DataRow dr_ILMS10 = DS.Tables[0].Rows.Count > 0 ? DS.Tables[0].Rows[0] : null;
                        if (dr_ILMS10["P10SPC"].ToString().Trim() == "Y")
                        {
                            WK21SD = "88";
                        }

                    }
                }
                #endregion
                #region ** CANCEL/REJECT  
                else if (dr_ILMS01["P1APRJ"].ToString().Trim() == "RJ" || dr_ILMS01["P1APRJ"].ToString().Trim() == "CN")
                {
                    bool IN88 = Array.Exists(ARRRES, element => element == dr_ILMS01["P1RESN"].ToString().Trim());
                    if (IN88)
                    {
                        WK21SD = "84";
                    }
                    else
                    {
                        switch (dr_ILMS01["P1APRJ"].ToString().Trim())
                        {
                            case "RJ":
                                if (dr_ILMS01["P1BRN"].ToString().Trim() == "701")
                                {
                                    WK21SD = "90";
                                }
                                else
                                {
                                    WK21SD = "83";
                                }
                                break;
                            case "CN":
                                if (dr_ILMS01["P1BRN"].ToString().Trim() == "701")
                                {
                                    WK21SD = "91";
                                }
                                else
                                {
                                    WK21SD = "85";
                                }
                                break;

                            default:
                                break;
                        }
                    }
                }
                #endregion
                #region EXSR      @WORDING 
                string sql_GNTS21 = $@"SELECT GS21RRNO,GS21RT,GS21AP,GS21SD,GS21FD
                                  ,GS21TD,GS21D1,GS21D2,GS21D3,GS21RM
                                  ,GS21US,GS21UT,GS21WS
                              FROM AS400DB01.GNOD0000.GNTS21 WITH (NOLOCK)
                              WHERE GS21AP = '{WK21AP}' AND GS21SD = {WK21SD} 
                              AND {WK21FD} between GS21FD and GS21TD
                              order by GS21AP,GS21SD,GS21FD desc";
                DS = dataCenter.GetDataset<DataTable>(sql_GNTS21, CommandType.Text).Result.data;
                if (ilobj.check_dataset(DS))
                {
                    DataRow dr_GNTS21 = DS.Tables[0].Rows.Count > 0 ? DS.Tables[0].Rows[0] : null;
                    if (WK21SD == "88")
                    {
                        #region EXSR      @ELOAN
                        string WKLOAN = "0";
                        string WKTERM = "0";
                        string WKINST = "0";
                        string WKFRIN = "0";
                        string KMD012BR = PIBRN;
                        string KMD012AP = dr_ILMS01["P1APNO"].ToString().Trim();
                        string sql_ILMD012 = $@"SELECT D012BR,D012AP,D012SQ,D012LT,D012CT
                                          ,D012TT,D012FM,D012TO,D012IR,D012CR
                                          ,D012FR,D012PA,D012IA,D012CA,D012FA
                                          ,D012IN,D012DF,D012FI,D012UD,D012UT
                                          ,D012US,D012PG,D012WS,D012RS
                                      FROM AS400DB01.ILOD0001.ILMD012 WITH (NOLOCK)
                                      WHERE D012BR = {KMD012BR} AND D012AP = {KMD012AP}";
                        DS = dataCenter.GetDataset<DataTable>(sql_ILMD012, CommandType.Text).Result.data;
                        if (ilobj.check_dataset(DS))
                        {
                            foreach (DataRow dr in DS.Tables[0].Rows)
                            {
                                if (Convert.ToInt32(dr["D012IN"].ToString()) > Convert.ToInt32(WKINST))
                                {
                                    WKINST = dr["D012IN"].ToString();
                                    break;
                                }
                            }
                            WKLOAN = (Convert.ToInt32(dr_ILMS01["P1PRIC"].ToString()) - Convert.ToInt32(dr_ILMS01["P1DOWN"].ToString())).ToString();
                            WKTERM = dr_ILMS01["P1TERM"].ToString();
                            WKFRIN = dr_ILMS01["P1FRAM"].ToString();
                            string SHFDUE = dr_ILMS01["P1FDUE"].ToString();
                            string wkttext =
                            WKTEXT = string.Concat(
                                    SubstringTrim(dr_GNTS21["GS21D1"].ToString(), 0, 13), " ",
                                    WKLOAN.Trim(), " ",
                                    SubstringTrim(dr_GNTS21["GS21D1"].ToString(), 22, 3), " ",
                                    SubstringTrim(dr_GNTS21["GS21D2"].ToString(), 0, 8), " ",
                                    WKTERM.Trim(), " ",
                                    SubstringTrim(dr_GNTS21["GS21D2"].ToString(), 12, 9), " ",
                                    WKINST.Trim(), " ",
                                    SubstringTrim(dr_GNTS21["GS21D2"].ToString(), 30, 10), " ",
                                    WKFRIN.Trim(), " ",
                                    SubstringTrim(dr_GNTS21["GS21D3"].ToString(), 0, 27), " ",
                                    SubstringTrim(SHFDUE, 4, 2), "/",
                                    SubstringTrim(SHFDUE, 2, 2), " ",
                                    SubstringTrim(dr_GNTS21["GS21D3"].ToString(), 33, 9));
                        }
                        #endregion
                    }
                    else if (WK21SD == "89")
                    {
                        #region  EXSR     @ELOAN_NEW  
                        string WKLOAN = dr_ILMS01["P1PRAM"].ToString();
                        WKTEXT = string.Concat(
                                    dr_GNTS21["GS21D1"].ToString().Trim(), " ",
                                    WKLOAN.Trim(), " ",
                                    dr_GNTS21["GS21D2"].ToString().Trim(),
                                    dr_GNTS21["GS21D3"].ToString().Trim());
                        #endregion
                    }
                    else if (WK21SD == "91")
                    {
                        string wkttext = dr_GNTS21["GS21D1"].ToString().Trim() + dr_GNTS21["GS21D2"].ToString().Trim() + dr_GNTS21["GS21D3"].ToString().Trim();
                    }
                    else
                    {
                        WKTEXT = dr_GNTS21["GS21D1"].ToString().Trim() + dr_GNTS21["GS21D2"].ToString().Trim();
                    }
                }
                else
                {
                    poerrc = "Y";
                    poerrm = "NOT FOUND SMS WORDING";
                    return true;
                }
                switch (dr_ILMS01["P1APRJ"].ToString().Trim())
                {
                    case "AP":
                        string sql_ILMS02 = $@"SELECT P2BRN,P2CONT,P2LNTY,P2CSNO,P2APNO,P2APPT,P2CRCD,P2ATCD,P2LOCA,P2PRCR
                                          ,P2VDID,P2MKID,P2CAMP,P2CMSQ,P2CMCT,P2ITEM,P2PRIC,P2QTY,P2PURC,P2VATR
                                          ,P2VATA,P2DOWN,P2DISC,P2TERM,P2RANG,P2NDUE,P2DTE1,P2CNDT,P2BKDT,P2BFAM
                                          ,P2BFBL,P2BFWV,P2HFAM,P2HFBL,P2HFWV,P2LNDR,P2DUTR,P2INFR,P2INTR,P2CRUR
                                          ,P2TOAM,P2OSAM,P2PCAM,P2PCBL,P21DUE,P2FDAM,P2DIFF,P2DIFB,P2FRTM,P2FRDT
                                          ,P2FRAM,P2CYCC,P2DUTY,P2DUTB,P2DTWV,P2FEE,P2FEEB,P2UFEB,P2FEIB,P2CRUA
                                          ,P2CRUB,P2UCRB,P2UCIB,P2UIDA,P2INTB,P2UBAS,P2UIDB,P2MBAS,P2MAMT,P2MBAL
                                          ,P2MINB,P2VBAS,P2VAMT,P2VBAL,P2VINB,P2CMBS,P2CMAM,P2CMBL,P2CEXB,P2WLET
                                          ,P2ODTM,P2ODAM,P2ODPC,P2SODT,P2ODT2,P2ODA2,P2ODP2,P2SOD2,P2ODDY,P2SODD
                                          ,P2MXOD,P2MOD2,P2PENL,P2PNRC,P2PNWV,P2SIAD,P2STIA,P2LGFG,P2PTFG,P2ACFG
                                          ,P2ACDT,P2STAD,P2MKAC,P2MADT,P2MSDT,P2VDAC,P2VADT,P2VSDT,P2EXAC,P2EADT
                                          ,P2ESDT,P2STBL,P2STDC,P2SUSR,P2RJLB,P2RJST,P2SDTE,P2RESN,P2INFG,P2LPPD
                                          ,P2LSTM,P2LSTY,P2LPDT,P2LPTM,P2LMVD,P2SOST,P2SOSD,P2SOSA,P2SPCP,P2SINT
                                          ,P2SCRU,P2SFEE,P2COL,P2TEAM,P2CIC,P2TMDT,P2TMNO,P2FILL,P2UPDT,P2UPTM
                                          ,P2PROG,P2USER,P2DDSP,P2DEL
                                      FROM AS400DB01.ILOD0001.ILMS02 WITH (NOLOCK)
                                      WHERE P2BRN = {WKBRN} AND P2APNO =  {WKAPNO} ";
                        DS = dataCenter.GetDataset<DataTable>(sql_ILMS02, CommandType.Text).Result.data;
                        if (ilobj.check_dataset(DS))
                        {
                            DataRow dr_ILMS02 = DS.Tables[0].Rows.Count > 0 ? DS.Tables[0].Rows[0] : null;
                            string KMD012BR = dr_ILMS01["P1BRN"].ToString();
                            string KMD012AP = dr_ILMS01["P1APNO"].ToString();
                            string sql_ILMD012 = $@"SELECT D012BR,D012AP,D012SQ,D012LT,D012CT
                                          ,D012TT,D012FM,D012TO,D012IR,D012CR
                                          ,D012FR,D012PA,D012IA,D012CA,D012FA
                                          ,D012IN,D012DF,D012FI,D012UD,D012UT
                                          ,D012US,D012PG,D012WS,D012RS
                                      FROM AS400DB01.ILOD0001.ILMD012 WITH (NOLOCK)
                                      WHERE D012BR = {KMD012BR} AND D012AP = {KMD012AP}";
                            DS = dataCenter.GetDataset<DataTable>(sql_ILMD012, CommandType.Text).Result.data;
                            if (ilobj.check_dataset(DS))
                            {
                                DataRow dr_ILMD012 = DS.Tables[0].Rows.Count > 0 ? DS.Tables[0].Rows[0] : null;
                                string WK12IN = dr_ILMD012["D012IN"].ToString();
                                WKTEXT = string.Concat(
                                        "ใบสมัครของท่านอนุมัติแล้ว ",
                                        "กรุณาติดต่อที่ร้านเพื่อ",
                                        "รับสินค้า อนุมัติวงเงิน ",
                                        dr_ILMS02["P2TOAM"].ToString().Trim(), // ลบช่องว่างที่ปลายของ P2TOAM
                                        " บ. ระยะเวลา ",
                                        dr_ILMS02["P2TERM"].ToString(), // แปลง P2TERM เป็นสตริง
                                        "  งวด งวดละ ",
                                        WK12IN.Trim(), // ลบช่องว่างที่ปลายของ WK12IN
                                        "  บ. งวดแรก ",
                                        dr_ILMS02["P2FDAM"].ToString().Trim(), // ลบช่องว่างที่ปลายของ P2FDAM
                                        "  บ.");
                                WKTEXT2 = "เริ่มผ่อนชำระวันที่ " + dr_ILMS02["P21DUE"].ToString().Trim().PadLeft(8, '0').Substring(6, 2) + "/"
                                    + dr_ILMS02["P21DUE"].ToString().Trim().PadLeft(8, '0').Substring(4, 2) + "/"
                                    + dr_ILMS02["P21DUE"].ToString().Trim().PadLeft(8, '0').Substring(0, 4) + " ขอบคุณที่วางใจใช้" + "บริการยูเมะพลัส";

                            }
                        }
                        break;
                    case "RJ":
                    case "CN":
                        WKTEXT = "ขออภัย ใบสมัครของท่านไม่ผ่าน" + "การอนุมัติ บริษัทฯยกเลิกการ" +
                                "ผ่อนสินค้าของท่านแล้ว ขอบคุณ" + "ที่ใช้บริการยูเมะพลัส";
                        break;
                    default:
                        break;
                }
                #endregion
                //  SRCDTA = string.Concat(TXT1.Trim(), MOBTEL.Trim(), "#", WKTEXT.Trim(), "#T");
                SRCDTA = string.Concat(MOBTEL.Trim(), "#", WKTEXT.Trim(), "");
                #endregion
                #region  EXSR      @SEND_SMS
                Connect_SmsAPI smsAPI = new Connect_SmsAPI();
                var resSMS = smsAPI.SendSMS(SRCDTA, MOBTEL);
                if (!string.IsNullOrEmpty(WKTEXT2))
                {
                    //  SRCDTA = string.Concat(TXT1.Trim(), MOBTEL.Trim(), "#", WKTEXT2.Trim(), "#T");
                    SRCDTA = string.Concat(MOBTEL.Trim(), "#", WKTEXT2.Trim(), "");
                    resSMS = smsAPI.SendSMS(SRCDTA, MOBTEL);
                }
                if (!resSMS.success)
                {
                    poerrc = "Y";
                    poerrm = "Send SMS Error ...!!" + resSMS.message;
                    SNDSMS = "N";
                    #region  EXSR      @SMSTYPE 
                    SMSTYPE = CheckSmsType(WK21SD);
                    #endregion
                    string sql_Update = $@"UPDATE AS400DB01.ILOD0001.ILMS01
                                            SET P1FILL = STUFF(STUFF(P1FILL + ' ', 22, 2, '{SMSTYPE}') + ' ', 24, 1, '{SNDSMS}')
                                            WHERE P1BRN = {WKBRN} AND P1APNO = {WKAPNO} ";
                    bool transaction = dataCenter.Sqltr == null ? true : false;
                    int afrows = dataCenter.Execute(sql_Update, CommandType.Text, transaction).Result.afrows;
                }
                else
                {
                    poerrc = "";
                    poerrm = "Send SMS Complete ...!!";
                    SNDSMS = "Y";
                    #region  EXSR      @SMSTYPE 
                    SMSTYPE = CheckSmsType(WK21SD);
                    #endregion
                    string sql_Update = $@"UPDATE AS400DB01.ILOD0001.ILMS01
                                            SET P1FILL = STUFF(STUFF(P1FILL + ' ', 22, 2, '{SMSTYPE}') + ' ', 24, 1, '{SNDSMS}')
                                            WHERE P1BRN = {WKBRN} AND P1APNO = {WKAPNO} ";
                    bool transaction = dataCenter.Sqltr == null ? true : false;
                    int afrows = dataCenter.Execute(sql_Update, CommandType.Text, transaction).Result.afrows;
                }
                #endregion
                return true;
            }
            catch(Exception e)
            {
                Utility.WriteLog(e);
                poerrm = e.Message.ToString();
                return false;
            }
        }
        public string CheckSmsType(string WK21SD)
        {
            switch (WK21SD)
            {
                case ("81"):
                    return "A1";

                case ("82"):
                    return "A2";

                case ("88"):
                    return "A3";

                case ("89"):
                    return "A4";

                case ("83"):
                    return "R1";

                case ("90"):
                    return "R2";

                case ("84"):
                    return "C1";

                case ("85"):
                    return "C2";

                case ("91"):
                    return "C3";
                default:
                    return "";
            }
            
        }
        static string SubstringTrim(string input, int startIndex, int length)
        {
            // ตัดข้อความและลบช่องว่างที่ปลายทั้งสองข้าง
            if (startIndex < 0 || length <= 0 || startIndex >= input.Length)
                return "";
            return input.Substring(startIndex, Math.Min(length, input.Length - startIndex)).Trim();
        }
        private bool IsValidPhoneNumber(string phoneNumber)
        {
            // รูปแบบเบอร์โทรศัพท์มือถือที่ถูกต้อง
            // ต้องเริ่มต้นด้วย +66 หรือ 0 และตามด้วยเลข 6, 8, หรือ 9 ตามด้วยอีก 8 ตัวเลข
            string pattern = @"^(\+66|0)[689][0-9]{8}$";

            // ใช้ Regular Expression ในการตรวจสอบเบอร์โทรศัพท์
            Regex regex = new Regex(pattern);

            // ตรวจสอบว่าตรงกับรูปแบบที่กำหนดหรือไม่
            return regex.IsMatch(phoneNumber);
        }
    }
}