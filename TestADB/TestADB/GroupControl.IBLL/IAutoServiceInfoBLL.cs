using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GroupControl.Model;

namespace GroupControl.IBLL
{
    public interface IAutoServiceInfoBLL : IBaseBLL<AutoServiceInfo, AutoServiceInfoViewModel>
    {
        /// <summary>
        /// 添加任务关联多个或者一个设备
        /// </summary>
        /// <param name="viewModel"></param>
        int AddFriendTask(AutoServiceInfoViewModel viewModel);

        /// <summary>
        /// 获取当前任务相关连的设备
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        IList<AutoServiceToDevice> GetTaskAboutDeviceList(AutoServiceInfoViewModel viewModel);

        /// <summary>
        /// /获取跟任务列表相关的分组列表
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>

        IList<AutoServiceInfoViewModel> GetTaskListWithGroupInfo(AutoServiceInfoViewModel viewModel);
    }
}
