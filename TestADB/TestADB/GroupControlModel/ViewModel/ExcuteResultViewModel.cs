using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupControl.Model
{
    public class ExcuteResultViewModel
    {
        public string ResultString { get; set; }

        public int ResultID { get; set; }

        public EnumStatus ResultStatus { get; set; }
    }

    public class ExcuteResultViewModel<T> : ExcuteResultViewModel where T : class
    {
        public IList<T> List { get; set; }

        public T t { get; set; }
    }
}
