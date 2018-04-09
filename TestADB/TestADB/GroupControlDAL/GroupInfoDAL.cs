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
    public class GroupInfoDAL : BaseDAL<GroupInfo, GroupInfoViewModel>, IGroupInfoDAL
    {
        public override IList<GroupInfoViewModel> GetListWithUnion(GroupInfoViewModel queryTermViewModel)
        {
            return CommonAction<GroupInfoViewModel, IList<GroupInfoViewModel>>(queryTermViewModel, null, null, (o, con, tr) =>
            {
                o.SQLStr = @"  select gr.*,gtd.Device from dbo.GroupInfo  as gr
  
                              left  join dbo.GroupToDevice as gtd
  
                              on gr.ID=gtd.GroupID ";

                return con.QueryAsync<GroupInfo, GroupToDevice, GroupInfoViewModel>(PageHelper.CreateListSql(o),

                    (gr, gtd) =>
                    {

                        var viewModel = new GroupInfoViewModel();

                        viewModel.GroupInfoModel = gr;

                        viewModel.GroupToDeviceModel = gtd;

                        return viewModel;

                    },

                    o.ParamDict, tr, true, "CreateDate").Result.ToList();



            });
        }


        /// <summary>
        ///获取当前任务涉及到的分组
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public IList<GroupInfoViewModel> GetUnionListWithTask(GroupInfoViewModel viewModel)
        {
            return CommonAction<GroupInfoViewModel, IList<GroupInfoViewModel>>(viewModel, null, null, (o, con, tr) =>
            {
                o.SQLStr = @" select * from ( select gi.ID,gi.Name,COUNT(Device) as DevicesCount from GroupInfo as gi
  
                                  inner join 
  
                                  (
                                    select GroupID,Device from GroupToDevice as gtd 
    
                                    where  exists ( select AutoServiceID,Device   from  AutoServiceToDevice  as astd  where AutoServiceID=@AutoServiceID and astd.Device=gtd.Device )
    
                                  ) as gtdd
  
                                  on gi.ID=gtdd.GroupID  group by ID,Name ) as _t ";

                return con.QueryAsync<GroupInfo, GroupInfoViewModel, GroupInfoViewModel>(PageHelper.CreateListSql(o),

                    (gr, gtd) =>
                    {
                        gtd.GroupInfoModel = gr;

                        return gtd;

                    },

                    o.ParamDict, tr, true, "DevicesCount").Result.ToList();



            });
        }
    }
}
