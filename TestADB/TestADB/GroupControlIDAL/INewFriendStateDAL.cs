using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GroupControl.Model;

namespace GroupControl.IDAL
{
    public interface INewFriendStateDAL : IBaseDAL<NewFriendState, NewFriendStateViewModel>
    {

        /// <summary>
        /// 导入手机通讯录之后 
        /// 在微信通讯录里面不显示
        /// 此时要对数据进行回收 
        /// 此时这个获取的列表就是 导入通讯录成功 但是在微信通讯录上面不显示的 相关的设备列表
        /// </summary>
        /// <returns></returns>
        IList<NewFriendState> GetListWithImportFailed();

        /// <summary>
        /// 批量更新设备回收数据状态
        /// </summary>
        void BatchUpdateRecoveryState(IList<NewFriendState> viewModelList);
    }
}


