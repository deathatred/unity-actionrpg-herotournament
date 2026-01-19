using UnityEngine;

[CreateAssetMenu(fileName = "SummonSkeleton", menuName = "Player Spells/SummonSkeleton")]
public class SummonSkeletonSO : SpellSO
{
    public GameObject _skeletonPrefab;
    public override void Activate(PlayerSpellContext ctx)
    {
        ctx.SpellCasting.ExecuteSummoningSpell(_skeletonPrefab);
        ctx.Audio.PlaySpellAudio(this);
    }
}
