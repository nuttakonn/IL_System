using EB_Service.Commons;
using EB_Service.DAL;
using ILSystem.App_Code.Commons;
using ILSystem.App_Code.BLL.DataCenter;
using Microsoft.Win32.SafeHandles;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using WebPasswordEncryption;

namespace EB_Service.DAL
{
    public class DataCenter : UserInfo
    {
        protected string _domainAD;
        protected string _connectionString;
        protected string _connectionStringBulk;
        const int LOGON32_PROVIDER_DEFAULT = 0;
        const int LOGON32_LOGON_INTERACTIVE = 2;

        private ClassPasswordEncryption _encryption;
        private string _username;
        private string _password;
        private readonly string _serverName;
        private SqlConnection m_Sqlcon;
        private SqlConnection m_SqlBulkcon;
        private SqlTransaction m_Sqltr;
        private string m_UserName;
        private string m_User;
        private string m_Wrkstn = "";
        UserInfo m_userInfo;

        public DataCenter(UserInfo userInfo)
        {
            m_userInfo = (UserInfo)userInfo;

            _domainAD = ConfigurationManager.AppSettings["ADDomain"];
            _connectionString = ConfigurationManager.ConnectionStrings["MSSQLConnection"].ToString();
            _connectionStringBulk = ConfigurationManager.ConnectionStrings["MSSQLBulkConnection"].ToString();
            if (SqlCon == null)
            {
                SqlCon = new SqlConnection();
                SqlCon.ConnectionString = _connectionString;
            }

            if(SqlBulkCon == null)
            {
                SqlBulkCon = new SqlConnection();
                SqlBulkCon.ConnectionString = _connectionStringBulk;
            }          
            _encryption = new ClassPasswordEncryption();
            //HttpContext context = HttpContext.Current;
            _serverName = Environment.MachineName.Trim();
        }

        public UserInfo UserInfomation
        {
            set
            {
                m_userInfo = value;
                m_UserName = m_userInfo.Username;
                m_User = m_userInfo.Username;
                m_Wrkstn = m_userInfo.LocalClient;
            }
        }

        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern bool LogonUser(string lpszUsername, string lpszDomain, string lpszPassword, int dwLogonType, int dwLogonProvider, out SafeAccessTokenHandle phToken);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool CloseHandle(SafeAccessTokenHandle hObject);

        public SqlConnection GetOpenConnection()
        {
            SqlCon.Open();

            return SqlCon;
        }

        public async Task<(bool success, string message, T data)> Get<T>(string commandText, CommandType commandType,  List<SqlParameter> parameters = null) where T : new()
        {
            var result = new T();
            try
            {
                if (string.IsNullOrWhiteSpace(commandText)) throw new ArgumentException($"'{nameof(commandText)}' cannot be null or whitespace.", nameof(commandText));
                Utility.WriteLogResponse("Get ", commandText.ToString());
#if DEBUG
                var DS = _GetTest(commandText, commandType, parameters);
#else
           var DS = _Get(commandText, commandType, parameters);
#endif
                result = DataSetToList<T>(DS);
                //Utility.WriteLogResponse("success ", DS.Tables.Count > 0 ? JsonConvert.SerializeObject(DS.Tables[0]) : "");

                return await Task.FromResult((true, "success", result));
            }
            catch (Exception ex)
            {
                Utility.WriteLog(ex);
                return await Task.FromResult((false, ex.Message, result));
            }
        }

        public async Task<(bool success, string message, DataSet data)> GetDataset<T>(string commandText, CommandType commandType, List<SqlParameter> parameters = null) where T : new()
        {
            var result = new DataSet();
            try
            {
                if (string.IsNullOrWhiteSpace(commandText)) throw new ArgumentException($"'{nameof(commandText)}' cannot be null or whitespace.", nameof(commandText));
                Utility.WriteLogResponse("GetDataset ", commandText.ToString());
#if DEBUG
                var DS = _GetTest(commandText, commandType, parameters);
#else
           var DS = _Get(commandText, commandType, parameters);
#endif
                result = DS;
                //Utility.WriteLogResponse("success ", DS.Tables.Count > 0 ? JsonConvert.SerializeObject(DS.Tables[0]) : "");

                return await Task.FromResult((true, "success", result));
            }
            catch (Exception ex)
            {
                Utility.WriteLog(ex);
                return await Task.FromResult((false, ex.Message, result));
            }
        }

     

