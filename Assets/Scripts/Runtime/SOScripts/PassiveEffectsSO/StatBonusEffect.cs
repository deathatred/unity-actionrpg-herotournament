using UnityEngine;

[CreateAssetMenu(menuName = "Talent Tree/Effects/Stat Bonus")]
public class StatBonusEffect : PassiveEffect
{
    public int StrengthBonus;
    public int AgilityBonus;
    public int IntellectBonus;
    public int SpellPowerBonus;

    public override void Apply(PlayerStats stats)
    {
        stats.ChangeOutsideStat(StatType.Strength,StrengthBonus);
        stats.ChangeOutsideStat(StatType.Agility,AgilityBonus);
        stats.ChangeOutsideStat(StatType.Intellect, IntellectBonus);
        stats.ChangeOutsideStat(StatType.SpellPower, SpellPowerBonus);
    }

    public override void Remove(PlayerStats stats)
    {
        stats.ChangeOutsideStat(StatType.Strength, -StrengthBonus);
        stats.ChangeOutsideStat(StatType.Agility, -AgilityBonus);
        stats.ChangeOutsideStat(StatType.Intellect, -IntellectBonus);
        stats.ChangeOutsideStat(StatType.SpellPower, -SpellPowerBonus);
    }
}
