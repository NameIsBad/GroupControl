using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using GroupControl.BLL;
using GroupControl.Model;
using GroupControl.Common;
using GroupControl.Helper;

namespace GroupControl.WinForm
{
    public partial class TimerSendFriendCircle : BaseForm
    {

        private EnumPublishContentType _currentSelectContentType;

        private DateTime _startDate;

        private DateTime _startTime;

        private IList<string> _pictureList;

        private IList<string> _vedioList;

        private EnumSendType _currentSendType;

        private WXHelper wxHelper;

        private Button addContentBtn;

        public TimerSendFriendCircle()
        {
            InitializeComponent();

        }

        private void TimerSendFriendCircle_Load(object sender, EventArgs e)
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

            wxHelper = SingleHepler<WXHelper>.Instance;

            CheckSomeEquipmentToDO(this.flowLayoutPanelWithEquipment);

            InitGroup(this.flowLayoutPanelWithGroup);

        }


        private void TimerSendFriendCircle_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            DisposeControl();
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


        /// <summary>
        /// 发布内容类型改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckedChanged(object sender, EventArgs e)
        {
            var selectRadioButton = sender as RadioButton;

            var type = 0;

            int.TryParse(selectRadioButton.Tag.ToString(), out type);

            _currentSelectContentType = (EnumPublishContentType)type;

            this.flowLayoutPanel1.Controls.Clear();

            switch (_currentSelectContentType)
            {
                case EnumPublishContentType.PictureAndWord:

                    _pictureList = new List<string>();

                    break;
                case EnumPublishContentType.VedioAndWord:

                    _vedioList = new List<string>();

                    break;
                case EnumPublishContentType.LinkAndWord:
                    break;
                default:
                    break;
            }

            AddBtnWithAddContent();

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
                fileDialog.FilterIndex =1;
                fileDialog.RestoreDirectory = true;
                switch (_currentSelectContentType)
                {
                    case EnumPublishContentType.PictureAndWord:
                        fileDialog.Filter = "JPEG文件(*.jpg)|*.jpg|GIF文件(*.gif)|*.gif|png文件(*.png)|*.png";
                        break;
                    case EnumPublishContentType.VedioAndWord:

                        fileDialog.Filter = "MP4文件(*.mp4)|*.mp4";
                        break;
                    case EnumPublishContentType.LinkAndWord:
                        break;
                    default:
                        break;
                }

                //判断用户是否正确的选择了文件
                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                   try
                    {
                        switch (_currentSelectContentType)
                        {
                            case EnumPublishContentType.PictureAndWord:

                                var imageNames = fileDialog.FileNames;

                                if (null == _pictureList)
                                {
                                    _pictureList = new List<string>();
                                }

                                if (imageNames != null && imageNames.Count()>0)
                                {
                                    imageNames.ToList().ForEach((o) =>
                                    {
                                        _pictureList.Add(o);

                                        DymicAddPicBox(o);
                                    });
                                }
                           
                                break;
                            case EnumPublishContentType.VedioAndWord:

                                if (null == _vedioList)
                                {
                                    _vedioList = new List<string>();
                                }

                                _vedioList.Add(fileDialog.FileName);

                                DymicAddMedia(fileDialog.FileName);

                                break;
                            case EnumPublishContentType.LinkAndWord:
                                break;
                            default:
                                break;
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


        /// <summary>
        /// 添加素材按钮
        /// </summary>
        private void AddBtnWithAddContent()
        {

            addContentBtn = new Button();

            addContentBtn.Click += Btn_Click;

            addContentBtn.Margin =new Padding() { Left=5, Right=5, Bottom=5, Top=5 };

            addContentBtn.Height = this.flowLayoutPanel1.Height-10;

            addContentBtn.Width = (this.flowLayoutPanel1.Width - 100) / 10;

            addContentBtn.Text = "添加";

            addContentBtn.TextAlign = ContentAlignment.MiddleCenter;

            this.flowLayoutPanel1.Controls.Add(addContentBtn);
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

                if (null == picBoxList || picBoxList.Count < 9)
                {
                    return true;
                }

            }
            else if (_currentSelectContentType == EnumPublishContentType.VedioAndWord)
            {
                var vedioList = GetChildControls<Label>(this.flowLayoutPanel1);

                if (null == vedioList || vedioList.Count == 0)
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

            switch (_currentSelectContentType)
            {
                case EnumPublishContentType.PictureAndWord:

                    if (null != _pictureList)
                    {
                        _pictureList.Remove(currentControl.Name);
                    }

                    break;
                case EnumPublishContentType.VedioAndWord:

                    if (null != _vedioList)
                    {
                        _vedioList.Remove(currentControl.Name);
                    }

                    break;
                case EnumPublishContentType.LinkAndWord:
                    break;
                default:
                    break;
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

        private void Label_DoubleClick(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button10_Click(object sender, EventArgs e)
        {
            ///获取本地根路径
            wxHelper.GetNativeRootPath(_currentSendType);

            wxHelper.Content = this.textBox3.Text;

            wxHelper.PublishContentType = _currentSelectContentType;

            switch (_currentSelectContentType)
            {
                case EnumPublishContentType.PictureAndWord:

                    wxHelper.UpLoadPathList = _pictureList;

                    break;
                case EnumPublishContentType.VedioAndWord:

                    wxHelper.UpLoadPathList = _vedioList;

                    break;
                case EnumPublishContentType.LinkAndWord:

                    wxHelper.UpLoadPathList = _pictureList;

                    break;
                default:
                    break;
            }

            if ((null == wxHelper.UpLoadPathList || wxHelper.UpLoadPathList.Count == 0) && string.IsNullOrEmpty(wxHelper.Content))
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

                var viewModel = new AutoServiceInfoViewModel()
                {
                    AutoServiceInfoModel = new AutoServiceInfo()
                    {
                        ContentType = _currentSelectContentType,
                        MapUrl = relativePath,
                        StartDate = startDate,
                        Status = _currentSendType == EnumSendType.HandSend ? EnumTaskStatus.Executing : EnumTaskStatus.Start,
                        ServiceType = EnumTaskType.SendFriendCircle,
                        SendType = _currentSendType
                    }
                };

                viewModel.Devices = GetCheckedEquipmentName(this.flowLayoutPanelWithEquipment);

                viewModel.GroupIDs = GetCheckedEquipmentName(this.flowLayoutPanelWithGroup);

                CreateTaskToQueue(viewModel,()=> {

                    ///保存上传内容到本地
                    wxHelper.SaveFileToNative(relativePath, _currentSelectContentType);

                });

                this.Close();
            }
            catch (Exception ex)
            {

            }

        }
    }
}
