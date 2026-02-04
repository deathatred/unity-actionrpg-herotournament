using UnityEngine;
using Zenject;

public class WizardEffects : MonoBehaviour
{
    private EventBus _eventBus;
    [SerializeField] private ParticleSystem _selfIgniteParticles;
    [Inject]
    private void Construct(EventBus eventBus)
    {
        _eventBus = eventBus;
    }
    private void OnEnable()
    {
        _eventBus.Subscribe<PlayerBonusEffectAppliedEvent>(ApplyBonusEffectVFX);
    }
    private void OnDisable()
    {
        _eventBus.Unsubscribe<PlayerBonusEffectAppliedEvent>(ApplyBonusEffectVFX);
    }
    private void ApplyBonusEffectVFX(PlayerBonusEffectAppliedEvent e)
    {
        Debug.Log("here");
        if (e.BonusEffect == GlobalData.SELF_IGNITE_EFFECT)
        {
            Debug.Log("here inside");
            PlaySelfIgniteParticles(e.Duration);
        }
    }
    public void PlaySelfIgniteParticles(float duration)
    {
        Quaternion rotation = Quaternion.Euler(-90f, 0, 0);
        ParticleSystem particles = Instantiate(_selfIgniteParticles, transform.position, rotation, transform);
        var main = particles.main;
        main.duration = duration;
        particles.Play();
    } 
}
