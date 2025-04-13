#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System;
using System.ComponentModel;
using com.IvanMurzak.Unity.MCP.Common;
using UnityEditor;

namespace com.IvanMurzak.Unity.MCP.Editor.API
{
    public partial class Tool_Component
    {
        [Tool("Component_Add",
            Title = "Add Component to a GameObject",
            Description = "Add a component to a GameObject.")]
        public string Add(
            [Description("Full name of the Component. It should include full namespace path and the class name.")]
            string fullName)
        {
            throw new NotImplementedException("Add Component to a GameObject is not implemented yet.");
        }
    }
}