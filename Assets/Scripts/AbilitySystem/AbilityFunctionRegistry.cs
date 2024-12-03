using System;
using System.Linq;
using System.Collections.Generic;

public static class AbilityFunctionRegistry
{
    // Cache the discovered types to avoid repeated reflection
    private static List<Type> _abilityFunctions;

    public static List<Type> GetAllAbilityFunctions()
    {
        if (_abilityFunctions == null)
        {
            // Find all types in the current assembly that implement IAbilityFunction
            _abilityFunctions = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => typeof(IAbilityFunction).IsAssignableFrom(type) && !type.IsAbstract && !type.IsInterface)
                .ToList();
        }

        return _abilityFunctions;
    }
}