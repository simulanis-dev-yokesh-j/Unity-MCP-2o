#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System;
using System.ComponentModel;
using System.Linq;
using com.IvanMurzak.Unity.MCP.Common;
using com.IvanMurzak.Unity.MCP.Editor.Utils;

namespace com.IvanMurzak.Unity.MCP.Editor.API
{
    public partial class Tool_Component
    {
        [Tool
        (
            "Component_Add",
            Title = "Add Component to a GameObject",
            Description = "Add a component to a GameObject."
        )]
        public string Add
        (
            [Description("Full name of the Component. It should include full namespace path and the class name.")]
            string componentName,
            [Description("Path to the GameObject (including the name of the GameObject).")]
            string gameObjectPath
        )
        => MainThread.Run(() =>
        {
            var go = GameObjectUtils.FindByPath(gameObjectPath);
            if (go == null)
                return $"[Error] GameObject '{gameObjectPath}' not found.";

            var type = Type.GetType(componentName) ?? AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .FirstOrDefault(t => t.FullName == componentName);
            if (type == null)
                return $"[Error] Component type '{componentName}' not found.";

            go.AddComponent(type);

            return $"[Success] Added component '{componentName}' to GameObject '{gameObjectPath}'.";
        });
    }
}