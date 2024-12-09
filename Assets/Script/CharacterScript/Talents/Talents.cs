using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// The Talents are highly customizable, either belonging to the Active or Passive Type.
/// No objects of type "Talent" should ever be created
/// </summary>
[System.Serializable]
public abstract class Talents
{
    /// <summary>
    /// The current or starting level of this talent
    /// </summary>
    public int Level { get; set; } = 1;
    /// <summary>
    /// The max level this talent can reach
    /// </summary>
    public int MaxLevel { get; set; } = 1;
    /// <summary>
    /// Whether this talent can be refunded, or if it is custom
    /// </summary>
    public bool Refundable { get; set; } = true;
    /// <summary>
    /// The name of the talent, which can be used as an identifier
    /// </summary>
    public string Name { get; set; }
    public string Description { get; set; }

    /// <summary>
    /// This determines how many characters are targeted by this ability
    /// It is really only used by the BattleManager to determine who should be targeted
    /// </summary>
    public TARGETTYPE TargetType { get; set; }

    /// <summary>
    /// The damage subtype that defines this talent
    /// </summary>
    public DamageSubType SubType { get; set; }

    /// <summary>
    /// The damage subtype that defines this talent
    /// </summary>
    public DamageType PrimaryType { get; set; }

    /// <summary>
    /// This represents how much scaling this ability has
    /// off weapon damage alone
    /// </summary>
    public float WeaponScaling { get; set; } = 0f;

    /// <summary>
    /// Again, used by the BattleManager to determine who should be targeted
    /// </summary>
    public int Targets { get; set; } = 1;

    /// <summary>
    /// The Lower Range of an ability is meant to be the lower bound for damage/healing.
    /// When this ability is used, a random range is calculated between the lower range and 1f
    /// to get the actual value produced by this skill.
    /// This defaults to .7f, or 70%
    /// </summary>
    public float LowerRange { get; set; } = .7f;

    /// <summary>
    /// Cooldown represents the number of turns one will have to wait before using the skill
    /// </summary>
    public int Cooldown { get; set; } = 0;

    /// <summary>
    /// Cooldown Counter counts the actual turns before a skill can be used again
    /// </summary>
    public int CooldownCounter { get; set; } = 0;

    public int BaseDamage { get; set; } = 0;


    public int ManaCost { get; set; } = 0;
    public int StaminaCost { get; set; } = 0;
    public int HealthCost { get; set; } = 0;

    /// <summary>
    /// Whether or not this ability can or can't crit
    /// </summary>
    public bool CanCrit { get; set; } = true;

    /// <summary>
    /// Simple version of getting mana cost
    /// </summary>
    /// <returns></returns>
    public virtual int GetManaCost() { return ManaCost; }
    /// <summary>
    /// Simple version of getting stamina cost
    /// </summary>
    /// <returns></returns>
    public virtual int GetStaminaCost() { return ManaCost; }
    /// <summary>
    /// Simple version of getting stamina cost
    /// </summary>
    /// <returns></returns>
    public virtual int GetHealthCost() { return ManaCost; }

    /// <summary>
    /// This represents the required weapon for this talent if any
    /// </summary>
    public List<WeaponType> RequiredWeapons = new List<WeaponType>();

    public Dictionary<STATS, float> AttributeScaling = new Dictionary<STATS, float>();

    public Talents() 
    {
    
    }

    // ABSTRACT SECTION

    /// <summary>
    /// Levels a particular talent up
    /// </summary>
    public abstract void LevelUp();


    /// <summary>
    /// This will invoke the skill on a set of targets.
    /// Dependant on the skill
    /// </summary>
    /// <param name="targets"></param>
    public abstract void Invoke(List<Humanoid> targets, Humanoid owner);

    //END OF ABSTRACT SECTION

