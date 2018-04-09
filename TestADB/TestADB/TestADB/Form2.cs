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

namespace TestADB
{
    public partial class Form2 : BaseForm
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

        public Form2()
        {
            InitializeComponent();

            //  this.pictureBox1.MouseClick += PictureBox1_MouseClick;

            this.pictureBoxSame.MouseDown += PictureBox1_MouseDown;

            this.pictureBoxSame.MouseMove += PictureBox1_MouseMove;

            this.pictureBoxSame.MouseUp += PictureBox1_MouseUp;

            this.ActiveControl = this.pictureBoxSame;

            this.StartPosition = FormStartPosition.CenterParent;

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

            //初始化分辨率
            wmPointModel = baseAction.GetWMSize(_currentDevice);

            this.pictureBoxSame.Name = string.Format(string.Format("Big_{0}", partialControlName), _currentDevice);

            this.pictureBoxSame.Tag = string.Format(partialControlName, _currentDevice);

            this.pictureBoxSame.BackColor = Color.Black;

            this.pictureBoxSame.Image=this.Tag as Image;

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

            CheckSomeEquipmentToDO(this.flowLayoutPanelWithEquipment,new  DeviceToNickNameViewModel() {Device = _currentDevice });

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
        /// 回到主屏幕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            SameScreenOperation((device) =>
            {

                baseAction.InitProcess(device, "shell input keyevent 3");

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

                    var taskList=new List<Task>();

                    list.ToList().ForEach((item) =>
                    {

                       var task=Task.Factory.StartNew((device) =>
                        {
                            action(device as string);

                        }, item, TaskCreationOptions.LongRunning);

                        taskList.Add(task);

                    });

                }

            });


        }

        /// <summary>
        /// 返回键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            SameScreenOperation((device) =>
            {

                baseAction.InitProcess(device, "shell input keyevent 4");

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
        /// 解屏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UnLock_Click(object sender, EventArgs e)
        {
            SameScreenOperation((device) =>
            {

                baseAction.UnlockSingleScreen(device);

            });
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


        /// <summary>
        /// 启动微信
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            SameScreenOperation((device) =>
            {

                baseAction.InitProcess(device, " shell am force-stop com.tencent.mm");

                baseAction.InitProcess(device, " shell am start com.tencent.mm/com.tencent.mm.ui.LauncherUI", true);

            });
        }

        private void pictureBoxSame_Click(object sender, EventArgs e)
        {

        }
    }
}
