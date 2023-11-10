using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerApplication
{
    public class ServerAddress
    {
        private static int localPortNumber = new Random(1234).Next(2000, 10000);

        public static int LocalPortNumber
        {
            get { return localPortNumber; }
        }
        
        public static string GetLocalIP()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork && ip.ToString() != "192.168.56.1")
                {
                    return ip.ToString();
                }
            }
            return "?";
        }

        public static string LocalIPNumber
        {
            get { return GetLocalIP(); }
        }

    }
}
