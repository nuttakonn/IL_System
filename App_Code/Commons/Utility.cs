using ESB.WebAppl.ILSystem.commons;
using ILSystem.App_Code.Commons;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Compilation;
using System.Web.Configuration;
using WebPasswordEncryption;

namespace EB_Service.Commons
{
    public static class Utility
    {
        public static string GetHandlerPath(Type type)
        {
            HttpHandlersSection httpHandlersSection = (HttpHandlersSection)WebConfigurationManager.GetSection("system.web/httpHandlers");

            foreach (HttpHandlerAction httpHandlerAction in httpHandlersSection.Handlers)
            {
                Type httpHandlerActionType = BuildManager.GetType(httpHandlerAction.Type, true);

                if (type.IsAssignableFrom(httpHandlerActionType))
                {
                    return httpHandlerAction.Path;
                }
            }

            string message = string.Format("No HTTP handler defined for '{0}'.", type);
            throw new ArgumentOutOfRangeException(message);
        }

        public static string GetUrl(string baseUrl, NameValueCollection parameters)
        {
            return baseUrl + "?" + UrlEncode(parameters);
        }

        public static string GetUrl(string baseUrl, string param1Name, object param1Value)
        {
            NameValueCollection nvc = new NameValueCollection(1);
            nvc.Add(param1Name, param1Value == null ? null : param1Value.ToString());

            return GetUrl(baseUrl, nvc);
        }

        public static string UrlEncode(NameValueCollection parameters)
        {
            StringBuilder stringBuilder = new StringBuilder();
            bool first = true;
            foreach (string parameter in parameters)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    stringBuilder.Append("&");
                }
                stringBuilder.Append(parameter);
                stringBuilder.Append("=");

