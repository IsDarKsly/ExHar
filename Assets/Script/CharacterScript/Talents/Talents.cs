using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public abstract class Talents
{
    /// <summary>
    /// The current or starting level of this talent
    /// </summary>
    public int Level { get; set; }
    /// <summary>
    /// The max level this talent can reach
    /// </summary>
    public int MaxLevel { get; set; }
    /// <summary>
    /// Whether this talent can be refunded, or if it is custom
    /// </summary>
    public bool Refundable { get; set; }
    /// <summary>
    /// The name of the talent, which can be used as an identifier
    /// </summary>
    public string Name { get; set; }
    public string Description { get; set; }
    /// <summary>
    /// Will be used in setting the appearance of this talent
    /// </summary>
    public int SpriteID { get; set; }

    /// <summary>
    /// This determines how many characters are targeted by this ability
    /// It is really only used by the BattleManager to determine who should be targeted
    /// </summary>
    public TARGETTYPE TargetType { get; set; }

    /// <summary>
    /// Again, used by the BattleManager to determine who should be targeted
    /// </summary>
    public int Targets { get; set; }

    public Talents() 
    {
    
    }

    /// <summary>
    /// Levels a particular talent up
    /// </summary>
    public abstract void LevelUp();

}

public enum TARGETTYPE { SINGLE, MULTIPLE, ALLENEMIES, ALLALLIES, SELF }