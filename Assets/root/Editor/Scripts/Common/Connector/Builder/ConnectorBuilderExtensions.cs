#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace com.IvanMurzak.UnityMCP.Common.API
{
    public static class ConnectorBuilderExtensions
    {
        public static IConnectorBuilder AddConnector(this IServiceCollection services)
            => new ConnectorBuilder(services);

        // Tools --------------------------------------------------------------------------------------------

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

            foreach (var method in targetType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance))
            {
                if (method.GetCustomAttribute<ToolAttribute>() is not null)
                {
                    if (method.IsStatic)
                        builder.AddCommand(targetType.FullName, method.Name, Command.CreateFromStaticMethod(method));
                    else
                        builder.AddCommand(targetType.FullName, method.Name, Command.CreateFromClassMethod(targetType, method));
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

        // Prompt --------------------------------------------------------------------------------------------

        public static IConnectorBuilder WithPrompts(this IConnectorBuilder builder, params Type[] targetTypes)
            => WithPrompts(builder, targetTypes.ToArray());

        public static IConnectorBuilder WithPrompts(this IConnectorBuilder builder, IEnumerable<Type> targetTypes)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));
            if (targetTypes == null)
                throw new ArgumentNullException(nameof(targetTypes));

            foreach (var targetType in targetTypes)
            {
                if (targetType is not null)
                {
                    foreach (var method in targetType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance))
                    {
                        if (method.GetCustomAttribute<PromptAttribute>() is not null)
                        {
                            // builder.Services.AddSingleton((Func<IServiceProvider, Prompt>)(method.IsStatic ?
                            //     services => McpServerPrompt.Create(method, options: new() { Services = services }) :
                            //     services => McpServerPrompt.Create(method, promptType, new() { Services = services })));
                        }
                    }
                }
            }

            return builder;
        }

        public static IConnectorBuilder WithPromptsFromAssembly(this IConnectorBuilder builder, Assembly? assembly = null)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            assembly ??= Assembly.GetCallingAssembly();

            return builder.WithPrompts(
                from t in assembly.GetTypes()
                where t.GetCustomAttribute<PromptTypeAttribute>() is not null
                select t);
        }
    }
}