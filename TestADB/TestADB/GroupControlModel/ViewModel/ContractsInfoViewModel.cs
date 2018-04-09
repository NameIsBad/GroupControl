using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupControl.Model
{
   public  class ContractsInfoViewModel:BaseViewModel
    {
        public ContractsInfo ContractsInfoModel { get; set;}

        public IList<ContractInfoDetail> ContractInfoDetailList { get; set; }

        public IList<DeviceToNickNameViewModel> DeviceToNickNameList { get; set; }
    }
}
