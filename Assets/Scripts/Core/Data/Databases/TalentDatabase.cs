using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Database/Talent Database")]
public class TalentDatabase : ScriptableObject
{
    public List<TalentSO> AllTalents;

    private Dictionary<string, TalentSO> _cache;

    public void Init()
    {
        _cache = new Dictionary<string, TalentSO>();
        foreach (var talent in AllTalents)
        {
            _cache[talent.ID] = talent;
        }
    }

    public TalentSO GetById(string id)
    {
        return _cache.TryGetValue(id, out var talent) ? talent : null;
    }
}
