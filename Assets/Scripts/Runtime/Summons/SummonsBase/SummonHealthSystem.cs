using Assets.Scripts.Runtime.BaseLogic;
using Assets.Scripts.Runtime.SOScripts.SummonsSO;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Scripts.Runtime.Summons.SummonsBase
{
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
}