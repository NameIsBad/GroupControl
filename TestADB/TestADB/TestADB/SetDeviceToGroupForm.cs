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
    public partial class SetDeviceToGroupForm : BaseForm
    {
        public SetDeviceToGroupForm()
        {
            InitializeComponent();
        }

        private int _currentGroupID = 0;

        private void SetDeviceToGroupForm_Load(object sender, EventArgs e)
        {

            this.ActiveControl = this.groupsPanel;

            ///初始化分组
            InitGroup(this.groupsPanel, (control, group) =>
            {

                var radio = new RadioButton();

                radio.Click += new EventHandler(GroupCheckChange);

                return radio;

            },isShow:true);

            ///初始化设备
            InitEquipment();

        }


        private void SetDeviceToGroupForm_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            // DisposeControl();
        }

        public void InitEquipment()
        {
            ShowCurrentEquipmentList(this.devicesPanel);
        }


        public void GroupCheckChange(object sender, EventArgs e)
        {
            var radioBtn = sender as RadioButton;

            int.TryParse(radioBtn.Name, out _currentGroupID);

            this.devicesPanel.Controls.Clear();


            //获取当前分组的设备
            ShowCurrentEquipmentList(this.devicesPanel, new DeviceToNickNameViewModel() { GroupID = _currentGroupID });


        }

        /// <summary>
        /// 添加设备到分组
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetDevicesToGroup_Click(object sender, EventArgs e)
        {
            if (_currentGroupID == 0)
            {
                MessageBox.Show("请选择分组！");

                return;
            }

            var selectEquipments = GetCheckedEquipmentName(this.devicesPanel);

            if (null == selectEquipments || selectEquipments.Count == 0)
            {
                MessageBox.Show("请选择设备！");

                return;
            }

            Task.Factory.StartNew(() =>
            {

                SingleHepler<GroupInfoBLL>.Instance.BatchInsertGroupToDevice(selectEquipments.Select(o =>
                {

                    return new GroupToDevice() { Device = o, GroupID = _currentGroupID };

                }).ToList());


            }).ContinueWith((task) =>
            {

                baseAction.SetContentWithCurrentThread<object>(this.devicesPanel, null, (control, obj) =>
                {
                    this.devicesPanel.Controls.Clear();

                });

                //获取当前分组的设备
                ShowCurrentEquipmentList(this.devicesPanel, new DeviceToNickNameViewModel() { GroupID = _currentGroupID });


            });
        }
    }
}
