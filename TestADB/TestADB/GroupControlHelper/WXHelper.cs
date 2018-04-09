using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.IO;
using System.Drawing;
using System.Security.AccessControl;
using System.Data;
using System.Xml;

using GroupControl.BLL;
using GroupControl.Model;
using GroupControl.Common;


namespace GroupControl.Helper
{
    public class WXHelper
    {
        private BaseAction _baseAction = SingleHepler<BaseAction>.Instance;

        private IList<string> upLoadPathList;

        private EnumPublishContentType publishContentType;

        private string _content;

        private string _rootPath = string.Empty;

        public string Content
        {
            get
            {
                return _content;
            }

            set
            {
                _content = value;
            }
        }

        public IList<string> UpLoadPathList
        {
            get
            {
                return upLoadPathList;
            }

            set
            {
                upLoadPathList = value;
            }
        }

        public EnumPublishContentType PublishContentType
        {
            get
            {
                return publishContentType;
            }

            set
            {
                publishContentType = value;
            }
        }


        #region 保存文件到本地

        /// <summary>
        /// 单个手机发送朋友圈
        /// </summary>
        /// <param name="relativePath"></param>
        /// <param name="currentSelectContentType"></param>
        public void SaveFileToNative(string relativePath, EnumPublishContentType currentSelectContentType)
        {

            try
            {
                var path = string.Format(@"{0}\{1}", _rootPath, relativePath);

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                switch (currentSelectContentType)
                {
                    case EnumPublishContentType.PictureAndWord:

                        Parallel.Invoke(() =>
                        {
                            SaveImages(path);

                        }, () =>
                        {

                            SaveContent(path);

                        });


                        break;

                    case EnumPublishContentType.VedioAndWord:

                        Parallel.Invoke(() =>
                        {
                            SaveImages(path);

                        }, () =>
                        {

                            SaveContent(path);

                        });


                        break;

                    case EnumPublishContentType.LinkAndWord:

                        SaveContent(path);

                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void GetNativeRootPath(EnumSendType currentSendType)
        {
            switch (currentSendType)
            {
                case EnumSendType.AutoSend:

                    _rootPath = SingleHepler<ConfigInfo>.Instance.AutoSendFriendFileUrl;

                    break;
                case EnumSendType.HandSend:

                    _rootPath = SingleHepler<ConfigInfo>.Instance.HandSendFriendFileUrl;

                    break;
                default:
                    break;
            }
        }

        /// <summary>
        ///保存文字内容
        /// </summary>
        /// <param name="path"></param>
        public void SaveContent(string path)
        {

            if (string.IsNullOrEmpty(_content))
            {
                return;
            }

            var contentFullPath = Path.Combine(path, "content.txt");

            try
            {
                using (var fileStream = new FileStream(contentFullPath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
                {
                    using (var streamWriter = new StreamWriter(fileStream, Encoding.UTF8))
                    {
                        streamWriter.Write(_content);

                        streamWriter.Close();
                    }

                    fileStream.Close();

                }
            }
            catch (Exception ex)
            {

            }

        }

        public void SaveImages(string path)
        {

            if (null != upLoadPathList && upLoadPathList.Count > 0)
            {
                var index = 1;

                upLoadPathList.ToList().ForEach((item) =>
                {

                    try
                    {
                        if (File.Exists(item))
                        {
                            using (var fileStream = new FileStream(item, FileMode.Open, FileAccess.Read))
                            {

                                var currentByte = new byte[fileStream.Length];

                                fileStream.Read(currentByte, 0, currentByte.Length);

                                var extension = Path.GetExtension(item).ToLower();

                                if (publishContentType == EnumPublishContentType.PictureAndWord)
                                {
                                    extension = ".png";
                                }

                                var fileName = string.Format("{0}{1}{2}", DateTime.Now.ToString("yyyyMMddHHmmss") + DateTime.Now.Millisecond, index, extension);

                                var fullName = Path.Combine(path, fileName);

                                File.WriteAllBytes(fullName, currentByte);

                                fileStream.Close();

                            }
                        }

                        index++;
                    }
                    catch (Exception ex)
                    {

                    }

                });
            }
        }

        #endregion

        #region 发朋友圈

        /// <summary>
        /// 单个发送朋友圈
        /// </summary>
        /// <param name="device"></param>-
        public void SendFriendCircle(UploadFilesToPhoneViewModel viewModel)
        {
            ///检测设备是否处于连接状态
            if (_baseAction.CheckEquipmentIsConnecting(viewModel.Device))
            {
                return;
            }

            var isSuccess = false;

            _baseAction.UnlockSingleScreen(viewModel.Device);

            OpenWX(viewModel.Device);

            ///获取屏幕分辨率
            WMPoint wmPoint = _baseAction.GetWMSize(viewModel.Device);

            ////我
            _baseAction.actionDirectStr(viewModel.Device, 700, 1180, wmPoint);

            //////相册
            _baseAction.actionDirectStr(viewModel.Device, 450, 650, wmPoint);

            //检测是否进入我的相册
            isSuccess = _baseAction.CircleDetection("SnsUserUI", viewModel.Device);

            if (!isSuccess)
            {
                return;
            }

            /////今天
            _baseAction.actionDirectStr(viewModel.Device, 200, 910, wmPoint);

            /////从相册中选择
            _baseAction.actionDirectStr(viewModel.Device, 360, 700, wmPoint);

            ///是否进入相册列表
            isSuccess = _baseAction.CircleDetection("AlbumPreviewUI", viewModel.Device);


            if (!isSuccess)
            {
                //滑动一下 去除通知
                _baseAction.InitProcessWithTaskState(viewModel.Device, " shell input swipe 100 100 600  100");

                ///去除意外弹框
                _baseAction.actionDirectStr(viewModel.Device, 200, 100, wmPoint);

                /////今天
                _baseAction.actionDirectStr(viewModel.Device, 200, 900, wmPoint);

                /////从相册中选择
                _baseAction.actionDirectStr(viewModel.Device, 360, 700, wmPoint);

                isSuccess = _baseAction.CircleDetection("AlbumPreviewUI", viewModel.Device);

                if (!isSuccess)
                {
                    return;
                }
            }

            if (null != viewModel.Files && viewModel.Files.Count() > 0)
            {
                WMPoint wmPointModel = _baseAction.ConvertPointWithDiffentWM(viewModel.Device, 120, 200, wmPoint);

                var index = 1;

                viewModel.Files.ToList().ForEach((file) =>
                {

                    if (file.Extension.Equals(".txt"))
                    {
                        return;
                    }

                    var direction = _baseAction.func(wmPointModel, "shell input tap");

                    ///图片中选择
                    _baseAction.InitProcessWithTaskState(viewModel.Device, direction);

                    if (index % 4 == 0)
                    {
                        wmPointModel.WM_X = 120;

                        wmPointModel.WM_Y += 180;
                    }
                    else
                    {
                        wmPointModel.WM_X += 180;
                    }

                    index++;

                });
            }

            ///发送的是视频
            if (viewModel.PublishContentType == EnumPublishContentType.VedioAndWord)
            {
                //检测是否进入视频预览界面 
                isSuccess = _baseAction.CircleDetection("ImagePreviewUI", viewModel.Device);

                if (!isSuccess)
                {
                    return;
                }
            }

            ///向上滑动 防止出现通知 
            _baseAction.InitProcessWithTaskState(viewModel.Device, " shell input swipe 100 100 600  100");

            ////完成
            _baseAction.actionDirectStr(viewModel.Device, 700, 100, wmPoint);

            Thread.Sleep(1000);

            var list = _baseAction.InitProcessWithTaskState(viewModel.Device, " shell dumpsys window | grep mCurrentFocus", true);

            var stateStr = list.FirstOrDefault();

            //检测是否进入确定发送朋友圈页面
            while (stateStr.Contains("AlbumPreviewUI") || stateStr.Contains("ImagePreviewUI") || stateStr.Contains("VideoCompressUI"))
            {

                list = _baseAction.InitProcessWithTaskState(viewModel.Device, " shell dumpsys window | grep mCurrentFocus", true);

                stateStr = list.FirstOrDefault();

            }

            ///点击text 获取焦点
            _baseAction.actionDirectStr(viewModel.Device, 100, 200, wmPoint);

            ///输入文字
            _baseAction.InitProcessWithTaskState(viewModel.Device, "shell am broadcast -a ADB_INPUT_TEXT --es msg '" + viewModel.Content + "'");


            ///向上滑动 防止出现通知 
            _baseAction.InitProcessWithTaskState(viewModel.Device, " shell input swipe 100 100 600  100");

            // 点击确定发送朋友圈按钮
            _baseAction.actionDirectStr(viewModel.Device, 700, 100, wmPoint);

            //Thread.Sleep(1000);

            //list = _baseAction.InitProcessWithTaskState(viewModel.Device, " shell dumpsys window | grep mCurrentFocus", true);

            //stateStr = list.FirstOrDefault();

            /////是否进入朋友圈界面

            //while (stateStr.Contains("SnsUploadUI"))
            //{
            //    ///向上滑动 防止出现通知 
            //    _baseAction.InitProcessWithTaskState(viewModel.Device, " shell input swipe 100 100 600  100");

            //    ///点击确定发送朋友圈按钮
            //    _baseAction.InitProcessWithTaskState(viewModel.Device, "shell input tap 700 100");

            //    list = _baseAction.InitProcessWithTaskState(viewModel.Device, " shell dumpsys window | grep mCurrentFocus", true);

            //    stateStr = list.FirstOrDefault();

            //    Thread.Sleep(500);

            //}


        }

        /// <summary>
        /// 群发朋友圈
        /// </summary>
        public IList<Task> BatchSendFriendCircle(AutoServiceInfoViewModel viewModel)
        {

            return _baseAction.CommonAction<UploadFilesToPhoneViewModel>(viewModel.Devices, (item) =>
             {
                 var currentViewModel = UploadToPhine(

                     new UploadFilesToPhoneViewModel()
                     {
                         Device = item.Device,

                         Path = viewModel.Path,

                         PublishContentType = viewModel.PublishContentType
                     });

                 SendFriendCircle(currentViewModel);

             });
        }
        #endregion

        #region 批量添加好友

        /// <summary>
        /// 批量添加好友
        /// </summary>
        /// <returns></returns>
        public IList<Task> BatchAddFriend(AutoServiceInfoViewModel viewModel)
        {
            return _baseAction.CommonAction<WXActionViewModel>(viewModel.Devices, (currentData) =>
            {

                currentData.IsStatisticsFriendCount = viewModel.AutoServiceInfoModel.IsStatisticsFriendCount;

                currentData.SayHiContent = viewModel.AutoServiceInfoModel.SayHelloContent;

                currentData.RemarkContent = viewModel.AutoServiceInfoModel.RemarkContent;

                currentData.AddCount = viewModel.AutoServiceInfoModel.AddCount;

                currentData.Sex = viewModel.AutoServiceInfoModel.Sex;

                SingleAddFriend(currentData);

            });
        }

        /// <summary>
        /// 在搜索中 批量添加好友
        /// </summary>
        /// <returns></returns>
        public IList<Task> BatchAddFriendWithSearch(AutoServiceInfoViewModel viewModel)
        {
            var currentDevices = _baseAction.Devices;

            var searchContent = viewModel.SearchContents;

            var index = 0;

            var rounding = 0;

            var remainder = 0;

            var wxActionViewModelList = new List<WXActionViewModel>();

            var taskArray = new List<Task>();

            if (null == searchContent || searchContent.Count == 0)
            {
                return new List<Task>();
            }

            if (null != viewModel.Devices && viewModel.Devices.Count > 0)
            {
                currentDevices = viewModel.Devices;
            }

            ///取整
            rounding = searchContent.Count / currentDevices.Count;

            //取余
            remainder = searchContent.Count % currentDevices.Count;


            Func<string, WXActionViewModel> currentFuncWithCreateModel = (device) =>
            {
                var currentDevice = device;

                return new WXActionViewModel()
                {
                    Device = currentDevice,

                    IsStatisticsFriendCount = viewModel.AutoServiceInfoModel.IsStatisticsFriendCount,

                    SayHiContent = viewModel.AutoServiceInfoModel.SayHelloContent,

                    RemarkContent = viewModel.AutoServiceInfoModel.RemarkContent,

                    AddCount = viewModel.AutoServiceInfoModel.AddCount,

                    Sex = viewModel.AutoServiceInfoModel.Sex,

                    IsSearch = true
                };
            };

            Func<int, int, IList<string>, int> currentFuncWithForeach = (startIndex, step, list) =>
               {
                   foreach (var currentDevice in list)
                   {

                       var currentModel = currentFuncWithCreateModel(currentDevice);

                       currentModel.SearchContents = searchContent.Skip(startIndex).Take(step).ToList();

                       wxActionViewModelList.Add(currentModel);

                       startIndex += step;
                   }

                   return startIndex;

               };

            if (rounding > 0)
            {
                index = currentFuncWithForeach(index, rounding, currentDevices);
            }

            if (remainder > 0 && rounding > 0)
            {
                var currentModelList = wxActionViewModelList.Take(remainder).ToList();

                foreach (var wxActionViewModel in currentModelList)
                {
                    wxActionViewModel.SearchContents.Add(searchContent.Skip(index).Take(1).FirstOrDefault());

                    index++;
                }
            }

            else if (remainder > 0 && rounding == 0)
            {
                var currentDeviceList = currentDevices.Take(remainder).ToList();

                currentFuncWithForeach(0, 1, currentDeviceList);
            }

            _baseAction.InitEquipmentWithTaskState((dic) =>
            {

                var currentDic = dic;

                foreach (var wxActionViewModel in wxActionViewModelList)
                {

                    var task = _baseAction.MonitorTask((currentViewModel) =>
                    {

                        var model = currentViewModel as WXActionViewModel;

                        SingleAddFriend(model);

                    }, wxActionViewModel);

                    currentDic.Add(wxActionViewModel.Device, EnumTaskState.Start);

                    taskArray.Add(task);
                }


                return currentDic;

            });

            return taskArray;
        }


        /// <summary>
        /// 单个加好友
        /// </summary>
        public void SingleAddFriend(WXActionViewModel viewModel)
        {
            //获取当前手机的分辨率
            var wmPoint = _baseAction.GetWMSize(viewModel.Device);

            EnterNewFriendPageAction(viewModel, wmPoint);

            ///搜索
            if (viewModel.IsSearch)
            {
                AddFriendWithSearch(viewModel, wmPoint);
            }
            else
            {
                AddFriendWithContracts(viewModel, wmPoint);
            }

        }

        /// <summary>
        /// 通过搜索加好友
        /// </summary>
        /// <param name="viewModel"></param>
        public void AddFriendWithSearch(WXActionViewModel viewModel, WMPoint wmPoint)
        {
            var searchContentList = viewModel.SearchContents;

            if (null == searchContentList || searchContentList.Count == 0)
            {
                return;
            }

            ///点击添加朋友
            _baseAction.actionDirectStr(viewModel.Device, 700, 100, wmPoint);

            ///是否进入添加朋友界面
            var isSuccess = _baseAction.CircleDetection("AddMoreFriendsUI", viewModel.Device);

            if (!isSuccess)
            {
                return;
            }

            //点击搜索框
            _baseAction.actionDirectStr(viewModel.Device, 360, 250, wmPoint);

            ///是否进入搜索界面
            isSuccess = _baseAction.CircleDetection("FTSAddFriendUI", viewModel.Device);

            if (!isSuccess)
            {
                return;
            }

            foreach (var searchContent in searchContentList)
            {
                viewModel.IsGoOn = true;

                viewModel.XMLName = "DetailUI";

                ///检测设备是否处于连接状态
                if (_baseAction.CheckEquipmentIsConnecting(viewModel.Device))
                {
                    break;
                }

                ///清空输入框
                _baseAction.actionDirectStr(viewModel.Device, 700, 100, wmPoint);

                ///输入搜索内容
                _baseAction.InitProcessWithTaskState(viewModel.Device, "shell am broadcast -a ADB_INPUT_TEXT --es msg '" + searchContent + "'");

                ///点击搜索按钮  
                _baseAction.actionDirectStr(viewModel.Device, 360, 200, wmPoint);

                //检测是否进入详细资料
                isSuccess = _baseAction.CircleDetection("ContactInfoUI", viewModel.Device);

                if (!isSuccess)
                {
                    continue;
                }

                viewModel = _baseAction.CreatUIXML(viewModel, (o, doc) => { return BeforeAddFriendWithValidate(doc, o); });

                FromDetailPageToValidatePage(viewModel, wmPoint: wmPoint);

                //返回
                _baseAction.actionDirectStr(viewModel.Device, 50, 100, wmPoint);


            }
        }

        /// <summary>
        /// 通过联系人加好友
        /// </summary>
        /// <param name="viewModel"></param>
        public void AddFriendWithContracts(WXActionViewModel viewModel, WMPoint wmPoint)
        {
            var y = 443;

            var i = 1;

            var isValidate = false;

            var stateStr = string.Empty;

            var isSuccess = false;

            viewModel.XMLName = "deleteAddedFriend";

            while (i <= viewModel.AddCount)
            {

                ///检测设备是否处于连接状态
                if (_baseAction.CheckEquipmentIsConnecting(viewModel.Device))
                {
                    return;
                }

                stateStr = string.Empty;

                isSuccess = false;

                var returnModel = IsHaveNewFriend(viewModel);

                if (!returnModel.IsValidatedState)
                {
                    return;
                }


                returnModel = DeleteFriend(viewModel);

                if (!returnModel.IsValidatedState)
                {
                    ///长按当前新朋友
                    _baseAction.actionDirectSwipStr(viewModel.Device, 100, y, 100, y, wmPoint);

                    ///删除当前新朋友
                    _baseAction.actionDirectStr(viewModel.Device, 360, 640, wmPoint);
                }

                else
                {
                    #region 加好友

                    _baseAction.actionDirectStr(viewModel.Device, 100, y, wmPoint);

                    //检测是否进入详细资料
                    isSuccess = _baseAction.CircleDetection("ContactInfoUI", viewModel.Device);

                    if (!isSuccess)
                    {
                        return;
                    }

                    viewModel.IsGoOn = true;

                    viewModel.XMLName = "DetailUI";

                    viewModel = _baseAction.CreatUIXML(viewModel, (o, doc) => { return BeforeAddFriendWithValidate(doc, o); });

                    ///页面过渡的时候检测
                    FromDetailPageToValidatePage(viewModel, (o) =>
                    {

                        if (o)
                        {
                            i++;
                        }

                    }, wmPoint);

                    //点击去除不必要的弹框
                    // _baseAction.InitProcessWithTaskState(viewModel.Device, "shell input tap 360 100");

                    //返回到新朋友
                    _baseAction.actionDirectStr(viewModel.Device, 50, 100, wmPoint);

                    //检测是否进入新朋友界面
                    isSuccess = _baseAction.CircleDetection("FMessageConversationUI", viewModel.Device);

                    if (isSuccess)
                    {

                        ///长按当前新朋友
                        _baseAction.actionDirectSwipStr(viewModel.Device, 100, y, 100, y, wmPoint);

                        ///删除当前新朋友
                        _baseAction.actionDirectStr(viewModel.Device, 360, 640, wmPoint);

                    }


                    #endregion
                }

            }
        }

        public void FromDetailPageToValidatePage(WXActionViewModel viewModel, Action<bool> currentAction = null, WMPoint wmPoint = null)
        {
            var isSuccess = true;

            ///可以执行下去
            if (!viewModel.IsGoOn)
            {
                return;
            }
            ///点击添加到通讯录按钮 或者 验证通过按钮
            _baseAction.InitProcessWithTaskState(viewModel.Device, string.Format("shell input tap {0} {1}", (viewModel.LeftWidth + viewModel.RightWidth) / 2, (viewModel.TopHeight + viewModel.BottomHeight) / 2));

            ///检测是直接变成好友 还是进入验证页面
            var isContinueing = true;

            while (isContinueing)
            {
                var list = _baseAction.InitProcessWithTaskState(viewModel.Device, " shell dumpsys window | grep mCurrentFocus", true);

                if (null == list || list.Count == 0)
                {
                    isSuccess = false;

                    isContinueing = false;
                }

                ///检测是否进入验证页面
                if (string.Join("|", list).Contains("SayHiWithSnsPermissionUI"))
                {
                    isSuccess = true;

                    isContinueing = false;
                }

                ///检测是否直接变成 发送消息 
                else
                {
                    var returnData = _baseAction.CreatUIXML(viewModel, (o, doc) => { return CheckIsTurnDirectlyNewFriend(doc); });

                    ///表示 就是直接变成发消息  不用验证
                    if (returnData.IsValidatedState)
                    {
                        isSuccess = false;

                        isContinueing = false;
                    }
                }

            }

            if (isSuccess)
            {
                ///进入的是打招呼界面
                isSuccess = EnterSayHelloActivity(viewModel, wmPoint);

                if (null != currentAction)
                {
                    currentAction.Invoke(isSuccess);
                }



            }
        }


        #region 查看粉丝数


        /// <summary>
        /// 批量查询粉丝数
        /// </summary>
        /// <returns></returns>
        public IList<Task> BatchQueryFansCount(AutoServiceInfoViewModel viewModel)
        {
            return _baseAction.CommonAction<WXActionViewModel>(viewModel.Devices, (item) =>
            {
                var wmPoint = _baseAction.GetWMSize(item.Device);

                StatisticsFriendCount(item, wmPoint);

            });
        }


        /// <summary>
        /// 统计粉丝数
        /// </summary>
        /// <param name="viewModel"></param>
        public void StatisticsFriendCount(WXActionViewModel viewModel, WMPoint wmPoint)
        {

            _baseAction.UnlockSingleScreen(viewModel.Device);

            OpenWX(viewModel.Device);

            ////通讯录
            _baseAction.actionDirectStr(viewModel.Device, 300, 1180, wmPoint);

            ///点击 #
            var y = 1060;

            if (!string.IsNullOrEmpty(viewModel.Device) && viewModel.Device.Length <= 8)
            {
                y += _baseAction.DValue;
            }

            _baseAction.actionDirectStr(viewModel.Device, 700, y, wmPoint);

            //viewModel.PullNativePath = string.Format(@"{0}\{1}", SingleHepler<ConfigInfo>.Instance.CutImageFileUrl, viewModel.Device);

            viewModel.XMLName = "recordUI";

            while (string.IsNullOrEmpty(viewModel.FriendCountStr))
            {
                ///滑动进入到页面最低端
                _baseAction.InitProcessWithTaskState(viewModel.Device, "shell input swipe 100 1000 100  -10000");

                ///获取通讯录页面的XML文档
                viewModel = _baseAction.CreatUIXML(viewModel, (o, doc) =>
                {

                    XmlNode callOnNode1 = doc.SelectSingleNode("//node[contains(@bounds,'[0,1060]')]");
                    XmlNode callOnNode2 = doc.SelectSingleNode("//node[contains(@text,'位联系人')]");

                    if (callOnNode1 != null)
                    {
                        callOnNode1 = callOnNode1.LastChild;
                        o.FriendCountStr = GetFansCountByStr(callOnNode1.InnerXml);
                    }
                    else if (null != callOnNode2)
                    {
                        var value = callOnNode2.Attributes["text"].Value;

                        if (!string.IsNullOrEmpty(value))
                        {
                            o.FriendCountStr = Regex.Match(value, @"(\d+)").Value;
                        }
                    }

                    return o;

                });

                Thread.Sleep(500);
            }

            Task.Factory.StartNew(() =>
            {

                SingleHepler<DeviceToNickNameBLL>.Instance.UpDateFriendCount(new DeviceToNickNameViewModel() { Device = viewModel.Device, FriendCount = int.Parse(viewModel.FriendCountStr) });

            });
        }

        #endregion

        /// <summary>
        /// 添加好友添加纪录
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="viewModel"></param>
        public void AddFriendInfoRecord(WXActionViewModel viewModel)
        {
            Task.Factory.StartNew(() =>
            {

                //XmlNode nickNode = doc.SelectSingleNode("//node[contains(@bounds,'[204,214]') and contains(@resource-id,'com.tencent.mm:id')]");

                var nick = string.Empty;

                SingleHepler<AddFriendInfoRecordBLL>.Instance.Insert(new AddFriendInfoRecord()
                {

                    CreateDate = DateTime.Now,

                    DataSource = viewModel.RemarkContent,

                    Nick = nick,

                    Sex = Convert.ToInt32(viewModel.Sex),

                    Device = viewModel.Device
                });

            });
        }



        //进入新朋友页面
        public void EnterNewFriendPageAction(WXActionViewModel viewModel, WMPoint wmPoint = null)
        {

            _baseAction.UnlockSingleScreen(viewModel.Device);

            OpenWX(viewModel.Device, wmPoint);

            //viewModel.PullNativePath = string.Format(@"{0}\{1}", SingleHepler<ConfigInfo>.Instance.CutImageFileUrl, viewModel.Device);

            ////通讯录 
            _baseAction.actionDirectStr(viewModel.Device, 300, 1180, wmPoint);

            Thread.Sleep(2000);

            ////新朋友
            _baseAction.actionDirectStr(viewModel.Device, 100, 230, wmPoint);

            //检测是否进入新的朋友界面
            var isSuccess = _baseAction.CircleDetection("FMessageConversationUI", viewModel.Device);

            if (!isSuccess)
            {
                return;
            }
        }



        /// <summary>
        /// 点击添加到通讯录的时候  直接变成发送消息  而不是到验证页面
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public WXActionViewModel CheckIsTurnDirectlyNewFriend(XmlDocument doc)
        {
            XmlNodeList nodeList = doc.SelectNodes("//node[@class='android.widget.Button']");

            Lazy<WXActionViewModel> _lazy = new Lazy<WXActionViewModel>();

            if (null != nodeList && nodeList.Count > 0)
            {
                var currenNode = nodeList[0];

                var textValue = currenNode.Attributes["text"].Value;

                ///回复
                if (textValue.Length == 3)
                {
                    _lazy.Value.IsValidatedState = true;

                }
            }

            return _lazy.Value;
        }

        /// <summary>
        /// 加好友之前 进行检测
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public WXActionViewModel BeforeAddFriendWithValidate(XmlDocument doc, WXActionViewModel viewModel)
        {
            #region 进行检测

            var sex = EnumCheckSexType.Other;

            XmlNodeList nodeList = doc.SelectNodes("//node[@class='android.widget.Button']");

            XmlNode clickBtn = null;

            if (null != nodeList && nodeList.Count > 0)
            {
                var currenNode = nodeList[0];

                var textValue = currenNode.Attributes["text"].Value;

                ///回复
                if (textValue.Length == 2)
                {
                    var secondNode = nodeList[1];

                    textValue = secondNode.Attributes["text"].Value;

                    ///通过验证
                    if (textValue.Length == 4)
                    {
                        clickBtn = secondNode;
                    }

                }
                ///通过验证  添加到通讯录
                else if (textValue.Length == 4 || textValue.Length == 6)
                {
                    clickBtn = currenNode;
                }
            }

            XmlNode sexNode = doc.FirstChild;

            if (viewModel.Sex != EnumCheckSexType.Other)
            {
                // resource-id="com.tencent.mm:id/ad_" class="android.widget.ImageView"
                var personInfoNode = doc.SelectSingleNode("//node[contains(@bounds,'[204,214]')]");

                if (null != personInfoNode)
                {
                    sexNode = personInfoNode.LastChild;

                    var text = sexNode.Attributes["content-desc"].Value;

                    var bounds = sexNode.Attributes["bounds"].Value;

                    ///表示不是乱码
                    if (!text.Equals("?"))
                    {
                        sex = text.Equals("男") ? EnumCheckSexType.Man : (text.Equals("女") ? EnumCheckSexType.Woman : EnumCheckSexType.NoSex);
                    }
                    else
                    {
                        var currentModel = ValidateSexWithColorPoint(viewModel.Device, bounds);

                        sex = currentModel.Sex;
                    }


                }

                //只加男的 并且 当前好有也是男的  或者   ///只加女的 并且当前好友也是女的
                if (!(viewModel.Sex == EnumCheckSexType.Man && sex == EnumCheckSexType.Man)
                    && !(viewModel.Sex == EnumCheckSexType.Woman && sex == EnumCheckSexType.Woman)
                    && !(viewModel.Sex == EnumCheckSexType.NoMan && (sex == EnumCheckSexType.NoSex || sex == EnumCheckSexType.Woman))
                    && !(viewModel.Sex == EnumCheckSexType.NoWoman && (sex == EnumCheckSexType.NoSex || sex == EnumCheckSexType.Man))
                    )
                {
                    sexNode = null;
                }

            }

            ///记录添加新朋友记录
            if (null != clickBtn)
            {
                AddFriendInfoRecord(new WXActionViewModel() { Device = viewModel.Device, RemarkContent = viewModel.RemarkContent, Sex = (EnumCheckSexType)sex });
            }

            ///别人有回复 则放弃这个朋友
            if (null == sexNode || null == clickBtn)
            {
                viewModel.IsGoOn = false;
            }
            else
            {

                var strValue = clickBtn.Attributes["bounds"].Value;

                viewModel = GetStartBoundsWithEndBounds<WXActionViewModel>(viewModel, strValue, (o) =>
                {

                    return o;

                });
            }


            return viewModel;

            #endregion
        }


        /// <summary>
        /// 分析 打招呼页面 XML文档
        /// </summary>
        public WXActionViewModel ValiDateSayHelloPage(XmlDocument doc, WXActionViewModel viewModel)
        {
            var node = doc.SelectSingleNode("//node[contains(@bounds,'[24,146][696,228]')]");
            var node1920= doc.SelectSingleNode("//node[contains(@bounds,'[36,204][1044,326]')]");
            var bounds = string.Empty;

            if (null == node &&  node1920==null)
            {
                viewModel.IsValidatedState = false;

                return viewModel;
            }

            if (node==null)
            {
                node = node1920;
            }

            ///验证申请 设置备注 填入备注
            XmlNodeList nodeList = node.ParentNode.ParentNode.ChildNodes;

            var text = node.Attributes["text"].Value;

            ///打招呼
            if (text.Length == 15)
            {
                viewModel.IsValidatedState = true;

                bounds = nodeList[2].ChildNodes[0].Attributes["bounds"].Value;
            }
            else
            {
                viewModel.IsValidatedState = false;

                bounds = nodeList[1].ChildNodes[0].Attributes["bounds"].Value;
            }

            viewModel = GetStartBoundsWithEndBounds<WXActionViewModel>(viewModel, bounds, (o) =>
            {

                return o;

            });

            return viewModel;
        }


        /// <summary>
        /// 进入打招呼页面
        /// </summary>
        public bool EnterSayHelloActivity(WXActionViewModel viewModel, WMPoint wmPoint = null)
        {
            #region 分析 打招呼页面 XML文档

            viewModel.XMLName = "SayHelloPage";

            Action<int, int> _currentAction = (x, y) =>
             {

                 _baseAction.actionDirectStr(viewModel.Device, x, y, wmPoint);

             };

            Action<string> _currentInputContentAction = (content) =>
            {

                _baseAction.InitProcessWithTaskState(viewModel.Device, "shell am broadcast -a ADB_INPUT_TEXT --es msg '" + content + "'");

            };

            //_currentAction.Invoke(650, 240);

            _currentAction.Invoke(650, 240);

            var currentViewModel = _baseAction.CreatUIXML(viewModel, (o, doc) => { return ValiDateSayHelloPage(doc, o); });

            ///表示是要发送验证申请的
            if (currentViewModel.IsValidatedState)
            {
                _currentInputContentAction(viewModel.SayHiContent);

                ///设置备注文本框获取焦点
                _currentAction.Invoke(650, 465);

                //删除默认备注
                _currentAction.Invoke(650, 465);

            }

            if (!string.IsNullOrEmpty(viewModel.RemarkContent))
            {
                ///输入备注
                _currentInputContentAction(viewModel.RemarkContent);
            }
            else
            {
                ///点击 填入 按钮  输入备注
                _currentAction.Invoke(viewModel.RightWidth - 30, viewModel.BottomHeight - 15);
            }

            #endregion

            ///向上滑动 防止出现通知 
            _baseAction.InitProcessWithTaskState(viewModel.Device, " shell input swipe 100 100 600  100");

            //发送 
            _baseAction.actionDirectStr(viewModel.Device, 700, 100, wmPoint);

            //检测是否进入详细资料 有可能对方删除或者拉黑了我  会出现验证失败  不会自动返回详细资料页面
            var isSuccess = _baseAction.CircleDetection("ContactInfoUI", viewModel.Device, false);

            if (!isSuccess)
            {
                //返回
                _baseAction.actionDirectStr(viewModel.Device, 50, 100, wmPoint);
            }

            return isSuccess;
        }

        /// <summary>
        /// 查看当天打过招呼的好友的状态
        /// </summary>
        public WXActionViewModel DeleteFriend(WXActionViewModel viewModel)
        {
            var model = _baseAction.CreatUIXML(viewModel, (o, doc) =>
              {

                  ///表示添加 或者 接受  按钮
                  var currentModel1280 = doc.SelectSingleNode("//node[contains(@bounds,'[584,383][704,443]')]");
                  var currentModel1920 = doc.SelectSingleNode("//node[contains(@bounds,'[876,559][1056,649]')]");
                  var innerXml = string.Empty;

                  Action<string> func = (xml) =>
                  {
                      if (xml.Contains("添加") || xml.Contains("接受"))
                      {
                          o.IsValidatedState = true;
                      }

                      else
                      {
                          var firstIndex = innerXml.IndexOf("?");

                          var lastIndex = innerXml.LastIndexOf("?");

                          if ((lastIndex - firstIndex) == 1)
                          {
                              o.IsValidatedState = true;
                          }
                      }
                  };

                  if (currentModel1280 != null)
                  {
                      innerXml = currentModel1280.InnerXml;
                      func(innerXml);
                  }
                  else if (currentModel1920 != null)
                  {
                      innerXml = currentModel1920.InnerXml;
                      func(innerXml);
                  }
                  else
                  {
                      o.IsValidatedState = false;
                  }

                  return o;

              });

            return model;
        }

        /// <summary>
        /// 单条判断通讯录是否有好友
        /// </summary>
        /// <returns></returns>
        public WXActionViewModel IsHaveNewFriend(WXActionViewModel viewModel)
        {
            viewModel.XMLName = "isHaveNewFriend";

            var model = _baseAction.CreatUIXML(viewModel, (o, doc) =>
            {

                Func<XmlNode> _pointFunc = () =>
                {

                    return doc.SelectSingleNode("//node[contains(@bounds,'[116,539][340,583]')]");

                };

                Func<XmlNode> _textFunc = () =>
                {

                    return doc.SelectSingleNode("//node[@text='添加手机联系人']");

                };

                if (null != _pointFunc.Invoke() || null != _textFunc.Invoke())
                {
                    o.IsValidatedState = false;

                }
                else
                {
                    o.IsValidatedState = true;
                }

                return o;

            });

            ///记录设备新朋友状态
            Task.Factory.StartNew(() =>
            {
                SingleHepler<NewFriendStateBLL>.Instance.SaveChange(new NewFriendStateViewModel() { Device = viewModel.Device, IsImportContracts = false, IsHaveNewFriend = model.IsValidatedState });

            });


            return model;

        }

        /// <summary>
        /// 批量判断通讯录是否有好友
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        public void BatchCheckIsHaveNewFriend()
        {
            if (null == _baseAction.Devices || _baseAction.Devices.Count == 0)
            {
                return;
            }

            _baseAction.CommonAction<WXActionViewModel>(_baseAction.Devices, (model) =>
            {
                var wmPoint = _baseAction.GetWMSize(model.Device);

                EnterNewFriendPageAction(model, wmPoint);

                IsHaveNewFriend(model);

            });
        }

        #endregion

        #region 打开微信

        private void OpenWX(string device, WMPoint wmPointModel = null)
        {

            _baseAction.OpenApp(device, "com.tencent.mm", "com.tencent.mm/com.tencent.mm.ui.LauncherUI", () =>
            {

                var stateStr = string.Empty;

                if (wmPointModel == null)
                {
                    wmPointModel = _baseAction.GetWMSize(device);
                }

                while (!stateStr.Contains("PopupWindow"))
                {
                    ///向上滑动 防止出现通知 
                    _baseAction.InitProcessWithTaskState(device, " shell input swipe 100 100 600  100");

                    _baseAction.actionDirectStr(device, 700, 100, wmPointModel);

                    var list = _baseAction.InitProcessWithTaskState(device, " shell dumpsys window | grep mCurrentFocus", true);

                    if (null == list || list.Count == 0)
                    {
                        return;
                    }

                    stateStr = string.Join("|", list);

                    Thread.Sleep(200);
                }

                _baseAction.actionDirectStr(device, 700, 100, wmPointModel);

            });

        }

        #endregion

        #region 向群聊中发送消息

        /// <summary>
        /// 群发朋友圈
        /// </summary>
        public IList<Task> BatchSendMessageToGroup(AutoServiceInfoViewModel viewModel)
        {

            return _baseAction.CommonAction<WXActionViewModel>(viewModel.Devices, (item) =>
            {
                SendMessageToGroup(item);

            });
        }


        /// <summary>
        /// 单个手机发群消息
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="wmPoint"></param>
        public void SendMessageToGroup(WXActionViewModel viewModel)
        {
            //获取当前手机的分辨率
            var wmPoint = _baseAction.GetWMSize(viewModel.Device);

            _baseAction.UnlockSingleScreen(viewModel.Device);

            OpenWX(viewModel.Device);

            ////右边的加号
            _baseAction.actionDirectStr(viewModel.Device, 700, 100, wmPoint);

            ///点击发起群聊
            _baseAction.actionDirectStr(viewModel.Device, 680, 200, wmPoint);

            //检测是否进入发起群聊  会一直检测
            _baseAction.CircleDetection("SelectContactUI", viewModel.Device, false);

            ///点击选择一个群
            _baseAction.actionDirectStr(viewModel.Device, 400, 360, wmPoint);

            //检测是否进入选择群聊界面  会一直检测
            _baseAction.CircleDetection("GroupCardSelectUI", viewModel.Device, false);

            ///分析页面布局
            _baseAction.CreatUIXML(new WXActionViewModel() { Device = viewModel.Device, XMLName = "groupChatList" }, (o, doc) =>
            {
                return AnalysisGroupChatList(o, doc);
            });

        }


        /// <summary>
        /// 分析群聊中的群列表面
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="doc"></param>
        /// <returns></returns>
        public WXActionViewModel AnalysisGroupChatList(WXActionViewModel viewModel, XmlDocument doc)
        {
            XmlNodeList nodeList = doc.SelectNodes("//node[@class='android.widget.TextView'][@index='1']");

            return new WXActionViewModel();
        }

        #endregion

        #region 获取当前没有新朋友的设备

        /// <summary>
        /// 检测设备中是否有新朋友
        /// </summary>
        public IList<string> CheckEquipmentIsHaveNewsFriend()
        {
            var devices = _baseAction.Devices;

            var equipmentDevices = new Lazy<List<string>>();

            var taskList = new Lazy<List<Task>>();

            if (null == devices || devices.Count == 0)
            {
                return default(List<string>);
            }


            foreach (var device in devices)
            {
                var task = Task.Factory.StartNew(() =>
                  {
                      var currentViewModel = new WXActionViewModel() { Device = device };

                      var wmPoint = _baseAction.GetWMSize(device);

                      EnterNewFriendPageAction(new WXActionViewModel() { Device = device }, wmPoint);

                      currentViewModel = IsHaveNewFriend(currentViewModel);

                      if (currentViewModel.IsValidatedState)
                      {
                          equipmentDevices.Value.Add(device);
                      }

                  });

                taskList.Value.Add(task);
            }


            Task.WaitAll(taskList.Value.ToArray());

            return equipmentDevices.Value;

        }

        #endregion

        /// <summary>
        /// 本地文件上传到手机
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public UploadFilesToPhoneViewModel UploadToPhine(UploadFilesToPhoneViewModel viewModel)
        {
            var folder = new DirectoryInfo(viewModel.Path);

            var files = folder.GetFiles();

            viewModel.Files = files;

            var fullPath = Path.Combine(viewModel.Path, "content.txt");

            if (File.Exists(fullPath))
            {
                using (var fileStream = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                    {
                        viewModel.Content = streamReader.ReadToEnd();

                        streamReader.Close();
                    }

                    fileStream.Close();

                }
            }

            if ((null == files || files.ToList().Count == 0) && string.IsNullOrEmpty(viewModel.Content))
            {
                return default(UploadFilesToPhoneViewModel);
            }

            //删除文件夹
            _baseAction.InitProcessWithTaskState(viewModel.Device, "shell rm -r /sdcard/ZQKj/", true);

            ///创建文件夹
            _baseAction.InitProcessWithTaskState(viewModel.Device, "shell mkdir /sdcard/ZQKj", true);

            if (null != files && files.Count() > 0)
            {

                files.OrderByDescending(o => o.Name).ToList().ForEach((o) =>
                {
                    var direction = string.Format("push {0} /sdcard/ZQKj", o.FullName);

                    if (!o.Extension.Equals(".txt"))
                    {
                        var returnData = _baseAction.InitProcessWithTaskState(viewModel.Device, direction, true);

                        ///刷新单图  要不然 微信 图片选择器中的图片 不显示
                        _baseAction.InitProcessWithTaskState(viewModel.Device, "shell am broadcast -a android.intent.action.MEDIA_SCANNER_SCAN_FILE -d  file:///storage/emulated/0/ZQKj/" + o.Name, true);

                    }

                });


                ////批量刷新  要不然 微信 图片选择器中的图片 不显示
                //_baseAction.InitProcessWithTaskState(viewModel.Device, "shell am broadcast -a android.intent.action.MEDIA_MOUNTED -d  file:///storage/emulated/0/ZQKj/", true);
            }

            return viewModel;
        }

        /// <summary>
        /// 解析手机当前控件的起始坐标 终止坐标
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="strValue"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public T GetStartBoundsWithEndBounds<T>(WXActionViewModel viewModel, string strValue, Func<WXActionViewModel, T> func = null) where T : class
        {

            if (!string.IsNullOrEmpty(strValue))
            {
                var list = strValue.Replace("[", "").Replace(",", "|").Replace("]", "|").Split('|').ToList().Where(q => !string.IsNullOrEmpty(q)).ToList();

                viewModel.LeftWidth = int.Parse(list.FirstOrDefault());

                viewModel.RightWidth = int.Parse(list.Skip(2).Take(1).FirstOrDefault());

                viewModel.TopHeight = int.Parse(list.Skip(1).Take(1).FirstOrDefault());

                viewModel.BottomHeight = int.Parse(list.LastOrDefault());

                return func(viewModel);

            }

            return viewModel as T;
        }

        public WXActionViewModel ValidateSexWithColorPoint(string device, string bounds)
        {
            var currentValidateSexImagePath = string.Format(@"{0}\{1}", SingleHepler<ConfigInfo>.Instance.CutImageFileUrl, device);

            //截图
            _baseAction.InitProcessWithTaskState(device, " shell screencap -p /sdcard/validateSex.jpg");

            var returnData = _baseAction.InitProcessWithTaskState(device, string.Format("pull /sdcard/validateSex.jpg {0}", currentValidateSexImagePath), true);

            return GetStartBoundsWithEndBounds<WXActionViewModel>(new WXActionViewModel(), bounds, (o) =>
            {

                Bitmap bmp = new Bitmap(string.Format(@"{0}\validateSex.jpg", currentValidateSexImagePath));

                Color color = bmp.GetPixel((o.LeftWidth + o.RightWidth) / 2, (o.TopHeight + o.BottomHeight) / 2);

                ///女
                if (color.R == 238 && color.G == 115 && color.B == 87)
                {
                    o.Sex = EnumCheckSexType.Woman;
                }

                //男
                else if (color.R == 88 && color.G == 155 && color.B == 238)
                {
                    o.Sex = EnumCheckSexType.Man;
                }
                else
                {
                    o.Sex = EnumCheckSexType.NoSex;
                }

                return o;

            });
        }

        public string GetFansCountByStr(string xmlData)
        {
            if (string.IsNullOrEmpty(xmlData))
            {
                new ArgumentNullException();
            }

            var index1 = xmlData.IndexOf("text=");

            var index2 = xmlData.IndexOf("resource-id");

            var containDataStr = xmlData.Substring(index1, index2 - index1);

            var chars = containDataStr.ToCharArray();

            var len = chars.Length;

            var over = false;

            var strBD = new StringBuilder();

            for (int i = 0; i < len; i++)
            {
                var data = -1;

                var isOk = int.TryParse(chars[i].ToString(), out data);

                if (!isOk && over)
                {
                    break;
                }

                if (isOk)
                {
                    over = true;

                    strBD.Append(data.ToString());
                }

            }

            return strBD.ToString();
        }
    }
}
