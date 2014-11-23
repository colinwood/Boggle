using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CustomNetworking;
using BB;
using System.Threading;

namespace BoggleServer
{
    class BoggleServer
    {
        private Queue<Player> player_queue;
        private HashSet<BoggleGame> game_set;
        private TcpListener server;
        private HashSet<String> dictionary;
        private string default_board;
        private object lock_object = new Object();
        public static ManualResetEvent allDone = new ManualResetEvent(false);
        private int time_limit; 

        static void Main(string[] args)
        {
            int time_limit = 0;
            string dictionary_path = args[1];
            HashSet<String> dictionary = new HashSet<string>();

            //Check to see if there are the right number of arguments
            if (args.Length < 2 || args.Length > 3)
            {
                Console.WriteLine("Error, inccorect number of arguments.\n Please enter the game timer amount and the path to a dictionary file!");
                return;
            }

            //No default board provided proceed accordingly
            else if(args.Length == 2)
            {
                if(Int32.TryParse(args[0], out time_limit) && IsValidPath(args[1]))
                {
                    dictionary = ParseDictionary(dictionary_path, dictionary);
                    if(dictionary.Count != 0)
                    {
                        BoggleServer boggle_server = new BoggleServer(2000, time_limit, dictionary, "");     
                    }           
                }
                else{
                    Console.WriteLine("One of the argumnets you passed in was not valid please try again! ");
                }
            }
            
            //Default board provided we need to parse that txt file and proceed accodingly. 
            else if(args.Length == 3)
            {
                if(Int32.TryParse(args[0], out time_limit) && IsValidPath(args[1]) && IsValidDefaultBoard(args[2])){
                    dictionary = ParseDictionary(dictionary_path, dictionary);
                    if (dictionary.Count != 0)
                    {
                        BoggleServer boggle_server = new BoggleServer(2000, time_limit, dictionary, args[2]);
                    }  
                }
                else{
                    Console.WriteLine("Invalid default board please enter a string of 16 characters! ");
                }
            }

        }

        private static HashSet<string> ParseDictionary(string dictionary_path, HashSet<string> dictionary)
        {
            try
            {
                using (FileStream file_stream = File.Open(dictionary_path, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(file_stream))
                    {
                        while (!reader.EndOfStream)
                        {
                            dictionary.Add(reader.ReadLine().Trim().ToUpper());
                        }
                    }
                }
                Console.WriteLine("Succeffuly loaded file");
                return dictionary;
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
                return new HashSet<string>();
            }

            
        }

        private static bool IsValidDefaultBoard(string default_board)
        {
            return Regex.IsMatch(default_board, @"([a-z]|[A-Z]){16}");

        }

        private static bool IsValidPath(string path)
        {
            return Regex.IsMatch(path, @".*txt");
        }
        
        public BoggleServer(int port, int time_limit, HashSet<string> dictionary, string default_board)
        {
                this.time_limit = time_limit;
                this.dictionary = dictionary;
                this.default_board = default_board;
                
                this.player_queue = new Queue<Player>();
                this.game_set = new HashSet<BoggleGame>();
                this.server = new TcpListener(IPAddress.Any, 2000);
                this.server.Start();

                ServerStatus("Waiting for players to connect...");
                ThreadPool.QueueUserWorkItem(UpdateServer);
                server.BeginAcceptSocket(PlayerConnected, null);
                allDone.WaitOne();
                
                
        }

