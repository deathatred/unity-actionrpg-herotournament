using Assets.Scripts.Core.Observer;
using Assets.Scripts.Runtime.SOScripts;
using Assets.Scripts.Runtime.UI.Views;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Scripts.Runtime.UI
{
    public class TalentContainerSingleUI : MonoBehaviour
    {
        [Inject] private EventBus _eventBus;
        [SerializeField] private TalentSO _talentSO;
        [SerializeField] private Image _talentImage;
        [SerializeField] private Button _talentButton;
        [SerializeField] private TextMeshProUGUI _talentPointsText;
        [SerializeField] private TextMeshProUGUI _talentPointsMaxText;
        [SerializeField] private GameObject _chainsObject;

        private TalentTreeViewUI _talentTreeViewUI;
        private int _currentTalentPoints;
        private int _maxTalentPoints;

        private void OnEnable()
        {
            BindButtons();
        }
        private void Awake()
        {
            SetImage();
            SetMaxLevel();
        }
        private void OnDisable()
        {
            UnbindButtons();
        }
        private void BindButtons()
        {
            _talentButton.onClick.AddListener(SpellContainerButtonPressed);
        }
        private void UnbindButtons()
        {
            _talentButton.onClick.RemoveListener(SpellContainerButtonPressed);
        }
        private void SpellContainerButtonPressed()
        {
            _talentTreeViewUI.SetSpellContainerChosen(this);
            _eventBus.Publish(new TalentContainerPressedEvent(_talentSO));
        }
        private void SetImage()
        {
            if (_talentSO.Spell != null)
            {
                _talentImage.sprite = _talentSO.Spell.Icon;
            }
            else
            {
                _talentImage.sprite = _talentSO.Icon;
            }
        }
        private void SetMaxLevel()
        {
            _maxTalentPoints = _talentSO.MaxLevel;
            _talentPointsMaxText.text = $"/{_talentSO.MaxLevel}";
        }
        public void AddLevelToSpell()
        {
            if (_currentTalentPoints + 1 > _maxTalentPoints)
            {
                return;
            }
            _currentTalentPoints++;
            _talentPointsText.text = _currentTalentPoints.ToString();
        }
        public void SetChainsActive()
        {
            _chainsObject.SetActive(true);
        }
        public void SetTalentTreeView(TalentTreeViewUI view)
        {
            _talentTreeViewUI = view;
        }
        public TalentSO GetTalentSO()
        {
            return _talentSO;
        }
    }
}