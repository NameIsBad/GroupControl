using GroupControl.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using GroupControl.BLL;
using GroupControl.Common;
using GroupControl.Helper;
using GroupControl.Model;

namespace GroupControl.WinForm
{
    public partial class ImportContractsForm : BaseForm
    {

        private int _currentImportCount = 200;

        private int _customCheckDay = 1;

        private IList<VCardViewModel> _currentVCardViewModelList;

        private IList<NewFriendState> _currentNoFriendList;

        public ImportContractsForm()
        {
            InitializeComponent();
        }



        /// <summary>
        /// 选择要导入的文本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectTxt_Click(object sender, EventArgs e)
        {
            using (var fileDialog = new OpenFileDialog())
            {

                //判断用户是否正确的选择了文件
                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    //获取用户选择文件的后缀名
                    string extension = Path.GetExtension(fileDialog.FileName);

                    try
                    {
                        if (!extension.ToLower().Equals(".txt"))
                        {
                            MessageBox.Show("请选择文本文档！");

                            return;
                        }


                        this.textBox1.Text = fileDialog.FileName;

                    }
                    catch (Exception ex)
                    {

                    }
                }

                if (!string.IsNullOrEmpty(this.textBox1.Text))
                {
                    var path = this.textBox1.Text;

                    var fileName = Path.GetFileName(path).Split('.')[0];

                    _currentVCardViewModelList = baseAction.GetPersonInfoFromTxt(new VCardViewModel() { Name = fileName, SourceFilePath = path });

                    if (null == _currentVCardViewModelList || _currentVCardViewModelList.Count == 0)
                    {

                        MessageBox.Show("上传的文件内容内容为空！");

                        return;
                    }

                    SetTextBoxCursor(this.textBox2, string.Empty);

                    SetTextBoxCursor(this.textBox2, string.Format("总计 {0} 条数据\r\n\r\n", _currentVCardViewModelList.Count));

                }
            }
        }


        /// <summary>
        /// 开始导入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStartImport_Click(object sender, EventArgs e)
        {
            if (_taskQueue.Count > 0)
            {
                CustomDialog();

                return;
            }

            if (null == _currentVCardViewModelList || _currentVCardViewModelList.Count == 0)
            {

                MessageBox.Show("请上传包含包含通讯录数据的文本文档！");

                return;
            }

            if (null == _currentNoFriendList || _currentNoFriendList.Count == 0)
            {
                MessageBox.Show("暂无可用设备！");

                return;
            }

            _currentVCardViewModelList = _currentVCardViewModelList.Select(o =>
            {

                o.ImportCount = _currentImportCount;

                return o;

            }).ToList();

            var deviceList = GetCheckedEquipmentName(this.flowLayoutPanelWithEquipment);

            if (null == deviceList || deviceList.Count == 0)
            {
                deviceList = _currentNoFriendList.Select(o => o.Device).ToList();
            }

            try
            {
                Task.Factory.StartNew(() =>
                {
                    var taskList = baseAction.ImportContracts(ref _currentVCardViewModelList, deviceList, (o) =>
                    {
                        baseAction.SetContentWithCurrentThread<string>(this.textBox2, o, (control, str) =>
                        {

                            var textBox = control as TextBox;

                            SetTextBoxCursor(textBox, str);

                        });
                    });

                    Task.WhenAll(taskList.ToArray()).ContinueWith(task =>
                    {

                        baseAction.SetContentWithCurrentThread<string>(this.textBox2, "全部导入完毕！", (control, str) =>
                        {

                            var textBox = control as TextBox;

                            SetTextBoxCursor(textBox, str);

                        });

                    }).ContinueWith(task => {

                        ///插入导入记录
                        SingleHepler<ContractsInfoBLL>.Instance.InsertContractWithContractDetail(

                            new ContractsInfoViewModel()
                            {

                                ContractsInfoModel = new ContractsInfo() { Name = _currentVCardViewModelList.FirstOrDefault().Name, CreateDate = DateTime.Now, RecordCount = _currentVCardViewModelList.Count },

                                ContractInfoDetailList = _currentVCardViewModelList.Select(o =>
                                {

                                    return new ContractInfoDetail() {  Content = o.MobilePhone, CreateDate = DateTime.Now, Device =o.Device };

                                }).ToList()

                            });

                        ///刷新设备
                        GetEquipmentWithNoNewFriend(new NewFriendStateViewModel() { });

                    });

                });

            }
            catch (Exception ex)
            {

            }


        }

