using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime;
using System.Security;

namespace ComportMCInterface
{
    public partial class MainForm : Form
    {
        string _ComportReceiveData = string.Empty;

        bool _ClientOpen = false;
        byte[] dataRecv;
        byte[] dataSend;
        TcpClient _Client = new TcpClient();
        NetworkStream _Stream;
        ASCIIEncoding encoding = new ASCIIEncoding();
        public MainForm()
        {
            InitializeComponent();
            cmb_PortName.Items.AddRange(SerialPort.GetPortNames());
            cmb_PortName.SelectedIndex = 0;
            cmb_BaudRate.SelectedIndex = 3;
            cmb_DataBit.SelectedIndex = 3;
            cmb_PartyBit.SelectedIndex = 0;
            cmb_StopBit.SelectedIndex = 0;

            CheckForIllegalCrossThreadCalls = false;
            Thread _MainReceive = new Thread(MainThread);
            _MainReceive.Start();
            _MainReceive.IsBackground = true;
            int[] _value = { 1, 2, 3, 4, 5, 6 };
            WriteDeviceBlock("ZR 1000", _value);
        }
        private void logDisplay(string message)
        {
            message = message.IndexOf(Environment.NewLine) >= 0 ? message : message + Environment.NewLine;
            txt_Log.Text += '[' + DateTime.Now.ToString("HH:mm:ss.fff") + "] " + message;
        }
        private void MainThread()
        {
            while (true)
            {
                try
                {
                    if (_ClientOpen)
                    {
                        dataRecv = new byte[_Client.ReceiveBufferSize];
                        int length = _Stream.Read(dataRecv, 0, dataRecv.Length);
                        string s_receive = encoding.GetString(dataRecv, 0, length);
                        logDisplay("[Server>]: " + s_receive);
                    }
                    Thread.Sleep(1);
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        #region Event region
        private void cmb_PortName_DropDown(object sender, EventArgs e)
        {
            cmb_PortName.Items.Clear();
            cmb_PortName.Items.AddRange(SerialPort.GetPortNames());
        }
        private void btn_Connect_Click(object sender, EventArgs e)
        {
            //Serial comport connect
            if (sr_ComPort.IsOpen)
            {
                sr_ComPort.Close();
                logDisplay("[System]Comport is closed" + Environment.NewLine);
            }
            else
            {
                try
                {
                    txt_Log.Clear();
                    sr_ComPort.PortName = cmb_PortName.SelectedItem.ToString();
                    sr_ComPort.BaudRate = Convert.ToInt32(cmb_BaudRate.SelectedItem);
                    sr_ComPort.DataBits = Convert.ToInt32(cmb_DataBit.SelectedItem);
                    sr_ComPort.Parity = (Parity)Enum.Parse(typeof(Parity), cmb_PartyBit.Text);
                    sr_ComPort.StopBits = (StopBits)Enum.Parse(typeof(StopBits), cmb_StopBit.Text);
                    sr_ComPort.Open();
                    logDisplay("[System]Comport is open " + sr_ComPort.PortName + Environment.NewLine);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            //Server TCP Connect
            if (!_Client.Connected)
            {
                try
                {
                    _Client = new TcpClient();
                    _Client.SendTimeout = 1000;
                    _Client.Connect(txt_HostIP.Text, int.Parse(txt_HostPort.Text));
                    _Stream = _Client.GetStream();
                    logDisplay("[System]Server connected " + txt_HostIP.Text + Environment.NewLine);
                    _ClientOpen = true;
                }
                catch (Exception ex)
                {
                    _ClientOpen = false;
                    MessageBox.Show(ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                _Client.Close();
                _ClientOpen = false;
                logDisplay("[System]Server dissconnect" + Environment.NewLine);
            }
            //Check connect
            if (sr_ComPort.IsOpen && _Client.Connected)
            {
                btn_Connect.Text = "Disconnect";
                btn_Connect.BackColor = Color.LightGreen;
            }
            else
            {
                _Client.Close();
                sr_ComPort.Close();
                btn_Connect.Text = "Connect";
                btn_Connect.BackColor = Color.Transparent;
            }
        }
        private void sr_ComPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            _ComportReceiveData = sr_ComPort.ReadLine();
            logDisplay("[Vision>]: " + _ComportReceiveData);
        }
        private void txt_Log_TextChanged(object sender, EventArgs e)
        {
            txt_Log.SelectionStart = txt_Log.Text.Length;
            txt_Log.ScrollToCaret();
            if (txt_Log.Text.Length > UInt16.MaxValue) txt_Log.Clear();
        }
        #endregion

        #region MC Protocol funtion
        // GetAddress(string address)
        // Summary:
        //     An array of bytes with length 8.
        //
        // Parameters:
        //   address:
        //      The string of device address
        //      format: "RegisterSymbol RegisterBinary"
        //      example: ZR1000 is "ZR 1000"
        //
        // Returns:
        //     An array of bytes with length 8.
        private byte[] GetAddress(string address)
        {
            byte[] _Address = new byte[4];
            string RegisterSymbol = address.Split(' ')[0].Trim();
            int RegisterBinary = Convert.ToInt16(address.Split(' ')[1].Trim());
            _Address = BitConverter.GetBytes(RegisterBinary);
            switch (RegisterSymbol)
            {
                case "D":
                    {
                        _Address[3] = 0xA8;
                        break;
                    }
                case "W":
                    {
                        _Address[3] = 0xB4;
                        break;
                    }
                case "R":
                    {
                        _Address[3] = 0xAF;
                        break;
                    }
                case "ZR":
                    {
                        _Address[3] = 0xB0;
                        break;
                    }
                case "SD":
                    {
                        _Address[3] = 0xA9;
                        break;
                    }

            }
            return _Address;
        }
        private int WriteDeviceBlock(string szDevice, int[] Value)
        {
            // Setting method for 4E frame
            // 21 byte (Header + Subheader + Access route + Request data + Monitoring timer) + Value.Length * 2 (Request data)
            byte[] _WriteBuffer = new byte[21 + Value.Length * 2];

            //Header Subheader Access route
            _WriteBuffer[0] = 0x50; //Header 1byte
            _WriteBuffer[1] = 0x00; //SubHeader (Free) 6byte
            _WriteBuffer[2] = 0x00; //SubHeader (Free) 6byte
            _WriteBuffer[3] = 0x01; //SubHeader (Serial) 6byte
            _WriteBuffer[4] = 0x00; //SubHeader (Serial) 6byte
            _WriteBuffer[5] = 0x00; //SubHeader (Fixed) 6byte
            _WriteBuffer[6] = 0x54; //SubHeader (Fixed) 6byte
            Array.Copy(BitConverter.GetBytes(Value.Length * 2 + 12), 0, _WriteBuffer, 7, 2); //RequestData (Monitoring timer ~ Request data)Byte
            _WriteBuffer[9] = 0x10; //Monitoring timer (250 ms per value)
            _WriteBuffer[10] = 0x00; //Monitoring timer (250 ms per value)
            //Message format batch write (0x00, 0x00, 0x14, 0x01)
            //Message format batch read (0x00, 0x00, 0x04, 0x01)
            _WriteBuffer[11] = 0x01; // Command 4C/3C/4E/3E frame
            _WriteBuffer[12] = 0x14; // Command 4C/3C/4E/3E frame
            _WriteBuffer[13] = 0x00; // Subcommand For MELSEC-Q/L series
            _WriteBuffer[14] = 0x00; // Subcommand For MELSEC-Q/L series
            Array.Copy(GetAddress(szDevice), 0, _WriteBuffer, 15, 4); //Device name (3byte head number, 1 byte device code)
            Array.Copy(BitConverter.GetBytes(Value.Length), 0, _WriteBuffer, 19, 2); //Data length (Word)
            for(int i = 0; i < Value.Length; i++)
            {
                _WriteBuffer[21 + i * 2] = BitConverter.GetBytes(Value[i])[0];
                _WriteBuffer[22 + i * 2] = BitConverter.GetBytes(Value[i])[1];
            }
            _Stream.Write(_WriteBuffer, 0, _WriteBuffer.Length);
            
            //Wait response message
            byte[] _SendHeader = new byte[8];
            byte[] _ReceiveHeader = new byte[8];
            double _TimeOut;
            Array.Copy(_WriteBuffer, _SendHeader, 8);
            DateTime _timeStart = DateTime.Now;
            do
            {
                _TimeOut = (DateTime.Now - _timeStart).TotalMilliseconds;
                Array.Copy(dataRecv, _ReceiveHeader, 8);
            } while (Array.Equals(_SendHeader, _ReceiveHeader) || _TimeOut > 5000);
            if (_TimeOut > 5000) return -1;
            else return dataRecv[11] << 8 | dataRecv[12];
        }
        #endregion
    }
}
