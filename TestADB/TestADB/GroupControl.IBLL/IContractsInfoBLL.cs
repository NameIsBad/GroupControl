using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GroupControl.Model;

namespace GroupControl.IBLL
{
    public interface IContractsInfoBLL : IBaseBLL<ContractsInfo, ContractsInfoViewModel>
    {
        void InsertContractWithContractDetail(ContractsInfoViewModel viewModel);

        IList<ContractInfoDetail> GetListWithContractInfoDetailList(IList<ContractInfoDetail> list);
    }
}
