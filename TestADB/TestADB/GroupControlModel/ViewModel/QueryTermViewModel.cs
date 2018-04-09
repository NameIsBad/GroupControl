using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DapperExtensions;

namespace GroupControl.Model
{
    public class QueryTermViewModel
    {
        public PredicateGroup PredGroup { get; set; }

        public IList<ISort> SortList { get; set; } 

        
    }
}
