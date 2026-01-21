using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "RPG/Class")]
public class ClassSO : ScriptableObject
{
    public string ClassName;
    public Sprite ClassIcon;
    public ClassSpec DefaultSpec;
    public CharacterSoundsSO ClassSound;
    public PlayerClassDefaultStatsSO DefaultStatsSO;
    public List<ClassSpecSO> Specs; 

    public ClassSpecSO GetSpecSO(ClassSpec spec)
    {
        foreach (ClassSpecSO s in Specs)
        {
            if (s.Spec == spec)
            {
                return s;
            }
        }
        return null;
    }
}
