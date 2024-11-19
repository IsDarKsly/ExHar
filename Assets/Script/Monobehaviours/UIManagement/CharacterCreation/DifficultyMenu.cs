using UnityEngine;

public class DifficultyMenu : MonoBehaviour
{
    public void SetActive(bool b) 
    {
        gameObject.SetActive(b);
    }

    /// <summary>
    /// Chooses a difficulty mode for the game
    /// Finishing this finalizes the character creator
    /// </summary>
    /// <param name="i"></param>
    public void ChoseDifficulty(int i) 
    { 
        CharacterCreator.Instance.difficulty = (DIFFICULTY)i;

        string title = i == 0 ? LocalizationManager.Instance.ReadUIDictionary("Classic") : LocalizationManager.Instance.ReadUIDictionary("Timed");
        string desc = i == 0 ? LocalizationManager.Instance.ReadUIDictionary("ClassicDesc") : LocalizationManager.Instance.ReadUIDictionary("TimedDesc");

        MenuManager.Instance.ShowDetails(title, desc, () => { MenuManager.Instance.ActivateConfirmMenu(() => { SetActive(false); CharacterCreator.Instance.FinalizeCharacter(); }); });
    }

}
