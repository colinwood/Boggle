using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CustomNetworking;

namespace DummyClient
{
    class Program
    {
        static StringSocket ss;
        public static ManualResetEvent allDone = new ManualResetEvent(false);

        static void Main(string[] args)
        {

            TcpClient client = new TcpClient("localhost", 2000);
            Socket clientSocket = client.Client;
            ss = new StringSocket(clientSocket, new UTF8Encoding());
            Console.Write("You are Connected To the server Boggle warrior! \n\n What is your name?");
            String name = Console.ReadLine();

            ss.BeginSend("PLAY " + name + "\n", (e, o) => { }, name);
            ss.BeginReceive(NewGameCallBack, ss);  
            allDone.WaitOne();

            ThreadPool.QueueUserWorkItem(GetTimer);
            while (true)
            {
                string word = Console.ReadLine();
                ss.BeginSend(word + "\n", (e, o) => { }, name);
            }
        }

        public static void NewGameCallBack(String s, Exception e, object payload)
        {
            Console.WriteLine(s);
            allDone.Set();
        }

        private static void GetTimer(Object o)
        {
            ss.BeginReceive(TimerCallBack, ss);
            allDone.WaitOne();
        }
        public static void TimerCallBack(String s, Exception e, object payload)
        {   
            Console.WriteLine(s);
            allDone.Set();
            ss.BeginReceive(TimerCallBack, ss);
            allDone.WaitOne();
        }
       
    }
}
