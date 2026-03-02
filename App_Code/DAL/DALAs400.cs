using System;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.Collections;
using IBM.Data.DB2.iSeries;

namespace EB_Service.DAL
{
    /// <summary>
    /// Summary description for DALAs400.
    /// </summary>

    public class As400DAL : DABasis
    {
        public As400DAL()
        {
            this.DatabaseType = DatabaseTypes.DbAS400;
        }

        public As400DAL(string strUserName, string strPassword, string strLib)
        {
            this.DatabaseType = DatabaseTypes.DbAS400;
            this.UserName = strUserName;
            this.Password = strPassword;
            this.AS400Lib = strLib;
            //this.AppSettingKey = "ConnectionStringAs400";
        }

        public As400DAL(string strUserName, string strPassword, string strLib, Providor400Types provtype)
        {
            this.Providor400Type = provtype;
            this.UserName = strUserName;
            this.Password = strPassword;
            this.AS400Lib = strLib;
        }

        public bool As400Authentication(ref ArrayList arRoles)
        {
            bool bRet = false;
            iDB2Command cmd = new iDB2Command();

            try
            {
                cmd.Connection = iDB2Con;
                cmd.Transaction = _tr;
                cmd.CommandTimeout = 30;
                cmd.CommandType = System.Data.CommandType.Text;

                string strSQL = @"SELECT
	                                  GNTA04.T04RID as T04RID
                                  FROM
	                                  GNTA04
                                  INNER JOIN
	                                  GNTA05 
                                  ON
	                                  GNTA04.T04RID = GNTA05.T05RID 
                                  AND GNTA04.T04PID = GNTA05.T05OID
                                  WHERE
	                                  (GNTA05.T05NAM ='{0}') 
                                  GROUP BY
	                                  GNTA04.T04RID
                                  ORDER BY
	                                  GNTA04.T04RID";

                strSQL = String.Format(strSQL, this.UserName);

                //Peak 05/02/2550 แก้ไข Query เปลี่ยนจาก Lib HM เป็น Lib GN
                //strSQL = String.Format(strSQL,AppConfig.AHPBackupLib,this.UserName); 
                //strSQL = String.Format(strSQL, AppConfig.AS400AutLib, this.UserName);
                cmd.CommandText = strSQL;
                iDB2DataReader dr = cmd.ExecuteReader();
                bRet = dr.HasRows;
                if (bRet)
                {
                    while (dr.Read())
                        arRoles.Add(dr.GetiDB2Numeric(0));

                    dr.Close();
                }
            }
            catch (Exception e)
            {
                string str = e.Message;
            }
            //finally
            //{
            //    if (cmd.Connection.State == ConnectionState.Open)
            //    {
            //        cmd.Connection.Close();
            //    }
            //}

            return bRet;
        }

        //public bool AssignAuthenticationDB2Lib()
        //{
        //    bool bRet = false;
        //    iDB2Command cmd = new iDB2Command();
        //    iDB2Connection cnn = this.GetiSeriesDbAs400Connection();
        //    try
        //    {
        //        cnn.Open();
        //        cmd.Connection = cnn;
        //        cmd.CommandTimeout = 30;
        //        cmd.CommandType = CommandType.Text;
        //        cmd.CommandText = "CALL " + AppConfig.AS400AssignLib + ".HMSRBKCL";
        //        cmd.ExecuteNonQuery();
        //        bRet = true;
        //    }
        //    catch (Exception e)
        //    {
        //        string str = e.Message;
        //    }
        //    finally
        //    {
        //        cnn.Close();
        //        cnn.Dispose();
        //    }

        //    return bRet;

        //}

        /*
        public bool AssignAuthenticationLib()
        {
            bool bRet = false;
            if (Assignlib_Flag)
            {
                bRet = true;
            }
            else
            {
                iDB2Command cmd = new iDB2Command();
                try
                {
                    cmd.Connection = iDB2Con;
                    cmd.Transaction = _tr;
                    cmd.CommandTimeout = 30;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "CALL HMSRBKCL";
                    cmd.ExecuteNonQuery();
                    bRet = true;
                    Assignlib_Flag = true;
                }
                catch (Exception e)
                {
                    string str = e.Message;
                }
                finally
                {
                }
            }
            return bRet;
        }
        */

        //[56691] เพิ่ม StoreProcedure ตัวใหม่สำหรับ Initial Lib
        public bool AssignAuthenticationLib(string strBizforInit, string strBranchNo)
        {
            bool bRet = false;
            if (Assignlib_Flag)
            {
                bRet = true;
            }
            else
            {
                //ConnectAs400();
                iDB2Command cmd = new iDB2Command();
                try
                {
                    cmd.Connection = iDB2Con;
                    cmd.Transaction = _tr;
                    cmd.CommandTimeout = 30;

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "GNINZ00CL";

                    cmd.Parameters.Add("WIAPPL", iDB2DbType.iDB2Char, 2).Value = strBizforInit;
                    cmd.Parameters.Add("WIBRAN", iDB2DbType.iDB2Char, 3).Value = strBranchNo;

                    cmd.ExecuteNonQuery();
                    bRet = true;
                    Assignlib_Flag = true;
                }
                catch (Exception e)
                {
                    string str = e.Message;
                }
                finally
                {
                    //if (cmd.Connection.State == ConnectionState.Open)
                    //{
                    //    cmd.Connection.Close();
                    //}
                }
            }
            return bRet;
        }

        public iDB2Command SetScriptCmd(string strcmd)
        {
            iDB2Command cmd = new iDB2Command();

            cmd.CommandText = strcmd;
            cmd.CommandTimeout = 0;

            return cmd;
        }

        public bool ExecuteTrans(ArrayList arSQL)
        {
            bool bRet = true;

            try
            {
                iDB2Command cmd = null;
                try
                {
                    foreach (object obj in arSQL)
                    {
                        if (obj is IBM.Data.DB2.iSeries.iDB2Command)
                        {
                            //DataCenterFraud.WKO_MsgError="";
                            cmd = obj as iDB2Command;
                            cmd.Connection = iDB2Con;
                            cmd.Transaction = _tr;
                            //DataCenterFraud.WKO_MsgError =cmd.CommandText.ToString().Trim();   //10/04/2007 เพื่อจับ Error Msg Execute SQL
                            cmd.ExecuteNonQuery();
                        }
                        else if (obj is string)
                        {
                            //DataCenterFraud.WKO_MsgError="";
                            cmd = new iDB2Command();
                            cmd.Connection = iDB2Con;
                            cmd.CommandText = obj as String;
                            cmd.Transaction = _tr;
                            //DataCenterFraud.WKO_MsgError =cmd.CommandText.ToString().Trim(); //10/04/2007 เพื่อจับ Error Msg Execute SQL
                            cmd.ExecuteNonQuery();
                        }
                    }
                    CommitAs400();
                }
                catch (iDB2Exception e)
                {
                    RollbackAs400();
                    bRet = false;
                }
                finally
                {
                    if (cmd.Connection.State == ConnectionState.Open)
                    {
                        cmd.Connection.Close();
                    }
                }
            }
            catch (iDB2Exception iDb2ex)
            {
                bRet = false;
            }
            return bRet;
        }

