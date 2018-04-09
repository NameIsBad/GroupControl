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
using GroupControl.Helper;
using GroupControl.Common;
using GroupControl.Model;

namespace TestADB
{
    public partial class AddGroupForm : BaseForm
    {

        private GroupInfo _currentGroupInfo;

        public AddGroupForm()
        {
            InitializeComponent();
        }

        private void AddGroupForm_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            this.Dispose();

            if (null!= _currentGroupInfo)
            {
                SetDeviceToGroupForm setDeviceToGroupForm = new SetDeviceToGroupForm();

                setDeviceToGroupForm.Tag = _currentGroupInfo;

                setDeviceToGroupForm.ShowDialog();
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            var name = this.textBox1.Text;

            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("分组名称不能为空!");

                return;
            }

            Task.Factory.StartNew<ExcuteResultViewModel>(() =>
            {
               return  SingleHepler<GroupInfoBLL>.Instance.Insert(new GroupInfo() { CreateDate=DateTime.Now, Name=name});

            }).ContinueWith((task)=> {

                var returnData = task.Result;

                if (returnData.ResultStatus == EnumStatus.Success)
                {

                    _currentGroupInfo = new GroupInfo() { ID=returnData.ResultID,Name=this.textBox1.Text};

                    SingleHepler<BaseAction>.Instance.SetContentWithCurrentThread<object>(this, null, (form, str) =>
                    {

                        (form as Form).Close();

                    });
                }

            });
        }
    }
}
