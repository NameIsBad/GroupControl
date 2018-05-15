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
using GroupControl.Helper;
using GroupControl.Common;
using System.IO;

namespace GroupControl.WinForm
{
    public partial class SendIdleFishPost : BaseForm
    {

        private EnumPublishContentType _currentSelectContentType;

        private DateTime _startDate;

        private DateTime _startTime;

        private IList<string> _pictureList;

        private IList<string> _vedioList;

        private EnumSendType _currentSendType;

        private IdleFishHelper idleFishHepler;

        private WXHelper wxHelper;

        private Button addContentBtn;

        public SendIdleFishPost()
        {
            InitializeComponent();
        }

        private void SendIdleFishPost_Load(object sender, EventArgs e)
        {
            this.startdate.CustomFormat = "yyyy/MM/dd";
            this.startdate.Format = DateTimePickerFormat.Custom;
            _startDate = DateTime.Now;

            this.starttime.CustomFormat = "HH:mm";
            this.starttime.Format = DateTimePickerFormat.Custom;
            this.starttime.ShowUpDown = true;
            _startTime = DateTime.Now;

            _currentSendType = ((EnumSendType)Convert.ToInt32(this.Tag));

            _currentSelectContentType = EnumPublishContentType.PictureAndWord;

            this.Send.Text = _currentSendType == EnumSendType.AutoSend ? "提交" : "发送";

            var isShow = _currentSendType == EnumSendType.AutoSend;

            if (!isShow)
            {
                this.SetStartDate.Hide();
            }

            AddBtnWithAddContent();

            idleFishHepler = SingleHepler<IdleFishHelper>.Instance;

            CheckSomeEquipmentToDO(this.flowLayoutPanelWithEquipment);

            InitGroup(this.flowLayoutPanelWithGroup);
        }


        /// <summary>
        /// 添加素材按钮
        /// </summary>
        private void AddBtnWithAddContent()
        {

            addContentBtn = new Button();

            addContentBtn.Click += Btn_Click;

            addContentBtn.Margin = new Padding() { Left = 5, Right = 5, Bottom = 5, Top = 5 };

            addContentBtn.Height = this.flowLayoutPanel1.Height - 10;

            addContentBtn.Width = (this.flowLayoutPanel1.Width - 100) / 10;

            addContentBtn.Text = "添加";

            addContentBtn.TextAlign = ContentAlignment.MiddleCenter;

            this.flowLayoutPanel1.Controls.Add(addContentBtn);
        }

