
using Connect.Common.Contract;
using Connect.Common.Interface;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;

namespace Connect.Common
{
    public enum L2LMessageType
    {
        Information,
        Warning,
        Error,
        Critical,
    }

    public class L2LMessageEventArgs : EventArgs
    {
        /// <summary>
        /// This type of message
        /// </summary>
        public L2LMessageType Type { get; private set; }
        /// <summary>
        /// String that contain a message
        /// </summary>
        public String Message { get; private set; }
        /// <summary>
        /// This code defined by each library. 0 is not used
        /// </summary>
        public int Code { get; private set; }

        public L2LMessageEventArgs(L2LMessageType type, String message)
        {
            Type = type;
            Message = message;
            Code = 0;
        }

        public L2LMessageEventArgs(L2LMessageType type, String message, int code)
        {
            Type = type;
            Message = message;
            Code = code;
        }
    }
    public delegate void L2LMessageEventHandler(object sender, L2LMessageEventArgs e);

    public class MessageEventArgs : EventArgs
    {
        public String Message { get; private set; }

        public MessageEventArgs(String message)
        {
            Message = message;
        }
    }
    public delegate void MessageEventHandler(object sender, MessageEventArgs e);

    public class DataEventArgs : EventArgs
    {
        public Byte[] Data { get; private set; }

        public DataEventArgs(Byte[] data)
        {
            Data = data;
        }
    }
    public delegate void DataEventHandler(object sender, DataEventArgs e);

    public class BooleanEventArgs : EventArgs
    {
        public bool Value { get; private set; }

        public BooleanEventArgs(bool val)
        {
            Value = val;
        }
    }
    public delegate void BooleanEventHandler(object sender, BooleanEventArgs e);

    public class StringCollectionEventArgs : EventArgs
    {
        public ICollection<String> Collection { get; private set; }

        public StringCollectionEventArgs(ICollection<String> collection)
        {
            Collection = collection;
        }
    }
    public delegate void StringCollectionEventHandler(object sender, StringCollectionEventArgs e);

    public class ObjectEventArgs : EventArgs
    {
        public object Object { get; private set; }

        public ObjectEventArgs(Object obj)
        {
            Object = obj;
        }

        public new static readonly ObjectEventArgs Empty = new ObjectEventArgs(null);
    }
    public delegate void ObjectEventHandler(object sender, ObjectEventArgs e);

    public class ResponsedEventArgs<T> : EventArgs
    {
        public bool IsSucceed { get; private set; }
        public T Entity { get; private set; }

        public ResponsedEventArgs(bool succeed, T entity)
        {
            IsSucceed = succeed;
            Entity = entity;
        }
    }
    public delegate void ResponsedEventHandler<T>(object sender, ResponsedEventArgs<T> e);

    public class EventArgs<T> : EventArgs
    {
        public T Data { get; private set; }

        public EventArgs(T data)
        {
            Data = data;
        }
    }
    public class ServiceEventArgs<T> : EventArgs
    {
        public T Data { get; private set; }
        ServiceErrorInfo InfoError { get; set; }
        public ServiceEventArgs(ServiceErrorInfo infoError, T data)
        {
            Data = data;
            InfoError = infoError;
        }
    }
    public delegate void GeneralEventHandler<T>(object sender, EventArgs<T> e);
    public delegate void SerialDataReceivedEventHandler<T>(object sender, EventArgs<T> e);
    public delegate void ServiceEventHandler<T>(object sender, ServiceEventArgs<T> e);
    public class IndexEventArgs<T> : EventArgs
    {
        public int Index { get; private set; }
        public T Data { get; private set; }

        public IndexEventArgs(int index, T data)
        {
            Data = data;
            Index = index;
        }
    }
    public delegate void IndexEventHandler<T>(object sender, IndexEventArgs<T> e);

    public class EntityEventArgs<T> : EventArgs where T : IEntity
    {
        public T Entity { get; private set; }

        public EntityEventArgs(T entity)
        {
            Entity = entity;
        }
    }
    public delegate void EntityEventHandler<T>(object sender, EntityEventArgs<T> e) where T : IEntity;

    public struct MessageCode
    {
        public int Code;
        public String Message;

        public MessageCode(int code, String msg)
        {
            Code = code;
            Message = msg;
        }
    }

}