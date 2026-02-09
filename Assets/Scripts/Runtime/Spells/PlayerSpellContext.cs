using UnityEngine;

public class PlayerSpellContext
{
    public IHealthSystem Self;
    public Transform EnemyTransform;
    public IHealthSystem EnemyHealthSystem;
    public PlayerStats Stats;
    public PlayerSpellCasting SpellCasting;
    public PlayerAudio Audio;
    public EnemyStatusEffectsManager EnemyStatusEffectsManager;
    public PlayerSpellContext(IHealthSystem self, Transform enemyTransform,
        IHealthSystem enemyHealthSystem, PlayerStats stats, PlayerSpellCasting spellCasting, PlayerAudio audio, 
        EnemyStatusEffectsManager enemyStatusEffectsManager)
    {
        Self = self;
        EnemyTransform = enemyTransform;
        EnemyHealthSystem = enemyHealthSystem;
        Stats = stats;
        SpellCasting = spellCasting;
        Audio = audio;
        EnemyStatusEffectsManager = enemyStatusEffectsManager;
    }
}
