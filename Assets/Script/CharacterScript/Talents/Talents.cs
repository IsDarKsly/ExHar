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

    public Talents() 
    {
    
    }

    public Talents(string name, string description, int level, int maxlevel, bool refundable, int spriteid) 
    {
        this.Name = name;
        this.Description = description;
        this.Level = level;
        this.MaxLevel = maxlevel;
        this.Refundable = refundable;
        this.SpriteID = spriteid;
    }

    /// <summary>
    /// Add talent to person
    /// </summary>
    public abstract void AddTalent(Humanoid person);

    /// <summary>
    /// Levels a particular talent up
    /// </summary>
    public abstract void LevelUp();

}
