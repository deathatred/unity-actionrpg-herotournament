using Assets.Scripts.Runtime.Managers;
using Assets.Scripts.Runtime.SOScripts;
using Assets.Scripts.Runtime.SOScripts.SpellSOs;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Runtime.Player
{
    public class PlayerAudio : MonoBehaviour
    {
        [Inject] private AudioManager _audioManager;
        private CharacterSoundsSO _characterSounds;
        public void InitCharacterSounds(CharacterSoundsSO sounds)
        {
            _characterSounds = sounds;
        }
        public void PlayFootstepSound()
        {
            int random = Random.Range(0, _characterSounds.StepSounds.Length);
            _audioManager.Play2D(_characterSounds.StepSounds[random], 0.01f);
        }
        public void PlaySpellAudio(SpellSO spell, Transform target = null)
        {
            if (target == null)
            {
                _audioManager.Play2D(spell.SoundEffect, 0.1f);
            }
            else
            {
                _audioManager.Play3D(spell.SoundEffect, target, 0.1f);
            }
        }
        public void PlayAttackAudio()
        {
            int random = Random.Range(0, _characterSounds.AttackSounds.Length);
            _audioManager.Play2D(_characterSounds.AttackSounds[random], 0.1f);
        }
        public void PlayPreattackAudio()
        {
            int random = Random.Range(0, _characterSounds.PreattackSounds.Length);
            _audioManager.Play2D(_characterSounds.PreattackSounds[random], 0.1f);
        }
    }
}