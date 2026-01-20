using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class CharacterCreationViewUI : MonoBehaviour
{
    [SerializeField] private Button _wizardClassButton;
    [SerializeField] private Button _knightClassButton;
    private EventBus _eventBus;
    [Inject]
    private void Construct(EventBus eventBus)
    {
        _eventBus = eventBus;
    }
    private void OnEnable()
    {
        BindButtons();
    }
    private void OnDisable()
    {
        UnbindButtons();
    }
    private void BindButtons()
    {
        _wizardClassButton.onClick.AddListener(SelectWizardClass);
        _knightClassButton.onClick.AddListener(SelectKnightClass);
    }
    private void UnbindButtons()
    {
        _wizardClassButton.onClick.RemoveListener(SelectWizardClass);
        _knightClassButton.onClick.RemoveListener(SelectKnightClass);
    }
    private void SelectWizardClass()
    {
        _eventBus.Publish(new ClassSelectedEvent(PlayerClass.Wizard));
    }
    private void SelectKnightClass()
    {
        _eventBus.Publish(new ClassSelectedEvent(PlayerClass.Knight));
    }
}
