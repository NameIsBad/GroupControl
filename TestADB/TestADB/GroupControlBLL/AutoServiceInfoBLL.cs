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
    public class AutoServiceInfoBLL : BaseBLL<AutoServiceInfo, AutoServiceInfoViewModel>, IAutoServiceInfoBLL
    {

        public AutoServiceInfoBLL()
        {
            _dal = SingleHepler<AutoServiceInfoDAL>.Instance;
        }

        public IList<AutoServiceInfoViewModel> GetTaskListWithGroupInfo(AutoServiceInfoViewModel viewModel)
        {
            var list =GetList(viewModel);

            var currentViewModelList = new Lazy<List<AutoServiceInfoViewModel>>();

            if (null==list  || list.Count==0)
            {
                return default(List<AutoServiceInfoViewModel>);
            }

            foreach (var item in list)
            {

                var currentModel  = new GroupInfoViewModel() { ParamDict = new Dictionary<string, object>() { { "AutoServiceID", item.ID } } };

                var groupViewModelList = SingleHepler<GroupInfoDAL>.Instance.GetUnionListWithTask(currentModel);

                currentViewModelList.Value.Add(new AutoServiceInfoViewModel() { AutoServiceInfoModel=item,GroupInfoViewModelList=groupViewModelList });

            }


            return currentViewModelList.Value;

        }

        public override IList<AutoServiceInfo> GetList(AutoServiceInfoViewModel viewModel)
        {
            var whereStr = new StringBuilder("1=1");

            IDictionary<string, object> _dic = new Dictionary<string, object>();

            if (viewModel.ComputerID > 0)
            {
                whereStr.Append(" AND ComputerID=@ComputerID ");

                _dic.Add("ComputerID", viewModel.ComputerID);
            }

            if (((int)viewModel.Status) > 0)
            {
                whereStr.Append(" AND Status=@Status ");

                _dic.Add("Status", viewModel.Status);
            }

            if (((int)viewModel.TaskType) > 0)
            {
                whereStr.Append(" AND ServiceType=@TaskType ");

                _dic.Add("TaskType", viewModel.TaskType);
            }

            ///检测当天的任务
            whereStr.Append(" AND DateDiff(DAY,StartDate,getDate())=0");

            viewModel.ParamDict = _dic;

            viewModel.IsDesc = true;

            viewModel.WhereStr = whereStr.ToString();

            return _dal.GetList(viewModel);
        }


        /// <summary>
        /// 添加任务关联多个或者一个设备
        /// </summary>
        /// <param name="viewModel"></param>
        public int AddFriendTask(AutoServiceInfoViewModel viewModel)
        {

            if (null != viewModel.GroupIDs && viewModel.GroupIDs.Count > 0)
            {
                var list = SingleHepler<GroupInfoBLL>.Instance.GetGroupToDeviceList(new DeviceToNickNameViewModel() { Devices = viewModel.GroupIDs });

                if (null != list && list.Count > 0)
                {
                    viewModel.Devices = list.Select(o => o.Device).ToList();
                }
            }

            return SingleHepler<AutoServiceInfoDAL>.Instance.ActionWithTran<int>(viewModel, (o, conn, tran) =>
            {
                var currentModel = o as AutoServiceInfoViewModel;

                var returnData = _dal.Insert(currentModel.AutoServiceInfoModel, conn, tran);

                var autoServiceToDeviceDal = SingleHepler<AutoServiceToDeviceDAL>.Instance;

                autoServiceToDeviceDal.Delete(new AutoServiceToDevice() { AutoServiceID = returnData }, conn, tran);

                if (null != currentModel.Devices && currentModel.Devices.Count > 0)
                {

                    var devices = currentModel.Devices.Select(x => { return new AutoServiceToDevice() { AutoServiceID = returnData, Device = x }; }).ToList();

                    autoServiceToDeviceDal.BatchInsert(devices, conn, tran);

                }

                return returnData;

            });
        }


        /// <summary>
        /// 获取当前任务相关连的设备
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public IList<AutoServiceToDevice> GetTaskAboutDeviceList(AutoServiceInfoViewModel viewModel)
        {
            var pg = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };

            pg.Predicates.Add(Predicates.Field<AutoServiceToDevice>(f => f.AutoServiceID, Operator.Eq, viewModel.ID));

            return SingleHepler<AutoServiceToDeviceDAL>.Instance.GetList(new QueryTermViewModel() { PredGroup = pg });
        }
    }
}
