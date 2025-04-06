using System;
using System.Threading.Tasks;
using UnityEditor;

namespace com.IvanMurzak.UnityMCP.Editor
{
    public static class MainThread
    {
        public static T Run<T>(Func<T> func) => RunAsync(func).Result;
        public static Task<T> RunAsync<T>(Func<T> func)
        {
            var tcs = new TaskCompletionSource<T>();

            void Execute()
            {
                try
                {
                    T result = func();
                    tcs.SetResult(result);
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
                finally
                {
                    EditorApplication.update -= Execute;
                }
            }

            EditorApplication.update += Execute;
            return tcs.Task;
        }

        public static void Run(Action action) => RunAsync(action).Wait();
        public static Task RunAsync(Action action)
        {
            var tcs = new TaskCompletionSource<bool>();

            void Execute()
            {
                try
                {
                    action();
                    tcs.SetResult(true);
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
                finally
                {
                    EditorApplication.update -= Execute;
                }
            }

            EditorApplication.update += Execute;
            return tcs.Task;
        }
    }
}