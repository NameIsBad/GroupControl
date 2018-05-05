using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupControl.Model
{
    public class BaseViewModel
    {
        public int ID { get; set; }

        public int PageIndex { get; set; }

        public int PageCount { get; set; }

        public string OrderStr { get; set; }

        public bool IsDesc { get; set; }
        public string SQLStr { get; set; }

        public string WhereStr { get; set; }

        public string Device { get; set; }

        public int LeftWidth { get; set; }

        public int RightWidth { get; set; }

        public int TopHeight { get; set; }

        public int BottomHeight { get; set; }

        public string PullNativePath { get; set; }

        public string XMLName { get; set; }

        /// <summary>
        /// 查询条件
        /// </summary>
        public IDictionary<string, object> ParamDict { get; set; }
    }
}
