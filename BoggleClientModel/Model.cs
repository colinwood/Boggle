using CustomNetworking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace BoggleClientModel
{
    public class Model
    {
        public int time;
        public string board;
        public int self_score;
        public int opponent_score;
        public string self_name;
        public string opponent_name;
        public string ip_address;
        public string messages;
        public string enter_word;
        private StringSocket ss;
        private  ManualResetEvent allDone = new ManualResetEvent(false);

        public Model()
        {
        }

        public void Connect(string name, string ip_address){

            this.ip_address = ip_address;
            this.messages = "Looking for an Opponent";
            this.enter_word = "";
            TcpClient client = new TcpClient(ip_address, 2000);
            Socket clientSocket = client.Client;
            ss = new StringSocket(clientSocket, new UTF8Encoding());
            
            this.self_name = name;

            ss.BeginSend("PLAY " + name + "\n", (e, o) => { }, name);
            ss.BeginReceive(NewGameCallBack, ss);  
            allDone.WaitOne();
            
            Thread t1 = new Thread(() => Listen());
            t1.Start();

        }

        private void Listen()
        {
            ss.BeginReceive(ListenCallback, ss);
            allDone.WaitOne(); 
        }

        public void ListenCallback(String s, Exception e, object payload)
        {
            s = s.ToUpper();
            if (s.Contains("TIME"))
            {
                s = s.Substring(6);
                Int32.TryParse(s, out time);
            } 
            else if (s.Contains("SCORE"))
            {
                 string[] incoming = Regex.Split(s, @"[\s]");
                 Int32.TryParse(incoming[1], out self_score);
                 Int32.TryParse(incoming[2], out opponent_score);
            }
            else if (s.Contains("IGNORING"))
            {
                messages = "Please don't try to hack our server, it is smarter than you are";
            }
            else if (s.Contains("TERMINATED"))
            {
                messages = "Congratulations your opponent gave up! You Win!";
            }
            else if (s.Contains("STOP"))
            {
               s = s.Substring(6);
               string[] game_summary = Regex.Split(s, @"[0-9]+");
          
               messages = String.Join("\n", game_summary );
            }
            allDone.Set();
            Listen();            

        }

        public void NewGameCallBack(String s, Exception e, object payload)
        {
            string[] incoming_message = Regex.Split(s, @"[\s]");
            if (incoming_message[0] == "START")
            {
                board = incoming_message[1];
                Int32.TryParse(incoming_message[2], out time);
                opponent_name = incoming_message[3];
                self_score = 0;
                opponent_score = 0;
            }
            allDone.Set();
        }

        public void PlayWord(String s)
        {
            ss.BeginSend("WORD " + s + "\n", (e, o) => { }, s);
        }
    }
}
