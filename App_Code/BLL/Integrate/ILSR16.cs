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
    public class ILSR16 : UserInfo
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
        private string sql = "";

        public ILSR16()
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
        private string W12CSN;
        private string PIRSQ;
        private string PITELT;
        private string PIEXTN;

        public bool CALL_ILSR16(EB_Service.DAL.DataCenter dataCenter, string Mode_IN, string CSN_IN, string BRN_IN, string APPNO_IN, ref string prmError, string strBizInit, string strBranchNo)
        {
            ILDataCenter ilobj = new ILDataCenter();
            #region @DEL01        BEGSR
            sql = $@"DELETE FROM CustomerDB01.CustomerInfo.CustomerReference
                WHERE CustID = (SELECT TOP(1) ID FROM CustomerDB01.CustomerInfo.CustomerGeneral WITH (NOLOCK) WHERE CISNumber = '{CSN_IN}')
                AND ReferenceID = (SELECT TOP(1) ID FROM GeneralDB01.GeneralInfo.GeneralCenter WHERE TYPE = 'ReferenceID' AND Code = '1')";
            bool transaction = dataCenter.Sqltr == null ? true : false;
            int afrows = dataCenter.Execute(sql, CommandType.Text, transaction).Result.afrows;
            if(afrows == -1)
            {
                prmError = "Y";
                dataCenter.RollbackMssql();
                dataCenter.CloseConnectSQL();
                return false;
            }
            #endregion
            #region @DEL11        BEGSR
            sql = $@"DELETE FROM CustomerDB01.CustomerInfo.CustomerAddress
                    WHERE CustID = (SELECT TOP(1) ID FROM CustomerDB01.CustomerInfo.CustomerGeneral WITH (NOLOCK) WHERE CISNumber = '{CSN_IN}')
                    AND CustRefID <> 0 ";
            transaction = dataCenter.Sqltr == null ? true : false;
            afrows = dataCenter.Execute(sql, CommandType.Text, transaction).Result.afrows;
            if (afrows == -1)
            {
                prmError = "Y";
                dataCenter.RollbackMssql();
                dataCenter.CloseConnectSQL();
                return false;
            }
            #endregion
            sql = $@"SELECT W12STS, W12CSN, W12RSQ, W12REL, W12NME, W12SNM, W12HTL, W12HEX, W12OTL, W12OEX, W12MOB
                    FROM AS400DB01.ILOD0001.ILWK12 WITH (NOLOCK)
                    WHERE W12CSN = {CSN_IN} AND W12BRN = {BRN_IN} AND W12APN = {APPNO_IN} ";
            DataSet DS = dataCenter.GetDataset<DataTable>(sql, CommandType.Text).Result.data;
            if (ilobj.check_dataset(DS))
            {
                foreach(DataRow dr in DS.Tables[0].Rows)
                {
                    W12CSN = dr["W12CSN"].ToString();
                    PIRSQ = dr["W12RSQ"].ToString();
                    PITELT = dr["W12HTL"].ToString();
                    PIEXTN = dr["W12HTL"].ToString();
                    if (string.IsNullOrEmpty(dr["W12STS"].ToString()))
                    {
                        #region EXSR      @INS01
                        sql = $@"INSERT INTO CustomerDB01.CustomerInfo.CustomerReference
                            (CustID, ReferenceID, Sequence, RelationID, TitleID, NameInTHAI, SurnameInTHAI, UpdateDate, UpdateBy, Application)
                            VALUES
                            (
                            (SELECT TOP(1)ID FROM CustomerDB01.CustomerInfo.CustomerGeneral WITH (NOLOCK) WHERE CISNumber = '{dr["W12CSN"].ToString()}' ),
                            (SELECT TOP(1)ID FROM GeneralDB01.GeneralInfo.GeneralCenter WITH (NOLOCK) WHERE Type = 'ReferenceID' AND Code = '1'),
                            '{dr["W12RSQ"].ToString()}',
                            (SELECT ISNULL(
                            (SELECT ID FROM GeneralDB01.GeneralInfo.GeneralCenter WITH (NOLOCK) WHERE Type = 'RelationID' AND Code = '{dr["W12REL"].ToString()}'
                            ), (SELECT ID FROM GeneralDB01.GeneralInfo.GeneralCenter WITH (NOLOCK) WHERE Type = 'OtherID')
                            )),
                            (SELECT TOP(1)ID FROM GeneralDB01.GeneralInfo.GeneralCenter WITH (NOLOCK) WHERE Type = 'TitleID' AND Code = '155'),
                            '{dr["W12NME"].ToString()}',
                            '{dr["W12SNM"].ToString()}',
                            GETDATE(),
                            '{m_UserInfo.Username}',
                            'ILSR16'
                            )";
                        transaction = dataCenter.Sqltr == null ? true : false;
                        afrows = dataCenter.Execute(sql, CommandType.Text, transaction).Result.afrows;
                        if(afrows == -1)
                        {
                            prmError = "Y";
                            dataCenter.RollbackMssql();
                            dataCenter.CloseConnectSQL();
                            return false;
                        }
                        #endregion
                        #region EXSR      @INS11
                        sql = $@"INSERT INTO CustomerDB01.CustomerInfo.CustomerAddress
                            (CustID, CustRefID, IsShipTo, AddressCodeID, BuildingTitleID , AmphurID, TambolID, ProvinceID,  TelephoneNumber1, ExtensionNumber1, TelephoneNumber2, ExtensionNumber2, Mobile, UpdateDate, UpdateBy, Application)
                            VALUES
                            (
                            (SELECT TOP(1)ID FROM CustomerDB01.CustomerInfo.CustomerGeneral WITH (NOLOCK) WHERE CISNumber = '{dr["W12CSN"].ToString()}' ),
                            (SELECT TOP(1)ID FROM GeneralDB01.GeneralInfo.GeneralCenter WITH (NOLOCK) WHERE Type = 'ReferenceID' AND Code = '1'),
                            (CASE WHEN 1 <> 0 THEN '' ELSE 'Y' END),
                            (SELECT TOP(1)ID FROM GeneralDB01.GeneralInfo.GeneralCenter WITH (NOLOCK) WHERE Type = 'AddressCodeID' AND Code = 'H'),
                            (SELECT TOP(1) ID FROM GeneralDB01.GeneralInfo.GeneralCenter WITH (NOLOCK) WHERE Type = 'OtherID' AND Code = '9999999999'),
                            (SELECT TOP(1) ID FROM GeneralDB01.GeneralInfo.AddrAumphur WITH (NOLOCK) WHERE Code = '0'),
                            (SELECT TOP(1) ID FROM GeneralDB01.GeneralInfo.AddrTambol WITH (NOLOCK) WHERE Code = '0'),
                            (SELECT TOP(1) ID FROM GeneralDB01.GeneralInfo.AddrProvince WITH (NOLOCK) WHERE Code = '0'),
                            '{dr["W12HTL"].ToString()}',
                            '{dr["W12HEX"].ToString()}',
                            '{dr["W12OTL"].ToString()}',
                            '{dr["W12OEX"].ToString()}',
                            '{dr["W12MOB"].ToString()}',
                            GETDATE(),
                            '{m_UserInfo.Username}',
                            'ILSR16'
                            )";
                        transaction = dataCenter.Sqltr == null ? true : false;
                        afrows = dataCenter.Execute(sql, CommandType.Text, transaction).Result.afrows;
                        if (afrows == -1)
                        {
                            prmError = "Y";
                            dataCenter.RollbackMssql();
                            dataCenter.CloseConnectSQL();
                            return false;
                        }
                        #endregion
                    }
                }
            }
            string poerr = "";
            //ilobj.CALL_CSSR16(Mode_IN, "A", W12CSN, PIRSQ, "A", "H", PITELT, PIEXTN,ref poerr, strBizInit, strBranchNo);
            prmError = poerr;
            dataCenter.CommitMssql();
            dataCenter.CloseConnectSQL();
            return true;
        }
    }
}