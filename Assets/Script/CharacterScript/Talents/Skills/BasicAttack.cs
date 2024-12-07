using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class BasicAttack : ActiveTalents
{

    /// <summary>
    /// Here we define the main characteristics of a basic attack
    /// </summary>
    public BasicAttack()
    {
        Name = "Basic Attack";
        Description = "Basic Attack Desc";
        SpriteID = 0;
        Level = 1;
        MaxLevel = 1;
        Refundable = false;
        TargetType = TARGETTYPE.SINGLE;
        Targets = 1;
    }


    public override void Invoke(List<Humanoid> targets, Humanoid owner)
    {
        Debug.Log($"{owner.Name} is using a basic attack!");
        var target = targets[0];    //  Realistically only one target

        int targHealth = target.GetHealth();

        if (owner.MainHand == null && owner.OffHand == null)
        {
            Debug.Log($"Unarmed basic attack");
            Dictionary<DamageType, int> damagePortion = new Dictionary<DamageType, int>();
            Dictionary<DamageSubType, float> damagePercent = new Dictionary<DamageSubType, float>();
            damagePortion[DamageType.Physical] = owner.GetStat(STATS.Strength);
            damagePercent[DamageSubType.Crushing] = 1f;

            Damage damage = new Damage(damagePortion, damagePercent);

            if (owner.DidItCrit())
            {
                damage.IsCritical = true;
                var Keys = new List<DamageType>(damage.damagePortion.Keys);
                foreach (var key in Keys)
                {
                    damage.damagePortion[key] *= 2;
                }
            }

            foreach (var p in owner.PassiveTalents.FindAll(p => p.trigger == PASSIVETRIGGER.BeforeIAttack))
            {
                p.Invoke(ref damage, owner);
            }

            target.CalculateDamageTaken(damage);
        }

        // Same logic for MainHand and OffHand
        if (owner.MainHand != null && owner.MainHand.WeaponType != WeaponType.Shield)
        {
            Debug.Log($"MainHand basic attack");
            Damage damage = owner.MainHand.GetWeaponDamage(owner);
            if (owner.DidItCrit())
            {
                damage.IsCritical = true;
                var Keys = new List<DamageType>(damage.damagePortion.Keys);
                foreach (var key in Keys)
                {
                    damage.damagePortion[key] *= 2;
                }
            }

            foreach (var p in owner.PassiveTalents.FindAll(p=>p.trigger == PASSIVETRIGGER.BeforeIAttack)) 
            {
                p.Invoke(ref damage, owner);
            }

            target.CalculateDamageTaken(damage);
        }

        if (owner.OffHand != null && owner.OffHand.WeaponType != WeaponType.Shield)
        {
            Debug.Log($"Offhand basic attack");
            Damage damage = owner.OffHand.GetWeaponDamage(owner);
            if (owner.DidItCrit())
            {
                damage.IsCritical = true;
                var Keys = new List<DamageType>(damage.damagePortion.Keys);
                foreach (var key in Keys)
                {
                    damage.damagePortion[key] *= 2;
                }
            }

            foreach (var p in owner.PassiveTalents.FindAll(p => p.trigger == PASSIVETRIGGER.BeforeIAttack))
            {
                p.Invoke(ref damage, owner);
            }

            target.CalculateDamageTaken(damage);
        }

        owner.ChangeThreat(targHealth - target.GetHealth());
    }

}
