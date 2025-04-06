using com.IvanMurzak.Unity.MCP.Common;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using System;
using System.Threading.Tasks;
using Debug = UnityEngine.Debug;

namespace com.IvanMurzak.Unity.MCP.Editor
{
    static partial class Startup
    {
        const string ServerProjectName = "com.IvanMurzak.Unity.MCP.Server";
        public static string ServerSourcePath => Path.GetFullPath(Path.Combine(Application.dataPath, "../Library", "PackageCache", PackageName, "Server"));
        public static string ServerSourceAlternativePath => Path.GetFullPath(Path.Combine(Application.dataPath, "root", "Server"));
        public static string ServerRootPath => Path.GetFullPath(Path.Combine(Application.dataPath, "../Library", ServerProjectName.ToLower()));
        public static string ServerExecutablePath => Path.Combine(ServerRootPath, $"bin~/Release/net9.0/{ServerProjectName}.exe");
        public static bool IsServerCompiled => File.Exists(ServerExecutablePath);

        public static void CompileServerIfNeeded()
        {
            if (IsServerCompiled)
                return;
            CompileServer();
        }

        public static void CompileServer()
        {
            var message = "<b><color=yellow>Server Build</color></b>";
            Debug.Log($"{Consts.Log.Tag} {message} <color=orange>⊂(◉‿◉)つ</color>");

            CopyServerSources();

            var processStartInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = "build -c Release",
                WorkingDirectory = ServerRootPath,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            Debug.Log($"{Consts.Log.Tag} Building server at <color=#8CFFD1>{ServerRootPath}</color>");
            Debug.Log($"{Consts.Log.Tag} Command: <color=#8CFFD1>{processStartInfo.FileName} {processStartInfo.Arguments}</color>");

            Task.Run(() =>
            {
                try
                {
                    using (var process = new Process { StartInfo = processStartInfo })
                    {
                        process.Start();

                        // Read the output and error streams
                        var output = process.StandardOutput.ReadToEnd();
                        var error = process.StandardError.ReadToEnd();

                        process.WaitForExit();

                        MainThread.RunAsync(() =>
                        {
                            // Log the results
                            Debug.Log($"{Consts.Log.Tag} Build Output:\n{output}");
                            if (!string.IsNullOrEmpty(error))
                            {
                                Debug.LogError($"{Consts.Log.Tag} Build Errors:\n{error}");
                            }
                            MenuItems.PrintConfig();
                        });
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError($"{Consts.Log.Tag} Failed to execute dotnet command. Ensure dotnet CLI is installed and accessible in the environment.\n{ex}");
                }
            });
        }
        public static void CopyServerSources()
        {
            Debug.Log($"{Consts.Log.Tag} Delete sources at: <color=#8CFFD1>{ServerRootPath}</color>");
            DirectoryUtils.Delete(ServerRootPath, recursive: true);

            Debug.Log($"{Consts.Log.Tag} Copy sources from: <color=#8CFFD1>{ServerSourcePath}</color>");
            try
            {
                DirectoryUtils.Copy(ServerSourcePath, ServerRootPath, "*/bin~", "*/obj~", "*\\bin~", "*\\obj~", "*.meta");
            }
            catch (DirectoryNotFoundException)
            {
                Debug.Log($"{Consts.Log.Tag} Copy sources from: <color=#8CFFD1>{ServerSourceAlternativePath}</color>");
                try
                {
                    DirectoryUtils.Copy(ServerSourceAlternativePath, ServerRootPath, "*/bin~", "*/obj~", "*\\bin~", "*\\obj~", "*.meta");
                }
                catch (DirectoryNotFoundException ex)
                {
                    Debug.LogError($"{Consts.Log.Tag} Server source directory not found. Please check the path: <color=#8CFFD1>{ServerSourcePath}</color> or <color=#8CFFD1>{ServerSourceAlternativePath}</color>");
                    Debug.LogError($"{Consts.Log.Tag} It may happen if the package was added into a project using local path reference. Please consider to use a package from the registry instead. Follow official installation instructions at https://github.com/IvanMurzak/Unity-MCP");
                    Debug.LogException(ex);
                    return;
                }
            }
        }
    }
}