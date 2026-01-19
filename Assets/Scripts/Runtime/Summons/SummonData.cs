using UnityEngine;


public class SummonData : MonoBehaviour
{
    [SerializeField] private SummonDataSO _data;

    public SummonDataSO GetSummonData()
    {
        return _data;
    }
}
