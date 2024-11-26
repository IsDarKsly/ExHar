using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Humanoid //Will represent any character with a Name, Health, Etc
{
    public string name { get; set; } //The name of our Character
    public CLASS spec { get; set; } //The Class of our Character

    public RACE race { get; set; } //The race of the character

    public bool gender { get; set; } //The Gender of our Character, true if male

    public string they { get => gender ? "he" : "she"; } //Sets the Pronoun for they
    public string their { get => gender ? "his" : "her"; } //Sets the Pronoun for their

    public Appearance appearance { get; set; } //The appearance of our character

    //these points have to do with the allocated points a character has chosen
    public int availablePoints { get; set; }
    public int pointStrength { get; set; }
    public int pointDexterity { get; set; }
    public int pointIntellect { get; set; }
    public int pointWisdom { get; set; }
    public int pointConstitution { get; set; }
    //Conclusion of point allocated Stats

    /// <summary>
    /// The level of this character
    /// </summary>
    public int Level { get; set; }

    /// <summary>
    /// The XP this character has
    /// </summary>
    public int XP { get; set; }

    /// <summary>
    /// The XP required for this character to level up
    /// </summary>
    public int XPrequired { get { return (int)(115 * Mathf.Pow(Level, 1.14738f)); } }


    //  These temp values get set to whatever values the corresponding stat was at when last leveled
    public int tmp_con { get; set; } = 0;
    public int tmp_str { get; set; } = 0;
    public int tmp_wis { get; set; } = 0;
    public int tmp_dex { get; set; } = 0;
    public int tmp_int { get; set; } = 0;

    /// <summary>
    /// Default constructor
    /// </summary>
    public Humanoid() 
    {
    
    }

    /// <summary>
    /// Default constructor for level 1 character
    /// </summary>
    public Humanoid(string _name, bool _gender, RACE _race, CLASS _spec, Appearance _appearance, int[] _stats )
    {
        name = _name;
        gender = _gender;
        race = _race;
        spec = _spec;
        appearance = _appearance;
        Level = 1;
        SetStatPoint(STATS.Strength, _stats[0]);
        SetStatPoint(STATS.Dexterity, _stats[1]);
        SetStatPoint(STATS.Intellect, _stats[2]);
        SetStatPoint(STATS.Wisdom, _stats[3]);
        SetStatPoint(STATS.Constitution, _stats[4]);
    }

    /// <summary>
    /// Adds some amount of XP to the character and levels them up if applicable
    /// </summary>
    /// <param name="amount"></param>
    public void AddXp(int amount) 
    {
        XP += amount;
        if (XP >= XPrequired) //    Level up!
        {
            XP = 0;
            Level++;
            availablePoints += 4;

            tmp_con = pointConstitution;
            tmp_str = pointStrength;
            tmp_dex = pointDexterity;
            tmp_wis = pointWisdom;
            tmp_int = pointIntellect;
        }
    }

    /// <summary>
    /// Resets what points have been spent
    /// </summary>
    public void ResetPoints() 
    {
        availablePoints = pointConstitution + pointStrength + pointWisdom + pointDexterity + pointIntellect;
        pointIntellect = 0;
        pointStrength = 0;
        pointDexterity = 0;
        pointConstitution = 0;
        pointWisdom = 0;
        tmp_con = pointConstitution;
        tmp_str = pointStrength;
        tmp_dex = pointDexterity;
        tmp_wis = pointWisdom;
        tmp_int = pointIntellect;
    }


    /// <summary>
    /// Used for getting a given stat
    /// Additive first, then the multipliers
    /// </summary>
    public int GetStat(STATS stat)
    {
        return (int)( (10 + GetStatAdditions(stat)) * GetStatMultipliers(stat) );
    }

    /// <summary>
    /// Returns all the Additive bonus for a given stat
    /// </summary>
    /// <param name="stat"></param>
    /// <returns></returns>
    public int GetStatAdditions(STATS stat) 
    {
        switch (stat)
        {
            case STATS.Constitution:
                return pointConstitution;
            case STATS.Strength:
                return pointStrength;
            case STATS.Dexterity:
                return pointDexterity;
            case STATS.Intellect:
                return pointIntellect;
            case STATS.Wisdom:
                return pointWisdom;
        }
        return 0;
    }

    /// <summary>
    /// Returns all the multiplicative bonuses for a given stat
    /// </summary>
    /// <param name="stat"></param>
    /// <returns></returns>
    public float GetStatMultipliers(STATS stat) 
    {
        return (1 * GetGenderMultiplier(stat) * GetClassMultiplier(stat) * GetRacialMultiplier(stat));
    }

    /// <summary>
    /// Gets this characters class multiplier for a specific stat
    /// </summary>
    /// <param name="stat"></param>
    /// <returns></returns>
    public float GetClassMultiplier(STATS stat) 
    {
        switch (spec) 
        {
            case (CLASS.MAGE):
                switch (stat) 
                {
                    case STATS.Constitution:
                        return 0.95f;
                    case STATS.Strength:
                        return 0.95f;
                    case STATS.Dexterity:
                        return 1f;
                    case STATS.Intellect:
                        return 1.05f;
                    case STATS.Wisdom:
                        return 1f;
                }
                break;
            case (CLASS.ROGUE):
                switch (stat)
                {
                    case STATS.Constitution:
                        return 0.95f;
                    case STATS.Strength:
                        return 1f;
                    case STATS.Dexterity:
                        return 1.025f;
                    case STATS.Intellect:
                        return 1f;
                    case STATS.Wisdom:
                        return 1.025f;
                }
                break;
            case (CLASS.WARRIOR):
                switch (stat)
                {
                    case STATS.Constitution:
                        return 1.025f;
                    case STATS.Strength:
                        return 1.025f;
                    case STATS.Dexterity:
                        return 1f;
                    case STATS.Intellect:
                        return 0.95f;
                    case STATS.Wisdom:
                        return 1f;
                }
                break;
        }
        return 1f;
    }

    /// <summary>
    /// Gets this characters gender multiplier for a specific stat
    /// </summary>
    /// <param name="stat"></param>
    /// <returns></returns>
    public float GetGenderMultiplier(STATS stat)
    {
        switch (gender)
        {
            case (true):
                switch (stat)
                {
                    case STATS.Constitution:
                        return 1.025f;
                    case STATS.Strength:
                        return 1.025f;
                    case STATS.Dexterity:
                        return 1f;
                    case STATS.Intellect:
                        return 1f;
                    case STATS.Wisdom:
                        return 1f;
                }
                break;
            case (false):
                switch (stat)
                {
                    case STATS.Constitution:
                        return 1f;
                    case STATS.Strength:
                        return 1f;
                    case STATS.Dexterity:
                        return 1.025f;
                    case STATS.Intellect:
                        return 1.025f;
                    case STATS.Wisdom:
                        return 1f;
                }
                break;
        }
        return 1f;
    }

    /// <summary>
    /// Gets this characters racial multiplier for a specific stat
    /// </summary>
    /// <param name="stat"></param>
    /// <returns></returns>
    public float GetRacialMultiplier(STATS stat)
    {
        switch (race)
        {
            case (RACE.Wild_One):
                switch (stat)
                {
                    case STATS.Constitution:
                        return 1f;
                    case STATS.Strength:
                        return 1f;
                    case STATS.Dexterity:
                        return 1f;
                    case STATS.Intellect:
                        return 1f;
                    case STATS.Wisdom:
                        return 1.05f;
                }
                break;
            case (RACE.Arenaen):
                switch (stat)
                {
                    case STATS.Constitution:
                        return 1.05f;
                    case STATS.Strength:
                        return 1f;
                    case STATS.Dexterity:
                        return 1f;
                    case STATS.Intellect:
                        return 1f;
                    case STATS.Wisdom:
                        return 1f;
                }
                break;
            case (RACE.Westerner):
                switch (stat)
                {
                    case STATS.Constitution:
                        return 1f;
                    case STATS.Strength:
                        return 1.05f;
                    case STATS.Dexterity:
                        return 1f;
                    case STATS.Intellect:
                        return 1f;
                    case STATS.Wisdom:
                        return 1f;
                }
                break;
            case (RACE.Avition):
                switch (stat)
                {
                    case STATS.Constitution:
                        return 1f;
                    case STATS.Strength:
                        return 1f;
                    case STATS.Dexterity:
                        return 1f;
                    case STATS.Intellect:
                        return 1.05f;
                    case STATS.Wisdom:
                        return 1f;
                }
                break;
            case (RACE.Novun):
                switch (stat)
                {
                    case STATS.Constitution:
                        return 1f;
                    case STATS.Strength:
                        return 1f;
                    case STATS.Dexterity:
                        return 1f;
                    case STATS.Intellect:
                        return 1.025f;
                    case STATS.Wisdom:
                        return 1.025f;
                }
                break;
            case (RACE.Umbran):
                switch (stat)
                {
                    case STATS.Constitution:
                        return 1f;
                    case STATS.Strength:
                        return 1f;
                    case STATS.Dexterity:
                        return 1.05f;
                    case STATS.Intellect:
                        return 1f;
                    case STATS.Wisdom:
                        return 1f;
                }
                break;
        }
        return 1f;
    }

    /// <summary>
    /// Used for setting stat points.
    /// </summary>
    public void SetStatPoint(STATS stat, int points) 
    {
        if ((points) < 0 || (points) > 89) 
        {
            Debug.Log("Invalid Point num");
            return;
        } 

        switch (stat)
        {
            case STATS.Constitution:
                pointConstitution = points;
                break;
            case STATS.Strength:
                pointStrength = points;
                break;
            case STATS.Dexterity:
                pointDexterity = points;
                break;
            case STATS.Intellect:
                pointIntellect = points;
                break;
            case STATS.Wisdom:
                pointWisdom = points;
                break;
        }

        if ((pointConstitution+pointStrength+pointIntellect+pointWisdom+pointDexterity) > (10 + ((Level-1)*4))) 
        {
            Debug.LogError($"Nu-uh {stat}, {points}, {(10 + (Level * 4))}");
            Application.Quit();
        }

    }



    /// <summary>
    /// A variation of SetStatPoints for when a player levels up. We don't want the player reducing a given stat to before the level. So we restrict
    /// </summary>
    public void LevelStatPoint(STATS stat, int points)
    {
        switch (stat)
        {
            case STATS.Constitution:
                if ( (pointConstitution+points) < tmp_con || (pointConstitution + points) > 89 ) return;
                pointConstitution += points;
                availablePoints -= points;
                break;
            case STATS.Strength:
                if ((pointStrength + points) < tmp_str || (pointStrength + points) > 89) return;
                pointStrength += points;
                availablePoints -= points;
                break;
            case STATS.Dexterity:
                if ((pointDexterity + points) < tmp_dex || (pointDexterity + points) > 89) return;
                pointDexterity += points;
                availablePoints -= points;
                break;
            case STATS.Intellect:
                if ((pointIntellect + points) < tmp_int || (pointIntellect + points) > 89) return;
                pointIntellect += points;
                availablePoints -= points;
                break;
            case STATS.Wisdom:
                if ((pointWisdom + points) < tmp_wis || (pointWisdom + points) > 89) return;
                pointWisdom += points;
                availablePoints -= points;
                break;
        }

    }

    /// <summary>
    /// Sets the available points to a new value
    /// </summary>
    /// <param name="points"></param>
    public void SetAvailablePoints(int points) 
    {
        availablePoints = points;
    }

    /// <summary>
    /// adds the available points with a new value
    /// </summary>
    /// <param name="points"></param>
    public void AddAvailablePoints(int points)
    {
        availablePoints += points;
    }

    /// <summary>
    /// Sets the Race of a character
    /// </summary>
    public void SetRace(RACE r) 
    {
        race = r;
    }

    /// <summary>
    /// Sets the class of a character
    /// </summary>
    public void SetClass(CLASS c) 
    {
        spec = c;
    }

    /// <summary>
    /// Sets the appearance of a character
    /// </summary>
    public void SetAppearance(Appearance app) 
    {
        appearance = app;
    }

    /// <summary>
    /// Sets the name of this character
    /// </summary>
    /// <param name="n"></param>
    public void SetName(string n) 
    {
        name = n;
    }

    /// <summary>
    /// true for male I think,
    /// false for female. maybe...
    /// man i wish there were a simpler way to represent this
    /// </summary>
    /// <param name="g"></param>
    public void SetGender(bool g) 
    {
        gender = g;
    }

}

public enum CLASS { MAGE, ROGUE, WARRIOR } //Our enums representing a characters class
public enum RACE { Wild_One, Arenaen, Westerner, Avition, Novun, Umbran } //Our enums representing a characters race

public enum STATS { Constitution, Strength, Dexterity, Intellect, Wisdom } //Our enum representing a stat, this simply helps with getting and setting