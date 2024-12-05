public interface IExecution
{
    // Executes the function with a runtime context
    Monad<AbilityRuntimeContext> Execute(Monad<AbilityRuntimeContext> runtimeContext);

    // Validates the function's setup
    string Validate();
}

public interface IPostExecution
{
    // Executes the function with a runtime context
    Monad<AbilityRuntimeContext> PostExecute(Monad<AbilityRuntimeContext> runtimeContext);

    // Validates the function's setup
    string Validate();
}

public interface IRequired
{
    string[] Requires { get; } // Dependencies this step needs
}