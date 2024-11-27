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
    public SerializableDictionary<STATS, int> StatBonuses { get; set; } = new SerializableDictionary<STATS, int>();
    public SerializableDictionary<STATS, float> StatMultipliers { get; set; } = new SerializableDictionary<STATS, float>();

    /// <summary>
    /// A flat number used to deal or reduce physical/magical damage
    /// </summary>
    public SerializableDictionary<DamageType, int> EquipmentValue { get; set; } = new SerializableDictionary<DamageType, int>();

    /// <summary>
    /// A Percentage used to resist a percentage of damage, or determine what
    /// </summary>
    public SerializableDictionary<DamageSubType, float> EquipmentPercent { get; set; } = new SerializableDictionary<DamageSubType, float>();

    public int GetStatBonus(STATS stat) => StatBonuses.ContainsKey(stat) ? StatBonuses[stat] : 0;

    public float GetMultiplier(STATS stat) => StatMultipliers.ContainsKey(stat) ? StatMultipliers[stat] : 1f;

    public Equipment() { }

    public Equipment(string name, string description, int id)
        : base(name, description, id)
    {

    }

    /// <summary>
    /// When this equipment is custom generated, it will randomize its existing fields
    /// </summary>
    public virtual void Randomize() 
    {
        foreach (var bonus in StatBonuses) 
        {
            StatBonuses[bonus.Key] = (int)(StatBonuses[bonus.Key] * Random.Range(0.75f, 1.25f));
        }
        foreach (var bonus in StatMultipliers)
        {
            StatMultipliers[bonus.Key] = (StatMultipliers[bonus.Key] * Random.Range(0.75f, 1.25f));
        }
        foreach (var bonus in EquipmentValue)
        {
            EquipmentValue[bonus.Key] = (int)(EquipmentValue[bonus.Key] * Random.Range(0.75f, 1.25f));
        }
        foreach (var bonus in EquipmentPercent)
        {
            EquipmentPercent[bonus.Key] = (EquipmentPercent[bonus.Key] * Random.Range(0.75f, 1.25f));
        }
    }

    /// <summary>
    /// Gets the corresponding multiplier for a rarity
    /// </summary>
    /// <param name="_rarity"></param>
    /// <returns></returns>
    public float GetRarityMultiplier(RARITY _rarity) 
    {
        if (_rarity == RARITY.Legendary) _rarity = RARITY.Epic;

        float multiplier = 1f;

        switch (RARITY)
        {
            case RARITY.Epic:
                multiplier = 1.5f;
                break;
            case RARITY.Rare:
                multiplier = 1.25f;
                break;
            case RARITY.Uncommon:
                multiplier = 1.1f;
                break;
        }

        return multiplier;
    }

    /// <summary>
    /// Applies a rarity to an item, which boosts its stats.
    /// Generated items cannot be legendary, and are demoted to epic
    /// </summary>
    public virtual void SetRarity(RARITY _rarity) 
    {
        if (_rarity == RARITY.Legendary) _rarity = RARITY.Epic;

        RARITY = _rarity;

        foreach (var bonus in StatBonuses)
        {
            StatBonuses[bonus.Key] = (int)(StatBonuses[bonus.Key] * GetRarityMultiplier(RARITY));
        }
        foreach (var bonus in StatMultipliers)
        {
            StatMultipliers[bonus.Key] = (StatMultipliers[bonus.Key] * GetRarityMultiplier(RARITY));
        }
        foreach (var bonus in EquipmentValue)
        {
            EquipmentValue[bonus.Key] = (int)(EquipmentValue[bonus.Key] * GetRarityMultiplier(RARITY));
        }
        foreach (var bonus in EquipmentPercent)
        {
            EquipmentPercent[bonus.Key] = (EquipmentPercent[bonus.Key] * GetRarityMultiplier(RARITY));
        }

    }

}

/// <summary>
/// Any class that attempts to deal any kind of damage must send the calculated values
/// in the damage struct.
/// </summary>
public struct Damage 
{
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
    /// Constructor for this 
    /// </summary>
    /// <param name="t_dam"></param>
    /// <param name="t_per"></param>
    public Damage(Dictionary<DamageType, int> t_dam, Dictionary<DamageSubType, float> t_per) 
    {
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
public enum DamageSubType { Crushing, Slashing, Stabbing, Fire, Water, Ice, Lightning, Light, Dark }

