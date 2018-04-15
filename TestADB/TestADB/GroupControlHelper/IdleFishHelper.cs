using GroupControl.Common;
using GroupControl.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GroupControl.Helper
{

    /// <summary>
    ///闲鱼
    /// </summary>
    public class IdleFishHelper
    {
        private BaseAction baseAction = SingleHepler<BaseAction>.Instance;


        /// <summary>
        /// 打开闲鱼
        /// </summary>
        /// <param name="device"></param>
        public void OpenIdleFish(string device)
        {
            baseAction.OpenApp(device, "com.taobao.idlefish", "com.taobao.idlefish/com.taobao.fleamarket.home.activity.MainActivity", (action) =>
            {
                var stateStr = string.Empty;

                while (!stateStr.Contains("MainActivity"))
                {
                    action(stateStr);
                }

            });
        }

        /// <summary>
        /// 单个发布信息
        /// </summary>
        public void SinglePublishMessageInfo(string device)
        {
            //获取当前手机的分辨率
            var wmPoint = baseAction.GetWMSize(device);

            ///点击首页发布按钮
            baseAction.actionDirectStr(device, 360, 1205, wmPoint);

            ///循环检测是否进入发布页面
            baseAction.CircleDetection("PublishEntryActivity", device, false);

            ///点击发布闲置
            baseAction.actionDirectStr(device, 147, 924, wmPoint);

            ///循环检测是否进入选择图片页面
            baseAction.CircleDetection("MultiMediaStudioActivity", device, false);

            ///点击相册选择照片
            baseAction.actionDirectStr(device, 147, 924, wmPoint);



        }
    }
}
