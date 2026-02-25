using Assets.Scripts.Runtime.SOScripts;
using Assets.Scripts.Runtime.SOScripts.SpellSOs;
using Assets.Scripts.Runtime.SpellsContext;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Runtime.SOScripts.SpellSOs.Spells
{
    [CreateAssetMenu(fileName = "MagicDart", menuName = "Wizard Spells/MagicDart")]
    public class MagicDartSO : SpellSO
    {
        public ProjectileSO MagicDart;
        public override void Activate(PlayerSpellContext ctx)
        {
            ctx.SpellCasting.ExecuteProjectileSpellAsync(MagicDart, ctx.EnemyTransform).Forget();
            ctx.Audio.PlaySpellAudio(this);
        }
    }
}