        private void ImportContractsForm_Load(object sender, EventArgs e)
        {

            Func<EnumSelectDateTimeType, string, RadioButton> currentFunc = (value, text) =>
            {
                var radioWithTimeSpan = new RadioButton();

                radioWithTimeSpan.Text = text;

                radioWithTimeSpan.Tag = ((int)value).ToString();

                radioWithTimeSpan.Click += (o, q) =>
                {
                    var radio = o as RadioButton;

                    GetEquipmentWithNoNewFriend(new NewFriendStateViewModel() { SelectDateTimeType = (EnumSelectDateTimeType)Convert.ToInt32(radio.Tag) });

                };

                return radioWithTimeSpan;
            };

            var currentControls = this.flowLayoutPanelWithTimeSpan.Controls;

            currentControls.Add(currentFunc(EnumSelectDateTimeType.OneDay, "一天"));

            currentControls.Add(currentFunc(EnumSelectDateTimeType.OneWeek, "一周"));

            currentControls.Add(currentFunc(EnumSelectDateTimeType.OneMonth, "一个月"));

            CustomNumericUpDown(this.numericUpDownImportCount, _currentImportCount, 1000, 10, (o, s) =>
             {

                 var control = (NumericUpDown)o;

                 _currentImportCount = (int)control.Value;

             });


            CustomNumericUpDown(this.customDateType, _customCheckDay, 100, 1, (o, s) =>
           {

               var control = (NumericUpDown)o;

               _customCheckDay = (int)control.Value;

           });

            GetEquipmentWithNoNewFriend(new NewFriendStateViewModel() { });

        }


        /// <summary>
        /// 清空通讯录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClearContracts_Click(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void GetEquipmentWithNoNewFriend(NewFriendStateViewModel viewModel)
        {

            Task.Factory.StartNew<IList<NewFriendState>>((model) =>
            {

                var currentDevices = model as NewFriendStateViewModel;

                currentDevices.CurentUseingEquipmentList = baseAction.Devices;

                _currentNoFriendList = SingleHepler<NewFriendStateBLL>.Instance.GetList(currentDevices);

                return _currentNoFriendList;

            }, viewModel).ContinueWith((task) =>
            {

                var returnData = task.Result;

                baseAction.SetContentWithCurrentThread<object>(this.flowLayoutPanelWithEquipment, null, (control, str) =>
                {
                    control.Controls.Clear();

                });

                if (null == returnData || returnData.Count == 0)
                {
                    return;
                }

                baseAction.SetContentWithCurrentThread<object>(this, null, (control, str) =>
                {
                    control.Text = string.Format("当前设备数 {0}", returnData.Count);

                });

                var equipmentList = returnData.Select(o =>
                {

                    return o.Device;

                }).ToList();

                ShowCurrentEquipmentList(this.flowLayoutPanelWithEquipment, equipmentList: equipmentList);

            });
        }


        /// <summary>
        /// 自定义时间查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {

            GetEquipmentWithNoNewFriend(new NewFriendStateViewModel() { CustomCheckDay = _customCheckDay });


        }

        private void button2_Click(object sender, EventArgs e)
        {
            BatchCreatePhoneNumberForm form = new BatchCreatePhoneNumberForm();

            form.Text = "生成手机号";

            form.StartPosition = FormStartPosition.CenterParent;

            form.ShowDialog();
        }
    }
}
