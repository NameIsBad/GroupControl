using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GroupControl.Model;

namespace GroupControl.IBLL
{
    public interface IGroupInfoBLL : IBaseBLL<GroupInfo, BaseViewModel>
    {
        IList<GroupInfoViewModel> GetGroupListWithDevice(BaseViewModel viewModel);




    }
}
