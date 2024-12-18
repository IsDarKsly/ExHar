using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// The Active talents class at the moment really only exists to provide a layer of
/// differentiation between Talents meant to be activated manually during combat
/// and passive talents, which trigger at specific points during specific events in combat.
/// 
/// Whether this class will truly differ from its parent class, I am not sure
/// 
/// No Object of type ActiveTalents should ever be declared. This class should always be
/// inherited by any talent class meant to be activated
/// </summary>
[System.Serializable]
public abstract class ActiveTalents : Talents
{



    public ActiveTalents() 
    {
    
    }

    /// <summary>
    /// Levels a particular talent up.
    /// Dependant on the specific talent
    /// </summary>
    public override void LevelUp() 
    {
       //Debug.LogError($"This passive skill {Name} is not programmed properly");
    }

    /// <summary>
    /// This will invoke the skill on a set of targets.
    /// Dependant on the skill
    /// </summary>
    /// <param name="targets"></param>
    public override void Invoke(List<Humanoid> targets, Humanoid owner) 
    {
       //Debug.LogError($"This passive skill {Name} is not programmed properly");
    }





}