        public DataSet _Get(string commandText, CommandType type, IEnumerable<SqlParameter> param = null)
        {
            try 
            {
                DataSet DS = new DataSet();
                UserInfomation = m_userInfo;
                //m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
                //Utility.WriteLogString(m_userInfo.Username , "GET");
                //Utility.WriteLogString(m_userInfo.Password, "GET");
                bool methodStatus = LogonUser(m_userInfo.Username,
                                                _domainAD,
                                                m_userInfo.Password,
                                                LOGON32_LOGON_INTERACTIVE,
                                                LOGON32_PROVIDER_DEFAULT,
                                                out SafeAccessTokenHandle safeAccessTokenHandle);

                if (methodStatus)
                {
                    try
                    {
                        WindowsIdentity.RunImpersonated(safeAccessTokenHandle, new Action(
                        () =>
                        {
                            try
                            {
                                if (SqlCon != null && SqlCon.State != ConnectionState.Open)
                                    SqlCon.Open();
                                SqlDataAdapter sqlData = new SqlDataAdapter(commandText, SqlCon);
                                sqlData.SelectCommand.CommandType = type;
                                sqlData.SelectCommand.Transaction = Sqltr;
                                sqlData.SelectCommand.CommandTimeout = 600;

                                if (param != null && param.Any())
                                {
                                    foreach (var p in param)
                                    {
                                        sqlData.SelectCommand.Parameters.AddWithValue(p.ParameterName, p.Value);
                                    }
                                }
                                sqlData.Fill(DS);
                            }
                            catch (Exception ex)
                            {
                                Utility.WriteLog(ex);
                                throw new Exception(ex.Message);
                            }
                            finally
                            {
                            }
                        }));
                    }
                    finally
                    {
                        CloseHandle(safeAccessTokenHandle);
                    }
                }
                else
                {
                    throw new Exception($"Username or Password is invalid.");
                }
                //if (DS.Tables.Count < 1 || DS.Tables[0].Rows.Count < 1) throw new Exception($"Data not found.");

                return DS;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public DataSet _GetTest(string commandText, CommandType type, IEnumerable<SqlParameter> param = null)
        {
            DataSet DS = new DataSet();

            try
            {
                if (SqlCon != null && SqlCon.State != ConnectionState.Open)
                    SqlCon.Open();
                SqlDataAdapter sqlData = new SqlDataAdapter(commandText, SqlCon);
                sqlData.SelectCommand.CommandType = type;
                sqlData.SelectCommand.Transaction = Sqltr;
                sqlData.SelectCommand.CommandTimeout = 600;
                if (param != null && param.Any())
                {
                    foreach (var p in param)
                    {
                        sqlData.SelectCommand.Parameters.AddWithValue(p.ParameterName, p.Value);
                    }
                }
                sqlData.Fill(DS);
            }
            finally
            {
            }

            //if (DS.Tables.Count < 1 || DS.Tables[0].Rows.Count < 1) throw new Exception($"Data not found.");

            return DS;
        }
        public async Task<(bool success, string message, int afrows)> Execute(string commandText, CommandType type, bool transaction = false, IEnumerable<SqlParameter> param = null, bool DBlink = false)
        {
            (bool success, string message, int afrows) result;
            try
            {
                if (string.IsNullOrWhiteSpace(commandText))
                {
                    throw new ArgumentException($"'{nameof(commandText)}' cannot be null or whitespace.", nameof(commandText));
                }
                Utility.WriteLogResponse("Execute ", commandText.ToString());

#if DEBUG
                var afrows = await _ExecuteTest(commandText, type, param, transaction);
#else
                var afrows = await _Execute(commandText, type, param, transaction);
#endif
                if (afrows > 0) result = (true, "success", afrows);
                else result = (true, "The command was executed but 0 row affected.", afrows);
                //Utility.WriteLogResponse("Execute ", result.message.ToString());
            }
            catch (Exception ex)
            {
                Utility.WriteLog(ex);
                result = (false, ex.Message, -1);
            }
            return await Task.FromResult(result);
        }
       
        public async Task<int> _Execute(string commandText, CommandType type, IEnumerable<SqlParameter> param = null, bool transaction = false)
        {
            int afrows = 0;
            //m_userInfo = (UserInfo)HttpContext.Current.Session["UserInfo"];
            UserInfomation = m_userInfo;

            //Utility.WriteLogString(m_userInfo.Username, "Execute");
            //Utility.WriteLogString(m_userInfo.Password, "Execute");
            bool methodStatus = LogonUser(m_userInfo.Username,
                                            _domainAD,
                                            m_userInfo.Password,
                                LOGON32_LOGON_INTERACTIVE,
                                LOGON32_PROVIDER_DEFAULT,
                                out SafeAccessTokenHandle safeAccessTokenHandle);

            if (methodStatus)
            {
                WindowsIdentity.RunImpersonated(safeAccessTokenHandle, new Action(
                () =>
                {
                    if (SqlCon != null && SqlCon.State != ConnectionState.Open)
                        SqlCon.Open();
                    if(transaction)
                        Sqltr = SqlCon.BeginTransaction();
                    try
                    {
                        SqlCommand sqlCmd = new SqlCommand(commandText, SqlCon);
                        sqlCmd.CommandType = type;
                        sqlCmd.Transaction = Sqltr;
                        sqlCmd.CommandTimeout = 600;

                        if (param != null && param.Any()) param.ToList().ForEach(p => sqlCmd.Parameters.AddWithValue(p.ParameterName, p.Value));
                        afrows = sqlCmd.ExecuteNonQuery();
                    }
                    catch (SqlException sqlError)
                    {
                        throw sqlError;
                    }
                }));
                return await Task.FromResult(afrows);
            }
            else
            {
                throw new Exception($"Username or Password is invalid.");
            }
        }

        public async Task<int> _ExecuteTest(string commandText, CommandType type, IEnumerable<SqlParameter> param = null,bool transaction = false) // for test on localhost
        {
            int afrows = 0;
            if (SqlCon != null && SqlCon.State != ConnectionState.Open)
                SqlCon.Open();
            if (transaction)
                Sqltr = SqlCon.BeginTransaction();
            try
            {
                SqlCommand sqlCmd = new SqlCommand(commandText, SqlCon);
                sqlCmd.CommandType = type;
                sqlCmd.Transaction = Sqltr;
                sqlCmd.CommandTimeout = 600;

                if (param != null && param.Any()) param.ToList().ForEach(p => sqlCmd.Parameters.AddWithValue(p.ParameterName, p.Value));
                afrows = sqlCmd.ExecuteNonQuery();
            }
            catch (SqlException sqlError)
            {
                throw sqlError;
            }
            return await Task.FromResult(afrows);
        }

        public async Task<(bool success, string message)> ExecuteBulk(string spName, IEnumerable<SqlParameter> parameters)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(spName))
                    throw new ArgumentException("Stored procedure name is required.");

                Utility.WriteLogResponse("ExecuteBulk", spName);

#if DEBUG
                await _ExecuteBulkTest(spName, CommandType.StoredProcedure, parameters, false);
#else
                await _ExecuteBulk(spName, CommandType.StoredProcedure, parameters, false);
#endif

                return (true, "success");
            }
            catch (SqlException sqlEx)
            {
                // error จาก THROW / RAISERROR ใน SP
                Utility.WriteLog(sqlEx);
                return (false, sqlEx.Message);
            }
            catch (Exception ex)
            {
                Utility.WriteLog(ex);
                return (false, ex.Message);
            }
        }
        public async Task<int> _ExecuteBulk(string commandText, CommandType type, IEnumerable<SqlParameter> param = null, bool transaction = false)
        {
            int afrows = 0;
            UserInfomation = m_userInfo;

            bool methodStatus = LogonUser(
                m_userInfo.Username,
                _domainAD,
                m_userInfo.Password,
                LOGON32_LOGON_INTERACTIVE,
                LOGON32_PROVIDER_DEFAULT,
                out SafeAccessTokenHandle safeAccessTokenHandle);

            if (!methodStatus)
                throw new Exception("Username or Password is invalid.");

            WindowsIdentity.RunImpersonated(
                safeAccessTokenHandle,
                () =>
                {
                    if (SqlBulkCon.State != ConnectionState.Open)
                        SqlBulkCon.Open();

                    try
                    {
                        using (SqlCommand sqlCmd = new SqlCommand(commandText, SqlBulkCon))
                        {
                            sqlCmd.CommandType = type;
                            sqlCmd.CommandTimeout = 600;

                            if (param != null)
                            {
                                foreach (var p in param)
                                    sqlCmd.Parameters.Add(p); // ⭐ สำคัญ
                            }

                            afrows = sqlCmd.ExecuteNonQuery();
                            var successParam = sqlCmd.Parameters["@Success"];
                            if (successParam != null)
                            {
                                bool success = (bool)successParam.Value;
                                if (success)
                                {
                                    Utility.WriteLogResponse("ExecuteNonQueryBulk", "Operation succeeded.");
                                }
                                else
                                {
                                    var errorMessageParam = sqlCmd.Parameters["@ErrorMessage"];
                                    if (errorMessageParam != null)
                                    {
                                        string errorMessage = errorMessageParam.Value.ToString();
                                        Utility.WriteLogResponse("ExecuteNonQueryBulk", $"Error: {errorMessage}");
                                    }
                                }
                            }
                        }
                    }
                    catch
                    {
                        throw;
                    }
                });
            return await Task.FromResult(afrows);
        }
        public async Task<int> _ExecuteBulkTest(
            string commandText,
            CommandType type,
            IEnumerable<SqlParameter> param = null,
            bool transaction = false)
        {
            int afrows = 0;

            if (SqlBulkCon.State != ConnectionState.Open)
                SqlBulkCon.Open();

            using (SqlCommand sqlCmd = new SqlCommand(commandText, SqlBulkCon))
            {
                sqlCmd.CommandType = type;
                sqlCmd.CommandTimeout = 600;

                if (param != null)
                {
                    foreach (var p in param)
                        sqlCmd.Parameters.Add(p);
                }

                afrows = sqlCmd.ExecuteNonQuery();
                var successParam = sqlCmd.Parameters["@Success"];
                if (successParam != null)
                {
                    bool success = (bool)successParam.Value;
                    if (success)
                    {
                        Utility.WriteLogResponse("ExecuteNonQueryBulk", "Operation succeeded.");
                    }
                    else
                    {
                        var errorMessageParam = sqlCmd.Parameters["@ErrorMessage"];
                        if (errorMessageParam != null)
                        {
                            string errorMessage = errorMessageParam.Value.ToString();
                            Utility.WriteLogResponse("ExecuteNonQueryBulk", $"Error: {errorMessage}"); 
                        }
                    }
                }
            }

            return await Task.FromResult(afrows);
        }
        
