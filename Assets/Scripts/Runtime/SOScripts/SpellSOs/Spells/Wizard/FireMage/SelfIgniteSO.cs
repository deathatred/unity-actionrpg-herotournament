using Assets.Scripts.Core.Enums;
using Assets.Scripts.Core.General;
using Assets.Scripts.Runtime.SOScripts.SpellSOs;
using Assets.Scripts.Runtime.SpellsContext;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Runtime.SOScripts.SpellSOs.Spells
{
    [CreateAssetMenu(fileName = "SelfIgnite", menuName = "Wizard Spells/SelfIgnite")]
    public class SelfIgniteSO : SpellSO
    {
        [SerializeField] private int _spellPowerBonus;
        [SerializeField] private float _bonusDuration;
        public override void Activate(PlayerSpellContext ctx)
        {
            ctx.Audio.PlaySpellAudio(this);
            ctx.Stats.ApplyTemporaryBonusAsync(GlobalData.SELF_IGNITE_EFFECT, StatType.SpellPower, _spellPowerBonus, _bonusDuration).Forget();
        }
    }
}