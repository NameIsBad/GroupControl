using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GroupControl.Model;

namespace GroupControl.IDAL
{
    public interface IGroupInfoDAL : IBaseDAL<GroupInfo, GroupInfoViewModel>
    {
        IList<GroupInfoViewModel> GetUnionListWithTask(GroupInfoViewModel viewModel);
    }
}
