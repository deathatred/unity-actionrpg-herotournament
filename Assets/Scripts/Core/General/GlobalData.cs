
using UnityEngine;

public static class GlobalData 
{
    #region Player
    public const float ROTATION_DURATION = 0.25f;
    public const float KNIGHT_ATTACK_RANGE = 1.8f;
    public const float MAGE_ATTACK_RANGE = 2f;
    #endregion
    #region BonusEffects
    public const string SELF_IGNITE_EFFECT = "SelfIgnite";
    public const string HOLY_HEAL_EFFECT = "HolyHeal";
    #endregion
    #region ObjectPool 
    public const int DEFAULT_PROJECTILE_POOL_SIZE = 50;
    #endregion
    #region Stats 
    public const int STR_TO_ATK_RATIO = 2;
    public const int STR_TO_HEALTH_RATIO = 10;
    public const int INT_TO_MANA_RATIO = 10;
    #endregion
    #region Spells 
    public const string DOUBLE_SLASH = "Double Slash";
    public const float SUMMON_RADIUS = 2f;
    #endregion
    #region SpellsValues 
    public const int HOLY_HEAL_AMOUNT = 120;
    #endregion
    #region Inventory 
    public const int DEFAULT_INVENTORY_SIZE = 20;
    #endregion
}
