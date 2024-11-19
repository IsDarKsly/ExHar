using UnityEngine;

/// <summary>
/// The Audio Manager manages all audio
/// </summary>
public class AudioManager : MonoBehaviour
{
    //Public variables

    /// <summary>
    /// The Instance that serves as our interface into this object
    /// </summary>
    public static AudioManager Instance { get; private set; }

    //Private variables

    //Private methods

    private void Awake() //A singleton will allow us to destroy other Instances of this object, since we only need one
    {
        if (Instance == null) //If we arent the Instance and Instance isnt null
        {
            Instance = this; //We set Instance to ourself
            DontDestroyOnLoad(this); //Mark this gameobject to remain after loading to new scenes
            return;
        }
        Destroy(this); //We delete ourself
    }

    //Public methods
}
