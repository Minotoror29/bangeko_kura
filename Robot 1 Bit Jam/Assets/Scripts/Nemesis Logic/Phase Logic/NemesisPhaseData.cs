using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Nemesis Phase")]
public class NemesisPhaseData : ScriptableObject
{
    public float pauseAfterDash;
    [Tooltip("Time between each projectile")] public float shootTime;
    public int minProjectilesPerSalvo;
    public int maxProjectilesPerSalvo;
    public float pauseAfterShoot;

    public NemesisPhase Phase(NemesisController controller)
    {
        return new NemesisPhase(this, controller);
    }
}
