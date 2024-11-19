using System.Collections;
using UnityEngine;

/// <summary>
/// Overarching control of the game's state, such as transitioning between menus, levels, or game-over scenarios
/// </summary>
public class GameManager : MonoBehaviour
{
    // Public variables

    /// <summary>
    /// The Instance that serves as our interface into this object
    /// </summary>
    public static GameManager Instance { get; private set; }

    //  Private Variables

    //  Private Methods

    private void Awake() //A singleton will allow us to destroy other Instances of this object, since we only need one
    {
        if (Instance == null) //If we arent the Instance and Instance isnt null
        {
            Instance = this; //We set Instance to ourself
            StartCoroutine(WaitForDependency());
            DontDestroyOnLoad(this); //Mark this gameobject to remain after loading to new scenes
            return;
        }
        Destroy(this); //We delete ourself
    }

    /// <summary>
    /// This Coroutine the Localization manager to wait for its dependencies to exist before allowing use of any Dependent methods
    /// </summary>
    private IEnumerator WaitForDependency()
    {
        yield return new WaitUntil(() => (DataManager.Instance && LocalizationManager.Instance && MenuManager.Instance && ParticleManager.Instance));

        ParticleManager.Instance.StartParticles(ParticleType.SandStorm);
        DataManager.Instance.LoadOptions();
        LocalizationManager.Instance.ReloadUI();
        MenuManager.Instance.SetUpOptions();
        LocalizationManager.Instance.UpdateUI();

    }

    //  Public Methods

    /// <summary>
    /// Safely saves the game before quitting
    /// </summary>
    public void QuitGame() 
    {
        DataManager.Instance.SaveGame();
        Application.Quit();
    }
}
