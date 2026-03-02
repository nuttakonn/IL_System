using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ILSystem.App_Code.Commons
{
    public class UserInfo
    {
        public UserInfo()
        {
            _arRoleID = new ArrayList();
        }

        private long _id;
        private string _name = "";
        private string _username = "";
        private string _password = "";
        private string _client = "SERVER";
        private string _GDepartment = "";
        private int _portalid;
        private ArrayList _arRoleID;
        private bool _IsAdmin = false;
        private bool _IsAuthenMode = true;

        private string _BizInit = "RL";
        private string _BranchNo = "1"; //[56991] เพิ่ม Property สำหรับเก็บ Branch เพื่อใช้ในเรื่องของ Card
        private string _BranchDescEN = "1"; //[56991] เพิ่ม Property สำหรับเก็บ Branch เพื่อใช้ในเรื่องของ Card
        private string _BranchApp = "1";

        private string _AuthenCopy = ""; //-- 61053 : Check Authorize Copy Data [2015-03-27]

        private string _accessKey = "";  //SSO BOT Report





        //[DBPrimaryKeyField("ID_User",DbType.Int16)]
        //[DBColumn("ID_User",DbType.Int16,false)]
        public long UserID
        {
            set { _id = value; }

            get { return _id; }
        }


        //[DBColumn("Name",DbType.String,true)]
        public string Name
        {
            set { _name = value; }

            get { return _name; }
        }

        //[DBColumn("Username",DbType.String,false)]
        public string Username
        {
            set { _username = value; }

            get { return _username; }
        }

        //[DBColumn("Password",DbType.String,false)]
        public string Password
        {
            set { _password = value; }

            get { return _password; }
        }

        public string LocalClient
        {
            set { _client = value; }

            get { return _client; }
        }

        public int PortalID
        {
            set { _portalid = value; }

            get { return _portalid; }
        }

        public ArrayList RolesID
        {
            set { _arRoleID = value; }

            get { return _arRoleID; }
        }

        public bool IsAdmin
        {
            set { _IsAdmin = value; }

            get { return _IsAdmin; }
        }

        public bool IsAuthenMode
        {
            set { _IsAuthenMode = value; }

            get { return _IsAuthenMode; }
        }

        public string GDepartment
        {
            set { _GDepartment = value; }

            get { return _GDepartment; }
        }

        //[56991] เพิ่ม Property สำหรับเก็บ Branch เพื่อใช้ในเรื่องของ Card
        public string BizInit
        {
            set { _BizInit = value; }

            get { return _BizInit; }
        }
        public string BranchNo
        {
            set { _BranchNo = value; }

            get { return _BranchNo; }
        }
        public string BranchDescEN
        {
            set { _BranchDescEN = value; }

            get { return _BranchDescEN; }
        }
        public string BranchApp
        {
            set { _BranchApp = value; }

            get { return _BranchApp; }
        }
        //--------------------------
        //-- 61053 : Check Authorize Copy Data [2015-03-27]
        public string AuthenCopy
        {
            set { _AuthenCopy = value; }

            get { return _AuthenCopy; }
        }

        //BOT Report
        public string AccessKey
        {
            set { _accessKey = value; }

            get { return _accessKey; }
        }
    }
}