using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class EffectsViewUI : MonoBehaviour
{
    [Inject] private EventBus _eventBus;
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI _levelUpText;
    [SerializeField] private TextMeshProUGUI _statPointBonusText;
    [SerializeField] private TextMeshProUGUI _talentPointBonusText;
    [SerializeField] private List<Image> _levelUpArrowsImageList;

    [Header("Arrow Settings")]
    [SerializeField] private float _arrowMoveDistance = 10f;
    [SerializeField] private float _arrowMoveDuration = 1f;
    [SerializeField] private float _arrowSpawnDelay = 0.15f;

    private Sequence _arrowLoopSequence;
    private void Awake()
    {
        ResetVisuals();
    }

    public void OnEnable()
    {
        SubscribeToEvents();
    }

    public void OnDisable()
    {
        UnsubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        _eventBus.Subscribe<PlayerLevelChangedEvent>(OnLevelUp);
    }

    private void UnsubscribeToEvents()
    {
        _eventBus.Unsubscribe<PlayerLevelChangedEvent>(OnLevelUp);
    }

    private void OnLevelUp(PlayerLevelChangedEvent e)
    {
        PlayLevelUpEffect();
    }
    private void PlayLevelUpEffect()
    {
        ResetVisuals();

        _levelUpText.transform.localScale = Vector3.zero;
        _statPointBonusText.transform.localScale = Vector3.zero;
        _levelUpText.alpha = 1f;
        _statPointBonusText.alpha = 1f;
        _talentPointBonusText.transform.localScale = Vector3.zero;
        _talentPointBonusText.alpha = 1f;
        _levelUpText.transform.DOScale(1f, 0.35f).SetEase(Ease.OutBack);
        _statPointBonusText.transform.DOScale(1f, 0.35f).SetEase(Ease.OutBack);
        _talentPointBonusText.transform.DOScale(1f, 0.35f).SetEase(Ease.OutBack);
        _arrowLoopSequence?.Kill();
        _arrowLoopSequence = DOTween.Sequence();

        _arrowLoopSequence.AppendCallback(SpawnFreeArrow);
        _arrowLoopSequence.AppendInterval(_arrowSpawnDelay);
        _arrowLoopSequence.SetLoops(-1);

        _levelUpText.DOFade(0f, 0.4f).SetDelay(2.2f);
        _statPointBonusText.DOFade(0f, 0.8f).SetDelay(3.4f);
        _talentPointBonusText.DOFade(0f, 0.8f).SetDelay(3.4f);
        DOVirtual.DelayedCall(2.5f, StopArrowRain);
    }

    private void SpawnFreeArrow()
    {
        Image freeArrow = GetRandomFreeArrow();

        if (freeArrow != null)
        {
            PlaySingleArrowRandomX(freeArrow);
        }
        else
        {
            Debug.LogWarning("Not enought arrows");
        }
    }

    private Image GetRandomFreeArrow()
    {
        var availableArrows = _levelUpArrowsImageList.Where(a => !DOTween.IsTweening(a)).ToList();

        if (availableArrows.Count == 0) return null;

        return availableArrows[Random.Range(0, availableArrows.Count)];
    }

    private void PlaySingleArrowRandomX(Image arrow)
    {
        arrow.DOKill();

        RectTransform rect = arrow.rectTransform;

        float randomX = Random.Range(-500f, 500f);
        Vector2 startPos = new Vector2(randomX, -80f);
        Vector2 targetPos = startPos + Vector2.up * _arrowMoveDistance;

        rect.anchoredPosition = startPos;

        Sequence seq = DOTween.Sequence();
        seq.SetTarget(arrow);
        seq.Append(rect.DOAnchorPos(targetPos, _arrowMoveDuration).SetEase(Ease.Linear));

        seq.Join(arrow.DOFade(1f, 0.2f));
        seq.Insert(_arrowMoveDuration - 0.3f, arrow.DOFade(0f, 0.3f));
    }
    private void StopArrowRain()
    {
        _arrowLoopSequence?.Kill();
    }

    private void ResetVisuals()
    {
        _levelUpText.alpha = 0f;
        _statPointBonusText.alpha = 0f;
        _talentPointBonusText.alpha = 0f;
        _levelUpText.transform.localScale = Vector3.zero;
        _statPointBonusText.transform.localScale = Vector3.zero;

    }
}