                string value = parameters[parameter];
                if (value != null)
                {
                    string encodedValue = HttpUtility.UrlEncode(value);
                    encodedValue = encodedValue.Replace("+", "%20");
                    stringBuilder.Append(encodedValue);
                }
            }
            return stringBuilder.ToString();
        }

        public static string EncryptPassWord(string Pass)
        {
            ClassPasswordEncryption enc_Password = new ClassPasswordEncryption();
            return enc_Password.Encrypt_Password(Pass);
        }

        public static string DecryptPassWord(string Pass)
        {
            ClassPasswordEncryption enc_Password = new ClassPasswordEncryption();
            return enc_Password.Decrypt_Password(Pass);
        }

        //BOT Report
        public static string Base64Decode(string val)
        {
            try
            {
                var base64EncodedBytes = Convert.FromBase64String(val);
                return Encoding.UTF8.GetString(base64EncodedBytes);
            }
            catch (Exception)
            {
            }
            return val;
        }
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }
        public static void WriteLog(Exception ex)
        {
            try
            {
                if (!Convert.ToBoolean(WebConfigurationManager.AppSettings["WriteLog"].ToString()))
                {
                    return;
                }
                string sLogFormat;
                sLogFormat = DateTime.Now.ToString("dd/MM/yyyy") + " " + DateTime.Now.ToLongTimeString().ToString() + " => ";
                var st = new StackTrace(ex, true);
                var frames = st.GetFrames();
                var traceString = new StringBuilder();

                foreach (var frame in frames)
                {
                    if (frame.GetFileLineNumber() < 1)
                        continue;
                    traceString.Append(" File: " + frame.GetFileName());
                    traceString.Append(", Method:" + frame.GetMethod().Name);
                    traceString.Append(", LineNumber: " + frame.GetFileLineNumber());
                    traceString.Append(", Message: " + ex.Message.ToString());
                    traceString.Append(", InnerException: " + (ex.InnerException != null ? ex.InnerException.ToString() : "null"));
                    traceString.Append("  -->  ");
                }
                if (!Directory.Exists(WebConfigurationManager.AppSettings["PathLog"].ToString()))
                {
                    Directory.CreateDirectory(WebConfigurationManager.AppSettings["PathLog"].ToString());
                }
                string PathFileName = GetPathFileName();
                var strSpliter = PathFileName.Split('_', '.');
                var counter = strSpliter[2];
                if (File.Exists(PathFileName))
                {
                    FileInfo info = new FileInfo(PathFileName);
                    if (info.Length > Convert.ToInt32(WebConfigurationManager.AppSettings["FileLogSize"].ToString()))
                    {
                        PathFileName = GetPathFileName(Convert.ToInt32(counter) + 1, "ex");
                    }
                }
                if (!File.Exists(PathFileName))
                {
                    using (StreamWriter writer = File.CreateText(PathFileName))
                    {
                        writer.WriteLine(sLogFormat + traceString.ToString());
                    }
                }
                else
                {
                    using (StreamWriter writer = File.AppendText(PathFileName))
                    {
                        writer.WriteLine(sLogFormat + traceString.ToString());
                    }
                }
                //FileStream steam = null;
                //using (var mutex = new Mutex(false, ""))
                //{
                //    mutex.WaitOne();
                //    try
                //    {
                //        using (steam = new FileStream(PathFileName, FileMode.Append))
                //        {
                //            using (StreamWriter writer = new StreamWriter(steam, Encoding.UTF8))
                //            {
                //                writer.WriteLine(sLogFormat + traceString.ToString());
                //            }
                //            steam.Close();
                //        }
                //    }
                //    finally
                //    {
                //        mutex.ReleaseMutex();
                //    }

                //}
            }
            catch
            {

                return;
            }

        }
        public static void WriteLogString(string Error, string Query = "")
        {
            try
            {
                if (!Convert.ToBoolean(WebConfigurationManager.AppSettings["WriteLog"].ToString()))
                {
                    return;
                }
                string sLogFormat;
                sLogFormat = DateTime.Now.ToString("dd/MM/yyyy") + " " + DateTime.Now.ToLongTimeString().ToString() + " => ";
                var traceString = new StringBuilder();
                traceString.Append(" : " + Error);
                if (!string.IsNullOrEmpty(Query))
                    traceString.Append(", Query: " + Query);
                traceString.Append("  -->  ");
                if (!Directory.Exists(WebConfigurationManager.AppSettings["PathLog"].ToString()))
                {
                    Directory.CreateDirectory(WebConfigurationManager.AppSettings["PathLog"].ToString());
                }
                string PathFileName = GetPathFileName();
                var strSpliter = PathFileName.Split('_','.');
                var counter = strSpliter[2];
                if (File.Exists(PathFileName))
                {
                    FileInfo info = new FileInfo(PathFileName);
                    if (info.Length > Convert.ToInt32(WebConfigurationManager.AppSettings["FileLogSize"].ToString()))
                    {
                        PathFileName = GetPathFileName(Convert.ToInt32(counter)+1);
                    }
                }

                if (!File.Exists(PathFileName))
                {
                    using (StreamWriter writer = File.CreateText(PathFileName))
                    {
                        writer.WriteLine(sLogFormat + traceString.ToString());
                    }
                }
                else
                {
                    using (StreamWriter writer = File.AppendText(PathFileName))
                    {
                        writer.WriteLine(sLogFormat + traceString.ToString());
                    }
                }

                //FileStream steam = null;
                //using (var mutex = new Mutex(false, ""))
                //{
                //    mutex.WaitOne();
                //    try
                //    {
                //        using (steam = new FileStream(PathFileName, FileMode.Append))
                //        {
                //            using (StreamWriter writer = new StreamWriter(steam, Encoding.UTF8))
                //            {
                //                writer.WriteLine(sLogFormat + traceString.ToString());
                //            }
                //            steam.Close();
                //        }
                //    }
                //    finally
                //    {
                //        mutex.ReleaseMutex();

                //    }

                //}
            }
            catch 
            {
                return;
            }

        }
        public static void WriteLogResponse(string Action, string Respone = "")
        {
            try
            {
                if (!Convert.ToBoolean(WebConfigurationManager.AppSettings["WriteLog"].ToString()))
                {
                    return;
                }
                string sLogFormat;
                sLogFormat = DateTime.Now.ToString("dd/MM/yyyy") + " " + DateTime.Now.ToLongTimeString().ToString() + " => ";
                var traceString = new StringBuilder();
                traceString.Append(" Action: " + Action);
                if (!string.IsNullOrEmpty(Respone))
                    traceString.Append(", Respone Json: " + Respone);
                traceString.Append("  -->  ");
                if (!Directory.Exists(WebConfigurationManager.AppSettings["PathLog"].ToString()))
                {
                    Directory.CreateDirectory(WebConfigurationManager.AppSettings["PathLog"].ToString());
                }
                string PathFileName = GetPathFileName();
                var strSpliter = PathFileName.Split('_', '.');
                var counter = strSpliter[2];
                if (File.Exists(PathFileName))
                {
                    FileInfo info = new FileInfo(PathFileName);
                    if (info.Length > Convert.ToInt32(WebConfigurationManager.AppSettings["FileLogSize"].ToString()))
                    {

                        PathFileName = GetPathFileName(Convert.ToInt32(counter) + 1);
                    }
                }
                if (!File.Exists(PathFileName))
                {
                    using (StreamWriter writer = File.CreateText(PathFileName))
                    {
                        writer.WriteLine(sLogFormat + traceString.ToString());
                    }
                }
                else
                {
                    using (StreamWriter writer = File.AppendText(PathFileName))
                    {
                        writer.WriteLine(sLogFormat + traceString.ToString());
                    }
                }
                //FileStream steam = null;
                //using (var mutex = new Mutex(false, ""))
                //{
                //    mutex.WaitOne();
                //    try
                //    {
                //        using (steam = new FileStream(PathFileName, FileMode.Append))
                //        {
                //            using (StreamWriter writer = new StreamWriter(steam, Encoding.UTF8))
                //            {
                //                writer.WriteLine(sLogFormat + traceString.ToString());
                //            }
                //            steam.Close();
                //        }
                //    }
                //    finally
                //    {
                //        mutex.ReleaseMutex();
                //    }
                //}
            }
            catch
            {
                return;
            }
        }

        public static string GetPathFileName(int counter = 1,string type = "")
        {
            string sErrorTime;
            string sYear = DateTime.Now.Year.ToString();
            string sMonth = DateTime.Now.Month.ToString("00");
            string sDay = DateTime.Now.Day.ToString("00");
            string sHour = DateTime.Now.Hour.ToString("00");
            string user = GetUserNameFromcookies();
            sErrorTime = sYear + sMonth + sDay + sHour;
            if (!string.IsNullOrEmpty(user))
                sErrorTime += "_" + user ;

            sErrorTime += "_" + counter + "_" + type;
            
            
            string filename = "Log_" + sErrorTime + ".txt";
            string Pathfilename = WebConfigurationManager.AppSettings["PathLog"].ToString() + @"\" + filename;
            return Pathfilename;
        }
        public static void CheckSubroutineStatus(DataSet ds)
        {
            string isSuccess;
            foreach (DataColumn dc in ds.Tables[0].Columns)
            {
                isSuccess = dc.ColumnName.ToString();
                if (isSuccess.ToUpper().Contains("ISSUCCESS"))
                    throw new Exception(ds.Tables[0].Rows[0]["message"].ToString());

            }
        }
        public static string GetUserNameFromcookies()
        {
            string username = "";
            // ตรวจสอบว่ามี HttpContext อยู่หรือไม่
            try
            {
                if (HttpContext.Current != null && HttpContext.Current.Request != null)
                {
                    var httpCookie = HttpContext.Current.Request.Cookies["AccessKey"];
                    if (httpCookie != null)
                    {
                        var empid = Utility.Base64Decode(httpCookie["acn"].ToString()).Split('_')[0];
                        username = Utility.DecryptPassWord(empid);
                    }
                }
            }
            catch
            {
                username = string.Empty;

            }
            return username;
        }
        public static void CheckSessionFromaRedis()
        {
            var httpCookie = HttpContext.Current.Request.Cookies["AccessKey"];
            UserInfo _UserInfo = new UserInfo();
            if (httpCookie == null)
            {
                HttpContext.Current.Response.Redirect(WebConfigurationManager.AppSettings["SSOSignOutURL"].ToString().Trim());
            }
            var empid = Utility.Base64Decode(httpCookie["acn"].ToString()).Split('_')[0];
            var connect = new Connect();
            string url = WebConfigurationManager.AppSettings["SSOTokenServiceAPIURL"].ToString().Trim();
            string action = "/Token/GetRedisValueBykey/" + httpCookie["acn"].ToString();
            string response = connect.ConnectAPIReturnString(url, action, "", "GET");
            var userInfo = response.Split('|');
            _UserInfo.AccessKey = httpCookie["acn"].ToString();
            _UserInfo.Username = Utility.DecryptPassWord(empid);
            _UserInfo.Password = Utility.DecryptPassWord(userInfo[2]);
            _UserInfo.PortalID = 27;
            _UserInfo.RolesID = new ArrayList();
            _UserInfo.LocalClient = (System.Net.Dns.GetHostEntry(HttpContext.Current.Request.UserHostAddress)).HostName.Split('.')[0].ToUpper();
            HttpContext.Current.Session["UserInfo"] = _UserInfo;
        }
        public static DataTable ToDataTable<T>(this IList<T> data)
        {
            DataTable table = new DataTable();

            if (typeof(T).IsValueType || typeof(T).Equals(typeof(string)))
            {

                DataColumn dc = new DataColumn("Value", typeof(T));
                table.Columns.Add(dc);
                foreach (T item in data)
                {
                    DataRow dr = table.NewRow();
                    dr[0] = item;
                    table.Rows.Add(dr);
                }
            }
            else
            {
                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
                foreach (PropertyDescriptor prop in properties)
                {
                    table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                }
                foreach (T item in data)
                {
                    DataRow row = table.NewRow();
                    foreach (PropertyDescriptor prop in properties)
                    {
                        try
                        {
                            row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                        }
                        catch (Exception ex)
                        {
                            row[prop.Name] = DBNull.Value;
                        }
                    }
                    table.Rows.Add(row);
                }
            }
            return table;
        }
    }

    public struct Screenmode
    {
        public bool NewMode;
        public bool EditMode;
        public bool DecisionMode;
    }
}