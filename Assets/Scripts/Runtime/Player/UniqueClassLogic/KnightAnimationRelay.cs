using UnityEngine;
using Zenject;


public class KnightAnimationRelay : MonoBehaviour
{
    [Inject] private PlayerAudio _playerAudio;
    [Inject] private PlayerSpellCasting _playerSpell;
    [Inject] private PlayerAnimations _playerAnimations;
    [Inject] private PlayerAttackSystem _playerAttackSystem;
    [Inject] private PlayerInteractions _playerInteractions;
    [Inject] private PlayerHealthSystem _playerHealthSystem;
    [Inject] private PlayerStats _playerStats;
    

    [SerializeField] private TrailRenderer[] _bloodTrails;
    public void EndAttack()
    {
        _playerAnimations.EndAttackingAnimation();
        _playerAudio.PlayAttackAudio();
    }
    public void EndCollect()
    {
        _playerInteractions.GetCurrentItemTarget()?.Collect();
        _playerAnimations.SetIsCollectingFalse();
    }
    public void AttackEnded()
    {
        _playerAttackSystem.OnAttackAnimationFinished();
    }
    public void SpellEnded()
    {
        _playerSpell.SpellCastingEnded();
    }
    public void OnAnimationEvent()
    {
        SpellSO lastSpell = _playerSpell.LastCastSpell;

        
        PlayerSpellContext ctx = new PlayerSpellContext(_playerHealthSystem,
            _playerInteractions.GetCurrentEnemyTarget()?.transform,
            _playerInteractions.GetCurrentEnemyTarget(), _playerStats, _playerSpell, _playerAudio);

        lastSpell.Activate(ctx);
       
    }
    public void PlayPreattackSound()
    {
        _playerAudio.PlayPreattackAudio();
    }
    public void PlayFootstepSound()
    {
        _playerAudio.PlayFootstepSound();
    }
    public void EnableBloodTrail()
    {
        foreach (var trail in  _bloodTrails)
        {
            trail.emitting = true;
        }
    }
    public void DisableBloodTrail()
    {
        foreach (var trail in _bloodTrails)
        {
            trail.emitting = false;
        }
    }
}
