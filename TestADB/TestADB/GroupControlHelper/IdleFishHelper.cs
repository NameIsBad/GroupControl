using GroupControl.Common;
using GroupControl.Model;
using GroupControl.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace GroupControl.Helper
{

    /// <summary>
    ///闲鱼
    /// </summary>
    public class IdleFishHelper
    {
        private BaseAction baseAction = SingleHepler<BaseAction>.Instance;

        private Func<IdleFishActionViewModel, string, IdleFishActionViewModel> func
        {
            get
            {
                return (model, xmlSelectStr) =>
               {

                   return baseAction.CreatUIXML<IdleFishActionViewModel>(model, (o, doc) =>
                   {

                       XmlNode node = doc.SelectSingleNode(xmlSelectStr);

                       var strValue = node.Attributes["bounds"].Value;

                       return baseAction.GetStartBoundsWithEndBounds<IdleFishActionViewModel>(o, strValue, (q) =>
                       {
                           return q as IdleFishActionViewModel;
                       });

                   });

               };
            }
        }

        /// <summary>
        /// 选择分类  CategoryActivity
        /// </summary>
        private Action<IdleFishActionViewModel, WMPoint> CategoryAction
        {
            get
            {
                return (viewModel, wmPoint) =>
                {

                    ///点击分类
                    baseAction.actionDirectStr(viewModel.Device, (viewModel.LeftWidth + viewModel.RightWidth) / 2, (viewModel.TopHeight + viewModel.BottomHeight) / 2, wmPoint);

                    //循环检测是否出现价格输入页面
                    baseAction.CircleDetection("CategoryActivity", viewModel.Device, false);

                    viewModel.XMLName = "categoryactivity";

                    //输入价格
                    baseAction.CreatUIXML<IdleFishActionViewModel>(viewModel, (o, doc) =>
                    {
                        XmlNode node = doc.SelectSingleNode("//node[@resource-id='com.taobao.idlefish:id/category_sub_view']");

                        var strValue = node.Attributes["bounds"].Value;

                        var boundsModel = baseAction.GetStartBoundsWithEndBounds<IdleFishActionViewModel>(o, strValue);

                        baseAction.actionDirectStr(viewModel.Device, (boundsModel.LeftWidth + boundsModel.RightWidth) / 2, (boundsModel.TopHeight + boundsModel.BottomHeight) / 2, wmPoint);

                        return boundsModel;

                    });

                };
            }
        }

        /// <summary>
        ///输入价格
        /// </summary>
        public Action<IdleFishActionViewModel, WMPoint> PriceAction
        {
            get
            {
                return (viewModel, wmPoint) =>
                {
                    ///点击价格
                    baseAction.actionDirectStr(viewModel.Device, (viewModel.LeftWidth + viewModel.RightWidth) / 2, (viewModel.TopHeight + viewModel.BottomHeight) / 2, wmPoint);

                    //循环检测是否出现价格输入页面
                    baseAction.CircleDetection("PopupWindow", viewModel.Device, false);

                    viewModel.XMLName = "inputprice";

                    //输入价格
                    baseAction.CreatUIXML<IdleFishActionViewModel>(viewModel, (o, doc) =>
                    {
                        XmlNode node = doc.SelectSingleNode("//node[@resource-id='com.taobao.idlefish:id/input_price']");

                        var strValue = node.Attributes["bounds"].Value;

                        var boundsModel = baseAction.GetStartBoundsWithEndBounds<IdleFishActionViewModel>(o, strValue);

                        ///点击text 获取焦点
                        baseAction.actionDirectStr(viewModel.Device, (boundsModel.LeftWidth + boundsModel.RightWidth) / 2, (boundsModel.TopHeight + boundsModel.BottomHeight) / 2, wmPoint);

                        ///输入文字
                        baseAction.InitProcessWithTaskState(viewModel.Device, "shell am broadcast -a ADB_INPUT_TEXT --es msg '" + viewModel.Price + "'");

                        //确定
                        baseAction.actionDirectStr(viewModel.Device, 100, 100, wmPoint);

                        return boundsModel;

                    });
                };
            }
        }

        /// <summary>
        ///输入标题或者内容
        /// </summary>
        public Action<IdleFishActionViewModel, WMPoint> TxtAction
        {
            get
            {
                return (viewModel, wmPoint) =>
                {
                    ///点击text 获取焦点
                    baseAction.actionDirectStr(viewModel.Device, (viewModel.LeftWidth + viewModel.RightWidth) / 2, (viewModel.TopHeight + viewModel.BottomHeight) / 2, wmPoint);

                    ///输入文字
                    baseAction.InitProcessWithTaskState(viewModel.Device, "shell am broadcast -a ADB_INPUT_TEXT --es msg '" + viewModel.CurrentContent + "'");
                };
            }
        }

        /// <summary>
        /// 点击确认发布按钮
        /// </summary>
        private Action<IdleFishActionViewModel, WMPoint> ClickAction
        {
            get
            {
                return (viewModel, wmPoint) =>
                 {
                     var stateStr = string.Empty;

                    ///点击按钮
                    baseAction.actionDirectStr(viewModel.Device, (viewModel.LeftWidth + viewModel.RightWidth) / 2, (viewModel.TopHeight + viewModel.BottomHeight) / 2, wmPoint);

                    //检测微信运行状态
                    while (stateStr.Contains("PublishActivity"))
                     {

                        ////是否被移除
                        var isRemove = baseAction.CheckEquipmentIsConnecting(viewModel.Device);

                         if (isRemove)
                         {
                             return;
                         }

                         var list = baseAction.InitProcessWithTaskState(viewModel.Device, " shell dumpsys window | grep mCurrentFocus", true);

                         if (null == list || list.Count == 0)
                         {
                             return;
                         }

                         stateStr = string.Join("|", list);

                         if (!stateStr.Contains("PublishActivity"))
                         {
                             return;
                         }

                         Thread.Sleep(1000);

                        ///点击按钮
                        baseAction.actionDirectStr(viewModel.Device, (viewModel.LeftWidth + viewModel.RightWidth) / 2, (viewModel.TopHeight + viewModel.BottomHeight) / 2, wmPoint);

                     }
                 };
            }
        }



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
                    stateStr = action(stateStr);
                }

            });
        }

        /// <summary>
        /// 批量发布信息
        /// </summary>
        public IList<Task> BatchPublishMessageinfo(AutoServiceInfoViewModel viewModel)
        {
            return baseAction.CommonAction<IdleFishActionViewModel>(viewModel.Devices, (currentData) =>
            {
                var currentViewModel = baseAction.UploadToPhine(new UploadFilesToPhoneViewModel()
                {

                    Device = currentData.Device,
                    Path = viewModel.Path,
                    PublishContentType = viewModel.PublishContentType

                });

                currentData.Title = viewModel.AutoServiceInfoModel.SayHelloContent;

                currentData.Description = viewModel.AutoServiceInfoModel.RemarkContent;

                currentData.Price = viewModel.AutoServiceInfoModel.AddCount;

                SinglePublishMessageInfo(currentData, currentViewModel);

            });
        }


        /// <summary>
        /// 单个发布信息
        /// </summary>
        public void SinglePublishMessageInfo(IdleFishActionViewModel idleFishViewModel, UploadFilesToPhoneViewModel uploadFilesViewModel)
        {

            OpenIdleFish(idleFishViewModel.Device);

            ///获取当前手机的分辨率
            var wmPoint = baseAction.GetWMSize(idleFishViewModel.Device);

            ///点击首页发布按钮
            baseAction.actionDirectStr(idleFishViewModel.Device, 360, 1205, wmPoint);

            ///循环检测是否进入发布页面
            baseAction.CircleDetection("PublishEntryActivity", idleFishViewModel.Device, false);

            ///点击发布闲置
            baseAction.actionDirectStr(idleFishViewModel.Device, 147, 924, wmPoint);

            ///循环检测是否进入拍摄界面
            baseAction.CircleDetection("MultiMediaStudioActivity", idleFishViewModel.Device, false);

            ///点击相册选择照片
            baseAction.actionDirectStr(idleFishViewModel.Device, 120, 1235, wmPoint);

            ///循环检测是否进入图片选择界面
            baseAction.CircleDetection("MultiMediaStudioActivity", idleFishViewModel.Device, false);

            ///选择照片
            baseAction.SelectImage(uploadFilesViewModel.Files, wmPoint, idleFishViewModel.Device, 147, 97, 180, 180);

            ///分析确定按钮
            IdleFishActionViewModel currentViewModel = func(new IdleFishActionViewModel() { Device = idleFishViewModel.Device, XMLName = "idlefish_selectimage" }, "//node[@class='android.widget.Button']");

            //点击确定按钮
            baseAction.actionDirectStr(idleFishViewModel.Device, (currentViewModel.LeftWidth + currentViewModel.RightWidth) / 2, (currentViewModel.TopHeight + currentViewModel.BottomHeight) / 2, wmPoint);

            ///循环检测是否进入编辑界面
            baseAction.CircleDetection("MultiMediaEditorActivity", idleFishViewModel.Device, false);

            //分析下一步按钮的位置
            currentViewModel.XMLName = "effect_next";
            currentViewModel = func(currentViewModel, "//node[@resource-id='com.taobao.idlefish:id/effect_next']");

            //点击下一步按钮
            baseAction.actionDirectStr(idleFishViewModel.Device, (currentViewModel.LeftWidth + currentViewModel.RightWidth) / 2, (currentViewModel.TopHeight + currentViewModel.BottomHeight) / 2, wmPoint);

            //循环检测是否进入发布界面
            baseAction.CircleDetection("PublishActivity", idleFishViewModel.Device, false);

            //发布宝贝页面
            PublishPage(idleFishViewModel, wmPoint);

        }

        /// <summary>
        /// 发布宝贝页面
        /// </summary>
        public void PublishPage(IdleFishActionViewModel idleFishViewModel, WMPoint wmPoint)
        {
            var currentViewModel = idleFishViewModel;

            currentViewModel.XMLName = "publishactivity";

            ///分析布局
            Action<IdleFishActionViewModel, string, XmlDocument, Action<IdleFishActionViewModel, WMPoint>> func = (model, xmlSelectStr, doc, action) =>
             {
                ///标题
                XmlNode node = doc.SelectSingleNode(xmlSelectStr);

                 var strValue = node.Attributes["bounds"].Value;

                 var returnData = baseAction.GetStartBoundsWithEndBounds<IdleFishActionViewModel>(model, strValue);

                 returnData.CurrentContent = returnData.Title;

                 if (xmlSelectStr.Contains("content"))
                 {
                     returnData.CurrentContent = returnData.Description;
                 }

                 action(returnData, wmPoint);

             };

            baseAction.CreatUIXML<IdleFishActionViewModel>(currentViewModel, (o, doc) =>
            {
                ///标题
                func(o, "//node[@resource-id='com.taobao.idlefish:id/title']", doc, TxtAction);

                ///描述
                func(o, "//node[@resource-id='com.taobao.idlefish:id/content']", doc, TxtAction);

                ///价格
                func(o, "//node[@resource-id='com.taobao.idlefish:id/price']", doc, PriceAction);

                //分类
                func(o, "//node[@resource-id='com.taobao.idlefish:id/cate_name']", doc, CategoryAction);

                //确认发布按钮
                func(o, "//node[@resource-id='com.taobao.idlefish:id/publish_button']", doc, ClickAction);

                return o;

            });
        }




    }
}
