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

}