        public bool iDB2ExecuteProgram(string PGM, ref iDB2Command cmd, ref string ErrorMsg)
        {
            bool bRet = true;
            ErrorMsg = "";

            try
            {
                //ConnectAs400();
                //bRet = AssignAuthenticationLib();
                //if (bRet)
                //{				
                cmd.Connection = iDB2Con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = PGM.Trim();
                cmd.Transaction = _tr;
                cmd.ExecuteNonQuery();
                //}				
            }
            catch (iDB2Exception oleex)
            {
                bRet = false;
                ErrorMsg = oleex.Message;
                //throw oleex;
            }
            //finally
            //{
            //    if (cmd.Connection.State == ConnectionState.Open)
            //    {
            //        cmd.Connection.Close();
            //    }
            //}
            return bRet;
        }

        public bool iDB2ExecuteProgram(string PGM, ref iDB2Command cmd, ref string ErrorMsg, string strBizforInit, string strBranchNo) //[56991] เพิ่ม Parameter สำหรับ Initial Lib (BizforInit, Branch No)
        {
            bool bRet = true;
            ErrorMsg = "";

            try
            {
                //ConnectAs400();
                bRet = AssignAuthenticationLib(strBizforInit, strBranchNo);
                if (bRet)
                {
                    cmd.Connection = iDB2Con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = PGM.Trim();
                    cmd.Transaction = _tr;

                    cmd.ExecuteNonQuery();
                }
            }
            catch (iDB2Exception oleex)
            {
                bRet = false;
                ErrorMsg = oleex.Message;
                //throw oleex;
            }
            finally
            {
                if (cmd.Connection.State == ConnectionState.Open)
                {
                    cmd.Connection.Close();
                }
            }

            return bRet;
        }

        public DataSet ExecuteDataReader_to_DataSet_iSeries(string SQLCommand)
        {
            DataSet DS = new DataSet();
            iDB2Command cmd = new iDB2Command();
            iDB2DataReader dr = null;

            try
            {
                this.ConnectAs400();
                cmd.Connection = iDB2Con;
                cmd.Transaction = _tr;
                cmd.CommandText = SQLCommand;

                //iDB2DataAdapter adp = new iDB2DataAdapter(cmd);
                //adp.Fill(DS);                
                dr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                DS.Tables.Add(dt);
            }
            catch (iDB2Exception iDb2ex)
            {
            }
            finally
            {
                if (cmd.Connection.State == ConnectionState.Open)
                {
                    cmd.Connection.Close();
                }
                this.CloseConnect();
            }

            return DS;
        }

        public bool ExecuteSubRoutine(string PGM, ref iDB2Command cmd)
        {
            bool bRet = true;
            try
            {
                ConnectAs400();
                //bRet = AssignAuthenticationLib();
                //if (bRet)
                //{				
                cmd.Connection = iDB2Con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = PGM.Trim();
                cmd.Transaction = _tr;
                cmd.ExecuteNonQuery();
                //}				
            }
            catch (iDB2Exception oleex)
            {
                bRet = false;
                da_LastError = oleex.Message.ToString();
            }
            finally
            {
                if (cmd.Connection.State == ConnectionState.Open)
                {
                    //cmd.Connection.Close();
                }
            }
            return bRet;
        }

        public bool ExecuteSubRoutine(string PGM, ref iDB2Command cmd, string strBizforInit, string strBranchNo) //[56991] เพิ่ม Parameter สำหรับ Initial Lib (BizforInit, Branch No)
        {
            bool bRet = true;
            try
            {
                ConnectAs400();
                bRet = AssignAuthenticationLib(strBizforInit, strBranchNo);
                if (bRet)
                {
                    cmd.Connection = iDB2Con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = PGM.Trim();
                    cmd.Transaction = _tr;
                    cmd.ExecuteNonQuery();
                }
            }
            catch (iDB2Exception oleex)
            {
                bRet = false;
                da_LastError = oleex.Message.ToString();
            }
            //finally
            //{
            //    CloseConnect();
            //}
            return bRet;
        }

        public iDB2DataReader iDB2ExecuteDataReader(string sqlCmd)
        {
            iDB2DataReader dr = null;
            iDB2Command iDb2Cmd = null;
            try
            {
                iDb2Cmd = new iDB2Command(sqlCmd, iDB2Con);
                iDb2Cmd.Transaction = _tr;
                dr = iDb2Cmd.ExecuteReader();
            }
            catch (iDB2Exception iDb2ex)
            {
                return dr;
            }
            //finally
            //{
            //    if (iDb2Cmd.Connection.State == ConnectionState.Open)
            //    {
            //        iDb2Cmd.Connection.Close();
            //    }
            //}

            return dr;
        }

        public iDB2DataReader iDB2ExecuteDataReader(iDB2Command iDb2Cmd)
        {
            iDB2DataReader dr = null;
            try
            {
                iDb2Cmd.Connection = iDB2Con;
                iDb2Cmd.Transaction = _tr;
                dr = iDb2Cmd.ExecuteReader();
            }
            catch (iDB2Exception iDb2ex)
            {
                return dr;
            }
            //finally
            //{
            //    if (iDb2Cmd.Connection.State == ConnectionState.Open)
            //    {
            //        iDb2Cmd.Connection.Close();
            //    }
            //}

            return dr;
        }

        public DataSet ExecuteDataSet(string SQLCommand)
        {
            DataSet DS;

            DS = ExecuteDataSetiSeries(SQLCommand);

            return DS;
        }

        private DataSet ExecuteDataSetiSeries(string SQLCommand)
        {
            DataSet DS = new DataSet();
            iDB2Command cmd = new iDB2Command();

            try
            {
                this.ConnectAs400();
                cmd.Connection = iDB2Con;
                cmd.Transaction = _tr;
                cmd.CommandText = SQLCommand;

                iDB2DataAdapter adp = new iDB2DataAdapter(cmd);

                adp.Fill(DS);

            }
            catch (iDB2Exception iDb2ex)
            {
            }
            //finally
            //{
            //    if (cmd.Connection.State == ConnectionState.Open)
            //    {
            //        cmd.Connection.Close();
            //    }
            //    this.CloseConnect();
            //}

            return DS;
        }

        public DataSet ExecuteDataSet(iDB2Command db2Command)
        {
            DataSet DS = null;
            try
            {
                this.ConnectAs400();
                db2Command.Connection = iDB2Con;
                db2Command.Transaction = _tr;
                iDB2DataAdapter adp = new iDB2DataAdapter(db2Command);
                DS = new DataSet();
                adp.Fill(DS);
            }
            catch (iDB2Exception iDb2ex)
            {
            }
            //finally
            //{
            //    if (db2Command.Connection.State == ConnectionState.Open)
            //    {
            //        db2Command.Connection.Close();
            //    }
            //    this.CloseConnect();
            //}
            this.CloseConnect();

            if ((DS == null) || (DS.Tables.Count <= 0))
            {
                return null;
            }

            return DS;
        }

