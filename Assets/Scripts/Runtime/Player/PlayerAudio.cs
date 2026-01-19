using UnityEngine;
using Zenject;

public class PlayerAudio : MonoBehaviour
{
    [Inject] private AudioManager _audioManager;
    [SerializeField] private CharacterSoundsSO _characterSounds;
    public void PlayFootstepSound()
    {
        int random = Random.Range(0, _characterSounds.StepSounds.Length);
        _audioManager.Play2D(_characterSounds.StepSounds[random], 0.01f);
    }
    public void PlaySpellAudio(SpellSO spell)
    {
        _audioManager.Play2D(spell.SoundEffect, 0.1f);
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
