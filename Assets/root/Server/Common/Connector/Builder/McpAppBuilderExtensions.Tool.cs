#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace com.IvanMurzak.Unity.MCP.Common
{
    public static partial class McpAppBuilderExtensions
    {
        public static IMcpAppBuilder WithTools(this IMcpAppBuilder builder, params Type[] targetTypes)
            => WithTools(builder, targetTypes.ToArray());

        public static IMcpAppBuilder WithTools(this IMcpAppBuilder builder, IEnumerable<Type> targetTypes)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));
            if (targetTypes == null)
                throw new ArgumentNullException(nameof(targetTypes));

            foreach (var targetType in targetTypes)
                WithTools(builder, targetType);

            return builder;
        }

        public static IMcpAppBuilder WithTools<T>(this IMcpAppBuilder builder)
            => WithTools(builder, typeof(T));

        public static IMcpAppBuilder WithTools(this IMcpAppBuilder builder, Type targetType)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));
            if (targetType == null)
                throw new ArgumentNullException(nameof(targetType));

            var logger = builder.Services.BuildServiceProvider().GetRequiredService<ILoggerFactory>().CreateLogger<RunTool>();

            foreach (var method in targetType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance))
            {
                var attribute = method.GetCustomAttribute<ToolAttribute>();
                if (attribute == null)
                    continue;

                if (string.IsNullOrEmpty(attribute.Name))
                    throw new ArgumentException($"Tool name cannot be null or empty. Type: {targetType.Name}, Method: {method.Name}");

                var className = targetType.GetCustomAttribute<ToolTypeAttribute>()?.Path ?? targetType.FullName;
                if (className == null)
                    throw new InvalidOperationException($"Type {targetType.Name} does not have a full name.");

                var runner = method.IsStatic
                    ? RunTool.CreateFromStaticMethod(logger, method)
                    : RunTool.CreateFromClassMethod(logger, targetType, method);

                builder.AddTool(attribute.Name, runner);
            }
            return builder;
        }

        public static IMcpAppBuilder WithToolsFromAssembly(this IMcpAppBuilder builder, Assembly? assembly = null)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            assembly ??= Assembly.GetCallingAssembly();

            return builder.WithTools(
                from t in assembly.GetTypes()
                where t.GetCustomAttribute<ToolTypeAttribute>() is not null
                select t);
        }
    }
}