using UnityEngine;

public class GolemAudio : CharacterAudioBase<GolemSoundsSO>
{
    public void PlaySmashSound()
    {
        _audioManager.Play3D(_characterSoundsSO.SmashImpactSound, transform, 0.1f);
    }
}
