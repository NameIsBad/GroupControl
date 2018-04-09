using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Dapper;
using DapperExtensions;

using GroupControl.Model;

namespace GroupControl.Common
{
    public class PageHelper
    {

        /// <summary>
        /// 联合查询 分页
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public static PagerViewModel<T> GetPageList<T>(SQLViewModel viewModel) where T:class,new()
        {
            using (var conn =SingleHepler<ConnectionFactory>.Instance.CreateConnection())
            {
                return conn.QueryAsync<T>(viewModel.SQLStr, viewModel.Params).ContinueWith((task) =>
                {

                    var returnData = task.Result;

                    var totalCount = 0;

                    var totalResult = conn.ExecuteReaderAsync(viewModel.CounSQLStr, viewModel.PageCount).Result;

                    if (totalResult.Read())
                    {
                        totalCount = Convert.ToInt32(totalResult["TotalCount"]);
                    }

                    var pager = new PagerViewModel<T>() { TotalCount = totalCount };

                    if (null != returnData)
                    {
                        pager.ItemList = returnData.ToList<T>();
                    }

                    return pager;

                }).Result;
            }
        }

        public static string CreateSql(BaseViewModel viewModel, Func<BaseViewModel, int, int, string> func)
        {
            if (string.IsNullOrEmpty(viewModel.SQLStr))
            {
                throw new ArgumentNullException("sql语句不能为空号!");
            }

            var formatSqlStr = viewModel.SQLStr.ToLower();

            var selectStrIndex = formatSqlStr.IndexOf("select");

            var fromStrIndex = formatSqlStr.IndexOf("from");

            if (selectStrIndex > -1 && fromStrIndex > -1)
            {

                return func(viewModel, selectStrIndex, fromStrIndex);

            }
            else
            {
                throw new ArgumentException("sql语句错误");
            }
        }


        public static string CreateTotalCountSql(BaseViewModel viewModel)
        {


            return CreateSql(viewModel, (model, selectStrIndex, fromStrIndex) =>
            {
                var replaceStr = model.SQLStr.Substring(selectStrIndex + 6, fromStrIndex - (selectStrIndex + 6));

                var newSqlStr  = model.SQLStr.Replace(replaceStr, " count(1) as TotalCount ");

                if (!string.IsNullOrEmpty(viewModel.WhereStr))
                {
                    newSqlStr = string.Format("{0}  where  {1}", newSqlStr, viewModel.WhereStr);
                }

                return newSqlStr;

            });


        }


        /// <summary>
        /// 列表多表查询sql
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public static string CreateListSql(BaseViewModel viewModel)
        {
            return CreateSql(viewModel, (model, selectStrIndex, fromStrIndex) =>
            {
                var newSqlStr =model.SQLStr;

                if (!string.IsNullOrEmpty(viewModel.WhereStr))
                {
                    newSqlStr = string.Format("select * from ({0}) as _t where {1}", newSqlStr,viewModel.WhereStr);
                }

                if (string.IsNullOrEmpty(viewModel.OrderStr))
                {
                    viewModel.OrderStr = "ID";
                }


                newSqlStr = string.Format(" {0} order by {1} {2} ", newSqlStr, viewModel.OrderStr,viewModel.IsDesc?"desc":"asc");

                return newSqlStr;

            });
        }


