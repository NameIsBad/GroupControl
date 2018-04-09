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
    public class GroupInfoBLL : BaseBLL<GroupInfo, BaseViewModel>, IGroupInfoBLL
    {
        public override IList<GroupInfo> GetList(BaseViewModel viewModel)
        {
            var queryTermViewModel = new QueryTermViewModel() { };

            queryTermViewModel.SortList = new List<ISort>() { new Sort() { Ascending = false, PropertyName = "ID" } };

            return SingleHepler<GroupInfoDAL>.Instance.GetList(queryTermViewModel);
        }

        public IList<GroupInfoViewModel> GetGroupListWithDevice(BaseViewModel viewModel)
        {
            Lazy<List<GroupInfoViewModel>> currentLazy = new Lazy<List<GroupInfoViewModel>>();

            var list = SingleHepler<GroupInfoDAL>.Instance.GetList(new QueryTermViewModel()
            {
                SortList = new List<ISort>() { new Sort() { Ascending = false, PropertyName = "ID" } }
            });

            if (null == list || list.Count == 0)
            {
                return default(List<GroupInfoViewModel>);
            }

            foreach (var groupInfo in list)
            {
                var equipmentList = SingleHepler<GroupToDeviceDAL>.Instance.GetList(new DeviceToNickNameViewModel() { Devices = new List<string>() { groupInfo.ID.ToString() } });

                currentLazy.Value.Add(new GroupInfoViewModel() { GroupInfoModel = groupInfo, GroupToDeviceList = equipmentList });

            }

            return currentLazy.Value;

        }

        public void BatchInsertGroupToDevice(IList<GroupToDevice> list)
        {
            if (null == list && list.Count > 0)
            {
                return;
            }

            var _dal = SingleHepler<GroupToDeviceDAL>.Instance;

            _dal.ActionWithTran(list, (o, conn, tran) =>
            {
                var currentList = o as IList<GroupToDevice>;

                ///删除之前设备关联的分组
                ///
                _dal.BatchDelete(currentList.Select(q => q.Device).ToList(), conn, tran);

                ///批量插入
                _dal.BatchInsert(list, conn, tran);

            });
        }

        public IList<GroupToDevice> GetGroupToDeviceList(DeviceToNickNameViewModel viewModel)
        {
            return SingleHepler<GroupToDeviceDAL>.Instance.GetList(viewModel);

        }
    }
}
