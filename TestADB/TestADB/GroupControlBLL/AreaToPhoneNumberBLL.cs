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
    public class AreaToPhoneNumberBLL : BaseBLL<AreaToPhoneNumber, AreaToPhoneNumberViewModel>, IAreaToPhoneNumberBLL
    {
        public AreaToPhoneNumberBLL()
        {
            _dal = SingleHepler<AreaToPhoneNumberDAL>.Instance;
        }

        public override IList<AreaToPhoneNumber> GetList(AreaToPhoneNumberViewModel viewModel)
        {
            var whereStr = new StringBuilder("1=1");

            IDictionary<string, object> _dic = new Dictionary<string, object>();

            if (!string.IsNullOrEmpty(viewModel.AreaCode))
            {
                whereStr.Append(" AND AreaCode=@AreaCode ");

                _dic.Add("AreaCode", viewModel.AreaCode);
            }

            if (!string.IsNullOrEmpty(viewModel.Header))
            {
                whereStr.Append(" AND NumberSection like @NumberSection ");

                _dic.Add("NumberSection", ""+ viewModel.Header + "%");
            }

            if (viewModel.Type>0)
            {
                whereStr.Append(" AND Type=@Type ");

                _dic.Add("Type", viewModel.Type);
            }

            if (!string.IsNullOrEmpty(viewModel.LinkUrl))
            {
                whereStr.Append(" AND LinkUrl=@LinkUrl ");

                _dic.Add("LinkUrl", viewModel.LinkUrl);
            }

            viewModel.ParamDict = _dic;

            viewModel.IsDesc = true;

            viewModel.WhereStr = whereStr.ToString();

            return _dal.GetList(viewModel);
        }
    }
}
