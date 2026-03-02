
using ILSystem.App_Code.BLL.DataCenter;
using ILSystem.App_Code.Commons;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;

namespace ILSystem.App_Code.BLL.Integrate
{
    public class CSSR16 : UserInfo
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
        //private DALMssql _dataCenter;
        //public EB_Service.DAL.DataCenter _dataCenter;

        public CSSR16(UserInfo userInfo)
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


        public void checkTelType(EB_Service.DAL.DataCenter dataCenter, string PIMODE, string PITYPE, string PICSNO, string PIRSQ, string PISHTO, string PITELY, string PITELT, string PIEXTN, ref string POERR)
        {
            string M11CDE = "", WKSKIP = "";
            int WKLIMIT = 0;


            // Main process																
            if (PITYPE == "M" && PISHTO == "A")
            {
                POERR = "Y";
            }
            else
            {
                // @Del11															
                DeleteCustomerAddress(dataCenter, PICSNO);
                // @Del12															
                CheckBeforeDelCustomerTelephone(dataCenter, PICSNO);
                if (PITYPE == "A")
                {
                    // @INSAT														
                    GetCusAddressForInsertTel(dataCenter, PICSNO, PISHTO);
                }
                else
                {
                    // @INSMT														
                    GetInputTelForInsertTel(dataCenter, PISHTO, PICSNO, PIRSQ, PITELY, PITELT, PIEXTN);
                }

            }
            return;
        }

        private void DeleteCustomerAddress(EB_Service.DAL.DataCenter dataCenter, string strCSNO)
        {
            DataSet dtcsms11 = new DataSet();
            //string sql = "";
            string sql = $@"select GC.Code as M11CDE , CA.ID, CA.CustID,CA.CustRefID as M11REF,CA.AddressCodeID,CA.TelephoneNumber1 as M11TEL,CA.TelephoneNumber2 as M11TL2,CA.TelephoneNumber3 as M11TL3,CA.Fax as M11FAX, CA.Mobile as M11MOB,																				
			CA.CreateBy, CA.CreateDate, CA.UpdateDate,CA.SysStartTime,CA.SysEndTime
			from [CustomerDB01].[CustomerInfo].[CustomerAddress]   CA WITH (NOLOCK)
		   left join [generalDB01].[GeneralInfo].[GeneralCenter] GC WITH (NOLOCK) on Type = 'AddressCodeID' and CA.AddressCodeID = GC.ID
			where CustID = (select id from[CustomerDB01].[CustomerInfo].[CustomerGeneral] WITH (NOLOCK) where CISNumber = { strCSNO } )	";

            ILDataCenter ilobj = new ILDataCenter();
            ILDataSubroutine dataSubroutine = new ILDataSubroutine(m_UserInfo);

            dtcsms11 = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
            if (ilobj.check_dataset(dtcsms11))
            {

            }
                //       foreach (DataRow drcs11 in dtcsms11.Rows)
                //       {
                //           if (drcs11["M11CDE"].ToString() == "C")
                //           {
                //               // insert CSMS11HS																				
                //               // delete CSMS11	
                //               string sqldel = "";
                //               sqldel = $@"delect* from[CustomerDB01].[CustomerInfo].[CustomerAddress] CA
                //where CA.ID =  { drcs11["ID"].ToString()}";

                //           }
                //       }
            }

