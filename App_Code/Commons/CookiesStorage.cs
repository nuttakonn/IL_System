using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace ILSystem.App_Code.Commons
{
    public class CookiesStorage
    {
        public DataSet GetCookiesDataSetByKey(string key)
        {
            DataSet ds = new DataSet();
            if (HttpContext.Current.Request.Cookies[key]?.Value != null)
            {
                string dataCampaign = HttpContext.Current.Request.Cookies[key]?.Value;
                string decodedValue = HttpUtility.UrlDecode(dataCampaign, System.Text.Encoding.UTF8);
                if (!string.IsNullOrEmpty(decodedValue))
                {
                    var dt = JsonConvert.DeserializeObject<DataTable>(decodedValue);
                    ds.Tables.Add(dt);
                }
            }
            else
            {
                ds = SetDataSetNull();
            }

            return ds;
        }

        public DataTable GetCookiesDataTableByKey(string key)
        {
            DataTable dt = new DataTable();
            if (HttpContext.Current.Request.Cookies[key]?.Value != null)
            {
                string dataCampaign = HttpContext.Current.Request.Cookies[key]?.Value;
                string decodedValue = HttpUtility.UrlDecode(dataCampaign, System.Text.Encoding.UTF8);
                if (!string.IsNullOrEmpty(decodedValue))
                {
                    dt = JsonConvert.DeserializeObject<DataTable>(decodedValue);
                }
            }
            else
            {
                SetCookiesDataTableByName(key, dt);
            }

            return dt;
        }

        public bool GetCookiesBoolByKey(string key)
        {
            bool flag = false;
            if (HttpContext.Current.Request.Cookies[key]?.Value != null)
            {
                string dataCampaign = HttpContext.Current.Request.Cookies[key]?.Value;
                string decodedValue = HttpUtility.UrlDecode(dataCampaign, System.Text.Encoding.UTF8);
                if (!string.IsNullOrEmpty(decodedValue))
                {
                    flag = JsonConvert.DeserializeObject<bool>(decodedValue);
                }
            }
            else
            {
                SetCookiesBoolByName(key, flag);
            }

            return flag;
        }

        public string GetCookiesStringByKey(string key)
        {
            string data = string.Empty;
            if (HttpContext.Current.Request.Cookies[key]?.Value != null)
            {
                data = HttpContext.Current.Request.Cookies[key]?.Value;
            }
            return data;
        }

        public void SetCookiesDataSetByName(string key, DataSet ds)
        {
            try
            {
                if (check_dataset(ds))
                {
                    string json = DataTableToJsonObj(ds.Tables[0]);
                    string encodedValue = HttpUtility.UrlEncode(json, System.Text.Encoding.UTF8);
                    HttpCookie cookie = new HttpCookie(key, encodedValue);
                    cookie.Expires = DateTime.Now.AddMinutes(720);
                    HttpContext.Current.Response.Cookies.Add(cookie);
                }
                else
                {
                    HttpCookie cookie = new HttpCookie(key, JsonConvert.SerializeObject(ds));
                    cookie.Expires = DateTime.Now.AddMinutes(720);
                    HttpContext.Current.Response.Cookies.Add(cookie);
                }

            }
            catch (Exception)
            {
                throw;
            }

        }
        public string SetHisdenValueByDataSet(DataSet ds)
        {
            try
            {
                if (ds.Tables.Count > 0)
                {
                     string json = DataTableToJsonObj(ds.Tables[0]);
                    return json;
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception)
            {
                return string.Empty;
            }

        }

        public void SetCookiesDataTableByName(string key, DataTable dt)
        {
            try
            {
                string json = JsonConvert.SerializeObject(dt);
                string encodedValue = HttpUtility.UrlEncode(json, System.Text.Encoding.UTF8);
                HttpCookie cookie = new HttpCookie(key, encodedValue);
                cookie.Expires = DateTime.Now.AddMinutes(720);
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
            catch (Exception)
            {
                throw;
            }

        }
        public void SetCookiesBoolByName(string key, bool flag)
        {
            try
            {
                string json = JsonConvert.SerializeObject(flag);
                string encodedValue = HttpUtility.UrlEncode(json, System.Text.Encoding.UTF8);
                HttpCookie cookie = new HttpCookie(key, encodedValue);
                cookie.Expires = DateTime.Now.AddMinutes(720);
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
            catch (Exception)
            {
                throw;
            }

        }
        public void SetCookiesStringByName(string key, string value)
        {
            try
            {
                string json = JsonConvert.SerializeObject(value);
                string encodedValue = HttpUtility.UrlEncode(json, System.Text.Encoding.UTF8);
                HttpCookie cookie = new HttpCookie(key, encodedValue);
                cookie.Expires = DateTime.Now.AddMinutes(720);
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
            catch (Exception)
            {
                throw;
            }

        }
        public string DataTableToJsonObj(DataTable dt)
        {
            DataSet ds = new DataSet();
            ds.Merge(dt);
            StringBuilder JsonString = new StringBuilder();

            if (ds.Tables[0].Rows.Count > 0)
            {
                return JsonConvert.SerializeObject(dt);
            }
            else
            {
                JsonString.Append("[");
                JsonString.Append("{");
                foreach (var cl in ds.Tables[0].Columns)
                {
                    JsonString.Append("\"" + cl.ToString() + "\":" + "\"" + string.Empty + "\",");
                }
                JsonString.Append("}");
                JsonString.Append("]");
                return JsonString.ToString();
            }
        }

        public DataSet SetDataSetNull()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            if (dt.Rows.Count <= 0)
            {
                ds.Tables.Add(dt);
            }
            return ds;
        }

        public string JsonSerializeObjectHiddenDataSet(DataSet ds)
        {
            if (!check_dataset(ds))
            {
                return string.Empty;
            }
            return JsonConvert.SerializeObject(ds);
        }
        public string JsonSerializeObjectHiddenDataDataTable(DataTable dt)
        {
            if (dt.Rows.Count <= 0)
            {
                return string.Empty;
            }
            return JsonConvert.SerializeObject(dt);
        }

        public DataSet JsonDeserializeObjectHiddenDataSet(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                return new DataSet();
            }
            return JsonConvert.DeserializeObject<DataSet>(json);
        }
        public DataTable JsonDeserializeObjectHiddenDataTable(string json)
        {
            try
            {
                if (string.IsNullOrEmpty(json))
                {
                    return new DataTable();
                }
                DataTable dt = new DataTable();
                dt = JsonConvert.DeserializeObject<DataTable>(json);
                return dt;
            }
            catch (Exception ex)
            {
                return new DataTable();
            }
           
        }

        public void ClearCookies(string listcookies)
        {
            var listName = listcookies.Split(',');
            foreach (var name in listName)
            {
                if (HttpContext.Current.Request.Cookies[name] != null)
                {
                    HttpCookie cookies = HttpContext.Current.Request.Cookies[name];
                    cookies.Expires = DateTime.Now.AddDays(-1);
                    HttpContext.Current.Response.Cookies.Add(cookies);
                }
            }
            
        }
        public void ClearMaxLargeCookies(string listcookies)
        {
            var listName = listcookies.Split(',');
            foreach (var name in listName)
            {
                int i = 0;
                if (HttpContext.Current.Request.Cookies[$"{name}_{i}"] != null)
                {
                    while (HttpContext.Current.Request.Cookies[$"{name}_{i}"] != null)
                    {
                        HttpCookie cookies = HttpContext.Current.Request.Cookies[$"{name}_{i}"];
                        cookies.Expires = DateTime.Now.AddDays(-1);
                        HttpContext.Current.Response.Cookies.Add(cookies);
                        i++;
                    }
                }
            }
            

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
        public bool check_dataTable(DataTable dt)
        {
            try
            {
                if (dt == null)
                {
                    return false;
                }
                
                if (dt.Rows.Count == 0)
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

        public void SetCookMaxLargeCookie(string key, DataSet ds, int maxLength = 2048)
        {
            try
            {
                if (check_dataset(ds))
                {

                    string chunkName = string.Empty;
                    string value = DataTableToJsonObj(ds.Tables[0]);
                    var bytLength = System.Text.ASCIIEncoding.ASCII.GetByteCount(value);
                    int chunkSize = maxLength - key.Length - 20; // Reserve some space for additional metadata
                    int totalChunks = (int)Math.Ceiling((double)bytLength / chunkSize);

                    for (int i = 0; i < totalChunks; i++)
                    {
                        chunkName = $"{key}_{i}";  // Name cookies with _0, _1, etc.
                        string chunkValue = value.Substring(i * chunkSize, Math.Min(chunkSize, value.Length - (i * chunkSize)));

                        string encodedValue = HttpUtility.UrlEncode(chunkValue, System.Text.Encoding.UTF8);
                        HttpCookie cookie = new HttpCookie(chunkName, encodedValue);
                        cookie.Expires = DateTime.Now.AddMinutes(720);
                        HttpContext.Current.Response.Cookies.Add(cookie);
                    }

                    
                }
                else
                {
                    HttpCookie cookie = new HttpCookie(key, JsonConvert.SerializeObject(ds));
                    cookie.Expires = DateTime.Now.AddMinutes(720);
                    HttpContext.Current.Response.Cookies.Add(cookie);
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataSet GetCookMaxLargeCookie(string key)
        {
            DataSet ds = new DataSet();
            List<string> chunks = new List<string>();
            int i = 0;
            string dataCampaign = "";
            if (HttpContext.Current.Request.Cookies[$"{key}_{i}"]?.Value != null)
            {
                if (HttpContext.Current.Request.Cookies[$"{key}_{i}"] != null)
                {
                    while (HttpContext.Current.Request.Cookies[$"{key}_{i}"] != null)
                    {
                        chunks.Add(HttpContext.Current.Request.Cookies[$"{key}_{i}"].Value);
                        i++;
                    }
                    dataCampaign = string.Join("", chunks);
                }
                string decodedValue = HttpUtility.UrlDecode(dataCampaign, System.Text.Encoding.UTF8);
                if (!string.IsNullOrEmpty(decodedValue))
                {
                    var dt = JsonConvert.DeserializeObject<DataTable>(decodedValue);
                    ds.Tables.Add(dt);
                }
            }
            else
            {
                ds = SetDataSetNull();
            }

            return ds;
        }

        //public void SetMaxLargeLocalStorage(string key, DataSet ds)
        //{
        //    var storage = new LocalStorage();
        //    try
        //    {
        //        if (check_dataset(ds))
        //        {
        //            string json = DataTableToJsonObj(ds.Tables[0]);
        //            string encodedValue = HttpUtility.UrlEncode(json, System.Text.Encoding.UTF8);
        //            storage.Store(key, encodedValue);
        //        }
        //        else
        //        {
        //            storage.Store(key, string.Empty);
        //        }

        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

    }
}