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
using GroupControl.Model;
using GroupControl.Common;
using GroupControl.Helper;

namespace TestADB
{
    public partial class TaskListForm : BaseForm
    {

        public TaskListForm()
        {
            InitializeComponent();
        }

        private void TaskListForm_Load(object sender, EventArgs e)
        {

            InitDataGridView(this.taskListGridView, new Dictionary<string, string>() {

                { "执行时间","执行时间"},

                { "任务类型","任务类型"},

                { "分组信息(设备数)","分组信息(设备数)"},

                { "任务状态","任务状态"},

                  { "操作","操作"}

            });

            this.taskListGridView.CellContentClick += TaskListGridView_CellContentClick;

            GetTaskList();

        }

        private void GetTaskList()
        {
            Task.Factory.StartNew<IList<AutoServiceInfoViewModel>>(() =>
            {
                var list = SingleHepler<AutoServiceInfoBLL>.Instance.GetTaskListWithGroupInfo(new AutoServiceInfoViewModel() { ComputerID = _currentComputerInfo.ID });

                return list;

            }).ContinueWith((task) =>
            {

                var returnData = task.Result;

                if (null != returnData && returnData.Count > 0)
                {
                    returnData.ToList().ForEach((item) =>
                    {
                        AddRowToDataGrridView(item, this.taskListGridView, (c, o) =>
                        {
                            var currentTaskWithGroupInfoStr = "--/--";

                            if (null != o.GroupInfoViewModelList && o.GroupInfoViewModelList.Count > 0)
                            {
                                var returnList = o.GroupInfoViewModelList.Select(q => { return string.Format("{0}({1})", q.GroupInfoModel.Name, q.DevicesCount); });

                                currentTaskWithGroupInfoStr = String.Join(",", returnList);
                            };

                            c.Cells[0] = new DataGridViewTextBoxCell() { Value = o.AutoServiceInfoModel.StartDate.ToString("yyyy/MM/dd HH:mm") };

                            c.Cells[1] = new DataGridViewTextBoxCell()
                            {
                                Value = DataUtil.GetEnumDescription(o.AutoServiceInfoModel.ServiceType)
                            };

                            c.Cells[2] = new DataGridViewTextBoxCell()
                            {
                                Value = currentTaskWithGroupInfoStr
                            };

                            c.Cells[3] = new DataGridViewTextBoxCell()
                            {
                                Value = DataUtil.GetEnumDescription(o.AutoServiceInfoModel.Status)
                            };


                            Action _currentAction = () =>
                            {
                                c.Cells[4] = new DataGridViewTextBoxCell() { Value = "--/--" };
                            };
                            
                            if (o.AutoServiceInfoModel.Status != EnumTaskStatus.End && o.AutoServiceInfoModel.StartDate < DateTime.Now)
                            {
                                ValidateIsHaveTaskFromQueue(o.AutoServiceInfoModel.ID, viewModel =>
                                {
                                    if (null == viewModel)
                                    {
                                        _currentAction.Invoke();
                                    }
                                    else
                                    {
                                        var currentCell = new DataGridViewButtonCell() { Value = "取 消 任 务" };

                                        currentCell.Style = new DataGridViewCellStyle() { Padding = new Padding() { Left = 10, Right = 10 } };

                                        currentCell.Tag = o.AutoServiceInfoModel.ID;

                                        c.Cells[4] = currentCell;
                                    }

                                });


                            }
                            else
                            {
                                _currentAction.Invoke();
                            }

                        });

                    });
                }


            });


        }


        /// <summary>
        /// 点击单元格事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TaskListGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }

            var currentCell = this.taskListGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];

            if (null == currentCell || null == (currentCell as DataGridViewButtonCell))
            {
                return;
            }

            var id = 0;

            int.TryParse(Convert.ToString(currentCell.Tag), out id);

            ValidateIsHaveTaskFromQueue(id, async  (viewModel)=> {

                if (null==viewModel)
                {
                    return;
                }

                viewModel.Status = EnumTaskStatus.End;

                ///如果取消的是当前正在执行的任务 则得取消正在执行的任务
                if (id == viewModel.ID)
                {
                   await CancelCurrentTask();
                }

                UpdateTaskStateAndUI(viewModel, () =>
                {
                    this.taskListGridView.Rows[e.RowIndex].Cells[e.ColumnIndex - 1].Value = "结束";

                    this.taskListGridView.Rows[e.RowIndex].Cells[e.ColumnIndex] = new DataGridViewTextBoxCell() { Value = "--/--" };

                });

            });

        }

        /// <summary>
        /// 检测是否在任务队列中存在当前任务
        /// </summary>
        /// <param name="taskID"></param>
        /// <returns></returns>
        private void ValidateIsHaveTaskFromQueue(int taskID,Action<AutoServiceInfoViewModel> currentAction=null)
        {
            if (null == _taskQueue || _taskQueue.Count == 0)
            {
                return;
            }
            ///移除队列中的所有元素 并跟新任务状态
            lock (_taskLockObject)
            {
                if (null == _taskQueue || _taskQueue.Count == 0)
                {
                    return;
                }

                var len = _taskQueue.Count;

                var currentRunTask = _taskQueue.Peek();

                var taskModel = default(AutoServiceInfoViewModel);

                for (int i = 0; i < len; i++)
                {
                    var currentTask = _taskQueue.ElementAt(i);

                    if (currentTask.ID == taskID)
                    {
                        taskModel = currentTask;

                        break;
                    }

                }

                currentAction?.Invoke(taskModel);
            }
        }


        public void UpdateTaskStateAndUI(AutoServiceInfoViewModel viewModel,Action currentAction=null)
        {
            ///任务完成 跟新数据库任务状态
            _currentAction(viewModel.ID).ContinueWith((task) =>
            {

                var result = task.Result;

                if (result.ResultStatus != EnumStatus.Success)
                {
                    baseAction.SetContentWithCurrentThread<string>(this.taskListGridView, null, (control, str) =>
                    {
                        CustomDialog("取消任务失败");

                    });

                    return;

                }

                currentAction?.Invoke();

            });
        }
    }
}
