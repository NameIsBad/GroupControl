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
    public class DeviceToNickNameDAL : BaseDAL<DeviceToNickName, DeviceToNickNameViewModel>, IDeviceToNickNameDAL
    {
        public override IList<DeviceToNickName> GetList(DeviceToNickNameViewModel viewModel)
        {
            return CommonAction<DeviceToNickNameViewModel, IList<DeviceToNickName>>(viewModel, null, null, (o, con, tr) =>
            {
                viewModel.SQLStr = @"SELECT    [ID]
                                              ,[Device]
                                              ,[NickName]
                                              ,[FriendCount]
                                              ,[CreateDate]
                                          FROM  [DeviceToNickName]";

                return con.QueryAsync<DeviceToNickName>(PageHelper.CreateListSql(o), o.ParamDict, tr).Result.ToList();

            });
        }

        /// <summary>
        /// 批量获取设备中的微信粉丝数量
        /// </summary>
        /// <param name="list"></param>
        /// <param name="connn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public IList<DeviceToNickName> GeDeviceListWithUsingEquipments(IList<DeviceToNickName> list)
        {
            var returnData = new List<DeviceToNickName>();

            var parameters = new List<IDataParameter> { new SqlParameter("@TVP_DeviceToNickName", SqlDbType.Structured) };

            parameters[0].Value = DataUtil.ToDataTable<DeviceToNickName>(list);

            using (var reader = SingleHepler<SQLAdaptor>.Instance.ExecuteReader("Pro_BatchDeviceToNickName", parameters, CommandType.StoredProcedure))
            {

                while (reader.Read())
                {
                    var currentModel = new DeviceToNickName();

                    currentModel.ID = Convert.ToInt32(reader["ID"]);

                    currentModel.Device = Convert.IsDBNull(reader["Device"]) ? string.Empty : Convert.ToString(reader["Device"]);

                    currentModel.NickName = Convert.IsDBNull(reader["NickName"]) ? string.Empty : Convert.ToString(reader["NickName"]);

                    currentModel.FriendCount = Convert.ToInt32(reader["FriendCount"]);

                    currentModel.CreateDate = Convert.ToDateTime(reader["CreateDate"]);

                    returnData.Add(currentModel);

                }

                reader.Close();
            }

            return returnData;
        }

        public override IList<DeviceToNickNameViewModel> GetListWithUnion(DeviceToNickNameViewModel queryTermViewModel)
        {
            var returnData = new Lazy<List<DeviceToNickNameViewModel>>();

            var parameters = new List<IDataParameter> { new SqlParameter("@TVP_ContractsInfo", SqlDbType.Structured) };

            parameters[0].Value = DataUtil.ToDataTable<ContractsInfo>(queryTermViewModel.ContractsInfoList);

            using (var reader = SingleHepler<SQLAdaptor>.Instance.ExecuteReader("Pro_Batch_GET_DeviceToNickName_ContractInfoDetail_ContractInfoIDs", parameters, CommandType.StoredProcedure))
            {

                while (reader.Read())
                {
                    var currentModel = new DeviceToNickNameViewModel();

                    currentModel.ContractInfoID = Convert.ToInt32(reader["ContractsInfoID"]);

                    currentModel.Device = Convert.IsDBNull(reader["Device"]) ? string.Empty : Convert.ToString(reader["Device"]);

                    currentModel.NickName = Convert.IsDBNull(reader["NickName"]) ? string.Empty : Convert.ToString(reader["NickName"]);

                    currentModel.ImportContentCount = Convert.ToInt32(reader["ImportContentCount"]);

                    returnData.Value.Add(currentModel);

                }

                reader.Close();
            }

            return returnData.Value;

        }
    }
}
