using Assets.Scripts.Runtime.SOScripts;
using Assets.Scripts.Runtime.SOScripts.SpellSOs;
using Assets.Scripts.Runtime.SpellsContext;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Runtime.SOScripts.SpellSOs.Spells
{
    [CreateAssetMenu(fileName = "IceShackles", menuName = "Wizard Spells/IceShackles")]
    public class IceShacklesSO : SpellSO
    {
        [SerializeField] private EnemyStatusEffectSO _statusEffect;
        public override void Activate(PlayerSpellContext ctx)
        {
            ctx.Audio.PlaySpellAudio(this, ctx.EnemyTransform);
            ctx.EnemyStatusEffectsManager.ApplyStatusEffect(_statusEffect);
        }
    }
}