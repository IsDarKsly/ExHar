using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Humanoid //Will represent any character with a Name, Health, Etc
{
    public string name { get; set; } //The name of our Character
    public CLASS spec { get; set; } //The Class of our Character

    public RACE race { get; set; } //The race of the character

    public bool gender { get; set; } //The Gender of our Character, true if male

    public string they { get => gender ? "he" : "she"; } //Sets the Pronoun for they
    public string their { get => gender ? "his" : "her"; } //Sets the Pronoun for their

    public int Hp;

    public int Mana;

    public int Stamina;

    public Appearance appearance { get; set; } //The appearance of our character

    //these points have to do with the allocated points a character has chosen
    public int availablePoints { get; set; }
    public int pointStrength { get; set; }
    public int pointDexterity { get; set; }
    public int pointIntellect { get; set; }
    public int pointWisdom { get; set; }
    public int pointConstitution { get; set; }
    //Conclusion of point allocated Stats

    //Equipment
    public Armor Helmet { get; set; }
    public Armor Chest { get; set; }
    public Armor Leggings { get; set; }
    public Armor Boots { get; set; }
    public Armor Gloves { get; set; }
    public Jewelry Amulet { get; set; }
    public Jewelry Ring1 { get; set; }
    public Jewelry Ring2 { get; set; }
    public Weapon MainHand { get; set; }
    public Weapon OffHand { get; set; }
    //End of Equipment

    /// <summary>
    /// The list of statuses this humanoid has
    /// </summary>
    public List<StatusEffect> statuses = new List<StatusEffect>();

    /// <summary>
    /// Points available to spend on talents
    /// </summary>
    public int TalentPoints { get; set; } = 0;

    /// <summary>
    /// The list of active talents, also known as skills, this unit has
    /// </summary>
    public List<ActiveTalents> ActiveTalents = new List<ActiveTalents>();

    /// <summary>
    /// The list of passive talents this unit has
    /// </summary>
    public List<PassiveTalents> PassiveTalents = new List<PassiveTalents>();

    /// <summary>
    /// The level of this character
    /// </summary>
    public int Level { get; set; }

    /// <summary>
    /// The XP this character has
    /// </summary>
    public int XP { get; set; }

    /// <summary>
    /// The XP required for this character to level up
    /// </summary>
    public int XPrequired { get { return (int)(115 * Mathf.Pow(Level, 1.14738f)); } }


    //  These temp values get set to whatever values the corresponding stat was at when last leveled
    public int tmp_con { get; set; } = 0;
    public int tmp_str { get; set; } = 0;
    public int tmp_wis { get; set; } = 0;
    public int tmp_dex { get; set; } = 0;
    public int tmp_int { get; set; } = 0;

    /// <summary>
    /// Default constructor
    /// </summary>
    public Humanoid() 
    {
    
    }

    /// <summary>
    /// Default constructor for level 1 character
    /// </summary>
    public Humanoid(string _name, bool _gender, RACE _race, CLASS _spec, Appearance _appearance, int[] _stats )
    {
        name = _name;
        gender = _gender;
        race = _race;
        spec = _spec;
        appearance = _appearance;
        Level = 1;
        SetStatPoint(STATS.Strength, _stats[0]);
        SetStatPoint(STATS.Dexterity, _stats[1]);
        SetStatPoint(STATS.Intellect, _stats[2]);
        SetStatPoint(STATS.Wisdom, _stats[3]);
        SetStatPoint(STATS.Constitution, _stats[4]);
    }

    /// <summary>
    /// Removes a status effect from the list of statuses
    /// </summary>
    /// <param name="status"></param>
    public void RemoveStatus(StatusEffect status) 
    {
        statuses.Remove(status);
    }

    /// <summary>
    /// Clears the status effect list
    /// </summary>
    /// <param name="status"></param>
    public void ClearStatusList()
    {
        statuses.Clear();
    }

    /// <summary>
    /// Adds a status effect to a character,
    /// if a status already exists via identifier the duration is refreshed.
    /// if this status is also a resource changer, a stack is added, which changes the end calculation.
    /// Multiple stuns are not allowed
    /// </summary>
    /// <param name="status"></param>
    public void AddStatus(StatusEffect status)
    {
        if (status.stun && statuses.Exists(t => t.stun || t.stunres)) //    If this is a stun, and we are already stunned or have stun resistance
        {
            Debug.Log("Character is already stunned or has stun resistance");
            return;
        }

        var existingstatus = statuses.Find(t => t.identifier == status.identifier); // See if this status already exists
        if (existingstatus != null) 
        {
            if (existingstatus.resourceboost)   //  This status effects a resource 
            {
                statuses.Find(t => t.identifier == status.identifier).Duration = status.Duration;   //  Refresh the duration
                existingstatus.stacks++;    //  Add a stack
            }
            else    //  This effect is meant to be replaced, like an aura
            {
                RemoveStatus(existingstatus);
                AddStatus(status);
            }

            return;
        }

        statuses.Add(status);
    }

    /// <summary>
    /// Returns true if the character is stunned
    /// </summary>
    /// <returns></returns>
    public bool IsStunned() 
    {
        return statuses.Exists(t=>t.stun);  //  There is a stun in the status list
    }

    /// <summary>
    /// Adds some amount of XP to the character and levels them up if applicable
    /// </summary>
    /// <param name="amount"></param>
    public void AddXp(int amount) 
    {
        XP += amount;
        if (XP >= XPrequired) //    Level up!
        {
            XP = 0;
            Level++;
            TalentPoints++;
            availablePoints += 4;

            tmp_con = pointConstitution;
            tmp_str = pointStrength;
            tmp_dex = pointDexterity;
            tmp_wis = pointWisdom;
            tmp_int = pointIntellect;
        }
    }

    /// <summary>
    /// Resets what points have been spent
    /// </summary>
    public void ResetPoints() 
    {
        availablePoints = pointConstitution + pointStrength + pointWisdom + pointDexterity + pointIntellect;
        pointIntellect = 0;
        pointStrength = 0;
        pointDexterity = 0;
        pointConstitution = 0;
        pointWisdom = 0;
        tmp_con = pointConstitution;
        tmp_str = pointStrength;
        tmp_dex = pointDexterity;
        tmp_wis = pointWisdom;
        tmp_int = pointIntellect;
    }

    /// <summary>
    /// Resets the talents
    /// </summary>
    public void ResetTalents() 
    {
        foreach (var skill in ActiveTalents) 
        {
            if (skill.Refundable) TalentPoints++;
        }

        foreach (var skill in PassiveTalents)
        {
            if (skill.Refundable) TalentPoints++;
        }

        var newactives = ActiveTalents.FindAll(t=>!t.Refundable);   //  Get a new list of only non refundables
        var newpassives = PassiveTalents.FindAll(t => !t.Refundable);

        ActiveTalents = newactives; //  Overwrite old list with new list
        PassiveTalents = newpassives;

    }

    /// <summary>
    /// Adds the diff to the Mana, use negatives for mana loss
    /// </summary>
    /// <param name="diff"></param>
    public void ChangeMana(int diff) 
    {
        Mana += diff;
        Mana = GetMana();
    }

    /// <summary>
    /// Adds the diff to the Health, use negatives for mana loss
    /// </summary>
    /// <param name="diff"></param>
    public void ChangeHealth(int diff)
    {
        Hp += diff;
        Hp = GetHealth();
    }

    /// <summary>
    /// Adds the diff to the Stamina, use negatives for mana loss
    /// </summary>
    /// <param name="diff"></param>
    public void ChangeStamina(int diff)
    {
        Stamina += diff;
        Stamina = GetStamina();
    }

    /// <summary>
    /// Gets the mana this character has
    /// </summary>
    /// <returns></returns>
    public int GetMana()
    {
        if (Mana > GetMaxMana())
        {
            Mana = GetMaxMana();
        }
        else if (Mana < 0) 
        {
            Mana = 0;
        }

        return (Mana);
    }

    /// <summary>
    /// Gets the mana this character has
    /// </summary>
    /// <returns></returns>
    public int GetHealth()
    {
        if (Hp > GetMaxHealth())
        {
            Hp = GetMaxHealth();
        }
        else if (Hp < 0)
        {
            Hp = 0;
        }

        return (Hp);
    }

    /// <summary>
    /// Gets the mana this character has
    /// </summary>
    /// <returns></returns>
    public int GetStamina()
    {
        if (Stamina > GetMaxStamina())
        {
            Stamina = GetMaxStamina();
        }
        else if (Stamina < 0)
        {
            Stamina = 0;
        }

        return (Stamina);
    }

    /// <summary>
    /// Gets the maximum mana this character can have
    /// </summary>
    /// <returns></returns>
    public int GetMaxMana()
    {
        return GetStat(STATS.Intellect) * 5;
    }

    /// <summary>
    /// Gets the maximum health this character can have
    /// </summary>
    /// <returns></returns>
    public int GetMaxHealth()
    {
        return GetStat(STATS.Constitution) * 10;
    }

    /// <summary>
    /// Gets the maximum stamina this character can have
    /// </summary>
    /// <returns></returns>
    public int GetMaxStamina()
    {
        return GetStat(STATS.Intellect) * 5;
    }

    /// <summary>
    /// Used for getting a given stat
    /// Additive first, then the multipliers
    /// </summary>
    public int GetStat(STATS stat)
    {
        return (int)( (10 + GetStatAdditions(stat)) * GetStatMultipliers(stat) );
    }

    /// <summary>
    /// Returns all the Additive bonus for a given stat
    /// </summary>
    /// <param name="stat"></param>
    /// <returns></returns>
    public int GetStatAdditions(STATS stat) 
    {
        int _stat = 0;

        switch (stat)
        {
            case STATS.Constitution:
                _stat = pointConstitution;
                break;
            case STATS.Strength:
                _stat = pointStrength;
                break;
            case STATS.Dexterity:
                _stat = pointDexterity;
                break;
            case STATS.Intellect:
                _stat = pointIntellect;
                break;
            case STATS.Wisdom:
                _stat = pointWisdom;
                break;
        }

        _stat += GetFlatEquipmentBonus(stat);
        _stat += GetFlatStatusEffect(stat);
        _stat += GetFlatPassiveEffect(stat);
        return _stat;
    }

    /// <summary>
    /// Returns the flat effects for all statuses
    /// </summary>
    /// <returns></returns>
    public int GetFlatStatusEffect(STATS stat) 
    {
        int num = 0;

        foreach (var status in statuses) 
        {
            num += status.GetStatChange(stat);
        }

        return num;
    }

    /// <summary>
    /// Returns the flat effects for all passives
    /// </summary>
    /// <returns></returns>
    public int GetFlatPassiveEffect(STATS stat)
    {
        int num = 0;

        foreach (var status in PassiveTalents)
        {
            if(status.trigger==PASSIVETRIGGER.Consistent) num += status.GetStatBonus(stat);
        }

        return num;
    }

    /// <summary>
    /// Returns all the multiplicative bonuses for a given stat
    /// </summary>
    /// <param name="stat"></param>
    /// <returns></returns>
    public float GetStatMultipliers(STATS stat) 
    {
        return (1 * GetGenderMultiplier(stat) * GetClassMultiplier(stat) * GetRacialMultiplier(stat) * GetEquipmentMult(stat));
    }



    /// <summary>
    /// Gets this characters class multiplier for a specific stat
    /// </summary>
    /// <param name="stat"></param>
    /// <returns></returns>
    public float GetClassMultiplier(STATS stat) 
    {
        switch (spec) 
        {
            case (CLASS.MAGE):
                switch (stat) 
                {
                    case STATS.Constitution:
                        return 0.95f;
                    case STATS.Strength:
                        return 0.95f;
                    case STATS.Dexterity:
                        return 1f;
                    case STATS.Intellect:
                        return 1.05f;
                    case STATS.Wisdom:
                        return 1f;
                }
                break;
            case (CLASS.ROGUE):
                switch (stat)
                {
                    case STATS.Constitution:
                        return 0.95f;
                    case STATS.Strength:
                        return 1f;
                    case STATS.Dexterity:
                        return 1.025f;
                    case STATS.Intellect:
                        return 1f;
                    case STATS.Wisdom:
                        return 1.025f;
                }
                break;
            case (CLASS.WARRIOR):
                switch (stat)
                {
                    case STATS.Constitution:
                        return 1.025f;
                    case STATS.Strength:
                        return 1.025f;
                    case STATS.Dexterity:
                        return 1f;
                    case STATS.Intellect:
                        return 0.95f;
                    case STATS.Wisdom:
                        return 1f;
                }
                break;
        }
        return 1f;
    }

    /// <summary>
    /// Gets this characters gender multiplier for a specific stat
    /// </summary>
    /// <param name="stat"></param>
    /// <returns></returns>
    public float GetGenderMultiplier(STATS stat)
    {
        switch (gender)
        {
            case (true):
                switch (stat)
                {
                    case STATS.Constitution:
                        return 1.025f;
                    case STATS.Strength:
                        return 1.025f;
                    case STATS.Dexterity:
                        return 1f;
                    case STATS.Intellect:
                        return 1f;
                    case STATS.Wisdom:
                        return 1f;
                }
                break;
            case (false):
                switch (stat)
                {
                    case STATS.Constitution:
                        return 1f;
                    case STATS.Strength:
                        return 1f;
                    case STATS.Dexterity:
                        return 1.025f;
                    case STATS.Intellect:
                        return 1.025f;
                    case STATS.Wisdom:
                        return 1f;
                }
                break;
        }
        return 1f;
    }

    /// <summary>
    /// Gets this characters racial multiplier for a specific stat
    /// </summary>
    /// <param name="stat"></param>
    /// <returns></returns>
    public float GetRacialMultiplier(STATS stat)
    {
        switch (race)
        {
            case (RACE.Wild_One):
                switch (stat)
                {
                    case STATS.Constitution:
                        return 1f;
                    case STATS.Strength:
                        return 1f;
                    case STATS.Dexterity:
                        return 1f;
                    case STATS.Intellect:
                        return 1f;
                    case STATS.Wisdom:
                        return 1.05f;
                }
                break;
            case (RACE.Arenaen):
                switch (stat)
                {
                    case STATS.Constitution:
                        return 1.05f;
                    case STATS.Strength:
                        return 1f;
                    case STATS.Dexterity:
                        return 1f;
                    case STATS.Intellect:
                        return 1f;
                    case STATS.Wisdom:
                        return 1f;
                }
                break;
            case (RACE.Westerner):
                switch (stat)
                {
                    case STATS.Constitution:
                        return 1f;
                    case STATS.Strength:
                        return 1.05f;
                    case STATS.Dexterity:
                        return 1f;
                    case STATS.Intellect:
                        return 1f;
                    case STATS.Wisdom:
                        return 1f;
                }
                break;
            case (RACE.Avition):
                switch (stat)
                {
                    case STATS.Constitution:
                        return 1f;
                    case STATS.Strength:
                        return 1f;
                    case STATS.Dexterity:
                        return 1f;
                    case STATS.Intellect:
                        return 1.05f;
                    case STATS.Wisdom:
                        return 1f;
                }
                break;
            case (RACE.Novun):
                switch (stat)
                {
                    case STATS.Constitution:
                        return 1f;
                    case STATS.Strength:
                        return 1f;
                    case STATS.Dexterity:
                        return 1f;
                    case STATS.Intellect:
                        return 1.025f;
                    case STATS.Wisdom:
                        return 1.025f;
                }
                break;
            case (RACE.Umbran):
                switch (stat)
                {
                    case STATS.Constitution:
                        return 1f;
                    case STATS.Strength:
                        return 1f;
                    case STATS.Dexterity:
                        return 1.05f;
                    case STATS.Intellect:
                        return 1f;
                    case STATS.Wisdom:
                        return 1f;
                }
                break;
        }
        return 1f;
    }

    /// <summary>
    /// Used for setting stat points. Only for construction really
    /// </summary>
    public void SetStatPoint(STATS stat, int points) 
    {
        if ((points) < 0 || (points) > 89) 
        {
            Debug.Log("Invalid Point num");
            return;
        } 

        switch (stat)
        {
            case STATS.Constitution:
                pointConstitution = points;
                break;
            case STATS.Strength:
                pointStrength = points;
                break;
            case STATS.Dexterity:
                pointDexterity = points;
                break;
            case STATS.Intellect:
                pointIntellect = points;
                break;
            case STATS.Wisdom:
                pointWisdom = points;
                break;
        }

        if ((pointConstitution+pointStrength+pointIntellect+pointWisdom+pointDexterity) > (10 + ((Level-1)*4))) 
        {
            Debug.LogError($"Nu-uh {stat}, {points}, {(10 + (Level * 4))}");
            Application.Quit();
        }
    }

    /// <summary>
    /// Gets the flat bonus for all equipment
    /// </summary>
    /// <param name="stat"></param>
    /// <returns></returns>
    public int GetFlatEquipmentBonus(STATS stat) 
    {
        int bonus = 0;

        if (Helmet != null) bonus += Helmet.GetStatBonus(stat);
        if (Chest != null) bonus += Chest.GetStatBonus(stat);
        if (Leggings != null) bonus += Leggings.GetStatBonus(stat);
        if (Boots != null) bonus += Boots.GetStatBonus(stat);
        if (Gloves != null) bonus += Gloves.GetStatBonus(stat);
        if (Amulet != null) bonus += Amulet.GetStatBonus(stat);
        if (Ring1 != null) bonus += Ring1.GetStatBonus(stat);
        if (Ring2 != null) bonus += Ring1.GetStatBonus(stat);
        if (MainHand != null) bonus += MainHand.GetStatBonus(stat);
        if (OffHand != null) bonus += OffHand.GetStatBonus(stat);

        return bonus;
    }

    /// <summary>
    /// Gets the multipliers for equipments
    /// Each multiplier is actually multiplicative
    /// </summary>
    /// <param name="stat"></param>
    /// <returns></returns>
    public float GetEquipmentMult(STATS stat)
    {
        float bonus = 1f;

        if (Helmet != null) bonus += Helmet.GetMultiplier(stat);
        if (Chest != null) bonus += Chest.GetMultiplier(stat);
        if (Leggings != null) bonus += Leggings.GetMultiplier(stat);
        if (Boots != null) bonus += Boots.GetMultiplier(stat);
        if (Gloves != null) bonus += Gloves.GetMultiplier(stat);
        if (Amulet != null) bonus += Amulet.GetMultiplier(stat);
        if (Ring1 != null) bonus += Ring1.GetMultiplier(stat);
        if (Ring2 != null) bonus += Ring1.GetMultiplier(stat);
        if (MainHand != null) bonus += MainHand.GetMultiplier(stat);
        if (OffHand != null) bonus += OffHand.GetMultiplier(stat);

        return bonus;
    }

    /// <summary>
    /// Calculates the defense from armor, shields, statuses, and talents
    /// </summary>
    /// <returns></returns>
    public int CalculateFlatDefense(DamageType type) 
    {
        int num = 0;

        if (Helmet != null) num += Helmet.GetFlatDefense(type);
        if (Chest != null) num += Chest.GetFlatDefense(type);
        if (Leggings != null) num += Leggings.GetFlatDefense(type);
        if (Boots != null) num += Boots.GetFlatDefense(type);
        if (Gloves != null) num += Gloves.GetFlatDefense(type);
        if (MainHand != null && MainHand.WeaponType == WeaponType.Shield) num += MainHand.GetFlatDefense(type);
        if (OffHand != null && MainHand.WeaponType == WeaponType.Shield) num += OffHand.GetFlatDefense(type);

        foreach (var status in statuses)
        {
            num += status.GetDefenseChange(type);
        }

        foreach (var skill in PassiveTalents)
        {
            num += skill.GetDefenseBonus(type);
        }

        return num;
    }

    /// <summary>
    /// Gets the Resistance from armor, jewelry, statuses, and talents
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public float CalculateResistance(DamageSubType type) 
    {
        float num = 0;

        if (Helmet != null) num += Helmet.GetResistance(type);
        if (Chest != null) num += Chest.GetResistance(type);
        if (Leggings != null) num += Leggings.GetResistance(type);
        if (Boots != null) num += Boots.GetResistance(type);
        if (Gloves != null) num += Gloves.GetResistance(type);
        if (Amulet != null) num += Amulet.GetResistance(type);
        if (Ring1 != null) num += Ring1.GetResistance(type);
        if (Ring2 != null) num += Ring2.GetResistance(type);

        foreach (var status in statuses) 
        {
            num += status.GetResistanceChange(type);
        }

        foreach (var skill in PassiveTalents)
        {
            num += skill.GetResistance(type);
        }

        return num;
    }


    /// <summary>
    /// A variation of SetStatPoints for when a player levels up. We don't want the player reducing a given stat to before the level. So we restrict
    /// </summary>
    public void LevelStatPoint(STATS stat, int points)
    {
        switch (stat)
        {
            case STATS.Constitution:
                if ( (pointConstitution+points) < tmp_con || (pointConstitution + points) > 89 ) return;
                pointConstitution += points;
                availablePoints -= points;
                break;
            case STATS.Strength:
                if ((pointStrength + points) < tmp_str || (pointStrength + points) > 89) return;
                pointStrength += points;
                availablePoints -= points;
                break;
            case STATS.Dexterity:
                if ((pointDexterity + points) < tmp_dex || (pointDexterity + points) > 89) return;
                pointDexterity += points;
                availablePoints -= points;
                break;
            case STATS.Intellect:
                if ((pointIntellect + points) < tmp_int || (pointIntellect + points) > 89) return;
                pointIntellect += points;
                availablePoints -= points;
                break;
            case STATS.Wisdom:
                if ((pointWisdom + points) < tmp_wis || (pointWisdom + points) > 89) return;
                pointWisdom += points;
                availablePoints -= points;
                break;
        }

    }

    /// <summary>
    /// Sets the available points to a new value
    /// </summary>
    /// <param name="points"></param>
    public void SetAvailablePoints(int points) 
    {
        availablePoints = points;
    }

    /// <summary>
    /// adds the available points with a new value
    /// </summary>
    /// <param name="points"></param>
    public void AddAvailablePoints(int points)
    {
        availablePoints += points;
    }

    /// <summary>
    /// Sets the Race of a character
    /// </summary>
    public void SetRace(RACE r) 
    {
        race = r;
    }

    /// <summary>
    /// Sets the class of a character
    /// </summary>
    public void SetClass(CLASS c) 
    {
        spec = c;
    }

    /// <summary>
    /// Sets the appearance of a character
    /// </summary>
    public void SetAppearance(Appearance app) 
    {
        appearance = app;
    }

    /// <summary>
    /// Sets the name of this character
    /// </summary>
    /// <param name="n"></param>
    public void SetName(string n) 
    {
        name = n;
    }

    /// <summary>
    /// true for male I think,
    /// false for female. maybe...
    /// man i wish there were a simpler way to represent this
    /// </summary>
    /// <param name="g"></param>
    public void SetGender(bool g) 
    {
        gender = g;
    }

}

public enum CLASS { MAGE, ROGUE, WARRIOR } //Our enums representing a characters class
public enum RACE { Wild_One, Arenaen, Westerner, Avition, Novun, Umbran } //Our enums representing a characters race
public enum STATS { Constitution, Strength, Dexterity, Intellect, Wisdom } //Our enum representing a stat, this simply helps with getting and setting
public enum RESOURCES { Health, Stamina, Mana } //Enum representing resources