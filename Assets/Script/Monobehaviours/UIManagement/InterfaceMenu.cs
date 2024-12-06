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

}
