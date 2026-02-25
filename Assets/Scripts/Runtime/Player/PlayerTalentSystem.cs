using Assets.Scripts.Core.Data;
using Assets.Scripts.Core.Observer;
using Assets.Scripts.Runtime.Enums;
using Assets.Scripts.Runtime.Events;
using Assets.Scripts.Runtime.SOScripts;
using Assets.Scripts.Runtime.SOScripts.ClassSO;
using Assets.Scripts.Runtime.UI.UIEvents;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Runtime.Player
{
    public class PlayerTalentSystem : MonoBehaviour
    {
        public int TalentPoints { get; private set; } = 1;

        [Inject] private TalentDatabase _talentDatabase;
        [Inject] private EventBus _eventBus;
        [Inject] private PlayerSpellbook _playerSpellbook;
        [Inject] private PlayerStats _playerStats;

        private PlayerClassSO _currentClass;
        private ClassSpecSO _currentClassSpec;

        private Dictionary<TalentSO, int> _learnedTalents = new();


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
            _eventBus.Subscribe<LearnButtonPressedEvent>(LearnTalentSubscriber);
        }
        private void UnsubscribeFromEvents()
        {
            _eventBus.Unsubscribe<LearnButtonPressedEvent>(LearnTalentSubscriber);
        }
        private void LearnTalentSubscriber(LearnButtonPressedEvent e)
        {
            if (TalentPoints > 0)
            {
                if (e.TalentSO.Prerequisites.Length >= 1)
                {
                    foreach (var talent in e.TalentSO.Prerequisites)
                    {
                        if (_learnedTalents.ContainsKey(talent))
                        {
                            LearnTalent(e.TalentSO);
                            return;
                        }
                    }
                }
                else
                {
                    LearnTalent(e.TalentSO);
                }
            }
        }
        private void LearnTalent(TalentSO talentSO)
        {
            bool firstTimeLearned = !_learnedTalents.ContainsKey(talentSO);

            if (firstTimeLearned)
            {
                _learnedTalents.Add(talentSO, 1);
                ApplyTalentFirstTime(talentSO);
            }
            else
            {
                _learnedTalents[talentSO]++;
                ApplyTalentLevelUp(talentSO);
            }

            TalentPoints--;
            _eventBus.Publish(new TalentPointsChangedEvent(TalentPoints));

            if (_currentClassSpec == null && _currentClass.DefaultSpec != talentSO.Spec)
            {
                _currentClassSpec = _currentClass.GetSpecSO(talentSO.Spec);
                _eventBus.Publish(new PlayerSpecChosenEvent(_currentClassSpec));
            }
        }

        private void ApplyTalentFirstTime(TalentSO talent)
        {
            if (talent.SpellType == SpellType.Active)
            {
                _playerSpellbook.UnlockSpell(talent.Spell.Name);
            }

            if (talent.SpellType == SpellType.Passive)
            {
                foreach (var p in talent.PassiveEffects)
                    p.Apply(_playerStats);
            }
        }

        private void ApplyTalentLevelUp(TalentSO talent)
        {
            if (talent.SpellType == SpellType.Passive)
            {
                foreach (var p in talent.PassiveEffects)
                    p.Apply(_playerStats);
            }
        }
        private bool CheckBranch(TalentSO talent)
        {
            if (talent.Spec == _currentClass.DefaultSpec)
                return true;
            if (_currentClassSpec == null)
                return true;
            return _currentClassSpec.Spec == talent.Spec;
        }
        public void InitCurrentClass(PlayerClassSO currentClass)
        {
            _currentClass = currentClass;
        }
        public void AddTalentPoint()
        {
            TalentPoints++;
            _eventBus.Publish(new TalentPointChangedEvent(TalentPoints));
        }
        public bool CanLearn(TalentSO talent)
        {
            if (TalentPoints <= 0)
                return false;
            if (_learnedTalents.ContainsKey(talent) && _learnedTalents[talent] >= talent.MaxLevel)
                return false;
            if (talent.Prerequisites == null || talent.Prerequisites.Length == 0)
            {
                return CheckBranch(talent);
            }
            bool hasAnyPrerequisite = talent.Prerequisites.Any(req => _learnedTalents.ContainsKey(req));
            if (!hasAnyPrerequisite)
                return false;

            return CheckBranch(talent);
        }
        public TalentSaveData[] GetLearnedTalents()
        {
            List<TalentSaveData> res = new();
            foreach (var talent in _learnedTalents)
            {
                var data = new TalentSaveData();
                data.ID = talent.Key.ID;
                data.Level = talent.Value;
                res.Add(data);
            }
            return res.ToArray();
        }
        public PlayerClassSO GetCurrentClass()
        {
            return _currentClass;
        }
        public ClassSpecSO GetCurrentClasSpec()
        {
            return _currentClassSpec;
        }
        public void RestoreTalentPoints(int amount)
        {
            TalentPoints = amount;
            _eventBus.Publish(new TalentPointChangedEvent(TalentPoints));
        }
        public void RestoreTalents(TalentSaveData[] data)
        {
            if (data == null)
            {
                return;
            }
            _learnedTalents.Clear();
            _currentClassSpec = null;
            foreach (var t in data)
            {
                TalentSO talent = _talentDatabase.GetById(t.ID);
                if (talent == null) continue;

                for (int i = 0; i < t.Level; i++)
                {
                    RestoreTalent(talent);
                }

            }
        }
        private void RestoreTalent(TalentSO talentSO)
        {
            bool firstTimeLearned = !_learnedTalents.ContainsKey(talentSO);

            if (firstTimeLearned)
            {
                _learnedTalents.Add(talentSO, 1);
                ApplyTalentFirstTime(talentSO);
            }
            else
            {
                _learnedTalents[talentSO]++;
                ApplyTalentLevelUp(talentSO);

            }
            if (_currentClassSpec == null && _currentClass.DefaultSpec != talentSO.Spec)
            {
                _currentClassSpec = _currentClass.GetSpecSO(talentSO.Spec);
                _eventBus.Publish(new PlayerSpecChosenEvent(_currentClassSpec));
            }
        }
    }
}