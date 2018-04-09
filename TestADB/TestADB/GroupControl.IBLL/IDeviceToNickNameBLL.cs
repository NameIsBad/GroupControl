using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GroupControl.Model;

namespace GroupControl.IBLL
{
    public interface IDeviceToNickNameBLL : IBaseBLL<DeviceToNickName, DeviceToNickNameViewModel>
    {

        /// <summary>
        /// 更新粉丝数
        /// </summary>
        /// <param name="viewModel"></param>
        void UpDateFriendCount(DeviceToNickNameViewModel viewModel);

        /// <summary>
        /// 批量获取设备中的微信粉丝数量
        /// </summary>
        /// <param name="list"></param>
        /// <param name="connn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        IList<DeviceToNickName> GetEquipmentFriendCountList(IList<string> deviceList);

        IList<DeviceToNickName> GetImportFailedDeviceList();
    }
}
