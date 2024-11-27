using UnityEngine;
[System.Serializable]
public class ActiveTalents : Talents
{

    public ActiveTalents() 
    {
    
    }

    public ActiveTalents(string name, string description, int level, int maxlevel, bool refundable, int spriteid) : base(name, description, level,  maxlevel, refundable, spriteid) 
    {
    
    }

    /// <summary>
    /// Add talent to person
    /// </summary>
    public override void AddTalent(Humanoid person) 
    {
        var copycat = person.ActiveTalents.Find(t => t.Name == this.Name);

        if (copycat != null)    //  This talent already exists in our list of talents 
        {
            if (copycat.Level < copycat.MaxLevel) copycat.LevelUp();  //  Level up that talent
            return;
        }

        person.ActiveTalents.Add(this);
    }

    /// <summary>
    /// Levels a particular talent up
    /// </summary>
    public override void LevelUp() 
    {
    
    }

}