        public int ExecuteNonQuery(string SQLCommand)
        {
            int iRet;
            iRet = ExecuteNonQueryiSeries(SQLCommand);

            return iRet;
        }

        private int ExecuteNonQueryiSeries(string SQLCommand)
        {
            int iRet = -1;
            iDB2Command cmd = null;
            try
            {
                this.ConnectAs400();
                cmd = new iDB2Command(SQLCommand, iDB2Con);
                cmd.Transaction = _tr;
                iRet = cmd.ExecuteNonQuery();
                this.CommitAs400();
            }
            catch (iDB2Exception iDb2ex)
            {
                this.RollbackAs400();
                //throw iDb2ex; 
            }
            finally
            {
                if (cmd.Connection.State == ConnectionState.Open)
                {
                    cmd.Connection.Close();
                }
                this.CloseConnect();
            }

            return iRet;
        }

        public int ExecuteNonQuery(iDB2Command db2Command)
        {
            int iRet = -1;
            try
            {
                this.ConnectAs400();
                db2Command.Connection = iDB2Con;
                db2Command.Transaction = _tr;
                db2Command.ExecuteNonQuery();
                iRet = 1;
            }
            catch (iDB2Exception iDb2ex)
            {
                iRet = -1;
            }
            finally
            {
                /***** ถ้าทำงานแบบ Transaction จะไม่สามารถ Close Connection ส่วนนี้ได้ *****/
                //if (db2Command.Connection.State == ConnectionState.Open)
                //{
                //    db2Command.Connection.Close();
                //}
            }

            return iRet;
        }

        public int ExecuteNonQueryWithCommit(iDB2Command db2Command)
        {
            int iRet = -1;
            try
            {
                this.ConnectAs400();
                db2Command.Connection = iDB2Con;
                db2Command.Transaction = _tr;
                db2Command.ExecuteNonQuery();
                CommitAs400();
                iRet = 1;
            }
            catch (iDB2Exception iDb2ex)
            {
                iRet = -1;
                RollbackAs400();
            }
            finally
            {
                if (db2Command.Connection.State == ConnectionState.Open)
                {
                    db2Command.Connection.Close();
                }
                this.CloseConnect();
            }

            return iRet;
        }

        public iDB2DataReader ExecuteDataReader(iDB2Connection iDb2Conn, string SqlCmd)
        {
            iDB2DataReader dr = null;
            iDB2Command iDb2Cmd = null;
            try
            {
                iDb2Conn.Open();
                iDb2Cmd = new iDB2Command(SqlCmd, iDb2Conn);
                dr = iDb2Cmd.ExecuteReader();

            }
            catch (iDB2Exception iDb2ex)
            {
                throw iDb2ex;
            }
            //finally
            //{
            //    if (iDb2Cmd.Connection.State == ConnectionState.Open)
            //    {
            //        iDb2Cmd.Connection.Close();
            //    }
            //}

            return dr;
        }

        public iDB2DataReader ExecuteDataReader(iDB2Connection iDb2Conn, iDB2Command iDb2Cmd)
        {
            iDB2DataReader dr = null;
            try
            {
                iDb2Conn.Open();
                iDb2Cmd.Connection = iDb2Conn;
                dr = iDb2Cmd.ExecuteReader();
            }
            catch (iDB2Exception iDb2ex)
            {
                throw iDb2ex;
            }
            //finally
            //{
            //    if (iDb2Cmd.Connection.State == ConnectionState.Open)
            //    {
            //        iDb2Cmd.Connection.Close();
            //    }
            //}

            return dr;
        }

        public OleDbDataReader ExecuteDataReader(OleDbConnection OleConn, string SqlCmd)
        {
            OleDbDataReader dr = null;
            OleDbCommand OleCmd = null;
            try
            {
                OleConn.Open();
                OleCmd = new OleDbCommand(SqlCmd, OleConn);
                dr = OleCmd.ExecuteReader();
            }
            catch (Exception iDb2ex)
            {
                throw iDb2ex;
            }
            //finally
            //{
            //    if (OleCmd.Connection.State == ConnectionState.Open)
            //    {
            //        OleCmd.Connection.Close();
            //    }
            //}

            return dr;
        }

        public OleDbDataReader ExecuteDataReader(OleDbConnection OleConn, OleDbCommand OleCmd)
        {
            OleDbDataReader dr = null;
            try
            {
                OleConn.Open();
                OleCmd.Connection = OleConn;
                dr = OleCmd.ExecuteReader();
            }
            catch (Exception iDb2ex)
            {
                throw iDb2ex;
            }
            //finally
            //{
            //    if (OleCmd.Connection.State == ConnectionState.Open)
            //    {
            //        OleCmd.Connection.Close();
            //    }
            //}

            return dr;
        }

        public DataSet ExecSubRoutine_DS(ref iDB2Command command, ref string Error)
        {
            DataSet returnDS = new DataSet();
            Error = "";
            try
            {
                //ConnectAs400();
                //bool bRet = AssignAuthenticationLib(); // AssignAuthenticationLib();
                //if (bRet)
                //{
                command.Connection = iDB2Con;
                command.Transaction = _tr;
                iDB2DataAdapter adp = new iDB2DataAdapter(command);

                DataSet DS = new DataSet();
                adp.Fill(DS);
                returnDS = DS;
                //}

            }
            catch (iDB2Exception oleex)
            {
                Error = oleex.Message.ToString();
            }
            //finally
            //{
            //    if (command.Connection.State == ConnectionState.Open)
            //    {
            //        command.Connection.Close();
            //    }
            //}

            return returnDS;
        }

        //[56691] เพิ่ม Function ExecSubRoutine_DS ที่มี Parm สำหรับเช็คว่าควรเรียก StoreProcedure Initial Lib หรือไม่
        public DataSet ExecSubRoutine_DS(ref iDB2Command command, ref string Error, string strBizInit, string strBranchNo)
        {
            DataSet returnDS = new DataSet();
            Error = "";
            try
            {
                bool bRet = AssignAuthenticationLib(strBizInit, strBranchNo);

                if (bRet)
                {
                    command.Connection = iDB2Con;
                    command.Transaction = _tr;
                    //iDB2DataAdapter adp = new iDB2DataAdapter(command);

                    iDB2DataReader dr = command.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(dr);

                    DataSet DS = new DataSet();
                    DS.Tables.Add(dt);  //adp.Fill(DS);                    
                    returnDS = DS;
                }
            }
            catch (iDB2Exception oleex)
            {
                Error = oleex.Message.ToString();
            }
            //finally
            //{
            //    if (command.Connection.State == ConnectionState.Open)
            //    {
            //        command.Connection.Close();
            //    }
            //}

            return returnDS;
        }

        #region no connection
        //  Add By Supawadee Date: 16/05/2014:  For connect AS400  without commit or rollback function.

