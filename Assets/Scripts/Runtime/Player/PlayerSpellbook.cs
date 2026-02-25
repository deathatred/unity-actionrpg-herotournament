using Assets.Scripts.Core.Observer;
using Assets.Scripts.Runtime.Events.PlayerSpellCastEvent;
using Assets.Scripts.Runtime.Managers;
using Assets.Scripts.Runtime.SOScripts.SpellSOs;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Runtime.Player
{
    public class PlayerSpellbook : MonoBehaviour
    {
        [Inject] private EventBus _eventBus;
        private Dictionary<string, SpellSO> _knownSpellsDict;

        private void Awake()
        {
            _knownSpellsDict = new Dictionary<string, SpellSO>();

        }
        public void UnlockSpell(string name)
        {
            var spell = GlobalSpellbook.Instance.GetSpellByName(name);
            if (spell != null)
            {
                _knownSpellsDict[name] = spell;
                _eventBus.Publish(new PlayerSpellUnlockedEvent(spell));
            }
            else
            {
                Debug.LogWarning($"Cannot unlock {name}: spell not found in general spellbook!");
            }
        }
        public SpellSO GetUnlockedSpellByName()
        {
            if (_knownSpellsDict.TryGetValue(name, out var spell))
                return spell;

            Debug.LogWarning($"Spell {name} not found in general spellbook!");
            return null;
        }
        public bool IsSpellUnlocked(string name)
        {
            return _knownSpellsDict.ContainsKey(name);
        }
    }
}