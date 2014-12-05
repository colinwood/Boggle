﻿using CustomNetworking;
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

        public delegate void UpdateScore(String p1_score, String p2_score);
        public delegate void UpdateTime(String p1_score, String p2_score);
        public delegate void StartGame(string board, int time, string opponent );
        public delegate void EndGame(string GameSummary);
        public delegate void OpponentDisconnect();
        public delegate void ServerDisconnect();

        public event UpdateScore UpdateScoreEvent;
        public event UpdateTime UpdateTimeEvent;
        public event StartGame StartGameEvent;
        public event EndGame EndGameEvent;
        public event OpponentDisconnect OpponentDisconnectEvent;
        public event ServerDisconnect ServerDisconnectEvent;



        private int time;
        private string board;
        private int self_score;
        private int opponent_score;
        private string self_name;
        private string opponent_name;
        private string ip_address;
        private string messages;
        private string enter_word;
        private StringSocket ss;
        private  ManualResetEvent allDone = new ManualResetEvent(false);
        private TcpClient client;
        private Socket clientSocket;

        public Model()
        {
        }

        public void Connect(string name, string ip_address){

            this.ip_address = ip_address;
            this.messages = "Looking for an Opponent";
            this.enter_word = "";
            client = new TcpClient(ip_address, 2000);
            clientSocket = client.Client;
            ss = new StringSocket(clientSocket, new UTF8Encoding());
            
            this.self_name = name;

            if (ss.SocketConnected())
            {
                ss.BeginSend("PLAY " + name + "\n", SendMessageCallback, name);
                ss.BeginReceive(NewGameCallBack, ss);
                allDone.WaitOne();
                Thread t1 = new Thread(() => Listen());
                t1.Start();
            }
        }

        private void SendMessageCallback(Exception e, object o)
        {
            if (e != null)
            {
                ss.Close();
                clientSocket.Close();
            }
        }

        private void Listen()
        {
            if (ss.SocketConnected())
            {
                ss.BeginReceive(ListenCallback, ss);
                allDone.WaitOne();
            }
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
                ss.Close();
            }
            else if (s.Contains("STOP"))
            {
               s = s.Substring(6);
               string[] game_summary = Regex.Split(s, @"[0-9]+");
               
               messages = String.Join("\n", game_summary );
               ss.Close();
            }
            allDone.Set();
            Listen();
        }

        public void NewGameCallBack(String s, Exception e, object payload)
        {

            if (e != null)
            {
                ss.Close();
                clientSocket.Close();
            }
            else
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
        }

        public void PlayWord(String s)
        {
            ss.BeginSend("WORD " + s + "\n", (e, o) => { }, s);
        }
    }
}
