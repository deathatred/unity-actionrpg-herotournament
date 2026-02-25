using Assets.Scripts.Runtime.SOScripts.SpellSOs;
using Cysharp.Threading.Tasks;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Runtime.UI
{
    public class SpellHotkeysUI : MonoBehaviour
    {
        [SerializeField] private Image _lockImage;
        [SerializeField] private Image _spellImage;
        [SerializeField] private Image _cooldownImage;
        [SerializeField] private TextMeshProUGUI _spellHotkey;
        [SerializeField] private TextMeshProUGUI _cooldownText;

        public bool IsSet { get; private set; } = false;

        private SpellSO _settedSpell;
        private CancellationTokenSource _cts;


        public void SetSpell(SpellSO SO)
        {
            _lockImage.enabled = false;
            _spellHotkey.gameObject.SetActive(true);
            _spellImage.sprite = SO.Icon;
            _spellImage.color = Color.white;
            _settedSpell = SO;
            IsSet = true;
        }
        public void TryStartCooldown(SpellSO spell)
        {
            if (spell == _settedSpell)
            {
                StartCooldown(spell.Cooldown).Forget();
            }
        }
        private async UniTask StartCooldown(float cooldown)
        {
            _cts?.Cancel();
            _cts = new CancellationTokenSource();

            float timeLeft = cooldown;
            _cooldownImage.fillAmount = 1f;
            _cooldownImage.gameObject.SetActive(true);
            while (timeLeft > 0f)
            {
                timeLeft -= Time.deltaTime;
                _cooldownImage.fillAmount = timeLeft / cooldown;
                _cooldownText.text = Mathf.Ceil(timeLeft).ToString();

                await UniTask.Yield(PlayerLoopTiming.Update, _cts.Token);
            }
            _cooldownImage.fillAmount = 0f;
            _cooldownText.text = "";
            _cooldownImage.gameObject.SetActive(false);

        }
    }
}