    /// <summary>
    /// Getter for any displaying text to use.
    /// Should return a translated string
    /// </summary>
    /// <returns></returns>
    public virtual string GetCostText() { return null; }
    /// <summary>
    /// Getter for any displaying weapon scaling text to use
    /// </summary>
    /// <returns></returns>
    public virtual string GetScalingText() { return (WeaponScaling > 0f) ? (WeaponScaling * 100).ToString() : null; }
    /// <summary>
    /// Gets the translated name for this Talent
    /// </summary>
    /// <returns></returns>
    public virtual string GetNameText() { return LocalizationManager.Instance.ReadUIDictionary(Name); }
    /// <summary>
    /// Gets the translated description for this talent
    /// </summary>
    /// <returns></returns>
    public virtual string GetDescriptionText() { return LocalizationManager.Instance.ReadUIDictionary(Description); }
    /// <summary>
    /// Gets the translated cooldown
    /// </summary>
    /// <returns></returns>
    public virtual string GetCooldownText() { return OffCooldown() ? Cooldown.ToString() : CooldownCounter.ToString(); }
    /// <summary>
    /// Gets the translated level to max level comparison
    /// </summary>
    /// <returns></returns>
    public virtual string GetLevelText() { return Level.ToString() + "/" + MaxLevel.ToString(); }
    /// <summary>
    /// Gets the healing or damage value with translate "Damage" or "Healing" in front of it
    /// </summary>
    /// <returns></returns>
    public virtual string GetDamageText(Humanoid owner) { return ((int)GetDamageCalculation(owner, true).damagePortion[PrimaryType]*LowerRange).ToString() + "-" + GetDamageCalculation(owner, true).damagePortion[PrimaryType].ToString(); }
    /// <summary>
    /// Gets the number of targets this skill can hit.
    /// Examples: 
    /// Single Target
    /// Self
    /// Targets: 2
    /// All Allies
    /// All Enemies
    /// </summary>
    /// <returns></returns>
    public virtual string GetTargetsText() 
    {
        switch (TargetType) 
        {
            case TARGETTYPE.SINGLE:
                return LocalizationManager.Instance.ReadUIDictionary("Single");
            case TARGETTYPE.MULTIPLE:
                return LocalizationManager.Instance.ReadUIDictionary("Multiple") + ": " + Targets.ToString();
            case TARGETTYPE.SELF:
                return LocalizationManager.Instance.ReadUIDictionary("Self");
            case TARGETTYPE.ALLALLIES:
                return LocalizationManager.Instance.ReadUIDictionary("All Allies");
            case TARGETTYPE.ALLENEMIES:
                return LocalizationManager.Instance.ReadUIDictionary("All Enemies");
        }
        return "";
    }
    /// <summary>
    /// Checks both owners weapons to see if they have required weapon,
    /// If they do, return True
    /// </summary>
    /// <returns></returns>
    public bool HasRequiredWeapon(Humanoid Owner) 
    {
        if (RequiredWeapons.Count == 0) return true;    //  No required weapon
        if (Owner.MainHand != null && RequiredWeapons.Contains(Owner.MainHand.WeaponType)) return true;
        if (Owner.OffHand != null && RequiredWeapons.Contains(Owner.OffHand.WeaponType)) return true;
            return false;
    }
    
    /// <summary>
    /// Subtracts one from the Cooldown Counter
    /// </summary>
    public virtual void Tick()
    {
        CooldownCounter--;
        if (CooldownCounter < 0) CooldownCounter = 0;
    }


    /// <summary>
    /// This boolean checks pre-requisites to see if the owner can cast the talent
    /// </summary>
    /// <returns></returns>
    public virtual bool OwnerCanCast(Humanoid owner)
    {
        return (owner.GetMana() >= GetManaCost() && owner.GetStamina() >= GetStaminaCost() && owner.GetHealth() >= GetHealthCost());
    }

    /// <summary>
    /// Returns true if this ability is off cooldown
    /// </summary>
    /// <returns></returns>
    public virtual bool OffCooldown()
    {
        return CooldownCounter <= 0;
    }

    /// <summary>
    /// Attempts to get the specific scaling of an attribute for this talent. If one does not exist, returns null
    /// </summary>
    /// <returns></returns>
    public string GetAttributeScalingText(STATS key) 
    {
        return (AttributeScaling.ContainsKey(key)) ? (AttributeScaling[key]*100).ToString() : null;
    }

