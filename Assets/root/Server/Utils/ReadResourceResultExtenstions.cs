using System;
using System.Collections.Generic;
using ModelContextProtocol.Protocol.Types;

namespace com.IvanMurzak.Unity.MCP.Server
{
    public static class ReadResourceResultExtenstions
    {
        public static ReadResourceResult SetError(this ReadResourceResult target, string uri, string message)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            var error = new TextResourceContents()
            {
                Uri = uri,
                MimeType = "plain/text",
                Text = message
            };

            target.Contents ??= new List<ResourceContents>(1);
            target.Contents.Clear();
            target.Contents.Add(error);

            return target;
        }
    }
}