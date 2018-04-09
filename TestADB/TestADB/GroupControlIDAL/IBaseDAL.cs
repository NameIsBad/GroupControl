using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GroupControl.Model;

namespace GroupControl.IDAL
{
    public interface IBaseDAL<T, TViewModel> where T : class, new() where TViewModel : BaseViewModel, new()
    {

        IDbConnection Conn { get; }

        int Insert(T t, IDbConnection conn = null, IDbTransaction tran = null);

        void BatchInsert(IList<T> t, IDbConnection conn = null, IDbTransaction tran = null);

        bool Update(T t, IDbConnection conn = null, IDbTransaction tran = null);

        bool Delete(T t, IDbConnection conn = null, IDbTransaction tran = null);

        T Get(int ID, IDbConnection conn = null, IDbTransaction tran = null);

        T Get(string id, IDbConnection conn = null, IDbTransaction tran = null);

        /// <summary>
        /// 动态查询条件
        /// </summary>
        /// <param name="predGroup"></param>
        /// <returns></returns>
        IList<T> GetList(QueryTermViewModel queryTermViewModel, IDbConnection conn = null, IDbTransaction tran = null);


        /// <summary>
        /// 拼接动态查询条件
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        IList<T> GetList(TViewModel viewModel);

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="t"></param>
        PagerViewModel<T> GetPageList(TViewModel viewModel, QueryTermViewModel queryTermViewModel, IDbConnection conn = null, IDbTransaction tran = null);

        /// <summary>
        /// 异步分页
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="queryTermViewModel"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        PagerViewModel<T> GetAsnyPageList(TViewModel viewModel, QueryTermViewModel queryTermViewModel, IDbConnection conn = null, IDbTransaction tran = null);


        /// <summary>
        /// 联合查询
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        PagerViewModel<TViewModel> GetPageListWithUnion(TViewModel viewModel);


        /// <summary>
        /// 联合查询
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        IList<TViewModel> GetListWithUnion(TViewModel viewModel);
    }
}
