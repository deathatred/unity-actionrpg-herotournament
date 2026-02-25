using Assets.Scripts.Runtime.SOScripts.EnemiesSO;
using UnityEngine;

namespace Assets.Scripts.Runtime.Enemies.EnemyBase
{
    public class EnemyData : MonoBehaviour
    {
        [SerializeField] protected EnemyDataSO _data;
        public EnemyDataSO GetEnemyData()
        {
            return _data;
        }
    }
}