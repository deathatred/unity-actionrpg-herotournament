using Assets.Scripts.Runtime.SOScripts.SpellSOs;
using Assets.Scripts.Runtime.SpellsContext;
using UnityEngine;

namespace Assets.Scripts.Runtime.SOScripts.SpellSOs.Spells
{
    [CreateAssetMenu(fileName = "SummonSkeleton", menuName = "Player Spells/SummonSkeleton")]
    public class SummonSkeletonSO : SpellSO
    {
        [SerializeField] private GameObject _skeletonPrefab;
        public override void Activate(PlayerSpellContext ctx)
        {
            ctx.SpellCasting.ExecuteSummoningSpell(_skeletonPrefab);
            ctx.Audio.PlaySpellAudio(this);
        }
    }
}