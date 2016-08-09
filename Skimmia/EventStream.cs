using System;
using System.Collections.Generic;

namespace Skimmia
{
    public class EventStream<T>
    {
        private readonly List<Action<T>> _callbacks = new List<Action<T>>();

        public void Subscribe(Action<T> callback)
        {
            _callbacks.Add(callback);
        }

        internal void Next(T @event)
        {
            _callbacks.ForEach(cb => cb(@event));
        }
    }
}