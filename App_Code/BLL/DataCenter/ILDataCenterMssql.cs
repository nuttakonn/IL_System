using EB_Service.DAL;
using ILSystem.App_Code.Commons;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Globalization;
using DevExpress.Web.ASPxEditors;
using static ILSystem.App_Code.Model.AS400DB01.AS400DB01Model;
using static ILSystem.App_Code.Model.CUSTOMERDB01.CUSTOMERDB01Model;
using Newtonsoft.Json;
using ILSystem.App_Code.Commons.APIService;
using System.Net;
using System.Configuration;
using System.IO;
using Newtonsoft.Json.Linq;
using ESB.WebAppl.ILSystem.Models;
using UserCenterModel;
using System.Collections;
using System.Web.Configuration;
using ESBiDB2;
using EB_Service.Commons;
//using ESBiDB2.iDB2Command;
namespace ILSystem.App_Code.BLL.DataCenter
{
    public class ILDataCenterMssql : UserInfo
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
        public UserInfo m_UserInfo;
        public EB_Service.DAL.DataCenter _dataCenter;

        public ILDataCenterMssql(UserInfo userInfo)
        {
            m_UserInfo = (UserInfo)userInfo;
            _dataCenter = new EB_Service.DAL.DataCenter(m_UserInfo);
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
        public DataSet getNote(string csn)
        {
            DataSet ds = new DataSet();
            //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_UserInfo;
            try
            {
                if (csn.Trim() != "")
                {

                    string sql = $@"select CONCAT(SUBSTRING(CAST(p38dat AS nvarchar),7,2),'/',SUBSTRING(CAST(p38dat AS nvarchar),5,2),'/',SUBSTRING(CAST(p38dat AS nvarchar),1,4)) as M38DAT_, 
                                    CONCAT(SUBSTRING(CAST(FORMAT(P38TIM, '000000') AS nvarchar), 1, 2), ':', SUBSTRING(CAST(FORMAT(P38TIM, '000000') AS nvarchar), 3, 2), ':', SUBSTRING(CAST(FORMAT(P38TIM, '000000') AS nvarchar), 5, 2)) AS M38TIM,
                                    p38acd as M38ACD, p38rcd as M38RCD, p38usr as M38USR, CONCAT(p38de1, p38de2) as M38DES
                                    FROM AS400DB01.ILOD0001.ilms38 WITH(NOLOCK)
                                    WHERE P38CSN = {csn} ORDER BY p38dat DESC";

                    ds = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
                    _dataCenter.CloseConnectSQL();
                }
            }
            catch (Exception ex)
            {
                _dataCenter.CloseConnectSQL();
            }
            return ds;
        }
        public int computeAge(string birthDate)
        {
            try
            {

                DateTime dateNow = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy", m_DThai), m_DThai);
                //DateTime birthDate_ = Convert.ToDateTime(birthDate.Trim().Substring(8, 2).ToString() + "/" + birthDate.Trim().Substring(5, 2).ToString() + "/" + (int.Parse(birthDate.Trim().Substring(0, 4)) + 543).ToString(), m_DThai);
                DateTime birthDate_ = Convert.ToDateTime(birthDate, m_DThai);
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
        public DataSet getApplyType()
        {
            DataSet ds = new DataSet();
            //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_UserInfo;
            try
            {
                string sql = @"select gn61cd, gn61dt from AS400DB01.GNOD0000.GNTB61 WITH (NOLOCK) WHERE SUBSTRING(GN61FL,2,1) = 1 AND GN61DL = ''";

                var result = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result;
                if (result.success)
                {
                    ds = result.data;

                }
                return ds;
            }
            catch (Exception ex)
            {
                return ds;
            }
        }
        public ArrayList RP_Param(string m_program, string dateFrom = "", string dateTo = "", string year = "")
        {
            //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_UserInfo;
            string company = "";
            try
            {
                //CALL_GNSRCONM("E", "F", "0", ref company);
                //CloseConnectioDAL();
            }
            catch (Exception ex)
            {
                //CloseConnectioDAL();
            }


            ArrayList aryParm = new ArrayList();

            aryParm.Add(m_UserInfo.Username.ToUpper().Trim());
            aryParm.Add(DateTime.Now.ToString("dd/MM/yyyy", m_DThai)); //  date
            aryParm.Add(DateTime.Now.ToString("HH:mm:ss")); // time
            aryParm.Add(m_UserInfo.LocalClient.ToString()); // work station
            aryParm.Add(m_program);  // program
            aryParm.Add(company); //  company
            aryParm.Add(m_UserInfo.BranchDescEN.ToString()); // branch
            aryParm.Add(dateFrom); // month1
            aryParm.Add(dateTo); // month2
            aryParm.Add(year); // year
            aryParm.Add(m_UserInfo.BranchApp.ToString()); // branch

            //CloseConnectioDAL();

            return aryParm;
        }

        public DataSet RP_Change_Occupation_sql(string PFRYMD, string PTOYMD, string PDOCTPY, string PCHANEL, string PUSER, string POCCCD, string POPTS)
        {
            DataSet ds = new DataSet();
            //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_UserInfo;
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


                //string sql = " SELECT * FROM ( " +
                //             " SELECT  SUBSTR(DIGITS(A.D5FDAT),7,2) ||'/'||SUBSTR(DIGITS(A.D5FDAT),5,2)||'/'||SUBSTR(DIGITS(A.D5FDAT),1,4)   FDATE, " +
                //             " CASE WHEN A.M00EBC <> 0 THEN " +
                //             " SUBSTR(DIGITS(A.M00EBC),1,4) ||'-'||SUBSTR(DIGITS(A.M00EBC),5,4)||'-'||SUBSTR(DIGITS(A.M00EBC),9,4)||'-'||SUBSTR(DIGITS(A.M00EBC),13,4) ELSE '' END  EBC_NO, " +
                //             " CASE A.D5FBUS  WHEN 'PW' THEN  CASE  WHEN PWMS01.P1CNT# <> 0 THEN DIGITS(PWMS01.P1CNT#) ELSE '' END  " +
                //             " WHEN 'RL' THEN  CASE  WHEN RLMS01.P1CNT# <> 0 THEN DIGITS(RLMS01.P1CNT#) ELSE '' END " +
                //             " WHEN 'IL' THEN  CASE  WHEN ILMS01.P1CONT <> 0 THEN DIGITS(ILMS01.P1CONT) ELSE '' END " +
                //             " ELSE  '' END TMCONT , " +
                //             " D5OCCO , " +
                //             " D5OCCN, " +
                //             " CASE  WHEN (A.D5FLBY = '01' AND ILMS01.P1APPT = '01') THEN '1' " +
                //             " WHEN (A.D5FLBY = '01' AND ILMS01.P1APPT = '02') THEN '2' " +
                //             " WHEN (A.D5FLBY = '02' ) THEN '3' " +
                //             " WHEN (A.D5FLBY = '07' ) THEN '5' " +
                //             " ELSE '4' END   TMCHANNEL , " +
                //             " CASE  WHEN (A.D5FLBY = '01' AND ILMS01.P1APPT = '01') THEN 'NORMAL'||A.FLAG_RV " +
                //             " WHEN (A.D5FLBY = '01' AND ILMS01.P1APPT = '02') THEN 'EBC'||A.FLAG_RV " +
                //             " WHEN (A.D5FLBY = '02' ) THEN 'UPD.SALARY'||A.FLAG_RV " +
                //             " WHEN (A.D5FLBY = '07' ) THEN 'CHG CUST.'||A.FLAG_RV " +
                //             " ELSE 'FULL APP. ' END   TMCHANNEL_name ,D5FUSR , " +
                //             " CASE A.D5FBUS   WHEN 'PW' THEN DIGITS(PWMS01.P1APDT) " +
                //             " WHEN 'RL' THEN DIGITS(RLMS01.P1APDT) " +
                //             " WHEN 'IL' THEN DIGITS(ILMS01.P1APDT) " +
                //             " ELSE   '' END TMAPDT , D5FLAG, " +
                //             " CASE  WHEN  (A.D5FLAG = 'Y') OR (D5FLAG='N' AND D5QUTA > 0) THEN 'N' " +
                //             " ELSE  'Y' END TMDOCR ,  " +
                //             " TRIM(M00TNM) || '  '||TRIM(M00TSN)  CUSTNAME , DIGITS(M00CSN) M00CSN " +
                //             " FROM " +
                //             "( SELECT csms035.*,M00TNM,M00TSN,M00EBC,CSMS032.*,CSMS032HS.*,m00csn, " +
                //             " CASE WHEN D5FLBY = '01' THEN CASE  WHEN (D5FBUS = CSMS032.D3FBUS AND D5FAPN = CSMS032.D3FAPN AND CSMS032.D3CBUS = 'RV' ) THEN '(RV1)' ELSE ''  END  " +
                //             " WHEN D5FLBY = '02' THEN CASE WHEN (D5FBUS = CSMS032.D3FBUS AND D5FSLD = CSMS032.D3FSLD AND D5FSLT = CSMS032.D3FSLT  AND CSMS032.D3CBUS = 'RV') THEN '(RV1)' ELSE '' END " +
                //             " WHEN D5FLBY = '01' THEN CASE WHEN (D5FBUS = CSMS032HS.D3FBUS AND D5FAPN = CSMS032HS.D3FAPN AND CSMS032HS.D3CBUS = 'RV' ) THEN '(RV1)'  ELSE ''  END " +
                //             " WHEN D5FLBY = '02' THEN CASE WHEN (D5FBUS = CSMS032HS.D3FBUS AND D5FSLD = CSMS032HS.D3FSLD AND D5FSLT = CSMS032HS.D3FSLT AND CSMS032HS.D3CBUS = 'RV' ) THEN '(RV1)' ELSE ''  END  " +
                //             " ELSE '' END FLAG_RV " +
                //             " FROM csms035 JOIN " +
                //             " CSMS00 ON D5CSNO = M00CSN " +
                //             " LEFT JOIN CSMS032  ON  D5CSNO = CSMS032.D3CSNO " +
                //             " LEFT JOIN CSMS032HS  ON  D5CSNO = CSMS032HS.D3CSNO " +
                //             " WHERE ((D5FBUS IN ('IL','')) OR (D5FLBY IN ('01','02','03','04','07')) ) " +
                //             " AND D5FDAT  BETWEEN " + PFRYMD + " AND " + PTOYMD +
                //             " AND D5OCCO <> D5OCCN ) A " +
                //             " LEFT JOIN  ILMS01 ON  ( A.D5FBRN = ILMS01.P1BRN   AND A.D5FAPN = ILMS01.P1APNO ) " +
                //             " LEFT JOIN  PWMS01 ON  ( A.D5FBRN = PWMS01.P1BRN   AND A.D5FAPN = PWMS01.P1APNO ) " +
                //             " LEFT JOIN  RLMS01 ON  ( A.D5FBRN = RLMS01.P1BRN   AND A.D5FAPN = RLMS01.P1APNO ) " +
                //             " WHERE 1 = 1 " +
                //             " ORDER BY A.D5FDAT ,A.M00EBC " +
                //             " ) B  WHERE 1 = 1 " + where ;

                string sql2 = $@" SELECT * FROM ( 
                              SELECT  SUBSTRING(CAST(A.D5FDAT as nvarchar),7,2) +'/'+SUBSTRING(CAST(A.D5FDAT as nvarchar),5,2)+'/'+SUBSTRING(CAST(A.D5FDAT as nvarchar),1,4)   FDATE, 
							  A.m00ebc,
                              CASE WHEN A.M00EBC <> 0 THEN 
                              SUBSTRING(CAST(A.M00EBC as nvarchar),1,4) +'-'+SUBSTRING(CAST(A.M00EBC as nvarchar),5,4)+'-'+SUBSTRING(CAST(A.M00EBC as nvarchar),9,4)+'-'+SUBSTRING(CAST(A.M00EBC as nvarchar),13,4) ELSE '' END  EBC_NO, 
                              CASE A.D5FBUS  WHEN 'PW' THEN  CASE  WHEN PWMS01.P1CNT# <> 0 THEN CAST(PWMS01.P1CNT# as nvarchar) ELSE '' END  
                              WHEN 'RL' THEN  CASE  WHEN RLMS01.P1CNT# <> 0 THEN CAST(RLMS01.P1CNT# as nvarchar) ELSE '' END 
                              WHEN 'IL' THEN  CASE  WHEN ILMS01.P1CONT <> 0 THEN CAST(ILMS01.P1CONT as nvarchar) ELSE '' END 
                              ELSE  '' END TMCONT , 
                              D5OCCO , 
                              D5OCCN, 
                              CASE  WHEN (A.D5FLBY = '01' AND ILMS01.P1APPT = '01') THEN '1' 
                              WHEN (A.D5FLBY = '01' AND ILMS01.P1APPT = '02') THEN '2' 
                              WHEN (A.D5FLBY = '02' ) THEN '3' 
                              WHEN (A.D5FLBY = '07' ) THEN '5' 
                              ELSE '4' END   TMCHANNEL , 
                              CASE  WHEN (A.D5FLBY = '01' AND ILMS01.P1APPT = '01') THEN 'NORMAL'+A.FLAG_RV 
                              WHEN (A.D5FLBY = '01' AND ILMS01.P1APPT = '02') THEN 'EBC'+A.FLAG_RV 
                              WHEN (A.D5FLBY = '02' ) THEN 'UPD.SALARY'+A.FLAG_RV 
                              WHEN (A.D5FLBY = '07' ) THEN 'CHG CUST.'+A.FLAG_RV 
                              ELSE 'FULL APP. ' END   TMCHANNEL_name ,D5FUSR , 
                              CASE A.D5FBUS   WHEN 'PW' THEN CAST(PWMS01.P1APDT as nvarchar) 
                              WHEN 'RL' THEN CAST(RLMS01.P1APDT as nvarchar) 
                              WHEN 'IL' THEN CAST(ILMS01.P1APDT as nvarchar) 
                              ELSE   '' END TMAPDT , D5FLAG, 
                              CASE  WHEN  (A.D5FLAG = 'Y') OR (D5FLAG='N' AND D5QUTA > 0) THEN 'N' 
                              ELSE  'Y' END TMDOCR ,  
                              TRIM(M00TNM) + '  '+TRIM(M00TSN)  CUSTNAME , CAST(CISNumber as nvarchar) M00CSN 
                              FROM 
                              (SELECT csms035.*,CSMS032.*,cg.CISNumber,NameInTHAI as M00TNM ,SurnameInTHAI  as M00TSN, '' as FLAG_RV , 
							  CASE WHEN (SELECT TOP(1) R2CDNO FROM AS400DB01.RLOD0001.RLMC02 WITH (NOLOCK) WHERE cg.CISNumber = R2CSNO)  IS NOT NULL THEN
                              COALESCE ((SELECT TOP (1) R2CDNO FROM AS400DB01.RLOD0001.RLMC02 WITH (NOLOCK) WHERE R2CSNO = cg.CISNumber), 0) 
                              WHEN (SELECT TOP(1) M20EBC FROM AS400DB01.CSOD0001.CSMS20 G WITH (NOLOCK) WHERE cg.CISNumber = M20CSN)  IS NOT NULL THEN
                              COALESCE ((SELECT TOP (1) M20EBC FROM AS400DB01.CSOD0001.CSMS20 G WITH (NOLOCK) WHERE cg.CISNumber = M20CSN), 0) 
                              ELSE NULL END AS m00ebc
                              FROM AS400DB01.CSOD0001.csms035 WITH (NOLOCK) 
                              JOIN CUSTOMERDB01.CustomerInfo.CustomerGeneral cg WITH(NOLOCK) on (CAST(D5CSNO as nvarchar) = cg.CISNumber)
                              LEFT JOIN AS400DB01.CSOD0001.CSMS032 WITH (NOLOCK)  ON  D5CSNO = CSMS032.D3CSNO 
                              WHERE ((D5FBUS IN ('IL','')) OR (D5FLBY IN ('01','02','03','04','07')) ) 
                              AND CAST(D5FDAT as nvarchar)  BETWEEN  '{PFRYMD}'  AND  '{PTOYMD}' 
                              AND D5OCCO <> D5OCCN ) as A
                              LEFT JOIN AS400DB01.ILOD0001.ILMS01 WITH (NOLOCK) ON  ( A.D5FBRN = ILMS01.P1BRN   AND A.D5FAPN = ILMS01.P1APNO ) 
                              LEFT JOIN AS400DB01.PWOD0001.PWMS01 WITH (NOLOCK) ON  ( A.D5FBRN = PWMS01.P1BRN   AND A.D5FAPN = PWMS01.P1APNO ) 
                              LEFT JOIN  AS400DB01.RLOD0001.RLMS01 WITH (NOLOCK) ON  ( A.D5FBRN = RLMS01.P1BRN   AND A.D5FAPN = RLMS01.P1APNO ) 
                              WHERE 1 = 1 
							   --ORDER BY A.D5FDAT, A.M00EBC 
                              ) as  B 
							  WHERE 1 = 1  {where}";

                var result = _dataCenter.GetDataset<DataTable>(sql2, CommandType.Text).Result;
                if (result.success)
                {
                    ds = result.data;
                }
                return ds;


            }
            catch (Exception ex)
            {
                return ds;
            }

            return ds;
        }

        public DataSet RP_PrintSticker(string WSBRN, string WSSDTE, string WSEDTE, string WSTYPE, string WSOPT1, string WSOPT2, string WSDFMT, ref DataSet ds_tb28)
        {
            DataSet ds = new DataSet();
            //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_UserInfo;
            try
            {
                bool Rescall = true;
                //bool Rescall = CALL_ILC021CL1(WSBRN, WSSDTE, WSEDTE, WSTYPE, WSOPT1, WSOPT2, WSDFMT, m_UserInfo.BizInit, m_UserInfo.BranchNo);
                if (Rescall)
                {


                    if (WSOPT2.Trim() == "X")
                    {
                        //string sql = $@" SELECT   '" + WSTYPE + "'||'-'||SUBSTR(M40BDT,3)||'-'||M40CON CONT ,CASE  WHEN M00EBC <> 0 THEN SUBSTR(DIGITS(M00EBC),1,1)||'-'||SUBSTR(DIGITS(M00EBC),2,3)||'-'||SUBSTR(DIGITS(M00EBC),5) ELSE '' END EBC  , " +
                        //             " CASE  WHEN  D45RDT <> 0 THEN SUBSTR(D45RDT,7,2)||'/'||SUBSTR(D45RDT,5,2)||'/'||SUBSTR(D45RDT,1,4) ELSE '' END FOL_DATE , " +
                        //             " CASE  WHEN  P2TMDT <> 0 THEN SUBSTR(P2TMDT,7,2)||'/'||SUBSTR(P2TMDT,5,2)||'/'||SUBSTR(P2TMDT,1,4) ELSE '' END TER_DATE , " +
                        //             " TRIM(GNB2TD)||TRIM(M00TNM)||'  ' ||TRIM(M00TSN) CUSTNM , " +
                        //             " SUBSTR(M40BDT,7,2)||'/'||SUBSTR(M40BDT,5,2)||'/'||SUBSTR(M40BDT,1,4) BOOKDATE " +
                        //             " FROM ILMS40 " +
                        //             " JOIN ILMS02 ON (M40CON = P2CONT AND  M40BRN = P2BRN) " +
                        //             " LEFT JOIN ILMD45 ON (M40CON = D45CNT AND  M40BRN = D45BRN) " +
                        //             " LEFT JOIN CSMS00 ON (M40CSN = M00CSN) " +
                        //             " LEFT JOIN GNMB20 ON (M00TTL = GNB2TC)" +
                        //             " WHERE M40BDT BETWEEN " + WSSDTE + " AND " + WSEDTE + " AND " +
                        //             " ((M40DEL = '') OR ( M40DEL ='X' AND P2DEL ='X' AND P2LOCA ='301')) " +
                        //             " AND M40BRN =  " + WSBRN + " ORDER BY M40BDT ASC";
                        string sql = $@" SELECT '{WSTYPE}'+'-'+SUBSTRING(CAST(M40BDT as nvarchar),0,3)+'-'+CAST(M40CON as nvarchar) CONT ,
                                      CASE WHEN (SELECT TOP(1) R2CDNO FROM AS400DB01.RLOD0001.RLMC02 WITH (NOLOCK) WHERE cg.CISNumber = R2CSNO)  IS NOT NULL THEN
                                      COALESCE ((SELECT TOP (1) R2CDNO FROM AS400DB01.RLOD0001.RLMC02 WITH (NOLOCK) WHERE R2CSNO = cg.CISNumber), 0) 
                                      WHEN (SELECT TOP(1) M20EBC FROM AS400DB01.CSOD0001.CSMS20 G WITH (NOLOCK) WHERE cg.CISNumber = M20CSN)  IS NOT NULL THEN
                                      COALESCE ((SELECT TOP (1) M20EBC FROM AS400DB01.CSOD0001.CSMS20 G WITH (NOLOCK) WHERE cg.CISNumber = M20CSN), 0) 
                                      ELSE NULL END AS M00EBC,
                                      CASE  WHEN  D45RDT <> 0 THEN SUBSTRING(CAST(D45RDT as nvarchar),7,2)+'/'+SUBSTRING(CAST(D45RDT as nvarchar),5,2)+'/'+SUBSTRING(CAST(D45RDT as nvarchar),1,4) ELSE '' END FOL_DATE , 
                                      CASE  WHEN  P2TMDT <> 0 THEN SUBSTRING(CAST(P2TMDT as nvarchar),7,2)+'/'+SUBSTRING(CAST(P2TMDT as nvarchar),5,2)+'/'+SUBSTRING(CAST(P2TMDT as nvarchar),1,4) ELSE '' END TER_DATE , 
									  gc.DescriptionTHAI + trim(trim(NameInTHAI) + ' ' + trim(SurnameInTHAI)) as CUSTNM,
                                      SUBSTRING(CAST(M40BDT as nvarchar),7,2)+'/'+SUBSTRING(CAST(M40BDT as nvarchar),5,2)+'/'+SUBSTRING(CAST(M40BDT as nvarchar),1,4) BOOKDATE 
                                      FROM AS400DB01.ILOD0001.ILMS40  WITH (NOLOCK)
                                      JOIN AS400DB01.ILOD0001.ILMS02 WITH (NOLOCK) ON M40CON = CAST(P2CONT as nvarchar) AND  CAST(M40BRN as nvarchar)= P2BRN 
                                      LEFT JOIN AS400DB01.ILOD0001.ILMD45 WITH (NOLOCK) ON M40CON  = CAST(D45CNT as nvarchar) AND  CAST(M40BRN as nvarchar) = CAST(D45BRN as nvarchar) 
									  left join CUSTOMERDB01.CustomerInfo.CustomerGeneral cg WITH(NOLOCK) on (CAST(M40CSN as nvarchar) = cg.CISNumber)
									  left join GeneralDB01.GeneralInfo.GeneralCenter gc WITH(NOLOCK) on (CAST(cg.TitleID as nvarchar) = gc.ID and gc.[Type] = 'TitleID')
                                      WHERE ( CAST(M40BDT as nvarchar) BETWEEN  '{WSSDTE}'  AND  '{WSEDTE}')  
                                      AND ((CAST(M40DEL as nvarchar )= '') OR ( M40DEL ='X' AND P2DEL ='X' AND P2LOCA ='301')) 
                                      AND CAST(M40BRN as nvarchar) =  '{WSBRN}'  ORDER BY M40BDT ASC";
                        var result = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result;
                        if (result.success)
                        {
                            ds = result.data;
                        }
                        //return ds;
                    }

                    if (WSOPT1.Trim() == "X")
                    {
                        string sql_iltb28 = " select CONT from AS400DB01.ILOD0001.iltb28 WITH (NOLOCK) ";

                        //ds_tb28 = RetriveAsDataSet(sql_iltb28);
                        var result = _dataCenter.GetDataset<DataTable>(sql_iltb28, CommandType.Text).Result;
                        if (result.success)
                        {
                            ds_tb28 = result.data;
                        }
                        //return ds_tb28;
                    }


                }
                _dataCenter.CloseConnectSQL();

            }
            catch (Exception ex)
            {
                _dataCenter.CloseConnectSQL();
                //CloseConnectioDAL();
            }

            return ds;
        }


        public DataSet RP_IL_Pending(string RP_Type, string DateF, string DateT, string brn, bool all_PD, string sts_PD)
        {
            DataSet ds = new DataSet();
            //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_UserInfo;
            try
            {
                string sqlWhere = " WHERE p1aprj IN ('MI','PD') and SUBSTRING(CAST(p1fill as nvarchar),25,1) IN ('1','2','3','4') ";
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
                    sql = " SELECT p1resn as pending_code, trim(g25des) as pending_desc, count(*) as [case], " +
                            " ((count(*) * 100.00) / (select count(*) from AS400DB01.ILOD0001.ilms01 " + sqlWhere + " )) as Percent_Case, " +
                            " sum(p1pram) as Loan_amt, FORMAT(case when sum(p1pram) > 0 then " +
                            " ((sum(p1pram) * 100.00) / (select sum(p1pram) from AS400DB01.ILOD0001.ilms01 " + sqlWhere + " )) else 0 end ,'N2')  as Percent_Loan, " +
                            " sum(p1coam) as Cont_amt, " +
                            " FORMAT(case when sum(p1coam) > 0 then " +
                            " ((sum(p1coam) * 100.00) / (select sum(p1coam) from AS400DB01.ILOD0001.ilms01 " + sqlWhere + " )) else 0 end ,'N2') as Percent_Cont " +
                            " FROM AS400DB01.ILOD0001.ilms01 WITH (NOLOCK) " +
                            " LEFT JOIN AS400DB01.GNOD0000.gntb25 WITH (NOLOCK) ON (p1resn=g25rcd) " +
                            sqlWhere +
                            " GROUP BY p1resn, g25des " +
                            " ORDER BY p1resn  ";

                }

                else if (RP_Type == "D")
                {
                    sql = " SELECT SUBSTRING(CAST(p1apdt as nvarchar),7,2)+'/'+SUBSTRING(CAST(p1apdt as nvarchar),5,2)+'/'+SUBSTRING(CAST(p1apdt as nvarchar),1,4) as p1apdt, " +
                           " CAST(p1brn as nvarchar) p1brn, CAST(FORMAT(p1apno,'00000000000') as nvarchar) p1apno, p1pram, p1coam, " +
                           " case when SUBSTRING(CAST(p1fill as nvarchar),25,1) in ('1') then 'KEY-IN STEP1'  " +
                           " when SUBSTRING(CAST(p1fill as nvarchar),25,1) in ('2','4') then 'INTERVIEW' " +
                           " when SUBSTRING(CAST(p1fill as nvarchar),25,1) in ('3') then 'KESSAI' " +
                           " else 'END CASE' end Processing, " +
                           " case when (p1aprj = 'PD' and p1avdt > 0) then SUBSTRING(CAST(p1avdt as nvarchar),7,2)+'/'+SUBSTRING(CAST(p1avdt as nvarchar),5,2)+'/'+SUBSTRING(CAST(p1avdt as nvarchar),1,4) " +
                           " else '' end Pending_Date, " +
                           " case when (p1aprj = 'PD' and p1avtm > 0) then SUBSTRING(CAST(p1avtm as nvarchar),1,2)+':'+SUBSTRING(CAST(p1avtm as nvarchar),3,2)+':'+SUBSTRING(CAST(p1avtm as nvarchar),5,2) " +
                           " else '' end Pending_Time, " +
                           " case when p1aprj = 'PD' then p1auth else '' end Pending_user, " +
                           " case when p1aprj = 'PD' then p1resn else '' end Pending_code, " +
                           " case when p1aprj = 'PD' then g25des else '' end Pending_desc  " +
                           " FROM AS400DB01.ILOD0001.ilms01 WITH (NOLOCK) " +
                           " LEFT JOIN AS400DB01.GNOD0000.GNTB25 WITH (NOLOCK) on (p1resn=g25rcd) " +
                           sqlWhere + " ORDER by SUBSTRING(CAST(p1apdt as nvarchar),7,4)+SUBSTRING(CAST(p1apdt as nvarchar),4,2)+SUBSTRING(CAST(p1apdt as nvarchar),1,2)";

                }

                var result = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result;
                if (result.success)
                {
                    ds = result.data;
                }
                return ds;

            }
            catch (Exception ex)
            {
                return ds;
            }
            //return ds;
        }

        public DataSet checkLockPending(string appNo, string brn)
        {
            DataSet ds = new DataSet();
            //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_UserInfo;
            try
            {
                string sql = $@"SELECT p1aprj, p1auth, (SUBSTRING(CAST(p1avdt as varchar),7,2)) + '/'+(SUBSTRING(CAST(p1avdt as varchar),5,2)) + '/'+ (SUBSTRING(CAST(p1avdt as varchar),1,4)) as p1avdt  FROM AS400DB01.ILOD0001.ilms01 WITH (NOLOCK) WHERE p1apno =  { appNo} AND p1brn = { brn}  ";
                var result = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result;
                if (result.success)
                {
                    ds = result.data;

                }
                return ds;
            }
            catch (Exception ex)
            {
                return ds;
            }
        }

        public DataSet getGNTB262()
        {
            DataSet ds = new DataSet();
            //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_UserInfo;
            try
            {
                string sql = "SELECT G62RCD,G62TDS FROM AS400DB01.GNOD0000.GNTB262 WITH (NOLOCK) WHERE G62SEL = 'Y' AND G62ILF = 'Y' AND G62DEL=' ' ";
                var result = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result;
                if (result.success)
                {
                    ds = result.data;

                }
                return ds;
            }
            catch (Exception ex)
            {
                return ds;
            }
        }
        public DataSet RP_SummaryReject(string WBDATF, string WBDATT, string WBRN, string WAPP_, string WPCODE)
        {
            ILDataCenter ilobj = new ILDataCenter();
            DataSet ds = new DataSet();
            //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_UserInfo;
            try
            {
                string PDATEF = WBDATF.Substring(4, 4).ToString() + WBDATF.Substring(2, 2).ToString() + WBDATF.Substring(0, 2).ToString();
                string PDATET = WBDATT.Substring(4, 4).ToString() + WBDATT.Substring(2, 2).ToString() + WBDATT.Substring(0, 2).ToString();
                string sql = $@"EXEC [AS400DB01].[ILOD0001].[sp_GetSummaryCanceledRejectedReport]
		                    N'{PDATEF.ToString()}',
		                    N'{PDATET.ToString()}',
		                    N'{WBRN.ToString()}',
		                    N'{WAPP_.ToString()}',
		                    N'{WPCODE.ToString()}' ";
                ds = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
                if (!ilobj.check_dataset(ds))
                {
                    DataTable dt = new DataTable();
                    dt.Clear();
                    dt.Columns.Add("BRANCH_CODE");
                    dt.Columns.Add("BRANCH_NAME");
                    dt.Columns.Add("Status");
                    dt.Columns.Add("REASON_CODE");
                    dt.Columns.Add("DESCRIPTION");
                    dt.Columns.Add("CASE");
                    dt.Columns.Add("LOANT_AMOUNT");
                    dt.Columns.Add("PERCENT_LOAN");
                    dt.Columns.Add("CONTRACT_AMOUNT");
                    dt.Columns.Add("PERCENT_CONTRACT");
                    ds.Tables.Add(dt);
                }
                //bool Rescall = true;//CALL_ILR0502C2(WBDATF, WBDATT, WBRN, WAPP_, WPCODE, m_UserInfo.BizInit, m_UserInfo.BranchNo);
                //if (Rescall)
                //{
                //    //string sql = " SELECT case when (SUBSTRING(F198,1,1) = 0 OR SUBSTRING(F198,1,1) = '') THEN SUBSTRING(F198,2,198)  ELSE '' END   F198  ,case when (SUBSTRING(F198,1,1) = 0 OR SUBSTRING(F198,1,1) = '') THEN 0 ELSE '1' END  newpage " +
                //    //                 " FROM qtemp/text198 ";
                //    //string sql_198 = " SELECT case   substr(F198,2,198) F198 " +
                //    //                 " FROM qtemp/text198 ";

                //    string condition = "";

                //    sql += condition;
                //    var result = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result;
                //    if (result.success)
                //    {
                //        ds = result.data;

                //    }
                //    return ds;

                //}

            }
            catch (Exception ex)
            {
                return ds;
            }
            return ds;
        }


        public DataSet getILTB01(string code = "")
        {
            DataSet ds = new DataSet();
            //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_UserInfo;
            try
            {
                string condition = "";
                string sql = @"SELECT T1BRN, T1BNME FROM AS400DB01.ILOD0001.ILTB01 WITH (NOLOCK) ";
                if (code.Trim() != "")
                {
                    condition += " WHERE T1BRN = " + code;
                }

                sql += condition;
                var result = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result;
                if (result.success)
                {
                    ds = result.data;

                }
                return ds;
            }
            catch (Exception ex)
            {
                return ds;
            }
        }
        public DataSet getApplyVia()
        {
            DataSet ds = new DataSet();
            //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_UserInfo;
            try
            {
                string sql = @"select gn16cd, gn16dt from AS400DB01.GNOD0000.gntb16 WITH (NOLOCK) where SUBSTRING(GN16FL,2,1) = 1 order by gn16cd asc";

                var result = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result;
                if (result.success)
                {
                    ds = result.data;

                }
                return ds;
            }
            catch (Exception ex)
            {
                return ds;
            }
        }

        public DataSet getApplyChannel()
        {
            DataSet ds = new DataSet();
            //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_UserInfo;
            try
            {
                string sql = @"select gs16cd, gs16dt from AS400DB01.GNOD0000.gnts16 WITH (NOLOCK) order by gs16cd";

                var result = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result;
                if (result.success)
                {
                    ds = result.data;

                }
                return ds;
            }
            catch (Exception ex)
            {
                return ds;
            }
        }
        public DataSet getApplySubChannel(string applych)
        {
            DataSet ds = new DataSet();
            //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_UserInfo;
            try
            {
                string sql = $@"select gs17sc, gs17dt from AS400DB01.GNOD0000.gnts17 WITH (NOLOCK) where gs17cd = '{applych}' order by gs17sc ";

                var result = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result;
                if (result.success)
                {
                    ds = result.data;

                }
                return ds;
            }
            catch (Exception ex)
            {
                return ds;
            }
        }

        public DataSet getSubMaritalStatus(string codeMarital, string code)
        {
            DataSet ds = new DataSet();
            //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_UserInfo;
            try
            {
                string condition = "";
                string sql = "";
                if (codeMarital != "")
                {
                    sql = $@"SELECT [S44MRT],[S44SMT],[S44TDS],[S44EDS],[S44FIL],[S44UDT],[S44UTM],[S44UUS],[S44UPG],[S44UDS],[S44STS] FROM [AS400DB01].[GNOD0000].[GNTS44] WITH (NOLOCK)
                                WHERE S44MRT =  {codeMarital}";

                    if (code.Trim() != "")
                    {
                        condition = " AND S44MRT = '" + code + "'";
                    }
                }
                else
                {
                    sql = $@"SELECT [S44MRT],[S44SMT],[S44TDS],[S44EDS],[S44FIL],[S44UDT],[S44UTM],[S44UUS],[S44UPG],[S44UDS],[S44STS] FROM [AS400DB01].[GNOD0000].[GNTS44] WITH (NOLOCK) ";
                }


                string orderBy = " ORDER BY s44mrt,s44smt ";
                sql += condition + orderBy;


                var result = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result;
                if (result.success)
                {
                    ds = result.data;

                }
                return ds;
            }
            catch (Exception ex)
            {
                return ds;
            }
        }
        public DataSet getBusinessType(string code)
        {
            DataSet ds = new DataSet();
            //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_UserInfo;
            try
            {
                string condition = "";
                string sql = "SELECT [GN22CD],[GN22DT],[GN22DE],[GN22FL],[GN22UD],[GN22UT],[GN22US],[GN22WS],[GN22DL] FROM [AS400DB01].[GNOD0000].[GNTB22] WITH (NOLOCK)  ";
                if (code.Trim() != "")
                {
                    condition = " WHERE GN22CD = '" + code + "'";
                }
                string orderBy = " ORDER BY gn22cd ";
                sql += condition + orderBy;


                var result = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result;
                if (result.success)
                {
                    ds = result.data;

                }
                return ds;
            }
            catch (Exception ex)
            {
                return ds;
            }
        }

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

        public DataSet getEmployeeType(string code)
        {
            DataSet ds = new DataSet();
            //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_UserInfo;
            try
            {
                string condition = "";
                string sql = @" SELECT CODE AS GN68CD, DescriptionTHAI AS GN68DT,DescriptionENG AS GN68DE, UpdateDate ,UpdateBy FROM GeneralDB01.GeneralInfo.GeneralCenter WITH (NOLOCK) WHERE Type = 'EmploymentTypeID'  ";
                if (code.Trim() != "")
                {
                    condition = "AND Code = '" + code + "'";
                }
                string orderBy = " ORDER BY Code ";

                sql += condition + orderBy;

                var result = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result;
                if (result.success)
                {
                    ds = result.data;

                }
                return ds;
            }
            catch (Exception ex)
            {
                return ds;
            }
        }

        public DataSet getSalaryType(string code)
        {
            DataSet ds = new DataSet();
            //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_UserInfo;
            try
            {
                string condition = "";
                string sql = "SELECT [GN17CD],[GN17DT],[GN17DE],[GN17FL],[GN17UD],[GN17UT],[GN17US],[GN17WS],[GN17DL] FROM [AS400DB01].[GNOD0000].[GNTB17] WITH (NOLOCK) ";
                if (code.Trim() != "")
                {
                    condition = " WHERE gn17cd = '" + code + "'";
                }
                string orderBy = " ORDER BY gn17cd ";

                sql += condition + orderBy;

                var result = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result;
                if (result.success)
                {
                    ds = result.data;

                }
                return ds;
            }
            catch (Exception ex)
            {
                return ds;
            }
        }
        public DataSet getslipdoc()
        {
            DataSet ds = new DataSet();
            //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_UserInfo;
            try
            {
                string condition = "";
                string sql = @"SELECT GS26CD, GS26DT, DescriptionTHAI as gs25de FROM AS400DB01.[GNOD0000].[GNTS26] WITH (NOLOCK)
                            LEFT JOIN GeneralDB01.GeneralInfo.GeneralCenter WITH (NOLOCK) ON (gs26sc = Code AND Type = 'CalculateIncomeID')";

                var result = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result;
                if (result.success)
                {
                    ds = result.data;

                }
                return ds;
            }
            catch (Exception ex)
            {
                return ds;
            }
        }
        public DataSet getContract()
        {
            DataSet ds = new DataSet();
            //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_UserInfo;
            try
            {
                string condition = "";
                string sql = @" SELECT gn70cd, gn70dt FROM AS400DB01.GNOD0000.gntb70 WITH (NOLOCK) ";

                var result = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result;
                if (result.success)
                {
                    ds = result.data;

                }
                return ds;
            }
            catch (Exception ex)
            {
                return ds;
            }
        }

        public DataSet getILTB40(string desc)
        {
            DataSet ds = new DataSet();
            //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_UserInfo;
            try
            {
                string sql = " SELECT * FROM [AS400DB01].[ILOD0001].ILTB40 WITH (NOLOCK) LEFT JOIN  [AS400DB01].[ILOD0001].ILTB00 WITH (NOLOCK) ON T40LTY=T00LTY WHERE t40del = '' ";
                string cond = "";
                if (desc != "")
                {
                    cond = " AND UPPER(T40TYP)  like '" + desc.ToUpper() + "%'  OR UPPER(T40DES) like '" + desc.ToUpper() + "%'";
                }

                sql += cond;
                var result = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result;
                if (result.success)
                {
                    ds = result.data;

                }
                return ds;
            }
            catch (Exception ex)
            {
                return ds;
            }
        }

        public DataSet getILTB41(string desc, string prodType)
        {
            DataSet ds = new DataSet();
            //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_UserInfo;
            try
            {
                string sql = " SELECT * FROM [AS400DB01].[ILOD0001].ILTB41 WITH (NOLOCK) WHERE T41TYP = " + prodType + "  AND  T41DEL =  '' ";
                string cond = "";
                if (desc != "")
                {
                    cond = " AND (UPPER(T41COD)  like '" + desc.ToUpper() + "%'  OR UPPER(T41DES) like '" + desc.ToUpper() + "%') ";
                }

                sql += cond;

                var result = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result;
                if (result.success)
                {
                    ds = result.data;

                }
                return ds;
            }
            catch (Exception ex)
            {
                return ds;
            }
        }

        public DataSet getILTB42(string desc)
        {
            DataSet ds = new DataSet();
            //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_UserInfo;
            try
            {
                string sql = " SELECT * FROM [AS400DB01].[ILOD0001].ILTB42 WITH (NOLOCK) ";
                string cond = "";
                if (desc != "")
                {
                    cond = " WHERE  UPPER(T42BRD)  like '" + desc.ToUpper() + "%'  OR UPPER(T42DES) like '" + desc.ToUpper() + "%'";
                }

                sql += cond;
                var result = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result;
                if (result.success)
                {
                    ds = result.data;

                }
                return ds;
            }
            catch (Exception ex)
            {
                return ds;
            }
        }
        public DataSet getILTB06()
        {
            DataSet ds = new DataSet();
            //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_UserInfo;
            try
            {
                string condition = "";
                string sql = " SELECT T06LON,T06DUT FROM [AS400DB01].[ILOD0001].ILTB06 WITH (NOLOCK) ";
                var result = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result;
                if (result.success)
                {
                    ds = result.data;

                }
                return ds;
            }
            catch (Exception ex)
            {
                return ds;
            }
        }
        public DataSet getCampaign(string vendorCode, string totalTerm, string campCode = "", string campSeq = "", string camRsq = "", string appdate = "")
        {
            DataSet ds = new DataSet();
            //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_UserInfo;
            try
            {
                string sql = "";
                if (campCode == "")
                {
                    sql = " SELECT c01cmp, c01cnm,c01fin,c02ifr, c02csq,c02rsq ,C01CTY,C02AIR,C02ACR,c02tot,c02fmt " +
                                 " FROM [AS400DB01].[ILOD0001].ilcp08 WITH (NOLOCK) " +
                                 " LEFT JOIN [AS400DB01].[ILOD0001].ilcp01 WITH (NOLOCK) on (c08cmp=c01cmp and C01SDT <= " + appdate.Trim() + " AND C01EDT >= " + appdate.Trim() + ") " +
                                 " LEFT JOIN [AS400DB01].[ILOD0001].ilcp02 WITH (NOLOCK) ON (c08cmp=c02cmp) " +
                                 " LEFT JOIN [AS400DB01].[ILOD0001].ilcp06 WITH (NOLOCK) ON (c08cmp=c06cmp) " +
                                 " LEFT JOIN [AS400DB01].[ILOD0001].ilms10 WITH (NOLOCK) ON (c08ven=p10ven) " +
                                 " LEFT JOIN [AS400DB01].[ILOD0001].ilcp09 WITH (NOLOCK) ON (c08cmp=c09cmp) " +
                                 " WHERE c09brn = " + m_UserInfo.BranchApp + " AND c01cst='A' " +
                                 " AND ((c08ven = " + (!string.IsNullOrEmpty(vendorCode) ? vendorCode : "0") + ") OR (c08ven=0))  AND c08rst='' " +
                                 " AND c06apt='01' AND " + appdate.Trim() + " <= C01CAD AND c01cty='R' " +
                                 " and c02ttm = " + totalTerm + " and c02tot = " + totalTerm + " " +
                                 " AND not exists " +
                                 " (SELECT C81CMP,C81VEN,C81END,C81FIL FROM [AS400DB01].[ILOD0001].ilcp081 WITH (NOLOCK) WHERE ilcp08.c08cmp = c81cmp and c81ven= " + (!string.IsNullOrEmpty(vendorCode) ? vendorCode : "0") +
                                 " AND c81end <= " + appdate.Trim() + ") " +
                                 " ORDER BY c08cmp,c02csq,c02rsq ";
                }
                else if (campCode != "" && campSeq != "" && camRsq == "")
                {
                    sql = " SELECT c01nxd,c01fin,c01mkc,c02rsq,c02ttm,c02ifr,c02inr,c02crr,C01CTY,C02AIR,C02ACR, " +
                          " c02tot,c02fmt FROM [AS400DB01].[ILOD0001].ilcp02 WITH (NOLOCK) " +
                          " LEFT JOIN [AS400DB01].[ILOD0001].ilcp01 WITH (NOLOCK) ON c02cmp=c01cmp " +
                          " WHERE c02cmp = " + campCode +
                          " AND c02csq = " + campSeq + "AND C01SDT <= " + appdate.Trim() + " AND C01EDT >= " + appdate.Trim() + " ";

                }
                else if (campCode != "" && campSeq != "" && camRsq != "")
                {
                    sql = " SELECT c01nxd,c01fin,c01mkc,c02rsq,c02ttm,c02ifr,c02inr,c02crr,C01CTY,C02AIR,C02ACR, " +
                          " c02tot,c02fmt FROM [AS400DB01].[ILOD0001].ilcp02 WITH (NOLOCK) " +
                          " LEFT JOIN [AS400DB01].[ILOD0001].ilcp01 WITH (NOLOCK) ON c02cmp=c01cmp " +
                          " WHERE c02cmp = " + campCode + 
                          " AND c02csq = " + campSeq +
                          " AND C02RSQ = " + camRsq  + "AND C01SDT <= " + appdate.Trim() + " AND C01EDT >= " + appdate.Trim() + " " ;

                }
                var result = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result;
                if (result.success)
                {
                    ds = result.data;

                }
                return ds;
            }
            catch (Exception ex)
            {
                return ds;
            }
        }

        public DataSet getGNMX01(string appdate)
        {
            DataSet ds = new DataSet();
            //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_UserInfo;
            try
            {
                string sql = " SELECT [MX1APP],[MX1INT],[MX1CRU],[MX1PNT],[MX1INF],[MX1INA],[MX1MAX],[MX1STD],[MX1END],[MX1UPD],[MX1UPT],[MX1UPU],[MX1STS] FROM [AS400DB01].[GNOD0000].[GNMX01] WITH (NOLOCK) " +
                            " WHERE mx1app = 'IL' " +
                            " AND " + appdate + ">= MX1STD " +
                            " AND " + appdate + "<= MX1END ";
                var result = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result;
                if (result.success)
                {
                    ds = result.data;

                }
                return ds;
            }
            catch (Exception ex)
            {
                return ds;
            }
        }

        public async Task<DataSet> getTambol(string code, string name = "")
        {
            DataSet ds = new DataSet();
            //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_UserInfo;
            try
            {
                string condition = " WHERE 1 = 1";
                string sql = " SELECT Code AS gn18cd, DescriptionTHAI AS gn18dt FROM GeneralDB01.GeneralInfo.AddrTambol  WITH (NOLOCK) ";

                if (code.Trim() != "")
                {
                    condition += " AND  Code = '" + code + "'";
                }
                if (name.Trim() != "")
                {
                    condition += " AND DescriptionTHAI = '" + name + "'";
                }

                sql += condition;
                var result = await _dataCenter.GetDataset<DataTable>(sql, CommandType.Text);
                if (result.success)
                    ds = result.data;
                return await Task.FromResult<DataSet>(ds);
            }
            catch (Exception ex)
            {
                return await Task.FromResult<DataSet>(ds);
            }
        }
        public async Task<DataSet> getAmphur(string nameAmphur, string nameTambol, string code = "")
        {
            DataSet ds = new DataSet();
            //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_UserInfo;
            try
            {
                string sql = "";

                iDB2Command cmd = new iDB2Command();
                if (nameTambol.Trim() != "")
                {
                    sql = " SELECT Au.Code AS gn19cd, Au.DescriptionTHAI AS gn19dt FROM GeneralDB01.GeneralInfo.AddrRelation Ar  WITH (NOLOCK) " +
                          " JOIN GeneralDB01.GeneralInfo.AddrAumphur Au  WITH (NOLOCK)  ON (Au.ID = Ar.AumphurID) " +
                          $" WHERE TambolID IN (SELECT ID FROM GeneralDB01.GeneralInfo.AddrTambol  WITH (NOLOCK)  WHERE DescriptionTHAI = '{nameTambol}') " +
                          " ORDER BY Au.DescriptionTHAI ";

                    //cmd.CommandText = sql;
                    //cmd.Parameters.Add("@tambol", nameTambol.Trim());
                }
                else if (nameAmphur.Trim() != "")
                {
                    sql = " SELECT Code AS  gn19cd, DescriptionTHAI AS gn19dt FROM GeneralDB01.GeneralInfo.AddrAumphur  WITH (NOLOCK) " +
                          $" WHERE DescriptionTHAI = '{nameAmphur}' ";
                    //cmd.CommandText = sql;
                    //cmd.Parameters.Add("@amphur", nameAmphur.Trim());
                }
                else if (code.Trim() != "")
                {
                    sql = " select Code AS  gn19cd, DescriptionTHAI AS gn19dt FROM GeneralDB01.GeneralInfo.AddrAumphur  WITH (NOLOCK)  " +
                          " where Code  = " + code;
                    //cmd.CommandText = sql;
                }

                var result = await _dataCenter.GetDataset<DataTable>(sql, CommandType.Text);
                if (result.success)
                    ds = result.data;
                return await Task.FromResult<DataSet>(ds);
            }
            catch (Exception ex)
            {
                return await Task.FromResult<DataSet>(ds);
            }
        }

        public async Task<DataSet> getProvince(string tambol, string amphur, string code = "")
        {
            DataSet ds = new DataSet();
            //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_UserInfo;
            try
            {
                iDB2Command cmd = new iDB2Command();
                string sql = "";
                if (tambol == "" && amphur != "")
                {
                    sql = " SELECT Ap.Code AS gn20cd, Ap.DescriptionTHAI AS gn20dt, Ar.ZipCode AS  gn21zp, At.Code AS gn18cd, At.DescriptionTHAI AS gn18dt " +
                          " FROM GeneralDB01.GeneralInfo.AddrRelation Ar WITH (NOLOCK) " +
                          " JOIN GeneralDB01.GeneralInfo.AddrProvince Ap WITH (NOLOCK) ON (Ap.ID = Ar.ProvinceID)" +
                          " JOIN GeneralDB01.GeneralInfo.AddrTambol At WITH (NOLOCK) ON (At.ID = Ar.TambolID)" +
                          $" WHERE Ar.AumphurID in (SELECT ID FROM GeneralDB01.GeneralInfo.AddrAumphur WITH (NOLOCK) WHERE DescriptionTHAI = '{amphur}') ";
                    //+
                    //      " AND gn21pr=gn20cd and gn21tm=gn18cd ";
                    //cmd.CommandText = sql;
                    //cmd.Parameters.Add("@amphur", amphur.Trim());

                }
                else if (tambol != "" && amphur != "")
                {
                    sql = " SELECT Ap.Code AS gn20cd, Ap.DescriptionTHAI AS gn20dt, Ar.ZipCode AS  gn21zp, At.Code AS gn18cd, At.DescriptionTHAI AS gn18dt " +
                          " FROM GeneralDB01.GeneralInfo.AddrRelation Ar  WITH (NOLOCK) " +
                          " JOIN GeneralDB01.GeneralInfo.AddrProvince Ap  WITH (NOLOCK)  ON (Ap.ID = Ar.ProvinceID)" +
                          " JOIN GeneralDB01.GeneralInfo.AddrTambol At  WITH (NOLOCK)  ON (At.ID = Ar.TambolID)" +
                          $" WHERE Ar.TambolID IN (SELECT ID FROM GeneralDB01.GeneralInfo.AddrTambol  WITH (NOLOCK) WHERE DescriptionTHAI = '{tambol}') " +
                          $" AND Ar.AumphurID in (SELECT ID FROM GeneralDB01.GeneralInfo.AddrAumphur  WITH (NOLOCK) WHERE DescriptionTHAI = '{amphur}')  ";
                    //+
                    //" AND gn21pr=gn20cd AND gn21tm=gn18cd ";
                    //cmd.CommandText = sql;
                    //cmd.Parameters.Add("@tambol", tambol.Trim());
                    //cmd.Parameters.Add("@amphur", amphur.Trim());
                }
                else if (code != "")
                {
                    sql = " select Code AS gn20cd, DescriptionTHAI AS gn20dt from GeneralDB01.GeneralInfo.AddrProvince   WITH (NOLOCK)  where Code = " + code;
                    cmd.CommandText = sql;
                }
                var result = await _dataCenter.GetDataset<DataTable>(sql, CommandType.Text);
                if (result.success)
                    ds = result.data;
                return await Task.FromResult<DataSet>(ds);
            }
            catch (Exception ex)
            {
                return await Task.FromResult<DataSet>(ds);
            }
        }

        public async Task<DataSet> getAddress(string desc)
        {
            DataSet ds = new DataSet();
            //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_UserInfo;
            try
            {
                iDB2Command cmd = new iDB2Command();
                cmd.Parameters.Clear();

                string condition = "";

                if (desc.Trim() != "")
                {
                    condition = $" WHERE Atm.DescriptionTHAI LIKE '%{desc}%' OR Aa.DescriptionTHAI LIKE '%{desc}%' OR Ap.DescriptionTHAI LIKE '%{desc}%' OR ZipCode LIKE '%{desc}%' ";
                }

                string sql = @"SELECT CONCAT(TRIM(Atm.DescriptionTHAI),'-',TRIM(Aa.DescriptionTHAI),'-',TRIM(Ap.DescriptionTHAI),'-', TRIM(ZipCode)) AS [Address],
                                CONCAT(FORMAT(CONVERT(INT,Atm.Code),'00000'),'-', FORMAT(CONVERT(INT,Aa.Code),'0000'), '-', FORMAT(CONVERT(INT,Ap.Code),'000'), '-', ZipCode) As Code
                                FROM GeneralDB01.GeneralInfo.AddrRelation Ar  WITH (NOLOCK) 
                                JOIN GeneralDB01.GeneralInfo.AddrTambol Atm  WITH (NOLOCK) ON Ar.TambolID = Atm.ID
                                JOIN GeneralDB01.GeneralInfo.AddrAumphur Aa  WITH (NOLOCK)  ON Ar.AumphurID = Aa.ID
                                JOIN GeneralDB01.GeneralInfo.AddrProvince Ap  WITH (NOLOCK) ON Ar.ProvinceID = Ap.ID " + condition;

                var result = await _dataCenter.GetDataset<DataTable>(sql, CommandType.Text);
                if (result.success)
                    ds = result.data;
                return await Task.FromResult<DataSet>(ds);
            }
            catch (Exception ex)
            {
                return await Task.FromResult<DataSet>(ds);
            }
        }

        public DataSet getProductType(string desc)
        {
            DataSet ds = new DataSet();
            try
            {
                string condition = "";
                string sql = @"SELECT T40TYP,T40DES FROM AS400DB01.ILOD0001.ILTB40 WITH (NOLOCK) LEFT JOIN  AS400DB01.ILOD0001.ILTB00 WITH (NOLOCK) ON T40LTY=T00LTY WHERE t40del = ''  ";
                if (desc.Trim() != "")
                {
                    condition += " AND UPPER(T40TYP)  like '" + desc.ToUpper() + "%'  OR UPPER(T40DES) like '" + desc.ToUpper() + "%'";
                }

                sql += condition;
                var result = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result;
                if (result.success)
                {
                    ds = result.data;

                }
                return ds;
            }
            catch (Exception ex)
            {
                return ds;
            }
        }

        public DataSet getProductBrand(string desc)
        {
            DataSet ds = new DataSet();
            try
            {
                string condition = "";
                string sql = @"SELECT T42BRD,T42DES FROM AS400DB01.ILOD0001.ILTB42 WITH (NOLOCK)  ";
                if (desc.Trim() != "")
                {
                    condition += " WHERE  UPPER(T42BRD)  like '" + desc.ToUpper() + "%'  OR UPPER(T42DES) like '" + desc.ToUpper() + "%'";
                }

                sql += condition;
                var result = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result;
                if (result.success)
                {
                    ds = result.data;

                }
                return ds;
            }
            catch (Exception ex)
            {
                return ds;
            }
        }
        public DataSet getProductCode(string desc, string prodType)
        {
            DataSet ds = new DataSet();
            try
            {
                string condition = "";
                string sql = @"SELECT T41COD,T41DES FROM AS400DB01.ILOD0001.ILTB41 WITH (NOLOCK) WHERE T41TYP = " + prodType + "AND  T41DEL =  '' ";
                if (desc.Trim() != "")
                {
                    condition += " AND (UPPER(T41COD)  like '" + desc.ToUpper() + "%'  OR UPPER(T41DES) like '" + desc.ToUpper() + "%') ";
                }

                sql += condition;
                var result = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result;
                if (result.success)
                {
                    ds = result.data;

                }
                return ds;
            }
            catch (Exception ex)
            {
                return ds;
            }
        }

        public DataSet getResultCode()
        {
            DataSet ds = new DataSet();
            try
            {
                string sql = @"SELECT g25rcd, g25des FROM AS400DB01.GNOD0000.GNTB25 WITH (NOLOCK) ORDER BY g25rcd  ";
                var result = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result;
                if (result.success)
                {
                    ds = result.data;

                }
                return ds;
            }
            catch (Exception ex)
            {
                return ds;
            }
        }

        public DataSet getTypeBusiness(string code)
        {
            DataSet ds = new DataSet();
            try
            {
                string sql = @"SELECT Code, DescriptionTHAI FROM GeneralDB01.GeneralInfo.GeneralCenter WITH (NOLOCK) where Type = 'CompanyBusinessID'  ";
                string condition = "";
                if (code.Trim() != "")
                {
                    condition += " and  Code = '" + code + "'  order by Code";
                }

                sql += condition;
                var result = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result;
                if (result.success)
                {
                    ds = result.data;

                }
                return ds;
            }
            catch (Exception ex)
            {
                return ds;
            }
        }

        public DataSet getGeneralCenter(string type, string id = "", string code = "")
        {
            string condition = $" WHERE  [Type]='{type}'";
            if (!string.IsNullOrEmpty(id))
            {
                condition += $" AND [ID]={id}";
            }

            if (!string.IsNullOrEmpty(code))
            {
                condition += $" AND [code]='{code}'";

            }

            DataSet ds = new DataSet();
            try
            {
                string sql = $@"SELECT  [ID]
                                          ,[Type]
                                          ,[Code]
                                          ,[DescriptionTHAI]
                                          ,[DescriptionENG]
                                          ,[ShortName]
                                          ,[Sorting]
                                          ,[RecordStatus]
                                          ,[Application]
                                          ,[CreateBy]
                                          ,[CreateDate]
                                          ,[UpdateBy]
                                          ,[UpdateDate]
                                          ,[IsDelete]
                                      FROM [GeneralDB01].[GeneralInfo].[GeneralCenter]  WITH (NOLOCK)  {condition} order by Sorting asc";
                var result = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result;
                if (result.success)
                {
                    ds = result.data;

                }
                return ds;
            }
            catch (Exception ex)
            {
                return ds;
            }
        }
    }
    public class ILDataSubroutine
    {
        private string url;
        private ulong m_UdpD = Convert.ToUInt32(DateTime.Now.ToString("yyyyMMdd", new CultureInfo("th-TH")));
        private ulong m_UdpT = Convert.ToUInt32(DateTime.Now.ToString("HHmmss", new CultureInfo("th-TH")));
        private string m_UserName;
        private string m_User;
        private string m_Wrkstn = "";
        private UserInfo m_UserInfo;


        public ILDataSubroutine(UserInfo userInfo)
        {
            url = WebConfigurationManager.AppSettings["hisunApi"].ToString().Trim();
            //m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            m_UserInfo = userInfo;
        }

        public bool Call_ILSR97(string prmWKCDE, string prmWKFMT, string strBizInit, string strBranchNo, ref string prmWKDTE)
        {
            DataSet dsResult = new DataSet();
            ESBiDB2.iDB2Command cmd = new ESBiDB2.iDB2Command();
            try
            {

                //GeneralLib;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "ILSR97";

                //gentoken
                cmd.Token = ReuseableToken.token;
                cmd.AccessKey = m_UserInfo.AccessKey; //acn
                cmd.Url = url; //api hisun

                //// INPUT
                cmd.Parameters.Add("WKCDE", ESBiDB2.iDB2DbType.iDB2Char, 2).Value = prmWKCDE.ToString().Trim();
                cmd.Parameters.Add("WKFMT", ESBiDB2.iDB2DbType.iDB2Char, 3).Value = prmWKFMT.ToString().Trim();

                //// OUTPUT
                cmd.Parameters.Add("WKDTE", ESBiDB2.iDB2DbType.iDB2Decimal, 0).Direction = ParameterDirection.Output;
                cmd.Parameters["WKDTE"].Precision = 8;
                cmd.Parameters["WKDTE"].Scale = 0;

                Utility.WriteLogResponse("Request to : " + url + "/" + cmd.CommandText.ToString());
                dsResult = cmd.ExecuteReaderOptionalDataSet();
                Utility.WriteLogResponse("Response from : " + url + "/" + cmd.CommandText.ToString(), JsonConvert.SerializeObject(dsResult));

                if (dsResult.Tables.Count > 0)
                {
                    Utility.CheckSubroutineStatus(dsResult);
                    prmWKDTE = cmd.Parameters["WKDTE"]?.Value?.ToString().Trim();
                    if (prmWKDTE == null)
                    {
                        prmWKDTE = "";
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Utility.WriteLog(ex);
                return false;
            }
        }
        public DataSet Get_GNSRAM(string prmstrIDNo, string prmstrName, string prmstrSurname, string prmstrBirthDateYMD, string prmstrPROC,
            string strBizInit, string strBranchNo)
        {
            DataSet dsResult = new DataSet();
            DataSet ds = new DataSet();
            ESBiDB2.iDB2Command cmd = new ESBiDB2.iDB2Command();
            try
            {
                //GeneralLib;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GNSRAM";

                //gentoken
                cmd.Token = ReuseableToken.token;
                cmd.AccessKey = m_UserInfo.AccessKey; //acn
                cmd.Url = url; //api hisun

                // INPUT
                cmd.Parameters.Add("PIIDNO", iDB2DbType.iDB2Char, 15).Value = prmstrIDNo.Trim();
                cmd.Parameters.Add("PINAME", iDB2DbType.iDB2Char, 50).Value = prmstrName.Trim();
                cmd.Parameters.Add("PISNME", iDB2DbType.iDB2Char, 50).Value = prmstrSurname.Trim();
                cmd.Parameters.Add("PIBDTE", iDB2DbType.iDB2Char, 8).Value = prmstrBirthDateYMD.Trim();
                cmd.Parameters.Add("PIPROC", iDB2DbType.iDB2Char, 3).Value = prmstrPROC;
                //OUTPUT DATA                                         
                cmd.Parameters.Add("POAGRP", iDB2DbType.iDB2Char, 3).Direction = ParameterDirection.Output;  //Amlo Group
                cmd.Parameters.Add("POAFGC", iDB2DbType.iDB2Char, 20).Direction = ParameterDirection.Output;  //Amlo Message
                cmd.Parameters.Add("POMTYP", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;  //Match Type
                cmd.Parameters.Add("POERR", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;  //Flag Error
                cmd.Parameters.Add("WPOLR", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;  //SETON LR

                Utility.WriteLogResponse("Request to : " + url + "/" + cmd.CommandText.ToString());
                ds = cmd.ExecuteReaderOptionalDataSet();
                Utility.WriteLogResponse("Response from : " + url + "/" + cmd.CommandText.ToString(), JsonConvert.SerializeObject(ds));
                if (ds.Tables.Count > 0)
                {
                    Utility.CheckSubroutineStatus(ds);
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
                    if (cmd.Parameters["POAGRP"]?.Value?.ToString().Trim() != "")
                    {
                        dr["POAGRP"] = cmd.Parameters["POAGRP"]?.Value?.ToString().Trim() == null ? "" : cmd.Parameters["POAGRP"]?.Value?.ToString().Trim();
                    }
                    if (cmd.Parameters["POAFGC"]?.Value?.ToString().Trim() != "")
                    {
                        dr["POAFGC"] = cmd.Parameters["POAFGC"]?.Value?.ToString().Trim() == null ? "" : cmd.Parameters["POAFGC"]?.Value?.ToString().Trim();
                    }
                    if (cmd.Parameters["POMTYP"]?.Value?.ToString().Trim() != "")
                    {
                        dr["POMTYP"] = cmd.Parameters["POMTYP"]?.Value?.ToString().Trim() == null ? "" : cmd.Parameters["POMTYP"]?.Value?.ToString().Trim();
                    }
                    if (cmd.Parameters["POERR"]?.Value?.ToString().Trim() != "")
                    {
                        dr["POERR"] = cmd.Parameters["POERR"]?.Value?.ToString().Trim() == null ? "" : cmd.Parameters["POERR"]?.Value?.ToString().Trim();
                    }
                    if (cmd.Parameters["WPOLR"]?.Value?.ToString().Trim() != "")
                    {
                        dr["WPOLR"] = cmd.Parameters["WPOLR"]?.Value?.ToString().Trim() == null ? "" : cmd.Parameters["WPOLR"]?.Value?.ToString().Trim();
                    }
                    dt.Rows.Add(dr);
                    dsResult.Tables.Add(dt);
                }
                return dsResult;
            }
            catch (Exception ex)
            {
                Utility.WriteLog(ex);
                return dsResult;
            }
        }
        public bool Call_GNSRCID(string prmID, ref string prmError, string strBizInit, string strBranchNo)
        {
            DataSet dsResult = new DataSet();
            ESBiDB2.iDB2Command cmd = new ESBiDB2.iDB2Command();
            try
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GNSRCID";

                //gentoken
                cmd.Token = ReuseableToken.token;
                cmd.AccessKey = m_UserInfo.AccessKey; //acn
                cmd.Url = url; //api hisun

                cmd.Parameters.Add("WIIDNO", iDB2DbType.iDB2Char, 15).Value = prmID;
                cmd.Parameters.Add("ERROR", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;

                Utility.WriteLogResponse("Request to : " + url + "/" + cmd.CommandText.ToString());
                dsResult = cmd.ExecuteReaderOptionalDataSet();
                Utility.WriteLogResponse("Response from : " + url + "/" + cmd.CommandText.ToString(), JsonConvert.SerializeObject(dsResult));

                if (dsResult.Tables.Count > 0)
                {
                    Utility.CheckSubroutineStatus(dsResult);
                    prmError = cmd.Parameters["ERROR"]?.Value?.ToString();
                    if (prmError == null)
                    {
                        prmError = "";
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Utility.WriteLog(ex);
                return false;
            }
        }
        public bool Call_GNSR16(string csn_no, string tel, string ext, ref string prmTelType, ref string prmZip, ref string prmO_TLDS, ref string prmError, string strBizInit, string strBranchNo)
        {

            ESBiDB2.iDB2Command cmd = new ESBiDB2.iDB2Command();
            cmd.CommandType = CommandType.StoredProcedure;
            //cmd.CommandText = "RLSR256";
            //gentoken
            cmd.Token = ReuseableToken.token;
            cmd.AccessKey = m_UserInfo.AccessKey; //acn
            cmd.Url = url; //api hisun
            try
            {

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

                DataSet dsResult = new DataSet();

                Utility.WriteLogResponse("Request to : " + url + "/" + cmd.CommandText.ToString());
                var dt = cmd.ExecuteReaderOptionalDataSet();
                Utility.WriteLogResponse("Response from : " + url + "/" + cmd.CommandText.ToString(), JsonConvert.SerializeObject(dt));

                if (dt.Tables.Count > 0)
                    Utility.CheckSubroutineStatus(dt);
                var TOA = dt.Tables[0];
                //var DS = dt.Tables[1];
                prmTelType = cmd.Parameters["H_TLTY"]?.Value?.ToString().Trim();
                prmError = cmd.Parameters["WKERR"]?.Value?.ToString().Trim();
                prmO_TLDS = cmd.Parameters["O_TLDS"]?.Value?.ToString().Trim();
                prmZip = cmd.Parameters["H_ZIP"]?.Value?.ToString().Trim();
                if (prmTelType == null)
                {
                    prmTelType = "";
                }
                if (prmError == null)
                {
                    prmError = "";
                }
                if (prmO_TLDS == null)
                {
                    prmO_TLDS = "";
                }
                if (prmZip == null)
                {
                    prmZip = "";
                }
                return true;

                //var drt = cmd.ExecuteNonQuery();
                //if (drt == 1)
                //{
                //    prmTelType = cmd.Parameters["H_TLTY"].Value.ToString().Trim();
                //    prmError = cmd.Parameters["WKERR"].Value.ToString().Trim();
                //    prmO_TLDS = cmd.Parameters["O_TLDS"].Value.ToString().Trim();
                //    prmZip = cmd.Parameters["H_ZIP"].Value.ToString().Trim();
                //    return true;

                //}
                //else
                //{
                //    return false;
                //}

            }
            catch (Exception ex)
            {
                Utility.WriteLog(ex);

                return false;
            }
        }
        public bool CALL_CSSRRFAM(string CSN_IN, ref string prmError, ref string prmErrorRes, string strBizInit, string strBranchNo)
        {
            DataSet dsResult = new DataSet();
            ESBiDB2.iDB2Command cmd = new ESBiDB2.iDB2Command();

            try
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "CSSRRFCM";

                //gentoken
                cmd.Token = ReuseableToken.token;
                cmd.AccessKey = m_UserInfo.AccessKey; //acn
                cmd.Url = url; //api hisun

                // INPUT
                cmd.Parameters.Add("CSNSTR", iDB2DbType.iDB2Char, 8).Value = CSN_IN.ToString().Trim();

                // OUTPUT
                cmd.Parameters.Add("ERRFLG", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("RESFLG", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;

                Utility.WriteLogResponse("Request to : " + url + "/" + cmd.CommandText.ToString());
                dsResult = cmd.ExecuteReaderOptionalDataSet();
                Utility.WriteLogResponse("Response from : " + url + "/" + cmd.CommandText.ToString(), JsonConvert.SerializeObject(dsResult));

                if (dsResult.Tables.Count > 0)
                {
                    Utility.CheckSubroutineStatus(dsResult);
                    prmError = cmd.Parameters["ERRFLG"]?.Value?.ToString().Trim();
                    prmErrorRes = cmd.Parameters["RESFLG"]?.Value?.ToString().Trim();
                    if (prmError == null)
                    {
                        prmError = "";
                    }
                    if (prmErrorRes == null)
                    {
                        prmErrorRes = "";
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Utility.WriteLog(ex);
                return false;
            }

        }
        public bool Call_GNSRBL(string prmIID, string prmTNM, string prmTSN, ref string prmPercent, ref string prmError, ref string prmWPOLR, string strBizInit, string strBranchNo)
        {
            DataSet dsResult = new DataSet();
            ESBiDB2.iDB2Command cmd = new ESBiDB2.iDB2Command();
            try
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GNSRBL";

                //gentoken
                cmd.Token = ReuseableToken.token;
                cmd.AccessKey = m_UserInfo.AccessKey; //acn
                cmd.Url = url; //api hisun

                //// INPUT
                cmd.Parameters.Add("WPIID", iDB2DbType.iDB2Char, 15).Value = prmIID.ToString().Trim();
                cmd.Parameters.Add("WPTNM", iDB2DbType.iDB2Char, 50).Value = prmTNM.ToString().Trim();
                cmd.Parameters.Add("WPTSN", iDB2DbType.iDB2Char, 50).Value = prmTSN.ToString().Trim();

                //// OUTPUT
                cmd.Parameters.Add("WPPER", iDB2DbType.iDB2Char, 3).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("WPERR", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("WPOLR", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;

                Utility.WriteLogResponse("Request to : " + url + "/" + cmd.CommandText.ToString());
                dsResult = cmd.ExecuteReaderOptionalDataSet();
                Utility.WriteLogResponse("Response from : " + url + "/" + cmd.CommandText.ToString(), JsonConvert.SerializeObject(dsResult));

                if (dsResult.Tables.Count > 0)
                {
                    Utility.CheckSubroutineStatus(dsResult);
                    prmPercent = cmd.Parameters["WPPER"]?.Value?.ToString().Trim();
                    prmError = cmd.Parameters["WPERR"]?.Value?.ToString().Trim();
                    if (prmPercent == null)
                    {
                        prmPercent = "";
                    }
                    if (prmError == null)
                    {
                        prmError = "";
                    }
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                Utility.WriteLog(ex);
                return false;
            }

        }

        public bool Call_GNSRCS(string WPIDNO, string WPAPPL, string WPBRNO, string WPAPNO, string strBizInit, string strBranchNo, ref string WPHSTS,
                        ref string WPERR, ref string WPMSG)
        {
            DataSet dsResult = new DataSet();
            ESBiDB2.iDB2Command cmd = new ESBiDB2.iDB2Command();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "GNSRCS";

            //gentoken
            cmd.Token = ReuseableToken.token;
            cmd.AccessKey = m_UserInfo.AccessKey; //acn
            cmd.Url = url; //api hisun

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

            Utility.WriteLogResponse("Request to : " + url + "/" + cmd.CommandText.ToString());
            dsResult = cmd.ExecuteReaderOptionalDataSet();
            Utility.WriteLogResponse("Response from : " + url + "/" + cmd.CommandText.ToString(), JsonConvert.SerializeObject(dsResult));


            if (dsResult.Tables.Count > 0)
            {
                Utility.CheckSubroutineStatus(dsResult);
                WPHSTS = cmd.Parameters["WPHSTS"]?.Value?.ToString().Trim();
                WPERR = cmd.Parameters["WPERR"]?.Value?.ToString().Trim();
                WPMSG = cmd.Parameters["WPMSG"]?.Value?.ToString().Trim();
                if (WPHSTS == null)
                {
                    WPHSTS = "";
                }
                if (WPERR == null)
                {
                    WPERR = "";
                }
                if (WPMSG == null)
                {
                    WPMSG = "";
                }
                return true;
            }
            else
            {
                return false;
            }
            //try
            //{

            //}
            //catch (Exception ex)
            //{
            //    Utility.WriteLog(ex);
            //    return false;
            //}
        }

        public bool Call_GNSRNM(string prmName, string prmSurname, string prmLang, ref string prmError, ref string prmErrorMsg,
                                string strBizInit, string strBranchNo)
        {
            DataSet dsResult = new DataSet();
            ESBiDB2.iDB2Command cmd = new ESBiDB2.iDB2Command();
            try
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GNSRNM";

                //gentoken
                cmd.Token = ReuseableToken.token;
                cmd.AccessKey = m_UserInfo.AccessKey; //acn
                cmd.Url = url; //api hisun

                cmd.Parameters.Add("NAME", iDB2DbType.iDB2Char, 50).Value = prmName;
                cmd.Parameters.Add("SURNAME", iDB2DbType.iDB2Char, 50).Value = prmSurname;
                cmd.Parameters.Add("LANGUAGE", iDB2DbType.iDB2Char, 1).Value = prmLang;
                cmd.Parameters.Add("ERROR", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("MSG", iDB2DbType.iDB2Char, 50).Direction = ParameterDirection.Output;

                Utility.WriteLogResponse("Request to : " + url + "/" + cmd.CommandText.ToString());
                dsResult = cmd.ExecuteReaderOptionalDataSet();
                Utility.WriteLogResponse("Response from : " + url + "/" + cmd.CommandText.ToString(), JsonConvert.SerializeObject(dsResult));


                if (dsResult.Tables.Count > 0)
                {
                    Utility.CheckSubroutineStatus(dsResult);
                    prmError = cmd.Parameters["ERROR"]?.Value?.ToString();
                    prmErrorMsg = cmd.Parameters["MSG"]?.Value?.ToString();
                    if (prmError == null)
                    {
                        prmError = "";
                    }
                    if (prmErrorMsg == null)
                    {
                        prmErrorMsg = "";
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Utility.WriteLog(ex);
                return false;
            }
        }

        public bool Call_GNP0221(string prmDate, ref string prmError, string strBizInit, string strBranchNo)
        {
            DataSet dsResult = new DataSet();
            ESBiDB2.iDB2Command cmd = new ESBiDB2.iDB2Command();
            try
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GNP0221";

                //gentoken
                cmd.Token = ReuseableToken.token;
                cmd.AccessKey = m_UserInfo.AccessKey; //acn
                cmd.Url = url; //api hisun

                cmd.Parameters.Add("WPIDT", iDB2DbType.iDB2Char, 8).Value = prmDate;
                cmd.Parameters.Add("WPFMT", iDB2DbType.iDB2Char, 3).Value = "DMY";
                cmd.Parameters.Add("WPBOC", iDB2DbType.iDB2Char, 1).Value = "B";
                cmd.Parameters.Add("WPERR", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;

                Utility.WriteLogResponse("Request to : " + url + "/" + cmd.CommandText.ToString());
                dsResult = cmd.ExecuteReaderOptionalDataSet();
                Utility.WriteLogResponse("Response from : " + url + "/" + cmd.CommandText.ToString(), JsonConvert.SerializeObject(dsResult));


                if (dsResult.Tables.Count > 0)
                {
                    Utility.CheckSubroutineStatus(dsResult);
                    prmError = cmd.Parameters["WPERR"]?.Value?.ToString();
                    if (prmError == null)
                    {
                        prmError = "";
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Utility.WriteLog(ex);
                return false;
            }
        }
        public bool Call_GNP023(string prm1, string prm2, string prm3, string prm4, string prm5, string prm6,
                        ref string prmCalcDate, ref string prmError,
                        string strBizInit, string strBranchNo)
        {
            DataSet dsResult = new DataSet();
            ESBiDB2.iDB2Command cmd = new ESBiDB2.iDB2Command();
            try
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GNP023";

                //gentoken
                cmd.Token = ReuseableToken.token;
                cmd.AccessKey = m_UserInfo.AccessKey; //acn
                cmd.Url = url; //api hisun

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

                Utility.WriteLogResponse("Request to : " + url + "/" + cmd.CommandText.ToString());
                dsResult = cmd.ExecuteReaderOptionalDataSet();
                Utility.WriteLogResponse("Response from : " + url + "/" + cmd.CommandText.ToString(), JsonConvert.SerializeObject(dsResult));

                if (dsResult.Tables.Count > 0)
                {
                    Utility.CheckSubroutineStatus(dsResult);
                    prmError = cmd.Parameters["WPERR"]?.Value?.ToString();
                    prmCalcDate = cmd.Parameters["WPODT"]?.Value?.ToString();
                    if (prmError == null)
                    {
                        prmError = "";
                    }
                    if (prmCalcDate == null)
                    {
                        prmCalcDate = "";
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Utility.WriteLog(ex);
                return false;
            }

        }

        public bool Call_ILP036(string prmExpireDate, string prmBirthDate, string prmAppDate, string prmformatdmy, string prmyeartype, ref string prmError,
        string strBizInit, string strBranchNo)
        {
            DataSet dsResult = new DataSet();
            ESBiDB2.iDB2Command cmd = new ESBiDB2.iDB2Command();

            try
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "ILP036";

                //gentoken
                cmd.Token = ReuseableToken.token;
                cmd.AccessKey = m_UserInfo.AccessKey; //acn
                cmd.Url = url; //api hisun

                cmd.Parameters.Add("WPID1", iDB2DbType.iDB2Char, 8).Value = prmExpireDate;
                cmd.Parameters.Add("WPID2", iDB2DbType.iDB2Char, 8).Value = prmBirthDate;
                cmd.Parameters.Add("WPID3", iDB2DbType.iDB2Char, 8).Value = prmAppDate;
                cmd.Parameters.Add("WPFMT", iDB2DbType.iDB2Char, 3).Value = prmformatdmy;
                cmd.Parameters.Add("WPBOC", iDB2DbType.iDB2Char, 1).Value = prmyeartype;
                cmd.Parameters.Add("WPERR", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;

                Utility.WriteLogResponse("Request to : " + url + "/" + cmd.CommandText.ToString());
                dsResult = cmd.ExecuteReaderOptionalDataSet();
                Utility.WriteLogResponse("Response from : " + url + "/" + cmd.CommandText.ToString(), JsonConvert.SerializeObject(dsResult));

                if (dsResult.Tables.Count > 0)
                {
                    Utility.CheckSubroutineStatus(dsResult);
                    prmError = cmd.Parameters["WPERR"]?.Value?.ToString();
                    if (prmError == null)
                    {
                        prmError = "";
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Utility.WriteLog(ex);
                return false;
            }


        }

        public bool Call_GNSR48(string prmTel, string prm2, ref string prmError, ref string prmErrorMsg,
        string strBizInit, string strBranchNo, ref string prmOTYPE)
        {
            DataSet dsResult = new DataSet();
            ESBiDB2.iDB2Command cmd = new ESBiDB2.iDB2Command();

            try
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GNSR48";

                //gentoken
                cmd.Token = ReuseableToken.token;
                cmd.AccessKey = m_UserInfo.AccessKey; //acn
                cmd.Url = url; //api hisun

                cmd.Parameters.Add("ITEL#", iDB2DbType.iDB2Char, 12).Value = prmTel;
                cmd.Parameters.Add("IFORM", iDB2DbType.iDB2Char, 1).Value = prm2;
                cmd.Parameters.Add("OTYPE", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("OERR", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("OMSG", iDB2DbType.iDB2Char, 40).Direction = ParameterDirection.Output;

                bool isMock = Convert.ToBoolean(WebConfigurationManager.AppSettings["isMock"].ToString());
                if (isMock)
                {
                    prmError = "";
                    prmErrorMsg = "";
                    if (prmTel.Length == 10)
                        prmOTYPE = "M";
                    else
                        prmOTYPE = "P";
                    return true;
                }

                Utility.WriteLogResponse("Request to : " + url + "/" + cmd.CommandText.ToString());
                dsResult = cmd.ExecuteReaderOptionalDataSet();
                Utility.WriteLogResponse("Response from : " + url + "/" + cmd.CommandText.ToString(), JsonConvert.SerializeObject(dsResult));

                if (dsResult.Tables.Count > 0)
                {
                    Utility.CheckSubroutineStatus(dsResult);
                    prmError = cmd.Parameters["OERR"]?.Value?.ToString();
                    prmErrorMsg = cmd.Parameters["OMSG"]?.Value?.ToString();
                    prmOTYPE = cmd.Parameters["OTYPE"]?.Value?.ToString();
                    if (prmError == null)
                    {
                        prmError = "";
                    }
                    if (prmErrorMsg == null)
                    {
                        prmErrorMsg = "";
                    }
                    if (prmOTYPE == null)
                    {
                        prmOTYPE = "";
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Utility.WriteLog(ex);
                return false;
            }

        }

        public bool CALL_CSSRW11(string prmBiz, string prmIDNO, string prmAct, string prmRes, string prmNote1, string prmNote2, ref string prmErrMsg,
           string strBizInit, string strBranchNo)
        {
            //m_LastError = "";
            DataSet dsResult = new DataSet();
            ESBiDB2.iDB2Command cmd = new ESBiDB2.iDB2Command();

            try
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "CSSRW11";

                //gentoken
                cmd.Token = ReuseableToken.token;
                cmd.AccessKey = m_UserInfo.AccessKey; //acn
                cmd.Url = url; //api hisun

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
                cmd.Parameters.Add("PIUSER", iDB2DbType.iDB2Char, 10).Value = m_UserInfo.Username;
                cmd.Parameters.Add("PITIME", iDB2DbType.iDB2Decimal, 0).Value = Convert.ToUInt32(DateTime.Now.ToString("HHmmss"));
                cmd.Parameters.Add("POERRC", iDB2DbType.iDB2Char, 3).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("POEMSG", iDB2DbType.iDB2Char, 80).Direction = ParameterDirection.Output;

                bool isMock = Convert.ToBoolean(WebConfigurationManager.AppSettings["isMock"].ToString());
                if (isMock)
                {
                    prmErrMsg = "";
                    return true;
                }

                Utility.WriteLogResponse("Request to : " + url + "/" + cmd.CommandText.ToString());
                dsResult = cmd.ExecuteReaderOptionalDataSet();
                Utility.WriteLogResponse("Response from : " + url + "/" + cmd.CommandText.ToString(), JsonConvert.SerializeObject(dsResult));

                if (dsResult.Tables.Count > 0)
                {
                    Utility.CheckSubroutineStatus(dsResult);
                    prmErrMsg = cmd.Parameters["POEMSG"]?.Value?.ToString().Trim();
                    if (prmErrMsg == null)
                    {
                        prmErrMsg = "";
                    }
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                Utility.WriteLog(ex);
                return false;
            }

        }

        public bool Call_RLSRAUR(string prmID, string prmBirthDate, string prmThainame, string prmThaisurname,
                                 string prmNCB, string prmStatus, string prmOfficeName, string prmOfficeProvince, string prmSalary, string prmMobile, string prmZipcode,
                                 ref string prmWPRJAC, ref string prmWPRJCD, ref string prmWPRJDS, ref string prmError, string prmBusiness,
                                 string strBizInit, string strBranchNo)
        {
            DataSet dsResult = new DataSet();
            ESBiDB2.iDB2Command cmd = new ESBiDB2.iDB2Command();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "RLSRAUR";

            //gentoken
            cmd.Token = ReuseableToken.token;
            cmd.AccessKey = m_UserInfo.AccessKey; //acn
            cmd.Url = url; //api hisun

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

            Utility.WriteLogResponse("Request to : " + url + "/" + cmd.CommandText.ToString());

            bool isMock = Convert.ToBoolean(WebConfigurationManager.AppSettings["isMock"]);
            //if (isMock) 
            //{
            //   //string mock = JsonConvert.SerializeObject(dsResult);
            //   // string Smock = @"{""Table1"":[{""Precision"":""0"",""Scale"":""0"",""DbType"":""22"",""Value"":""1575773748308"",""Direction"":""1"",""IsNullable"":""False"",""ParameterName"":""WPIDNO"",""Size"":""15"",""SourceColumn"":"""",""SourceColumnNullMapping"":""False"",""SourceVersion"":""1536""},{""Precision"":""0"",""Scale"":""0"",""DbType"":""22"",""Value"":""25321221"",""Direction"":""1"",""IsNullable"":""False"",""ParameterName"":""WPBIRT"",""Size"":""8"",""SourceColumn"":"""",""SourceColumnNullMapping"":""False"",""SourceVersion"":""1536""},{""Precision"":""0"",""Scale"":""0"",""DbType"":""22"",""Value"":""ดำ"",""Direction"":""1"",""IsNullable"":""False"",""ParameterName"":""WPNAME"",""Size"":""50"",""SourceColumn"":"""",""SourceColumnNullMapping"":""False"",""SourceVersion"":""1536""},{""Precision"":""0"",""Scale"":""0"",""DbType"":""22"",""Value"":""แดง"",""Direction"":""1"",""IsNullable"":""False"",""ParameterName"":""WPSNAM"",""Size"":""50"",""SourceColumn"":"""",""SourceColumnNullMapping"":""False"",""SourceVersion"":""1536""},{""Precision"":""0"",""Scale"":""0"",""DbType"":""22"",""Value"":"""",""Direction"":""1"",""IsNullable"":""False"",""ParameterName"":""WPNCBF"",""Size"":""1"",""SourceColumn"":"""",""SourceColumnNullMapping"":""False"",""SourceVersion"":""1536""},{""Precision"":""0"",""Scale"":""0"",""DbType"":""22"",""Value"":"""",""Direction"":""1"",""IsNullable"":""False"",""ParameterName"":""WPHSTS"",""Size"":""1"",""SourceColumn"":"""",""SourceColumnNullMapping"":""False"",""SourceVersion"":""1536""},{""Precision"":""0"",""Scale"":""0"",""DbType"":""22"",""Value"":"""",""Direction"":""1"",""IsNullable"":""False"",""ParameterName"":""WPOFFN"",""Size"":""50"",""SourceColumn"":"""",""SourceColumnNullMapping"":""False"",""SourceVersion"":""1536""},{""Precision"":""0"",""Scale"":""0"",""DbType"":""22"",""Value"":"""",""Direction"":""1"",""IsNullable"":""False"",""ParameterName"":""WPOPRV"",""Size"":""3"",""SourceColumn"":"""",""SourceColumnNullMapping"":""False"",""SourceVersion"":""1536""},{""Precision"":""0"",""Scale"":""0"",""DbType"":""22"",""Value"":"""",""Direction"":""1"",""IsNullable"":""False"",""ParameterName"":""WPSALA"",""Size"":""13"",""SourceColumn"":"""",""SourceColumnNullMapping"":""False"",""SourceVersion"":""1536""},{""Precision"":""0"",""Scale"":""0"",""DbType"":""22"",""Value"":"""",""Direction"":""1"",""IsNullable"":""False"",""ParameterName"":""WPMOBL"",""Size"":""1"",""SourceColumn"":"""",""SourceColumnNullMapping"":""False"",""SourceVersion"":""1536""},{""Precision"":""0"",""Scale"":""0"",""DbType"":""22"",""Value"":"""",""Direction"":""1"",""IsNullable"":""False"",""ParameterName"":""WPOZIP"",""Size"":""5"",""SourceColumn"":"""",""SourceColumnNullMapping"":""False"",""SourceVersion"":""1536""},{""Precision"":""0"",""Scale"":""0"",""DbType"":""22"",""Value"":"""",""Direction"":""2"",""IsNullable"":""False"",""ParameterName"":""WPRJAC"",""Size"":""6"",""SourceColumn"":"""",""SourceColumnNullMapping"":""False"",""SourceVersion"":""1536""},{""Precision"":""0"",""Scale"":""0"",""DbType"":""22"",""Value"":"""",""Direction"":""2"",""IsNullable"":""False"",""ParameterName"":""WPRJCD"",""Size"":""6"",""SourceColumn"":"""",""SourceColumnNullMapping"":""False"",""SourceVersion"":""1536""},{""Precision"":""0"",""Scale"":""0"",""DbType"":""22"",""Value"":"""",""Direction"":""2"",""IsNullable"":""False"",""ParameterName"":""WPRJDS"",""Size"":""100"",""SourceColumn"":"""",""SourceColumnNullMapping"":""False"",""SourceVersion"":""1536""},{""Precision"":""0"",""Scale"":""0"",""DbType"":""22"",""Value"":"""",""Direction"":""2"",""IsNullable"":""False"",""ParameterName"":""WPERR#"",""Size"":""1"",""SourceColumn"":"""",""SourceColumnNullMapping"":""False"",""SourceVersion"":""1536""},{""Precision"":""0"",""Scale"":""0"",""DbType"":""22"",""Value"":""IL"",""Direction"":""1"",""IsNullable"":""False"",""ParameterName"":""WPBUS"",""Size"":""2"",""SourceColumn"":"""",""SourceColumnNullMapping"":""False"",""SourceVersion"":""1536""}]}";
            //   // dsResult = JsonConvert.DeserializeObject<DataSet>(Smock);

            //    prmWPRJAC = "";
            //    prmWPRJCD = "";
            //    prmWPRJDS = "";
            //    prmError = "";
            //    return true;
            //}

            dsResult = cmd.ExecuteReaderOptionalDataSet();
            Utility.WriteLogResponse("Response from : " + url + "/" + cmd.CommandText.ToString(), JsonConvert.SerializeObject(dsResult));
            if (dsResult.Tables.Count > 0)
            {
                Utility.CheckSubroutineStatus(dsResult);
                prmWPRJAC = cmd.Parameters["WPRJAC"]?.Value?.ToString();
                prmWPRJCD = cmd.Parameters["WPRJCD"]?.Value?.ToString();
                prmWPRJDS = cmd.Parameters["WPRJDS"]?.Value?.ToString();
                prmError = cmd.Parameters["WPERR#"]?.Value?.ToString();
                if (prmWPRJAC == null)
                {
                    prmWPRJAC = "";
                }
                if (prmWPRJCD == null)
                {
                    prmWPRJCD = "";
                }
                if (prmWPRJDS == null)
                {
                    prmWPRJDS = "";
                }
                if (prmError == null)
                {
                    prmError = "";
                }
                return true;
            }
            else
            {
                return false;
            }
            //try
            //{

            //}
            //catch (Exception ex)
            //{
            //    Utility.WriteLog(ex);
            //    return false;
            //}

        }


        public bool Call_ILSR01(string prmbranch, string prmloantype, ref string prmAppNo, ref string prmError, ref string prmErrorMsg,
        string strBizInit, string strBranchNo)
        {
            DataSet dsResult = new DataSet();
            ESBiDB2.iDB2Command cmd = new ESBiDB2.iDB2Command();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ILSR01";

            //gentoken
            cmd.Token = ReuseableToken.token;
            cmd.AccessKey = m_UserInfo.AccessKey; //acn
            cmd.Url = url; //api hisun

            cmd.Parameters.Add("WBRN", iDB2DbType.iDB2Char, 3).Value = prmbranch;
            cmd.Parameters.Add("WILNTY", iDB2DbType.iDB2Char, 2).Value = prmloantype;
            cmd.Parameters.Add("WAPPNO", iDB2DbType.iDB2Char, 11).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("WOERRF", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("WOEMSG", iDB2DbType.iDB2Char, 60).Direction = ParameterDirection.Output;

            Utility.WriteLogResponse("Request to : " + url + "/" + cmd.CommandText.ToString());

            dsResult = cmd.ExecuteReaderOptionalDataSet();
            Utility.WriteLogResponse("Response from : " + url + "/" + cmd.CommandText.ToString(), JsonConvert.SerializeObject(dsResult));

            if (dsResult.Tables.Count > 0)
            {
                Utility.CheckSubroutineStatus(dsResult);
                prmAppNo = cmd.Parameters["WAPPNO"]?.Value?.ToString();
                prmError = cmd.Parameters["WOERRF"]?.Value?.ToString();
                prmErrorMsg = cmd.Parameters["WOEMSG"]?.Value?.ToString();
                if (prmAppNo == null)
                {
                    prmAppNo = "";
                }
                if (prmError == null)
                {
                    prmError = "";
                }
                if (prmErrorMsg == null)
                {
                    prmErrorMsg = "";
                }
                return true;
            }
            else
            {
                return false;
            }
            //try
            //{

            //}
            //catch (Exception ex)
            //{
            //    Utility.WriteLog(ex);
            //    return false;
            //}

        }

        public bool Call_CSSR10(string prmID, ref string prmCSN, ref string prmError, ref string prmErrorMsg,
        string strBizInit, string strBranchNo)
        {
            DataSet dsResult = new DataSet();
            ESBiDB2.iDB2Command cmd = new ESBiDB2.iDB2Command();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "CSSR10";

            //gentoken
            cmd.Token = ReuseableToken.token;
            cmd.AccessKey = m_UserInfo.AccessKey; //acn
            cmd.Url = url; //api hisun

            cmd.Parameters.Add("W10IDN", iDB2DbType.iDB2Char, 15).Value = prmID;
            cmd.Parameters.Add("W10CSN", iDB2DbType.iDB2Decimal, 0).Direction = ParameterDirection.Output;
            cmd.Parameters["W10CSN"].Precision = 8;
            cmd.Parameters["W10CSN"].Scale = 0;
            cmd.Parameters.Add("W10FLG", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("W10MSG", iDB2DbType.iDB2Char, 60).Direction = ParameterDirection.Output;

            Utility.WriteLogResponse("Request to : " + url + "/" + cmd.CommandText.ToString());

            dsResult = cmd.ExecuteReaderOptionalDataSet();
            Utility.WriteLogResponse("Response from : " + url + "/" + cmd.CommandText.ToString(), JsonConvert.SerializeObject(dsResult));

            if (dsResult.Tables.Count > 0)
            {
                Utility.CheckSubroutineStatus(dsResult);
                prmCSN = cmd.Parameters["W10CSN"]?.Value?.ToString();
                prmError = cmd.Parameters["W10FLG"]?.Value?.ToString();
                prmErrorMsg = cmd.Parameters["W10MSG"]?.Value?.ToString();
                if (prmCSN == null)
                {
                    prmCSN = "";
                }
                if (prmError == null)
                {
                    prmError = "";
                }
                if (prmErrorMsg == null)
                {
                    prmErrorMsg = "";
                }
                return true;
            }
            else
            {
                return false;
            }
            //try
            //{

            //}
            //catch (Exception ex)
            //{
            //    Utility.WriteLog(ex);
            //    return false;
            //}

        }

        public bool CALL_CSSR16(string Mode_IN, string Type_IN, string CSN_IN, string RSQ_IN, string SHIPTO_IN, string Teltype_IN, string Telno_IN, string Telext_IN,
                               ref string prmError, string strBizInit, string strBranchNo)
        {
            DataSet dsResult = new DataSet();
            ESBiDB2.iDB2Command cmd = new ESBiDB2.iDB2Command();

            try
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "CSSR16";

                //gentoken
                cmd.Token = ReuseableToken.token;
                cmd.AccessKey = m_UserInfo.AccessKey; //acn
                cmd.Url = url; //api hisun

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

                Utility.WriteLogResponse("Request to : " + url + "/" + cmd.CommandText.ToString());
                dsResult = cmd.ExecuteReaderOptionalDataSet();
                Utility.WriteLogResponse("Response from : " + url + "/" + cmd.CommandText.ToString(), JsonConvert.SerializeObject(dsResult));

                if (dsResult.Tables.Count > 0)
                {
                    Utility.CheckSubroutineStatus(dsResult);
                    prmError = cmd.Parameters["POERR"]?.Value?.ToString().Trim();
                    if (prmError == null)
                    {
                        prmError = "";
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Utility.WriteLog(ex);
                return false;
            }
        }

        public bool Call_ILSRSMS(string PIMODE, string PIBUS, string PIBRN, string PIAPNO, string PITEL_, ref string POERRC, ref string POERRM, string strBizInit, string strBranchNo)
        {
            DataSet dsResult = new DataSet();
            ESBiDB2.iDB2Command cmd = new ESBiDB2.iDB2Command();

            try
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "ILSRSMS";

                //gentoken
                cmd.Token = ReuseableToken.token;
                cmd.AccessKey = m_UserInfo.AccessKey; //acn
                cmd.Url = url; //api hisun

                cmd.Parameters.Add("PIMODE", iDB2DbType.iDB2Char, 1).Value = PIMODE;
                cmd.Parameters.Add("PIBUS", iDB2DbType.iDB2Char, 2).Value = PIBUS;

                cmd.Parameters.Add("PIBRN", iDB2DbType.iDB2Char, 3).Value = PIBRN;

                cmd.Parameters.Add("PIAPNO", iDB2DbType.iDB2Char, 11).Value = PIAPNO;

                cmd.Parameters.Add("PITEL#", iDB2DbType.iDB2Char, 10).Value = PITEL_;

                cmd.Parameters.Add("POERRC", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output; //16
                cmd.Parameters["POERRC"].Value = "";

                cmd.Parameters.Add("POERRM", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output; //16
                cmd.Parameters["POERRM"].Value = "";

                bool isMock = Convert.ToBoolean(WebConfigurationManager.AppSettings["isMock"].ToString());
                if (isMock)
                {
                    POERRC = "";
                    POERRM = "";
                    return true;
                }

                Utility.WriteLogResponse("Request to : " + url + "/" + cmd.CommandText.ToString());
                dsResult = cmd.ExecuteReaderOptionalDataSet();
                Utility.WriteLogResponse("Response from : " + url + "/" + cmd.CommandText.ToString(), JsonConvert.SerializeObject(dsResult));

                if (dsResult.Tables.Count > 0)
                {
                    Utility.CheckSubroutineStatus(dsResult);
                    POERRC = cmd.Parameters["POERRC"]?.Value?.ToString().Trim();
                    POERRM = cmd.Parameters["POERRM"]?.Value?.ToString().Trim();
                    if (POERRC == null)
                    {
                        POERRC = "";
                    }
                    if (POERRM == null)
                    {
                        POERRM = "";
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Utility.WriteLog(ex);
                return false;
            }
        }
        public bool Call_GNSR69(string prmAppcode, string prmBranch, string prmAppno, string prmID, string prmCSN, string prmName, string prmBirthdate, string prmWPPRE,
                                  ref string prmResult, ref string prmResCrReview, ref string prmErrorCode, ref string prmError,
                                  string strBizInit, string strBranchNo)
        {
            DataSet dsResult = new DataSet();
            ESBiDB2.iDB2Command cmd = new ESBiDB2.iDB2Command();

            try
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GNSR69";

                //gentoken
                cmd.Token = ReuseableToken.token;
                cmd.AccessKey = m_UserInfo.AccessKey; //acn
                cmd.Url = url; //api hisun

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

                Utility.WriteLogResponse("Request to : " + url + "/" + cmd.CommandText.ToString());
                dsResult = cmd.ExecuteReaderOptionalDataSet();
                Utility.WriteLogResponse("Response from : " + url + "/" + cmd.CommandText.ToString(), JsonConvert.SerializeObject(dsResult));

                if (dsResult.Tables.Count > 0)
                {
                    Utility.CheckSubroutineStatus(dsResult);
                    prmResult = cmd.Parameters["WPORES"]?.Value?.ToString();
                    prmResCrReview = cmd.Parameters["WPORCR"]?.Value?.ToString();
                    if (prmResult == null)
                    {
                        prmResult = "";
                    }
                    if (prmResCrReview == null)
                    {
                        prmResCrReview = "";
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Utility.WriteLog(ex);
                return false;
            }
        }

        public bool CALL_CSSR36(string prmCSN, ref string STS, ref string MSG, ref string FLG,
                                string strBizInit, string strBranchNo)
        {
            DataSet dsResult = new DataSet();
            ESBiDB2.iDB2Command cmd = new ESBiDB2.iDB2Command();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "CSSR36";

            //gentoken
            cmd.Token = ReuseableToken.token;
            cmd.AccessKey = m_UserInfo.AccessKey; //acn
            cmd.Url = url; //api hisun

            cmd.Parameters.Add("W0CSNO", iDB2DbType.iDB2Char, 8).Value = prmCSN;

            //***   out put ***//
            cmd.Parameters.Add("W0PSTS", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;
            cmd.Parameters["W0PSTS"].Value = "";
            cmd.Parameters.Add("W0MSGL", iDB2DbType.iDB2Char, 50).Direction = ParameterDirection.Output;
            cmd.Parameters["W0MSGL"].Value = "";
            cmd.Parameters.Add("W0NFLG", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;
            cmd.Parameters["W0NFLG"].Value = "";

            Utility.WriteLogResponse("Request to : " + url + "/" + cmd.CommandText.ToString());
            bool isMock = Convert.ToBoolean(WebConfigurationManager.AppSettings["isMock"]);
            if (isMock)
            {
                //string mock = JsonConvert.SerializeObject(dsResult);
                // string Smock = @"{""Table1"":[{""Precision"":""0"",""Scale"":""0"",""DbType"":""22"",""Value"":""1575773748308"",""Direction"":""1"",""IsNullable"":""False"",""ParameterName"":""WPIDNO"",""Size"":""15"",""SourceColumn"":"""",""SourceColumnNullMapping"":""False"",""SourceVersion"":""1536""},{""Precision"":""0"",""Scale"":""0"",""DbType"":""22"",""Value"":""25321221"",""Direction"":""1"",""IsNullable"":""False"",""ParameterName"":""WPBIRT"",""Size"":""8"",""SourceColumn"":"""",""SourceColumnNullMapping"":""False"",""SourceVersion"":""1536""},{""Precision"":""0"",""Scale"":""0"",""DbType"":""22"",""Value"":""ดำ"",""Direction"":""1"",""IsNullable"":""False"",""ParameterName"":""WPNAME"",""Size"":""50"",""SourceColumn"":"""",""SourceColumnNullMapping"":""False"",""SourceVersion"":""1536""},{""Precision"":""0"",""Scale"":""0"",""DbType"":""22"",""Value"":""แดง"",""Direction"":""1"",""IsNullable"":""False"",""ParameterName"":""WPSNAM"",""Size"":""50"",""SourceColumn"":"""",""SourceColumnNullMapping"":""False"",""SourceVersion"":""1536""},{""Precision"":""0"",""Scale"":""0"",""DbType"":""22"",""Value"":"""",""Direction"":""1"",""IsNullable"":""False"",""ParameterName"":""WPNCBF"",""Size"":""1"",""SourceColumn"":"""",""SourceColumnNullMapping"":""False"",""SourceVersion"":""1536""},{""Precision"":""0"",""Scale"":""0"",""DbType"":""22"",""Value"":"""",""Direction"":""1"",""IsNullable"":""False"",""ParameterName"":""WPHSTS"",""Size"":""1"",""SourceColumn"":"""",""SourceColumnNullMapping"":""False"",""SourceVersion"":""1536""},{""Precision"":""0"",""Scale"":""0"",""DbType"":""22"",""Value"":"""",""Direction"":""1"",""IsNullable"":""False"",""ParameterName"":""WPOFFN"",""Size"":""50"",""SourceColumn"":"""",""SourceColumnNullMapping"":""False"",""SourceVersion"":""1536""},{""Precision"":""0"",""Scale"":""0"",""DbType"":""22"",""Value"":"""",""Direction"":""1"",""IsNullable"":""False"",""ParameterName"":""WPOPRV"",""Size"":""3"",""SourceColumn"":"""",""SourceColumnNullMapping"":""False"",""SourceVersion"":""1536""},{""Precision"":""0"",""Scale"":""0"",""DbType"":""22"",""Value"":"""",""Direction"":""1"",""IsNullable"":""False"",""ParameterName"":""WPSALA"",""Size"":""13"",""SourceColumn"":"""",""SourceColumnNullMapping"":""False"",""SourceVersion"":""1536""},{""Precision"":""0"",""Scale"":""0"",""DbType"":""22"",""Value"":"""",""Direction"":""1"",""IsNullable"":""False"",""ParameterName"":""WPMOBL"",""Size"":""1"",""SourceColumn"":"""",""SourceColumnNullMapping"":""False"",""SourceVersion"":""1536""},{""Precision"":""0"",""Scale"":""0"",""DbType"":""22"",""Value"":"""",""Direction"":""1"",""IsNullable"":""False"",""ParameterName"":""WPOZIP"",""Size"":""5"",""SourceColumn"":"""",""SourceColumnNullMapping"":""False"",""SourceVersion"":""1536""},{""Precision"":""0"",""Scale"":""0"",""DbType"":""22"",""Value"":"""",""Direction"":""2"",""IsNullable"":""False"",""ParameterName"":""WPRJAC"",""Size"":""6"",""SourceColumn"":"""",""SourceColumnNullMapping"":""False"",""SourceVersion"":""1536""},{""Precision"":""0"",""Scale"":""0"",""DbType"":""22"",""Value"":"""",""Direction"":""2"",""IsNullable"":""False"",""ParameterName"":""WPRJCD"",""Size"":""6"",""SourceColumn"":"""",""SourceColumnNullMapping"":""False"",""SourceVersion"":""1536""},{""Precision"":""0"",""Scale"":""0"",""DbType"":""22"",""Value"":"""",""Direction"":""2"",""IsNullable"":""False"",""ParameterName"":""WPRJDS"",""Size"":""100"",""SourceColumn"":"""",""SourceColumnNullMapping"":""False"",""SourceVersion"":""1536""},{""Precision"":""0"",""Scale"":""0"",""DbType"":""22"",""Value"":"""",""Direction"":""2"",""IsNullable"":""False"",""ParameterName"":""WPERR#"",""Size"":""1"",""SourceColumn"":"""",""SourceColumnNullMapping"":""False"",""SourceVersion"":""1536""},{""Precision"":""0"",""Scale"":""0"",""DbType"":""22"",""Value"":""IL"",""Direction"":""1"",""IsNullable"":""False"",""ParameterName"":""WPBUS"",""Size"":""2"",""SourceColumn"":"""",""SourceColumnNullMapping"":""False"",""SourceVersion"":""1536""}]}";
                // dsResult = JsonConvert.DeserializeObject<DataSet>(Smock);

                STS = "";
                MSG = "";
                FLG = "";
                return true;
            }
            dsResult = cmd.ExecuteReaderOptionalDataSet();
            Utility.WriteLogResponse("Response from : " + url + "/" + cmd.CommandText.ToString(), JsonConvert.SerializeObject(dsResult));

            if (dsResult.Tables.Count > 0)
            {
                Utility.CheckSubroutineStatus(dsResult);
                STS = cmd.Parameters["W0PSTS"]?.Value?.ToString().Trim();
                MSG = cmd.Parameters["W0MSGL"]?.Value?.ToString().Trim();
                FLG = cmd.Parameters["W0NFLG"]?.Value?.ToString().Trim();
                if (STS == null)
                {
                    STS = "";
                }
                if (MSG == null)
                {
                    MSG = "";
                }
                if (FLG == null)
                {
                    FLG = "";
                }
                return true;
            }
            else
            {
                return false;
            }
            //try
            //{

            //}
            //catch (Exception ex)
            //{
            //    Utility.WriteLog(ex);
            //    return false;
            //}


        }

        public bool Call_GNSRBLC(string ID_IN, string CSCD_IN, string APPCODE_IN, string SOURCE_IN, string Case_IN,
                                 ref string prmError, ref string prmErrorMsg, string strBizInit, string strBranchNo)
        {
            DataSet dsResult = new DataSet();
            ESBiDB2.iDB2Command cmd = new ESBiDB2.iDB2Command();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "GNSRBLC";

            //gentoken
            cmd.Token = ReuseableToken.token;
            cmd.AccessKey = m_UserInfo.AccessKey; //acn
            cmd.Url = url; //api hisun

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

            Utility.WriteLogResponse("Request to : " + url + "/" + cmd.CommandText.ToString());
            bool isMock = Convert.ToBoolean(WebConfigurationManager.AppSettings["isMock"]);
            if (isMock)
            {
                //string mock = JsonConvert.SerializeObject(dsResult);
                // string Smock = @"{""Table1"":[{""Precision"":""0"",""Scale"":""0"",""DbType"":""22"",""Value"":""1575773748308"",""Direction"":""1"",""IsNullable"":""False"",""ParameterName"":""WPIDNO"",""Size"":""15"",""SourceColumn"":"""",""SourceColumnNullMapping"":""False"",""SourceVersion"":""1536""},{""Precision"":""0"",""Scale"":""0"",""DbType"":""22"",""Value"":""25321221"",""Direction"":""1"",""IsNullable"":""False"",""ParameterName"":""WPBIRT"",""Size"":""8"",""SourceColumn"":"""",""SourceColumnNullMapping"":""False"",""SourceVersion"":""1536""},{""Precision"":""0"",""Scale"":""0"",""DbType"":""22"",""Value"":""ดำ"",""Direction"":""1"",""IsNullable"":""False"",""ParameterName"":""WPNAME"",""Size"":""50"",""SourceColumn"":"""",""SourceColumnNullMapping"":""False"",""SourceVersion"":""1536""},{""Precision"":""0"",""Scale"":""0"",""DbType"":""22"",""Value"":""แดง"",""Direction"":""1"",""IsNullable"":""False"",""ParameterName"":""WPSNAM"",""Size"":""50"",""SourceColumn"":"""",""SourceColumnNullMapping"":""False"",""SourceVersion"":""1536""},{""Precision"":""0"",""Scale"":""0"",""DbType"":""22"",""Value"":"""",""Direction"":""1"",""IsNullable"":""False"",""ParameterName"":""WPNCBF"",""Size"":""1"",""SourceColumn"":"""",""SourceColumnNullMapping"":""False"",""SourceVersion"":""1536""},{""Precision"":""0"",""Scale"":""0"",""DbType"":""22"",""Value"":"""",""Direction"":""1"",""IsNullable"":""False"",""ParameterName"":""WPHSTS"",""Size"":""1"",""SourceColumn"":"""",""SourceColumnNullMapping"":""False"",""SourceVersion"":""1536""},{""Precision"":""0"",""Scale"":""0"",""DbType"":""22"",""Value"":"""",""Direction"":""1"",""IsNullable"":""False"",""ParameterName"":""WPOFFN"",""Size"":""50"",""SourceColumn"":"""",""SourceColumnNullMapping"":""False"",""SourceVersion"":""1536""},{""Precision"":""0"",""Scale"":""0"",""DbType"":""22"",""Value"":"""",""Direction"":""1"",""IsNullable"":""False"",""ParameterName"":""WPOPRV"",""Size"":""3"",""SourceColumn"":"""",""SourceColumnNullMapping"":""False"",""SourceVersion"":""1536""},{""Precision"":""0"",""Scale"":""0"",""DbType"":""22"",""Value"":"""",""Direction"":""1"",""IsNullable"":""False"",""ParameterName"":""WPSALA"",""Size"":""13"",""SourceColumn"":"""",""SourceColumnNullMapping"":""False"",""SourceVersion"":""1536""},{""Precision"":""0"",""Scale"":""0"",""DbType"":""22"",""Value"":"""",""Direction"":""1"",""IsNullable"":""False"",""ParameterName"":""WPMOBL"",""Size"":""1"",""SourceColumn"":"""",""SourceColumnNullMapping"":""False"",""SourceVersion"":""1536""},{""Precision"":""0"",""Scale"":""0"",""DbType"":""22"",""Value"":"""",""Direction"":""1"",""IsNullable"":""False"",""ParameterName"":""WPOZIP"",""Size"":""5"",""SourceColumn"":"""",""SourceColumnNullMapping"":""False"",""SourceVersion"":""1536""},{""Precision"":""0"",""Scale"":""0"",""DbType"":""22"",""Value"":"""",""Direction"":""2"",""IsNullable"":""False"",""ParameterName"":""WPRJAC"",""Size"":""6"",""SourceColumn"":"""",""SourceColumnNullMapping"":""False"",""SourceVersion"":""1536""},{""Precision"":""0"",""Scale"":""0"",""DbType"":""22"",""Value"":"""",""Direction"":""2"",""IsNullable"":""False"",""ParameterName"":""WPRJCD"",""Size"":""6"",""SourceColumn"":"""",""SourceColumnNullMapping"":""False"",""SourceVersion"":""1536""},{""Precision"":""0"",""Scale"":""0"",""DbType"":""22"",""Value"":"""",""Direction"":""2"",""IsNullable"":""False"",""ParameterName"":""WPRJDS"",""Size"":""100"",""SourceColumn"":"""",""SourceColumnNullMapping"":""False"",""SourceVersion"":""1536""},{""Precision"":""0"",""Scale"":""0"",""DbType"":""22"",""Value"":"""",""Direction"":""2"",""IsNullable"":""False"",""ParameterName"":""WPERR#"",""Size"":""1"",""SourceColumn"":"""",""SourceColumnNullMapping"":""False"",""SourceVersion"":""1536""},{""Precision"":""0"",""Scale"":""0"",""DbType"":""22"",""Value"":""IL"",""Direction"":""1"",""IsNullable"":""False"",""ParameterName"":""WPBUS"",""Size"":""2"",""SourceColumn"":"""",""SourceColumnNullMapping"":""False"",""SourceVersion"":""1536""}]}";
                // dsResult = JsonConvert.DeserializeObject<DataSet>(Smock);

                prmError = "";
                prmErrorMsg = "";
                return true;
            }
            dsResult = cmd.ExecuteReaderOptionalDataSet();
            Utility.WriteLogResponse("Response from : " + url + "/" + cmd.CommandText.ToString(), JsonConvert.SerializeObject(dsResult));

            if (dsResult.Tables.Count > 0)
            {
                Utility.CheckSubroutineStatus(dsResult);
                prmError = cmd.Parameters["WKERR"]?.Value?.ToString().Trim();
                prmErrorMsg = cmd.Parameters["WKMSG"]?.Value?.ToString().Trim();
                if (prmError == null)
                {
                    prmError = "";
                }
                if (prmErrorMsg == null)
                {
                    prmErrorMsg = "";
                }
                return true;
            }
            else
            {
                return false;
            }
            //try
            //{

            //}
            //catch (Exception ex)
            //{
            //    Utility.WriteLog(ex);
            //    return false;
            //}

        }



        public bool CALL_GNP014(string IDATE, string ITYPDATE, string IBORC, ref string OERRCHK, ref string ONAMED,
                              string strBizInit, string strBranchNo)
        {
            try
            {
                ESBiDB2.iDB2Command cmd = new ESBiDB2.iDB2Command();
                cmd.CommandType = CommandType.StoredProcedure;
                //gentoken
                cmd.Token = ReuseableToken.token;
                cmd.AccessKey = m_UserInfo.AccessKey; //acn
                cmd.Url = url; //api hisun

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GNP014";

                cmd.Parameters.Add("IDATE", iDB2DbType.iDB2Char, 6).Value = IDATE;
                cmd.Parameters.Add("ITYPDATE", iDB2DbType.iDB2Char, 3).Value = ITYPDATE;
                cmd.Parameters.Add("IBORC", iDB2DbType.iDB2Char, 1).Value = IBORC;


                //***   out put ***//
                cmd.Parameters.Add("ONAMED", iDB2DbType.iDB2Char, 3).Direction = ParameterDirection.Output;
                //cmd.Parameters["ONAMED"].Value = "";
                cmd.Parameters.Add("OERRCHK", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;
                //cmd.Parameters["OERRCHK"].Value = "";



                Utility.WriteLogResponse("Request to : " + url + "/" + cmd.CommandText.ToString());
                var dt = cmd.ExecuteReaderOptionalDataSet();
                Utility.WriteLogResponse("Response from : " + url + "/" + cmd.CommandText.ToString(), JsonConvert.SerializeObject(dt));

                var TOA = dt.Tables[0];

                if (dt.Tables.Count > 0)
                {
                    Utility.CheckSubroutineStatus(dt);
                    ONAMED = cmd.Parameters["ONAMED"]?.Value?.ToString().Trim();
                    OERRCHK = cmd.Parameters["OERRCHK"]?.Value?.ToString().Trim();
                    if (ONAMED == null)
                    {
                        ONAMED = "";
                    }
                    if (OERRCHK == null)
                    {
                        OERRCHK = "";
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Utility.WriteLog(ex);
                return false;
            }

        }

        public bool Call_CSSR07(string prmID_NO, string prmCDTE, ref string salary, ref string date, ref string time, string strBizInit, string strBranchNo)
        {
            try
            {
                ESBiDB2.iDB2Command cmd = new ESBiDB2.iDB2Command();
                cmd.CommandType = CommandType.StoredProcedure;
                //gentoken
                cmd.Token = ReuseableToken.token;
                cmd.AccessKey = m_UserInfo.AccessKey; //acn
                cmd.Url = url; //api hisun

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

                Utility.WriteLogResponse("Request to : " + url + "/" + cmd.CommandText.ToString());
                var dt = cmd.ExecuteReaderOptionalDataSet();
                Utility.WriteLogResponse("Response from : " + url + "/" + cmd.CommandText.ToString(), JsonConvert.SerializeObject(dt));

                var TOA = dt.Tables[0];

                if (dt.Tables.Count > 0)
                {
                    Utility.CheckSubroutineStatus(dt);
                    salary = cmd.Parameters["WOSALA"]?.Value?.ToString().Trim();
                    date = cmd.Parameters["WOSDTE"]?.Value?.ToString().Trim();
                    time = cmd.Parameters["WOSTIM"]?.Value?.ToString().Trim();
                    if (salary == null)
                    {
                        salary = "";
                    }
                    if (date == null)
                    {
                        date = "";
                    }
                    if (time == null)
                    {
                        time = "";
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Utility.WriteLog(ex);
                return false;
            }

        }



        public bool CALL_GNP0371(string prmDATE1, string prmDATE2, string prmFMT, string prmBOC, string prmAPT, string prmBUS, string prmCST,
                                    string strBizInit, string strBranchNo, ref string AGE, ref string Error)
        {
            try
            {
                ESBiDB2.iDB2Command cmd = new ESBiDB2.iDB2Command();
                cmd.CommandType = CommandType.StoredProcedure;
                //gentoken
                cmd.Token = ReuseableToken.token;
                cmd.AccessKey = m_UserInfo.AccessKey; //acn
                cmd.Url = url; //api hisun

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

                Utility.WriteLogResponse("Request to : " + url + "/" + cmd.CommandText.ToString());
                var dt = cmd.ExecuteReaderOptionalDataSet();
                Utility.WriteLogResponse("Response from : " + url + "/" + cmd.CommandText.ToString(), JsonConvert.SerializeObject(dt));

                var TOA = dt.Tables[0];

                if (dt.Tables.Count > 0)
                {
                    Utility.CheckSubroutineStatus(dt);
                    AGE = cmd.Parameters["W#AGE"]?.Value?.ToString().Trim();
                    Error = cmd.Parameters["WPERR"]?.Value?.ToString().Trim();
                    if (AGE == null)
                    {
                        AGE = "";
                    }
                    if (Error == null)
                    {
                        Error = "";
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Utility.WriteLog(ex);
                return false;
            }

        }


        public bool Call_GNSR49(string prmTel, string prmProvince, ref string prmError, string strBizInit, string strBranchNo)
        {
            try
            {
                ESBiDB2.iDB2Command cmd = new ESBiDB2.iDB2Command();
                cmd.CommandType = CommandType.StoredProcedure;
                //gentoken
                cmd.Token = ReuseableToken.token;
                cmd.AccessKey = m_UserInfo.AccessKey; //acn
                cmd.Url = url; //api hisun

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GNSR49";

                cmd.Parameters.Add("WPITL", iDB2DbType.iDB2Char, 3).Value = prmTel;
                cmd.Parameters.Add("WPPCD", iDB2DbType.iDB2Char, 3).Value = prmProvince;
                cmd.Parameters.Add("WPERR", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;

                Utility.WriteLogResponse("Request to : " + url + "/" + cmd.CommandText.ToString());
                var dt = cmd.ExecuteReaderOptionalDataSet();
                Utility.WriteLogResponse("Response from : " + url + "/" + cmd.CommandText.ToString(), JsonConvert.SerializeObject(dt));

                var TOA = dt.Tables[0];

                if (dt.Tables.Count > 0)
                {
                    Utility.CheckSubroutineStatus(dt);
                    prmError = cmd.Parameters["WPERR"]?.Value?.ToString();
                    if (prmError == null)
                    {
                        prmError = "";
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Utility.WriteLog(ex);
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
            DataSet dsResult = new DataSet();
            ESBiDB2.iDB2Command cmd = new ESBiDB2.iDB2Command();
            cmd.CommandType = CommandType.StoredProcedure;
            //gentoken
            cmd.Token = ReuseableToken.token;
            cmd.AccessKey = m_UserInfo.AccessKey; //acn
            cmd.Url = url; //api hisun
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

                Utility.WriteLogResponse("Request to : " + url + "/" + cmd.CommandText.ToString());
                dsResult = cmd.ExecuteReaderOptionalDataSet();
                Utility.WriteLogResponse("Response from : " + url + "/" + cmd.CommandText.ToString(), JsonConvert.SerializeObject(dsResult));


                if (dsResult.Tables.Count > 0)
                {
                    Utility.CheckSubroutineStatus(dsResult);
                    POPCAM = cmd.Parameters["POPCAM"]?.Value?.ToString().Trim();
                    POINTR = cmd.Parameters["POINTR"]?.Value?.ToString().Trim();
                    POCRUR = cmd.Parameters["POCRUR"]?.Value?.ToString().Trim();
                    POINFR = cmd.Parameters["POINFR"]?.Value?.ToString().Trim();
                    PODIFR = cmd.Parameters["PODIFR"]?.Value?.ToString().Trim();
                    POINST = cmd.Parameters["POINST"]?.Value?.ToString().Trim();
                    POTOAM = cmd.Parameters["POTOAM"]?.Value?.ToString().Trim();
                    PODUTY = cmd.Parameters["PODUTY"]?.Value?.ToString().Trim();
                    POINTB = cmd.Parameters["POINTB"]?.Value?.ToString().Trim();
                    POCRUB = cmd.Parameters["POCRUB"]?.Value?.ToString().Trim();
                    POINFB = cmd.Parameters["POINFB"]?.Value?.ToString().Trim();
                    POCONA = cmd.Parameters["POCONA"]?.Value?.ToString().Trim();
                    POFDAT = cmd.Parameters["POFDAT"]?.Value?.ToString().Trim();
                    POAINR = cmd.Parameters["POAINR"]?.Value?.ToString().Trim();
                    POACRU = cmd.Parameters["POACRU"]?.Value?.ToString().Trim();
                    PODDAT = cmd.Parameters["PODDAT"]?.Value?.ToString().Trim();
                    POPPRN = cmd.Parameters["POPPRN"]?.Value?.ToString().Trim();
                    POINSD = cmd.Parameters["POINSD"]?.Value?.ToString().Trim();
                    POINTD = cmd.Parameters["POINTD"]?.Value?.ToString().Trim();
                    POCRUD = cmd.Parameters["POCRUD"]?.Value?.ToString().Trim();
                    POINFD = cmd.Parameters["POINFD"]?.Value?.ToString().Trim();
                    POCDAT = cmd.Parameters["POCDAT"]?.Value?.ToString().Trim();
                    POINCM = cmd.Parameters["POINCM"]?.Value?.ToString().Trim();
                    POREBT = cmd.Parameters["POREBT"]?.Value?.ToString().Trim();
                    POCLSA = cmd.Parameters["POCLSA"]?.Value?.ToString().Trim();

                    POFLAG = cmd.Parameters["POFLAG"]?.Value?.ToString().Trim();
                    if (POPCAM == null)
                    {
                        POPCAM = "";
                    }
                    if (POINTR == null)
                    {
                        POINTR = "";
                    }
                    if (POCRUR == null)
                    {
                        POCRUR = "";
                    }
                    if (POINFR == null)
                    {
                        POINFR = "";
                    }
                    if (PODIFR == null)
                    {
                        PODIFR = "";
                    }
                    if (POINST == null)
                    {
                        POINST = "";
                    }
                    if (POTOAM == null)
                    {
                        POTOAM = "";
                    }
                    if (PODUTY == null)
                    {
                        PODUTY = "0";
                    }
                    if (POINTB == null)
                    {
                        POINTB = "0";
                    }
                    if (POCRUB == null)
                    {
                        POCRUB = "0";
                    }
                    if (POINFB == null)
                    {
                        POINFB = "0";
                    }
                    if (POCONA == null)
                    {
                        POCONA = "0";
                    }
                    if (POFDAT == null)
                    {
                        POFDAT = "";
                    }
                    if (POAINR == null)
                    {
                        POAINR = "0";
                    }
                    if (POACRU == null)
                    {
                        POACRU = "0";
                    }
                    if (PODDAT == null)
                    {
                        PODDAT = "";
                    }
                    if (POPPRN == null)
                    {
                        POPPRN = "";
                    }
                    if (POINSD == null)
                    {
                        POINSD = "";
                    }
                    if (POINTD == null)
                    {
                        POINTD = "";
                    }
                    if (POCRUD == null)
                    {
                        POCRUD = "";
                    }
                    if (POINFD == null)
                    {
                        POINFD = "";
                    }
                    if (POCDAT == null)
                    {
                        POCDAT = "";
                    }
                    if (POINCM == null)
                    {
                        POINCM = "";
                    }
                    if (POREBT == null)
                    {
                        POREBT = "";
                    }
                    if (POCLSA == null)
                    {
                        POCLSA = "";
                    }
                    if (POFLAG == null)
                    {
                        POFLAG = "";
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Utility.WriteLog(ex);
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
            DataSet dsResult = new DataSet();
            ESBiDB2.iDB2Command cmd = new ESBiDB2.iDB2Command();
            cmd.CommandType = CommandType.StoredProcedure;
            //gentoken
            cmd.Token = ReuseableToken.token;
            cmd.AccessKey = m_UserInfo.AccessKey; //acn
            cmd.Url = url; //api hisun
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

                Utility.WriteLogResponse("Request to : " + url + "/" + cmd.CommandText.ToString());
                dsResult = cmd.ExecuteReaderOptionalDataSet();
                Utility.WriteLogResponse("Response from : " + url + "/" + cmd.CommandText.ToString(), JsonConvert.SerializeObject(dsResult));


                if (dsResult.Tables.Count > 0)
                {
                    Utility.CheckSubroutineStatus(dsResult);
                    POPCAM = cmd.Parameters["POPCAM"]?.Value?.ToString().Trim();
                    POINTR = cmd.Parameters["POINTR"]?.Value?.ToString().Trim();
                    POCRUR = cmd.Parameters["POCRUR"]?.Value?.ToString().Trim();
                    POINFR = cmd.Parameters["POINFR"]?.Value?.ToString().Trim();
                    PODIFR = cmd.Parameters["PODIFR"]?.Value?.ToString().Trim();
                    POINST = cmd.Parameters["POINST"]?.Value?.ToString().Trim();
                    POTOAM = cmd.Parameters["POTOAM"]?.Value?.ToString().Trim();
                    PODUTY = cmd.Parameters["PODUTY"]?.Value?.ToString().Trim();
                    POINTB = cmd.Parameters["POINTB"]?.Value?.ToString().Trim();
                    POCRUB = cmd.Parameters["POCRUB"]?.Value?.ToString().Trim();
                    POINFB = cmd.Parameters["POINFB"]?.Value?.ToString().Trim();
                    POCONA = cmd.Parameters["POCONA"]?.Value?.ToString().Trim();
                    POFDAT = cmd.Parameters["POFDAT"]?.Value?.ToString().Trim();
                    POAINR = cmd.Parameters["POAINR"]?.Value?.ToString().Trim();
                    POACRU = cmd.Parameters["POACRU"]?.Value?.ToString().Trim();
                    PODDAT = cmd.Parameters["PODDAT"]?.Value?.ToString().Trim();
                    POPPRN = cmd.Parameters["POPPRN"]?.Value?.ToString().Trim();
                    POINSD = cmd.Parameters["POINSD"]?.Value?.ToString().Trim();
                    POINTD = cmd.Parameters["POINTD"]?.Value?.ToString().Trim();
                    POCRUD = cmd.Parameters["POCRUD"]?.Value?.ToString().Trim();
                    POINFD = cmd.Parameters["POINFD"]?.Value?.ToString().Trim();
                    POCDAT = cmd.Parameters["POCDAT"]?.Value?.ToString().Trim();
                    POINCM = cmd.Parameters["POINCM"]?.Value?.ToString().Trim();
                    POREBT = cmd.Parameters["POREBT"]?.Value?.ToString().Trim();
                    POCLSA = cmd.Parameters["POCLSA"]?.Value?.ToString().Trim();

                    POFLAG = cmd.Parameters["POFLAG"]?.Value?.ToString().Trim();
                    if (POPCAM == null)
                    {
                        POPCAM = "";
                    }
                    if (POINTR == null)
                    {
                        POINTR = "";
                    }
                    if (POCRUR == null)
                    {
                        POCRUR = "";
                    }
                    if (POINFR == null)
                    {
                        POINFR = "";
                    }
                    if (PODIFR == null)
                    {
                        PODIFR = "";
                    }
                    if (POINST == null)
                    {
                        POINST = "";
                    }
                    if (POTOAM == null)
                    {
                        POTOAM = "";
                    }
                    if (PODUTY == null)
                    {
                        PODUTY = "0";
                    }
                    if (POINTB == null)
                    {
                        POINTB = "0";
                    }
                    if (POCRUB == null)
                    {
                        POCRUB = "0";
                    }
                    if (POINFB == null)
                    {
                        POINFB = "0";
                    }
                    if (POCONA == null)
                    {
                        POCONA = "0";
                    }
                    if (POFDAT == null)
                    {
                        POFDAT = "";
                    }
                    if (POAINR == null)
                    {
                        POAINR = "0";
                    }
                    if (POACRU == null)
                    {
                        POACRU = "0";
                    }
                    if (PODDAT == null)
                    {
                        PODDAT = "";
                    }
                    if (POPPRN == null)
                    {
                        POPPRN = "";
                    }
                    if (POINSD == null)
                    {
                        POINSD = "";
                    }
                    if (POINTD == null)
                    {
                        POINTD = "";
                    }
                    if (POCRUD == null)
                    {
                        POCRUD = "";
                    }
                    if (POINFD == null)
                    {
                        POINFD = "";
                    }
                    if (POCDAT == null)
                    {
                        POCDAT = "";
                    }
                    if (POINCM == null)
                    {
                        POINCM = "";
                    }
                    if (POREBT == null)
                    {
                        POREBT = "";
                    }
                    if (POCLSA == null)
                    {
                        POCLSA = "";
                    }
                    if (POFLAG == null)
                    {
                        POFLAG = "";
                    }


                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                Utility.WriteLog(ex);
                return false;
            }
        }

        public bool Call_GNSR093(string IDNO, string salary, ref string WPERR, ref string WPAYMENT, string strBizInit, string strBranchNo)
        {

            DataSet dsResult = new DataSet();
            try
            {

                ESBiDB2.iDB2Command cmd = new ESBiDB2.iDB2Command();
                cmd.CommandType = CommandType.StoredProcedure;
                //gentoken
                cmd.Token = ReuseableToken.token;
                cmd.AccessKey = m_UserInfo.AccessKey; //acn
                cmd.Url = url; //api hisun
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

                //bool isMock = Convert.ToBoolean(WebConfigurationManager.AppSettings["isMock"]);
                //if(isMock)
                //{
                //    WPAYMENT = "15000";
                //    WPERR = "";
                //    return true;
                //}


                Utility.WriteLogResponse("Request to : " + url + "/" + cmd.CommandText.ToString());
                var dt = cmd.ExecuteReaderOptionalDataSet();
                Utility.WriteLogResponse("Response from : " + url + "/" + cmd.CommandText.ToString(), JsonConvert.SerializeObject(dt));

                var TOA = dt.Tables[0];

                if (dt.Tables.Count > 0)
                {
                    Utility.CheckSubroutineStatus(dt);
                    WPAYMENT = cmd.Parameters["WPAYMENT"]?.Value?.ToString().Trim();
                    WPERR = cmd.Parameters["WPERR"]?.Value?.ToString().Trim();
                    if (WPAYMENT == null)
                    {
                        WPAYMENT = "";
                    }
                    if (WPERR == null)
                    {
                        WPERR = "";
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Utility.WriteLog(ex);
                return false;
            }

        }

        public bool CALL_GNSRGNOC(string prmBUS, string prmBRN, string prmAPPNO, string prmARRAY,
                                  ref string prmOCSNO, ref string OPD, ref string OGNO, ref string ORANK, ref string OINC, ref string OACL,
                                  ref string O2GNO, ref string O2RANK, ref string O2ACL, ref string O2RNK, ref string O21ACL, ref string OTYPE,
                                  string strBizInit, string strBranchNo)
        {
            try
            {
                ESBiDB2.iDB2Command cmd = new ESBiDB2.iDB2Command();
                cmd.CommandType = CommandType.StoredProcedure;
                //gentoken
                cmd.Token = ReuseableToken.token;
                cmd.AccessKey = m_UserInfo.AccessKey; //acn
                cmd.Url = url; //api hisun


                //m_da400.AS400Lib = EB_Service.Commons.AppConfig.GNPGMLib;  //GeneralLib;
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

                bool isMock = Convert.ToBoolean(WebConfigurationManager.AppSettings["isMock"].ToString());
                if (isMock)
                {
                    prmOCSNO = "";
                    OPD = "1.000000000";
                    OGNO = "0";
                    ORANK = "0";
                    OINC = "0.00";
                    OACL = "50000.00";
                    O2GNO = "100";
                    O2RANK = "10";
                    O2ACL = "48600";
                    O2RNK = "10";
                    O21ACL = "48600";
                    OTYPE = "8";
                    return true;
                }

                Utility.WriteLogResponse("Request to : " + url + "/" + cmd.CommandText.ToString());
                var dt = cmd.ExecuteReaderOptionalDataSet();
                Utility.WriteLogResponse("Response from : " + url + "/" + cmd.CommandText.ToString(), JsonConvert.SerializeObject(dt));


                //var TOA = dt.Tables[0];

                if (dt.Tables.Count > 0)
                {
                    Utility.CheckSubroutineStatus(dt);
                    prmOCSNO = cmd.Parameters["O@CSNO"]?.Value?.ToString().Trim();
                    OPD = cmd.Parameters["O@PD"]?.Value?.ToString().Trim();
                    OGNO = cmd.Parameters["O@GNO"]?.Value?.ToString().Trim();
                    ORANK = cmd.Parameters["O@RANK"]?.Value?.ToString().Trim();
                    OINC = cmd.Parameters["O@INC"]?.Value?.ToString().Trim();
                    OACL = cmd.Parameters["O@ACL"]?.Value?.ToString().Trim();
                    O2GNO = cmd.Parameters["O2@GNO"]?.Value?.ToString().Trim();
                    O2RANK = cmd.Parameters["O2@RANK"]?.Value?.ToString().Trim();
                    O2ACL = cmd.Parameters["O2@#ACL"]?.Value?.ToString().Trim();
                    O2RNK = cmd.Parameters["O2@#RNK"]?.Value?.ToString().Trim();
                    O21ACL = cmd.Parameters["O2@#ACL"]?.Value?.ToString().Trim();
                    OTYPE = cmd.Parameters["O@TYPE"]?.Value?.ToString().Trim();
                    if (prmOCSNO == null)
                    {
                        prmOCSNO = "";
                    }
                    if (OPD == null)
                    {
                        OPD = "0";
                    }
                    if (OGNO == null)
                    {
                        OGNO = "0";
                    }
                    if (ORANK == null)
                    {
                        ORANK = "0";
                    }
                    if (OINC == null)
                    {
                        OINC = "0";
                    }
                    if (OACL == null)
                    {
                        OACL = "0";
                    }
                    if (O2GNO == null)
                    {
                        O2GNO = "0";
                    }
                    if (O2RANK == null)
                    {
                        O2RANK = "0";
                    }
                    if (O2ACL == null)
                    {
                        O2ACL = "0";
                    }
                    if (O2RNK == null)
                    {
                        O2RNK = "0";
                    }
                    if (O21ACL == null)
                    {
                        O21ACL = "0";
                    }
                    if (OTYPE == null)
                    {
                        OTYPE = "";
                    }


                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Utility.WriteLog(ex);
                return false;
            }
        }
        public bool CALL_GNSR86(string prmPPBUSS, string prmPPLNTY, string prmPPBRN, string prmPPAPDT, string prmPPAVDT, string prmPPRANK, string prmPPITCL,
                                ref string PPOTCL, ref string POERR, ref string PERMSG, string strBizInit, string strBranchNo)
        {
            try
            {
                ESBiDB2.iDB2Command cmd = new ESBiDB2.iDB2Command();
                cmd.CommandType = CommandType.StoredProcedure;
                //gentoken
                cmd.Token = ReuseableToken.token;
                cmd.AccessKey = m_UserInfo.AccessKey; //acn
                cmd.Url = url; //api hisun

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

                //bool isMock = Convert.ToBoolean(WebConfigurationManager.AppSettings["isMock"].ToString());
                //if (isMock)
                //{
                //    PPOTCL = "50000";
                //    POERR = "";
                //    PERMSG = "";
                //    return true;
                //}

                Utility.WriteLogResponse("Request to : " + url + "/" + cmd.CommandText.ToString());
                var dt = cmd.ExecuteReaderOptionalDataSet();
                Utility.WriteLogResponse("Response from : " + url + "/" + cmd.CommandText.ToString(), JsonConvert.SerializeObject(dt));

                var TOA = dt.Tables[0];

                if (dt.Tables.Count > 0)
                {
                    Utility.CheckSubroutineStatus(dt);
                    PPOTCL = cmd.Parameters["PPOTCL"]?.Value?.ToString().Trim();
                    POERR = cmd.Parameters["POERR"]?.Value?.ToString().Trim();
                    PERMSG = cmd.Parameters["PERMSG"]?.Value?.ToString().Trim();
                    if (PPOTCL == null)
                    {
                        PPOTCL = "0";
                    }
                    if (POERR == null)
                    {
                        POERR = "";
                    }
                    if (PERMSG == null)
                    {
                        PERMSG = "";
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Utility.WriteLog(ex);
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
                ESBiDB2.iDB2Command cmd = new ESBiDB2.iDB2Command();
                cmd.CommandType = CommandType.StoredProcedure;
                //gentoken
                cmd.Token = ReuseableToken.token;
                cmd.AccessKey = m_UserInfo.AccessKey; //acn
                cmd.Url = url; //api hisun
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

                //bool isMock = Convert.ToBoolean(WebConfigurationManager.AppSettings["isMock"].ToString());
                //if(isMock)
                //{
                //    POBOTL = "250000.00";
                //    PONOAP = "0";
                //    POCSBL = "0.00";
                //    POCRAV = "87500.00";
                //    POEBCL = "0.00";
                //    POAVAP = "0.00";
                //    POSTS = "87500.00";
                //    POOLDC = "";
                //    POVENR = "Y";
                //    POPERV = "";
                //    POEBCS = "0.00";
                //    POERR = "";
                //    PERMSG = "";
                //    return true;
                //}

                Utility.WriteLogResponse("Request to : " + url + "/" + cmd.CommandText.ToString());
                var dt = cmd.ExecuteReaderOptionalDataSet();
                Utility.WriteLogResponse("Response from : " + url + "/" + cmd.CommandText.ToString(), JsonConvert.SerializeObject(dt));

                var TOA = dt.Tables[0];

                if (dt.Tables.Count > 0)
                {
                    Utility.CheckSubroutineStatus(dt);
                    POBOTL = cmd.Parameters["POBOTL"]?.Value?.ToString().Trim();
                    PONOAP = cmd.Parameters["PONOAP"]?.Value?.ToString().Trim();
                    POCSBL = cmd.Parameters["POCSBL"]?.Value?.ToString().Trim();
                    POCRAV = cmd.Parameters["POCRAV"]?.Value?.ToString().Trim();
                    POAPAM = cmd.Parameters["POAPAM"]?.Value?.ToString().Trim();
                    POEBCL = cmd.Parameters["POEBCL"]?.Value?.ToString().Trim();
                    POAVAP = cmd.Parameters["POAVAP"]?.Value?.ToString().Trim();
                    POSTS = cmd.Parameters["POSTS"]?.Value?.ToString().Trim();
                    POOLDC = cmd.Parameters["POOLDC"]?.Value?.ToString().Trim();
                    POVENR = cmd.Parameters["POVENR"]?.Value?.ToString().Trim();
                    POPERV = cmd.Parameters["POPERV"]?.Value?.ToString().Trim();
                    POEBCS = cmd.Parameters["POEBCS"]?.Value?.ToString().Trim();
                    POERR = cmd.Parameters["POERR"]?.Value?.ToString().Trim();
                    PERMSG = cmd.Parameters["PERMSG"]?.Value?.ToString().Trim();
                    if (POBOTL == null)
                    {
                        POBOTL = "0";
                    }
                    if (PONOAP == null)
                    {
                        PONOAP = "0";
                    }
                    if (POCSBL == null)
                    {
                        POCSBL = "0";
                    }
                    if (POCRAV == null)
                    {
                        POCRAV = "0";
                    }
                    if (POAPAM == null)
                    {
                        POAPAM = "0";
                    }
                    if (POEBCL == null)
                    {
                        POEBCL = "0";
                    }
                    if (POAVAP == null)
                    {
                        POAVAP = "0";
                    }
                    if (POSTS == null)
                    {
                        POSTS = "";
                    }
                    if (POOLDC == null)
                    {
                        POOLDC = "";
                    }
                    if (POVENR == null)
                    {
                        POVENR = "";
                    }
                    if (POPERV == null)
                    {
                        POPERV = "0";
                    }
                    if (POERR == null)
                    {
                        POERR = "";
                    }
                    if (PERMSG == null)
                    {
                        PERMSG = "";
                    }
                    return true;
                }
                else
                {
                    return false;

                }
            }
            catch (Exception ex)
            {
                Utility.WriteLog(ex);
                return false;
            }
        }

        public bool Call_ILSR75(string PICARD, string strBizInit, string strBranchNo, ref string prmError, ref string prmMsg)
        {
            DataSet dsResult = new DataSet();
            ESBiDB2.iDB2Command cmd = new ESBiDB2.iDB2Command();
            try
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "ILSR75";

                //gentoken
                cmd.Token = ReuseableToken.token;
                cmd.AccessKey = m_UserInfo.AccessKey; //acn
                cmd.Url = url; //api hisun

                // INPUT
                cmd.Parameters.Add("PICARD", iDB2DbType.iDB2Char, 16).Value = PICARD.ToString().Trim();
                cmd.Parameters.Add("PIBRN", iDB2DbType.iDB2Char, 3).Value = strBranchNo.ToString().Trim();

                // OUTPUT
                cmd.Parameters.Add("POERRF", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("POERRM", iDB2DbType.iDB2Char, 100).Direction = ParameterDirection.Output;

                Utility.WriteLogResponse("Request to : " + url + "/" + cmd.CommandText.ToString());
                var dt = cmd.ExecuteReaderOptionalDataSet();
                Utility.WriteLogResponse("Response from : " + url + "/" + cmd.CommandText.ToString(), JsonConvert.SerializeObject(dt));

                var TOA = dt.Tables[0];

                if (dt.Tables.Count > 0)
                {
                    Utility.CheckSubroutineStatus(dt);
                    prmError = cmd.Parameters["POERRF"].Value.ToString().Trim();
                    prmMsg = cmd.Parameters["POERRM"].Value.ToString().Trim();
                    if (prmError == null)
                    {
                        prmError = "";
                    }
                    if (prmMsg == null)
                    {
                        prmMsg = "";
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Utility.WriteLog(ex);
                return false;
            }
        }

        public bool Call_GNCSC07(string prmIDNO, string strBizInit, string strBranchNo, ref DataSet ds)
        {
            DataSet dsResult = new DataSet();
            ESBiDB2.iDB2Command cmd = new ESBiDB2.iDB2Command();
            try
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GNCSC07";

                //gentoken
                cmd.Token = ReuseableToken.token;
                cmd.AccessKey = m_UserInfo.AccessKey; //acn
                cmd.Url = url; //api hisun

                // INPUT
                cmd.Parameters.Add("WIIDNO", iDB2DbType.iDB2Char, 15).Value = prmIDNO.ToString().Trim();

                // OUTPUT
                cmd.Parameters.Add("WORESL", iDB2DbType.iDB2Char, 2).Direction = ParameterDirection.Output;

                Utility.WriteLogResponse("Request to : " + url + "/" + cmd.CommandText.ToString());
                var dt = cmd.ExecuteReaderOptionalDataSet();
                Utility.WriteLogResponse("Response from : " + url + "/" + cmd.CommandText.ToString(), JsonConvert.SerializeObject(dt));

                var TOA = dt.Tables[0];

                if (dt.Tables.Count > 0)
                {
                    Utility.CheckSubroutineStatus(dt);
                    ds = dt;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Utility.WriteLog(ex);
                return false;
            }
        }

        public bool Call_CSSR67(string prm1, string prm2, ref string prmErrMsg, string strBizInit, string strBranchNo)
        {
            DataSet dsResult = new DataSet();
            ESBiDB2.iDB2Command cmd = new ESBiDB2.iDB2Command();
            try
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "CSSR67";

                //gentoken
                cmd.Token = ReuseableToken.token;
                cmd.AccessKey = m_UserInfo.AccessKey; //acn
                cmd.Url = url; //api hisun

                // INPUT
                cmd.Parameters.Add("I_CSN", iDB2DbType.iDB2Char, 8).Value = prm1;
                cmd.Parameters.Add("I_PGM", iDB2DbType.iDB2Char, 10).Value = prm2;

                // OUTPUT
                cmd.Parameters.Add("O_ERR", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;

                Utility.WriteLogResponse("Request to : " + url + "/" + cmd.CommandText.ToString());
                var dt = cmd.ExecuteReaderOptionalDataSet();
                Utility.WriteLogResponse("Response from : " + url + "/" + cmd.CommandText.ToString(), JsonConvert.SerializeObject(dt));

                var TOA = dt.Tables[0];

                if (dt.Tables.Count > 0)
                {
                    Utility.CheckSubroutineStatus(dt);
                    prmErrMsg = cmd.Parameters["O_ERR"]?.Value?.ToString();
                    if (prmErrMsg == null)
                    {
                        prmErrMsg = "";
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Utility.WriteLog(ex);
                return false;
            }
        }

        public bool Call_ILSR73(string CONTNO1, string SGDATE, ref string FLGERR1, string FLGMSG1, string strBizInit, string strBranchNo)
        {
            DataSet dsResult = new DataSet();
            ESBiDB2.iDB2Command cmd = new ESBiDB2.iDB2Command();
            try
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "ILSR73";

                //gentoken
                cmd.Token = ReuseableToken.token;
                cmd.AccessKey = m_UserInfo.AccessKey; //acn
                cmd.Url = url; //api hisun

                // INPUT
                cmd.Parameters.Add("CONTNO1", iDB2DbType.iDB2Char, 16).Value = CONTNO1;
                cmd.Parameters.Add("SGDATE", iDB2DbType.iDB2Char, 8).Value = SGDATE;


                cmd.Parameters.Add("FLGERR1", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;
                cmd.Parameters["FLGERR1"].Value = "";

                cmd.Parameters.Add("FLGMSG1", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;
                cmd.Parameters["FLGMSG1"].Value = "";


                Utility.WriteLogResponse("Request to : " + url + "/" + cmd.CommandText.ToString());
                var dt = cmd.ExecuteReaderOptionalDataSet();
                Utility.WriteLogResponse("Response from : " + url + "/" + cmd.CommandText.ToString(), JsonConvert.SerializeObject(dt));

                var TOA = dt.Tables[0];

                if (dt.Tables.Count > 0)
                {
                    Utility.CheckSubroutineStatus(dt);
                    FLGERR1 = cmd.Parameters["FLGERR1"]?.Value?.ToString().Trim();
                    FLGMSG1 = cmd.Parameters["FLGMSG1"]?.Value?.ToString().Trim();
                    if (FLGERR1 == null)
                    {
                        FLGERR1 = "";
                    }
                    if (FLGMSG1 == null)
                    {
                        FLGMSG1 = "";
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Utility.WriteLog(ex);
                return false;
            }
        }

        public bool Call_CSSR06C(ILDataCenter ilObj, string WISYS, string WIIDNO, string WIBRN, string WIAPNO, string WICONT_,
                                string WITCA, string WIRES, string WIAPDT, string WIBHDT, string WICSTY, string WIBNGS, string WISALA,
                                string WITCL, string WIPCAM, string WIVDID, string WIUSOP, ref string WOERR, ref string WOERRM
                                , string strBizInit, string strBranchNo)
        {
            DataSet dsResult = new DataSet();
            ESBiDB2.iDB2Command cmd = new ESBiDB2.iDB2Command();
            try
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "CSSR06C";

                //gentoken
                cmd.Token = ReuseableToken.token;
                cmd.AccessKey = m_UserInfo.AccessKey; //acn
                cmd.Url = url; //api hisun

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

                Utility.WriteLogResponse("Request to : " + url + "/" + cmd.CommandText.ToString());
                var dt = cmd.ExecuteReaderOptionalDataSet();
                Utility.WriteLogResponse("Response from : " + url + "/" + cmd.CommandText.ToString(), JsonConvert.SerializeObject(dt));

                var TOA = dt.Tables[0];

                if (dt.Tables.Count > 0)
                {
                    Utility.CheckSubroutineStatus(dt);
                    WOERR = cmd.Parameters["WOERR"]?.Value?.ToString().Trim();
                    WOERRM = cmd.Parameters["WOERRM"]?.Value?.ToString().Trim();
                    if (WOERR == null)
                    {
                        WOERR = "";
                    }
                    if (WOERRM == null)
                    {
                        WOERRM = "";
                    }
                    return true;
                }
                else
                {
                    return false;
                }
                //WOERR = "";
                //WOERRM = "";
                //return true;

            }
            catch (Exception ex)
            {
                Utility.WriteLogString("CSSR06C", ex.Message.ToString());
                return false;
            }
        }

        public bool Call_CSSR035(string PICSNO, string PIOCCU, string PIDOCF, ref string POERRC, ref string POERRM, string strBizInit, string strBranchNo)
        {
            DataSet dsResult = new DataSet();
            ESBiDB2.iDB2Command cmd = new ESBiDB2.iDB2Command();
            try
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "CSSR035";

                //gentoken
                cmd.Token = ReuseableToken.token;
                cmd.AccessKey = m_UserInfo.AccessKey; //acn
                cmd.Url = url; //api hisun


                cmd.Parameters.Add("PICSNO", iDB2DbType.iDB2Decimal, 0).Value = PICSNO;
                cmd.Parameters["PICSNO"].Precision = 8;
                cmd.Parameters["PICSNO"].Scale = 0;

                cmd.Parameters.Add("PIOCCU", iDB2DbType.iDB2Char, 3).Value = PIOCCU;
                cmd.Parameters.Add("PIDOCF", iDB2DbType.iDB2Char, 1).Value = PIDOCF;

                cmd.Parameters.Add("POERRC", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;
                cmd.Parameters["POERRC"].Value = "";


                cmd.Parameters.Add("POERRM", iDB2DbType.iDB2Char, 64).Direction = ParameterDirection.Output;
                cmd.Parameters["POERRM"].Value = "";

                Utility.WriteLogResponse("Request to : " + url + "/" + cmd.CommandText.ToString());
                var dt = cmd.ExecuteReaderOptionalDataSet();
                Utility.WriteLogResponse("Response from : " + url + "/" + cmd.CommandText.ToString(), JsonConvert.SerializeObject(dt));

                var TOA = dt.Tables[0];

                if (dt.Tables.Count > 0)
                {
                    Utility.CheckSubroutineStatus(dt);
                    POERRC = cmd.Parameters["POERRC"]?.Value?.ToString().Trim();
                    POERRM = cmd.Parameters["POERRM"]?.Value?.ToString().Trim();
                    if (POERRC == null)
                    {
                        POERRC = "";
                    }
                    if (POERRM == null)
                    {
                        POERRM = "";
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Utility.WriteLog(ex);
                return false;
            }
        }

        public bool Call_ILSR02(string WBRN, string WILNTY, ref string WCONT, ref string WOERRF, ref string WOEMSG, string strBizInit, string strBranchNo)
        {
            DataSet dsResult = new DataSet();
            ESBiDB2.iDB2Command cmd = new ESBiDB2.iDB2Command();
            try
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "ILSR02";

                //gentoken
                cmd.Token = ReuseableToken.token;
                cmd.AccessKey = m_UserInfo.AccessKey; //acn
                cmd.Url = url; //api hisun

                cmd.Parameters.Add("WBRN", iDB2DbType.iDB2Char, 3).Value = WBRN;
                cmd.Parameters.Add("WILNTY", iDB2DbType.iDB2Char, 2).Value = WILNTY;
                cmd.Parameters.Add("WCONT", iDB2DbType.iDB2Char, 16).Direction = ParameterDirection.Output;
                cmd.Parameters["WCONT"].Value = "";

                cmd.Parameters.Add("WOERRF", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;
                cmd.Parameters["WOERRF"].Value = "";

                cmd.Parameters.Add("WOEMSG", iDB2DbType.iDB2Char, 60).Direction = ParameterDirection.Output;
                cmd.Parameters["WOEMSG"].Value = "";
                Utility.WriteLogResponse("Request to : " + url + "/" + cmd.CommandText.ToString());
                var dt = cmd.ExecuteReaderOptionalDataSet();
                Utility.WriteLogResponse("Response from : " + url + "/" + cmd.CommandText.ToString(), JsonConvert.SerializeObject(dt));

                var TOA = dt.Tables[0];

                if (dt.Tables.Count > 0)
                {
                    Utility.CheckSubroutineStatus(dt);
                    WCONT = cmd.Parameters["WCONT"]?.Value?.ToString().Trim();
                    WOERRF = cmd.Parameters["WOERRF"]?.Value?.ToString().Trim();
                    WOEMSG = cmd.Parameters["WOEMSG"]?.Value?.ToString().Trim();
                    if (WCONT == null)
                    {
                        WCONT = "";
                    }
                    if (WOERRF == null)
                    {
                        WOERRF = "";
                    }
                    if (WOEMSG == null)
                    {
                        WOEMSG = "";
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Utility.WriteLog(ex);
                return false;
            }
        }

        public bool Call_GNSR45(string IOFNME, string ITITLE, string ITELOF, string IBIZ, string IBRAN, string IAPP, string IAPDTE,
                                ref string OAPP, ref string OMSGA, ref string OTEL, ref string OMSGT, ref string ONER, ref string OMSG
                                , string strBizInit, string strBranchNo)
        {
            DataSet dsResult = new DataSet();
            ESBiDB2.iDB2Command cmd = new ESBiDB2.iDB2Command();
            try
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GNSR45";

                //gentoken
                cmd.Token = ReuseableToken.token;
                cmd.AccessKey = m_UserInfo.AccessKey; //acn
                cmd.Url = url; //api hisun

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

                Utility.WriteLogResponse("Request to : " + url + "/" + cmd.CommandText.ToString());
                var dt = cmd.ExecuteReaderOptionalDataSet();
                Utility.WriteLogResponse("Response from : " + url + "/" + cmd.CommandText.ToString(), JsonConvert.SerializeObject(dt));

                var TOA = dt.Tables[0];

                if (dt.Tables.Count > 0)
                {
                    Utility.CheckSubroutineStatus(dt);
                    OAPP = cmd.Parameters["OAPP"]?.Value?.ToString().Trim();
                    OMSGA = cmd.Parameters["OMSGA"]?.Value?.ToString().Trim();
                    OTEL = cmd.Parameters["OTEL"]?.Value?.ToString().Trim();
                    OMSGT = cmd.Parameters["OMSGT"]?.Value?.ToString().Trim();
                    ONER = cmd.Parameters["ONER"]?.Value?.ToString().Trim();
                    OMSG = cmd.Parameters["OMSG"]?.Value?.ToString().Trim();
                    if (OAPP == null)
                    {
                        OAPP = "0";
                    }
                    if (OMSGA == null)
                    {
                        OMSGA = "";
                    }
                    if (OTEL == null)
                    {
                        OTEL = "0";
                    }
                    if (OMSGT == null)
                    {
                        OMSGT = "";
                    }
                    if (ONER == null)
                    {
                        ONER = "";
                    }
                    if (OMSG == null)
                    {
                        OMSG = "";
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Utility.WriteLog(ex);
                return false;
            }
        }

        public bool CALL_CSSR31(string prmPICSN, string prmPISAL, string prmPIFLG, string prmPIRANK, string prmPIOCC, string prmBRN, string prmPISINCP, string prmPINCAJ,
                                ref string POSALN, ref string POORNK, ref string POOTMS, ref string POOACL, ref string POARNK, ref string POATMS, ref string POAACL,
                                ref string PORRNK, ref string PORTMS, ref string PORACL, ref string POFTCL, ref string POSTRP, ref string POTOTP, ref string POAFLG,
                                ref string POHAVE, ref string POODGC, ref string POMDL, ref string POPD, ref string POGNO,
                                string strBizInit, string strBranchNo)
        {
            try
            {
                EB_Service.DAL.DataCenter MSSQL = new EB_Service.DAL.DataCenter(m_UserInfo);

                ESBiDB2.iDB2Command cmd = new ESBiDB2.iDB2Command();
                cmd.CommandType = CommandType.StoredProcedure;
                //gentoken
                cmd.Token = ReuseableToken.token;
                cmd.AccessKey = m_UserInfo.AccessKey; //acn
                cmd.Url = url; //api hisun
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

                bool isMock = Convert.ToBoolean(WebConfigurationManager.AppSettings["isMock"].ToString());
                if (isMock)
                {
                    POSALN = "0.00";
                    POORNK = "0";
                    POOTMS = "0.000";
                    POOACL = "0.00";
                    POARNK = "0";
                    POATMS = "0.000";
                    POAACL = "0.00";
                    PORRNK = "0";
                    PORTMS = "0.000";
                    PORACL = "0.00";
                    POFTCL = "0.00";
                    POSTRP = "0";
                    POTOTP = "0";
                    POAFLG = "";
                    POHAVE = "N";
                    POODGC = "0";
                    POMDL = "";
                    POPD = "0.000000000";
                    POGNO = "0";
                    return true;
                }


                Utility.WriteLogResponse("Request to : " + url + "/" + cmd.CommandText.ToString());
                var dt = cmd.ExecuteReaderOptionalDataSet();
                Utility.WriteLogResponse("Response from : " + url + "/" + cmd.CommandText.ToString(), JsonConvert.SerializeObject(dt));

                var TOA = dt.Tables[0];

                if (dt.Tables.Count > 0)
                {
                    Utility.CheckSubroutineStatus(dt);
                    POSALN = cmd.Parameters["POSALN"]?.Value?.ToString().Trim();
                    POORNK = cmd.Parameters["POORNK"]?.Value?.ToString().Trim();
                    POOTMS = cmd.Parameters["POOTMS"]?.Value?.ToString().Trim();
                    POOACL = cmd.Parameters["POOACL"]?.Value?.ToString().Trim();
                    POARNK = cmd.Parameters["POARNK"]?.Value?.ToString().Trim();
                    POATMS = cmd.Parameters["POATMS"]?.Value?.ToString().Trim();
                    POAACL = cmd.Parameters["POAACL"]?.Value?.ToString().Trim();
                    PORRNK = cmd.Parameters["PORRNK"]?.Value?.ToString().Trim();
                    PORTMS = cmd.Parameters["PORTMS"]?.Value?.ToString().Trim();
                    PORACL = cmd.Parameters["PORACL"]?.Value?.ToString().Trim();
                    POFTCL = cmd.Parameters["POFTCL"]?.Value?.ToString().Trim();
                    POSTRP = cmd.Parameters["POSTRP"]?.Value?.ToString().Trim();
                    POTOTP = cmd.Parameters["POTOTP"]?.Value?.ToString().Trim();
                    POAFLG = cmd.Parameters["POAFLG"]?.Value?.ToString().Trim();
                    POHAVE = cmd.Parameters["POHAVE"]?.Value?.ToString().Trim();
                    POODGC = cmd.Parameters["POODGC"]?.Value?.ToString().Trim();
                    POMDL = cmd.Parameters["POMDL"]?.Value?.ToString().Trim();
                    POPD = cmd.Parameters["POPD"]?.Value?.ToString().Trim();
                    POGNO = cmd.Parameters["POGNO"]?.Value?.ToString().Trim();
                    if (POSALN == null)
                    {
                        POSALN = "0";
                    }
                    if (POORNK == null)
                    {
                        POORNK = "0";
                    }
                    if (POOTMS == null)
                    {
                        POOTMS = "0";
                    }
                    if (POOACL == null)
                    {
                        POOACL = "0";
                    }
                    if (POARNK == null)
                    {
                        POARNK = "0";
                    }
                    if (POATMS == null)
                    {
                        POATMS = "0";
                    }
                    if (POAACL == null)
                    {
                        POAACL = "0";
                    }
                    if (PORRNK == null)
                    {
                        PORRNK = "0";
                    }
                    if (PORTMS == null)
                    {
                        PORTMS = "0";
                    }
                    if (PORACL == null)
                    {
                        PORACL = "0";
                    }
                    if (POFTCL == null)
                    {
                        POFTCL = "0";
                    }
                    if (POSTRP == null)
                    {
                        POSTRP = "0";
                    }
                    if (POTOTP == null)
                    {
                        POTOTP = "0";
                    }
                    if (POAFLG == null)
                    {
                        POAFLG = "";
                    }
                    if (POHAVE == null)
                    {
                        POHAVE = "";
                    }
                    if (POODGC == null)
                    {
                        POODGC = "0";
                    }
                    if (POMDL == null)
                    {
                        POMDL = "";
                    }
                    if (POPD == null)
                    {
                        POPD = "0";
                    }
                    if (POGNO == null)
                    {
                        POGNO = "0";
                    }
                    //-- เพิ่มตอน Request 75972 เนื่องจาก Version source หายตั้งแต่ 08/05/2018
                    DataSet DS_RLTB10 = get_RLTB10();
                    if (DS_RLTB10 != null && DS_RLTB10.Tables.Count > 0)
                    {
                        if (DS_RLTB10.Tables[0].Rows.Count > 0)
                        {
                            if (Convert.ToDecimal(prmPISAL) < Convert.ToDecimal(DS_RLTB10.Tables[0].Rows[0]["T10CD2"]))
                            {
                                #region "check  Freeze TCL"
                                DataSet ds = new DataSet();
                                try
                                {


                                    var resultTCL = MSSQL.GetDataset<DataSet>($@" SELECT M57NTCL FROM [AS400DB01].[CSOD0001].csms57 WHERE m57csn = " + prmPICSN + " AND m57lkb = 'BOT' ", CommandType.Text).Result;

                                    ds = resultTCL.data.Tables.Count > 0 ? resultTCL.data : new DataSet();


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
                                }
                                #endregion
                            }
                        }
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Utility.WriteLog(ex);
                return false;

            }
        }

        public bool CALL_ILSR16(string Mode_IN, string CSN_IN, string BRN_IN, string APPNO_IN, ref string prmError, string strBizInit, string strBranchNo)
        {
            DataSet dsResult = new DataSet();
            ESBiDB2.iDB2Command cmd = new ESBiDB2.iDB2Command();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ILSR16";

            //gentoken
            cmd.Token = ReuseableToken.token;
            cmd.AccessKey = m_UserInfo.AccessKey; //acn
            cmd.Url = url; //api hisun

            // INPUT
            cmd.Parameters.Add("WIMODE", iDB2DbType.iDB2Char, 1).Value = Mode_IN.ToString().Trim();
            cmd.Parameters.Add("WICSN", iDB2DbType.iDB2Char, 8).Value = CSN_IN.ToString().Trim();
            cmd.Parameters.Add("WIBRN", iDB2DbType.iDB2Char, 3).Value = BRN_IN.ToString().Trim();
            cmd.Parameters.Add("WIAPN", iDB2DbType.iDB2Char, 11).Value = APPNO_IN.ToString().Trim();

            // OUTPUT
            cmd.Parameters.Add("WOERR", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;

            var dt = cmd.ExecuteReaderOptionalDataSet();


            if (dt.Tables.Count > 0)
            {
                Utility.CheckSubroutineStatus(dt);
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
                                 ref string prmReqno, ref string prmErrMsg, string strBizInit, string strBranchNo) //[56991] เพิ่ม Parameter IP, Parm สำหรับ Initial Lib -Biz, -Branch No
        {
            DataSet dsResult = new DataSet();
            ESBiDB2.iDB2Command cmd = new ESBiDB2.iDB2Command();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "CSSR034";

            //gentoken
            cmd.Token = ReuseableToken.token;
            cmd.AccessKey = m_UserInfo.AccessKey; //acn
            cmd.Url = url; //api hisun

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

            var dt = cmd.ExecuteReaderOptionalDataSet();

            if (dt.Tables.Count > 0)
            {
                Utility.CheckSubroutineStatus(dt);
                prmReqno = cmd.Parameters["POERRC"]?.Value?.ToString();
                prmErrMsg = cmd.Parameters["POERRM"]?.Value?.ToString();
                if (prmReqno == null)
                {
                    prmReqno = "";
                }
                if (prmErrMsg == null)
                {
                    prmErrMsg = "";
                }
                return true;
            }
            else
                return false;
        }
        public DataSet CALL_ILE02222(string prmIDNO, string prmGROUP, string prmTEL, string prmBUS,
           string strBizInit, string strBranchNo)
        {
            DataSet dsResult = new DataSet();
            ESBiDB2.iDB2Command cmd = new ESBiDB2.iDB2Command();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ILE02222";

            //gentoken
            cmd.Token = ReuseableToken.token;
            cmd.AccessKey = m_UserInfo.AccessKey; //acn
            cmd.Url = url; //api hisun

            cmd.Parameters.Add("PIIDNO", iDB2DbType.iDB2Char, 15).Value = prmIDNO.ToString().Trim();
            cmd.Parameters.Add("PIGROP", iDB2DbType.iDB2Char, 5).Value = prmGROUP.ToString().Trim();
            cmd.Parameters.Add("PITELN", iDB2DbType.iDB2Char, 20).Value = prmTEL.ToString().Trim();
            cmd.Parameters.Add("PIBUS", iDB2DbType.iDB2Char, 2).Value = prmBUS.ToString().Trim();

            //cmd.Parameters.Add("PIIDNO", iDB2DbType.iDB2Char, 15).Value = "1341400082108";//prmIDNO.ToString().Trim();
            //cmd.Parameters.Add("PIGROP", iDB2DbType.iDB2Char, 5).Value = "TH"; //prmGROUP.ToString().Trim();
            //cmd.Parameters.Add("PITELN", iDB2DbType.iDB2Char, 20).Value = "021111111"; //prmTEL.ToString().Trim();
            //cmd.Parameters.Add("PIBUS", iDB2DbType.iDB2Char, 2).Value = "IL";// prmBUS.ToString().Trim();
            try
            {
                var dt = cmd.ExecuteReaderOptionalDataSet();

                if (dt.Tables.Count > 0)
                {
                    Utility.CheckSubroutineStatus(dt);
                    if (dt.Tables.Count > 1)
                    {
                        DataTable dataTable = dt.Tables["Table2"];
                        dt.Tables.Remove(dataTable);
                        dsResult.Tables.Add(dataTable);

                    }
                }
            }
            catch (Exception ex)
            {
                return dsResult;
            }
            return dsResult;
        }

        public bool CALL_CSSR032(string PPMODE, string PPAPPL, string PPFLAG, string PPCSNO, string PPBUSS, string PPBRAN, string PPAPNO,
                                 string PPSDAT, string PPSTIM, ref string PRTNCD, ref string PERMSG1, string strBizInit, string strBranchNo)
        {
            DataSet dsResult = new DataSet();
            ESBiDB2.iDB2Command cmd = new ESBiDB2.iDB2Command();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "CSSR032";

            //gentoken
            cmd.Token = ReuseableToken.token;
            cmd.AccessKey = m_UserInfo.AccessKey; //acn
            cmd.Url = url; //api hisun

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
            cmd.Parameters.Add("PRTNCD", iDB2DbType.iDB2Char, 3).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("PERMSG1", iDB2DbType.iDB2Char, 64).Direction = ParameterDirection.Output;

            var dt = cmd.ExecuteReaderOptionalDataSet();

            if (dt.Tables.Count > 0)
            {
                Utility.CheckSubroutineStatus(dt);
                PRTNCD = cmd.Parameters["PRTNCD"]?.Value?.ToString().Trim();
                PERMSG1 = cmd.Parameters["PERMSG1"]?.Value?.ToString().Trim();
                if (PRTNCD == null)
                {
                    PRTNCD = "";
                }
                if (PERMSG1 == null)
                {
                    PERMSG1 = "";
                }
                return true;
            }
            else
                return false;
        }
        public bool Call_CSSR68(string prm1, string prm2, ref string prmErrMsg, string strBizInit, string strBranchNo)
        {
            ESBiDB2.iDB2Command cmd = new ESBiDB2.iDB2Command();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "CSSR68";

            //gentoken
            cmd.Token = ReuseableToken.token;
            cmd.AccessKey = m_UserInfo.AccessKey; //acn
            cmd.Url = url; //api hisun

            // Parameter In
            cmd.Parameters.Add("PICSNO", iDB2DbType.iDB2Char, 8).Value = prm1;
            cmd.Parameters.Add("PIALL", iDB2DbType.iDB2Char, 1).Value = prm2;
            cmd.Parameters.Add("POERR", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;

            var dt = cmd.ExecuteReaderOptionalDataSet();


            if (dt.Tables.Count > 0)
            {
                Utility.CheckSubroutineStatus(dt);
                prmErrMsg = cmd.Parameters["POERR"]?.Value?.ToString();
                if (prmErrMsg == null)
                {
                    prmErrMsg = "";
                }
                return true;
            }
            else
                return false;
        }
        public bool CALL_CSGC216(string PICSNO, string PIIDNO, string strBizInit, string strBranchNo, ref string POERR, ref string POMSG)
        {
            iDB2Command cmd = new iDB2Command();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "CSGC216";

            //gentoken
            cmd.Token = ReuseableToken.token;
            cmd.AccessKey = m_UserInfo.AccessKey; //acn
            cmd.Url = url; //api hisun

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

            var dt = cmd.ExecuteReaderOptionalDataSet();

            if (dt.Tables.Count > 0)
            {
                Utility.CheckSubroutineStatus(dt);
                POERR = cmd.Parameters["POERR"]?.Value?.ToString().Trim();
                POMSG = cmd.Parameters["POMSG"]?.Value?.ToString().Trim();
                if (POERR == null)
                {
                    POERR = "";
                }
                if (POMSG == null)
                {
                    POMSG = "";
                }
                return true;
            }
            else
                return false;
        }
        public bool Call_ILSR73(string CONTNO1, string SGDATE, string FLGERR1, string FLGMSG1, string strBizInit, string strBranchNo)
        {
            iDB2Command cmd = new iDB2Command();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ILSR73";

            //gentoken
            cmd.Token = ReuseableToken.token;
            cmd.AccessKey = m_UserInfo.AccessKey; //acn
            cmd.Url = url; //api hisun

            cmd.Parameters.Add("CONTNO1", iDB2DbType.iDB2Char, 16).Value = CONTNO1;
            cmd.Parameters.Add("SGDATE", iDB2DbType.iDB2Char, 8).Value = SGDATE;


            cmd.Parameters.Add("FLGERR1", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;
            cmd.Parameters["FLGERR1"].Value = "";

            cmd.Parameters.Add("FLGMSG1", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;
            cmd.Parameters["FLGMSG1"].Value = "";

            var dt = cmd.ExecuteReaderOptionalDataSet();

            if (dt.Tables.Count > 0)
            {
                Utility.CheckSubroutineStatus(dt);
                FLGERR1 = cmd.Parameters["FLGERR1"]?.Value?.ToString().Trim();
                FLGMSG1 = cmd.Parameters["FLGMSG1"]?.Value?.ToString().Trim();
                if (FLGERR1 == null)
                {
                    FLGERR1 = "";
                }
                if (FLGMSG1 == null)
                {
                    FLGMSG1 = "";
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Call_ILG002CL(string prmBRN, string prmSDATE, string prmEDATE, string prmSTIM, string prmETIM, string prmUSER, string prmVEND, string prmTFAX, string prmAPP, string prmTYP, string strBizInit, string strBranchNo)
        {
            iDB2Command cmd = new iDB2Command();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ILG002CL";

            //gentoken
            cmd.Token = ReuseableToken.token;
            cmd.AccessKey = m_UserInfo.AccessKey; //acn
            cmd.Url = url; //api hisun

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

            var dt = cmd.ExecuteReaderOptionalDataSet();

            if (dt.Tables.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CALL_ILSR37(string PIAPRJ, string PILOCA, string PIRSTS, ref string POTEXT, string strBizInit, string strBranchNo)
        {

            DataSet dsResult = new DataSet();
            try
            {

                ESBiDB2.iDB2Command cmd = new ESBiDB2.iDB2Command();
                cmd.CommandType = CommandType.StoredProcedure;
                //gentoken
                cmd.Token = ReuseableToken.token;
                cmd.AccessKey = m_UserInfo.AccessKey; //acn
                cmd.Url = url; //api hisun
                //GeneralLib;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "ILSR37";

                cmd.Parameters.Add("PIAPRJ", iDB2DbType.iDB2Char, 2).Value = PIAPRJ;
                cmd.Parameters.Add("PILOCA", iDB2DbType.iDB2Char, 3).Value = PILOCA;
                cmd.Parameters.Add("PIRSTS", iDB2DbType.iDB2Char, 1).Value = PIRSTS;


                cmd.Parameters.Add("POTEXT", iDB2DbType.iDB2Char, 20).Direction = ParameterDirection.Output;
                cmd.Parameters["POTEXT"].Value = "";

                Utility.WriteLogResponse("Request to : " + url + "/" + cmd.CommandText.ToString());
                var dt = cmd.ExecuteReaderOptionalDataSet();
                Utility.WriteLogResponse("Response from : " + url + "/" + cmd.CommandText.ToString(), JsonConvert.SerializeObject(dt));

                var TOA = dt.Tables[0];

                if (dt.Tables.Count > 0)
                {
                    Utility.CheckSubroutineStatus(dt);
                    POTEXT = cmd.Parameters["POTEXT"]?.Value?.ToString().Trim();
                    if (POTEXT == null)
                    {
                        POTEXT = "";
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Utility.WriteLog(ex);
                return false;
            }

        }
        public bool CALL_GNSRCONM(string WKLNG, string WKTYP, string WKLEN, ref string WKNME)
        {
            string m_UdpTime = DateTime.Now.ToString("HHmmss");
            iDB2Command cmd = new iDB2Command();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "GNSRCONM";

            //gentoken
            cmd.Token = ReuseableToken.token;
            cmd.AccessKey = m_UserInfo.AccessKey; //acn
            cmd.Url = url; //api hisun

            // Parameter In
            cmd.Parameters.Add("WKLNG", iDB2DbType.iDB2Char, 2).Value = WKLNG;
            cmd.Parameters.Add("WKTYP", iDB2DbType.iDB2Char, 0).Value = WKTYP;
            cmd.Parameters.Add("WKLEN", iDB2DbType.iDB2Decimal, 0).Value = WKLEN;
            cmd.Parameters["WKLEN"].Precision = 3;
            cmd.Parameters["WKLEN"].Scale = 0;
            cmd.Parameters.Add("WKNME", iDB2DbType.iDB2Char, 100).Direction = ParameterDirection.Output;

            Utility.WriteLogResponse("Request to : " + url + "/" + cmd.CommandText.ToString());
            var dt = cmd.ExecuteReaderOptionalDataSet();
            Utility.WriteLogResponse("Response from : " + url + "/" + cmd.CommandText.ToString(), JsonConvert.SerializeObject(dt));

            if (dt.Tables.Count > 0)
            {
                Utility.CheckSubroutineStatus(dt);
                WKNME = cmd.Parameters["WKNME"]?.Value?.ToString().Trim();
                if (WKNME == null)
                {
                    WKNME = "";
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Call_ILSR10(string WIPMTY, string WIADD, string WIPROV, string WIRUNN, ref string WOCODE, ref string WOERR, ref string WOEMSG)
        {
            iDB2Command cmd = new iDB2Command();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ILSR10";

            //gentoken
            cmd.Token = ReuseableToken.token;
            cmd.AccessKey = m_UserInfo.AccessKey; //acn
            cmd.Url = url; //api hisun

            //Input
            cmd.Parameters.Add("WIPMTY", iDB2DbType.iDB2Char, 1).Value = WIPMTY;
            cmd.Parameters.Add("WIADD", iDB2DbType.iDB2Char, 1).Value = WIADD;
            cmd.Parameters.Add("WIPROV", iDB2DbType.iDB2Char, 2).Value = WIPROV;
            cmd.Parameters.Add("WIRUNN", iDB2DbType.iDB2Char, 6).Value = WIRUNN;

            //Output
            cmd.Parameters.Add("WOCODE", iDB2DbType.iDB2Char, 12).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("WOERR", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("WOEMSG", iDB2DbType.iDB2Char, 60).Direction = ParameterDirection.Output;

            Utility.WriteLogResponse("Request to : " + url + "/" + cmd.CommandText.ToString());
            var dt = cmd.ExecuteReaderOptionalDataSet();
            Utility.WriteLogResponse("Response from : " + url + "/" + cmd.CommandText.ToString(), JsonConvert.SerializeObject(dt));

            if (dt.Tables.Count > 0)
            {
                Utility.CheckSubroutineStatus(dt);
                WOCODE = cmd.Parameters["WOCODE"]?.Value?.ToString().Trim();
                WOERR = cmd.Parameters["WOERR"]?.Value?.ToString().Trim();
                WOEMSG = cmd.Parameters["WOEMSG"]?.Value?.ToString().Trim();
                if (WOCODE == null)
                {
                    WOCODE = "";
                }
                if (WOERR == null)
                {
                    WOERR = "";
                }
                if (WOEMSG == null)
                {
                    WOEMSG = "";
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Call_ILSR92(string WILNTY, string WIRECD, ref string WORUNN, ref string WOLENG, ref string WOERRF, ref string WOEMSG)
        {
            iDB2Command cmd = new iDB2Command();

            /* m_da400.AS400Lib = EB_Service.Commons.AppConfig.GNPGMLib;  *///GeneralLib;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ILSR92";

            //gentoken
            cmd.Token = ReuseableToken.token;
            cmd.AccessKey = m_UserInfo.AccessKey; //acn
            cmd.Url = url; //api hisun

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

            Utility.WriteLogResponse("Request to : " + url + "/" + cmd.CommandText.ToString());
            var dt = cmd.ExecuteReaderOptionalDataSet();
            Utility.WriteLogResponse("Response from : " + url + "/" + cmd.CommandText.ToString(), JsonConvert.SerializeObject(dt));

            if (dt.Tables.Count > 0)
            {
                Utility.CheckSubroutineStatus(dt);
                WORUNN = cmd.Parameters["WORUNN"]?.Value?.ToString().Trim();
                WOLENG = cmd.Parameters["WOLENG"]?.Value?.ToString().Trim();
                WOERRF = cmd.Parameters["WOERRF"]?.Value?.ToString().Trim();
                WOEMSG = cmd.Parameters["WOEMSG"]?.Value?.ToString().Trim();
                if (WORUNN == null)
                {
                    WORUNN = "";
                }
                if (WOLENG == null)
                {
                    WOLENG = "";
                }
                if (WOERRF == null)
                {
                    WOERRF = "";
                }
                if (WOEMSG == null)
                {
                    WOEMSG = "";
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool Call_GNSR221(string WKAPPL, string WKCONT, string WKCKDT, ref string WKODT1, ref string WKODA1, ref string WKODT2, ref string WKODA2)
        {
            iDB2Command cmd = new iDB2Command();

            /* m_da400.AS400Lib = EB_Service.Commons.AppConfig.GNPGMLib;  *///GeneralLib;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "GNSR221";

            //gentoken
            cmd.Token = ReuseableToken.token;
            cmd.AccessKey = m_UserInfo.AccessKey; //acn
            cmd.Url = url; //api hisun

            //Input
            cmd.Parameters.Add("WKAPPL", iDB2DbType.iDB2Char, 2).Value = WKAPPL;
            cmd.Parameters.Add("WKCONT", iDB2DbType.iDB2Char, 16).Value = WKCONT;
            cmd.Parameters.Add("WKCKDT", iDB2DbType.iDB2Decimal, 0).Value = WKCKDT;
            cmd.Parameters["WKCKDT"].Precision = 6;
            cmd.Parameters["WKCKDT"].Scale = 0;


            //Output
            cmd.Parameters.Add("WKODT1", iDB2DbType.iDB2Decimal, 0).Direction = ParameterDirection.Output;
            cmd.Parameters["WKODT1"].Precision = 3;
            cmd.Parameters["WKODT1"].Scale = 0;
            cmd.Parameters["WKODT1"].Value = 0;

            cmd.Parameters.Add("WKODA1", iDB2DbType.iDB2Decimal, 0).Direction = ParameterDirection.Output;
            cmd.Parameters["WKODA1"].Precision = 13;
            cmd.Parameters["WKODA1"].Scale = 2;
            cmd.Parameters["WKODA1"].Value = 0;

            cmd.Parameters.Add("WKODT2", iDB2DbType.iDB2Decimal, 0).Direction = ParameterDirection.Output;
            cmd.Parameters["WKODT2"].Precision = 3;
            cmd.Parameters["WKODT2"].Scale = 0;
            cmd.Parameters["WKODT2"].Value = 0;

            cmd.Parameters.Add("WKODA2", iDB2DbType.iDB2Decimal, 0).Direction = ParameterDirection.Output;
            cmd.Parameters["WKODA2"].Precision = 13;
            cmd.Parameters["WKODA2"].Scale = 2;
            cmd.Parameters["WKODA2"].Value = 0;

            Utility.WriteLogResponse("Request to : " + url + "/" + cmd.CommandText.ToString());
            var dt = cmd.ExecuteReaderOptionalDataSet();
            Utility.WriteLogResponse("Response from : " + url + "/" + cmd.CommandText.ToString(), JsonConvert.SerializeObject(dt));

            if (dt.Tables.Count > 0)
            {
                Utility.CheckSubroutineStatus(dt);
                WKODT1 = cmd.Parameters["WKODT1"]?.Value?.ToString().Trim();
                WKODA1 = cmd.Parameters["WKODA1"]?.Value?.ToString().Trim();
                WKODT2 = cmd.Parameters["WKODT2"]?.Value?.ToString().Trim();
                WKODA2 = cmd.Parameters["WKODA2"]?.Value?.ToString().Trim();
                if (WKODT1 == null)
                {
                    WKODT1 = "0";
                }
                if (WKODA1 == null)
                {
                    WKODA1 = "0";
                }
                if (WKODT2 == null)
                {
                    WKODT2 = "0";
                }
                if (WKODA2 == null)
                {
                    WKODA2 = "0";
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public DataSet getVendor(string vendor = "", string brn = "")
        {
            DataSet ds = new DataSet();
            //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            //UserInfomation = m_UserInfo;

            EB_Service.DAL.DataCenter MSSQL = new EB_Service.DAL.DataCenter(m_UserInfo);


            try
            {
                string sql = "";
                if (vendor == "")
                {
                    sql = " SELECT distinct(FORMAT(P10VEN,'000000000000')) as p10ven, p10nam, p10fi1,p10grd,P16RNK,p10spc FROM [AS400DB01].ILOD0001.ilms10 " +
                          " LEFT JOIN [AS400DB01].ILOD0001.ilms16 ON (p10VEN=P16VEN and (" + m_UdpD.ToString() + " BETWEEN P16STD AND P16END )) " +
                          " WHERE p10del = '' AND " + m_UdpD.ToString().Trim() +
                          " BETWEEN p10fjd    AND p10edt " +
                          //" AND  (" + m_UdpD.ToString() + " BETWEEN P16STD AND P16END )" +
                          " AND exists (SELECT d10ven FROM [AS400DB01].ILOD0001.ilmd10 WHERE ilms10.p10ven=d10ven and d10apt='01') " +
                          " AND P10BRN = " + brn +
                          " ORDER BY p10nam, p10ven ";
                }
                else
                {
                    sql = " SELECT FORMAT(P10VEN,'000000000000') as p10ven, p10nam, p10fi1,p10grd,P16RNK,p10spc FROM [AS400DB01].ILOD0001.ilms10 " +
                          " LEFT JOIN [AS400DB01].ILOD0001.ilms16 ON (p10VEN=P16VEN and (" + m_UdpD.ToString() + " BETWEEN P16STD AND P16END )) " +
                          " WHERE p10del = '' AND " + m_UdpD.ToString().Trim() +
                          " BETWEEN p10fjd    AND p10edt " +
                          //" AND  (" + m_UdpD.ToString() + " BETWEEN P16STD AND P16END )" +
                          " AND p10ven = " + vendor +
                          " AND exists (SELECT d10ven FROM [AS400DB01].ILOD0001.ilmd10 WHERE ilms10.p10ven=d10ven and d10apt='01') " +
                          " AND P10BRN = " + brn +
                          " ORDER BY p10nam, p10ven ";
                }
                //Utility.WriteLogString("sql vendor =>"+sql);
                var resultTCL = MSSQL.GetDataset<DataSet>(sql, CommandType.Text).Result;

                ds = resultTCL.data.Tables.Count > 0 ? resultTCL.data : new DataSet();

                //ds = RetriveAsDataSet(sql);
                //CloseConnectioDAL();
            }
            catch (Exception ex)
            {
                //CloseConnectioDAL();
            }
            return ds;
        }
        public DataSet getILTB06()
        {
            DataSet ds = new DataSet();
            //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            EB_Service.DAL.DataCenter MSSQL = new EB_Service.DAL.DataCenter(m_UserInfo);

            try
            {
                string condition = "";
                string sql = " SELECT * FROM [AS400DB01].ILOD0001.ILTB06 ";

                var resultTCL = MSSQL.GetDataset<DataSet>(sql, CommandType.Text).Result;

                ds = resultTCL.data.Tables.Count > 0 ? resultTCL.data : new DataSet();

            }
            catch (Exception ex)
            {
            }
            return ds;
        }
        public DataSet getPaymentType()
        {
            DataSet ds = new DataSet();
            EB_Service.DAL.DataCenter MSSQL = new EB_Service.DAL.DataCenter(m_UserInfo);
            //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            try
            {
                string sql = "SELECT gt48tc,gt48td FROM [AS400DB01].GNOD0000.gntb48 WITH (NOLOCK) WHERE gt48fl = 'IL'  ";

                var resultTCL = MSSQL.GetDataset<DataSet>(sql, CommandType.Text).Result;

                ds = resultTCL.data.Tables.Count > 0 ? resultTCL.data : new DataSet();

            }
            catch (Exception ex)
            {
            }
            return ds;
        }
        public DataSet getBankCode()
        {
            DataSet ds = new DataSet();
            EB_Service.DAL.DataCenter MSSQL = new EB_Service.DAL.DataCenter(m_UserInfo);

            try
            {
                string sql = " SELECT g32bnk, gnb30c,g32fil FROM [AS400DB01].GNOD0000.gnmb32 WITH (NOLOCK) " +
                             " LEFT JOIN [AS400DB01].GNOD0000.gnmb30 WITH (NOLOCK) on(g32bnk = gnb30a) " +
                             " WHERE g32app='IL' AND g32typ = 'A' ";
                var result = MSSQL.GetDataset<DataSet>(sql, CommandType.Text).Result;

                ds = result.data.Tables.Count > 0 ? result.data : new DataSet();
            }
            catch (Exception ex)
            {

            }
            return ds;
        }

        public DataSet getBankBranch(string bankCode)
        {
            DataSet ds = new DataSet();
            //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            EB_Service.DAL.DataCenter MSSQL = new EB_Service.DAL.DataCenter(m_UserInfo);

            try
            {
                string sql = " SELECT gnb31c, gnb31d   " +
                             " FROM [AS400DB01].GNOD0000.gnmb31 WITH (NOLOCK) " +
                             " WHERE gnb31a = '" + bankCode + "' ORDER BY gnb31c ";
                var result = MSSQL.GetDataset<DataSet>(sql, CommandType.Text).Result;

                ds = result.data.Tables.Count > 0 ? result.data : new DataSet();

            }
            catch (Exception ex)
            {

            }
            return ds;
        }

        public DataSet getAccountType()
        {
            DataSet ds = new DataSet();
            EB_Service.DAL.DataCenter MSSQL = new EB_Service.DAL.DataCenter(m_UserInfo);

            try
            {
                string sql = " SELECT gn13cd,gn13td FROM [AS400DB01].GNOD0000.gntb13 WITH (NOLOCK) ORDER BY gn13cd ";
                var result = MSSQL.GetDataset<DataSet>(sql, CommandType.Text).Result;

                ds = result.data.Tables.Count > 0 ? result.data : new DataSet();

            }
            catch (Exception ex)
            {

            }
            return ds;
        }

        public DataSet getDebitAccountByCSN(string csn)
        {
            DataSet ds = new DataSet();
            EB_Service.DAL.DataCenter MSSQL = new EB_Service.DAL.DataCenter(m_UserInfo);

            try
            {
                string sql = " SELECT  DISTINCT gn13cd,gn13td,gnb31a,gnb31c,gnb31d, " +
                             " gnb30a,gnb30c,p00cis,p00bnk,p00aty,p00bbr,p00sts,p00bac  " +
                             " FROM [AS400DB01].ILOD0001.ilms00 " +
                             " LEFT JOIN [AS400DB01].GNOD0000.gnmb30 ON P00BNK = gnb30a " +
                             " LEFT JOIN [AS400DB01].GNOD0000.gnmb31 ON P00BNK = gnb31a AND P00BBR = gnb31c " +
                             " LEFT JOIN [AS400DB01].GNOD0000.gntb13 ON p00aty = gn13cd " +
                             " WHERE p00cis= " + csn;

                var result = MSSQL.GetDataset<DataSet>(sql, CommandType.Text).Result;

                ds = result.data.Tables.Count > 0 ? result.data : new DataSet();

            }
            catch (Exception ex)
            {
            }
            return ds;
        }

        public DataSet getILTB09l1()
        {
            DataSet ds = new DataSet();
            EB_Service.DAL.DataCenter MSSQL = new EB_Service.DAL.DataCenter(m_UserInfo);
            try
            {
                string sql = "SELECT * FROM [AS400DB01].ILOD0001.iltb09 WHERE T09CDE= '01'";
                var result = MSSQL.GetDataset<DataSet>(sql, CommandType.Text).Result;

                ds = result.data.Tables.Count > 0 ? result.data : new DataSet();
            }
            catch (Exception ex)
            {
            }
            return ds;

        }
        public DataSet get_RLTB10()
        {
            DataSet ds = new DataSet();
            //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            try
            {
                EB_Service.DAL.DataCenter MSSQL = new EB_Service.DAL.DataCenter(m_UserInfo);

                var resultTCL = MSSQL.GetDataset<DataSet>($@"SELECT T10CD2 FROM  [AS400DB01].[RLOD0001].[RLTB10]  WHERE T10RCD = '44' AND T10CD1 = '02'", CommandType.Text).Result;

                ds = resultTCL.data.Tables.Count > 0 ? resultTCL.data : new DataSet();
            }
            catch (Exception ex)
            {
                Utility.WriteLog(ex);
            }
            return ds;

        }

        public DataSet getGNMB20(string code)
        {
            DataSet ds = new DataSet();
            //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            try
            {
                EB_Service.DAL.DataCenter MSSQL = new EB_Service.DAL.DataCenter(m_UserInfo);

                string sql = "SELECT Code as gnb2tc,DescriptionTHAI as gnb2td, SUBSTRING(CAST(ShortName as nvarchar),1,1) as gnb2fl FROM GeneralDB01.GeneralInfo.GeneralCenter WITH (NOLOCK) WHERE Type = 'TitleID' AND TRIM(ShortName) = '" + code + "' ORDER BY Code";
                var result = MSSQL.GetDataset<DataSet>(sql, CommandType.Text).Result;
                ds = result.data.Tables.Count > 0 ? result.data : new DataSet();
            }
            catch (Exception ex)
            {
                Utility.WriteLog(ex);
            }
            return ds;
        }

        //***  get province Amphur Tambol ***//
        public DataSet getTambol(string code, string name = "")
        {
            DataSet ds = new DataSet();
            //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            try
            {
                EB_Service.DAL.DataCenter MSSQL = new EB_Service.DAL.DataCenter(m_UserInfo);
                string condition = " WHERE 1 = 1";
                string sql = " SELECT Code as gn18cd, DescriptionTHAI as gn18dt FROM GeneralDB01.GeneralInfo.AddrTambol ";

                if (code.Trim() != "")
                {
                    condition += " AND  Code = " + code;
                }
                if (name.Trim() != "")
                {
                    condition += " AND DescriptionTHAI = @tambol ";
                }

                sql += condition;
                ESBiDB2.iDB2Command cmd = new ESBiDB2.iDB2Command();
                cmd.CommandText = sql;
                cmd.Parameters.Add("@tambol", name.Trim());
                Utility.WriteLogResponse("Request to : " + url + "/" + cmd.CommandText.ToString());
                var dt = cmd.ExecuteReaderOptionalDataSet();
                Utility.WriteLogResponse("Response from : " + url + "/" + cmd.CommandText.ToString(), JsonConvert.SerializeObject(dt));
            }
            catch (Exception ex)
            {
                Utility.WriteLog(ex);
            }
            return ds;
        }




    }
    public class ILDataCenterMssqlInterview
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
        public EB_Service.DAL.DataCenter _dataCenter;
        //private As400DAL m_da400 = new As400DAL();
        public ILDataCenterMssqlInterview(UserInfo userInfo)
        {
            m_UserInfo = userInfo;
            _dataCenter = new EB_Service.DAL.DataCenter(m_UserInfo);
        }
        public string LastError;

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



        public async Task<DataSet> getResultCode()
        {
            DataSet ds = new DataSet();
            //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_UserInfo;
            try
            {
                string sql = " SELECT g25rcd, g25des FROM AS400DB01.GNOD0000.GNTB25 ORDER BY g25rcd ";

                var result = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result;
                if (result.success)
                {
                    ds = result.data;

                }
                return await Task.FromResult<DataSet>(ds);
            }
            catch (Exception ex)
            {
                return await Task.FromResult<DataSet>(ds);
            }
        }

        public bool SavePendingStatus(ILDataCenterMssqlInterview dataCenterMssqlInterview, string curdate, string appNo, string appDate, string reason, string Username, string LocalClient, string BranchApp)
        {
            try
            {

                string sqlUpd = " UPDATE AS400DB01.ILOD0001.ilms01 SET " +
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
                int resUdp = dataCenterMssqlInterview._dataCenter.Execute(sqlUpd, CommandType.Text, dataCenterMssqlInterview._dataCenter.Sqltr == null ? true : false).Result.afrows;
                if (resUdp == -1)
                {
                    dataCenterMssqlInterview._dataCenter.RollbackMssql();
                    dataCenterMssqlInterview._dataCenter.CloseConnectSQL();
                    return false;
                }
                dataCenterMssqlInterview._dataCenter.CommitMssql();
                dataCenterMssqlInterview._dataCenter.CloseConnectSQL();
                return true;


            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<DataSet> getActionCode(string code = "")
        {
            DataSet ds = new DataSet();
            //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_UserInfo;
            try
            {
                string condition = "";
                string sql = " SELECT G24ACD,G24DES FROM AS400DB01.GNOD0000.GNTB24 ";
                if (code.Trim() != "")
                {
                    condition = " WHERE G24ACD = '" + code.ToUpper() + "'";
                }
                sql += condition;
                var result = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result;
                if (result.success)
                {
                    ds = result.data;

                }
                return await Task.FromResult<DataSet>(ds);
            }
            catch (Exception ex)
            {
                return await Task.FromResult<DataSet>(ds);
            }
        }
        public async Task<DataSet> getCSMS00(string csn)
        {
            DataSet ds = new DataSet();
            //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_UserInfo;
            try
            {
                string sql = @"SELECT  cg.[ID]
                              ,[CustomerTypeID] as M00CST
                              ,FORMAT(BirthDate,'dd/MM/yyyy','th-TH') as M00BDT
                              ,[CISNumber] as M00CSN
							  ,cw.OfficeName as M00OFC
                              ,[TitleID] as M00TTL
                              ,[NameInENG] as M00ENM
                              ,[SurnameInENG] as M00ESN
                              ,[NickName] as M00NNM
                              ,[NameInTHAI] as M00TNM
                              ,[SurnameInTHAI] as M00TSN
                              ,[SexID] as M00SEX
                              ,[MaritalStatusID] as M00MST
                              ,[CardTypeID] as M00IDT
                              ,[IDCard] as m00idn
                              ,[IDCardIssued] as M00IDP
                              ,FORMAT(IDCardExpiredDate,'dd/MM/yyyy','th-TH') as M00EID
                              ,[EmailAddress] as M00EML
                              ,[ResidentalStatusID] as M00RST
                              ,[ResidentalYear] as M00RYR
                              , CASE WHEN [ResidentalMonth] IS NULL then '0' else [ResidentalMonth] end as M00RMO
                              , CASE WHEN [NoOfChildren] IS NULL then '0' else [NoOfChildren] end as M00TOC
                              ,[NoOfFamily] as M00TOF
                              ,cg.[RecordStatus]
                              ,cg.[Application] as M00UPG
                              ,cg.[CreateBy]
                              ,CONVERT(NVARCHAR,cg.[CreateDate],120) as CreateDate
                              ,cg.[UpdateBy]
                              ,CONVERT(NVARCHAR,cg.[UpdateDate],120) as UpdateDate
                              ,cg.[IsDelete]
                              ,CONVERT(NVARCHAR,cg.[SysStartTime]) as SysStartTime
                              ,CONVERT(NVARCHAR,cg.[SysEndTime]) as SysEndTime
                              ,cg.[ChangedBy]
                               ,cg.[ContactTime] as M00OTM
							  ,cw.TotalOfficer as M00TOO
							  ,cw.CompanyWorkedYear as M00WCY
							,cw.StartWorkedMonth as M00WMO
							,cw.StartWorkedYear as M00WYR
							,CASE WHEN cw.TotalWorkedMonth IS NULL then '0' else cw.TotalWorkedMonth end as M00WTM
							,cw.TotalWorkedYear as M00WTY
							,cw.DepartmentName as M00DPT
							,cw.OfficeTitleID
							,gc.Code as M00OFT
							,cw.CompanyBusinessID as M00BUS
							,cw.EmploymentTypeID as M00EPT
							,cw.OccupationID as M00OCC
							,cw.PositionID as M00POS
							,cs.SalaryAMT as M00SAL
							,cs.OtherIncome as M00INC
							,cs.OtherIncomeSource as M00OPL
							,cs.HouseRent as M00EA1
							,cs.HouseLoan as M00EA2
							,cs.CarLoan as M00EA3
							,cs.OtherLoan as M00EA4

                          FROM [CustomerDB01].[CustomerInfo].[CustomerGeneral] cg WITH (NOLOCK)
						  left join CustomerDB01.CustomerInfo.CustomerWorked cw WITH (NOLOCK) on cw.CustID = cg.ID
                          left join GeneralDB01.GeneralInfo.generalcenter gc WITH (NOLOCK) on gc.ID = cw.OfficeTitleID and gc.Type = 'OfficeTitleID' 
						  left join CustomerDB01.CustomerInfo.CustomerSalary cs WITH (NOLOCK) on cs.CustID = cg.ID
						  where cg.CISNumber =" + csn;

                var result = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result;
                if (result.success)
                {
                    ds = result.data;
                    //var data = JsonConvert.SerializeObject(result.data);
                    //ds.Tables.Add((DataTable)JsonConvert.DeserializeObject(data, typeof(DataTable)));
                }
                return await Task.FromResult<DataSet>(ds);



            }
            catch (Exception ex)
            {
                return await Task.FromResult<DataSet>(ds);
            }
        }
        public async Task<DataSet> getCSMS11(string csn)
        {
            DataSet ds = new DataSet();
            //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_UserInfo;
            try
            {
                string sql = @"SELECT ca.[ID]
	                      ,cg.CISNumber
                          ,[CustID]
                          ,[CustRefID] as M11REF
                          ,gc.Code as M11CDE
                          ,[AddressNumber] as M11ADR
                          ,[Village] as M11VIL
                          ,[BuildingTitleID] as M11BIL
                          ,[BuildingName]  as M11BLN
                          ,[Floor] as M11FLO
                          ,[Room] as M11ROM
                          ,[Moo] as M11MOO
                          ,[Road] as M11ROD
                          ,[Soi] as M11SOI
                          ,[AmphurID] as M11AMP
                          ,[TambolID] as M11TAM
                          ,[ProvinceID] as M11PRV
                          ,[PostalAreaCode] as M11ZIP
                          ,[IsShipTo] as M00DSN
                          ,ca.[RecordStatus]
                          ,ca.[Application]
                          ,ca.[CreateBy]
                          ,CONVERT(NVARCHAR,ca.[CreateDate],120) as CreateDate
                          ,ca.[UpdateBy]
                          ,CONVERT(NVARCHAR,ca.[UpdateDate],120) as UpdateDate
                          ,ca.[IsDelete]
                          ,ca.[SysStartTime]
                          ,ca.[SysEndTime]
                          ,ca.[ChangedBy]
                          ,[TelephoneNumber1] as m11tel
                          ,[ExtensionNumber1] as M11EXT
                          ,[TelephoneNumber2]
                          ,[ExtensionNumber2]
                          ,[TelephoneNumber3]
                          ,[ExtensionNumber3]
                          ,[Fax]
                          ,[Mobile] as M11MOB
                          ,[Seq]
                          ,[TelephoneTypeOtherID]
                      FROM [CustomerDB01].[CustomerInfo].[CustomerAddress]  ca WITH (NOLOCK)
                      join CustomerDB01.CustomerInfo.CustomerGeneral cg WITH (NOLOCK) on ca.CustID = cg.ID
                      join GeneralDB01.GeneralInfo.GeneralCenter gc WITH (NOLOCK) on gc.ID = ca.AddressCodeID and gc.Type='AddressCodeID'
                      
                      where cg.CISNumber = " + csn + "and ca.CustRefID=0";

                var result = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result;
                if (result.success)
                {
                    ds = result.data;

                }
                return await Task.FromResult<DataSet>(ds);



            }
            catch (Exception ex)
            {
                return await Task.FromResult<DataSet>(ds);
            }
        }
        public async Task<DataSet> CheckProvinceNA(string csn)
        {
            DataSet ds = new DataSet();
            //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_UserInfo;
            try
            {
                string sql = $@"SELECT ca.[ID]
	                      ,cg.CISNumber
                          ,cg.IDCard
                          ,[CustID]
                          ,[CustRefID] as M11REF
                          ,gc.Code as M11CDE
                          ,[AddressNumber] as M11ADR
                          ,[Village] as M11VIL
                          ,[BuildingTitleID] as M11BIL
                          ,[BuildingName]  as M11BLN
                          ,[Floor] as M11FLO
                          ,[Room] as M11ROM
                          ,[Moo] as M11MOO
                          ,[Road] as M11ROD
                          ,[Soi] as M11SOI
                          ,[AmphurID] as M11AMP
                          ,[TambolID] as M11TAM
                          ,[ProvinceID] as M11PRV
                          ,[PostalAreaCode] as M11ZIP
                          ,[IsShipTo] as M00DSN
                          ,[TelephoneNumber1] as m11tel
                          ,[ExtensionNumber1] as M11EXT
                      FROM [CustomerDB01].[CustomerInfo].[CustomerAddress]  ca WITH (NOLOCK)
                      join CustomerDB01.CustomerInfo.CustomerGeneral cg WITH (NOLOCK) on ca.CustID = cg.ID
                      join GeneralDB01.GeneralInfo.GeneralCenter gc WITH (NOLOCK) on gc.ID = ca.AddressCodeID and gc.Type='AddressCodeID' and gc.Code = 'H'
                      join GeneralDB01.GeneralInfo.AddrProvince adp WITH (NOLOCK) on adp.ID = ca.ProvinceID and adp.Code = 0                     
                      where cg.CISNumber = {csn} and ca.CustRefID = 0";

                var result = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result;
                if (result.success)
                {
                    ds = result.data;

                }
                return await Task.FromResult<DataSet>(ds);



            }
            catch (Exception ex)
            {
                return await Task.FromResult<DataSet>(ds);
            }
        }
        public async Task<DataSet> getCSMS13(string appNo, string brn, string csn)
        {
            DataSet ds = new DataSet();
            //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_UserInfo;
            try
            {
                string sql = $@"SELECT  [M13RRNO],[M13APP],[M13CSN],[M13BRN],[M13APN],[M13CNT],[M13EBC],[M13APT],[M13SAP],[M13CRT]
                                          ,[M13CRN],[M13APV],[M13CHA],[M13SCH],[M13ARS],[M13SEX],[M13MRT],[M13SMT],[M13BUT],[M13OCC]
                                          ,[M13POS],[M13OFF],[M13OBJ],[M13LNA],[M13PBL],[M13RES],[M13CON],[M13CNF],[M13TTL],[M13HTL]
                                          ,[M13HEX],[M13OTL],[M13OEX],[M13MTL],[M13WKY],[M13WKM],[M13EMP],[M13SHF],[M13RTL],[M13SLT]
                                          ,[M13SLD],[M13CRD],[M13MCD],[M13NET],[M13PAB],[M13MIS],[M13CMP],[M13CSQ],[M13TRM],[M13TCL]
                                          ,[M13TCA],[M13GOL],[M13RCL],[M13RCA],[M13ROC],[M13BDT],[M13CHL],[M13OZP],[M13OTM],[M13OAM]
                                          ,[M13OPV],[M13HZP],[M13HTM],[M13HAM],[M13HPV],[M13JSY],[M13JSM],[M13LYR],[M13LMT],[M13FDT]
                                          ,[M13MOB],[M13FIL],[M13OZZ],[M13ENT],[M131ID],[M13GNO],[M13ACL],[M13BOT],[M13CBL],[M13#RK]
                                          ,[M13#GN],[M13#AC],[M13CCD],[M13CRU],[M13CUD],[M13ACD],[M13AUS],[M13AUD],[M13SNO],[M13SAD]
                                          ,[M13SAT],[M13OSL],[M13OSD],[M13OST],[M13SST],[M13SAJ],[M13CAL],[M13DOC],[M13UPG],[M13UDT]
                                          ,[M13UTM],[M13USR],[M13WKS],[M13IZP],[M13ITM],[M13IAM],[M13IPV],[M13RKF],[M13SFD],[M13HHD],[M13CDD],[M13PRF],[M13PNM],[M13PSN],[M13PRC],[M13PPT]
                                      FROM [AS400DB01].[CSOD0001].[CSMS13] WITH (NOLOCK)  WHERE M13APN = '{appNo}' AND M13BRN = {brn} AND  M13APP = 'IL' AND M13CSN =  {csn}";

                var result = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result;
                if (result.success)
                {
                    ds = result.data;

                }
                return await Task.FromResult<DataSet>(ds);
            }
            catch (Exception ex)
            {
                throw new Exception("getCSMS13 " + ex);
            }
        }
        public async Task<DataSet> getRLtb71(string code_Rcd, string code_cd1, string code_cd2 = "", string t71syr = "")
        {
            DataSet ds = new DataSet();
            //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_UserInfo;
            try
            {
                string orderBy = "";
                string condition = "";
                string sql = " SELECT t71cd1,t71cd2, t71dst FROM AS400DB01.RLOD0001.rltb71 WITH (NOLOCK) WHERE t71del= '' ";
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
                var result = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result;
                if (result.success)
                {
                    ds = result.data;
                }
                return await Task.FromResult<DataSet>(ds);
            }
            catch (Exception ex)
            {
                return await Task.FromResult<DataSet>(ds);
            }
        }
        public async Task<DataSet> getCustVerifyCall(string branch, string appNo)
        {
            DataSet ds = new DataSet();
            //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_UserInfo;
            try
            {

                string sql = $@" SELECT   
                             W5CSNO,W5BRN, W5APNO,W5CSTY,W5SBCD,W5FVTO,W5FVTH,W5FVTM,W5FVTE,W5CCRD, 
                             W5CUPD,W5CUPT,W5CUSR,W5CWRK,W5CACD,W5CAUD,W5CAUT,W5CAUS,W5CAUW,W5VRTO,W5OFC , 
                             W5SALT,W5INET,W5SSO ,W5BOL ,W5HOTL,W5HOEX,W5WOTL,W5WOEX,W5MOTL,W5VOOT,W5VONM, 
                             W5TONM,W5VOAD,W5TOAD,W5EMST,W5RVTO,W5IPTO,W5TITO,W5TYEM,W5LETN,W5LETP,W5LETD, 
                             W5OCRD,W5OUPD,W5OUPT,W5OUSR,W5OWRK,W5OACD,W5OAUD,W5OAUT,W5OAUS,W5OAUW,W5VRTH, 
                             W5HTL ,W5HTEX,W5VHHT,W5VHNM,W5THNM,W5VHAD,W5THAD,W5TYTL,W5RVTH,W5TVTH,W5IPTH,
                             W5TITH,W5HCRD,W5HUPD,W5HUPT,W5HUSR,W5HWRK,W5HACD,W5HAUD,W5HAUT,W5HAUS,W5HAUW,
                             W5VRTM,W5MBTL,W5VMTL,W5MCRD,W5MUPD,W5MUPT,W5MUSR,W5MWRK,W5MACD,W5MAUD,W5MAUT,
                             W5MAUS,W5MAUW,W5VRTE,W5ECRD,W5EUPD,W5EUPT,W5EUSR,W5EWRK,W5EACD,W5EAUD,W5EAUT,
                             W5EAUS,W5EAUW,W5CRTD,W5CRTT,W5FIL ,W5UPDT,W5UPTM,W5USER,W5PRGM,W5WRKS, w5cusr as I_Type,
                             case when w5fvto = 'Y' then w5ousr else '' end I_TO,
                             case when w5fvto = 'Y' then w5oaus else '' end K_TO,
                             case when w5fvth = 'Y' then w5husr else '' end I_TH,
                             case when w5fvth = 'Y' then w5haus else '' end K_TH,
                             case when w5fvtm = 'Y' then w5musr else '' end I_TM,
                             case when w5fvtm = 'Y' then w5maus else '' end K_TM,
                             case when w5fvte = 'Y' then w5eusr else '' end I_TE,
                             case when w5fvte = 'Y' then w5eaus else '' end K_TE,
                             W5KTDL, W5KCRD,W5KUPD, W5KUPT, W5KUSR,W5KWRK,W5KACD, W5KAUD,W5KAUT, W5KAUS,W5KAUW
                             FROM AS400DB01.ILOD0001.ilwk05 WITH (NOLOCK)
                             WHERE w5brn =  {branch}
                             AND w5apno  =  {appNo} ";

                var result = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result;
                if (result.success)
                {
                    ds = result.data;
                }
                return await Task.FromResult<DataSet>(ds);
            }
            catch (Exception ex)
            {
                return await Task.FromResult<DataSet>(ds);
            }
        }
        public async Task<DataSet> get_csms30(string csn)
        {
            DataSet ds = new DataSet();
            //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_UserInfo;
            try
            {

                //string sql = $@" SELECT M30CSN FROM  [AS400DB01].[CSOD0001].[CSMS30] WITH (NOLOCK)  WHERE M30CSN = {csn}";
                string sql = $@"SELECT P2CONT FROM [AS400DB01].PLOD0001.PLMS02 WITH (NOLOCK)
                            JOIN CustomerDB01.CustomerInfo.CustomerGeneral WITH (NOLOCK) ON (P2IDNO = IDCard)
                            WHERE CISNumber = {csn}";
                var result = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result;
                if (result.success)
                {
                    ds = result.data;
                    return await Task.FromResult<DataSet>(ds);
                }
                sql = $@"SELECT P2CONT FROM [AS400DB01].PMOD0001.PMMS02 WITH (NOLOCK)
                            JOIN CustomerDB01.CustomerInfo.CustomerGeneral WITH (NOLOCK) ON (P2IDNO = IDCard)
                            WHERE CISNumber = {csn} ";
                result = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result;
                if (result.success)
                {
                    ds = result.data;
                    return await Task.FromResult<DataSet>(ds);
                }
                sql = $@"SELECT P2CONT FROM [AS400DB01].PWOD0001.PWMS02 WITH (NOLOCK)
                            JOIN CustomerDB01.CustomerInfo.CustomerGeneral WITH (NOLOCK) ON (P2IDNO = IDCard)
                            WHERE CISNumber = {csn} ";
                if (result.success)
                {
                    ds = result.data;
                    return await Task.FromResult<DataSet>(ds);
                }
                sql = $@"SELECT P2CONT FROM [AS400DB01].RLOD0001.RLMS02 WITH (NOLOCK) WHERE P2CSNO = {csn} ";
                if (result.success)
                {
                    ds = result.data;
                    return await Task.FromResult<DataSet>(ds);
                }
                sql = $@"SELECT P2CONT FROM [AS400DB01].PHOD0001.PHMS02 WITH (NOLOCK)
                            JOIN CustomerDB01.CustomerInfo.CustomerGeneral WITH (NOLOCK) ON (P2IDNO = IDCard)
                            WHERE CISNumber = {csn} ";
                if (result.success)
                {
                    ds = result.data;
                    return await Task.FromResult<DataSet>(ds);
                }
                sql = $@"SELECT P2CONT FROM [AS400DB01].ILOD0001.ILMS02 WHERE P2CSNO = {csn} ";
                if (result.success)
                {
                    ds = result.data;
                    return await Task.FromResult<DataSet>(ds);
                }
                sql = $@"SELECT P2CONT FROM [AS400DB01].HMOD0001.HMMS02 WITH (NOLOCK)
                            JOIN CustomerDB01.CustomerInfo.CustomerGeneral WITH (NOLOCK) ON (P2IDNO = IDCard)
                            WHERE CISNumber = {csn} ";
                if (result.success)
                {
                    ds = result.data;
                    return await Task.FromResult<DataSet>(ds);
                }
                return await Task.FromResult<DataSet>(ds);
            }
            catch (Exception ex)
            {
                return await Task.FromResult<DataSet>(ds);
            }

        }
        public async Task<DataSet> getILWK12(string appNo, string branch, string csn)
        {
            DataSet ds = new DataSet();
            //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_UserInfo;
            try
            {
                string sql = $@" SELECT   W12RSQ  Seq, W12REL Relation,W12NME Name,W12SNM SurName, 
                             SUBSTRING(W12HTL,1,9) Tel_1,SUBSTRING(W12HTL,11,4)  To_1, W12HEX Ext_1, 
                             SUBSTRING(W12OTL,1,9) Tel_2,SUBSTRING(W12OTL,11,4)  To_2, W12OEX Ext_2, 
                             W12MOB Mobile,W12VTE Ver,GN14TD Rel_DES 
                             FROM AS400DB01.ILOD0001.ILWK12 join AS400DB01.GNOD0000.gntb14  on  W12REL = GN14CD  where W12APN = {appNo} AND W12BRN = {branch}
                             ORDER BY W12RSQ ";

                var result = await _dataCenter.GetDataset<DataTable>(sql, CommandType.Text);
                if (result.success && result.data != null && result.data?.Tables?[0] != null && result.data?.Tables?[0].Rows.Count != 0)
                {
                    ds = result.data;
                }
                else
                {
                    sql = $@" SELECT Seq,Relation,Name,SurName,Tel_1,To_1,Ext_1,Tel_2,To_2,Ext_2,Mobile,Ver,GN14TD Rel_DES  
                           FROM (SELECT m01seq Seq, m01rel Relation, m01tnm Name, m01tsn SurName,SUBSTRING(m11tel,1,9) Tel_1,SUBSTRING(m11tel,11,4)  To_1, m11ext Ext_1, 
                           SUBSTRING(m11tl2,1,9) Tel_2,SUBSTRING(m11tl2,11,4)  To_2, m11ex2 Ext_2, m11mob Mobile,'N' Ver 
                           FROM AS400DB01.CSOD0001.csms01 JOIN  AS400DB01.CSOD0001.csms11  ON  (m01csn = m11csn and m01ref = m11ref and m01seq = m11rsq) WHERE m01csn = { csn } AND  m01ref = '1'  )  a 
                           left  join AS400DB01.GNOD0000.gntb14  on  a.Relation = GN14CD   ";

                    result = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result;
                    if (result.success)
                    {
                        ds = result.data;
                    }
                }
                return await Task.FromResult<DataSet>(ds);
            }
            catch (Exception ex)
            {
                return await Task.FromResult<DataSet>(ds);
            }
        }
        public async Task<DataSet> getGNTB14()
        {
            DataSet ds = new DataSet();
            //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_UserInfo;
            try
            {
                string sql = " select gn14cd,gn14td from AS400DB01.GNOD0000.gntb14 order by gn14cd ";

                var result = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result;
                if (result.success)
                    ds = result.data;
                return await Task.FromResult<DataSet>(ds);
            }
            catch (Exception ex)
            {
                return await Task.FromResult<DataSet>(ds);
            }
        }
        public async Task<DataSet> getGNTB67(bool tel, bool ext)
        {
            DataSet ds = new DataSet();
            //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_UserInfo;
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

                string sql = "SELECT * FROM AS400DB01.GNOD0000.gntb67 WITH (NOLOCK) WHERE GN67CD = '" + code + "'";
                var result = await _dataCenter.GetDataset<DataTable>(sql, CommandType.Text);
                if (result.success)
                    ds = result.data;
                return await Task.FromResult<DataSet>(ds);
            }
            catch (Exception ex)
            {
                return await Task.FromResult<DataSet>(ds);
            }
        }
        public async Task<DataSet> getRLtb70(string code, string sub_code)
        {
            DataSet ds = new DataSet();
            //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_UserInfo;
            try
            {

                string sql = @" SELECT T70HST,T70SBC,T70VTO,T70VTH,T70VTM
                                ,T70VTE,T70FIL,T70UDT,T70UTM,T70USR,T70DSP,T70DEL  
                                FROM  AS400DB01.RLOD0001.RLTB70 WITH (NOLOCK) WHERE T70HST = '" + code + "' " +
                             " AND T70SBC = '" + sub_code + "' " +
                             " AND T70DEL <> 'X' ";
                var result = await _dataCenter.GetDataset<DataTable>(sql, CommandType.Text);
                if (result.success)
                    ds = result.data;
                return await Task.FromResult<DataSet>(ds);
            }
            catch (Exception ex)
            {
                return await Task.FromResult<DataSet>(ds);
            }
        }

        public async Task<DataSet> get_ilms01(string P1BRN, string P1APNO)
        {
            DataSet ds = new DataSet();
            //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_UserInfo;
            try
            {

                string sql = " SELECT *  FROM  AS400DB01.ILOD0001.ILMS01 WITH (NOLOCK)  WHERE P1BRN = " + P1BRN +
                             " AND P1APNO = " + P1APNO;

                var result = await _dataCenter.GetDataset<DataTable>(sql, CommandType.Text);
                if (result.success)
                    ds = result.data;
                return await Task.FromResult<DataSet>(ds);
            }
            catch (Exception ex)
            {
                return await Task.FromResult<DataSet>(ds);
            }
        }
        public async Task<DataSet> getCSMS07(ILDataCenterMssqlInterview ilObj, string appNo, string branch_app, string csn)
        {
            DataSet ds = new DataSet();
            //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_UserInfo; try
            {

                ////UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
                //UserInfomation = m_UserInfo;
                string sql = " SELECT c07csn FROM  AS400DB01.CSOD0001.csms07   WITH (NOLOCK) " +
                             " WHERE  c07csn =  " + csn +
                             " AND c07app = 'IL' and c07apn = " + appNo +
                             " and c07brn = " + branch_app;

                var result = await _dataCenter.GetDataset<DataTable>(sql, CommandType.Text);
                if (result.success)
                    ds = result.data;
                return await Task.FromResult<DataSet>(ds);
            }
            catch (Exception ex)
            {
                return await Task.FromResult<DataSet>(ds);
            }
        }

        public async Task<DataSet> get_RLTB10()
        {
            DataSet ds = new DataSet();
            //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_UserInfo;
            try
            {

                string sql = " SELECT T10CD2 FROM  AS400DB01.RLOD0001.RLTB10   WITH (NOLOCK) WHERE T10RCD = '44' AND T10CD1 = '02'";

                var result = await _dataCenter.GetDataset<DataTable>(sql, CommandType.Text);
                if (result.success)
                    ds = result.data;
                return await Task.FromResult<DataSet>(ds);
            }
            catch (Exception ex)
            {
                return await Task.FromResult<DataSet>(ds);
            }

        }
        public async Task<DataSet> getGNTS18(string gs18sc = "")
        {
            DataSet ds = new DataSet();
            //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_UserInfo;
            try
            {
                string condition = "";
                string sql = "";
                if (gs18sc.Trim() == "")
                {
                    sql = " SELECT SubSalaryType AS gs18sc, DescriptionTHAI AS gs18dt FROM GeneralDB01.GeneralInfo.SalaryWorkType WITH (NOLOCK) WHERE IsDelete = '' ORDER BY SubSalaryType ";
                }
                else
                {
                    sql = " SELECT SubSalaryType AS gs18sc, DescriptionTHAI AS gs18dt, AutoAdjustAMT AS gs18aj FROM GeneralDB01.GeneralInfo.SalaryWorkType WITH (NOLOCK)  WHERE SubSalaryType = " + gs18sc + " AND IsDelete = '' ORDER BY SubSalaryType ";
                }
                var result = await _dataCenter.GetDataset<DataTable>(sql, CommandType.Text);
                if (result.success)
                    ds = result.data;
                return await Task.FromResult<DataSet>(ds);
            }
            catch (Exception ex)
            {
                return await Task.FromResult<DataSet>(ds);
            }
        }
        public async Task<DataSet> get_CSTB05()
        {
            DataSet ds = new DataSet();
            //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_UserInfo;
            try
            {
                string mode = "";
                //try
                //{
                //    mode = WebConfigurationManager.AppSettings["Mode"].ToString().Trim();
                //}
                //catch
                //{
                //    mode = "O";
                //}
                mode = "O";
                string sql = " SELECT t05inc FROM AS400DB01.CSOD0001.CSTB05  WITH (NOLOCK) WHERE t05brn = " + m_UserInfo.BranchApp +
                             " AND t05app = 'IL' " +
                             " AND t05ven = 0 " +
                             " AND t05opr = '" + mode + "'";
                var result = await _dataCenter.GetDataset<DataSet>(sql, CommandType.Text);
                if (result.success)
                    ds = result.data;
                _dataCenter.CloseConnectSQL();
                return await Task.FromResult<DataSet>(ds);
            }
            catch (Exception ex)
            {
                _dataCenter.CloseConnectSQL();
                return await Task.FromResult<DataSet>(ds);
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
            //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];

            ILDataSubroutine iLDataSubroutine = new ILDataSubroutine(m_UserInfo);
            busobj.UserInfomation = m_UserInfo;
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
                if (m13net.Contains("."))
                {
                    string[] Salary = m13net.Split('.');
                    m13net = Salary[0];
                }
                if (m13ttl.Contains("0"))
                {
                    string[] HomeType = m13ttl.Split('0');
                    m13ttl = HomeType[1];
                }
                //string Array100 = "66036658F046131010276014405105030000230002300002000040100125660816000110110  0000000000110110OFF";
                string Array100 = csn.PadRight(8, '0') +
                                             m13sex.PadLeft(1, '0') +
                                             Convert.ToInt32(age).ToString().PadLeft(3, '0') +
                                             m13mrt.PadLeft(1, '0') +
                                             m13ttl +
                                             appMobile +
                                             m13res.PadLeft(2, '0') +
                                             resident_y.PadLeft(4, '0') +
                                             workyear.PadLeft(4, '0') +
                                             m13occ.PadLeft(3, '0') +
                                             m13but.PadLeft(2, '0') +
                                             m13slt.PadLeft(2, '0') +
                                             m13off.PadLeft(6, '0') +
                                             m13net.PadLeft(8, '0') +//
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
                                             m13saj.PadLeft(8, '0') +//
                                             M13CHA.PadLeft(3, '0') +
                                             m13ozp.PadLeft(5, '0') +
                                             "OFF";

                DS = _dataCenter.GetDataset<DataSet>("select m3csno, m3crlm, m3acam from AS400DB01.CSOD0001.CSMS03  WITH (NOLOCK) where m3csno = " + csn + " and m3del='' ", CommandType.Text).Result.data;
                _dataCenter.CloseConnectSQL();
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

                    //Mock ไม่ได้
                    iLDataSubroutine.CALL_GNP0371(birthdate, "", "YMD", "B", "", "IL", "",
                                        m_UserInfo.BizInit.ToString(), m_UserInfo.BranchNo.ToString(),
                                        ref in_AGE, ref error);


                    iLDataSubroutine.CALL_GNSRGNOC("IL", brn, appno, Array100.ToString(),
                                          ref outCSNO, ref outOPD, ref outOGNO, ref outORANK, ref outOINC, ref outOACL,  //4,5,6,7,8,9
                                          ref outO2GNO, ref outO2RANK, ref outO2ACL, ref outO2RNK, ref outO21ACL, ref outOTYPE, //10,11,12,13,14,15
                                          m_UserInfo.BizInit.ToString(), m_UserInfo.BranchNo.ToString());

                    outCSNO = csn;
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
                    }
                }

                EdtBOTLoan = (Convert.ToDecimal(m13net) * 5).ToString();

                string outSTS = "", outMSG = "", outFLG = "";
                iLDataSubroutine.CALL_CSSR36(csn, ref outSTS, ref outMSG, ref outFLG, m_UserInfo.BizInit.ToString(), m_UserInfo.BranchNo.ToString());
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

                            iLDataSubroutine.CALL_GNSRGNOC("IL", brn, appno.ToString(), Array100.ToString(),
                                                  ref outCSNO, ref outOPD, ref outOGNO, ref outORANK, ref outOINC, ref outOACL,  //4,5,6,7,8,9
                                                  ref outO2GNO, ref outO2RANK, ref outO2ACL, ref outO2RNK, ref outO21ACL, ref outOTYPE, //10,11,12,13,14,15
                                                  m_UserInfo.BizInit.ToString(), m_UserInfo.BranchNo.ToString());
                            outCSNO = csn;
                        }
                        DS.Clear();
                    }
                    else
                    {
                        err = "Pleaes verify judgment before save product";
                        _dataCenter.CloseConnectSQL();
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
                        G_PD = outOPD.Trim().Replace(".", "").PadLeft(12, '0');
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

                            if (G_Rank_for_GNSR031.ToString() == "")
                            {
                                G_Rank_for_GNSR031 = "0";
                            }
                            iLDataSubroutine.CALL_CSSR31(csn, m13net, "I", G_Rank_for_GNSR031.Trim(),
                                               m13occ, m_UserInfo.BranchNo.ToString().Trim(), m13sst, m13saj,
                                              ref outPOSALN, ref outPOORNK, ref outPOOTMS, ref outPOOACL, ref outPOARNK, ref outPOATMS, ref outPOAACL, //8,9,10,11,12,13,14
                                              ref outPORRNK, ref outPORTMS, ref outPORACL, ref outPOFTCL, ref outPOSTRP, ref outPOTOTP, ref outPOAFLG, //15,16,17,18,19,20,21
                                              ref outPOHAVE, ref outPOODGC, ref outPOMDL, ref outPOPD, ref outPOGNO, //22,23,24,25,26
                                              m_UserInfo.BizInit.ToString(), m_UserInfo.BranchNo.ToString());
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
                //try
                //{
                //    mode = WebConfigurationManager.AppSettings["Mode"].ToString().Trim();
                //}
                //catch
                //{
                //    mode = "O";
                //}
                mode = "O";

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
                    DS = _dataCenter.GetDataset<DataSet>("select C07CNT from AS400DB01.CSOD0001.CSMS07  WITH (NOLOCK) " +
                                                 "where c07csn=" + csn + " and " +
                                                 "c07app='IL' and c07apn=" + appno + " and c07brn=" + brn + " and " +
                                                 "c07acl=" + G_ACL + " and c07ftc=" + G_Final_TCL + " ", CommandType.Text).Result.data;
                    _dataCenter.CloseConnectSQL();
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
                    iLDataSubroutine.CALL_GNSR86("IL", "01", m_UserInfo.BranchApp.ToString(), appDate,
                                       Date_97, E_rank.Trim(), EdtACL.Trim(),
                                       ref outPPOTCL, ref outPOERR, ref outPERMSG, m_UserInfo.BizInit.ToString(), m_UserInfo.BranchNo.ToString());

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
                iLDataSubroutine.Call_GNSR87("IL", csn, m_UserInfo.BranchApp.ToString(), appno.ToString().Trim(), appDate,
                                   birthdate, "1", "Y", m13net, GNSR87_9.ToString(), loanReq,
                                   vendor, "O", GNSR87_13.ToString(),
                                   ref POBOTL, ref PONOAP, ref POCSBL, ref POCRAV, ref POAPAM, ref POEBCL, ref POAVAP, ref POSTS, ref POOLDC, //14,15,16,17,18,19,20,21,22
                                   ref POVENR, ref POPERV, ref POEBCS, ref POERR, ref PERMSG, //23,24,25,26,27
                                   m_UserInfo.BizInit.ToString(), m_UserInfo.BranchNo.ToString());

                EdtBOTLoan = POBOTL.ToString();
                EdtNetIncome = m13net;
                EdtCrBal = POCSBL.ToString();
                EAAA = POCRAV.ToString();

                //ปัดเศษ เช่น คำนวณ sal*1.5 = 29999*1.5 = 49998.5 ให้ปัดเป็น 44900
                if (Convert.ToDecimal(string.IsNullOrEmpty(EdtTCL) ? "0" : EdtTCL) > Convert.ToDecimal(string.IsNullOrEmpty(EdtBOTLoan) ? "0" : EdtBOTLoan))
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

                iLDataSubroutine.Call_GNSR093(idno.ToString(), m13net, ref Error_GNSR093, ref payment_ability, m_UserInfo.BizInit, m_UserInfo.BranchNo);
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
                _dataCenter.CloseConnectSQL();
                ds_result.Tables.Add(dt);
                return false;
                //throw new Exception(ex.Message.ToString());

            }
        }
        public bool SaveStepCustomer(EB_Service.DAL.DataCenter dataCenter, string curdate, string appNo, string appDate, string step, string programName, string Username, string LocalClient, string BranchApp, string p1aprj)
        {
            try
            {

                string sqlUpd = "";

                if (step == "2")
                {
                    sqlUpd = " UPDATE AS400DB01.ILOD0001.ILMS01 SET " +
                                " p1fill = CONCAT(SUBSTRING(p1fill,1,24),'" + step + "',SUBSTRING(p1fill,26,2),'  ',SUBSTRING(p1fill,30,9))," +
                                " p1aprj = " + p1aprj + ", " +
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
                    sqlUpd = " UPDATE AS400DB01.ILOD0001.ILMS01 SET " +
                                " p1fill = CONCAT(SUBSTRING(p1fill,1,24),'" + step + "',SUBSTRING(p1fill,26,2),'PI',SUBSTRING(p1fill,30,9))," +
                                " p1aprj = " + p1aprj + ", " +
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
                    sqlUpd = " UPDATE AS400DB01.ILOD0001.ILMS01 SET " +
                             " p1fill = CONCAT(SUBSTRING(p1fill,1,24),'" + step + "',SUBSTRING(p1fill,26,13))," +
                             " p1aprj = " + p1aprj + ", " +
                             " p1resn = p1resn," +
                             " p1updt = " + curdate + "," +
                             " p1uptm = " + m_UdpT + "," +
                             " p1upus = '" + Username + "'," +
                             " p1prog = '" + programName + "', " +
                             " p1wsid = '" + LocalClient + "'" +
                             " WHERE p1apno = " + appNo +
                             " AND p1brn = " + BranchApp;
                }
                int resUdp = dataCenter.Execute(sqlUpd, CommandType.Text).Result.afrows;
                if (resUdp == -1)
                {
                    dataCenter.RollbackMssql();
                    dataCenter.CloseConnectSQL();
                    return false;
                }
                return true;


            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool SaveRejectStatus(EB_Service.DAL.DataCenter dataCenter, string curdate, string appNo, string appDate, string reason, string p1aprj, string p1loca, string step, string programName, string Username, string LocalClient, string BranchApp, string csn, string idNO)
        {
            try
            {
                string sqlUpd = " UPDATE AS400DB01.ILOD0001.ilms01 SET " +
                                " p1fill = CONCAT(SUBSTRING(p1fill,1,24),'" + step + "',SUBSTRING(p1fill,26,13))," +
                                " p1stdt = " + curdate + ", " + //  เพิ่มเติมวันที่ Update เมื่อ Auto Reject 
                                " P1STTM = " + m_UdpT + " ," +
                                " p1aprj = '" + p1aprj + "', " +
                                " p1loca = '" + p1loca + "', " +
                                " p1crcd = '" + Username + "'," +
                                " p1avdt = " + curdate + "," +
                                " p1avtm = " + m_UdpT + "," +
                                " p1resn = '" + reason + "'," +
                                " p1updt = " + curdate + "," +
                                " p1uptm = " + m_UdpT + "," +
                                " p1upus = '" + Username + "'," +
                                " p1wsid = '" + LocalClient + "'," +
                                " p1prog = '" + programName + "' " +
                                " WHERE p1apno = " + appNo +
                                " AND p1brn = " + BranchApp;
                int resUdp = dataCenter.Execute(sqlUpd, CommandType.Text).Result.afrows;
                if (resUdp == -1)
                {
                    dataCenter.RollbackMssql();
                    dataCenter.CloseConnectSQL();
                    return false;
                }

                string NoteDesc_ILMS38 = "";
                DataSet ds_note = _dataCenter.GetDataset<DataSet>("select g25des from AS400DB01.GNOD0000.GNTB25  WITH (NOLOCK) where g25rcd = '" + reason + "' ", CommandType.Text).Result.data;
                if (ds_note != null)
                {
                    foreach (DataRow dr in ds_note.Tables[0].Rows)
                    {
                        NoteDesc_ILMS38 = dr["g25des"].ToString().Trim();
                    }
                    ds_note.Clear();
                }

                // เพิ่ม ให้ update document completed เมื่อ Auto reject ตาม Req No.  10/02/2558
                string upDateIlmd01 = "update AS400DB01.ILOD0001.ilmd01 set d1dccm = 'Y' " +
                                  "where d1idno = '" + idNO + "' and d1srno = '" + appNo.PadLeft(11, '0') + "' ";
                int res_ilmd01 = _dataCenter.Execute(upDateIlmd01, CommandType.Text).Result.afrows;
                if (res_ilmd01 == -1)
                {
                    dataCenter.RollbackMssql();
                    dataCenter.CloseConnectSQL();
                    return false;
                }
                Connect_NoteAPI noteAPI = new Connect_NoteAPI();
                var resNote = noteAPI.AddNote(csn, "0", "ADD", reason, NoteDesc_ILMS38.ToString().Trim(), curdate.ToString().Trim(), m_UpdTime.ToString().Trim()).Result;

                int affectedRows = Convert.ToInt32(resNote.success);
                if (affectedRows == -1)
                {
                    dataCenter.RollbackMssql();
                    dataCenter.CloseConnectSQL();
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
                string sql = " SELECT * FROM CustomerDB01.CustomerInfo.CustomerAddress WITH (NOLOCK)" +
                             $" WHERE CustID = (SELECT TOP(1) ID FROM [CustomerDB01].[CustomerInfo].CustomerGeneral WITH (NOLOCK) WHERE CISNumber = '{csn}') " +
                             $" AND AddressCodeID = (SELECT TOP(1) ID FROM GeneralDB01.GeneralInfo.GeneralCenter WITH (NOLOCK) WHERE Type = 'AddressCodeID' AND Code = '{addrCode}') ";

                var res = _dataCenter.GetDataset<DataSet>(sql, CommandType.Text).Result;
                if (res.success)
                    ds_11 = res.data;

                if (ilObj.check_dataset(ds_11))
                {
                    sqlRet = " UPDATE CustomerDB01.CustomerInfo.CustomerAddress SET TelephoneNumber1 = '" + tel + "'," +
                             " ExtensionNumber1 = '" + tel_ext + "' ," +
                             $" UpdateDate ='{(int.Parse(curdate.Trim().Substring(0, 4)) - 543).ToString()}-{curdate.Trim().Substring(4, 2)}-{curdate.Trim().Substring(6, 2)} {m_UpdTime.ToString().PadLeft(6, '0').Substring(0, 2)}:{m_UpdTime.ToString().PadLeft(6, '0').Substring(2, 2)}:{m_UpdTime.ToString().PadLeft(6, '0').Substring(4, 2)}' " +
                             ", UpdateBy = '" + userName + "'," +
                             " Application = '" + programName + "'";
                    if (addrCode == "H")
                    {
                        if (mobile.Trim() != "")
                        {
                            sqlRet += " , Mobile = '" + mobile + "' ";
                        }
                    }
                    sqlRet += $" WHERE CustID = (SELECT TOP(1) ID FROM [CustomerDB01].[CustomerInfo].CustomerGeneral WITH (NOLOCK) WHERE CISNumber = '{csn}') " +
                              $" AND AddressCodeID = (SELECT TOP(1) ID FROM GeneralDB01.GeneralInfo.GeneralCenter WITH (NOLOCK) WHERE Type = 'AddressCodeID' AND Code = '{addrCode}') ";

                }
                //else if (ds_11.Tables[0].Rows.Count == 0)
                else
                {
                    sqlRet = " INSERT INTO  CustomerDB01.CustomerInfo.CustomerAddress ( CustID,AddressCodeID,TelephoneNumber1,ExtensionNumber1,Mobile,UpdateDate,UpdateBy,Application,BuildingTitleID,AmphurID,TambolID,ProvinceID  ) " +
                             " VALUES ( " +
                             $" (SELECT TOP(1) ID FROM [CustomerDB01].[CustomerInfo].CustomerGeneral WITH (NOLOCK) WHERE CISNumber = '{csn}'), " +
                             $" (SELECT TOP(1) ID FROM GeneralDB01.GeneralInfo.GeneralCenter WITH (NOLOCK) WHERE Type = 'AddressCodeID' AND Code = '{addrCode}'), " +
                             "'" + tel + "'," +
                             "'" + tel_ext + "'," +
                             "'" + mobile + "'," +
                             $"'{(int.Parse(curdate.Trim().Substring(0, 4)) - 543).ToString()}-{curdate.Trim().Substring(4, 2)}-{curdate.Trim().Substring(6, 2)} {m_UpdTime.ToString().PadLeft(6, '0').Substring(0, 2)}:{m_UpdTime.ToString().PadLeft(6, '0').Substring(2, 2)}:{m_UpdTime.ToString().PadLeft(6, '0').Substring(4, 2)}'," +
                             "'" + userName + "'," +
                             "'" + programName + "'," +
                             " (SELECT TOP(1)ID FROM GeneralDB01.GeneralInfo.GeneralCenter WITH (NOLOCK)  WHERE Type = 'BuildingTitleID' AND Code = 011), " +
                            " (SELECT TOP(1)ID FROM GeneralDB01.GeneralInfo.AddrAumphur WITH (NOLOCK)  WHERE Code = 0), " +
                            " (SELECT TOP(1)ID FROM GeneralDB01.GeneralInfo.AddrTambol WITH (NOLOCK)  WHERE Code = 0), " +
                            " (SELECT TOP(1)ID FROM GeneralDB01.GeneralInfo.AddrProvince WITH (NOLOCK)  WHERE Code = 0) " +
                             " )";
                }
            }
            catch (Exception ex)
            {

            }
            return sqlRet;
        }
        public string UPDATE_CustomerWorked(string csn, string M00UDT, string M00UTM, string M00UUS, string M00UPG, string M00UWS, string M00OFT, string M00OFC)
        {
            string sql = "";
            try
            {
                sql = " UPDATE CustomerDB01.CustomerInfo.CustomerWorked SET " +
                      " OfficeTitleID = (SELECT TOP(1) ID FROM GeneralDB01.GeneralInfo.GeneralCenter WITH (NOLOCK)  WHERE Type = 'OfficeTitleID' AND CODE ='" + M00OFT + "'), " +
                      " OfficeName = '" + M00OFC + "', " +
                      $" UpdateDate ='{(int.Parse(M00UDT.Trim().Substring(0, 4)) - 543).ToString()}-{M00UDT.Trim().Substring(4, 2)}-{M00UDT.Trim().Substring(6, 2)} {M00UTM.ToString().PadLeft(6, '0').Substring(0, 2)}:{M00UTM.ToString().PadLeft(6, '0').Substring(2, 2)}:{M00UTM.ToString().PadLeft(6, '0').Substring(4, 2)}', " +
                      " UpdateBy = '" + M00UUS + "'," +
                      " Application = '" + M00UPG + "'" +
                      " WHERE CustID = (SELECT TOP(1) ID FROM CustomerDB01.CustomerInfo.CustomerGeneral WITH (NOLOCK)  WHERE CISNumber = " + csn + ")";
            }
            catch (Exception ex)
            {
            }
            return sql;
        }
        public string INSERT_CustomerWorked(string csn, string M00UDT, string M00UTM, string M00UUS, string M00UPG, string M00UWS, string M00OFT, string M00OFC)
        {
            string sql = "";
            try
            {
                sql = " INSERT INTO CustomerDB01.CustomerInfo.CustomerWorked " +
                      " ([CustID] ,[OfficeTitleID], [OfficeName], [CreateDate] ,[CreateBy] ,[Application] ) VALUES (" +
                      " (SELECT TOP(1) ID FROM CustomerDB01.CustomerInfo.CustomerGeneral WITH (NOLOCK)  WHERE CISNumber = " + csn + ")," +
                      " (SELECT TOP(1) ID FROM GeneralDB01.GeneralInfo.GeneralCenter WITH (NOLOCK)  WHERE Type = 'OfficeTitleID' AND CODE ='" + M00OFT + "'), " +
                      " '" + M00OFC + "', " +
                      $" '{(int.Parse(M00UDT.Trim().Substring(0, 4)) - 543).ToString()}-{M00UDT.Trim().Substring(4, 2)}-{M00UDT.Trim().Substring(6, 2)} {M00UTM.ToString().PadLeft(6, '0').Substring(0, 2)}:{M00UTM.ToString().PadLeft(6, '0').Substring(2, 2)}:{M00UTM.ToString().PadLeft(6, '0').Substring(4, 2)}', " +
                      " '" + M00UUS + "'," +
                      " '" + M00UPG + "')";
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
                sql = $@" UPDATE  AS400DB01.CSOD0001.CSMS13 SET M13CRU = '' 
                        WHERE m13brn = {m13brn}
                        AND   m13apn = {m13apn} ";
            }
            catch (Exception ex)
            {

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
                sql = $@" INSERT INTO AS400DB01.ILOD0001.ilwk05 (w5csno, w5brn, w5apno, w5csty, w5sbcd, 
                       w5cupd, w5cupt, w5cusr, w5cwrk, w5inet, w5crtd, w5crtt, 
                       w5updt, w5uptm, w5user, w5prgm, w5wrks,  w5ktdl, w5kcrd,w5kupd, w5kupt, w5kusr,w5kwrk,w5kacd, w5kaud,w5kaut, w5kaus,w5kauw ) values (
                        { w5csno} ,
                        { w5brn} ,
                        {w5apno} ,
                        '{w5csty}',
                        '{w5sbcd}',
                        {w5cupd},
                        {w5cupt},
                        '{w5cusr}',
                        '{w5cwrk}',
                        {w5inet},
                        {w5crtd},
                        {w5crtt},
                        {w5updt},
                        {w5uptm},
                        '{w5user}',
                        '{w5prgm}',
                        '{w5wrks}',
                        '{w5ktdl}',
                        '{w5kcrd}',
                        {w5kupd},
                        {w5kupt},
                        '{w5kusr}',
                        '{w5kwrk}',
                        '{w5kacd}',
                        {w5kaud},
                        {w5kaut},
                        '{w5kaus}',
                        '{w5kauw}')";
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
                    sql = $@" INSERT INTO AS400DB01.ILOD0001.ilwk05hs  (SELECT 'C',  {m_UdpD.ToString()},{m_UdpT.ToString()},
                          '{userName}','{wrkStn}', W5CSNO,W5BRN,W5APNO,W5CSTY,W5SBCD,W5FVTO,W5FVTH,W5FVTM,
                          W5FVTE,W5CCRD,W5CUPD,W5CUPT,W5CUSR,W5CWRK,W5CACD,W5CAUD,W5CAUT,W5CAUS,W5CAUW,W5VRTO,W5OFC,W5SALT,W5INET,
                          W5SSO,W5BOL,W5HOTL,W5HOEX,W5WOTL,W5WOEX,W5MOTL,W5VOOT,W5VONM,W5TONM,W5VOAD,W5TOAD,W5EMST,W5RVTO,W5IPTO, 
                          W5TITO,W5TYEM,W5LETN,W5LETP,W5LETD,W5OCRD,W5OUPD,W5OUPT,W5OUSR,W5OWRK,W5OACD,W5OAUD,W5OAUT,W5OAUS,W5OAUW,
                          W5VRTH,W5HTL,W5HTEX,W5VHHT,W5VHNM,W5THNM,W5VHAD,W5THAD,W5TYTL,W5RVTH,W5TVTH,W5IPTH,W5TITH,W5HCRD,W5HUPD,
                          W5HUPT,W5HUSR,W5HWRK,W5HACD,W5HAUD,W5HAUT,W5HAUS,W5HAUW,W5VRTM,W5MBTL,W5VMTL,W5MCRD,W5MUPD,W5MUPT,W5MUSR,
                          W5MWRK,W5MACD,W5MAUD,W5MAUT,W5MAUS,W5MAUW,W5VRTE,W5ECRD,W5EUPD,W5EUPT,W5EUSR,W5EWRK,W5EACD,W5EAUD,
                          W5EAUT,W5EAUS,W5EAUW,W5FIL,W5UPDT,W5UPTM,W5USER,W5PRGM,W5WRKS , W5KTDL, W5KCRD,W5KUPD, W5KUPT, W5KUSR,W5KWRK,W5KACD, W5KAUD,W5KAUT, W5KAUS,W5KAUW
                          FROM AS400DB01.ILOD0001.ilwk05 WHERE w5brn = {brn} AND w5apno = {appNo} )";
                }

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


                    sql = " UPDATE AS400DB01.ILOD0001.ilwk05 SET " + condition + "," +
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
                    sql = " UPDATE AS400DB01.ILOD0001.ilwk05 SET " +
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

                sql = " UPDATE AS400DB01.ILOD0001.ilwk05 SET " +
                                   " w5vrto = '" + w5vrto + "'," +
                                   " w5fvto = '" + w5fvto + "'," +
                                   " w5salt = '" + w5salt + "'," +
                                   " w5inet = " + w5inet + "," +
                                   " w5sso  = '" + w5sso + "'," +
                                   " w5bol  = '" + w5bol + "'," +
                                   " w5hotl = '" + w5hotl + "'," +
                                   " w5hoex = '" + w5hoex + "', " +
                                   //" w5hoex = '" + w5hoex + "', " +
                                   " w5wotl = '" + w5wotl + "', " +
                                   " w5woex = '" + w5woex + "', " +
                                   //" w5woex = '" + w5woex + "', " +
                                   " w5motl = '" + w5motl + "'," +
                                   " w5voot = '" + w5voot + "'," +
                                   " w5vonm = '" + w5vonm + "'," +
                                   " w5tonm = '" + w5tonm + "'," +
                                   " w5voad = '" + w5voad + "'," +
                                   " w5toad = '" + w5toad + "'," +
                                   " w5emst = '" + w5emst + "'," +
                                   " w5rvto = '" + w5rvto + "'," +
                                   " w5ipto = '" + w5ipto + "'," +
                                   " w5tito = '" + w5tito + "'," +
                                   " w5tyem = '" + w5tyem + "', " +
                                   " w5letn = '" + w5letn + "'," +
                                   " w5letp = '" + w5letp + "', " +
                                   " w5letd = '" + w5letd + "', " +
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
                    sql = " UPDATE AS400DB01.ILOD0001.ilwk05 SET " +
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
                    sql = " update AS400DB01.ILOD0001.ilwk05 set " +
                                         " w5vrto = '" + w5vrto + "'," +
                                         " w5fvth = '" + w5fvth + "'," +
                                         " w5vrth = '" + w5vrth + "'," +
                                         " w5htl  = '" + w5htl + "', " +
                                         " w5htex =  '" + w5htex + "', " +
                                         //" w5htex = '"+w5htex+"', "+
                                         " w5vhht = '" + w5vhht + "', " +
                                         " w5vhnm = '" + w5vhnm + "', " +
                                         " w5thnm = '" + w5thnm + "', " +
                                         " w5vhad = '" + w5vhad + "', " +
                                         " w5thad = '" + w5thad + "', " +
                                         " w5tytl = '" + w5tytl + "', " +
                                         " w5rvth = '" + w5rvth + "', " +
                                         " w5ipth = '" + w5ipth + "', " +
                                         " w5tith = '" + w5tith + "', " +
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
                    sql = " UPDATE AS400DB01.ILOD0001.ilwk05 SET " +
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
                    sql = " update AS400DB01.ILOD0001.ilwk05 set " +
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
                    sql = " UPDATE AS400DB01.ILOD0001.ilwk05 SET " +
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
                    sql = " UPDATE AS400DB01.ILOD0001.ilwk05 SET " +
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
                    sql = " UPDATE AS400DB01.ILOD0001.ilwk05 SET " +
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
        public string DELETE_ILWK12(string brn, string appNo)
        {
            string sql = "";
            try
            {
                if (brn != "" && appNo != "")
                {
                    sql = " DELETE FROM AS400DB01.ILOD0001.ILWK12 WHERE W12BRN = " + brn + " AND W12apn = " + appNo;
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
                sql = " INSERT INTO AS400DB01.ILOD0001.ilwk12 ( " +
                      " W12CSN,W12BRN,W12APN,W12RSQ,W12REL,W12TTL, " +
                      " W12NME,W12SNM,W12HTL,W12HEX,W12OTL,W12OEX, " +
                      " W12MOB,W12VTE,W12UDT,W12UTM,W12UUS,W12UPG,W12UWS ) VALUES (" +
                        W12CSN + "," +
                        W12BRN + "," +
                        W12APN + "," +
                        W12RSQ + "," +
                      "'" + W12REL + "'," +
                      "'" + W12TTL + "'," +
                      "'" + W12NME + "'," +
                      "'" + W12SNM + "'," +
                      "'" + W12HTL + "'," +
                      "'" + W12HEX + "'," +
                      //"'" + W12HEX + "'," +
                      "'" + W12OTL + "'," +
                      "'" + W12OEX + "'," +
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

        public string INSERT_CSMS13_JUDG(string m13app, string m13csn, string m13brn, string m13apn, string m13apt, string m13apv, string m13cha, string m13sch, string m13bdt, string m13sex, string m13mrt, string m13smt, string m13but, string m13occ, string m13pos, string
                        m13off, string m13res, string m13con, string m13ttl, string m13htl, string m13hex, string m13mtl, string m13wky, string m13wkm, string m13slt, string m13sld, string m13net, string m13cmp, string m13csq, string m13trm, string
                        m13tcl, string m13tca, string m13gol, string m13chl, string m13hzp, string m13htm, string m13ham, string m13hpv, string m13ozp, string m13otm, string m13oam, string m13opv, string m13lyr, string m13lmt, string m13fdt, string
                        m13mob, string m13emp, string m13lna, string m13pbl, string m13fil, string m131id, string m13gno, string m13acl, string m13bot, string m13cbl, string m13_rk, string m13_gn, string m13_ac, string m13cru, string m13cud, string
                        m13aus, string m13aud, string m13rtl, string m13sad, string m13sat, string m13osl, string m13osd, string m13ost, string M13SST, string M13SAJ, string M13CAL, string M13DOC, string m13upg, string m13udt, string m13utm, string
                        m13usr, string m13wks, string M13OTL, string M13OEX, string M13CMP, string M13CSQ, string M13TRM, string m13izp, string m13itm, string m13iam, string m13ipv)
        {
            string sql = "";
            try
            {
                sql = " INSERT INTO AS400DB01.CSOD0001.CSMS13 ( " +
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
                       //+ " @m13hex  ,"
                       + "'" + m13hex + "',"
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
                     + "'" + M13OEX + "',"
                    //+ "   @M13OEX   ,"
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
                sql = " UPDATE AS400DB01.CSOD0001.csms13 SET " +
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
                      //" m13hex =  @m13hex  ," +
                      " m13hex = '" + m13hex + "'," +
                      " m13mtl = '" + m13mtl + "'," +
                      " m13wky = " + m13wky + "," +
                      " m13wkm = " + m13wkm + "," +
                      " m13slt = '" + m13slt + "'," +
                      " m13sld = '" + m13sld + "'," +
                      " m13net = " + m13net + "," +
                      " m13cmp = '" + m13cmp + "'," +
                      " m13csq = " + m13csq + "," +
                      " m13trm = " + m13trm + "," +
                      " m13tcl = " + (!string.IsNullOrEmpty(m13tcl) ? m13tcl : "0") + "," +
                      " m13tca = " + (!string.IsNullOrEmpty(m13tca) ? m13tca : "0") + "," +
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
                      " M13FIL = " + (!string.IsNullOrEmpty(M13FIL) ? M13FIL : "''") + "," +
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
                      //" M13OEX =  @M13OEX  " +
                      " M13OEX = '" + M13OEX + "'" +
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
                sql = " UPDATE AS400DB01.CSOD0001.csms13 SET " +
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
                sql = " UPDATE AS400DB01.CSOD0001.csms13 SET " +
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

    }
    public class ILDataCenterMssqlKeyinStep1
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
        public EB_Service.DAL.DataCenter _dataCenter;

        public string LastError;
        public ILDataCenterMssqlKeyinStep1(UserInfo userInfo)
        {
            m_UserInfo = (UserInfo)userInfo;
            _dataCenter = new EB_Service.DAL.DataCenter(m_UserInfo);
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


        public async Task<DataSet> getProduct_<T>(string vendorCode, string campCode, string campSeq, string productType = "", string productBrand = "", string productCode = "", string productModel = "")
        {
            DataSet ds = new DataSet();
            //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_UserInfo;
            try
            {
                string sqlChk = " SELECT c07pit FROM AS400DB01.ILOD0001.ilcp07  WITH (NOLOCK) " +
                                 " WHERE c07cmp = " + campCode +
                                 " AND c07csq = " + campSeq;
                var resILCP07 = await _dataCenter.GetDataset<DataTable>(sqlChk, CommandType.Text);
                DataSet dsChk = resILCP07.data;
                _dataCenter.CloseConnectSQL();
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
                }
                if (productBrand != "")
                {
                    condition += " AND ((T42BRD) = '" + productBrand.ToUpper() + "' " +
                                " )";
                }
                if (productCode != "")
                {
                    condition += " AND ((T41COD) = '" + productCode.ToUpper() + "' " +
                                " )";
                }
                if (productModel != "")
                {
                    condition += " AND ((t44itm) = '" + productModel.ToUpper() + "' " +
                                " )";
                }

                if (all_product == "Y")
                {
                    sql = " SELECT C07CMP,C07CSQ,C07PRC,C07DOW,c07pit,c07min,c07max,t44itm, t44des,t44cod,T44PGP,T44TYP,T44BRD,T44MDL from AS400DB01.ILOD0001.ilcp07 WITH (NOLOCK)" +
                            " LEFT JOIN AS400DB01.ILOD0001.ilcp01  WITH (NOLOCK)  on (c07cmp=c01cmp) " +
                            " LEFT JOIN AS400DB01.ILOD0001.ilcp09  WITH (NOLOCK)  on (c09cmp=c01cmp) " +
                            " LEFT JOIN AS400DB01.ILOD0001.iltb44  WITH (NOLOCK)  on (1=1) " +
                            " LEFT JOIN AS400DB01.ILOD0001.iltb40 WITH (NOLOCK)  on (t44typ=t40typ) " +
                            " LEFT JOIN AS400DB01.ILOD0001.iltb42 WITH (NOLOCK)  on (t44brd=t42brd) " +
                            " LEFT JOIN AS400DB01.ILOD0001.iltb41 WITH (NOLOCK)  on (t44typ=t41typ and t44cod=t41cod) " +
                            " LEFT JOIN AS400DB01.ILOD0001.iltb43 WITH (NOLOCK)  on (t44typ=t43typ and t44brd=t43brd and t44cod=t43cod and t44mdl=t43mdl) " +
                            " Where c07cmp=" + campCode + " and c07csq=" + campSeq + " and t44del='' and c09BRN=001 and c01cst='A' " +
                            " and c07cmp=" + campCode + " and c07csq=" + campSeq + " and t44del='' " +
                            " and exists (select * from AS400DB01.ILOD0001.ilms13 WITH (NOLOCK) where p13ven=" + vendorCode + " and t44typ = p13typ and p13del = '') " +
                            " and not exists (select * from AS400DB01.ILOD0001.ilcp04 WITH (NOLOCK) where (c04cmp=ilcp07.c07cmp and c04pty=iltb44.t44typ and c04pcd=0) " +
                            " or c04cmp=ilcp07.c07cmp and c04pty=iltb44.t44typ and c04pcd=iltb44.t44cod) " + condition;
                }
                else
                {
                    sql = "  select C07CMP,C07CSQ,C07PRC,C07DOW,c07pit,c07min,c07max,t44itm, t44des,t44cod,T44PGP,T44TYP,T44BRD,T44MDL from AS400DB01.ILOD0001.ilcp07 WITH (NOLOCK) " +
                            "  left join AS400DB01.ILOD0001.ilcp09 WITH (NOLOCK)  on (c09cmp=c07cmp) " +
                            "  left join AS400DB01.ILOD0001.ilcp04 WITH (NOLOCK)  on (c07cmp=c04cmp) " +
                            "  left join AS400DB01.ILOD0001.ilcp01 WITH (NOLOCK)  on (c07cmp=c01cmp) " +
                            "  left join AS400DB01.ILOD0001.iltb44 WITH (NOLOCK)  on (t44itm=c07pit) " +
                            "  left join AS400DB01.ILOD0001.iltb40 WITH (NOLOCK)  on (t44typ=t40typ) " +
                            "  left join AS400DB01.ILOD0001.iltb42 WITH (NOLOCK)  on (t44brd=t42brd) " +
                            "  left join AS400DB01.ILOD0001.iltb41 WITH (NOLOCK)  on (t44typ=t41typ and t44cod=t41cod) " +
                            "  left join AS400DB01.ILOD0001.iltb43 WITH (NOLOCK)  on (t44typ=t43typ and t44brd=t43brd and t44cod=t43cod and t44mdl=t43mdl) " +
                            "  Where c07cmp=" + campCode + " and c07csq= " + campSeq + " and t44del='' and c09BRN=001 and c01cst='A' " +
                            "  and (c04cmp is null or (t44typ <> c04pty and t44cod<>c04pcd)) " +
                            "  and exists (select * from AS400DB01.ILOD0001.ilms13 WITH (NOLOCK) where p13ven=" + vendorCode + " and t44typ = p13typ and p13del = '') " +
                            condition +
                            "  order by c07cmp,c07csq ";
                }
                resILCP07 = await _dataCenter.GetDataset<DataTable>(sql, CommandType.Text);

                ds = resILCP07.data;
                _dataCenter.CloseConnectSQL();
            }
            catch (Exception ex)
            {
                _dataCenter.CloseConnectSQL();
            }
            return ds;
        }

        public async Task<DataSet> getProductUS_<T>(string vendorCode, string campCode, string campSeq, string product = "")
        {
            DataSet ds = new DataSet();
            //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_UserInfo;
            try
            {
                string sqlChk = " SELECT c07pit FROM AS400DB01.ILOD0001.ilcp07  WITH (NOLOCK) " +
                                 " WHERE c07cmp = " + campCode +
                                 " AND c07csq = " + campSeq;
                var resILCP07 = await _dataCenter.GetDataset<DataTable>(sqlChk, CommandType.Text);
                DataSet dsChk = resILCP07.data;
                _dataCenter.CloseConnectSQL();
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
                    sql = " SELECT C07CMP,C07CSQ,C07PRC,C07DOW,c07pit,c07min,c07max,t44itm, t44des,t44cod,T44PGP,T44TYP,T44BRD,T44MDL from AS400DB01.ILOD0001.ilcp07 WITH (NOLOCK) " +
                            " LEFT JOIN AS400DB01.ILOD0001.ilcp01 WITH (NOLOCK)  on (c07cmp=c01cmp) " +
                            " LEFT JOIN AS400DB01.ILOD0001.ilcp09 WITH (NOLOCK)  on (c09cmp=c01cmp) " +
                            " LEFT JOIN AS400DB01.ILOD0001.iltb44 WITH (NOLOCK)  on (1=1) " +
                            " LEFT JOIN AS400DB01.ILOD0001.iltb40 WITH (NOLOCK)  on (t44typ=t40typ) " +
                            " LEFT JOIN AS400DB01.ILOD0001.iltb42 WITH (NOLOCK)  on (t44brd=t42brd) " +
                            " LEFT JOIN AS400DB01.ILOD0001.iltb41 WITH (NOLOCK)  on (t44typ=t41typ and t44cod=t41cod) " +
                            " LEFT JOIN AS400DB01.ILOD0001.iltb43 WITH (NOLOCK)  on (t44typ=t43typ and t44brd=t43brd and t44cod=t43cod and t44mdl=t43mdl) " +
                            " Where c07cmp=" + campCode + " and c07csq=" + campSeq + " and t44del='' and c09BRN=001 and c01cst='A' " +
                            " and c07cmp=" + campCode + " and c07csq=" + campSeq + " and t44del='' " +
                            " and exists (select P13RRNO,P13VEN,P13LTY,P13TYP,P13FIL from AS400DB01.ILOD0001.ilms13 WITH (NOLOCK) where p13ven=" + vendorCode + " and t44typ = p13typ and p13del = '') " +
                            " and not exists (select C04CMP,C04PTY,C04PCD,C04PIT from AS400DB01.ILOD0001.ilcp04 WITH (NOLOCK) where (c04cmp=ilcp07.c07cmp and c04pty=iltb44.t44typ and c04pcd=0) " +
                            " or c04cmp=ilcp07.c07cmp and c04pty=iltb44.t44typ and c04pcd=iltb44.t44cod) " + condition;
                }
                else
                {
                    sql = "  select C07CMP,C07CSQ,C07PRC,C07DOW,c07pit,c07min,c07max,t44itm, t44des,t44cod,T44PGP,T44TYP,T44BRD,T44MDL from AS400DB01.ILOD0001.ilcp07 WITH (NOLOCK) " +
                            "  left join AS400DB01.ILOD0001.ilcp09 WITH (NOLOCK)  on (c09cmp=c07cmp) " +
                            "  left join AS400DB01.ILOD0001.ilcp04 WITH (NOLOCK)  on (c07cmp=c04cmp) " +
                            "  left join AS400DB01.ILOD0001.ilcp01 WITH (NOLOCK)  on (c07cmp=c01cmp) " +
                            "  left join AS400DB01.ILOD0001.iltb44 WITH (NOLOCK)  on (t44itm=c07pit) " +
                            "  left join AS400DB01.ILOD0001.iltb40 WITH (NOLOCK)  on (t44typ=t40typ) " +
                            "  left join AS400DB01.ILOD0001.iltb42 WITH (NOLOCK)  on (t44brd=t42brd) " +
                            "  left join AS400DB01.ILOD0001.iltb41 WITH (NOLOCK)  on (t44typ=t41typ and t44cod=t41cod) " +
                            "  left join AS400DB01.ILOD0001.iltb43 WITH (NOLOCK)  on (t44typ=t43typ and t44brd=t43brd and t44cod=t43cod and t44mdl=t43mdl) " +
                            "  Where c07cmp=" + campCode + " and c07csq= " + campSeq + " and t44del='' and c09BRN=001 and c01cst='A' " +
                            "  and (c04cmp is null or (t44typ <> c04pty and t44cod <> c04pcd)) " +
                            "  and exists (select P13RRNO,P13VEN,P13LTY,P13TYP,P13FIL from AS400DB01.ILOD0001.ilms13 WITH (NOLOCK) where p13ven=" + vendorCode + " and t44typ = p13typ and p13del = '') " +
                            condition +
                            "  order by c07cmp,c07csq ";
                }
                resILCP07 = await _dataCenter.GetDataset<DataTable>(sql, CommandType.Text);

                ds = resILCP07.data;
                _dataCenter.CloseConnectSQL();
            }
            catch (Exception ex)
            {
                _dataCenter.CloseConnectSQL();
            }
            return ds;
        }
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



    }
    public class ILDataCenterMssqlUsingCard
    {
        private UserInfo m_UserInfo;
        private string m_UserName;
        private string m_User;
        public string LastError;
        private string m_Wrkstn = "";
        public EB_Service.DAL.DataCenter _dataCenter;
        public ILDataCenterMssqlUsingCard(UserInfo userInfo)
        {
            m_UserInfo = userInfo;
            _dataCenter = new EB_Service.DAL.DataCenter(m_UserInfo);

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


        public async Task<DataSet> getOfficeTitleUS(string IDNo)
        {
            DataSet ds = new DataSet();
            //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_UserInfo;
            try
            {
                string sql = @" select W.OfficeTitleID as off_title, W.OfficeName as off_name 
                                  from CustomerDB01.CustomerInfo.CustomerGeneral G WITH (NOLOCK)
                             left join CustomerDB01.CustomerInfo.CustomerWorked W  WITH (NOLOCK) on G.ID = W.CustID
                                 where G.IDCard  = '" + IDNo + "'";

                var result = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result;
                if (result.success)
                {
                    ds = result.data;

                }
                return await Task.FromResult<DataSet>(ds);
            }
            catch (Exception ex)
            {
                return await Task.FromResult<DataSet>(ds);
            }
        }

        public async Task<DataSet> getGenMaritalStatus(string IDNo)
        {
            DataSet ds = new DataSet();
            //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_UserInfo;
            try
            {
                string sql = @" SELECT ID,Type,Code,DescriptionTHAI 
                                  from GeneralDB01.GeneralInfo.GeneralCenter WITH (NOLOCK)
                                 WHERE Type = 'MaritalStatusID'
                                 AND Code  = " + IDNo;

                var result = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result;
                if (result.success)
                {
                    ds = result.data;

                }
                return await Task.FromResult<DataSet>(ds);
            }
            catch (Exception ex)
            {
                return await Task.FromResult<DataSet>(ds);
            }
        }

        public async Task<DataSet> getGenResidentType(string IDNo)
        {
            DataSet ds = new DataSet();
            //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_UserInfo;
            try
            {
                string sql = @" SELECT ID,Type,Code,DescriptionTHAI 
                                  from GeneralDB01.GeneralInfo.GeneralCenter WITH (NOLOCK)
                                 WHERE Type = 'ResidentalStatusID'
                                 AND Code  = " + IDNo;

                var result = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result;
                if (result.success)
                {
                    ds = result.data;

                }
                return await Task.FromResult<DataSet>(ds);
            }
            catch (Exception ex)
            {
                return await Task.FromResult<DataSet>(ds);
            }
        }

        public async Task<DataSet> getGenOccupation(string IDNo)
        {
            DataSet ds = new DataSet();
            //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_UserInfo;
            try
            {
                string sql = @" SELECT ID,Type,Code,DescriptionTHAI 
                                  from GeneralDB01.GeneralInfo.GeneralCenter WITH (NOLOCK)
                                 WHERE Type = 'OccupationID'
                                 AND Code  = " + IDNo;

                var result = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result;
                if (result.success)
                {
                    ds = result.data;

                }
                return await Task.FromResult<DataSet>(ds);
            }
            catch (Exception ex)
            {
                return await Task.FromResult<DataSet>(ds);
            }
        }

        public async Task<DataSet> getGenPosition(string IDNo)
        {
            DataSet ds = new DataSet();
            //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_UserInfo;
            try
            {
                string sql = @" SELECT ID,Type,Code,DescriptionTHAI 
                                  from GeneralDB01.GeneralInfo.GeneralCenter WITH (NOLOCK)
                                 WHERE Type = 'PositionID'
                                 AND Code  = " + IDNo;

                var result = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result;
                if (result.success)
                {
                    ds = result.data;

                }
                return await Task.FromResult<DataSet>(ds);
            }
            catch (Exception ex)
            {
                return await Task.FromResult<DataSet>(ds);
            }
        }

        public async Task<DataSet> getEmployeeType(string IDNo)
        {
            DataSet ds = new DataSet();
            //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_UserInfo;
            try
            {
                string sql = @" SELECT ID,Type,Code,DescriptionTHAI 
                                  from GeneralDB01.GeneralInfo.GeneralCenter WITH (NOLOCK)
                                 WHERE Type = 'EmploymentTypeID'
                                 AND Code  = " + IDNo;

                var result = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result;
                if (result.success)
                {
                    ds = result.data;

                }
                return await Task.FromResult<DataSet>(ds);
            }
            catch (Exception ex)
            {
                return await Task.FromResult<DataSet>(ds);
            }
        }

        public async Task<DataSet> getTCLUS(string CSN)
        {
            DataSet ds = new DataSet();
            //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_UserInfo;
            try
            {
                string sql = @" SELECT M3CRLM FROM AS400DB01.CSOD0001.CSMS03 WITH (NOLOCK) WHERE M3CSNO  = " + CSN + "AND M3DEL = '' ";

                var result = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result;
                if (result.success)
                {
                    ds = result.data;

                }
                return await Task.FromResult<DataSet>(ds);
            }
            catch (Exception ex)
            {
                return await Task.FromResult<DataSet>(ds);
            }
        }

        public async Task<DataSet> getILTB09()
        {
            DataSet ds = new DataSet();
            //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_UserInfo;
            try
            {
                string sql = @" SELECT * FROM AS400DB01.ILOD0001.ILTB09 WITH (NOLOCK) WHERE T09CDE= '01' ";

                var result = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result;
                if (result.success)
                {
                    ds = result.data;

                }
                return await Task.FromResult<DataSet>(ds);
            }
            catch (Exception ex)
            {
                return await Task.FromResult<DataSet>(ds);
            }
        }

        public async Task<DataSet> getILMS10(string vendorCode)
        {
            DataSet ds = new DataSet();
            //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_UserInfo;
            try
            {
                string sql = @" SELECT FORMAT(P10VEN,'000000000000') AS p10ven FROM AS400DB01.ILOD0001.ILMS10 WITH (NOLOCK) 
                                 WHERE p10ven = " + vendorCode +
                                 " AND p10del = ''  AND p10ats = 'Y' ";

                var result = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result;
                if (result.success)
                {
                    ds = result.data;

                }
                return await Task.FromResult<DataSet>(ds);
            }
            catch (Exception ex)
            {
                return await Task.FromResult<DataSet>(ds);
            }
        }

        public async Task<DataSet> getResultCodeWithCode(string ResultCode)
        {
            DataSet ds = new DataSet();
            //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_UserInfo;
            try
            {
                string sql = @" SELECT g25rcd, g25des FROM AS400DB01.GNOD0000.GNTB25 WITH (NOLOCK) WHERE G25RCD = " + "'" + ResultCode + "'";

                var result = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result;
                if (result.success)
                {
                    ds = result.data;

                }
                return await Task.FromResult<DataSet>(ds);
            }
            catch (Exception ex)
            {
                return await Task.FromResult<DataSet>(ds);
            }
        }

        public async Task<DataSet> getcsmh00(string csn)
        {
            DataSet ds = new DataSet();
            //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_UserInfo;
            try
            {
                string sql = @" select S.SalaryTypeCode as h00sst, C.SalaryAdjustAMT as h00saj,M.Code as h00cal
                                  FROM CustomerDB01.CustomerInfo.CustomerSalary C WITH (NOLOCK)
                                  LEFT JOIN GeneralDB01.GeneralInfo.SalaryWorkType S WITH (NOLOCK) on C.SalaryWorkTypeID = S.SubSalaryType
                                  LEFT JOIN GeneralDB01.GeneralInfo.GeneralCenter M  WITH (NOLOCK) on (C.CalculateIncomeID = M.ID and M.Type = 'CalculateIncomeID')
                                  where C.CustID = " + csn;

                var result = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result;
                if (result.success)
                {
                    ds = result.data;

                }
                return await Task.FromResult<DataSet>(ds);
            }
            catch (Exception ex)
            {
                return await Task.FromResult<DataSet>(ds);
            }
        }

        public async Task<DataSet> getCustInfoByCard(string CardNo)
        {
            DataSet ds = new DataSet();
            //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_UserInfo;
            string sql = "";
            string Scsn = "";
            try
            {
                sql = $@"SELECT CASE WHEN ISNULL(MC02.R2CDNO, 0) != 0  THEN ISNULL(MC02.R2CDNO, 0) ELSE ISNULL(ContCard.M20EBC, 0) END AS CardNo,CustGen.CISNumber
                           FROM CustomerDB01.CustomerInfo.CustomerGeneral  AS CustGen WITH (NOLOCK) 
                    OUTER APPLY (SELECT TOP 1 R2CDNO FROM AS400DB01.RLOD0001.RLMC02  WITH (NOLOCK) WHERE R2CSNO = CustGen.CISNumber)  AS MC02
                    LEFT JOIN AS400DB01.CSOD0001.CSMS20 AS ContCard WITH (NOLOCK) ON ContCard.M20CSN = CustGen.CISNumber AND ISNULL(ContCard.M20STS, '') = '' AND UPPER(ISNULL(ContCard.M20FLG, '')) = 'P'
                    WHERE (R2CDNO = '{CardNo}' OR ContCard.M20EBC = '{CardNo}' )";
                var resultC = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result;
                if (resultC.success)
                {
                    ds = resultC.data;
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow drs = ds.Tables[0]?.Rows[0];
                        Scsn = drs["CISNumber"].ToString().Trim();
                        sql = "";
                        ds = new DataSet();
                        sql = @"select G.CISNumber as CSN,G.ID
                                 , G.IDCard as IDCard
                                 , TRIM(G.NameInTHAI) as Name
                                 , TRIM(G.SurnameInTHAI) as Surname
                             	 , S.Code as Gender 
                                 , S.ShortName as gn_sex
                                     , T.Code as TitleCode
                                     , T.DescriptionTHAI as TitleName 
                             	     , FORMAT(G.BirthDate,'yyyyMMdd','th-TH') as BirthDate 
                                     , FORMAT(G.IDCardExpiredDate,'yyyyMMdd','th-TH') as ExpireDate
                                     , M.Code as MaritalStatus
                                     , M.DescriptionTHAI as MaritalStatus_Desc         
                                     , R.Code as TypeofResident
                                     , R.DescriptionTHAI as TypeofResident_Desc
                                     , G.NoOfFamily as TotalFamily
                                     , G.ResidentalYear as ResidentYear         
                                     , G.ResidentalMonth as ResidentMonth
                                     , C.Code as Business
                                     , C.DescriptionTHAI as Business_Desc
                                     , W.CompanyBusinessID     
                                     , CS.SalaryAMT as m00sal      
                                     , O.Code as Occupation       
                                     , O.DescriptionTHAI as Occupation_Desc       
                                     , P.Code as Position       
                                     , P.DescriptionTHAI as Position_Desc       
                                     , W.TotalOfficer as TotalEmployee       
                                     , E.Code as EmployeeType       
                                     , E.DescriptionTHAI as EmployeeType_Desc       
                                     , W.TotalWorkedYear as TotalWorkYear      
                                     , W.TotalWorkedMonth as TotalWorkMonth       
                                     , ST.Code as SalaryType      
                                     , ST.DescriptionTHAI as SalaryType_Desc      
                                     , F.Code as m00oft       
                                     , W.OfficeName as m00ofc   
                                     , A.IsShipTo as M00DSN       
                                     , A.TelephoneNumber1 as m11tel       
                                     , A.ExtensionNumber1 as M11EXT       
                                     , A.Mobile as  m11mob      
                                     , 'CSSR07' as LastUpdateSalary       
                                     , TB.Code as Tambol_H       
                                     , TB.DescriptionTHAI as Tambol_Desc_H       
                                     , AP.Code as Amphur_H       
                                     , AP.DescriptionTHAI as Amphur_Desc_H       
                                     , PV.Code as Province_H       
                                     , PV.DescriptionTHAI as Province_Desc_H       
                                     , A.PostalAreaCode as Zipcode_H       
                                     , IL.P1BRN as p1brn       
                                     , IL.P1APNO as p1apno      
                                     , IL.P1APDT as p1apdt       
                                     , IL.P1PRAM as p1pram      
                                     , IL.P1VDID as p1vdid     
                                     , IL.P1PDGP 
                                from CustomerDB01.CustomerInfo.CustomerGeneral G   WITH (NOLOCK)  
                           LEFT JOIN CustomerDB01.CustomerInfo.CustomerWorked W  WITH (NOLOCK)  on g.ID = W.CustID and w.IsDelete = ''        
                           LEFT JOIN CustomerDB01.CustomerInfo.CustomerSalary CS  WITH (NOLOCK)  on G.ID = CS.CustID
                           LEFT join CustomerDB01.CustomerInfo.CustomerAddress A WITH (NOLOCK) on(G.ID = A.CustID  and A.CustRefID = ''  and CustomerDB01.CustomerInfo.fnGetCodeById(A.AddressCodeID) = 'H'  )
                       
                           LEFT JOIN GeneralDB01.GeneralInfo.GeneralCenter S  WITH (NOLOCK) on G.SexID = S.ID and S.Type = 'SexID'     
                           LEFT JOIN GeneralDB01.GeneralInfo.GeneralCenter T  WITH (NOLOCK) on G.TitleID = T.ID and T.Type = 'TitleID'     
                           LEFT JOIN GeneralDB01.GeneralInfo.GeneralCenter M  WITH (NOLOCK)on G.MaritalStatusID = M.ID and M.Type = 'MaritalStatusID'    
                           LEFT JOIN GeneralDB01.GeneralInfo.GeneralCenter R  WITH (NOLOCK) on G.ResidentalStatusID = R.ID and R.Type = 'ResidentalStatusID'   
                           LEFT JOIN GeneralDB01.GeneralInfo.GeneralCenter C  WITH (NOLOCK) on W.CompanyBusinessID = C.ID and C.Type = 'CompanyBusinessID'   
                           LEFT JOIN GeneralDB01.GeneralInfo.GeneralCenter O  WITH (NOLOCK) on W.OccupationID = O.ID and O.Type = 'OccupationID'   
                           LEFT JOIN GeneralDB01.GeneralInfo.GeneralCenter P  WITH (NOLOCK) on (W.PositionID = P.ID and P.Type = 'PositionID')
                           LEFT JOIN GeneralDB01.GeneralInfo.GeneralCenter E  WITH (NOLOCK) on (W.EmploymentTypeID = E.ID and E.Type = 'EmploymentTypeID')
                           LEFT JOIN GeneralDB01.GeneralInfo.GeneralCenter ST  WITH (NOLOCK) on (CS.SalaryTypeID = ST.ID and ST.Type = 'SalaryTypeID')
                           LEFT JOIN GeneralDB01.GeneralInfo.GeneralCenter F  WITH (NOLOCK) on (W.OfficeTitleID = F.ID and F.Type = 'OfficeTitleID')
                           LEFT JOIN GeneralDB01.GeneralInfo.AddrTambol TB WITH (NOLOCK) on A.TambolID = TB.ID
                           LEFT JOIN GeneralDB01.GeneralInfo.AddrAumphur AP WITH (NOLOCK) on A.AmphurID = AP.ID
                           LEFT JOIN GeneralDB01.GeneralInfo.AddrProvince PV WITH (NOLOCK) on A.ProvinceID = PV.ID
                           LEFT JOIN AS400DB01.ILOD0001.ILMS01 IL WITH (NOLOCK) on ( P1BRN = " + m_UserInfo.BranchNo + " AND P1APNO = 0) " +
                               "where CISNumber = '" + Scsn + "' ";

                        var result = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result;
                        if (result.success)
                        {
                            ds = result.data;

                        }
                    }
                    else
                    {
                        ds = new DataSet();
                    }
                }

                return await Task.FromResult<DataSet>(ds);
            }
            catch (Exception ex)
            {
                return await Task.FromResult<DataSet>(ds);
            }
        }

        public string INSERT_ILMS01(string p1brn, string p1apno, string p1ltyp, string p1appt, string p1apvs,
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
            string sql = "";
            try
            {
                sql = $@" INSERT INTO AS400DB01.ILOD0001.ilms01 (p1brn , p1apno , p1ltyp , p1appt , p1apvs , 
                                                                 p1apdt, p1vdid , p1mkid , p1camp , p1cmsq, 
                                                                 p1item, p1pdgp , p1pric , p1qty  , p1purc ,
                                                                 p1vatr, p1vata , p1down , p1term , p1rang ,
                                                                 p1ndue, p1lndr , p1dutr , p1infr , p1intr ,
                                                                 p1crur, p1pram , p1inta , p1crua , p1infa ,
                                                                 p1duty, p1diff , p1coam , p1fdam , p1frtm ,
                                                                 p1frdt, p1fram , p1aprj , p1stdt , p1sttm ,
                                                                 p1fdue, p1csno , p1loca , p1crcd , P1RESN ,
                                                                 p1kusr, p1kdte , p1ktim , p1avdt , p1avtm ,
                                                                 p1fill, p1updt , p1uptm , p1upus , p1prog ,
                                                                 p1wsid, p1rsts , P1AUTH , p1cont , p1cndt,p1payt ,
                                                                 p1pbcd, p1pbrn , p1paty , p1pano)

                                                        values ( {p1brn},
                                                                 {p1apno},
                                                                '{p1ltyp}',
                                                                '{p1appt}', 
                                                                '{p1apvs}',
                                                                 {p1apdt},
                                                                 {p1vdid},
                                                                 {p1mkid},
                                                                 {p1camp},
                                                                 {p1cmsq},
                                                                 {p1item},
                                                                '{p1pdgp}',
                                                                 {p1pric},
                                                                 {p1qty},
                                                                 {p1purc},
                                                                 {p1vatr},
                                                                 {p1vata},
                                                                 {p1down},
                                                                 {p1term},
                                                                 {p1rang},
                                                                 {p1ndue},
                                                                 {p1lndr},
                                                                 {p1dutr},
                                                                 {p1infr},
                                                                 {p1intr},
                                                                 {p1crur},
                                                                 {p1pram},
                                                                 {p1inta},
                                                                 {p1crua},
                                                                 {p1infa},
                                                                 {p1duty},
                                                                 {p1diff},
                                                                 {p1coam},
                                                                 {p1fdam},
                                                                 {p1frtm},
                                                                 {p1frdt},
                                                                 {p1fram},
                                                                '{p1aprj}',
                                                                 {p1stdt} ,
                                                                 {p1sttm},
                                                                 {p1fdue},
                                                                 {p1csno},
                                                                '{p1loca}',
                                                                '{p1crcd}',
                                                                 '{P1RESN}',
                                                                '{p1kusr}',
                                                                 {p1kdte},
                                                                 {p1ktim},
                                                                 {p1avdt},
                                                                 {p1avtm},
                                                                '{p1fill}',
                                                                 {p1updt},
                                                                 {p1uptm},
                                                                '{p1upus}',
                                                                '{p1prog}',
                                                                '{p1wsid}',
                                                                '{p1rsts}',
                                                                '{P1AUTH}',
                                                                 {p1cont},
                                                                 {p1cndt},
                                                                 '{p1payt}',
                                                                 '{p1pbcd}',
                                                                 '{p1pbrn}',
                                                                 '{p1paty}',
                                                                 '{p1pano}' )";
            }
            catch (Exception ex)
            {

            }
            return sql;
        }

        public string INSERT_CSMS13UC(
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
                               , string m13aus, string m13aud)
        {
            string sql = "";
            try
            {
                sql = $@" INSERT INTO AS400DB01.CSOD0001.CSMS13
                             (m13app,m13csn,m13brn,m13apn,m13apt,
                             m13apv,m13sex,m13mrt,m13smt,m13but,
                             m13occ,m13pos,m13off,m13lna,m13res,
                             m13con,m13ttl,m13htl,m13hex,m13mtl,
                             m13wky,m13wkm,m13slt,m13net,m13cmp,
                             m13csq,m13trm,m13tcl,m13tca,m13gol,
                             m13bdt,m13chl,m13hzp,m13htm,m13ham,
                             m13hpv,m13lyr,m13lmt,m13fdt,m13mob,
                             m131id,m13gno,m13acl,m13bot,m13cbl,
                             M13#RK,M13#GN,m13#ac,m13cru,m13cud,
                             M13SAD,M13SAT,M13OSL,M13OSD,M13OST,
                             M13EMP,M13FIL,m13sst,m13saj,m13cal,
                             m13upg,m13udt,m13utm, m13usr,m13wks,
                             m13aus,m13aud) 
                           VALUES ('{m13app}'
                                  ,'{m13csn}'
                                  ,{m13brn}
                                  ,{m13apn}
                                  ,'{m13apt}'
                                  ,'{m13apv}'
                                  ,'{m13sex}'
                                  ,'{m13mrt}'
                                  ,'{m13smt}'
                                  ,'{m13but}'
                                  ,'{m13occ}'
                                  ,'{m13pos}'
                                  ,'{m13off}'
                                  ,{m13lna}
                                  ,'{m13res}'
                                  ,{m13con}
                                  ,'{m13ttl}'
                                  ,'{m13htl}'
                                  ,'{m13hex}'
                                  ,'{m13mtl}'
                                  ,{m13wky}
                                  ,{m13wkm}
                                  ,'{m13slt}'
                                  ,{m13net}
                                  ,'{m13cmp}'
                                  ,{m13csq}
                                  ,{m13trm}
                                  ,{m13tcl}
                                  ,{m13tca}
                                  ,{m13gol}
                                  ,{m13bdt}
                                  ,{m13chl}
                                  ,{m13hzp}
                                  ,{m13htm}
                                  ,{m13ham}
                                  ,{m13hpv}
                                  ,{m13lyr}
                                  ,{m13lmt}
                                  ,{m13fdt}
                                  ,'{m13mob}'
                                  ,{m131id}
                                  ,{m13gno}
                                  ,{m13acl}
                                  ,{m13bot}
                                  ,{m13cbl}
                                  ,{M13_RK}
                                  ,{M13_GN}
                                  ,{m13_ac}
                                  ,'{m13cru}'
                                  ,{m13cud}
                                  ,{M13SAD}
                                  ,{M13SAT}
                                  ,{M13OSL}
                                  ,{M13OSD}
                                  ,{M13OST}
                                  ,'{M13EMP}'
                                  ,'{M13FIL}'
                                  ,'{m13sst}'
                                  ,{m13saj}
                                  ,'{m13cal}'
                                  ,'{m13upg}'
                                  ,{m13udt}
                                  ,{m13utm}
                                  ,'{m13usr}'
                                  ,'{m13wks}'
                                  ,'{m13aus}'
                                  ,{m13aud} ) ";
            }
            catch (Exception ex)
            {
                //return res;
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
                    sql = $@" UPDATE  AS400DB01.CSOD0001.CSMS13 
                             SET  M13FIL = CONCAT(SUBSTRING(M13FIL,1,27),'{crmd}',SUBSTRING(M13FIL, 30, Len(M13FIL))) 
                        WHERE m13brn = {m13brn}
                        AND   m13apn = {m13apn}
                        AND   M13APP = 'IL' ";
                }
                else
                {
                    sql = $@" UPDATE  AS400DB01.CSOD0001.CSMS13 
                             SET  M13FIL = CONCAT(SUBSTRING(M13FIL,1,27),'{crrw}','{crmd}',SUBSTRING(M13FIL, 30, Len(M13FIL))) 
                        WHERE m13brn = {m13brn}
                        AND   m13apn = {m13apn}
                        AND   M13APP = 'IL' ";
                }
            }
            catch (Exception ex)
            {

            }
            return sql;
        }

        public string INSERT_ILMS02(string p2brn, string p2cont, string p2lnty, string p2csno, string p2apno,
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
                                string p2prog, string p2user, string p2ddsp)
        {
            string sql = "";
            try
            {
                sql = $@" INSERT INTO AS400DB01.ILOD0001.ILMS02
                             (p2brn,p2cont,p2lnty,p2csno,p2apno,
                             p2appt,p2crcd,p2atcd,p2loca,p2vdid,
                             p2mkid,p2camp,p2cmsq,p2cmct,p2item,
                             p2pric,p2qty,p2purc,p2vatr,p2vata,
                             p2down,p2term,p2rang,p2ndue,p2dte1,
                             p2cndt,p2bkdt,p2lndr,p2dutr,p2infr,
                             p2intr,p2crur,p2toam,p2osam,p2pcam,
                             p2pcbl,p21due,p2fdam,p2diff,p2difb,
                             p2frtm,p2frdt,p2fram,p2duty,p2dutb,
                             p2fee,p2feeb,p2ufeb,p2feib,p2crua,
                             p2crub,p2ucrb,p2ucib,p2uida,p2intb,
                             p2ubas,p2uidb,p2resn,p2updt,p2uptm,
                             p2prog,p2user,p2ddsp) 
                           VALUES ({p2brn}
                                  ,{p2cont}
                                  ,'02'
                                  ,{p2csno}
                                  ,{p2apno}
                                  ,'02'
                                  ,'{p2crcd}'
                                  ,'{p2atcd}'
                                  ,'250'
                                  ,{p2vdid}
                                  ,{p2mkid}
                                  ,{p2camp}
                                  ,{p2cmsq}
                                  ,'R'
                                  ,{p2item}
                                  ,{p2pric}
                                  ,{p2qty}
                                  ,{p2purc}
                                  ,{p2vatr}
                                  ,{p2vata}
                                  ,{p2down}
                                  ,{p2term}
                                  ,{p2rang}
                                  ,{p2ndue}
                                  ,'2'
                                  ,{p2cndt}
                                  ,{p2bkdt}
                                  ,{p2lndr}
                                  ,{p2dutr}
                                  ,{p2infr}
                                  ,{p2intr}
                                  ,{p2crur}
                                  ,{p2toam}
                                  ,{p2osam}
                                  ,{p2pcam}
                                  ,{p2pcbl}
                                  ,{p21due}
                                  ,{p2fdam}
                                  ,{p2diff}
                                  ,{p2difb}
                                  ,{p2frtm}
                                  ,{p2frdt}
                                  ,{p2fram}
                                  ,{p2duty}
                                  ,{p2dutb}
                                  ,{p2fee}
                                  ,{p2feeb}
                                  ,{p2ufeb}
                                  ,{p2feib}
                                  ,{p2crua}
                                  ,{p2crub}
                                  ,{p2ucrb}
                                  ,{p2ucib}
                                  ,{p2uida}
                                  ,{p2intb}
                                  ,{p2ubas}
                                  ,{p2uidb}
                                  ,'{p2resn}'
                                  ,{p2updt}
                                  ,{p2uptm}
                                  ,'{p2prog}'
                                  ,'{p2user}'
                                  ,'{p2ddsp}') ";
            }
            catch (Exception ex)
            {
                //return res;
            }
            return sql;
        }

        public string INSERT_ILMD012(string d012br, string d012ap, string d012sq, string d012lt, string d012ct, string d012tt
                               , string d012fm, string d012to, string d012ir, string d012cr, string d012fr
                               , string d012pa, string d012ia, string d012ca, string d012fa, string d012in
                               , string d012df, string d012ud, string d012ut, string d012us, string d012pg
                               , string d012ws)
        {
            string sql = "";
            try
            {
                sql = $@" INSERT INTO AS400DB01.ILOD0001.ILMD012
                             (d012br,d012ap,d012sq,d012lt,d012ct,d012tt,
                             d012fm,d012to,d012ir,d012cr,d012fr,
                             d012pa,d012ia,d012ca,d012fa,d012in,
                             d012df,d012ud,d012ut,d012us,d012pg,
                             d012ws) 
                           VALUES ({d012br}
                                  ,{d012ap}
                                  ,{d012sq}
                                  ,'{d012lt}'
                                  ,{d012ct}
                                  ,{d012tt}
                                  ,{d012fm}
                                  ,{d012to}
                                  ,{d012ir}
                                  ,{d012cr}
                                  ,{d012fr}
                                  ,{d012pa}
                                  ,{d012ia}
                                  ,{d012ca}
                                  ,{d012fa}
                                  ,{d012in}
                                  ,{d012df}
                                  ,{d012ud}
                                  ,{d012ut}
                                  ,'{d012us}'
                                  ,'{d012pg}'
                                  ,'{d012ws}' ) ";
            }
            catch (Exception ex)
            {
                //return res;
            }
            return sql;
        }

        public string INSERT_IlMS23(string p23csn, string p23cnt, string p23int, string p23crt, string p23iny
                                  , string p23cry, string p23atm, string p23ain, string p23acr, string p23ldt
                                  , string p23upd, string p23upt, string p23usr, string p23dsp, string p23upg
                                  , string p23del, string p23ttm, string p23in1, string p23tf2, string p23tt2, string p23in2, string p23in3)
        {
            string sql = "";
            try
            {
                sql = $@" INSERT INTO AS400DB01.ILOD0001.ILMS23
                             (P23CSN, P23CNT, P23INT, P23CRT, P23INY, P23CRY,
                              P23ATM, P23AIN, P23ACR, P23LDT, P23UPD, P23UPT, 
                              P23USR, P23DSP, P23UPG, P23DEL, P23TTM, P23IN1,
                              P23TF2, P23TT2, P23IN2, P23IN3 ) 
                           VALUES ({p23csn}
                                  ,{p23cnt}
                                  ,{p23int}
                                  ,{p23crt}
                                  ,{p23iny}
                                  ,{p23cry}
                                  ,{p23atm}
                                  ,{p23ain}
                                  ,{p23acr}
                                  ,{p23ldt}
                                  ,{p23upd}
                                  ,{p23upt}
                                  ,'{p23usr}'
                                  ,'{p23dsp}'
                                  ,'{p23upg}' 
                                  ,'{p23del}' 
                                  ,{p23ttm}
                                  ,{p23in1}
                                  ,{p23tf2}
                                  ,{p23tt2}
                                  ,{p23in2}
                                  ,{p23in3} ) ";
            }
            catch (Exception ex)
            {
                //return res;
            }
            return sql;
        }
        private ulong m_UdpT = Convert.ToUInt32(DateTime.Now.ToString("HHmmss"));
        public bool checkApproveCriteria(ILDataCenter ilObj, string productCode, string vendorCode, string brn, string appNo, string appDate, string appType, string csn, string idno, string date_97, string program_name, bool nothave_th, string BizInit, string BranchNo, string username, string LocalClient, ref string t22cod, ref string t22seq, ref string sqlRet, string ver_contact = "")
        {
            ILDataSubroutine iLDataSubroutine = new ILDataSubroutine(m_UserInfo);

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
                string sql = " SELECT m13bdt,m13mrt,m13sex,m13res,m13con,m13lyr*12+m13lmt as m13tre,m13but,m13occ, " +
                                 " m13pos,m13off,m13emp,m13wky*12+m13wkm as m13len,m13slt,m13net,m13htl,m13mtl,m13#rk,t44typ " +
                                 " FROM AS400DB01.CSOD0001.CSMS13 WITH (NOLOCK) " +
                                 " LEFT JOIN AS400DB01.ILOD0001.ILTB44 WITH (NOLOCK) ON ( " + productCode + " = " + " t44itm) " +
                                 " WHERE m13app = 'IL' " +
                                 " AND m13brn = " + brn +
                                 " AND m13apn = " + appNo;
                ds_13 = _dataCenter.GetDataset<DataSet>(sql, CommandType.Text).Result.data;
                //_dataCenter.CloseConnectSQL();

                if (!ilObj.check_dataset(ds_13))
                {
                    return true;
                }

                DataRow dr_13 = ds_13.Tables[0].Rows[0];
                //**  call GNP0371 **//
                string in_AGE = "", error = "";
                iLDataSubroutine.CALL_GNP0371(dr_13["m13bdt"].ToString().PadLeft(8, '0'), "", "YMD", "B", "", "IL", "", BizInit, BranchNo, ref in_AGE, ref error);

                //ilObj_.CloseConnectioDAL();

                if (in_AGE.Trim() == "")
                {
                    in_AGE = "0";
                }
                // ***  check criteria iltb21 ***//

                string sql_20 = " SELECT t20cod,t22ag1,t22ag2,t22ag3,t22mrt,t22sex,t22res,t22tof,t22tre,t22bus,t22occ,t22pos, " +
                              " t22toe,t22emt,t22len,t22tys,t22sa1,t22sa2,t22sa3,t22teh,t22mob,t22cod,t22seq,t20des,t22vto,t22rnk " +
                              " FROM AS400DB01.ILOD0001.iltb20 WITH (NOLOCK) " +
                              " LEFT JOIN AS400DB01.ILOD0001.iltb21 WITH (NOLOCK) on(t21cod=t20cod) " +
                              " LEFT JOIN AS400DB01.ILOD0001.iltb22 WITH (NOLOCK) on(t22cod=t20cod) " +
                              " WHERE  t20std<= " + appDate + " AND " + " t20end >= " + appDate +
                              " AND " + appType_con +
                              " AND t20sts = 'A' AND t20del=  '' " +
                              " AND ((t21ven = " + vendorCode +
                              " AND (t21pty = " + dr_13["t44typ"].ToString() + " OR t21pty = 0 )  " +
                              " AND t21del = '' ) OR ( t21ven =  0)) " +
                              " AND t22del = '' ";
                //" AND t21ven = " + vendorCode +
                //" AND (t21pty = " + dr_13["t44typ"].ToString() + " OR t21pty = 0) AND " +
                //" t21del = '' AND t22del = '' with ur ";

                DataSet ds_20 = _dataCenter.GetDataset<DataSet>(sql_20, CommandType.Text).Result.data;
                // _dataCenter.CloseConnectSQL();
                //ilObj_.CloseConnectioDAL();
                if (!ilObj.check_dataset(ds_20))
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
                    string sql_22 = " SELECT t22cod,t22seq FROM AS400DB01.ILOD0001.iltb22 WITH (NOLOCK) " + condition;
                    DataSet ds_22 = _dataCenter.GetDataset<DataSet>(sql_22, CommandType.Text).Result.data;
                    _dataCenter.CloseConnectSQL();
                    //ilObj_.CloseConnectioDAL();
                    if (ilObj.check_dataset(ds_22))
                    {
                        foreach (DataRow dr_22 in ds_22.Tables[0].Rows)
                        {
                            string sql_23 = " SELECT * FROM AS400DB01.ILOD0001.iltb23 WITH (NOLOCK) " +
                                            " WHERE t23cod = " + dr_20["t22cod"].ToString() +
                                            " AND t23seq = " + dr_20["t22seq"].ToString() +
                                            " AND t23brn = " + brn +
                                            " AND t23apn = " + appNo +
                                            " AND t23csn = " + csn;
                            DataSet ds_23 = _dataCenter.GetDataset<DataSet>(sql_23, CommandType.Text).Result.data;
                            _dataCenter.CloseConnectSQL();


                            if (!ilObj.check_dataset(ds_23))
                            {
                                // ***  insert into iltb23  ***//
                                //iDB2Command cmd = new iDB2Command();
                                string sqlInsert = " INSERT INTO AS400DB01.ILOD0001.iltb23(t23cod,t23seq,t23brn,t23apn,t23csn,t23apt,t23upd,t23upt,t23upg,t23usr,t23uws) " +
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
                                    bool res_Call_GNSRCS = iLDataSubroutine.Call_GNSRCS(idno, "IL", brn, appNo, BizInit, BranchNo, ref WPHSTS, ref WPERR, ref WPMSG);
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

    }
    public class ILDataCenterMssqlTCL
    {
        private UserInfo m_UserInfo;
        private string m_UserName;
        private string m_User;
        public string LastError;
        private string m_Wrkstn = "";
        public EB_Service.DAL.DataCenter _dataCenter;
        public ILDataCenterMssqlTCL(UserInfo userInfo)
        {
            m_UserInfo = userInfo;
            _dataCenter = new EB_Service.DAL.DataCenter(m_UserInfo);

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

        public string InsertIlMS23(string p23csn, string p23cnt, string p23int, string p23crt, string p23iny, string p23cry, string p23atm, string p23ain,
            string p23acr, string p23ldt, string p23upd, string p23upt, string p23usr, string p23dsp, string p23upg, string p23del
            , string p23ttm, string p23in1, string p23tf2, string p23tt2, string p23in2, string p23in3)
        {
            string sql = "";
            try
            {
                sql = string.Format("INSERT INTO AS400DB01.ILOD0001.ILMS23 " +
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

    }
    public class ILDataCenterMssqlProduct
    {
        private UserInfo m_UserInfo;
        private string m_UserName;
        private string m_User;
        public string LastError;
        private string m_Wrkstn = "";
        public EB_Service.DAL.DataCenter _dataCenter;
        public ILDataCenterMssqlProduct(UserInfo userInfo)
        {
            m_UserInfo = userInfo;
            _dataCenter = new EB_Service.DAL.DataCenter(m_UserInfo);

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
                sql = " INSERT INTO AS400DB01.ILOD0001.ilmd012 ( " +
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
    }
    public class ILDataCenterMssqlReceiveFaxDoc
    {
        private UserInfo m_UserInfo;
        private string m_UserName;
        private string m_User;
        public string LastError;
        private string m_Wrkstn = "";
        public EB_Service.DAL.DataCenter _dataCenter;
        public ILDataCenterMssqlReceiveFaxDoc(UserInfo userInfo)
        {
            m_UserInfo = userInfo;
            _dataCenter = new EB_Service.DAL.DataCenter(m_UserInfo);

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

        public async Task<DataSet> getILMS10_setReceive(string vendorDes, string vendorCode = "")
        {
            DataSet ds = new DataSet();
            //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_UserInfo;
            try
            {
                string sql = "";
                if (vendorDes.Trim() != "")
                {
                    sql = " SELECT  p10ven,p10nam,p10fi1 FROM AS400DB01.ILOD0001.ILMS10 WITH (NOLOCK) WHERE FORMAT(p10ven,'000000000000') LIKE '" + vendorDes + "%'" +
                          " AND p10del <> 'X'  OR UPPER(P10TNM) LIKE  '%" + vendorDes.ToUpper() + "%'";

                }
                else if (vendorDes.Trim() == "" && vendorCode.Trim() != "")
                {
                    sql = " SELECT p10ven,p10nam,p10fi1 FROM AS400DB01.ILOD0001.ILMS10 WITH (NOLOCK) WHERE p10ven = " + vendorCode +
                           " AND p10del <> 'X' ";
                }
                else
                {
                    sql = " SELECT p10ven,p10nam,p10fi1 FROM AS400DB01.ILOD0001.ILMS10 WITH (NOLOCK) WHERE " +
                                 " p10del <> 'X'   ";
                }
                var result = await _dataCenter.GetDataset<DataTable>(sql, CommandType.Text);
                if (result.success)
                    ds = result.data;
                return await Task.FromResult<DataSet>(ds);
            }
            catch (Exception ex)
            {
                return await Task.FromResult<DataSet>(ds);
            }
        }

        public async Task<DataSet> get_ilmd01_Date(string appDate)
        {
            DataSet ds = new DataSet();
            //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_UserInfo;
            try
            {
                string sql = "";
                sql = " SELECT SUM(CASE WHEN d1abrn <=1 THEN 1 ELSE 0 end) as SUMHQ , " +
                               " SUM (CASE WHEN d1abrn > 1 THEN 1 ELSE 0  END) as SUMURT, " +
                               " SUM (CASE WHEN D1DCCM = 'Y' THEN 1 ELSE 0 END) as SUMCOMP, " +
                               " SUM (CASE WHEN D1DCCM <> 'Y' THEN 1 ELSE 0 END) as SUMNOTCOMP, " +
                               " SUM (1) as TOTALCASE " +
                               " FROM  AS400DB01.ILOD0001.ILMD01 WITH (NOLOCK) where d1apdt = " + appDate +
                               " AND d1del=''  ";
                var result = await _dataCenter.GetDataset<DataTable>(sql, CommandType.Text);
                if (result.success)
                    ds = result.data;
                return await Task.FromResult<DataSet>(ds);
            }
            catch (Exception ex)
            {
                return await Task.FromResult<DataSet>(ds);
            }
        }
        public async Task<DataSet> get_ilmd01_Hour(string appDate)
        {
            DataSet ds = new DataSet();
            //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_UserInfo;
            try
            {
                string sql = "";
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
                    sql = "SELECT    " +
                                case_time +
                                " AS FXTM " +
                                " , COUNT(*) as TOTALCASE " +
                                " FROM AS400DB01.ILOD0001.ilmd01 WITH (NOLOCK) WHERE d1apdt = " + appDate +
                                " AND d1del='' " +
                                " GROUP BY " +
                                case_time +
                                " ORDER BY " +
                                case_time;
                }

                var result = await _dataCenter.GetDataset<DataTable>(sql, CommandType.Text);
                if (result.success)
                    ds = result.data;
                return await Task.FromResult<DataSet>(ds);
            }
            catch (Exception ex)
            {
                return await Task.FromResult<DataSet>(ds);
            }
        }

        public async Task<DataSet> get_app_ilmd01(string appDate, string name = "", string appNo = "", string mode = "")
        {
            DataSet ds = new DataSet();
            //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_UserInfo;
            try
            {
                string sql = "";
                if (mode == "")
                {

                    if (appDate != "")
                    {


                        sql = " SELECT CONCAT(SUBSTRING(CAST(D1APDT AS nvarchar),7,2),'/',SUBSTRING(CAST(D1APDT AS nvarchar),5,2),'/',SUBSTRING(CAST(D1APDT AS nvarchar),1,4)) D1APDT,D1SRNO,D1IDNO,D1ABRN,D1VNID, " +
                                    " CONCAT(SUBSTRING(CAST(D1FXTM AS nvarchar),1,2), ':',SUBSTRING(CAST(D1FXTM AS nvarchar),3,2)) as D1FXTM1," +
                                    " D1FXTM,D1DCCM,D1TNAM,D1RMAK,D1UPUS " +
                                    "  FROM AS400DB01.ILOD0001.ilmd01 WITH (NOLOCK)  " +
                                    " WHERE d1apdt =  " + appDate +
                                    " AND d1del = '' ORDER BY D1SRNO  , D1FXTM ASC   ";

                    }
                }
                else if (mode == "GET" && appNo.Trim() != "")
                {
                    sql = @" SELECT D1APDT,D1SRNO,D1IDNO,D1ABRN,D1VNID
                                  ,D1FXTM,D1DCCM,D1TNAM,D1RESN,D1RMAK,D1UPDT
                                  ,D1UPTM,D1UPUS,D1PROG,D1WSID,D1DEL " +
                                " FROM AS400DB01.ILOD0001.ilmd01 WITH (NOLOCK) " +
                                " WHERE D1SRNO =  '" + appNo + "' ORDER BY D1SRNO  , D1FXTM ASC ";
                }

                var result = await _dataCenter.GetDataset<DataTable>(sql, CommandType.Text);
                if (result.success)
                    ds = result.data;
                return await Task.FromResult<DataSet>(ds);
            }
            catch (Exception ex)
            {
                return await Task.FromResult<DataSet>(ds);
            }
        }

        public async Task<DataSet> get_ilms01Rec(string P1BRN, string P1APNO)
        {
            DataSet ds = new DataSet();
            //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_UserInfo;
            try
            {

                string sql = " SELECT p1apdt,p1kdte,p1ktim  FROM  AS400DB01.ILOD0001.ILMS01 WITH (NOLOCK)  WHERE P1BRN = " + P1BRN +
                             " AND P1APNO = " + P1APNO;

                var result = await _dataCenter.GetDataset<DataTable>(sql, CommandType.Text);
                if (result.success)
                    ds = result.data;
                return await Task.FromResult<DataSet>(ds);
            }
            catch (Exception ex)
            {
                return await Task.FromResult<DataSet>(ds);
            }
        }

        public async Task<DataSet> get_ilms01_setTimeReceive(string appNo, string Branch)
        {
            DataSet ds = new DataSet();
            //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_UserInfo;
            try
            {

                string sql = " SELECT idcard as m00idn, NameInTHAI as M00TNM,p1vdid,SurnameInTHAI as M00TSN FROM AS400DB01.ILOD0001.ILMS01 WITH (NOLOCK) " +
                                 " LEFT JOIN CustomerDB01.CustomerInfo.CustomerGeneral    WITH (NOLOCK) ON(p1csno=CISNumber) " +
                                 " WHERE p1brn = " + Branch +
                                 " AND p1apno = '" + appNo + "' ";

                var result = await _dataCenter.GetDataset<DataTable>(sql, CommandType.Text);
                if (result.success)
                    ds = result.data;
                return await Task.FromResult<DataSet>(ds);
            }
            catch (Exception ex)
            {
                return await Task.FromResult<DataSet>(ds);
            }
        }

        public string updateILMD01_setTime(string oper, string D1APDT, string D1SRNO, string D1IDNO, string D1ABRN,
                                   string D1VNID, string D1FXTM, string D1DCCM, string D1TNAM, string D1RMAK,
                                   string D1UPDT, string D1UPTM, string D1UPUS, string D1PROG, string D1WSID, string IDNO_OLD = "")
        {
            //int res = 0;
            string sql = "";
            try
            {
                if (oper == "EDIT")
                {
                    sql = " UPDATE AS400DB01.ILOD0001.ILMD01 SET "
                         + " D1APDT = " + D1APDT + ","
                         + " D1SRNO = '" + D1SRNO + "',"
                         + " D1IDNO = '" + D1IDNO + "',"
                         + " D1ABRN = " + D1ABRN + ","
                         + " D1VNID = " + D1VNID + ","
                         + " D1FXTM = " + D1FXTM + ","
                         + " D1DCCM = '" + D1DCCM + "',"
                         //+ " D1TNAM =  @D1TNAM,"
                         //+ " D1RMAK =  @D1RMAK,"
                         + " D1TNAM = '" + D1TNAM + "',"
                         + " D1RMAK = '" + D1RMAK + "',"
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
                    sql = " INSERT INTO AS400DB01.ILOD0001.ILMD01 (D1APDT, D1SRNO,D1IDNO,D1ABRN,D1VNID,D1FXTM,D1DCCM,D1TNAM,D1RMAK,D1UPDT,D1UPTM,D1UPUS,D1PROG,D1WSID,D1DEL) "
                          + " VALUES ("
                          + D1APDT + ","
                          + "'" + D1SRNO + "',"
                          + "'" + D1IDNO + "',"
                          + D1ABRN + ","
                          + D1VNID + ","
                          + D1FXTM + ","
                          + "'" + D1DCCM + "',"
                          //+ "@D1TNAM,"
                          //+ "@D1RMAK,"
                          + "'" + D1TNAM + "',"
                          + "'" + D1RMAK + "',"
                          + "'" + D1UPDT + "',"
                          + "'" + D1UPTM + "',"
                          + "'" + D1UPUS + "',"
                          + "'" + D1PROG + "',"
                          + "'" + D1WSID + "',"
                          + "       ''     )";
                }
                else if (oper == "DELETE")
                {
                    sql = " DELETE FROM AS400DB01.ILOD0001.ILMD01 "
                        + " WHERE  D1APDT = " + D1APDT
                        + " AND D1SRNO    = '" + D1SRNO + "'"
                        + " AND D1IDNO    = '" + D1IDNO + "'"
                        + " AND D1FXTM    = " + D1FXTM;
                }
            }
            catch (Exception ex)
            {
                //return res;
            }
            return sql;
        }
    }
    public class ILDataCenterResendBureau
    {

        private ulong m_UpdDate = Convert.ToUInt32(DateTime.Now.ToString("yyMMdd"));
        private ulong m_UpdTime = Convert.ToUInt32(DateTime.Now.ToString("HHmmss"));
        private ulong m_UdpD = Convert.ToUInt32(DateTime.Now.ToString("yyyyMMdd", new CultureInfo("th-TH")));
        private ulong m_UdpT = Convert.ToUInt32(DateTime.Now.ToString("HHmmss", new CultureInfo("th-TH")));
        private UserInfo m_UserInfo;
        private string m_UserName;
        private string m_User;
        public string LastError;
        private string m_Wrkstn = "";
        public EB_Service.DAL.DataCenter _dataCenter;

        public ILDataCenterResendBureau(UserInfo userInfo)
        {
            m_UserInfo = userInfo;
            _dataCenter = new EB_Service.DAL.DataCenter(m_UserInfo);
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

        // *********    Resend Bureau **********//
        // get resend 
        // edit 10/07/2558  Req:61756 
        // เปลี่ยนเงื่อนไขจาก Resend ได้ภายในวัน เป็น สามารถ Resend ย้อนหลังได้ 6 วัน
        public DataSet get_resend_bureau(string appDate, string Branch)
        {
            ILDataCenter ilobj = new ILDataCenter();
            DataSet ds = new DataSet();
            try
            {

                //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
                UserInfomation = m_UserInfo;
                if (appDate != "")
                {
                    string sql_RLMS97 = " SELECT P97CDT FROM AS400DB01.RLOD0001.RLMS97 WITH (NOLOCK) WHERE P97REC = '69'";
                    ds = _dataCenter.GetDataset<DataTable>(sql_RLMS97, CommandType.Text).Result.data;

                    //ilobj.CloseConnectioDAL();
                    if (ilobj.check_dataset(ds))
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
                        //             " WHERE g76bus= 'IL'  " +
                        //             " AND g76rdt >=  " + RLDate97 +
                        //             " AND g76rdt BETWEEN " + dateStart + " AND " + dateEnd +
                        //             " AND g76res = 'C' AND P1APPT = '01' AND g76sts = '' AND g76brn = " + Branch;
                        string sql = $@"SELECT brname, g76apn, g76idn,
                                      NameInTHAI m00tnm, SurnameInTHAI m00tsn, g76rdt, g76brn, CISNumber m00csn, 
                                      FORMAT( BirthDate, 'yyyymmdd', 'th-TH' ) m00bdt, DescriptionTHAI gnb2td,
                                      CASE WHEN (SELECT TOP(1) R2CDNO FROM AS400DB01.RLOD0001.RLMC02 WITH (NOLOCK) WHERE cg.CISNumber = R2CSNO)  IS NOT NULL THEN
                                      COALESCE ((SELECT TOP (1) R2CDNO FROM AS400DB01.RLOD0001.RLMC02 WITH (NOLOCK) WHERE R2CSNO = cg.CISNumber), 0) 
                                      WHEN (SELECT TOP(1) M20EBC FROM AS400DB01.CSOD0001.CSMS20 G WITH (NOLOCK) WHERE cg.CISNumber = M20CSN)  IS NOT NULL THEN
                                      COALESCE ((SELECT TOP (1) M20EBC FROM AS400DB01.CSOD0001.CSMS20 G WITH (NOLOCK) WHERE cg.CISNumber = M20CSN), 0) 
                                      ELSE NULL END AS m00ebc,
                                      CONCAT(SUBSTRING(CAST(g76rdt AS nvarchar ),5,2),'/',SUBSTRING(CAST(g76rdt AS nvarchar ),3,2),'/','25',SUBSTRING(CAST(g76rdt AS nvarchar ),1,2)) appdate,BRCODE 
                                      FROM AS400DB01.GNOD0000.GNMS76 WITH (NOLOCK)
                                      JOIN AS400DB01.ILOD0001.ILMS01 WITH (NOLOCK) ON (g76apn=p1apno and g76brn=p1brn)
                                      LEFT JOIN CustomerDB01.CustomerInfo.CustomerGeneral cg WITH (NOLOCK) ON (IDCard = G76IDN)
                                      LEFT JOIN GeneralDB01.GeneralInfo.GeneralCenter gc WITH (NOLOCK) ON (gc.ID = cg.TitleID AND gc.Type = 'TitleID')
                                      LEFT JOIN AS400DB01.SYOD0000.SYFBRDES WITH (NOLOCK) ON (BRCODE =  G76BRN)
                                      WHERE g76bus = 'IL' AND G76RDT >= {RLDate97} AND G76RDT BETWEEN {dateStart} AND {dateEnd}
                                      AND g76res = 'C' AND P1APPT = '01' AND g76sts = '' AND g76brn = {Branch} ";

                        ds = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
                        _dataCenter.CloseConnectSQL();
                    }

                }

            }
            catch (Exception ex)
            {
                _dataCenter.CloseConnectSQL();
            }
            return ds;

        }

        public void resendBureau(ILDataCenter ilObj, string date97, string app, string brn, ref bool resBureau, ref string resNCB, ref string detail)
        {
            try
            {
                ILDataSubroutine iLDataSubroutine = new ILDataSubroutine(m_UserInfo);
                if (app != "" && brn != "")
                {
                    //*** check from ilms01 ***//

                    DataSet ds_01 = checkDataResendBureau(app, brn);

                    if (ilObj.check_dataset(ds_01))
                    {
                        DataRow dr_01 = ds_01.Tables[0].Rows[0];
                        //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
                        ilObj.UserInfomation = m_UserInfo;
                        string bureau_prm = "";
                        string ErrorCode = "";
                        string Error = "";
                        string crreview_prm = "";
                        //*************** Check NCB ***************//
                        Connect_NcbAPI ncbAPI = new Connect_NcbAPI(m_UserInfo);
                        var resCheckNCBAPI = ncbAPI.CheckNCBGateway("IL", brn, dr_01["P1APNO"].ToString().Trim(), dr_01["m00idn"].ToString().Trim(), dr_01["m00csn"].ToString(),
                                                       dr_01["M00TNM"].ToString().Trim() + " " + dr_01["M00TNM"].ToString().Trim(),
                                                      dr_01["M00BDT"].ToString().Trim(), "", m_UserInfo.Username.Trim()).Result;
                        if (resCheckNCBAPI.success)
                        {
                            var jsonData = JsonConvert.SerializeObject(resCheckNCBAPI.data);
                            var resData = (JObject)JsonConvert.DeserializeObject(jsonData, typeof(JObject));
                            bureau_prm = resData["resultNCB"].ToString().Trim();
                            crreview_prm = resData["resultNCBCreditReview"].ToString().Trim();
                        }
                        // Todo Call NCB
                        //bool call_GNSR69 = iLDataSubroutine.Call_GNSR69("IL", brn, dr_01["P1APNO"].ToString().Trim(), dr_01["m00idn"].ToString().Trim(), dr_01["m00csn"].ToString(),
                        //                                      dr_01["M00TNM"].ToString().Trim() + " " + dr_01["M00TNM"].ToString().Trim(),
                        //                                      dr_01["M00BDT"].ToString().Trim(), "",
                        //                                      ref bureau_prm, ref crreview_prm, ref ErrorCode, ref Error, m_UserInfo.BizInit, m_UserInfo.BranchNo);
                        //ilObj.CloseConnectioDAL();

                        resNCB = bureau_prm;
                        DataSet ds_reslNCB = new DataSet();
                        string sql_resNCB = " SELECT g101ed FROM AS400DB01.GNOD0000.gntb101 WITH (NOLOCK) WHERE g101cd = '" + bureau_prm.Trim() + "'";
                        var res = _dataCenter.GetDataset<DataTable>(sql_resNCB, CommandType.Text).Result;
                        if (res.success)
                            ds_reslNCB = res.data;
                        //DataSet ds_reslNCB = ilObj.RetriveAsDataSetNoConnect(sql_resNCB);
                        if (ilObj.check_dataset(ds_reslNCB))
                        {
                            DataRow dr_resNCB = ds_reslNCB.Tables[0].Rows[0];
                            detail = dr_resNCB["g101ed"].ToString().Trim();
                        }

                        bool save_76 = false;



                        DataSet ds_76 = getGNMS76("IL", dr_01["P1APNO"].ToString().Trim(), dr_01["P1BRN"].ToString().Trim());
                        if (ilObj.check_dataset(ds_76))
                        {
                            save_76 = true;
                        }
                        //** ilms01hs **//

                        iDB2Command cmd = new iDB2Command();
                        //cmd.Parameters.Clear();
                        //cmd.CommandText = ilObj.InsertILMS01HS_autoPay(m_UdpD.ToString(), m_UpdTime.ToString(), m_UserInfo.Username, m_UserInfo.LocalClient, dr_01["P1BRN"].ToString().Trim(), dr_01["P1APNO"].ToString().Trim());

                        //int res_01HS = ilObj.ExecuteNonQuery(cmd);
                        //if (res_01HS == -1)
                        //{
                        //    ilObj.RollbackDAL();
                        //    ilObj.CloseConnectioDAL();

                        //    resBureau = false;
                        //    return;
                        //}
                        bool transaction = _dataCenter.Sqltr == null ? true : false;
                        if (bureau_prm != "C" && save_76)
                        {
                            // update gnms76
                            cmd.Parameters.Clear();
                            cmd.CommandText = updateGNMS76(bureau_prm, "IL", brn, app);
                            transaction = _dataCenter.Sqltr == null ? true : false;
                            int res_gnms76 = _dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                            if (res_gnms76 == -1)
                            {
                                _dataCenter.RollbackMssql();
                                _dataCenter.CloseConnectSQL();

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

                            string sql_reslCode = " SELECT g25des FROM AS400DB01.GNOD0000.gntb25 WITH (NOLOCK) WHERE g25rcd = '" + res_code + "'";
                            DataSet ds_reslCode = _dataCenter.GetDataset<DataTable>(sql_reslCode, CommandType.Text).Result.data; ;
                            if (!ilObj.check_dataset(ds_reslCode))
                            {
                                if (_dataCenter.Sqltr != null)
                                {
                                    _dataCenter.RollbackMssql();
                                    _dataCenter.CloseConnectSQL();
                                }

                                resBureau = false;
                                return;
                            }
                            DataRow dr_reslCode = ds_reslCode.Tables[0].Rows[0];
                            Connect_NoteAPI noteAPI = new Connect_NoteAPI();
                            string ErrMsgNote = "";
                            bool AddNote = iLDataSubroutine.CALL_CSSRW11("IL", dr_01["M00IDN"].ToString().Trim(), "ADD", res_code, dr_reslCode["g25des"].ToString().Trim(), "", ref ErrMsgNote, m_UserInfo.BizInit, m_UserInfo.BranchNo);
                            var resNote = noteAPI.AddNote(dr_01["M00IDN"].ToString().Trim(), "0", "ADD", res_code, dr_reslCode["g25des"].ToString().Trim(), m_UpdDate.ToString().Trim(), m_UpdTime.ToString().Trim()).Result;

                            if (!resNote.success || ErrMsgNote.Trim() != "")
                            {
                                //L_message.Text = "Cannot Save note : " + ErrMsgNote.ToString().Trim();
                                if (_dataCenter.Sqltr != null)
                                {
                                    _dataCenter.RollbackMssql();
                                    _dataCenter.CloseConnectSQL();
                                }
                                resBureau = false;
                                return;
                            }

                            string sql_csms11 = $@"SELECT IsShipTo m00dsn, ca.PostalAreaCode m11zip 
                                                    FROM CustomerDB01.CustomerInfo.CustomerGeneral cg WITH(NOLOCK)
                                                    LEFT JOIN CustomerDB01.CustomerInfo.CustomerAddress ca WITH(NOLOCK) ON(cg.ID = ca.CustID AND ca.CustRefID = 0 AND ca.IsShipTo = 'Y')
                                                    WHERE cg.CISNumber = {dr_01["M00CSN"].ToString().Trim()} ";

                            DataSet ds_csms11 = _dataCenter.GetDataset<DataTable>(sql_csms11, CommandType.Text).Result.data;
                            string chk_zip = "";
                            string chk_dsn = "";
                            if (ilObj.check_dataset(ds_csms11))
                            {
                                DataRow dr_ms11 = ds_csms11.Tables[0].Rows[0];  //
                                chk_dsn = dr_ms11["m00dsn"].ToString();
                                chk_zip = dr_ms11["m11zip"].ToString();

                            }

                            //ติดไว้ก่อน csms11wk
                            //if (chk_dsn.Trim() != "")
                            //{
                            //    string sql_csms11wk = " SELECT m11idn FROM csms11wk " +
                            //                          " WHERE m11idn = '" + dr_01["M00IDN"].ToString().Trim() + "'" +
                            //                          " AND m11flg   = 'R' " +
                            //                          " AND m11ebc   = " + app;
                            //    DataSet ds_resCSMS11wk = ilObj.RetriveAsDataSetNoConnect(sql_csms11wk);
                            //    if (ilObj.check_dataset(ds_resCSMS11wk))
                            //    {
                            //        if (_dataCenter.Sqltr != null)
                            //        {
                            //            _dataCenter.RollbackMssql();
                            //            _dataCenter.CloseConnectSQL();
                            //        }
                            //        resBureau = false;
                            //        return;
                            //    }
                            //    //DataRow dr_csms11 = ds_resCSMS11wk.Tables[0].Rows[0];
                            //    //string zipCode = dr_csms11["m11zip"].ToString().Trim();
                            //    //string dsn = dr_csms11["m00dsn"].ToString().Trim();

                            //    string insertCSMS11wk = " Insert into csms11wk " +
                            //                          " (m11brn,m11idn,m11ebc,m11csn,m11dsn,m11flg,m11zip," +
                            //                          " m11udt,m11utm,m11uus,m11uws,m11upg) " +
                            //                          " VALUES ( " +
                            //                          brn + "," +
                            //                          "'" + dr_01["M00IDN"].ToString().Trim() + "'," +
                            //                          app + "," +
                            //                          dr_01["M00CSN"].ToString().Trim() + "," +
                            //                          "'" + chk_dsn + "'," +
                            //                          "'R'," +
                            //                          chk_zip + "," +
                            //                          m_UdpD + "," +
                            //                          m_UpdTime + "," +
                            //                          "'" + m_UserInfo.Username + "'," +
                            //                          "'" + m_UserInfo.LocalClient + "'," +
                            //                          "'IL_RESEND')";

                            //    cmd.Parameters.Clear();
                            //    cmd.CommandText = insertCSMS11wk;
                            //    int res_insertCsms11 = ilObj.ExecuteNonQuery(cmd);
                            //    if (res_insertCsms11 == -1)
                            //    {
                            //        ilObj.RollbackDAL();
                            //        ilObj.CloseConnectioDAL();
                            //        resBureau = false;
                            //        return;
                            //    }
                            //}
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
                                    string sql_reslCode = " SELECT g25des FROM AS400DB01.GNOD0000.gntb25 WITH (NOLOCK) WHERE g25rcd = '" + res_code + "'";
                                    //string sql_reslCode = " SELECT g101ed FROM gntb101 WHERE g101cd = '" + res_code + "'";
                                    DataSet ds_reslCode = _dataCenter.GetDataset<DataTable>(sql_reslCode, CommandType.Text).Result.data;

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


                                string ErrMsgNote = "";
                                bool AddNote = iLDataSubroutine.CALL_CSSRW11("IL", dr_01["M00IDN"].ToString().Trim(), "NCB", res_code, desc, "", ref ErrMsgNote, m_UserInfo.BizInit, m_UserInfo.BranchNo);
                                if (!AddNote || ErrMsgNote.Trim() != "")
                                {
                                    //L_message.Text = "Cannot Save note : " + ErrMsgNote.ToString().Trim();
                                    _dataCenter.RollbackMssql();
                                    _dataCenter.CloseConnectSQL();
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
                            bool call_GNSRBLC = iLDataSubroutine.Call_GNSRBLC(dr_01["M00IDN"].ToString().Trim(), dr_01["M00CSN"].ToString().Trim(), "IL", "005", "15",
                                                                    ref Error_GNSRBLC, ref ErrorMsg_GNSRBLC, m_UserInfo.BizInit, m_UserInfo.BranchNo);
                            if (ErrorMsg_GNSRBLC.ToString().Trim() != "")
                            {
                                _dataCenter.RollbackMssql();
                                _dataCenter.CloseConnectSQL();
                                resBureau = false;
                                return;
                            }

                            string updateILMS01 = " UPDATE AS400DB01.ILOD0001.ILMS01 SET p1loca = '210', p1aprj = 'RJ', p1rsts = 'B', p1resn='BL3', " +
                                                  " p1fill = CONCAT(SUBSTRING(p1fill,1,20),'" + bureau_prm + "',SUBSTRING(p1fill,22,LEN(p1fill))), " +
                                                  " P1STDT = " + date97 + " ," +  // เพิ่มวันที่ Status date เมื่อทำการ Reject
                                                  " P1STTM = " + m_UdpT + " ," +  // เพิ่มวันที่ Status date เมื่อทำการ Reject
                                                  " p1avdt = " + date97 + ", " +  // เพิ่มวันที่ Approve date เมื่อทำการ Reject
                                                  " p1avtm = " + m_UdpT + ", " +  // เพิ่มวันที่ Approve time เมื่อทำการ Reject
                                                  " p1updt = " + m_UdpD + " ,p1uptm = " + m_UdpT + ",p1upus ='" + m_UserInfo.Username + "'," +
                                                  " p1prog  = 'IL_RESEND' , p1wsid = '" + m_UserInfo.LocalClient + "'" +
                                                  " WHERE   p1brn = " + m_UserInfo.BranchApp + " AND p1apno = " + app;

                            cmd.Parameters.Clear();
                            cmd.CommandText = updateILMS01;
                            transaction = _dataCenter.Sqltr == null ? true : false;
                            int res_updateILMS01 = _dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                            if (res_updateILMS01 == -1)
                            {
                                _dataCenter.RollbackMssql();
                                _dataCenter.CloseConnectSQL();
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

                            string updateILMS01 = " UPDATE AS400DB01.ILOD0001.ILMS01 SET " +
                                                 " p1loca = '210', p1aprj = 'RJ', p1resn =  '" + p1resn + "' ," +
                                                 " p1fill = CONCAT(SUBSTRING(p1fill,1,20),'" + bureau_prm + "',SUBSTRING(p1fill,22,LEN(p1fill))) , " +
                                                 " P1STDT = " + date97 + " ," +  // เพิ่มวันที่ Status date เมื่อทำการ Reject
                                                 " P1STTM = " + m_UdpT + " ," +
                                                 " p1avdt = " + date97 + ", " +  // เพิ่มวันที่ Approve date เมื่อทำการ Reject
                                                 " p1avtm = " + m_UdpT + ", " +  // เพิ่มวันที่ Approve time เมื่อทำการ Reject
                                                 " p1updt = " + m_UdpD + " ,p1uptm = " + m_UdpT + ",p1upus ='" + m_UserInfo.Username + "'," +
                                                 " p1prog  = 'IL_RESEND' , p1wsid = '" + m_UserInfo.LocalClient + "'" +
                                                 " WHERE   p1brn = " + m_UserInfo.BranchApp + " AND p1apno = " + app;

                            cmd.Parameters.Clear();
                            cmd.CommandText = updateILMS01;
                            transaction = _dataCenter.Sqltr == null ? true : false;
                            int res_updateILMS01 = _dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                            if (res_updateILMS01 == -1)
                            {
                                _dataCenter.RollbackMssql();
                                _dataCenter.CloseConnectSQL();
                                resBureau = false;
                                return;
                            }
                        }
                        else if (bureau_prm == "P" || bureau_prm == "N" || bureau_prm == "D" || bureau_prm == "C")
                        {
                            string updateILMS01 = " UPDATE AS400DB01.ILOD0001.ILMS01 SET p1fill = CONCAT(SUBSTRING(p1fill,1,20),'" + bureau_prm + "',SUBSTRING(p1fill,22,LEN(p1fill))) " +
                                                 " ,p1updt = " + m_UdpD + " ,p1uptm = " + m_UdpT + ",p1upus ='" + m_UserInfo.Username + "'," +
                                                 " p1prog  = 'IL_RESEND' , p1wsid = '" + m_UserInfo.LocalClient + "'" +
                                                 " WHERE   p1brn = " + m_UserInfo.BranchApp + " AND p1apno = " + app;

                            cmd.Parameters.Clear();
                            cmd.CommandText = updateILMS01;
                            transaction = _dataCenter.Sqltr == null ? true : false;
                            int res_updateILMS01 = _dataCenter.Execute(cmd.CommandText, CommandType.Text, transaction).Result.afrows;
                            if (res_updateILMS01 == -1)
                            {
                                _dataCenter.RollbackMssql();
                                _dataCenter.CloseConnectSQL();
                                resBureau = false;
                                return;
                            }

                        }
                        resBureau = true;

                        _dataCenter.CommitMssql();
                        _dataCenter.CloseConnectSQL();
                        return;

                    }

                }
                _dataCenter.RollbackMssql();
                _dataCenter.CloseConnectSQL();
                resBureau = false;
                return;
            }
            catch (Exception ex)
            {
                _dataCenter.RollbackMssql();
                _dataCenter.CloseConnectSQL();
                resBureau = false;
                return;
            }
        }
        public DataSet checkDataResendBureau(string appNo, string Branch)
        {
            DataSet ds = new DataSet();
            try
            {

                //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
                UserInfomation = m_UserInfo;


                //string sql = " SELECT m00idn,m00csn,gnb2td,m00tnm,m00tsn,m00bdt,p1appt,P1APNO,P1BRN" +
                //                 " FROM ilms01 " +
                //                 " LEFT JOIN csms00 ON(p1csno=m00csn) " +
                //                 " LEFT JOIN gnmb20 ON(m00ttl=gnb2tc) " +
                //                 " WHERE p1brn = " + Branch + " AND p1apno = " + appNo;
                string sql = $@"SELECT IDCard m00idn,CISNumber m00csn,DescriptionTHAI gnb2td,NameInTHAI m00tnm,SurnameInTHAI m00tsn,FORMAT(BirthDate,'yyyymmdd','th-TH' ) m00bdt,p1appt,P1APNO,P1BRN
                              FROM AS400DB01.ILOD0001.ILMS01 WITH (NOLOCK)
                              LEFT JOIN CustomerDB01.CustomerInfo.CustomerGeneral cg WITH (NOLOCK) ON (P1CSNO = cg.CISNumber)
                              LEFT JOIN GeneralDB01.GeneralInfo.GeneralCenter gc WITH (NOLOCK) ON (cg.TitleID = gc.ID AND Type = 'TitleID')
                              WHERE p1brn = {Branch} AND p1apno = {appNo}";

                var res = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result;
                if (res.success)
                    ds = res.data;
                _dataCenter.CloseConnectSQL();
            }
            catch (Exception ex)
            {
                _dataCenter.CloseConnectSQL();
            }
            return ds;

        }
        public DataSet getGNMS76(string biz, string appNo, string Branch)
        {
            DataSet ds = new DataSet();
            try
            {
                string sql = " SELECT g76bus FROM AS400DB01.GNOD0000.gnms76 WITH (NOLOCK) " +
                             " WHERE g76bus = '" + biz + "'" +
                             " AND g76brn = " + Branch +
                             " AND g76apn = " + appNo;

                var res = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result;
                if (res.success)
                    ds = res.data;
                _dataCenter.CloseConnectSQL();
            }
            catch (Exception ex)
            {
                _dataCenter.CloseConnectSQL();
            }
            return ds;

        }
        public string updateGNMS76(string res_bur, string biz, string brn, string app_no)
        {
            string sql = "";
            try
            {
                sql = " UPDATE AS400DB01.GNOD0000.gnms76 SET g76res = '" + res_bur + "' " +
                             " WHERE  g76bus = '" + biz + "' AND g76brn = '" + brn + "'" +
                             " AND    g76apn = " + app_no;

            }
            catch (Exception ex)
            {

            }
            return sql;
        }

    }
    public class ILDataCenterUpdateAutoPay
    {
        private ulong m_UpdDate = Convert.ToUInt32(DateTime.Now.ToString("yyMMdd"));
        private ulong m_UpdTime = Convert.ToUInt32(DateTime.Now.ToString("HHmmss"));
        private ulong m_UdpD = Convert.ToUInt32(DateTime.Now.ToString("yyyyMMdd"));
        private ulong m_UdpT = Convert.ToUInt32(DateTime.Now.ToString("HHmmss"));
        private UserInfo m_UserInfo;
        private string m_UserName;
        private string m_User;
        public string LastError;
        private string m_Wrkstn = "";
        public EB_Service.DAL.DataCenter _dataCenter;
        public ILDataCenterUpdateAutoPay(UserInfo userInfo)
        {
            m_UserInfo = userInfo;
            _dataCenter = new EB_Service.DAL.DataCenter(m_UserInfo);

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

        public DataSet get_cust_autoPay(string conType, string ID, string app, string ebc,
                                       string cust, string cont, string bank, string brn,
                                       string dateFrom, string dateTo)
        {
            DataSet ds = new DataSet();
            try
            {

                //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
                UserInfomation = m_UserInfo;
                string where = "";
                string cond = "";
                if (ID.Trim() != "")
                {
                    cond += " AND cg.IDCard = '" + ID + "'";
                }
                if (app.Trim() != "")
                {
                    cond += " AND p1apno = '" + app + "'";
                }
                if (ebc.Trim() != "")
                {
                    cond += " AND (R2CDNO = '" + ebc + "' OR M20EBC = '" + ebc + "')";
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


                string sql = $@"SELECT CONCAT(gc.DescriptionTHAI,NameInTHAI,' ',SurnameInTHAI) fname, cg.IDCard m00idn,p1apno,p1pram,p1csno,p1vdid,
                                p10nam, p1payt, p1pbcd, p1pbrn, p1paty, p1pano,FORMAT(p1cont,'0000000000000000')  p1cont,
                                p1brn, p00bst, p00doc
                                FROM AS400DB01.ILOD0001.ILMS01 WITH (NOLOCK)
                                LEFT JOIN AS400DB01.ILOD0001.ILMS00 WITH (NOLOCK) ON (p1csno = p00cis and p1pbcd = p00bnk AND p1pbrn = p00bbr AND p1pano = p00bac )
                                LEFT JOIN CustomerDB01.CustomerInfo.CustomerGeneral cg WITH (NOLOCK) ON (P1CSNO = cg.CISNumber)
                                LEFT JOIN AS400DB01.RLOD0001.RLMC02 WITH (NOLOCK) ON (R2CSNO = cg.CISNumber)
                                LEFT JOIN AS400DB01.CSOD0001.CSMS20 WITH (NOLOCK) ON (M20CSN = cg.CISNumber)
                                LEFT JOIN AS400DB01.ILOD0001.ILMS10 WITH (NOLOCK) ON (p1vdid = p10ven)
                                LEFT JOIN GeneralDB01.GeneralInfo.GeneralCenter gc WITH (NOLOCK) ON (cg.TitleID = gc.ID AND Type = 'TitleID')
                                {where}  {cond}";

                ds = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
                _dataCenter.CloseConnectSQL();
            }
            catch (Exception ex)
            {
                _dataCenter.CloseConnectSQL();
            }
            return ds;

        }

        public DataSet get_ilms00_autopay(string csn)
        {
            DataSet ds = new DataSet();
            try
            {
                //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
                UserInfomation = m_UserInfo;

                string sql = " select p00cis,p00sts from AS400DB01.ILOD0001.ILMS00 WITH (NOLOCK) " +
                             " where p00cis = " + csn + " and p00sts = 'A' ";


                ds = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
                _dataCenter.CloseConnectSQL();

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
                //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
                UserInfomation = m_UserInfo;

                string sql = " select p1rsts from AS400DB01.ILOD0001.ILMS01 WITH (NOLOCK) " +
                             " where p1apno  = " + appNo + " AND p1cont = " + cont;


                ds = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
                _dataCenter.CloseConnectSQL();

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
                //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
                UserInfomation = m_UserInfo;

                string sql = " SELECT p00cis FROM AS400DB01.ILOD0001.ILMS00 WITH (NOLOCK) " +
                             " WHERE p00cis = " + csn +
                             " AND p00bnk = '" + bankCode + "'" +
                             " AND p00bbr = '" + bankBranch + "'" +
                             " AND p00bac = '" + bankAcc + "' ";

                ds = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
                _dataCenter.CloseConnectSQL();

            }
            catch (Exception ex)
            {
            }
            return ds;
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

                        sql = " INSERT INTO AS400DB01.ILOD0001.ILMS00 " +
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
                        sql = " UPDATE AS400DB01.ILOD0001.ILMS00 SET " +
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
                        sql = " UPDATE AS400DB01.ILOD0001.ILMS00 SET " +
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
                        sql = " UPDATE AS400DB01.ILOD0001.ILMS00 SET " +
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

                    sql = " UPDATE AS400DB01.ILOD0001.ilms01 SET " +
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
                    sql = " UPDATE AS400DB01.ILOD0001.ilms01 SET " +
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
                //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
                UserInfomation = m_UserInfo;

                string sql = " SELECT SUBSTRING(gnb30f,5,2) AS LenAccNo FROM AS400DB01.GNOD0000.GNMB30 WITH (NOLOCK)  " +
                             " WHERE gnb30a = '" + bankCode + "'";


                ds = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
                _dataCenter.CloseConnectSQL();

            }
            catch (Exception ex)
            {
            }
            return ds;
        }
    }

    public class ILDataCenterClosingPayment
    {

        private ulong m_UpdDate = Convert.ToUInt32(DateTime.Now.ToString("yyMMdd"));
        private ulong m_UpdTime = Convert.ToUInt32(DateTime.Now.ToString("HHmmss"));
        private ulong m_UdpD = Convert.ToUInt32(DateTime.Now.ToString("yyyyMMdd", new CultureInfo("th-TH")));
        private ulong m_UdpT = Convert.ToUInt32(DateTime.Now.ToString("HHmmss", new CultureInfo("th-TH")));
        private UserInfo m_UserInfo;
        private string m_UserName;
        private string m_User;
        public string LastError;
        private string m_Wrkstn = "";
        public EB_Service.DAL.DataCenter _dataCenter;
        public ILDataCenterClosingPayment(UserInfo userInfo)
        {
            m_UserInfo = userInfo;
            _dataCenter = new EB_Service.DAL.DataCenter(m_UserInfo);

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
        public DataSet get_ilmd012(string cont, string app)
        {
            DataSet ds = new DataSet();
            try
            {
                //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
                UserInfomation = m_UserInfo;

                string sql = " SELECT d012tt,D012BR,d012ir,d012cr FROM AS400DB01.ILOD0001.ILMD012 WITH (NOLOCK) " +
                             " WHERE d012ap = " + app +
                             " AND   d012ct = '" + cont + "'" +
                             " ORDER BY d012sq  ";

                ds = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;


            }
            catch (Exception ex)
            {
            }
            return ds;
        }
        public DataSet get_ilms23(string cont)
        {
            DataSet ds = new DataSet();
            try
            {
                //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
                UserInfomation = m_UserInfo;

                string sql = $@"SELECT [P23CSN],[P23CNT],[P23ICD],[P23INT],[P23CRT],[P23INY],[P23CRY]
                            ,[P23TTM],[P23IN1],[P23TF2],[P23TT2],[P23IN2],[P23IN3]
                            ,[P23ATM],[P23AIN],[P23ACR],[P23LDT],[P23LDU],[P23LPC]
                             ,[P23UPD],[P23UPT],[P23USR],[P23DSP],[P23UPG],[P23DEL],[P23FLG]
                            FROM[AS400DB01].[ILOD0001].[ILMS23] WHERE P23CNT = {cont}";

                ds = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;


            }
            catch (Exception ex)
            {
            }
            return ds;
        }

        public DataSet get_ExpenseType(string appdate)
        {
            DataSet ds = new DataSet();
            try
            {
                //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
                UserInfomation = m_UserInfo;

                string sql = " SELECT G00AMT FROM AS400DB01.GNOD0000.GNTB100 WITH (NOLOCK) " +
                             " WHERE g00app = 'IL' and G00CDE = 'NCB' " +
                             " AND g00efd <= " + appdate +
                             " ORDER BY G00EFD ";

                ds = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;


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
                //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
                UserInfomation = m_UserInfo;

                string sql = " SELECT CG.IDCard as m00idn,GT.DescriptionTHAI as gnb2td,TRIM(CG.NameInTHAI) as m00tnm,TRIM(CG.SurnameInTHAI) as m00tsn,p1ltyp,p1csno,p1brn,p1apno, " +
                             " FORMAT(I1.p1cont,'0000000000000000') p1cont,p2cndt,p1term,p1pram,p1coam,p2osam,p2lndr, " +
                             " p2dutr,p2cndt,p2term,p2rang,p2ndue,p2frtm,p2infr,0,0, " +
                             " p1crcd,p1auth,p1apdt,p1vdid,p1payt,p1kusr,p1appt,p2frtm,p2duty, " +
                             " p1aprj,p1loca,p1rsts,p2sost,p2odt2,p1item,p2fdam,p2frdt,p2fram, " +
                             " p1camp,p1cmsq,p1avdt,p1purc,p1down,p1stdt,p1crcd,p1auth,p2pcbl, " +
                             " p1qty,P1FILL,FORMAT(CG.BirthDate,'yyyyMMdd','th-TH')  as BirthDate, " + 
                             " CASE " +
                             " WHEN EC.R2CDNO IS NOT NULL THEN EC.R2CDNO " +
                             " WHEN M20EBC IS NOT NULL THEN M20EBC " +
                             " ELSE NULL END as m00ebc " +
                             " FROM AS400DB01.ILOD0001.ILMS01   I1 WITH (NOLOCK) " +
                             " LEFT JOIN CustomerDB01.CustomerInfo.CustomerGeneral CG WITH (NOLOCK) on(I1.p1csno=CG.CISNumber) " +
                             " left join GeneralDB01.GeneralInfo.GeneralCenter GT WITH (NOLOCK) on(CG.TitleID = GT.ID) " +
                             " LEFT JOIN AS400DB01.RLOD0001.RLMC02 EC WITH (NOLOCK) on (EC.R2CSNO = CG.CISNumber) " +
                             " LEFT JOIN AS400DB01.CSOD0001.CSMS20 WITH (NOLOCK) on (M20CSN = CG.CISNumber) " +
                             " LEFT JOIN AS400DB01.ILOD0001.ilms02 I2 WITH (NOLOCK) on (I1.p1csno = I2.p2csno and I1.p1apno=I2.p2apno) " +
                             " WHERE p1apno = " + app +
                             " AND  p1brn = " + brn;

                ds = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;


            }
            catch (Exception ex)
            {
                Utility.WriteLogString(ex.ToString());
            }
            return ds;
        }

        public DataSet get_ILClosingPayment(string brn, string ID, string contractNo, string appNo, string csn)
        {
            DataSet ds = new DataSet();
            try
            {
                //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
                UserInfomation = m_UserInfo;

                string StrWhere = "";
                if (ID != "")
                {
                    StrWhere = " WHERE CG.IDCard = '" + ID + "' ";
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

                string sql = " SELECT CG.IDCard as m00idn,T.DescriptionTHAI as gnb2td,TRIM(CG.NameInTHAI) as m00tnm,TRIM(CG.SurnameInTHAI) as m00tsn " +
                             " ,TRIM(T.DescriptionTHAI)+ ' '+ TRIM(CG.NameInTHAI)+ ' '+ TRIM(CG.SurnameInTHAI) name_,p1ltyp,p1csno,p1brn,p1apno " +
                             " ,FORMAT(I1.p1cont,'0000000000000000') p1cont,p2cndt,p1term,p1pram,p1coam,p2osam,p2lndr " +
                             " ,p2dutr,p2cndt,p2term,p2rang,p2ndue,p2frtm,p2infr,0,0,p1crcd,p1auth,p1apdt,p1vdid,P10TNM,p1payt,p1kusr,p1appt,p2frtm,p2duty " +
                             " ,p1aprj,p1loca,p1rsts,p2sost,p2odt2,p1item,T44DES,p2fdam,p2frdt,p2fram  " +
                             " ,p1camp,p1cmsq,p1avdt,p1purc,p1down,p1stdt,p1crcd,p1auth,p2pcbl " +
                             " ,p1qty ,SUBSTRING(CAST(p1apdt AS nvarchar),7,2)+'/'+ SUBSTRING(CAST(p1apdt AS nvarchar),5,2)+'/'+SUBSTRING(CAST(p1apdt AS nvarchar),1,4) appDate,TRIM(p1payt)+' : '+TRIM(gt48ed) recieve   " +
                             " FROM AS400DB01.ILOD0001.ILMS01 I1 WITH (NOLOCK) " +
                             " LEFT JOIN CustomerDB01.CustomerInfo.CustomerGeneral CG WITH (NOLOCK) on(I1.p1csno=CG.CISNumber) " +
                             " LEFT JOIN GeneralDB01.GeneralInfo.GeneralCenter T  WITH (NOLOCK) on CG.TitleID = T.ID and T.Type = 'TitleID'   " +
                             " INNER JOIN AS400DB01.ILOD0001.ilms02 I2 WITH (NOLOCK) on(I1.p1csno=I2.p2csno and I1.p1apno=I2.p2apno)   " +
                             " LEFT JOIN AS400DB01.ILOD0001.ilms10 on(p1vdid = P10VEN)  " +
                             " LEFT JOIN AS400DB01.ILOD0001.iltb44 on(p1item = T44ITM)  " +
                             " LEFT JOIN AS400DB01.GNOD0000.GNTB48 on(p1payt = GT48TC) ";
                sql += StrWhere;
                ds = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;


            }
            catch (Exception ex)
            {
                Utility.WriteLogString(ex.ToString());
            }
            return ds;
        }
    }

    public class ILDataCenterCancelUnsign
    {

        private ulong m_UpdDate = Convert.ToUInt32(DateTime.Now.ToString("yyMMdd"));
        private ulong m_UpdTime = Convert.ToUInt32(DateTime.Now.ToString("HHmmss"));
        private ulong m_UdpD = Convert.ToUInt32(DateTime.Now.ToString("yyyyMMdd", new CultureInfo("th-TH")));
        private ulong m_UdpT = Convert.ToUInt32(DateTime.Now.ToString("HHmmss", new CultureInfo("th-TH")));
        private UserInfo m_UserInfo;
        private string m_UserName;
        private string m_User;
        public string LastError;
        private string m_Wrkstn = "";
        public EB_Service.DAL.DataCenter _dataCenter;
        public ILDataCenterCancelUnsign(UserInfo userInfo)
        {
            m_UserInfo = userInfo;
            _dataCenter = new EB_Service.DAL.DataCenter(m_UserInfo);

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
        public DataSet get_cust_cancel_unsign(string s_search, string Branch, string search_by)
        {
            DataSet ds = new DataSet();
            try
            {
                //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
                UserInfomation = m_UserInfo;
                string where = " WHERE p2brn= " + Branch + " AND ";
                if (search_by == "1")
                {
                    where += " p2apno = " + s_search;
                }
                else if (search_by == "2")
                {
                    where += " p2cont = " + s_search;

                }

                string sql = " SELECT CG.IDCard as m00idn,TRIM(CG.NameInTHAI) as m00tnm,TRIM(CG.SurnameInTHAI) as m00tsn, " +
                             " S.Code as Gender , S.ShortName as  m00sex,FORMAT(CG.BirthDate,'dd/MM/yyyy','th-TH') as m00bdt, " +
                             " p2brn,p2cont,p2csno,p2apno,FORMAT(p2cont,'0000000000000000')as p2cont1,p3crlm,p3pcam,p2pcam, " +
                             " p2toam,p2item,p2qty,t44des,p2bkdt, " +
                             " CASE " +
                             " WHEN (SELECT TOP(1) R2CDNO FROM AS400DB01.RLOD0001.RLMC02 WITH (NOLOCK) WHERE cg.CISNumber = R2CSNO)  IS NOT NULL THEN " +
                             " COALESCE ((SELECT TOP (1) R2CDNO FROM AS400DB01.RLOD0001.RLMC02 WITH (NOLOCK) WHERE R2CSNO = cg.CISNumber), 0) " + 
                             " WHEN (SELECT TOP(1) M20EBC FROM AS400DB01.CSOD0001.CSMS20 G WITH (NOLOCK) WHERE cg.CISNumber = M20CSN)  IS NOT NULL THEN " +
                             " COALESCE ((SELECT TOP (1) M20EBC FROM AS400DB01.CSOD0001.CSMS20 G WITH (NOLOCK) WHERE cg.CISNumber = M20CSN), 0) " +
                             " ELSE NULL END AS m00ebc " +
                             " FROM AS400DB01.ILOD0001.ilms02 I2 WITH (NOLOCK) " +
                             " LEFT JOIN CustomerDB01.CustomerInfo.CustomerGeneral CG WITH (NOLOCK) on(I2.p2csno=CG.CISNumber) " +
                             " LEFT JOIN GeneralDB01.GeneralInfo.GeneralCenter T  WITH (NOLOCK) on CG.TitleID = T.ID and T.Type = 'TitleID'   " +
                             " LEFT JOIN GeneralDB01.GeneralInfo.GeneralCenter S  WITH (NOLOCK) on CG.SexID = S.ID and S.Type = 'SexID' " +
                             " LEFT JOIN AS400DB01.ILOD0001.ilms03 WITH (NOLOCK) on(p2csno=p3csno) " +
                             " LEFT JOIN AS400DB01.ILOD0001.iltb44 WITH (NOLOCK) on(p2item=t44itm) " +
                 where + " AND  p2loca in('250','255') AND p2del = '' ";
                ds = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;


            }
            catch (Exception ex)
            {
            }
            return ds;
        }

        public DataSet get_csms032(string csn, string brn, string appNo)
        {
            DataSet ds = new DataSet();
            try
            {
                //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
                UserInfomation = m_UserInfo;

                string sql = " SELECT d3osal FROM AS400DB01.CSOD0001.CSMS032 WITH (NOLOCK)  JOIN AS400DB01.CSOD0001.csms13 WITH (NOLOCK)  " +
                             " ON  d3csno = m13csn " +
                             " WHERE d3csno=" + csn + " AND d3flag = 'C' " +
                             " AND m13app = 'IL' " +
                             " AND m13brn= " + brn +
                             " AND m13apn = " + appNo +
                             " AND SUBSTRING(m13fil,20,1) = 'C' ";

                ds = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;


            }
            catch (Exception ex)
            {
            }
            return ds;
        }

        public DataSet get_csms20(string csn)
        {
            DataSet ds = new DataSet();
            try
            {
                //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
                UserInfomation = m_UserInfo;

                string sql = "  SELECT m20avi FROM AS400DB01.CSOD0001.csms20 WITH (NOLOCK) WHERE m20csn = " + csn;

                ds = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;

            }
            catch (Exception ex)
            {
            }
            return ds;
        }

        public DataSet get_ilms03(string csn)
        {
            DataSet ds = new DataSet();
            try
            {
                //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
                UserInfomation = m_UserInfo;

                string sql = "  SELECT p3pcam FROM AS400DB01.ILOD0001.ilms03 WITH (NOLOCK) WHERE p3csno = " + csn;

                ds = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;

            }
            catch (Exception ex)
            {
            }
            return ds;
        }

        public string updateCSMS20(string M20AVI, string csn, string M20UPG, string userName)
        {
            string sql = "";
            try
            {
                sql = "  UPDATE AS400DB01.CSOD0001.CSMS20 SET " +
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

        public string UpdateILMS03(string P3PCAM, string csn, string P3PGMN, string UserName, string wrkstn)
        {
            string sql = "";
            try
            {
                sql = " UPDATE AS400DB01.ILOD0001.ILMS03 SET " +
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
                sql = " UPDATE AS400DB01.ILOD0001.ILMS01 SET " +
                       " p1loca = '300', " +
                       " p1rsts = 'X' ," +
                       " p1fill = '" + date_User + "' + SUBSTRING(p1fill,LEN('" + date_User + "')+1,LEN(p1fill)), " +
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

        public string updateILMS02(string brn, string cont, string appNo, string p2prog)
        {
            string sql = "";
            try
            {
                sql = " UPDATE AS400DB01.ILOD0001.ilms02 SET " +
                       " p2loca = '300' ," +
                       " p2del  = 'X', " +
                       " p2lmvd = " + m_UdpD + "," +
                       " p2updt = " + m_UdpD + "," +
                       " p2uptm = " + m_UpdTime + "," +
                       " p2user = '" + m_UserName + "'," +
                       " p2prog = '" + p2prog + "'," +
                       " p2ddsp = '" + m_Wrkstn + "'" +
                       " WHERE  p2brn = " + brn +
                       " AND    p2cont = " + cont +
                       " AND    p2apno = " + appNo;
            }
            catch (Exception ex)
            {

            }
            return sql;
        }
    }
    public class ILDataCenterMssqlReport
    {
        private DateTimeFormatInfo m_DThai = new CultureInfo("th-TH", false).DateTimeFormat;
        private ulong m_UpdDate = Convert.ToUInt32(DateTime.Now.ToString("yyMMdd"));
        private ulong m_UpdTime = Convert.ToUInt32(DateTime.Now.ToString("HHmmss"));
        private ulong m_UdpD = Convert.ToUInt32(DateTime.Now.ToString("yyyyMMdd"));
        private ulong m_UdpT = Convert.ToUInt32(DateTime.Now.ToString("HHmmss"));
        private UserInfo m_UserInfo;
        private string m_UserName;
        private string m_User;
        public string LastError;
        private string m_Wrkstn = "";
        public EB_Service.DAL.DataCenter _dataCenter;
        public ILDataCenterMssqlReport(UserInfo userInfo)
        {
            m_UserInfo = userInfo;
            _dataCenter = new EB_Service.DAL.DataCenter(m_UserInfo);

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

        public DataSet RP_AutoPay(string pay_type, string dateFrom, string dateTo, string debit_bank, string brn)
        {
            DataSet ds = new DataSet();
            //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_UserInfo;
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


                    sql = $@"SELECT p00std, p00bst, p00doc,
                            CASE WHEN R2CDNO IS NOT NULL
                            THEN CONCAT(SUBSTRING(CAST(FORMAT(R2CDNO,'0000000000000000') AS nvarchar),1,4),'-',SUBSTRING(CAST(FORMAT(R2CDNO,'0000000000000000') AS nvarchar),5,4),'-',SUBSTRING(CAST(FORMAT(R2CDNO,'0000000000000000') AS nvarchar),9,4),'-',SUBSTRING(CAST(FORMAT(R2CDNO,'0000000000000000') AS nvarchar),13,4))
                            WHEN M20EBC IS NOT NULL
                            THEN CONCAT(SUBSTRING(CAST(FORMAT(M20EBC,'0000000000000000') AS nvarchar),1,4),'-',SUBSTRING(CAST(FORMAT(M20EBC,'0000000000000000') AS nvarchar),5,4),'-',SUBSTRING(CAST(FORMAT(M20EBC,'0000000000000000') AS nvarchar),9,4),'-',SUBSTRING(CAST(FORMAT(M20EBC,'0000000000000000') AS nvarchar),13,4))
                            ELSE '0'
                            END AS m00ebc1,
                            CONCAT(TRIM(gc.DescriptionTHAI), TRIM(cg.NameInTHAI), ' ', TRIM(cg.SurnameInTHAI)) name_,
                            FORMAT(p1cont, '0000000000000000') p1cont1, p1pbcd, p1pbrn, p1pano, 
                            p00aty, gc2.DescriptionENG gn13ed
                            FROM AS400DB01.ILOD0001.ILMS00 WITH(NOLOCK)
                            LEFT JOIN AS400DB01.ILOD0001.ILMS01 WITH(NOLOCK) ON(p00cnt = p1cont AND p00cis = p1csno and p00bnk = p1pbcd and  p00bbr = p1pbrn and p00bac = p1pano)
                            LEFT JOIN CustomerDB01.CustomerInfo.CustomerGeneral cg WITH(NOLOCK) ON(P00CIS = cg.CISNumber)
                            LEFT JOIN GeneralDB01.GeneralInfo.GeneralCenter gc WITH(NOLOCK) ON(cg.TitleID = gc.ID AND gc.Type = 'TitleID')
                            LEFT JOIN GeneralDB01.GeneralInfo.GeneralCenter gc2 WITH(NOLOCK) ON(gc2.Code = p00aty AND gc2.Type = 'FinancialCodeID')
                            LEFT JOIN AS400DB01.RLOD0001.RLMC02 WITH (NOLOCK) ON (R2CSNO = cg.CISNumber)
                            LEFT JOIN AS400DB01.CSOD0001.CSMS20 WITH (NOLOCK) ON (M20CSN = cg.CISNumber)
                            {where}";
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

                    sql = $@"SELECT p00std, p00bst, p00doc,
                            CASE WHEN R2CDNO IS NOT NULL
                            THEN CONCAT(SUBSTRING(CAST(FORMAT(R2CDNO,'0000000000000000') AS nvarchar),1,4),'-',SUBSTRING(CAST(FORMAT(R2CDNO,'0000000000000000') AS nvarchar),5,4),'-',SUBSTRING(CAST(FORMAT(R2CDNO,'0000000000000000') AS nvarchar),9,4),'-',SUBSTRING(CAST(FORMAT(R2CDNO,'0000000000000000') AS nvarchar),13,4))
                            WHEN M20EBC IS NOT NULL                            
                            THEN CONCAT(SUBSTRING(CAST(FORMAT(M20EBC,'0000000000000000') AS nvarchar),1,4),'-',SUBSTRING(CAST(FORMAT(M20EBC,'0000000000000000') AS nvarchar),5,4),'-',SUBSTRING(CAST(FORMAT(M20EBC,'0000000000000000') AS nvarchar),9,4),'-',SUBSTRING(CAST(FORMAT(M20EBC,'0000000000000000') AS nvarchar),13,4))
                            ELSE '0'
                            END AS m00ebc1,
                            CONCAT(TRIM(gc.DescriptionTHAI),TRIM(cg.NameInTHAI),' ',TRIM(cg.SurnameInTHAI)) name_,
                            FORMAT(p1cont,'0000000000000000') p1cont1, p1pbcd, p1pbrn, p1pano, 
                            p00aty, gc2.DescriptionENG gn13ed
                            FROM AS400DB01.ILOD0001.ILMS00 WITH (NOLOCK)
                            LEFT JOIN AS400DB01.ILOD0001.ILMS01 WITH (NOLOCK) ON ( p00cnt = p1cont AND p00cis = p1csno and p1pbcd = p00bnk and p1pbrn = p00bbr and p1pano = p00bac) 
                            LEFT JOIN CustomerDB01.CustomerInfo.CustomerGeneral cg WITH (NOLOCK) ON (P1CSNO = cg.CISNumber)
                            LEFT JOIN GeneralDB01.GeneralInfo.GeneralCenter gc WITH (NOLOCK) ON (cg.TitleID = gc.ID AND gc.Type = 'TitleID')
                            LEFT JOIN GeneralDB01.GeneralInfo.GeneralCenter gc2 WITH (NOLOCK) ON (gc2.Code = p00aty AND gc2.Type = 'FinancialCodeID')                            
                            LEFT JOIN AS400DB01.RLOD0001.RLMC02 WITH (NOLOCK) ON (R2CSNO = cg.CISNumber)
                            LEFT JOIN AS400DB01.CSOD0001.CSMS20 WITH (NOLOCK) ON (M20CSN = cg.CISNumber)
                            {where}";



                }

                ds = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
                _dataCenter.CloseConnectSQL();

            }
            catch (Exception ex)
            {
                _dataCenter.CloseConnectSQL();
            }

            return ds;
        }
        public ArrayList RP_Param(string m_program, string dateFrom = "", string dateTo = "", string year = "")
        {
            //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_UserInfo;
            string company = "";
            ILDataSubroutine dataSubroutine = new ILDataSubroutine(m_UserInfo);
            try
            {
                dataSubroutine.CALL_GNSRCONM("E", "F", "0", ref company);
            }
            catch (Exception ex)
            {
            }
            ArrayList aryParm = new ArrayList();

            aryParm.Add(m_UserInfo.Username.ToUpper().Trim());
            aryParm.Add(DateTime.Now.ToString("dd/MM/yyyy", m_DThai)); //  date
            aryParm.Add(DateTime.Now.ToString("HH:mm:ss")); // time
            aryParm.Add(m_UserInfo.LocalClient.ToString()); // work station
            aryParm.Add(m_program);  // program
            aryParm.Add(company); //  company
            aryParm.Add(m_UserInfo.BranchDescEN.ToString()); // branch
            aryParm.Add(dateFrom); // month1
            aryParm.Add(dateTo); // month2
            aryParm.Add(year); // year
            aryParm.Add(m_UserInfo.BranchApp.ToString()); // branch
            return aryParm;
        }
        public DataSet RP_DailyApplication_Analysis(string WKBRN, string WKYMD, string WKOPTS)
        {
            DataSet ds = new DataSet();
            ILDataCenter ilobj = new ILDataCenter();
            //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_UserInfo;
            try
            {
                //ติด CALL_ILR033C2
                bool Rescall = ilobj.CALL_ILR033C2(WKBRN, WKYMD, WKOPTS, m_UserInfo.BizInit, m_UserInfo.BranchNo);
                if (Rescall)
                {

                    string sql_198 = " SELECT  substr(F198,2,198) F198 ,  " +
                                     " CASE when (substr(F198,1,1) = '1') THEN '1' ELSE '0' END  newpage " +
                                     " FROM qtemp/text198 ";

                    ds = ilobj.RetriveAsDataSet(sql_198);

                }
                ilobj.CloseConnectioDAL();

            }
            catch (Exception ex)
            {
                ilobj.CloseConnectioDAL();
            }

            return ds;
        }
        public DataSet RP_Document_Debit(string dateFrom, string dateTo, string debit_bank, string brn)
        {
            DataSet ds = new DataSet();
            //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_UserInfo;
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


                string sql = $@"SELECT p00bnk,p00std,p1pano,'' as bank_,
                                CASE WHEN R2CDNO IS NOT NULL THEN
                                SUBSTRING(RIGHT('00' + CAST(R2CDNO AS nvarchar),16),1,4)+'-'+SUBSTRING(RIGHT('00' + CAST(R2CDNO AS nvarchar),16),5,4)+'-'+SUBSTRING(RIGHT('00' + CAST(R2CDNO AS nvarchar),16),9,4)+'-'+SUBSTRING(RIGHT('00' + CAST(R2CDNO AS nvarchar),16),13,4)
                                WHEN M20EBC IS NOT NULL THEN
                                SUBSTRING(RIGHT('00' + CAST(M20EBC AS nvarchar),16),1,4)+'-'+SUBSTRING(RIGHT('00' + CAST(M20EBC AS nvarchar),16),5,4)+'-'+SUBSTRING(RIGHT('00' + CAST(M20EBC AS nvarchar),16),9,4)+'-'+SUBSTRING(RIGHT('00' + CAST(M20EBC AS nvarchar),16),13,4)  
                                ELSE '0' END AS m00ebc1, 
                                TRIM(gc.DescriptionTHAI)+TRIM(NameInTHAI)+' '+TRIM(SurnameInTHAI) name_ ,RIGHT('00' + p1cont, 16 ) p1cont1,p1pbrn,bb.BankNameTHAI gnb31d, ' ' as remark_
                                FROM AS400DB01.ILOD0001.ilms00 WITH (NOLOCK)
                                LEFT JOIN AS400DB01.ILOD0001.ilms01 WITH (NOLOCK) ON (P00CNT = P1CONT AND p00cis = p1csno AND p00bnk=p1pbcd AND p00bbr = p1pbrn and p00bac = p1pano)  
                                LEFT JOIN CustomerDB01.CustomerInfo.CustomerGeneral cg WITH (NOLOCK) ON (p00cis = cg.CISNumber)
                                LEFT JOIN AS400DB01.RLOD0001.RLMC02 WITH (NOLOCK) ON (R2CSNO = cg.CISNumber)
                                LEFT JOIN AS400DB01.CSOD0001.CSMS20 WITH (NOLOCK) ON (M20CSN = cg.CISNumber)
                                LEFT JOIN GeneralDB01.GeneralInfo.GeneralCenter gc WITH (NOLOCK) ON (TitleID = gc.ID AND Type = 'TitleID')
                                LEFT JOIN GeneralDB01.GeneralInfo.Bank bk WITH (NOLOCK) ON (p1pbcd = bk.BankCode)
                                LEFT JOIN GeneralDB01.GeneralInfo.BankBranch bb WITH (NOLOCK) ON(bk.ID = bb.BankID AND p1pbrn = BankBranchCode AND  TRIM(p1pbrn) <> '' )
                                {where}";



                ds = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
                _dataCenter.CloseConnectSQL();

            }
            catch (Exception ex)
            {
                _dataCenter.CloseConnectSQL();
            }

            return ds;
        }
        public DataSet getFAXCampaign(string prmType, ref string errmsg)
        {
            DataSet ds = new DataSet();
            string strWhere = "";
            try
            {
                //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
                UserInfomation = m_UserInfo;

                if ((prmType == "5") || (prmType == "6") || (prmType == "7"))
                {
                    strWhere = " and c01brn = 701 ";
                }

                string sql = "Select c01cmp, c01cnm " +
                             "from AS400DB01.ILOD0001.ilcp01 WITH (NOLOCK) " +
                             "where c01sty = '" + prmType + "' and c01edt >= " + m_UdpD +
                             strWhere +
                             " order by c01cmp";

                ds = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
                _dataCenter.CloseConnectSQL();
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                ds = null;
            }

            return ds;

        }
        public DataSet getFAXProduct(string campaignCode, string vendorCode, ref string errmsg)
        {
            DataSet ds = new DataSet();
            string sql = $@"
                    SELECT C01CMP AS CAMPAIGNCODE, C01CNM AS CAMPAIGNNAME, C01SDT CAMPAIGN_START, C01EDT as CAMPAIGN_END, C01CLD AS CAMPAIGN_ENDBILL
                    , T44ITM AS PROITEM ,T41DES AS PRODUCT,T42DES AS BRAND,T43DES as MODEL ,C02INR AS INT ,C02CRR AS CRU,C02FMT PERIODFROM,C02TOT as PERIODTO
                    ,CASE WHEN C07MAX <=0 THEN C07MIN ELSE C07MAX END AS PRICE, P10TNM AS VENDOR
                    FROM AS400DB01.ILOD0001.ILCP07 WITH (NOLOCK)
                    LEFT JOIN AS400DB01.ILOD0001.ILMS10 WITH (NOLOCK) ON P10VEN = {vendorCode}
                    LEFT JOIN AS400DB01.ILOD0001.ILCP01 WITH (NOLOCK) ON C01CMP = C07CMP
                    LEFT JOIN AS400DB01.ILOD0001.ILCP02 WITH (NOLOCK) ON C01CMP = C02CMP
                    LEFT JOIN AS400DB01.ILOD0001.ILTB44 WITH (NOLOCK) ON C07LNT = T44LTY AND CASE WHEN C07PIT = 0 THEN T44ITM ELSE C07PIT END = T44ITM
                    LEFT JOIN AS400DB01.ILOD0001.ILTB41 WITH (NOLOCK) ON T41LTY = T44LTY AND T41TYP = T44TYP AND T41COD = T44COD
                    LEFT JOIN AS400DB01.ILOD0001.ILTB42 WITH (NOLOCK) ON T42BRD = T44BRD
                    LEFT JOIN AS400DB01.ILOD0001.ILTB43 WITH (NOLOCK) ON T43LTY = T44LTY AND T43TYP = T44TYP AND T43BRD = T42BRD AND T43COD = T44COD AND T43MDL = T44MDL
                    WHERE C01CMP = '{campaignCode}' AND T41DEL = '' AND T42DEL = '' AND T43DEL = '' AND T44DEL = '' 
            ";
            try
            {
                //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
                UserInfomation = m_UserInfo;
                ds = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
                _dataCenter.CloseConnectSQL();
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
                //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
                UserInfomation = m_UserInfo;

                if ((prmType == "5") || (prmType == "6") || (prmType == "7"))
                {
                    strWhere = " and P10BRN = 701 ";
                }

                sql = "Select c08ven, p10tnm " +
                      "from AS400DB01.ILOD0001.ilcp08 WITH (NOLOCK) " +
                      "left join AS400DB01.ILOD0001.ilms10 WITH (NOLOCK) on c08ven = P10VEN " +
                      "where c08cmp = " + prmCampaign + strWhere +
                      " and c08ven = p10ven " +
                      "order by c08ven";

                ds = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
                _dataCenter.CloseConnectSQL();
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                ds = null;
            }

            return ds;
        }
        public DataSet Get_ILFB01()
        {
            DataSet ds = new DataSet();
            //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_UserInfo;
            try
            {
                string sql = @"
                SELECT F01RST, F01VEN, F01NMT, F01TBL, F01AMP, F01PRO, F01FAX, F01NAM, F01GNM, F01CIS,
                CAST(CAST(F01EBC AS BIGINT)AS nvarchar) F01EBC, CAST(CAST(F01CNT AS BIGINT)AS nvarchar) F01CNT, F01APT, F01PRD, F01AMO, F01CBU, F01PUR, F01DOW, 
                F01COA, F01DUT, F01PAY, F01BRN, F01APP, F01PRA, F01INA, F01CRA, F01FPA, F01FIA, F01FCA, F01FDU, F01FBU,      
                F01FOA, P90NAME, P90SURN,F02FRM,F02TO,F02IRT,F02CR,F02PA
                FROM AS400DB01.ILOD0001.ILFB01 WITH (NOLOCK)
                LEFT JOIN AS400DB01.ILOD0001.ILMS90 WITH (NOLOCK) ON(F01BRN=P90BRN AND F01APP=P90APNO) 
                LEFT JOIN AS400DB01.ILOD0001.ILFB02 WITH (NOLOCK) ON(F01BRN=F02BRN AND F01APP=F02APP) 
                WHERE F01RST = '0' 
                ORDER BY F01VEN 
                ";
                //F01VEN = '010103520029'
                ds = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
                ds.Tables[0].TableName = "approveTable";
                sql = @"
                SELECT F01RST, F01VEN, F01NMT, F01TBL, F01AMP, F01PRO, F01FAX, F01NAM, F01APP, 
                F01APT, F01PRD, F01AMO, F01PUR, F01DOW, P90NAME, P90SURN
                FROM AS400DB01.ILOD0001.ILFB01 WITH (NOLOCK) 
                LEFT JOIN AS400DB01.ILOD0001.ILMS90 WITH (NOLOCK) ON(F01BRN=P90BRN AND F01APP=P90APNO) 
                WHERE F01RST = '1' 
                ORDER BY F01VEN
                ";
                DataSet dsReject = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
                dsReject.Tables[0].TableName = "rejectTable";
                ds.Tables.Add(dsReject.Tables[0].Copy());

                sql = @"
                SELECT F01RST, F01VEN, F01NMT, F01TBL, F01AMP, F01PRO, F01FAX, F01NAM, F01APP,
                F01APT, F01PRD, F01AMO, F01PUR, F01DOW, P90NAME, P90SURN
                FROM AS400DB01.ILOD0001.ILFB01 WITH (NOLOCK) 
                LEFT JOIN AS400DB01.ILOD0001.ILMS90 WITH (NOLOCK) ON(F01BRN=P90BRN AND F01APP=P90APNO)
                WHERE F01RST = '2' 
                ORDER BY F01VEN
                ";
                DataSet dsCancel = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
                dsCancel.Tables[0].TableName = "cancelTable";
                ds.Tables.Add(dsCancel.Tables[0].Copy());
                //sql = @"
                //SELECT SUM(CASE WHEN F01RST = '0' THEN 1 ELSE 0 END) AS APPROVE
                //        ,SUM(CASE WHEN F01RST = '1' THEN 1 ELSE 0 END) AS REJECT
                //        ,SUM(CASE WHEN F01RST = '2' THEN 1 ELSE 0 END) AS CANCEL
                //        ,F01VEN,F01NMT, F01FAX
                //FROM AS400DB01.ILOD0001.ILFB01 WITH (NOLOCK) 
                //LEFT JOIN AS400DB01.ILOD0001.ILMS90 WITH (NOLOCK) ON(F01BRN=P90BRN AND F01APP=P90APNO) 
                //WHERE F01RST IN('0','1','2')
                //GROUP BY F01VEN,F01NMT, F01FAX
                //ORDER BY F01VEN
                //";
                sql = @"SELECT  summary.*, '0' + TRY_CAST(P10FX1 AS NVARCHAR(MAX)) AS F01FAX 
                        FROM AS400DB01.ILOD0001.ilms10 WITH (NOLOCK)
                        JOIN 
                        (SELECT SUM(CASE WHEN F01RST = '0' THEN 1 ELSE 0 END) AS APPROVE
                        ,SUM(CASE WHEN F01RST = '1' THEN 1 ELSE 0 END) AS REJECT
                        ,SUM(CASE WHEN F01RST = '2' THEN 1 ELSE 0 END) AS CANCEL
                        ,F01VEN,F01NMT
                        FROM AS400DB01.ILOD0001.ILFB01 WITH (NOLOCK) 
                        LEFT JOIN AS400DB01.ILOD0001.ILMS90 WITH (NOLOCK) ON(F01BRN=P90BRN AND F01APP=P90APNO) 
                        WHERE F01RST IN('0','1','2')
                        GROUP BY F01VEN,F01NMT) AS summary ON (P10VEN = F01VEN )";
                DataSet dsSummary = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
                dsSummary.Tables[0].TableName = "summaryTable";
                ds.Tables.Add(dsSummary.Tables[0].Copy());
                _dataCenter.CloseConnectSQL();
            }
            catch (Exception ex) { _dataCenter.CloseConnectSQL(); }
            return ds;
        }
        public DataSet RP_PrintSticker(string WSBRN, string WSSDTE, string WSEDTE, string WSTYPE, string WSOPT1, string WSOPT2, string WSDFMT, ref DataSet ds_tb28)
        {
            DataSet ds = new DataSet();
            //ILDataCenter ilobj = new ILDataCenter();
            //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_UserInfo;
            try
            {
                string sql = $@"EXEC [AS400DB01].[ILOD0001].[sp_GetPrintStickerReport]
		                    N'{WSBRN.ToString()}',
		                    N'{WSSDTE.ToString()}',
		                    N'{WSEDTE.ToString()}',
		                    N'{WSTYPE.ToString()}'";
                ds = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
                //if (WSOPT2.Trim() == "X")
                //{
                //}
                if (WSOPT1.Trim() == "X")
                {
                    DataTable dt28 = new DataTable();
                    DataRow dr28;
                    dt28.Columns.Add("CONT");
                    foreach(DataRow dr in ds.Tables[0].Rows)
                    {
                        dr28 = dt28.NewRow();
                        dr28["CONT"] = dr["CONT"].ToString();
                        dt28.Rows.Add(dr28);
                    }
                    // string sql_iltb28 = " select CONT from  AS400DB01.ILOD0001.iltb28 WITH (NOLOCK)";
                    ds_tb28.Tables.Add(dt28);
                }
                //     bool Rescall = ilobj.check_dataset(ds);
                //     //bool Rescall = CALL_ILC021CL1(WSBRN, WSSDTE, WSEDTE, WSTYPE, WSOPT1, WSOPT2, WSDFMT, m_UserInfo.BizInit, m_UserInfo.BranchNo);
                //     if (Rescall)
                //     {
                //         if (WSOPT2.Trim() == "X")
                //         {
                //             sql = $@"SELECT   '  {WSTYPE}  '+'-'+SUBSTRING(CAST(M40BDT AS NVARCHAR),3,LEN(M40BDT)-2) + '-' + M40CON CONT ,
                //                           CASE  WHEN R2CDNO <> 0 THEN SUBSTRING(FORMAT(CAST(R2CDNO AS NVARCHAR), '0000000000000000'),1,1)+'-'+SUBSTRING(FORMAT(CAST(R2CDNO AS NVARCHAR), '0000000000000000'),2,3)+'-'+SUBSTRING(FORMAT(CAST(R2CDNO AS NVARCHAR), '0000000000000000'),5,LEN(FORMAT(CAST(R2CDNO AS NVARCHAR), '0000000000000000'))-4) ELSE '' END EBC  ,  
                //                           CASE  WHEN  D45RDT <> 0 THEN SUBSTRING(CAST(D45RDT AS NVARCHAR),7,2)+'/'+SUBSTRING(CAST(D45RDT AS NVARCHAR),5,2)+'/'+SUBSTRING(CAST(D45RDT AS NVARCHAR),1,4) ELSE '' END FOL_DATE ,  
                //                           CASE  WHEN  P2TMDT <> 0 THEN SUBSTRING(CAST(P2TMDT AS NVARCHAR),7,2)+'/'+SUBSTRING(CAST(P2TMDT AS NVARCHAR),5,2)+'/'+SUBSTRING(CAST(P2TMDT AS NVARCHAR),1,4) ELSE '' END TER_DATE ,  
                //                           TRIM(gc.DescriptionTHAI)+TRIM(NameInTHAI)+'  ' +TRIM(SurnameInTHAI) CUSTNM ,  
                //                           SUBSTRING(CAST(M40BDT AS NVARCHAR),7,2)+'/'+SUBSTRING(CAST(M40BDT AS NVARCHAR),5,2)+'/'+SUBSTRING(CAST(M40BDT AS NVARCHAR),1,4) BOOKDATE  
                //                           FROM AS400DB01.ILOD0001.ILMS40 WITH (NOLOCK)
                //                           JOIN  AS400DB01.ILOD0001.ILMS02 WITH (NOLOCK) ON (M40CON = P2CONT AND  M40BRN = P2BRN)  
                //                           LEFT JOIN AS400DB01.ILOD0001.ILMD45 WITH (NOLOCK) ON (M40CON = D45CNT AND  M40BRN = D45BRN)  
                //                           LEFT JOIN CustomerDB01.CustomerInfo.CustomerGeneral cg WITH (NOLOCK) ON (M40CSN = CISNumber)
                //LEFT JOIN AS400DB01.RLOD0001.RLMC02 WITH (INDEX (FIX_R2CSNO_R2RCDT)) ON (M40CSN = R2CSNO AND R2CSNO  = CISNumber)
                //                           LEFT JOIN GeneralDB01.GeneralInfo.GeneralCenter gc WITH (NOLOCK) ON (TitleID = gc.ID AND Type = 'TitleID') 
                //                           WHERE M40BDT BETWEEN   {WSSDTE}   AND   {WSEDTE}   AND  
                //                           ((M40DEL = '') OR ( M40DEL ='X' AND P2DEL ='X' AND P2LOCA ='301'))  
                //                           AND M40BRN =    {WSBRN}   ORDER BY M40BDT ASC";
                //             ds = _dataCenter.GetDataset<DataTable>(sql,CommandType.Text).Result.data;
                //         }
                //         if (WSOPT1.Trim() == "X")
                //         {
                //             string sql_iltb28 = " select CONT from  AS400DB01.ILOD0001.iltb28 WITH (NOLOCK)";
                //             ds_tb28 = _dataCenter.GetDataset<DataTable>(sql_iltb28, CommandType.Text).Result.data;
                //         }
                //    }

                _dataCenter.CloseConnectSQL();

            }
            catch (Exception ex)
            {
                _dataCenter.CloseConnectSQL();
            }

            return ds;
        }

    }
    public class ILDataCenterReportUsingTime
    {
        private UserInfo m_UserInfo;
        private string m_UserName;
        private string m_User;
        public string LastError;
        private string m_Wrkstn = "";
        public EB_Service.DAL.DataCenter _dataCenter;
        public ILDataCenterReportUsingTime(UserInfo userInfo)
        {
            m_UserInfo = userInfo;
            _dataCenter = new EB_Service.DAL.DataCenter(m_UserInfo);

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

        public DataSet getGNAT07()
        {
            DataSet ds = new DataSet();
            try
            {
                //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
                UserInfomation = m_UserInfo;

                string sql = "  select at07gd,at07ds from AS400DB01.GNOD0000.gnat07 WITH (NOLOCK) ";

                ds = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;

            }
            catch (Exception ex)
            {
            }
            return ds;
        }

        public DataSet RP_UsingSpeedTime(string WBDATF, string WBDATT, string WBRN, string WUSER, string WDEPT)
        {
            DataSet ds = new DataSet();
            string wherebrn = "";
            if (WBRN.ToString().Trim() != "")
            {
                wherebrn = "and p1brn = " + WBRN.ToString().Trim() + " ";
            }
            try
            {
                //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
                UserInfomation = m_UserInfo;

                string condition = " AND exists (SELECT * FROM [AS400DB01].[SYOD0000].[SYFUSDES] WITH (NOLOCK) " +
                                   " JOIN AS400DB01.GNOD0000.GNAT04 WITH (NOLOCK) ON USEMID = AT04EM " +
                                   " JOIN AS400DB01.GNOD0000.GNAT05 WITH (NOLOCK) ON AT04JD = AT05JD " +
                                   " JOIN AS400DB01.GNOD0000.GNAT06 WITH (NOLOCK) ON AT05DP = AT06DP " +
                                   " WHERE ilms01.p1crcd = uscode and AT06GD in ('" +
                                    WDEPT +
                                   "') and uscode <> '' )";
                if (WUSER != "")
                {
                    condition += " AND  p1crcd = '" + WUSER + "'";

                }


                //string sql = @" select distinct IDCard ,P1APNO,SUBSTRING(CAST(p1apdt AS varchar),7,2) + '/' + SUBSTRING(CAST(p1apdt AS varchar),5,2) + '/'+ SUBSTRING(CAST(p1apdt AS varchar),1,4) AS AppDate,  " +
                //             " CONCAT(trim(NameInTHAI),' ',trim(SurnameInTHAI)) as Name,FORMAT(P1VDID,'000000000000') as Vendor , p1cont as Contract, " +
                //             " CONCAT(SUBSTRING(CAST(a.mlsstr AS varchar),1,2),':',SUBSTRING(CAST(a.mlsstr AS varchar),3,2),':',SUBSTRING(CAST(a.mlsstr AS varchar),5,2)) as StartTime, " +
                //             " CONCAT(SUBSTRING(CAST(p1avtm AS varchar),1,2),':',SUBSTRING(CAST(p1avtm AS varchar),3,2),':',SUBSTRING(CAST(p1avtm AS varchar),5,2)) as EndTime,  " +
                //         //    " --RIGHT('00'||trim(char(timestampdiff(8,char(timestamp(substr(digits(p1apdt),1,4)||'-'||substr(digits(p1apdt),5,2)||'-'||substr(digits(p1apdt),7,2)||'-'||substr(digits(p1avtm),1,2)||'.'||substr(digits(p1avtm),3,2)||'.'||substr " +
                //         //    " --(digits(p1avtm),5,2)) -  timestamp(substr(digits(p1apdt),1,4)||'-'||substr(digits(p1apdt),5,2)||'-'||substr(digits(p1apdt),7,2)||'-'||substr(digits(a.mlsstr),1,2)||'.'||substr(digits(a.mlsstr),3,2)||'.'||substr(digits(a.mlsstr),5,2)))))),2) ||':'|| " +
                //         //    " --RIGHT('00'||trim(char(mod(timestampdiff(4,char(timestamp(substr(digits(p1apdt),1,4)||'-'||substr(digits(p1apdt),5,2)||'-'||substr(digits(p1apdt),7,2)||'-'||substr(digits(p1avtm),1,2)||'.'||substr(digits(p1avtm),3,2)||'.'|| " +
                //         //    " --substr(digits(p1avtm),5,2)) -  timestamp(substr(digits(p1apdt),1,4)||'-'||substr(digits(p1apdt),5,2)||'-'||substr(digits(p1apdt),7,2)||'-'||substr(digits(a.mlsstr),1,2)||'.'||substr(digits(a.mlsstr),3,2)||'.'||substr(digits(a.mlsstr),5,2)))),60))) ,2) " +
                //         //    " --||':'|| RIGHT('00'||trim(char(mod(timestampdiff(2,char(timestamp(substr(digits(p1apdt),1,4)||'-'||substr(digits(p1apdt),5,2)||'-'||substr(digits(p1apdt),7,2)||'-'||substr(digits(p1avtm),1,2)||'.'||substr(digits(p1avtm),3,2)||'.'|| " +
                //         //    " --substr(digits(p1avtm),5,2)) - timestamp(substr(digits(p1apdt),1,4)||'-'||substr(digits(p1apdt),5,2)||'-'||substr(digits(p1apdt),7,2)||'-'||substr(digits(a.mlsstr),1,2)||'.'||substr(digits(a.mlsstr),3,2)||'.'||substr(digits(a.mlsstr),5,2)))),60))) ,2) as SpeedTime, " +
                //             " '0' as SpeedTime," +
                //             " case when (USFNME <> '' AND USFNME IS NOT null) then USFNME else p1crcd end AS p1crcd " +
                //             " FROM AS400DB01.ILOD0001.ILMS01   WITH (NOLOCK) " +
                //             " LEFT JOIN [CustomerDB01].[CustomerInfo].[CustomerGeneral] WITH (NOLOCK)  ON CISNumber = p1csno " +
                //             " LEFT JOIN AS400DB01.CSOD0001.CSMSLS  a WITH (NOLOCK) on (IDCard=a.mlsidn and a.mlsuus=p1crcd and a.mlsres='OPC003' and a.mlsstr < p1avtm) " +
                //             " LEFT JOIN [AS400DB01].[SYOD0000].[SYFUSDES] WITH (NOLOCK) on (USEMID = p1crcd  AND USEMID != '' AND USEMID IS NOT NULL)  " +

                //             " WHERE p1appt='02' and (a.mlsstr = (select max(b.mlsstr) FROM AS400DB01.CSOD0001.CSMSLS b where IDCard = b.mlsidn  " +
                //             " AND b.mlsuus=ilms01.p1crcd AND b.mlsres='OPC003' AND b.mlsstr < ilms01.p1avtm) or a.mlsstr is null)  " +
                //             " and p1apdt between " + WBDATF + " and " + WBDATT + " " +
                //             " " + wherebrn + " " +
                //             condition +
                //             " ORDER BY AppDate, StartTime ";
                string sql = $@"select distinct IDCard ,P1APNO,SUBSTRING(CAST(p1apdt AS varchar),7,2) + '/' + SUBSTRING(CAST(p1apdt AS varchar),5,2) + '/'+ SUBSTRING(CAST(p1apdt AS varchar),1,4) AS AppDate,   
                              CONCAT(trim(NameInTHAI),' ',trim(SurnameInTHAI)) as Name,FORMAT(P1VDID,'000000000000') as Vendor , CAST(FORMAT(p1cont,'0000000000000000') AS varchar) as Contract,  
                              CASE 
                                WHEN a.mlsstr <> 0 THEN
                                CONCAT(SUBSTRING(CAST(FORMAT(a.mlsstr, '000000') AS varchar),1,2),':',SUBSTRING(CAST(FORMAT(a.mlsstr, '000000') AS varchar),3,2),':',SUBSTRING(CAST(FORMAT(a.mlsstr, '000000') AS varchar),5,2))  
                              ELSE ''
                              END AS StartTime,
                                CONCAT(SUBSTRING(CAST(p1avtm AS varchar),1,2),':',SUBSTRING(CAST(p1avtm AS varchar),3,2),':',SUBSTRING(CAST(p1avtm AS varchar),5,2)) as EndTime,   
                                                CASE
                            WHEN a.mlsstr IS NOT NULL AND p1ktim IS NOT NULL AND (a.mlsstr <> 0 AND p1ktim <> 0)
                            THEN
                                FORMAT(CAST(DATEDIFF(SECOND,SUBSTRING(RIGHT('000000'+TRY_CAST(a.mlsstr AS nvarchar),6),1,2) + ':' +
                                    SUBSTRING(RIGHT('000000'+TRY_CAST(a.mlsstr AS nvarchar),6),3,2) + ':' +
                                    SUBSTRING(RIGHT('000000'+TRY_CAST(a.mlsstr AS nvarchar),6),5,2),
                                    SUBSTRING(RIGHT('000000'+TRY_CAST(p1ktim AS nvarchar),6),1,2) + ':' +
                                    SUBSTRING(RIGHT('000000'+TRY_CAST(p1ktim AS nvarchar),6),3,2) + ':' +
                                    SUBSTRING(RIGHT('000000'+TRY_CAST(p1ktim AS nvarchar),6),5,2))AS INT)/ 3600 , 'D2') + ':' +
                                FORMAT((CAST(DATEDIFF(SECOND,SUBSTRING(RIGHT('000000'+TRY_CAST(a.mlsstr AS nvarchar),6),1,2) + ':' +
                                    SUBSTRING(RIGHT('000000'+TRY_CAST(a.mlsstr AS nvarchar),6),3,2) + ':' +
                                    SUBSTRING(RIGHT('000000'+TRY_CAST(a.mlsstr AS nvarchar),6),5,2),
                                    SUBSTRING(RIGHT('000000'+TRY_CAST(p1ktim AS nvarchar),6),1,2) + ':' +
                                    SUBSTRING(RIGHT('000000'+TRY_CAST(p1ktim AS nvarchar),6),3,2) + ':' +
                                    SUBSTRING(RIGHT('000000'+TRY_CAST(p1ktim AS nvarchar),6),5,2))AS INT)% 3600) / 60 , 'D2') + ':' +
                                FORMAT(CAST(DATEDIFF(SECOND,SUBSTRING(RIGHT('000000'+TRY_CAST(a.mlsstr AS nvarchar),6),1,2) + ':' +
                                    SUBSTRING(RIGHT('000000'+TRY_CAST(a.mlsstr AS nvarchar),6),3,2) + ':' +
                                    SUBSTRING(RIGHT('000000'+TRY_CAST(a.mlsstr AS nvarchar),6),5,2),
                                    SUBSTRING(RIGHT('000000'+TRY_CAST(p1ktim AS nvarchar),6),1,2) + ':' +
                                    SUBSTRING(RIGHT('000000'+TRY_CAST(p1ktim AS nvarchar),6),3,2) + ':' +
                                    SUBSTRING(RIGHT('000000'+TRY_CAST(p1ktim AS nvarchar),6),5,2))AS INT)% 60 , 'D2')
                            ELSE NULL END SpeedTime,
  case when (USFNME <> '' AND USFNME IS NOT null) then USFNME else p1crcd end AS p1crcd 
                              FROM AS400DB01.ILOD0001.ILMS01   WITH (NOLOCK) 
                              LEFT JOIN [CustomerDB01].[CustomerInfo].[CustomerGeneral] WITH (NOLOCK)  ON CISNumber = p1csno 
                              LEFT JOIN AS400DB01.CSOD0001.CSMSLS  a WITH (NOLOCK) on (IDCard=a.mlsidn and a.mlsuus=p1crcd and a.mlsres='OPC003' and a.mlsstr < p1avtm) 
                              LEFT JOIN [AS400DB01].[SYOD0000].[SYFUSDES] WITH (NOLOCK) on (USEMID = p1crcd  AND USEMID != '' AND USEMID IS NOT NULL)  
                             
                              WHERE p1appt='02' and (a.mlsstr = (select max(b.mlsstr) FROM AS400DB01.CSOD0001.CSMSLS b where IDCard = b.mlsidn  
                              AND b.mlsuus=ilms01.p1crcd AND b.mlsres='OPC003' AND b.mlsstr < ilms01.p1avtm) or a.mlsstr is null)  
                              and p1apdt between  '{WBDATF}'   and '{WBDATT}' 
                                {wherebrn}  {condition}
                              ORDER BY AppDate, StartTime ";

                ds = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;

            }
            catch (Exception ex)
            {
            }
            return ds;
        }
    }

    public class ILDataCenterOnMobile
    {
        private UserInfo m_UserInfo;
        private string m_UserName;
        private string m_User;
        public string LastError;
        private string m_Wrkstn = "";
        public EB_Service.DAL.DataCenter _dataCenter;
        public ILDataCenterOnMobile(UserInfo userInfo)
        {
            m_UserInfo = userInfo;
            _dataCenter = new EB_Service.DAL.DataCenter(m_UserInfo);

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


        public DataSet RP_ILOnMobile(string DateF, string DateT, string contract)
        {
            DataSet ds = new DataSet();
            //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_UserInfo;

            UserInfomation = m_UserInfo;
            try
            {
                string sqlWhere = " WHERE 1=1 ";
                if (!string.IsNullOrEmpty(contract))
                {
                    sqlWhere += " AND cd.ContractCust = '" + contract + "'";
                }

                sqlWhere += "AND cd.CreateDate >= '" + DateF + "' AND cd.CreateDate < '" + DateT + "' ";

                string sql = "";

                sql = $@"SELECT CustID,cd.AppID,cd.ContractCust,cd.TitleNameCust as Prefix,cg.NameInTHAI  as Firstname,cg.SurnameInTHAI  as Surname,TermCondVersion 
                    ,(case when CreditModelStatus = 'Y' then 'ยินยอม' 
                        when CreditModelStatus = 'X' then 'ไม่ยินยอม' end) as CreditModelStatus 
                    ,(case WHEN ContractStatus = 'P' AND DeliverProductStatus = 'N' THEN 'สัญญาอนุมัติ' 
                         WHEN ContractStatus = 'R' AND DeliverProductStatus = 'N' THEN 'ไม่ผ่านเงื่อนไข'
                         WHEN ContractStatus = 'C' AND DeliverProductStatus = 'N' THEN 'ลูกค้ายืนยันรับสินค้า'
                         WHEN ContractStatus = 'X' AND DeliverProductStatus = 'N' THEN 'ลูกค้ายกเลิกสัญญา'
                         WHEN ContractStatus = 'T' AND DeliverProductStatus = 'N' THEN 'ลูกค้าไม่กดยืนยันหรือยกเลิก'
                         WHEN ContractStatus = 'C' AND DeliverProductStatus = 'Y' THEN 'ร้านค้าส่งมอบสินค้า'
                         WHEN ContractStatus = 'C' AND DeliverProductStatus = 'X' THEN 'ร้านค้ายกเลิกสัญญา'
                         WHEN ContractStatus = 'C' AND DeliverProductStatus = 'T' THEN 'ร้านค้าไม่กดยืนยันหรือยกเลิก'  end) as ContractStatus
                    ,(case when DeliverProductStatus = 'Y' then 'ส่งมอบสินค้า' 
                        when DeliverProductStatus = 'X' then 'ไม่ส่งมอบสินค้า' 
                        when DeliverProductStatus = 'T' then 'ยกเลิกสัญญาโดยระบบ' 
                        when DeliverProductStatus = 'N' then 'ยังไม่ได้ส่งมอบ' end) as DeliverProductStatus 
                    ,DescriptionContSts,cd.VendorCode,(trim(p10nam) +' / '+ trim(p10fi1)) as VendorName,VendorQRCount 
                    ,(convert(varchar,(FORMAT(DATEADD(YEAR, 543, cd.CreateDate),'dd/MM/yyyy'))) + ' ' + convert(varchar, cd.CreateDate, 8)) as CreateDate 
                    ,(convert(varchar,(FORMAT(DATEADD(YEAR, 543, cd.UpdateDate),'dd/MM/yyyy'))) + ' ' + convert(varchar, cd.UpdateDate, 8)) as UpdateDate 
                    From BusinessDB01.ILSystem.ContractDetailMobile cd WITH (NOLOCK) 
                    left join BusinessDB01.ILSystem.ContractILMobile ct WITH (NOLOCK) on ct.ContractCust = cd.ContractCust 
                    INNER JOIN CustomerDB01.CustomerInfo.CustomerGeneral cg WITH (NOLOCK) on cg.ID = ct.CustID 
                    left join BusinessDB01.ILSystem.ManageVendorIL v WITH (NOLOCK) on v.VendorCode = cd.VendorCode 
                    left join AS400DB01.ILOD0001.ILMS10	il10 WITH (NOLOCK) on il10.P10VEN = cd.VendorCode
                    { sqlWhere }  ORDER BY CreateDate ASC";

                var result = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result;
                if (result.success)
                {
                    ds = result.data;

                }
                return ds;
            }
            catch (Exception)
            {
                return ds;
            }
        }

        public DataSet RP_ILOnMobile_Detail(string contract, string appId)
        {
            DataSet ds = new DataSet();
            //UserInfo m_UserInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_UserInfo;

            try
            {
                string sql = $@"SELECT cd.ContractCust,cd.AppID,UmayCardNo,ct.CustID ,cd.TitleNameCust as Prefix,cd.FirstNameCust as Firstname,cd.LastNameCust as Surname,cg.IDCard  as IDCard,cg.BirthDate  as Birthday,ProductTypeCode,ProductTypeDesc,CampaignCode,ProductBrandDesc,ProductNameCode,ProductNameDesc
                    ,ProductCodeDesc,ProductPrice,DownPayment,LoanRequest,StampDutyFee,NCBFee,InterrestRate,CreditUsageRate,TotalTerm,FirstInstallment,SecondInstallment
                    ,LastInstallment,cd.VendorCode,(trim(p10nam) +' / '+ trim(p10fi1)) as VendorName  
                    ,(convert(varchar, cd.CreateDate, 103) + ' ' + convert(varchar, cd.CreateDate, 8)) as CreateDate 
                    ,(convert(varchar, cd.CreateDate, 103)) as CreateDateTime 
                    ,(convert(varchar, cd.CreateDate, 8)) as CreateTime 
                    From BusinessDB01.ILSystem.ContractDetailMobile cd WITH (NOLOCK) 
                    left join BusinessDB01.ILSystem.ContractILMobile ct WITH (NOLOCK) on ct.ContractCust = cd.ContractCust 
                    INNER JOIN CustomerDB01.CustomerInfo.CustomerGeneral cg WITH (NOLOCK) on cg.ID = ct.CustID 
                    left join BusinessDB01.ILSystem.ManageVendorIL v WITH (NOLOCK) on v.VendorCode = cd.VendorCode 
                    left join AS400DB01.ILOD0001.ILMS10	il10 WITH (NOLOCK) on il10.P10VEN = cd.VendorCode
                    Where cd.ContractCust='{ contract }' and cd.AppID='{ appId }'";

                var result = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result;
                if (result.success)
                {
                    ds = result.data;

                }
                return ds;
            }
            catch (Exception)
            {
                return ds;
            }
        }
        public DataSet Sp_GetProductItem(string SearchBy, string SearchValue, int PageNo, int PageSize)
        {
            DataSet ds = new DataSet();
            UserInfomation = m_UserInfo;
            try
            {
                string sql = $@"EXEC [AS400DB01].[ILOD0001].[sp_GetProductItem]
		                    N'{SearchBy}',
		                    N'{SearchValue}',
		                    N'{PageNo}',
		                    N'{PageSize}'";
                ds = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
                _dataCenter.CloseConnectSQL();

            }
            catch (Exception ex)
            {
                _dataCenter.CloseConnectSQL();
            }

            return ds;
        }

    }

    public class ILDataCenterOnMasterFile
    {
        private UserInfo m_UserInfo;
        private string m_UserName;
        private string m_User;
        public string LastError;
        private string m_Wrkstn = "";
        public EB_Service.DAL.DataCenter _dataCenter;
        public ILDataCenterOnMasterFile(UserInfo userInfo)
        {
            m_UserInfo = userInfo;
            _dataCenter = new EB_Service.DAL.DataCenter(m_UserInfo);

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

        public DataSet Sp_GetProductItem(string SearchBy, string SearchValue, int PageNo, int PageSize)
        {
            DataSet ds = new DataSet();
            UserInfomation = m_UserInfo;
            try
            {
                string sql = $@"EXEC [AS400DB01].[ILOD0001].[sp_GetProductItem]
		                    N'{SearchBy}',
		                    N'{SearchValue}',
		                    N'{PageNo}',
		                    N'{PageSize}'";
                ds = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
                _dataCenter.CloseConnectSQL();

            }
            catch (Exception ex)
            {
                _dataCenter.CloseConnectSQL();
            }

            return ds;
        }

        public DataSet Sp_GetProductModel(string SearchBy, string SearchValue, int PageNo, int PageSize)
        {
            DataSet ds = new DataSet();
            UserInfomation = m_UserInfo;
            try
            {
                string sql = $@"EXEC [AS400DB01].[ILOD0001].[sp_GetProductModel]
		                    N'{SearchBy}',
		                    N'{SearchValue}',
		                    N'{PageNo}',
		                    N'{PageSize}'";
                ds = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
                _dataCenter.CloseConnectSQL();

            }
            catch (Exception ex)
            {
                _dataCenter.CloseConnectSQL();
            }

            return ds;
        }

        public DataSet sp_GetVendorMaster(string SearchBy, string SearchValue, int PageNo, int PageSize)
        {
            DataSet ds = new DataSet();
            UserInfomation = m_UserInfo;
            try
            {
                string sql = $@"EXEC [AS400DB01].[ILOD0001].[sp_GetVendorMaster]
		                    N'{SearchBy}',
		                    N'{SearchValue}',
		                    N'{PageNo}',
		                    N'{PageSize}'";
                ds = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
                _dataCenter.CloseConnectSQL();

            }
            catch (Exception ex)
            {
                _dataCenter.CloseConnectSQL();
            }

            return ds;
        }

        public DataSet Sp_GetBrand(string SearchBy, string SearchValue, int PageNo, int PageSize)
        {
            DataSet ds = new DataSet();
            UserInfomation = m_UserInfo;
            try
            {
                string sql = $@"EXEC [AS400DB01].[ILOD0001].[sp_GetBrand]
		                    N'{SearchBy}',
		                    N'{SearchValue}',
		                    N'{PageNo}',
		                    N'{PageSize}'";
                ds = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
                _dataCenter.CloseConnectSQL();

            }
            catch (Exception ex)
            {
                _dataCenter.CloseConnectSQL();
            }

            return ds;
        }

        public DataSet Sp_GetProductCode(string SearchBy, string SearchValue, int PageNo, int PageSize)
        {
            DataSet ds = new DataSet();
            UserInfomation = m_UserInfo;
            try
            {
                string sql = $@"EXEC [AS400DB01].[ILOD0001].[sp_GetProductCode]
		                    N'{SearchBy}',
		                    N'{SearchValue}',
		                    N'{PageNo}',
		                    N'{PageSize}'";
                ds = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
                _dataCenter.CloseConnectSQL();

            }
            catch (Exception ex)
            {
                _dataCenter.CloseConnectSQL();
            }

            return ds;
        }

        public DataSet Sp_GetVendorApplicationType(string Type, string SearchBy, string SearchValue, int PageNo, int PageSize)
        {
            DataSet ds = new DataSet();
            UserInfomation = m_UserInfo;
            try
            {
                string sql = $@"EXEC [AS400DB01].[ILOD0001].[Sp_GetVendorApplicationType]
		                    N'{Type}',
		                    N'{SearchBy}',
		                    N'{SearchValue}',
		                    N'{PageNo}',
		                    N'{PageSize}'";
                ds = _dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
                _dataCenter.CloseConnectSQL();

            }
            catch (Exception ex)
            {
                _dataCenter.CloseConnectSQL();
            }

            return ds;
        }
    }
}
