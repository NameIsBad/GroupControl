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
using GroupControl.BLL;
using GroupControl.Helper;

namespace GroupControl.WinForm
{
    public partial class SearchFriendFrom : BaseForm
    {
        private EnumCheckSexType _currentSex = EnumCheckSexType.Other;

        public SearchFriendFrom()
        {
            InitializeComponent();
        }

        private void SearchFriendFrom_Load(object sender, EventArgs e)
        {
            ///初始化设备
            CheckSomeEquipmentToDO(this.flowLayoutPanelWithEquipment);

            InitGroup(this.flowLayoutPanelWithGroup);
        }

        public void CheckedChanged(object sender, EventArgs e)
        {
            var radioButton = sender as RadioButton;

            _currentSex = (EnumCheckSexType)Convert.ToInt32(radioButton.Tag);
        }

        private void button1_Click(object sender, EventArgs e)
        {

            var searchContentList = ReadSearchContentFromText();

            if (null== searchContentList || searchContentList.Count==0)
            {
                MessageBox.Show("请输入要搜索的内容，一行一个");

                return;
            }

            var viewModel = new AutoServiceInfoViewModel()
            {
                AutoServiceInfoModel = new AutoServiceInfo()
                {
                    ServiceType = EnumTaskType.SearchAddFriend,
                    StartDate = DateTime.Now,
                    AddCount = 0,
                    RemarkContent = string.IsNullOrEmpty(this.remarkText.Text) ? "搜索加人" : this.remarkText.Text,
                    SayHelloContent = string.IsNullOrEmpty(this.sayHelloText.Text) ? "你好！": this.sayHelloText.Text,
                    Status = EnumTaskStatus.Executing,
                    SendType = EnumSendType.HandSend,
                    Sex = _currentSex,
                    IsStatisticsFriendCount = false
                }
            };

            viewModel.Devices = GetCheckedEquipmentName(this.flowLayoutPanelWithEquipment);

            viewModel.GroupIDs = GetCheckedEquipmentName(this.flowLayoutPanelWithGroup);

            viewModel.SearchContents = searchContentList;

            CreateTaskToQueue(viewModel);

            this.Close();
        }

        /// <summary>
        /// 读取富文本中的搜索内容
        /// </summary>
        public IList<string> ReadSearchContentFromText()
        {
            var text = this.searchText.Text;

            var strList=new  List<string>();

            if (string.IsNullOrEmpty(text))
            {
                return default(List<string>);
            }

            var lines= this.searchText.Lines;

            foreach (var line in lines)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    strList.Add(line);
                }
            }

            return strList;

        }

    }
}