        public static string CreatePageSql(BaseViewModel viewModel)
        {

            return CreateSql(viewModel, (model, selectStrIndex, fromStrIndex) =>
            {
                  var replaceStr = model.SQLStr.Substring(0, fromStrIndex);

                var newStr = string.Format(" {0} ,ROW_NUMBER() OVER( Order by {1} {2} ) AS RowNumber ", replaceStr, string.IsNullOrEmpty(model.OrderStr)?"ID": model.OrderStr, model.IsDesc ? "DESC" : "");

                newStr = model.SQLStr.Replace(replaceStr, newStr);

                newStr = string.Format("select * from ( {0} WHERE {1} ) as _temp where  RowNumber>(@PageIndex-1)*@PageCount and RowNumber<=@PageIndex*@PageCount", newStr, string.IsNullOrEmpty(model.WhereStr)?"":model.WhereStr);

                return newStr;

            });

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public static string CreatePageSqlWithNoWhereStr(BaseViewModel viewModel)
        {

            return CreateSql(viewModel, (model, selectStrIndex, fromStrIndex) =>
            {
                var replaceStr = model.SQLStr.Substring(0, fromStrIndex);

                var newStr = string.Format(" {0} ,ROW_NUMBER() OVER( Order by {1} {2} ) AS RowNumber ", replaceStr, string.IsNullOrEmpty(model.OrderStr) ? "ID" : model.OrderStr, model.IsDesc ? "DESC" : "");

                newStr = model.SQLStr.Replace(replaceStr, newStr);

                newStr = string.Format("select * from ({0}) as _temp where RowNumber>(@PageIndex-1)*@PageCount and RowNumber<=@PageIndex*@PageCount", newStr);

                return newStr;

            });

        }

        public static string CreateParamsStr(BaseViewModel viewModel)
        {
            var sqlStr = viewModel.SQLStr;

            var orderStr = viewModel.OrderStr;

            if (string.IsNullOrEmpty(viewModel.SQLStr))
            {
                throw new ArgumentNullException("sql语句不能为空！");
            }

            if (!string.IsNullOrEmpty(viewModel.WhereStr))
            {
                sqlStr=string.Format("{0} where {1}", sqlStr, viewModel.WhereStr);
            }

            if (string.IsNullOrEmpty(orderStr))
            {
                orderStr = "ID";
            }

            sqlStr = string.Format("select * from ({0}) as _t", sqlStr);

            return sqlStr;
        }

        #region 生成分页SQL语句

        /// <summary>
        /// 用于 sqlserver
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="selectSql"></param>
        /// <param name="SqlCount"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public static string GetPagingSql(int pageIndex, int pageSize, string selectSql, string SqlCount, string orderBy)
        {
            if (pageIndex == 0)
                pageIndex = 1;
            if (pageSize == 0)
                pageSize = int.MaxValue;
            StringBuilder sbSql = new StringBuilder("DECLARE @pageIndex int,@pageSize int\n");
            sbSql.AppendFormat("SET @pageIndex = {0}\n", pageIndex);
            sbSql.AppendFormat("SET @pageSize = {0}\n", pageSize);
            sbSql.AppendFormat("SELECT * FROM (SELECT *, ROW_NUMBER() OVER({0}) AS RankNumber from (\n", orderBy);
            sbSql.AppendFormat("{0}\n", selectSql);
            sbSql.Append(") as topT) AS subT\n");
            sbSql.Append(" WHERE rankNumber BETWEEN (@pageIndex-1)*@pageSize+1 AND @pageIndex*@pageSize\n");

            sbSql.AppendFormat("{0}\n", SqlCount);
            return sbSql.ToString();

        }
        #endregion




        /// <summary>
        /// 用于Oracle
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="selectSql"></param>
        /// <param name="SqlCount"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public static string GetOraclePagingSql(int pageIndex, int pageSize, string selectSql, string SqlCount, string orderBy) {
            if (pageIndex <= 0)
            {
                pageIndex = 1;
            }
            var toSkip = (pageIndex - 1) * pageSize;
            var topLimit = toSkip + pageSize;
            var sb = new StringBuilder();
            sb.AppendLine("SELECT * FROM (");
            sb.AppendLine("SELECT \"_ss_data_1_\".*, ROWNUM RNUM FROM (");
            sb.Append(selectSql.Trim().TrimEnd(';'));
            sb.AppendLine(") \"_ss_data_1_\"");
            sb.AppendFormat("WHERE ROWNUM <= {0}) \"_ss_data_2_\" ", topLimit);
            sb.AppendLine("");
            sb.AppendFormat("WHERE \"_ss_data_2_\".RNUM > {0} ", toSkip);
            sb.AppendLine("");
            //if (isReturnCount)
            //{
            //    sb.AppendLine(PageCount(sql));
            //}
            return sb.ToString();
        }



        /// <summary>
        /// MySql
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="selectSql"></param>
        /// <param name="SqlCount"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public static string GetMySqlPagingSql(int pageIndex, int pageSize, string selectSql, string SqlCount, string orderBy)
        {

            return "";
        }


    }
}
