using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupControl.Model.ViewModel
{
   public  class IdleFishActionViewModel:BaseViewModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }

        public string CurrentContent { get; set; }
    }
}
