using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
//using LegalSystem.Common.Model;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using ILSystem.App_Code.Model;

namespace ILSystem.App_Code.DAL
{
    public class APIServices
    {
        private readonly int _timeout;
        private readonly string _url;
        private readonly object _data;
        public APIServiceMethod _method;
        private readonly Hashtable _header;
        private readonly bool _useJwt;

        public APIServices(APIServiceOptions opts)
        {
            this._url = opts.url;
            this._header = opts.header;
            this._method = opts.method;
            this._data = opts.data;
            this._timeout = opts.timeout;
            this._useJwt = opts.useJwt;

        }

        public ResponseModel connectAPI()
        {
            try
            {
                ResponseModel responseModel = new ResponseModel();
                var httpWebRequest = HttpWebRequest.Create(this._url);

                if (_timeout > 0)
                {
                    httpWebRequest.Timeout = _timeout;
                }

                if (this._method != null)
                {
                    httpWebRequest.Method = this._method.ToString();
                }

                httpWebRequest.ContentType = "application/json";

                if (this._header != null)
                {
                    foreach (DictionaryEntry hash in this._header)
                    {
                        httpWebRequest.Headers[hash.Key.ToString()] = hash.Value.ToString();
                    }
                }

                if (this._method != APIServiceMethod.GET)
                {
                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        streamWriter.Write(this._data);
                    }
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    responseModel = JObject.Parse(streamReader.ReadToEnd()).ToObject<ResponseModel>();
                }

                return responseModel;

            }
            catch (WebException ex)
            {

                string corrId = Guid.NewGuid().ToString();
                List<string> msg = new List<string>();
                //Logger log = new Logger();
                msg.Add(DateTime.Now.ToString());
                msg.Add(String.Format("RequestID: {0}", corrId));
                msg.Add(String.Format("Request Method: {0}", this._method.ToString()));
                msg.Add(String.Format("Request Url: {0}", this._url));
                msg.Add(String.Format("Request Body: {0}", this._data == null ? string.Empty : JsonConvert.SerializeObject(this._data, Formatting.None)));

                //Logger.WriteLogApi(String.Join("| ", msg.ToArray()));

                msg = new List<string>();
                msg.Add(DateTime.Now.ToString());
                msg.Add(String.Format("ResponseID: {0}", corrId));
                msg.Add(String.Format("StatusCode: {0}", (ex.Response as HttpWebResponse).StatusCode.ToString()));
                //Logger.WriteLogApi(String.Join("| ", msg.ToArray()));

                throw ex;
            }
        }

        public string connectAPIReturnString()
        {
            var httpWebRequest = HttpWebRequest.Create(this._url);

            if (_timeout > 0)
            {
                httpWebRequest.Timeout = _timeout;
            }

            if (_method != null)
            {
                httpWebRequest.Method = this._method.ToString();
            }

            httpWebRequest.ContentType = "application/json";

            if (this._header != null)
            {
                foreach (DictionaryEntry hash in this._header)
                {
                    httpWebRequest.Headers[hash.Key.ToString()] = hash.Value.ToString();
                }
            }

            if (this._method != APIServiceMethod.GET)
            {
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(this._data);
                }
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                return streamReader.ReadToEnd();
            }
        }
    }

    public class APIServiceOptions
    {
        public string url { set; get; }
        public object data { set; get; }
        public int timeout { set; get; }
        public APIServiceMethod method { set; get; }
        public Hashtable header { set; get; }
        public bool useJwt { set; get; }
    }


    public enum APIServiceMethod
    {
        GET = 0,
        POST = 1,
        PUT = 2,
        DELETE = 3
    }
}