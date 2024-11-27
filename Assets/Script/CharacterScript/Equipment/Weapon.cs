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
    public SerializableDictionary<DamageType, SerializableDictionary<STATS, float>> WeaponScaling = new SerializableDictionary<DamageType, SerializableDictionary<STATS, float>>();


    /// <summary>
    /// Gets the lower bound of damage this weapon can do
    /// This is simplly 70% of the calculated damage
    /// </summary>
    /// <returns></returns>
    public int GetLowerBound() 
    {
        return 0;
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
        WeaponScaling[DamageType.Physical] = new SerializableDictionary<STATS, float>();
        WeaponScaling[DamageType.Magical] = new SerializableDictionary<STATS, float>();

        WeaponType = wType;
        WeaponWeight = wWeight;
    }

    /// <summary>
    /// Calculates the damage dealt by the weapon.
    /// </summary>
    /// <param name="humanoid">The character wielding the weapon</param>
    /// <returns>The calculated damage</returns>
    public Damage GetWeaponDamage(Humanoid humanoid)
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

        // Calculate final damage
        return new Damage(EquipmentValue, EquipmentPercent);
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

}

public enum WeaponType { Shield, Sword, Hammer, Axe, Scythe, Dagger, Bow, Rapier, Staff, Wand, Grimoire }
public enum WeaponWeight { OneHand, TwoHand }

