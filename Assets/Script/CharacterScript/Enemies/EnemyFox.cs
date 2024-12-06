using UnityEngine;

public class EnemyFox : Enemy
{
    public EnemyFox()
    {
        Name = "Fox";
        Description = "Fox Desc";

        pointConstitution = 5;  //  50 health

        MainHand = new Weapon();
        MainHand.EquipmentValue[DamageType.Physical] = 5;

        Chest = new Armor();
        Chest.EquipmentValue[DamageType.Physical] = 5;

        AddActiveTalent(new BasicAttack());

        appearance = new Appearance();
        appearance.PRESET = PRESETAPPEARANCE.MONSTER;
        appearance.SpriteID = 0;
        
    }

    /// <summary>
    /// The fox is very simple, and will just basic attack
    /// </summary>
    public override void TakeAction()
    {
        var skill = ActiveTalents.Find(x => x.Name == "Basic Attack");  //  Get the basic attack skill

        Humanoid target = BattleManager.Instance.GetHighestThreat();
        System.Collections.Generic.List<Humanoid> targets = new System.Collections.Generic.List<Humanoid>();
        targets.Add(target);

        skill.Invoke(targets, this);
    }

}
