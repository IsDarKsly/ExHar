using TMPro;
using UnityEngine;

public class LocalizableText : MonoBehaviour
{
    //  public variables
    public string text_key;
    public bool is_custom;
    //  private variables

    // private methods

    // public methods

    /// <summary>
    /// Calls for this specific text asset to update its text to a targeted word in another language
    /// Also changes the font and whether the Text is RightToLeft as required.
    /// If text is deemed custom {Has a unique player name} nothing is updated save for the font
    /// </summary>
    public void UpdateText() 
    {
        TMP_Text text = GetComponent<TMP_Text>();

        text.font = LocalizationManager.Instance.GetFont();
        if (is_custom) return; 

        text.isRightToLeftText = LocalizationManager.Instance.IsRightToLeft();
        text.text = LocalizationManager.Instance.ReadUIDictionary(text_key);
    }


}
