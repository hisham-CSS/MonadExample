public interface IAbilityFunction
{
    // Executes the function with a runtime context
    Monad<AbilityRuntimeContext> Execute(Monad<AbilityRuntimeContext> runtimeContext);

    // Validates the function's setup
    string Validate();
}
