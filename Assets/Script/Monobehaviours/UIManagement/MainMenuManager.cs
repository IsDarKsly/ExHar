using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// Managing class for the main menu, not a singleton and will be destroyed after loading
/// </summary>
public class MainMenuManager : MonoBehaviour
{
    //  Public Variables

    public static MainMenuManager Instance;

    [SerializeField] public GameObject[] saveSlots = new GameObject[3]; //Will represent our 3 save slot gameObjects
    [SerializeField] public GameObject[] deleteSlots = new GameObject[3];   //Will represent our 3 delete slots

    //  Private Variables

    //  Private Methods

    //Called as it is loaded
    private void Awake()
    {
        if (Instance == null) //If we arent the Instance and Instance isnt null
        {
            Instance = this; //We set Instance to ourself
            StartCoroutine(WaitForDependency());
            return;
        }
        Destroy(this); //We delete ourself
    }

    /// <summary>
    /// This Coroutine the Localization manager to wait for its dependencies to exist before allowing use of any Dependent methods
    /// </summary>
    private IEnumerator WaitForDependency()
    {
        yield return new WaitUntil(() => DataManager.Instance);
        SetSaveTexts();
    }

    /// <summary>
    /// Checks to see if save files exist for the names at folders to show, sets save files if there are
    /// </summary>
    private void SetSaveTexts() 
    {
        for (int i = 0; i < 3; i++)
        {
            saveSlots[i].transform.Find("Button").GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = LocalizationManager.Instance.ReadUIDictionary("Create");
            saveSlots[i].GetComponentInChildren<LocalizableText>().is_custom = false;
            deleteSlots[i].SetActive(false);

            if (File.Exists(DataManager.FOLDERPATH + "save" + i.ToString() + @"\ex.txt")) //Checks to see if a character file exists at this location
            {
                LoadClass.LoadValue(out string x, DataManager.FOLDERPATH + "save" + i.ToString() + @"\name.txt"); //We begin by loading the name data we find at this location
                saveSlots[i].transform.Find("Button").GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = x; //We set the visual slot at this index
                saveSlots[i].GetComponentInChildren<LocalizableText>().is_custom = true;
                deleteSlots[i].SetActive(true);
            }
        }
    }

    private void createCharacter() //This will begin the character creation at the given slot
    {
        gameObject.transform.Find("character_Menu").gameObject.SetActive(false); //Finds the characterMenu and sets it to inactive
        gameObject.transform.Find("new_Menu").gameObject.SetActive(true); //Finds the new character and sets it to active
        gameObject.transform.Find("new_Menu").Find("name_Menu").gameObject.SetActive(true); //Also set the name input to active
    }

    //  Public Methods

    //Quits the Application
    public void quitApp()
    {
        Application.Quit(); //....Quits the App
    }

    public void selectSlot(int slot)
    {
        DataManager.Instance.saveSlot = slot; //Sets the saveslot
        

        if (File.Exists(DataManager.FOLDERPATH + "save" + slot.ToString() + @"\ex.txt")) //Checks to see if a character file exists at this location
        {
            Debug.Log("This will begin loading: " + DataManager.FOLDERPATH + "save" + slot.ToString() + @"\ex.txt"); //Here we will start loading the next scene
        }
        else
        {
            createCharacter(); //Runs the create character function
        }
    }
    
    /// <summary>
    /// Starts deletion process of a character
    /// </summary>
    /// <param name="i"></param>
    public void StartDelete(int i) 
    {
        MenuManager.Instance.ActivateConfirmMenu(() => { DataManager.Instance.DeleteCharacter(i); SetSaveTexts(); });
    }



}
