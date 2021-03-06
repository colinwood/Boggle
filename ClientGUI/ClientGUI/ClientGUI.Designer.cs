﻿﻿namespace ClientGUI
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
            this.Board1 = new System.Windows.Forms.Label();
            this.Board2 = new System.Windows.Forms.Label();
            this.Board4 = new System.Windows.Forms.Label();
            this.Board5 = new System.Windows.Forms.Label();
            this.Board6 = new System.Windows.Forms.Label();
            this.Board7 = new System.Windows.Forms.Label();
            this.Board8 = new System.Windows.Forms.Label();
            this.Board12 = new System.Windows.Forms.Label();
            this.Board11 = new System.Windows.Forms.Label();
            this.Board10 = new System.Windows.Forms.Label();
            this.Board9 = new System.Windows.Forms.Label();
            this.Board13 = new System.Windows.Forms.Label();
            this.Board14 = new System.Windows.Forms.Label();
            this.Board15 = new System.Windows.Forms.Label();
            this.Board16 = new System.Windows.Forms.Label();
            this.Board3 = new System.Windows.Forms.Label();
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
            this.label1.Font = new System.Drawing.Font("Segoe Marker", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(11, 63);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "IP Address";
            // 
            // IPAddressTextBox
            // 
            this.IPAddressTextBox.Location = new System.Drawing.Point(87, 62);
            this.IPAddressTextBox.Name = "IPAddressTextBox";
            this.IPAddressTextBox.Size = new System.Drawing.Size(136, 20);
            this.IPAddressTextBox.TabIndex = 1;
            this.IPAddressTextBox.TextChanged += new System.EventHandler(this.IPAddressTextBox_TextChanged);
            this.IPAddressTextBox.MouseHover += new System.EventHandler(this.IPAddressTextBox_MouseHover);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe Marker", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(2, 94);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 19);
            this.label2.TabIndex = 2;
            this.label2.Text = "Your Name";
            // 
            // PlayerNameTextBox
            // 
            this.PlayerNameTextBox.Location = new System.Drawing.Point(86, 93);
            this.PlayerNameTextBox.Name = "PlayerNameTextBox";
            this.PlayerNameTextBox.Size = new System.Drawing.Size(137, 20);
            this.PlayerNameTextBox.TabIndex = 3;
            this.PlayerNameTextBox.TextChanged += new System.EventHandler(this.PlayerNameTextBox_TextChanged);
            // 
            // ConnectButton
            // 
            this.ConnectButton.Enabled = false;
            this.ConnectButton.Location = new System.Drawing.Point(235, 62);
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
            this.DisconnectButton.Location = new System.Drawing.Point(235, 91);
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
            this.label3.Font = new System.Drawing.Font("Segoe Marker", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(11, 478);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 19);
            this.label3.TabIndex = 5;
            this.label3.Text = "Messages";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe Marker", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(8, 288);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 19);
            this.label4.TabIndex = 7;
            this.label4.Text = "Your Score";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe Marker", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(58, 181);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 19);
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
            this.tableLayoutPanel1.Controls.Add(this.Board1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.Board2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.Board4, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.Board5, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.Board6, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.Board7, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.Board8, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.Board12, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.Board11, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.Board10, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.Board9, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.Board13, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.Board14, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.Board15, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.Board16, 3, 3);
            this.tableLayoutPanel1.Controls.Add(this.Board3, 2, 0);
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
            // Board1
            // 
            this.Board1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Board1.AutoSize = true;
            this.Board1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Board1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Board1.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Board1.Location = new System.Drawing.Point(0, 0);
            this.Board1.Margin = new System.Windows.Forms.Padding(0);
            this.Board1.Name = "Board1";
            this.Board1.Size = new System.Drawing.Size(52, 56);
            this.Board1.TabIndex = 0;
            this.Board1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Board2
            // 
            this.Board2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Board2.AutoSize = true;
            this.Board2.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Board2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Board2.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Board2.Location = new System.Drawing.Point(52, 0);
            this.Board2.Margin = new System.Windows.Forms.Padding(0);
            this.Board2.Name = "Board2";
            this.Board2.Size = new System.Drawing.Size(52, 56);
            this.Board2.TabIndex = 0;
            this.Board2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Board4
            // 
            this.Board4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Board4.AutoSize = true;
            this.Board4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Board4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Board4.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Board4.Location = new System.Drawing.Point(156, 0);
            this.Board4.Margin = new System.Windows.Forms.Padding(0);
            this.Board4.Name = "Board4";
            this.Board4.Size = new System.Drawing.Size(53, 56);
            this.Board4.TabIndex = 0;
            this.Board4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Board5
            // 
            this.Board5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Board5.AutoSize = true;
            this.Board5.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Board5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Board5.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Board5.Location = new System.Drawing.Point(0, 56);
            this.Board5.Margin = new System.Windows.Forms.Padding(0);
            this.Board5.Name = "Board5";
            this.Board5.Size = new System.Drawing.Size(52, 56);
            this.Board5.TabIndex = 0;
            this.Board5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Board6
            // 
            this.Board6.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Board6.AutoSize = true;
            this.Board6.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Board6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Board6.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Board6.Location = new System.Drawing.Point(52, 56);
            this.Board6.Margin = new System.Windows.Forms.Padding(0);
            this.Board6.Name = "Board6";
            this.Board6.Size = new System.Drawing.Size(52, 56);
            this.Board6.TabIndex = 0;
            this.Board6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Board7
            // 
            this.Board7.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Board7.AutoSize = true;
            this.Board7.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Board7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Board7.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Board7.Location = new System.Drawing.Point(104, 56);
            this.Board7.Margin = new System.Windows.Forms.Padding(0);
            this.Board7.Name = "Board7";
            this.Board7.Size = new System.Drawing.Size(52, 56);
            this.Board7.TabIndex = 0;
            this.Board7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Board8
            // 
            this.Board8.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Board8.AutoSize = true;
            this.Board8.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Board8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Board8.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Board8.Location = new System.Drawing.Point(156, 56);
            this.Board8.Margin = new System.Windows.Forms.Padding(0);
            this.Board8.Name = "Board8";
            this.Board8.Size = new System.Drawing.Size(53, 56);
            this.Board8.TabIndex = 0;
            this.Board8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Board12
            // 
            this.Board12.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Board12.AutoSize = true;
            this.Board12.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Board12.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Board12.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Board12.Location = new System.Drawing.Point(156, 112);
            this.Board12.Margin = new System.Windows.Forms.Padding(0);
            this.Board12.Name = "Board12";
            this.Board12.Size = new System.Drawing.Size(53, 56);
            this.Board12.TabIndex = 0;
            this.Board12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Board11
            // 
            this.Board11.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Board11.AutoSize = true;
            this.Board11.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Board11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Board11.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Board11.Location = new System.Drawing.Point(104, 112);
            this.Board11.Margin = new System.Windows.Forms.Padding(0);
            this.Board11.Name = "Board11";
            this.Board11.Size = new System.Drawing.Size(52, 56);
            this.Board11.TabIndex = 0;
            this.Board11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Board10
            // 
            this.Board10.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Board10.AutoSize = true;
            this.Board10.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Board10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Board10.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Board10.Location = new System.Drawing.Point(52, 112);
            this.Board10.Margin = new System.Windows.Forms.Padding(0);
            this.Board10.Name = "Board10";
            this.Board10.Size = new System.Drawing.Size(52, 56);
            this.Board10.TabIndex = 0;
            this.Board10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Board9
            // 
            this.Board9.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Board9.AutoSize = true;
            this.Board9.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Board9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Board9.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Board9.Location = new System.Drawing.Point(0, 112);
            this.Board9.Margin = new System.Windows.Forms.Padding(0);
            this.Board9.Name = "Board9";
            this.Board9.Size = new System.Drawing.Size(52, 56);
            this.Board9.TabIndex = 0;
            this.Board9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Board13
            // 
            this.Board13.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Board13.AutoSize = true;
            this.Board13.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Board13.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Board13.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Board13.Location = new System.Drawing.Point(0, 168);
            this.Board13.Margin = new System.Windows.Forms.Padding(0);
            this.Board13.Name = "Board13";
            this.Board13.Size = new System.Drawing.Size(52, 59);
            this.Board13.TabIndex = 0;
            this.Board13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Board14
            // 
            this.Board14.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Board14.AutoSize = true;
            this.Board14.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Board14.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Board14.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Board14.Location = new System.Drawing.Point(52, 168);
            this.Board14.Margin = new System.Windows.Forms.Padding(0);
            this.Board14.Name = "Board14";
            this.Board14.Size = new System.Drawing.Size(52, 59);
            this.Board14.TabIndex = 0;
            this.Board14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Board15
            // 
            this.Board15.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Board15.AutoSize = true;
            this.Board15.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Board15.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Board15.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Board15.Location = new System.Drawing.Point(104, 168);
            this.Board15.Margin = new System.Windows.Forms.Padding(0);
            this.Board15.Name = "Board15";
            this.Board15.Size = new System.Drawing.Size(52, 59);
            this.Board15.TabIndex = 0;
            this.Board15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Board16
            // 
            this.Board16.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Board16.AutoSize = true;
            this.Board16.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Board16.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Board16.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Board16.Location = new System.Drawing.Point(156, 168);
            this.Board16.Margin = new System.Windows.Forms.Padding(0);
            this.Board16.Name = "Board16";
            this.Board16.Size = new System.Drawing.Size(53, 59);
            this.Board16.TabIndex = 0;
            this.Board16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Board3
            // 
            this.Board3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Board3.AutoSize = true;
            this.Board3.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Board3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Board3.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Board3.Location = new System.Drawing.Point(104, 0);
            this.Board3.Margin = new System.Windows.Forms.Padding(0);
            this.Board3.Name = "Board3";
            this.Board3.Size = new System.Drawing.Size(52, 56);
            this.Board3.TabIndex = 0;
            this.Board3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MessageTextBox
            // 
            this.MessageTextBox.Font = new System.Drawing.Font("Segoe Marker", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MessageTextBox.Location = new System.Drawing.Point(84, 416);
            this.MessageTextBox.Name = "MessageTextBox";
            this.MessageTextBox.ReadOnly = true;
            this.MessageTextBox.Size = new System.Drawing.Size(282, 153);
            this.MessageTextBox.TabIndex = 10;
            this.MessageTextBox.Text = "";
            // 
            // PlayerScore
            // 
            this.PlayerScore.Font = new System.Drawing.Font("Segoe Marker", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PlayerScore.Location = new System.Drawing.Point(15, 310);
            this.PlayerScore.Name = "PlayerScore";
            this.PlayerScore.ReadOnly = true;
            this.PlayerScore.Size = new System.Drawing.Size(56, 20);
            this.PlayerScore.TabIndex = 11;
            // 
            // Timer
            // 
            this.Timer.Font = new System.Drawing.Font("Segoe Marker", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Timer.Location = new System.Drawing.Point(61, 205);
            this.Timer.Name = "Timer";
            this.Timer.ReadOnly = true;
            this.Timer.Size = new System.Drawing.Size(58, 20);
            this.Timer.TabIndex = 12;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe Marker", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(89, 288);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(101, 19);
            this.label6.TabIndex = 13;
            this.label6.Text = "Opponent Score";
            // 
            // OpponentScore
            // 
            this.OpponentScore.Font = new System.Drawing.Font("Segoe Marker", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OpponentScore.Location = new System.Drawing.Point(108, 310);
            this.OpponentScore.Name = "OpponentScore";
            this.OpponentScore.ReadOnly = true;
            this.OpponentScore.Size = new System.Drawing.Size(56, 20);
            this.OpponentScore.TabIndex = 14;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe Marker", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(185, 131);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(78, 19);
            this.label7.TabIndex = 15;
            this.label7.Text = "Enter Word:";
            // 
            // WordEntry
            // 
            this.WordEntry.Enabled = false;
            this.WordEntry.Font = new System.Drawing.Font("Segoe Marker", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.WordEntry.Location = new System.Drawing.Point(262, 130);
            this.WordEntry.Name = "WordEntry";
            this.WordEntry.Size = new System.Drawing.Size(142, 20);
            this.WordEntry.TabIndex = 16;
            this.WordEntry.KeyDown += new System.Windows.Forms.KeyEventHandler(this.EnterPressed);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe Marker", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(24, 229);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(140, 19);
            this.label8.TabIndex = 17;
            this.label8.Text = " You\'re playing against";
            // 
            // OpponentName
            // 
            this.OpponentName.Font = new System.Drawing.Font("Segoe Marker", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OpponentName.Location = new System.Drawing.Point(63, 256);
            this.OpponentName.Name = "OpponentName";
            this.OpponentName.ReadOnly = true;
            this.OpponentName.Size = new System.Drawing.Size(56, 20);
            this.OpponentName.TabIndex = 18;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe Marker", 30F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(142, 9);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(157, 46);
            this.label9.TabIndex = 19;
            this.label9.Text = "BOGGLE!";
            // 
            // ClientGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ClientSize = new System.Drawing.Size(429, 586);
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
            this.Name = "ClientGUI";
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
         private System.Windows.Forms.Label Board1;
         private System.Windows.Forms.Label Board2;
         private System.Windows.Forms.Label Board3;
         private System.Windows.Forms.Label Board4;
         private System.Windows.Forms.Label Board5;
         private System.Windows.Forms.Label Board6;
         private System.Windows.Forms.Label Board7;
         private System.Windows.Forms.Label Board8;
         private System.Windows.Forms.Label Board12;
         private System.Windows.Forms.Label Board11;
         private System.Windows.Forms.Label Board10;
         private System.Windows.Forms.Label Board9;
         private System.Windows.Forms.Label Board13;
         private System.Windows.Forms.Label Board14;
         private System.Windows.Forms.Label Board15;
         private System.Windows.Forms.Label Board16;
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