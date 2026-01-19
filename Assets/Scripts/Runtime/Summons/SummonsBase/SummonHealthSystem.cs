

using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class SummonHealthSystem : BaseHealthSystem<SummonDataSO>
{
    public override int GetMaxHpFromData()
    {
        return _data.MaxHealth;
    }

    protected override void DeathLogic()
    {
        
    }
}

