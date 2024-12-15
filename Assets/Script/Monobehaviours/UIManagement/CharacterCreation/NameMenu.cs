using UnityEngine;

public class NameMenu : MonoBehaviour
{
    //  Public Variables
    public TMPro.TMP_Text name_Text;
    public ClassMenu classmenu;
    //  Private Variables

    //  Private Functions

    //  Public Functions

    /// <summary>
    /// Allows the player to progress once a valid size name has been entered
    /// </summary>
    public void NameEntered() 
    {
        if (name_Text.text.Length > 1) 
        {
            CharacterCreator.Instance.charName = name_Text.text;
            MenuManager.Instance.ActivateConfirmMenu(() => { SetActive(false); classmenu.SetActive(true); });
        }
    }

    /// <summary>
    /// Sets the GameObject to active or not
    /// </summary>
    public void SetActive(bool a) 
    {
        gameObject.SetActive(a);
    }
}
