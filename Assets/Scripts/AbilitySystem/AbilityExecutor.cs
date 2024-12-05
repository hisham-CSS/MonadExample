using System.Linq;
using UnityEngine;

public class AbilityExecutor : MonoBehaviour
{
    public void ExecuteAbility(AbilityContext abilityContext, GameObject caster, GameObject target)
    {
        AbilityRuntimeContext runtimeContext = new AbilityRuntimeContext
        {
            Caster = caster,
            Target = target
        };

        Monad<AbilityRuntimeContext> monad = new Monad<AbilityRuntimeContext>(runtimeContext);

        // Execute all Executions
        foreach (IExecution execution in abilityContext.ExecutionSteps.OfType<IExecution>())
        {
            monad = execution.Execute(monad);
            if (!monad.Value.IsSuccessful)
            {
                Debug.LogError($"Execution step {execution.GetType().Name} failed. Aborting ability.");
                return; // Stop ability execution on failure
            }
        }

        // Execute all PostExecutions
        foreach (IPostExecution postExecution in abilityContext.ExecutionSteps.OfType<IPostExecution>())
        {
            if (postExecution is IRequired requiredStep)
            {
                // Validate dependencies
                foreach (var dependency in requiredStep.Requires)
                {
                    if (!abilityContext.ExecutionSteps.Any(e => e.Name == dependency))
                    {
                        Debug.LogWarning($"Skipping PostExecution {postExecution.GetType().Name}: Missing required dependency {dependency}.");
                        continue;
                    }
                }
            }

            monad = postExecution.PostExecute(monad);
        }

        Debug.Log($"Ability {abilityContext.AbilityName} execution {(monad.Value.IsSuccessful ? "succeeded" : "failed")}.");
    }
}
