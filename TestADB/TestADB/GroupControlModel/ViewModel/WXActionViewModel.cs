using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupControl.Model
{
    public class WXActionViewModel:BaseViewModel
    {
        public string RemarkContent { get; set; }

        public string SayHiContent { get; set; }

        public int AddCount { get; set; }

        public EnumCheckSexType Sex { get; set; }

        public string XMLName { get; set; }

        public string PullNativePath { get; set; }

        /// <summary>
        /// 是否统计粉丝数
        /// </summary>
        public bool IsStatisticsFriendCount { get; set; }

        public string FriendCountStr { get; set; }

        public bool IsGoOn { get; set; }

        public int CurrentAddFriendCount { get; set; }


        public bool IsValidatedState { get; set; }

        public IList<string> SearchContents { get; set; }


        public bool IsSearch { get; set; }
    }
}
