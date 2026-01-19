using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "RPG/Class Spec")]
public class ClassSpecSO : ScriptableObject
{
    public ClassSO SpecsClass;
    public string SpecName;
    public ClassSpec Spec;
    public Sprite Background;
    public Sprite Icon; 
}
