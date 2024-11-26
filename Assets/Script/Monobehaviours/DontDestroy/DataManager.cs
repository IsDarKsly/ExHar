using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// Managing save/load functionality, player progress, and any persistent data across sessions.
/// </summary>
public class DataManager : MonoBehaviour
{
    //  Public Variables

    /// <summary>
    /// Returns the game path to the /Sandstorm/ Directory
    /// </summary>
    public static string FOLDERPATH { get {return Application.persistentDataPath + @"\Sandstorm\"; } }

    /// <summary>
    /// Returns the existing save path
    /// </summary>
    public static string SAVEPATH { get {return FOLDERPATH + "save" + Instance.saveSlot.ToString(); } }

    /// <summary>
    /// The Instance that serves as our interface into this object
    /// </summary>
    public static DataManager Instance { get; private set; } //Our singleton

    /// <summary>
    /// Our main character
    /// </summary>
    public MainCharacter playerCharacter { get; private set; }

    /// <summary>
    /// Our inventory of items
    /// </summary>
    public Inventory inventory { get; private set; }

    /// <summary>
    /// The Roster of Allies, this includes active party members
    /// </summary>
    public Roster roster { get; private set; }

    /// <summary>
    /// options
    /// </summary>
    public Options options { get; private set; }

    public int saveSlot; //the current Save slot

    //  Private Variables

    //  Private Methods

    private void Awake() //A singleton will allow us to destroy other Instances of this object, since we only need one
    {
        if (Instance == null) //If we arent the Instance and Instance isnt null
        {
            Instance = this; //We set Instance to ourself

            CheckDirectories();
            DontDestroyOnLoad(this); //Mark this gameobject to remain after loading to new scenes
            return;
        }
        Destroy(this.gameObject); //We delete ourself
    }

    /// <summary>
    /// Checks to see if directories exist for file paths.
    /// </summary>
    private void CheckDirectories()
    {
        Directory.CreateDirectory(FOLDERPATH); //Quickly makes a new Folder if one does not exist

        for (int i = 0; i < 3; i++) //Creates 3 saves if they dont exist
        {
            Directory.CreateDirectory(FOLDERPATH + "save" + i.ToString()); //Code that creates the character folders
        }
    }


    //  Public Methods

    /// <summary>
    /// Will attempt to load all important data to a given slot
    /// </summary>
    public void LoadGame() //Will load the character at a given slot
    {
        
        playerCharacter = LoadClass.Load<MainCharacter>(DataManager.SAVEPATH + @"\player"); //Loads the player character and all the children classes inside     
        inventory = LoadClass.Load<Inventory>(DataManager.SAVEPATH + @"\inventory");
        roster = LoadClass.Load<Roster>(DataManager.SAVEPATH + @"\roster");

        MenuManager.Instance.PopulateMenus();
    }

    /// <summary>
    /// Will attempt to save all important data
    /// </summary>
    public void SaveGame()
    {
        SaveClass.Save<MainCharacter>(playerCharacter, DataManager.SAVEPATH + @"\player");
        SaveClass.Save<Inventory>(inventory, DataManager.SAVEPATH + @"\inventory");
        SaveClass.Save<Roster>(roster, DataManager.SAVEPATH + @"\roster");

        SaveClass.SaveValue(1, DataManager.SAVEPATH + @"\ex.txt"); //Will save the existence of a character
        SaveClass.SaveValue(playerCharacter.name, DataManager.SAVEPATH + @"\name.txt"); //Will save the characters name
    }

    /// <summary>
    /// Saves general game settings 
    /// </summary>
    public void SaveOptions() 
    {
        SaveClass.Save<Options>(options, DataManager.FOLDERPATH + "options");
    }

    /// <summary>
    /// Loads options if they do exist, otherwise creates default set of options
    /// </summary>
    public void LoadOptions() 
    {
        if (File.Exists(DataManager.FOLDERPATH + "options")) options = LoadClass.Load<Options>(DataManager.FOLDERPATH + "options");
        else 
        {
            options = new Options();
            SaveOptions();
        }
    } 


    public void CreateGame(MainCharacter character)//Will create and overwrite a character at the given slot
    {
        playerCharacter = character; //Creates a new mainCharacter based around the values in the character creator
        roster = new Roster();  //  Creates new roster
        inventory = new Inventory();    //  Creates new inventory

        SaveGame(); //Overwrites the character using the new playerCharacter value
        

        TransitionManager.Instance.FadeToBlack(3, ()=> { LoadGame(); GameManager.Instance.LoadScene("Game"); },2);
    }

    /// <summary>
    /// This will delete the data at a specific location
    /// </summary>
    /// <param name="i"></param>
    public void DeleteCharacter(int i) 
    {
        saveSlot = i;
        File.Delete(SAVEPATH+@"\ex.txt");
    }
}