        private void PlayerConnected(IAsyncResult result)
        {
            lock (player_queue)
            {
                ServerStatus("New Player Connected");
                Socket listener;
                try
                {
                    listener = server.EndAcceptSocket(result);          
                    StringSocket string_socket = new StringSocket(listener, new UTF8Encoding());
                    string_socket.BeginReceive(WaitForName, string_socket);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                server.BeginAcceptSocket(PlayerConnected, null);
            } 
        }

        private bool IsValidPlayCommand(string s)
        {
            return Regex.IsMatch(s, @"PLAY\s\.*");
        }

        private void WaitForName(String line, Exception e, object socket)
        {      
            lock (player_queue)
            {
                
                StringSocket player_socket = (StringSocket) socket;
                if (IsValidPlayCommand(line) && player_socket.SocketConnected())
                {
                    string player_name = line.Substring(5);
                    Console.WriteLine("Player " + player_name + " is ready");
                    Player new_player = new Player(player_socket, line.Substring(5));
                    
                    if (player_queue.Count % 2 == 0)
                    {
                        player_queue.Enqueue(new_player);
                        ServerStatus(line.Substring(5) + " is ready to play.");
                    }
                    else
                    {
                        Player player_1 = player_queue.Dequeue();
                        Player player_2 = new_player;
                        BoggleGame new_game = new BoggleGame(player_1, player_2, this.time_limit, this.default_board, this.dictionary);
                        game_set.Add(new_game);
                        ServerStatus("Player connected and game started : " + player_1.player_name + " vs " + player_2.player_name );
                    }
                }
            }          
        }

        private void ServerStatus(string message)
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            int players = this.player_queue.Count + (game_set.Count * 2);
            Console.WriteLine("Boggle Server Status");
            Console.WriteLine("Players Connected : " + players);
            Console.WriteLine("Active Games : " + game_set.Count);
            foreach (BoggleGame game in game_set)
            {
                Console.WriteLine(game.ToString());
            }
            Console.WriteLine(message);
        }

        private void UpdateServer(Object o)
        {
            while (true)
            {
                Thread.Sleep(1000);
                ServerStatus("");
            }      
        }

        private class BoggleGame
        {
            private Player player_1;
            private Player player_2;
            private int game_timer;
            private BoggleBoard game_board;
            private HashSet<string> player_1_words;
            private HashSet<string> player_2_words;
            private HashSet<string> dictionary;
            private int player_1_score;
            private int player_2_score;
            public static ManualResetEvent listen_1 = new ManualResetEvent(false);
            public static ManualResetEvent listen_2 = new ManualResetEvent(false);
            
            public bool players_connected{get;set;}

            public BoggleGame(Player player_1, Player player_2, int game_timer, string default_board, HashSet<string> dictionary)
            {
                this.player_1 = player_1;
                this.player_2 = player_2;
                this.game_timer = game_timer;
                this.dictionary = dictionary;
                this.player_1_words = new HashSet<string>();
                this.player_2_words = new HashSet<string>();

                this.player_1_score = 0;
                this.player_2_score = 0;
                this.players_connected = true;

                if (default_board.Length == 16)
                    this.game_board = new BoggleBoard(default_board);
                else
                    this.game_board = new BoggleBoard();
                NewGame();

            }

