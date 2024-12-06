using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PassiveTalents : Talents
{
    public PASSIVETRIGGER trigger;

    public Dictionary<STATS, int> statboost = new Dictionary<STATS, int>();
    public Dictionary<DamageType, int> defenseboost = new Dictionary<DamageType, int>();
    public Dictionary<DamageSubType, float> resistanceboost = new Dictionary<DamageSubType, float>();
    public Dictionary<DamageType, Dictionary<DamageSubType, int>> damageboost = new Dictionary<DamageType, Dictionary<DamageSubType, int>>();

    public PassiveTalents() 
    {
        damageboost[DamageType.Physical] = new Dictionary<DamageSubType, int>();
        damageboost[DamageType.Magical] = new Dictionary<DamageSubType, int>();
    }

    public int GetStatBonus(STATS stat) => statboost.ContainsKey(stat) ? statboost[stat] : 0;

    public int GetDefenseBonus(DamageType type) => defenseboost.ContainsKey(type) ? defenseboost[type] : 0;

    public float GetResistance(DamageSubType type) => resistanceboost.ContainsKey(type) ? resistanceboost[type] : 0f;

    /// <summary>
    /// Returns a damage struct if this talent provides a bonus to damage
    /// </summary>
    /// <param name="type"></param>
    /// <param name="s_type"></param>
    /// <returns></returns>
    public Damage GetBonusDamage(DamageType type, DamageSubType s_type) 
    {
        if (!damageboost[type].ContainsKey(s_type)) return new Damage(null, null);
        Dictionary<DamageType, int> t_dam = new Dictionary<DamageType, int>();
        t_dam[type] = damageboost[type][s_type];
        Dictionary<DamageSubType, float> t_per = new Dictionary<DamageSubType, float>();
        t_per[s_type] = 1.0f;

        return new Damage(t_dam, t_per);
    }

    /// <summary>
    /// Levels a particular talent up
    /// </summary>
    public override void LevelUp()
    {

    }

    /// <summary>
    /// This will invoke the skill
    /// </summary>
    /// <param name="targets"></param>
    public override void Invoke(List<Humanoid> targets, Humanoid owner)
    {

    }

    /// <summary>
    /// This invokation call takes a reference to some current instance of damage if necessary
    /// </summary>
    /// <param name="damage"></param>
    public virtual void Invoke(ref Damage damage, Humanoid owner) { }


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
    public virtual string GetName() { return null; }
    /// <summary>
    /// Gets the translted description for this talent
    /// </summary>
    /// <returns></returns>
    public virtual string GetDescription() { return null; }
}

public enum PASSIVETRIGGER { StartOfMatch, StartOfRound, EndOfRound, BeforeIAttack, BeforeImAttacked, Consistent } 