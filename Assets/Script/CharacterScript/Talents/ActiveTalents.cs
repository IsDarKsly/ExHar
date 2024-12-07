using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ActiveTalents : Talents
{

    public int ManaCost { get; set; } = 0;
    public int StaminaCost { get; set; } = 0;
    public int HealthCost { get; set; } = 0;

    /// <summary>
    /// Cooldown represents the number of turns one will have to wait before using the skill
    /// </summary>
    public int Cooldown { get; set; } = 0;

    /// <summary>
    /// Cooldown Counter counts the actual turns before a skill can be used again
    /// </summary>
    public int CooldownCounter { get; set; } = 0;

    public ActiveTalents() 
    {
    
    }

    /// <summary>
    /// Levels a particular talent up.
    /// Dependant on the specific talent
    /// </summary>
    public override void LevelUp() { return; }

    /// <summary>
    /// This will invoke the skill on a set of targets.
    /// Dependant on the skill
    /// </summary>
    /// <param name="targets"></param>
    public virtual void Invoke(List<Humanoid> targets, Humanoid owner) { Debug.LogError("This skill is not programmed properly"); }


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
    /// Getter for any displaying text to use.
    /// Should return a tranlstated string
    /// </summary>
    /// <returns></returns>
    public virtual string GetCost() { return null; }
    /// <summary>
    /// Getter for any displaying text to use
    /// Should return a tranlstated string
    /// </summary>
    /// <returns></returns>
    public virtual string GetScaling() { return null; }
    /// <summary>
    /// Gets the translated name for this Talent
    /// </summary>
    /// <returns></returns>
    public virtual string GetName() { return LocalizationManager.Instance.ReadUIDictionary(Name); }
    /// <summary>
    /// Gets the translted description for this talent
    /// </summary>
    /// <returns></returns>
    public virtual string GetDescription() { return LocalizationManager.Instance.ReadUIDictionary(Description); }

    /// <summary>
    /// Subtracts one from the Cooldown Counter
    /// </summary>
    public virtual void Tick() 
    {
        CooldownCounter--;
        if (CooldownCounter < 0) CooldownCounter = 0;
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
    /// This boolean checks pre-requisites to see if the owner can cast the talent
    /// </summary>
    /// <returns></returns>
    public virtual bool OwnerCanCast(Humanoid owner) 
    {
        return (owner.GetMana() >= GetManaCost() && owner.GetStamina() >= GetStaminaCost() && owner.GetHealth() >= GetHealthCost());
    }

}
