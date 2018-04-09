using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GroupControl.Model;

namespace GroupControl.Model
{
    public class AutoServiceInfoViewModel : BaseViewModel
    {
        public EnumTaskStatus Status { get; set; }

        public EnumTaskType TaskType { get; set; }

        public DateTime? CreateDate { get; set; }

        public IList<string> Devices { get; set; }

        public IList<string> GroupIDs { get; set; }

        public IList<string> SearchContents { get; set; }

        public string Path { get; set; }

        public EnumPublishContentType PublishContentType { get; set; }

        public AutoServiceInfo AutoServiceInfoModel { get; set; }

        public bool IsContainGroupInfo { get; set; }

        public int ComputerID { get; set; }

        public IList<GroupInfoViewModel> GroupInfoViewModelList { get; set; }

    }
}
