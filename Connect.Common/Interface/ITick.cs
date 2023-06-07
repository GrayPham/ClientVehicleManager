using System;

namespace Connect.Common.Interface
{
    /// <summary>
    /// Support 100ms Tick
    /// </summary>
    public interface ITick
    {
        /// <summary>
        /// Caller call each 100ms
        /// </summary>
        event EventHandler Tick;
    }
}
