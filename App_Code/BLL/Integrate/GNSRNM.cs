using EB_Service.Commons;
using ILSystem.App_Code.Commons;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace ILSystem.App_Code.BLL.Integrate
{
    public class GNSRNM
    {
        public bool Call_GNSRNM(string prmName, string prmSurname, string prmLang, ref string prmError, ref string prmErrorMsg,
                        string strBizInit, string strBranchNo)
        {
            try
            {
                if(string.IsNullOrEmpty(prmName) || string.IsNullOrEmpty(prmSurname) || prmName.Trim() == "." || prmSurname.Trim() == ".")
                {
                    prmError = "Y";
                    prmErrorMsg = "INCOMPLETE DATA (NAME OR SURNAME)";
                    return false;
                }
                if (prmLang.ToUpper() == "E")
                {
                    if (!IsEnglish(prmName))
                    {
                        prmError = "Y";
                        prmErrorMsg = "INVALID NAME";
                        return false;
                    }
                    if (!IsEnglish(prmSurname))
                    {
                        prmError = "Y";
                        prmErrorMsg = "INVALID SURNAME'";
                        return false;
                    }
                }
                else if (prmLang.ToUpper() == "T")
                {
                    if (!IsThai(prmName))
                    {
                        prmError = "Y";
                        prmErrorMsg = "INVALID NAME";
                        return false;
                    }
                    if (!IsThai(prmSurname))
                    {
                        prmError = "Y";
                        prmErrorMsg = "INVALID SURNAME'";
                        return false;
                    }
                }
                else
                {
                    prmError = "Y";
                    prmErrorMsg = "INCOMPLETE LANGUAGE(TH OR ENG)";
                    return false;
                }
                return true;
            }
            catch(Exception ex)
            {
                Utility.WriteLog(ex);
                return false;
            }
        }

        static bool IsThai(string input)
        {
            // Regular expression สำหรับตรวจสอบภาษาไทย
            Regex regex = new Regex(@"^[\u0E00-\u0E7F\s]+$");
            return regex.IsMatch(input);
        }
        static bool IsEnglish(string input)
        {
            // Regular expression สำหรับตรวจสอบภาษาอังกฤษ, '.', ' ', และ '-'
            Regex regex = new Regex(@"^[A-Za-z\s.\-]+$");
            return regex.IsMatch(input);
        }
    }
}