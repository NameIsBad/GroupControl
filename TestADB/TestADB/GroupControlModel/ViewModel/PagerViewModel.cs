using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupControl.Model
{
    public class PagerViewModel<T> where T : class,new()
    {

        public int PageIndex { get; set; }

        public int PageCount { get; set; }

        public IList<T> ItemList { get; set; }

        public int TotalCount { get; set; }

    }
}
