using System;

namespace Connect.Common.Collection
{
    public class ThreadSafeQueue<T>
    {
        private readonly Object _tLock;
        private readonly T[] _buf;

        private int _getFromIndex;
        private int _putToIndex;
        private readonly int _size;
        private int _numItems;

        public int Count { get { lock (_tLock) return _numItems; } }

        public ThreadSafeQueue(int capacity)
        {
            if (capacity <= 0) throw new ArgumentOutOfRangeException("capacity");

            _tLock = new Object();
            _buf = new T[capacity];

            _getFromIndex = 0;
            _putToIndex = 0;
            _size = capacity;
            _numItems = 0;
        }

        /// <summary>
        /// Queue an item if no error happens
        /// </summary>
        /// <param name="item">Item will be queued</param>
        /// <returns>true if success, false if error or full</returns>
        public bool TryQueue(T item)
        {
            lock (_tLock)
            {
                if (_numItems == _size) return false;
                _buf[_putToIndex] = item;
                _putToIndex++;
                if (_putToIndex == _size) _putToIndex = 0;
                _numItems++;
            }
            return true;
        }

        /// <summary>
        /// Dequeue an item if no error happens
        /// </summary>
        /// <param name="?">Item that get from Queue</param>
        /// <param name="result">out return value</param>
        /// <returns>true if success, false if error or empty</returns>
        public bool TryDequeue(out T result)
        {
            lock (_tLock)
            {

                if (_numItems == 0)
                {
                    result = default(T);
                    return false;
                }

                T item = _buf[_getFromIndex];
                _getFromIndex++;
                if (_getFromIndex == _size) _getFromIndex = 0;
                _numItems--;
                result = item;
                return true;
            }
        }
    }
}