using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The fireball spell hurls a Fireball at a single target
/// </summary>
[System.Serializable]
public class FireBall : ActiveTalents
{
    /// <summary>
    /// Base values for fireball
    /// </summary>
    public FireBall() 
    {
        Name = "Fireball";
        Description = "Fireball Desc";
        ManaCost = 10;
        MaxLevel = 5;
        Cooldown = 3;
        AttributeScaling[STATS.Intellect] = 0.7f;
        Targets = 1;
        TargetType = TARGETTYPE.SINGLE;
        BaseDamage = 10;
        PrimaryType = DamageType.Magical;
        SubType = DamageSubType.Fire;
        Refundable = true;
    }

    public override void LevelUp()
    {
        Level++;
        BaseDamage += 10;
    }

    public override void Invoke(List<Humanoid> targets, Humanoid owner)
    {
        CooldownCounter = Cooldown;
        Damage damage = GetDamageCalculation(owner);

        foreach (var target in targets) 
        {
            target.CalculateDamageTaken(damage);
        }
    }

    public override string GetDamageText(Humanoid owner)
    {
        return LocalizationManager.Instance.ReadUIDictionary("Damage") + ": " + base.GetDamageText(owner);
    }

}
