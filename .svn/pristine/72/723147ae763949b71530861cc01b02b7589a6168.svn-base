// Authored By Victor Marruffo & Colin Wood
// 11/25/2014
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

    /// <summary>
    /// Multithreaded server that allows connection of clients to play boggle. 
    /// Requires that after a client connects that they send the appropriate Play command
    /// and then pairs the first two users in a game. After the game has finished disconnects players
    /// and send a game summary
    /// </summary>
    public class BoggleServer
    {
        /// <summary>
        /// Queue to hold the player waiting to play
        /// </summary>
        private Queue<Player> player_queue;

        /// <summary>
        /// Set that contains the active games.The server does check on the connection of the players
        /// and removes the game if theplayers are disconnected. 
        /// </summary>
        private HashSet<BoggleGame> game_set;

        /// <summary>
        /// Tcp listenter for the client to communicate to the server with. 
        /// </summary>
        private TcpListener server;

        /// <summary>
        /// Underlying dictionary that contains a list of legal words.
        /// </summary>
        private HashSet<String> dictionary;

        /// <summary>
        /// A user input board of 16 characters
        /// </summary>
        private string default_board;

        /// <summary>
        /// Object used to prevent thread overlap. 
        /// </summary>
        private object lock_object = new Object();

        /// <summary>
        /// Reset event object used to tell the server when the first client has connected. 
        /// </summary>
        public static ManualResetEvent allDone = new ManualResetEvent(false);

        /// <summary>
        /// Default time limit to be used for each game. 
        /// </summary>
        private int time_limit; 

        public static void Main(string[] args)
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

        public void Stop()
        {
            this.server.Stop();
        }
        /// <summary>
        /// Iterates over a given txt file and puts it into a hash set game_set.
        /// </summary>
        /// <param name="dictionary_path">path to a given dictionary.txt file</param>
        /// <param name="dictionary">the hash set to have the dictionary to</param>
        /// <returns></returns>
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

        /// <summary>
        /// Checks for a 16 character string of letters. 
        /// </summary>
        /// <param name="default_board">string of characters to be checked</param>
        /// <returns></returns>
        private static bool IsValidDefaultBoard(string default_board)
        {
            return Regex.IsMatch(default_board, @"([a-z]|[A-Z]){16}");

        }
        /// <summary>
        /// ensure that the dictionary is a txt file
        /// </summary>
        /// <param name="path">Path to the txt file</param>
        /// <returns></returns>
        private static bool IsValidPath(string path)
        {
            return Regex.IsMatch(path, @".*txt");
        }
        
        /// <summary>
        /// Constructor that start up an instance of the boggle server.
        /// </summary>
        /// <param name="port">Port which clients can access the server. </param>
        /// <param name="time_limit">How long will all of the games be</param>
        /// <param name="dictionary">Dictionary to lookup and see words are valid. </param>
        /// <param name="default_board">16 character string that will be used to craete a board</param>
        public BoggleServer(int port, int time_limit, HashSet<string> dictionary, string default_board)
        {
                //initiate all the private member vairables accordingly
                this.time_limit = time_limit;
                this.dictionary = dictionary;
                this.default_board = default_board;               
                this.player_queue = new Queue<Player>();
                this.game_set = new HashSet<BoggleGame>();
                
                //Create a new listener on port 2000 and fire up the server
                this.server = new TcpListener(IPAddress.Any, 2000);
                this.server.Start();

               
                ServerStatus("Waiting for players to connect...");
                //Start up a thread that updates the server status once every second. 
                ThreadPool.QueueUserWorkItem(UpdateServer);
                
                //start accepting new players on the server
                server.BeginAcceptSocket(PlayerConnected, null);
                allDone.WaitOne();         
        }


        /// <summary>
        /// What happens once a player connects. 
        /// </summary>
        /// <param name="result"></param>
        private void PlayerConnected(IAsyncResult result)
        {
            lock (player_queue)
            {
                server.BeginAcceptSocket(PlayerConnected, null); //start accepting more players. 
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
                
            } 
        }

        /// <summary>
        /// Checks to see if the user has said they would like to play  with their name after connecintg. 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private bool IsValidPlayCommand(string s)
        {
            return Regex.IsMatch(s, @"PLAY\s\.*");
        }


        /// <summary>
        /// What happens once the player send the play command. 
        /// </summary>
        /// <param name="line">The players name</param>
        /// <param name="e">Exception thrown if there is an issue receiving</param>
        /// <param name="socket">//UNderlying string socket used to communicate with this player.</param>
        private void WaitForName(String line, Exception e, object socket)
        {      
            //stop other threads here
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
                //ignore any invalid commands
                else if (!IsValidPlayCommand(line) && player_socket.SocketConnected())
                {
                    player_socket.BeginSend("IGNORING" + line, null, null );
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

        /// <summary>
        /// Updates the console displaying information about how many games are running
        /// and how many players are active. 
        /// </summary>
        /// <param name="o"></param>
        private void UpdateServer(Object o)
        {
            while (true)
            {
                Thread.Sleep(200);

                //Create a list and find all the games that don't have connected players. 
                HashSet<BoggleGame> to_delete = new HashSet<BoggleGame>();
                foreach (BoggleGame game in game_set)
                {
                    if (!game.players_connected)
                    {
                        to_delete.Add(game);
                    }
                }
                foreach (BoggleGame game in to_delete)
                {
                    game_set.Remove(game);
                }
                //Check for players waiting that are not connected
                HashSet<Player> players_to_delete = new HashSet<Player>();
                foreach (Player player in player_queue)
                {
                    if (!player.string_socket.SocketConnected())
                    {
                        players_to_delete.Add(player);
                    }
                }
                foreach (Player p in players_to_delete)
                {
                    player_queue.Dequeue();
                }
                ServerStatus("");
            }      
        }

        /// <summary>
        /// Boggle game class that contains the logic for pairing players in a game and keeping 
        /// score of the words played.
        /// </summary>
        private class BoggleGame
        {
            /// <summary>
            /// Player objects that are used to keep track of the incoming messages. 
            /// </summary>
            private Player player_1;
            private Player player_2;

            private int game_timer;
            /// <summary>
            /// Letters the player can use to submit game objects. 
            /// </summary>
            private BoggleBoard game_board;
            
            private HashSet<string> player_1_words;
            private HashSet<string> player_2_words;
            
            /// <summary>
            /// Dictionary used to check players words submitted. 
            /// </summary>
            private HashSet<string> dictionary;
            
            /// <summary>
            /// Score objects used to keep track of each players score
            /// </summary>
            private int player_1_score;
            private int player_2_score;
            

            /// <summary>
            /// Threading objecnts used for telling the thread to wait until it hears ssomething from a player. 
            /// </summary>
            public static ManualResetEvent listen_1 = new ManualResetEvent(false);
            public static ManualResetEvent listen_2 = new ManualResetEvent(false);

            /// <summary>
            /// Variable used to keep track of the common words played by both opponenets.
            /// </summary>
            private HashSet<String> common_words;

            /// <summary>
            /// Checks the sockets of both players to tell whether or not they are connected. 
            /// </summary>
            public bool players_connected{get;set;}

            /// <summary>
            /// Sets for the illegal words
            /// </summary>
            private HashSet<String> player_1_illegal;
            private HashSet<String> player_2_illegal;

            /// <summary>
            /// Constuctor used to make a new boggle game
            /// </summary>
            /// <param name="player_1">Player 1 that is to be passed with its name and socket. </param>
            /// <param name="player_2">Player 2 that is to be passed with its name and socket.</param>
            /// <param name="game_timer">How long the game should last</param>
            /// <param name="default_board">User input board should be 16 characters.</param>
            /// <param name="dictionary">UNderlying dictionary used to check and see if the words are legal.</param>
            public BoggleGame(Player player_1, Player player_2, int game_timer, string default_board, HashSet<string> dictionary)
            {
                //initialize the private member variables accordingly. 
                this.player_1 = player_1;
                this.player_2 = player_2;
                this.game_timer = game_timer;
                this.dictionary = dictionary;
                this.player_1_words = new HashSet<string>();
                this.player_2_words = new HashSet<string>();
                this.common_words = new HashSet<string>();
                this.player_1_illegal = new HashSet<string>();
                this.player_2_illegal = new HashSet<string>();
                this.player_1_score = 0;
                this.player_2_score = 0;
                this.players_connected = true;

                //Check to see if there is a valid 16 character user input board to use and if not, use the default. 
                if (default_board.Length == 16)
                    this.game_board = new BoggleBoard(default_board);
                else
                    this.game_board = new BoggleBoard();
                //Start up a new game
                NewGame();

            }

            /// <summary>
            /// Provides a basic summary of the game in a tostring format. 
            /// </summary>
            /// <returns>Basic game summary used on the server to display basic information.</returns>
            public override string ToString()
            {
                return String.Format(player_1.player_name + " " + player_1_score + " vs " + player_2.player_name + " " + player_2_score);
            }

            /// <summary>
            /// Starts a new game using the 2 players
            /// </summary>
            private void NewGame()
            {
                ///Sends each player the start message
                player_1.string_socket.BeginSend("START " + this.game_board.ToString() + " " + this.game_timer + " " + player_2.player_name + "\n", (e, o) => { }, player_1.string_socket);
                player_2.string_socket.BeginSend("START " + this.game_board.ToString() + " " + this.game_timer + " " + player_1.player_name + "\n", (e, o) => { }, player_2.string_socket);

                //Use a different thread to send the time listen, and check the network connections. 
                Thread t0 = new Thread(CheckConnections);
                Thread t1 = new Thread(SendTime);
                Thread t2 = new Thread(Listen1);
                Thread t3 = new Thread(Listen2);
                Thread t4 = new Thread(SendScore);
                Thread[] threads = {t0, t1,t2,t3,t4};

                //Start up each thread. 
                foreach (Thread t in threads)
                {
                    t.Start();
                }         
            }
            
            /// <summary>
            /// Listens to player 1 for incoming messages
            /// </summary>
            private void Listen1()
            {
                if (!players_connected || this.game_timer == 0)
                {
                    return;
                }
                player_1.string_socket.BeginReceive(RCallback, player_1);
                listen_1.WaitOne();
            }
            
            /// <summary>
            /// Listens to player 2 for incoming messages
            /// </summary>
            private void Listen2()
            {
                if (!players_connected || this.game_timer == 0 )
                {
                    
                    return;
                }
                player_2.string_socket.BeginReceive(RCallback, player_2);
                listen_2.WaitOne();
            }
            
            /// <summary>
            /// Looks at the word receved and who it came from and processes it accordingly. Stops calling the 
            /// begin receive method once the game timer reaches 0 as the game is over. 
            /// </summary>
            /// <param name="s">Incoming message</param>
            /// <param name="e">error message resulting from a failed receive attempt</param>
            /// <param name="payload">Who sent it.</param>
            private void RCallback(String s, Exception e, Object payload)
            {
                if(s.Contains("WORD "))
                {
                    s = s.Substring(5);
                    ///Always check to see if the players are still connected before using a socket. 
                    if (players_connected)
                    {
                        //message came from player 1
                        if ((Player)payload == player_1 && game_timer > 0)
                        {
                            player_1_words.Add(s.ToUpper());

                            //increment the commmon words counter if the other players queue contains this word. 
                            if (player_2_words.Contains(s.ToUpper()))
                                common_words.Add(s.ToUpper());
                        
                            //Print that it showed up on the server and continue to listen until the game is over
                            Console.WriteLine("Word received from " + player_1.player_name + " : " + s);
                            Listen1();
                        }

                        //message came from player 2
                        else if ((Player)payload == player_2 && game_timer > 0)
                        {               
                            player_2_words.Add(s.ToUpper());

                            //increment the commmon words counter if the other players queue contains this word. 
                            if (player_1_words.Contains(s.ToUpper()))
                                common_words.Add(s.ToUpper());

                            Console.WriteLine("Word received from " + player_2.player_name + " : " + s);
                            Listen2();
                        }
                    }
                
                }
            }

            /// <summary>
            /// Method that updates each player score. Probably could have a had a more efficient way of doing this but hey
            /// it works.
            /// </summary>
            private void Update_Score()
            {
                //update player 1 score
                player_1_score = 0;
                player_2_score = 0;

                HashSet<String> toDelete = new HashSet<string>();

                foreach(string word in player_1_words)
                {   
                    //word too short
                    if (word.Length < 3)
                    {
                        toDelete.Add(word);
                    }
                    else if (player_2_words.Contains(word))
                    {
                        //word in common so remove points from this player
                        toDelete.Add(word);
                        
                    }
                    else if (!this.dictionary.Contains(word) || !this.game_board.CanBeFormed(word))
                    {
                        //illegal word was entered
                        player_1_score--;
                        player_1_illegal.Add(word);
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
                    //word too short
                    if (word.Length < 3)
                    {
                        toDelete.Add(word);
                    }
                    else if (player_1_words.Contains(word))
                    {
                        toDelete.Add(word);
                    }
                    else if (!this.dictionary.Contains(word) || !this.game_board.CanBeFormed(word))
                    {
                        //player entered an illegal word
                        player_2_score--;
                        player_2_illegal.Add(word);
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
                //Delete any invalid words. 
                foreach(String word in toDelete)
                {
                        player_1_words.Remove(word);
                        player_2_words.Remove(word);
                }
            }

            /// <summary>
            /// Checks to see if the players scores have changed every half second and if so sends the updated score to 
            /// both players. 
            /// </summary>
            private void SendScore()
            {
                while (this.game_timer > 0 && players_connected)
                {          
                        Thread.Sleep(500);
                        int p1 = player_1_score;
                        int p2 = player_2_score;
                        Update_Score();
                        if (p1 != player_1_score || p2 != player_2_score)
                        {
                            player_1.string_socket.BeginSend("SCORE " + this.player_1_score + " " + this.player_2_score + "\n", (e, o) => { }, player_1.string_socket);
                            player_2.string_socket.BeginSend("SCORE " + this.player_2_score + " " + this.player_1_score + "\n", (e, o) => { }, player_2.string_socket);
                        }                   
                }
            }
           
            /// <summary>
            /// Send the time ever second to player 1 and player 2. 
            /// </summary>
            private void SendTime()
            {
                while (this.game_timer > 0 && players_connected)
                {
                    Thread.Sleep(1000);
                    if (player_1.string_socket.SocketConnected() && player_2.string_socket.SocketConnected())
                    {
                        player_1.string_socket.BeginSend("TIME " + game_timer.ToString() + "\n", (e, o) => { }, player_1.string_socket);
                        player_2.string_socket.BeginSend("TIME " + game_timer.ToString() + "\n", (e, o) => { }, player_2.string_socket);
                        this.game_timer--;
                    }
                }
                if (game_timer == 0 && players_connected)
                {
                    EndGame();
                }
            }
    
            /// <summary>
            /// When time has expired, the server ignores any further communication from the clients 
            /// and shuts down the game. First, it transmits the final score to both clients as described
            /// above. Next, it transmits a game summary line to both clients. Suppose that during the
            /// game the client played a legal words that weren't played by the opponent, the opponent 
            /// played b legal words that weren't played by the client, both players played c legal words
            /// in common, the client played d illegal words, and the opponent played e illegal words.
            /// The game summary command should be "STOP a #1 b #2 c #3 d #4 e #5", where a, b, c, d, and 
            /// e are the counts described above and #1, #2, #3, #4, and #5 are the corresponding 
            /// space-separated lists of words.
            /// </summary>
            private void EndGame()
            {
                //format a string to be sent to the client with a summary
                string player_1_summary = String.Format("STOP {0} {1} {2} {3} {4} {5} {6} {7} {8} {9}\n", 
                    player_1_words.Count(),   
                    String.Join(" ", player_1_words),
                    player_2_words.Count(),
                    String.Join(" ", player_2_words),
                    common_words.Count(),
                    String.Join(" ", common_words),
                    player_1_illegal.Count(),
                    String.Join(" ", player_1_illegal),
                    player_2_illegal.Count(),
                    String.Join(" ", player_2_illegal)                    
                    );
                
                string player_2_summary = String.Format("STOP {0} {1} {2} {3} {4} {5} {6} {7} {8} {9}\n",
                    player_2_words.Count(),
                    String.Join(" ", player_2_words),
                    player_1_words.Count(),
                    String.Join(" ", player_1_words),
                    common_words.Count(),
                    String.Join(" ", common_words),
                    player_2_illegal.Count(),
                    String.Join(" ", player_2_illegal),
                    player_1_illegal.Count(),
                    String.Join(" ", player_1_illegal)
                    );

                //Send out the summary to both players. 
                player_1.string_socket.BeginSend(player_1_summary, (e, o) => { }, player_1.string_socket);
                player_2.string_socket.BeginSend(player_2_summary, (e, o) => { }, player_2.string_socket);
                
                //Close the underlying sockets
                player_1.string_socket.Close();
                player_2.string_socket.Close();
                
            }

            /// <summary>
            /// Methd that checks that each player is connected evry .5 seconds if for any reason one player disconnects
            /// checks if either player is still connected and sends them a message
            /// </summary>
            /// <param name="mickey"></param>
            private void CheckConnections(Object mickey)
            {
                while (true && game_timer != 0)
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
        
        /// <summary>
        /// Player class that is used within boggle game to keep track of the score. 
        /// </summary>
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
