namespace Utilities
{
    using System;

    namespace RedWolves.Solitaire.Core.Event
    {
        public class GameEvent<T>
        {
            private Action<T> callback;

            public void Subscribe(Action<T> callback)
            {
                this.callback += callback;
            }
        
            public void Unsubscribe(Action<T> callback)
            {
                this.callback -= callback;
            }

            public void Publish(T args)
            {
                callback?.Invoke(args);
            }
        }
    }
}