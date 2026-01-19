using DG.Tweening;
using TMPro;
using UnityEngine;

public class LoadingViewUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _loadingText;

    private Tween _loadingTween;

    private void OnEnable()
    {
        AnimateLoadingText();
    
    }

    private void OnDisable()
    {
        _loadingTween?.Kill();
    }

    private void AnimateLoadingText()
    {
        string baseText = "Loading";
        int dotCount = 0;

        _loadingTween = DOVirtual.DelayedCall(0.4f, () =>
        {
            dotCount = (dotCount + 1) % 4;
            _loadingText.text = baseText + new string('.', dotCount);
        })
        .SetLoops(-1, LoopType.Restart);
    }
}

