using UnityEngine;

namespace Assets.Scripts.Runtime.SOScripts.EnemiesSO
{
    [CreateAssetMenu(menuName = "Enemies Data")]
    public class EnemyDataSO : ScriptableObject
    {
        public string ID;
        public int MaxHealth;
        public int Damage;
        public string Name;
        public int XpReward;
        public float ViewAngle;
        public float ViewDistance;
        public float LeashRange;
        public float AttackRange;
        public float LoseSightTime;
        public float AttackCooldown;
        public float CloseRangeTrigger;
        public GameObject Prefab;
    }
}