using System;
using System.Text;
using System.Web.Configuration;

namespace EB_Service.Commons
{
    /// <summary>
    /// Summary description for AppEnum.
    /// </summary>
    public enum ViewStateEnum
    {
        NEW_OBJECT = 0,
        VIEW_NAME,
        OBJECT_ID,
        EDIT_STATE
    }

    public class UserControlsConfig
    {
        public static string CallDatePicker(string ctlPath, string targetId, string DefaultDate)
        {
            DateTime defDt = DateTime.Today;
            try
            {
                defDt = DateTime.Parse(DefaultDate);
            }
            catch { }
            StringBuilder strFnc = new StringBuilder();
            strFnc.Append("javascript:CallCalendarPagePopup('");
            strFnc.Append(ctlPath + "',");
            strFnc.Append("'dd/MM/yyyy','");
            strFnc.Append(targetId);
            strFnc.Append("','" + defDt.ToString("dd/MM/yyyy"));
            strFnc.Append("',this);");
            return strFnc.ToString();
        }

    }



    /// <summary>
    /// Summary description for AppConfig
    /// </summary>

    public class AppConfig
    {
        public AppConfig()
        {
        }


        public static string iSeries400ConnectionString
        {
            get { return WebConfigurationManager.ConnectionStrings["iSeries400ConnectionString"].ConnectionString; }
        }

        public static string OleDB400ConnectionString
        {
            get { return WebConfigurationManager.ConnectionStrings["OleDB400ConnectionString"].ConnectionString; }
        }

        //Peak 28/05/2550 เพิ่มItem สำหรับการเรียกใช้ค่าของFile รูปที่นำไปแสดงในส่วนของ Header และ Announcement
        public static string OracleConnectionString
        {
            get { return WebConfigurationManager.ConnectionStrings["OracleConnectionString"].ConnectionString; }
        }

        //Peak 28/05/2550 เพิ่มItem สำหรับการเรียกใช้ค่าของFile รูปที่นำไปแสดงในส่วนของ Header และ Announcement
        public static string MySqlConnectionString
        {
            get { return WebConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString; }
        }

        public static string OraAutHost
        {
            get { return WebConfigurationManager.AppSettings["OraAutHost"]; }
        }

        public static string OraAutSID
        {
            get { return WebConfigurationManager.AppSettings["OraAutSID"]; }
        }

        public static string OraAutUser
        {
            get { return WebConfigurationManager.AppSettings["OraAutUser"]; }
        }

        public static string OraAutPwd
        {
            get { return WebConfigurationManager.AppSettings["OraAutPwd"]; }
        }

        public static string OraProHost
        {
            get { return WebConfigurationManager.AppSettings["OraProHost"]; }
        }

        public static string OraProSID
        {
            get { return WebConfigurationManager.AppSettings["OraProSID"]; }
        }

        public static string OraProUser
        {
            get { return WebConfigurationManager.AppSettings["OraProUser"]; }
        }

        public static string OraProPwd
        {
            get { return WebConfigurationManager.AppSettings["OraProPwd"]; }
        }


        public static string MySqlAutHost
        {
            get { return WebConfigurationManager.AppSettings["MySqlAutHost"]; }
        }

        public static string MySqlAutDbName
        {
            get { return WebConfigurationManager.AppSettings["MySqlAutDbName"]; }
        }

        public static string MySqlAutUser
        {
            get { return WebConfigurationManager.AppSettings["MySqlAutUser"]; }
        }

        public static string MySqlAutPwd
        {
            get { return WebConfigurationManager.AppSettings["MySqlAutPwd"]; }
        }

        public static string MySqlProHost
        {
            get { return WebConfigurationManager.AppSettings["MySqlProHost"]; }
        }

        public static string MySqlProDbName
        {
            get { return WebConfigurationManager.AppSettings["MySqlProDbName"]; }
        }

        public static string MySqlProUser
        {
            get { return WebConfigurationManager.AppSettings["MySqlProUser"]; }
        }

        public static string MySqlProPwd
        {
            get { return WebConfigurationManager.AppSettings["MySqlProPwd"]; }
        }


      

      
       
       

        //public static string DEMOLib
        //{
        //    get { return WebConfigurationManager.AppSettings["DEMOLib"]; }
        //}

        //public static string DEMOPGMLib
        //{
        //    get { return WebConfigurationManager.AppSettings["DEMOPGMLib"]; }
        //}

        public static string GNPGMLib
        {
            get { return WebConfigurationManager.AppSettings["XLib"]; }
        }

        public static string GNLib
        {
            get { return WebConfigurationManager.AppSettings["XLib"]; }
        }

        public static string GeneralLib
        {
            get { return WebConfigurationManager.AppSettings["XLib"]; }
        }

        public static string BLLib
        {
            get { return WebConfigurationManager.AppSettings["XLib"]; }
        }

        public static string CSLib
        {
            get { return WebConfigurationManager.AppSettings["XLib"]; }
        }

