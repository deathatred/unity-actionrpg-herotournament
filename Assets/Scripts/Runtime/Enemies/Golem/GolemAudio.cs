using Assets.Scripts.Runtime.BaseLogic;
using Assets.Scripts.Runtime.SOScripts.EnemiesSO.Golem;
using UnityEngine;

namespace Assets.Scripts.Runtime.Enemies.Golem
{
    public class GolemAudio : CharacterAudioBase<GolemSoundsSO>
    {
        public void PlaySmashSound()
        {
            _audioManager.Play3D(_characterSoundsSO.SmashImpactSound, transform, 0.1f);
        }
    }
}