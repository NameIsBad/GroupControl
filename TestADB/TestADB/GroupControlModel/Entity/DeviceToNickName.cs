using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupControl.Model
{
   public  class DeviceToNickName
    {
        public int ID { get; set; }

        public string Device { get; set; }

        public string NickName { get; set; }

        public int FriendCount { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