        public static string CSPGMLib
        {
            get { return WebConfigurationManager.AppSettings["XLib"]; }
        }

        public static string ILLib
        {
            get { return WebConfigurationManager.AppSettings["XLib"]; }
        }
        public static string ILPGMLib
        {
            get { return WebConfigurationManager.AppSettings["XLib"]; }
        }

        public static string HMLib
        {
            get { return WebConfigurationManager.AppSettings["XLib"]; }
        }
        public static string HMPGMLib
        {
            get { return WebConfigurationManager.AppSettings["XLib"]; }
        }

        public static string PHLib
        {
            get { return WebConfigurationManager.AppSettings["XLib"]; }
        }
        public static string PHPGMLib
        {
            get { return WebConfigurationManager.AppSettings["XLib"]; }
        }
        public static string PLLib
        {
            get { return WebConfigurationManager.AppSettings["XLib"]; }
        }

        public static string PLPGMLib
        {
            get { return WebConfigurationManager.AppSettings["XLib"]; }
        }

        public static string PMLib
        {
            get { return WebConfigurationManager.AppSettings["XLib"]; }
        }
        public static string PMPGMLib
        {
            get { return WebConfigurationManager.AppSettings["XLib"]; }
        }

        public static string PWLib
        {
            get { return WebConfigurationManager.AppSettings["XLib"]; }
        }
        public static string PWPGMLib
        {
            get { return WebConfigurationManager.AppSettings["XLib"]; }
        }

        public static string RLLib
        {
            get { return WebConfigurationManager.AppSettings["XLib"]; }
        }
        public static string RLPGMLib
        {
            get { return WebConfigurationManager.AppSettings["XLib"]; }
        }

        public static string HPLib
        {
            get { return WebConfigurationManager.AppSettings["XLib"]; }
        }

        public static string HPLibR
        {
            get { return WebConfigurationManager.AppSettings["XLib"]; }
        }
        //
        public static string HPLibBR101
        {
            get { return WebConfigurationManager.AppSettings["XLib"]; }
        }
        public static string HPLibBR102
        {
            get { return WebConfigurationManager.AppSettings["XLib"]; }
        }
        public static string HPLibBR103
        {
            get { return WebConfigurationManager.AppSettings["XLib"]; }
        }
        public static string HPLibBR201
        {
            get { return WebConfigurationManager.AppSettings["XLib"]; }
        }
        public static string HPLibBR202
        {
            get { return WebConfigurationManager.AppSettings["XLib"]; }
        }
        public static string HPLibBR210
        {
            get { return WebConfigurationManager.AppSettings["XLib"]; }
        }
        public static string HPLibBR301
        {
            get { return WebConfigurationManager.AppSettings["XLib"]; }
        }
        public static string HPLibBR302
        {
            get { return WebConfigurationManager.AppSettings["XLib"]; }
        }
        public static string HPLibBR401
        {
            get { return WebConfigurationManager.AppSettings["XLib"]; }
        }
        //
        public static string HPPGMLib
        {
            get { return WebConfigurationManager.AppSettings["XLib"]; }
        }


        public static string QTempLib
        {
            get { return WebConfigurationManager.AppSettings["XLib"]; }
        }
        public static string CTLib
        {
            get { return WebConfigurationManager.AppSettings["XLib"]; }
        }
        public static string SystemLib
        {
            get { return WebConfigurationManager.AppSettings["XLib"]; }
        }

        //Peak 31/07/2550 เพิ่ม Lib อีก 1 Lib
        public static string SystemTestLib
        {
            get { return WebConfigurationManager.AppSettings["XLib"]; }
        }

        public static string AS400PrinterName
        {
            get { return WebConfigurationManager.AppSettings["AS400PrinterName"]; }
        }

        public static string EasybuyAgent1
        {
            get { return WebConfigurationManager.AppSettings["easybuyAgent1"]; }
        }

        public static string EasybuyAgent2
        {
            get { return WebConfigurationManager.AppSettings["easybuyAgent2"]; }
        }

        public static string DateTimeFormat
        {
            get { return WebConfigurationManager.AppSettings["DefaultDateTimeFormat"]; }
        }

        public static string DataGridImagesURLPath()
        {
            string nameStr = WebConfigurationManager.AppSettings["VirtualName"];
            if ((nameStr == null) || (nameStr == ""))
                return WebConfigurationManager.AppSettings["DataGridImagesPath"].ToString();
            else
                return "/" + nameStr + WebConfigurationManager.AppSettings["DataGridImagesPath"].ToString();

            return string.Empty;
        }

        public static int Language
        {
            get
            {
                string strLang = WebConfigurationManager.AppSettings["CaptionLanguage"];
                strLang = strLang.ToUpper();

                switch (strLang)
                {
                    case "THAI":
                        return 1;
                    case "NATION1":
                        return 2;
                    case "NATION2":
                        return 3;
                    case "NATION3":
                        return 4;
                    default: // ENG - English
                        return 0;
                }
            }
        }
    }

}