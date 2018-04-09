using GroupControl.Common;
using GroupControl.Helper;
using GroupControl.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestADB
{
    public partial class SendGroupMessageForm : BaseForm
    {

        private DateTime _startDate;

        private DateTime _startTime;

        private EnumSendType _currentSendType;

        private WXHelper wxHelper;


        public SendGroupMessageForm()
        {
            InitializeComponent();
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

        private void SendGroupMessageForm_Load(object sender, EventArgs e)
        {
            this.startdate.CustomFormat = "yyyy/MM/dd";
            this.startdate.Format = DateTimePickerFormat.Custom;
            _startDate = DateTime.Now;

            this.starttime.CustomFormat = "HH:mm";
            this.starttime.Format = DateTimePickerFormat.Custom;
            this.starttime.ShowUpDown = true;
            _startTime = DateTime.Now;

            _currentSendType = ((EnumSendType)Convert.ToInt32(this.Tag));

            this.Send.Text = _currentSendType == EnumSendType.AutoSend ? "提交" : "发送";

            var isShow = _currentSendType == EnumSendType.AutoSend;

            if (!isShow)
            {
                this.SetStartDate.Hide();
            }

            wxHelper = SingleHepler<WXHelper>.Instance;

            CheckSomeEquipmentToDO(this.flowLayoutPanelWithEquipment);

            InitGroup(this.flowLayoutPanelWithGroup);
        }

        private void SendGroupMessageForm_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            DisposeControl();
        }

        private void Send_Click(object sender, EventArgs e)
        {
            try
            {
                ///添加任务记录
                var startDate = DateTime.Parse(string.Format("{0} {1}", _startDate.ToString("yyyy-MM-dd"), _startTime.ToString("HH:mm")));

                var viewModel = new AutoServiceInfoViewModel()
                {
                    AutoServiceInfoModel = new AutoServiceInfo()
                    {
                        SayHelloContent = this.textBox3.Text,
                        StartDate = startDate,
                        Status = _currentSendType == EnumSendType.HandSend ? EnumTaskStatus.Executing : EnumTaskStatus.Start,
                        ServiceType = EnumTaskType.SendGroupMessage,
                        SendType = _currentSendType
                    }
                };

                viewModel.Devices = GetCheckedEquipmentName(this.flowLayoutPanelWithEquipment);

                viewModel.GroupIDs = GetCheckedEquipmentName(this.flowLayoutPanelWithGroup);

                CreateTaskToQueue(viewModel);

                this.Close();
            }
            catch (Exception ex)
            {

            }
        }
    }
}
