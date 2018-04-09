using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroupControl.Model;

namespace GroupControl.Model
{
    public class UpdateWithTaskStateViewModel
    {
        public EnumTaskState TaskStatus { get; set; }

        public string EquipmentDevice { get; set; }

        public string ShowMessage { get; set; }


    }
}
