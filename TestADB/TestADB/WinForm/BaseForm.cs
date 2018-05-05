using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using GroupControl.Model;
using GroupControl.Common;
using GroupControl.Helper;
using GroupControl.BLL;

using System.Runtime.InteropServices;
using System.Windows.Forms.VisualStyles;
using ContentAlignment = System.Drawing.ContentAlignment;
using GroupControl.WinForm.CustomControl;

namespace GroupControl.WinForm
{
    public partial class BaseForm : Form
    {

        private const string tips = "当前还有未执行完毕的任务，请查看历史任务列表，获取详细信息！";

        protected static readonly object _taskLockObject = new object();

        protected BaseAction baseAction;

        protected string partialControlName = "{0}_Picture";

        protected static string _currentMACAddress = GroupControl.WinForm.Properties.Resources.Code;

        protected static ComputerInfo _currentComputerInfo;

        protected static Queue<AutoServiceInfoViewModel> _taskQueue = new Queue<AutoServiceInfoViewModel>();

        protected static List<string> infoList = new List<string> {
            "* daemon not running. starting it now at tcp:5037 *",
            "* daemon not running. starting it now on port 5037 *",
            "* daemon started successfully *",
            "List of devices attached"
        };


        ///任务完成 跟新数据库任务状态
        protected Func<int, Task<ExcuteResultViewModel>> _currentAction = (id) =>
         {

             return Task.Factory.StartNew<ExcuteResultViewModel>(() =>
             {

                 var currentTaskModel = SingleHepler<AutoServiceInfoBLL>.Instance.GetModel(id);

                 if (null != currentTaskModel)
                 {
                     currentTaskModel.Status = EnumTaskStatus.End;

                     return SingleHepler<AutoServiceInfoBLL>.Instance.Update(currentTaskModel);
                 }

                 return new ExcuteResultViewModel() { ResultID = 0, ResultStatus = EnumStatus.Error };

             });

         };

        protected IList<DeviceToNickNameViewModel> DevicesWithNickList { get; set; }

        public BaseForm()
        {

            this.MaximizeBox = false;

            this.MinimizeBox = false;

            baseAction = SingleHepler<BaseAction>.Instance;

            InitializeComponent();

            this.FormClosed += (sender, e) =>
            {
                baseAction.CheckSomeEquipments.Value.ToList().ForEach(o => o.Value.Checked = false);

                ///点选单次有效
                baseAction.CheckSomeEquipments.Value.Clear();

                DisposeControl();

            };
        }


        #region 内部方法

        /// <summary>
        /// 获取选中设备名称 就是设备号
        /// </summary>
        /// <param name="parentControl"></param>
        /// <returns></returns>
        public IList<string> GetCheckedEquipmentName(Control parentControl)
        {
            var list = new List<string>();

            var controls = parentControl.Controls;

            foreach (Control item in controls)
            {

                var currentRadioButtonControl = item as RadioButton;

                if (null != currentRadioButtonControl && currentRadioButtonControl.Checked)
                {
                    list.Add(item.Name);

                    continue;
                }

                var currentCheckBoxControl = item as CheckBox;

                if (null != currentCheckBoxControl && currentCheckBoxControl.Checked)
                {
                    list.Add(item.Name);
                }

            }

            return list;
        }

