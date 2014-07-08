using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    class Program
    {
        private const int PORT_NUMBER = 49000; // 1024 - 49151 : http://en.wikipedia.org/wiki/List_of_TCP_and_UDP_port_numbers
        private static UdpClient udpClient;

        private static void Main(string[] args)
        {
            udpClient = new UdpClient(PORT_NUMBER);
            udpClient.EnableBroadcast = true;
     
            Console.WriteLine("listening for messages...");
            BeginReceive();

            Console.WriteLine("Client Main done.");
            Console.ReadLine();
        }

        /// <summary>
        /// UDP receive call-back
        /// </summary>
        /// <param name="asyncResult"></param>
        private static void ReceiveCallback(IAsyncResult asyncResult)
        {
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, PORT_NUMBER);

            byte[] bytes = udpClient.EndReceive(asyncResult, ref ipEndPoint);
            
            string message = Encoding.ASCII.GetString(bytes);
            Console.WriteLine("From {0} received: {1} ", ipEndPoint.Address, message);

            // keep reciving... 
            BeginReceive();
        }

        private static void BeginReceive()
        {
            udpClient.BeginReceive(new AsyncCallback(ReceiveCallback), new object());
        }
    }
}
