using UnityEngine;

namespace Assets.Scripts.Runtime.SOScripts
{
    [CreateAssetMenu(fileName = "ClassDefaultStats")]
    public class PlayerClassDefaultStatsSO : ScriptableObject
    {
        public int DefaultHealth;
        public int DefaultMana;
        public int Strenght;
        public int Agility;
        public int Intellect;
        public int AttackPower;
        public int Armor;
        public int SpellPower;
        public int MoveSpeed;
        public int Vampirism;
        public int AttackSpeed;
        public int CriticalRate;
    }
}