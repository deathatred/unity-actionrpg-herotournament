using Assets.Scripts.Runtime.SOScripts.SummonsSO;
using UnityEngine;

namespace Assets.Scripts.Runtime.Summons
{
    public class SummonData : MonoBehaviour
    {
        [SerializeField] private SummonDataSO _data;

        public SummonDataSO GetSummonData()
        {
            return _data;
        }
    }
}