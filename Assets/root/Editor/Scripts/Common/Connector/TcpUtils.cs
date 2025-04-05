
using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace com.IvanMurzak.UnityMCP.Common.API
{
    public static class TcpUtils
    {
        public static async Task SendAsync(NetworkStream stream, string message, CancellationToken cancellationToken)
        {
            var messageBytes = Encoding.UTF8.GetBytes(message);
            var lengthPrefix = BitConverter.GetBytes(messageBytes.Length);

            // Send length and message
            await stream.WriteAsync(lengthPrefix, 0, lengthPrefix.Length, cancellationToken);
            if (cancellationToken.IsCancellationRequested)
            {
                throw new OperationCanceledException(cancellationToken);
            }
            await stream.WriteAsync(messageBytes, 0, messageBytes.Length, cancellationToken);
        }

        public static async Task<string> ReadResponseAsync(NetworkStream stream, CancellationToken cancellationToken)
        {
            var lengthBuffer = new byte[4];
            await ReadExactlyAsync(stream, lengthBuffer, 4, cancellationToken);
            var length = BitConverter.ToInt32(lengthBuffer, 0);

            var buffer = new byte[length];
            await ReadExactlyAsync(stream, buffer, length, cancellationToken);

            return Encoding.UTF8.GetString(buffer);
        }

        private static async Task ReadExactlyAsync(Stream stream, byte[] buffer, int totalBytes, CancellationToken cancellationToken)
        {
            var offset = 0;
            while (offset < totalBytes)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    throw new OperationCanceledException(cancellationToken);
                }
                var bytesRead = await stream.ReadAsync(buffer, offset, totalBytes - offset);
                if (bytesRead == 0)
                {
                    throw new IOException("Unexpected end of stream");
                }
                offset += bytesRead;
            }
        }
    }
}