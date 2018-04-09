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
   public  class AreaToPhoneNumberDAL : BaseDAL<AreaToPhoneNumber, AreaToPhoneNumberViewModel>, IAreaToPhoneNumberDAL
    {
        public override void BatchInsert(IList<AreaToPhoneNumber> t, IDbConnection conn = null, IDbTransaction tran = null)
        {
            CommonAction<IList<AreaToPhoneNumber>>(t, conn, tran, (model, con, tr) =>
            {

                var parameters = new List<IDataParameter> { new SqlParameter("@TVP_AreaToPhoneNumber", SqlDbType.Structured) };

                parameters[0].Value = DataUtil.ToDataTable<AreaToPhoneNumber>(model);

                SingleHepler<SQLAdaptor>.Instance.ExecuteNonQuery(con, "Pro_BatchTVP_AreaToPhoneNumber", parameters, CommandType.StoredProcedure);


            });
        }

        public override IList<AreaToPhoneNumber> GetList(AreaToPhoneNumberViewModel viewModel)
        {
            return CommonAction<AreaToPhoneNumberViewModel, IList<AreaToPhoneNumber>>(viewModel, null, null, (o, con, tr) =>
            {
                viewModel.SQLStr = @"SELECT  [ID]
                                          ,[NumberSection]
                                          ,[AreaCode]
                                          ,[Type]
                                          ,[LinkUrl]
                                      FROM [AreaToPhoneNumber]";

                return con.QueryAsync<AreaToPhoneNumber>(PageHelper.CreateListSql(o), o.ParamDict, tr).Result.ToList();

            });
        }
    }
}
