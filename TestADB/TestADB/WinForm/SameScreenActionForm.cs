using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Threading;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Timers;

using GroupControl.Common;
using GroupControl.Helper;
using GroupControl.BLL;
using GroupControl.Model;
using static GroupControl.WinForm.CustomControl.RoundPanel;

namespace GroupControl.WinForm
{
    public partial class SameScreenForm : BaseForm
    {

        private string _currentDevice;

        public delegate void ProjectionControl();

        public event ProjectionControl _projectionControl;

        private int StartX;

        private int StartY;

        private int EndX;

        private int EndY;

        private WMPoint wmPointModel;

        private Stopwatch stopwatch;

        private PictureBox pictureBoxSame;

        public SameScreenForm()
        {
            InitializeComponent();

            //  this.pictureBox1.MouseClick += PictureBox1_MouseClick;

            baseAction = SingleHepler<BaseAction>.Instance;

            this.Text = "同屏操作";


            this.FormClosed += Form2_FormClosed;


        }

        private void PictureBox1_MouseUp(object sender, MouseEventArgs e)
        {

            stopwatch.Stop();

            var timeSpan = stopwatch.ElapsedMilliseconds;

            stopwatch.Reset();

            this.Cursor = Cursors.WaitCursor;

            var x1 = (StartX * 100 / this.pictureBoxSame.Width) * wmPointModel.WM_X / 100;

            var y1 = (StartY * 100 / this.pictureBoxSame.Height) * wmPointModel.WM_Y / 100;

            var x2 = (e.X * 100 / this.pictureBoxSame.Width) * wmPointModel.WM_X / 100;

            var y2 = (e.Y * 100 / this.pictureBoxSame.Height) * wmPointModel.WM_Y / 100;


            if (StartX - e.X != 0 && StartY - e.Y != 0)
            {

                SameScreenOperation((device) =>
                {

                    baseAction.InitProcess(device, string.Format("shell input swipe {0} {1} {2} {3}", x1, y1, x2, y2));

                });


            }
            else
            {
                SameScreenOperation((device) =>
                {

                    if (timeSpan >= 1000)
                    {
                        ///长按
                        baseAction.InitProcess(device, string.Format("shell input swipe {0} {1} {2} {3} {4}", x1, y1, x1, y1, timeSpan));
                    }
                    else
                    {
                        baseAction.InitProcess(device, string.Format("shell input tap {0} {1}", x1, y1));
                    }



                });
            }


            this.Cursor = Cursors.Default;
        }

        private void PictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void PictureBox1_MouseDown(object sender, MouseEventArgs e)
        {

            StartX = e.X;

            StartY = e.Y;

            if (null == stopwatch)
            {
                stopwatch = new Stopwatch();
            }

            this.Cursor = Cursors.Hand;

            stopwatch.Start();



        }

        private void Form2_Load(object sender, EventArgs e)
        {
            _currentDevice = this.Name as string;

            CustomControl.RoundPanel _panel = new CustomControl.RoundPanel();
            _panel.Height = this.splitContainer1.Panel1.Height - 20;
            _panel.Width = this.splitContainer1.Panel1.Width - 10;
            _panel.Location = new Point(10, 10);
            _panel.RoundeStyle = RoundStyle.All;
            _panel.Radius = 30;
            _panel.BorderColor = Color.Black;

            pictureBoxSame = new PictureBox();
            pictureBoxSame.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBoxSame.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBoxSame.MouseDown += PictureBox1_MouseDown;
            pictureBoxSame.MouseMove += PictureBox1_MouseMove;
            pictureBoxSame.MouseUp += PictureBox1_MouseUp;
            pictureBoxSame.Height = _panel.Height - 70;
            pictureBoxSame.Width = _panel.Width - 20;
            pictureBoxSame.Location = new Point(10, 30);

            FlowLayoutPanel fp = new FlowLayoutPanel();
            fp.Height = 30;
            fp.Width = pictureBoxSame.Width;
            fp.Location = new Point(10, pictureBoxSame.Height + pictureBoxSame.Location.Y + 5);
            fp.Parent = _panel;

            var currentWidth = (fp.Width - 20) * 13 / 84;

            SetBottomBtn(fp, currentWidth, returnBottomBtnEvent());

            this.splitContainer1.Panel1.Controls.Add(_panel);
            _panel.Controls.Add(pictureBoxSame);
            _panel.Controls.Add(fp);

            this.ActiveControl = pictureBoxSame;

            this.StartPosition = FormStartPosition.CenterParent;

            //初始化分辨率
            wmPointModel = baseAction.GetWMSize(_currentDevice);

            this.pictureBoxSame.Name = string.Format(string.Format("Big_{0}", partialControlName), _currentDevice);

            this.pictureBoxSame.Tag = string.Format(partialControlName, _currentDevice);

            this.pictureBoxSame.BackColor = Color.Black;

            this.pictureBoxSame.Image = this.Tag as Image;

            baseAction.CurrentSameScreenControl = this.pictureBoxSame;

            var allCheckBox = new CheckBox()
            {

                Text = "全选",

                Name = "allCheck",

                ForeColor = Color.DarkTurquoise,

                TextAlign = ContentAlignment.MiddleLeft

            };

            allCheckBox.CheckedChanged += (er, obj) =>
            {
                var checkBox = er as CheckBox;

                CheckAllAndUpdateUI(this.flowLayoutPanelWithEquipment, checkBox);

            };



            this.flowLayoutPanelWithEquipment.Controls.Add(allCheckBox);

            CheckSomeEquipmentToDO(this.flowLayoutPanelWithEquipment, new DeviceToNickNameViewModel() { Device = _currentDevice });

            InitGroup(this.panelWithSameScreenGroup);

        }


