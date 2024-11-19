using UnityEngine;

public class RaceMenu : MonoBehaviour
{
    //  Public Variable

    public LooksMenu looksMenu;
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
    public void setRace(int i) //Decides what our class spec is and updates the prompt
    {
        CharacterCreator.Instance.race = (RACE)i; //Sets our class
        CharacterCreator.Instance.AppearanceObj.GetAppearance().race = (RACE)i; // set our racial appearance

        string title = ""; //We create some variables to switch around to represent the class weve chosen
        string desc = ""; //^

        switch (CharacterCreator.Instance.race) //Check the value we have chosen
        {
            case RACE.Wild_One: //Wild one?
                title = LocalizationManager.Instance.ReadUIDictionary("Wild One");
                desc = LocalizationManager.Instance.ReadUIDictionary("Wild One Desc");
                break;
            case RACE.Arenaen: //Arenaen?
                title = LocalizationManager.Instance.ReadUIDictionary("Arenaen");
                desc = LocalizationManager.Instance.ReadUIDictionary("ArenaenDesc");
                break;
            case RACE.Westerner: //Westerner?
                title = LocalizationManager.Instance.ReadUIDictionary("Westerner");
                desc = LocalizationManager.Instance.ReadUIDictionary("WesternerDesc");
                break;
            case RACE.Avition: //Avition?
                title = LocalizationManager.Instance.ReadUIDictionary("Avition");
                desc = LocalizationManager.Instance.ReadUIDictionary("AvitionDesc");
                break;
            case RACE.Novun: //Novun?
                title = LocalizationManager.Instance.ReadUIDictionary("Novun");
                desc = LocalizationManager.Instance.ReadUIDictionary("NovunDesc");
                break;
            case RACE.Umbran: //Umbran?
                title = LocalizationManager.Instance.ReadUIDictionary("Umbran");
                desc = LocalizationManager.Instance.ReadUIDictionary("UmbranDesc");
                break;
        }

        MenuManager.Instance.ShowDetails(title, desc, () => { SetActive(false); looksMenu.SetActive(true); });
        

    }
}
