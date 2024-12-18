using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The Equipment class represents any equipment that can be placed on a character
/// Each equipment can have stat bonuses and stat multipliers
/// </summary>
/// 
[System.Serializable]
public class Equipment : Item
{
    /// <summary>
    /// Flat Bonuses applied to the equipped characters stats
    /// </summary>
    public Dictionary<STATS, int> StatBonuses { get; set; } = new Dictionary<STATS, int>();

    /// <summary>
    /// Multiplicative Bonuses applied to the equipped characters stats
    /// </summary>
    public Dictionary<STATS, float> StatMultipliers { get; set; } = new Dictionary<STATS, float>();

    /// <summary>
    /// A flat number used to deal or reduce physical/magical damage
    /// </summary>
    public Dictionary<DamageType, int> EquipmentValue { get; set; } = new Dictionary<DamageType, int>();

    /// <summary>
    /// A Percentage used to resist a percentage of damage, or determine what percent of damage is being dealt as a subtype
    /// </summary>
    public Dictionary<DamageSubType, float> EquipmentPercent { get; set; } = new Dictionary<DamageSubType, float>();

    /// <summary>
    /// The equipment slot this item will occupy
    /// </summary>
    public EQUIPMENTSLOT equipmentslot;

    /// <summary>
    /// The passives on this piece of equipment
    /// </summary>
    public List<PassiveTalents> passives { get; set; } = new List<PassiveTalents>();

    public int GetStatBonus(STATS stat) => StatBonuses.ContainsKey(stat) ? StatBonuses[stat] : 0;

    public float GetMultiplier(STATS stat) => StatMultipliers.ContainsKey(stat) ? StatMultipliers[stat] : 0f;

    /// <summary>
    /// Whether a piece of equipment is currently equipped or not
    /// </summary>
    public bool IsEquipped { get; set; } = false;

    public Equipment() { }

    public Equipment(string name, string description)
        : base(name, description)
    {

    }

    /// <summary>
    /// This should equip or unequip this item
    /// </summary>
    /// <param name="target"></param>
    /// <exception cref="System.NotImplementedException"></exception>
    public override bool Use(Humanoid target)
    {
        var tup = target.IsEquipped(this);
        if (tup.Item1)  //  This item is equipped, so we unequip
        {
            target.UnequipSlot(tup.Item2);
            return true;
        }

        if (target.HasAvailableSlot(this))  //  If the character has an available slot to equip this to 
        {
            target.EquipSlot(equipmentslot, this);  //  This item is not equipped, so we equip
            return true;
        }

        return false;
    }

}

/// <summary>
/// Any class that attempts to deal any kind of damage must send the calculated values
/// in the damage struct.
/// </summary>
public struct Damage 
{
    /// <summary>
    /// Whether or not this damage is critical
    /// Default to false
    /// </summary>
    public bool IsCritical;

    /// <summary>
    /// Whether this damage can be dodged or not
    /// defaults to True (Most attacks can be dodged)
    /// </summary>
    public bool IsDodgeable;

    /// <summary>
    /// This represents the exact portion of damage
    /// </summary>
    public Dictionary<DamageType, int> damagePortion;

    /// <summary>
    /// This represents the exact proportion of damage types that can be dealt.
    /// These should always add up to 1 (IE. 0.7 slashing, 0.3 fire means a weapon does 70% slashing, 30% fire damage)
    /// </summary>
    public Dictionary<DamageSubType, float> damagePercent;

    /// <summary>
    /// The constructor takes a Dictionary of flat damages and a dictionary of percentages. It can also take whether this damage is dodgeable (for players only) or if this damage is a critical strike (for use by the animator)
    /// </summary>
    /// <param name="t_dam"></param>
    /// <param name="t_per"></param>
    public Damage(Dictionary<DamageType, int> t_dam, Dictionary<DamageSubType, float> t_per, bool iscrit = false, bool candodge = true) 
    {
        IsDodgeable = candodge;
        IsCritical = iscrit;
        damagePortion = t_dam;
        damagePercent = t_per;
    }

}

/// <summary>
/// The different types of damage a weapon or spell can deal
/// </summary>
public enum DamageType { Physical, Magical }

/// <summary>
/// The different subtypes types of damage a weapon or spell can deal percentages of
/// </summary>
public enum DamageSubType { Crushing, Slashing, Stabbing, Fire, Water, Ice, Lightning, Light, Dark, Bleeding, Poison }

