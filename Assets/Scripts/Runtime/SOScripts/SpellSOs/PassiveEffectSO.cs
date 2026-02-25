using Assets.Scripts.Runtime.Player;
using UnityEngine;

namespace Assets.Scripts.Runtime.SOScripts.SpellSOs
{
    public abstract class PassiveEffect : ScriptableObject
    {
        public abstract void Apply(PlayerStats stats);
        public abstract void Remove(PlayerStats stats);
    }
}