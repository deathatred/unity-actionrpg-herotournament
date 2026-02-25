using Assets.Scripts.Runtime.SOScripts;
using Assets.Scripts.Runtime.SOScripts.SpellSOs;
using Assets.Scripts.Runtime.SpellsContext;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Runtime.SOScripts.SpellSOs.Spells
{
    [CreateAssetMenu(fileName = "IceArrow", menuName = "Wizard Spells/IceArrow")]
    public class IceArrowSO : SpellSO
    {
        [SerializeField] private ProjectileSO _iceArrow;
        public override void Activate(PlayerSpellContext ctx)
        {
            ctx.SpellCasting.ExecuteProjectileSpellAsync(_iceArrow, ctx.EnemyTransform).Forget();
            ctx.Audio.PlaySpellAudio(this);
        }
    }
}