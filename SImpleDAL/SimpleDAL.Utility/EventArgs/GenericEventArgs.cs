using System;

namespace SimpleDAL.Utility
{
    public class EventArgs<T> : EventArgs
    {
        public T EventData { get; private set; }

        public EventArgs(T EventData)
        {
            this.EventData = EventData;
        }
    }
}