        private void CheckBeforeDelCustomerTelephone(EB_Service.DAL.DataCenter dataCenter, string PICSNO)
        {
            DataSet dtcsms12 = new DataSet();
            //Field ที่ต้องการใช้ M12REF, M12ACD, M12RSQ , M12TTY
            string sql = $@"select CT.ID,CT.CustID, CT.CustRefID, CR.ReferenceID,GC.Code as M12REF, CT.AddressCodeID ,GA.Code as M12ACD, CT.TelephoneTypeID, GT.Code as M12TTY, CR.Sequence as M12RSQ
                         from[CustomerDB01].[CustomerInfo].[CustomerTelephone] CT WITH (NOLOCK)
                         left join[CustomerDB01].[CustomerInfo].[CustomerReference] CR WITH (NOLOCK)  on CR.CustID = CT.CustID and CT.CustRefID = CR.ID
                         left join[generalDB01].[GeneralInfo].[GeneralCenter] GC WITH (NOLOCK) on GC.ID = CR.ReferenceID  and GC.Type = 'ReferenceID'
        left join [generalDB01].[GeneralInfo].[GeneralCenter] GA WITH (NOLOCK) on GA.ID = CT.AddressCodeID  and GA.Type = 'AddressCodeID'
        left join [generalDB01].[GeneralInfo].[GeneralCenter] GT WITH (NOLOCK) on GT.ID = CT.TelephoneTypeID  and GT.Type = 'TelephoneTypeID'
        where CT.CustID = (select id from[CustomerDB01].[CustomerInfo].[CustomerGeneral] WITH (NOLOCK) where CISNumber = { PICSNO } )";

            ILDataCenter ilobj = new ILDataCenter();
            ILDataSubroutine dataSubroutine = new ILDataSubroutine(m_UserInfo);


            dtcsms12 = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
            if (!ilobj.check_dataset(dtcsms12))
            {
                return;
            }

            foreach (DataRow drcs12 in dtcsms12.Tables[0].Rows)
            {
                string WKSKIP = "", PISHTO = "", PITYPE = "", PIRSQ = "", PITELY = "";
                PITYPE = drcs12["M12TTY"].ToString();
                if (PISHTO != "A")
                {
                    if (PISHTO == "R")
                    {
                        if (drcs12["M12REF"].ToString() != "1")
                        {
                            WKSKIP = "Y";
                            DeleteCustomerTelephone(dataCenter, WKSKIP, drcs12["ID"].ToString());


                            continue;
                        }
                        else
                        {
                            if (drcs12["M12REF"].ToString() != "")
                            {
                                WKSKIP = "Y";
                                DeleteCustomerTelephone(dataCenter, WKSKIP, drcs12["ID"].ToString());


                                continue;
                            }
                            if (PISHTO != drcs12["M12ACD"].ToString())
                            {
                                WKSKIP = "Y";
                                DeleteCustomerTelephone(dataCenter, WKSKIP, drcs12["ID"].ToString());


                                continue;
                            }
                        }
                    }
                }
                if (PITYPE != "M")
                {
                    if (PIRSQ != drcs12["M12RSQ"].ToString())
                    {
                        WKSKIP = "Y";
                        DeleteCustomerTelephone(dataCenter, WKSKIP, drcs12["ID"].ToString());


                        continue;
                    }
                    if (PITELY != drcs12["M12TTY"].ToString())
                    {
                        WKSKIP = "Y";
                        DeleteCustomerTelephone(dataCenter, WKSKIP, drcs12["ID"].ToString());


                        continue;
                    }
                }
            }

        }
        private void GetCusAddressForInsertTel(EB_Service.DAL.DataCenter dataCenter, string strCSNO, string PISHTO)
        {
            DataTable dtcsms11 = new DataTable();
            // Field ที่ต้องการใช้ M11CSN, M11CDE , M11REF, M11TEL, M11RSQ, M11TL2, M11TL3, M11FAX, M11MOB
            string sql = $@"select GC.Code as M11CDE , CA.ID, CA.CustID,CA.CustRefID as M11REF,CA.AddressCodeID,CA.TelephoneNumber1 as M11TEL,CA.TelephoneNumber2 as M11TL2,CA.TelephoneNumber3 as M11TL3,CA.Fax as M11FAX, CA.Mobile as M11MOB,												
		CA.CreateBy, CA.CreateDate, CA.UpdateDate,CA.SysStartTime,CA.SysEndTime
        from [CustomerDB01].[CustomerInfo].[CustomerAddress]  CA WITH (NOLOCK)
       left join[generalDB01].[GeneralInfo].[GeneralCenter] GC WITH (NOLOCK) on Type = 'AddressCodeID' and CA.AddressCodeID = GC.ID
        where CustID = (select id from[CustomerDB01].[CustomerInfo].[CustomerGeneral] WITH (NOLOCK) where CISNumber = { strCSNO })";

            foreach (DataRow drcs11 in dtcsms11.Rows)
            {
                // @CHK2													
                string WKSKIP = "";
                if (PISHTO != "A")
                {
                    if (PISHTO == "R")
                    {
                        if (drcs11["M11REF"].ToString() != "1")
                        {
                            WKSKIP = "Y";
                        }
                        else
                        {
                            if (PISHTO != drcs11["M11CDE"].ToString())
                            {
                                WKSKIP = "Y";
                            }
                        }
                    }
                }
                if (WKSKIP == "")
                {
                    // @WRTAT												
                    string M12CSN = "0", M12RSQ = "0", M12UTM, M12SEQ = "0", M12UDT = "0";
                    string WKTELF = "", WKTEL = "", WKTELE = "", M12TTY = "", strTypeTel = "";
                    M12CSN = drcs11["M11CSN"].ToString();
                    string M12REF = drcs11["M11REF"].ToString();
                    M12RSQ = drcs11["M11RSQ"].ToString();
                    string M12ACD = drcs11["M11CDE"].ToString();
                    M12UDT = 25430000 + DateTime.Now.ToString("MMYYDD");
                    M12UTM = DateTime.Now.ToString();
                    string M12USR = m_UserInfo.Username;
                    string M12PGM = "CSSR16";
                    string M12UWS = m_UserInfo.LocalClient;
                    M12SEQ = "0";
                    if (drcs11["M11TEL"].ToString() != "" && drcs11["M11TEL"].ToString().Contains("**"))
                    {
                        WKTELF = drcs11["M11TEL"].ToString();


                        string WKSTAR = CheckTelephone(WKTELF);


                        M12TTY = "P";
                        M12SEQ = "1";
                        InsertCustomerTelephone(dataCenter, WKSTAR, M12CSN, M12REF, M12RSQ, M12ACD, M12UDT, M12UTM, M12USR, M12PGM, M12UWS, M12SEQ, M12TTY);


                    }
                    if (drcs11["M11TL2"].ToString() != "" && !drcs11["M11TL2"].ToString().Contains("**"))
                    {
                        WKTELF = drcs11["M11TL2"].ToString();


                        string WKSTAR = CheckTelephone(WKTELF);


                        M12TTY = "P";
                        M12SEQ = "1";
                        InsertCustomerTelephone(dataCenter, WKSTAR, M12CSN, M12REF, M12RSQ, M12ACD, M12UDT, M12UTM, M12USR, M12PGM, M12UWS, M12SEQ, M12TTY);


                    }
                    if (drcs11["M11TL3"].ToString() != "" && !drcs11["M11TL3"].ToString().Contains("**"))
                    {
                        WKTELF = drcs11["M11TL3"].ToString();


                        string WKSTAR = CheckTelephone(WKTELF);


                        M12TTY = "P";
                        M12SEQ = "1";
                        InsertCustomerTelephone(dataCenter, WKSTAR, M12CSN, M12REF, M12RSQ, M12ACD, M12UDT, M12UTM, M12USR, M12PGM, M12UWS, M12SEQ, M12TTY);


                    }
                    if (drcs11["M11FAX"].ToString() != "" && !drcs11["M11FAX"].ToString().Contains("**"))
                    {
                        WKTELF = drcs11["M11FAX"].ToString();


                        string WKSTAR = CheckTelephone(WKTELF);


                        M12TTY = "F";
                        M12SEQ = "1";
                        InsertCustomerTelephone(dataCenter, WKSTAR, M12CSN, M12REF, M12RSQ, M12ACD, M12UDT, M12UTM, M12USR, M12PGM, M12UWS, M12SEQ, M12TTY);


                    }
                    if (drcs11["M11MOB"].ToString() != "" && !drcs11["M11MOB"].ToString().Contains("**"))
                    {
                        WKTELF = drcs11["M11MOB"].ToString();


                        string WKSTAR = CheckTelephone(WKTELF);


                        M12TTY = "M";
                        M12SEQ = "1";
                        InsertCustomerTelephone(dataCenter, WKSTAR, M12CSN, M12REF, M12RSQ, M12ACD, M12UDT, M12UTM, M12USR, M12PGM, M12UWS, M12SEQ, M12TTY);


                    }
                }
            }

        }

