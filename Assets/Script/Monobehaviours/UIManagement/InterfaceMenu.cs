using System.Collections.Generic;
using UnityEngine;

public class InterfaceMenu : MonoBehaviour
{

    public void PauseButton() 
    {
        MenuManager.Instance.PauseGame();
    }

    public void PartyButton() 
    {
        MenuManager.Instance.PartyMenuObj.SetActive(true);
    }

    public void InventoryButton() 
    {
        MenuManager.Instance.InventoryMenuObj.SetActive(true);
    }

    public void TestBattle() 
    {
        var list = new List<Enemy>();
        list.Add(new EnemyFox());
        BattleManager.Instance.StartMatch(TRANSITIONTYPE.Fade, 1.0f, list);
    }

    public void AddOneClone() 
    {
        int[] stats = { 1, 1, 1, 1, 1};
        DataManager.Instance.AddToRoster(new Humanoid("Clone", true, RACE.Westerner, CLASS.WARRIOR, new Appearance(), stats));
    }

    public void AddEquipment() 
    {
        Armor chestPiece = new Armor();
        chestPiece.Name = "Chest";
        chestPiece.Name = "Chest Desc";
        chestPiece.equipmentslot = EQUIPMENTSLOT.Chest;
        chestPiece.EquipmentValue[DamageType.Physical] = 10;
        chestPiece.StatMultipliers[STATS.Constitution] = 1f;
        Armor headPiece = new Armor();
        headPiece.Name = "Helm";
        headPiece.Name = "Helm Desc";
        headPiece.equipmentslot = EQUIPMENTSLOT.Helm;
        headPiece.EquipmentValue[DamageType.Physical] = 10;
        headPiece.StatMultipliers[STATS.Constitution] = 1f;
        DataManager.Instance.AddToInventory(chestPiece);
        DataManager.Instance.AddToInventory(headPiece);
    }

}
