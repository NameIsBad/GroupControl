using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using GroupControl.Common;
using GroupControl.Helper;
using GroupControl.Model;
using GroupControl.BLL;
using System.IO;

namespace TestADB
{
    public partial class RecoveryDataForm : BaseForm
    {
        public RecoveryDataForm()
        {
            InitializeComponent();
        }

        private void RecoveryDataForm_Load(object sender, EventArgs e)
        {
            CustomDialog(@"在回收数据之前，请先： 
                           1.检测通讯录是否有好友.
                           2.至少成功完成一次添加好友的操作.
                           3.以上两个操作无先后顺序.
                           点击确定按钮执行回收数据", async () =>
            {
                await RecoveryData();

                SetTextBoxCursorWithNoDelegate(this.textBox1, "执行完毕，点击关闭按钮退出！");

            },()=> {

                this.Close();

            });
        }

        /// <summary>
        ///
        /// </summary>
        public async Task RecoveryData()
        {
            var availableDeviceList = await ValidateEquipment();

            if (null == availableDeviceList || availableDeviceList.Count == 0)
            {
                SetTextBoxCursorWithNoDelegate(this.textBox1, "没有可进行数据回收的设备！");

                return;
            }

            SetTextBoxCursorWithNoDelegate(this.textBox1, string.Join("\r\n\r\n", availableDeviceList.Select(o=>o.NickName).ToList()));

            var returnData = await QueryImportData(availableDeviceList);

            if (null == returnData || returnData.Count == 0)
            {
                SetTextBoxCursorWithNoDelegate(this.textBox1, "没有可供导出的数据！");

                return;
            }

            await ImportData(returnData, availableDeviceList);
        }

        /// <summary>
        /// 验证可用于导出数据的设备
        /// </summary>
        /// <returns></returns>
        private async Task<IList<DeviceToNickName>> ValidateEquipment()
        {
            SetTextBoxCursorWithNoDelegate(this.textBox1, "正在查询可用于回收数据的相关设备...");

            var deviceList = await Task.Factory.StartNew<IList<DeviceToNickName>>(() =>
            {

                return SingleHepler<DeviceToNickNameBLL>.Instance.GetImportFailedDeviceList();

            });


            return deviceList;


        }

        /// <summary>
        /// 查询导出的数据
        /// </summary>
        /// <returns></returns>
        public async Task<IList<ContractInfoDetail>> QueryImportData(IList<DeviceToNickName> currentDeviceList)
        {
            SetTextBoxCursorWithNoDelegate(this.textBox1, "正在查询需要导出数据...");

            ///开始导出数据
            return await Task.Factory.StartNew<IList<ContractInfoDetail>>(() =>
            {

                return SingleHepler<ContractsInfoBLL>.Instance.GetListWithContractInfoDetailList(

                    currentDeviceList.Select(o =>
                    {
                        return new ContractInfoDetail() { Device = o.Device, CreateDate = DateTime.Now };

                    }).ToList());
            });
        }


        /// <summary>
        /// 导出数据
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public async Task ImportData(IList<ContractInfoDetail> contentList, IList<DeviceToNickName> availableDeviceList)
        {

            var dialog = new FolderBrowserDialog();

            dialog.Description = "请选择文件保存路径";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string foldPath = dialog.SelectedPath;

                var content = string.Join("\r\n", contentList.Select(o => o.Content).ToList());

                var fileName = string.Join("_", availableDeviceList.Select(o => o.NickName).ToList());

                var contentFullPath = Path.Combine(foldPath, string.Format("{0}.txt", fileName));

                SetTextBoxCursorWithNoDelegate(this.textBox1, "正在删除设备通讯录...");

                ///更新设备回收数据状态
                await Task.Factory.StartNew(() =>
                {

                    availableDeviceList.ToList().ForEach(item =>
                    {
                        ///清空手机上的通讯录
                        baseAction.InitProcess(item.Device, "shell pm clear com.android.providers.contacts");

                        ///删除VCF文件
                        baseAction.InitProcess(item.Device, "shell rm /sdcard/contacts.vcf");

                    });

                });

                SetTextBoxCursorWithNoDelegate(this.textBox1, "删除完毕.");

                SetTextBoxCursorWithNoDelegate(this.textBox1, "正在更新设备回收数据状态...");

                ///更新设备回收数据状态
                await Task.Factory.StartNew(() =>
                {
                    SingleHepler<NewFriendStateBLL>.Instance.BatchUpdateRecoveryState(availableDeviceList.Select(o =>
                    {

                        return new NewFriendState() { Device = o.Device };

                    }).ToList());

                });

                SetTextBoxCursorWithNoDelegate(this.textBox1, "更新完毕");

                SetTextBoxCursorWithNoDelegate(this.textBox1, "正在导出数据...");

                await Task.Factory.StartNew(() =>
                {
                    FileHelper.ReadToFile(contentFullPath, content);

                });

                SetTextBoxCursorWithNoDelegate(this.textBox1, string.Format("导出成功,请在 {0} 中查看", contentFullPath));

            }
        }
    }
}
