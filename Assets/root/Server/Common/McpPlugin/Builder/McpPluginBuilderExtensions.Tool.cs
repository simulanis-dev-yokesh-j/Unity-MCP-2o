#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace com.IvanMurzak.Unity.MCP.Common
{
    public static partial class McpPluginBuilderExtensions
    {
        public static IMcpPluginBuilder WithTools(this IMcpPluginBuilder builder, params Type[] targetTypes)
            => WithTools(builder, targetTypes.ToArray());

        public static IMcpPluginBuilder WithTools(this IMcpPluginBuilder builder, IEnumerable<Type> targetTypes)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));
            if (targetTypes == null)
                throw new ArgumentNullException(nameof(targetTypes));

            foreach (var targetType in targetTypes)
                WithTools(builder, targetType);

            return builder;
        }

        public static IMcpPluginBuilder WithTools<T>(this IMcpPluginBuilder builder)
            => WithTools(builder, typeof(T));

        public static IMcpPluginBuilder WithTools(this IMcpPluginBuilder builder, Type targetType)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));
            if (targetType == null)
                throw new ArgumentNullException(nameof(targetType));

            var logger = builder.Services.BuildServiceProvider().GetRequiredService<ILoggerFactory>().CreateLogger<RunTool>();

            foreach (var method in targetType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance))
            {
                var attribute = method.GetCustomAttribute<McpPluginToolAttribute>();
                if (attribute == null)
                    continue;

                if (string.IsNullOrEmpty(attribute.Name))
                    throw new ArgumentException($"Tool name cannot be null or empty. Type: {targetType.Name}, Method: {method.Name}");

                var className = targetType.GetCustomAttribute<McpPluginToolTypeAttribute>()?.Path ?? targetType.FullName;
                if (className == null)
                    throw new InvalidOperationException($"Type {targetType.Name} does not have a full name.");

                var runner = method.IsStatic
                    ? RunTool.CreateFromStaticMethod(logger, method, attribute.Title)
                    : RunTool.CreateFromClassMethod(logger, targetType, method, attribute.Title);

                builder.AddTool(attribute.Name, runner);
            }
            return builder;
        }

        public static IMcpPluginBuilder WithToolsFromAssembly(this IMcpPluginBuilder builder, IEnumerable<Assembly> assemblies)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));
            if (assemblies == null)
                throw new ArgumentNullException(nameof(assemblies));

            foreach (var assembly in assemblies)
                WithToolsFromAssembly(builder, assembly);

            return builder;
        }
        public static IMcpPluginBuilder WithToolsFromAssembly(this IMcpPluginBuilder builder, Assembly? assembly = null)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            assembly ??= Assembly.GetCallingAssembly();

            return builder.WithTools(
                from t in assembly.GetTypes()
                where t.GetCustomAttribute<McpPluginToolTypeAttribute>() is not null
                select t);
        }
    }
}