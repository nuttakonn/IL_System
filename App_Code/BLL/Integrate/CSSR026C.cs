using EB_Service.Commons;
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
using System.Threading.Tasks;
using System.Web;

namespace ILSystem.App_Code.BLL.Integrate
{

    public class CSSR026C : UserInfo
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
        public CSSR026C(UserInfo userInfo)
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

        public bool CALL_CSSR026C(string idCard, string Appno, ref bool POError)
        {
            try
            {
                ILDataCenterMssqlInterview CallHisunCustomer = new ILDataCenterMssqlInterview(m_UserInfo);
                ILDataCenter ilobj = new ILDataCenter();
                ILDataSubroutine iLDataSubroutine = new ILDataSubroutine(m_UserInfo);
                DataSet DS = new DataSet();
                string sqlCustomer = $@"SELECT CG.ID,IDCard,CISNumber,FORMAT(BirthDate,'ddMMyyyy','th-TH') as M00BDT,M13BDT
                                              ,M13CHL as M00TOC,M13SLT as M00SAT ,GM.ID as M00MST,GS.ID as M00SEX
                                              ,GR.ID as M00RST ,M13CON as M00TOF,M13LYR as M00RYR,M13LMT as M00RMO,M13BUT as M00BUS
                                              ,G0.ID as M00OCC,GP.ID as M00POS,convert(int,M13OFF) as M00TOO,M13WKY as M00WTY,M13WKM as M00WTM
                                              ,M13SLT as M00SAT,M13NET as M00SAL, '0' as M00INC,M13APP,M13EMP as M00EPT
                                              ,CS.SalaryAdjustAMT, M13CAL, M13HEX, M13HZP, M13HTM, M13HAM, M13HPV, M13MTL, M13HTL, M13MTL, M13HPV
                                              ,M13OZP, M13OTM, M13OAM, M13OPV
                                         from CustomerDB01.CustomerInfo.CustomerGeneral CG WITH (NOLOCK)
                                         join AS400DB01.CSOD0001.csms13 C3  WITH (NOLOCK) on CG.CISNumber = C3.M13CSN
                                         join CustomerDB01.CustomerInfo.CustomerSalary CS WITH (NOLOCK) on CG.id = CS.Custid
                                         join CustomerDB01.CustomerInfo.CustomerWorked CW WITH (NOLOCK) on CG.id = CW.Custid
                                         join GeneralDB01.GeneralInfo.GeneralCenter GM WITH (NOLOCK) on (GM.Code = C3.M13MRT and GM.Type = 'MaritalStatusID')
                                         join GeneralDB01.GeneralInfo.GeneralCenter GS WITH (NOLOCK) on (GS.Code = C3.M13SEX and GS.Type = 'SexID')
                                         join GeneralDB01.GeneralInfo.GeneralCenter GR WITH (NOLOCK) on (GR.Code = C3.M13RES and GR.Type = 'ResidentalStatusID')
                                         join GeneralDB01.GeneralInfo.GeneralCenter GSA WITH (NOLOCK) on (GSA.Code = C3.M13SLT and GSA.Type = 'SalaryTypeID')
                                         join GeneralDB01.GeneralInfo.GeneralCenter GW WITH (NOLOCK) on (GW.Code = C3.M13BUT and GW.Type = 'CompanyBusinessID')
                                         join GeneralDB01.GeneralInfo.GeneralCenter G0 WITH (NOLOCK) on (G0.Code = C3.M13OCC and G0.Type = 'OccupationID') 
                                         join GeneralDB01.GeneralInfo.GeneralCenter GP WITH (NOLOCK) on (GP.Code = C3.M13POS and GP.Type = 'PositionID')
                                         left join GeneralDB01.GeneralInfo.GeneralCenter GI WITH (NOLOCK) on (GI.Code = C3.M13CAL and GI.Type = 'CalculateIncomeID')
                                         where CG.IDCard  = {idCard} and C3.M13APN = {Appno} ";
                DS = _dataCenter.GetDataset<DataTable>(sqlCustomer, CommandType.Text).Result.data;
                _dataCenter.CloseConnectSQL();
                if (!ilobj.check_dataset(DS))
                {
                    DataRow dr_CS00 = DS.Tables[0].Rows.Count > 0 ? DS.Tables[0].Rows[0] : null;
                    //bool resGNP0371 = iLDataSubroutine.CALL_GNP0371(dr_ILCS00["M00BDT"].ToString().Trim(), "", "YMD", "B", "", "RL", "",
                    //                              "", "",ref Age, ref Error);
                    //if (resGNP0371 == false || Error.Trim() == "Y")
                    //{
                    //    M00AGE = "0";
                    //}
                    //else
                    //{
                    //    M00AGE = Age;
                    //}
                    bool success = true;
                    string cmdCusGeneral;
                    cmdCusGeneral = $@"Update CustomerDB01.CustomerInfo.CustomerGeneral set
                                              NoOfChildren = {dr_CS00["M00TOC"].ToString().Trim()}, 
                                              MaritalStatusID = {dr_CS00["M00MST"].ToString().Trim()}, 
                                              SexID = {dr_CS00["M00SEX"].ToString().Trim()},  
                                              ResidentalStatusID = {dr_CS00["M00RST"].ToString().Trim()}, 
                                              NoOfFamily = {dr_CS00["M00TOF"].ToString().Trim()}, 
                                              ResidentalYear = {dr_CS00["M00RYR"].ToString().Trim()}, 
                                              ResidentalMonth = {dr_CS00["M00RMO"].ToString().Trim()}, 
                                              UpdateDate = '{m_UdpD.ToString().PadLeft(8, '0').Substring(0, 4)}-{m_UdpD.ToString().PadLeft(8, '0').Substring(4, 2)}-{m_UdpD.ToString().PadLeft(8, '0').Substring(6, 2)} {m_UpdTime.ToString().PadLeft(6, '0').Substring(0, 2)}:{m_UpdTime.ToString().PadLeft(6, '0').Substring(2, 2)}:{m_UpdTime.ToString().PadLeft(6, '0').Substring(4, 2)}', 
                                              UpdateBy = '{m_UserName}'
                                             
                                        where ID = {dr_CS00["ID"].ToString().Trim()}";



                    bool transaction = CallHisunCustomer._dataCenter.Sqltr == null ? true : false;
                    var res_CusGeneral = CallHisunCustomer._dataCenter.Execute(cmdCusGeneral, CommandType.Text, transaction).Result;
                    if (res_CusGeneral.afrows == -1)
                    {
                        success = false;
                        Utility.WriteLogString(res_CusGeneral.message.ToString(), cmdCusGeneral);
                        goto commit_rollback;

                    }

                    string cmdCusSalary;
                    cmdCusSalary = $@"Update CustomerDB01.CustomerInfo.CustomerSalary set
                                             SalaryTypeID = {dr_CS00["M00SAT"].ToString().Trim()}, 
                                             SalaryAMT = {dr_CS00["M00SAL"].ToString().Trim()}, 
                                             UpdateDate_CSMS00 = '{m_UdpD.ToString().PadLeft(8, '0').Substring(0, 4)}-{m_UdpD.ToString().PadLeft(8, '0').Substring(4, 2)}-{m_UdpD.ToString().PadLeft(8, '0').Substring(6, 2)} {m_UpdTime.ToString().PadLeft(6, '0').Substring(0, 2)}:{m_UpdTime.ToString().PadLeft(6, '0').Substring(2, 2)}:{m_UpdTime.ToString().PadLeft(6, '0').Substring(4, 2)}', 
                                             Application = 'CSSR06C' ,
                                             UpdateBy = '{m_UserName}'
                                             where CustID = {dr_CS00["ID"].ToString().Trim()}";



                    transaction = CallHisunCustomer._dataCenter.Sqltr == null ? true : false;
                    var res_CusSalary = CallHisunCustomer._dataCenter.Execute(cmdCusSalary, CommandType.Text, transaction).Result;
                    if (res_CusSalary.afrows == -1)
                    {
                        success = false;
                        Utility.WriteLogString(res_CusSalary.message.ToString(), cmdCusSalary);
                        goto commit_rollback;

                    }

                    string cmdCusWork;
                    cmdCusWork = $@"Update CustomerDB01.CustomerInfo.CustomerWorked set
                                           CompanyBusinessID = {dr_CS00["M00BUS"].ToString().Trim()}, 
                                           OccupationID = {dr_CS00["M00OCC"].ToString().Trim()}, 
                                           PositionID = {dr_CS00["M00POS"].ToString().Trim()}, 
                                           TotalOfficer = {dr_CS00["M00TOO"].ToString().Trim()}, 
                                           TotalWorkedYear = {dr_CS00["M00WTY"].ToString().Trim()}, 
                                           TotalWorkedMonth = {dr_CS00["M00WTM"].ToString().Trim()}, 
                                           UpdateDate = '{m_UdpD.ToString().PadLeft(8, '0').Substring(0, 4)}-{m_UdpD.ToString().PadLeft(8, '0').Substring(4, 2)}-{m_UdpD.ToString().PadLeft(8, '0').Substring(6, 2)} {m_UpdTime.ToString().PadLeft(6, '0').Substring(0, 2)}:{m_UpdTime.ToString().PadLeft(6, '0').Substring(2, 2)}:{m_UpdTime.ToString().PadLeft(6, '0').Substring(4, 2)}', 
                                           Application = 'CSSR06C' ,
                                           UpdateBy = '{m_UserName}'
                                           where CustID = {dr_CS00["ID"].ToString().Trim()}";



                    transaction = CallHisunCustomer._dataCenter.Sqltr == null ? true : false;
                    var res_CusWork = CallHisunCustomer._dataCenter.Execute(cmdCusWork, CommandType.Text, transaction).Result;
                    if (res_CusWork.afrows == -1)
                    {
                        success = false;
                        Utility.WriteLogString(res_CusWork.message.ToString(), cmdCusWork);
                        goto commit_rollback;

                    }

                    
                    string sqlCustomerHome = $@"UPDATE CustomerDB01.CustomerInfo.CustomerAddress
                                        SET TelephoneNumber1 = '{dr_CS00["M13HTL"].ToString()}',
                                        ExtensionNumber1 = '{dr_CS00["M13HEX"].ToString()}',
                                        Mobile = '{dr_CS00["M13MTL"].ToString()}',
                                        PostalAreaCode = {dr_CS00["M13HZP"].ToString()},
                                        TambolID = (SELECT TOP(1) ID FROM GeneralDB01.GeneralInfo.AddrTambol WITH (NOLOCK) WHERE Code = '{dr_CS00["M13HTM"].ToString()}'),
                                        AmphurID = (SELECT TOP(1) ID FROM GeneralDB01.GeneralInfo.AddrAumphur WITH (NOLOCK) WHERE Code = '{dr_CS00["M13HAM"].ToString()}'),
                                        ProvinceID = (SELECT TOP(1) ID FROM GeneralDB01.GeneralInfo.AddrProvince WITH (NOLOCK) WHERE Code = '{dr_CS00["M13HPV"].ToString()}'),
                                        UpdateDate = GETDATE(),
                                        UpdateBy = '{m_UserName}',
                                        Application = 'CSSR06C'
                                        WHERE CustID = (SELECT TOP(1) ID FROM CustomerDB01.CustomerInfo.CustomerGeneral WHERE IDCard = '{idCard}')
                                        AND CustRefID = 0 AND AddressCodeID = (SELECT TOP(1) ID FROM GeneralDB01.GeneralInfo.GeneralCenter WITH (NOLOCK) WHERE Code = 'H' AND Type = 'AddressCodeID')";
                    
                    transaction = CallHisunCustomer._dataCenter.Sqltr == null ? true : false;
                    int afrows = CallHisunCustomer._dataCenter.Execute(sqlCustomerHome, CommandType.Text, transaction).Result.afrows;
                    if (afrows == -1)
                    {
                        success = false;
                        Utility.WriteLogString(sqlCustomerHome.ToString(), sqlCustomerHome);
                        goto commit_rollback;

                    }
                    string sqlCustomerOffice = $@"UPDATE CustomerDB01.CustomerInfo.CustomerAddress 
                                            SET PostalAreaCode = {dr_CS00["M13OZP"].ToString()},
                                            TambolID = (SELECT TOP(1) ID FROM GeneralDB01.GeneralInfo.AddrTambol WITH (NOLOCK) WHERE Code = '{dr_CS00["M13OTM"].ToString()}'),
                                            AmphurID = (SELECT TOP(1) ID FROM GeneralDB01.GeneralInfo.AddrAumphur WITH (NOLOCK) WHERE Code = '{dr_CS00["M13OAM"].ToString()}'),
                                            ProvinceID = (SELECT TOP(1) ID FROM GeneralDB01.GeneralInfo.AddrProvince WITH (NOLOCK) WHERE Code = '{dr_CS00["M13OPV"].ToString()}'),
                                            UpdateDate = GETDATE(),
                                            UpdateBy = '{m_UserName}',
                                            Application = 'CSSR06C'
                                            WHERE CustID = (SELECT TOP(1) ID FROM CustomerDB01.CustomerInfo.CustomerGeneral WHERE IDCard = '{idCard}') 
                                            AND CustRefID = 0 AND AddressCodeID = (SELECT TOP(1) ID FROM GeneralDB01.GeneralInfo.GeneralCenter WITH (NOLOCK) WHERE Code = 'O' AND Type = 'AddressCodeID')";

                    transaction = CallHisunCustomer._dataCenter.Sqltr == null ? true : false;
                    int afrows2 = CallHisunCustomer._dataCenter.Execute(cmdCusWork, CommandType.Text, transaction).Result.afrows;
                    if (afrows2 == -1)
                    {
                        success = false;
                        Utility.WriteLogString(sqlCustomerOffice.ToString(), sqlCustomerHome);
                        goto commit_rollback;

                    }

                commit_rollback:
                    if (success)
                    {
                        CallHisunCustomer._dataCenter.CommitMssql();
                        CallHisunCustomer._dataCenter.CloseConnectSQL();
                        POError = true;

                    }
                    else
                    {
                        CallHisunCustomer._dataCenter.RollbackMssql();
                        CallHisunCustomer._dataCenter.CloseConnectSQL();
                        POError = false;
                    }

                    return POError;

                }
                return true;

            }
            catch
            {
                return false;
            }
        }

    }
}