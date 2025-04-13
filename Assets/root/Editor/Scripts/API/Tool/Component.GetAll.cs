#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System;
using System.ComponentModel;
using System.Linq;
using com.IvanMurzak.Unity.MCP.Common;
using UnityEditor;

namespace com.IvanMurzak.Unity.MCP.Editor.API
{
    public partial class Tool_Component
    {
        [Tool("Component_Get_All",
            Title = "Get list of all Components",
            Description = "Returns the list of all available components in the project.")]
        public string GetAll(
            [Description("Substring for searching components. Could be empty.")]
            string? search = null)
        {
            var componentTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => typeof(UnityEngine.Component).IsAssignableFrom(type) && !type.IsAbstract)
                .Select(type => type.FullName)
                .ToList();

            if (!string.IsNullOrEmpty(search))
            {
                componentTypes = componentTypes
                    .Where(typeName => typeName != null && typeName.Contains(search, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            return string.Join("\n", componentTypes);
        }
    }
}