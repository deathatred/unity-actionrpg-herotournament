using UnityEngine;

namespace Assets.Scripts.Runtime.SOScripts.SummonsSO
{
    [CreateAssetMenu(fileName = "SummonData")]
    public class SummonDataSO : ScriptableObject
    {
        public int MaxHealth;
        public int Damage;
        public string Name;
    }
}