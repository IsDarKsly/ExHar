using UnityEngine;

/// <summary>
/// Jewelry cannot have flat defenses, but more commonly have percent resitances to 
/// non physical subtypes of damage. They also more commonly have passive skills attached to them
/// </summary>
/// 
[System.Serializable]
public class Jewelry : Equipment
{
    JeweleryType JeweleryType;

    public Jewelry() { }

    public Jewelry(string name, string description, int id, JeweleryType jeweleryType) : base(name, description, id)
    {
        JeweleryType = jeweleryType;
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

}

public enum JeweleryType { Amulet, Ring }