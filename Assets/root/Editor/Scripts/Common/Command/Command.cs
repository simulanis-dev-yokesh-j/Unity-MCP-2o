using System;
using System.Collections.Generic;
using System.Reflection;

namespace com.IvanMurzak.UnityMCP.Common
{
    /// <summary>
    /// Provides functionality to execute methods dynamically, supporting both static and instance methods.
    /// Allows for parameter passing by position or by name, with support for default parameter values.
    /// </summary>
    public partial class Command
    {
        private readonly MethodInfo _methodInfo;
        private readonly object? _targetInstance;
        private readonly Type? _targetType;

        /// <summary>
        /// Initializes the Command with the target method information.
        /// </summary>
        /// <param name="type">The type containing the static method.</param>
        public static Command CreateFromStaticMethod(MethodInfo methodInfo)
            => new Command(methodInfo);

        /// <summary>
        /// Initializes the Command with the target instance method information.
        /// </summary>
        /// <param name="targetInstance">The instance of the object containing the method.</param>
        /// <param name="methodInfo">The MethodInfo of the instance method to execute.</param>
        public static Command CreateFromInstanceMethod(object targetInstance, MethodInfo methodInfo)
            => new Command(targetInstance, methodInfo);

        /// <summary>
        /// Initializes the Command with the target instance method information.
        /// </summary>
        /// <param name="targetInstance">The instance of the object containing the method.</param>
        /// <param name="methodInfo">The MethodInfo of the instance method to execute.</param>
        public static Command CreateFromClassMethod(Type targetType, MethodInfo methodInfo)
            => new Command(targetType, methodInfo);

        Command(MethodInfo methodInfo)
        {
            if (methodInfo == null)
                throw new ArgumentNullException(nameof(methodInfo));
            if (!methodInfo.IsStatic)
                throw new ArgumentException("The provided method must be static.");

            _methodInfo = methodInfo;
        }

        Command(object targetInstance, MethodInfo methodInfo)
        {
            if (targetInstance == null)
                throw new ArgumentNullException(nameof(targetInstance));
            if (methodInfo == null)
                throw new ArgumentNullException(nameof(methodInfo));
            if (methodInfo.IsStatic)
                throw new ArgumentException("The provided method must be an instance method. Use the other constructor for static methods.");

            _targetInstance = targetInstance;
            _methodInfo = methodInfo;
        }

        Command(Type targetType, MethodInfo methodInfo)
        {
            if (targetType == null)
                throw new ArgumentNullException(nameof(targetType));
            if (methodInfo == null)
                throw new ArgumentNullException(nameof(methodInfo));
            if (methodInfo.IsStatic)
                throw new ArgumentException("The provided method must be an instance method. Use the other constructor for static methods.");

            _targetType = targetType;
            _methodInfo = methodInfo;
        }

        /// <summary>
        /// Executes the target static method with the provided arguments.
        /// </summary>
        /// <param name="parameters">The arguments to pass to the method.</param>
        /// <returns>The result of the method execution, or null if the method is void.</returns>
        public object Execute(params object[] parameters)
        {
            if (_methodInfo == null)
                throw new InvalidOperationException("The method information is not initialized.");

            // If _targetInstance is null and _targetType is set, create an instance of the target type
            var instance = _targetInstance ?? (_targetType != null ? Activator.CreateInstance(_targetType) : null);

            // Invoke the method (static or instance)
            return _methodInfo.Invoke(instance, BuildParameters(parameters));
        }

        /// <summary>
        /// Executes the target method with named parameters.
        /// Missing parameters will be filled with their default values or the type's default value if no default is defined.
        /// </summary>
        /// <param name="namedParameters">A dictionary mapping parameter names to their values.</param>
        /// <returns>The result of the method execution, or null if the method is void.</returns>
        public object Execute(Dictionary<string, object?> namedParameters)
        {
            if (_methodInfo == null)
                throw new InvalidOperationException("The method information is not initialized.");

            // If _targetInstance is null and _targetType is set, create an instance of the target type
            var instance = _targetInstance ?? (_targetType != null ? Activator.CreateInstance(_targetType) : null);

            // Invoke the method (static or instance)
            return _methodInfo.Invoke(instance, BuildParameters(namedParameters));
        }

        object[] BuildParameters(object[] parameters)
        {
            var methodParameters = _methodInfo.GetParameters();

            // Prepare the final arguments array, filling in default values where necessary
            var finalParameters = new object[methodParameters.Length];
            for (int i = 0; i < methodParameters.Length; i++)
            {
                if (i < parameters.Length)
                {
                    // Use the provided parameter value
                    finalParameters[i] = parameters[i];
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

        object[] BuildParameters(Dictionary<string, object?> namedParameters)
        {
            var methodParameters = _methodInfo.GetParameters();

            // Prepare the final arguments array
            var finalParameters = new object[methodParameters.Length];
            for (int i = 0; i < methodParameters.Length; i++)
            {
                var parameter = methodParameters[i];

                if (namedParameters != null && namedParameters.TryGetValue(parameter.Name!, out var value))
                {
                    // Use the provided named parameter value
                    finalParameters[i] = value;
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