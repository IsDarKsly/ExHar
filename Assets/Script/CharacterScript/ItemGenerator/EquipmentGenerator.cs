using System.Collections.Generic;
using UnityEngine;

public static class EquipmentGenerator
{
    /// <summary>
    /// This list will contain all possible level 1 armors
    /// </summary>
    public static List<Armor> LV1Armors = new List<Armor>() 
    {
        
    };
    /// <summary>
    /// This list will contain all possible level 10 armors
    /// </summary>
    public static List<Armor> LV10Armors = new List<Armor>()
    {
       
    };
    /// <summary>
    /// This list will contain all possible level 20 armors
    /// </summary>
    public static List<Armor> LV20Armors = new List<Armor>()
    {
        
    };
    /// <summary>
    /// This list will contain all possible level 30 armors
    /// </summary>
    public static List<Armor> LV30Armors = new List<Armor>()
    {
        
    };
    /// <summary>
    /// This list will contain all possible level 40 armors
    /// </summary>
    public static List<Armor> LV40Armors = new List<Armor>()
    {
        
    };
    /// <summary>
    /// This list will contain all possible level 50 armors
    /// </summary>
    public static List<Armor> LV50Armors = new List<Armor>()
    {
       
    };

    /// <summary>
    /// This list will contain all possible level 1 Jewelry
    /// </summary>
    public static List<Jewelry> LV1Jewelry = new List<Jewelry>()
    {
        
    };
    /// <summary>
    /// This list will contain all possible level 10 Jewelry
    /// </summary>
    public static List<Jewelry> LV10Jewelry = new List<Jewelry>()
    {
        
    };
    /// <summary>
    /// This list will contain all possible level 20 Jewelry
    /// </summary>
    public static List<Jewelry> LV20Jewelry = new List<Jewelry>()
    {
       
    };
    /// <summary>
    /// This list will contain all possible level 30 Jewelry
    /// </summary>
    public static List<Jewelry> LV30Jewelry = new List<Jewelry>()
    {
       
    };
    /// <summary>
    /// This list will contain all possible level 40 Jewelry
    /// </summary>
    public static List<Jewelry> LV40Jewelry = new List<Jewelry>()
    {
        
    };
    /// <summary>
    /// This list will contain all possible level 50 Jewelry
    /// </summary>
    public static List<Jewelry> LV50Jewelry = new List<Jewelry>()
    {
       
    };

    /// <summary>
    /// This list will contain all possible level 1 Weapons
    /// </summary>
    public static List<Weapon> LV1Weapons = new List<Weapon>()
    {
        
    };
    /// <summary>
    /// This list will contain all possible level 10 Weapons
    /// </summary>
    public static List<Weapon> LV10Weapons = new List<Weapon>()
    {
        
    };
    /// <summary>
    /// This list will contain all possible level 20 Weapons
    /// </summary>
    public static List<Weapon> LV20Weapons = new List<Weapon>()
    {
       
    };
    /// <summary>
    /// This list will contain all possible level 30 Weapons
    /// </summary>
    public static List<Weapon> LV30Weapons = new List<Weapon>()
    {
        
    };
    /// <summary>
    /// This list will contain all possible level 40 Weapons
    /// </summary>
    public static List<Weapon> LV40Weapons = new List<Weapon>()
    {
        
    };
    /// <summary>
    /// This list will contain all possible level 50 Weapons
    /// </summary>
    public static List<Weapon> LV50Weapons = new List<Weapon>()
    {
       
    };


    /// <summary>
    /// Will return a list of all viable armors
    /// </summary>
    /// <param name="person"></param>
    /// <returns></returns>
    public static List<Armor> GetArmorList(Humanoid person) 
    {
        List<Armor> list = new List<Armor>();

        int level = person.Level / 10;

        switch (level) 
        {
            case 50:
                list.AddRange(LV50Armors);
                goto case 40;
            case 40:
                list.AddRange(LV50Armors);
                goto case 30;
            case 30:
                list.AddRange(LV50Armors);
                goto case 20;
            case 20:
                list.AddRange(LV50Armors);
                goto case 10;
            case 10:
                list.AddRange(LV50Armors);
                goto case 0;
            case 0:
                list.AddRange(LV50Armors);
                break;
        }
        return list;
    }