        public int ExecuteNonQueryNoCommit(iDB2Command db2Command)  // SUPAWADEE
        {
            int iRet = -1;
            try
            {
                //this.ConnectAs400();
                db2Command.Connection = iDB2Con;
                db2Command.Transaction = _tr;
                iRet = db2Command.ExecuteNonQuery();
                //this.CommitAs400();
                //iRet = 1;
            }
            catch (iDB2Exception iDb2ex)
            {
                iRet = -1;
                //this.RollbackAs400();
            }
            //finally
            //{
            //    if (db2Command.Connection.State == ConnectionState.Open)
            //    {
            //        db2Command.Connection.Close();
            //    }
            //}

            //this.CloseConnect();
            return iRet;
        }

        public DataSet ExecuteDataSetNoCloseConnect(iDB2Command db2Command)
        {
            DataSet DS = null;

            try
            {
                //this.ConnectAs400();
                db2Command.Connection = iDB2Con;
                db2Command.Transaction = _tr;
                iDB2DataAdapter adp = new iDB2DataAdapter(db2Command);
                DS = new DataSet();
                adp.Fill(DS);
            }
            catch (iDB2Exception iDb2ex)
            {

            }
            //finally
            //{
            //    if (db2Command.Connection.State == ConnectionState.Open)
            //    {
            //        db2Command.Connection.Close();
            //    }
            //}
            //  this.CloseConnect();
            if ((DS == null) || (DS.Tables.Count <= 0))
            {
                return null;
            }
            return DS;
        }

        public bool ExecuteSubRoutineNoConnect(string PGM, ref iDB2Command cmd, string strBizforInit, string strBranchNo) //[56991] เพิ่ม Parameter สำหรับ Initial Lib (BizforInit, Branch No)
        {
            bool bRet = true;

            try
            {
                //ConnectAs400();
                bRet = AssignAuthenticationLib(strBizforInit, strBranchNo);
                if (bRet)
                {
                    cmd.Connection = iDB2Con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = PGM.Trim();
                    cmd.Transaction = _tr;
                    cmd.ExecuteNonQuery();
                }
            }
            catch (iDB2Exception oleex)
            {
                bRet = false;
            }
            //finally
            //{
            //    if (cmd.Connection.State == ConnectionState.Open)
            //    {
            //        cmd.Connection.Close();
            //    }
            //}

            return bRet;
        }
        #endregion

        //		public bool IsPrivilegeConnect()
        //		{
        //			if(this.UserName==null || this.UserName == "")
        //				return m_bPrivilege = false;
        //
        //			m_bPrivilege = false;
        //			OleDbConnection conn = this.GetOleDbConnection();
        //			OleDbCommand command = new OleDbCommand(); 
        //			try
        //			{
        //				conn.Open(); 
        //				command.Connection = conn;
        //				command.CommandTimeout = 80; 
        //				command.CommandType = CommandType.Text;
        //				command.CommandText = "{{" + String.Format("CALL /QSYS.LIB/{0}.LIB/PWSRAUT.PGM(?,?)",
        //					this.Lib) + "}}";
        //				
        //				command.Parameters.Add(new OleDbParameter("P1",OleDbType.Char,10)).Value = this.UserName;
        //				command.Parameters.Add(new OleDbParameter("P2",OleDbType.Char,1)).Direction = ParameterDirection.Output;
        //				
        //				command.Prepare();
        //				command.ExecuteNonQuery();
        //
        //				LogSystem.Logger.WriteTrace(false,String.Format("Check authorize by User :{0}",this.UserName));  
        //				if(command.Parameters[1].Value.ToString() != "Y")
        //					m_bPrivilege = true;
        //              	
        //				LogSystem.Logger.WriteTrace(false,String.Format("authentication command return :{0}",command.Parameters[1].Value));
        //				command.Dispose(); 	
        //			}
        //			catch(Exception e)
        //			{
        //				LogSystem.Logger.ErrorRoutine(false,e); 
        //				
        //			}
        //			finally
        //			{
        //				conn.Close();
        //				conn.Dispose();
        //			}
        //			return m_bPrivilege;
        //		}

