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

        public void OpenIdleFish(string device)
        {
            baseAction.OpenApp(device, "com.taobao.idlefish", "com.taobao.idlefish/com.taobao.fleamarket.message.activity.NewChatActivity", () =>
            {


            });
        }
    }
}
