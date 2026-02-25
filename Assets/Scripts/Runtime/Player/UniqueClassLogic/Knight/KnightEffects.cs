using UnityEngine;

namespace Assets.Scripts.Runtime.Player.UniqueClassLogic.Knight
{
    public class KnightEffects : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _holyHealParticles;

        public void PlayHolyHealParticles()
        {
            Quaternion rotation = Quaternion.Euler(-90f, 0, 0);
            ParticleSystem particles = Instantiate(_holyHealParticles, transform.position, rotation, transform);
        }
    }
}