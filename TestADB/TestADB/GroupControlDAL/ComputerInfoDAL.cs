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
    public class ComputerInfoDAL:BaseDAL<ComputerInfo,ComputerInfoViewModel>,IComputerInfoDAL
    {
        public override IList<ComputerInfo> GetList(ComputerInfoViewModel viewModel)
        {
            return CommonAction<ComputerInfoViewModel, IList<ComputerInfo>>(viewModel, null, null, (o, con, tr) =>
            {
                viewModel.SQLStr = @"SELECT  [MACAddress]

                                                     ,[ID]

                                                     ,[CreateDate]

                                                     ,[ExpireDate]

                                                  FROM [ComputerInfo]";

                return con.QueryAsync<ComputerInfo>(PageHelper.CreateListSql(o), o.ParamDict, tr).Result.ToList();

            });
        }
    }
}
