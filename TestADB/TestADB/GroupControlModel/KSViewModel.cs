using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupControl.Model
{
    public class KSViewModel:BaseViewModel
    {
        public IList<string> PhotoIDList
        {
            get
            {
                if (!string.IsNullOrEmpty(PhotoIDs))
                {
                    return PhotoIDs.Replace("\r\n","@").Split('@').ToList();
                }

                return default(IList<string>);
            }
        }

        public IList<string> CommentList {

            get
            {
                if (!string.IsNullOrEmpty(Comments))
                {
                    return Comments.Replace("\r\n", "@").Split('@').ToList();
                }

                return default(IList<string>);
            }

        }

        public IList<string> Devices { get; set; }

        public string PhotoIDs { get; set; }

        public string Comments { get; set; }


    }
}
