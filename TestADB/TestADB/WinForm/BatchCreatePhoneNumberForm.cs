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
using GroupControl.Common;
using GroupControl.Helper;
using GroupControl.Model;
using GroupControl.Model;

namespace GroupControl.WinForm
{
    public partial class BatchCreatePhoneNumberForm : BaseForm
    {

        private static Dictionary<int, IList<string>> _dict = new Dictionary<int, IList<string>>{
            { 1,new List<string>() {"134","135","136","137","138","139","150","151","152","157","158","159","188" } },
            { 2, new List<string>() { "130","131","132","155","156"} },
            { 3, new List<string>() {"133","153","189" } } };
        private string _selectHeader = string.Empty;
        private int _selectType = 1;
        private string _selectAreaCode = string.Empty;

        public BatchCreatePhoneNumberForm()
        {
            InitializeComponent();
        }

        private void BatchCreatePhoneNumberForm_Load(object sender, EventArgs e)
        {
            this.flowLayoutPanel2.Controls.AddRange(_dict[1].ToList().Select(o => {
                RadioButton button= new RadioButton() { Text = o, TextAlign = ContentAlignment.MiddleLeft };
                button.Click +=(s,args)=> {
                    _selectHeader =(s as RadioButton).Text;
                    GetDataAndShow();
                };

                return button;
            }).ToArray());
            (this.flowLayoutPanel2.Controls[0] as RadioButton).Checked = true;

            Task.Factory.StartNew<IList<AreaInfo>>(() =>
            {
                var areaInfoList = SingleHepler<AreaInfoBLL>.Instance.GetList(null);
                if (areaInfoList != null && areaInfoList.Count > 0)
                {
                    baseAction.SetContentWithCurrentThread<IList<AreaInfo>>(this.listBox1, areaInfoList, (control, list) =>
                    {
                        this.listBox1.DataSource = list;
                        this.listBox1.DisplayMember = "Name";
                        this.listBox1.ValueMember = "Code";
                    });
                }

                return areaInfoList;

            }).ContinueWith(task =>
            {
                var list = task.Result;
                if (list == null || list.Count == 0)
                {
                    return;
                }
                var firstModel = list.FirstOrDefault();
                _selectAreaCode = firstModel.Code;
                _selectHeader = this.flowLayoutPanel2.Controls[0].Text;
                GetDataAndShow();
            });
        }

        /// <summary>
        /// 点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBox1_MouseCaptureChanged(object sender, EventArgs e)
        {
            var name = this.listBox1.SelectedItem;
            _selectAreaCode = this.listBox1.SelectedValue.ToString();
            GetDataAndShow();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton button = (RadioButton)sender;
            _selectType = Convert.ToInt16(button.Tag);
            this.flowLayoutPanel2.Controls.Clear();
            this.flowLayoutPanel2.Controls.AddRange(_dict[_selectType].ToList().Select(o => {
                RadioButton rbutton = new RadioButton() { Text = o, TextAlign = ContentAlignment.MiddleLeft };
                rbutton.Click += (s, args) => {
                    _selectHeader = (s as RadioButton).Text;
                    GetDataAndShow();
                };

                return rbutton;
            }).ToArray());
            (this.flowLayoutPanel2.Controls[0] as RadioButton).Checked = true;
            _selectHeader =this.flowLayoutPanel2.Controls[0].Text;
            GetDataAndShow();
        }

        private void GetDataAndShow()
        {
            this.flowLayoutPanel1.Controls.Clear();
            var areaToPhoneList = SingleHepler<AreaToPhoneNumberBLL>.Instance.GetList(new AreaToPhoneNumberViewModel() { AreaCode = _selectAreaCode, Type = _selectType, Header = _selectHeader });
            baseAction.SetContentWithCurrentThread<IList<AreaToPhoneNumber>>(this.flowLayoutPanel1, areaToPhoneList, (control, ls) =>
            {
                if (areaToPhoneList != null && areaToPhoneList.Count > 0)
                {
                    ls.ToList().ForEach(o =>
                    {
                        control.Controls.Add(new RadioButton() { Text = o.NumberSection, TextAlign = ContentAlignment.MiddleLeft });
                    });
                }
            });
        }
    }
}
