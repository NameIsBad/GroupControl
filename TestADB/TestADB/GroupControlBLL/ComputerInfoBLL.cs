using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Dapper;
using DapperExtensions;

using GroupControl.IBLL;
using GroupControl.DAL;
using GroupControl.Model;
using GroupControl.Common;

namespace GroupControl.BLL
{
    public class ComputerInfoBLL : BaseBLL<ComputerInfo, ComputerInfoViewModel>, IComputerInfoBLL
    {

        public ComputerInfoBLL()
        {
            _dal = SingleHepler<ComputerInfoDAL>.Instance;
        }

        public override IList<ComputerInfo> GetList(ComputerInfoViewModel viewModel)
        {
            var whereStr = new StringBuilder("1=1");

            IDictionary<string, object> _dic = new Dictionary<string, object>();

            whereStr.Append(" AND MACAddress=@MACAddress ");

            _dic.Add("MACAddress", viewModel.MACAddress);

            ///检测当天的任务
            whereStr.Append(" AND CreateDate < [ExpireDate]");

            viewModel.ParamDict = _dic;

            viewModel.IsDesc = false;

            viewModel.WhereStr = whereStr.ToString();

            return _dal.GetList(viewModel);
        }
    }
}
