using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;

namespace GroupControl.Model
{
    public class VCardViewModel
    {

        /// <summary>
        /// 导入数量
        /// </summary>
        public int ImportCount { get; set; }

        public string Name { get; set; }

        public string Company { get; set; }

        public string Job { get; set; }


        public string WorkPhone { get; set; }

        public string MobilePhone { get; set; }

        public string Email { get; set; }

        public string WorkAddress { get; set; }

        /// <summary>
        /// 文件来源
        /// </summary>
        public string SourceFilePath { get; set; }

        public string Device { get; set; }
    }
}
