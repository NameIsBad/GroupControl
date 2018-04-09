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
    public class NewFriendStateDAL : BaseDAL<NewFriendState, NewFriendStateViewModel>, INewFriendStateDAL
    {
        public NewFriendStateDAL()
        {

        }

        /// <summary>
        /// 导入手机通讯录之后 
        /// 在微信通讯录里面不显示
        /// 此时要对数据进行回收 
        /// 此时这个获取的列表就是 导入通讯录成功 但是在微信通讯录上面不显示的 相关的设备列表
        /// </summary>
        /// <returns></returns>
        public IList<NewFriendState> GetListWithImportFailed()
        {
            return CommonAction<IList<NewFriendState>>(null, con =>
            {
                var sqlStr = @"select a.Device,a.ImportDate from 
  
                                  (
  
                                    select Device,ImportDate from  dbo.NewFriendState where IsHaveNewFriend=0 and IsRecovered=0
  
                                  ) as a
  
                                  inner join
  
                                  (
  
                                     SELECT  Device,MAX(CreateDate) as MaxAddDate,MIN(CreateDate)as MinAddDate
  
                                     FROM [AddFriendInfoRecord]  where Device is not null  group by Device having COUNT(Device)>=1 
  
                                  ) as b
  
                                  on
                                  a.Device=b.Device 

                                  and b.MaxAddDate < a.ImportDate 

                                  and  DATEDIFF(MONTH,a.ImportDate,GETDATE())<1 

                                 ";


                return con.Query<NewFriendState>(sqlStr, new { }, null).ToList();

            });


            // return returnData;
        }

        /// <summary>
        /// 批量更新设备回收数据状态
        /// </summary>
        public void BatchUpdateRecoveryState(IList<NewFriendState> viewModelList)
        {
            CommonAction<IList<NewFriendState>>(viewModelList, null, (t, con) =>
            {
                var sql = @"update NewFriendState  set RecoveryDate=GETDATE(),IsRecovered=1 where Device in @Devices";

                var devices = viewModelList.Select(o => o.Device).ToArray();

                var returnData=con.Execute(sql, new { Devices = devices });

            });
        }
    }
}