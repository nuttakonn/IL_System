using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using IBM.Data.DB2.iSeries;
using EB_Service.Commons;
using EB_Service.DAL;

namespace EB_Service.BLL.Authentication
{
    public static class Authen_MainSystemBLL
    {
        //public Authen_MainSystemBLL()
        //{
        //    //
        //    // TODO: Add constructor logic here
        //    //
        //}
        //public static bool IsPriviledge(string strUser, string strPassword)
        //{
        //    bool bRet = false;
        //    As400DAL obj = new As400DAL(strUser, strPassword, AppConfig.AS400AutLib);
        //    try
        //    {

        //        obj.ConnectAs400();
        //        iDB2Command cmd = new iDB2Command();
        //        //cmd.CommandText = String.Format("SELECT T05NAM FROM {0}.GNTA05 WHERE T05NAM=?", AppConfig.AS400AutLib);
        //        cmd.CommandText = String.Format("SELECT T05NAM FROM GNTA05 WHERE T05NAM=?", AppConfig.AS400AutLib);
        //        iDB2Parameter prm = cmd.CreateParameter();
        //        prm.Value = strUser;
        //        cmd.Parameters.Add(prm);
        //        iDB2DataReader dr = obj.iDB2ExecuteDataReader(cmd);
        //        if (dr.HasRows)
        //            bRet = true;

        //        dr.Close();

        //    }
        //    catch (iDB2ConnectionFailedException fe)
        //    {
        //        Console.WriteLine(fe.Message);
        //    }
        //    catch (iDB2ConnectionTimeoutException te)
        //    {
        //        Console.WriteLine(te.Message);
        //    }
        //    catch (iDB2Exception e)
        //    {
        //        Console.WriteLine(e.Message);
        //    }
        //    obj.CloseConnect();
        //    return bRet;
        //}

        public static bool IsPriviledgeOracleByUser(string strUser, string strPassword)
        {
            bool bRet = false;
            /*
            OracleConnection con = null;
            try
            {
                OracleDAL obj = new OracleDAL(strUser, strPassword);
                con = obj.GetDbOraConnection();
                OracleCommand cmd = con.CreateCommand();
                //cmd.CommandText = String.Format("SELECT T05NAM FROM {0}.GNTA05 WHERE T05NAM=?", AppConfig.OraAutUser);
                cmd.CommandText = String.Format("SELECT T05NAM FROM GNTA05 WHERE T05NAM=?", AppConfig.OraAutUser);
                OracleParameter prm = cmd.CreateParameter();
                prm.Value = strUser;
                cmd.Parameters.Add(prm);
                OracleDataReader dr = obj.ExecuteDataReader(con, cmd);
                if (!dr.IsDBNull(0))
                    bRet = true;

                dr.Close();
                con.Close();
            }
            catch (OracleException fe)
            {

            }
            
            if (con != null)
            {
                con.Close();
                con.Dispose();
            }
            */
            return bRet;
        }


        //public static DataSet Get_MainSystemByUserName(string strUser, string strPassword)
        //{
        //    /*
        //    string strSQL = String.Format("SELECT C.T05OID as SystemID, "
        //        + "(SELECT A.T03ALI  FROM {0}.GNTA03 A WHERE A.T03PID=0 And A.T03ISP='Y' " 
        //        + " AND A.T03OID=(SELECT D.T03PID FROM {0}.GNTA03 D WHERE D.T03OID=C.T05OID) )  as Parent," 
        //        + " (SELECT B.T03URL FROM {0}.GNTA03 B WHERE B.T03OID=C.T05OID) as URL, "
        //        + " (SELECT B.T03NAM FROM {0}.GNTA03 B WHERE B.T03OID=C.T05OID) as PortalName, "
        //        + " '' as ImgURL, '' as Description "
        //        + ", B.T03ORD as SortMenu "
        //        + " FROM {0}.GNTA05 C Join {0}.GNTA03 B On B.T03OID = C.T05OID "
        //        + "  WHERE (C.T05NAM=?) GROUP BY C.T05OID, B.T03ORD Order by B.T03ORD ", AppConfig.AS400AutLib);
        //    */
        //    string strSQL = String.Format("SELECT C.T05OID as SystemID, "
        //                    + "(SELECT A.T03ALI  FROM GNTA03 A WHERE A.T03PID=0 And A.T03ISP='Y' "
        //                    + " AND A.T03OID=(SELECT D.T03PID FROM GNTA03 D WHERE D.T03OID=C.T05OID) )  as Parent,"
        //                    + " (SELECT B.T03URL FROM GNTA03 B WHERE B.T03OID=C.T05OID) as URL, "
        //                    + " (SELECT B.T03NAM FROM GNTA03 B WHERE B.T03OID=C.T05OID) as PortalName, "
        //                    + " '' as ImgURL, '' as Description "
        //                    + ", B.T03ORD as SortMenu "
        //                    + " FROM GNTA05 C Join GNTA03 B On B.T03OID = C.T05OID "
        //                    + "  WHERE (C.T05NAM=?) GROUP BY C.T05OID, B.T03ORD Order by B.T03ORD ", AppConfig.AS400AutLib);

