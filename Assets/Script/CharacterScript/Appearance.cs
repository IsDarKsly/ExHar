using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Appearance //Will control the Appearance of a character
{   
    //All of the custom parameters of a character

    public RACE race = RACE.Wild_One; //Default Wild_One

    public bool Male = true; //Default true


    private int eye = 0;
    private int eyecolor = 0;
    private int eyebrow = 0;
    private int eyebrowcolor = 0;
    private int extra = 0;
    private int extracolor = 0;
    private int hair = 0;
    private int haircolor = 0;
    private int nose = 0;
    private int mouth = 0;
    private int skinshape = 0;
    private int skincolor = 0;

    public PRESETAPPEARANCE PRESET = PRESETAPPEARANCE.CUSTOM; //Will assume custom character

    /// <summary>
    /// The name of the character this appearance belongs to. This will be used by sprite dictionaries to quickly assign Custom characters their sprites
    /// </summary>
    public string Name = "";


    //Using get and set to determ an varying value
    public int Eye //Refers to the shape of the eye
    {
        get //If we attempt to get the value, and it is out of the range of our list, we return the beggining of the corresponding list
        {
            return getValueUsingList<Sprite>(eye, (Male ? SpriteManager.Instance.maleSprites.eye : SpriteManager.Instance.femaleSprites.eye));
        }
        set //If we attempt to set a new value, but the value is not within the range, we will adjust the value to represent the beggining or end of a list
        {
            eye = setValueUsingList<Sprite>(value, (Male ? SpriteManager.Instance.maleSprites.eye : SpriteManager.Instance.femaleSprites.eye));          
        }
    }
    public int EyeColor //Refers to color of eye
    {
        get //If we attempt to get the value, and it is out of the range of our list, we return the beggining of the corresponding list
        {
            return getValueUsingList<Color>(eyecolor, SpriteManager.Instance.raceColor[(int)race].eyecolors);
        }
        set //If we attempt to set a new value, but the value is not within the range, we will adjust the value to represent the beggining or end of a list
        {
            eyecolor = setValueUsingList<Color>(value, SpriteManager.Instance.raceColor[(int)race].eyecolors);
        }
    }
    public int Eyebrow
    {
        get //If we attempt to get the value, and it is out of the range of our list, we return the beggining of the corresponding list
        {
            return getValueUsingList<Sprite>(eyebrow, (Male ? SpriteManager.Instance.maleSprites.eyebrow : SpriteManager.Instance.femaleSprites.eyebrow));
        }
        set //If we attempt to set a new value, but the value is not within the range, we will adjust the value to represent the beggining or end of a list
        {
            eyebrow = setValueUsingList<Sprite>(value, (Male ? SpriteManager.Instance.maleSprites.eyebrow : SpriteManager.Instance.femaleSprites.eyebrow));
        }
    }//Refers to Eyebrow shape
    public int EyebrowColor
    {
        get //If we attempt to get the value, and it is out of the range of our list, we return the beggining of the corresponding list
        {
            return getValueUsingList<Color>(eyebrowcolor, SpriteManager.Instance.raceColor[(int)race].haircolors);
        }
        set //If we attempt to set a new value, but the value is not within the range, we will adjust the value to represent the beggining or end of a list
        {
            eyebrowcolor = setValueUsingList<Color>(value, SpriteManager.Instance.raceColor[(int)race].haircolors);
        }
    }//Refers to Eyebrow color
    public int Extra
    {
        get //If we attempt to get the value, and it is out of the range of our list, we return the beggining of the corresponding list
        {
            return getValueUsingList<Sprite>(extra, (Male ? SpriteManager.Instance.maleSprites.extra : SpriteManager.Instance.femaleSprites.extra));
        }
        set //If we attempt to set a new value, but the value is not within the range, we will adjust the value to represent the beggining or end of a list
        {
            extra = setValueUsingList<Sprite>(value, (Male ? SpriteManager.Instance.maleSprites.extra : SpriteManager.Instance.femaleSprites.extra));
        }
    }//Refers to extra shape
    public int ExtraColor
    {
        get //If we attempt to get the value, and it is out of the range of our list, we return the beggining of the corresponding list
        {
            return getValueUsingList<Color>(extracolor, SpriteManager.Instance.raceColor[(int)race].extracolors);
        }
        set //If we attempt to set a new value, but the value is not within the range, we will adjust the value to represent the beggining or end of a list
        {
            extracolor = setValueUsingList<Color>(value, SpriteManager.Instance.raceColor[(int)race].extracolors);
        }
    }//Refers to extra color
    public int Hair
    {
        get //If we attempt to get the value, and it is out of the range of our list, we return the beggining of the corresponding list
        {
            return getValueUsingList<Sprite>(hair, (Male ? SpriteManager.Instance.maleSprites.hair : SpriteManager.Instance.femaleSprites.hair));
        }
        set //If we attempt to set a new value, but the value is not within the range, we will adjust the value to represent the beggining or end of a list
        {
            hair = setValueUsingList<Sprite>(value, (Male ? SpriteManager.Instance.maleSprites.hair : SpriteManager.Instance.femaleSprites.hair));
        }
    }//etc
    public int HairColor
    {
        get //If we attempt to get the value, and it is out of the range of our list, we return the beggining of the corresponding list
        {
            return getValueUsingList<Color>(haircolor, SpriteManager.Instance.raceColor[(int)race].haircolors);
        }
        set //If we attempt to set a new value, but the value is not within the range, we will adjust the value to represent the beggining or end of a list
        {
            haircolor = setValueUsingList<Color>(value, SpriteManager.Instance.raceColor[(int)race].haircolors);
        }
    }
    public int Nose
    {
        get //If we attempt to get the value, and it is out of the range of our list, we return the beggining of the corresponding list
        {
            return getValueUsingList<Sprite>(nose, (Male ? SpriteManager.Instance.maleSprites.nose : SpriteManager.Instance.femaleSprites.nose));
        }
        set //If we attempt to set a new value, but the value is not within the range, we will adjust the value to represent the beggining or end of a list
        {
            nose = setValueUsingList<Sprite>(value, (Male ? SpriteManager.Instance.maleSprites.nose : SpriteManager.Instance.femaleSprites.nose));
        }
    }
    public int Mouth
    {
        get //If we attempt to get the value, and it is out of the range of our list, we return the beggining of the corresponding list
        {
            return getValueUsingList<Sprite>(mouth, (Male ? SpriteManager.Instance.maleSprites.mouth : SpriteManager.Instance.femaleSprites.mouth));
        }
        set //If we attempt to set a new value, but the value is not within the range, we will adjust the value to represent the beggining or end of a list
        {
            mouth = setValueUsingList<Sprite>(value, (Male ? SpriteManager.Instance.maleSprites.mouth : SpriteManager.Instance.femaleSprites.mouth));
        }
    }
    public int SkinShape
    {
        get //If we attempt to get the value, and it is out of the range of our list, we return the beggining of the corresponding list
        {
            return getValueUsingList<Sprite>(skinshape, (Male ? SpriteManager.Instance.maleSprites.skinshape : SpriteManager.Instance.femaleSprites.skinshape));
        }
        set //If we attempt to set a new value, but the value is not within the range, we will adjust the value to represent the beggining or end of a list
        {
            skinshape = setValueUsingList<Sprite>(value, (Male ? SpriteManager.Instance.maleSprites.skinshape : SpriteManager.Instance.femaleSprites.skinshape));
        }
    }
    public int SkinColor
    {
        get //If we attempt to get the value, and it is out of the range of our list, we return the beggining of the corresponding list
        {
            return getValueUsingList<Color>(skincolor, SpriteManager.Instance.raceColor[(int)race].skincolors);
        }
        set //If we attempt to set a new value, but the value is not within the range, we will adjust the value to represent the beggining or end of a list
        {
            skincolor = setValueUsingList<Color>(value, SpriteManager.Instance.raceColor[(int)race].skincolors);
        }
    }

    //End of custom Parameters


    public int getValueUsingList<T>(int i, List<T> l) //This function will allow us get the variables more easily by checking to see if we are within range before using them
    {
        if (i >= l.Count || i < 0)  //  If our value somehow ended up outside of the bounds, return 0
            return 0;
        return i;
    }

    public int setValueUsingList<T>( int v, List<T> l) //This function will allow us to set the variables more easily while looping nicely around the list
    {
        if (v < l.Count && v >= 0)  //  If this new value is within the bounds of this list, return the value as valid
        {
            return v;
        }
        else if (v >= l.Count)  //  If this new value is greater than its corresponding list, return to the first item in the list
            return 0;
        else
            return l.Count - 1; //  If this new value is less than zero, return the last item in the list
    }

}

/// <summary>
/// This will determine whether a character is just an entire preset sprite
/// </summary>
public enum PRESETAPPEARANCE { CUSTOM, PRESET, MONSTER}

/// <summary>
/// The emotion enum can be used to represent what a character is feeling.
/// </summary>
public enum Emotion { NEUTRAL, HAPPY, SAD, ANGRY };

/// <summary>
/// A characters Injured status can be used to visibly represent how worn they are from battle
/// Healthy characters are above 80%
/// HalfHealth characters are above 40%, below 80%
/// Near Death characters are Below 40%
/// </summary>
public enum InjuredStatus { HEALTHY, HALFHEALTH, NEARDEATH};

