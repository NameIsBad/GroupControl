using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using GroupControl.Model;

namespace TestADB
{
    public partial class QueryFansForm : BaseForm
    {
        public QueryFansForm()
        {
            InitializeComponent();
        }

        private void queryFansBtn_Click(object sender, EventArgs e)
        {
            if (_taskQueue.Count > 0)
            {
                CustomDialog();

                return;
            }

            var viewModel = new AutoServiceInfoViewModel()
            {
                AutoServiceInfoModel = new AutoServiceInfo()
                {
                    ServiceType = EnumTaskType.QueryFansCount,
                    StartDate =DateTime.Now,
                    Status = EnumTaskStatus.Executing,
                    SendType = EnumSendType.HandSend
                }
            };

            viewModel.Devices = GetCheckedEquipmentName(this.flowLayoutPanelWithEquipment);

            viewModel.GroupIDs = GetCheckedEquipmentName(this.flowLayoutPanelWithGroup);

            CreateTaskToQueue(viewModel);

            this.Close();
        }

        private void QueryFansForm_Load(object sender, EventArgs e)
        {
            CheckSomeEquipmentToDO(this.flowLayoutPanelWithEquipment);

            InitGroup(this.flowLayoutPanelWithGroup);
        }
    }
}
