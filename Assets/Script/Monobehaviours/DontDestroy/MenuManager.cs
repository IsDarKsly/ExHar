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
    /// Get current character simply returns the current character denoted by the partymanager
    /// </summary>
    /// <returns></returns>
    public Humanoid GetCurrentCharacter() 
    {
        return PartyMenuObj.GetCurrentCharacter();
    }

    /// <summary>
    /// This will add a visual roster member object to the roster list
    /// </summary>
    /// <param name="person"></param>
    public void AddRosterMember(Humanoid person) 
    {
        PartyMenuObj.AddRosterMember(person);
    }

    /// <summary>
    /// Destroys a characters roster gameobject
    /// </summary>
    /// <param name="person"></param>
    public void DestroyRosterObject(Humanoid person) 
    {
        PartyMenuObj.DestroyRosterObject(person);
    }

    /// <summary>
    /// The create object for inventory function serves to create the visual component for
    /// any object added to the inventory through the datamanager
    /// </summary>
    public void CreateObjectForInventory(Item item)
    {
        InventoryMenuObj.CreateObjectForInventory(item);
    }

    /// <summary>
    /// This function should really only be called from the datamanager to remind
    /// the inventory menu what should not be displayed
    /// </summary>
    public void RemoveObjectFromInventory(Item item)
    {
        InventoryMenuObj.RemoveObjectFromInventory(item);
    }

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

    /// <summary>
    /// Toggles the partymenu state
    /// </summary>
    public void TogglePartyMenu() 
    {
        PartyMenuObj.SetActive(!PartyMenuObj.gameObject.activeSelf);
    }

    /// <summary>
    /// Toggles the inventory state
    /// </summary>
    public void ToggleInventoryMenu()
    {
        InventoryMenuObj.SetActive(!InventoryMenuObj.gameObject.activeSelf);
    }

    /// <summary>
    /// Toggles the skill menu state
    /// The skill menu is for displaying skills the user has
    /// </summary>
    public void ToggleSkillMenu()
    {
        SkillMenuObj.SetActive(!SkillMenuObj.gameObject.activeSelf);
    }

    /// <summary>
    /// Toggles the Talent menu state
    /// The talent menu shows the talent tree
    /// </summary>
    public void ToggleTalentMenu()
    {
        TalentMenuObj.SetActive(!TalentMenuObj.gameObject.activeSelf);
    }

    /// <summary>
    /// Simply turns off each menu, should be used right before an event like battle starting or dialogue
    /// </summary>
    public void TurnOffAllMenus() 
    {
        OptionMenuObj.gameObject.SetActive(false);

        ConfirmMenuObj.gameObject.SetActive(false);

        DetailsMenuObj.gameObject.SetActive(false);

        PauseMenuObj.gameObject.SetActive(false);

        PartyMenuObj.gameObject.SetActive(false);

        InventoryMenuObj.gameObject.SetActive(false);

        SkillMenuObj.gameObject.SetActive(false);

        TalentMenuObj.gameObject.SetActive(false);

        SkillDetailsMenuObj.gameObject.SetActive(false);

        CharacterDetailsMenuObj.gameObject.SetActive(false);
    }

}
