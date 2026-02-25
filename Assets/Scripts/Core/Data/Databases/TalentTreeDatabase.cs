using Assets.Scripts.Core.Enums;
using Assets.Scripts.Runtime.UI;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Database/TalentTree Database")]
public class TalentTreeDatabase : ScriptableObject
{
    public List<GameObject> AllTalentTrees;

    private Dictionary<PlayerClass, GameObject> _cache;

    public void Init()
    {
        _cache = new Dictionary<PlayerClass, GameObject>();
        foreach (var talentTree in AllTalentTrees)
        {
            var treeUI = talentTree.GetComponent<TalentTreeUI>();

            _cache[treeUI.GetTalentTreeClass()] = talentTree;
        }
    }

    public GameObject GetByClass(PlayerClass playerClass)
    {
        return _cache.TryGetValue(playerClass, out var talentTree) ? talentTree : null;
    }
}

