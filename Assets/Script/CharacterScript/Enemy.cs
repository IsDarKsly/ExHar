using UnityEngine;

public class Enemy : Humanoid
{
    public ENEMYTYPE type;
    public string Description { get; set; }

    public  int SpriteID { get; set; }
    /// <summary>
    /// Gets the translated name for this Talent
    /// </summary>
    /// <returns></returns>
    public virtual string GetName() { return LocalizationManager.Instance.ReadUIDictionary(Name); }
    /// <summary>
    /// Gets the translted description for this talent
    /// </summary>
    /// <returns></returns>
    public virtual string GetDescription() { return LocalizationManager.Instance.ReadUIDictionary(Description); }

    /// <summary>
    /// The enemy will take action dependant on its defined type.
    /// This is subject to being overridden by derived classes
    /// </summary>
    public virtual void TakeAction() 
    {
        switch (type) 
        {
            case ENEMYTYPE.NORMAL:

                break;
            case ENEMYTYPE.SUPPORT:
                break;
            case ENEMYTYPE.ASSASSIN:
                break;
        }
    }

}

/// <summary>
/// Enemy type will help discern what specifically an enemy will prioritize during an enemies turn.
/// NORMAL: normals will simply use any skill on the highest threat target if its up.
/// SUPPORT: suppports will typically check if an enemy ally is low, and use healing abilites on them, otherwise they buff teammates if its up.
/// ASSASSIN: assassins will ignore threat, always targeting the lowest character on the party
/// GODLY: I really just wanted to have a godly genre of enemy, probably will have very unique properties 
/// </summary>
public enum ENEMYTYPE { NORMAL, SUPPORT, ASSASSIN, GODLY }