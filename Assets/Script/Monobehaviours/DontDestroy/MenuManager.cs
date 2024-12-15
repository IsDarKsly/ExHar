using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class MenuManager : MonoBehaviour
{
    // Public variables
    public OptionMenu OptionMenuObj;

    public ConfirmMenu ConfirmMenuObj;

    public DetailsMenu DetailsMenuObj;

    public PauseMenu PauseMenuObj;

    public PartyMenu PartyMenuObj;

    public InventoryMenu InventoryMenuObj;

    public SkillMenu SkillMenuObj;

    public TalentMenu TalentMenuObj;

    public SkillDetailsMenu SkillDetailsMenuObj;

    public CharacterDetailsMenu CharacterDetailsMenuObj;
    /// <summary>
    /// The Instance that serves as our interface into this object
    /// </summary>
    public static MenuManager Instance { get; private set; }

    /// <summary>
    /// Returns true if the mouse is on the left side of the screen
    /// </summary>
    public bool mouse_is_left { get { return m_position.x < (Screen.width / 2); } }

    public Vector2 m_position { get; private set; }
    //  Private Variables


    //  Private Methods

    private void Awake() //A singleton will allow us to destroy other Instances of this object, since we only need one
    {
        if (Instance == null) //If we arent the Instance and Instance isnt null
        {
            Instance = this; //We set Instance to ourself
            DontDestroyOnLoad(this); //Mark this gameobject to remain after loading to new scenes
            return;
        }
        Destroy(this.gameObject); //We delete ourself
    }

    /// <summary>
    /// Records the position of the mouse on the screens
    /// </summary>
    private void Update()
    {
        m_position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    }

    //  Public Methods

    /// <summary>
    /// Causes Options to update its data to reflect the users
    /// </summary>
    public void SetUpOptions() 
    {
        OptionMenuObj.SetUp();
    }

    /// <summary>
    /// Updates the Unity window using all the provided options
    /// Dependent on datamanager
    /// Dependent on Localization Manager
    /// </summary>
    public void UpdateFromOptions() 
    {
        // Update Resolution and Fullscreen
        Screen.SetResolution(DataManager.Instance.options.resolution.x, DataManager.Instance.options.resolution.y, DataManager.Instance.options.isFullscreen);

        LocalizationManager.Instance.ReloadUI();
        LocalizationManager.Instance.UpdateUI();

        DataManager.Instance.SaveOptions();

        if (PartyMenuObj.GetCurrentCharacter() != null) PartyMenuObj.UpdateCurrentCharacter();
    }

    /// <summary>
    /// Toggles the Confirm menu
    /// </summary>
    public void ActivateConfirmMenu(UnityAction action) 
    {
        ConfirmMenuObj.ActivateSelf(action);
    }

    /// <summary>
    /// Toggle activates the options menu
    /// </summary>
    public void ToggleOptionsMenu() 
    {
        OptionMenuObj.ToggleSelf();
        DataManager.Instance.SaveOptions();
    }

    /// <summary>
    /// Shows the provided title and description, localization is expected of the caller, not this function
    /// </summary>
    /// <param name="title"></param>
    /// <param name="details"></param>
    public void ShowDetails(string title, string details) 
    {
        DetailsMenuObj.ActiveDetails(title, details);
    }

    /// <summary>
    /// Shows the provided title and description, localization is expected of the caller, not this function
    /// Also adds an action to the Confirmation button
    /// </summary>
    /// <param name="title"></param>
    /// <param name="details"></param>
    public void ShowDetails(string title, string details, UnityAction action)
    {
        DetailsMenuObj.ActiveDetails(title, details, action);
    }

    /// <summary>
    /// Shows relevant details of a skill
    /// </summary>
    /// <param name="person"></param>
    /// <param name="skill"></param>
    public void ShowSkillDetails(ActiveTalents skill, Humanoid person)
    {  
        SkillDetailsMenuObj.ActivateDetails(skill, person);
    }

    /// <summary>
    /// Hides skill details
    /// </summary>
    public void HideSkillDetails() 
    {
        SkillDetailsMenuObj.HideDetails();
    }

    /// <summary>
    /// Shows relevant details of a character
    /// </summary>
    /// <param name="person"></param>
    /// <param name="skill"></param>
    public void ShowHumanoidDetails(Humanoid person)
    {
        CharacterDetailsMenuObj.ActivateDetails(person);
    }

    /// <summary>
    /// Hides skill details
    /// </summary>
    public void HideHumanoidDetails()
    {
        CharacterDetailsMenuObj.HideDetails();
    }

    /// <summary>
    /// Hides the details
    /// </summary>
    public void HideDetails() 
    {
        DetailsMenuObj.SetActive(false);
    }

    /// <summary>
    /// Sets the pause menu to active
    /// </summary>
    public void PauseGame() 
    {
        PauseMenuObj.SetActive(true);
    }

    /// <summary>
    /// When the game initially loads, this function will populate all relevant menus (Inventory and Party menu)
    /// </summary>
    public void PopulateMenus() 
    {
        PartyMenuObj.Load();
        InventoryMenuObj.Load();
    }
}
