using ServerCore;
using System;
using System.Net;
using System.Threading;

namespace Server
{
    class Program
    {
        static Listener _listener = new Listener();

        static void Main(string[] args)
        {
            PacketManager.Instance.Register();

            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

            _listener.Init(endPoint, () => new ClientSession());
            Console.WriteLine("Listening . . .");

            while (true)
            {
                ;
            }

        }
    }
}
