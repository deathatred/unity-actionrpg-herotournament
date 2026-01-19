using UnityEngine;

[CreateAssetMenu(fileName = "SummonData")]
public class SummonDataSO : ScriptableObject
{
    public int MaxHealth;
    public int Damage;
    public string Name;
}
