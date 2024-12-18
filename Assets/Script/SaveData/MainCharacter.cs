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

    /// <summary>
    /// Default constructor for level 1 character
    /// </summary>
    public MainCharacter(string _name, bool _gender, RACE _race, CLASS _spec, Appearance _appearance, int[] _stats, DIFFICULTY _gamemode) : base(_name, _gender, _race, _spec,  _appearance, _stats)
    {
        gamemode = _gamemode;

        // Experimental part
        AddActiveTalent(new FireBall());
        AddActiveTalent(new FireBall());
        //End of experimental

        ChangeHealth(GetMaxHealth());
        ChangeMana(GetMaxMana());
        ChangeStamina(GetMaxStamina());
    }

}

public enum DIFFICULTY { CLASSIC, TIMED }