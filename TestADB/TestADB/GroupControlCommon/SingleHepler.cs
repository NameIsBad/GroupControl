using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupControl.Common
{
    public class SingleHepler<T> where T:class,new()
    {
        private readonly static object _obj = new object();

        private static T t;

        public static T Instance
        {
            get
            {
                if (null == t)
                {
                    lock (_obj)
                    {
                        if (null==t)
                        {
                            t = new T();
                        }
                    }
                }

                return t;

            }
        }
    }
}
