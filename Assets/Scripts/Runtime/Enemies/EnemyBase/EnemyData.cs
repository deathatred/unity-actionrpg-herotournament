using UnityEngine;

public class EnemyData : MonoBehaviour 
{
    [SerializeField] protected EnemyDataSO _data;
    public EnemyDataSO GetEnemyData()
    {
        return _data;
    }
}
