using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace GroupControl.Common
{
    public class ConfigInfo
    {

        public string HandSendFriendFileUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["HandSendFriendFileUrl"].ToString();
            }
        }

        public string AutoSendFriendFileUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["AutoSendFriendFileUrl"].ToString();
            }
        }

        public string ADBExeFileUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["ADBExeFileUrl"].ToString();
            }
        }

        public string CutImageFileUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["CutImageFileUrl"].ToString();
            }
        }

        public string ADBKeyBoardUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["ADBKeyBoardUrl"].ToString();
            }
        }

        public string WXAppUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["WXAppUrl"].ToString();
            }
        }


        public string ContractsFileUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["ContractsFileUrl"].ToString();
            }
        }

    }
}
