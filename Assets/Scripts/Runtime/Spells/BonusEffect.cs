using UnityEngine;

public class BonusEffect
{
    public StatType StatType;
    public int Amount;
    public float RemainingTime;

    public BonusEffect(StatType statType, int amount, float remainingTime)
    {
        StatType = statType;
        Amount = amount;
        RemainingTime = remainingTime;
    }
}