        //		public bool IsPrivilegeConnect()
        //		{
        //			if(this.UserName==null || this.UserName == "")
        //				return m_bPrivilege = false;
        //
        //			m_bPrivilege = false;
        //			OleDbConnection conn = this.GetOleDbAs400Connection();
        //			try
        //			{
        //				conn.Open();
        //				string strsql = String.Format("select t04usr from {0}.pwtb04 where t04lcd='350' and t04usr='{1}'",this.Lib,UserName);     
        //				OleDbCommand strcom = new OleDbCommand(strsql,conn);
        //				strcom.CommandType  = CommandType.Text;
        //				OleDbDataReader rd = strcom.ExecuteReader();
        //				LogSystem.Logger.WriteTrace(false,String.Format("Check authorize by SQL command :{0}",strsql));  
        //				if(rd.HasRows)
        //					m_bPrivilege = true;
        //              	
        //				LogSystem.Logger.WriteTrace(false,String.Format("authentication command return :{0}",m_bPrivilege));
        //				strcom.Dispose(); 	
        //			}
        //			catch(Exception e)
        //			{
        //				LogSystem.Logger.ErrorRoutine(false,e); 
        //				string s = e.Message;
        //			}
        //			finally
        //			{
        //				conn.Close();
        //				conn.Dispose();
        //			}
        //			return m_bPrivilege;
        //		}
        //		
        //		public bool AssignAuthenticationLib()
        //		{
        //			
        //			bool bRet = false;
        //			OleDbCommand command = new OleDbCommand();
        //			OleDbConnection conn = this.GetOleDbAs400Connection();
        //			
        //			try
        //			{
        //				conn.Open(); 
        //				command.Connection = conn;
        //				command.CommandTimeout = 80; 
        //				command.CommandType = CommandType.Text;
        //				command.CommandText = "{{" + String.Format("CALL /QSYS.LIB/{0}.LIB/{1}.PGM(?,?,?,?,?,?)",
        //					AppConfig.AuthenticateLib(),AppConfig.AuthenticatePGM()) + "}}";
        //				
        //				command.Parameters.Add(new OleDbParameter("P1",OleDbType.Char,2)).Value = "PW";
        //				command.Parameters.Add(new OleDbParameter("P2",OleDbType.Char,3)).Value = "001";
        //				command.Parameters.Add(new OleDbParameter("P3",OleDbType.Char,1)).Value = "O";
        //				command.Parameters.Add(new OleDbParameter("P4",OleDbType.Char,1)).Value = "0";
        //				command.Parameters.Add(new OleDbParameter("P5",OleDbType.Char,AppConfig.AuthenticatePrm1().Length)).Value = AppConfig.AuthenticatePrm1();
        //				command.Parameters.Add(new OleDbParameter("P6",OleDbType.Char,AppConfig.AuthenticatePrm2().Length)).Value = AppConfig.AuthenticatePrm2();
        //				command.Prepare();
        //				command.ExecuteNonQuery();
        //				bRet = true;
        //			}
        //			catch(Exception e)
        //			{
        //				string str = e.Message; 
        //			}
        //			finally
        //			{
        //				conn.Close();
        //				conn.Dispose();
        //			}
        //			
        //			return bRet;
        //				
        //		}
        //
        //		public bool SendContactsReceive(Hashtable _htParam)
        //		{
        //			
        //			bool bRet = false;
        //			OleDbCommand command = new OleDbCommand();
        //			OleDbConnection conn = this.GetOleDbAs400Connection();
        //			Logger.WriteTrace(false,"Call sub routine for deduct account and print slip"); 
        //			try
        //			{
        //				Logger.WriteTrace(false,String.Format("Paid by contract :{0}",_htParam["CONTID"]));
        //				Logger.WriteTrace(false,String.Format("Paid total amount :{0}",_htParam["TOTAMT"]));
        //				Logger.WriteTrace(false,String.Format("Instruction by user :{0}  on client :{1}",_htParam["USER"],_htParam["IPADDRESS"]));
        //				conn.Open(); 
        //				command.Connection = conn;
        //				command.CommandTimeout = 80; 
        //				command.CommandType = CommandType.Text;
        //				command.CommandText = "{{" + String.Format("CALL /QSYS.LIB/{0}.LIB/{1}.PGM(?,?,?,?,?,?,?,?,?)",
        //					this.Lib,AppConfig.CallSendPaidPGMLib()) + "}}";
        //				//command.Parameters.Add(new OleDbParameter("P1",OleDbType.Char,((string)_htParam["OPTTYPE"]).Length)).Value = _htParam["OPTTYPE"].ToString();
        //				command.Parameters.Add(new OleDbParameter("P1",OleDbType.Char,50)).Value = (string)_htParam["OPTTYPE"];
        //				//command.Parameters.Add(new OleDbParameter("P2",OleDbType.Char,((string)_htParam["BUSTYPE"]).Length)).Value = _htParam["BUSTYPE"].ToString();
        //				command.Parameters.Add(new OleDbParameter("P2",OleDbType.Char,2*50)).Value = (string)_htParam["BUSTYPE"];
        //				//command.Parameters.Add(new OleDbParameter("P3",OleDbType.Char,((string)_htParam["IDNO"]).Length)).Value = _htParam["IDNO"].ToString();
        //				command.Parameters.Add(new OleDbParameter("P3",OleDbType.Char,15*50)).Value = (string)_htParam["IDNO"];
        //				//command.Parameters.Add(new OleDbParameter("P4",OleDbType.Char,((string)_htParam["CONTID"]).Length)).Value = _htParam["CONTID"].ToString();
        //				command.Parameters.Add(new OleDbParameter("P4",OleDbType.Char,16*50)).Value = (string)_htParam["CONTID"];
        //				//command.Parameters.Add(new OleDbParameter("P5",OleDbType.Char,((string)_htParam["RECAMT"]).Length)).Value = _htParam["RECAMT"].ToString();
        //				command.Parameters.Add(new OleDbParameter("P5",OleDbType.Char,13*50)).Value = (string)_htParam["RECAMT"];
        //				//command.Parameters.Add(new OleDbParameter("P6",OleDbType.Char,((string)_htParam["TOTTRN"]).Length)).Value = _htParam["TOTTRN"].ToString();
        //				command.Parameters.Add(new OleDbParameter("P6",OleDbType.Char,((string)_htParam["TOTTRN"]).Length)).Value = (string)_htParam["TOTTRN"];
        //				//command.Parameters.Add(new OleDbParameter("P7",OleDbType.Char,((string)_htParam["TOTAMT"]).Length)).Value = _htParam["TOTAMT"].ToString();
        //				command.Parameters.Add(new OleDbParameter("P7",OleDbType.Char,((string)_htParam["TOTAMT"]).Length)).Value = (string)_htParam["TOTAMT"];
        //				//command.Parameters.Add(new OleDbParameter("P8",OleDbType.Char,((string)_htParam["USER"]).Length)).Value = _htParam["USER"].ToString();
        //				command.Parameters.Add(new OleDbParameter("P8",OleDbType.Char,((string)_htParam["USER"]).Length)).Value = (string)_htParam["USER"];
        //				//command.Parameters.Add(new OleDbParameter("P9",OleDbType.Char,((string)_htParam["PRINTER"]).Length)).Value = _htParam["PRINTER"].ToString();
        //				command.Parameters.Add(new OleDbParameter("P9",OleDbType.Char,((string)_htParam["PRINTER"]).Length)).Value = ((string)_htParam["PRINTER"]);
        //				command.Parameters.Add(new OleDbParameter("P10",OleDbType.Char,((string)_htParam["IPADDRESS"]).Length)).Value = ((string)_htParam["IPADDRESS"]);
        //				command.Parameters.Add(new OleDbParameter("R10",OleDbType.Char,1)).Direction = ParameterDirection.Output;
        //				command.Prepare();
        //				command.ExecuteNonQuery();
        //							
        //				if(command.Parameters[10].Value.ToString().Length == 0)
        //					bRet = true;
        //				
        //			}
        //			catch(System.Data.OleDb.OleDbException e)
        //			{
        //				string str = e.Message;
        //				Logger.ErrorRoutine(false,(Exception)e); 
        //			}
        //			finally
        //			{
        //				command.Dispose(); 
        //				conn.Close();
        //				conn.Dispose();
        //			}
        //			
        //			return bRet;
        //				
        //		}
        //
        //		
        //		
        //		public DataTable GetContactsFrom400(string _strIDNo)
        //		{
        //			
        //			//OleDbCommand command = new OleDbCommand();
        //			OleDbConnection conn = this.GetOleDbAs400Connection();
        //			DataTable dtRet = GetContactsFrom400(conn,_strIDNo);
        //			conn.Close();
        //			conn.Dispose(); 
        //			return dtRet;
        //			
        //		}
        //		
        //		public DataTable GetContactsFrom400(OleDbConnection _conn,string _strIDNo)
        //		{
        //			
        //			OleDbCommand command = new OleDbCommand();
        //			//OleDbConnection conn = this.GetOleDbConnection();
        //			   
        //			DataTable dataCont = null;
        //			try
        //			{
        //				if(_conn.State == System.Data.ConnectionState.Closed)   
        //					_conn.Open(); 
        //				command.Connection = _conn;
        //				command.CommandTimeout = 80;
        //				command.CommandType = CommandType.Text; 
        //				command.CommandText = "{{CALL /QSYS.LIB/"+this.Lib+".LIB/CSSR14CL.PGM(?,?,?,?,?,?,?,?,?,?,?)}}";
        //				// ID
        //				command.Parameters.Add(new OleDbParameter("@IDNO",OleDbType.Char,15)).Value = _strIDNo;
        //				// APP
        //				command.Parameters.Add(new OleDbParameter("APP",OleDbType.Char,2*60)).Direction = ParameterDirection.Output;
        //				
        //				// CNT
        //				command.Parameters.Add(new OleDbParameter("CNT",OleDbType.Char,16*60)).Direction = ParameterDirection.Output;
        //				
        //				//OSA
        //				command.Parameters.Add(new OleDbParameter("OSA",OleDbType.Char,13*60)).Direction = ParameterDirection.Output;
        //				
        //				// DOA
        //				command.Parameters.Add(new OleDbParameter("DOA",OleDbType.Char,13*60)).Direction = ParameterDirection.Output;
        //				
        //				// new Install
        //				command.Parameters.Add(new OleDbParameter("Install",OleDbType.Char,13*60)).Direction = ParameterDirection.Output;
        //				
        //				// New Penalty
        //				command.Parameters.Add(new OleDbParameter("Penalty",OleDbType.Char,13*60)).Direction = ParameterDirection.Output;
        //				
        //				// New Othor
        //				command.Parameters.Add(new OleDbParameter("Othor",OleDbType.Char,13*60)).Direction = ParameterDirection.Output;
        //				
        //				// DDT
        //				command.Parameters.Add(new OleDbParameter("P6",OleDbType.Char,6*60)).Direction = ParameterDirection.Output;
        //				
        //				// CLA
        //				command.Parameters.Add(new OleDbParameter("P7",OleDbType.Char,13*60)).Direction = ParameterDirection.Output;
        //				
        //				// STS
        //				command.Parameters.Add(new OleDbParameter("P8",OleDbType.Char,3*60)).Direction = ParameterDirection.Output;
        //
        //				// CNT Detail
        //				command.Parameters.Add(new OleDbParameter("DETAIL",OleDbType.Char,50*60)).Direction = ParameterDirection.Output;
        //				
        //				// TOTODA
        //				command.Parameters.Add(new OleDbParameter("P9",OleDbType.Char,13*60)).Direction = ParameterDirection.Output;
        //				
        //				// TOTCLA
        //				command.Parameters.Add(new OleDbParameter("P10",OleDbType.Char,13*60)).Direction = ParameterDirection.Output;
        //				
        //				command.Parameters.Add(new OleDbParameter("P11",OleDbType.Char,50)).Direction = ParameterDirection.Output;
        //				command.Parameters.Add(new OleDbParameter("P12",OleDbType.Char,60)).Direction = ParameterDirection.Output;
        //				
        //				command.Prepare();
        //				command.ExecuteNonQuery();
        //				string strID = (string)command.Parameters[0].Value;
        //				string strApp = (string)command.Parameters[1].Value;
        //				string strCustFullName = (string)command.Parameters[14].Value;
        //				int iCount = 0;
        //				if(strApp.Length > 0)
        //					iCount = (strApp.Length / 2);
        //				if(iCount > 0)
        //				{
        //					dataCont = CreateDataTable();
        //					
        //					// Create three new DataRow objects and add them to the DataTable
        //					for (int i = 0; i < iCount; i++)
        //					{
        //						DataRow	myDataRow = dataCont.NewRow();
        //						//myDataRow["ROWID"] = i + 1;
        //						myDataRow["CNT"] = (command.Parameters[2].Value.ToString().Length ==0)?
        //											"":command.Parameters[2].Value.ToString().Substring(i*16,16).Trim();
        //						myDataRow["CST"] = strID.Trim();
        //						myDataRow["CSN"] = strCustFullName;
        //						myDataRow["APP"] = strApp.Substring(i*2,2).Trim();
        //						myDataRow["OSA"] = (command.Parameters[3].Value.ToString().Length ==0)?
        //											0:ConvertToDouble(command.Parameters[3].Value.ToString().Substring(i*13,13).Trim());
        //						myDataRow["ODA"] = (command.Parameters[4].Value.ToString().Length ==0)?
        //											0:ConvertToDouble(command.Parameters[4].Value.ToString().Substring(i*13,13).Trim());
        //						myDataRow["ISA"] = (command.Parameters[5].Value.ToString().Length ==0)?
        //							0:ConvertToDouble(command.Parameters[5].Value.ToString().Substring(i*13,13).Trim());
        //						myDataRow["PNA"] = (command.Parameters[6].Value.ToString().Length ==0)?
        //							0:ConvertToDouble(command.Parameters[6].Value.ToString().Substring(i*13,13).Trim());
        //						myDataRow["OTHOR"] = (command.Parameters[7].Value.ToString().Length ==0)?
        //							0:ConvertToDouble(command.Parameters[7].Value.ToString().Substring(i*13,13).Trim());
        //
        //						myDataRow["STS"] = (command.Parameters[10].Value.ToString().Length ==0)?
        //							"":command.Parameters[10].Value.ToString().Substring(i*3,3).Trim();
        //
        //						if((string)myDataRow["STS"]=="W/O" || (string)myDataRow["STS"]=="L/C")
        //							myDataRow["DDT"] = "00/00/0000";
        //						else
        //							myDataRow["DDT"] = (command.Parameters[8].Value.ToString().Length ==0)?
        //											"00/00/0000":ConvertToThaiBuddhaDate(command.Parameters[8].Value.ToString().Substring(i*6,6).Trim());
        //						
        //						myDataRow["CLA"] = (command.Parameters[9].Value.ToString().Length ==0)?
        //											0:ConvertToDouble(command.Parameters[9].Value.ToString().Substring(i*13,13).Trim());
        //						
        //						myDataRow["DETAIL"] = (command.Parameters[11].Value.ToString().Length ==0)?
        //							"":command.Parameters[11].Value.ToString().Substring(i*50,50).Trim();
        //						myDataRow["TOTODA"] = (command.Parameters[12].Value.ToString().Length ==0)?
        //											0:ConvertToDouble(command.Parameters[12].Value.ToString().Substring(i*13,13).Trim());
        //						myDataRow["TOTCLA"] = (command.Parameters[13].Value.ToString().Length ==0)?
        //											0:ConvertToDouble(command.Parameters[13].Value.ToString().Substring(i*13,13).Trim());
        //						myDataRow["WKERR"] = (command.Parameters[15].Value.ToString().Length ==0)?
        //											"":command.Parameters[15].Value.ToString().Substring(i*1,1).Trim();
        //						
        //						dataCont.Rows.Add(myDataRow);  
        //					}
        //				}
        //				command.Dispose(); 
        //			}
        //			catch(Exception e)
        //			{
        //				string str = e.Message; 
        //			}
        //			
        //			return dataCont;
        //		
        //		}
        //
        //		private double ConvertToDouble(string strInput)
        //		{
        //			string strRet = strInput.Replace("\0","");
        //			double dbRet = 0.00;
        //			
        //			if(strRet.Length == 0)
        //				return dbRet;
        //			strRet = strRet.Insert(strRet.Length-2,"."); 
        //			return dbRet = System.Double.Parse(strRet); 
        //		}
        //
        //		private string ConvertToThaiBuddhaDate(string strInput)
        //		{
        //			string strRet = strInput.Replace("\0","");
        //			strRet = String.Format("{0}000000",strRet);
        //			strRet = strRet.Substring(0,6);
        //			if(strRet == "000000")
        //				return "00/00/0000";
        //
        //			CultureInfo cul = new System.Globalization.CultureInfo("th-TH",true);
        //			  
        //			System.DateTime dt = new DateTime(Convert.ToInt32(strRet.Substring(4,2)) + 2500,
        //				Convert.ToInt32(strRet.Substring(2,2)),	Convert.ToInt32(strRet.Substring(0,2)),cul.Calendar);
        //			
        //			return dt.ToString("dd/MM/yyyy",cul.DateTimeFormat); 
        //		}
        //
        //		public static DataTable CreateDataTable()
        //		{
        //			System.Data.DataTable myDataTable = new DataTable("Contacts");
        //			// Declare variables for DataColumn and DataRow objects.
        //			DataColumn myDataColumn;
        //			
        //			
        //			// Create new DataColumn, set DataType, ColumnName and add to DataTable.    
        //			myDataColumn = new DataColumn();
        //			myDataColumn.DataType = System.Type.GetType("System.Int32");
        //			myDataColumn.ColumnName = "RECTYP";
        //			myDataColumn.ReadOnly = false;
        //			// Add the Column to the DataColumnCollection.
        //			myDataTable.Columns.Add(myDataColumn);
        //			
        //			// Create new DataColumn, set DataType, ColumnName and add to DataTable.    
        //			myDataColumn = new DataColumn();
        //			myDataColumn.DataType = System.Type.GetType("System.String");
        //			myDataColumn.ColumnName = "CNT";
        //			myDataColumn.ReadOnly = true;
        //			myDataColumn.Unique = true;
        //			// Add the Column to the DataColumnCollection.
        //			myDataTable.Columns.Add(myDataColumn);
        //
        //			// Create new DataColumn, set DataType, ColumnName and add to DataTable.    
        //			myDataColumn = new DataColumn();
        //			myDataColumn.DataType = System.Type.GetType("System.String");
        //			myDataColumn.ColumnName = "CST";
        //			myDataColumn.ReadOnly = true;
        //			myDataColumn.Unique = false;
        //			// Add the Column to the DataColumnCollection.
        //			myDataTable.Columns.Add(myDataColumn);
        //
        //			// Create new DataColumn, set DataType, ColumnName and add to DataTable.    
        //			myDataColumn = new DataColumn();
        //			myDataColumn.DataType = System.Type.GetType("System.String");
        //			myDataColumn.ColumnName = "CSN";
        //			myDataColumn.ReadOnly = true;
        //			myDataColumn.Unique = false;
        //			// Add the Column to the DataColumnCollection.
        //			myDataTable.Columns.Add(myDataColumn);
        //
        //			// Create DataColumn, set DataType, ColumnName and add to DataTable.    
        //			myDataColumn = new DataColumn();
        //			myDataColumn.DataType = System.Type.GetType("System.String");
        //			myDataColumn.ColumnName = "APP";
        //			myDataColumn.ReadOnly = true;
        //			myDataColumn.Unique = false;
        //			// Add the Column to the DataColumnCollection.
        //			myDataTable.Columns.Add(myDataColumn);
        //
        //			// Create DataColumn, set DataType, ColumnName and add to DataTable.    
        //			myDataColumn = new DataColumn();
        //			myDataColumn.DataType = System.Type.GetType("System.Double");
        //			myDataColumn.ColumnName = "OSA";
        //			myDataColumn.ReadOnly = true;
        //			myDataColumn.Unique = false;
        //			// Add the Column to the DataColumnCollection.
        //			myDataTable.Columns.Add(myDataColumn);
        //
        //			// Create DataColumn, set DataType, ColumnName and add to DataTable.    
        //			myDataColumn = new DataColumn();
        //			myDataColumn.DataType = System.Type.GetType("System.Double");
        //			myDataColumn.ColumnName = "ODA";
        //			myDataColumn.ReadOnly = true;
        //			myDataColumn.Unique = false;
        //			// Add the Column to the DataColumnCollection.
        //			myDataTable.Columns.Add(myDataColumn);
        //
        //			// Create DataColumn, set DataType, ColumnName and add to DataTable.    
        //			myDataColumn = new DataColumn();
        //			myDataColumn.DataType = System.Type.GetType("System.Double");
        //			myDataColumn.ColumnName = "ISA";
        //			myDataColumn.ReadOnly = false;
        //			myDataColumn.Unique = false;
        //			// Add the Column to the DataColumnCollection.
        //			myDataTable.Columns.Add(myDataColumn);
        //
        //			// Create DataColumn, set DataType, ColumnName and add to DataTable.    
        //			myDataColumn = new DataColumn();
        //			myDataColumn.DataType = System.Type.GetType("System.Double");
        //			myDataColumn.ColumnName = "PNA";
        //			myDataColumn.ReadOnly = false;
        //			myDataColumn.Unique = false;
        //			// Add the Column to the DataColumnCollection.
        //			myDataTable.Columns.Add(myDataColumn);
        //
        //			// Create DataColumn, set DataType, ColumnName and add to DataTable.    
        //			myDataColumn = new DataColumn();
        //			myDataColumn.DataType = System.Type.GetType("System.Double");
        //			myDataColumn.ColumnName = "OTHOR";
        //			myDataColumn.ReadOnly = false;
        //			myDataColumn.Unique = false;
        //			// Add the Column to the DataColumnCollection.
        //			myDataTable.Columns.Add(myDataColumn);
        // 
        //			// Create second column.
        //			myDataColumn = new DataColumn();
        //			myDataColumn.DataType = System.Type.GetType("System.String");
        //			myDataColumn.ColumnName = "DDT";
        //			//myDataColumn.AutoIncrement = false;
        //			myDataColumn.Caption = "Due Date";
        //			myDataColumn.ReadOnly = false;
        //			myDataColumn.Unique = false;
        //			// Add the column to the table.
        //			myDataTable.Columns.Add(myDataColumn);
        //
        //			// Create second column.
        //			myDataColumn = new DataColumn();
        //			myDataColumn.DataType = System.Type.GetType("System.Double");
        //			myDataColumn.ColumnName = "CLA";
        //			//myDataColumn.AutoIncrement = false;
        //			myDataColumn.Caption = "Closing Amount";
        //			myDataColumn.ReadOnly = true;
        //			myDataColumn.Unique = false;
        //			// Add the column to the table.
        //			myDataTable.Columns.Add(myDataColumn);
        //
        //			// Create second column.
        //			myDataColumn = new DataColumn();
        //			myDataColumn.DataType = System.Type.GetType("System.String");
        //			myDataColumn.ColumnName = "STS";
        //			//myDataColumn.AutoIncrement = false;
        //			myDataColumn.Caption = "Status";
        //			myDataColumn.ReadOnly = false;
        //			myDataColumn.Unique = false;
        //			// Add the column to the table.
        //			myDataTable.Columns.Add(myDataColumn);
        //
        //			// Create DataColumn, set DataType, ColumnName and add to DataTable.    
        //			myDataColumn = new DataColumn();
        //			myDataColumn.DataType = System.Type.GetType("System.String");
        //			myDataColumn.ColumnName = "DETAIL";
        //			myDataColumn.ReadOnly = true;
        //			myDataColumn.Unique = false;
        //			// Add the Column to the DataColumnCollection.
        //			myDataTable.Columns.Add(myDataColumn);
        //			 
        //			// Create second column.
        //			myDataColumn = new DataColumn();
        //			myDataColumn.DataType = System.Type.GetType("System.Double");
        //			myDataColumn.ColumnName = "TOTODA";
        //			//myDataColumn.AutoIncrement = false;
        //			myDataColumn.Caption = "Total Overdue Amount";
        //			myDataColumn.ReadOnly = false;
        //			myDataColumn.Unique = false;
        //			// Add the column to the table.
        //			myDataTable.Columns.Add(myDataColumn);
        //
        //			// Create second column.
        //			myDataColumn = new DataColumn();
        //			myDataColumn.DataType = System.Type.GetType("System.Double");
        //			myDataColumn.ColumnName = "TOTCLA";
        //			//myDataColumn.AutoIncrement = false;
        //			myDataColumn.Caption = "Total Closing";
        //			myDataColumn.ReadOnly = false;
        //			myDataColumn.Unique = false;
        //			// Add the column to the table.
        //			myDataTable.Columns.Add(myDataColumn);
        //			
        //				// Create second column.
        //			myDataColumn = new DataColumn();
        //			myDataColumn.DataType = System.Type.GetType("System.String");
        //			myDataColumn.ColumnName = "WKERR";
        //			//myDataColumn.AutoIncrement = false;
        //			myDataColumn.Caption = "Work Error";
        //			myDataColumn.ReadOnly = false;
        //			myDataColumn.Unique = false;
        //			// Add the column to the table.
        //			myDataTable.Columns.Add(myDataColumn);
        // 
        //			// Make the ID column the primary key column.
        //			DataColumn[] PrimaryKeyColumns = new DataColumn[1];
        //			PrimaryKeyColumns[0] = myDataTable.Columns["CNT"];
        //			myDataTable.PrimaryKey = PrimaryKeyColumns;
        //
        //			return myDataTable;
        // 
        //		}
        //		
        //		public DataTable GetDataTableByTableName(string TableName)
        //		{
        //			
        //			OleDbCommand selectCMD ;
        //			OleDbConnection conn = this.GetOleDbAs400Connection();
        //			DataSet GenericDS = new DataSet();
        //			try
        //			{
        //				conn.Open();
        //				selectCMD = new OleDbCommand("SELECT * FROM " + TableName,conn) ;
        //				selectCMD.CommandTimeout = 30;
        //				OleDbDataAdapter GenericDA = new OleDbDataAdapter();
        //				GenericDA.SelectCommand = selectCMD;
        //				GenericDA.MissingSchemaAction =MissingSchemaAction.AddWithKey;
        //				
        //				GenericDA.Fill(GenericDS, TableName);
        //				selectCMD.Dispose(); 
        //			}
        //			catch{}
        //			finally
        //			{
        //				
        //				conn.Close();
        //				conn.Dispose();
        //			}
        //			return GenericDS.Tables[0];
        //		}
        //
        //		public DataSet GetDataSetBySQLCommand(string SQLCommand)
        //		{
        //			OleDbCommand selectCMD ;
        //			OleDbConnection conn = this.GetOleDbAs400Connection();
        //			DataSet GenericDS = new DataSet();
        //			try
        //			{
        //				conn.Open();
        //				selectCMD = new OleDbCommand(SQLCommand,conn) ;
        //				selectCMD.CommandTimeout = 0;
        //				OleDbDataAdapter GenericDA = new OleDbDataAdapter();
        //				GenericDA.SelectCommand = selectCMD;
        //				//	GenericDA.MissingSchemaAction =MissingSchemaAction.AddWithKey;
        //				
        //				GenericDA.Fill(GenericDS);
        //				selectCMD.Dispose(); 
        //			}
        //			catch(Exception e){
        //				string s = e.Message; 
        //			}
        //			finally
        //			{
        //				 
        //				conn.Close();
        //				conn.Dispose();
        //			}
        //			return GenericDS;
        //		}
        //
        //		public DataSet GetDataSetBySQLCommand(OleDbConnection _conn,string SQLCommand)
        //		{
        //			OleDbCommand selectCMD;
        //			DataSet GenericDS = new DataSet();
        //			try
        //			{
        //			
        //				selectCMD = new OleDbCommand(SQLCommand,_conn) ;
        //				selectCMD.CommandTimeout = 0;
        //				OleDbDataAdapter GenericDA = new OleDbDataAdapter();
        //				GenericDA.SelectCommand = selectCMD;
        //				//	GenericDA.MissingSchemaAction =MissingSchemaAction.AddWithKey;
        //				GenericDA.Fill(GenericDS);
        //				selectCMD.Dispose();
        //			}
        //			catch(Exception e)
        //			{
        //				string s = e.Message; 
        //			}
        //			 
        //			return GenericDS;
        //		}
        //
        //		public DataSet GetDataSetByOleDbCommand(OleDbCommand cmd)
        //		{
        //			
        //			OleDbConnection conn = this.GetOleDbAs400Connection();
        //			DataSet GenericDS = null;
        //			try
        //			{
        //				conn.Open();
        //				cmd.Connection = conn;
        //				cmd.CommandTimeout = 0;
        //				OleDbDataAdapter GenericDA = new OleDbDataAdapter();
        //				GenericDA.SelectCommand = cmd;
        //				GenericDS = new DataSet();
        //				GenericDA.Fill(GenericDS);
        //				
        //				if(GenericDS.Tables[0].Rows.Count==1)
        //				{
        //					
        //					this.Lib = AppConfig.CallPGMLib();
        //					DataTable dtContact = GetContactsFrom400(conn,(string)GenericDS.Tables[0].Rows[0][0]);
        //					dtContact.TableName = "Contacts";
        //					GenericDS.Tables.Clear();
        //					GenericDS.Tables.Add(dtContact);
        //					
        //				}
        //				
        //			}
        //			catch(Exception e)
        //			{
        //				string s = e.Message; 
        //			}
        //			finally
        //			{
        //				conn.Close();
        //				conn.Dispose();
        //			}
        //			return GenericDS;
        //		}
        //	}
        //
        //
        //	public class SendPrintedEventArgs : EventArgs
        //	{
        //		private bool _IsError = true;
        //		private Exception _e;
        //
        //		public bool IsError
        //		{
        //			get{return _IsError;}
        //		}
        //		public Exception ex
        //		{
        //			get{return _e;}
        //		}
        //
        //		public SendPrintedEventArgs(bool IsError,Exception ex)
        //		{
        //			_IsError = IsError;
        //			_e = ex;
        //		}
        //	}

        public string da_LastError { get; set; }
    }
}