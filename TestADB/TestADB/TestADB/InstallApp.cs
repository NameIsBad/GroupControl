using GroupControl.Common;
using GroupControl.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestADB
{
    public partial class InstallApp : BaseForm
    {

        private EnumInstallAppType currentInstallAppType = EnumInstallAppType.KeyBoard;

        private StringBuilder _stringBulider = new StringBuilder();

        private IList<string> appPathList = new List<string>();

        public delegate void AfreshSameScreenControl(string device);

        public event AfreshSameScreenControl _afreshSameScreenControl;


        /// <summary>
        /// 当前输入法ID
        /// </summary>
        private string _currentTypeWrittingID = "com.android.adbkeyboard/.AdbIME";

        public InstallApp()
        {
            InitializeComponent();

            this.textBoxOut.BackColor = Color.Black;

            this.textBoxOut.ForeColor = Color.White;

            this.textBoxOut.ReadOnly = true;

            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void InstallApp_Load(object sender, EventArgs e)
        {
            currentInstallAppType = (EnumInstallAppType)this.Tag;

            switch (currentInstallAppType)
            {
                case EnumInstallAppType.KeyBoard:

                    this.InstallAppDir.Hide();

                    this.changeTypeWritting.Show();


                    break;
                case EnumInstallAppType.InstallApp:

                    this.InstallAppDir.Show();

                    this.changeTypeWritting.Hide();

                    break;
                default:
                    break;
            }

            ///获取设备
            GetDeviceWithXCheckBox(this.checkDevices);

        }

        private void InstallApp_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            DisposeControl();
        }


        public void OutStr(string tip)
        {
            baseAction.SetContentWithCurrentThread<string>(this.textBoxOut, tip, (control, str) =>
            {
                var textBox = control as TextBox;

                SetTextBoxCursor(textBox,str);
            });
        }

        /// <summary>
        /// 切换输入法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TypeWirtting_ValueChanged(object sender, EventArgs e)
        {
            var control = (RadioButton)sender;

            _currentTypeWrittingID = Convert.ToString(control.Tag);
        }

        /// <summary>
        /// 执行命令 切换输入法
        /// </summary>
        public void InstallKeyBoardApp(string device)
        {

            ///com.android.adbkeyboard/.AdbIME
            ///com.sohu.inputmethod.sogou/.SogouIME
            ///切换输入法
            var list = baseAction.InitProcess(device, string.Format("shell ime enable  {0}", _currentTypeWrittingID), true);

            var returnStr = string.Join("|", list);

            if (returnStr.Contains("enabled"))
            {
                list = baseAction.InitProcess(device, string.Format("shell ime set {0}", _currentTypeWrittingID), true);

                returnStr = string.Join("|", list);

                ///选择输入法
                if (returnStr.Contains("selected"))
                {

                    SetTextBoxCursor(this.textBoxOut, string.Format("{0} 输入法切换成功 \r\n\r\n", device));

                }
            }
        }


        /// <summary>
        /// 安装App
        /// </summary>
        /// <param name="device"></param>
        public void Install(string device)
        {

            if (null != appPathList && appPathList.Count() > 0)
            {
                appPathList.ToList().ForEach((item) =>
                {
                    ///安装输入法
                    var list = baseAction.InitProcess(device, string.Format("install -r {0} ", item), true);

                    if (null != list && list.Count > 0)
                    {
                        var returnStr = string.Join("|", list);

                        //表示安装成功
                        if (returnStr.Contains("Success"))
                        {
                            SetTextBoxCursor(this.textBoxOut, string.Format("{0} {1} 安装成功", device,item));
                        }
                    }

                });

                _afreshSameScreenControl(device);


            }
        }


        /// <summary>
        /// 安装App或者更改输入法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInstall_Click(object sender, EventArgs e)
        {

            this.textBoxOut.Text = string.Empty;

            var btn = (Button)sender;

            btn.Parent.Hide();

            if (currentInstallAppType == EnumInstallAppType.InstallApp && (null == appPathList || appPathList.Count == 0))
            {
                MessageBox.Show("请选择安装App所在目录");

                return;
            }


            Task.Factory.StartNew(() =>
            {

                Action<string> action = null;

                if (null != baseAction.Devices && baseAction.Devices.Count > 0)
                {

                    var taskList = new List<Task>();

                    switch (currentInstallAppType)
                    {
                        case EnumInstallAppType.KeyBoard:

                            action = (item) =>
                            {

                                SetTextBoxCursor(this.textBoxOut, string.Format("{0}  开始切换......", item));

                                InstallKeyBoardApp(item);

                            };

                            break;
                        case EnumInstallAppType.InstallApp:

                            action = (item) =>
                            {

                                SetTextBoxCursor(this.textBoxOut, string.Format("{0}  开始安装......", item));

                                Install(item);

                            };

                            break;
                        default:
                            break;
                    }

                    var devices = GetCheckedEquipmentName(this.checkDevices);

                    if (null == devices || devices.Count == 0)
                    {
                        devices = baseAction.Devices;
                    }

                    devices.ToList().ForEach((item) =>
                    {
                        var task = Task.Factory.StartNew(() =>
                           {

                               action(item);

                           });

                        taskList.Add(task);

                    });

                    Task.WaitAll(taskList.ToArray());
                }

            }).ContinueWith((task) =>
            {

                SetTextBoxCursor(this.textBoxOut, "全部操作完毕");

                baseAction.SetContentWithCurrentThread<object>(btn, null, (control, obj) =>
                {
                    btn.Parent.Show();

                });

            });

        }

        /// <summary>
        /// 选择安装文件所在目录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void selectDirPath_Click(object sender, EventArgs e)
        {
            using (var folderBrowserDialog = new FolderBrowserDialog())
            {

                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    textBoxDirPath.Text = folderBrowserDialog.SelectedPath;
                }
            }


            if (string.IsNullOrEmpty(textBoxDirPath.Text))
            {
                return;
            }

            var folder = new DirectoryInfo(textBoxDirPath.Text);

            var files = folder.GetFiles();

            if (null != files && files.Count() > 0)
            {
                files.ToList().ForEach((item) =>
                {
                    if (item.Extension.ToLower().Contains("apk"))
                    {
                        appPathList.Add(item.FullName);
                    }
                });
            }

        }
    }
}
