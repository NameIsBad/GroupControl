using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using GroupControl.BLL;
using GroupControl.Model;
using GroupControl.Common;
using GroupControl.Helper;

namespace TestADB
{
    public partial class TimerAddFriend : BaseForm
    {
        private DateTime _startDate;

        private DateTime _startTime;

        private EnumSendType _currentSendType;

        private string _sayHiContent = string.Empty;

        private string _remarkContent = string.Empty;

        private int _addFriendCount = 7;

        private EnumCheckSexType _currentSex = EnumCheckSexType.Other;

        public TimerAddFriend()
        {
            InitializeComponent();
        }

        private void TimerAddFriend_Load(object sender, EventArgs e)
        {
            this.startdate.CustomFormat = "yyyy/MM/dd";
            this.startdate.Format = DateTimePickerFormat.Custom;
            _startDate = DateTime.Now;

            this.starttime.CustomFormat = "HH:mm";
            this.starttime.Format = DateTimePickerFormat.Custom;
            this.starttime.ShowUpDown = true;
            _startTime = DateTime.Now;

            baseAction = SingleHepler<BaseAction>.Instance;

            _currentSendType = (EnumSendType)Convert.ToInt32(this.Tag);

            this.AddFriend.Text = _currentSendType == EnumSendType.AutoSend ? "提交任务" : "开始";

            if (_currentSendType==EnumSendType.HandSend)
            {
                SetStartDate.Hide();
            }


            CustomNumericUpDown(this.addFriendCount, _addFriendCount, 10, 1, (o, s) =>
            {
                var control = (NumericUpDown)o;

                _addFriendCount = (int)control.Value;

            });

            this.txtSayHello.Text = "你好！很高兴认识你";

            ///初始化设备
            CheckSomeEquipmentToDO(this.flowLayoutPanelWithEquipment);

            InitGroup(this.flowLayoutPanelWithGroup);

        }

        private void TimerAddFriend_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            DisposeControl();
        }


        public void CheckedChanged(object sender,EventArgs e)
        {
            var radioButton = sender as RadioButton;

            _currentSex = (EnumCheckSexType)Convert.ToInt32(radioButton.Tag);
        }

        private void TimePicker_ValueChanged(object sender, EventArgs e)
        {
            var dateTimePicker = sender as DateTimePicker;

            switch (dateTimePicker.Name.ToLower())
            {
                case "startdate":

                    _startDate = dateTimePicker.Value;

                    break;
                case "starttime":

                    _startTime = dateTimePicker.Value;

                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// 添加好友
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddFriend_Click(object sender, EventArgs e)
        {

            _remarkContent = this.txtRemarkContent.Text;

            _sayHiContent = this.txtSayHello.Text;

            var startDate = DateTime.Parse(string.Format("{0} {1}", _startDate.ToString("yyyy-MM-dd"), _startTime.ToString("HH:mm")));

            var viewModel = new AutoServiceInfoViewModel()
            {
                AutoServiceInfoModel = new AutoServiceInfo()
                {
                    ServiceType = EnumTaskType.AddFriend,

                    StartDate = startDate,

                    AddCount = _addFriendCount,

                    RemarkContent = _remarkContent,

                    SayHelloContent = _sayHiContent,

                    Status = _currentSendType == EnumSendType.AutoSend ? EnumTaskStatus.Start : EnumTaskStatus.Executing,

                    SendType = _currentSendType,

                    Sex = _currentSex
                }
            };

            viewModel.Devices = GetCheckedEquipmentName(this.flowLayoutPanelWithEquipment);

            viewModel.GroupIDs = GetCheckedEquipmentName(this.flowLayoutPanelWithGroup);

            CreateTaskToQueue(viewModel);

            this.Close();
        }

    }
}
