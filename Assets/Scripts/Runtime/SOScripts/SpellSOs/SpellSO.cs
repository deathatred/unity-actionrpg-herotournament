using Assets.Scripts.Core.Enums;
using Assets.Scripts.Runtime.SpellsContext;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Runtime.SOScripts.SpellSOs
{
    [CreateAssetMenu(fileName = "Spell")]
    public abstract class SpellSO : ScriptableObject
    {
        public PlayerClass Class;
        public string Name;
        public string About;
        public int ManaCost;
        public float Cooldown;
        public string AnimationTrigger;
        public Sprite Icon;
        public float Range;
        public bool CanBeCastWhileMoving;
        public AudioClip SoundEffect;

        public abstract void Activate(PlayerSpellContext ctx);
    }
}