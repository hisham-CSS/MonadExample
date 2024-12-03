using UnityEngine;

public class AbilityExecutor : MonoBehaviour
{
    public void ExecuteAbility(AbilityContext abilityContext, GameObject caster, GameObject target)
    {
        var runtimeContext = new AbilityRuntimeContext
        {
            Caster = caster,
            Target = target
        };

        var monad = new Monad<AbilityRuntimeContext>(runtimeContext);

        foreach (var step in abilityContext.ExecutionSteps)
        {
            monad = step.Execute(monad);
            if (!monad.Value.IsSuccessful) break; // Stop execution on failure
        }

        Debug.Log($"Ability {abilityContext.AbilityName} execution {(monad.Value.IsSuccessful ? "succeeded" : "failed")}.");
    }
}
