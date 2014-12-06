using BoggleClientModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientGUI
{
    public partial class ClientGUI : Form
    {

        public ClientGUI() { InitializeComponent(); }
        private Model gameModel;
        
        /// <summary>
        /// Connect to a game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConnectButton_Click(object sender, EventArgs e)
        {
            gameModel = new Model();
            this.MessageTextBox.Text = "Connecting to server...";
            

            //Handle the events using the correct methods
            gameModel.ConnectEvent += ConnectionMade;
            gameModel.StartGameEvent += StartGame;
            gameModel.UpdateScoreEvent += UpdateScore;
            gameModel.OpponentDisconnectEvent += OpponentDisconnected;
            gameModel.UpdateTimeEvent += UpdateTime;
            gameModel.EndGameEvent += EndGame;
            gameModel.ServerDisconnectEvent += ServerDisconnect;
            
            
            ThreadPool.QueueUserWorkItem(t => gameModel.Connect(this.PlayerNameTextBox.Text, this.IPAddressTextBox.Text));
            
        }


        /// <summary>
        /// What to do when server disconnects
        /// </summary>
        private void ServerDisconnect()
        {
            ThreadSafeCall(() =>
            {
                MessageTextBox.Text = "Connection to server lost click find a player to rejoin";
                DisconnectButton.Enabled = false;
                ConnectButton.Enabled = true;
            }
                );
        }

        /// <summary>
        /// End the game and corresponding gui buttons
        /// </summary>
        /// <param name="GameSummary"></param>
        private void EndGame(string GameSummary)
        {
            ThreadSafeCall(() => {
                MessageTextBox.Text = GameSummary;
                ConnectButton.Enabled = true;
                DisconnectButton.Enabled = false;
            }
            );
        }


        /// <summary>
        /// Update the time
        /// </summary>
        /// <param name="time"></param>
        private void UpdateTime(string time)
        {
            ThreadSafeCall(() =>
            {
                Timer.Text = time;            
            }
            ); 
        }

        private void OpponentDisconnected()
        {
            ThreadSafeCall(() =>
            {
                MessageTextBox.Text = "Your opponent gave up! You Win!";
                ConnectButton.Enabled = true;
            });
        }


        private void UpdateScore(string p1_score, string p2_score)
        {
            ThreadSafeCall(() =>
            {
                PlayerScore.Text = p1_score;
                OpponentScore.Text = p2_score;
            }
            );   
        }
        private void ConnectionMade(bool connection_made)
        {
            ThreadSafeCall(() =>{
    
                if(connection_made == true){
                    this.ConnectButton.Enabled = false;
                    this.DisconnectButton.Enabled = true;                  
                    this.MessageTextBox.Text = "Connected to server waiting for opponent...";
                    
                }
                else
                {
                    this.MessageTextBox.Text = "Connection to server failed try again!";
                }
            }
            );
            
        }

        private void ThreadSafeCall(Action method)
        {
            this.Invoke(method); 

        }

        /// <summary>
        /// Set up the board and initalize all the values of the gui fields. 
        /// </summary>
        /// <param name="board"></param>
        /// <param name="time"></param>
        /// <param name="opponent"></param>
        private void StartGame(string board, int time, string opponent)
        {
            ThreadSafeCall(() => {

            ConnectButton.Enabled = false;
            Board1.Text = board[0].ToString();         
            Board2.Text = board[1].ToString();
            Board3.Text = board[2].ToString();
            Board4.Text = board[3].ToString();
            Board5.Text = board[4].ToString();
            Board6.Text = board[5].ToString();
            Board7.Text = board[6].ToString();
            Board8.Text = board[7].ToString();
            Board9.Text = board[8].ToString();
            Board10.Text = board[9].ToString();
            Board11.Text = board[10].ToString();
            Board12.Text = board[11].ToString();         
            Board13.Text = board[12].ToString();
            Board14.Text = board[13].ToString();
            Board15.Text = board[14].ToString();
            Board16.Text = board[15].ToString();
            Timer.Text = time.ToString();
            OpponentName.Text = opponent.ToString();
            PlayerScore.Text = "0";
            OpponentScore.Text = "0";
            WordEntry.Enabled = true;
            DisconnectButton.Enabled = true;
            MessageTextBox.Text = "Game Started! Good luck!";
        }
        );
        }

        private void ConnectInfoEntered()
        {
            Boolean validIP = Regex.IsMatch(IPAddressTextBox.Text, @"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}|localhost");
            if(validIP && PlayerNameTextBox.Text.Length > 0 ){
                ConnectButton.Enabled = true;
            }
        }

        /// <summary>
        /// Disconnect from the server.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DisconnectButton_Click(object sender, EventArgs e)
        {
            gameModel.Disconnect();
            this.DisconnectButton.Enabled = false;
            this.ConnectButton.Enabled = true;
            this.MessageTextBox.Text = "You have disconnected.. Click find a player to join a game.";
        }

        /// <summary>
        /// play a word
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EnterPressed(object sender, KeyEventArgs e)
        {
            ThreadSafeCall(() =>
            {
                if (WordEntry.Text != "" && e.KeyCode == Keys.Enter)
                {
                    gameModel.PlayWord(WordEntry.Text);
                    WordEntry.Text = "";
                }
            }
            );
        }

        private void PlayerNameTextBox_TextChanged(object sender, EventArgs e)
        {
            ConnectInfoEntered();
        }

        private void IPAddressTextBox_TextChanged(object sender, EventArgs e)
        {
            ConnectInfoEntered();
        }

        /// <summary>
        /// Just a hint on entering an ip. Note the game will not allow you to connect unless you enter in a valid one. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IPAddressTextBox_MouseHover(object sender, EventArgs e)
        {
            ToolTip toolTip1 = new ToolTip();
            // Set up the delays for the ToolTip.
            toolTip1.AutoPopDelay = 5000;
            toolTip1.InitialDelay = 1000;
            toolTip1.ShowAlways = true;
            

            // Set up the ToolTip text for the Button and Checkbox.
            toolTip1.SetToolTip(this.IPAddressTextBox, "Enter a valid IP such as : \"localhost\" or \"54.148.22.148\" " ) ;
            toolTip1.Active = true;
        }

    }


}
