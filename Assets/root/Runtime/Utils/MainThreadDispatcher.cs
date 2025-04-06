using System;
using System.Collections.Concurrent;
using UnityEngine;

namespace com.IvanMurzak.Unity.MCP
{
    public class MainThreadDispatcher : MonoBehaviour
    {
        private static readonly ConcurrentQueue<Action> _actions = new ConcurrentQueue<Action>();
        private static MainThreadDispatcher _instance;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            if (_instance == null)
            {
                var obj = new GameObject("MainThreadDispatcher");
                _instance = obj.AddComponent<MainThreadDispatcher>();
                DontDestroyOnLoad(obj);
            }
        }

        public static void Enqueue(Action action)
        {
            _actions.Enqueue(action);
        }

        private void Update()
        {
            while (_actions.TryDequeue(out var action))
            {
                action.Invoke();
            }
        }
    }
}