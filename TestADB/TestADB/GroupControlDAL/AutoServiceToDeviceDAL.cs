using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

using Dapper;
using DapperExtensions;

using GroupControl.IDAL;
using GroupControl.Model;
using GroupControl.Common;


namespace GroupControl.DAL
{
    public class AutoServiceToDeviceDAL : BaseDAL<AutoServiceToDevice, BaseViewModel>, IAutoServiceToDeviceDAL
    {
        public override void BatchInsert(IList<AutoServiceToDevice> t, IDbConnection conn = null, IDbTransaction tran = null)
        {
            CommonAction<IList<AutoServiceToDevice>>(t, conn, tran, (model, con, tr) =>
            {

                var parameters = new List<IDataParameter> { new SqlParameter("@TVP_AutoServiceToDevice", SqlDbType.Structured) };

                parameters[0].Value = DataUtil.ToDataTable<AutoServiceToDevice>(model);

                SingleHepler<SQLAdaptor>.Instance.ExecuteNonQuery(tr, "Pro_BatchAutoServiceToDevice", parameters, CommandType.StoredProcedure);


            });
        }

        public override bool Delete(AutoServiceToDevice t, IDbConnection conn = null, IDbTransaction tran = null)
        {
            return CommonAction<AutoServiceToDevice, bool>(t, conn, tran, (o, con, tr) =>
            {
                var returnData = con.Execute("delete from AutoServiceToDevice where AutoServiceID=@AutoServiceID", o, tr);

                return returnData > 0;

            });
        }
    }
}
