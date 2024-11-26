using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The roster will have the list of recruited members as well as the immediate members in the party
/// </summary>
[System.Serializable]
public class Roster
{
    public List<Humanoid> rosterlist = new List<Humanoid>();
    public Humanoid[] playerparty = new Humanoid[4];

    /// <summary>
    /// Default constructor
    /// </summary>
    public Roster() 
    {
    
    }

}
