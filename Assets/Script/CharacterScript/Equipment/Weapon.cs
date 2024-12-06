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


    /// <summary>
    /// Takes a damage reference and randomly calculates its damage between 70% and 100%
    /// </summary>
    /// <returns></returns>
    public void GetDamageRange(ref Damage damage) 
    {
        foreach (var kvp in damage.damagePortion) 
        {
            damage.damagePortion[kvp.Key] = (int)(damage.damagePortion[kvp.Key]*Random.Range(0.7f, 1f));
        }
    }

    public Weapon() { }

    /// <summary>
    /// Constructor that should be called when creating a weapon
    /// </summary>
    /// <param name="name"></param>
    /// <param name="description"></param>
    /// <param name="id"></param>
    /// <param name="eType"></param>
    /// <param name="wType"></param>
    /// <param name="wWeight"></param>
    public Weapon(string name, string description, int id, WeaponType wType, WeaponWeight wWeight) : base(name, description, id)
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
    /// <param name="raw">If you want the true max damage, no randomization, set this to true</param>
    /// <returns>The calculated damage</returns>
    public Damage GetWeaponDamage(in Humanoid humanoid, bool raw = false)
    {
        // No damage for shields or general equipment
        if (WeaponType == WeaponType.Shield) return new Damage(null, null);


        foreach (var flatdam in EquipmentValue) //  For every Damage type we have
        {
            foreach (var scalingMultiplier in WeaponScaling[flatdam.Key])    //  For every scaling type that 
            {
                EquipmentValue[flatdam.Key] += (int)(humanoid.GetStat(scalingMultiplier.Key) * scalingMultiplier.Value); //  set new value to scaled value
            }
            EquipmentValue[flatdam.Key] = (int)(EquipmentValue[flatdam.Key]*(WeaponWeight == WeaponWeight.TwoHand ? 1.75f : 1.0f));
        }

        var dam = new Damage(EquipmentValue, EquipmentPercent);

        if (!raw) GetDamageRange(ref dam);

        // Calculate final damage
        return dam;
    }

    /// <summary>
    /// Randomizes the properties of the weapon
    /// </summary>
    public override void Randomize()
    {
        base.Randomize();

        foreach (var flatdam in EquipmentValue) //  For every Damage type we have
        {
            foreach (var scalingMultiplier in WeaponScaling[flatdam.Key])    //  For every scaling type that 
            {
                WeaponScaling[flatdam.Key][scalingMultiplier.Key] = WeaponScaling[flatdam.Key][scalingMultiplier.Key] * Random.Range(0.75f, 1.25f); //  set new value to scaled value
            }
        }
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

