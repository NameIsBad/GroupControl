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
    public class NewFriendStateBLL : BaseBLL<NewFriendState, NewFriendStateViewModel>, INewFriendStateBLL
    {
        /// <summary>
        /// 添加设备新朋友状态记录 或者 更新新朋友状态记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ExcuteResultViewModel SaveChange(NewFriendStateViewModel model)
        {
            var returnData = new Lazy<ExcuteResultViewModel>(() => new ExcuteResultViewModel() { ResultID = 0, ResultStatus = EnumStatus.Error });

            if (string.IsNullOrEmpty(model.Device))
            {
                return returnData.Value;
            }

            var pg = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };

            pg.Predicates.Add(Predicates.Field<NewFriendState>(f => f.Device, Operator.Eq, model.Device));

            var modelList = _dal.GetList(new QueryTermViewModel() { PredGroup = pg });


            ///没有状态记录
            if (null == modelList || modelList.Count == 0)
            {
                var newFriendStateModel = new NewFriendState()
                {
                    Device = model.Device,

                    CheckDate = DateTime.Now,

                    RecoveryDate = DateTime.Parse(DataUtil.MIN_DATE_TIME),

                    IsRecovered = false,

                    ImportDate = model.IsImportContracts ? DateTime.Now :DateTime.Parse(DataUtil.MIN_DATE_TIME),

                    IsHaveNewFriend = model.IsHaveNewFriend ? true : false

                };

                returnData.Value.ResultID = _dal.Insert(newFriendStateModel);

                returnData.Value.ResultStatus = EnumStatus.Success;

                return returnData.Value;
            }

            var currentModel = modelList.FirstOrDefault();

            Action<Action> currentAction = (action) =>
            {

                currentModel.IsHaveNewFriend = model.IsHaveNewFriend ? true : false;

                action();

            };

            Action importAction = () =>
            {
                currentModel.IsRecovered = false;

                currentModel.ImportDate = DateTime.Now;
            };

            Action CheckAction = () =>
            {
                currentModel.CheckDate = DateTime.Now;
            };

            currentAction(model.IsImportContracts ? importAction : CheckAction);

            returnData.Value.ResultID = currentModel.ID;

            returnData.Value.ResultStatus = _dal.Update(currentModel) ? EnumStatus.Success : EnumStatus.Error;

            return returnData.Value;
        }


        public override IList<NewFriendState> GetList(NewFriendStateViewModel viewModel)
        {

            var returnUseingEquipmentList = new Lazy<List<NewFriendState>>();

            var pg = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };

            pg.Predicates.Add(Predicates.Field<NewFriendState>(f => f.IsHaveNewFriend, Operator.Eq, false));


            Action<DateTime> currentAction = (time) =>
            {

                pg.Predicates.Add(Predicates.Field<NewFriendState>(f => f.ImportDate, Operator.Lt, time));

            };

            switch (viewModel.SelectDateTimeType)
            {
                case EnumSelectDateTimeType.OneDay:

                    currentAction.Invoke(DateTime.Now.AddDays(-1));

                    break;

                case EnumSelectDateTimeType.OneWeek:

                    currentAction.Invoke(DateTime.Now.AddDays(-7));

                    break;

                case EnumSelectDateTimeType.OneMonth:

                    currentAction.Invoke(DateTime.Now.AddMonths(-1));

                    break;

                case EnumSelectDateTimeType.ThreeMonth:

                    currentAction.Invoke(DateTime.Now.AddMonths(-3));

                    break;

                case EnumSelectDateTimeType.SixMonth:

                    currentAction.Invoke(DateTime.Now.AddMonths(-6));

                    break;

                case EnumSelectDateTimeType.OneYear:

                    currentAction.Invoke(DateTime.Now.AddYears(-1));

                    break;

                default:

                    ///是否是自定义时间
                    if (viewModel.CustomCheckDay > 0)
                    {
                        currentAction.Invoke(DateTime.Now.AddDays(-viewModel.CustomCheckDay));
                    }

                    break;
            }


            var queryDataList = _dal.GetList(new QueryTermViewModel() { PredGroup = pg });

            if (null == queryDataList || queryDataList.Count == 0)
            {
                return default(IList<NewFriendState>);
            }

            else if (null == viewModel.CurentUseingEquipmentList || viewModel.CurentUseingEquipmentList.Count == 0)
            {
                return queryDataList;
            }


            queryDataList.ToList().ForEach((item) =>
            {

                if (viewModel.CurentUseingEquipmentList.Any(o => o == item.Device))
                {
                    returnUseingEquipmentList.Value.Add(item);
                }


            });


            return returnUseingEquipmentList.Value;

        }

        /// <summary>
        /// 批量更新设备回收数据状态
        /// </summary>
        public void BatchUpdateRecoveryState(IList<NewFriendState> viewModelList)
        {
            if (null== viewModelList || viewModelList.Count==0)
            {
                return;
            }

            SingleHepler<NewFriendStateDAL>.Instance.BatchUpdateRecoveryState(viewModelList);
        }

    }
}
