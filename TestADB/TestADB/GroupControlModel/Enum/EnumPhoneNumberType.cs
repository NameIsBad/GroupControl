using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using System.ComponentModel;

namespace GroupControl.Model
{
    public enum EnumPhoneNumberType
    {
        /// <summary>
        /// 中国移动通信
        /// </summary>
        [Description("中国移动通信")]
        CMCC=1,

        /// <summary>
        /// 中国联通通讯
        /// </summary>
        [Description("中国联通通讯")]
        CUCC=2,

        /// <summary>
        /// 中国电信
        /// </summary>
        [Description("中国电信")]
        CTCC=3
    }
}
