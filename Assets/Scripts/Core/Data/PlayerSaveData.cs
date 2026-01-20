using System;
using UnityEngine;

[Serializable]
public class PlayerSaveData
{
    public PlayerClass PlayerClass;
    public int CurrentLevel;
    public int ExpAmount;
    public int ExpRequired;
    public int LevelPointsAmount;
    public int TalentPointsAmount;
    public int CurrentHealth;
    public int CurrentMana;
    public StatsSaveData StatsData;
    public TalentSaveData[] LearnedTalentsIds;
    public InventoryItemsSaveData[] InventoryItems;
    public Vector3 Position;
}
