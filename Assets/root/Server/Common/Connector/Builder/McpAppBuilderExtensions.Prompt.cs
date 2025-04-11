#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace com.IvanMurzak.Unity.MCP.Common
{
    public static partial class McpAppBuilderExtensions
    {
        public static IMcpAppBuilder WithPrompts(this IMcpAppBuilder builder, params Type[] targetTypes)
            => WithPrompts(builder, targetTypes.ToArray());

        public static IMcpAppBuilder WithPrompts(this IMcpAppBuilder builder, IEnumerable<Type> targetTypes)
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

        public static IMcpAppBuilder WithPromptsFromAssembly(this IMcpAppBuilder builder, Assembly? assembly = null)
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