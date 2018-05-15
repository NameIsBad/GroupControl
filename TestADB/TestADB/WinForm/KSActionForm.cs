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

namespace GroupControl.WinForm
{
    public partial class KSActionForm : BaseForm
    {
        private DateTime _startDate;

        private DateTime _startTime;

        private EnumSendType _currentSendType;

        public KSActionForm()
        {
            InitializeComponent();
        }

        private void KSActionForm_Load(object sender, EventArgs e)
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

            this.KSAction.Text = _currentSendType == EnumSendType.AutoSend ? "提交任务" : "开始";

            if (_currentSendType == EnumSendType.HandSend)
            {
                SetStartDate.Hide();
            }

            ///初始化设备
            CheckSomeEquipmentToDO(this.flowLayoutPanelWithEquipment);

            InitGroup(this.flowLayoutPanelWithGroup);
        }

        private void KSActionForm_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            DisposeControl();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenLive();

           // this.Close();
        }


        /// <summary>
        /// 普通操作
        /// </summary>
        public void GeneralAction()
        {
            var startDate = DateTime.Parse(string.Format("{0} {1}", _startDate.ToString("yyyy-MM-dd"), _startTime.ToString("HH:mm")));

            var viewModel = new AutoServiceInfoViewModel()
            {
                AutoServiceInfoModel = new AutoServiceInfo()
                {
                    ServiceType = EnumTaskType.KSAction,

                    StartDate = startDate,

                    Status = _currentSendType == EnumSendType.AutoSend ? EnumTaskStatus.Start : EnumTaskStatus.Executing,

                    SendType = _currentSendType,

                    SayHelloContent = this.commentText.Text,

                    RemarkContent = this.photosText.Text
                }
            };

            viewModel.Devices = GetCheckedEquipmentName(this.flowLayoutPanelWithEquipment);

            viewModel.GroupIDs = GetCheckedEquipmentName(this.flowLayoutPanelWithGroup);

            CreateTaskToQueue(viewModel);
        }


        /// <summary>
        /// 打开主播房间
        /// </summary>
        public void OpenLive()
        {
            var ksViewModel = new KSViewModel() { PhotoIDs= this.photosText.Text};

            if (_taskQueue.Count > 0)
            {
                CustomDialog();

                return;
            }

            if (baseAction.Devices == null || baseAction.Devices.Count == 0)
            {
                return;
            }

            var checkList = GetCheckedEquipmentName(this.flowLayoutPanelWithEquipment);

            if (checkList==null || checkList.Count==0)
            {
                checkList = baseAction.Devices;
            }

            if (ksViewModel.PhotoIDList == null || ksViewModel.PhotoIDList.Count()==0)
            {
                return;
            }

            Task.Factory.StartNew(() =>
            {
                var index = 0;

                if (checkList.Count >= ksViewModel.PhotoIDList.Count)
                {
                    ksViewModel.PhotoIDList.ToList().ForEach(o =>
                    {
                        Task.Factory.StartNew((i) =>
                        {
                            var currentIndex = int.Parse(i.ToString());

                            SingleHepler<KSHelper>.Instance.SingleOpenLive(o, checkList.Skip(currentIndex).Take(1).FirstOrDefault());

                        },index);

                        index++;
                    });
                }
                else
                {
                    checkList.ToList().ForEach(o =>
                    {
                        
                        Task.Factory.StartNew((i) =>
                        {
                            var currentIndex = int.Parse(i.ToString());

                            SingleHepler<KSHelper>.Instance.SingleOpenLive(ksViewModel.PhotoIDList.Skip(currentIndex).Take(1).FirstOrDefault(), o);

                        }, index);

                        index++;
                    });
                }

            });

        }
    }
}