        /// <summary>
        /// 获取正常运行的设备 在添加任务的时候 进行关联
        /// </summary>
        public void GetDeviceWithXCheckBox(Control parentControl)
        {
            Task.Factory.StartNew(() =>
            {

                var devicesWithNickList = GetDeviceToNickNameUnionEquipments();

                if (devicesWithNickList!=null && devicesWithNickList.Count>0)
                {
                    devicesWithNickList.ToList().ForEach(o =>
                    {

                        baseAction.SetContentWithCurrentThread<DeviceToNickNameViewModel>(parentControl, o, (control, device) =>
                        {
                            CheckBox _checkBox = new CheckBox();

                            _checkBox.Text = device.NickName;

                            _checkBox.Name = device.Device;

                            _checkBox.TextAlign = ContentAlignment.MiddleLeft;

                            control.Controls.Add(_checkBox);

                        });

                    });
                }

            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async void CheckSomeEquipmentToDO(Control parentControl, DeviceToNickNameViewModel viewModel=null)
        {
            await ShowCurrentEquipmentList(parentControl, viewModel);

            if (!parentControl.HasChildren)
            {
                return;
            }

            var childrenList = parentControl.Controls;

            foreach (CheckBox item in childrenList)
            {
                if (baseAction.CheckSomeEquipments.Value.ContainsKey(item.Name))
                {
                    item.Checked = true;
                }
            }
        }

        public Task ShowCurrentEquipmentList(Control parentControl, DeviceToNickNameViewModel viewModel = null, IList<string> equipmentList = null)
        {
           return Task.Factory.StartNew(() =>
            {
                var devices = baseAction.Devices;

                if (null == viewModel)
                {
                    viewModel = new DeviceToNickNameViewModel();
                }

                if (null != equipmentList && equipmentList.Count > 0)
                {
                    devices = equipmentList.Where(o => devices.Contains(o)).ToList();
                }

                var devicesWithNickList = SingleHepler<DeviceToNickNameBLL>.Instance.GetEquipmentListWithGroupInfo(new DeviceToNickNameViewModel() { Devices = devices });

                ShowCheckBoxWithEquipmentList(parentControl, devicesWithNickList, viewModel);

            });

        }

        protected IList<DeviceToNickNameViewModel> GetDeviceToNickNameUnionEquipments()
        {
            if (null == baseAction.Devices || baseAction.Devices.Count == 0)
            {
                return default(IList<DeviceToNickNameViewModel>);
            }

            return SingleHepler<DeviceToNickNameBLL>.Instance.GetEquipmentListWithGroupInfo(new DeviceToNickNameViewModel() { Devices = baseAction.Devices });

            //if (null != devicesWithNickList && devicesWithNickList.Count > 0)
            //{

            //    devicesWithNickList = baseAction.Devices.Select(o =>
            //    {
            //        var matchList = devicesWithNickList.Where(q => q.Device == o).ToList();

            //        if (null != matchList && matchList.Count > 0)
            //        {
            //            return matchList.FirstOrDefault();
            //        }

            //        return new DeviceToNickName() { NickName = o, Device = o };

            //    }).ToList();

            //    return devicesWithNickList;
            //}


            //return baseAction.Devices.Select(o =>
            //{
            //    return new DeviceToNickName() { NickName = o, Device = o };

            //}).ToList();
        }

        protected Control GetControlByName(Control parentControl, string name)
        {
            var boxs = parentControl.Controls;

            if (null != boxs && boxs.Count > 0)
            {
                foreach (Control item in boxs)
                {
                    if (item.Name == name)
                    {
                        return item;
                    }
                }
            }

            return default(Control);

        }

        protected void ShowCheckBoxWithEquipmentList(Control parentControl, IList<DeviceToNickNameViewModel> list, DeviceToNickNameViewModel viewModel)
        {
            if (null == list || list.Count == 0)
            {
                return;
            }

            list.ToList().ForEach(o =>
            {

                baseAction.SetContentWithCurrentThread<DeviceToNickNameViewModel>(parentControl, o, (control, device) =>
                {
                    CheckBox _checkBox = new CheckBox();

                    var text = device.NickName;

                    Action<CheckBox> action = (p) =>
                    {

                        var currendBox = p as CheckBox;

                        currendBox.Font = new Font(currendBox.Font, currendBox.Font.Style | FontStyle.Bold);

                        currendBox.ForeColor = Color.Black;

                    };

                    if (!string.IsNullOrEmpty(device.GroupName))
                    {
                        text = string.Format("{0}({1})", device.NickName, device.GroupName);

                        if (device.GroupID == viewModel.GroupID && viewModel.GroupID > 0)
                        {
                            action(_checkBox);
                        }
                        else
                        {
                            _checkBox.ForeColor = Color.DarkGray;
                        }

                    }
                    else
                    {
                        _checkBox.ForeColor = Color.OrangeRed;
                    }

                    _checkBox.Text = text;

                    _checkBox.Name = device.Device;

                    _checkBox.TextAlign = ContentAlignment.MiddleLeft;

                    if (device.GroupID == viewModel.GroupID && viewModel.GroupID > 0)
                    {
                        _checkBox.Checked = true;
                    }

                    if (device.Device.Equals(viewModel.Device))
                    {
                        action(_checkBox);
                    }

                    control.Controls.Add(_checkBox);

                });

            });
        }

        protected void InitGroup(Control parentControl, Func<Control, GroupInfoViewModel, Control> func = null, IList<GroupInfoViewModel> devices = null, bool isShow = false)
        {
            Task.Factory.StartNew(() =>
            {
                var list = SingleHepler<GroupInfoBLL>.Instance.GetGroupListWithDevice(new BaseViewModel());

                if (null != list && list.Count > 0)
                {
                    list.ToList().ForEach((item) =>
                    {
                        baseAction.SetContentWithCurrentThread<GroupInfoViewModel>(parentControl, item, (control, group) =>
                        {
                            var equipmentCount = item.GroupToDeviceList?.Count;

                            Control _currentControl;

                            var currentEqipmentList = item.GroupToDeviceList?.Where(o =>
                            {

                                return baseAction.Devices.Any(q => q.Equals(o.Device));

                            }).ToList();

                            if ((null == currentEqipmentList || currentEqipmentList.Count == 0) && !isShow)
                            {
                                return;
                            }

                            Func<Control> currentFunc = () =>
                            {
                                var radio = new RadioButton();

                                return radio;
                            };

                            if (null != func)
                            {
                                _currentControl = func.Invoke(control, group);
                            }
                            else
                            {
                                _currentControl = currentFunc.Invoke();
                            }

                            _currentControl.Text = string.Format("{0}({2}/{1})", group.GroupInfoModel.Name, equipmentCount, currentEqipmentList?.Count());

                            _currentControl.Name = Name = group.GroupInfoModel.ID.ToString();

                            control.Controls.Add(_currentControl);

                        });

                    });
                }


            });
        }

        public void InsertTaskModelToQueue(AutoServiceInfoViewModel viewModel)
        {
            var currentViewModel = viewModel;

            ///获取当前任务关联的设备
            var modelList = SingleHepler<AutoServiceInfoBLL>.Instance.GetTaskAboutDeviceList(new AutoServiceInfoViewModel() { ID = currentViewModel.ID });

            if (null != modelList && modelList.Count > 0)
            {
                currentViewModel.Devices = modelList.Select(o => o.Device).ToList();
            }

            ///任务入队
            lock (_taskLockObject)
            {
                _taskQueue.Enqueue(currentViewModel);
            }
        }

        protected void ShowForm(Form form)
        {
            form.StartPosition = FormStartPosition.CenterParent;

            form.ShowDialog();
        }


        /// <summary>
        /// 设置文本框的光标
        /// </summary>
        /// <param name="textBox"></param>
        /// <param name="str"></param>
        public void SetTextBoxCursor(TextBox textBox, string str)
        {
            baseAction.SetContentWithCurrentThread<string>(textBox, str, (control, text) =>
            {

                var currentControl = control as TextBox;

                SetTextBoxCursorWithNoDelegate(currentControl, text);

            });
        }

        public void SetTextBoxCursorWithNoDelegate(TextBox currentControl, string str)
        {
          
            currentControl.AppendText(string.Format("{0}\r\n\r\n", str));     // 追加文本，并且使得光标定位到插入地方。

            currentControl.ScrollToCaret();

            currentControl.Focus();//获取焦点

            currentControl.Select(currentControl.TextLength, 0);//光标定位到文本最后

            currentControl.ScrollToCaret();//滚动到光标处
        }


        /// <summary>
        /// 根据分组获取设备
        /// </summary>
        /// <param name="grouplist"></param>
        /// <returns></returns>

        public IList<string> GetDeviceByGroups(IList<string> grouplist)
        {
            if (null != grouplist && grouplist.Count > 0)
            {
                var returnData = SingleHepler<GroupInfoBLL>.Instance.GetGroupToDeviceList(new DeviceToNickNameViewModel() { Devices = grouplist });

                if (null != returnData && returnData.Count > 0)
                {
                    return returnData.Select(o => o.Device).ToList();
                }
            }

            return default(List<string>);
        }


        /// <summary>
        ///  DataGridView自适应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DataGridViewAutoSize(DataGridView view)
        {

            var width = 0;

            //对于DataGridView的每一个列都调整
            for (int i = 0; i < view.Columns.Count; i++)
            {
                view.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;

                //将每一列都调整为自动适应模式
                view.AutoResizeColumn(i, DataGridViewAutoSizeColumnMode.AllCells);
                //记录整个DataGridView的宽度
                width += view.Columns[i].Width;
            }
            //判断调整后的宽度与原来设定的宽度的关系，如果是调整后的宽度大于原来设定的宽度，
            //则将DataGridView的列自动调整模式设置为显示的列即可，
            //如果是小于原来设定的宽度，将模式改为填充。
            if (width > view.Size.Width)
            {
                view.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCellsExceptHeader;
            }
            else
            {
                view.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }

            view.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            #region 设置单元格内容居中

            DataGridViewCellStyle cellStyle = new DataGridViewCellStyle();

            cellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;

            view.DefaultCellStyle = cellStyle;

            #endregion

        }

        #endregion

        private void BaseForm_Load(object sender, EventArgs e)
        {
            this.SetStyle(ControlStyles.DoubleBuffer, true);

            this.SetStyle(ControlStyles.UserPaint, true);

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);

            // skinEngine1.SkinFile = Application.StartupPath + @"\Page.ssk";
        }



        /// <summary>
        /// 创建任务 入库 入队
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="func"></param>
        public void CreateTaskToQueue(AutoServiceInfoViewModel viewModel, Action func = null)
        {
            Task.Factory.StartNew<AutoServiceInfoViewModel>((obj) =>
            {

                func?.Invoke();

                var model = obj as AutoServiceInfoViewModel;

                if (null != _currentComputerInfo)
                {
                    model.AutoServiceInfoModel.ComputerID = _currentComputerInfo.ID;
                }

                if ((null == viewModel.Devices || viewModel.Devices.Count == 0) && (null == viewModel.GroupIDs || viewModel.GroupIDs.Count == 0))
                {
                    viewModel.Devices = baseAction.Devices;
                }

                var returnData = SingleHepler<AutoServiceInfoBLL>.Instance.AddFriendTask(model);

                model.ID = returnData;

                return model;

            }, viewModel).ContinueWith((task) =>
            {

                if (task.Result.AutoServiceInfoModel.SendType == EnumSendType.HandSend)
                {

                    InsertTaskModelToQueue(task.Result);

                }

            });
        }

        protected void DisposeControl()
        {
            var controls = this.Controls;

            if (null != controls && controls.Count > 0)
            {
                foreach (Control item in controls)
                {
                    baseAction.SetContentWithCurrentThread<object>(item, null, (control, obj) =>
                    {

                        control.Dispose();

                    });
                }
            }

            this.Dispose();
        }

        public void CheckAllAndUpdateUI(Control parentControl, CheckBox actonControl = null, bool checkState = false)
        {
            var currentParentControl = parentControl;

            var childControl = currentParentControl.Controls;

            var isCheck = checkState;

            if (null != actonControl)
            {
                isCheck = actonControl.Checked;
            }

            if (null != childControl && childControl.Count > 0)
            {
                foreach (Control o in childControl)
                {
                    var currentChildControl = o as CheckBox;

                    currentChildControl.Checked = isCheck;

                }
            }
        }


        /// <summary>
        /// 生成Button
        /// </summary>
        /// <param name="parentControl"></param>
        /// <param name="text"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public Button CreateActionBtn(Control parentControl, string text, int width = 0,bool isRound=false)
        {

            var currentWidth = width;

            if (currentWidth == 0)
            {
                currentWidth = parentControl.Width;
            }

            var foreColor = Color.WhiteSmoke;

            var backColor = Color.Black ;

            var btn = new Button() { Text = text,  ForeColor = foreColor, BackColor = backColor, Width = currentWidth, FlatStyle = FlatStyle.Flat, FlatAppearance = { BorderSize = 0 } };


            ///圆角button
            if (isRound)
            {
                btn = new RoundButton() { Text = text,Width = currentWidth, FlatStyle = FlatStyle.Flat, FlatAppearance = { BorderSize = 0 } };
            }

            parentControl.Controls.Add(btn);

            return btn;

        }

        protected Panel GetPictureByName(Control parentControl, string name, Func<string, Control, bool> action = null)
        {
            try
            {
                var boxs = parentControl.Controls;

                var currentBox = new Panel();

                if (null != boxs && boxs.Count > 0)
                {
                    foreach (var item in boxs)
                    {
                        Panel _box = item as Panel;

                        var returnData = action?.Invoke(name, _box);

                        if ((null != returnData && returnData.Value) || _box.Name == name)
                        {
                            currentBox = _box;

                            break;
                        }

                    }
                }

                return currentBox;
            }
            catch (Exception ex)
            {

                //throw;
            }

            return default(Panel);
        }

        public void CustomBtnUI(Button btnButton)
        {
            btnButton.ForeColor = Color.WhiteSmoke;

            btnButton.BackColor = Color.Gray;

            btnButton.FlatStyle = FlatStyle.Flat;

            btnButton.FlatAppearance.BorderSize = 0;

            btnButton.Size = new Size(102, 23);

            btnButton.Margin = new Padding(5);
        }


        public void InitDataGridView(DataGridView gridView, Dictionary<string, string> headerDic)
        {
            foreach (var item in headerDic.Keys)
            {
                gridView.Columns.Add(item, headerDic[item]);
            }

            DataGridViewAutoSize(gridView);
        }


        public NumericUpDown CustomNumericUpDown(NumericUpDown currentNumericUpDown, int defaultValue, int maxiMum, int increment, Action<object, EventArgs> action)
        {
            currentNumericUpDown.Minimum = 1;

            currentNumericUpDown.Maximum = maxiMum;   //设置允许的最大值 

            currentNumericUpDown.DecimalPlaces = 0;

            currentNumericUpDown.Increment = increment;       //设置步长为1

            currentNumericUpDown.TextAlign = HorizontalAlignment.Center;

            currentNumericUpDown.ReadOnly = true;

            currentNumericUpDown.Value = defaultValue;

            currentNumericUpDown.ValueChanged += (o, e) =>
            {

                action(o, e);


            };

            return currentNumericUpDown;
        }

        protected void AddRowToDataGrridView<T>(T model, DataGridView gridView, Action<DataGridViewRow, T> action) where T : class
        {
            baseAction.SetContentWithCurrentThread<T>(gridView, model, (control, currentModel) =>
            {
                DataGridView currentControl = control as DataGridView;

                currentControl.Controls.Clear();

                int index = currentControl.Rows.Add();

                action(currentControl.Rows[index], model);

            });
        }

        protected void CustomDialog(string message = tips, Action action = null,Action cancleAction=null)
        {
            var btns = action == null ? MessageBoxButtons.OK : MessageBoxButtons.OKCancel;

            DialogResult result = MessageBox.Show(message, "提示信息", btns);

            ///表示确定
            if (result == DialogResult.OK)
            {
                action?.Invoke();
            }
            else if(result==DialogResult.Cancel)
            {
                cancleAction?.Invoke();
            }

        }

        /// <summary>
        /// 取消当前正在执任务的设备
        /// </summary>
        protected async Task CancelCurrentTask()
        {
            await Task.Factory.StartNew(() =>
            {
                if (null != baseAction.Devices && baseAction.Devices.Count > 0)
                {
                    foreach (var device in baseAction.Devices)
                    {
                        CancleTask(device);

                    }
                }
            });

        }

        protected void CancleTask(string device)
        {
            var viewModel = new UpdateWithTaskStateViewModel()
            {
                EquipmentDevice = device,

                TaskStatus = EnumTaskState.BeingCancelled
            };

            var currentTaskState = baseAction.CheckTaskState(device);

            if (currentTaskState != EnumTaskState.Cancle && currentTaskState != EnumTaskState.Complete)
            {
                baseAction._tagTaskState(viewModel);

                var returnData= baseAction.SingleSetTaskState(device);

                viewModel.TaskStatus = returnData;

                baseAction._tagTaskState(viewModel);

                //if (returnData != EnumTaskState.Cancle)
                //{
                    ///viewModel.TaskStatus = returnData;

                    ///baseAction._tagTaskState(viewModel);
                //}
            }

        }

    }

}
