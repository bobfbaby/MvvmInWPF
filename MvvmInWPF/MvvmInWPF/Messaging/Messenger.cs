using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace MvvmInWPF.Messaging
{
    public class Messenger
    {
        private static readonly object CreationLock = new object();

        private static readonly ConcurrentDictionary<MessengerKey, object> Dictionary =
            new ConcurrentDictionary<MessengerKey, object>();

        private static Messenger instance;

        public static Messenger Default
        {
            get
            {
                if (instance == null)
                {
                    lock (CreationLock)
                    {
                        if (instance == null)
                        {
                            instance = new Messenger();
                        }
                    }
                }

                return instance;
            }
        }

        public void Subscribe(object subscriber, MessengerEvent mgsEvent, Action handler)
        {
            var key = new MessengerKey(subscriber, mgsEvent, Dictionary.Count);
            Dictionary.TryAdd(key, handler);
        }

        public void Subscribe<T>(object subscriber, MessengerEvent mgsEvent, Action<T> handler)
        {
            var key = new MessengerKey(subscriber, mgsEvent, Dictionary.Count);
            Dictionary.TryAdd(key, handler);
        }


        public void Unsubscribe(object subscriber, MessengerEvent msgEvent)
        {
            object action;
            MessengerKey key = GetMessengerKey(subscriber, msgEvent);


            if (subscriber != null)
            {
                if (key != null)
                {
                    Dictionary.TryRemove(key, out action);
                }
            }
        }


        public void Notify<T>(T message, MessengerEvent context)
        {
            Console.WriteLine("Notify requested for " + context);

            IEnumerable<KeyValuePair<MessengerKey, object>> result;

            if (context == null)
            {
                // Get all recipients where the context is null.
                result = from r in Dictionary where r.Key.Context == null orderby r.Key.ID select r;
            }
            else
            {
                // Get all recipients where the context is matching.
                result = from r in Dictionary
                    where r.Key.Context != null && r.Key.Context.Equals(context)
                    orderby r.Key.ID
                    select r;
            }

            foreach (var action in result.Select(x => x.Value))
            {
                // Send the message to all recipients.
                if (action.GetType() == typeof(Action<T>))
                {
                    ((Action<T>) action)(message);
                    Console.WriteLine("Sending broadcast " + ((Action<T>) action).Method.Name);
                }
                else if (action.GetType() == typeof(Action))
                {
                    ((Action) action)();
                    Console.WriteLine("Sending broadcast " + ((Action) action).Method.Name);
                }
            }
        }


        private MessengerKey GetMessengerKey(object subscriber, MessengerEvent msgEvent)
        {
            MessengerKey item = null;
            try
            {
                foreach (var key in Dictionary.Keys)
                {
                    if (subscriber == key.Subscriber && msgEvent == (MessengerEvent) key.Context)
                    {
                        item = key;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return item;
        }


        protected class MessengerKey
        {
            public object Subscriber { get; private set; }

            public MessengerEvent Context { get; private set; }

            public int ID { get; private set; }

            public MessengerKey(object subscriber, MessengerEvent context, int id)
            {
                this.Subscriber = subscriber;
                this.Context = context;
                this.ID = id;
            }

            protected bool Equals(MessengerKey key)
            {
                return Equals(Subscriber, key.Subscriber) && Equals(Context, key.Context);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != GetType()) return false;

                return Equals((MessengerKey) obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return ((Subscriber != null ? Subscriber.GetHashCode() : 0) * 397) ^
                           (Context != null ? Context.GetHashCode() : 0);
                }
            }
        }
    }
}