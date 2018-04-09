using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupControl.Model
{
    public class AutoServiceInfo
    {
        public int ID { get; set; }

        /// <summary>
        /// 服务类型 发朋友圈 添加朋友
        /// </summary>
        public EnumTaskType ServiceType { get; set; }

        /// <summary>
        /// 服务开启时间
        /// </summary>

        public DateTime StartDate { get; set; }

        /// <summary>
        /// 间隔时间
        /// </summary>
        public int IntervalDate { get; set; }


        /// <summary>
        /// 间隔时间类型（小时 分钟 秒等）
        /// </summary>
        public int IntervalDateType { get; set; }

        /// <summary>
        /// 执行次数
        /// </summary>
        public int ExecutionTimes { get; set; }

        /// <summary>
        /// 映射地址 （发送朋友圈的素材 对应本地的文件夹）
        /// </summary>
        public string MapUrl { get; set; }

        /// <summary>
        /// 发布内容类型
        /// </summary>
        public EnumPublishContentType ContentType { get; set; }


        /// <summary>
        /// 任务状态
        /// </summary>
        public EnumTaskStatus Status { get; set; }

        public string RemarkContent { get; set; }

        public string SayHelloContent { get; set; }

       
        /// <summary>
        /// 操作类型 手动  自动
        /// </summary>
        public EnumSendType SendType { get; set; }


        /// <summary>
        /// 添加好友个数
        /// </summary>
        public int AddCount { get; set; }

        public EnumCheckSexType Sex { get; set; }

        /// <summary>
        /// 是否统计粉丝数
        /// </summary>
        public bool IsStatisticsFriendCount { get; set; }

        /// <summary>
        /// 电脑唯一标识
        /// </summary>
        public int ComputerID { get; set; }
    }
}
