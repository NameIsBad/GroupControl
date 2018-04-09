using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupControl.Model
{
    public class DeviceToNickNameViewModel : BaseViewModel
    {

        public string NickName { get; set; }

        public string Device { get; set; }

        public int FriendCount { get; set; }

        public int GroupID { get; set; }

        public string GroupName { get; set; }

        public IList<string> Devices { get; set; }

        public DateTime ImportDate { get; set; }

        public int ImportContentCount { get; set; }

        public IList<ContractsInfo> ContractsInfoList { get; set; }

        public int ContractInfoID { get; set; }

    }
}
