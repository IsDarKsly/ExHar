using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Any weapon
/// </summary>
/// 
[System.Serializable]
public class Weapon : Equipment
{
    public WeaponType WeaponType { get; set; } // Optional, depends on EquipmentType
    public WeaponWeight WeaponWeight { get; set; } // Optional, depends on WeaponType

    /// <summary>
    /// Represents the various physical boosting scaling factors a weapon has.
    /// Should really only be 2 stats at any given time
    /// </summary>
    public Dictionary<DamageType, Dictionary<STATS, float>> WeaponScaling = new Dictionary<DamageType, Dictionary<STATS, float>>();


    public Weapon() { }

    /// <summary>
    /// Constructor that should be called when creating a weapon
    /// </summary>
    /// <param name="name"></param>
    /// <param name="description"></param>
    /// <param name="wType"></param>
    /// <param name="wWeight"></param>
    public Weapon(string name, string description, WeaponType wType, WeaponWeight wWeight) : base(name, description)
    {
        WeaponScaling[DamageType.Physical] = new Dictionary<STATS, float>();
        WeaponScaling[DamageType.Magical] = new Dictionary<STATS, float>();

        WeaponType = wType;
        WeaponWeight = wWeight;
    }

    /// <summary>
    /// Constructor for weapons that enemies possess
    /// </summary>
    public Weapon(WeaponType wType, WeaponWeight wWeight)
    {
        WeaponScaling[DamageType.Physical] = new Dictionary<STATS, float>();
        WeaponScaling[DamageType.Magical] = new Dictionary<STATS, float>();

        WeaponType = wType;
        WeaponWeight = wWeight;
    }

    /// <summary>
    /// Calculates the damage dealt by the weapon.
    /// </summary>
    /// <param name="humanoid">The character wielding the weapon</param>
    /// <returns>The calculated damage</returns>
    public Damage GetWeaponDamage(in Humanoid humanoid)
    {
        // No damage for shields or general equipment
        if (WeaponType == WeaponType.Shield) return new Damage(null, null);

        //Our temporary new dictionary that will copy all necessary values for the new damage
        Dictionary<DamageType, int> tempDic = new Dictionary<DamageType, int>();

        foreach (var damagetype in EquipmentValue)  //  For every damage type this weapon has
        {
            tempDic[damagetype.Key] = damagetype.Value; //  Assign new value to the Temporary Dictionary
            foreach (var damagescaling in WeaponScaling[damagetype.Key])    //  For every stat scaling that this weapon has
            {
                tempDic[damagetype.Key] += (int)(tempDic[damagetype.Key] * WeaponScaling[damagetype.Key][damagescaling.Key]);   //  Append temporary dictionary balue
            }
            tempDic[damagetype.Key] = (int)(tempDic[damagetype.Key] * (WeaponWeight == WeaponWeight.TwoHand ? 1.75f : 1.0f));   //  Add bonus for two handed
        }

        var dam = new Damage(tempDic, EquipmentPercent);

        // Calculate final damage
        return dam;
    }

    /// <summary>
    /// Returns the flat value associated with this damage type
    /// ONLY IF THIS IS A SHIELD
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public int GetFlatDefense(DamageType type)
    {
        if (WeaponType != WeaponType.Shield) return 0;
        return EquipmentValue.ContainsKey(type) ? EquipmentValue[type] : 0;
    }

    /// <summary>
    /// ONLY IF THIS IS A SHIELD
    /// Returns the percent value associated with this resistance
    /// This should be additive with other similar effects
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public float GetResistance(DamageSubType type)
    {
        if (WeaponType != WeaponType.Shield) return 0;
        return EquipmentPercent.ContainsKey(type) ? EquipmentPercent[type] : 0f;
    }

}

public enum WeaponType { Shield, Sword, Hammer, Axe, Scythe, Dagger, Bow, Rapier, Staff, Wand, Grimoire }
public enum WeaponWeight { OneHand, TwoHand }

