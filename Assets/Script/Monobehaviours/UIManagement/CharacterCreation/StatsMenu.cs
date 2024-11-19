using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class StatsMenu : MonoBehaviour
{
    //  Public Variables
    public GameObject statContButton;

    public TMP_Text[] statText = new TMP_Text[5];

    //  Private Variables
 
    private int remainingPoints = 10; //integer for remaining stats points to start
    private readonly string[] statName = { "Strength", "Dexterity", "Intellect", "Wisdom", "Constitution" };
    //  Private Functions

    private void Awake()
    {
        StartCoroutine(WaitForDependency());
    }

    IEnumerator WaitForDependency() 
    {
        yield return new WaitUntil(()=>(CharacterCreator.Instance && LocalizationManager.Instance && LocalizationManager.Instance.UIReady() && CharacterCreator.Instance));

        InitializeText();
    }

    /// <summary>
    /// Initially sets all the text values for each stat
    /// </summary>
    private void InitializeText() 
    {
        for (int i = 0; i < 5; i++) 
        {
            setStatText(i);
        }
    }

    /// <summary>
    /// Sets the stat text for this element
    /// </summary>
    /// <param name="i"></param>
    private void setStatText(int i) //sets the text field
    {

        statText[i].text = LocalizationManager.Instance.ReadUIDictionary(statName[i]) + " : " + CharacterCreator.Instance.stats[i];

        statText[5].text = LocalizationManager.Instance.ReadUIDictionary("Points") + $" { remainingPoints}";

        statContButton.SetActive(remainingPoints == 0); //Set active based on remaining points value
    }

    //  Public Functions
    public void increase_Stat(int i) //increase stats
    {

        if (remainingPoints > 0)
        {
            CharacterCreator.Instance.stats[i] += 1;
            remainingPoints--;
        }

        setStatText(i);

    }

    public void decrease_Stat(int i) //decrease stats
    {
        if (CharacterCreator.Instance.stats[i] > 10)
        {
            CharacterCreator.Instance.stats[i] -= 1;
            remainingPoints++;
        }

        setStatText(i);
    }


    /// <summary>
    /// Will display this specific stat on the menu display panel
    /// </summary>
    /// <param name="i"></param>
    public void displayStat(int i) //Displays the hovered stat
    {

        string title = LocalizationManager.Instance.ReadUIDictionary(statName[i]);
        string desc = LocalizationManager.Instance.ReadUIDictionary(statName[i]+"Desc");

        MenuManager.Instance.ShowDetails(title, desc);
    }

    /// <summary>
    /// Hides details
    /// </summary>
    public void HideDisplay() 
    {
        MenuManager.Instance.HideDetails();
    }
    /// <summary>
    /// Sets the GameObject to active or not
    /// </summary>
    public void SetActive(bool a)
    {
        gameObject.SetActive(a);
    }
}
