using UnityEngine;

/// <summary>
/// The talent menu will show all skills learned and not yet learned by a character, and allow
/// the player to select new talents to learn, and to reset talents on a character
/// </summary>
public class TalentMenu : MonoBehaviour
{
    /// <summary>
    /// Sets the gameobject to either active or not
    /// </summary>
    /// <param name="b"></param>
    public void SetActive(bool b)
    {
        gameObject.SetActive(b);
    }
}
