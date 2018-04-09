using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupControl.Model
{
    public class ContractsInfo
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public DateTime CreateDate { get; set; }
        public int RecordCount { get; set; }
    }
}
