using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GroupControl.Model;

namespace GroupControl.IBLL
{
    public interface INewFriendStateBLL : IBaseBLL<NewFriendState, NewFriendStateViewModel>
    {
        /// <summary>
        /// 添加设备新朋友状态记录 或者 更新新朋友状态记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ExcuteResultViewModel SaveChange(NewFriendStateViewModel model);


        /// <summary>
        /// 批量更新设备回收数据状态
        /// </summary>
        void BatchUpdateRecoveryState(IList<NewFriendState> viewModelList);
    }
}