        private T DataSetToList<T>(DataSet DS) where T : new()
        {
            var result = new T();
            if (result is IList && result.GetType().IsGenericType)
            {
                result = ConvertToModel<T>(DS.Tables[0]);
            }
            else
            {
                result = ConvertToModel<List<T>>(DS.Tables[0]).FirstOrDefault();
            }
            return result;
        }

        private List<T> DataSetToListII<T>(DataSet DS) where T : new()
        {
            var result = new List<T>();
            if (result is IList && result.GetType().IsGenericType)
            {
                result = ConvertToListModel<T>(DS.Tables[0]);
            }
            else
            {
                result = ConvertToModel<List<T>>(DS.Tables[0]);
            }
            return result;
        }
        private List<T> ConvertToListModel<T>(object dt)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                FloatFormatHandling = FloatFormatHandling.DefaultValue,
                MissingMemberHandling = MissingMemberHandling.Ignore,
            };
            var strDt = JsonConvert.SerializeObject(dt);
            return JsonConvert.DeserializeObject<List<T>>(strDt, settings);
        }

        private T ConvertToModel<T>(object dt)
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

        public DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties by using reflection   
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names  
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {

                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }

            return dataTable;
        }

        public string GetServerName()
        {
            string targetName = "";

            if (_serverName.Length == 11)
            {
                targetName = _serverName.Substring(5);
            }
            else if (_serverName.Length > 11)
            {
                if ((_serverName.Length - 10) > 10)
                {
                    targetName = _serverName.Substring(_serverName.Length - 10, 10);
                }
                else
                {
                    targetName = _serverName.Substring(_serverName.Length - 10);
                }
            }
            else if (_serverName.Length <= 10)
            {
                return _serverName ?? "";
            }
            return targetName;
        }

        public bool OpenConnectSQL()
        {
            bool bRet = true;
            try
            {
                SqlCon.Open();
            }
            catch
            {
                bRet = false;
            }

            return bRet;

        }

        public bool CloseConnectSQL()
        {
            bool bRet = true;
            try
            {
                SqlCon.Close();
            }
            catch
            {
                bRet = false;
            }

            return bRet;

        }

        public bool RollbackMssql()
        {
            bool bRet = true;
            try
            {
                Sqltr.Rollback();
            }
            catch(Exception ex)
            {
                Utility.WriteLog(ex);
                bRet = false;
            }

            return bRet;
        }
        public bool CommitMssql()
        {
            bool bRet = true;
            try
            {
                Sqltr.Commit();

            }
            catch(Exception ex)
            {
                Utility.WriteLog(ex);
                bRet = false;
            }

            return bRet;
        }
        public SqlConnection SqlCon
        {
            set { m_Sqlcon = value; }
            get { return m_Sqlcon; }
        }
        public SqlConnection SqlBulkCon
        {
            set { m_SqlBulkcon = value; }
            get { return m_SqlBulkcon; }
        }

        public SqlTransaction Sqltr
        {
            set { m_Sqltr = value; }
            get { return m_Sqltr; }
        }
    }
}