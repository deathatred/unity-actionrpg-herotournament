using UnityEngine;

namespace Assets.Scripts.Runtime.SOScripts
{
    [CreateAssetMenu(fileName = "Character Sound")]
    public class CharacterSoundsSO : ScriptableObject
    {
        public AudioClip[] PreattackSounds;
        public AudioClip[] AttackSounds;
        public AudioClip SpecialAttackSound;
        public AudioClip[] StepSounds;
        public AudioClip DeathSound;
    }
}