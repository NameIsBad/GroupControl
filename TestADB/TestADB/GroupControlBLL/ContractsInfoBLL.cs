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
    public class ContractsInfoBLL : BaseBLL<ContractsInfo, ContractsInfoViewModel>, IContractsInfoBLL
    {


        public override IList<ContractsInfo> GetList(ContractsInfoViewModel viewModel)
        {
            var queryTermViewModel = new QueryTermViewModel() { };

            var pg = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };

            return SingleHepler<ContractsInfoDAL>.Instance.GetList(queryTermViewModel);
        }

        public override IList<ContractsInfoViewModel> GetListWithUnion(ContractsInfoViewModel viewModel)
        {
            var pg = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };

            ///GT  大于  
            pg.Predicates.Add(Predicates.Field<ContractsInfo>(f => f.CreateDate, Operator.Gt, DateTime.Now.AddDays(-15)));

            var listData = SingleHepler<ContractsInfoDAL>.Instance.GetList(new QueryTermViewModel()
            {
                PredGroup =pg, SortList = new List<ISort>() { new Sort() { Ascending = false, PropertyName = "ID" } }
            });

            if (null == listData || listData.Count == 0)
            {
                return default(IList<ContractsInfoViewModel>);
            }

            Func<IList<ContractsInfo>, Task<IList<ContractsInfoViewModel>>> _currentAction = async (o) =>
             {
                 var returnData = await Task.Factory.StartNew((obj) =>
                 {
                     var currentContractInfo = obj as IList<ContractsInfo>;

                     return SingleHepler<DeviceToNickNameDAL>.Instance.GetListWithUnion(

                         new DeviceToNickNameViewModel()
                         {
                             ContractsInfoList = listData

                         });

                 }, o);

                 if (null == returnData || returnData.Count == 0)
                 {
                     return default(IList<ContractsInfoViewModel>);
                 }

                 return await Task.Factory.StartNew<IList<ContractsInfoViewModel>>(() =>
                 {
                     var returnListData = new Lazy<List<ContractsInfoViewModel>>();

                     o.ToList().ForEach(item =>
                     {
                         var list = returnData.Where(p => p.ContractInfoID == item.ID).ToList();

                         returnListData.Value.Add(new ContractsInfoViewModel()
                         {

                             ContractsInfoModel = item,

                             DeviceToNickNameList = list

                         });

                     });

                     return returnListData.Value;

                 });
             };

            return _currentAction.Invoke(listData).GetAwaiter().GetResult();

        }

        public void InsertContractWithContractDetail(ContractsInfoViewModel viewModel)
        {
            SingleHepler<ContractsInfoDAL>.Instance.ActionWithTran(viewModel, (o, conn, tran) =>
            {

                var currentViewModel = o as ContractsInfoViewModel;

                if (null == currentViewModel.ContractsInfoModel)
                {
                    return;
                }

                var returnData = _dal.Insert(currentViewModel.ContractsInfoModel, conn, tran);

                var list = currentViewModel.ContractInfoDetailList;

                if (null == list || list.Count == 0)
                {
                    return;
                }

                list = list.Select(q =>
                {
                    q.ContractsInfoID = returnData;

                    return q;

                }).ToList();

                SingleHepler<ContractInfoDetailsDAL>.Instance.BatchInsert(list, conn, tran);

            });
        }


        /// <summary>
        /// 根据获取多个设备相关的导入信息列表
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public IList<ContractInfoDetail> GetListWithContractInfoDetailList(IList<ContractInfoDetail> list)
        {
            if (null == list || list.Count == 0)
            {
                return default(IList<ContractInfoDetail>);
            }

            return SingleHepler<ContractInfoDetailsDAL>.Instance.GetListWithContractInfoDetailList(list);
        }
    }
}
