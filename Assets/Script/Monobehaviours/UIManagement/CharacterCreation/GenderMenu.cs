using UnityEngine;

public class GenderMenu : MonoBehaviour
{
    //  Public Variables
    public ClassMenu classmenu;
    //  Private Variables

    //  Private Functions

    //  Public Functions
    /// <summary>
    /// Sets the GameObject to active or not
    /// </summary>
    public void SetActive(bool a)
    {
        gameObject.SetActive(a);
    }
     
    /// <summary>
    /// Sets gender and shows description true for male
    /// </summary>
    /// <param name="g"></param>
    public void ChooseGender(bool g) 
    {
        CharacterCreator.Instance.Gender = g;
        CharacterCreator.Instance.AppearanceObj.GetAppearance().Male = g;
        string gen = g ? LocalizationManager.Instance.ReadUIDictionary("Male") : LocalizationManager.Instance.ReadUIDictionary("Female");
        string details = g ? LocalizationManager.Instance.ReadUIDictionary("MaleDesc") : LocalizationManager.Instance.ReadUIDictionary("FemaleDesc");

        MenuManager.Instance.ShowDetails( gen, details, () => {SetActive(false); classmenu.SetActive(true); });
    }

}
