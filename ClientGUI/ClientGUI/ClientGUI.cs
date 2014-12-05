﻿using BoggleClientModel;
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
        private Boolean serverConnected;

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            gameModel = new Model();
            this.MessageTextBox.Text = "Connecting to server...";
            
            gameModel.ConnectEvent += ConnectionMade;
            gameModel.StartGameEvent += StartGame;
            gameModel.UpdateScoreEvent += UpdateScore;
            gameModel.OpponentDisconnectEvent += OpponentDisconnected;
            gameModel.UpdateTimeEvent += UpdateTime;
            gameModel.EndGameEvent += EndGame;
            
            ThreadPool.QueueUserWorkItem(t => gameModel.Connect(this.PlayerNameTextBox.Text, this.IPAddressTextBox.Text));
            
        }

        private void EndGame(string GameSummary)
        {
            ThreadSafeCall(() => {
                MessageTextBox.Text = GameSummary;
                ConnectButton.Enabled = true;
            }
            );
        }



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

        private void ResetBord()
        {

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
                    this.serverConnected = true;
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
            DisconnectButton.Enabled = false;
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



        private void DisconnectButton_Click(object sender, EventArgs e)
        {
            
        }

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

    }


}
