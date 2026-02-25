using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Core.Enums;

namespace Assets.Scripts.Runtime.SOScripts.ClassSO
{
    [CreateAssetMenu(menuName = "RPG/Class Spec")]
    public class ClassSpecSO : ScriptableObject
    {
        public PlayerClassSO SpecsClass;
        public string SpecName;
        public ClassSpec Spec;
        public Sprite Background;
        public Sprite Icon;
    }
}