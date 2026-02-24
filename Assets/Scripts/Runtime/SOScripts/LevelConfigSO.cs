using Assets.Scripts.Core.Enums;
using Assets.Scripts.Core.Utils;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig")]
public class LevelConfigSO : ScriptableObject
{
    public LevelType LevelType;
    public int SurvivalDuration = 70;
    public int SpawnInterval = 10;
    public EnemyWave Wave;
    public AudioClip _levelMusic;
    public AudioClip _levelAmbient;
}
