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

namespace GroupControl.DAL
{
    public class AutoServiceInfoDAL: BaseDAL<AutoServiceInfo, AutoServiceInfoViewModel>,IAutoServiceInfoDAL
    {

        public override IList<AutoServiceInfo> GetList(AutoServiceInfoViewModel viewModel)
        {
            return CommonAction<AutoServiceInfoViewModel, IList<AutoServiceInfo>>(viewModel, null, null, (o, con, tr) =>
            {
                viewModel.SQLStr = @"SELECT ID,[ServiceType]
                                              ,[StartDate]
                                              ,[IntervalDate]
                                              ,[IntervalDateType]
                                              ,[ExecutionTimes]
                                              ,[MapUrl]
                                              ,[ContentType]
                                              ,[Status]
                                              ,[SayHelloContent]
                                              ,[AddCount]
                                              ,Sex
                                              ,IsStatisticsFriendCount
                                              ,ComputerID
                                          FROM [AutoServiceInfo]";

                return con.QueryAsync<AutoServiceInfo>(PageHelper.CreateListSql(o), o.ParamDict, tr).Result.ToList();

            });
        }

    }
}
