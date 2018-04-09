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
    public class KSHelper
    {
        private BaseAction _baseAction = SingleHepler<BaseAction>.Instance;

        /// <summary>
        /// 打开快手
        /// </summary>
        /// <param name="device"></param>
        public void OpenKS(string device)
        {

           _baseAction.OpenApp(device, "com.smile.gifmaker", "com.smile.gifmaker/com.yxcorp.gifshow.HomeActivity", () =>
            {

                var stateStr = string.Empty;

                while (!stateStr.Contains("HomeActivity"))
                {

                    var list = _baseAction.InitProcessWithTaskState(device, " shell dumpsys window | grep mCurrentFocus", true);

                    if (null == list || list.Count == 0)
                    {
                        return;
                    }

                    stateStr = string.Join("|", list);

                    Thread.Sleep(200);
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
            _baseAction.InitProcessWithTaskState(device, string.Format(" shell am  start -n com.smile.gifmaker/com.yxcorp.gifshow.detail.PhotoDetailActivity -d {0} ",photoID));

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
    }
}
