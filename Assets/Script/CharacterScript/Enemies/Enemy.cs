using System.Collections.Generic;
using UnityEngine;

public class Enemy : Humanoid
{
    public ENEMYTYPE type;
    public string Description { get; set; }

    public  int SpriteID { get; set; }
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
    /// The enemy will take action dependant on its defined type.
    /// This is subject to being overridden by derived classes
    /// </summary>
    public virtual void TakeAction() 
    {
        switch (type) 
        {
            case ENEMYTYPE.NORMAL:

                break;
            case ENEMYTYPE.SUPPORT:
                break;
            case ENEMYTYPE.ASSASSIN:
                break;
        }
    }

    /// <summary>
    /// Functionally no different from the Humanoid version save for the lost ability to DODGE
    /// </summary>
    public override void CalculateDamageTaken(Damage damage)
    {
        Debug.Log($"{Name} is under attack!");
        if (damage.damagePortion == null || damage.damagePercent == null) return;   //  No damage here


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

            dam *= (1f - CalculateResistance(perc_dam.Key));  //  Calculate player resistance (1-60% means new damage taken is 40% as effective)
            if (dam < 0) dam = 0;


            ChangeResourceBattle((int)(-1 * dam), perc_dam.Key, RESOURCES.Health, damage.IsCritical);
        }
    }

}

/// <summary>
/// Enemy type will help discern what specifically an enemy will prioritize during an enemies turn.
/// NORMAL: normals will simply use any skill on the highest threat target if its up.
/// SUPPORT: suppports will typically check if an enemy ally is low, and use healing abilites on them, otherwise they buff teammates if its up.
/// ASSASSIN: assassins will ignore threat, always targeting the lowest character on the party
/// GODLY: I really just wanted to have a godly genre of enemy, probably will have very unique properties 
/// </summary>
public enum ENEMYTYPE { NORMAL, SUPPORT, ASSASSIN, GODLY }