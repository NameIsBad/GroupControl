using GroupControl.Common;
using GroupControl.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace GroupControl.Helper
{
    public class KSHelper
    {
        private BaseAction _baseAction = SingleHepler<BaseAction>.Instance;

        /// <summary>
        /// 打开快手
        /// </summary>
        /// <param name="device"></param>
        public void OpenKS(string device)
        {

           _baseAction.OpenApp(device, "com.smile.gifmaker", "com.smile.gifmaker/com.yxcorp.gifshow.HomeActivity", (action) =>
            {

                var stateStr = string.Empty;

                while (!stateStr.Contains("HomeActivity"))
                {
                    stateStr=action(stateStr);
                }

            });

        }

        /// <summary>
        /// 根据详情页Id 进入详情页
        /// </summary>
        /// <param name="photoID"></param>
        public void EnterPhotoDetailByPhotoID(string photoID, string device)
        {
            //进入详情页  4307533760
            var returnData=_baseAction.InitProcessWithTaskState(device, string.Format(" shell am  start -n com.smile.gifmaker/com.yxcorp.gifshow.detail.PhotoDetailActivity -d {0} ",photoID));

            _baseAction.CircleDetection("PhotoDetailActivity", device);
        }

        /// <summary>
        /// 点赞
        /// </summary>
        /// <param name="device"></param>
        public void LikeIt(string device,WMPoint wmPoint)
        {
            _baseAction.actionDirectStr(device, 100,100, wmPoint);
        }

        /// <summary>
        /// 关注
        /// </summary>
        public void Follow(string device, WMPoint wmPoint)
        {
            _baseAction.actionDirectStr(device, 600, 100, wmPoint);
        }

        /// <summary>
        /// 评论
        /// </summary>
        /// <param name="device"></param>
        public void comment(string device, string content, WMPoint wmPoint)
        {
            if (string.IsNullOrEmpty(content))
            {
                return;
            }

            ///滑动到最下面
            _baseAction.InitProcessWithTaskState(device, "shell input swipe 100 1000 100  0");

            ///点击text 获取焦点
            _baseAction.actionDirectStr(device, 300, 1225, wmPoint);

            ///输入文字
            _baseAction.InitProcessWithTaskState(device, "shell am broadcast -a ADB_INPUT_TEXT --es msg '" + content + "'");

            // 点击确定发送评论
            _baseAction.actionDirectStr(device, 600, 1225, wmPoint);
        }

        /// <summary>
        /// 多个手机操作
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public IList<Task> BatchAction(KSViewModel viewModel)
        {
            return _baseAction.CommonAction<KSViewModel>(viewModel.Devices, (model) => {

                model.PhotoIDs = viewModel.PhotoIDs;
                model.Comments = viewModel.Comments;

                SingleAction(model);

            });
        }

        /// <summary>
        /// 单个设备的操作
        /// </summary>
        public void SingleAction(KSViewModel viewModel)
        {
            if (string.IsNullOrEmpty(viewModel.PhotoIDs))
            {
                return;
            }

            ///检测设备是否处于连接状态
            if (_baseAction.CheckEquipmentIsConnecting(viewModel.Device))
            {
                return;
            }

            _baseAction.UnlockSingleScreen(viewModel.Device);

            OpenKS(viewModel.Device);

            viewModel.PhotoIDList.ToList().ForEach(o =>
            {
                EnterPhotoDetailByPhotoID(o, viewModel.Device);

                var wmPoint = _baseAction.GetWMSize(viewModel.Device);

                LikeIt(viewModel.Device, wmPoint);

                Follow(viewModel.Device, wmPoint);

                var content = string.Empty;

                if (null != viewModel.CommentList && viewModel.CommentList.Count > 0)
                {
                    var index = new Random().Next(viewModel.CommentList.Count);

                    content = viewModel.CommentList[index];
                }

                comment(viewModel.Device, content, wmPoint);

                _baseAction.actionDirectStr(viewModel.Device, 50, 50, wmPoint);

            });
        }

        /// <summary>
        /// 打开单个直播  一个手机 同时只有一个直播
        /// </summary>
        public void SingleOpenLive(string photoID,string device)
        {
            _baseAction.UnlockSingleScreen(device);

            var wmPoint = _baseAction.GetWMSize(device);

            OpenKS(device);

            //点击左上角
            _baseAction.actionDirectStr(device, 50, 50, wmPoint);

            //点击查找
            _baseAction.actionDirectStr(device, 100, 641, wmPoint);

            //循环检测是否进入查找页面
            _baseAction.CircleDetection("SearchActivity", device);

            var viewModel = _baseAction.CreatUIXML(new KSViewModel() { Device=device}, (o, doc) =>
            {

                ///resource-id="com.smile.gifmaker:id/editor"
                XmlNode editorNode = doc.SelectSingleNode("//node[@resource-id='com.smile.gifmaker:id/editor']");

                var strValue = editorNode.Attributes["bounds"].Value;

                return _baseAction.GetStartBoundsWithEndBounds<KSViewModel>(o, strValue);

            });

            //切换输入法
            _baseAction.InstallKeyBoardApp(device, "com.android.adbkeyboard/.AdbIME");

            //获取文本框焦点
            _baseAction.actionDirectStr(device, (viewModel.LeftWidth + viewModel.RightWidth) / 2, (viewModel.TopHeight + viewModel.BottomHeight) / 2, wmPoint);

            ///输入文字
            _baseAction.InitProcessWithTaskState(viewModel.Device, "shell am broadcast -a ADB_INPUT_TEXT --es msg '" + photoID + "'");

            //还原输入法
            _baseAction.InstallKeyBoardApp(device, "com.sohu.inputmethod.sogou/.SogouIME");

            //获取文本框焦点
            _baseAction.actionDirectStr(device, (viewModel.LeftWidth + viewModel.RightWidth) / 2, (viewModel.TopHeight + viewModel.BottomHeight) / 2, wmPoint);

            //点击确定
            _baseAction.actionDirectStr(device, 655, 1210, wmPoint);

            Thread.Sleep(1000);

            //viewModel=_baseAction.CreatUIXML(new KSViewModel() { Device = device }, (o, doc) =>
            //{
            //    XmlNode editorNode = doc.SelectSingleNode("//node[@resource-id='com.smile.gifmaker:id/name']");

            //    var strValue = editorNode.Attributes["bounds"].Value;

            //    return _baseAction.GetStartBoundsWithEndBounds<KSViewModel>(o, strValue);

            //});

            //进入个人详情页
            //_baseAction.actionDirectStr(device, (viewModel.LeftWidth + viewModel.RightWidth) / 2, (viewModel.TopHeight + viewModel.BottomHeight) / 2, wmPoint);
            _baseAction.actionDirectStr(device,342, 421, wmPoint);
        }
    }
}
