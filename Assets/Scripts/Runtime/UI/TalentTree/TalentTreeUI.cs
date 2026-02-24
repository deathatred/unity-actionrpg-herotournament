using Assets.Scripts.Core.Enums;
using Assets.Scripts.Core.Observer;
using Firebase.Database;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class TalentTreeUI : MonoBehaviour
{
    [Inject] private EventBus _eventBus;
    [SerializeField] private Image _talentTreeBackground;
    [SerializeField] private TextMeshProUGUI _talentPointsAmountText;
    [SerializeField] private TextMeshProUGUI _specNameText;
    [SerializeField] private List<TalentContainerSingleUI> _talents;
    [SerializeField] private PlayerClass _talentTreeClass;
    [SerializeField] private Button _backButton;
 

    private void OnEnable()
    {
        SubscribeToEvents();   
    }
    private void OnDisable()
    {
        UnsubscribeFromEvents();
    }
    private void SubscribeToEvents()
    {
        _eventBus.Subscribe<PlayerSpecChosenEvent>(BlockBranch);
    }
    private void UnsubscribeFromEvents()
    {
        _eventBus.Unsubscribe<PlayerSpecChosenEvent>(BlockBranch);
    }
    private void BlockBranch(PlayerSpecChosenEvent e)
    {
        LockOppositeBranch(e.Spec);
    }
    public void SetSpellPointsAmountText(int amount)
    {
       _talentPointsAmountText.text = amount.ToString();
    }
    public void SetSpecName(string name)
    {       
        _specNameText.text = name; 
    }
    public void SetSpecBackground(Sprite background)
    {
        _talentTreeBackground.sprite = background;
    }
    public TextMeshProUGUI GetSpellPointsAmountText()
    {
        return _talentPointsAmountText;
    }
    public void InitTalentsTalentTreeView(TalentTreeViewUI view)
    {
        foreach (var talent in _talents)
        {
            talent.SetTalentTreeView(view);
        }
    }
    private void LockOppositeBranch(ClassSpecSO spec)
    {
        foreach (var talentContainer in _talents)
        {
            var talentSpec = talentContainer.GetTalentSO().Spec;

            if (talentSpec == spec.SpecsClass.DefaultSpec)
                continue;

            if (talentSpec != spec.Spec)
            {
                talentContainer.SetChainsActive();
            }
        }
    }
    public List<TalentContainerSingleUI> GetTalentContainersFromTalentTree()
    {
        List<TalentContainerSingleUI> res = new List<TalentContainerSingleUI>();
        foreach (var talentContainer in _talents)
        {
            res.Add(talentContainer);
        }
        return res;
    }
    public Button GetBackButton()
    {
        return _backButton;
    }
    public PlayerClass GetTalentTreeClass()
    {
        return _talentTreeClass;
    }
}
