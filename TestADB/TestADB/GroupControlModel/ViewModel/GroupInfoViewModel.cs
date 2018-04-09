using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupControl.Model
{
   public  class GroupInfoViewModel:BaseViewModel
    {
        public GroupInfo GroupInfoModel { get; set; }

        public GroupToDevice GroupToDeviceModel { get; set; }

        public IList<GroupToDevice> GroupToDeviceList { get; set; }

        public int DevicesCount { get; set; }
    }
}
