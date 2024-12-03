using UnityEngine;

public class AbilityRuntimeContext
{
    public GameObject Caster { get; set; }
    public GameObject Target { get; set; }

    public bool IsSuccessful { get; set; } = true; // Default success state
}
