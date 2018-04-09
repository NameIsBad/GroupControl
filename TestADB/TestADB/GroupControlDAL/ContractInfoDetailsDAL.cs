using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Dapper;
using DapperExtensions;

using GroupControl.IDAL;
using GroupControl.Model;
using GroupControl.Common;
using System.Data;
using System.Data.SqlClient;

namespace GroupControl.DAL
{
    public class ContractInfoDetailsDAL : BaseDAL<ContractInfoDetail, BaseViewModel>,IContractInfoDetailsDAL
    {
        public override void BatchInsert(IList<ContractInfoDetail> t, IDbConnection conn = null, IDbTransaction tran = null)
        {
            CommonAction<IList<ContractInfoDetail>>(t, conn, tran, (model, con, tr) =>
            {

                var parameters = new List<IDataParameter> { new SqlParameter("@TVP_ContractInfoDetail", SqlDbType.Structured)};

                parameters[0].Value = DataUtil.ToDataTable<ContractInfoDetail>(model);

                SingleHepler<SQLAdaptor>.Instance.ExecuteNonQuery(tr, "Pro_BatchContractInfoDetail", parameters, CommandType.StoredProcedure);


            });
        }

        /// <summary>
        /// 根据获取多个设备相关的导入信息列表
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public IList<ContractInfoDetail> GetListWithContractInfoDetailList(IList<ContractInfoDetail> list)
        {
            var returnData = new Lazy<List<ContractInfoDetail>>();

            var parameters = new List<IDataParameter> { new SqlParameter("@TVP_ContractInfoDetail", SqlDbType.Structured) };

            parameters[0].Value = DataUtil.ToDataTable<ContractInfoDetail>(list);

            using (var reader = SingleHepler<SQLAdaptor>.Instance.ExecuteReader("Pro_Get_ContractInfoDetailList", parameters, CommandType.StoredProcedure))
            {

                while (reader.Read())
                {
                    var currentModel = new ContractInfoDetail();

                    currentModel.Device = Convert.IsDBNull(reader["Device"]) ? string.Empty : Convert.ToString(reader["Device"]);

                    currentModel.Content = Convert.IsDBNull(reader["Content"]) ? string.Empty : Convert.ToString(reader["Content"]);

                    returnData.Value.Add(currentModel);

                }

                reader.Close();
            }

            return returnData.Value;
        }

    }
}
