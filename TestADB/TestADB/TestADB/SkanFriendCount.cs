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
using GroupControl.Common;
using GroupControl.Helper;
using GroupControl.BLL;

namespace TestADB
{
    public partial class SkanFriendCount : BaseForm
    {
        public SkanFriendCount()
        {
            InitializeComponent();
        }

        private void SkanFriendCount_Load(object sender, EventArgs e)
        {

            InitDataGridView(this.fiendListDataView, new Dictionary<string, string>() {

                { "设备号别名","设备号别名"},

                { "设备号","设备号"},

                { "粉丝数量","粉丝数量"},

                {"查看时间","查看时间" }

            });

            Task.Factory.StartNew<IList<DeviceToNickName>>(() =>
            {
                var list = SingleHepler<DeviceToNickNameBLL>.Instance.GetEquipmentFriendCountList(baseAction.Devices);

                return list;

            }).ContinueWith((task) =>
            {

                var returnData = task.Result;

                if (null != returnData && returnData.Count > 0)
                {
                    returnData.ToList().ForEach((item) =>
                    {
                        AddRowToDataGrridView(item, this.fiendListDataView, (c, o) =>
                        {

                            c.Cells[0] = new DataGridViewTextBoxCell() { Value = o.NickName };
                            c.Cells[1] = new DataGridViewTextBoxCell() { Value = o.Device };
                            c.Cells[2] = new DataGridViewTextBoxCell() { Value = o.FriendCount.ToString() };
                            c.Cells[3] = new DataGridViewTextBoxCell() { Value = o.CreateDate.ToString("yyyy-MM-dd HH:ss") };

                        });

                    });
                }


            });
        }


    }
}
