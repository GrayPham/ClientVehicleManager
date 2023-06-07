using Connect.Common;
using Connect.Common.Interface;
using Connect.Common.Logging;
using nsFramework.Common.Pattern;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace Connect.SocketClient
{
    public class SerialPortMin : IPort
    {
        #region Private Member
        //-----------------------------------------------------------------------------------------------------

        protected System.IO.Ports.SerialPort SerialPort;
        protected readonly ILog Log;
        protected Timer Timer100Ms;

        //-----------------------------------------------------------------------------------------------------
        #endregion


        #region Interface Implementation
        //-----------------------------------------------------------------------------------------------------

        #region Interface Implementation - Properties
        //-----------------------------------------------------------------------------------------------------

        public string Port
        {
            get
            {
                if (SerialPort != null)
                {
                    return SerialPort.PortName;
                }
                return null;
            }
            set
            {
                if (SerialPort != null)
                {
                    SerialPort.PortName = value;
                }
            }
        }

        public bool IsOpened
        {
            get
            {
                if (SerialPort != null)
                {
                    return SerialPort.IsOpen;
                }
                return false;
            }
        }

        //-----------------------------------------------------------------------------------------------------
        #endregion

        public void Open()
        {
            Log.Trace("");
            Debug.Assert(SerialPort != null, "Port is null");

            try
            {
                SerialPort.Open();
                OnL2LMessageUpdate(L2LMessageType.Information, L2LCode.OpenSucceed);
                OnOpenResponsed(true);
                OnOpenStateChanged(true);
            }
            catch (UnauthorizedAccessException ex)
            {
                Log.Error(ex.Message);
                OnL2LMessageUpdate(L2LMessageType.Error, L2LCode.AccessDenied);
                OnOpenResponsed(false);
            }
            catch (IOException ex)
            {
                Log.Error(ex.Message);
                OnL2LMessageUpdate(L2LMessageType.Error, L2LCode.InvalidState);
                OnOpenResponsed(false);
            }
            catch (InvalidOperationException ex)
            {
                Log.Error(ex.Message);
                OnL2LMessageUpdate(L2LMessageType.Error, L2LCode.AlreadyOpen);
                OnOpenResponsed(false);
            }
            catch (ArgumentException ex)
            {
                Log.Error(ex.Message);
                OnL2LMessageUpdate(L2LMessageType.Error, L2LCode.InvalidParameter);
                OnOpenResponsed(false);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                OnL2LMessageUpdate(L2LMessageType.Error, L2LCode.UnknownError);
                OnOpenResponsed(false);
            }
        }

        public void Close()
        {
            Log.Trace("");
            Debug.Assert(SerialPort != null, "Port is null");

            try
            {
                SerialPort.Close();
                OnCloseResponsed(true);
                OnOpenStateChanged(false);
            }
            catch (IOException ex)
            {
                Log.Error(ex.Message);
                OnL2LMessageUpdate(L2LMessageType.Error, L2LCode.InvalidState);
                OnCloseResponsed(true);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                OnL2LMessageUpdate(L2LMessageType.Error, L2LCode.UnknownError);
                OnCloseResponsed(false);
            }
        }

        public void Write(string message)
        {
            Log.Trace("");

            Debug.Assert(SerialPort != null, "Port is null");

            try
            {
                SerialPort.Write(message);
                OnWriteResponsed(true);
            }
            catch (ArgumentNullException ex)
            {
                Log.Error(ex.Message);
                OnL2LMessageUpdate(L2LMessageType.Error, L2LCode.BufferNull);

                OnWriteResponsed(false);
            }
            catch (InvalidOperationException ex)
            {
                Log.Error(ex.Message);
                OnL2LMessageUpdate(L2LMessageType.Error, L2LCode.PortIsNotOpen);
                OnWriteResponsed(false);
            }
            catch (TimeoutException ex)
            {
                Log.Error(ex.Message);
                OnL2LMessageUpdate(L2LMessageType.Error, L2LCode.WriteTimeOut);
                OnWriteResponsed(false);
            }
            catch (ArgumentException ex)
            {
                Log.Error(ex.Message);
                OnL2LMessageUpdate(L2LMessageType.Error, L2LCode.WriteInvalidParameter);
                OnWriteResponsed(false);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                OnL2LMessageUpdate(L2LMessageType.Error, L2LCode.UnknownError);
                OnWriteResponsed(false);
            }
        }

        public void Write(byte[] data)
        {
            Log.Trace("");
            Debug.Assert(SerialPort != null, "Port is null");

            try
            {
                SerialPort.Write(data, 0, data.Length);
                OnWriteResponsed(true);
            }
            catch (ArgumentNullException ex)
            {
                Log.Error(ex.Message);
                OnL2LMessageUpdate(L2LMessageType.Error, L2LCode.BufferNull);

                OnWriteResponsed(false);
            }
            catch (InvalidOperationException ex)
            {
                Log.Error(ex.Message);
                OnL2LMessageUpdate(L2LMessageType.Error, L2LCode.PortIsNotOpen);
                OnWriteResponsed(false);
            }
            catch (TimeoutException ex)
            {
                Log.Error(ex.Message);
                OnL2LMessageUpdate(L2LMessageType.Error, L2LCode.WriteTimeOut);
                OnWriteResponsed(false);
            }
            catch (ArgumentException ex)
            {
                Log.Error(ex.Message);
                OnL2LMessageUpdate(L2LMessageType.Error, L2LCode.WriteInvalidParameter);
                OnWriteResponsed(false);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                OnL2LMessageUpdate(L2LMessageType.Error, L2LCode.UnknownError);
                OnWriteResponsed(false);
            }
        }

        #region Public Events

        public event BooleanEventHandler OpenResponsed;
        public event BooleanEventHandler CloseResponsed;
        public event BooleanEventHandler WriteResponsed;
        public event BooleanEventHandler OpenStateChanged;
        public event DataEventHandler DataReceived;
        public event L2LMessageEventHandler L2LMessageUpdated;
        public event EventHandler Tick;

        private void OnOpenResponsed(Boolean succeed)
        {
            Log.Trace("");

            if (OpenResponsed != null) OpenResponsed(this, new BooleanEventArgs(succeed));
        }

        private void OnCloseResponsed(Boolean succeed)
        {
            Log.Trace("");

            if (CloseResponsed != null) CloseResponsed(this, new BooleanEventArgs(succeed));
        }

        private void OnWriteResponsed(Boolean succeed)
        {
            Log.Trace("");

            if (WriteResponsed != null) WriteResponsed(this, new BooleanEventArgs(succeed));
        }

        private void OnOpenStateChanged(Boolean state)
        {
            Log.Trace("");

            if (OpenStateChanged != null) OpenStateChanged(this, new BooleanEventArgs(state));
        }

        private void OnDataReceived(Byte[] data)
        {
            Log.Trace("");

            if (DataReceived != null) DataReceived(this, new DataEventArgs(data));
        }

        private void OnL2LMessageUpdate(L2LMessageType type, L2LCode code)
        {
            Log.Trace("");

            if (L2LMessageUpdated != null) L2LMessageUpdated(this, new L2LMessageEventArgs(type, Resource.GetCodeText(code), (int)code));
        }

        private void OnTick()
        {
            Log.Trace("");

            if (Tick != null) Tick(this, EventArgs.Empty);
        }
        #endregion

        //-----------------------------------------------------------------------------------------------------
        #endregion

        #region Public
        //-----------------------------------------------------------------------------------------------------

        public SerialPortMin(System.IO.Ports.SerialPort serialPort, ILog log)
        {
            Log = log ?? Singleton<DummyLog>.Instance;

            if (serialPort != null)
            {
                SerialPort = serialPort;
                serialPort.DataReceived += OnSerialPortDataReceived;
            }

            Timer100Ms = new Timer(TimerCallback, null, 100, 100);
        }

        public SerialPortMin(System.IO.Ports.SerialPort serialPort)
        {
            Log = Singleton<DummyLog>.Instance;

            if (serialPort != null)
            {
                SerialPort = serialPort;
                serialPort.DataReceived += OnSerialPortDataReceived;
            }

            Timer100Ms = new Timer(TimerCallback, null, 100, 100);
        }

        ~SerialPortMin()
        {
            Timer100Ms.Dispose();
            if (SerialPort != null)
            {
                SerialPort.Close();
                SerialPort.Dispose();
            }
        }

        public void InvokeTick()
        {
            throw new NotImplementedException();
        }

        //-----------------------------------------------------------------------------------------------------
        #endregion

        #region Private Method
        //-----------------------------------------------------------------------------------------------------

        void TimerCallback(object obj)
        {
            OnTick();
        }

        protected void OnSerialPortDataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            Log.Trace("");
            Debug.Assert(SerialPort != null, "Port is null");

            var length = SerialPort.BytesToRead;
            var buf = new byte[length];
            var readLength = SerialPort.Read(buf, 0, length);
            if (readLength == 0) return;
            OnDataReceived(buf);
        }

        //-----------------------------------------------------------------------------------------------------
        #endregion
    }
}
