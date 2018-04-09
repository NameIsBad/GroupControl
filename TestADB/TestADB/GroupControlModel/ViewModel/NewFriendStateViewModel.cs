using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupControl.Model
{
   public  class NewFriendStateViewModel:BaseViewModel
    {
        public string Device { get; set; }

        public bool IsImportContracts { get; set; }

        public bool IsHaveNewFriend { get; set; }

        public EnumSelectDateTimeType SelectDateTimeType { get; set; }


        /// <summary>
        /// 自定义查询时间
        /// </summary>
        public DateTime? CustomDate { get; set; }


        public IList<string> CurentUseingEquipmentList { get; set; }


        public int CustomCheckDay { get; set; }

    }
}
