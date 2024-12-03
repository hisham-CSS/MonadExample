using UnityEngine;

public class CheckMana : AbilityFunctionBase
{
    public float RequiredMana;

    public override Monad<AbilityRuntimeContext> Execute(Monad<AbilityRuntimeContext> runtimeContext)
    {
        var context = runtimeContext.Value;

        if (context.Caster.GetComponent<Mana>().CurrentMana >= RequiredMana)
        {
            Debug.Log("Mana check passed.");
            return runtimeContext;
        }

        Debug.LogError("Not enough mana.");
        context.IsSuccessful = false;
        return new Monad<AbilityRuntimeContext>(context);
    }

    public override string Validate()
    {
        if (RequiredMana <= 0)
            return "RequiredMana must be greater than 0.";
        return string.Empty;
    }
}
