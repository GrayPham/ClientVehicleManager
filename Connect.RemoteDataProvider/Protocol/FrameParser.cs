using Connect.Common;
using Connect.RemoteDataProvider.Protocol;
using System;
using System.Threading;

namespace nsConnect.RemoteDataProvider.Protocol
{
    public class FrameParser
    {
        #region Private
        //**--------------------------------------------------------------------------------

        private const int TimeFrameExpired = 10000;
        private bool _isStartFrameWaiting = true;
        private Timer _timer;
        private Boolean _isTimeOut;
        private Boolean _isProcessing;
        private readonly object _locker = new object();
        private readonly object _parseLocker = new object();
        private enum ProcessStep
        {
            GetSignature,
            GetPackageNumber,
            GetFunctionCode,
            GetFrameDataLength,
            GetFrameData
        }
        private Frame _frame;
        private int _posSignature;
        private int _pos;
        private readonly byte[] _data = new byte[4];
        private ProcessStep _step;

        //**--------------------------------------------------------------------------------
        #endregion

        #region Event
        //**--------------------------------------------------------------------------------

        /// <summary>
        /// Fired from other thread
        /// </summary>
        public event GeneralEventHandler<Frame> ValidFrameDetected;
        private void OnValidFrameDetected(Frame frame)
        {
            if (ValidFrameDetected != null) ValidFrameDetected(this, new EventArgs<Frame>(frame));
        }

        /// <summary>
        /// Fired from other thread
        /// </summary>
        public event EventHandler TimeOut;
        private void OnTimeOut()
        {
            if (TimeOut != null) TimeOut(this, EventArgs.Empty);
        }

        //**--------------------------------------------------------------------------------
        #endregion

        #region Public Method
        //**--------------------------------------------------------------------------------

        public void Parse(Byte[] data)
        {
            if (data == null) return;
            if (data.Length == 0) return;
            lock (_parseLocker)
            {

                var pos = 0;
                lock (_locker)
                {
                    _isProcessing = true;
                    if (_isTimeOut)
                    {
                        _isTimeOut = false;
                        _isStartFrameWaiting = true;
                        _posSignature = 0;
                    }
                }

                while (pos < data.Length)
                {
                    var b = data[pos];
                    if (_isStartFrameWaiting)
                    {
                        if (b == Frame.StartBytes[_posSignature])
                        {
                            _posSignature++;
                            if (_posSignature == Frame.StartBytes.Length)
                            {
                                // Start signature matched here
                                _isStartFrameWaiting = false;
                                _posSignature = 0;
                                _pos = 0;
                                _frame = new Frame();
                                _step = ProcessStep.GetSignature;
                                _timer = new Timer(TimerTrigger, null, TimeFrameExpired, Timeout.Infinite);
                            }
                        }
                        else
                        {
                            _posSignature = 0;
                            // Recheck if a start byte dectected again
                            if (b == Frame.StartBytes[_posSignature]) _posSignature = 1;
                        }
                    }
                    else // Get Data here
                    {
                        switch (_step)
                        {
                            case ProcessStep.GetSignature:
                                _data[_pos] = b;
                                _pos++;
                                if (_pos >= 4)
                                {
                                    _frame.Signature = BitConverter.ToUInt32(_data, 0);
                                    _step = ProcessStep.GetPackageNumber;
                                    _pos = 0;
                                }
                                break;
                            case ProcessStep.GetPackageNumber:
                                _data[_pos] = b;
                                _pos++;
                                if (_pos >= 4)
                                {
                                    _frame.FrameID = BitConverter.ToInt32(_data, 0);
                                    _step = ProcessStep.GetFunctionCode;
                                    _pos = 0;
                                }
                                break;
                            case ProcessStep.GetFunctionCode:
                                _data[_pos] = b;
                                _pos++;
                                if (_pos >= 2)
                                {
                                    _frame.FunctionCode = BitConverter.ToUInt16(_data, 0);
                                    _step = ProcessStep.GetFrameDataLength;
                                    _pos = 0;
                                }
                                break;
                            case ProcessStep.GetFrameDataLength:
                                _data[_pos] = b;
                                _pos++;
                                if (_pos >= 4)
                                {
                                    _frame.Data = new byte[0];
                                    _frame.DataLength = BitConverter.ToUInt32(_data, 0);
                                    if (_frame.DataLength == 0)
                                    {
                                        // Valid Frame Detect
                                        _timer.Change(Timeout.Infinite, Timeout.Infinite);
                                        _timer.Dispose();
                                        lock (_locker)
                                        {
                                            _isTimeOut = false;
                                        }
                                        _isStartFrameWaiting = true;
                                        _posSignature = 0;
                                        OnValidFrameDetected(_frame);
                                        break;
                                    }
                                    _step = ProcessStep.GetFrameData;
                                    _pos = 0;
                                    _frame.Data = new byte[_frame.DataLength];
                                }
                                break;
                            case ProcessStep.GetFrameData:
                                if (_frame == null) break;
                                _frame.Data[_pos] = b;
                                _pos++;
                                if (_pos >= _frame.DataLength)
                                {
                                    // Valid Frame Detect
                                    _timer.Change(Timeout.Infinite, Timeout.Infinite);
                                    _timer.Dispose();
                                    lock (_locker)
                                    {
                                        _isTimeOut = false;
                                    }
                                    _isStartFrameWaiting = true;
                                    _posSignature = 0;
                                    OnValidFrameDetected(_frame);
                                }
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                    pos++;
                }

                lock (_locker)
                {
                    _isProcessing = false;
                    if (_isTimeOut) OnTimeOut();
                }
            }
        }

        public void Reset()
        {
            _isTimeOut = false;
            _isStartFrameWaiting = true;
            _posSignature = 0;
        }

        //**--------------------------------------------------------------------------------
        #endregion

        #region Private Method
        //**--------------------------------------------------------------------------------

        private void TimerTrigger(object obj)
        {
            lock (_locker)
            {
                _isTimeOut = true;
                if (!_isProcessing) OnTimeOut();
            }
        }

        //**--------------------------------------------------------------------------------
        #endregion
    }
}
