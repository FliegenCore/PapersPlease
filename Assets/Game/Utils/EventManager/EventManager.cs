using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Common
{
    public class EventManager
    {
        private Dictionary<Type, Dictionary<object, object>> m_Events;

        public EventManager()
        {
            m_Events = new Dictionary<Type, Dictionary<object, object>>();
        }

        private bool CheckSub<T>(object listener)
        {
            var type = typeof(T);
            if (!m_Events.ContainsKey(type))
            {
                m_Events.Add(type, new Dictionary<object, object>());
            }

            if (m_Events[type].ContainsKey(listener))
            {
                Debug.LogError($"you try to subscribe on {type} twice with object: {listener} type: {listener.GetType()}");
                return false;
            }

            return true;
        }

        public void Subscribe<T>(object listener, Action action) where T : class
        {
            if (CheckSub<T>(listener))
            {
                var type = typeof(T);

                m_Events[type].Add(listener, action);
            }
        }

        public void Subscribe<T, U>(object listener, Action<U> action) where T : class
        {
            if (CheckSub<T>(listener))
            {
                var type = typeof(T);

                m_Events[type].Add(listener, action);
            }
        }

        public void Subscribe<T, U, Q>(object listener, Action<U, Q> action) where T : class
        {
            if (CheckSub<T>(listener))
            {
                var type = typeof(T);

                m_Events[type].Add(listener, action);
            }
        }
        public void Subscribe<T, U, Q, E>(object listener, Action<U, Q, E> action) where T : class
        {
            if (CheckSub<T>(listener))
            {
                var type = typeof(T);

                m_Events[type].Add(listener, action);
            }
        }

        public void Unsubscribe<T>(object listener) where T : class
        {
            var type = typeof(T);
            if (m_Events.ContainsKey(type))
            {
                m_Events[type].Remove(listener);
            }
        }

        public void TriggerEvenet<T>()
        {
            var type = typeof(T);

            if (m_Events.ContainsKey(type))
            {
                if (m_Events.TryGetValue(type, out var events))
                {
                    foreach (var myEvent in events)
                    {
                        Action action = myEvent.Value as Action;
                        if (action == null)
                        {
                            Debug.LogError($"u trigger {type}, u not add action");
                        }

                        action.Invoke();
                    }
                }
            }
        }

        public void TriggerEvenet<T, U>(U arg)
        {
            var type = typeof(T);

            if (m_Events.ContainsKey(type))
            {
                if (m_Events.TryGetValue(type, out var events))
                {
                    foreach (var myEvent in events)
                    {
                        Action<U> action = myEvent.Value as Action<U>;
                        if (action == null)
                        {
                            Debug.LogError($"u trigger {type}, u not add action");
                        }

                        action.Invoke(arg);
                    }
                }
            }
        }

        public void TriggerEvenet<T, U, Q>(U arg, Q arg2)
        {
            var type = typeof(T);

            if (m_Events.ContainsKey(type))
            {
                if (m_Events.TryGetValue(type, out var events))
                {
                    foreach (var myEvent in events)
                    {
                        Action<U, Q> action = myEvent.Value as Action<U, Q>;
                        if (action == null)
                        {
                            Debug.LogError($"u trigger {type}, u not add action");
                        }

                        action.Invoke(arg, arg2);
                    }
                }
            }
        }

        public void TriggerEvenet<T, U, Q, E>(U arg, Q arg2, E arg3)
        {
            var type = typeof(T);

            if (m_Events.ContainsKey(type))
            {
                if (m_Events.TryGetValue(type, out var events))
                {
                    foreach (var myEvent in events)
                    {
                        Action<U, Q, E> action = myEvent.Value as Action<U, Q, E>;
                        if (action == null)
                        {
                            Debug.LogError($"u trigger {type}, u not add action");
                        }

                        action.Invoke(arg, arg2, arg3);
                    }
                }
            }
        }

    }
}
