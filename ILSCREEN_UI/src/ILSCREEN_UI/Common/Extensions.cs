using ILSCREEN_UI.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System.Data;
using System.Globalization;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace ILSCREEN_UI.Common
{
    public static class Extensions
    {
        public static string Base64Encode(this string value)
        {
            try
            {
                var strbyte = Encoding.UTF8.GetBytes(value);
                var base64EncodedBytes = Convert.ToBase64String(strbyte);
                return base64EncodedBytes;
            }
            catch
            {
                // ignored
            }

            return value;
        }

        public static string Base64Decode(this string value)
        {
            try
            {
                var base64EncodedBytes = Convert.FromBase64String(value);
                return Encoding.UTF8.GetString(base64EncodedBytes);
            }
            catch (Exception)
            {
                // ignored
            }
            return value;
        }
        public static bool IsBase64String(this string base64)
        {
            if (string.IsNullOrEmpty(base64) || base64.Length % 4 != 0 || base64.Contains(" ") || base64.Contains("\t") || base64.Contains("\n") || !base64.Contains("="))
            {
                return false;
            }
            try
            {
                _ = Convert.FromBase64String(base64);
                return true;
            }
            catch (Exception)
            {
                // 
            }
            return false;
        }

        public static string PBKDF2Encrypt(this string text, string key = "ESB")
        {
            byte[] salt1 = new byte[8];
            using (RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetBytes(salt1);
            }

            int myIterations = 1000;
            Rfc2898DeriveBytes k1 = new Rfc2898DeriveBytes(key, salt1, myIterations);

            TripleDES encAlg = TripleDES.Create();
            encAlg.Key = k1.GetBytes(16);

            var encryptionStream = new MemoryStream();
            CryptoStream encrypt = new CryptoStream(encryptionStream, encAlg.CreateEncryptor(), CryptoStreamMode.Write);

            byte[] utfD1 = Encoding.UTF8.GetBytes(text);

            encrypt.Write(utfD1, 0, utfD1.Length);
            encrypt.FlushFinalBlock();
            encrypt.Close();

            byte[] edata1 = encryptionStream.ToArray();
            k1.Reset();

            return $"{System.Convert.ToBase64String(salt1)}|{System.Convert.ToBase64String(encAlg.IV)}|{System.Convert.ToBase64String(edata1)}";
        }

        public static string PBKDF2Decrypt(this string eText)
        {
            var dText = eText.Split('|');
            var salt1 = Convert.FromBase64String(dText[0]);
            var iv = Convert.FromBase64String(dText[1]);
            var edata1 = Convert.FromBase64String(dText[2]);
            var key = "ESB";

            Rfc2898DeriveBytes k2 = new Rfc2898DeriveBytes(key, salt1);
            TripleDES decAlg = TripleDES.Create();
            decAlg.Key = k2.GetBytes(16);
            decAlg.IV = iv;
            MemoryStream decryptionStreamBacking = new MemoryStream();
            CryptoStream decrypt = new CryptoStream(decryptionStreamBacking, decAlg.CreateDecryptor(), CryptoStreamMode.Write);
            decrypt.Write(edata1, 0, edata1.Length);
            decrypt.Flush();
            decrypt.Close();
            k2.Reset();

            string data2 = new UTF8Encoding(false).GetString(decryptionStreamBacking.ToArray());
            return data2;
        }

        public static T ConvertToModel<T>(this object dt)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                FloatFormatHandling = FloatFormatHandling.DefaultValue,
            };
            var strDt = JsonConvert.SerializeObject(dt);
            return JsonConvert.DeserializeObject<T>(strDt, settings);
        }

        public static T Clone<T>(this T obj)
        {
            if (ReferenceEquals(obj, null))
            {
                return default;
            }
            var strobj = JsonConvert.SerializeObject(obj);
            return JsonConvert.DeserializeObject<T>(strobj);
        }
        public static List<T> Clones<T>(this List<T> obj)
        {
            if (ReferenceEquals(obj, null))
            {
                return default;
            }
            var strobj = JsonConvert.SerializeObject(obj);
            return JsonConvert.DeserializeObject<List<T>>(strobj);
        }

        public static bool TryGetQueryString<T>(this NavigationManager navManager, string key, out T value)
        {
            var uri = navManager.ToAbsoluteUri(navManager.Uri);

            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue(key, out var valueFromQueryString))
            {
                if (typeof(T) == typeof(int) && int.TryParse(valueFromQueryString, out var valueAsInt))
                {
                    value = (T)(object)valueAsInt;
                    return true;
                }

                if (typeof(T) == typeof(string))
                {
                    value = (T)(object)valueFromQueryString.ToString();
                    return true;
                }

                if (typeof(T) == typeof(decimal) && decimal.TryParse(valueFromQueryString, out var valueAsDecimal))
                {
                    value = (T)(object)valueAsDecimal;
                    return true;
                }
            }

            value = default;
            return false;
        }

        public static string GetQueryString(this NavigationManager navManager, string key)
        {
            var uri = navManager.ToAbsoluteUri(navManager.Uri);

            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue(key, out var valueFromQueryString))
            {
                return valueFromQueryString.ToString();
            }

            return null;
        }

        public static string ConvertDataTableToHTML(this DataTable dt)
        {
            string html = "<table class='table'>";
            //add header row
            html += "<tr>";
            for (int i = 0; i < dt.Columns.Count; i++)
                html += "<td>" + dt.Columns[i].ColumnName + "</td>";
            html += "</tr>";
            //add rows
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                html += "<tr>";
                for (int j = 0; j < dt.Columns.Count; j++)
                    html += "<td>" + dt.Rows[i][j].ToString() + "</td>";
                html += "</tr>";
            }
            html += "</table>";
            return html;
        }
        public static bool IsAnyNullOrEmpty(this object myObject)
        {
            if (myObject == null) return true;
            foreach (PropertyInfo pi in myObject.GetType().GetProperties())
            {
                if (pi.PropertyType == typeof(string))
                {
                    string value = (string)pi.GetValue(myObject);
                    if (string.IsNullOrEmpty(value))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool IsAny(this object myObject)
        {
            foreach (PropertyInfo pi in myObject.GetType().GetProperties())
            {
                if (pi.PropertyType == typeof(string))
                {
                    string value = (string)pi.GetValue(myObject);
                    if (!string.IsNullOrEmpty(value))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static object GetValueProperty(this object myObject, string propertyName)
        {
            try
            {
                var prop = myObject.GetType().GetProperties();
                return prop.FirstOrDefault(x => x.Name.ToLower() == propertyName.ToLower()).GetValue(myObject);
            }
            catch { return null; }
        }

        public static string GetBetweenString(this string strSource, string strStart, string strEnd)
        {
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                int Start, End;
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }
            return "";
        }

        public static async Task<T> CheckResult<T>(this ApiServiceResponseModel<ResponseModel> serviceResult)
        {
            if (!serviceResult.IsSuccessStatusCode)
            {
                throw new InvalidOperationException(serviceResult.ReturnMessage);
            }

            if (!serviceResult.Entity.success)
            {
                throw new InvalidOperationException($"Service : {serviceResult.RequestUri} | Message : {serviceResult.Entity.message}");
            }

            //return await Task.FromResult(esbUtil.Common.MapValue<T, object>(serviceResult.Entity.data)); //ของเดิม ใช้ Lib Easybuy.Utility
            return await Task.FromResult(ConvertToModel<T>(serviceResult.Entity.data));
        }

        public static byte[] GetFileArray(this IFormFile file)
        {
            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                return ms.ToArray();
            }
        }

        static public string ToCheckNull(this string input)
        {
            try
            {
                var resp = "";
                if (input != null)
                {
                    resp = input;
                }
                return resp;
            }
            catch (Exception ex)
            {
                throw new Exception($"Cannot convert Null to Empty {ex} {input}");
            }
        }
    }

    public static class StringExtension
    {
        static public bool IsValidThaiAlphabet(this string str)
        {
            Regex rgx = new Regex("[ก-ฮะ-์]");
            string result = rgx.Replace(str, "").Trim();
            return string.IsNullOrEmpty(result);
        }

        static public bool IsValidEnglishAlphabet(this string str)
        {
            Regex rgx = new Regex(@"^[A-Za-z]+");
            string result = rgx.Replace(str, "").Trim();
            return string.IsNullOrEmpty(result);
        }

        static public bool BuddhistDateStringToDate(this string dateString, ref DateTime date, string dateFormat = "yyyyMMdd")
        {
            try
            {
                if ("th".Equals(CultureInfo.CurrentCulture.ToString().Substring(0, 2)))
                {
                    date = DateTime.ParseExact(dateString, dateFormat, null);
                }
                else
                {
                    date = DateTime.ParseExact(dateString, dateFormat, new CultureInfo("th-TH"));
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Cannot convert string to date {ex} {dateString}");
            }
        }
    }

    public static class EntitesExtension
    {
        static public bool IsEmpty<T>(this IEnumerable<T> obj)
        {
            if (obj?.Any() != true)
            {
                return true;
            }
            return false;
        }
    }

    public static class DateTimeExtension
    {
        private static bool isBuddhistFormat = $"th".Equals(CultureInfo.CurrentCulture.ToString().Substring(0, 2));
        public static string ToBuddhistDateTimeStringFormat(this DateTime? dateTime, string format = "dd/MM/yyyy HH:mm:ss")
        {
            if (dateTime == null)
                return null;

            if (!dateTime.HasValue)
                return string.Empty;

            if (isBuddhistFormat)
                return dateTime.Value.ToString(format);


            Thread.CurrentThread.CurrentCulture = new CultureInfo("th-TH");
            string result = dateTime.Value.ToString(format);
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            return result;
        }

        public static string ToUniversalDateTimeStringFormat(this DateTime? dateTime, string format = "dd/MM/yyyy HH:mm:ss")
        {
            if (dateTime == null)
                return null;

            if (!dateTime.HasValue)
                return string.Empty;

            if (!isBuddhistFormat)
                return dateTime.Value.ToString(format);


            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            string result = dateTime.Value.ToString(format);
            Thread.CurrentThread.CurrentCulture = new CultureInfo("th-TH");
            return result;
        }

        //static public string ToBuddhistDateTimeString(this DateTime date, string dateFormat = "yyyyMMdd", string timeFormat = "HH:mm:ss")
        //{
        //    try
        //    {
        //        if (string.IsNullOrWhiteSpace(timeFormat))
        //        {
        //            return date.ToBuddhistDateString(dateFormat);
        //        }
        //        else
        //        {
        //            return date.ToString($"{dateFormat} {timeFormat}", CultureInfo.CreateSpecificCulture("th-TH"));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception($"Cannot Convert DateTime to Buddhist DateTime String {ex} {date.ToString()}");
        //    }
        //}

        //static public string ToUniversalDateTimeString(this DateTime date, string dateFormat = "yyyyMMdd", string timeFormat = "HH:mm:ss")
        //{
        //    try
        //    {
        //        if (string.IsNullOrWhiteSpace(timeFormat))
        //        {
        //            return date.ToUniversalDateString(dateFormat);
        //        }
        //        else
        //        {
        //            return date.ToString($"{dateFormat} {timeFormat}", CultureInfo.CreateSpecificCulture("en-US"));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception($"Cannot Convert DateTime to Universal DateTime String {ex} {date.ToString()}");
        //    }
        //}

        static public DateTime ToBuddhistDateTime(this DateTime date)
        {
            try
            {
                return date.AddYears(543);
            }
            catch (Exception ex)
            {
                throw new Exception($"Cannot Convert DateTime to Buddhist DateTime {ex} {date.ToString()}");
            }
        }

        static public DateTime ToUniversalDateTime(this DateTime date)
        {
            try
            {
                return date.AddYears(-543);
            }
            catch (Exception ex)
            {
                throw new Exception($"Cannot Convert DateTime to Universal DateTime {ex} {date.ToString()}");
            }
        }

        static public string ToThDateStrToEnDateStr(this string buddhistDate)
        {
            try
            {
                var dd1 = buddhistDate.Substring(0, 2);
                var mm1 = buddhistDate.Substring(3, 2);
                var y1 = Convert.ToInt16(buddhistDate.Substring(6, 4));
                var year1 = (y1 - 543).ToString();
                var dateStr = year1 + "-" + mm1 + "-" + dd1;
                DateTime conDate = Convert.ToDateTime(dateStr);
                var resp = conDate.ToString("yyyy-MM-dd");
                return resp;
            }
            catch (Exception ex)
            {
                throw new Exception($"Cannot convert string to date {ex} {buddhistDate}");
            }
        }


    }
}
