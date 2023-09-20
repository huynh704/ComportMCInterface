namespace ComportMCInterface
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.txt_Log = new System.Windows.Forms.TextBox();
            this.btn_Connect = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txt_HeadDevice_w = new System.Windows.Forms.TextBox();
            this.txt_HostPort = new System.Windows.Forms.TextBox();
            this.txt_HostIP = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cmb_StopBit = new System.Windows.Forms.ComboBox();
            this.cmb_PartyBit = new System.Windows.Forms.ComboBox();
            this.cmb_DataBit = new System.Windows.Forms.ComboBox();
            this.cmb_BaudRate = new System.Windows.Forms.ComboBox();
            this.cmb_PortName = new System.Windows.Forms.ComboBox();
            this.sr_ComPort = new System.IO.Ports.SerialPort(this.components);
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txt_Log
            // 
            this.txt_Log.Location = new System.Drawing.Point(222, 12);
            this.txt_Log.Multiline = true;
            this.txt_Log.Name = "txt_Log";
            this.txt_Log.ReadOnly = true;
            this.txt_Log.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txt_Log.Size = new System.Drawing.Size(566, 329);
            this.txt_Log.TabIndex = 31;
            this.txt_Log.TextChanged += new System.EventHandler(this.txt_Log_TextChanged);
            // 
            // btn_Connect
            // 
            this.btn_Connect.Location = new System.Drawing.Point(6, 303);
            this.btn_Connect.Name = "btn_Connect";
            this.btn_Connect.Size = new System.Drawing.Size(210, 38);
            this.btn_Connect.TabIndex = 30;
            this.btn_Connect.Text = "Connect";
            this.btn_Connect.UseVisualStyleBackColor = true;
            this.btn_Connect.Click += new System.EventHandler(this.btn_Connect_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txt_HeadDevice_w);
            this.groupBox2.Controls.Add(this.txt_HostPort);
            this.groupBox2.Controls.Add(this.txt_HostIP);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Location = new System.Drawing.Point(6, 192);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(210, 105);
            this.groupBox2.TabIndex = 29;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "PLC Server config";
            // 
            // txt_HeadDevice_w
            // 
            this.txt_HeadDevice_w.Location = new System.Drawing.Point(89, 72);
            this.txt_HeadDevice_w.Name = "txt_HeadDevice_w";
            this.txt_HeadDevice_w.Size = new System.Drawing.Size(100, 20);
            this.txt_HeadDevice_w.TabIndex = 7;
            this.txt_HeadDevice_w.Text = "D 0";
            // 
            // txt_HostPort
            // 
            this.txt_HostPort.Location = new System.Drawing.Point(89, 45);
            this.txt_HostPort.Name = "txt_HostPort";
            this.txt_HostPort.Size = new System.Drawing.Size(55, 20);
            this.txt_HostPort.TabIndex = 6;
            this.txt_HostPort.Text = "1100";
            // 
            // txt_HostIP
            // 
            this.txt_HostIP.Location = new System.Drawing.Point(89, 19);
            this.txt_HostIP.Name = "txt_HostIP";
            this.txt_HostIP.Size = new System.Drawing.Size(100, 20);
            this.txt_HostIP.TabIndex = 5;
            this.txt_HostIP.Text = "192.168.1.39";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(7, 75);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(77, 13);
            this.label8.TabIndex = 3;
            this.label8.Text = "Head Address:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(55, 48);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "Port:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(36, 22);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(48, 13);
            this.label7.TabIndex = 4;
            this.label7.Text = "Address:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cmb_StopBit);
            this.groupBox1.Controls.Add(this.cmb_PartyBit);
            this.groupBox1.Controls.Add(this.cmb_DataBit);
            this.groupBox1.Controls.Add(this.cmb_BaudRate);
            this.groupBox1.Controls.Add(this.cmb_PortName);
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(210, 179);
            this.groupBox1.TabIndex = 28;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Vision comunication config";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 148);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 13);
            this.label5.TabIndex = 29;
            this.label5.Text = "Stop Bit:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 118);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 13);
            this.label4.TabIndex = 30;
            this.label4.Text = "Party Bit:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 88);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 13);
            this.label3.TabIndex = 31;
            this.label3.Text = "Data Bit:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 32;
            this.label2.Text = "Baud Rate:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 33;
            this.label1.Text = "Port Name:";
            // 
            // cmb_StopBit
            // 
            this.cmb_StopBit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_StopBit.FormattingEnabled = true;
            this.cmb_StopBit.Items.AddRange(new object[] {
            "One",
            "Two",
            "OnePointFive"});
            this.cmb_StopBit.Location = new System.Drawing.Point(76, 144);
            this.cmb_StopBit.Name = "cmb_StopBit";
            this.cmb_StopBit.Size = new System.Drawing.Size(113, 21);
            this.cmb_StopBit.TabIndex = 24;
            // 
            // cmb_PartyBit
            // 
            this.cmb_PartyBit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_PartyBit.FormattingEnabled = true;
            this.cmb_PartyBit.Items.AddRange(new object[] {
            "None",
            "Odd",
            "Even",
            "Mark",
            "Space"});
            this.cmb_PartyBit.Location = new System.Drawing.Point(76, 114);
            this.cmb_PartyBit.Name = "cmb_PartyBit";
            this.cmb_PartyBit.Size = new System.Drawing.Size(113, 21);
            this.cmb_PartyBit.TabIndex = 25;
            // 
            // cmb_DataBit
            // 
            this.cmb_DataBit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_DataBit.FormattingEnabled = true;
            this.cmb_DataBit.Items.AddRange(new object[] {
            "5",
            "6",
            "7",
            "8"});
            this.cmb_DataBit.Location = new System.Drawing.Point(76, 84);
            this.cmb_DataBit.Name = "cmb_DataBit";
            this.cmb_DataBit.Size = new System.Drawing.Size(113, 21);
            this.cmb_DataBit.TabIndex = 26;
            // 
            // cmb_BaudRate
            // 
            this.cmb_BaudRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_BaudRate.FormattingEnabled = true;
            this.cmb_BaudRate.Items.AddRange(new object[] {
            "1200",
            "2400",
            "4800",
            "9600",
            "19200",
            "38400",
            "57600",
            "115200",
            "230400"});
            this.cmb_BaudRate.Location = new System.Drawing.Point(76, 54);
            this.cmb_BaudRate.Name = "cmb_BaudRate";
            this.cmb_BaudRate.Size = new System.Drawing.Size(113, 21);
            this.cmb_BaudRate.TabIndex = 27;
            // 
            // cmb_PortName
            // 
            this.cmb_PortName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_PortName.FormattingEnabled = true;
            this.cmb_PortName.Location = new System.Drawing.Point(76, 24);
            this.cmb_PortName.Name = "cmb_PortName";
            this.cmb_PortName.Size = new System.Drawing.Size(113, 21);
            this.cmb_PortName.TabIndex = 28;
            this.cmb_PortName.DropDown += new System.EventHandler(this.cmb_PortName_DropDown);
            // 
            // sr_ComPort
            // 
            this.sr_ComPort.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.sr_ComPort_DataReceived);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(795, 349);
            this.Controls.Add(this.txt_Log);
            this.Controls.Add(this.btn_Connect);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Comport <> MC Protocol";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt_Log;
        private System.Windows.Forms.Button btn_Connect;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txt_HostPort;
        private System.Windows.Forms.TextBox txt_HostIP;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmb_StopBit;
        private System.Windows.Forms.ComboBox cmb_PartyBit;
        private System.Windows.Forms.ComboBox cmb_DataBit;
        private System.Windows.Forms.ComboBox cmb_BaudRate;
        private System.Windows.Forms.ComboBox cmb_PortName;
        private System.IO.Ports.SerialPort sr_ComPort;
        private System.Windows.Forms.TextBox txt_HeadDevice_w;
        private System.Windows.Forms.Label label8;
    }
}

