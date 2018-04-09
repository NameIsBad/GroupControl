using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupControl.Model
{
    public class ContractInfoDetail
    {
        public int ID { get; set; }


        public int ContractsInfoID { get; set; }

        public string Device { get; set; }

        public string Content { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
