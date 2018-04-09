using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupControl.Model
{
   public  class ComputerInfo
    {

        public DateTime? CreateDate { get; set; }

        public DateTime? ExpireDate { get; set; }

        public int ID { get; set; }

        public string MACAddress { get; set; }
    }
}
