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
using System.Data.SqlClient;
using System.Data;

namespace GroupControl.DAL
{
   public class GroupToDeviceDAL : BaseDAL<GroupToDevice, DeviceToNickNameViewModel>, IGroupToDeviceDAL
    {
        public override IList<GroupToDevice> GetList(DeviceToNickNameViewModel viewModel)
        {
            return CommonAction<DeviceToNickNameViewModel,IList<GroupToDevice>>(viewModel, null,null, (o, con, tr) =>
            {
                var sqlStr = @"select * from dbo.GroupToDevice where  GroupID in (@GroupIDs)";


                return con.QueryAsync<GroupToDevice>(sqlStr, new { GroupIDs=viewModel.Devices.ToArray()},tr).Result.ToList();


            });
        }

        public override void BatchInsert(IList<GroupToDevice> t, IDbConnection conn = null, IDbTransaction tran = null)
        {
            CommonAction<IList<GroupToDevice>>(t, conn, tran, (model, con, tr) =>
            {

                var parameters = new List<IDataParameter> { new SqlParameter("@TVP_GroupToDevice", SqlDbType.Structured) };

                parameters[0].Value = DataUtil.ToDataTable<GroupToDevice>(model);

                SingleHepler<SQLAdaptor>.Instance.ExecuteNonQuery(tr, "Pro_BatchGroupToDevice", parameters, CommandType.StoredProcedure);


            });
        }

        /// <summary>
        /// 按照设备名称 批量删除  设备跟分组的对应关系
        /// </summary>
        /// <param name="devices"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        public void BatchDelete(IList<string> devices,IDbConnection conn=null,IDbTransaction tran=null)
        {
            CommonAction<IList<string>>(devices, conn, tran, (o, con, tr) =>
            {
                var sqlStr=@"delete from dbo.GroupToDevice where  Device in @Devices";

                con.Execute(sqlStr, new { Devices=o },tr);


            });

            
        }
    }
}
