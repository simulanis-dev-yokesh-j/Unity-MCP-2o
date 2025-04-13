#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using com.IvanMurzak.Unity.MCP.Common;
using Debug = UnityEngine.Debug;

namespace com.IvanMurzak.Unity.MCP.Editor.Utils
{
    public static class ProcessUtils
    {
        public static async Task<(string output, string error)> Run(ProcessStartInfo processStartInfo)
        {
            Debug.Log($"{Consts.Log.Tag} Command: <color=#8CFFD1>{processStartInfo.FileName} {processStartInfo.Arguments}</color>");

            var output = string.Empty;
            var error = string.Empty;

            await Task.Run(() =>
            {
                try
                {
                    using (var process = new Process { StartInfo = processStartInfo })
                    {
                        process.Start();

                        // Read the output and error streams
                        output = process.StandardOutput.ReadToEnd();
                        error = process.StandardError.ReadToEnd();

                        process.WaitForExit();
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError($"{Consts.Log.Tag} Failed to execute command: {processStartInfo.FileName} {processStartInfo.Arguments}");
                    Debug.LogException(ex);

                    error = ex.Message;
                    // error = "Failed to execute 'dotnet' command. Ensure 'dotnet' CLI is installed and accessible in the environment"
                }
            });
            return (output, error);
        }
    }
}