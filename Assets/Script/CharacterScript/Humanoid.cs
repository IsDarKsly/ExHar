using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Humanoid //Will represent any character with a Name, Health, Etc
{
    public string Name { get; set; } //The name of our Character
    public CLASS spec { get; set; } //The Class of our Character

    public RACE race { get; set; } //The race of the character

    public bool gender { get; set; } //The Gender of our Character, true if male

    public bool IsCustom { get; set; } = false; //Whether the given character is a custom character

    public string he_she { get => gender ? "he" : "she"; } //Sets the Pronoun
    public string his_her { get => gender ? "his" : "her"; } //Sets the Pronoun

    public int Hp;

    public int Mana;

    public int Stamina;

    public int Threat;

    public Appearance appearance { get; set; } = new Appearance(); //The appearance of our character

    //these points have to do with the allocated points a character has chosen
    public int availablePoints { get; set; }
    public int pointStrength { get; set; }
    public int pointDexterity { get; set; }
    public int pointIntellect { get; set; }
    public int pointWisdom { get; set; }
    public int pointConstitution { get; set; }
    //Conclusion of point allocated Stats

    //Equipment
    private Armor _Helmet;
    private Armor _Chest;
    private Armor _Leggings;
    private Armor _Boots;
    private Armor _Gloves;
    private Jewelry _Amulet;
    private Jewelry _Ring1;
    private Jewelry _Ring2;
    private Weapon _MainHand;
    private Weapon _OffHand;

    public Armor Helmet { get => _Helmet; set => _Helmet = value; }
    public Armor Chest { get => _Chest; set => _Chest = value; }
    public Armor Leggings { get => _Leggings; set => _Leggings = value; }
    public Armor Boots { get => _Boots; set => _Boots = value; }
    public Armor Gloves { get => _Gloves; set => _Gloves = value; }
    public Jewelry Amulet { get => _Amulet; set => _Amulet = value; }
    public Jewelry Ring1 { get => _Ring1; set => _Ring1 = value; }
    public Jewelry Ring2 { get => _Ring2; set => _Ring2 = value; }
    public Weapon MainHand { get => _MainHand; set => _MainHand = value; }
    public Weapon OffHand { get => _OffHand; set => _OffHand = value; }
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
    /// A simple bool to be used in the battle manager, lets characters act
    /// </summary>
    public bool turn = false;

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
        Name = _name;
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
        AddActiveTalent(new BasicAttack());
    }

    /// <summary>
    /// Adds a passive talent to this character,
    /// If one already exists, the existin talent is leveled up
    /// </summary>
    public void AddPassiveTalent(PassiveTalents talent) 
    {
        var existingTalent = PassiveTalents.Find(t => t.Name == talent.Name);
        if (existingTalent != null) //  This talent already exists in our talents
        {
            existingTalent.LevelUp();
        }
        else 
        {
            PassiveTalents.Add(talent);
        }
    }

    /// <summary>
    /// Adds an active talent to this character,
    /// If one already exists, the existing talent is leveled up
    /// </summary>
    public void AddActiveTalent(ActiveTalents talent)
    {
        var existingTalent = ActiveTalents.Find(t => t.Name == talent.Name);
        if (existingTalent != null) //  This talent already exists in our talents
        {
            existingTalent.LevelUp();
        }
        else
        {
            ActiveTalents.Add(talent);
        }
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
           //Debug.Log("Character is already stunned or has stun resistance");
            return;
        }

        var existingstatus = statuses.Find(t => t.Name == status.Name); // See if this status already exists
        if (existingstatus != null) 
        {
            if (existingstatus.resourceboost)   //  This status effects a resource 
            {
                statuses.Find(t => t.Name == status.Name).Duration = status.Duration;   //  Refresh the duration
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
    /// Adds the diff to threat, use negative for threat loss
    /// </summary>
    public void ChangeThreat(int diff) 
    {
        Threat += diff;
        Threat = GetThreat();
    }

    /// <summary>
    /// Gets the threat this character has
    /// </summary>
    /// <returns></returns>
    public int GetThreat()
    {
        if (Threat < 0)
        {
            Threat = 0;
        }

        return (Threat);
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
        return GetStat(STATS.Wisdom) * 5;
    }

    /// <summary>
    /// Gets the maximum health this character can have
    /// </summary>
    /// <returns></returns>
    public int GetMaxHealth()
    {
        return GetStat(STATS.Constitution) * 5;
    }

    /// <summary>
    /// Gets the maximum stamina this character can have
    /// Baseline character has 100
    /// </summary>
    /// <returns></returns>
    public int GetMaxStamina()
    {
        return 80 + GetStat(STATS.Constitution) * 2;
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
        //Debug.Log($"{Name} {stat.ToString()}: After Point addition: {_stat}");
        _stat += GetFlatEquipmentBonus(stat);
        //Debug.Log($"{Name} {stat.ToString()}: After Equipment addition: {_stat}");
        _stat += GetFlatStatusEffect(stat);
        //Debug.Log($"{Name} {stat.ToString()}: After Buff addition: {_stat}");
        _stat += GetFlatPassiveEffect(stat);
        //Debug.Log($"{Name} {stat.ToString()}: After Passive addition: {_stat}");
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
        //Debug.Log($"{Name} {stat.ToString()}: equip multipliers: {(GetEquipmentMult(stat))}");
        //Debug.Log($"{Name} {stat.ToString()}: race multipliers: {(GetRacialMultiplier(stat))}");
        //Debug.Log($"{Name} {stat.ToString()}: class multipliers: {(GetClassMultiplier(stat))}");
        //Debug.Log($"{Name} {stat.ToString()}: gender multipliers: {(GetGenderMultiplier(stat))}");
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
           //Debug.Log("Invalid Point num");
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
           //Debug.LogError($"Nu-uh {stat}, {points}, {(10 + (Level * 4))}");
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
        //Debug.Log($"{Name} {stat.ToString()}: Helmet multipliers: {Helmet?.GetMultiplier(stat)}");
        if (Chest != null) bonus += Chest.GetMultiplier(stat);
        //Debug.Log($"{Name} {stat.ToString()}: Chest multipliers: {Chest?.GetMultiplier(stat)}");
        if (Leggings != null) bonus += Leggings.GetMultiplier(stat);
        //Debug.Log($"{Name} {stat.ToString()}: Leggings multipliers: {Leggings?.GetMultiplier(stat)}");
        if (Boots != null) bonus += Boots.GetMultiplier(stat);
        //Debug.Log($"{Name} {stat.ToString()}: Boots multipliers: {Boots?.GetMultiplier(stat)}");
        if (Gloves != null) bonus += Gloves.GetMultiplier(stat);
        //Debug.Log($"{Name} {stat.ToString()}: Gloves multipliers: {Gloves?.GetMultiplier(stat)}");
        if (Amulet != null) bonus += Amulet.GetMultiplier(stat);
        //Debug.Log($"{Name} {stat.ToString()}: Amulet multipliers: {Amulet?.GetMultiplier(stat)}");
        if (Ring1 != null) bonus += Ring1.GetMultiplier(stat);
        //Debug.Log($"{Name} {stat.ToString()}: Ring1 multipliers: {Ring1?.GetMultiplier(stat)}");
        if (Ring2 != null) bonus += Ring2.GetMultiplier(stat);
        //Debug.Log($"{Name} {stat.ToString()}: Ring2 multipliers: {Ring2?.GetMultiplier(stat)}");
        if (MainHand != null) bonus += MainHand.GetMultiplier(stat);
        //Debug.Log($"{Name} {stat.ToString()}: MainHand multipliers: {MainHand?.GetMultiplier(stat)}");
        if (OffHand != null) bonus += OffHand.GetMultiplier(stat);
        //Debug.Log($"{Name} {stat.ToString()}: OffHand multipliers: {OffHand?.GetMultiplier(stat)}");

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
        if (MainHand != null && MainHand.WeaponType == WeaponType.Shield) num += MainHand.GetResistance(type);
        if (OffHand != null && MainHand.WeaponType == WeaponType.Shield) num += MainHand.GetResistance(type);

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
    /// Uses a skill
    /// </summary>
    /// <param name="skill"></param>
    public void UseSkill(string skillName, List<Humanoid> targets) 
    {
        //Debug.Log($"Using skill {skillName}");
        var skill = ActiveTalents.Find(t=>t.Name==skillName);
        if (CanICast(skillName)) 
        {
            //Debug.Log($"Skill is of type {skill.GetType().Name}");
            skill.Invoke(targets, this);
            return;
        }
        //Debug.LogError($"{Name} attempted to use {skillName}, which {he_she} doesnt have");
    }

    /// <summary>
    /// Return a true value if the character has this skill, and can cast it
    /// </summary>
    /// <returns></returns>
    public bool CanICast(string skillName) 
    {
        var skill = ActiveTalents.Find(t => t.Name == skillName);
        if (skill != null && skill.OwnerCanCast(this))
        {
            return true;
        }
        else 
        {
            return false;
        }
    }


    /// <summary>
    /// This function will calculate the damage this character should take before actually taking that damage.
    /// </summary>
    public virtual void CalculateDamageTaken(Damage damageStruct) 
    {
        if (damageStruct.damagePortion == null || damageStruct.damagePercent == null) 
        {
           //Debug.Log("The damage struct had null proportions");
        } 

        if (damageStruct.IsDodgeable && DidIDodge() && !this.GetType().IsSubclassOf(typeof(Enemy)))     //  if we dodged the attack (Not for any enemies)
        {
           //Debug.Log("We dodged the attack");
            return;
        }

        var damage = new Damage(new Dictionary<DamageType, int>(), new Dictionary<DamageSubType, float>()); //  We wont to make sure that no reference is being edited, as other enemies will take different damage if it does

        foreach (var damKVP in damageStruct.damagePortion) // We copy the flat values
        {
            damage.damagePortion[damKVP.Key] = damKVP.Value;
        }

        foreach (var perKVP in damageStruct.damagePercent) // We copy the percent values
        {
            damage.damagePercent[perKVP.Key] = perKVP.Value;
        }


        var keys = new List<DamageType>(damage.damagePortion.Keys);
        foreach (var key in keys)  //  Calculate damage for this portion
        {
            damage.damagePortion[key] -= CalculateFlatDefense(key);   //  Simple flat reductions applied to the damage 
            if (damage.damagePortion[key] < 0) damage.damagePortion[key] = 0; //  Dont let damage dip below zero
        }

        foreach (var p in PassiveTalents.FindAll(p => p.trigger == PASSIVETRIGGER.BeforeImAttacked))    //  Passive talent calculation
        {
            p.Invoke(ref damage, this);
        }

        foreach (var perc_dam in damage.damagePercent) //   Calculate percentage for damages
        {
            float dam = 0;  //  Initialize a damage we will be taking

            foreach (var flat_dam in damage.damagePortion) //  Add up the respective portions for this subtype
            {
                dam += damage.damagePortion[flat_dam.Key] * damage.damagePercent[perc_dam.Key]; 
            }

            dam *= (1f-CalculateResistance(perc_dam.Key));  //  Calculate player resistance (1-60% means new damage taken is 40% as effective)
            if(dam < 0) dam = 0;

            ChangeResourceBattle((int)(-1*dam), perc_dam.Key, RESOURCES.Health,damage.IsCritical, true);
        }
    }

    /// <summary>
    /// This function will calculate the healing a character recieves, those resistant to a values given healing component resist a portion of it
    /// </summary>
    public void CalculateHealingTaken(int value, DamageSubType subType, RESOURCES resource)
    {

        float newvalue = value * (1f - CalculateResistance(subType));

        ChangeResourceBattle((int)newvalue, subType, RESOURCES.Health, false, true);
        
    }

    /// <summary>
    /// Set's all the cooldowns back to zero for this character
    /// </summary>
    public void RestoreCooldowns() 
    {
        foreach (var skill in ActiveTalents) 
        {
            skill.CooldownCounter = 0;
        }

        foreach (var skill in PassiveTalents)
        {
            skill.CooldownCounter = 0;
        }
    }

    /// <summary>
    /// Removes all status effects from this character
    /// </summary>
    public void RemoveEffects() 
    {
        foreach (var status in statuses) 
        {
            statuses.Remove(status);
        }
    }

    /// <summary>
    /// Calculates what this characters dodge chance is.
    /// Scales with dexterity
    /// </summary>
    /// <returns></returns>
    public float GetDodgeChance() 
    {
        return (40 * Mathf.Log10(GetStat(STATS.Dexterity) / 15) + 5)/100;
    }

    /// <summary>
    /// Calculates whether or not this character dodged an attack
    /// </summary>
    /// <returns></returns>
    public bool DidIDodge() 
    {
        float r = Random.Range(0f, 1f);
        return (r <= GetDodgeChance());
    }

    /// <summary>
    /// Gets the crit chance for this character
    /// </summary>
    /// <returns></returns>
    public float GetCritChance() 
    {
        return GetStat(STATS.Wisdom)/200f;
    }

    /// <summary>
    /// Returns true if the character crit
    /// </summary>
    /// <returns></returns>
    public bool DidItCrit() 
    {
        float r = Random.Range(0f, 1f);
        return (r <= GetCritChance());
    }

    /// <summary>
    /// The actual code for taking damage during battle, triggers animations for battle manager
    /// </summary>
    public void ChangeResourceBattle(int value, DamageSubType type, RESOURCES resourceType, bool crit, bool animate) 
    {
        if (value == 0) return; // No need to do anything if nothing occurs
       //Debug.Log($"{Name} is having resource changed");
        switch (resourceType) 
        {
            case RESOURCES.Health:
                ChangeHealth(value);
                break;
            case RESOURCES.Stamina:
                ChangeStamina(value);
                return;
            case RESOURCES.Mana:
                ChangeMana(value);
                return;
        }

        //Here would be where we tell Battle manager what to animate  
        if(animate) BattleManager.Instance.animationManager.QueueAnimationResourceChange(new AnimatableResourceChange(value, type, resourceType, crit, this));
    }

    /// <summary>
    /// The full restore function fully restores the resources of this character.
    /// It is not meant to be used during combat unless to fully instantiate characters
    /// </summary>
    /// <param name="resourceType"></param>
    public void FullRestore() 
    {
        Hp = GetMaxHealth();
        Stamina = GetMaxStamina();
        Mana = GetMaxMana();
    }

    /// <summary>
    /// The actual slot being equiped to, this should add any passive talents on an item
    /// to the player equipping it
    /// </summary>
    public void Equip<T>(ref T equipmentslot, Equipment equipment) where T : Equipment
    {
        Debug.Log("Equipping item");
        DataManager.Instance.RemoveFromInventory(equipment);
        equipmentslot = (T)equipment;
    }

    /// <summary>
    /// This will add whatever is in our equipment slot at this location back to the inventory
    /// before setting the slot to null
    /// This needs to remove any talents that exist only because of this armor
    /// </summary>
    public void Unequip<T>(ref T equipmentslot) where T : Equipment
    {
        Debug.Log("Unequiping item");
        if (equipmentslot == null) return;  //  No need to unequip what already exists
        DataManager.Instance.AddToInventory(equipmentslot);
        equipmentslot = null;
    }

    /// <summary>
    /// Here we equip a slot with an item.
    /// Logically, we should attempt to equip to the first available slot
    /// IE Ring1, but if that spot is occupied and Ring 2 is not, we should equip to Ring 2
    /// Same applies to main hand and offhand, except we ignore this for Two-Handed items
    /// </summary>
    public void EquipSlot(EQUIPMENTSLOT slot, Equipment equipment) 
    {
        switch (slot) 
        {
            case EQUIPMENTSLOT.MainHand:
                if (MainHand != null && OffHand == null && ((Weapon)equipment).WeaponWeight != WeaponWeight.TwoHand)    //  If Mainhand is occupied and offhand isnt and this isnt a twohanded weapon
                {
                    Equip(ref _OffHand, equipment); //  Equip this weapon to offhand
                    return;
                }
                Equip(ref _MainHand, equipment);    //  Otherwise equip this as intended
                break;
            case EQUIPMENTSLOT.OffHand:
                if (OffHand != null && MainHand == null && ((Weapon)equipment).WeaponWeight != WeaponWeight.TwoHand)    //  If Offhand is occupied and mainhand isnt and this isnt a twohanded weapon
                {
                    Equip(ref _MainHand, equipment); //  Equip this weapon to mainhand
                    return;
                }
                Equip(ref _OffHand, equipment);    //  Otherwise equip this as intended
                break;
            case EQUIPMENTSLOT.Helm:
                Equip(ref _Helmet, equipment);
                break;
            case EQUIPMENTSLOT.Chest:
                Equip(ref _Chest, equipment);
                break;
            case EQUIPMENTSLOT.Legs:
                Equip(ref _Leggings, equipment);
                break;
            case EQUIPMENTSLOT.Boots:
                Equip(ref _Boots, equipment);
                break;
            case EQUIPMENTSLOT.Gloves:
                Equip(ref _Gloves, equipment);
                break;
            case EQUIPMENTSLOT.Amulet:
                Equip(ref _Amulet, equipment);
                break;
            case EQUIPMENTSLOT.Ring1:
                if (Ring1 != null && Ring2 == null) 
                {
                    Equip(ref _Ring2, equipment); //  Equip this weapon to ring2 slot if available
                    return;
                }
                Equip(ref _Ring1, equipment);   //  Equip as intended
                break;
            case EQUIPMENTSLOT.Ring2:
                if (Ring2 != null && Ring1 == null)
                {
                    Equip(ref _Ring1, equipment); //  Equip this weapon to ring2 slot if available
                    return;
                }
                Equip(ref _Ring2, equipment);   //  Equip as intended
                break;
        }
    }

    /// <summary>
    /// Here we unequip an equipment slot
    /// </summary>
    public void UnequipSlot(EQUIPMENTSLOT slot) 
    {
        switch (slot)
        {
            case EQUIPMENTSLOT.MainHand:
                if (MainHand != null) Unequip(ref _MainHand);
                break;
            case EQUIPMENTSLOT.OffHand:
                if (OffHand != null) Unequip(ref _OffHand);
                break;
            case EQUIPMENTSLOT.Helm:
                if (Helmet != null) Unequip(ref _Helmet);
                break;
            case EQUIPMENTSLOT.Chest:
                if (Chest != null) Unequip(ref _Chest);
                break;
            case EQUIPMENTSLOT.Legs:
                if (Leggings != null) Unequip(ref _Leggings);
                break;
            case EQUIPMENTSLOT.Boots:
                if (Boots != null) Unequip(ref _Boots);
                break;
            case EQUIPMENTSLOT.Gloves:
                if (Gloves != null) Unequip(ref _Gloves);
                break;
            case EQUIPMENTSLOT.Amulet:
                if (Amulet != null) Unequip(ref _Amulet);
                break;
            case EQUIPMENTSLOT.Ring1:
                if (Ring1 != null) Unequip(ref _Ring1);
                break;
            case EQUIPMENTSLOT.Ring2:
                if (Ring2 != null) Unequip(ref _Ring2);
                break;
        }
    }

    /// <summary>
    /// Checks each equipment slot to see if it matches the given item
    /// </summary>
    /// <param name="equipment"></param>
    /// <returns>Returns true if the item is equipped and the exact equipment slot</returns>
    public (bool, EQUIPMENTSLOT) IsEquipped(Equipment equipment) 
    {
        if (Helmet != null && Helmet == equipment) return (true, EQUIPMENTSLOT.Helm);
        if (Chest != null && Chest == equipment) return (true, EQUIPMENTSLOT.Chest);
        if (Leggings != null && Leggings == equipment) return (true, EQUIPMENTSLOT.Legs);
        if (Boots != null && Boots == equipment) return (true, EQUIPMENTSLOT.Boots);
        if (Gloves != null && Gloves == equipment) return (true, EQUIPMENTSLOT.Gloves);
        if (Amulet != null && Amulet == equipment) return (true, EQUIPMENTSLOT.Amulet);
        if (Ring1 != null && Ring1 == equipment) return (true, EQUIPMENTSLOT.Ring1);
        if (Ring2 != null && Ring2 == equipment) return (true, EQUIPMENTSLOT.Ring2);
        if (MainHand != null && MainHand == equipment) return (true, EQUIPMENTSLOT.MainHand);
        if (OffHand != null && OffHand == equipment) return (true, EQUIPMENTSLOT.OffHand);
        return (false, EQUIPMENTSLOT.Helm);
    }

    /// <summary>
    /// We check if the character has a slot viable for this equipment
    /// </summary>
    public bool HasAvailableSlot(Equipment equipment)
    {
        switch (equipment.equipmentslot)
        {
            case EQUIPMENTSLOT.MainHand:
                return (MainHand == null || (MainHand != null && OffHand == null && ((Weapon)equipment).WeaponWeight != WeaponWeight.TwoHand));
            case EQUIPMENTSLOT.OffHand:
                return (OffHand == null || (OffHand != null && MainHand == null && ((Weapon)equipment).WeaponWeight != WeaponWeight.TwoHand));
            case EQUIPMENTSLOT.Helm:
                return (Helmet == null);
            case EQUIPMENTSLOT.Chest:
                return (Chest == null);
            case EQUIPMENTSLOT.Legs:
                return (Leggings == null);
            case EQUIPMENTSLOT.Boots:
                return (Boots == null);
            case EQUIPMENTSLOT.Gloves:
                return (Gloves == null);
            case EQUIPMENTSLOT.Amulet:
                return (Amulet == null);
            case EQUIPMENTSLOT.Ring1:
                return (Ring1 == null || (Ring1 != null && Ring2 == null));
            case EQUIPMENTSLOT.Ring2:
                return (Ring2 == null || (Ring2 != null && Ring1 == null));
        }
        return true;
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
        Name = n;
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

    /// <summary>
    /// Ticks this character, done at the start of their respective battle turn.
    /// Causes Dots to proc, buffs and cooldowns to countdown
    /// </summary>
    public void Tick() 
    {
        if (GetHealth() <= 0)   //  If this character is dead, remove buffs, reset cooldowns
        {
            ClearStatusList();
            RestoreCooldowns();
            return;
        }

        //Restore 20% of stamina naturally per turn
        ChangeResourceBattle((int)(GetMaxStamina()*.2f), DamageSubType.Lightning, RESOURCES.Stamina, false, false);

        foreach (var skill in ActiveTalents) 
        {
            skill.Tick();
        }

        foreach (var skill in PassiveTalents)
        {
            skill.Tick();
        }

        foreach (var buff in statuses) 
        {
            buff.Tick(this);
        }
    }

    /// <summary>
    /// Returns the translated name of a character
    /// </summary>
    /// <returns></returns>
    public virtual string GetName() 
    {
        return LocalizationManager.Instance.ReadName(Name);
    }

    /// <summary>
    /// GetDescription returns a translated variation of a characters race then gender.
    /// This is to be used by UI.
    /// </summary>
    /// <returns></returns>
    public virtual string GetDescription() 
    {
        return (LocalizationManager.Instance.ReadUIDictionary(race.ToString()) + "\n" + (gender ? LocalizationManager.Instance.ReadUIDictionary("Male") : LocalizationManager.Instance.ReadUIDictionary("Female")));
    }

}

public enum CLASS { MAGE, ROGUE, WARRIOR } //Our enums representing a characters class
public enum RACE { Wild_One, Arenaen, Westerner, Avition, Novun, Umbran } //Our enums representing a characters race
public enum STATS { Constitution, Strength, Dexterity, Intellect, Wisdom } //Our enum representing a stat, this simply helps with getting and setting
public enum RESOURCES { Health, Stamina, Mana } //Enum representing resources

/// <summary>
/// This is used in the equiping/unequiping process
/// </summary>
public enum EQUIPMENTSLOT { Helm, Chest, Legs, Boots, Gloves, MainHand, OffHand, Amulet, Ring1, Ring2 }