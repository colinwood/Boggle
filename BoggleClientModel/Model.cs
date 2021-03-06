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
    /// <summary>
    /// Represent the M in mvc. Communicates with the gui and the server 
    /// to allow for smooth seperation of logic and control. 
    /// </summary>
    public class Model
    {
        /// <summary>
        /// Event delegates for the individual occurences within a game
        /// </summary>
        
        public delegate void Connected(Boolean connection_made);
        public delegate void UpdateScore(String p1_score, String p2_score);
        public delegate void UpdateTime(String time);
        public delegate void StartGame(string board, int time, string opponent );
        public delegate void EndGame(string GameSummary);
        public delegate void OpponentDisconnect();
        public delegate void ServerDisconnect();

        /// <summary>
        /// Events to be fired at the corresponding times. 
        /// </summary>
        public event Connected ConnectEvent;
        public event UpdateScore UpdateScoreEvent;
        public event UpdateTime UpdateTimeEvent;
        public event StartGame StartGameEvent;
        public event EndGame EndGameEvent;
        public event OpponentDisconnect OpponentDisconnectEvent;
        public event ServerDisconnect ServerDisconnectEvent;


        /// <summary>
        /// Private member variables that are pretty self explanatory
        /// </summary>
        private int time;
        private string board;
        private int self_score;
        private int opponent_score;
        private string self_name;
        private string opponent_name;
        private string ip_address;
        private Boolean server_connected;
        
        private StringSocket ss;
        private  ManualResetEvent allDone = new ManualResetEvent(false);
        private TcpClient client;
        private Socket clientSocket;

        public Model()
        {
        }

        /// <summary>
        /// OPens up a socket and connects to the appropriate server using the geven ip address
        /// </summary>
        /// <param name="name"></param>
        /// <param name="ip_address"></param>
        public void Connect(string name, string ip_address){

            this.ip_address = ip_address;
            
            
            try
            {
                client = new TcpClient(ip_address, 2000);
                clientSocket = client.Client;
                ss = new StringSocket(clientSocket, new UTF8Encoding());
                
                
                if (ss.SocketConnected())
                {
                    
                    ConnectEvent(true);
                    ThreadPool.QueueUserWorkItem(o => MoniterServer());
                    ss.BeginSend("PLAY " + name + "\n", SendMessageCallback, name);
                    ss.BeginReceive(NewGameCallBack, ss);
                    allDone.WaitOne();
                    Thread t1 = new Thread(() => Listen());
                    t1.Start();
                }
            }
            catch (Exception e)
            {
                ConnectEvent(false);
                
            }
            this.self_name = name;

            
        }

        /// <summary>
        /// Loop that checks on the server staus every half second and 
        /// fires the server disconnect event if the server shuts down.
        /// </summary>
        private void MoniterServer()
        {
            server_connected = true;
            while (server_connected)
            {
                Thread.Sleep(500);
                server_connected = ss.SocketConnected();
            }
            ServerDisconnectEvent();
        }

        /// <summary>
        /// Callback for when message is sent. Checks for exceptions. 
        /// </summary>
        /// <param name="e"></param>
        /// <param name="o"></param>
        private void SendMessageCallback(Exception e, object o)
        {
            if (e != null)
            {
                ss.Close();
                clientSocket.Close();
            }
        }


        /// <summary>
        /// Constantly listens to the server for connections. 
        /// </summary>
        private void Listen()
        {

            if (ss.SocketConnected())
            {
                ss.BeginReceive(ListenCallback, ss);
                allDone.WaitOne();
            }
        }

        /// <summary>
        /// Closes the socket connection appropriately
        /// </summary>
        public void Disconnect()
        {
            ss.Close();
        }

        /// <summary>
        /// Deciphers what typ eof message the server send and fires off the appropriate event accordingly. 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        /// <param name="payload"></param>
        public void ListenCallback(String s, Exception e, object payload)
        {
            
                s = s.ToUpper();
                if (s.Contains("TIME"))
                {
                    s = s.Substring(5);
                    Int32.TryParse(s, out time);
                    allDone.Set();
                    UpdateTimeEvent(time.ToString());
                    Listen();
                }
                else if (s.Contains("SCORE"))
                {
                    string[] incoming = Regex.Split(s, @"[\s]");
                    Int32.TryParse(incoming[1], out self_score);
                    Int32.TryParse(incoming[2], out opponent_score);
                    UpdateScoreEvent(self_score.ToString(), opponent_score.ToString());
                    allDone.Set();
                    Listen();
                }
                else if (s.Contains("IGNORING"))
                {                   
                    allDone.Set();
                    Listen();
                }
                else if (s.Contains("TERMINATED"))
                {
                    
                    OpponentDisconnectEvent();
                    ss.Close();
                    
                }
                else if (s.Contains("STOP"))
                {
                    
                    s = s.Substring(5);
                    //0 word 0 word 0 word 0 word 0 word
                    string[] game_summary = Regex.Split(s, @"[0-9]");
                    StringBuilder stats = new StringBuilder();
                    for(int i = 1; i < 6 ; i++){
                        
                        if (i == 1)
                        {
                            stats.AppendLine("Player legal words:");
                        }
                        if (i == 2)
                        {
                            stats.AppendLine("Opponent legal words:");
                        }
                        if (i == 3)
                        {
                            stats.AppendLine("Common words:");
                        }
                        if (i == 4)
                        {
                            stats.AppendLine("Player illegal words:");
                        }
                        if (i == 5)
                        {
                            stats.AppendLine("Opponent illegal words:");
                        }

                        stats.AppendLine(game_summary[i] + "\n");  
                  
                    }
                    
                    EndGameEvent(stats.ToString());
                    ss.Close();
                }
                
            
            
        }

        /// <summary>
        /// Fires the appropriate event callback for a new game
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        /// <param name="payload"></param>
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
                    StartGameEvent(this.board, this.time, this.opponent_name);
                }
                allDone.Set();
            }
        }

        /// <summary>
        /// Sends the word to play. 
        /// </summary>
        /// <param name="s"></param>
        public void PlayWord(String s)
        {
            ss.BeginSend("WORD " + s + "\n", (e, o) => { }, s);
        }
    }
}
