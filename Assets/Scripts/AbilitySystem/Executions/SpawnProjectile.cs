using UnityEngine;

public class SpawnProjectile : AbilityFunctionBase
{
    public GameObject ProjectilePrefab;
    public float Speed;
    public float Lifetime;

    public override Monad<AbilityRuntimeContext> Execute(Monad<AbilityRuntimeContext> runtimeContext)
    {
        if (ProjectilePrefab != null)
        {
            var casterPosition = runtimeContext.Value.Caster.transform.position;
            var targetPosition = runtimeContext.Value.Target.transform.position;
            var direction = (targetPosition - casterPosition).normalized;

            var projectile = Instantiate(ProjectilePrefab, casterPosition, Quaternion.identity);
            var rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = direction * Speed;
            }

            Destroy(projectile, Lifetime);
            return runtimeContext;
        }

        Debug.LogError("ProjectilePrefab is not assigned.");
        runtimeContext.Value.IsSuccessful = false;
        return runtimeContext;
    }

    public override string Validate()
    {
        string errorMsg = string.Empty;
        if (!ProjectilePrefab)
            errorMsg = "ProjectilePrefab must be assigned. ";

        if (Speed <= 0)
            errorMsg += "Speed should be greater than 0. ";

        if (Lifetime <= 0)
            errorMsg += "Lifetime should be greater than 0. ";

        return errorMsg;
    }
}