        /// <summary>
        /// 添加图片或者视频
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Click(object sender, EventArgs e)
        {
            using (var fileDialog = new OpenFileDialog())
            {
                fileDialog.Multiselect = true;//允许同时选择多个文件 
                fileDialog.FilterIndex = 1;
                fileDialog.RestoreDirectory = true;
                fileDialog.Filter = "JPEG文件(*.jpg)|*.jpg|GIF文件(*.gif)|*.gif|png文件(*.png)|*.png";

                //判断用户是否正确的选择了文件
                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var imageNames = fileDialog.FileNames;

                        if (null == _pictureList)
                        {
                            _pictureList = new List<string>();
                        }

                        if (imageNames != null && imageNames.Count() > 0)
                        {
                            imageNames.ToList().ForEach((o) =>
                            {
                                _pictureList.Add(o);

                                DymicAddPicBox(o);
                            });
                        }

                        var isShow = IsShowAddBtn();

                        var btn = sender as Button;

                        if (isShow)
                        {
                            btn.Show();

                            return;
                        }

                        btn.Hide();
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
        }

        private IList<Control> GetChildControls<T>(FlowLayoutPanel parentControl) where T : Control
        {
            var childControls = parentControl.Controls;

            var controlList = new List<Control>();

            if (null != childControls && childControls.Count > 0)
            {
                foreach (var item in childControls)
                {
                    var currentControl = item as T;

                    if (null != currentControl)
                    {
                        controlList.Add(currentControl);
                    }
                }
            }

            return controlList;
        }

        private bool IsShowAddBtn()
        {
            if (_currentSelectContentType == EnumPublishContentType.PictureAndWord)
            {

                var picBoxList = GetChildControls<PictureBox>(this.flowLayoutPanel1);

                if (null == picBoxList || picBoxList.Count < 10)
                {
                    return true;
                }

            }

            return false;
        }

        /// <summary>
        /// 双击移除图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveControlWithDoubleClick<T>(object sender, EventArgs e) where T : Control
        {
            var currentControl = sender as T;

            if (this.flowLayoutPanel1.Controls.Contains(currentControl))
            {
                this.flowLayoutPanel1.Controls.Remove(currentControl);
            }

            if (null != _pictureList)
            {
                _pictureList.Remove(currentControl.Name);
            }

            if (null != addContentBtn)
            {
                if (IsShowAddBtn())
                {
                    addContentBtn.Show();
                }
                else
                {
                    addContentBtn.Hide();
                }


            }

            currentControl.Dispose();

        }

        public void DymicAddPicBox(string path)
        {
            PictureBox currentPictureBox = new PictureBox();

            currentPictureBox.BackgroundImageLayout = ImageLayout.Stretch;

            currentPictureBox.Margin = new Padding() { Left = 5, Right = 5, Bottom = 5, Top = 5 };

            currentPictureBox.Height = this.flowLayoutPanel1.Height - 10;

            currentPictureBox.Width = (this.flowLayoutPanel1.Width - 100) / 10;

            currentPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;

            currentPictureBox.Image = Image.FromFile(path);

            currentPictureBox.DoubleClick += RemoveControlWithDoubleClick<PictureBox>;

            currentPictureBox.Name = path;

            this.flowLayoutPanel1.Controls.Add(currentPictureBox);
        }

        public void DymicAddMedia(string path)
        {
            Label label = new Label();

            label.Width = 90;

            label.BackColor = Color.Gray;

            label.TextAlign = ContentAlignment.MiddleCenter;

            label.Height = 100;

            label.Name = path;

            label.Text = Path.GetFileName(path);

            label.DoubleClick += RemoveControlWithDoubleClick<Label>;

            this.flowLayoutPanel1.Controls.Add(label);
        }



        /// <summary>
        /// 发送
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Send_Click(object sender, EventArgs e)
        {
            ///获取本地根路径
            baseAction.GetNativeRootPath(_currentSendType);

            baseAction.PublishContentType = _currentSelectContentType;

            baseAction.UpLoadPathList = _pictureList;

            if (string.IsNullOrEmpty(this.txt_title.Text) || string.IsNullOrEmpty(this.txt_price.Text) || string.IsNullOrEmpty(this.txt_content.Text))
            {
                MessageBox.Show("标题，描述以及价格都不能为空!");

                return;
            }

            if ((null == baseAction.UpLoadPathList || baseAction.UpLoadPathList.Count == 0))
            {
                MessageBox.Show("素材内容不能为空!");

                return;

            }


            try
            {

                var currentTaskDirName = string.Format("{0}-{1}-{2}-{3}", _startTime.Hour, _startTime.Minute, _startTime.Second, _startDate.Millisecond);

                var relativePath = string.Format(@"{0}\{1}", _startDate.ToString(@"yyyy-MM-dd"), currentTaskDirName);

                ///添加任务记录
                var startDate = DateTime.Parse(string.Format("{0} {1}", _startDate.ToString("yyyy-MM-dd"), _startTime.ToString("HH:mm")));

                var defaultPrice = 0;
                int.TryParse(this.txt_price.Text, out defaultPrice);

                var viewModel = new AutoServiceInfoViewModel()
                {          
                    AutoServiceInfoModel = new AutoServiceInfo()
                    {
                        ContentType = _currentSelectContentType,
                        MapUrl = relativePath,
                        StartDate = startDate,
                        Status = _currentSendType == EnumSendType.HandSend ? EnumTaskStatus.Executing : EnumTaskStatus.Start,
                        ServiceType = EnumTaskType.SendIdleFishPost,
                        SendType = _currentSendType,
                        SayHelloContent = this.txt_title.Text,
                        RemarkContent = this.txt_content.Text,
                        AddCount = defaultPrice
                    }
                };

                viewModel.Devices = GetCheckedEquipmentName(this.flowLayoutPanelWithEquipment);

                viewModel.GroupIDs = GetCheckedEquipmentName(this.flowLayoutPanelWithGroup);

                CreateTaskToQueue(viewModel, () => {

                    ///保存上传内容到本地
                    baseAction.SaveFileToNative(relativePath);

                });

                this.Close();
            }
            catch (Exception ex)
            {

            }
        }
    }
}
