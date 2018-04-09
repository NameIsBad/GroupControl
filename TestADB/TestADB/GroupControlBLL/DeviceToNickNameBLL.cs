using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Dapper;
using DapperExtensions;

using GroupControl.IBLL;
using GroupControl.DAL;
using GroupControl.Model;
using GroupControl.Common;

namespace GroupControl.BLL
{
    public class DeviceToNickNameBLL : BaseBLL<DeviceToNickName, DeviceToNickNameViewModel>, IDeviceToNickNameBLL
    {

        public DeviceToNickNameBLL()
        {
            _dal = SingleHepler<DeviceToNickNameDAL>.Instance;
        }

        public override IList<DeviceToNickName> GetList(DeviceToNickNameViewModel viewModel)
        {
            var whereStr = new StringBuilder("1=1");

            IDictionary<string, object> _dic = new Dictionary<string, object>();

            if (!string.IsNullOrEmpty(viewModel.Device))
            {
                whereStr.Append(" AND Device=@Device ");

                _dic.Add("Device", viewModel.Device);
            }

            viewModel.ParamDict = _dic;

            viewModel.IsDesc = false;

            viewModel.OrderStr = "NickName";

            viewModel.IsDesc = false;

            viewModel.WhereStr = whereStr.ToString();

            return _dal.GetList(viewModel);
        }


        /// <summary>
        /// 更新粉丝数
        /// </summary>
        /// <param name="viewModel"></param>
        public void UpDateFriendCount(DeviceToNickNameViewModel viewModel)
        {
            var list = GetList(viewModel);

            if (null != list && list.Count > 0)
            {
                var currentEquipment = list.FirstOrDefault();

                currentEquipment.FriendCount = viewModel.FriendCount;

                currentEquipment.CreateDate = DateTime.Now;

                SingleHepler<DeviceToNickNameDAL>.Instance.Update(currentEquipment);

                return;
            }

            SingleHepler<DeviceToNickNameDAL>.Instance.Insert(new DeviceToNickName() { CreateDate = DateTime.Now, Device = viewModel.Device, NickName = viewModel.Device, FriendCount = viewModel.FriendCount });

        }


        /// <summary>
        /// 批量获取设备中的微信粉丝数量
        /// </summary>
        /// <param name="list"></param>
        /// <param name="connn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public IList<DeviceToNickName> GetEquipmentFriendCountList(IList<string> deviceList)
        {
            if (null == deviceList || deviceList.Count == 0)
            {
                return new List<DeviceToNickName>();
            }

            var list = deviceList.Select(o =>
            {

                return new DeviceToNickName()
                {
                    Device = o,

                    NickName = o,

                    CreateDate = DateTime.Now,

                    FriendCount = 0

                };

            }).ToList();


            return SingleHepler<DeviceToNickNameDAL>.Instance.GeDeviceListWithUsingEquipments(list);
        }


        /// <summary>
        ///传入当前使用的设备
        ///获取当前分组相关的设备与传入的设备相关联
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public IList<DeviceToNickNameViewModel> GetEquipmentListWithGroupInfo(DeviceToNickNameViewModel viewModel)
        {
            if (null == viewModel.Devices || viewModel.Devices.Count==0)
            {
                return default(IList<DeviceToNickNameViewModel>);
            }

            ///获取传入的设备的信息
            var currentEquipmentList = SingleHepler<DeviceToNickNameDAL>.Instance.GeDeviceListWithUsingEquipments(viewModel.Devices.Select(o =>
               {

                   return new DeviceToNickName()
                   {
                       Device = o,

                       NickName = o,

                       CreateDate = DateTime.Now,

                       FriendCount = 0

                   };

               }).ToList());


            if (null != currentEquipmentList && currentEquipmentList.Count > 0)
            {

                IList<GroupInfoViewModel> list = null;

                var whereStr = new StringBuilder("1=1");

                IDictionary<string, object> _dic = new Dictionary<string, object>();

                ///表示要获取当前分组相关的设备
                if (viewModel.GroupID > 0)
                {

                    whereStr.Append(" AND ID=@ID ");

                    _dic.Add("ID", viewModel.GroupID);

                }

                list = SingleHepler<GroupInfoDAL>.Instance.GetListWithUnion(new GroupInfoViewModel() { ParamDict = _dic, WhereStr = whereStr.ToString() });

                if (null!=list && list.Count>0)
                {

                    var cuurentEquipmentWithGroupList = new List<DeviceToNickNameViewModel>();

                    currentEquipmentList.ToList().ForEach((item) =>
                    {

                        var currentViewModel =new  DeviceToNickNameViewModel();

                        currentViewModel.Device = item.Device;

                        currentViewModel.NickName = item.NickName;

                        currentViewModel.ID = item.ID;

                        var currentValidateModelList = list.Where(o => item.Device.Equals(o.GroupToDeviceModel.Device)).ToList();

                        if (null!= currentValidateModelList  && currentValidateModelList.Count>0 )
                        {
                            var currentValidateModel = currentValidateModelList.FirstOrDefault();

                            if (currentValidateModel.GroupToDeviceModel.Device.Equals(item.Device))
                            {
                                currentViewModel.GroupID = currentValidateModel.GroupInfoModel.ID;

                                currentViewModel.GroupName = currentValidateModel.GroupInfoModel.Name;
                            }

                          
                        }

                        cuurentEquipmentWithGroupList.Add(currentViewModel);

                    });

                    return cuurentEquipmentWithGroupList;
                }


                return currentEquipmentList.Select(o=> {

                    return new DeviceToNickNameViewModel()
                    {

                        Device = o.Device,

                        ID = o.ID,

                        NickName = o.NickName


                    };

                }).ToList();

            }

            return default(IList<DeviceToNickNameViewModel>);
        }


        /// <summary>
        /// 获取导入失败的设备信息列表
        /// </summary>
        /// <returns></returns>
        public IList<DeviceToNickName> GetImportFailedDeviceList()
        {
            var deviceList = SingleHepler<NewFriendStateDAL>.Instance.GetListWithImportFailed();

            var deviceInfoList = new Lazy<List<DeviceToNickNameViewModel>>();

            if (deviceList == null || deviceList.Count == 0)
            {
                return default(IList<DeviceToNickName>);
            }

            ///获取传入的设备的信息
            return SingleHepler<DeviceToNickNameDAL>.Instance.GeDeviceListWithUsingEquipments(deviceList.Select(o =>
            {

                return new DeviceToNickName()
                {
                    Device = o.Device,

                    NickName = string.Empty,

                    CreateDate = DateTime.Now,

                    FriendCount = 0

                };

            }).ToList());

        }
    }
}
