#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace com.IvanMurzak.Unity.MCP.Common
{
    public static partial class ConnectorBuilderExtensions
    {
        public static IConnectorBuilder WithTools(this IConnectorBuilder builder, params Type[] targetTypes)
            => WithTools(builder, targetTypes.ToArray());

        public static IConnectorBuilder WithTools(this IConnectorBuilder builder, IEnumerable<Type> targetTypes)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));
            if (targetTypes == null)
                throw new ArgumentNullException(nameof(targetTypes));

            foreach (var targetType in targetTypes)
                WithTools(builder, targetType);

            return builder;
        }

        public static IConnectorBuilder WithTools<T>(this IConnectorBuilder builder)
            => WithTools(builder, typeof(T));

        public static IConnectorBuilder WithTools(this IConnectorBuilder builder, Type targetType)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));
            if (targetType == null)
                throw new ArgumentNullException(nameof(targetType));

            var logger = builder.Services.BuildServiceProvider().GetRequiredService<ILoggerFactory>().CreateLogger<Command>();

            foreach (var method in targetType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance))
            {
                if (method.GetCustomAttribute<ToolAttribute>() is not null)
                {
                    var className = targetType.GetCustomAttribute<ToolTypeAttribute>()?.Path ?? targetType.FullName;
                    if (className == null)
                        throw new InvalidOperationException($"Type {targetType.Name} does not have a full name.");

                    if (method.IsStatic)
                        builder.AddCommand(className, method.Name, Command.CreateFromStaticMethod(logger, method));
                    else
                        builder.AddCommand(className, method.Name, Command.CreateFromClassMethod(logger, targetType, method));
                }
            }
            return builder;
        }

        public static IConnectorBuilder WithToolsFromAssembly(this IConnectorBuilder builder, Assembly? assembly = null)
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