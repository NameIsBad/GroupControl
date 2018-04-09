using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace GroupControl.Model
{
    public enum EnumTaskType
    {

        [Description("发朋友圈")]
        /// <summary>
        /// 发朋友圈
        /// </summary>
        SendFriendCircle = 1,

        [Description("通讯录添加好友")]
        /// <summary>
        /// 添加好友
        /// </summary>
        AddFriend = 2,


        [Description("搜索添加好友")]
        /// <summary>
        /// 搜索添加好友
        /// </summary>
        SearchAddFriend = 3,


        [Description("查询粉丝数")]
        /// <summary>
        /// 查询粉丝数
        /// </summary>
        QueryFansCount = 4,

        [Description("快手操作")]
        /// <summary>
        /// 查询粉丝数
        /// </summary>
        KSAction = 5,

        [Description("发送群消息")]
        /// <summary>
        /// 查询粉丝数
        /// </summary>
        SendGroupMessage = 6

    }
}
