using System;
using com.IvanMurzak.Unity.MCP.Common.Data;
using ModelContextProtocol.Protocol.Types;

namespace com.IvanMurzak.Unity.MCP.Server
{
    public static class ResponseResourceExtensions
    {
        public static ResourceContents ToResourceContents(this IResponseResource response)
        {
            if (response!.Text != null)
                return new TextResourceContents()
                {
                    Uri = response.Uri,
                    MimeType = response.MimeType,
                    Text = response.Text
                };

            if (response!.Blob != null)
                return new BlobResourceContents()
                {
                    Uri = response.Uri,
                    MimeType = response.MimeType,
                    Blob = response.Blob
                };

            throw new InvalidOperationException("Resource contents is null");
        }
    }
}