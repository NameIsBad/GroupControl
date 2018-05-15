using GroupControl.BLL;
using GroupControl.Common;
using GroupControl.Helper;
using GroupControl.Model;
using GroupControl.WinForm.CustomControl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static GroupControl.WinForm.CustomControl.RoundPanel;

namespace GroupControl.WinForm
{
    public partial class WorkPlaceForm : BaseForm
    {

        protected delegate void ReflashCurrentUsedEquipmentCount();

        protected event ReflashCurrentUsedEquipmentCount _reflashCurrentUsedEquipmentCount;

        private const int _width = 210;//190

        private const int _height = 355;//355

        /// <summary>
        /// 水平间隔 包含左右
        /// </summary>
        private const int _marginH = 10;

        /// <summary>
        /// 垂直间隔 包含上下
        /// </summary>
        private const int _marginV = 25;

        public WorkPlaceForm()
        {
            InitializeComponent();
        }

        private void WorkPlaceForm_Load(object sender, EventArgs e)
        {
            StartServer _startServerForm = new StartServer();

            _startServerForm.ShowDialog();

            this.MaximizeBox = true;

            this.MinimizeBox = true;

            //this.WindowState = FormWindowState.Maximized;

            baseAction = SingleHepler<BaseAction>.Instance;

            baseAction.ParentControl = this.flowLayoutPanel3;

            Task.Factory.StartNew(() =>
            {
                DevicesWithNickList = GetDeviceToNickNameUnionEquipments();

                baseAction.GetDevices(DevicesWithNickList, new MouseEventHandler(_picBox_MouseDoubleClick), (handler, o) =>
                {
                    CreateEquipmentMapToPC(o, handler);

                });

            }).ContinueWith((task) =>
            {

                _reflashCurrentUsedEquipmentCount += () =>
                {
                    var count = 0;

                    if (null != baseAction.Devices)
                    {

                        count = baseAction.Devices.Count;
                    }

                    baseAction.SetContentWithCurrentThread(this, string.Format("工作台 当前可用设备数量 {0} ", count), (form, param) =>
                    {
                        form.Text = param;

                    });


                };

                baseAction._tagTaskState += (viewModel) =>
                {
                    var currentViewModel = viewModel;

                    UpdateUIByTaskState(currentViewModel);

                };

                StartMonitorService();

                baseAction.ShowAllCutImageFromQuery((o) =>
                {
                    baseAction._tagTaskState(new UpdateWithTaskStateViewModel() { EquipmentDevice = o, TaskStatus = EnumTaskState.Start });

                }, (p) =>
                {

                    baseAction._tagTaskState(new UpdateWithTaskStateViewModel() { EquipmentDevice = p, TaskStatus = EnumTaskState.Complete });

                });


            });
        }

        private void 通讯录加好友ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowTimeAddFriend(EnumSendType.HandSend);
        }

