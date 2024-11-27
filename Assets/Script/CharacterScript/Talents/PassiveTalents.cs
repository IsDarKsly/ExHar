using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PassiveTalents : Talents
{
    public PASSIVETRIGGER trigger;

    public SerializableDictionary<STATS, int> statboost = new SerializableDictionary<STATS, int>();
    public SerializableDictionary<DamageType, int> defenseboost = new SerializableDictionary<DamageType, int>();
    public SerializableDictionary<DamageSubType, float> resistanceboost = new SerializableDictionary<DamageSubType, float>();
    public SerializableDictionary<DamageType, SerializableDictionary<DamageSubType, int>> damageboost = new SerializableDictionary<DamageType, SerializableDictionary<DamageSubType, int>>();

    public PassiveTalents() 
    {
    
    }

    public PassiveTalents(string name, string description, int level, int maxlevel, bool refundable, int spriteid) : base(name, description, level, maxlevel, refundable, spriteid)
    {
        damageboost[DamageType.Physical] = new SerializableDictionary<DamageSubType, int>();
        damageboost[DamageType.Magical] = new SerializableDictionary<DamageSubType, int>();
    }

    /// <summary>
    /// Add talent to person
    /// </summary>
    public override void AddTalent(Humanoid person)
    {
        var copycat = person.PassiveTalents.Find(t => t.Name == this.Name);

        if (copycat != null)    //  This talent already exists in our list of talents 
        {
            if(copycat.Level < copycat.MaxLevel) copycat.LevelUp();  //  Level up that talent
            return;
        }

        person.PassiveTalents.Add(this);
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

}

public enum PASSIVETRIGGER { StartOfMatch, StartOfRound, EndOfRound, BeforeIAttack, BeforeImAttacked, Consistent } 