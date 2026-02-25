using Assets.Scripts.Runtime.SOScripts;
using Assets.Scripts.Runtime.SOScripts.SpellSOs;
using Assets.Scripts.Runtime.SpellsContext;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Runtime.SOScripts.SpellSOs.Spells
{
    [CreateAssetMenu(fileName = "Fireball", menuName = "Wizard Spells/Fireball")]
    public class FireballSO : SpellSO
    {
        [SerializeField] private ProjectileSO _fireballProjectile;
        public override void Activate(PlayerSpellContext ctx)
        {
            ctx.SpellCasting.ExecuteProjectileSpellAsync(_fireballProjectile, ctx.EnemyTransform).Forget();
            ctx.Audio.PlaySpellAudio(this);
        }
    }
}