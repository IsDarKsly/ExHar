using UnityEngine;

/// <summary>
/// The skill menu will show the list of all skills, active and passive, that a character has enabled
/// </summary>
public class SkillMenu : MonoBehaviour
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
