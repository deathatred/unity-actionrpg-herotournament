using UnityEngine;
using UnityEngine.Rendering;
using Zenject;

public abstract class CharacterAudioBase<SoundSO> : MonoBehaviour where SoundSO : CharacterSoundsSO
{
    [Inject] protected AudioManager _audioManager;
    [SerializeField] protected SoundSO _characterSoundsSO;
    protected virtual void PlayAttackSound(float volume)
    {
        PlayRandom(_characterSoundsSO.AttackSounds, volume);
    }
    protected virtual void PlayDeathSound(float volume)
    {
        _audioManager.Play3D(_characterSoundsSO.DeathSound, transform, volume);

    }
    protected virtual void PlayPreattackSound(float volume)
    {
        PlayRandom(_characterSoundsSO.PreattackSounds, volume);
    }
    protected virtual void PlaySpecialAttackSound(float volume)
    {

    }
    protected virtual void PlayMoveSound(float volume)
    {
        PlayRandom(_characterSoundsSO.StepSounds, volume);
    }
    private void PlayRandom(AudioClip[] clips, float volume)
    {
        int random = Random.Range(0, clips.Length);
        _audioManager.Play3D(clips[random], transform, volume);
    }
    public void PlayPreattack(float volume)
    {
        PlayPreattackSound(volume);
    }
    public void PlayAttack(float volume)
    {
        PlayAttackSound(volume);
    }
    public void PlayMove(float volume   )
    {
        PlayMoveSound(volume);
    }
    public void PlaySpecialAttack(float volume)
    {
        PlaySpecialAttackSound(volume);
    }

    public void PlayDeath(float volume)
    {
        PlayDeathSound(volume);
    }

}