        void InsertCustomerTelephone(EB_Service.DAL.DataCenter dataCenter, string WKSTAR, string M12CSN, string M12REF, string M12RSQ, string M12ACD, string M12UDT, string M12UTM, string M12USR, string M12PGM, string M12UWS, string M12SEQ, string M12TTY)
        {
            //_dataCenter = new ILDataCenterMssql();
            DataSet ds = new DataSet();
            string strTypeTel = $@"select ID from[generalDB01].[GeneralInfo].[GeneralCenter] WITH (NOLOCK) where Type = 'TelephoneTypeID' and code = { M12TTY }";
            var result = dataCenter.GetDataset<DataTable>(strTypeTel, CommandType.Text).Result;
            if (result.success)
            {
                ds = result.data;

            }
            // insert CSMS12
            string sql = "insert into[CustomerDB01].[CustomerInfo].[CustomerTelephone]";
        }


        private void GetInputTelForInsertTel(EB_Service.DAL.DataCenter dataCenter, string PISHTO, string PICSNO, string PIRSQ, string PITELY, string PITELT, string PIEXTN)
        {
            int affectedRows;
            string M12REF = "";
            string M12ACD = "";
            string M12RSQ = "";
            string M12SEQ = "";
            string M12USR = m_UserInfo.Username;
            string M12UWS = m_UserInfo.LocalClient;
            if (PISHTO == "R")
            {
                M12REF = "1";
                M12ACD = "H";
            }
            else
            {
                M12ACD = PISHTO;
            }
            M12RSQ = PIRSQ;
            M12SEQ = "1";

            if (!string.IsNullOrEmpty(PITELT) && PITELT != "**")
            {
                DataSet ds = new DataSet();
                string strTypeTel = $@"select TelephoneNumber from [CustomerDB01].[CustomerInfo].[CustomerTelephone]  WITH (NOLOCK) 
                                       where CustID = (SELECT TOP(1) ID FROM [CustomerDB01].[CustomerInfo].[CustomerGeneral] WITH (NOLOCK) WHERE CISNumber = '{PICSNO}') and AddressCodeID = (SELECT TOP(1) ID FROM GeneralDB01.GeneralInfo.GeneralCenter WITH (NOLOCK) WHERE Type = 'AddressCodeID' AND Code = '{PISHTO}')
                                        and TelephoneTypeID = (SELECT TOP(1) ID FROM GeneralDB01.GeneralInfo.GeneralCenter WITH (NOLOCK) WHERE Type = 'TelephoneTypeID' AND Code = '{PITELY}')
                                        and TelephoneNumber = '{PITELT}' and ExtensionNumber = '{PIEXTN}'  ";
                var result = dataCenter.GetDataset<DataTable>(strTypeTel, CommandType.Text).Result;
                if (result.success)
                {
                    ds = result.data;
                }
                if (ds.Tables[0].Rows.Count == 0)
                {
                    string sql = $@"Insert into [CustomerDB01].[CustomerInfo].[CustomerTelephone] 
                                (CustID,CustRefID,AddressCodeID,TelephoneTypeID,TelephoneNumber,ExtensionNumber,Sequence,Application,CreateBy,CreateDate,UpdateBy,UpdateDate,IsDelete,Seq,TelephoneTypeOtherID)
                                values ( (SELECT TOP(1) ID FROM [CustomerDB01].[CustomerInfo].[CustomerGeneral] WITH (NOLOCK) WHERE CISNumber = '{PICSNO}')
                                        ,{M12RSQ}
                                        ,(SELECT TOP(1) ID FROM GeneralDB01.GeneralInfo.GeneralCenter WITH (NOLOCK) WHERE Type = 'AddressCodeID' AND Code = '{PISHTO}')
                                        ,(SELECT TOP(1) ID FROM GeneralDB01.GeneralInfo.GeneralCenter WITH (NOLOCK) WHERE Type = 'TelephoneTypeID' AND Code = '{PITELY}')
                                        ,'{PITELT}'
                                        ,'{PIEXTN}'  
                                        ,{M12SEQ}   
                                        ,'CSSR16' 
                                        ,'{M12USR}'    
                                        ,'{(int.Parse(m_UdpD.ToString().Substring(0, 4)) - 543).ToString()}-{m_UdpD.ToString().Substring(4, 2)}-{m_UdpD.ToString().Substring(6, 2)} {m_UpdTime.ToString().PadLeft(6, '0').Substring(0, 2)}:{m_UpdTime.ToString().PadLeft(6, '0').Substring(2, 2)}:{m_UpdTime.ToString().PadLeft(6, '0').Substring(4, 2)}'
                                        ,'{M12USR}'    
                                        ,'{(int.Parse(m_UdpD.ToString().Substring(0, 4)) - 543).ToString()}-{m_UdpD.ToString().Substring(4, 2)}-{m_UdpD.ToString().Substring(6, 2)} {m_UpdTime.ToString().PadLeft(6, '0').Substring(0, 2)}:{m_UpdTime.ToString().PadLeft(6, '0').Substring(2, 2)}:{m_UpdTime.ToString().PadLeft(6, '0').Substring(4, 2)}'
                                        ,''
                                        ,'0'
                                        ,'0' )";
                    var res_CSMS12 = dataCenter.Execute(sql, CommandType.Text, dataCenter.Sqltr == null ? true : false).Result;
                    affectedRows = -1;
                    affectedRows = res_CSMS12.afrows;
                    if (affectedRows < 0)
                    {
                        dataCenter.RollbackMssql();
                        dataCenter.CloseConnectSQL();
                    }
                }
            }
        }

