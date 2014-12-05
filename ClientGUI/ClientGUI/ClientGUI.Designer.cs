﻿namespace ClientGUI
 {
     partial class ClientGUI
     {

         private System.ComponentModel.IContainer components = null;


         protected override void Dispose(bool disposing)
         {
             if (disposing && (components != null))
             {
                 components.Dispose();
             }
             base.Dispose(disposing);
         }

         #region Windows Form Designer generated code

         private void InitializeComponent()
         {
             this.label1 = new System.Windows.Forms.Label();
             this.IPAddressTextBox = new System.Windows.Forms.TextBox();
             this.label2 = new System.Windows.Forms.Label();
             this.PlayerNameTextBox = new System.Windows.Forms.TextBox();
             this.ConnectButton = new System.Windows.Forms.Button();
             this.DisconnectButton = new System.Windows.Forms.Button();
             this.label3 = new System.Windows.Forms.Label();
             this.label4 = new System.Windows.Forms.Label();
             this.label5 = new System.Windows.Forms.Label();
             this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
             this.B1 = new System.Windows.Forms.Label();
             this.B2 = new System.Windows.Forms.Label();
             this.B3 = new System.Windows.Forms.Label();
             this.B4 = new System.Windows.Forms.Label();
             this.B5 = new System.Windows.Forms.Label();
             this.B6 = new System.Windows.Forms.Label();
             this.B7 = new System.Windows.Forms.Label();
             this.B8 = new System.Windows.Forms.Label();
             this.B9 = new System.Windows.Forms.Label();
             this.B10 = new System.Windows.Forms.Label();
             this.B12 = new System.Windows.Forms.Label();
             this.B13 = new System.Windows.Forms.Label();
             this.B14 = new System.Windows.Forms.Label();
             this.B15 = new System.Windows.Forms.Label();
             this.B16 = new System.Windows.Forms.Label();
             this.B17 = new System.Windows.Forms.Label();
             this.MessageTextBox = new System.Windows.Forms.RichTextBox();
             this.PlayerScore = new System.Windows.Forms.TextBox();
             this.Timer = new System.Windows.Forms.TextBox();
             this.label6 = new System.Windows.Forms.Label();
             this.OpponentScore = new System.Windows.Forms.TextBox();
             this.label7 = new System.Windows.Forms.Label();
             this.WordEntry = new System.Windows.Forms.TextBox();
             this.label8 = new System.Windows.Forms.Label();
             this.OpponentName = new System.Windows.Forms.TextBox();
             this.label9 = new System.Windows.Forms.Label();
             this.tableLayoutPanel1.SuspendLayout();
             this.SuspendLayout();
             // 
             // label1
             // 
             this.label1.AutoSize = true;
             this.label1.Location = new System.Drawing.Point(14, 65);
             this.label1.Name = "label1";
             this.label1.Size = new System.Drawing.Size(58, 13);
             this.label1.TabIndex = 0;
             this.label1.Text = "IP Address";
             // 
             // IPAddressTextBox
             // 
             this.IPAddressTextBox.Location = new System.Drawing.Point(79, 62);
             this.IPAddressTextBox.Name = "IPAddressTextBox";
             this.IPAddressTextBox.Size = new System.Drawing.Size(136, 20);
             this.IPAddressTextBox.TabIndex = 1;
             // 
             // label2
             // 
             this.label2.AutoSize = true;
             this.label2.Location = new System.Drawing.Point(5, 96);
             this.label2.Name = "label2";
             this.label2.Size = new System.Drawing.Size(60, 13);
             this.label2.TabIndex = 2;
             this.label2.Text = "Your Name";
             // 
             // PlayerNameTextBox
             // 
             this.PlayerNameTextBox.Location = new System.Drawing.Point(78, 93);
             this.PlayerNameTextBox.Name = "PlayerNameTextBox";
             this.PlayerNameTextBox.Size = new System.Drawing.Size(137, 20);
             this.PlayerNameTextBox.TabIndex = 3;
             // 
             // ConnectButton
             // 
             this.ConnectButton.Location = new System.Drawing.Point(234, 62);
             this.ConnectButton.Name = "ConnectButton";
             this.ConnectButton.Size = new System.Drawing.Size(139, 23);
             this.ConnectButton.TabIndex = 4;
             this.ConnectButton.Text = "Find a Player";
             this.ConnectButton.UseVisualStyleBackColor = true;
             this.ConnectButton.Click += new System.EventHandler(this.ConnectButton_Click);
             // 
             // DisconnectButton
             // 
             this.DisconnectButton.Enabled = false;
             this.DisconnectButton.Location = new System.Drawing.Point(234, 91);
             this.DisconnectButton.Name = "DisconnectButton";
             this.DisconnectButton.Size = new System.Drawing.Size(139, 23);
             this.DisconnectButton.TabIndex = 4;
             this.DisconnectButton.Text = "Disconnect";
             this.DisconnectButton.UseVisualStyleBackColor = true;
             this.DisconnectButton.Click += new System.EventHandler(this.DisconnectButton_Click);
             // 
             // label3
             // 
             this.label3.AutoSize = true;
             this.label3.Location = new System.Drawing.Point(12, 416);
             this.label3.Name = "label3";
             this.label3.Size = new System.Drawing.Size(55, 13);
             this.label3.TabIndex = 5;
             this.label3.Text = "Messages";
             // 
             // label4
             // 
             this.label4.AutoSize = true;
             this.label4.Location = new System.Drawing.Point(24, 234);
             this.label4.Name = "label4";
             this.label4.Size = new System.Drawing.Size(60, 13);
             this.label4.TabIndex = 7;
             this.label4.Text = "Your Score";
             // 
             // label5
             // 
             this.label5.AutoSize = true;
             this.label5.Location = new System.Drawing.Point(68, 137);
             this.label5.Name = "label5";
             this.label5.Size = new System.Drawing.Size(51, 13);
             this.label5.TabIndex = 8;
             this.label5.Text = "Time Left";
             // 
             // tableLayoutPanel1
             // 
             this.tableLayoutPanel1.ColumnCount = 4;
             this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
             this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
             this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
             this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
             this.tableLayoutPanel1.Controls.Add(this.B1, 0, 0);
             this.tableLayoutPanel1.Controls.Add(this.B2, 1, 0);
             this.tableLayoutPanel1.Controls.Add(this.B3, 3, 0);
             this.tableLayoutPanel1.Controls.Add(this.B4, 0, 1);
             this.tableLayoutPanel1.Controls.Add(this.B5, 1, 1);
             this.tableLayoutPanel1.Controls.Add(this.B6, 2, 1);
             this.tableLayoutPanel1.Controls.Add(this.B7, 3, 1);
             this.tableLayoutPanel1.Controls.Add(this.B8, 3, 2);
             this.tableLayoutPanel1.Controls.Add(this.B9, 2, 2);
             this.tableLayoutPanel1.Controls.Add(this.B10, 1, 2);
             this.tableLayoutPanel1.Controls.Add(this.B12, 0, 2);
             this.tableLayoutPanel1.Controls.Add(this.B13, 0, 3);
             this.tableLayoutPanel1.Controls.Add(this.B14, 1, 3);
             this.tableLayoutPanel1.Controls.Add(this.B15, 2, 3);
             this.tableLayoutPanel1.Controls.Add(this.B16, 3, 3);
             this.tableLayoutPanel1.Controls.Add(this.B17, 2, 0);
             this.tableLayoutPanel1.Location = new System.Drawing.Point(195, 161);
             this.tableLayoutPanel1.Name = "tableLayoutPanel1";
             this.tableLayoutPanel1.RowCount = 4;
             this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
             this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
             this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
             this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
             this.tableLayoutPanel1.Size = new System.Drawing.Size(209, 227);
             this.tableLayoutPanel1.TabIndex = 9;
             // 
             // BB11
             // 
             this.B1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
             | System.Windows.Forms.AnchorStyles.Left)
             | System.Windows.Forms.AnchorStyles.Right)));
             this.B1.AutoSize = true;
             this.B1.BackColor = System.Drawing.SystemColors.ControlLightLight;
             this.B1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
             this.B1.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
             this.B1.Location = new System.Drawing.Point(0, 0);
             this.B1.Margin = new System.Windows.Forms.Padding(0);
             this.B1.Name = "BB11";
             this.B1.Size = new System.Drawing.Size(52, 56);
             this.B1.TabIndex = 0;
             this.B1.Text = "";
             this.B1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
             // 
             // BB12
             // 
             this.B2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
             | System.Windows.Forms.AnchorStyles.Left)
             | System.Windows.Forms.AnchorStyles.Right)));
             this.B2.AutoSize = true;
             this.B2.BackColor = System.Drawing.SystemColors.ControlLightLight;
             this.B2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
             this.B2.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
             this.B2.Location = new System.Drawing.Point(52, 0);
             this.B2.Margin = new System.Windows.Forms.Padding(0);
             this.B2.Name = "BB12";
             this.B2.Size = new System.Drawing.Size(52, 56);
             this.B2.TabIndex = 0;
             this.B2.Text = "";
             this.B2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
             // 
             // BB14
             // 
             this.B3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
             | System.Windows.Forms.AnchorStyles.Left)
             | System.Windows.Forms.AnchorStyles.Right)));
             this.B3.AutoSize = true;
             this.B3.BackColor = System.Drawing.SystemColors.ControlLightLight;
             this.B3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
             this.B3.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
             this.B3.Location = new System.Drawing.Point(156, 0);
             this.B3.Margin = new System.Windows.Forms.Padding(0);
             this.B3.Name = "BB14";
             this.B3.Size = new System.Drawing.Size(53, 56);
             this.B3.TabIndex = 0;
             this.B3.Text = "";
             this.B3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
             // 
             // BB21
             // 
             this.B4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
             | System.Windows.Forms.AnchorStyles.Left)
             | System.Windows.Forms.AnchorStyles.Right)));
             this.B4.AutoSize = true;
             this.B4.BackColor = System.Drawing.SystemColors.ControlLightLight;
             this.B4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
             this.B4.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
             this.B4.Location = new System.Drawing.Point(0, 56);
             this.B4.Margin = new System.Windows.Forms.Padding(0);
             this.B4.Name = "BB21";
             this.B4.Size = new System.Drawing.Size(52, 56);
             this.B4.TabIndex = 0;
             this.B4.Text = "";
             this.B4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
             // 
             // BB22
             // 
             this.B5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
             | System.Windows.Forms.AnchorStyles.Left)
             | System.Windows.Forms.AnchorStyles.Right)));
             this.B5.AutoSize = true;
             this.B5.BackColor = System.Drawing.SystemColors.ControlLightLight;
             this.B5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
             this.B5.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
             this.B5.Location = new System.Drawing.Point(52, 56);
             this.B5.Margin = new System.Windows.Forms.Padding(0);
             this.B5.Name = "BB22";
             this.B5.Size = new System.Drawing.Size(52, 56);
             this.B5.TabIndex = 0;
             this.B5.Text = "";
             this.B5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
             // 
             // BB23
             // 
             this.B6.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
             | System.Windows.Forms.AnchorStyles.Left)
             | System.Windows.Forms.AnchorStyles.Right)));
             this.B6.AutoSize = true;
             this.B6.BackColor = System.Drawing.SystemColors.ControlLightLight;
             this.B6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
             this.B6.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
             this.B6.Location = new System.Drawing.Point(104, 56);
             this.B6.Margin = new System.Windows.Forms.Padding(0);
             this.B6.Name = "BB23";
             this.B6.Size = new System.Drawing.Size(52, 56);
             this.B6.TabIndex = 0;
             this.B6.Text = "";
             this.B6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
             // 
             // BB24
             // 
             this.B7.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
             | System.Windows.Forms.AnchorStyles.Left)
             | System.Windows.Forms.AnchorStyles.Right)));
             this.B7.AutoSize = true;
             this.B7.BackColor = System.Drawing.SystemColors.ControlLightLight;
             this.B7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
             this.B7.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
             this.B7.Location = new System.Drawing.Point(156, 56);
             this.B7.Margin = new System.Windows.Forms.Padding(0);
             this.B7.Name = "BB24";
             this.B7.Size = new System.Drawing.Size(53, 56);
             this.B7.TabIndex = 0;
             this.B7.Text = "";
             this.B7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
             // 
             // BB34
             // 
             this.B8.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
             | System.Windows.Forms.AnchorStyles.Left)
             | System.Windows.Forms.AnchorStyles.Right)));
             this.B8.AutoSize = true;
             this.B8.BackColor = System.Drawing.SystemColors.ControlLightLight;
             this.B8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
             this.B8.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
             this.B8.Location = new System.Drawing.Point(156, 112);
             this.B8.Margin = new System.Windows.Forms.Padding(0);
             this.B8.Name = "BB34";
             this.B8.Size = new System.Drawing.Size(53, 56);
             this.B8.TabIndex = 0;
             this.B8.Text = "";
             this.B8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
             // 
             // BB33
             // 
             this.B9.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
             | System.Windows.Forms.AnchorStyles.Left)
             | System.Windows.Forms.AnchorStyles.Right)));
             this.B9.AutoSize = true;
             this.B9.BackColor = System.Drawing.SystemColors.ControlLightLight;
             this.B9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
             this.B9.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
             this.B9.Location = new System.Drawing.Point(104, 112);
             this.B9.Margin = new System.Windows.Forms.Padding(0);
             this.B9.Name = "BB33";
             this.B9.Size = new System.Drawing.Size(52, 56);
             this.B9.TabIndex = 0;
             this.B9.Text = "";
             this.B9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
             // 
             // BB32
             // 
             this.B10.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
             | System.Windows.Forms.AnchorStyles.Left)
             | System.Windows.Forms.AnchorStyles.Right)));
             this.B10.AutoSize = true;
             this.B10.BackColor = System.Drawing.SystemColors.ControlLightLight;
             this.B10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
             this.B10.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
             this.B10.Location = new System.Drawing.Point(52, 112);
             this.B10.Margin = new System.Windows.Forms.Padding(0);
             this.B10.Name = "BB32";
             this.B10.Size = new System.Drawing.Size(52, 56);
             this.B10.TabIndex = 0;
             this.B10.Text = "";
             this.B10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
             // 
             // BB31
             // 
             this.B12.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
             | System.Windows.Forms.AnchorStyles.Left)
             | System.Windows.Forms.AnchorStyles.Right)));
             this.B12.AutoSize = true;
             this.B12.BackColor = System.Drawing.SystemColors.ControlLightLight;
             this.B12.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
             this.B12.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
             this.B12.Location = new System.Drawing.Point(0, 112);
             this.B12.Margin = new System.Windows.Forms.Padding(0);
             this.B12.Name = "BB31";
             this.B12.Size = new System.Drawing.Size(52, 56);
             this.B12.TabIndex = 0;
             this.B12.Text = "";
             this.B12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
             // 
             // BB41
             // 
             this.B13.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
             | System.Windows.Forms.AnchorStyles.Left)
             | System.Windows.Forms.AnchorStyles.Right)));
             this.B13.AutoSize = true;
             this.B13.BackColor = System.Drawing.SystemColors.ControlLightLight;
             this.B13.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
             this.B13.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
             this.B13.Location = new System.Drawing.Point(0, 168);
             this.B13.Margin = new System.Windows.Forms.Padding(0);
             this.B13.Name = "BB41";
             this.B13.Size = new System.Drawing.Size(52, 59);
             this.B13.TabIndex = 0;
             this.B13.Text = "";
             this.B13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
             // 
             // BB42
             // 
             this.B14.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
             | System.Windows.Forms.AnchorStyles.Left)
             | System.Windows.Forms.AnchorStyles.Right)));
             this.B14.AutoSize = true;
             this.B14.BackColor = System.Drawing.SystemColors.ControlLightLight;
             this.B14.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
             this.B14.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
             this.B14.Location = new System.Drawing.Point(52, 168);
             this.B14.Margin = new System.Windows.Forms.Padding(0);
             this.B14.Name = "BB42";
             this.B14.Size = new System.Drawing.Size(52, 59);
             this.B14.TabIndex = 0;
             this.B14.Text = "";
             this.B14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
             // 
             // BB43
             // 
             this.B15.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
             | System.Windows.Forms.AnchorStyles.Left)
             | System.Windows.Forms.AnchorStyles.Right)));
             this.B15.AutoSize = true;
             this.B15.BackColor = System.Drawing.SystemColors.ControlLightLight;
             this.B15.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
             this.B15.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
             this.B15.Location = new System.Drawing.Point(104, 168);
             this.B15.Margin = new System.Windows.Forms.Padding(0);
             this.B15.Name = "BB43";
             this.B15.Size = new System.Drawing.Size(52, 59);
             this.B15.TabIndex = 0;
             this.B15.Text = "";
             this.B15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
             // 
             // BB44
             // 
             this.B16.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
             | System.Windows.Forms.AnchorStyles.Left)
             | System.Windows.Forms.AnchorStyles.Right)));
             this.B16.AutoSize = true;
             this.B16.BackColor = System.Drawing.SystemColors.ControlLightLight;
             this.B16.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
             this.B16.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
             this.B16.Location = new System.Drawing.Point(156, 168);
             this.B16.Margin = new System.Windows.Forms.Padding(0);
             this.B16.Name = "BB44";
             this.B16.Size = new System.Drawing.Size(53, 59);
             this.B16.TabIndex = 0;
             this.B16.Text = "";
             this.B16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
             // 
             // BB13
             // 
             this.B17.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
             | System.Windows.Forms.AnchorStyles.Left)
             | System.Windows.Forms.AnchorStyles.Right)));
             this.B17.AutoSize = true;
             this.B17.BackColor = System.Drawing.SystemColors.ControlLightLight;
             this.B17.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
             this.B17.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
             this.B17.Location = new System.Drawing.Point(104, 0);
             this.B17.Margin = new System.Windows.Forms.Padding(0);
             this.B17.Name = "BB13";
             this.B17.Size = new System.Drawing.Size(52, 56);
             this.B17.TabIndex = 0;
             this.B17.Text = "";
             this.B17.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
             // 
             // MessageTextBox
             // 
             this.MessageTextBox.Location = new System.Drawing.Point(79, 394);
             this.MessageTextBox.Name = "MessageTextBox";
             this.MessageTextBox.ReadOnly = true;
             this.MessageTextBox.Size = new System.Drawing.Size(282, 53);
             this.MessageTextBox.TabIndex = 10;
             this.MessageTextBox.Text = "";
             // 
             // PlayerScore
             // 
             this.PlayerScore.Location = new System.Drawing.Point(28, 250);
             this.PlayerScore.Name = "PlayerScore";
             this.PlayerScore.ReadOnly = true;
             this.PlayerScore.Size = new System.Drawing.Size(56, 20);
             this.PlayerScore.TabIndex = 11;
             // 
             // Timer
             // 
             this.Timer.Location = new System.Drawing.Point(61, 153);
             this.Timer.Name = "Timer";
             this.Timer.ReadOnly = true;
             this.Timer.Size = new System.Drawing.Size(58, 20);
             this.Timer.TabIndex = 12;
             // 
             // label6
             // 
             this.label6.AutoSize = true;
             this.label6.Location = new System.Drawing.Point(99, 234);
             this.label6.Name = "label6";
             this.label6.Size = new System.Drawing.Size(85, 13);
             this.label6.TabIndex = 13;
             this.label6.Text = "Opponent Score";
             // 
             // OpponentScore
             // 
             this.OpponentScore.Location = new System.Drawing.Point(102, 250);
             this.OpponentScore.Name = "OpponentScore";
             this.OpponentScore.ReadOnly = true;
             this.OpponentScore.Size = new System.Drawing.Size(56, 20);
             this.OpponentScore.TabIndex = 14;
             // 
             // label7
             // 
             this.label7.AutoSize = true;
             this.label7.Location = new System.Drawing.Point(192, 133);
             this.label7.Name = "label7";
             this.label7.Size = new System.Drawing.Size(64, 13);
             this.label7.TabIndex = 15;
             this.label7.Text = "Enter Word:";
             // 
             // WordEntry
             // 
             this.WordEntry.Enabled = false;
             this.WordEntry.Location = new System.Drawing.Point(262, 130);
             this.WordEntry.Name = "WordEntry";
             this.WordEntry.Size = new System.Drawing.Size(142, 20);
             this.WordEntry.TabIndex = 16;
             this.WordEntry.KeyDown += new System.Windows.Forms.KeyEventHandler(this.EnterPressed);
             // 
             // label8
             // 
             this.label8.AutoSize = true;
             this.label8.Location = new System.Drawing.Point(36, 184);
             this.label8.Name = "label8";
             this.label8.Size = new System.Drawing.Size(113, 13);
             this.label8.TabIndex = 17;
             this.label8.Text = " You\'re playing against";
             // 
             // OpponentName
             // 
             this.OpponentName.Location = new System.Drawing.Point(63, 200);
             this.OpponentName.Name = "OpponentName";
             this.OpponentName.ReadOnly = true;
             this.OpponentName.Size = new System.Drawing.Size(56, 20);
             this.OpponentName.TabIndex = 18;
             // 
             // label9
             // 
             this.label9.AutoSize = true;
             this.label9.Location = new System.Drawing.Point(172, 22);
             this.label9.Name = "label9";
             this.label9.Size = new System.Drawing.Size(54, 13);
             this.label9.TabIndex = 19;
             this.label9.Text = "BOGGLE!";
             // 
             // GameClient
             // 
             this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
             this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
             this.BackColor = System.Drawing.SystemColors.ControlLightLight;
             this.ClientSize = new System.Drawing.Size(429, 457);
             this.Controls.Add(this.label9);
             this.Controls.Add(this.OpponentName);
             this.Controls.Add(this.label8);
             this.Controls.Add(this.WordEntry);
             this.Controls.Add(this.label7);
             this.Controls.Add(this.OpponentScore);
             this.Controls.Add(this.label6);
             this.Controls.Add(this.Timer);
             this.Controls.Add(this.PlayerScore);
             this.Controls.Add(this.MessageTextBox);
             this.Controls.Add(this.tableLayoutPanel1);
             this.Controls.Add(this.label5);
             this.Controls.Add(this.label4);
             this.Controls.Add(this.label3);
             this.Controls.Add(this.DisconnectButton);
             this.Controls.Add(this.ConnectButton);
             this.Controls.Add(this.PlayerNameTextBox);
             this.Controls.Add(this.label2);
             this.Controls.Add(this.IPAddressTextBox);
             this.Controls.Add(this.label1);
             this.ForeColor = System.Drawing.SystemColors.Desktop;
             this.Name = "GameClient";
             this.Text = "Boggle";
             this.tableLayoutPanel1.ResumeLayout(false);
             this.tableLayoutPanel1.PerformLayout();
             this.ResumeLayout(false);
             this.PerformLayout();

         }

         #endregion

         private System.Windows.Forms.Label label1;
         private System.Windows.Forms.TextBox IPAddressTextBox;
         private System.Windows.Forms.Label label2;
         private System.Windows.Forms.TextBox PlayerNameTextBox;
         private System.Windows.Forms.Button ConnectButton;
         private System.Windows.Forms.Button DisconnectButton;
         private System.Windows.Forms.Label label3;
         private System.Windows.Forms.Label label4;
         private System.Windows.Forms.Label label5;
         private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
         private System.Windows.Forms.Label B1;
         private System.Windows.Forms.Label B2;
         private System.Windows.Forms.Label B17;
         private System.Windows.Forms.Label B3;
         private System.Windows.Forms.Label B4;
         private System.Windows.Forms.Label B5;
         private System.Windows.Forms.Label B6;
         private System.Windows.Forms.Label B7;
         private System.Windows.Forms.Label B8;
         private System.Windows.Forms.Label B9;
         private System.Windows.Forms.Label B10;
         private System.Windows.Forms.Label B12;
         private System.Windows.Forms.Label B13;
         private System.Windows.Forms.Label B14;
         private System.Windows.Forms.Label B15;
         private System.Windows.Forms.Label B16;
         private System.Windows.Forms.RichTextBox MessageTextBox;
         private System.Windows.Forms.TextBox PlayerScore;
         private System.Windows.Forms.TextBox Timer;
         private System.Windows.Forms.Label label6;
         private System.Windows.Forms.TextBox OpponentScore;
         private System.Windows.Forms.Label label7;
         private System.Windows.Forms.TextBox WordEntry;
         private System.Windows.Forms.Label label8;
         private System.Windows.Forms.TextBox OpponentName;
         private System.Windows.Forms.Label label9;
     }

 }