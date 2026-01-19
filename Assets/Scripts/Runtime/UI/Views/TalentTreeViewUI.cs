using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class TalentTreeViewUI : MonoBehaviour
{
    [Inject] private PlayerTalentSystem _playerTalentSystem;
    [Inject] private EventBus _eventBus;

    [SerializeField] private Canvas _canvas;
    [SerializeField] private Button _backButton;
    [SerializeField] private GameObject _talentContextMenuContainer;
    [SerializeField] private TextMeshProUGUI _talentNameText;
    [SerializeField] private TextMeshProUGUI _talentAboutText;
    [SerializeField] private Button _learnButton;
    [SerializeField] private Button _closeButton;
    [SerializeField] private TalentTreeUI _currentTalentTree;

    private TalentContainerSingleUI _currentlyChosenTalentContainer;

    private void OnEnable()
    {
        BindButtons();
        SubscribeToEvents();
    }
    private void Start()
    {
        InitSpecInfo();
    }
    private void OnDisable()
    {
        UnbindButtons();
        UnsubscribeFromEvents();
        CloseContextMenu();
    }
    private void BindButtons()
    {
        _backButton.onClick.AddListener(BackPress);
        _closeButton.onClick.AddListener(CloseContextMenuPressed);
    }
    private void UnbindButtons()
    {
        _backButton.onClick.RemoveListener(BackPress);
        _closeButton.onClick.RemoveListener(CloseContextMenuPressed);
    }
    private void BackPress()
    {
        _eventBus.Publish(new BackButtonPressedEvent(BackButtonCaller.TalentTree));
    }
    private void CloseContextMenuPressed()
    {
        CloseContextMenu();
    }
    private void SubscribeToEvents()
    {
        _eventBus.Subscribe<TalentContainerPressedEvent>(ShowTalentContextMenu);
        _eventBus.Subscribe<TalentPointChangedEvent>(ChangeSpellPointsAmount);
        _eventBus.Subscribe<TalentPointsChangedEvent>(ChangeTalentPointAmount);
        _eventBus.Subscribe<PlayerSpecChosenEvent>(ChangeTalentTreeVisual);
        _eventBus.Subscribe<PlayerDataLoadedEvent>(RestoreTalentTree);
    }
    private void UnsubscribeFromEvents()
    {
        _eventBus.Unsubscribe<TalentContainerPressedEvent>(ShowTalentContextMenu);
        _eventBus.Unsubscribe<TalentPointChangedEvent>(ChangeSpellPointsAmount);
        _eventBus.Unsubscribe<TalentPointsChangedEvent>(ChangeTalentPointAmount);
        _eventBus.Unsubscribe<PlayerSpecChosenEvent>(ChangeTalentTreeVisual);
        _eventBus.Unsubscribe<PlayerDataLoadedEvent>(RestoreTalentTree);
    }
    private void ShowTalentContextMenu(TalentContainerPressedEvent e)
    {
        SetContextMenuToTalent(e.TalentSO);
    }
    private void ChangeSpellPointsAmount(TalentPointChangedEvent e)
    {
        _currentTalentTree.GetSpellPointsAmountText().text = e.TalentPoints.ToString();
    }
    private void ChangeTalentPointAmount(TalentPointsChangedEvent e)
    {
        _currentTalentTree.SetSpellPointsAmountText(e.TalentPoints);
    }
    private void ChangeTalentTreeVisual(PlayerSpecChosenEvent e)
    {
        _currentTalentTree.SetSpecBackground(e.Spec.Background);
        _currentTalentTree.SetSpecName(e.Spec.SpecName);
    }
    private void RestoreTalentTree(PlayerDataLoadedEvent e)
    {
        RestoreTalentUI(e.TalentSaveData);
    }
    private void SetContextMenuToTalent(TalentSO talentSO)
    {
        if (talentSO.Spell != null)
        {
            _talentNameText.text = talentSO.Spell.Name;
            _talentAboutText.text = talentSO.Spell.About;
        }
        else
        {
            _talentNameText.text = talentSO.TalentName;
            _talentAboutText.text = talentSO.Description;
        }
        _learnButton.interactable = _playerTalentSystem.CanLearn(talentSO);
        _learnButton.onClick.RemoveAllListeners();
        _learnButton.onClick.AddListener(() => LearnTalent(talentSO));
        MoveContextMenuToPointer(_talentContextMenuContainer);
        _talentContextMenuContainer.gameObject.SetActive(true);
    }
    private void LearnTalent(TalentSO talentSO)
    {
        _eventBus.Publish(new LearnButtonPressedEvent(talentSO));
        _currentlyChosenTalentContainer?.AddLevelToSpell();
        _learnButton.interactable = _playerTalentSystem.CanLearn(talentSO);
    }
    private void CloseContextMenu()
    {
        _talentContextMenuContainer.gameObject.SetActive(false);
    }
    private void MoveContextMenuToPointer(GameObject menu)
    {
        RectTransform canvasRect = (RectTransform)_canvas.transform;
        RectTransform rect = menu.GetComponent<RectTransform>();
        Vector2 screenPos =Input.touchCount > 0? Input.GetTouch(0).position: Input.mousePosition;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            screenPos,
            _canvas.worldCamera,
            out Vector2 localPoint
        );

        float offsetX = 250f;
        float offsetY = -350f;
        Vector2 targetPos = localPoint + new Vector2(offsetX, offsetY);
        Vector2 size = rect.rect.size;

        float halfW = canvasRect.rect.width / 2f;
        float halfH = canvasRect.rect.height / 2f;
        if (targetPos.x + size.x * (1f - rect.pivot.x) > halfW)
            offsetX = -Mathf.Abs(offsetX);

        if (targetPos.y - size.y * rect.pivot.y < -halfH)
            offsetY = Mathf.Abs(offsetY);
        targetPos = localPoint + new Vector2(offsetX, offsetY);
        float minX = -halfW + size.x * rect.pivot.x;
        float maxX = halfW - size.x * (1f - rect.pivot.x);
        float minY = -halfH + size.y * rect.pivot.y;
        float maxY = halfH - size.y * (1f - rect.pivot.y);

        targetPos.x = Mathf.Clamp(targetPos.x, minX, maxX);
        targetPos.y = Mathf.Clamp(targetPos.y, minY, maxY);

        rect.anchoredPosition = targetPos;
    }
    private void InitSpecInfo()
    {
        string specName = _playerTalentSystem.GetCurrentClasSpec() == null ?
            _playerTalentSystem.GetCurrentClass().DefaultSpec.ToString() : _playerTalentSystem.GetCurrentClasSpec().Spec.ToString();
        _currentTalentTree.SetSpecName(specName);
        _currentTalentTree.SetSpellPointsAmountText(_playerTalentSystem.TalentPoints);
    }
    private void RestoreTalentUI(TalentSaveData[] data)
    {
        if (data == null)
        {
            return;
        }
        var containers = _currentTalentTree.GetTalentContainersFromTalentTree();

        var saveLookup = new Dictionary<string, TalentSaveData>();
        for (int i = 0; i < data.Length; i++)
        {
            saveLookup[data[i].ID] = data[i];
        }

        for (int i = 0; i < containers.Count; i++)
        {
            var id = containers[i].GetTalentSO().ID;

            if (!saveLookup.TryGetValue(id, out var save))
                continue;

            for (int level = 0; level < save.Level; level++)
            {
                containers[i].AddLevelToSpell();
            }
        }
    }
    public void SetSpellContainerChosen(TalentContainerSingleUI spellContainer)
    {
        _currentlyChosenTalentContainer = spellContainer;
    }
}
