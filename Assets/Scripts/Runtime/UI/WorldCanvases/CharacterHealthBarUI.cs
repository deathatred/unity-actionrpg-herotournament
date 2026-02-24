using Assets.Scripts.Core.Interfaces;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CharacterHealthBarUI : MonoBehaviour
{
    [SerializeField] private Canvas _healthBar;
    [SerializeField] private Image _healthBarImage;

    private CancellationTokenSource _cts;
    private IHealthSystem _health;

    private void Awake()
    {
        _health = GetComponentInParent<IHealthSystem>();
        _health.OnHealthChanged += Animate;
        _health.OnDeath += Hide;
    }

    private void OnDestroy()
    {
        _cts?.Cancel();
        _health.OnHealthChanged -= Animate;
        _health.OnDeath -= Hide;
    }

    private void Animate(int current, int max)
    {
        _cts?.Cancel();
        _cts = new CancellationTokenSource();

        AnimateAsync((float)current / max, _cts.Token).Forget();
    }
    private void Hide(object obj, EventArgs arg)
    {
        _healthBar.gameObject.SetActive(false);
    }
    private async UniTask AnimateAsync(float target, CancellationToken token)
    {
        float start = _healthBarImage.fillAmount;
        float time = 0f;
        float animDuration = 0.3f;
        while (time < animDuration)
        {
            token.ThrowIfCancellationRequested();

            time += Time.deltaTime;
            float t = time / animDuration;
            _healthBarImage.fillAmount = Mathf.Lerp(start, target, t);

            await UniTask.Yield();
        }

        _healthBarImage.fillAmount = target;
    }

}
