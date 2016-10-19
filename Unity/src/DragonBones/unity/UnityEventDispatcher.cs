﻿using System.Collections.Generic;
using UnityEngine;

namespace DragonBones
{
    public class UnityEventDispatcher<T> : MonoBehaviour, IEventDispatcher<T>
    {
        private readonly Dictionary<string, ListenerDelegate<T>> _listeners = new Dictionary<string, ListenerDelegate<T>>();

        /**
         * @private
         */
        public UnityEventDispatcher()
        {
        }

        /**
         * @private
         */
        virtual public void _onClear()
        {
            _listeners.Clear();
        }
        
        public void DispatchEvent(string type, T eventObject)
        {
            if (!_listeners.ContainsKey(type))
            {
                return;
            }
            else
            {
                _listeners[type](type, eventObject);
            }
        }

        public bool HasEventListener(string type)
        {
            return _listeners.ContainsKey(type);
        }

        public void AddEventListener(string type, ListenerDelegate<T> listener)
        {
            if (_listeners.ContainsKey(type))
            {
                var delegates = _listeners[type].GetInvocationList();
                for (int i = 0, l = delegates.Length; i < l; ++i)
                {
                    if (listener == (object)delegates[i])
                    {
                        return;
                    }
                }

                _listeners[type] += listener;
            }
            else
            {
                _listeners.Add(type, listener);
            }
        }

        public void RemoveEventListener(string type, ListenerDelegate<T> listener)
        {
            if (!_listeners.ContainsKey(type))
            {
                return;
            }

            var delegates = _listeners[type].GetInvocationList();
            for (int i = 0, l = delegates.Length; i < l; ++i)
            {
                if (listener == (object)delegates[i])
                {
                    _listeners[type] -= listener;
                    break;
                }
            }

            if (_listeners[type] == null)
            {
                _listeners.Remove(type);
            }
        }
    }
}
