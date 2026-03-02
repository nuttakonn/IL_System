using EB_Service.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ILSystem.App_Code.BLL.Integrate
{
    public class GNP0221
    {
        public bool Call_GNP0221(string prmDate, ref string prmError, string strBizInit, string strBranchNo)
        {
            try
            {
                // ตรวจสอบว่า string มีความยาว 8 ตัวอักษรหรือไม่
                if (prmDate.Length != 8)
                {
                    prmError = "Y";
                    return false;
                }

                // แยกส่วนวัน, เดือน และปี
                string dayPart = prmDate.Substring(0, 2);
                string monthPart = prmDate.Substring(2, 2);
                string yearPart = prmDate.Substring(4, 4);

                // แปลงปี พ.ศ. เป็น ค.ศ.
                if (int.TryParse(yearPart, out int year))
                {
                    year -= 543; // แปลงจาก พ.ศ. เป็น ค.ศ.
                    if (year < 1900 || year > 2500)
                    {
                        prmError = "Y";
                        return false;
                    }
                }
                else
                {
                    prmError = "Y";
                    return false;
                }

                // ตรวจสอบความถูกต้องของวันและเดือน
                if (int.TryParse(dayPart, out int day) && int.TryParse(monthPart, out int month))
                {
                    // สร้างวันที่
                    DateTime dateTime;
                    dateTime = new DateTime(year, month, day);
                    return true; // วันที่ถูกต้อง
                }
                prmError = "Y";
                return false; // หากไม่สามารถแปลงวันหรือเดือน
            }
            catch (Exception ex)
            {
                Utility.WriteLog(ex);
                prmError = "Y";
                return false; // วันที่ไม่ถูกต้อง
            }
            
        }
    }
}