            public override string ToString()
            {
                return String.Format(player_1.player_name + " " + player_1_score + " vs " + player_2.player_name + " " + player_2_score);
            }
            private void NewGame()
            {
                player_1.string_socket.BeginSend("START " + this.game_board.ToString() + " " + this.game_timer + " " + player_2.player_name + "\n", (e, o) => { }, player_1.string_socket);
                player_2.string_socket.BeginSend("START " + this.game_board.ToString() + " " + this.game_timer + " " + player_1.player_name + "\n", (e, o) => { }, player_2.string_socket);

                //Use a different thread to send the time and listen to each player.
                ThreadPool.QueueUserWorkItem(CheckConnections);
                Thread t1 = new Thread(SendTime);
                Thread t2 = new Thread(Listen1);
                Thread t3 = new Thread(Listen2);
                Thread t4 = new Thread(SendScore);
                Thread[] threads = {t1,t2,t3,t4};

                foreach (Thread t in threads)
                {
                    t.Start();
                }
                
                
            }
            private void Listen1()
            {
                if (!players_connected)
                {
                    return;
                }
                player_1.string_socket.BeginReceive(RCallback, player_1);
                listen_1.WaitOne();
            }
            private void Listen2()
            {
                if (!players_connected)
                {
                    return;
                }
                player_2.string_socket.BeginReceive(RCallback, player_2);
                listen_2.WaitOne();
            }
            private void RCallback(String s, Exception e, Object payload)
            {
                
                if (players_connected)
                {
                    if ((Player)payload == player_1 && game_timer > 0)
                    {
                        player_1_words.Add(s.ToUpper());
                        Console.WriteLine("Word received from " + player_1.player_name + " : " + s);
                        Listen1();
                    }
                    else if ((Player)payload == player_2 && game_timer > 0)
                    {
                        Console.WriteLine("Word received from " + player_2.player_name + " : " + s);
                        player_2_words.Add(s.ToUpper());
                        Listen2();
                    }
                }
                else
                {
                    return;
                }
            }
            private void Update_Score()
            {
                //update player 1 score
                player_1_score = 0;
                player_2_score = 0;

                HashSet<String> toDelete = new HashSet<string>();

                foreach(string word in player_1_words)
                {
                    if (word.Length < 3)
                    {
                        toDelete.Add(word);
                    }
                    else if (player_2_words.Contains(word))
                    {
                        toDelete.Add(word);
                    }
                    else if (!this.dictionary.Contains(word))
                    {
                        player_1_score--;
                    }
                    else if (word.Length == 3 || word.Length == 4)
                    {
                        player_1_score++;
                    }
                    else if (word.Length == 5)
                    {
                        player_1_score = player_1_score + 2;
                    }
                    else if (word.Length == 6)
                    {
                        player_1_score = player_1_score + 3;
                    }
                    else if (word.Length == 7)
                    {
                        player_1_score = player_1_score + 5;
                    }
                    else if (word.Length > 7)
                    {
                        player_1_score = player_1_score + 11;
                    }
                }
                //Update player 2 score
                foreach (string word in player_2_words)
                {
                    if (word.Length < 3)
                    {
                        toDelete.Add(word);
                    }
                    else if (player_1_words.Contains(word))
                    {
                        toDelete.Add(word);
                    }
                    else if (!this.dictionary.Contains(word))
                    {
                        player_2_score--;
                    }
                    else if (word.Length == 3 || word.Length == 4)
                    {
                        player_2_score++;
                    }
                    else if (word.Length == 5)
                    {
                        player_2_score = player_2_score + 2;
                    }
                    else if (word.Length == 6)
                    {
                        player_2_score = player_2_score + 3;
                    }
                    else if (word.Length == 7)
                    {
                        player_2_score = player_2_score + 5;
                    }
                    else if (word.Length > 7)
                    {
                        player_2_score = player_2_score + 11;
                    }

                }

                foreach(String word in toDelete)
                {
                        player_1_words.Remove(word);
                        player_2_words.Remove(word);
                }
            }
            private void SendScore(Object thingy)
            {
                if (!players_connected)
                {
                    return;
                }
                while (this.game_timer > 0 && players_connected)
                {          
                        Thread.Sleep(500);
                        int p1 = player_1_score;
                        int p2 = player_1_score;
                        Update_Score();
                        if (p1 != player_1_score || p2 != player_2_score)
                        {
                            player_1.string_socket.BeginSend("SCORE " + this.player_1_score + " " + this.player_2_score + "\n", (e, o) => { }, player_1.string_socket);
                            player_2.string_socket.BeginSend("SCORE " + this.player_1_score + " " + this.player_2_score + "\n", (e, o) => { }, player_1.string_socket);
                        }                   
                }

            }
            private void SendTime()
            {
                
                while (this.game_timer > 0 && players_connected)
                {
                    Thread.Sleep(1000);
                    if (player_1.string_socket.SocketConnected() && player_2.string_socket.SocketConnected())
                    {
                        player_1.string_socket.BeginSend(game_timer.ToString() + "\n", (e, o) => { }, player_1.string_socket);
                        player_2.string_socket.BeginSend(game_timer.ToString() + "\n", (e, o) => { }, player_2.string_socket);
                        this.game_timer--;
                    }
                }
            }

            /// <summary>
            /// Methd that checks that each player is connected evry .5 seconds if for any reason one player disconnects
            /// checks if either player is still connected and sends them a message
            /// </summary>
            /// <param name="mickey"></param>
            private void CheckConnections(Object mickey)
            {
                while (true)
                {
                    //sleep for 1/2 a second to conserve resources
                    Thread.Sleep(500);
                    if(!player_1.string_socket.SocketConnected() || !player_2.string_socket.SocketConnected())
                    {
                        if (player_1.string_socket.SocketConnected())
                        {
                            player_1.string_socket.BeginSend("TERMINATED\n", (e, o) => { }, new Object());
                        }
                        if (player_2.string_socket.SocketConnected())
                        {
                            player_2.string_socket.BeginSend("TERMINATED\n", (e, o) => { }, new Object());
                        }
                        players_connected = false;
                        player_1.string_socket.Close();
                        player_2.string_socket.Close();
                        break;
                    }
                }
            }
        }
        
        private class Player
        {
            public string player_name { get; set; }
            public StringSocket string_socket { get; set; }
            
            public Player(StringSocket string_socket, string player_name)
            {
                this.string_socket = string_socket;
                this.player_name = player_name; 
            }
        }
    }
}