        private void 通讯录定时加好友ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ShowTimeAddFriend(EnumSendType.AutoSend);
        }

        private void 搜索加好友ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            SearchFriendFrom form = new SearchFriendFrom();

            form.Text = "搜索好友";

            ShowForm(form);
        }

        private void 手动发朋友圈ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowTimeSendFriendCircle(EnumSendType.HandSend);
        }

        private void 定时发朋友圈ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowTimeSendFriendCircle(EnumSendType.AutoSend);
        }

        private void 查看粉丝数ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            QueryFansForm form = new QueryFansForm();

            form.Text = "统计粉丝数";

            ShowForm(form);
        }

        private void 历史粉丝数ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SkanFriendCount _skanFriendCount = new SkanFriendCount();

            _skanFriendCount.Text = "查看历史粉丝数";

            ShowForm(_skanFriendCount);
        }

        private void 检测通讯录ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_taskQueue.Count > 0)
            {
                CustomDialog();

                return;
            }

            SingleHepler<WXHelper>.Instance.BatchCheckIsHaveNewFriend();
        }

        private void 导入通讯录ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImportContractsForm form = new ImportContractsForm();

            form.Text = "导入通讯录";

            ShowForm(form);
        }

        private void 回收数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RecoveryDataForm form = new RecoveryDataForm();

            form.Text = "回收数据";

            ShowForm(form);
        }

        private void 导入历史记录ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SkanImportContractsRecordForm form = new SkanImportContractsRecordForm();

            form.Text = "查看历史导入记录";

            ShowForm(form);
        }

        /// <summary>
        /// 闲鱼手动发贴
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 手动发贴ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SendIdleFishPost form = new SendIdleFishPost();

            form.Text = "闲鱼手动发贴";

            form.Tag = EnumSendType.HandSend;

            ShowForm(form);
        }


        /// <summary>
        /// 闲鱼自动发贴
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 定时发贴ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SendIdleFishPost form = new SendIdleFishPost();

            form.Text = "闲鱼自动发贴";

            form.Tag = EnumSendType.AutoSend;

            ShowForm(form);
        }

        private void 手动发群消息ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 自动发群消息ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private async void 取消所有任务ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await Task.Factory.StartNew(() =>
            {

                AbortAllTask();

            });

            await CancelCurrentTask();
        }

        private async void 取消当前任务ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await CancelCurrentTask();
        }

        private void 历史任务ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TaskListForm form = new TaskListForm();

            form.Text = (sender as ToolStripMenuItem).Text;

            ShowForm(form);
        }

        private void 解屏ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            baseAction.UnlockScreen(false);
        }

        private void 锁屏ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            baseAction.UnlockScreen(true);
        }

        private void 批量重启ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BatchTask("reboot");
        }

        private void 批量关机ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BatchTask(" shell reboot -p ");
        }

        private void 批量安装软件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var btn = (ToolStripMenuItem)sender;

            InstallApp _installApp = new InstallApp();

            _installApp.Tag = (EnumInstallAppType)Convert.ToInt32(btn.Tag);

            _installApp.Text = "安装装机软件";

            _installApp._afreshSameScreenControl += (device) =>
            {

                var currentControlList = baseAction.Dic.Value[device];

                baseAction.SingleShowImageWithSocket(currentControlList);

            };

            ShowForm(_installApp);
        }

        private void 更改输入法ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var btn = (ToolStripMenuItem)sender;

            InstallApp _installApp = new InstallApp();

            _installApp.Tag = (EnumInstallAppType)Convert.ToInt32(btn.Tag);

            _installApp.Text = "安装装机软件";

            _installApp._afreshSameScreenControl += (device) =>
            {

                var currentControlList = baseAction.Dic.Value[device];

                baseAction.SingleShowImageWithSocket(currentControlList);

            };

            ShowForm(_installApp);
        }

        private void 按编号搜索ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SearchWithNumberForm form = new SearchWithNumberForm();

            form.Text = "按编号搜索设备";

            form._identificationEquipmentWithNumber += (data) =>
            {

                try
                {
                    var currentPanle = GetPictureByName(this.flowLayoutPanel3, data, (number, control) =>
                    {
                        if (control.Tag.Equals(data))
                        {
                            return true;
                        }

                        return false;
                    });

                    if (null != currentPanle && currentPanle.Controls.Count > 0)
                    {
                        // currentPanle.Controls[0].BackColor = Color.Red;

                        form.Close();

                        form.FormClosed += (o, q) =>
                        {
                            SameScreenForm samScreen = new SameScreenForm();

                            samScreen.Name = currentPanle.Name.Split('_')[0];

                            samScreen.Tag = (currentPanle.Controls[1] as PictureBox).Image;

                            samScreen.MaximizeBox = false; /// 设置最大化按钮是否有效 

                            samScreen.MinimizeBox = false; /// 设置最小化按钮是否有效 

                            samScreen.ShowDialog();
                        };

                    }
                    else
                    {
                        form.Text = "设备编号不存在！";
                    }
                }
                catch (Exception)
                {

                    // throw;
                }

            };

            ShowForm(form);
        }

        private void 关注点赞评论ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            KSActionForm form = new KSActionForm();

            form.Text = (sender as ToolStripMenuItem).Text;

            form.Tag = EnumSendType.AutoSend;

            ShowForm(form);
        }

        #region 内部方法

        private Control CreateEquipmentMapToPC(DeviceToNickNameViewModel model, MouseEventHandler handler)
        {

            CustomControl.RoundPanel _panel = new CustomControl.RoundPanel();

            _panel.Width = _width;
            _panel.Height = _height;
            _panel.Margin = new Padding(10, 20, 10, 20);
            _panel.RoundeStyle = RoundStyle.All;
            _panel.Radius = 30;
            _panel.Tag = model.NickName;
            _panel.BorderColor = Color.Black;
            _panel.Name = string.Format(partialControlName, model.Device);

            var _btn = CreateActionBtn(_panel, model.NickName, _panel.Width - _marginH * 2);
            _btn.Name = model.Device;
            _btn.Location = new Point(_marginH, _marginV);
            _btn.Click += (o, s) =>
            {

                var currentBtn = (Button)o;

                OpenSetEquipmentFrom(currentBtn);

            };

            var _checkBox = new CheckBox();
            _checkBox.Location = new Point(_panel.Width - 15, _marginV);
            _checkBox.Width = 30;
            _checkBox.Tag = model.Device;
            _checkBox.Text = "_";
            _checkBox.CheckStateChanged += (s, o) =>
            {
                var currentCheckBox = s as CheckBox;

                var currentDevice = currentCheckBox.Tag as string;

                if (currentCheckBox.Checked)
                {
                    currentCheckBox.ForeColor = Color.OrangeRed;

                    baseAction.CheckSomeEquipments.Value.Add(currentDevice, currentCheckBox);
                }
                else
                {
                    currentCheckBox.ForeColor = Color.DarkGray;

                    baseAction.CheckSomeEquipments.Value.Remove(currentDevice);
                }

                // currentCheckBox.Checked = !currentCheckBox.Checked;
            };

            PictureBox _picBox = new PictureBox();
            _picBox.BackgroundImageLayout = ImageLayout.Stretch;
            _picBox.SizeMode = PictureBoxSizeMode.StretchImage;
            _picBox.MouseDoubleClick += handler;
            _picBox.BackColor = Color.Black;
            _picBox.Width = _panel.Width - _marginH * 2;
            _picBox.Height = _panel.Height - 95;
            _picBox.Location = new Point(_marginH, _btn.Height + _btn.Location.Y);

            FlowLayoutPanel fp = new FlowLayoutPanel();
            fp.Height = 30;
            fp.Width = _panel.Width - _marginH * 2;
            fp.Location = new Point(_marginH, _picBox.Height + _picBox.Location.Y);
            fp.Parent = _panel;
            SingleEquipmentAction(fp);
            fp.Name = string.Format("{0}_{1}", model.NickName, model.Device);

            _panel.Controls.Add(_picBox);

            _panel.Controls.Add(fp);

            //_panel.Controls.Add(_checkBox);

            baseAction.SetContentWithCurrentThread<Control>(this.flowLayoutPanel3, _panel, (parent, panel) =>
            {
                if (string.IsNullOrEmpty(GetPictureByName(parent, panel.Name).Name))
                {
                    parent.Controls.Add(panel);
                }

            });


            if (!baseAction.Dic.Value.Keys.Contains(model.Device))
            {
                baseAction.Dic.Value.Add(model.Device, _panel);
            }


            return _panel;

        }

        public void SingleEquipmentAction(FlowLayoutPanel fp)
        {

            var parentControl = fp.Parent;

            var device = parentControl.Name.Split('_')[0];

            var currentWidth = (parentControl.Width - _marginH * 2) * 11 / 50;

            var rebootBtn = CreateActionBtn(fp, "", currentWidth);
            rebootBtn.Image = Properties.Resources.reboot;
            rebootBtn.ImageAlign = ContentAlignment.MiddleCenter;
            // rebootBtn.TextAlign = ContentAlignment.MiddleRight;
            rebootBtn.MouseClick += (e, obj) =>
            {
                Task.Factory.StartNew((data) =>
                {
                    var currentDevice = data as string;

                    ///取消任务
                    CancleTask(currentDevice);

                    baseAction._tagTaskState(new UpdateWithTaskStateViewModel() { EquipmentDevice = currentDevice, TaskStatus = EnumTaskState.Start });

                    baseAction.InitProcess(currentDevice, "reboot");

                }, device);
            };

            var sameScreenBtn = CreateActionBtn(fp, "", currentWidth);
            sameScreenBtn.Image = Properties.Resources.samescreen;
            sameScreenBtn.ImageAlign = ContentAlignment.MiddleCenter;
            sameScreenBtn.MouseClick += (e, obj) =>
            {

                SameScreenAction(device, (parentControl.Controls[1] as PictureBox).Image);

            };

            var shadowBtn = CreateActionBtn(fp, "", currentWidth);
            shadowBtn.Image = Properties.Resources.shadow;
            shadowBtn.ImageAlign = ContentAlignment.MiddleCenter;
            //shadowBtn.TextAlign = ContentAlignment.MiddleRight;
            shadowBtn.MouseClick += (e, obj) =>
            {
                CancleTask(device);

                baseAction.MaxPort++;

                baseAction._tagTaskState(new UpdateWithTaskStateViewModel() { EquipmentDevice = device, TaskStatus = EnumTaskState.Start });

                baseAction.SingleShowImageWithSocket(parentControl, baseAction.MaxPort, (currentDevice) =>
                {

                    baseAction._tagTaskState(new UpdateWithTaskStateViewModel() { EquipmentDevice = device, TaskStatus = EnumTaskState.Complete });

                });
            };


            var overBtn = CreateActionBtn(fp, "", currentWidth);
            overBtn.Image = Properties.Resources.over;
            overBtn.ImageAlign = ContentAlignment.MiddleCenter;
            //overBtn.TextAlign = ContentAlignment.MiddleRight;
            overBtn.MouseClick += (e, obj) =>
            {
                var btn = e as Button;

                btn.Tag = device;

                Task.Factory.StartNew((data) =>
                {
                    var currentDevice = data as string;

                    CancleTask(currentDevice);

                }, device);

            };
        }

        public void ShowTimeAddFriend(EnumSendType type)
        {
            TimerAddFriend form = new TimerAddFriend();

            form.Text = "添加好友";

            form.Tag = type;

            ShowForm(form);
        }

        protected void ShowForm(Form form)
        {
            form.StartPosition = FormStartPosition.CenterParent;

            form.ShowDialog();
        }

        public void ShowTimeSendFriendCircle(EnumSendType type)
        {
            TimerSendFriendCircle form = new TimerSendFriendCircle();

            form.Tag = type;

            form.Text = "发朋友圈";

            ShowForm(form);
        }

        /// <summary>
        /// 结束当前所有任务
        /// </summary>
        private void AbortAllTask()
        {
            if (null == _taskQueue || _taskQueue.Count == 0)
            {
                return;
            }

            ///移除队列中的所有元素 并跟新任务状态
            lock (_taskLockObject)
            {
                while (_taskQueue.Count > 0)
                {
                    var currentTask = _taskQueue.Dequeue();

                    ///任务完成 跟新数据库任务状态
                    _currentAction(currentTask.ID);

                }
            }

        }

        /// <summary>
        /// 批量任务
        /// </summary>
        /// <param name="direct"></param>
        public void BatchTask(string direct)
        {

            Task.Factory.StartNew(() =>
            {

                if (null != baseAction.Devices && baseAction.Devices.Count > 0)
                {
                    foreach (var device in baseAction.Devices)
                    {
                        Task.Factory.StartNew((data) =>
                        {
                            var currentDevice = data as string;

                            ///取消任务
                            CancleTask(currentDevice);

                            baseAction._tagTaskState(new UpdateWithTaskStateViewModel()
                            {

                                EquipmentDevice = currentDevice,

                                TaskStatus = EnumTaskState.Start

                            });

                            baseAction.InitProcess(currentDevice, direct);

                        }, device);
                    }
                }

            });


        }

        private void _picBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            PictureBox _box = sender as PictureBox;

            SameScreenAction(_box.Parent.Name.Split('_')[0], _box.Image);
        }

        public void SameScreenAction(string tag, Image image)
        {
            SameScreenForm form = new SameScreenForm();

            form.Name = tag;

            form.Tag = image;

            form.MaximizeBox = false; /// 设置最大化按钮是否有效 

            form.MinimizeBox = false; /// 设置最小化按钮是否有效 

            form.ShowDialog();
        }

        /// <summary>
        /// 启动设备监控服务
        /// </summary>
        private void StartMonitorService()
        {
            Task.Factory.StartNew(() =>
            {
                while (true)
                {

                    try
                    {
                        var deviceList = baseAction.InitProcess(null, "devices", true);

                        deviceList = deviceList.Where(o => !infoList.Contains(o)).TakeWhile(o => !string.IsNullOrEmpty(o) && !o.Split('\t')[1].Equals("unauthorized")).Select(o => { return o.Split('\t')[0]; }).ToList();

                        if (null != deviceList && deviceList.Count > 0 && null != baseAction.Devices)
                        {
                            ///表示重新获取的设备的数量
                            var currentDeviceCount = baseAction.Devices.Count;

                            ///表示当前正在使用的设备数量
                            var usingDeviceCount = deviceList.Count;

                            ///表示有新增的设备
                            if (currentDeviceCount < usingDeviceCount)
                            {
                                DymicAddEquipment(deviceList);
                            }

                            ///表示有减少的设备
                            else if (currentDeviceCount > usingDeviceCount)
                            {
                                DymicSubtraction(deviceList);
                            }
                        }
                        else
                        {
                            RemoveUnConnecting(string.Empty);

                            // isHaveEquipment = false;
                        }

                        ///刷新可用设备数量
                        _reflashCurrentUsedEquipmentCount();

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }

                    Thread.Sleep(100);
                }

            }, TaskCreationOptions.LongRunning);
        }

        public void DymicSubtraction(IList<string> deviceList)
        {
            if (null == deviceList || deviceList.Count == 0)
            {
                return;
            }

            baseAction.Devices.ToList().ForEach((item) =>
            {
                var returnData = deviceList.Any(o => o.Equals(item));

                if (!returnData)
                {
                    RemoveUnConnecting(item);
                }

            });
        }

        /// 移除断开连接的设备
        /// </summary>
        public void RemoveUnConnecting(string device)
        {
            try
            {
                if (!string.IsNullOrEmpty(device))
                {
                    baseAction.Devices.Remove(device);
                }
                else
                {
                    baseAction.Devices = new List<string>();
                }

                ///移除设备
                baseAction.SetContentWithCurrentThread<string>(this.flowLayoutPanel3, device, (parent, currentDevice) =>
                {

                    if (string.IsNullOrEmpty(currentDevice))
                    {
                        parent.Controls.Clear();
                    }
                    else
                    {
                        var currentControl = GetControlByName(this.flowLayoutPanel3, string.Format("{0}_Picture", currentDevice));

                        parent.Controls.Remove(currentControl);
                    }
                });

                ///取消当前手机正在运行的任务
                CancleTask(device);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                throw;
            }
        }

        public void UpdateUIByTaskState(UpdateWithTaskStateViewModel viewModel)
        {
            var currentViewModel = viewModel;

            var color = Color.CadetBlue;

            var messageStr = string.Empty;

            switch (currentViewModel.TaskStatus)
            {
                case EnumTaskState.BeingCancelled:

                    color = Color.CadetBlue;

                    messageStr = "任务正在取消中";

                    break;

                case EnumTaskState.Cancle:

                    color = Color.Red;

                    messageStr = "任务取消完毕";

                    break;

                case EnumTaskState.Complete:

                    color = Color.Green;

                    messageStr = "任务执行完毕";

                    break;

                case EnumTaskState.Start:

                    color = Color.CadetBlue;

                    messageStr = "任务正在执行中";

                    break;

                case EnumTaskState.Error:

                    color = Color.Red;

                    messageStr = "任务执行出错";

                    break;
            }


            var currentControl = GetControlByName(this.flowLayoutPanel3, string.Format(partialControlName, currentViewModel.EquipmentDevice));

            if (null != currentControl)
            {
                var btn = currentControl.Controls[0];

                baseAction.SetContentWithCurrentThread(btn, messageStr, (control, str) =>
                {

                    var text = control.Text;

                    if (!string.IsNullOrEmpty(text))
                    {
                        text = text.Split(' ')[0];
                    }

                    text = string.Format("{0} {1}", text, string.IsNullOrEmpty(viewModel.ShowMessage) ? messageStr : viewModel.ShowMessage);

                    control.Text = text;

                    control.BackColor = color;

                });
            }
        }

        public void OpenSetEquipmentFrom(Button btn)
        {
            SetEquipment _setEquipment = new SetEquipment();

            _setEquipment.Tag = new DeviceToNickName() { Device = btn.Name, NickName = btn.Text };

            _setEquipment.StartPosition = FormStartPosition.CenterParent;

            _setEquipment._refreshControl += new SetEquipment.RefreshControl((text) =>
            {
                ///更新设备按钮中文本
                btn.Text = text;

            });

            ShowForm(_setEquipment);
        }

        public void DymicAddEquipment(IList<string> deviceList)
        {

            if (null == deviceList || deviceList.Count == 0)
            {
                return;
            }

            deviceList.ToList().ForEach((item) =>
            {
                var returnData = baseAction.Devices.Any(o => o.Equals(item));

                if (!returnData)
                {
                    var returnList = SingleHepler<DeviceToNickNameBLL>.Instance.GetList(new DeviceToNickNameViewModel() { Device = item });

                    var currentModel = new DeviceToNickNameViewModel() { Device = item, NickName = item };

                    if (null != returnList && returnList.Count > 0)
                    {
                        currentModel.NickName = returnList.FirstOrDefault().NickName;
                    }

                    var currentControl = CreateEquipmentMapToPC(currentModel, new MouseEventHandler(_picBox_MouseDoubleClick));

                    baseAction.Devices.Add(item);

                    if (!baseAction.Dic.Value.Keys.Contains(item))
                    {

                        baseAction.Dic.Value.Add(item, currentControl);

                    }

                    baseAction.MaxPort++;

                    baseAction._tagTaskState(new UpdateWithTaskStateViewModel() { EquipmentDevice = item, TaskStatus = EnumTaskState.Start });

                    baseAction.SingleShowImageWithSocket(currentControl, baseAction.MaxPort, (device) =>
                    {

                        baseAction._tagTaskState(new UpdateWithTaskStateViewModel() { EquipmentDevice = device, TaskStatus = EnumTaskState.Complete });

                    });


                }

            });
        }


        #endregion

        private void 主播ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            KSActionForm form = new KSActionForm();

            form.Text = (sender as ToolStripMenuItem).Text;

            form.Tag = EnumSendType.HandSend;

            ShowForm(form);
        }
    }
}
