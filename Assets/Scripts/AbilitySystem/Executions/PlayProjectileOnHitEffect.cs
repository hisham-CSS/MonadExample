using UnityEngine;

public class PlayProjectileOnHitEffect : AbilityFunctionBase, IPostExecution, IRequired
{
    public GameObject VisualEffectPrefab;

    public string[] Requires => new[] { "SpawnProjectile" };

    public Monad<AbilityRuntimeContext> PostExecute(Monad<AbilityRuntimeContext> runtimeContext)
    {
        runtimeContext.Value.SubscribeEvent("ProjectileHit", (eventData) =>
        {
            var hitTarget = eventData as GameObject;
            if (hitTarget != null && VisualEffectPrefab != null)
            {
                Instantiate(VisualEffectPrefab, hitTarget.transform.position, Quaternion.identity);
                Debug.Log("Played visual effect on hit.");
            }
        });
        // Play visual effect logic
        return runtimeContext;
    }

    public override string Validate()
    {
        return string.Empty;
    }
}