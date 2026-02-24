using Assets.Scripts.Core.Structs;
using UnityEngine;

public class EyeOfCthulhuAudio : CharacterAudioBase<EyeOfCthulhuSoundsSO>
{
    private AudioHandle _hoverHandle;
    private void OnEnable()
    {
        PlayHoverSound();
    }
    private void OnDisable()
    {
        StopHoverSound();
    }
    protected override void PlayDeathSound(float volume)
    {
        StopHoverSound();
        base.PlayDeathSound(volume);
    }
    public void PlayHoverSound()
    {
        if (_hoverHandle.IsValid)
            return; 

        _hoverHandle = _audioManager.Play3DFollow(
            _characterSoundsSO.HoverSound,
            transform,
            volume: 0.01f,
            loop: true,
            pitch: 1f
        );
    }
    public void StopHoverSound()
    {
        if (!_hoverHandle.IsValid)
            return;

        _audioManager.Stop(_hoverHandle);
        _hoverHandle = AudioHandle.Empty;
    }
}