        private void PictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            var x = e.X;

            var y = e.Y;

            var phoneX = (x * 100 / this.pictureBoxSame.Width) * wmPointModel.WM_X / 100;

            var phoneY = (y * 100 / this.pictureBoxSame.Height) * wmPointModel.WM_Y / 100;

            SameScreenOperation((device) =>
            {

                baseAction.InitProcess(device, string.Format("shell input tap {0} {1}", phoneX, phoneY));

            });

        }

        private void Form2_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {

            baseAction.CurrentSameScreenControl = default(Control);

            // DisposeControl();
        }

        /// <summary>
        /// 设置底部按钮
        /// </summary>
        /// <param name="panel"></param>
        /// <param name="width"></param>
        /// <param name="dic"></param>
        private void SetBottomBtn(FlowLayoutPanel panel, int width, Dictionary<Image, EventHandler> dic)
        {
            if (dic == null || dic.Count == 0)
            {
                return;
            }

            dic.Keys.ToList<Image>().ForEach(o =>
            {
                var action = dic[o];
                var btn = CreateActionBtn(panel, "", width);
                btn.Image = o;
                btn.ImageAlign = ContentAlignment.MiddleCenter;
                btn.Height = panel.Height;
                btn.Click += action;
            });

        }

        public void SameScreenOperation(Action<string> action)
        {

            Task.Factory.StartNew(() =>
            {

                IList<string> list = null;

                //分组
                var grouplist = GetCheckedEquipmentName(this.panelWithSameScreenGroup);

                var relationDevice = GetDeviceByGroups(grouplist);

                if (null != relationDevice && relationDevice.Count > 0)
                {
                    list = relationDevice;
                }
                else
                {
                    ///设备
                    list = GetCheckedEquipmentName(this.flowLayoutPanelWithEquipment);

                }

                Task.Factory.StartNew((device) =>
                {
                    action(device as string);

                }, _currentDevice, TaskCreationOptions.LongRunning);

                if (null != list && list.Count != 0)
                {
                    list = list.Where(o => !o.Equals(_currentDevice)).ToList();

                    var taskList = new List<Task>();

                    list.ToList().ForEach((item) =>
                    {

                        var task = Task.Factory.StartNew((device) =>
                           {
                               action(device as string);

                           }, item, TaskCreationOptions.LongRunning);

                        taskList.Add(task);

                    });

                }

            });


        }

        private void buttonLeft_Click(object sender, EventArgs e)
        {
            baseAction.InitProcess(_currentDevice, string.Format("shell input swipe {0} {1} {2} {3}", 600, 600, 200, 600));
        }

        private void buttonRight_Click(object sender, EventArgs e)
        {
            baseAction.InitProcess(_currentDevice, string.Format("shell input swipe {0} {1} {2} {3}", 200, 600, 700, 600));
        }

        /// <summary>
        ///对朋友圈 进行评论
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            var content = this.textBox1.Text;

            if (string.IsNullOrEmpty(content))
            {
                MessageBox.Show("请输入评论内容!");

                return;
            }

            SameScreenOperation((device) =>
            {

                ///输入评论内容
                baseAction.InitProcess(device, "shell am broadcast -a ADB_INPUT_TEXT --es msg '" + content + "'");

                ///点击确定按钮
              //  baseAction.InitProcess(device, "shell input tap 700 100");

            });


        }

        private Dictionary<Image, EventHandler> returnBottomBtnEvent()
        {
            return new Dictionary<Image, EventHandler>() {

                ///回到主屏幕
                {
                Properties.Resources.index,(o,s)=> { SameScreenOperation((device) =>{baseAction.InitProcess(device, "shell input keyevent 3");});}
                },

                ///返回键
                {
                Properties.Resources.back,(o,s)=> {SameScreenOperation((device) =>{baseAction.InitProcess(device, "shell input keyevent 4");});}
                },

                ///借屏键
                {
                Properties.Resources.light,(o,s)=> {SameScreenOperation((device) =>{baseAction.UnlockSingleScreen(device);});}
                },

                 ///启动微信
                {
                Properties.Resources.weixin,(o,s)=> {SameScreenOperation((device) =>{

                baseAction.InitProcess(device, " shell am force-stop com.tencent.mm");

                baseAction.InitProcess(device, " shell am start com.tencent.mm/com.tencent.mm.ui.LauncherUI", true);

                });}

                },

                ///启动闲鱼
                {
                Properties.Resources.idle,(o,s)=> {SameScreenOperation((device) =>{

                baseAction.InitProcess(device, " shell am force-stop com.taobao.idlefish");

                baseAction.InitProcess(device, " shell am start com.taobao.idlefish/com.taobao.fleamarket.home.activity.MainActivity", true);

                });}

                },
             
                ///启动快手
                {
                Properties.Resources.ks,(o,s)=> {SameScreenOperation((device) =>{

                baseAction.InitProcess(device, " shell am force-stop com.smile.gifmaker");

                baseAction.InitProcess(device, " shell am start com.smile.gifmaker/com.yxcorp.gifshow.HomeActivity", true);

                });}

                },

        };
        }
    }
}
