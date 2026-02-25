using Assets.Scripts.Runtime.SOScripts.SpellSOs;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Runtime.Managers
{
    public class GlobalSpellbook : MonoBehaviour
    {
        public static GlobalSpellbook Instance;
        [SerializeField] private List<SpellSO> _allSpells;

        private Dictionary<string, SpellSO> _spellDict;
        private void Awake()
        {
            InitSingleton();
            InitSpellDictionary();
        }
        private void InitSingleton()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        private void InitSpellDictionary()
        {
            _spellDict = new Dictionary<string, SpellSO>();

            foreach (var spell in _allSpells)
            {
                if (!_spellDict.ContainsKey(spell.Name))
                    _spellDict.Add(spell.Name, spell);
            }
        }
        public SpellSO GetSpellByName(string name)
        {
            if (_spellDict.TryGetValue(name, out var spell))
                return spell;

            Debug.LogWarning($"Spell {name} not found in general spellbook!");
            return null;
        }
    }
}