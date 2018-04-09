using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace GroupControl.Model
{



    /// <summary>
    /// 任务执行状态
    /// </summary>
    public enum EnumTaskStatus
    {

        [Description("准备中")]
        /// <summary>
        ///开始执行
        /// </summary>
        Start = 1,

        [Description("执行中")]
        /// <summary>
        /// 执行中
        /// </summary>
        Executing = 2,


        [Description("结束")]
        /// <summary>
        /// 结束
        /// </summary>
        End = 3


    }
}
