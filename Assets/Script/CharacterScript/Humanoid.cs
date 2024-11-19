using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Humanoid //Will represent any character with a Name, Health, Etc
{
    public string name { get; set; } //The name of our Character
    public CLASS spec { get; set; } //The Class of our Character

    public RACE race { get; set; } //The race of the character

    public bool gender { get; set; } //The Gender of our Character He = true

    public string they { get => gender ? "he" : "she"; } //Sets the Pronoun for they
    public string their { get => gender ? "his" : "her"; } //Sets the Pronoun for their

    //Will need more ironing out given the different buffs a "stat" can recieve
    public int pointStrength { get; set; }
    public int pointDexterity { get; set; }
    public int pointIntellect { get; set; }
    public int pointWisdom { get; set; }
    public int pointConstitution { get; set; }
    //Conclusion of point allocated Stats

    public Appearance appearance { get; set; } //The appearance of our character

    public Humanoid() //This will be called when loading from the deserializer
    {
        
    }

}

public enum CLASS { MAGE, ROGUE, WARRIOR } //Our enums representing a characters class
public enum RACE { Wild_One, Arenaen, Westerner, Avition, Novun, Umbran } //Our enums representing a characters race