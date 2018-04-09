using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GroupControl.Model;

namespace GroupControl.IDAL
{
    public interface IDeviceToNickNameDAL : IBaseDAL<DeviceToNickName, DeviceToNickNameViewModel>
    {
        /// <summary>
        /// 批量获取设备中的微信粉丝数量
        /// </summary>
        /// <param name="list"></param>
        /// <param name="connn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        IList<DeviceToNickName> GeDeviceListWithUsingEquipments(IList<DeviceToNickName> list);
    }
}