    /// <summary>
    /// The GetDamageCalculation takes the base damage and calculates out any and all relevant information
    /// </summary>
    /// <returns></returns>
    public virtual Damage GetDamageCalculation(Humanoid Owner, bool IsUI = false) 
    {
        Damage calcDamage = new Damage(new Dictionary<DamageType, int>(), new Dictionary<DamageSubType, float>());

        calcDamage.damagePortion[PrimaryType] = BaseDamage;
        calcDamage.damagePercent[SubType] = 1f;

        foreach (var scaler in AttributeScaling) // Adds attribute scaling values to temp damage
        {
            calcDamage.damagePortion[PrimaryType] += (int)(Owner.GetStat(scaler.Key)*scaler.Value);
        }

        Damage weaponScalingDamage = new Damage(new Dictionary<DamageType, int>(), new Dictionary<DamageSubType, float>());

        if (WeaponScaling > 0f) //There is some scaling factor for weapons, this should somehat augment the actual scaling of the skill itself
        {
            if (Owner.MainHand != null && Owner.MainHand.WeaponType != WeaponType.Shield)   //  Main hand is a weapon and not a shield
            {
                var MainHandDamage = Owner.MainHand.GetWeaponDamage(Owner);
                foreach (var kvp in MainHandDamage.damagePortion) 
                {
                    if (!weaponScalingDamage.damagePortion.ContainsKey(kvp.Key)) // We havent yet logged this damage type (Likely because this is the first round)
                    {
                        weaponScalingDamage.damagePortion[kvp.Key] = kvp.Value;
                        continue;
                    }
                    //We have this damage type already, append it
                    weaponScalingDamage.damagePortion[kvp.Key] += kvp.Value;
                }
            }
            if (Owner.OffHand != null && Owner.OffHand.WeaponType != WeaponType.Shield)   //  Off hand is a weapon and not a shield
            {
                var OffHandDamage = Owner.OffHand.GetWeaponDamage(Owner);
                foreach (var kvp in OffHandDamage.damagePortion)
                {
                    if (!weaponScalingDamage.damagePortion.ContainsKey(kvp.Key)) // We havent yet logged this damage type
                    {
                        weaponScalingDamage.damagePortion[kvp.Key] = kvp.Value;
                        continue;
                    }
                    //We have this damage type already, append it
                    weaponScalingDamage.damagePortion[kvp.Key] += kvp.Value;
                }
            }

            //  Now we must calculate the Weapon scaling effect on the damage

            List<DamageType> DamKeys = new List<DamageType>(weaponScalingDamage.damagePortion.Keys);
            foreach (var key in DamKeys) 
            {
                weaponScalingDamage.damagePortion[key] = (int)(weaponScalingDamage.damagePortion[key] * WeaponScaling); //  We scale the value from our calculations
                calcDamage.damagePortion[PrimaryType] += weaponScalingDamage.damagePortion[key];    //  We add the value to our primary damage
            }

        }


        if (CanCrit && !IsUI && Owner.DidItCrit())   //  Did we crit and we arent a UI element?
        {
            List<DamageType> DamKeys = new List<DamageType>(calcDamage.damagePortion.Keys);
            foreach (var key in DamKeys)
            {

                calcDamage.damagePortion[PrimaryType] *= 2; //  Double Damage for a crit
            }
        }

        if (!IsUI)  //  Random element from lower bound 
        {
            List<DamageType> DamKeys = new List<DamageType>(calcDamage.damagePortion.Keys);
            foreach (var key in DamKeys)
            {
                calcDamage.damagePortion[key] = (int)(calcDamage.damagePortion[key]*Random.Range(LowerRange,1f));    //  We add the value to our primary damage
            }
        }

        return calcDamage;
    }
}

public enum TARGETTYPE { SINGLE, MULTIPLE, ALLENEMIES, ALLALLIES, SELF }