using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Timers;

namespace Server
{
    class Program
    {
        private const int PORT_NUMBER = 49000; // 1024 - 49151 : http://en.wikipedia.org/wiki/List_of_TCP_and_UDP_port_numbers
        private static Timer broadcastTimer;

        private static void Main(string[] args)
        {
            broadcastTimer = new System.Timers.Timer(5000); // real value much bigger - client to store / cache.
            broadcastTimer.Elapsed += OnTimedEvent;
            broadcastTimer.Enabled = true;

            Console.WriteLine("Server Main done.");
            Console.ReadLine();
        }

        /// <summary>
        /// Timer elapsed handle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            Console.Write("Timer elapsed...");
            broadcast();
        }

        /// <summary>
        /// Broadcast our local IP.
        /// </summary>
        private static void broadcast()
        {
            UdpClient udpClient = new UdpClient();
            udpClient.EnableBroadcast = true;
            IPEndPoint ip = new IPEndPoint(IPAddress.Parse("255.255.255.255"), PORT_NUMBER);

            string message = "FTM @ " + LocalIpAddress();
            byte[] bytes = Encoding.ASCII.GetBytes(message);

            using (udpClient)
            {
                udpClient.Send(bytes, bytes.Length, ip);
                udpClient.Close();
            }

            Console.WriteLine("Sent: {0} ", message);
        }

        /// <summary>
        /// get the Ip of the first network adaptor
        /// </summary>
        /// <returns></returns>
        private static  IPAddress LocalIpAddress()
        {
            if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

                return host
                    .AddressList
                    .FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);
            }

            return null;
        }
    }
}
