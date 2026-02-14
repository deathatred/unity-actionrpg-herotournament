using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileSO")]
public class ProjectileSO : ScriptableObject
{
    public GameObject Prefab;
    public string Name;
    public float Speed;
    public int Damage;
    public bool Homing;
    public float HomingRotationSpeed = 120f;
    public EnemyStatusEffectSO StatusEffectSO;
    public float Lifetime;
    public AudioClip HitSound;
}
