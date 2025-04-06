#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using com.IvanMurzak.Unity.MCP.Common.Data;
using Microsoft.Extensions.Logging;

namespace com.IvanMurzak.Unity.MCP.Common
{
    /// <summary>
    /// Provides functionality to execute methods dynamically, supporting both static and instance methods.
    /// Allows for parameter passing by position or by name, with support for default parameter values.
    /// </summary>
    public partial class Command : ICommand
    {
        readonly MethodInfo _methodInfo;
        readonly object? _targetInstance;
        readonly Type? _targetType;
        readonly ILogger _logger;

        /// <summary>
        /// Initializes the Command with the target method information.
        /// </summary>
        /// <param name="type">The type containing the static method.</param>
        public static Command CreateFromStaticMethod(ILogger logger, MethodInfo methodInfo)
            => new Command(logger, methodInfo);

        /// <summary>
        /// Initializes the Command with the target instance method information.
        /// </summary>
        /// <param name="targetInstance">The instance of the object containing the method.</param>
        /// <param name="methodInfo">The MethodInfo of the instance method to execute.</param>
        public static Command CreateFromInstanceMethod(ILogger logger, object targetInstance, MethodInfo methodInfo)
            => new Command(logger, targetInstance, methodInfo);

        /// <summary>
        /// Initializes the Command with the target instance method information.
        /// </summary>
        /// <param name="targetInstance">The instance of the object containing the method.</param>
        /// <param name="methodInfo">The MethodInfo of the instance method to execute.</param>
        public static Command CreateFromClassMethod(ILogger logger, Type targetType, MethodInfo methodInfo)
            => new Command(logger, targetType, methodInfo);

        Command(ILogger logger, MethodInfo methodInfo)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));
            if (methodInfo == null)
                throw new ArgumentNullException(nameof(methodInfo));
            if (!methodInfo.IsStatic)
                throw new ArgumentException("The provided method must be static.");

            _logger = logger;
            _methodInfo = methodInfo;
        }

        Command(ILogger logger, object targetInstance, MethodInfo methodInfo)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));
            if (targetInstance == null)
                throw new ArgumentNullException(nameof(targetInstance));
            if (methodInfo == null)
                throw new ArgumentNullException(nameof(methodInfo));
            if (methodInfo.IsStatic)
                throw new ArgumentException("The provided method must be an instance method. Use the other constructor for static methods.");

            _logger = logger;
            _targetInstance = targetInstance;
            _methodInfo = methodInfo;
        }

        Command(ILogger logger, Type targetType, MethodInfo methodInfo)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));
            if (targetType == null)
                throw new ArgumentNullException(nameof(targetType));
            if (methodInfo == null)
                throw new ArgumentNullException(nameof(methodInfo));
            if (methodInfo.IsStatic)
                throw new ArgumentException("The provided method must be an instance method. Use the other constructor for static methods.");

            _logger = logger;
            _targetType = targetType;
            _methodInfo = methodInfo;
        }

        /// <summary>
        /// Executes the target static method with the provided arguments.
        /// </summary>
        /// <param name="parameters">The arguments to pass to the method.</param>
        /// <returns>The result of the method execution, or null if the method is void.</returns>
        public IResponseData Execute(params object[] parameters)
        {
            if (_methodInfo == null)
                throw new InvalidOperationException("The method information is not initialized.");

            // If _targetInstance is null and _targetType is set, create an instance of the target type
            var instance = _targetInstance ?? (_targetType != null ? Activator.CreateInstance(_targetType) : null);

            // Build the final parameters array, filling in default values where necessary
            var finalParameters = BuildParameters(parameters);
            if (finalParameters != null)
            {
                foreach (var parameter in finalParameters)
                    _logger.LogTrace("Parameter: {0}", parameter);
            }
            else
            {
                _logger.LogTrace("No parameters provided.");
            }

            // Invoke the method (static or instance)
            var result = _methodInfo.Invoke(instance, finalParameters);
            // if (result == null)
            //     return ResponseData.Error("Something went wrong. Result is null.");
            return result as IResponseData ?? ResponseData.Success(result?.ToString());
        }

        /// <summary>
        /// Executes the target method with named parameters.
        /// Missing parameters will be filled with their default values or the type's default value if no default is defined.
        /// </summary>
        /// <param name="namedParameters">A dictionary mapping parameter names to their values.</param>
        /// <returns>The result of the method execution, or null if the method is void.</returns>
        public IResponseData Execute(IDictionary<string, object?>? namedParameters)
        {
            if (_methodInfo == null)
                throw new InvalidOperationException("The method information is not initialized.");

            // If _targetInstance is null and _targetType is set, create an instance of the target type
            var instance = _targetInstance ?? (_targetType != null ? Activator.CreateInstance(_targetType) : null);

            // Build the final parameters array, filling in default values where necessary
            var finalParameters = BuildParameters(namedParameters);
            if (finalParameters != null)
            {
                foreach (var parameter in finalParameters)
                    _logger.LogTrace("Parameter: {0}", parameter);
            }
            else
            {
                _logger.LogTrace("No parameters provided.");
            }

            // Invoke the method (static or instance)
            var result = _methodInfo.Invoke(instance, finalParameters);

            // if (result == null)
            //     return ResponseData.Error("Something went wrong. Result is null.");
            return result as IResponseData ?? ResponseData.Success(result?.ToString());
        }

        object?[]? BuildParameters(object?[]? parameters)
        {
            if (parameters == null)
                return null;

            var methodParameters = _methodInfo.GetParameters();

            // Prepare the final arguments array, filling in default values where necessary
            var finalParameters = new object?[methodParameters.Length];
            for (int i = 0; i < methodParameters.Length; i++)
            {
                if (i < parameters.Length)
                {
                    // Handle JsonElement conversion
                    if (parameters[i] is JsonElement jsonElement)
                    {
                        finalParameters[i] = JsonSerializer.Deserialize(jsonElement.GetRawText(), methodParameters[i].ParameterType);
                    }
                    else
                    {
                        // Use the provided parameter value
                        finalParameters[i] = parameters[i];
                    }
                }
                else if (methodParameters[i].HasDefaultValue)
                {
                    // Use the default value if no value is provided
                    finalParameters[i] = methodParameters[i].DefaultValue;
                }
                else
                {
                    throw new ArgumentException($"No value provided for parameter '{methodParameters[i].Name}' and no default value is defined.");
                }
            }

            return finalParameters;
        }

        object?[]? BuildParameters(IDictionary<string, object?>? namedParameters)
        {
            if (namedParameters == null)
                return null;

            var methodParameters = _methodInfo.GetParameters();

            // Prepare the final arguments array
            var finalParameters = new object?[methodParameters.Length];
            for (int i = 0; i < methodParameters.Length; i++)
            {
                var parameter = methodParameters[i];

                if (namedParameters != null && namedParameters.TryGetValue(parameter.Name!, out var value))
                {
                    if (value is JsonElement jsonElement)
                    {
                        finalParameters[i] = JsonSerializer.Deserialize(jsonElement.GetRawText(), methodParameters[i].ParameterType);
                    }
                    else
                    {
                        // Use the provided parameter value
                        finalParameters[i] = value;
                    }
                }
                else if (parameter.HasDefaultValue)
                {
                    // Use the default value if no value is provided
                    finalParameters[i] = parameter.DefaultValue;
                }
                else
                {
                    // Use the type's default value if no value is provided
                    finalParameters[i] = parameter.ParameterType.IsValueType
                        ? Activator.CreateInstance(parameter.ParameterType)
                        : null;
                }
            }

            return finalParameters;
        }
    }
}