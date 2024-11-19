using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The class that represents all of the data around the player character
/// </summary>
[System.Serializable]
public class MainCharacter : Humanoid {

    //  Public variables
    public DIFFICULTY gamemode;

    //  Private variables
    
    //  Private functions

    //  Public functions
    public MainCharacter() //This will be called when loading from the Deserializer
    {
        
    }

}

public enum DIFFICULTY { CLASSIC, TIMED }