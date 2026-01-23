using UnityEngine;

[CreateAssetMenu(menuName = "Talent Tree/Talent")]
public class TalentSO : ScriptableObject
{
    public string ID;
    public string TalentName;
    public Sprite Icon;
    public string Description;

    public SpellType SpellType;
    public SpellSO Spell;
    public PassiveEffect[] PassiveEffects;
    public ClassSpec Spec; 
    public TalentSO[] Prerequisites;

    public int MaxLevel = 1;
}
