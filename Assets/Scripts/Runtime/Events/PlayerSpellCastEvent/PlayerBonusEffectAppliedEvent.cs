using Assets.Scripts.Core.Observer;
using UnityEngine;

public class PlayerBonusEffectAppliedEvent : GameEventBase
{
    public string BonusEffect { get; private set; }
    public float Duration { get; private set; }
    public PlayerBonusEffectAppliedEvent(string bonusEffect, float duration)
    {
        BonusEffect = bonusEffect;
        Duration = duration;
    }
}
