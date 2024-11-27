using UnityEngine;

/// <summary>
/// Armor can have flat defenses along with low resistence values for damage subtypes
/// </summary>
/// 
[System.Serializable]
public class Armor : Equipment
{
    ArmorType ArmorType;


    /// <summary>
    /// Returns the flat value associated with this damage type
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public int GetFlatDefense(DamageType type) 
    {
        return EquipmentValue.ContainsKey(type) ? EquipmentValue[type] : 0;
    }

    /// <summary>
    /// Returns the percent value associated with this resistance
    /// This should be additive with other similar effects
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public float GetResistance(DamageSubType type)
    {
        return EquipmentPercent.ContainsKey(type) ? EquipmentPercent[type] : 0f;
    }

    public Armor() { }

    public Armor(string name, string description, int id, ArmorType armortype) : base(name, description, id)
    {
        ArmorType = armortype;
    }

}

public enum ArmorType { Helmet, Chest, Leggings, Gloves, Boots }
