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
using System.Timers;
using System.IO;

using GroupControl.Common;
using GroupControl.Helper;
using GroupControl.BLL;
using GroupControl.Model;

namespace GroupControl.WinForm
{
    public partial class StartServer : BaseForm
    {

        private System.Timers.Timer timer;

        private System.Threading.Timer _threadTimer;

        public delegate void CatchError(Exception e);

        public event CatchError _catchError;


        public StartServer()
        {
            InitializeComponent();

            //ThreadPool.SetMinThreads(400, 400);

            this.textBox1.BackColor = Color.Black;

            this.textBox1.ForeColor = Color.White;

            this.textBox1.ReadOnly = true;

            //this.Text = "初始化服务";

            this.ControlBox = false;

            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private StringBuilder _textContent = new StringBuilder();

        private void StartServer_Load(object sender, EventArgs e)
        {
            baseAction = SingleHepler<BaseAction>.Instance;

            _catchError += (ex) =>
            {
                SetTextBoxCursor(this.textBox1, ex.ToString());

            };

            Task.Factory.StartNew(() =>
            {
                try
                {

                    SetTextBoxCursor(this.textBox1, "正在验证注册码......");

                    if (string.IsNullOrEmpty(_currentMACAddress))
                    {
                        SetTextBoxCursor(this.textBox1, "请输入产品密钥,按回车键确认:");

                        return;
                    }

                    ////读取数据库中注册码  判断是否存在
                    var currentComputerInfoList = SingleHepler<ComputerInfoBLL>.Instance.GetList(new ComputerInfoViewModel() { MACAddress = _currentMACAddress });

                    if (null == currentComputerInfoList || currentComputerInfoList.Count() == 0)
                    {
                        SetTextBoxCursor(this.textBox1, "MAC地址不合法，请联系系统管理员");

                        return;
                    }

                    _currentComputerInfo = currentComputerInfoList.FirstOrDefault();

                    SetTextBoxCursor(this.textBox1, "验证成功!");

                    SetTextBoxCursor(this.textBox1, "正在启动监控服务......");

                    baseAction.InitProcess(string.Empty, "kill-server");

                    var list = baseAction.InitProcess(string.Empty, "start-server", true);

                    if (null != list && list.Count >= 2)
                    {
                        SetTextBoxCursor(this.textBox1, "服务成功启动!");
                    }

                    SetTextBoxCursor(this.textBox1, "正在检测可用设备......");

                    var deviceList = baseAction.InitProcess(null, "devices", true);

                    deviceList = CheckIsHaveEquipment(deviceList);

                    deviceList = CheckIsHaveAuthorizedEquipment(deviceList);

                    if (deviceList != null && deviceList.Count > 0)
                    {
                        //去除设备号重复
                        deviceList = deviceList.Where((x, i) => deviceList.ToList().FindIndex(q => q.Equals(x)) == i).ToList();
                    }

                    baseAction.Devices = deviceList;

                    SetTextBoxCursor(this.textBox1, "设备初始化成功!");

                    SetTextBoxCursor(this.textBox1, "正在启动定时服务......");

                    TimerGetTask();

                    AsyncStartTask();

                    SetTextBoxCursor(this.textBox1, "正在进入操作主界面......");

                }
                catch (Exception exception)
                {
                    SetTextBoxCursor(this.textBox1, exception.Message.ToString());

                    throw;

                    //_catchError(exception);
                }

            }).ContinueWith((task) =>
            {
                baseAction.SetContentWithCurrentThread<object>(this, new object(), (control, o) =>
                {

                    var currentControl = control as Form;

                    currentControl.Close();

                    currentControl.Dispose();

                });

            });


        }


        /// <summary>
        /// 判断是否插入了设备
        /// </summary>
        /// <param name="deviceList"></param>
        private IList<string> CheckIsHaveEquipment(IList<string> deviceList)
        {
            var currentDeviceList = GetEquipment(deviceList);

            if (null == currentDeviceList || currentDeviceList.Count == 0)
            {

                SetTextBoxCursor(this.textBox1, "请插入设备......");
            }

            while (null == currentDeviceList || currentDeviceList.Count == 0)
            {
                currentDeviceList = baseAction.InitProcess(null, "devices", true);

                currentDeviceList = GetEquipment(currentDeviceList);

                Thread.Sleep(100);
            }

            return currentDeviceList;
        }


        /// <summary>
        /// 判断是否有可识别的设备
        /// </summary>
        public IList<string> CheckIsHaveAuthorizedEquipment(IList<string> deviceList)
        {
            var currentDeviceList = GetAuthorizedEquipment(deviceList);

            if (null == currentDeviceList || currentDeviceList.Count == 0)
            {
                SetTextBoxCursor(this.textBox1, "请允许USB调试!");
            }

            while (null == currentDeviceList || currentDeviceList.Count == 0)
            {

                currentDeviceList = baseAction.InitProcess(null, "devices", true);

                var currentContentEquipment = GetEquipment(currentDeviceList);

                if ((null == currentContentEquipment || currentContentEquipment.Count == 0))
                {

                    SetTextBoxCursor(this.textBox1, "请插入设备......");
                }

                while (null == currentContentEquipment || currentContentEquipment.Count == 0)
                {

                    currentContentEquipment = baseAction.InitProcess(null, "devices", true);

                    currentContentEquipment = GetEquipment(currentContentEquipment);

                    Thread.Sleep(100);
                }

                currentDeviceList = GetAuthorizedEquipment(currentDeviceList);

                Thread.Sleep(100);
            }

            return currentDeviceList;
        }

        private IList<string> GetEquipment(IList<string> deviceList)
        {

            if (null == deviceList || deviceList.Count == 0)
            {
                return default(IList<string>);
            }

            return deviceList.Where(o => !infoList.Contains(o)).TakeWhile(o => !string.IsNullOrEmpty(o)).ToList();
        }

        private IList<string> GetAuthorizedEquipment(IList<string> deviceList)
        {

            if (null == deviceList || deviceList.Count == 0)
            {
                return default(IList<string>);
            }

            return deviceList.Where(o => !infoList.Contains(o)).TakeWhile(o => !string.IsNullOrEmpty(o) && !o.Split('\t')[1].Equals("unauthorized")).Select(o => { return o.Split('\t')[0]; }).ToList();
        }


        /// <summary>
        /// 启动定时执行任务服务
        /// </summary>
        private void TimerGetTask()
        {
            timer = new System.Timers.Timer(5000);
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        /// <summary>
        /// 启动线程时间计时器
        /// </summary>
        private void TimerWithThread()
        {
            _threadTimer = new System.Threading.Timer(ThreadTimeAction, null, Timeout.Infinite, Timeout.Infinite);

            ///立即触发
            _threadTimer.Change(0, Timeout.Infinite);

        }



        private void AsyncStartTask()
        {
            Action currentAction = () =>
            {
                if (null == _taskQueue || _taskQueue.Count == 0)
                {
                    Thread.Sleep(500);

                    return;
                }

                Action<Action> _lockAction = (action) =>
                {
                    lock (_taskLockObject)
                    {
                        if (null == _taskQueue || _taskQueue.Count == 0)
                        {
                            return;
                        }

                        action.Invoke();
                    }
                };

                var currentTask = new AutoServiceInfoViewModel();

                _lockAction(() =>
                {

                    currentTask = _taskQueue.Peek();

                });


                ///当前任务是结束状态
                if (currentTask.Status == EnumTaskStatus.End)
                {
                    _lockAction(() =>
                    {

                        _taskQueue.Dequeue();

                    });

                    return;
                }

                try
                {

                    var wxHepler = SingleHepler<WXHelper>.Instance;

                    var ksHepler = SingleHepler<KSHelper>.Instance;

                    var idleHepler = SingleHepler<IdleFishHelper>.Instance;

                    var taskArray = new List<Task>();

                    Func<AutoServiceInfoViewModel, AutoServiceInfoViewModel> initFunc = (task) =>
                    {
                        var rootPath = SingleHepler<ConfigInfo>.Instance.AutoSendFriendFileUrl;

                        if (task.AutoServiceInfoModel.SendType == EnumSendType.HandSend)
                        {
                            rootPath = SingleHepler<ConfigInfo>.Instance.HandSendFriendFileUrl;
                        }

                        var root = string.Format(@"{0}\{1}", rootPath, task.AutoServiceInfoModel.MapUrl);

                        task.Path = root;

                        task.PublishContentType = task.AutoServiceInfoModel.ContentType;

                        return task;
                    };

                    switch (currentTask.AutoServiceInfoModel.ServiceType)
                    {
                        case EnumTaskType.SendFriendCircle:

                            currentTask=initFunc(currentTask);

                            taskArray = wxHepler.BatchSendFriendCircle(currentTask).ToList();

                            break;

                        case EnumTaskType.AddFriend:

                            taskArray = wxHepler.BatchAddFriend(currentTask).ToList();

                            break;

                        case EnumTaskType.SearchAddFriend:

                            taskArray = wxHepler.BatchAddFriendWithSearch(currentTask).ToList();

                            break;


                        case EnumTaskType.QueryFansCount:

                            taskArray = wxHepler.BatchQueryFansCount(currentTask).ToList();

                            break;

                        case EnumTaskType.KSAction:

                            taskArray = ksHepler.BatchAction(new KSViewModel() { Devices = currentTask.Devices, Comments = currentTask.AutoServiceInfoModel.SayHelloContent, PhotoIDs = currentTask.AutoServiceInfoModel.RemarkContent }).ToList();

                            break;
                        case EnumTaskType.SendGroupMessage:

                            taskArray = wxHepler.BatchSendMessageToGroup(currentTask).ToList();

                            break;
                        case EnumTaskType.SendIdleFishPost:

                            currentTask = initFunc(currentTask);

                           // taskArray = idleHepler.BatchPublishMessageinfo(currentTask).ToList();

                            break;

                        default:

                            break;
                    }

                    Task.WaitAll(taskArray.ToArray());

                    currentTask.Status = EnumTaskStatus.End;

                    baseAction.RemoveEquipmentStateFromQueue();

                    ///任务完成 跟新数据库任务状态
                    _currentAction(currentTask.ID);

                    Thread.Sleep(100);
                }
                catch (Exception ex)
                {
                    //throw;
                }

            };


            Task.Factory.StartNew(() =>
            {

                while (true)
                {
                    currentAction();
                }

            });
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var list = SingleHepler<AutoServiceInfoBLL>.Instance.GetList(new AutoServiceInfoViewModel() { Status = EnumTaskStatus.Start, ComputerID = _currentComputerInfo.ID });

            if (null != list && list.Count > 0)
            {

                timer.Stop();

                list.ToList().ForEach((item) =>
                {
                    if (DateTime.Compare(DateTime.Now, item.StartDate) >= 0)
                    {
                        item.Status = EnumTaskStatus.Executing;

                        SingleHepler<AutoServiceInfoBLL>.Instance.Update(item);

                        InsertTaskModelToQueue(new AutoServiceInfoViewModel() { AutoServiceInfoModel = item, ID = item.ID });
                    }

                });

                timer.Start();

            }


        }


        /// <summary>
        /// 线程池计时器回掉方法
        /// </summary>
        /// <param name="state"></param>
        private void ThreadTimeAction(object state)
        {
            Action _currentAction = () =>
            {
                ///方法返回前 设置 5秒之后再次触发
                _threadTimer.Change(5000, Timeout.Infinite);

            };

            var list = SingleHepler<AutoServiceInfoBLL>.Instance.GetList(new AutoServiceInfoViewModel() { Status = EnumTaskStatus.Start, ComputerID = _currentComputerInfo.ID });

            if (null == list || list.Count == 0)
            {
                _currentAction();

                return;
            }

            list.ToList().ForEach((item) =>
            {
                if (DateTime.Compare(DateTime.Now, item.StartDate) >= 0)
                {
                    item.Status = EnumTaskStatus.Executing;

                    SingleHepler<AutoServiceInfoBLL>.Instance.Update(item);

                    InsertTaskModelToQueue(new AutoServiceInfoViewModel() { AutoServiceInfoModel = item, ID = item.ID });
                }

            });

            _currentAction();

            ///方法执行完后  线程回归线程池 等待下一个任务
        }


    }
}
