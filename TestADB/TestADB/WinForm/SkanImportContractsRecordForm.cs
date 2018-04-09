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
using GroupControl.Common;
using GroupControl.Helper;
using GroupControl.BLL;

namespace GroupControl.WinForm
{
    public partial class SkanImportContractsRecordForm : BaseForm
    {
        public SkanImportContractsRecordForm()
        {
            InitializeComponent();
        }

        private void SkanImportContractsRecordForm_Load(object sender, EventArgs e)
        {
            InitDataGridView(this.fiendListDataView, new Dictionary<string, string>() {

                { "导入时间","导入时间"},

                { "文件名称","文件名称"},

                { "数量","数量"},

                { "相关设备","相关设备"},

            });

            Task.Factory.StartNew<IList<ContractsInfoViewModel>>(() =>
            {
                var list = SingleHepler<ContractsInfoBLL>.Instance.GetListWithUnion(new ContractsInfoViewModel() { });

                return list;

            }).ContinueWith((task) =>
            {

                var returnData = task.Result;

                if (null != returnData && returnData.Count > 0)
                {
                    returnData.ToList().ForEach((item) =>
                    {
                        AddRowToDataGrridView(item, this.fiendListDataView, (c, o) =>
                        {
                            Func<string> _currentAction = () =>
                            {

                                if (null == o.DeviceToNickNameList || o.DeviceToNickNameList.Count == 0)
                                {
                                    return string.Empty;
                                }

                                return string.Join<string>("\r\n", o.DeviceToNickNameList.Select(q => { return string.Format("{0}({1})", q.NickName, q.ImportContentCount); }));
                            };
    
                            c.Cells[0] = new DataGridViewTextBoxCell() { Value = o.ContractsInfoModel.CreateDate.ToString("yyyy-MM-dd HH:ss") };

                            c.Cells[1] = new DataGridViewTextBoxCell() { Value = o.ContractsInfoModel.Name.ToString() };

                            c.Cells[2] = new DataGridViewTextBoxCell() { Value = o.ContractsInfoModel.RecordCount.ToString() };

                            c.Cells[3] = new DataGridViewTextBoxCell() { Value = _currentAction.Invoke() };

                        });

                    });
                }


            });

        }
    }
}
