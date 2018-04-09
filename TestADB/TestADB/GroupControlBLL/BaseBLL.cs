using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GroupControl.DAL;
using GroupControl.IBLL;
using GroupControl.Model;
using GroupControl.IDAL;

namespace GroupControl.BLL
{
    public class BaseBLL<T1, T2> : IBaseBLL<T1, T2>
        where T1 : class, new()
        where T2 : BaseViewModel, new()
    {

        protected IBaseDAL<T1, T2> _dal = new BaseDAL<T1, T2>();

        public ExcuteResultViewModel Delete(T1 model)
        {
            var returnData = _dal.Delete(model);

            return new ExcuteResultViewModel() { ResultStatus = returnData ? EnumStatus.Success : EnumStatus.Error };
        }

        public T1 GetModel(int id)
        {
            return _dal.Get(id);
        }

        public virtual T2 GetModel(T2 t)
        {
            return default(T2);
        }

        public ExcuteResultViewModel Insert(T1 model)
        {
            var returnData = _dal.Insert(model);

            return new ExcuteResultViewModel() { ResultStatus = returnData > 0 ? EnumStatus.Success : EnumStatus.Error,ResultID=returnData };
        }

        public virtual ExcuteResultViewModel Update(T1 model)
        {
            var returnData = _dal.Update(model);

            return new ExcuteResultViewModel() { ResultStatus = returnData ? EnumStatus.Success : EnumStatus.Error };
        }

        public virtual IList<T2> GetListWithUnion(T2 viewModel)
        {
            return _dal.GetListWithUnion(viewModel);
        }

        public virtual IList<T1> GetList(T2 viewModel)
        {
            return _dal.GetList(new QueryTermViewModel());
        }

        public virtual PagerViewModel<T2> GetPageListWithUnion(T2 viewModel)
        {
            return _dal.GetPageListWithUnion(viewModel);
        }

        public virtual PagerViewModel<T1> GetPageList(T2 viewModel)
        {

            return _dal.GetPageList(viewModel, new QueryTermViewModel());
        }

        public virtual PagerViewModel<T1> GetAsnyPageList(T2 viewModel)
        {
            return _dal.GetAsnyPageList(viewModel, new QueryTermViewModel());
        }


        ///// <summary>
        ///// 获取查询条件
        ///// </summary>
        ///// <param name="viewModel"></param>
        ///// <param name="func"></param>
        ///// <returns></returns>
        //protected virtual PredicateGroup GetQueryTerms(T2 viewModel, Func<T2, PredicateGroup, PredicateGroup> func)
        //{
        //    var pg = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };

        //    return func(viewModel, pg);
        //}


        public virtual ExcuteResultViewModel BatchInsert(IList<T1> model)
        {
            if (null == model)
            {
                return new ExcuteResultViewModel() { ResultStatus = EnumStatus.Error };
            }

            _dal.BatchInsert(model);

            return new ExcuteResultViewModel() { ResultStatus = EnumStatus.Success };
        }
    }
}
