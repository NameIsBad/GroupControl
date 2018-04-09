using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GroupControl.WinForm
{
    public partial class SearchWithNumberForm : BaseForm
    {

        public  delegate void IdentificationEquipmentWithNumber(string number);

        public event IdentificationEquipmentWithNumber _identificationEquipmentWithNumber;

        public SearchWithNumberForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var text = this.textBox1.Text;

            if (string.IsNullOrEmpty(text))
            {
                MessageBox.Show("请输入设备编号！");

                return;
            }

            _identificationEquipmentWithNumber(text);
        }
    }
}
