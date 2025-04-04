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

        public static IConnectorBuilder WithCommands(this IConnectorBuilder builder, params Type[] promptTypes)
            => WithCommands(builder, promptTypes.ToArray());

        public static IConnectorBuilder WithCommands(this IConnectorBuilder builder, IEnumerable<Type> promptTypes)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));
            if (promptTypes == null)
                throw new ArgumentNullException(nameof(promptTypes));

            foreach (var promptType in promptTypes)
            {
                if (promptType is not null)
                {
                    foreach (var promptMethod in promptType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance))
                    {
                        if (promptMethod.GetCustomAttribute<PromptAttribute>() is not null)
                        {
                            // builder.Services.AddSingleton((Func<IServiceProvider, Prompt>)(promptMethod.IsStatic ?
                            //     services => McpServerPrompt.Create(promptMethod, options: new() { Services = services }) :
                            //     services => McpServerPrompt.Create(promptMethod, promptType, new() { Services = services })));
                        }
                    }
                }
            }

            return builder;
        }

        public static IConnectorBuilder WithCommandsFromAssembly(this IConnectorBuilder builder, Assembly? assembly = null)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            assembly ??= Assembly.GetCallingAssembly();

            return builder.WithCommands(
                from t in assembly.GetTypes()
                where t.GetCustomAttribute<PromptTypeAttribute>() is not null
                select t);
        }
    }
}