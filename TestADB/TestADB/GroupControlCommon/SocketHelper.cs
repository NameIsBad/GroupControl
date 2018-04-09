using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GroupControl.Common
{
    public class SocketHelper
    {

        /// <summary>
        /// 接受消息
        /// </summary>
        /// <param name="action"></param>
        public void Reciver(Action<byte[],int> action,int port=8000,int reciveDataLen=40960)
        {
            Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IPAddress myIP = IPAddress.Parse("127.0.0.1");

            IPEndPoint EPhost = new IPEndPoint(myIP, port);

            try
            {
                client.Connect(EPhost);

                while (true)
                {

                    byte[] byteData = new byte[reciveDataLen];

                    int receiveNumber = client.Receive(byteData);

                    try
                    {

                        if (receiveNumber > 0)
                        {

                            action(byteData, receiveNumber);
                        }

                    }
                    catch (Exception ex)
                    {

                        Console.WriteLine(ex.Message);

                        client.Close();

                        break;
                    }

                    Thread.Sleep(70);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

               // client.Shutdown(SocketShutdown.Receive);

                client.Close();
            }
        }
    }
}