    /// <summary>
    /// Will return a list of all viable Weapons
    /// </summary>
    /// <param name="person"></param>
    /// <returns></returns>
    public static List<Weapon> GetWeaponList(Humanoid person)
    {
        List<Weapon> list = new List<Weapon>();

        int level = person.Level / 10;

        switch (level)
        {
            case 50:
                list.AddRange(LV50Weapons);
                goto case 40;
            case 40:
                list.AddRange(LV50Weapons);
                goto case 30;
            case 30:
                list.AddRange(LV50Weapons);
                goto case 20;
            case 20:
                list.AddRange(LV50Weapons);
                goto case 10;
            case 10:
                list.AddRange(LV50Weapons);
                goto case 0;
            case 0:
                list.AddRange(LV50Weapons);
                break;
        }
        return list;
    }

    /// <summary>
    /// Will return a list of all viable Jewelry
    /// </summary>
    /// <param name="person"></param>
    /// <returns></returns>
    public static List<Jewelry> GetJewelryList(Humanoid person)
    {
        List<Jewelry> list = new List<Jewelry>();

        int level = person.Level / 10;

        switch (level)
        {
            case 50:
                list.AddRange(LV50Jewelry);
                goto case 40;
            case 40:
                list.AddRange(LV50Jewelry);
                goto case 30;
            case 30:
                list.AddRange(LV50Jewelry);
                goto case 20;
            case 20:
                list.AddRange(LV50Jewelry);
                goto case 10;
            case 10:
                list.AddRange(LV50Jewelry);
                goto case 0;
            case 0:
                list.AddRange(LV50Jewelry);
                break;
        }
        return list;
    }

    /// <summary>
    /// Generates an armor given a persons level
    /// </summary>
    /// <param name="person"></param>
    /// <returns></returns>
    public static Armor GenerateArmor(Humanoid person) 
    {
        List<Armor> armors = GetArmorList(person);

        Armor armor = armors[Random.Range(0, armors.Count)];
        armor.Randomize();
        armor.SetRarity(Item.GenerateRarity(person));
        return armor;
    }

    /// <summary>
    /// Generates a weapon given a persons level
    /// </summary>
    /// <param name="person"></param>
    /// <returns></returns>
    public static Weapon GenerateWeapon(Humanoid person) 
    {
        List<Weapon> weapons = GetWeaponList(person);

        Weapon weapon = weapons[Random.Range(0, weapons.Count)];
        weapon.Randomize();
        weapon.SetRarity(Item.GenerateRarity(person));
        return weapon;
    }

    /// <summary>
    /// Generates a jewelry given a persons level
    /// </summary>
    /// <param name="person"></param>
    /// <returns></returns>
    public static Jewelry GenerateJewelry(Humanoid person)
    {
        List<Jewelry> jewelries = GetJewelryList(person);

        Jewelry jewelry = jewelries[Random.Range(0, jewelries.Count)];
        jewelry.Randomize();
        jewelry.SetRarity(Item.GenerateRarity(person));

        return jewelry;
    }

    /// <summary>
    /// Generates an armor given a persons level, Rarity varient
    /// </summary>
    /// <param name="person"></param>
    /// <returns></returns>
    public static Armor GenerateArmor(Humanoid person, RARITY rarity)
    {
        List<Armor> armors = GetArmorList(person);

        Armor armor = armors[Random.Range(0, armors.Count)];
        armor.Randomize();
        armor.SetRarity(rarity);
        return armor;
    }

    /// <summary>
    /// Generates a weapon given a persons level, Rarity varient
    /// </summary>
    /// <param name="person"></param>
    /// <returns></returns>
    public static Weapon GenerateWeapon(Humanoid person, RARITY rarity)
    {
        List<Weapon> weapons = GetWeaponList(person);

        Weapon weapon = weapons[Random.Range(0, weapons.Count)];
        weapon.Randomize();
        weapon.SetRarity(rarity);
        return weapon;
    }

    /// <summary>
    /// Generates a jewelry given a persons level, Rarity varient
    /// </summary>
    /// <param name="person"></param>
    /// <returns></returns>
    public static Jewelry GenerateJewelry(Humanoid person, RARITY rarity)
    {
        List<Jewelry> jewelries = GetJewelryList(person);

        Jewelry jewelry = jewelries[Random.Range(0, jewelries.Count)];
        jewelry.Randomize();
        jewelry.SetRarity(rarity);

        return jewelry;
    }
}
