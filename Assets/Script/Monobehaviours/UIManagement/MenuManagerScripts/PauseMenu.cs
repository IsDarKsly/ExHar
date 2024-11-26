using UnityEngine;

/// <summary>
/// The pause menu will simply offer the ability to pause, save, and exit
/// </summary>
public class PauseMenu : MonoBehaviour
{

    /// <summary>
    /// Saves the game
    /// </summary>
    public void SaveButton()
    {
        DataManager.Instance.SaveGame();
    }

    /// <summary>
    /// Saves before exiting
    /// </summary>
    public void ExitButton()
    {
        SaveButton();
        Application.Quit();
    }

    /// <summary>
    /// Sets this scripts gameobject as active
    /// </summary>
    /// <param name="b"></param>
    public void SetActive(bool b) 
    {
        gameObject.SetActive(b);
    }

}
