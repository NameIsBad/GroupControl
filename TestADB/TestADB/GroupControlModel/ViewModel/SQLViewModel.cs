using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupControl.Model
{
    public class SQLViewModel : BaseViewModel
    {
        /// <summary>
        /// 查询SQL
        /// </summary>
        public string SQLStr { get; set; }

        /// <summary>
        /// 计算总数量SQL
        /// </summary>
        public string CounSQLStr { get; set; }


        public Dictionary<string,object> Params { get; set; }
    }
}
