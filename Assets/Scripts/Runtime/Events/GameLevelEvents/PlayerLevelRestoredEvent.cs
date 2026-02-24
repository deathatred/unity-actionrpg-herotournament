using Assets.Scripts.Core.Observer;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerLevelRestoredEvent : GameEventBase
{
    public int RestoredLevel;
    public int RestoredLevelPoints;
    public int CurrentXp;
    public int XpToNextLevel;
    public PlayerLevelRestoredEvent(int level, int levelPoints,int currentXp, int xpToNextLevel)
    {
        RestoredLevel = level;
        RestoredLevelPoints  = levelPoints;
        CurrentXp = currentXp;
        XpToNextLevel = xpToNextLevel;
    } 
    
}

