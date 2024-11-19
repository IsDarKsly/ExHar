using UnityEngine;

public class ClassMenu : MonoBehaviour
{
    //  Public Variables
    public RaceMenu raceMenu;
    //  Private Variables

    //  Private Functions

    //  Public Functions
    /// <summary>
    /// Sets the GameObject to active or not
    /// </summary>
    public void SetActive(bool a)
    {
        gameObject.SetActive(a);
    }

    /// <summary>
    /// Sets our class and displays parameters of said class
    /// </summary>
    /// <param name="i"></param>
    public void setClass(int i) //Decides what our class spec is and updates the prompt
    {
        CharacterCreator.Instance.spec = (CLASS)i; //Sets our class


        string title = ""; //We create some variables to switch around to represent the class weve chosen
        string desc = ""; //^


        switch (CharacterCreator.Instance.spec) //Check the value we have chosen
        {
            case CLASS.MAGE: //Mage?
                title = LocalizationManager.Instance.ReadUIDictionary("Mage");
                desc = LocalizationManager.Instance.ReadUIDictionary("MageDesc");

                break;
            case CLASS.WARRIOR: //Warrior?
                title = LocalizationManager.Instance.ReadUIDictionary("Warrior");
                desc = LocalizationManager.Instance.ReadUIDictionary("WarriorDesc");
                break;
            case CLASS.ROGUE: //Rogue?
                title = LocalizationManager.Instance.ReadUIDictionary("Rogue");
                desc = LocalizationManager.Instance.ReadUIDictionary("RogueDesc");
                break;
        }

        MenuManager.Instance.ShowDetails(title, desc, () => { SetActive(false); raceMenu.SetActive(true); });


    }
}
