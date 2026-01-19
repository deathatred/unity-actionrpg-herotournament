using UnityEngine;

public abstract class PassiveEffect : ScriptableObject
{
    public abstract void Apply(PlayerStats stats);
    public abstract void Remove(PlayerStats stats);
}
