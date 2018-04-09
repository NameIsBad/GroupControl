using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GroupControl.Common
{
    public  sealed class FileHelper
    {
        public static void ReadToFile(string fullName,string content)
        {
            try
            {
                using (var fileStream = new FileStream(fullName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
                {
                    using (var streamWriter = new StreamWriter(fileStream, Encoding.UTF8))
                    {
                        streamWriter.Write(content);

                        streamWriter.Close();
                    }

                    fileStream.Close();

                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
