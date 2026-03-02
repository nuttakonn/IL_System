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
    public class ILE0221 : UserInfo
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
        private DataSet dataSetResult;

        public ILE0221(UserInfo userInfo)
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

        public int WDATE;
        public int WKYY;
        public int WKMM;
        public string FSW1;
        public int COFFNM;
        public int CTELOF;
        public int RC;
        public string TEL_HO;
        public string TEL_OF;
        public string TEL_MO;
        public string TEL10;
        public string SURN;
        public string TYPETL;
        public string TELNO;
        public string OFFNME;
        public string TKTEL;
        public string CKTYPE;
        public string WKCSN;
        public string KCSN;
        public string KREF;
        public int KRSQ;
        public int A;
        public int TMPOD1U;
        public string WKTYPE;
        public string WKTELNO;
        public string L1STS;
        public string WKAPTY;
        public string L1CLO;
        public string L1ODT;
        public string L1WOAMT;
        public string L1ERR;
        public int WBRN;
        public string TMPBUS;
        public long WCNT;
        public int APPDT;
        public int APPYY;
        public int APPMM;
        public string WODT;
        public string WKAPPL;
        public string WKCONT;
        public string WKCKDT;

        public class T1AREA
        {
            public string T1APTY { get; set; }
            public int T1TOTC { get; set; }
            public int T1CURC { get; set; }
            public int T1NORC { get; set; }
            public int T1OD1C { get; set; }
            public int T1OD1U { get; set; }
            public int T1WOCS { get; set; }
            public int T1LOCS { get; set; }
            public int T1TOTA { get; set; }
            public int T1TOTCS { get; set; }
            public int T1FRAUD { get; set; }
            public string T1GROUP { get; set; }
            public string T1DESC { get; set; }
        }
        public List<T1AREA> lst_T1AREA = Enumerable.Repeat(new T1AREA(), 50).ToList();
        public string FHPMS30;
        public string GNMODE;
        public string ACDTLB;
        public DataTable dataTable;


        public DataSet Call_ILE0221(EB_Service.DAL.DataCenter dataCenter, string IDNO)
        {
            try
            {
                ILDataCenter ilobj = new ILDataCenter();
                ILDataSubroutine dataSubroutine = new ILDataSubroutine(m_UserInfo);
                dataSetResult = new DataSet();
                dataTable = new DataTable("Tables1");
                dataTable.Columns.Add("GROUP", typeof(string));
                dataTable.Columns.Add("DESC", typeof(string));
                dataTable.Columns.Add("BUS", typeof(string));
                dataTable.Columns.Add("ACT", typeof(int));
                dataTable.Columns.Add("CUR", typeof(int));
                dataTable.Columns.Add("NOR", typeof(int));
                dataTable.Columns.Add("OD1", typeof(int));
                dataTable.Columns.Add("OD1UP", typeof(int));
                dataTable.Columns.Add("WO", typeof(int));
                dataTable.Columns.Add("LC", typeof(int));
                dataTable.Columns.Add("TOT", typeof(int));
                dataTable.Columns.Add("CLS", typeof(int));
                dataTable.Columns.Add("FRD", typeof(int));
                dataSetResult.Tables.Add(dataTable);
                #region หา Date ILMS97
                string sql = "SELECT P97CDT FROM AS400DB01.ILOD0001.ILMS97 WITH (NOLOCK) WHERE P97REC = '01'";
                DataSet DS_ILMS97 = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
                if (ilobj.check_dataset(DS_ILMS97))
                {
                    DataRow dr_ILMS97 = DS_ILMS97.Tables[0].Rows.Count > 0 ? DS_ILMS97.Tables[0].Rows[0] : null;
                    WDATE = Convert.ToInt32(!string.IsNullOrEmpty(dr_ILMS97["P97CDT"].ToString()) ? dr_ILMS97["P97CDT"].ToString() : "0");
                    WKYY = Convert.ToInt32(Convert.ToString(WDATE).Substring(0, 4));
                    WKMM = Convert.ToInt32(Convert.ToString(WDATE).Substring(4, 2));
                }
                #endregion
                FSW1 = "0";
                #region                   EXSR      @Tel_Sur  
                COFFNM = 0;
                CTELOF = 0;
                RC = 0;
                TEL_HO = "000000000";
                TEL_OF = "000000000";
                TEL_MO = "0000000000";
                TEL10 = string.Empty;
                SURN = string.Empty;
                OFFNME = string.Empty;
                #region **  Tel. NO.  
                TKTEL = string.Empty;
                sql = $@"SELECT CISNumber M00CSN, IDCard M00IDN
                        FROM CustomerDB01.CustomerInfo.CustomerGeneral WITH (NOLOCK)
                        WHERE IDCard = '{IDNO}'";
                DataSet DS_CSMS00 = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
                if (ilobj.check_dataset(DS_CSMS00))
                {
                    DataRow dr_CSMS00 = DS_CSMS00.Tables[0].Rows.Count > 0 ? DS_CSMS00.Tables[0].Rows[0] : null;
                    WKCSN = dr_CSMS00["M00CSN"].ToString();
                    KCSN = WKCSN;
                    KREF = string.Empty;
                    KRSQ = 0;
                    sql = $@"SELECT gc.Code M11CDE, TelephoneNumber1 M11TEL, Mobile M11MOB 
                            FROM CustomerDB01.CustomerInfo.CustomerAddress ca WITH (NOLOCK)
                            INNER JOIN CustomerDB01.CustomerInfo.CustomerGeneral cg WITH (NOLOCK) ON (ca.CustID = cg.ID)
                            INNER JOIN GeneralDB01.GeneralInfo.GeneralCenter gc WITH (NOLOCK) ON (ca.AddressCodeID = gc.ID AND TYPE = 'AddressCodeID')
                            WHERE cg.CISNumber = '{KCSN}' AND ca.CustRefID = 0";
                    DataSet DS_CSMS11 = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
                    if (ilobj.check_dataset(DS_CSMS11))
                    {
                        foreach(DataRow dr_11 in DS_CSMS11.Tables[0].Rows)
                        {
                            TKTEL = string.Empty;
                            CKTYPE = string.Empty;
                            switch (dr_11["M11CDE"].ToString())
                            {
                                case "H":
                                    if (!string.IsNullOrWhiteSpace(dr_11["M11TEL"].ToString()) && dr_11["M11TEL"].ToString() != "**" && dr_11["M11TEL"].ToString() != "000000000")
                                    {
                                        TKTEL = dr_11["M11TEL"].ToString().Substring(0, Math.Min(9, dr_11["M11TEL"].ToString().Length));
                                        CKTYPE = "TH";
                                        #region EXSR      @GNTEL 
                                        GNTEL(dataCenter);
                                        #endregion
                                        #region EXSR      @MOVDS 
                                        MOVDS();
                                        #endregion
                                    }
                                    if (!string.IsNullOrWhiteSpace(dr_11["M11MOB"].ToString()) && dr_11["M11MOB"].ToString() != "**" && dr_11["M11MOB"].ToString() != "000000000")
                                    {
                                        TKTEL = dr_11["M11MOB"].ToString().Substring(0, Math.Min(10, dr_11["M11MOB"].ToString().Length));
                                        CKTYPE = "TM";
                                        #region EXSR      @GNTEL  
                                        GNTEL(dataCenter);
                                        #endregion
                                        #region EXSR      @MOVDS   
                                        MOVDS();
                                        #endregion
                                    }
                                    break;
                                case "O":
                                    if (!string.IsNullOrWhiteSpace(dr_11["M11TEL"].ToString()) && dr_11["M11TEL"].ToString() != "**" && dr_11["M11TEL"].ToString() != "000000000")
                                    {
                                        TKTEL = dr_11["M11TEL"].ToString().Substring(0, Math.Min(9, dr_11["M11TEL"].ToString().Length));
                                        CKTYPE = "TO";
                                        #region EXSR      @GNTEL  
                                        GNTEL(dataCenter);
                                        #endregion
                                        #region EXSR      @MOVDS   
                                        MOVDS();
                                        #endregion
                                    }
                                    break;
                            }
                        }
                    }

                }
                #endregion
                #region  **    Surname
                sql = $@"SELECT SurnameInTHAI M00TSN
                        FROM CustomerDB01.CustomerInfo.CustomerGeneral WITH (NOLOCK)
                        WHERE IDCard = '{IDNO}'";
                DataSet DS_CSMS00_SUR = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
                if (ilobj.check_dataset(DS_CSMS00_SUR))
                {
                    DataRow dr_SUR = DS_CSMS00_SUR.Tables[0].Rows.Count > 0 ? DS_CSMS00_SUR.Tables[0].Rows[0] : null;
                    SURN = dr_SUR["M00TSN"].ToString();
                    TYPETL = "SUR";
                    TELNO = dr_SUR["M00TSN"].ToString();
                    #region EXSR      @FindID2 
                    FindID2(dataCenter);
                    #endregion
                    #region EXSR      @MOVDS  
                    MOVDS();
                    #endregion
                }
                #endregion
                #region    **    Office Name 
                sql = $@"SELECT OfficeName M00OFC
                        JOIN CustomerDB01.CustomerInfo.CustomerWorked cw WITH (NOLOCK)  
                        INNER JOIN CustomerDB01.CustomerInfo.CustomerGeneral cg WITH (NOLOCK) ON (cw.CustID = cg.ID)
                        WHERE cg.CISNumber = '{KCSN}'";
                DataSet DS_CSMS00_OFF = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
                if (ilobj.check_dataset(DS_CSMS00_OFF))
                {
                    DataRow dr_OFF = DS_CSMS00_OFF.Tables[0].Rows.Count > 0 ? DS_CSMS00_OFF.Tables[0].Rows[0] : null;
                    if (!string.IsNullOrEmpty(dr_OFF["M00OFC"].ToString()))
                    {
                        OFFNME = dr_OFF["M00OFC"].ToString();
                        TYPETL = "OF";
                        TELNO = dr_OFF["M00OFC"].ToString();
                        #region EXSR      @FindID3 
                        FindID3(dataCenter);
                        #endregion
                        #region EXSR      @MOVDS 
                        MOVDS();
                        #endregion
                    }

                }
                #endregion
                #endregion

                return dataSetResult;
            }
            catch(Exception ex)
            {
                Utility.WriteLog(ex);
                return dataSetResult;
            }
        }
        public void GNTEL(EB_Service.DAL.DataCenter dataCenter)
        {
            ILDataCenter ilobj = new ILDataCenter();
            A = 1;
            lst_T1AREA = Enumerable.Repeat(new T1AREA(), 50).ToList();
            lst_T1AREA[A].T1APTY = string.Empty;
            string sql = $@"SELECT gca.Code M12ACD, gct.Code M12TTY, CustRefID M12REF, cg.CISnumber M12CSN, TelephoneNumber M12TEL
                        FROM CustomerDB01.CustomerInfo.CustomerTelephone ct WITH (NOLOCK)
                        INNER JOIN CustomerDB01.CustomerInfo.CustomerGeneral cg WITH (NOLOCK) ON (ct.CustID = cg.ID AND CustRefID = 0)
                        INNER JOIN GeneralDB01.GeneralInfo.GeneralCenter gca WITH (NOLOCK) ON (ct.AddressCodeID = gca.ID AND gca.TYPE = 'AddressCodeID')
                        INNER JOIN GeneralDB01.GeneralInfo.GeneralCenter gct WITH (NOLOCK) ON (ct.TelephoneTypeID = gct.ID AND gct.TYPE = 'TelephoneTypeID')
                        WHERE TelephoneNumber = '{TKTEL}'";
            DataSet DS_CSMS12 = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
            if (ilobj.check_dataset(DS_CSMS12))
            {
                foreach(DataRow dr_CSMS12 in DS_CSMS12.Tables[0].Rows)
                {
                    //if(Convert.ToInt32(dr_CSMS12["M12REF"].ToString()) != 0)
                    //{
                    //    //GOTO      NXT#T
                    //    continue;
                    //}
                    if(dr_CSMS12["M12ACD"].ToString() == "H" && dr_CSMS12["M12TTY"].ToString() == "P")
                    {
                        TYPETL = "TH";
                    }
                    else if (dr_CSMS12["M12ACD"].ToString() == "H" && dr_CSMS12["M12TTY"].ToString() == "M")
                    {
                        TYPETL = "TM";
                    }
                    else if (dr_CSMS12["M12ACD"].ToString() == "O" && dr_CSMS12["M12TTY"].ToString() == "P")
                    {
                        TYPETL = "TO";
                    }
                    else
                    {
                        //GOTO      NXT#T
                        continue;
                    }
                    if(CKTYPE == TYPETL)
                    {
                        WKTYPE = TYPETL;
                        WKTELNO = dr_CSMS12["M12TEL"].ToString();
                        #region                    EXSR      @FindID
                        if(dr_CSMS12["M12TEL"].ToString() == "999999999")
                        {
                            continue;
                        }
                        sql = $@"SELECT CISnumber M00CSN, IDCard M00IDN
                            FROM CustomerDB01.CustomerInfo.CustomerGeneral cg WITH (NOLOCK)
                            WHERE CISNumber = {dr_CSMS12["M12CSN"].ToString()} ";
                        DataSet DS_CSMS00 = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
                        if (ilobj.check_dataset(DS_CSMS00))
                        {
                            DataRow dr_CSMS00= DS_CSMS00.Tables[0].Rows.Count > 0 ? DS_CSMS00.Tables[0].Rows[0] : null;
                            #region    EXSR      @R_RL
                            R_RL(dataCenter, dr_CSMS00);
                            #endregion
                            #region    EXSR      @R_IL
                            R_IL(dataCenter, dr_CSMS00);
                            #endregion
                            #region    EXSR      @R_PW
                            R_PW(dataCenter, dr_CSMS00);
                            #endregion
                            #region    EXSR      @R_HM
                            R_HM(dataCenter, dr_CSMS00);
                            #endregion
                            #region    EXSR      @R_HP
                            R_HP(dataCenter, dr_CSMS00);
                            #endregion
                            #region    EXSR      @R_PM
                            R_PM(dataCenter, dr_CSMS00);
                            #endregion
                            #region    EXSR      @R_PL
                            R_PL(dataCenter, dr_CSMS00);
                            #endregion
                            #region    EXSR      @R_PH
                            R_PH(dataCenter, dr_CSMS00);
                            #endregion
                        }
                        #endregion
                    }
                }
            }
        }
        public void R_RL(EB_Service.DAL.DataCenter dataCenter, DataRow dr_CSMS00)
        {
            ILDataCenter ilobj = new ILDataCenter();
            L1STS = string.Empty;
            string sql = $@"SELECT P2DEL, P2BRN, P2CONT, P2APDT, P2ODTM
                                    FROM AS400DB01.RLOD0001.RLMS02 WITH (NOLOCK)
                                    WHERE P2CSNO = {dr_CSMS00["M00CSN"].ToString()} ";
            DataSet DS_RLMS02 = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
            if (ilobj.check_dataset(DS_RLMS02))
            {
                foreach (DataRow dr_RLMS02 in DS_RLMS02.Tables[0].Rows)
                {
                    if (dr_RLMS02["P2DEL"].ToString() != "X")
                    {
                        WKAPTY = string.Empty;
                        L1CLO = "000000000";
                        L1ODT = "00";
                        L1WOAMT = "000000000";
                        L1ERR = string.Empty;
                        WBRN = Convert.ToInt32(dr_RLMS02["P2BRN"].ToString());
                        TMPBUS = "RL";
                        WCNT = Convert.ToInt64(!string.IsNullOrEmpty(dr_RLMS02["P2CONT"].ToString()) ? dr_RLMS02["P2CONT"].ToString() : "0");
                        APPDT = Convert.ToInt32(!string.IsNullOrEmpty(dr_RLMS02["P2APDT"].ToString()) ? dr_RLMS02["P2APDT"].ToString() : "0");
                        APPYY = APPDT > 0 ? Convert.ToInt32(APPDT.ToString().Substring(0, 4)) : 0;
                        APPMM = APPDT > 0 ? Convert.ToInt32(APPDT.ToString().Substring(4, 2)) : 0;

                        if (dr_RLMS02["P2DEL"].ToString() == "C" || dr_RLMS02["P2DEL"].ToString() == "N")
                        {
                            L1STS = "CL";
                        }
                        else if (string.IsNullOrEmpty(dr_RLMS02["P2DEL"].ToString()))
                        {
                            L1STS = "AC";
                        }
                        else if (dr_RLMS02["P2DEL"].ToString() == "W")
                        {
                            L1STS = "WO";
                        }
                        WODT = dr_RLMS02["P2ODTM"].ToString();
                        switch (TYPETL)
                        {
                            case "OF":
                                if (WKYY == APPYY && WKMM == APPMM && dr_RLMS02["P2DEL"].ToString() != "T")
                                {
                                    COFFNM = 1;
                                }
                                break;
                            case "TO":
                                if (WKYY == APPYY && WKMM == APPMM && dr_RLMS02["P2DEL"].ToString() != "T")
                                {
                                    CTELOF = 1;
                                }
                                break;
                        }
                        #region                    EXSR      @MVSFL  
                        MVSFL(dataCenter);
                        #endregion
                    }
                }
            }
        }
        public void R_IL(EB_Service.DAL.DataCenter dataCenter, DataRow dr_CSMS00)
        {
            ILDataCenter ilobj = new ILDataCenter();
            string sql = $@"SELECT P2DEL, P2BRN, P2CONT, P2ACDT, P2ODTM, P2LOCA
                            FROM AS400DB01.ILOD0001.ILMS02 WITH (NOLOCK)
                            WHERE P2CSNO = {dr_CSMS00["M00CSN"]} ";
            DataSet DS_ILMS02 = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
            if (ilobj.check_dataset(DS_ILMS02))
            {
                foreach(DataRow dr_ILMS02 in DS_ILMS02.Tables[0].Rows)
                {
                    if(dr_ILMS02["P2DEL"].ToString() != "X")
                    {
                        WKAPTY = string.Empty;
                        L1STS = string.Empty;
                        L1CLO = "000000000";
                        L1ODT = "00";
                        L1WOAMT = "000000000";
                        L1ERR = string.Empty;
                        WBRN = Convert.ToInt32(!string.IsNullOrEmpty(dr_ILMS02["P2BRN"].ToString()) ? dr_ILMS02["P2BRN"].ToString() : "0");
                        TMPBUS = "IL";
                        WCNT = Convert.ToInt64(!string.IsNullOrEmpty(dr_ILMS02["P2CONT"].ToString()) ? dr_ILMS02["P2CONT"].ToString() : "0");
                        if (dr_ILMS02["P2DEL"].ToString() == "C" || dr_ILMS02["P2DEL"].ToString() == "N")
                        {
                            L1STS = "CL";
                        }
                        else if (dr_ILMS02["P2DEL"].ToString() == "L")
                        {
                            L1STS = "LC";
                        }
                        else if (string.IsNullOrEmpty(dr_ILMS02["P2DEL"].ToString()))
                        {
                            if(dr_ILMS02["P2LOCA"].ToString() == "275")
                            {
                                L1STS = "AC";
                            }
                            else
                            {
                                L1STS = "US";
                            }
                        }
                        else if (dr_ILMS02["P2DEL"].ToString() == "W")
                        {
                            L1STS = "WO";
                        }
                        WODT = dr_ILMS02["P2ODTM"].ToString();
                        #region                    EXSR      @MVSFL              
                        MVSFL(dataCenter);
                        #endregion
                    }
                }
            }
        }
        public void R_PW(EB_Service.DAL.DataCenter dataCenter, DataRow dr_CSMS00)
        {
            ILDataCenter ilobj = new ILDataCenter();
            string sql = $@"SELECT P2DEL, P2BRN, P2CONT, P2ODTM
                            FROM AS400DB01.PWOD0001.PWMS02 WITH (NOLOCK)
                            WHERE P2IDNO = '{dr_CSMS00["M00IDN"].ToString()}' ";
            DataSet DS_PWMS02 = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
            if (ilobj.check_dataset(DS_PWMS02))
            {
                foreach(DataRow dr_PWMS02 in DS_PWMS02.Tables[0].Rows)
                {
                    if (dr_PWMS02["P2DEL"].ToString() != "X")
                    {
                        WKAPTY = string.Empty;
                        L1STS = string.Empty;
                        L1CLO = "000000000";
                        L1ODT = "00";
                        L1WOAMT = "000000000";
                        L1ERR = string.Empty;
                        WBRN = Convert.ToInt32(!string.IsNullOrEmpty(dr_PWMS02["P2BRN"].ToString()) ? dr_PWMS02["P2BRN"].ToString() : "0");
                        TMPBUS = "PW";
                        WCNT = Convert.ToInt64(!string.IsNullOrEmpty(dr_PWMS02["P2CONT"].ToString()) ? dr_PWMS02["P2CONT"].ToString() : "0");
                        if (dr_PWMS02["P2DEL"].ToString() == "C" || dr_PWMS02["P2DEL"].ToString() == "N")
                        {
                            L1STS = "CL";
                        }
                        else if (string.IsNullOrEmpty(dr_PWMS02["P2DEL"].ToString()))
                        {
                            L1STS = "LC";
                        }
                        else if (dr_PWMS02["P2DEL"].ToString() == "W")
                        {
                            L1STS = "WO";
                        }
                        WODT = dr_PWMS02["P2ODTM"].ToString();
                        #region                   EXSR      @MVSFL            
                        MVSFL(dataCenter);
                        #endregion
                    }
                }
            }
        }
        public void R_HM(EB_Service.DAL.DataCenter dataCenter, DataRow dr_CSMS00)
        {
            ILDataCenter ilobj = new ILDataCenter();
            string sql = $@"SELECT P2DEL, P2BRN, P2CONT, P2LOCA
                            FROM AS400DB01.HMOD0001.HMMS02 WITH (NOLOCK)
                            WHERE P2IDNO = '{dr_CSMS00["M00IDN"].ToString()}' ";
            DataSet DS_HMMS02 = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
            if (ilobj.check_dataset(DS_HMMS02))
            {
                foreach (DataRow dr_HMMS02 in DS_HMMS02.Tables[0].Rows)
                {
                    if (dr_HMMS02["P2DEL"].ToString() != "X")
                    {
                        WKAPTY = string.Empty;
                        L1STS = string.Empty;
                        L1CLO = "000000000";
                        L1ODT = "00";
                        L1WOAMT = "000000000";
                        L1ERR = string.Empty;
                        WBRN = Convert.ToInt32(!string.IsNullOrEmpty(dr_HMMS02["P2BRN"].ToString()) ? dr_HMMS02["P2BRN"].ToString() : "0");
                        TMPBUS = "HM";
                        WCNT = Convert.ToInt64(!string.IsNullOrEmpty(dr_HMMS02["P2CONT"].ToString()) ? dr_HMMS02["P2CONT"].ToString() : "0");
                        if (dr_HMMS02["P2DEL"].ToString() == "C" || dr_HMMS02["P2DEL"].ToString() == "N")
                        {
                            L1STS = "CL";
                        }
                        else if (dr_HMMS02["P2DEL"].ToString() == "L")
                        {
                            L1STS = "LC";
                        }
                        else if (string.IsNullOrEmpty(dr_HMMS02["P2DEL"].ToString()))
                        {
                            if(dr_HMMS02["P2LOCA"].ToString() == "275")
                            {
                                L1STS = "AC";
                            }
                            else
                            {
                                L1STS = "US";
                            }
                        }
                        else if (dr_HMMS02["P2DEL"].ToString() == "W")
                        {
                            L1STS = "WO";
                        }
                        #region                   EXSR      @MVSFL            
                        MVSFL1(dataCenter);
                        #endregion
                    }
                }
            }
        }
        public void R_HP(EB_Service.DAL.DataCenter dataCenter, DataRow dr_CSMS00)
        {
            ILDataCenter ilobj = new ILDataCenter();
            string sql = $@"SELECT CUSTC
                        FROM AS400DB01.HPOD0000.HPMS10 WITH (NOLOCK)
                        WHERE IDNO10 = '{dr_CSMS00["M00IDN"].ToString()}'";
            DataSet DS_HPMS10 = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
            if (!ilobj.check_dataset(DS_HPMS10))
            {
                return;
            }
            DataRow dr_HPMS10 = DS_HPMS10.Tables[0].Rows.Count > 0 ? DS_HPMS10.Tables[0].Rows[0] : null;
            sql = $@"SELECT ACAPPL, ACDTLB, ACBRN
                    FROM AS400DB01.SYOD0000.SYFAPCTL WITH (NOLOCK)";
            DataSet DS_SYFAPCTL = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
            if (ilobj.check_dataset(DS_SYFAPCTL))
            {
                foreach(DataRow dr_SYFAPCTL in DS_SYFAPCTL.Tables[0].Rows)
                {
                    if(dr_SYFAPCTL["ACAPPL"].ToString() == "HP")
                    {
                        FHPMS30 = string.Empty;
                        ACDTLB = dr_SYFAPCTL["ACDTLB"].ToString();
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
                            if(dr_SYFAPCTL["ACBRN"].ToString() == "001")
                            {
                                ACDTLB = "HPTEST";
                            }
                        }
                        FHPMS30 = ACDTLB.Trim() + "/HPMS30L1";
                        sql = $@"SELECT DEL, STATUS, BRANCH, CONTNO
                                FROM AS400DB01.HPOD0000.HPMS30 WITH (NOLOCK)
                                WHERE CUSTC = {dr_HPMS10["CUSTC"]} ";
                        DataSet DS_HPMS30 = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
                        if (!ilobj.check_dataset(DS_HPMS30))
                        {
                            if(ACDTLB == "HPTEST")
                            {
                                return;
                            }
                            continue;
                        }
                        foreach(DataRow dr_HPMS30 in DS_HPMS30.Tables[0].Rows)
                        {
                            if(dr_HPMS30["DEL"].ToString() == "D" && dr_HPMS30["STATUS"].ToString() == "TM")
                            {
                                continue;
                            }
                            else
                            {
                                WKAPTY = string.Empty;
                                L1STS = string.Empty;
                                L1CLO = "000000000";
                                L1ODT = "00";
                                L1WOAMT = "000000000";
                                L1ERR = string.Empty;
                                WBRN = Convert.ToInt32(!string.IsNullOrEmpty(dr_HPMS30["BRANCH"].ToString()) ? dr_HPMS30["BRANCH"].ToString() : "0");
                                TMPBUS = "HP";
                                WCNT = Convert.ToInt64(!string.IsNullOrEmpty(dr_HPMS30["CONTNO"].ToString()) ? dr_HPMS30["CONTNO"].ToString() : "0");
                                if (string.IsNullOrEmpty(dr_HPMS30["DEL"].ToString()))
                                {
                                    L1STS = "AC";
                                }
                                else if (dr_HPMS30["DEL"].ToString() == "D")
                                {
                                    if(dr_HPMS30["STATUS"].ToString() == "A" || dr_HPMS30["STATUS"].ToString() == "AE" || dr_HPMS30["STATUS"].ToString() == "CL")
                                    {
                                        L1STS = "CL";
                                    }
                                    else if(dr_HPMS30["STATUS"].ToString() == "BD" || dr_HPMS30["STATUS"].ToString() == "WO")
                                    {
                                        L1STS = "WO";
                                    }
                                }
                                else if(dr_HPMS30["STATUS"].ToString() == "U" || dr_HPMS30["STATUS"].ToString() == "US")
                                {
                                    if (string.IsNullOrEmpty(dr_HPMS30["DEL"].ToString()))
                                    {
                                        L1STS = "US";
                                    }
                                }
                                #region                   EXSR      @MVSFL1                            
                                MVSFL1(dataCenter);
                                #endregion
                            }
                        }
                    }
                }
            }
        }
        public void R_PM(EB_Service.DAL.DataCenter dataCenter, DataRow dr_CSMS00)
        {
            ILDataCenter ilobj = new ILDataCenter();
            string sql = $@"SELECT P2DEL, P2BRN, P2CONT
                            FROM AS400DB01.PMOD0001.PMMS02 WITH (NOLOCK)
                            WHERE P2IDNO = '{dr_CSMS00["M00IDN"].ToString()}' ";
            DataSet DS_PMMS02 = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
            if (ilobj.check_dataset(DS_PMMS02))
            {
                foreach (DataRow dr_PMMS02 in DS_PMMS02.Tables[0].Rows)
                {
                    if (dr_PMMS02["P2DEL"].ToString() != "X")
                    {
                        WKAPTY = string.Empty;
                        L1STS = string.Empty;
                        L1CLO = "000000000";
                        L1ODT = "00";
                        L1WOAMT = "000000000";
                        L1ERR = string.Empty;
                        WBRN = Convert.ToInt32(!string.IsNullOrEmpty(dr_PMMS02["P2BRN"].ToString()) ? dr_PMMS02["P2BRN"].ToString() : "0");
                        TMPBUS = "PM";
                        WCNT = Convert.ToInt64(!string.IsNullOrEmpty(dr_PMMS02["P2CONT"].ToString()) ? dr_PMMS02["P2CONT"].ToString() : "0");
                        if (dr_PMMS02["P2DEL"].ToString() == "C" || dr_PMMS02["P2DEL"].ToString() == "N")
                        {
                            L1STS = "CL";
                        }
                        else if (string.IsNullOrEmpty(dr_PMMS02["P2DEL"].ToString()))
                        {
                            L1STS = "AC";
                        }
                        else if (dr_PMMS02["P2DEL"].ToString() == "W")
                        {
                            L1STS = "WO";
                        }
                        #region                   EXSR      @MVSFL            
                        MVSFL1(dataCenter);
                        #endregion
                    }
                }
            }
        }
        public void R_PL(EB_Service.DAL.DataCenter dataCenter, DataRow dr_CSMS00)
        {
            ILDataCenter ilobj = new ILDataCenter();
            string sql = $@"SELECT P2DEL, P2BRN, P2CONT
                            FROM AS400DB01.PLOD0001.PLMS02 WITH (NOLOCK)
                            WHERE P2IDNO = '{dr_CSMS00["M00IDN"].ToString()}' ";
            DataSet DS_PLMS02 = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
            if (ilobj.check_dataset(DS_PLMS02))
            {
                foreach (DataRow dr_PLMS02 in DS_PLMS02.Tables[0].Rows)
                {
                    if (dr_PLMS02["P2DEL"].ToString() != "X")
                    {
                        WKAPTY = string.Empty;
                        L1STS = string.Empty;
                        L1CLO = "000000000";
                        L1ODT = "00";
                        L1WOAMT = "000000000";
                        L1ERR = string.Empty;
                        WBRN = Convert.ToInt32(!string.IsNullOrEmpty(dr_PLMS02["P2BRN"].ToString()) ? dr_PLMS02["P2BRN"].ToString() : "0");
                        TMPBUS = "PL";
                        WCNT = Convert.ToInt64(!string.IsNullOrEmpty(dr_PLMS02["P2CONT"].ToString()) ? dr_PLMS02["P2CONT"].ToString() : "0");
                        if (dr_PLMS02["P2DEL"].ToString() == "C" || dr_PLMS02["P2DEL"].ToString() == "N")
                        {
                            L1STS = "CL";
                        }
                        else if (string.IsNullOrEmpty(dr_PLMS02["P2DEL"].ToString()))
                        {
                            L1STS = "AC";
                        }
                        else if (dr_PLMS02["P2DEL"].ToString() == "W")
                        {
                            L1STS = "WO";
                        }
                        #region                   EXSR      @MVSFL            
                        MVSFL1(dataCenter);
                        #endregion
                    }
                }
            }
        }
        public void R_PH(EB_Service.DAL.DataCenter dataCenter, DataRow dr_CSMS00)
        {
            ILDataCenter ilobj = new ILDataCenter();
            string sql = $@"SELECT P2DEL, P2BRN, P2CONT, P2LOCA
                            FROM AS400DB01.PHOD0001.PHMS02 WITH (NOLOCK)
                            WHERE P2IDNO = '{dr_CSMS00["M00IDN"].ToString()}' ";
            DataSet DS_PHMS02 = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
            if (ilobj.check_dataset(DS_PHMS02))
            {
                foreach (DataRow dr_PHMS02 in DS_PHMS02.Tables[0].Rows)
                {
                    if (dr_PHMS02["P2DEL"].ToString() != "X")
                    {
                        WKAPTY = string.Empty;
                        L1STS = string.Empty;
                        L1CLO = "000000000";
                        L1ODT = "00";
                        L1WOAMT = "000000000";
                        L1ERR = string.Empty;
                        WBRN = Convert.ToInt32(!string.IsNullOrEmpty(dr_PHMS02["P2BRN"].ToString()) ? dr_PHMS02["P2BRN"].ToString() : "0");
                        TMPBUS = "PH";
                        WCNT = Convert.ToInt64(!string.IsNullOrEmpty(dr_PHMS02["P2CONT"].ToString()) ? dr_PHMS02["P2CONT"].ToString() : "0");
                        if (dr_PHMS02["P2DEL"].ToString() == "C" || dr_PHMS02["P2DEL"].ToString() == "N")
                        {
                            L1STS = "CL";
                        }
                        else if (dr_PHMS02["P2DEL"].ToString() == "L")
                        {
                            L1STS = "LC";
                        }
                        else if (string.IsNullOrEmpty(dr_PHMS02["P2DEL"].ToString()))
                        {
                            if (dr_PHMS02["P2LOCA"].ToString() == "275")
                            {
                                L1STS = "AC";
                            }
                            else
                            {
                                L1STS = "US";
                            }
                        }
                        else if (dr_PHMS02["P2DEL"].ToString() == "W")
                        {
                            L1STS = "WO";
                        }
                        #region                   EXSR      @MVSFL            
                        MVSFL1(dataCenter);
                        #endregion
                    }
                }
            }
        }
        public void FindID2(EB_Service.DAL.DataCenter dataCenter)
        {
            ILDataCenter ilobj = new ILDataCenter();
            A = 1;
            TMPOD1U = 0;
            lst_T1AREA = Enumerable.Repeat(new T1AREA(), 50).ToList();
            lst_T1AREA[A].T1APTY = string.Empty;
            WKTYPE = TYPETL;
            WKTELNO = TELNO;
            string sql = $@"SELECT CISnumber M00CSN, IDCard M00IDN
                            FROM CustomerDB01.CustomerInfo.CustomerGeneral cg WITH (NOLOCK)
                            WHERE SurnameInTHAI = '{SURN}' ";
            DataSet DS_CSMS00 = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
            if (ilobj.check_dataset(DS_CSMS00))
            {
                foreach(DataRow dr_CSMS00 in DS_CSMS00.Tables[0].Rows)
                {
                    WKAPTY = string.Empty;
                    #region EXSR @R_RL
                    R_RL(dataCenter, dr_CSMS00);
                    #endregion
                    #region EXSR @R_PW
                    R_PW(dataCenter, dr_CSMS00);
                    #endregion
                    #region EXSR @R_IL
                    R_IL(dataCenter, dr_CSMS00);
                    #endregion
                    #region EXSR @R_HP
                    R_HP(dataCenter, dr_CSMS00);
                    #endregion
                    #region EXSR @R_PL
                    R_PL(dataCenter, dr_CSMS00);
                    #endregion
                    #region EXSR @R_PH
                    R_PH(dataCenter, dr_CSMS00);
                    #endregion
                    #region EXSR @R_HM
                    R_HM(dataCenter, dr_CSMS00);
                    #endregion
                    #region EXSR @R_PM
                    R_PM(dataCenter, dr_CSMS00);
                    #endregion

                }
            }
        }
        public void FindID3(EB_Service.DAL.DataCenter dataCenter)
        {
            ILDataCenter ilobj = new ILDataCenter();
            A = 1;
            TMPOD1U = 0;
            lst_T1AREA = Enumerable.Repeat(new T1AREA(), 50).ToList();
            lst_T1AREA[A].T1APTY = string.Empty;
            WKTYPE = TYPETL;
            WKTELNO = TELNO;
            string sql = $@"SELECT CISnumber M00CSN, IDCard M00IDN, cw.OfficeName M00OFC
                            FROM CustomerDB01.CustomerInfo.CustomerWorked cw WITH (NOLOCK)
                            INNER JOIN CustomerDB01.CustomerInfo.CustomerGeneral cg WITH (NOLOCK) ON (cw.CustID = cg.ID)
                            WHERE OfficeName = '{OFFNME}' ";
            DataSet DS_CSMS00 = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
            if (ilobj.check_dataset(DS_CSMS00))
            {
                foreach(DataRow dr_CSMS00 in DS_CSMS00.Tables[0].Rows)
                {
                    WKAPTY = string.Empty;
                    R_RL(dataCenter, dr_CSMS00);
                }
            }
        }
        public void MVSFL(EB_Service.DAL.DataCenter dataCenter)
        {
            ILDataCenter ilobj = new ILDataCenter();
            A = 1;
            if (string.IsNullOrEmpty(L1ERR))
            {
                if(TMPBUS != lst_T1AREA[A].T1APTY)
                {
                    lst_T1AREA[A] = new T1AREA();
                    lst_T1AREA[A].T1APTY = TMPBUS;
                    lst_T1AREA[A].T1GROUP = WKTYPE;
                    lst_T1AREA[A].T1DESC = WKTELNO;
                }
                if(L1STS == "AC")
                {
                    lst_T1AREA[A].T1TOTA++;
                    if (Convert.ToInt32(WODT) == 0)
                    {
                        lst_T1AREA[A].T1NORC++;
                    }
                    else if (Convert.ToInt32(WODT) == 1)
                    {
                        lst_T1AREA[A].T1CURC++;
                    }
                    else if (Convert.ToInt32(WODT) == 2)
                    {
                        lst_T1AREA[A].T1OD1C++;
                    }
                    else if (Convert.ToInt32(WODT) > 2)
                    {
                        lst_T1AREA[A].T1OD1U++;
                    }
                }
                else if (L1STS == "WO")
                {
                    lst_T1AREA[A].T1WOCS++;
                }
                else if (L1STS == "LC")
                {
                    lst_T1AREA[A].T1LOCS++;
                }
                else
                {
                    lst_T1AREA[A].T1TOTC++;
                }
                lst_T1AREA[A].T1TOTCS++;
            }
            #region             **  Check Case Fraud
            string sql = $@"SELECT M50CNT
                            FROM AS400DB01.CSOD0001.CSMS50 WITH (NOLOCK)
                            WHERE M50CNT = {WCNT}";
            DataSet DS_CSMS50 = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
            if (ilobj.check_dataset(DS_CSMS50))
            {
                lst_T1AREA[A].T1FRAUD++;
            }
            #endregion
        }
        public void MVSFL1(EB_Service.DAL.DataCenter dataCenter)
        {
            ILDataCenter ilobj = new ILDataCenter();
            ILDataSubroutine dataSubroutine = new ILDataSubroutine(m_UserInfo);
            WKAPPL = TMPBUS;
            WKCONT = Convert.ToString(WCNT);
            WKCKDT = Convert.ToString(WDATE);
            string WKODT1 = "";
            string WKODA1 = "";
            string WKODT2 = "";
            string WKODA2 = "";
            bool resGNSR221 = dataSubroutine.Call_GNSR221(WKAPPL, WKCONT, WKCKDT, ref WKODT1, ref WKODA1, ref WKODT2, ref WKODA2);
            WODT = WKODT2;
            A = 1;
            if (string.IsNullOrEmpty(L1ERR))
            {
                if (TMPBUS != lst_T1AREA[A].T1APTY)
                {
                    if (string.IsNullOrEmpty(lst_T1AREA[A].T1APTY))
                    {
                        lst_T1AREA[A] = new T1AREA();
                        lst_T1AREA[A].T1APTY = TMPBUS;
                        lst_T1AREA[A].T1GROUP = WKTYPE;
                        lst_T1AREA[A].T1DESC = WKTELNO;
                    }
                }
                if (L1STS == "US")
                {
                    lst_T1AREA[A].T1NORC++;
                }
                if (L1STS == "AC")
                {
                    lst_T1AREA[A].T1TOTA++;
                    if (Convert.ToInt32(WODT) == 0)
                    {
                        lst_T1AREA[A].T1NORC++;
                    }
                    else if (Convert.ToInt32(WODT) == 1)
                    {
                        lst_T1AREA[A].T1CURC++;
                    }
                    else if (Convert.ToInt32(WODT) == 2)
                    {
                        lst_T1AREA[A].T1OD1C++;
                    }
                    else if (Convert.ToInt32(WODT) > 2)
                    {
                        lst_T1AREA[A].T1OD1U++;
                    }
                }
                else if (L1STS == "WO")
                {
                    lst_T1AREA[A].T1WOCS++;
                }
                else if (L1STS == "LC")
                {
                    lst_T1AREA[A].T1LOCS++;
                }
                else
                {
                    lst_T1AREA[A].T1TOTC++;
                }
                lst_T1AREA[A].T1TOTCS++;
            }
            #region             **  Check Case Fraud
            string sql = $@"SELECT M50CNT
                            FROM AS400DB01.CSOD0001.CSMS50 WITH (NOLOCK)
                            WHERE M50CNT = {WCNT}";
            DataSet DS_CSMS50 = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
            if (ilobj.check_dataset(DS_CSMS50))
            {
                lst_T1AREA[A].T1FRAUD++;
            }
            #endregion
        }
        public void MOVDS()
        {
            for (int A = 0; A < 50; A = A)
            {
                A = A + 1;
                if(string.IsNullOrEmpty(lst_T1AREA[A].T1APTY))
                {
                    A = 50;
                    continue;
                }
                RC++;
                DataRow row = dataSetResult.Tables[0].NewRow();
                row["GROUP"] = lst_T1AREA[A].T1GROUP;
                row["DESC"] = lst_T1AREA[A].T1DESC;
                row["BUS"] = lst_T1AREA[A].T1APTY;
                row["ACT"] = lst_T1AREA[A].T1TOTA;
                row["CUR"] = lst_T1AREA[A].T1CURC;
                row["NOR"] = lst_T1AREA[A].T1NORC;
                row["OD1"] = lst_T1AREA[A].T1OD1C;
                row["OD1UP"] = lst_T1AREA[A].T1OD1U;
                row["WO"] = lst_T1AREA[A].T1WOCS;
                row["LC"] = lst_T1AREA[A].T1LOCS;
                row["TOT"] = lst_T1AREA[A].T1TOTCS;
                row["CLS"] = lst_T1AREA[A].T1TOTC;
                row["FRD"] = lst_T1AREA[A].T1FRAUD;
                dataSetResult.Tables[0].Rows.Add(row);
            }
        }
    }
}