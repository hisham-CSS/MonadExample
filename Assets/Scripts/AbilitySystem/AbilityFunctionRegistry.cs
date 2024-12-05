using System;
using System.Linq;
using System.Collections.Generic;

public static class AbilityFunctionRegistry
{
    // Cache discovered types to avoid repeated reflection
    private static Dictionary<Type, List<Type>> _abilityFunctionCache = new Dictionary<Type, List<Type>>();

    /// <summary>
    /// Retrieves all ability functions that implement the specified interface.
    /// </summary>
    /// <typeparam name="T">The interface type (e.g., IExecution or IPostExecution).</typeparam>
    /// <returns>List of types that implement the specified interface.</returns>
    public static List<Type> GetAllAbilityFunctions<T>() where T : class
    {
        if (!_abilityFunctionCache.TryGetValue(typeof(T), out var cachedTypes))
        {
            // Find all types in the current assembly that implement the given interface
            cachedTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => typeof(T).IsAssignableFrom(type) && !type.IsAbstract && !type.IsInterface)
                .ToList();

            _abilityFunctionCache[typeof(T)] = cachedTypes; // Cache the result
        }

        return cachedTypes;
    }
}