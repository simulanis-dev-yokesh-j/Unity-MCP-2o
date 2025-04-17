using com.IvanMurzak.Unity.MCP.Common;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using System;
using System.Threading.Tasks;
using Debug = UnityEngine.Debug;
using com.IvanMurzak.Unity.MCP.Editor.Utils;

namespace com.IvanMurzak.Unity.MCP.Editor
{
    static partial class Startup
    {
        public const string ServerProjectName = "com.IvanMurzak.Unity.MCP.Server";
        public static string ServerSourcePath => Path.GetFullPath(Path.Combine(Application.dataPath, "../Library", "PackageCache", PackageName, "Server"));
        public static string ServerSourceAlternativePath => Path.GetFullPath(Path.Combine(Application.dataPath, "root", "Server"));
        public static string ServerRootPath => Path.GetFullPath(Path.Combine(Application.dataPath, "../Library", ServerProjectName.ToLower()));
        public static string ServerExecutableFolder => Path.Combine(ServerRootPath, "bin~", "Release", "net9.0");
        public static string ServerExecutableFile => Path.Combine(ServerExecutableFolder, $"{ServerProjectName}");
        public static string ServerLogsPath => Path.Combine(ServerExecutableFolder, "logs", "server-log.txt");
        public static bool IsServerCompiled => FileUtils.FileExistsWithoutExtension(ServerExecutableFolder, ServerProjectName);

        public static string RawJsonConfiguration(int port) => Consts.MCP_Client.ClaudeDesktop.Config(
            ServerExecutableFile.Replace('\\', '/'),
            port
        );

        public static Task BuildServerIfNeeded(bool force = true)
        {
            if (IsServerCompiled)
                return Task.CompletedTask;
            return BuildServer(force);
        }

        public static async Task BuildServer(bool force = true)
        {
            var message = "<b><color=yellow>Server Build</color></b>";
            Debug.Log($"{Consts.Log.Tag} {message} <color=orange>⊂(◉‿◉)つ</color>");

            CopyServerSources();

            Debug.Log($"{Consts.Log.Tag} Building server at <color=#8CFFD1>{ServerRootPath}</color>");

            (string output, string error) = await ProcessUtils.Run(new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = "build -c Release",
                WorkingDirectory = ServerRootPath,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            });

            await MainThread.RunAsync(() => HandleBuildResult(output, error, force));
        }

        private static async Task HandleBuildResult(string output, string error, bool force)
        {
            if (output.Contains("Build FAILED") || output.Contains("MSBUILD : error"))
            {
                Debug.LogError($"{Consts.Log.Tag} <color=red>Build failed</color>. Check the output for details:\n{output}");
                if (force)
                {
                    if (ErrorUtils.ExtractProcessId(output, out var processId))
                    {
                        Debug.Log($"{Consts.Log.Tag} Detected another process which locks the file. Killing the process with ID: {processId}");
                        // Kill the process that locks the file
                        (string _output, string _error) = await ProcessUtils.Run(new ProcessStartInfo
                        {
                            FileName = "taskkill",
                            Arguments = $"/PID {processId} /F",
                            WorkingDirectory = ServerRootPath,
                            RedirectStandardOutput = true,
                            RedirectStandardError = true,
                            UseShellExecute = false,
                            CreateNoWindow = true
                        });
                        Debug.Log($"{Consts.Log.Tag} Trying to rebuild server one more time");
                        await BuildServer(force: false);
                        return;
                    }
                }
            }

            if (string.IsNullOrEmpty(error))
                Debug.Log($"{Consts.Log.Tag} Build succeeded:\n{output}");
            else
                Debug.LogError($"{Consts.Log.Tag} Build Errors:\n{error}");
        }

        public static void CopyServerSources()
        {
            Debug.Log($"{Consts.Log.Tag} Delete sources at: <color=#8CFFD1>{ServerRootPath}</color>");
            try
            {
                DirectoryUtils.Delete(ServerRootPath, recursive: true);
            }
            catch (UnauthorizedAccessException)
            {
                // ignore
            }

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