using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class PlayerAnimations : MonoBehaviour
{
    [Inject] private EventBus _eventBus;

    public static readonly int MovingHash = Animator.StringToHash("IsMoving");
    public static readonly int AttackIndexHash = Animator.StringToHash("AttackIndex");
    public static readonly int IsAttackingHash = Animator.StringToHash("IsAttacking");
    public static readonly int IsCollectingHash = Animator.StringToHash("IsCollecting");
    private const int NUMBER_OF_ATTACK_ANIMATIONS = 4;
    private Animator _animator;
    private bool _isMoving;

    private bool _animatorIsSet;

    private void Update()
    {
        if (_animatorIsSet)
        {
            HandleAnimations();
        }
    }
    public void SetAnimator(Animator animator)
    {
        _animator = animator;
        _animatorIsSet = true;
    }
    private void HandleAnimations()
    {
        _animator.SetBool(MovingHash, _isMoving);
    }
    public Animator GetPlayerAnimator()
    {
        return _animator;
    }
    public void SetIsCollectingTrue()
    {
        _animator.SetBool(IsCollectingHash, true);
    }
    public void SetIsCollectingFalse()
    {
        _animator.SetBool(IsCollectingHash, false);
    }
    public void SetIsMovingTrue()
    {
        _isMoving = true;
    }
    public void SetIsMovingFalse()
    {
        _isMoving = false;
    }
    public void SetSpellTrigger(string spellTrigger)
    {
        var triggerHash = Animator.StringToHash(spellTrigger);
        _animator.SetTrigger(triggerHash);
    }
    public void StartAttackingAnimation()
    {
        int random = Random.Range(1, NUMBER_OF_ATTACK_ANIMATIONS + 1);
        _animator.SetInteger(AttackIndexHash, random);
        _animator.SetTrigger(IsAttackingHash);
    }
    public void EndAttackingAnimation()
    {
        _eventBus.Publish(new PlayerAttackEndedEvent());
    }
}
