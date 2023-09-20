﻿using System;
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
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;

namespace ComportMCInterface
{
    public partial class MainForm : Form
    {
        ushort _sqMain = 0;

        string _ComportReceiveData = string.Empty;

        bool _ClientOpen = false;
        byte[] dataRecv;
        short[] dataSend;
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

            //CheckForIllegalCrossThreadCalls = false;
            Thread _MainReceive = new Thread(MainReceive);
            _MainReceive.Start();
            _MainReceive.IsBackground = true;

            Thread _GUIScan = new Thread(GUIScan);
            _GUIScan.Start();
            _GUIScan.IsBackground = true;

            Thread _AutoThread = new Thread(AutoThread);
            _AutoThread.Start();
            _AutoThread.IsBackground = true;


            //int[] _value = { 1, 2, 3, 4, 5, 6 };
            //WriteDeviceBlock("ZR 1000", _value);
        }
        private void logDisplay(string message)
        {
            message = message.IndexOf(Environment.NewLine) >= 0 ? message : message + Environment.NewLine;
            this.Invoke(new Action(() =>
            {
                txt_Log.Text += '[' + DateTime.Now.ToString("HH:mm:ss.fff") + "] " + message;
            }));
        }
        private void MainReceive()
        {
            dataRecv = new byte[_Client.ReceiveBufferSize];
            while (true)
            {
                try
                {
                    if (_ClientOpen)
                    {
                        if (_Client.Available > 0)
                        {
                            dataRecv = new byte[_Client.ReceiveBufferSize];
                            int length = _Stream.Read(dataRecv, 0, dataRecv.Length);
                            string s_receive = encoding.GetString(dataRecv, 0, length);
                            //logDisplay("[Server>]: Receive data complete");
                        }
                    }
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void GUIScan()
        {
            while (true)
            {
                try
                {
                    //this.Invoke(new Action(() =>
                    //{
                    //    this.Text = "Comport <> MC Protocol " + DateTime.Now.ToString("HH:mm:ss.fff");
                    //}));
                }
                catch { }
            }
        }
        private void AutoThread()
        {
            DateTime _ReconnectTime = DateTime.Now;
            while (true)
            {
                switch (_sqMain)
                {
                    case 0: // wait connecttion
                        {
                            if (_ClientOpen && sr_ComPort.IsOpen) _sqMain++;
                            break;
                        }
                    case 1: //wait serial data
                        {
                            if (_ComportReceiveData != string.Empty) _sqMain++;
                            break;
                        }
                    case 2: // check data format
                        {
                            short _iBuffer = 0;
                            string[] _SerialData = _ComportReceiveData.Split(',');
                            _sqMain++;

                            if (_SerialData.Length != 5)
                            {
                                logDisplay("[Vision] Data length is not correct");
                                _sqMain = 1;
                            }
                            else if(txt_HeadDevice_w.Text.Split(' ').Length != 2)
                            {
                                logDisplay("[System]Head address is not correct format");
                                _ComportReceiveData = string.Empty;
                                _sqMain = 1;
                            }
                            else
                            {
                                dataSend = new short[_SerialData.Length];
                                for (int i = 0; i < _SerialData.Length; i++)
                                {
                                    _SerialData[i] = _SerialData[i].Trim();
                                    if (short.TryParse(_SerialData[i], out _iBuffer)) dataSend[i] = _iBuffer;
                                    else
                                    {
                                        logDisplay("[Vision] Data format is not correct " + _SerialData[i]);
                                        _sqMain = 1;
                                        break;
                                    }
                                }
                            }
                            _ComportReceiveData = string.Empty;
                            break;
                        }
                    case 3: //write data to PLC
                        {
                            int iResult = WriteDeviceBlock(txt_HeadDevice_w.Text, dataSend);
                            if (iResult == 0)
                            {
                                logDisplay("[PLC>] Set data to PLC");
                                _sqMain = 1;
                            }
                            else if (iResult == -2)
                            {
                                logDisplay("[System] TCP Client connection error");
                                _sqMain = 5;
                            }
                            break;
                        }
                    case 4: //Reconnect
                        {
                            _ClientOpen = false;
                            sr_ComPort.Close();
                            if((DateTime.Now - _ReconnectTime).TotalMilliseconds > 1000)
                            {
                                _ClientOpen = false;
                                sr_ComPort.Close();
                                btn_Connect_Click(null, null);
                                if(_Client.Connected && sr_ComPort.IsOpen)
                                {
                                    logDisplay("[System] Reconnect complete");
                                    _sqMain = 0;
                                }
                            }
                            break;
                        }
                    case 5:
                        {
                            btn_Connect.BackColor = Color.Transparent;
                            btn_Connect.Invoke(new Action(() =>
                            {
                                btn_Connect.Text = "Connect";
                            }));
                            sr_ComPort.Close();
                            _Client.Close();
                            _ClientOpen = false;
                            _sqMain = 0;
                            break;
                        }
                }
                try
                {
                    this.Invoke(new Action(() =>
                    {
                        this.Text = "Comport <> MC Protocol " + _sqMain.ToString("(0) ") + DateTime.Now.ToString("HH:mm:ss.fff");
                    }));
                }
                catch { }
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
            if (!_ClientOpen)
            {
                try
                {
                    _Client = new TcpClient();
                    _Client.SendBufferSize = 1024;
                    _Client.ReceiveBufferSize = 1024;
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
            //if (sr_ComPort.IsOpen)
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
            //txt_Log.SelectionStart = txt_Log.Text.Length;
            txt_Log.ScrollToCaret();
            if (txt_Log.Text.Length > UInt16.MaxValue) txt_Log.Clear();
        }
        #endregion

        #region MC Protocol funtion
        // GetAddress(string address)
        // Summary:
        //     Convert string address to byte[] address MELSEC Communication Protocol
        //
        // Parameters:
        //   address:
        //      The string of device address
        //      format: "RegisterSymbol RegisterBinary"
        //      example: ZR1000 is "ZR 1000"
        //
        // Returns:
        //     byte[4] address MELSEC Communication Protocol
        private byte[] GetAddress(string address)
        {
            byte[] _Address = new byte[4];
            string RegisterSymbol;
            int RegisterBinary;

            try
            {
                RegisterSymbol = address.Split(' ')[0].Trim();
                RegisterBinary = Convert.ToInt16(address.Split(' ')[1].Trim());
            }
            catch (Exception ex)
            {
                logDisplay('[' + ex.Source + "] " + ex.Message + Environment.NewLine + address);
                return _Address;
            }
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
                case "M":
                    {
                        _Address[3] = 0x90;
                        break;
                    }
                case "L":
                    {
                        _Address[3] = 0x92;
                        break;
                    }
            }
            return _Address;
        }
        private int WriteDeviceBlock(string szDevice, short[] Value)
        {
            // Setting method for 3E frame
            // 21 byte (Header + Subheader + Access route + Request data + Monitoring timer) + Value.Length * 2 (Request data)
            byte[] _WriteBuffer = new byte[21 + Value.Length * 2];

            //Header Subheader Access route
            _WriteBuffer[0] = 0x50; //SubHeader (Free) 2byte
            _WriteBuffer[1] = 0x00; //SubHeader (Free) 2byte
            _WriteBuffer[2] = 0x00; //Network No
            _WriteBuffer[3] = 0xFF; //PC No
            _WriteBuffer[4] = 0xFF; //Request Module IO No
            _WriteBuffer[5] = 0x03; //Request Module IO No
            _WriteBuffer[6] = 0x00; //Request Station IO No
            Array.Copy(BitConverter.GetBytes(Value.Length * 2 + 12), 0, _WriteBuffer, 7, 2); //RequestData (Monitoring timer ~ Request data)Byte
            _WriteBuffer[9] = 0x10; //Monitoring timer (250 ms per value)
            _WriteBuffer[10] = 0x00; //Monitoring timer (250 ms per value)
            //Message format batch write (0x00, 0x00, 0x14, 0x01)
            _WriteBuffer[11] = 0x01; // Command 4C/3C/4E/3E frame
            _WriteBuffer[12] = 0x14; // Command 4C/3C/4E/3E frame
            _WriteBuffer[13] = 0x00; // Subcommand For MELSEC-Q/L series
            _WriteBuffer[14] = 0x00; // Subcommand For MELSEC-Q/L series
            Array.Copy(GetAddress(szDevice), 0, _WriteBuffer, 15, 4); //Device name (3byte head number, 1 byte device code)
            Array.Copy(BitConverter.GetBytes(Value.Length), 0, _WriteBuffer, 19, 2); //Data length (Word)
            for (int i = 0; i < Value.Length; i++)
            {
                _WriteBuffer[21 + i * 2] = BitConverter.GetBytes(Value[i])[0];
                _WriteBuffer[22 + i * 2] = BitConverter.GetBytes(Value[i])[1];
            }
            Array.Clear(dataRecv, 0, dataRecv.Length);
            try
            {
                _Stream.Write(_WriteBuffer, 0, _WriteBuffer.Length);
            }
            catch
            {
                return -2;
            }
            //Wait response message
            double _TimeOut;
            DateTime _timeStart = DateTime.Now;
            while (dataRecv[0] == 0)
            {
                _TimeOut = (DateTime.Now - _timeStart).TotalMilliseconds;
                if (_TimeOut > 1000) return -1;
            }
            //Return end code
            return dataRecv[12] << 8 | dataRecv[11];
        }
        private int ReadDeviceBlock(string szDevice, int length, out short[] Value)
        {
            // Setting method for 3E frame
            // 21 byte (Header + Subheader + Access route + Request data + Monitoring timer)
            byte[] _WriteBuffer = new byte[21];
            short[] _ReadBuffer = new short[length];
            Value = new short[length];

            //Header Subheader Access route
            _WriteBuffer[0] = 0x50; //SubHeader (Free) 2byte
            _WriteBuffer[1] = 0x00; //SubHeader (Free) 2byte
            _WriteBuffer[2] = 0x00; //Network No
            _WriteBuffer[3] = 0xFF; //PC No
            _WriteBuffer[4] = 0xFF; //Request Module IO No
            _WriteBuffer[5] = 0x03; //Request Module IO No
            _WriteBuffer[6] = 0x00; //Request Station IO No
            Array.Copy(BitConverter.GetBytes(12), 0, _WriteBuffer, 7, 2); //RequestData (Monitoring timer ~ Request data)Byte
            _WriteBuffer[9] = 0x10; //Monitoring timer (250 ms per value)
            _WriteBuffer[10] = 0x00; //Monitoring timer (250 ms per value)
            //Message format batch read (0x00, 0x00, 0x04, 0x01)
            _WriteBuffer[11] = 0x01; // Command 4C/3C/4E/3E frame
            _WriteBuffer[12] = 0x04; // Command 4C/3C/4E/3E frame
            _WriteBuffer[13] = 0x00; // Subcommand For MELSEC-Q/L series
            _WriteBuffer[14] = 0x00; // Subcommand For MELSEC-Q/L series
            Array.Copy(GetAddress(szDevice), 0, _WriteBuffer, 15, 4); //Device name (3byte head number, 1 byte device code)
            Array.Copy(BitConverter.GetBytes(length), 0, _WriteBuffer, 19, 2); //Data length (Word)

            Array.Clear(dataRecv, 0, dataRecv.Length);
            try
            {
                _Stream.Write(_WriteBuffer, 0, _WriteBuffer.Length);
            }
            catch
            {
                return -2;
            }

            //Wait response message
            double _TimeOut;
            DateTime _timeStart = DateTime.Now;
            while (dataRecv[0] == 0)
            {
                _TimeOut = (DateTime.Now - _timeStart).TotalMilliseconds;
                if (_TimeOut > 5000) return -1;
            }
            //Check End code normal == 0
            if ((dataRecv[10] << 8 | dataRecv[9]) != 0) return dataRecv[10] << 8 | dataRecv[9];
            //Get endpoint of response data
            int _dataResLeng = ((dataRecv[8] << 8 | dataRecv[7]) - 2) / 2;
            for (int i = 0; i < _dataResLeng; i++)
            {
                Value[i] = (short)(dataRecv[12 + i * 2] << 8 | dataRecv[11 + i * 2]);
            }
            //Return end code
            return dataRecv[10] << 8 | dataRecv[9];
        }
        private int WriteDevice(string szDevice, short Value)
        {
            short[] data = new short[1] { Value };
            return WriteDeviceBlock(szDevice, data);
        }
        private int ReadDevice(string szDevice, out short Value)
        {
            short[] data;
            Value = 0;

            int iResult = ReadDeviceBlock(szDevice, 1, out data);
            if (iResult == 0) Value = data[0];
            return iResult;
        }

        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            int iResult = 0;
            short[] _Read;
            txt_HeadDevice_w.Text = txt_HeadDevice_w.Text.ToUpper();
            iResult = ReadDeviceBlock(txt_HeadDevice_w.Text, 10, out _Read);
            if (iResult == 0)
            {
                for (int i = 0; i < _Read.Length; i++)
                {
                    logDisplay(txt_HeadDevice_w.Text.Split(' ')[0] + (Convert.ToInt16(txt_HeadDevice_w.Text.Split(' ')[1]) + i).ToString("0000") + ": " + _Read[i].ToString());
                }
            }
        }

        private void btn_Write_Click(object sender, EventArgs e)
        {
            int iResult = 0;
            txt_HeadDevice_w.Text = txt_HeadDevice_w.Text.ToUpper();
            short[] _Write = new short[10];
            Random _byteRND = new Random();
            for (int i = 0; i < _Write.Length; i++)
            {
                _Write[i] = (short)_byteRND.Next(0, 65535);
            }
            iResult = WriteDeviceBlock(txt_HeadDevice_w.Text, _Write);
        }
    }
}
