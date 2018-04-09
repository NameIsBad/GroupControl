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
using System.Threading;

namespace GroupControl.WinForm
{
    public partial class SetEquipment : BaseForm
    {

        private DeviceToNickName _currentEquipment;

        public delegate void RefreshControl(string text);

        public event RefreshControl _refreshControl;

        public SetEquipment()
        {
            InitializeComponent();

            this.Text = "修改设备别名";
        }



        /// <summary>
        /// 设置别名
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSetEquipmentName_Click(object sender, EventArgs e)
        {
            try
            {

                var nickName = this.textEquipmentNick.Text;

                var device = this.textEquipmentName.Text;

                if (string.IsNullOrEmpty(nickName))
                {
                    MessageBox.Show("设备别名不能为空！");

                    return;
                }

                nickName = nickName.Split(' ')[0];

                var bll = SingleHepler<DeviceToNickNameBLL>.Instance;

                var list = bll.GetList(new DeviceToNickNameViewModel() { Device = device });


                if (null != list && list.Count > 0)
                {
                    var currentModel = list.FirstOrDefault();

                    currentModel.NickName = nickName;

                    bll.Update(currentModel);
                }
                else
                {
                    bll.Insert(new DeviceToNickName() { Device = device, NickName = nickName, CreateDate = DateTime.Now, FriendCount = 0 });
                }

                this.Close();

                this.Dispose();

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private void SetEquipment_Load(object sender, EventArgs e)
        {

            _currentEquipment = this.Tag as DeviceToNickName;

            this.textEquipmentName.Text = _currentEquipment.Device;

            this.textEquipmentNick.Text = string.IsNullOrEmpty(_currentEquipment.NickName) ? _currentEquipment.Device : _currentEquipment.NickName.Split(' ')[0];
        }

        private void SetEquipment_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {

            ///防止事件在执行的之前 被其他的线程移除掉委托
            var temp = Volatile.Read(ref _refreshControl);

            if (null != temp)
            {
                temp(this.textEquipmentNick.Text);
            }
        }
    }
}
