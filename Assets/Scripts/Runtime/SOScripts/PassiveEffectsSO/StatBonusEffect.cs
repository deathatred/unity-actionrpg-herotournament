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
        stats.ChangeTalentStat(StatType.Strength,StrengthBonus);
        stats.ChangeTalentStat(StatType.Agility,AgilityBonus);
        stats.ChangeTalentStat(StatType.Intellect, IntellectBonus);
        stats.ChangeTalentStat(StatType.SpellPower, SpellPowerBonus);
    }

    public override void Remove(PlayerStats stats)
    {
        stats.ChangeTalentStat(StatType.Strength, -StrengthBonus);
        stats.ChangeTalentStat(StatType.Agility, -AgilityBonus);
        stats.ChangeTalentStat(StatType.Intellect, -IntellectBonus);
        stats.ChangeTalentStat(StatType.SpellPower, -SpellPowerBonus);
    }
}