        private void DeleteCustomerTelephone(EB_Service.DAL.DataCenter dataCenter, string strWKSKIP, string strCSNO)
        {

            if (strWKSKIP == "")
            {
                // insert CSMS12HS		
                // delete CSMS12		
                string sql = $@"delete from [CustomerDB01].[CustomerInfo].[CustomerTelephone] where ID = {strCSNO} ";
                var res_ILWK12 = dataCenter.Execute(sql, CommandType.Text, dataCenter.Sqltr == null ? true : false).Result;
                int res_del12 = res_ILWK12.afrows;
            }

        }

        private string CheckTelephone(string strTel)
        {
            string WKSTAR = "0";
            string WKEND = "0";
            string WKTEL = "";
            string WKTELE = "";
            if (strTel.Contains("-"))
            {
                string[] i = strTel.Split('-');


                if (i[0].Length <= 3)
                {
                    WKSTAR = i[0] + i[1];
                    WKEND = i[0] + i[1];
                }
                else
                {
                    WKSTAR = i[0];
                    WKEND = i[0].Substring(0, Convert.ToInt16(i[0].Length) - Convert.ToInt16(i[1].Length)) + i[1];
                }
            }
            else
            {
                WKTEL = strTel.Trim();
                WKSTAR = WKTEL;

                WKEND = WKTEL;

            }
            return WKSTAR;
        }



    }
}
