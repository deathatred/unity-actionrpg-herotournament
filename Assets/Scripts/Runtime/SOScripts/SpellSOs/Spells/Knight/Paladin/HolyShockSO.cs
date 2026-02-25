using Assets.Scripts.Runtime.SOScripts;
using Assets.Scripts.Runtime.SOScripts.SpellSOs;
using Assets.Scripts.Runtime.SpellsContext;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Runtime.SOScripts.SpellSOs.Spells
{
    [CreateAssetMenu(fileName = "HolyShock", menuName = "Player Spells/HolyShock")]
    public class HolyShockSO : SpellSO
    {
        [SerializeField] private ProjectileSO _lightProjectile;
        public override void Activate(PlayerSpellContext ctx)
        {
            ctx.SpellCasting.ExecuteProjectileSpellAsync(_lightProjectile,
                ctx.EnemyTransform).Forget();
            ctx.Audio.PlaySpellAudio(this);
        }
    }
}