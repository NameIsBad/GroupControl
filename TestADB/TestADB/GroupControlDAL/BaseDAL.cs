using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Dapper;
using DapperExtensions;

using GroupControl.IDAL;
using GroupControl.Model;
using GroupControl.Common;

namespace GroupControl.DAL
{
    public class BaseDAL<T, TViewModel> : IBaseDAL<T, TViewModel>
            where T : class, new()
            where TViewModel : BaseViewModel, new()
    {

        private static readonly object _obj = new object();

        public IDbConnection Conn
        {
            get
            {
                return SingleHepler<ConnectionFactory>.Instance.CreateConnection();
            }
        }


        public void CommonAction(IDbConnection conn, Action<IDbConnection> func)
        {
            if (null != conn)
            {
                return;
            }

            using (conn = new SqlConnection(ConnectionFactory.connString))
            {
                try
                {
                    conn.Open();

                    func(conn);
                }
                catch (Exception ex)
                {
                    conn.Close();

                    throw;
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                }
            }
        }

        public void CommonAction<T1>(T1 t, IDbConnection conn, Action<T1, IDbConnection> func)
        {

            if (null != conn)
            {
                return;
            }

            using (conn = new SqlConnection(ConnectionFactory.connString))
            {
                try
                {
                    conn.Open();

                    func(t, conn);
                }
                catch (Exception ex)
                {
                    conn.Close();

                    throw;
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                }
            }
        }

        public T1 CommonAction<T1>(IDbConnection conn,  Func<IDbConnection,T1> func)
        {
            if (null == conn)
            {
                conn = new SqlConnection(ConnectionFactory.connString);
            }

            using (conn)
            {
                try
                {
                    conn.Open();

                    return  func(conn);
                }
                catch (Exception ex)
                {
                    conn.Close();

                    throw;
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                }

                //  return default(T2);
            }
        }

        public T2 CommonAction<T1, T2>(T1 t, IDbConnection conn, Func<T1, IDbConnection, T2> func)
        {
            if (null == conn)
            {
                conn = new SqlConnection(ConnectionFactory.connString);
            }

            using (conn)
            {
                try
                {
                    conn.Open();

                    return func(t, conn);
                }
                catch (Exception ex)
                {
                    conn.Close();

                    throw;
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                }

                //  return default(T2);
            }
        }

        public void CommonAction<T1>(T1 t, IDbConnection conn, IDbTransaction tran, Action<T1, IDbConnection, IDbTransaction> func)
        {

            if (null == conn)
            {
                using (conn = new SqlConnection(ConnectionFactory.connString))
                {
                    try
                    {
                        conn.Open();

                        func(t, conn, tran);
                    }
                    catch (Exception ex)
                    {
                        conn.Close();

                        throw;
                    }

                    finally
                    {
                        if (conn.State != ConnectionState.Closed)
                        {
                            conn.Close();
                        }
                    }
                }
            }
            else
            {
                func(t, conn, tran);
            }
        }

        public T2 CommonAction<T1, T2>(T1 t, IDbConnection conn, IDbTransaction tran, Func<T1, IDbConnection, IDbTransaction, T2> func)
        {
            if (null == conn)
            {

                using (conn = new SqlConnection(ConnectionFactory.connString))
                {
                    try
                    {
                        conn.Open();

                        return func(t, conn, tran);
                    }
                    catch (Exception ex)
                    {
                        conn.Close();

                        //  return default(T2);
                        throw;
                    }

                    finally
                    {
                        if (conn.State != ConnectionState.Closed)
                        {
                            conn.Close();
                        }
                    }
                }

            }

            return func(t, conn, tran);
        }

        public virtual void BatchInsert(IList<T> t, IDbConnection conn = null, IDbTransaction tran = null)
        {
            CommonAction<IList<T>>(t, conn, tran, (o, con, tr) =>
            {
                con.Insert<T>(o, tr);

            });
        }

        /// <summary>
        /// 带返回值的插入
        /// </summary>
        /// <typeparam name="RT"></typeparam>
        /// <param name="t"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public virtual RT BatchInsert<RT>(IList<T> t, IDbConnection conn = null, IDbTransaction tran = null)
        {
            return default(RT);
        }

        public virtual bool Delete(T t, IDbConnection conn = null, IDbTransaction tran = null)
        {
            return CommonAction<T, bool>(t, conn, tran, (o, con, tr) =>
            {
                return con.Delete<T>(t, tr);

            });
        }

        public virtual int Insert(T t, IDbConnection conn = null, IDbTransaction tran = null)
        {

            return CommonAction<T, int>(t, conn, tran, (o, con, tr) =>
            {
                return con.Insert<T>(o, tr);

            });
        }



        public bool Update(T t, IDbConnection conn = null, IDbTransaction tran = null)
        {
            return CommonAction<T, bool>(t, conn, tran, (o, con, tr) =>
            {
                return con.Update<T>(o, tr);

            });
        }

        public T Get(int id, IDbConnection conn = null, IDbTransaction tran = null)
        {
            return CommonAction<int, T>(id, conn, tran, (o, con, tr) =>
            {

                return con.Get<T>(id, tr);

            });
        }

        public T Get(string id, IDbConnection conn = null, IDbTransaction tran = null)
        {
            return CommonAction<string, T>(id, conn, tran, (o, con, tr) =>
            {

                return con.Get<T>(id, tr);

            });
        }

        /// <summary>
        /// 谓词动态查询条件
        /// </summary>
        /// <param name="predGroup"></param>
        /// <returns></returns>
        public IList<T> GetList(QueryTermViewModel queryTermViewModel, IDbConnection conn = null, IDbTransaction tran = null)
        {
            return CommonAction<QueryTermViewModel, IList<T>>(queryTermViewModel, conn, tran, (o, con, tr) =>
            {
                if (null == queryTermViewModel.SortList || queryTermViewModel.SortList.Count == 0)
                {
                    queryTermViewModel.SortList = new List<ISort>() { new Sort() { Ascending = true, PropertyName = "ID" } };
                }

                var list = con.GetList<T>(o.PredGroup, o.SortList, tr);

                if (null != list && list.Count() > 0)
                {
                    return list.ToList<T>();
                }

                return null;

            });
        }


        /// <summary>
        /// 拼接动态查询条件
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public virtual IList<T> GetList(TViewModel viewModel)
        {
            return default(List<T>);
        }


        /// <summary>
        /// 联合查询列表
        /// </summary>
        /// <param name="queryTermViewModel"></param>
        /// <returns></returns>
        public virtual IList<TViewModel> GetListWithUnion(TViewModel queryTermViewModel)
        {
            return new List<TViewModel>();
        }


        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="t"></param>
        public PagerViewModel<T> GetAsnyPageList(TViewModel viewModel, QueryTermViewModel queryTermViewModel, IDbConnection conn = null, IDbTransaction tran = null)
        {

            return CommonAction<BaseViewModel, PagerViewModel<T>>(viewModel, conn, tran, (o, con, tr) =>
            {

                if (null == queryTermViewModel.SortList || queryTermViewModel.SortList.Count == 0)
                {
                    queryTermViewModel.SortList = new List<ISort>() { new Sort() { Ascending = false, PropertyName = "ID" } };
                }

                return con.GetPageAsync<T>(queryTermViewModel.PredGroup, queryTermViewModel.SortList, viewModel.PageIndex - 1, viewModel.PageCount, tr).ContinueWith<PagerViewModel<T>>((task) =>
                {

                    var count = con.CountAsync<T>(null, tr).Result;

                    var result = task.Result;

                    var pager = new PagerViewModel<T>() { TotalCount = count };

                    if (null != result)
                    {
                        pager.ItemList = result.ToList<T>();
                    }

                    return pager;

                }).Result;
            });
        }


        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="t"></param>
        public PagerViewModel<T> GetPageList(TViewModel viewModel, QueryTermViewModel queryTermViewModel, IDbConnection conn = null, IDbTransaction tran = null)
        {

            return CommonAction<BaseViewModel, PagerViewModel<T>>(viewModel, conn, tran, (o, con, tr) =>
            {

                if (null == queryTermViewModel.SortList || queryTermViewModel.SortList.Count == 0)
                {
                    queryTermViewModel.SortList = new List<ISort>() { new Sort() { Ascending = false, PropertyName = "ID" } };
                }

                var list = con.GetPage<T>(queryTermViewModel.PredGroup, queryTermViewModel.SortList, viewModel.PageIndex - 1, viewModel.PageCount, tr);

                var count = con.Count<T>(queryTermViewModel.PredGroup, tr);

                var pager = new PagerViewModel<T>() { TotalCount = count };

                if (null != list)
                {
                    pager.ItemList = list.ToList<T>();
                }

                return pager;
            });
        }


        /// <summary>
        /// 联合查询
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public virtual PagerViewModel<TViewModel> GetPageListWithUnion(TViewModel viewModel)
        {
            return new PagerViewModel<TViewModel>();
        }


        protected PagerViewModel<TViewModel> ReturnPager(IEnumerable<TViewModel> returnData, BaseViewModel viewModel, IDbConnection conn)
        {
            var pager = new PagerViewModel<TViewModel>();

            if (null != returnData)
            {
                pager.ItemList = returnData.ToList();
            }

            var totalCountResult = conn.ExecuteReaderAsync(PageHelper.CreateTotalCountSql(viewModel), viewModel.ParamDict);

            var totalCountReader = totalCountResult.Result;

            if (totalCountReader.Read())
            {
                pager.TotalCount = Convert.ToInt32(totalCountReader["TotalCount"]);
            }

            pager.PageIndex = viewModel.PageIndex;

            pager.PageCount = viewModel.PageCount;

            return pager;
        }


        /// <summary>
        /// 开启事务
        /// </summary>
        /// <param name="func"></param>
        protected void ActionWithTran(IDbConnection conn, Action<IDbConnection, IDbTransaction> func)
        {
            using (var tran = conn.BeginTransaction())
            {
                try
                {
                    func(conn, tran);

                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();

                    throw;
                }
                finally
                {
                    tran.Dispose();
                }
            }
        }

        public void ActionWithTran(object viewModel, Action<object, IDbConnection, IDbTransaction> func)
        {
            using (var _conn = new SqlConnection(ConnectionFactory.connString))
            {
                try
                {
                    _conn.Open();

                    using (var tran = _conn.BeginTransaction())
                    {
                        try
                        {

                            func(viewModel, _conn, tran);

                            tran.Commit();

                        }
                        catch (Exception ex)
                        {
                            tran.Rollback();

                            throw;
                        }
                        finally
                        {

                            tran.Dispose();

                        }
                    }
                }
                catch (Exception ex)
                {

                    _conn.Close();

                    throw;
                }

                finally
                {
                    if (_conn.State != ConnectionState.Closed)
                    {
                        _conn.Close();
                    }
                }
            }
        }


        public RT ActionWithTran<RT>(object viewModel, Func<object, IDbConnection, IDbTransaction, RT> func)
        {
            var rerturnData = default(RT);

            using (var _conn = new SqlConnection(ConnectionFactory.connString))
            {
                try
                {

                    _conn.Open();

                    using (var tran = _conn.BeginTransaction())
                    {
                        try
                        {

                            rerturnData = func(viewModel, _conn, tran);

                            tran.Commit();

                        }
                        catch (Exception ex)
                        {
                            tran.Rollback();

                            throw;
                        }
                        finally
                        {

                            tran.Dispose();

                        }
                    }
                }
                catch (Exception ex)
                {

                    _conn.Close();

                    throw;
                }

                finally
                {
                    if (_conn.State != ConnectionState.Closed)
                    {
                        _conn.Close();
                    }
                }
            }

            return rerturnData;
        }
    }
}
