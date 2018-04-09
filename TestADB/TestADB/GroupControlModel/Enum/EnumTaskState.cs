using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupControl.Model
{

    /// <summary>
    /// 任务执行状态
    /// </summary>
    public enum EnumTaskState
    {
        /// <summary>
        /// 开始
        /// </summary>
        Start = 1,

        /// <summary>
        ///完成
        /// </summary>
        Complete =2,

        /// <summary>
        /// 任务正在取消中
        /// </summary>
        BeingCancelled=3,

        /// <summary>
        /// 取消
        /// </summary>
        Cancle =4,

        /// <summary>
        /// 错误
        /// </summary>
        Error=5


    }
}
