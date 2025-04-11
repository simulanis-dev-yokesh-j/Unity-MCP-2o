using System;
using System.Collections.Generic;
using System.Linq;
using com.IvanMurzak.Unity.MCP.Common;
using com.IvanMurzak.Unity.MCP.Common.Data;
using ModelContextProtocol.Protocol.Types;

namespace com.IvanMurzak.Unity.MCP.Server
{

    public static class ToolsExtensions
    {
        public static CallToolResponse SetError(this CallToolResponse target, string message)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            target.IsError = true;
            target.Content ??= new List<Content>(1);
            target.Content.Add(new Content()
            {
                Type = "text",
                MimeType = Consts.MimeType.TextPlain,
                Text = message
            });

            return target;
        }

        public static ListToolsResult SetError(this ListToolsResult target, string message)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            target.Tools = new List<Tool>();

            return target;
        }

        public static Tool ToTool(this IResponseListTool response)
        {
            return new Tool()
            {
                Name = response.Name,
                Description = response.Description,
                InputSchema = response.InputSchema
            };
        }

        public static CallToolResponse ToCallToolRespose(this IResponseCallTool response)
        {
            return new CallToolResponse()
            {
                IsError = response.IsError,
                Content = response.Content
                    .Select(x => x.ToContent())
                    .ToList()
            };
        }

        public static Content ToContent(this ResponseCallToolContent response)
        {
            return new Content()
            {
                Type = response.Type,
                MimeType = response.MimeType,
                Text = response.Text,
                Data = response.Data
            };
        }
    }
}