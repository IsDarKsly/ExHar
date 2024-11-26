using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCreator : MonoBehaviour //This gameobject will contain any data placed in by the character creator
{
    public AppearanceObj AppearanceObj; // Continue button for stats

    public static CharacterCreator Instance; //Our singleton


    public string charName; //A name our character chooses
    public bool Gender; //The Gender our character chooses
    public CLASS spec; //The Class our character chooses
    public RACE race; //The race our character chooses
    public DIFFICULTY difficulty;

    [NonSerialized] public int[] stats = { 10,10,10,10,10 }; //stats representation

    private void Awake() //Called upon loading
    {
        if (Instance != null && Instance != this) //If we arent the Instance and Instance isnt null
        {
            Destroy(this); //We delete ourself
        }
        else //Otherwise
        {
            Instance = this; //We set Instance to ourself
        }

    }

    /// <summary>
    /// This will be the function that summates all the chosen options into one character
    /// </summary>
    public void FinalizeCharacter()
    {
        for(int i = 0; i < stats.Length; i++) 
        {
            stats[i] -= 10;
        }
        MainCharacter character = new MainCharacter(charName, Gender, race, spec, AppearanceObj.GetAppearance(), stats, difficulty);

        DataManager.Instance.CreateGame(character);
    }


}