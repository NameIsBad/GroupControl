using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GroupControl.Model;

namespace GroupControl.Model
{
   public  class AreaToPhoneNumberViewModel:BaseViewModel
    {
        public string LinkUrl { get; set; }

        public string AreaCode { get; set; }

        public int Type { get; set; }

        public string Header { get; set; }
    }
}
