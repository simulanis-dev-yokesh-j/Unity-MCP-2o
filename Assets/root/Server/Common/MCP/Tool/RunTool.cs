#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using com.IvanMurzak.Unity.MCP.Common.Data;
using com.IvanMurzak.Unity.MCP.Common.MCP;
using Microsoft.Extensions.Logging;

namespace com.IvanMurzak.Unity.MCP.Common
{
    /// <summary>
    /// Provides functionality to execute methods dynamically, supporting both static and instance methods.
    /// Allows for parameter passing by position or by name, with support for default parameter values.
    /// </summary>
    public partial class RunTool : MethodWrapper, IRunTool
    {
        /// <summary>
        /// Initializes the Command with the target method information.
        /// </summary>
        /// <param name="type">The type containing the static method.</param>
        public static RunTool CreateFromStaticMethod(ILogger logger, MethodInfo methodInfo)
            => new RunTool(logger, methodInfo);

        /// <summary>
        /// Initializes the Command with the target instance method information.
        /// </summary>
        /// <param name="targetInstance">The instance of the object containing the method.</param>
        /// <param name="methodInfo">The MethodInfo of the instance method to execute.</param>
        public static RunTool CreateFromInstanceMethod(ILogger logger, object targetInstance, MethodInfo methodInfo)
            => new RunTool(logger, targetInstance, methodInfo);

        /// <summary>
        /// Initializes the Command with the target instance method information.
        /// </summary>
        /// <param name="targetInstance">The instance of the object containing the method.</param>
        /// <param name="methodInfo">The MethodInfo of the instance method to execute.</param>
        public static RunTool CreateFromClassMethod(ILogger logger, Type targetType, MethodInfo methodInfo)
            => new RunTool(logger, targetType, methodInfo);

        public RunTool(ILogger logger, MethodInfo methodInfo) : base(logger, methodInfo) { }
        public RunTool(ILogger logger, object targetInstance, MethodInfo methodInfo) : base(logger, targetInstance, methodInfo) { }
        public RunTool(ILogger logger, Type targetType, MethodInfo methodInfo) : base(logger, targetType, methodInfo) { }

        /// <summary>
        /// Executes the target static method with the provided arguments.
        /// </summary>
        /// <param name="parameters">The arguments to pass to the method.</param>
        /// <returns>The result of the method execution, or null if the method is void.</returns>
        public IResponseData Run(params object?[] parameters)
        {
            // Invoke the method (static or instance)
            var result = Invoke(parameters);
            return result as IResponseData ?? ResponseData.Success(result?.ToString());
        }

        /// <summary>
        /// Executes the target method with named parameters.
        /// Missing parameters will be filled with their default values or the type's default value if no default is defined.
        /// </summary>
        /// <param name="namedParameters">A dictionary mapping parameter names to their values.</param>
        /// <returns>The result of the method execution, or null if the method is void.</returns>
        public IResponseData Run(IDictionary<string, JsonElement>? namedParameters)
        {
            var result = Invoke(namedParameters);
            return result as IResponseData ?? ResponseData.Success(result?.ToString());
        }
    }
}