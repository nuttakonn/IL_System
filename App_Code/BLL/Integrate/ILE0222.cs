using EB_Service.Commons;
using ILSystem.App_Code.BLL.DataCenter;
using ILSystem.App_Code.Commons;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace ILSystem.App_Code.BLL.Integrate
{
    //public class ILE0222 : UserInfo
    //{
    //    private DateTimeFormatInfo m_DThai = new CultureInfo("th-TH", false).DateTimeFormat;

    //    private ulong m_UdpD = Convert.ToUInt32(DateTime.Now.ToString("yyyyMMdd"));
    //    private ulong m_UdpT = Convert.ToUInt32(DateTime.Now.ToString("HHmmss"));
    //    private string m_UserName;
    //    private string m_User;
    //    private string m_Wrkstn = "";
    //    private string m_Autodial_Usage = "";
    //    private UserInfo m_UserInfo;
    //    private DataSet dataSetResult;
    //    private string CHKDTE;
    //    private string WPERR;

    //    public ILE0222(UserInfo userInfo)
    //    {
    //        m_UserInfo = userInfo;
    //    }

    //    public UserInfo UserInfomation
    //    {
    //        set
    //        {
    //            m_UserInfo = value;
    //            m_UserName = m_UserInfo.Username;
    //            m_User = m_UserInfo.Username;
    //            m_Wrkstn = m_UserInfo.LocalClient;
    //        }
    //    }

    //    public int CURDTE { get; private set; }
    //    public int WPIDT { get; private set; }
    //    public int TMDPTO { get; private set; }
    //    public int TMDPOF { get; private set; }
    //    public int MAXARR { get; private set; }
    //    public int X1 { get; private set; }
    //    public string KAR2 { get; private set; }
    //    public int RC { get; private set; }
    //    public int A1 { get; private set; }
    //    public int WKSEQ { get; private set; }
    //    public string WKSNME { get; private set; }
    //    public string WKOFNM { get; private set; }
    //    public int WKCSN { get; private set; }
    //    public int KCSN { get; private set; }
    //    public string KREF { get; private set; }
    //    public int KRSQ { get; private set; }
    //    public string KAR1 { get; private set; }
    //    public string TMTYP { get; private set; }
    //    public string WKTEL { get; private set; }
    //    public int TMCSN { get; private set; }
    //    public string TMIDN { get; private set; }
    //    public int A2 { get; private set; }
    //    public int KBRN { get; private set; }
    //    public long KCNT { get; private set; }
    //    public int KAPN { get; private set; }
    //    public string strIndex { get; private set; }
    //    public class Index
    //    {
    //        public string TM1GRP { get; set; } // ขนาด 1
    //        public string TM1SOR { get; set; } // ขนาด 1
    //        public string TM1TNO { get; set; } // ขนาด 20
    //        public string TM1BIZ { get; set; } // ขนาด 2
    //    }

    //    public class Data
    //    {
    //        public int TM1REJ { get; set; } // ขนาด 5
    //        public int TM1CAN { get; set; } // ขนาด 5
    //        public int TM1APV { get; set; } // ขนาด 5
    //        public string[] A1TEL { get; set; } // ขนาด 11, DIM(99999)

    //        public Data()
    //        {
    //            A1TEL = new string[99999]; // กำหนดขนาดของ A1TEL
    //        }
    //    }
    //    public string[] A2OFN = new string[99999];
    //    public string[] A2CSN { get; set; }
    //    public int[] A2BRN { get; set; }
    //    public int[] A2APN { get; set; }
    //    public string[] A2FLG { get; set; }
    //    public string FGWRT { get; private set; }
    //    public string TMROC { get; private set; }
    //    public string TMBIZ { get; private set; }
    //    public int TM2AVD { get; private set; }
    //    public string FHPMS25 { get; private set; }
    //    public string FHPMS30 { get; private set; }
    //    public string GNMODE { get; private set; }
    //    public string ACDTLB { get; private set; }
    //    public string ACBRN { get; private set; }
    //    public string T001 { get; private set; }
    //    public string TTGRP { get; private set; }
    //    public string TTTNO { get; private set; }

    //    public class RESULTDS
    //    {
    //        public string Group { get; set; }
    //        public string Desc { get; set; }
    //        public string Bus { get; set; }
    //        public int Approve { get; set; }
    //        public int Reject { get; set; }
    //        public int Cancel { get; set; }
    //        public int Total { get; set; }
    //    }
    //    public List<RESULTDS> lst_RESULTDS = Enumerable.Repeat(new RESULTDS(), 9999).ToList();

    //    public DataTable dataTable;
    //    public Index index = new Index();
    //    public Data data = new Data();
    //    public DataSet Call_ILE0222(EB_Service.DAL.DataCenter dataCenter, string PIIDNO)
    //    {
    //        try
    //        {
    //            ILDataCenter ilobj = new ILDataCenter();
    //            ILDataSubroutine dataSubroutine = new ILDataSubroutine(m_UserInfo);
    //            dataSetResult = new DataSet();
    //            dataTable = new DataTable("Tables1");
    //            dataTable.Columns.Add("Group", typeof(string));
    //            dataTable.Columns.Add("Desc", typeof(string));
    //            dataTable.Columns.Add("Bus", typeof(string));
    //            dataTable.Columns.Add("Approve", typeof(int));
    //            dataTable.Columns.Add("Reject", typeof(int));
    //            dataTable.Columns.Add("Cancel", typeof(int));
    //            dataTable.Columns.Add("Total", typeof(int));
    //            dataSetResult.Tables.Add(dataTable);
    //            #region หา Date ILMS97
    //            string sql = "SELECT P97CDT FROM AS400DB01.ILOD0001.ILMS97 WITH (NOLOCK) WHERE P97REC = '01'";
    //            DataSet DS_ILMS97 = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
    //            if (ilobj.check_dataset(DS_ILMS97))
    //            {
    //                DataRow dr_ILMS97 = DS_ILMS97.Tables[0].Rows.Count > 0 ? DS_ILMS97.Tables[0].Rows[0] : null;
    //                CURDTE = Convert.ToInt32(!string.IsNullOrEmpty(dr_ILMS97["P97CDT"].ToString()) ? dr_ILMS97["P97CDT"].ToString() : "0");
    //            }
    //            WPIDT = CURDTE;
    //            bool Call_GNP023 = dataSubroutine.Call_GNP023(Convert.ToString(WPIDT), "YMD", "B", "30", "D", "-", ref CHKDTE, ref WPERR, "", "");
    //            TMDPTO = 0;
    //            TMDPOF = 0;
    //            MAXARR = 99999;
    //            RC = 0;
    //            #endregion
    //            #region                    EXSR      @GNTMP 
    //            GNTMP(dataCenter, PIIDNO);
    //            #endregion
    //            #region                    EXSR      @MOVDT 
    //            MOVDT(dataCenter);
    //            #endregion
    //            if(dataCenter.Sqltr != null)
    //            {
    //                dataCenter.CommitMssql();
    //                dataCenter.CloseConnectSQL();
    //            }
    //            return dataSetResult;
    //        }
    //        catch (Exception ex)
    //        {
    //            if (dataCenter.Sqltr != null)
    //            {
    //                dataCenter.RollbackMssql();
    //                dataCenter.CloseConnectSQL();
    //            }
    //            Utility.WriteLog(ex);
    //            return dataSetResult;
    //        }
    //    }
    //    public void ClearA1TEL(string[] a1tel)
    //    {
    //        for (int i = 0; i < a1tel.Length; i++)
    //        {
    //            a1tel[i] = null; // หรือใช้ "" ถ้าต้องการให้เป็นค่าว่าง
    //        }
    //    }
    //    public void ClearA2OFN(string[] a2ofn)
    //    {
    //        for (int i = 0; i < a2ofn.Length; i++)
    //        {
    //            a2ofn[i] = null; // หรือใช้ "" ถ้าต้องการให้เป็นค่าว่าง
    //        }
    //    }
    //    public void GNTMP(EB_Service.DAL.DataCenter dataCenter, string PIIDNO)
    //    {
    //        ILDataCenter ilobj = new ILDataCenter();
    //        ClearA1TEL(data.A1TEL);
    //        A1 = 0;
    //        WKSEQ = 0; 
    //        string sql = $@"SELECT IDCard M00IDN, CISNumber M00CSN, SurnameInTHAI M00TSN, OfficeName M00OFC   
    //                            FROM CustomerDB01.CustomerInfo.CustomerGeneral cg WITH (NOLOCK)
    //                            JOIN CustomerDB01.CustomerInfo.CustomerWorked cw WITH (NOLOCK) ON (cg.ID = cw.CustID)
    //                            WHERE IDCard = '{PIIDNO}' ";
    //        DataSet DS_CSMS00 = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
    //        if (ilobj.check_dataset(DS_CSMS00))
    //        {
    //            DataRow dr_Customer = DS_CSMS00.Tables[0].Rows.Count > 0 ? DS_CSMS00.Tables[0].Rows[0] : null;
    //            WKSNME = dr_Customer["M00TSN"].ToString();
    //            WKOFNM = dr_Customer["M00OFC"].ToString();
    //            WKCSN = Convert.ToInt32(!String.IsNullOrEmpty(dr_Customer["M00CSN"].ToString()) ? dr_Customer["M00CSN"].ToString() : "0");
    //        }
    //        else
    //        {
    //            return;
    //        }
    //        KCSN = WKCSN;
    //        KREF = string.Empty;
    //        KRSQ = 0;
    //        sql = $@"SELECT gc.Code M11CDE, TelephoneNumber1 M11TEL, Mobile M11MOB 
    //                        FROM CustomerDB01.CustomerInfo.CustomerAddress ca WITH (NOLOCK)
    //                        JOIN CustomerDB01.CustomerInfo.CustomerGeneral cg WITH (NOLOCK) ON (ca.CustID = cg.ID AND ca.CustRefID = 0)
    //                        JOIN GeneralDB01.GeneralInfo.GeneralCenter gc WITH (NOLOCK) ON (ca.AddressCodeID = gc.ID AND Type = 'AddressCodeID')
    //                        WHERE CISNumber = '{WKCSN}' ";
    //        DataSet DS_CSMS11 = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
    //        if (ilobj.check_dataset(DS_CSMS11))
    //        {
    //            foreach(DataRow dr_csms11 in DS_CSMS11.Tables[0].Rows)
    //            {
    //                KAR1 = "";
    //                string M11CDE = dr_csms11["M11CDE"].ToString();
    //                string M11TEL = dr_csms11["M11TEL"].ToString();
    //                string M11MOB = dr_csms11["M11MOB"].ToString();
    //                switch (M11CDE)
    //                {
    //                    case "H":
    //                        if (!string.IsNullOrEmpty(M11TEL) && M11TEL != "**" && M11TEL != "000000000")
    //                        {
    //                            KAR1 = 'H' + M11TEL.Substring(0, Math.Min(9, M11TEL.Length));
    //                            #region                                   EXSR      @GNAR1    
    //                            GNAR1();
    //                            #endregion
    //                        }
    //                        if (!string.IsNullOrEmpty(M11MOB) && M11MOB != "**" && M11MOB != "000000000")
    //                        {
    //                            KAR1 = 'M' + M11MOB.Substring(0, Math.Min(10, M11MOB.Length));
    //                            #region                                   EXSR      @GNAR1       
    //                            GNAR1();
    //                            #endregion
    //                        }
    //                        break;
    //                    case "O":
    //                        if (!string.IsNullOrEmpty(M11TEL) && M11TEL != "**" && M11TEL != "000000000")
    //                        {
    //                            KAR1 = 'O' + M11TEL.Substring(0, Math.Min(9, M11TEL.Length));
    //                        }
    //                        break;
    //                }
    //            }
    //        }
    //        #region     ** GROUP 1   
    //        GROUP1(dataCenter);
    //        #endregion
    //        #region     ** GROUP 2   
    //        GROUP2(dataCenter);
    //        #endregion
    //        #region     ** GROUP 3   
    //        GROUP3(dataCenter);
    //        #endregion
    //        #region     ** GROUP 4   
    //        GROUP4(dataCenter);
    //        #endregion
    //        #region     ** GROUP 5   
    //        GROUP5(dataCenter);
    //        #endregion
    //    }
    //    public void GNAR1()
    //    {
    //        if (!string.IsNullOrEmpty(KAR1) && A1 < MAXARR)
    //        {
    //            A1++;
    //            data.A1TEL[A1] = KAR1;
    //        }
    //        //if (!string.IsNullOrEmpty(KAR1))
    //        //{
    //        //    X1 = 1;
    //        //    data.A1TEL[X1] = KAR1;
    //        //    if (!string.IsNullOrEmpty(data.A1TEL[X1]) && A1 < MAXARR)
    //        //    {
    //        //        A1++;
    //        //        data.A1TEL[A1] = KAR1;
    //        //    }
    //        //}
    //    }
    //    public void GROUP1(EB_Service.DAL.DataCenter dataCenter)
    //    {
    //        ILDataCenter ilobj = new ILDataCenter();
    //        A1 = 1;
    //        while (A1 < data.A1TEL.Length && !string.IsNullOrEmpty(data.A1TEL[A1]))
    //        {
    //            TMTYP = string.Empty;
    //            TMTYP = data.A1TEL[A1].Substring(0, 1);
    //            if (TMTYP == "H")
    //            {
    //                WKTEL = string.Empty;
    //                WKTEL = data.A1TEL[A1].Substring(1, Math.Min(10, data.A1TEL[A1].Length - 1));
    //                string sql = $@"SELECT gc.Code M12ACD, gc2.Code M12TTY, CustID M12CSN
    //                                        FROM CustomerDB01.CustomerInfo.CustomerTelephone ct WITH (NOLOCK)
    //                                        JOIN GeneralDB01.GeneralInfo.GeneralCenter gc WITH (NOLOCK) ON (ct.AddressCodeID = gc.ID AND gc.Type = 'AddressCodeID')
    //                                        JOIN GeneralDB01.GeneralInfo.GeneralCenter gc2 WITH (NOLOCK) ON (ct.TelephoneTypeID = gc2.ID AND gc2.Type = 'TelephoneTypeID')                            
    //                                        WHERE TelephoneNumber = '{WKTEL}' ";
    //                DataSet DS_CSMS12 = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
    //                if (ilobj.check_dataset(DS_CSMS12))
    //                {
    //                    foreach (DataRow dr_csms12 in DS_CSMS12.Tables[0].Rows)
    //                    {
    //                        if (dr_csms12["M12ACD"].ToString() == "H" && dr_csms12["M12TTY"].ToString() == "P")
    //                        {
    //                            sql = $@"SELECT CISNumber M00CSN, IDCard M00IDN 
    //                                            FROM CustomerDB01.CustomerInfo.CustomerGeneral cg WITH (NOLOCK) 
    //                                            WHERE cg.ID = {dr_csms12["M12CSN"].ToString()}";
    //                            DataSet DS_General = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
    //                            if (ilobj.check_dataset(DS_General))
    //                            {
    //                                DataRow dr_00 = DS_General.Tables[0].Rows.Count > 0 ? DS_General.Tables[0].Rows[0] : null;

    //                                TMCSN = Convert.ToInt32(!string.IsNullOrEmpty(dr_00["M00CSN"].ToString()) ? dr_00["M00CSN"].ToString() : "0");
    //                                TMIDN = dr_00["M00IDN"].ToString();
    //                            }
    //                            else
    //                            {
    //                                if (A1 == MAXARR)
    //                                {
    //                                    return;
    //                                }
    //                                A1++;
    //                            }
    //                            #region EXSR      @GNALL 
    //                            GNALL(dataCenter);
    //                            #endregion
    //                        }
    //                    }
    //                }
    //            }
    //            A1++;
    //        }
    //    }
    //    public void GROUP2(EB_Service.DAL.DataCenter dataCenter)
    //    {
    //        ILDataCenter ilobj = new ILDataCenter();
    //        A1 = 1;
    //        while (A1 < data.A1TEL.Length && !string.IsNullOrEmpty(data.A1TEL[A1]))
    //        {
    //            TMTYP = string.Empty;
    //            TMTYP = data.A1TEL[A1].Substring(0, 1);
    //            if (TMTYP == "O")
    //            {
    //                WKTEL = string.Empty;
    //                WKTEL = data.A1TEL[A1].Substring(1, Math.Min(10, data.A1TEL[A1].Length - 1));
    //                string sql = $@"SELECT gc.Code M12ACD, gc2.Code M12TTY, CustID M12CSN
    //                            FROM CustomerDB01.CustomerInfo.CustomerTelephone ct WITH (NOLOCK)
    //                            JOIN GeneralDB01.GeneralInfo.GeneralCenter gc WITH (NOLOCK) ON (ct.AddressCodeID = gc.ID AND gc.Type = 'AddressCodeID')
    //                            JOIN GeneralDB01.GeneralInfo.GeneralCenter gc2 WITH (NOLOCK) ON (ct.TelephoneTypeID = gc2.ID AND gc2.Type = 'TelephoneTypeID')                            
    //                            WHERE TelephoneNumber = '{WKTEL}' ";
    //                DataSet DS_CSMS12 = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
    //                if (ilobj.check_dataset(DS_CSMS12))
    //                {
    //                    foreach (DataRow dr_csms12 in DS_CSMS12.Tables[0].Rows)
    //                    {
    //                        if (dr_csms12["M12ACD"].ToString() == "O" && dr_csms12["M12TTY"].ToString() == "P")
    //                        {
    //                            sql = $@"SELECT CISNumber M00CSN, IDCard M00IDN 
    //                                FROM CustomerDB01.CustomerInfo.CustomerGeneral cg WITH (NOLOCK) 
    //                                WHERE cg.ID = {dr_csms12["M12CSN"].ToString()}";
    //                            DataSet DS_General = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
    //                            if (ilobj.check_dataset(DS_General))
    //                            {
    //                                DataRow dr_00 = DS_General.Tables[0].Rows.Count > 0 ? DS_General.Tables[0].Rows[0] : null;
    //                                TMCSN = Convert.ToInt32(!string.IsNullOrEmpty(dr_00["M00CSN"].ToString()) ? dr_00["M00CSN"].ToString() : "0");
    //                                TMIDN = dr_00["M00IDN"].ToString();
    //                            }
    //                            else
    //                            {
    //                                if (A1 == MAXARR)
    //                                {
    //                                    return;
    //                                }
    //                                A1++;
    //                            }
    //                        }
    //                        #region EXSR      @GNALL 
    //                        GNALL(dataCenter);
    //                        #endregion
    //                    }
    //                }
    //            }
    //            A1++;
    //        }
    //    }
    //    public void GROUP3(EB_Service.DAL.DataCenter dataCenter)
    //    {
    //        ILDataCenter ilobj = new ILDataCenter();
    //        A1 = 1;
    //        while (A1 < data.A1TEL.Length && !string.IsNullOrEmpty(data.A1TEL[A1]))
    //        {
    //            TMTYP = string.Empty;
    //            TMTYP = data.A1TEL[A1].Substring(0, 1);
    //            if (TMTYP == "M")
    //            {
    //                WKTEL = string.Empty;
    //                WKTEL = data.A1TEL[A1].Substring(1, Math.Min(10, data.A1TEL[A1].Length - 1));
    //                string sql = $@"SELECT gc.Code M12ACD, gc2.Code M12TTY, CustID M12CSN
    //                            FROM CustomerDB01.CustomerInfo.CustomerTelephone ct WITH (NOLOCK)
    //                            JOIN GeneralDB01.GeneralInfo.GeneralCenter gc WITH (NOLOCK) ON (ct.AddressCodeID = gc.ID AND gc.Type = 'AddressCodeID')
    //                            JOIN GeneralDB01.GeneralInfo.GeneralCenter gc2 WITH (NOLOCK) ON (ct.TelephoneTypeID = gc2.ID AND gc2.Type = 'TelephoneTypeID')                            
    //                            WHERE TelephoneNumber = '{WKTEL}' ";
    //                DataSet DS_CSMS12 = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
    //                if (ilobj.check_dataset(DS_CSMS12))
    //                {
    //                    foreach (DataRow dr_csms12 in DS_CSMS12.Tables[0].Rows)
    //                    {
    //                        if (dr_csms12["M12ACD"].ToString() == "H" && dr_csms12["M12TTY"].ToString() == "M")
    //                        {
    //                            sql = $@"SELECT CISNumber M00CSN, IDCard M00IDN 
    //                                FROM CustomerDB01.CustomerInfo.CustomerGeneral cg WITH (NOLOCK) 
    //                                WHERE cg.ID = {dr_csms12["M12CSN"].ToString()}";
    //                            DataSet DS_CSMS00 = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
    //                            if (ilobj.check_dataset(DS_CSMS00))
    //                            {
    //                                DataRow dr_00 = DS_CSMS00.Tables[0].Rows.Count > 0 ? DS_CSMS00.Tables[0].Rows[0] : null;
    //                                TMCSN = Convert.ToInt32(!string.IsNullOrEmpty(dr_00["M00CSN"].ToString()) ? dr_00["M00CSN"].ToString() : "0");
    //                                TMIDN = dr_00["M00IDN"].ToString();
    //                            }
    //                            else
    //                            {
    //                                if (A1 == MAXARR)
    //                                {
    //                                    return;
    //                                }
    //                                A1++;
    //                            }
    //                        }
    //                        #region EXSR      @GNALL 
    //                        GNALL(dataCenter);
    //                        #endregion
    //                    }
    //                }
    //            }
    //            A1++;
    //        }
    //    }
    //    public void GROUP4(EB_Service.DAL.DataCenter dataCenter)
    //    {
    //        ILDataCenter ilobj = new ILDataCenter();
    //        if (!string.IsNullOrEmpty(WKSNME))
    //        {
    //            string sql = $@"SELECT IDCard M00IDN, CISNumber M00CSN
    //                    FROM CustomerDB01.CustomerInfo.CustomerGeneral WITH (NOLOCK)
    //                    WHERE SurnameInTHAI = '{WKSNME}' ";
    //            DataSet DS_CSMS00 = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
    //            if (ilobj.check_dataset(DS_CSMS00))
    //            {
    //                foreach (DataRow dr_00 in DS_CSMS00.Tables[0].Rows)
    //                {
    //                    TMCSN = Convert.ToInt32(!string.IsNullOrEmpty(dr_00["M00CSN"].ToString()) ? dr_00["M00CSN"].ToString() : "0");
    //                    TMIDN = dr_00["M00IDN"].ToString();
    //                    TMTYP = "S";
    //                    #region EXSR      @GNALL 
    //                    GNALL(dataCenter);
    //                    #endregion
    //                }
    //            }
    //        }
    //    }
    //    public void GROUP5(EB_Service.DAL.DataCenter dataCenter)
    //    {
    //        ILDataCenter ilobj = new ILDataCenter();
    //        if (!string.IsNullOrEmpty(WKOFNM))
    //        {
    //            ClearA2OFN(A2OFN);
    //            A2 = 0;
    //            string sql = $@"SELECT D3BRN RLD3BRN, D3APNO RLD3APNO 
    //                        FROM AS400DB01.RLOD0001.RLMD03 WITH (NOLOCK)
    //                        WHERE D3OFC = '{WKOFNM}' AND D3DEL = '' ";
    //            DataSet DS_RLMD03 = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
    //            if (ilobj.check_dataset(DS_RLMD03))
    //            {
    //                foreach (DataRow dr in DS_RLMD03.Tables[0].Rows)
    //                {
    //                    KBRN = Convert.ToInt32(!string.IsNullOrEmpty(dr["RLD3BRN"].ToString()) ? dr["RLD3BRN"].ToString() : "0");
    //                    KAPN = Convert.ToInt32(!string.IsNullOrEmpty(dr["RLD3APNO"].ToString()) ? dr["RLD3APNO"].ToString() : "0");
    //                    sql = $@"SELECT P1APRJ R1P1APRJ, P1LOCA R1P1LOCA, P1RSTS R1P1RSTS, P1CSNO R1P1CSNO
    //                                FROM AS400DB01.RLOD0001.RLMS01 WITH (NOLOCK)
    //                                WHERE P1BRN = {KBRN.ToString()} AND P1APNO = {KAPN.ToString()} ";
    //                    DataSet DS_RLMS01 = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
    //                    if (ilobj.check_dataset(DS_RLMS01))
    //                    {
    //                        DataRow dr_RLMS01 = DS_RLMS01.Tables[0].Rows.Count > 0 ? DS_RLMS01.Tables[0].Rows[0] : null;
    //                        if (dr_RLMS01["R1P1APRJ"].ToString() == "R" || dr_RLMS01["R1P1APRJ"].ToString() == "C" ||
    //                            (dr_RLMS01["R1P1APRJ"].ToString() == "A" && Convert.ToInt32(dr_RLMS01["R1P1LOCA"].ToString()) >= 250 &&
    //                            dr_RLMS01["R1P1RSTS"].ToString() != "X"))
    //                        {
    //                            X1 = 1;
    //                            KAR2 = dr_RLMS01["R1P1CSNO"].ToString();
    //                            if (A2CSN != null)
    //                            {
    //                                bool IN50 = Array.Exists(A2CSN, element => element == KAR2);
    //                                if (!IN50)
    //                                {
    //                                    A2 = 1;
    //                                    A2CSN[A2] = KAR2;
    //                                    A2BRN[A2] = Convert.ToInt32(!string.IsNullOrEmpty(dr["RLD3BRN"].ToString()) ? dr["RLD3BRN"].ToString() : "0");
    //                                    A2APN[A2] = Convert.ToInt32(!string.IsNullOrEmpty(dr["RLD3APNO"].ToString()) ? dr["RLD3APNO"].ToString() : "0");
    //                                    A2FLG[A2] = "N";
    //                                }
    //                            }

    //                        }
    //                    }
    //                    if (A2 == MAXARR)
    //                    {
    //                        break;
    //                    }
    //                }
    //            }
    //            sql = $@"SELECT cg.CISNumber M00CSN, cg.IDCard M00IDN
    //                        FROM CustomerDB01.CustomerInfo.CustomerGeneral cg WITH (NOLOCK)
    //                        JOIN CustomerDB01.CustomerInfo.CustomerWorked cw WITH (NOLOCK) ON (cg.ID = cw.CustID)
    //                        WHERE cw.OfficeName = '{WKOFNM}' ";
    //            DataSet DS_CSMS00 = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
    //            if (ilobj.check_dataset(DS_CSMS00))
    //            {
    //                foreach (DataRow dr in DS_CSMS00.Tables[0].Rows)
    //                {
    //                    TMCSN = Convert.ToInt32(!string.IsNullOrEmpty(dr["M00CSN"].ToString()) ? dr["M00CSN"].ToString() : "0");
    //                    TMIDN = dr["M00IDN"].ToString();
    //                    TMTYP = "F";
    //                    X1 = 1;
    //                    KAR2 = TMCSN.ToString();
    //                    if (A2CSN != null)
    //                    {
    //                        bool IN50 = Array.Exists(A2CSN, element => element == KAR2);
    //                        if (IN50)
    //                        {
    //                            A2FLG[X1] = "Y";
    //                        }
    //                    }

    //                    #region EXSR      @GNALL 
    //                    GNALL(dataCenter);
    //                    #endregion
    //                }
    //                X1 = 1;
    //                while (A1 < A2OFN.Length && !string.IsNullOrEmpty(A2OFN[X1]))
    //                {
    //                    if (A2FLG[X1].ToString() == "N")
    //                    {
    //                        TMCSN = Convert.ToInt32(!string.IsNullOrEmpty(A2CSN[X1].ToString()) ? A2CSN[X1].ToString() : "0");
    //                        sql = $@"SELECT cg.CISNumber M00CSN, cg.IDCard M00IDN
    //                                FROM CustomerDB01.CustomerInfo.CustomerGeneral cg WITH (NOLOCK)
    //                                WHERE cg.CISNumber = '{TMCSN.ToString()}' ";
    //                        DataSet DS_Customer = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
    //                        if (ilobj.check_dataset(DS_Customer))
    //                        {
    //                            DataRow dr_Customer = DS_Customer.Tables[0].Rows.Count > 0 ? DS_Customer.Tables[0].Rows[0] : null;
    //                            TMIDN = dr_Customer["M00IDN"].ToString();
    //                            TMTYP = "F";
    //                            #region EXSR      @GNALL 
    //                            GNALL(dataCenter);
    //                            #endregion
    //                        }
    //                    }
    //                    if (X1 == MAXARR)
    //                    {
    //                        break;
    //                    }
    //                }
    //            }
    //        }
    //    }
    //    public void GNALL(EB_Service.DAL.DataCenter dataCenter)
    //    {
    //        #region  EXSR      @GN_RL
    //        GN_RL(dataCenter);
    //        #endregion
    //        #region  EXSR      @GN_PW
    //        GN_PW(dataCenter);
    //        #endregion
    //        #region  EXSR      @GN_IL
    //        GN_IL(dataCenter);
    //        #endregion
    //        #region  EXSR      @GN_PL
    //        GN_PL(dataCenter);
    //        #endregion
    //        #region  EXSR      @GN_PM
    //        GN_PM(dataCenter);
    //        #endregion
    //        #region  EXSR      @GN_HP
    //        GN_HP(dataCenter);
    //        #endregion
    //        #region  EXSR      @GN_HM
    //        GN_HM(dataCenter);
    //        #endregion
    //        #region  EXSR      @GN_PH
    //        GN_PH(dataCenter);
    //        #endregion
    //    }
    //    public void GN_RL(EB_Service.DAL.DataCenter dataCenter)
    //    {
    //        ILDataCenter ilobj = new ILDataCenter();
    //        string sql = $@"SELECT P1APRJ RLP1APRJ, P1LOCA RLP1LOCA, P1RSTS RLP1RSTS, P1AVDT RLP1AVDT
    //                    FROM AS400DB01.RLOD0001.RLMS01 WITH (NOLOCK)
    //                    WHERE P1CSNO = '{TMCSN}' ";
    //        DataSet DS_RL = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
    //        if (ilobj.check_dataset(DS_RL))
    //        {
    //            foreach (DataRow dr in DS_RL.Tables[0].Rows)
    //            {
    //                FGWRT = "N";
    //                if (dr["RLP1APRJ"].ToString() == "R" || dr["RLP1APRJ"].ToString() == "C" ||
    //                (dr["RLP1APRJ"].ToString() == "A" && Convert.ToInt32(dr["RLP1LOCA"].ToString()) >= 250 &&
    //                dr["RLP1RSTS"].ToString() != "X"))
    //                {
    //                    TMROC = dr["RLP1APRJ"].ToString();
    //                    TMBIZ = "RL";
    //                    #region EXSR      @WRTTM    
    //                    WRTTM(dataCenter);
    //                    #endregion
    //                    FGWRT = "Y";
    //                }
    //                TM2AVD = Convert.ToInt32(!string.IsNullOrEmpty(dr["RLP1AVDT"].ToString()) ? dr["RLP1AVDT"].ToString() : "0");
    //                if (TM2AVD < 25000000 && TM2AVD != 0)
    //                {
    //                    TM2AVD = TM2AVD + 25000000;
    //                }
    //                if (FGWRT == "Y")
    //                {
    //                    #region EXSR      @CNTOF      
    //                    CNTOF();
    //                    #endregion
    //                }
    //            }
    //        }
    //    }
    //    public void GN_PW(EB_Service.DAL.DataCenter dataCenter)
    //    {
    //        ILDataCenter ilobj = new ILDataCenter();
    //        string sql = $@"SELECT P1APRJ PWP1APRJ, P1LOCA PWP1LOCA, P1RSTS PWP1RSTS, P1CSID PWP1CSID, P1AVDT PWP1AVDT
    //                    FROM AS400DB01.PWOD0001.PWMS01 WITH (NOLOCK)
    //                    WHERE P1CSID = '{TMIDN}' ";
    //        DataSet DS_PW = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
    //        if (ilobj.check_dataset(DS_PW))
    //        {
    //            foreach (DataRow dr in DS_PW.Tables[0].Rows)
    //            {
    //                FGWRT = "N";
    //                if (dr["PWP1APRJ"].ToString() == "R" || dr["PWP1APRJ"].ToString() == "C" ||
    //                (dr["PWP1APRJ"].ToString() == "A" && Convert.ToInt32(dr["PWP1LOCA"].ToString()) >= 250 &&
    //                dr["PWP1RSTS"].ToString() != "X"))
    //                {
    //                    TMROC = dr["PWP1APRJ"].ToString();
    //                    TMBIZ = "PW";
    //                    #region EXSR      @WRTTM    
    //                    WRTTM(dataCenter);
    //                    #endregion
    //                    FGWRT = "Y";
    //                }
    //                TM2AVD = Convert.ToInt32(!string.IsNullOrEmpty(dr["PWP1AVDT"].ToString()) ? dr["PWP1AVDT"].ToString() : "0");
    //                if (TM2AVD < 25000000 && TM2AVD != 0)
    //                {
    //                    TM2AVD = TM2AVD + 25000000;
    //                }
    //                if (FGWRT == "Y")
    //                {
    //                    #region EXSR      @CNTOF      
    //                    CNTOF();
    //                    #endregion
    //                }
    //                //break;
    //            }
    //        }
    //    }
    //    public void GN_IL(EB_Service.DAL.DataCenter dataCenter)
    //    {
    //        ILDataCenter ilobj = new ILDataCenter();
    //        string sql = $@"SELECT P1APRJ ILP1APRJ, P1LOCA ILP1LOCA, P1STDT ILP1STDT, P1AVDT ILP1AVDT
    //                    FROM AS400DB01.ILOD0001.ILMS01 WITH (NOLOCK)
    //                    WHERE P1CSNO = '{TMCSN}'";
    //        DataSet DS_IL = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
    //        if (ilobj.check_dataset(DS_IL))
    //        {
    //            foreach (DataRow dr in DS_IL.Tables[0].Rows)
    //            {
    //                FGWRT = "N";
    //                if (dr["ILP1APRJ"].ToString() == "RJ" || dr["ILP1APRJ"].ToString() == "CN" ||
    //                    (dr["ILP1APRJ"].ToString() == "AP" && Convert.ToInt32(dr["ILP1LOCA"].ToString()) >= 275
    //                    && Convert.ToInt32(dr["ILP1LOCA"].ToString()) != 301))
    //                {
    //                    TMROC = dr["ILP1APRJ"].ToString();
    //                    if (dr["ILP1APRJ"].ToString() == "AP" && Convert.ToInt32(dr["ILP1LOCA"].ToString()) == 300)
    //                    {
    //                        TMROC = "C";
    //                    }
    //                    TMBIZ = "IL";
    //                    #region EXSR      @WRTTM    
    //                    WRTTM(dataCenter);
    //                    #endregion
    //                    FGWRT = "Y";
    //                }
    //                TM2AVD = Convert.ToInt32(!string.IsNullOrEmpty(dr["ILP1STDT"].ToString()) ? dr["ILP1STDT"].ToString() : "0");
    //                if (TM2AVD == 0)
    //                {
    //                    TM2AVD = Convert.ToInt32(!string.IsNullOrEmpty(dr["ILP1AVDT"].ToString()) ? dr["ILP1AVDT"].ToString() : "0");
    //                }
    //                if (Convert.ToInt32(dr["ILP1LOCA"].ToString()) == 300)
    //                {
    //                    sql = $@"SELECT P2LMVD ILP2LMVD
    //                                FROM AS400DB01.ILOD0001.ILMS02 WITH (NOLOCK) 
    //                                WHERE P2CSNO = '{TMCSN.ToString()}' AND P2DEL = '' ";
    //                    DataSet DS_IL2 = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
    //                    if (ilobj.check_dataset(DS_IL2))
    //                    {
    //                        DataRow dr_IL2 = DS_IL2.Tables[0].Rows.Count > 0 ? DS_IL2.Tables[0].Rows[0] : null;
    //                        TM2AVD = Convert.ToInt32(!string.IsNullOrEmpty(dr["ILP2LMVD"].ToString()) ? dr["ILP2LMVD"].ToString() : "0");
    //                    }
    //                }
    //                if (TM2AVD < 25000000 && TM2AVD != 0)
    //                {
    //                    TM2AVD = TM2AVD + 25000000;
    //                }
    //                if (FGWRT == "Y")
    //                {
    //                    #region EXSR      @CNTOF    
    //                    CNTOF();
    //                    #endregion
    //                }
    //                //break;
    //            }
    //        }
    //    }
    //    public void GN_PL(EB_Service.DAL.DataCenter dataCenter)
    //    {
    //        ILDataCenter ilobj = new ILDataCenter();
    //        string sql = $@"SELECT P1APRJ PLP1APRJ, P1LOCA PLP1LOCA, P1RSTS PLP1RSTS, P1CSID PLP1CSID, P1AVDT PLP1AVDT
    //                    FROM AS400DB01.PLOD0001.PLMS01 WITH (NOLOCK)
    //                    WHERE P1CSID = '{TMIDN}' ";
    //        DataSet DS_PL = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
    //        if (ilobj.check_dataset(DS_PL))
    //        {
    //            foreach (DataRow dr in DS_PL.Tables[0].Rows)
    //            {
    //                FGWRT = "N";
    //                if (dr["PLP1APRJ"].ToString() == "R" || dr["PLP1APRJ"].ToString() == "C" ||
    //                (dr["PLP1APRJ"].ToString() == "A" && Convert.ToInt32(dr["PLP1LOCA"].ToString()) >= 250 &&
    //                dr["PLP1RSTS"].ToString() != "X"))
    //                {
    //                    TMROC = dr["PLP1APRJ"].ToString();
    //                    TMBIZ = "PL";
    //                    #region EXSR      @WRTTM    
    //                    WRTTM(dataCenter);
    //                    #endregion
    //                    FGWRT = "Y";
    //                }
    //                TM2AVD = Convert.ToInt32(!string.IsNullOrEmpty(dr["PLP1AVDT"].ToString()) ? dr["PLP1AVDT"].ToString() : "0");
    //                if (TM2AVD < 25000000 && TM2AVD != 0)
    //                {
    //                    TM2AVD = TM2AVD + 25000000;
    //                }
    //                //break;
    //            }
    //        }
    //    }
    //    public void GN_PM(EB_Service.DAL.DataCenter dataCenter)
    //    {
    //        ILDataCenter ilobj = new ILDataCenter();
    //        string sql = $@"SELECT P1APRJ PMP1APRJ, P1LOCA PMP1LOCA, P1RSTS PMP1RSTS, P1CSID PMP1CSID, P1AVDT PMP1AVDT
    //                    FROM AS400DB01.PMOD0001.PMMS01 WITH (NOLOCK)
    //                    WHERE P1CSID = '{TMIDN}' ";
    //        DataSet DS_PM = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
    //        if (ilobj.check_dataset(DS_PM))
    //        {
    //            foreach (DataRow dr in DS_PM.Tables[0].Rows)
    //            {
    //                FGWRT = "N";
    //                if (dr["PMP1APRJ"].ToString() == "R" || dr["PMP1APRJ"].ToString() == "C" ||
    //                (dr["PMP1APRJ"].ToString() == "A" && Convert.ToInt32(dr["PMP1LOCA"].ToString()) >= 250 &&
    //                dr["PMP1RSTS"].ToString() != "X"))
    //                {
    //                    TMROC = dr["PMP1APRJ"].ToString();
    //                    TMBIZ = "PM";
    //                    #region EXSR      @WRTTM    
    //                    WRTTM(dataCenter);
    //                    #endregion
    //                    FGWRT = "Y";
    //                }
    //                TM2AVD = Convert.ToInt32(!string.IsNullOrEmpty(dr["PMP1AVDT"].ToString()) ? dr["PMP1AVDT"].ToString() : "0");
    //                if (Convert.ToInt32(TM2AVD) < 25000000 && Convert.ToInt32(TM2AVD) != 0)
    //                {
    //                    TM2AVD = TM2AVD + 25000000;
    //                }
    //                //break;
    //            }
    //        }
    //    }
    //    public void GN_HP(EB_Service.DAL.DataCenter dataCenter)
    //    {
    //        ILDataCenter ilobj = new ILDataCenter();
    //        string sql = $@"SELECT CUSTC AS TCUSTC 
    //                    FROM AS400DB01.HPOD0000.HPMS10 WITH (NOLOCK)
    //                    WHERE IDNO10 = '{TMIDN}'";
    //        DataSet DS_HP = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
    //        if (ilobj.check_dataset(DS_HP))
    //        {
    //            DataRow dr_hp10 = DS_HP.Tables[0].Rows.Count > 0 ? DS_HP.Tables[0].Rows[0] : null;
    //            sql = $@"SELECT ACAPPL, ACBRN
    //                        FROM AS400DB01.SYOD0000.SYFAPCTL WITH (NOLOCK)";
    //            DataSet DS_SY = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
    //            if (ilobj.check_dataset(DS_SY))
    //            {
    //                foreach (DataRow dr_sy in DS_SY.Tables[0].Rows)
    //                {
    //                    if (dr_sy["ACAPPL"].ToString() == "HP")
    //                    {
    //                        FHPMS25 = string.Empty;
    //                        FHPMS30 = string.Empty;
    //                        try
    //                        {
    //                            GNMODE = WebConfigurationManager.AppSettings["Mode"].ToString().Trim();
    //                        }
    //                        catch
    //                        {
    //                            GNMODE = "O";
    //                        }
    //                        if (GNMODE == "T")
    //                        {
    //                            ACDTLB = "X";
    //                            if (dr_sy["ACBRN"].ToString() == "001")
    //                            {
    //                                ACDTLB = "HPTEST";
    //                            }
    //                        }
    //                        FHPMS25 = ACDTLB.Trim() + "/HPMS25L9";
    //                        FHPMS30 = ACDTLB.Trim() + "/HPMS30";
    //                        sql = $@"SELECT [REASON], [STATUS], CONTNO, STATDATE
    //                            FROM AS400DB01.HPOD0000.HPMS25 WITH (NOLOCK)
    //                            WHERE CUSTC = '{dr_hp10["TCUSTC"].ToString()}'";
    //                        DataSet DS_HP25 = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
    //                        if (ilobj.check_dataset(DS_HP25))
    //                        {
    //                            foreach (DataRow dr_hp25 in DS_HP25.Tables[0].Rows)
    //                            {
    //                                FGWRT = "N";
    //                                TMROC = dr_hp25["REASON"].ToString().Substring(0, 1);
    //                                if (TMROC == "L" ||TMROC == "N" ||TMROC == "O" ||TMROC == "R" ||TMROC == "G" ||TMROC == "X" ||TMROC == "Z")
    //                                {
    //                                    TMROC = "R";
    //                                    TMBIZ = "HP";
    //                                    #region                    EXSR      @WRTTM     
    //                                    WRTTM(dataCenter);
    //                                    #endregion
    //                                    FGWRT = "Y";
    //                                }
    //                                if(TMROC == "S")
    //                                {
    //                                    TMROC = "C";
    //                                    TMBIZ = "HP";
    //                                    #region                    EXSR      @WRTTM     
    //                                    WRTTM(dataCenter);
    //                                    #endregion
    //                                    FGWRT = "Y";
    //                                }
    //                                if(dr_hp25["STATUS"].ToString() == "A")
    //                                {
    //                                    KBRN = Convert.ToInt32(!string.IsNullOrEmpty(dr_sy["ACBRN"].ToString()) ? dr_sy["ACBRN"].ToString() : "0");
    //                                    KCNT = Convert.ToInt64(!string.IsNullOrEmpty(dr_hp25["CONTNO"].ToString()) ? dr_hp25["CONTNO"].ToString() : "0");
    //                                    sql = $@"SELECT [DEL], [STATUS]
    //                                        FROM AS400DB01.HPOD0000.HPMS30 WITH (NOLOCK)
    //                                        WHERE BRANCH = '{KBRN.ToString()}' AND CONTNO = '{KCNT}'";
    //                                    DataSet DS_HP30 = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
    //                                    if (ilobj.check_dataset(DS_HP30))
    //                                    {
    //                                        DataRow dr_HP30 = DS_HP30.Tables[0].Rows.Count > 0 ? DS_HP30.Tables[0].Rows[0] : null;
    //                                        if((dr_HP30["DEL"].ToString() != "D" || dr_HP30["STATUS"].ToString() != "TM") && dr_HP30["DEL"].ToString() != "X")
    //                                        {
    //                                            TMROC = "A";
    //                                            TMBIZ = "HP";
    //                                            #region                    EXSR      @WRTTM     
    //                                            WRTTM(dataCenter);
    //                                            #endregion
    //                                            FGWRT = "Y";
    //                                        }
    //                                    }
    //                                    else
    //                                    {
    //                                        ACDTLB = "HPTEST";
    //                                    }
    //                                }
    //                                TM2AVD = Convert.ToInt32(!string.IsNullOrEmpty(dr_hp25["STATDATE"].ToString()) ? dr_hp25["STATDATE"].ToString() : "0");
    //                                if(TM2AVD < 25000000 && TM2AVD != 0)
    //                                {
    //                                    TM2AVD = TM2AVD + 25000000;
    //                                }
    //                            }
    //                        }
    //                        else
    //                        {
    //                            ACDTLB = "HPTEST";
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //    }
    //    public void GN_HM(EB_Service.DAL.DataCenter dataCenter)
    //    {
    //        ILDataCenter ilobj = new ILDataCenter();
    //        string sql = $@"SELECT P1APRJ HMP1APRJ, P1LOCA HMP1LOCA, P1STDT HMP1STDT, P1CSID HMP1CSID, P1AVDT HMP1AVDT
    //                    FROM AS400DB01.HMOD0001.HMMS01 WITH (NOLOCK)
    //                    WHERE P1CSID = '{TMIDN}' ";
    //        DataSet DS_HM = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
    //        if (ilobj.check_dataset(DS_HM))
    //        {
    //            foreach (DataRow dr in DS_HM.Tables[0].Rows)
    //            {
    //                FGWRT = "N";
    //                if (dr["HMP1APRJ"].ToString() == "R" || dr["HMP1APRJ"].ToString() == "C" ||
    //                (dr["HMP1APRJ"].ToString() == "A" && Convert.ToInt32(dr["HMP1LOCA"].ToString()) >= 275 &&
    //                Convert.ToInt32(dr["HMP1LOCA"].ToString()) >= 301))
    //                {
    //                    TMROC = dr["HMP1APRJ"].ToString();
    //                    if (dr["HMP1APRJ"].ToString() == "A" && Convert.ToInt32(dr["HMP1LOCA"].ToString()) == 300)
    //                    {
    //                        TMROC = "C";
    //                    }
    //                    TMBIZ = "HM";
    //                    #region EXSR      @WRTTM    
    //                    WRTTM(dataCenter);
    //                    #endregion
    //                    FGWRT = "Y";
    //                }
    //                TM2AVD = Convert.ToInt32(!string.IsNullOrEmpty(dr["HMP1STDT"].ToString()) ? dr["HMP1STDT"].ToString() : "0");
    //                if (TM2AVD == 0)
    //                {
    //                    TM2AVD = Convert.ToInt32(!string.IsNullOrEmpty(dr["HMP1AVDT"].ToString()) ? dr["HMP1AVDT"].ToString() : "0");
    //                }
    //                if (Convert.ToInt32(TM2AVD) < 25000000 && Convert.ToInt32(TM2AVD) != 0)
    //                {
    //                    TM2AVD = TM2AVD + 25000000;
    //                }
    //                //break;
    //            }
    //        }
    //    }
    //    public void GN_PH(EB_Service.DAL.DataCenter dataCenter)
    //    {
    //        ILDataCenter ilobj = new ILDataCenter();
    //        string sql = $@"SELECT P1APRJ PHP1APRJ, P1LOCA PHP1LOCA, P1STDT PHP1STDT, P1CSID PHP1CSID, P1AVDT PHP1AVDT
    //                    FROM AS400DB01.PHOD0001.PHMS01 WITH (NOLOCK)
    //                    WHERE P1CSID = '{TMIDN}' ";
    //        DataSet DS_PH = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
    //        if (ilobj.check_dataset(DS_PH))
    //        {
    //            foreach (DataRow dr in DS_PH.Tables[0].Rows)
    //            {
    //                FGWRT = "N";
    //                if (dr["PHP1APRJ"].ToString() == "R" || dr["PHP1APRJ"].ToString() == "C" ||
    //                (dr["PHP1APRJ"].ToString() == "A" && Convert.ToInt32(dr["PHP1LOCA"].ToString()) >= 275 &&
    //                Convert.ToInt32(dr["PHP1LOCA"].ToString()) >= 301))
    //                {
    //                    TMROC = dr["PHP1APRJ"].ToString();
    //                    if (dr["PHP1APRJ"].ToString() == "A" && Convert.ToInt32(dr["PHP1LOCA"].ToString()) == 300)
    //                    {
    //                        TMROC = "C";
    //                    }
    //                    TMBIZ = "PH";
    //                    #region EXSR      @WRTTM    
    //                    WRTTM(dataCenter);
    //                    #endregion
    //                    FGWRT = "Y";
    //                }
    //                TM2AVD = Convert.ToInt32(!string.IsNullOrEmpty(dr["PHP1STDT"].ToString()) ? dr["PHP1STDT"].ToString() : "0");
    //                if (Convert.ToInt32(TM2AVD) == 0)
    //                {
    //                    TM2AVD = Convert.ToInt32(!string.IsNullOrEmpty(dr["PHP1AVDT"].ToString()) ? dr["PHP1AVDT"].ToString() : "0");

    //                }
    //                if (Convert.ToInt32(TM2AVD) < 25000000 && Convert.ToInt32(TM2AVD) != 0)
    //                {
    //                    TM2AVD = TM2AVD + 25000000;
    //                }
    //                //break;
    //            }
    //        }
    //    }
    //    private void WRTTM(EB_Service.DAL.DataCenter dataCenter)
    //    {
    //        ILDataCenter ilobj = new ILDataCenter();
    //        T001 = string.Empty;
    //        switch (TMTYP)
    //        {
    //            case "H":
    //                index.TM1GRP = "1";
    //                index.TM1TNO = WKTEL;
    //                break;
    //            case "M":
    //                index.TM1GRP = "2";
    //                index.TM1TNO = WKTEL;
    //                break;
    //            case "O":
    //                index.TM1GRP = "3";
    //                index.TM1TNO = WKTEL;
    //                break;
    //            case "S":
    //                index.TM1GRP = "4";
    //                index.TM1TNO = WKTEL;
    //                break;
    //            case "F":
    //                index.TM1GRP = "5";
    //                index.TM1TNO = WKTEL;
    //                break;
    //        }
    //        index.TM1BIZ = TMBIZ;
    //        switch (TMBIZ)
    //        {
    //            case "HP":
    //                index.TM1SOR = "1";
    //                break;
    //            case "PL":
    //                index.TM1SOR = "2";
    //                break;
    //            case "PH":
    //                index.TM1SOR = "3";
    //                break;
    //            case "HM":
    //                index.TM1SOR = "4";
    //                break;
    //            case "PM":
    //                index.TM1SOR = "5";
    //                break;
    //            case "PW":
    //                index.TM1SOR = "6";
    //                break;
    //            case "IL":
    //                index.TM1SOR = "7";
    //                break;
    //            case "RL":
    //                index.TM1SOR = "8";
    //                break;
    //        }
    //        string sql = $@"SELECT T01RRNO, [INDEX], [DATA] 
    //                FROM AS400DB01.GNOD0000.GNTM01 WITH (NOLOCK)
    //                WHERE SUBSTRING(INDEX,1,1) = '{index.TM1GRP}' AND SUBSTRING(INDEX,2,1) = '{index.TM1SOR}'
    //                AND SUBSTRING(INDEX,3,20) = '{index.TM1TNO}' AND SUBSTRING(INDEX,23,2) = '{index.TM1BIZ}' ";
    //        DataSet DS_GNTM01 = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
    //        if (ilobj.check_dataset(DS_GNTM01))
    //        {
    //            DataRow DR_GNTM01 = DS_GNTM01.Tables[0].Rows.Count > 0 ? DS_GNTM01.Tables[0].Rows[0] : null;
    //            switch (TMROC)
    //            {
    //                case "R":
    //                case "RJ":
    //                    data.TM1REJ++;
    //                    break;
    //                case "C":
    //                case "CN":
    //                    data.TM1CAN++;
    //                    break;
    //                case "A":
    //                case "AP":
    //                    data.TM1APV++;
    //                    break;
    //            }
    //            //Update GNTM01
    //            string sql_Update = $@"UPDATE AS400DB01.GNOD0000.GNTM01
    //                                   SET DATA = '{data.TM1REJ.ToString().PadLeft(5, '0') + data.TM1CAN.ToString().PadLeft(5, '0') + data.TM1APV.ToString().PadLeft(5, '0') + data.A1TEL[A1].PadLeft(11, '0')}'
    //                                   WHERE T01RRNO = '{DR_GNTM01["T01RRNO"]}' AND INDEX = '{DR_GNTM01["INDEX"]}' ";
    //            bool transaction = dataCenter.Sqltr == null ? true : false;
    //            int afrows = dataCenter.Execute(sql_Update, CommandType.Text, transaction).Result.afrows;
    //        }
    //        else
    //        {
    //            data.TM1REJ = 0;
    //            data.TM1CAN = 0;
    //            data.TM1APV = 0;
    //            switch (TMROC)
    //            {
    //                case "R":
    //                case "RJ":
    //                    data.TM1REJ++;
    //                    break;
    //                case "C":
    //                case "CN":
    //                    data.TM1CAN++;
    //                    break;
    //                case "A":
    //                case "AP":
    //                    data.TM1APV++;
    //                    break;
    //            }
    //            //INSERT GNTM01
    //            string sql_Insert = $@"INSERT INTO AS400DB01.GNOD0000.GNTM01([INDEX],[DATA])
    //                                VALUES('{index.TM1GRP + index.TM1SOR + index.TM1TNO + index.TM1BIZ}',
    //                                '{data.TM1REJ.ToString().PadLeft(5, '0') + data.TM1CAN.ToString().PadLeft(5, '0') + data.TM1APV.ToString().PadLeft(5, '0') + data.A1TEL[A1].PadLeft(11, '0')}')";
    //            bool transaction = dataCenter.Sqltr == null ? true : false;
    //            int afrows = dataCenter.Execute(sql_Insert, CommandType.Text, transaction).Result.afrows;
    //        }
    //    }
    //    private void CNTOF()
    //    {
    //        if (TMTYP == "O" || TMTYP == "F")
    //        {
    //            if(TM2AVD > Convert.ToInt32(CHKDTE))
    //            {
    //                switch (TMTYP)
    //                {
    //                    case "O":
    //                        TMDPTO++;
    //                        break;
    //                    case "F":
    //                        TMDPOF++;
    //                        break;
    //                }
    //            }

    //        }
    //    }
    //    private void MOVDT(EB_Service.DAL.DataCenter dataCenter)
    //    {
    //        ILDataCenter ilobj = new ILDataCenter();
    //        string sql = $@"SELECT T01RRNO, [INDEX], [DATA] 
    //                FROM AS400DB01.GNOD0000.GNTM01 WITH (NOLOCK)";
    //        DataSet DS_GNTM01 = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
    //        if (ilobj.check_dataset(DS_GNTM01))
    //        {
    //            foreach (DataRow dr in DS_GNTM01.Tables[0].Rows)
    //            {
    //                if (RC < 9999)
    //                {
    //                    #region  EXSR      @MOVDS 
    //                    MOVDS();
    //                    #endregion
    //                }
    //            }
    //        }
    //        //if (RC < 9999)
    //        //{
    //        //    #region  EXSR      @MOVDS 
    //        //    MOVDS();
    //        //    #endregion
    //        //}
    //    }
    //    private void MOVDS()
    //    {
    //        ILDataCenter ilobj = new ILDataCenter();
    //        RC++;
    //        switch (Convert.ToString(index.TM1GRP))
    //        {
    //            case "1":
    //                lst_RESULTDS[RC].Group = "TH";
    //                lst_RESULTDS[RC].Desc = index.TM1TNO.Trim();
    //                break;
    //            case "2":
    //                lst_RESULTDS[RC].Group = "TM";
    //                lst_RESULTDS[RC].Desc = index.TM1TNO.Trim();
    //                break;
    //            case "3":
    //                lst_RESULTDS[RC].Group = "TO";
    //                lst_RESULTDS[RC].Desc = index.TM1TNO.Trim();
    //                break;
    //            case "4":
    //                lst_RESULTDS[RC].Group = "SUR";
    //                lst_RESULTDS[RC].Desc = WKSNME.Trim();
    //                break;
    //            case "5":
    //                lst_RESULTDS[RC].Group = "OF";
    //                lst_RESULTDS[RC].Desc = WKSNME.Trim();
    //                break;
    //        }
    //        lst_RESULTDS[RC].Bus = index.TM1BIZ;
    //        lst_RESULTDS[RC].Reject = data.TM1REJ;
    //        lst_RESULTDS[RC].Cancel = data.TM1CAN;
    //        lst_RESULTDS[RC].Approve = data.TM1APV;
    //        lst_RESULTDS[RC].Total = data.TM1REJ +data.TM1CAN + data.TM1APV;
    //        TTGRP = index.TM1GRP;
    //        TTTNO = index.TM1TNO;
    //        dataSetResult.Tables[0].Rows.Add(lst_RESULTDS[RC].Group, lst_RESULTDS[RC].Desc, lst_RESULTDS[RC].Bus, lst_RESULTDS[RC].Approve, lst_RESULTDS[RC].Reject, lst_RESULTDS[RC].Cancel, lst_RESULTDS[RC].Total);
    //    }
    //}
    #region old
    public class ILE0222 : UserInfo
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

        public ILE0222(UserInfo userInfo)
        {
            m_UserInfo = userInfo;
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

        public string WKSNME { get; private set; }
        public string WKOFNM { get; private set; }
        public string WKCSN { get; private set; }
        public string WPIDT { get; private set; }
        public int TMDPTO { get; private set; }
        public int TMDPOF { get; private set; }
        public int MAXARR { get; private set; }
        public int RC { get; private set; }
        public string KAR1 { get; private set; }
        public string[] A1TEL { get; private set; }
        public int A1 { get; private set; }
        public string TMTYP { get; private set; }
        public string WKTEL { get; private set; }
        public string sql { get; private set; }
        public DataSet DS { get; private set; }
        public string TMCSN { get; private set; }
        public string TMIDN { get; private set; }
        public string A2OFN { get; private set; }
        public int A2 { get; private set; }
        public string KBRN { get; private set; }
        public string KCNT { get; private set; }
        public string KAPN { get; private set; }
        public int X1 { get; private set; }
        public string KAR2 { get; private set; }
        public string[] A2CSN { get; private set; }
        public string[] A2BRN { get; private set; }
        public string[] A2APN { get; private set; }
        public string[] A2FLG { get; private set; }
        public string[] RESULTDS { get; private set; }
        public string Group { get; private set; }
        public string Desc { get; private set; }
        public string Biz { get; private set; }
        public int Reject { get; private set; }
        public int TM1REJ { get; private set; }
        public int Cancel { get; private set; }
        public int TM1CAN { get; private set; }
        public int Approve { get; private set; }
        public int TM1APV { get; private set; }
        public int Total { get; private set; }
        public string TTGRP { get; private set; }
        public string TTTNO { get; private set; }
        public string FGWRT { get; private set; }
        public string TMROC { get; private set; }
        public string TMBIZ { get; private set; }
        public string T001 { get; private set; }
        public string TM2AVD { get; private set; }
        public string FHPMS25 { get; private set; }
        public string FHPMS30 { get; private set; }
        public string GNMODE { get; private set; }
        public string ACDTLB { get; private set; }
        public string ACBRN { get; private set; }
        public string TM1GRP { get; private set; }
        public string TM1TNO { get; private set; }
        public string TM1BIZ { get; private set; }
        public string TM1SOR { get; private set; }
        public DataSet dataSet = new DataSet();
        public string CHKDTE = DateTime.Now.ToString("yyyyMMdd");


        public DataSet Call_ILE0222(EB_Service.DAL.DataCenter dataCenter, string PIIDNO)
        {
            try
            {
                TMDPTO = 0;
                TMDPOF = 0;
                MAXARR = 99999;
                RC = 0;
                string CHKDTE = "";
                string WPERR = "";
                A1TEL = new string[MAXARR];
                ILDataCenter ilobj = new ILDataCenter();
                ILDataSubroutine dataSubroutine = new ILDataSubroutine(m_UserInfo);
                DataSet DS = new DataSet();
                #region หา Date ILMS97
                string sql = "SELECT P97CDT FROM AS400DB01.ILOD0001.ILMS97 WITH (NOLOCK) WHERE P97REC = '01'";
                DS = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
                if (ilobj.check_dataset(DS))
                {
                    DataRow dr_ILMS97 = DS.Tables[0].Rows.Count > 0 ? DS.Tables[0].Rows[0] : null;
                    WPIDT = dr_ILMS97["P97CDT"].ToString();
                }
                bool Call_GNP023 = dataSubroutine.Call_GNP023(WPIDT, "YMD", "B", "30", "D", "-", ref CHKDTE, ref WPERR, "", "");
                #endregion
                #region EXSR      @GNTMP 
                GNTMP(dataCenter, PIIDNO);
                #endregion
                #region EXSR      @MOVDT  
                MOVDT(dataCenter);
                #endregion
                #region  EXSR      @RESULT
                RESULT();
                #endregion
                return dataSet;
            }
            catch (Exception ex)
            {
                return dataSet;
            }
        }

        private void GNTMP(EB_Service.DAL.DataCenter dataCenter, string PIIDNO)
        {
            ILDataCenter ilobj = new ILDataCenter();
            A1 = 0;
            sql = $@"SELECT IDCard M00IDN, CISNumber M00CSN, SurnameInTHAI M00TSN, OfficeName M00OFC   
                        FROM CustomerDB01.CustomerInfo.CustomerGeneral cg WITH (NOLOCK)
                        JOIN CustomerDB01.CustomerInfo.CustomerWorked cw WITH (NOLOCK) ON (cg.ID = cw.CustID)
                        WHERE IDCard = '{PIIDNO}' ";
            DS = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
            if (ilobj.check_dataset(DS))
            {
                DataRow dr_Customer = DS.Tables[0].Rows.Count > 0 ? DS.Tables[0].Rows[0] : null;
                WKSNME = dr_Customer["M00TSN"].ToString();
                WKOFNM = dr_Customer["M00OFC"].ToString();
                WKCSN = dr_Customer["M00CSN"].ToString();
            }
            else
            {
                return;
            }
            sql = $@"SELECT gc.Code M11CDE, TelephoneNumber1 M11TEL, Mobile M11MOB 
                    FROM CustomerDB01.CustomerInfo.CustomerAddress ca WITH (NOLOCK)
                    JOIN CustomerDB01.CustomerInfo.CustomerGeneral cg WITH (NOLOCK) ON (ca.CustID = cg.ID AND ca.CustRefID = 0)
                    JOIN GeneralDB01.GeneralInfo.GeneralCenter gc WITH (NOLOCK) ON (ca.AddressCodeID = gc.ID AND Type = 'AddressCodeID')
                    WHERE CISNumber = '{WKCSN}' ";
            DS = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
            if (ilobj.check_dataset(DS))
            {
                DataRow dr_CustomerAdress = DS.Tables[0].Rows.Count > 0 ? DS.Tables[0].Rows[0] : null;
                string M11CDE = dr_CustomerAdress["M11CDE"].ToString();
                string M11TEL = dr_CustomerAdress["M11TEL"].ToString();
                string M11MOB = dr_CustomerAdress["M11MOB"].ToString();
                KAR1 = "";

                switch (M11CDE)
                {
                    case "H":
                        if (!string.IsNullOrWhiteSpace(M11TEL) && M11TEL != "**" && M11TEL != "000000000")
                        {
                            KAR1 = "H" + M11TEL.Substring(0, Math.Min(9, M11TEL.Length));
                            GNAR1();
                        }
                        if (!string.IsNullOrWhiteSpace(M11MOB) && M11MOB != "**" && M11MOB != "000000000")
                        {
                            KAR1 = "M" + M11MOB.Substring(0, Math.Min(10, M11MOB.Length));
                            GNAR1();
                        }
                        break;
                    case "O":
                        if (!string.IsNullOrWhiteSpace(M11TEL) && M11TEL != "**" && M11TEL != "000000000")
                        {
                            KAR1 = "O" + M11TEL.Substring(0, Math.Min(9, M11TEL.Length));
                            GNAR1();
                        }
                        break;
                }

            }
            #region     ** GROUP 1   
            GROUP1(dataCenter);
            #endregion
            #region     ** GROUP 2   
            GROUP2(dataCenter);
            #endregion
            #region     ** GROUP 3   
            GROUP3(dataCenter);
            #endregion
            #region     ** GROUP 4   
            GROUP4(dataCenter);
            #endregion
            #region     ** GROUP 5   
            GROUP5(dataCenter);
            #endregion
        }
        public void GNAR1()
        {
            if (!string.IsNullOrWhiteSpace(KAR1))
            {
                int index = Array.IndexOf(A1TEL, KAR1);
                if (index == -1 && A1 < MAXARR)
                {
                    A1++;
                    A1TEL[A1] = KAR1;
                }
            }
        }
        public void GROUP1(EB_Service.DAL.DataCenter dataCenter)
        {
            ILDataCenter ilobj = new ILDataCenter();
            A1 = 1;
            while (A1 < A1TEL.Length && !string.IsNullOrEmpty(A1TEL[A1]))
            {
                TMTYP = string.Empty;
                TMTYP = A1TEL[A1].Substring(0, 1);
                if (TMTYP == "H")
                {
                    WKTEL = string.Empty;
                    WKTEL = A1TEL[A1].Substring(1, Math.Min(10, A1TEL[A1].Length - 1));
                    sql = $@"SELECT gc.Code M12ACD, gc2.Code M12TTY, CustID M12CSN
                            FROM CustomerDB01.CustomerInfo.CustomerTelephone ct WITH (NOLOCK)
                            JOIN GeneralDB01.GeneralInfo.GeneralCenter gc WITH (NOLOCK) ON (ct.AddressCodeID = gc.ID AND gc.Type = 'AddressCodeID')
                            JOIN GeneralDB01.GeneralInfo.GeneralCenter gc2 WITH (NOLOCK) ON (ct.TelephoneTypeID = gc2.ID AND gc2.Type = 'TelephoneTypeID')                            
                            WHERE TelephoneNumber = '{WKTEL}' ";
                    DS = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
                    if (ilobj.check_dataset(DS))
                    {
                        foreach (DataRow dr in DS.Tables[0].Rows)
                        {
                            if (dr["M12ACD"].ToString() == "H" && dr["M12TTY"].ToString() == "P")
                            {
                                sql = $@"SELECT CISNumber M00CSN, IDCard M00IDN 
                                FROM CustomerDB01.CustomerInfo.CustomerGeneral cg WITH (NOLOCK) 
                                WHERE cg.ID = {dr["M12CSN"].ToString()}";
                                DataSet DS_General = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
                                if (ilobj.check_dataset(DS_General))
                                {
                                    DataRow dr_00 = DS_General.Tables[0].Rows.Count > 0 ? DS_General.Tables[0].Rows[0] : null;

                                    TMCSN = dr_00["M00CSN"].ToString();
                                    TMIDN = dr_00["M00IDN"].ToString();
                                }
                                else
                                {
                                    NXGP1();
                                    return;
                                }
                            }
                            #region EXSR      @GNALL 
                            GNALL(dataCenter);
                            #endregion
                        }
                    }
                }
                A1++;
            }
        }
        public void GROUP2(EB_Service.DAL.DataCenter dataCenter)
        {
            ILDataCenter ilobj = new ILDataCenter();
            A1 = 1;
            while (A1 < A1TEL.Length && !string.IsNullOrEmpty(A1TEL[A1]))
            {
                TMTYP = string.Empty;
                TMTYP = A1TEL[A1].Substring(0, 1);
                if (TMTYP == "O")
                {
                    WKTEL = string.Empty;
                    WKTEL = A1TEL[A1].Substring(1, Math.Min(10, A1TEL[A1].Length - 1));
                    sql = $@"SELECT gc.Code M12ACD, gc2.Code M12TTY, CustID M12CSN
                            FROM CustomerDB01.CustomerInfo.CustomerTelephone ct WITH (NOLOCK)
                            JOIN GeneralDB01.GeneralInfo.GeneralCenter gc WITH (NOLOCK) ON (ct.AddressCodeID = gc.ID AND gc.Type = 'AddressCodeID')
                            JOIN GeneralDB01.GeneralInfo.GeneralCenter gc2 WITH (NOLOCK) ON (ct.TelephoneTypeID = gc2.ID AND gc2.Type = 'TelephoneTypeID')                            
                            WHERE TelephoneNumber = '{WKTEL}' ";
                    DS = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
                    if (ilobj.check_dataset(DS))
                    {
                        foreach (DataRow dr in DS.Tables[0].Rows)
                        {
                            if (dr["M12ACD"].ToString() == "O" && dr["M12TTY"].ToString() == "P")
                            {
                                sql = $@"SELECT CISNumber M00CSN, IDCard M00IDN 
                                FROM CustomerDB01.CustomerInfo.CustomerGeneral cg WITH (NOLOCK) 
                                WHERE cg.ID = {dr["M12CSN"].ToString()}";
                                DataSet DS_General = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
                                if (ilobj.check_dataset(DS_General))
                                {
                                    DataRow dr_00 = DS_General.Tables[0].Rows.Count > 0 ? DS_General.Tables[0].Rows[0] : null;
                                    TMCSN = dr_00["M00CSN"].ToString();
                                    TMIDN = dr_00["M00IDN"].ToString();
                                }
                                else
                                {
                                    NXGP2();
                                    return;
                                }
                            }
                            #region EXSR      @GNALL 
                            GNALL(dataCenter);
                            #endregion
                        }
                    }
                }
                A1++;
            }
        }
        public void GROUP3(EB_Service.DAL.DataCenter dataCenter)
        {
            ILDataCenter ilobj = new ILDataCenter();
            A1 = 1;
            while (A1 < A1TEL.Length && !string.IsNullOrEmpty(A1TEL[A1]))
            {
                TMTYP = string.Empty;
                TMTYP = A1TEL[A1].Substring(0, 1);
                if (TMTYP == "M")
                {
                    WKTEL = string.Empty;
                    WKTEL = A1TEL[A1].Substring(1, Math.Min(10, A1TEL[A1].Length - 1));
                    sql = $@"SELECT gc.Code M12ACD, gc2.Code M12TTY, CustID M12CSN
                            FROM CustomerDB01.CustomerInfo.CustomerTelephone ct WITH (NOLOCK)
                            JOIN GeneralDB01.GeneralInfo.GeneralCenter gc WITH (NOLOCK) ON (ct.AddressCodeID = gc.ID AND gc.Type = 'AddressCodeID')
                            JOIN GeneralDB01.GeneralInfo.GeneralCenter gc2 WITH (NOLOCK) ON (ct.TelephoneTypeID = gc2.ID AND gc2.Type = 'TelephoneTypeID')                            
                            WHERE TelephoneNumber = '{WKTEL}' ";
                    DS = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
                    if (ilobj.check_dataset(DS))
                    {
                        foreach (DataRow dr in DS.Tables[0].Rows)
                        {
                            if (dr["M12ACD"].ToString() == "H" && dr["M12TTY"].ToString() == "M")
                            {
                                sql = $@"SELECT CISNumber M00CSN, IDCard M00IDN 
                                FROM CustomerDB01.CustomerInfo.CustomerGeneral cg WITH (NOLOCK) 
                                WHERE cg.ID = {dr["M12CSN"].ToString()}";
                                DataSet DS_General = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
                                if (ilobj.check_dataset(DS))
                                {
                                    DataRow dr_00 = DS_General.Tables[0].Rows.Count > 0 ? DS_General.Tables[0].Rows[0] : null;
                                    TMCSN = dr_00["M00CSN"].ToString();
                                    TMIDN = dr_00["M00IDN"].ToString();
                                }
                                else
                                {
                                    NXGP3();
                                    return;
                                }
                            }
                            #region EXSR      @GNALL 
                            GNALL(dataCenter);
                            #endregion
                        }
                    }
                }
                A1++;
            }
        }
        public void GROUP4(EB_Service.DAL.DataCenter dataCenter)
        {
            ILDataCenter ilobj = new ILDataCenter();
            if (!string.IsNullOrEmpty(WKSNME))
            {
                sql = $@"SELECT IDCard M00IDN, CISNumber M00CSN
                    FROM CustomerDB01.CustomerInfo.CustomerGeneral WITH (NOLOCK)
                    WHERE SurnameInTHAI = '{WKSNME}' ";
                DS = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
                if (ilobj.check_dataset(DS))
                {
                    foreach (DataRow dr in DS.Tables[0].Rows)
                    {
                        TMCSN = dr["M00CSN"].ToString();
                        TMIDN = dr["M00IDN"].ToString();
                        TMTYP = "S";
                        #region EXSR      @GNALL 
                        GNALL(dataCenter);
                        #endregion
                    }
                }
            }
        }
        public void GROUP5(EB_Service.DAL.DataCenter dataCenter)
        {
            ILDataCenter ilobj = new ILDataCenter();
            if (!string.IsNullOrEmpty(WKOFNM))
            {
                A2OFN = string.Empty;
                A2 = 0;
                sql = $@"SELECT D3BRN RLD3BRN, D3APNO RLD3APNO 
                        FROM AS400DB01.RLOD0001.RLMD03 WITH (NOLOCK)
                        WHERE D3OFC = '{WKOFNM}' AND D3DEL = '' ";
                DS = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
                if (ilobj.check_dataset(DS))
                {
                    foreach (DataRow dr in DS.Tables[0].Rows)
                    {
                        KBRN = dr["RLD3BRN"].ToString();
                        KAPN = dr["RLD3APNO"].ToString();
                        sql = $@"SELECT P1APRJ R1P1APRJ, P1LOCA R1P1LOCA, P1RSTS R1P1RSTS, P1CSNO R1P1CSNO
                                FROM AS400DB01.RLOD0001.RLMS01 WITH (NOLOCK)
                                WHERE P1BRN = {KBRN} AND P1APNO = {KAPN} ";
                        DataSet DS_RLMS01 = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
                        if (ilobj.check_dataset(DS_RLMS01))
                        {
                            DataRow dr_RLMS01 = DS_RLMS01.Tables[0].Rows.Count > 0 ? DS_RLMS01.Tables[0].Rows[0] : null;
                            if (dr_RLMS01["R1P1APRJ"].ToString() == "R" || dr_RLMS01["R1P1APRJ"].ToString() == "C" ||
                                (dr_RLMS01["R1P1APRJ"].ToString() == "A" && Convert.ToInt32(dr_RLMS01["R1P1LOCA"].ToString()) >= 250 &&
                                dr_RLMS01["R1P1RSTS"].ToString() != "X"))
                            {
                                X1 = 1;
                                KAR2 = dr_RLMS01["R1P1CSNO"].ToString();
                                if (A2CSN != null)
                                {
                                    bool IN50 = Array.Exists(A2CSN, element => element == KAR2);
                                    if (!IN50)
                                    {
                                        A2 = 1;
                                        A2CSN[A2] = KAR2;
                                        A2BRN[A2] = dr["RLD3BRN"].ToString();
                                        A2APN[A2] = dr["RLD3APNO"].ToString();
                                        A2FLG[A2] = "N";
                                    }
                                }

                            }
                        }
                        if (A2 == MAXARR)
                        {
                            break;
                        }
                    }
                }
                sql = $@"SELECT cg.CISNumber M00CSN, cg.IDCard M00IDN
                        FROM CustomerDB01.CustomerInfo.CustomerGeneral cg WITH (NOLOCK)
                        JOIN CustomerDB01.CustomerInfo.CustomerWorked cw WITH (NOLOCK) ON (cg.ID = cw.CustID)
                        WHERE cw.OfficeName = '{WKOFNM}' ";
                DS = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
                if (ilobj.check_dataset(DS))
                {
                    foreach (DataRow dr in DS.Tables[0].Rows)
                    {
                        TMCSN = dr["M00CSN"].ToString();
                        TMIDN = dr["M00IDN"].ToString();
                        TMTYP = "F";
                        X1 = 1;
                        KAR2 = TMCSN;
                        if (A2CSN != null)
                        {
                            bool IN50 = Array.Exists(A2CSN, element => element == KAR2);
                            if (IN50)
                            {
                                A2FLG[X1] = "Y";
                            }
                        }

                        #region EXSR      @GNALL 
                        GNALL(dataCenter);
                        #endregion
                    }
                    X1 = 1;
                    while (A1 < A2OFN.Length && !string.IsNullOrEmpty(A2OFN[X1].ToString()))
                    {
                        if (A2FLG[X1].ToString() == "N")
                        {
                            TMCSN = A2CSN[X1].ToString();
                            sql = $@"SELECT cg.CISNumber M00CSN, cg.IDCard M00IDN
                                FROM CustomerDB01.CustomerInfo.CustomerGeneral cg WITH (NOLOCK)
                                WHERE cg.CISNumber = '{TMCSN}' ";
                            DataSet DS_Customer = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
                            if (ilobj.check_dataset(DS_Customer))
                            {
                                DataRow dr_Customer = DS_Customer.Tables[0].Rows.Count > 0 ? DS_Customer.Tables[0].Rows[0] : null;
                                TMIDN = dr_Customer["M00IDN"].ToString();
                                TMTYP = "F";
                                #region EXSR      @GNALL 
                                GNALL(dataCenter);
                                #endregion
                            }
                        }
                        if (X1 == MAXARR)
                        {
                            break;
                        }
                    }
                }
            }
        }
        public void NXGP1()
        {
            if (A1 == MAXARR)
            {
                return;
            }
            A1++;
            return;
        }
        public void NXGP2()
        {
            if (A1 == MAXARR)
            {
                return;
            }
            A1++;
            return;
        }
        public void NXGP3()
        {
            if (A1 == MAXARR)
            {
                return;
            }
            A1++;
            return;
        }
        public void GNALL(EB_Service.DAL.DataCenter dataCenter)
        {
            #region  EXSR      @GN_RL
            GN_RL(dataCenter);
            #endregion
            #region  EXSR      @GN_PW
            GN_PW(dataCenter);
            #endregion
            #region  EXSR      @GN_IL
            GN_IL(dataCenter);
            #endregion
            #region  EXSR      @GN_PL
            GN_PL(dataCenter);
            #endregion
            #region  EXSR      @GN_PM
            GN_PM(dataCenter);
            #endregion
            #region  EXSR      @GN_HP
            GN_HP(dataCenter);
            #endregion
            #region  EXSR      @GN_HM
            GN_HM(dataCenter);
            #endregion
            #region  EXSR      @GN_PH
            GN_PH(dataCenter);
            #endregion
        }
        public void GN_RL(EB_Service.DAL.DataCenter dataCenter)
        {
            ILDataCenter ilobj = new ILDataCenter();
            sql = $@"SELECT P1APRJ RLP1APRJ, P1LOCA RLP1LOCA, P1RSTS RLP1RSTS, P1AVDT RLP1AVDT
                    FROM AS400DB01.RLOD0001.RLMS01 WITH (NOLOCK)
                    WHERE P1CSNO = '{TMCSN}' ";
            DataSet DS_RL = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
            if (ilobj.check_dataset(DS_RL))
            {
                foreach (DataRow dr in DS_RL.Tables[0].Rows)
                {
                    FGWRT = "N";
                    if (dr["RLP1APRJ"].ToString() == "R" || dr["RLP1APRJ"].ToString() == "C" ||
                    (dr["RLP1APRJ"].ToString() == "A" && Convert.ToInt32(dr["RLP1LOCA"].ToString()) >= 250 &&
                    dr["RLP1RSTS"].ToString() != "X"))
                    {
                        TMROC = dr["RLP1APRJ"].ToString();
                        TMBIZ = "RL";
                        #region EXSR      @WRTTM    
                        WRTTM(dataCenter);
                        #endregion
                        FGWRT = "Y";
                    }
                    TM2AVD = dr["RLP1AVDT"].ToString();
                    if (Convert.ToInt32(TM2AVD) < 25000000 && Convert.ToInt32(TM2AVD) != 0)
                    {
                        TM2AVD = Convert.ToString(Convert.ToInt32(TM2AVD) + 25000000);
                    }
                    if (FGWRT == "Y")
                    {
                        #region EXSR      @CNTOF      
                        CNTOF();
                        #endregion
                    }
                    //break;
                }
            }
        }
        public void GN_PW(EB_Service.DAL.DataCenter dataCenter)
        {
            ILDataCenter ilobj = new ILDataCenter();
            sql = $@"SELECT P1APRJ PWP1APRJ, P1LOCA PWP1LOCA, P1RSTS PWP1RSTS, P1CSID PWP1CSID, P1AVDT PWP1AVDT
                    FROM AS400DB01.PWOD0001.PWMS01 WITH (NOLOCK)
                    WHERE P1CSID = '{TMIDN}' ";
            DataSet DS_PW = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
            if (ilobj.check_dataset(DS_PW))
            {
                foreach (DataRow dr in DS_PW.Tables[0].Rows)
                {
                    FGWRT = "N";
                    if (dr["PWP1APRJ"].ToString() == "R" || dr["PWP1APRJ"].ToString() == "C" ||
                    (dr["PWP1APRJ"].ToString() == "A" && Convert.ToInt32(dr["PWP1LOCA"].ToString()) >= 250 &&
                    dr["PWP1RSTS"].ToString() != "X"))
                    {
                        TMROC = dr["PWP1APRJ"].ToString();
                        TMBIZ = "PW";
                        #region EXSR      @WRTTM    
                        WRTTM(dataCenter);
                        #endregion
                        FGWRT = "Y";
                    }
                    TM2AVD = dr["PWP1AVDT"].ToString();
                    if (Convert.ToInt32(TM2AVD) < 25000000 && Convert.ToInt32(TM2AVD) != 0)
                    {
                        TM2AVD = Convert.ToString(Convert.ToInt32(TM2AVD) + 25000000);
                    }
                    if (FGWRT == "Y")
                    {
                        #region EXSR      @CNTOF      
                        CNTOF();
                        #endregion
                    }
                    //break;
                }
            }
        }
        public void GN_IL(EB_Service.DAL.DataCenter dataCenter)
        {
            ILDataCenter ilobj = new ILDataCenter();
            sql = $@"SELECT P1APRJ ILP1APRJ, P1LOCA ILP1LOCA, P1STDT ILP1STDT, P1AVDT ILP1AVDT
                    FROM AS400DB01.ILOD0001.ILMS01 WITH (NOLOCK)
                    WHERE P1CSNO = '{TMCSN}'";
            DataSet DS_IL = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
            if (ilobj.check_dataset(DS_IL))
            {
                foreach (DataRow dr in DS_IL.Tables[0].Rows)
                {
                    FGWRT = "N";
                    if (dr["ILP1APRJ"].ToString() == "RJ" || dr["ILP1APRJ"].ToString() == "CN" ||
                        (dr["ILP1APRJ"].ToString() == "AP" && Convert.ToInt32(dr["ILP1LOCA"].ToString()) >= 275
                        && Convert.ToInt32(dr["ILP1LOCA"].ToString()) != 301))
                    {
                        TMROC = dr["ILP1APRJ"].ToString();
                        if (dr["ILP1APRJ"].ToString() == "AP" && Convert.ToInt32(dr["ILP1LOCA"].ToString()) == 300)
                        {
                            TMROC = "C";
                        }
                        TMBIZ = "IL";
                        #region EXSR      @WRTTM    
                        WRTTM(dataCenter);
                        #endregion
                        FGWRT = "Y";
                    }
                    TM2AVD = dr["ILP1STDT"].ToString();
                    if (Convert.ToInt32(TM2AVD) == 0)
                    {
                        TM2AVD = dr["ILP1AVDT"].ToString();

                    }
                    if (Convert.ToInt32(dr["ILP1LOCA"].ToString()) == 300)
                    {
                        sql = $@"SELECT P2LMVD ILP2LMVD
                                FROM AS400DB01.ILOD0001.ILMS02 WITH (NOLOCK) 
                                WHERE P2CSNO = '{TMCSN}' AND P2DEL = '' ";
                        DataSet DS_IL2 = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
                        if (ilobj.check_dataset(DS_IL2))
                        {
                            DataRow dr_IL2 = DS_IL2.Tables[0].Rows.Count > 0 ? DS_IL2.Tables[0].Rows[0] : null;
                            TM2AVD = dr_IL2["ILP2LMVD"].ToString();
                        }
                    }
                    if (Convert.ToInt32(TM2AVD) < 25000000 && Convert.ToInt32(TM2AVD) != 0)
                    {
                        TM2AVD = TM2AVD + 25000000;
                    }
                    if (FGWRT == "Y")
                    {
                        CNTOF();
                    }
                    //break;
                }
            }
        }
        public void GN_PL(EB_Service.DAL.DataCenter dataCenter)
        {
            ILDataCenter ilobj = new ILDataCenter();
            sql = $@"SELECT P1APRJ PLP1APRJ, P1LOCA PLP1LOCA, P1RSTS PLP1RSTS, P1CSID PLP1CSID, P1AVDT PLP1AVDT
                    FROM AS400DB01.PLOD0001.PLMS01 WITH (NOLOCK)
                    WHERE P1CSID = '{TMIDN}' ";
            DataSet DS_PL = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
            if (ilobj.check_dataset(DS_PL))
            {
                foreach (DataRow dr in DS_PL.Tables[0].Rows)
                {
                    FGWRT = "N";
                    if (dr["PLP1APRJ"].ToString() == "R" || dr["PLP1APRJ"].ToString() == "C" ||
                    (dr["PLP1APRJ"].ToString() == "A" && Convert.ToInt32(dr["PLP1LOCA"].ToString()) >= 250 &&
                    dr["PLP1RSTS"].ToString() != "X"))
                    {
                        TMROC = dr["PLP1APRJ"].ToString();
                        TMBIZ = "PL";
                        #region EXSR      @WRTTM    
                        WRTTM(dataCenter);
                        #endregion
                        FGWRT = "Y";
                    }
                    TM2AVD = dr["PLP1AVDT"].ToString();
                    if (Convert.ToInt32(TM2AVD) < 25000000 && Convert.ToInt32(TM2AVD) != 0)
                    {
                        TM2AVD = Convert.ToString(Convert.ToInt32(TM2AVD) + 25000000);
                    }
                    //break;
                }
            }
        }
        public void GN_PM(EB_Service.DAL.DataCenter dataCenter)
        {
            ILDataCenter ilobj = new ILDataCenter();
            sql = $@"SELECT P1APRJ PMP1APRJ, P1LOCA PMP1LOCA, P1RSTS PMP1RSTS, P1CSID PMP1CSID, P1AVDT PMP1AVDT
                    FROM AS400DB01.PMOD0001.PMMS01 WITH (NOLOCK)
                    WHERE P1CSID = '{TMIDN}' ";
            DataSet DS_PM = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
            if (ilobj.check_dataset(DS_PM))
            {
                foreach (DataRow dr in DS_PM.Tables[0].Rows)
                {
                    FGWRT = "N";
                    if (dr["PMP1APRJ"].ToString() == "R" || dr["PMP1APRJ"].ToString() == "C" ||
                    (dr["PMP1APRJ"].ToString() == "A" && Convert.ToInt32(dr["PMP1LOCA"].ToString()) >= 250 &&
                    dr["PMP1RSTS"].ToString() != "X"))
                    {
                        TMROC = dr["PMP1APRJ"].ToString();
                        TMBIZ = "PM";
                        #region EXSR      @WRTTM    
                        WRTTM(dataCenter);
                        #endregion
                        FGWRT = "Y";
                    }
                    TM2AVD = dr["PMP1AVDT"].ToString();
                    if (Convert.ToInt32(TM2AVD) < 25000000 && Convert.ToInt32(TM2AVD) != 0)
                    {
                        TM2AVD = Convert.ToString(Convert.ToInt32(TM2AVD) + 25000000);
                    }
                    //break;
                }
            }
        }
        public void GN_HP(EB_Service.DAL.DataCenter dataCenter)
        {
            ILDataCenter ilobj = new ILDataCenter();
            sql = $@"SELECT * 
                    FROM AS400DB01.HPOD0000.HPMS10 WITH (NOLOCK)
                    WHERE IDNO10 = '{TMIDN}'";
            DataSet DS_HP = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
            if (ilobj.check_dataset(DS_HP))
            {
                DataRow dr_hp10 = DS_HP.Tables[0].Rows.Count > 0 ? DS_HP.Tables[0].Rows[0] : null;
                sql = $@"SELECT ACAPPL, ACBRN
                                    FROM AS400DB01.SYOD0000.SYFAPCTL WITH (NOLOCK)";
                DataSet DS_SY = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
                if (ilobj.check_dataset(DS_SY))
                {
                    foreach (DataRow dr_sy in DS_SY.Tables[0].Rows)
                    {
                        if (dr_sy["ACAPPL"].ToString() == "HP")
                        {
                            FHPMS25 = string.Empty;
                            FHPMS30 = string.Empty;
                            try
                            {
                                GNMODE = WebConfigurationManager.AppSettings["Mode"].ToString().Trim();
                            }
                            catch
                            {
                                GNMODE = "O";
                            }
                            if (GNMODE == "T")
                            {
                                ACDTLB = "X";
                                if (dr_sy["ACBRN"].ToString() == "001")
                                {
                                    ACDTLB = "HPTEST";
                                }
                            }
                            FHPMS25 = ACDTLB.Trim() + "/HPMS25L9";
                            FHPMS30 = ACDTLB.Trim() + "/HPMS30";
                            sql = $@"SELECT [REASON], [STATUS], CONTNO, STATDATE
                                        FROM AS400DB01.HPOD0000.HPMS25 WITH (NOLOCK)
                                        WHERE CUSTC = '{dr_hp10["TCUSTC"].ToString()}'";
                            DataSet DS_HP25 = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
                            if (ilobj.check_dataset(DS_HP25))
                            {
                                foreach (DataRow dr_hp25 in DS_HP25.Tables[0].Rows)
                                {
                                    FGWRT = "N";
                                    TMROC = dr_hp25["REASON"].ToString().Substring(0, 1);
                                    if (TMROC == "L" || TMROC == "N" || TMROC == "O" || TMROC == "R" || TMROC == "G" || TMROC == "X" || TMROC == "Z")
                                    {
                                        TMROC = "R";
                                        TMBIZ = "HP";
                                        #region                    EXSR      @WRTTM     
                                        WRTTM(dataCenter);
                                        #endregion
                                        FGWRT = "Y";
                                    }
                                    if (TMROC == "S")
                                    {
                                        TMROC = "C";
                                        TMBIZ = "HP";
                                        #region                    EXSR      @WRTTM     
                                        WRTTM(dataCenter);
                                        #endregion
                                        FGWRT = "Y";
                                    }
                                    if (dr_hp25["STATUS"].ToString() == "A")
                                    {
                                        KBRN = !string.IsNullOrEmpty(dr_sy["ACBRN"].ToString()) ? dr_sy["ACBRN"].ToString() : "0";
                                        KCNT = !string.IsNullOrEmpty(dr_hp25["CONTNO"].ToString()) ? dr_hp25["CONTNO"].ToString() : "0";
                                        sql = $@"SELECT [DEL], [STATUS]
                                                    FROM AS400DB01.HPOD0000.HPMS30 WITH (NOLOCK)
                                                    WHERE BRANCH = '{KBRN.ToString()}' AND CONTNO = '{KCNT}'";
                                        DataSet DS_HP30 = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
                                        if (ilobj.check_dataset(DS_HP30))
                                        {
                                            DataRow dr_HP30 = DS_HP30.Tables[0].Rows.Count > 0 ? DS_HP30.Tables[0].Rows[0] : null;
                                            if ((dr_HP30["DEL"].ToString() != "D" || dr_HP30["STATUS"].ToString() != "TM") && dr_HP30["DEL"].ToString() != "X")
                                            {
                                                TMROC = "A";
                                                TMBIZ = "HP";
                                                #region                    EXSR      @WRTTM     
                                                WRTTM(dataCenter);
                                                #endregion
                                                FGWRT = "Y";
                                            }
                                        }
                                        else
                                        {
                                            ACDTLB = "HPTEST";
                                        }
                                    }
                                    TM2AVD = !string.IsNullOrEmpty(dr_hp25["STATDATE"].ToString()) ? dr_hp25["STATDATE"].ToString() : "0";
                                    if (Convert.ToInt32(TM2AVD) < 25000000 && Convert.ToInt32(TM2AVD) != 0)
                                    {
                                        TM2AVD = TM2AVD + 25000000;
                                    }
                                }
                            }
                            else
                            {
                                ACDTLB = "HPTEST";
                            }
                        }
                    }
                }
                //if (ilobj.check_dataset(DS_HP))
                //{
                //    sql = $@"SELECT ACAPPL
                //            FROM AS400DB01.SYOD0000.SYFAPCTL WITH (NOLOCK)";
                //    DataSet DS_SY = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
                //    if (ilobj.check_dataset(DS_SY))
                //    {
                //        foreach (DataRow dr_sy in DS_SY.Tables[0].Rows)
                //        {
                //            if (dr_sy["ACAPPL"].ToString() == "HP")
                //            {
                //                FHPMS25 = string.Empty;
                //                FHPMS30 = string.Empty;
                //                if (GNMODE == "T")
                //                {
                //                    ACDTLB = "X";
                //                    if (ACBRN == "001")
                //                    {
                //                        ACDTLB = "HPTEST";
                //                    }
                //                }
                //                FHPMS25 = ACDTLB.Trim() + "/HPMS25L9";
                //                FHPMS30 = ACDTLB.Trim() + "/HPMS30";
                //            }
                //        }
                //    }
                //}
            }
        }
        public void GN_HM(EB_Service.DAL.DataCenter dataCenter)
        {
            ILDataCenter ilobj = new ILDataCenter();
            sql = $@"SELECT P1APRJ HMP1APRJ, P1LOCA HMP1LOCA, P1STDT HMP1STDT, P1CSID HMP1CSID, P1AVDT HMP1AVDT
                    FROM AS400DB01.HMOD0001.HMMS01 WITH (NOLOCK)
                    WHERE P1CSID = '{TMIDN}' ";
            DataSet DS_HM = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
            if (ilobj.check_dataset(DS_HM))
            {
                foreach (DataRow dr in DS_HM.Tables[0].Rows)
                {
                    FGWRT = "N";
                    if (dr["HMP1APRJ"].ToString() == "R" || dr["HMP1APRJ"].ToString() == "C" ||
                    (dr["HMP1APRJ"].ToString() == "A" && Convert.ToInt32(dr["HMP1LOCA"].ToString()) >= 275 &&
                    Convert.ToInt32(dr["HMP1LOCA"].ToString()) >= 301))
                    {
                        TMROC = dr["HMP1APRJ"].ToString();
                        if (dr["HMP1APRJ"].ToString() == "A" && Convert.ToInt32(dr["HMP1LOCA"].ToString()) == 300)
                        {
                            TMROC = "C";
                        }
                        TMBIZ = "HM";
                        #region EXSR      @WRTTM    
                        WRTTM(dataCenter);
                        #endregion
                        FGWRT = "Y";
                    }
                    TM2AVD = dr["HMP1STDT"].ToString();
                    if (Convert.ToInt32(TM2AVD) == 0)
                    {
                        TM2AVD = dr["HMP1AVDT"].ToString();
                    }
                    if (Convert.ToInt32(TM2AVD) < 25000000 && Convert.ToInt32(TM2AVD) != 0)
                    {
                        TM2AVD = Convert.ToString(Convert.ToInt32(TM2AVD) + 25000000);
                    }
                    //break;
                }
            }
        }
        public void GN_PH(EB_Service.DAL.DataCenter dataCenter)
        {
            ILDataCenter ilobj = new ILDataCenter();
            sql = $@"SELECT P1APRJ PHP1APRJ, P1LOCA PHP1LOCA, P1STDT PHP1STDT, P1CSID PHP1CSID, P1AVDT PHP1AVDT
                    FROM AS400DB01.PHOD0001.PHMS01 WITH (NOLOCK)
                    WHERE P1CSID = '{TMIDN}' ";
            DataSet DS_PH = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
            if (ilobj.check_dataset(DS_PH))
            {
                foreach (DataRow dr in DS_PH.Tables[0].Rows)
                {
                    FGWRT = "N";
                    if (dr["PHP1APRJ"].ToString() == "R" || dr["PHP1APRJ"].ToString() == "C" ||
                    (dr["PHP1APRJ"].ToString() == "A" && Convert.ToInt32(dr["PHP1LOCA"].ToString()) >= 275 &&
                    Convert.ToInt32(dr["PHP1LOCA"].ToString()) >= 301))
                    {
                        TMROC = dr["PHP1APRJ"].ToString();
                        if (dr["PHP1APRJ"].ToString() == "A" && Convert.ToInt32(dr["PHP1LOCA"].ToString()) == 300)
                        {
                            TMROC = "C";
                        }
                        TMBIZ = "PH";
                        #region EXSR      @WRTTM    
                        WRTTM(dataCenter);
                        #endregion
                        FGWRT = "Y";
                    }
                    TM2AVD = dr["PHP1STDT"].ToString();
                    if (Convert.ToInt32(TM2AVD) == 0)
                    {
                        TM2AVD = dr["PHP1AVDT"].ToString();
                    }
                    if (Convert.ToInt32(TM2AVD) < 25000000 && Convert.ToInt32(TM2AVD) != 0)
                    {
                        TM2AVD = Convert.ToString(Convert.ToInt32(TM2AVD) + 25000000);
                    }
                    //break;
                }
            }
        }
        private void WRTTM(EB_Service.DAL.DataCenter dataCenter)
        {
            ILDataCenter ilobj = new ILDataCenter();
            T001 = string.Empty;
            switch (TMTYP)
            {
                case "H":
                    TM1GRP = "1";
                    TM1TNO = WKTEL;
                    break;
                case "M":
                    TM1GRP = "2";
                    TM1TNO = WKTEL;
                    break;
                case "O":
                    TM1GRP = "3";
                    TM1TNO = WKTEL;
                    break;
                case "S":
                    TM1GRP = "4";
                    TM1TNO = WKTEL;
                    break;
                case "F":
                    TM1GRP = "5";
                    TM1TNO = WKTEL;
                    break;
            }
            TM1BIZ = TMBIZ;
            switch (TMBIZ)
            {
                case "HP":
                    TM1SOR = "1";
                    break;
                case "PL":
                    TM1SOR = "2";
                    break;
                case "PH":
                    TM1SOR = "3";
                    break;
                case "HM":
                    TM1SOR = "4";
                    break;
                case "PM":
                    TM1SOR = "5";
                    break;
                case "PW":
                    TM1SOR = "6";
                    break;
                case "IL":
                    TM1SOR = "7";
                    break;
                case "RL":
                    TM1SOR = "8";
                    break;
            }
            //sql = $@"SELECT T01RRNO, [INDEX], [DATA] 
            //        FROM AS400DB01.GNOD0000.GNTM01 WITH (NOLOCK)
            //        WHERE [INDEX] = '' ";
            //DataSet DS_GNTM01 = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
            //if (ilobj.check_dataset(DS_GNTM01))
            //{
            //    switch (TMROC)
            //    {
            //        case "R":
            //            TM1REJ++;
            //            break;
            //        case "C":
            //            TM1CAN++;
            //            break;
            //        case "A":
            //            TM1APV++;
            //            break;
            //    }
            //    //Update GNTM01
            //}
            //else
            //{
            //    //INSERT GNTM01
            //}
            switch (TMROC)
            {
                case "R":
                    TM1REJ++;
                    break;
                case "C":
                    TM1CAN++;
                    break;
                case "A":
                    TM1APV++;
                    break;
            }
        }
        private void CNTOF()
        {
            if ((TMTYP == "O" || TMTYP == "F") && Convert.ToInt32(TM2AVD) > Convert.ToInt32(CHKDTE))
            {
                switch (TMTYP)
                {
                    case "O":
                        TMDPTO++;
                        break;
                    case "F":
                        TMDPOF++;
                        break;
                }
            }
        }
        private void MOVDT(EB_Service.DAL.DataCenter dataCenter)
        {
            //ILDataCenter ilobj = new ILDataCenter();
            //sql = $@"SELECT T01RRNO, [INDEX], [DATA] 
            //        FROM AS400DB01.GNOD0000.GNTM01 WITH (NOLOCK)";
            //DataSet DS_GNTM01 = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
            //if (ilobj.check_dataset(DS_GNTM01))
            //{
            //    foreach (DataRow dr in DS_GNTM01.Tables[0].Rows)
            //    {
            //        if (RC < 9999)
            //        {
            //            #region  EXSR      @MOVDS 
            //            MOVDS();
            //            #endregion
            //        }
            //    }
            //}
            if (RC < 9999)
            {
                #region  EXSR      @MOVDS 
                MOVDS();
                #endregion
            }
        }
        private void MOVDS()
        {
            ILDataCenter ilobj = new ILDataCenter();
            RC++;
            RESULTDS = new string[RC];
            if (!ilobj.check_dataset(dataSet))
            {
                dataSet = new DataSet("Inquiry");

                // สร้าง DataTable
                DataTable dataTable = new DataTable("Table1");

                // เพิ่มคอลัมน์ลงใน DataTable
                DataColumn column1 = new DataColumn("GROUP", typeof(string));
                dataTable.Columns.Add(column1);
                DataColumn column2 = new DataColumn("DESC", typeof(string));
                dataTable.Columns.Add(column2);
                DataColumn column3 = new DataColumn("BUS", typeof(string));
                dataTable.Columns.Add(column3);
                DataColumn column4 = new DataColumn("APPROVE", typeof(string));
                dataTable.Columns.Add(column4);
                DataColumn column5 = new DataColumn("REJECT", typeof(string));
                dataTable.Columns.Add(column5);
                DataColumn column6 = new DataColumn("CANCEL", typeof(string));
                dataTable.Columns.Add(column6);
                DataColumn column7 = new DataColumn("TOTAL", typeof(string));
                dataTable.Columns.Add(column7);
                // เพิ่ม DataTable ลงใน DataSet
                dataSet.Tables.Add(dataTable);
            }
            //dataSet.Tables[0].Columns.Add("GROUP");
            //dataSet.Tables[0].Columns.Add("DESC");
            //dataSet.Tables[0].Columns.Add("BUS");
            //dataSet.Tables[0].Columns.Add("APPROVE");
            //dataSet.Tables[0].Columns.Add("REJECT");
            //dataSet.Tables[0].Columns.Add("CANCEL");
            //dataSet.Tables[0].Columns.Add("TOTAL");

            switch (Convert.ToString(TM1GRP))
            {
                case "1":
                    Group = "TH";
                    Desc = TM1TNO.Trim();
                    break;
                case "2":
                    Group = "TM";
                    Desc = TM1TNO.Trim();
                    break;
                case "3":
                    Group = "TO";
                    Desc = TM1TNO.Trim();
                    break;
                case "4":
                    Group = "SUR";
                    Desc = WKSNME.Trim();
                    break;
                case "5":
                    Group = "OF";
                    Desc = WKSNME.Trim();
                    break;
            }
            Biz = TM1BIZ;
            Reject = Convert.ToInt32(TM1REJ);
            Cancel = Convert.ToInt32(TM1CAN);
            Approve = Convert.ToInt32(TM1APV);
            Total = Convert.ToInt32(TM1REJ) + Convert.ToInt32(TM1CAN) + Convert.ToInt32(TM1APV);
            //Total = Total + TM1APV;
            TTGRP = TM1GRP;
            TTTNO = TM1TNO;
            dataSet.Tables[0].Rows.Add(Group, Desc, Biz, Approve.ToString(), Reject.ToString(), Cancel.ToString(), Total.ToString());
            //dataSet.Tables[0].Rows.Add(Approve);
            //dataSet.Tables[0].Rows.Add(Reject);
            //dataSet.Tables[0].Rows.Add(Cancel);
            //dataSet.Tables[0].Rows.Add(Total);
        }
        public void RESULT()
        {
            return;
        }
    }
    #endregion

}

