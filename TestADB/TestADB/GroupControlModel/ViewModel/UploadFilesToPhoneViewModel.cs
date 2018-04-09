using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GroupControl.Model
{
    public class UploadFilesToPhoneViewModel:BaseViewModel
    {
        public IList<FileInfo> Files
        {
            get;set;
        }

        public string Content { get; set; }

        public string Path { get; set; }

        public EnumPublishContentType PublishContentType { get; set; }


    }
}
