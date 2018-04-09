using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GroupControl.Model;

namespace GroupControl.IDAL
{
    public interface IContractInfoDetailsDAL : IBaseDAL<ContractInfoDetail, BaseViewModel>
    {
        IList<ContractInfoDetail> GetListWithContractInfoDetailList(IList<ContractInfoDetail> list);
    }
}
