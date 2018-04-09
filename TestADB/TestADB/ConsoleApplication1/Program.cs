using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {

            var currentByte = new byte[4] { 0, 0, 0, 0 };

            var ss = currentByte.Where(o => o > 0).ToList();

            Console.Read();


        }
    }
}