        //    As400DAL obj400 = new As400DAL(strUser, strPassword, AppConfig.AS400AutLib, Providor400Types.PriSeries);
        //    obj400.ConnectAs400();
        //    iDB2Command cmd = new iDB2Command(strSQL);
        //    iDB2Parameter prm = cmd.CreateParameter();
        //    prm.Value = strUser;
        //    cmd.Parameters.Add(prm);
        //    DataSet DS = obj400.ExecuteDataSet(cmd);
        //    obj400.CloseConnect();
        //    return DS;
        //}

        public static DataSet Get_MainSystemOracleByUserName(string strUser, string strPassword)
        {
            //string strSQL = String.Format("SELECT C.T05OID as SystemID, "
            //    + "(SELECT A.T03NAM  FROM {0}.GNTA03 A WHERE A.T03PID=0 And A.T03ISP='Y' "    //Peak 23/7/2009 เปลี่ยน Field จาก T03ALI เป็น T03NAM
            //    + " AND A.T03OID=(SELECT D.T03PID FROM {0}.GNTA03 D WHERE D.T03OID=C.T05OID) )  as Parent,"
            //    + " (SELECT B.T03URL FROM {0}.GNTA03 B WHERE B.T03OID=C.T05OID) as URL, "
            //    + " (SELECT B.T03ALI FROM {0}.GNTA03 B WHERE B.T03OID=C.T05OID) as PortalName, "  //Peak 23/7/2009 เปลี่ยน Field จาก T03NAM เป็น T03ALI
            //    + " (SELECT B.T03IMGURL FROM {0}.GNTA03 B WHERE B.T03OID=C.T05OID) as ImgURL, "
            //    + " (SELECT B.T03DESC FROM {0}.GNTA03 B WHERE B.T03OID=C.T05OID) as Description, "
            //    + " B.T03ORD as SortMenu "
            //    + " FROM {0}.GNTA05 C Join {0}.GNTA03 B On B.T03OID = C.T05OID "
            //    + "  WHERE (C.T05NAM=:T05NAM) GROUP BY C.T05OID, B.T03ORD Order by B.T03ORD ", AppConfig.OraAutUser);
            //OracleDAL obj = new OracleDAL(strUser, strPassword);
            //OracleCommand cmd = new OracleCommand(strSQL);
            //OracleParameter prm = cmd.CreateParameter();
            //prm.Value = strUser;
            //prm.ParameterName = ":T05NAM";
            //cmd.Parameters.Add(prm);
            DataSet DS = new DataSet();
            //DataSet DS = obj.ExecuteDataSet(cmd);


            return DS;
        }

        //public static string Get_Authorize_CopyData(string strUser, string strPassword)
        //{
        //    As400DAL obj400 = new As400DAL(strUser, strPassword, AppConfig.AS400AutLib, Providor400Types.PriSeries);
        //    string sResult = "N";
        //    try
        //    {
        //        obj400.ConnectAs400();

        //        iDB2Command cmd = new iDB2Command();
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.Add("WIUSER", iDB2DbType.iDB2Char, 10).Value = strUser.Trim();
        //        cmd.Parameters.Add("WOJDCD", iDB2DbType.iDB2Char, 10).Direction = ParameterDirection.Output;
        //        cmd.Parameters.Add("WOALLW", iDB2DbType.iDB2Char, 1).Direction = ParameterDirection.Output;

        //        if (obj400.ExecuteSubRoutine("GNCHKCPY", ref cmd))
        //        {
        //            if (cmd.Parameters["WOALLW"].Value.ToString() != "")
        //                sResult = cmd.Parameters["WOALLW"].Value.ToString();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        sResult = "N";
        //    }
        //    obj400.CloseConnect();
        //    return sResult;
        //}



    }
}