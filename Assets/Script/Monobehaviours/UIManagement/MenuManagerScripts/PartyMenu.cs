using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// The Party menu will show the players active party, and will allow the player to select from the entire roster
/// of available party members. From here the player should be able to access the Talent menu, Skill menu, Inventory menu
/// for any character on either the party or the roster
/// </summary>
public class PartyMenu : MonoBehaviour
{
    public CharacterInfoMenu characterinfo;

    /// <summary>
    /// This is a list containing texts for the current characters display info
    /// [0] Name text
    /// [1] Health text
    /// [2] Stamina text
    /// [3] Mana text
    /// [4] Constitution text
    /// [5] Strength text
    /// [6] Dexterity text
    /// [7] Intellect text
    /// [8] Wisdom text
    /// [9] Armor text
    /// [10] Resistance text
    /// </summary>
    public List<TMP_Text> infoList = new List<TMP_Text>();

    public GameObject[] partyslot = new GameObject[4];
    public GameObject rosterParent;
    public TMP_InputField input;

    public int SelectedSlot { get; private set; } = 4;


    private CharacterObject[] party = new CharacterObject[4];
    private List<CharacterObject> roster = new List<CharacterObject>();
    private Humanoid current_character;

    /// <summary>
    /// The prefab for creating new character objects
    /// </summary>
    [SerializeField] private GameObject characterPrefab;

    /// <summary>
    /// The appearance of the current selected character being displayed
    /// </summary>
    [SerializeField] private AppearanceObj appearnceObj;

    /// <summary>
    /// Default hover action for any gameobjects in this menu
    /// </summary>
    /// <param name="person"></param>
    private void DefaultHoverEnter(Humanoid person) 
    {
        characterinfo.Activate(person);
    }

    /// <summary>
    /// Default hover exit action for any gameobjects in this menu
    /// </summary>
    /// <param name="person"></param>
    private void DefaultHoverExit()
    {
        characterinfo.DeActivate();
    }

    /// <summary>
    /// Unsets this character from the party, back into the roster
    /// </summary>
    /// <param name="person"></param>
    private void UnSet(CharacterObject characterButton)
    {
        DataManager.Instance.roster.rosterlist.Add(characterButton.character);  //Add character back into roster

        for (int i = 0; i < 4; i++)  //  Find character in party list 
        {
            if (DataManager.Instance.roster.playerparty[i] == characterButton.character)    //  This is our guy
            {
                DataManager.Instance.roster.playerparty[i] = null;
            }
        }
        
        characterButton.gameObject.transform.SetParent(rosterParent.transform);
        characterButton.SetButtonEvent(() => Set(characterButton));
    }

    /// <summary>
    /// set this character int the party, from the roster
    /// </summary>
    /// <param name="person"></param>
    private void Set(CharacterObject characterButton)
    {
        //If a slot hasnt been selected, one will attempt to be found
        if (SelectedSlot == 4)
        {
            for (int i = 0; i < 4; i++)
            {
                if (DataManager.Instance.roster.playerparty[i] == null && party[i] == null) //    This slot is free
                {
                    DataManager.Instance.roster.playerparty[i] = characterButton.character;
                    party[i] = characterButton;
                    characterButton.gameObject.transform.SetParent(partyslot[i].transform);
                    characterButton.SetButtonEvent(()=>UnSet(characterButton));
                }
            }
        }
        else 
        {
            if (DataManager.Instance.roster.playerparty[SelectedSlot] == null && party[SelectedSlot] == null) //    This slot is free
            {
                DataManager.Instance.roster.playerparty[SelectedSlot] = characterButton.character;
                party[SelectedSlot] = characterButton;
                characterButton.gameObject.transform.SetParent(partyslot[SelectedSlot].transform);
                characterButton.SetButtonEvent(() => UnSet(characterButton));
            }
            SelectedSlot = 4;
        }
    }


    /// <summary>
    /// Sets the gameobject to either active or not
    /// </summary>
    /// <param name="b"></param>
    public void SetActive(bool b)
    {
        gameObject.SetActive(b);
    }

    /// <summary>
    /// Returns the current character
    /// </summary>
    /// <returns></returns>
    public Humanoid GetCurrentCharacter() 
    {
        return current_character;
    }

    /// <summary>
    /// Will mark a party slot as the next slot to be filled
    /// if this slot is already selected, deselect
    /// </summary>
    public void SelectSlot(int i)
    {
        if (i >= partyslot.Length) return;

        foreach (GameObject obj in partyslot)
        {
            obj.GetComponent<Image>().color = new Color(0.44f, 0.32f, 0.2f, 1f);
        }

        if (SelectedSlot == i)
        {
            SelectedSlot = 4;   //  Set to out of bounds number
        }
        else // otherwise
        {
            SelectedSlot = i;
            partyslot[i].GetComponent<Image>().color = new Color(0.44f, 0.32f, 0.2f, 0.25f);
        }
    }

    /// <summary>
    /// Sorts the roster list based on the name input
    /// </summary>
    public void SortByName()
    {
        if (input.text == "")
        {
            Unsort();
            return;
        }
        foreach (CharacterObject obj in roster)
        {
            obj.gameObject.SetActive(false);
            if (obj.character.name.Contains(input.text))
            {
                obj.gameObject.SetActive(true);
            }
        }
    }

    /// <summary>
    /// Sorts the roster list based on class
    /// </summary>
    public void SortByClass(int i)
    {
        foreach (CharacterObject obj in roster)
        {
            obj.gameObject.SetActive(false);
            if (obj.character.spec == (CLASS)i)
            {
                obj.gameObject.SetActive(true);
            }
        }
    }

    /// <summary>
    /// Unsorts the list
    /// </summary>
    public void Unsort()
    {
        foreach (CharacterObject obj in roster)
        {
            obj.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// When the game first loads a save file, this function will be called to populate
    /// the Roster, and the party spots
    /// </summary>
    public void Load()
    {
        current_character = DataManager.Instance.playerCharacter;

        for (int i = 0; i < 4; i++)
        {
            if (DataManager.Instance.roster.playerparty[i] != null)
            {
                GameObject clone = Instantiate(characterPrefab, partyslot[i].transform);
                clone.GetComponent<CharacterObject>().Initiate(DataManager.Instance.roster.playerparty[i],
                    () => { DefaultHoverEnter(DataManager.Instance.roster.playerparty[i]); },
                    () => { DefaultHoverExit(); },
                    () => { UnSet(clone.GetComponent<CharacterObject>()); });
            }
        }

        foreach (Humanoid person in DataManager.Instance.roster.rosterlist) 
        {
            GameObject clone = Instantiate(characterPrefab, rosterParent.transform);
            clone.GetComponent<CharacterObject>().Initiate(person,
                    () => { DefaultHoverEnter(person); },
                    () => { DefaultHoverExit(); },
                    () => { Set(clone.GetComponent<CharacterObject>()); });
        }

        UpdateCurrentCharacter();
    }

    /// <summary>
    /// Updates the details about the current character being displayed
    /// </summary>
    public void UpdateCurrentCharacter() 
    {
        appearnceObj.SetAppearance(current_character.appearance);
        appearnceObj.updateLooks();

        infoList[0].text = current_character.name;
        infoList[1].text = LocalizationManager.Instance.ReadUIDictionary("Health") + $": {current_character.GetHealth()} / {current_character.GetMaxHealth()}";
        infoList[2].text = LocalizationManager.Instance.ReadUIDictionary("Stamina") + $": {current_character.GetStamina()} / {current_character.GetMaxStamina()}";
        infoList[3].text = LocalizationManager.Instance.ReadUIDictionary("Mana") + $": {current_character.GetMana()} / {current_character.GetMaxMana()}";
        infoList[4].text = LocalizationManager.Instance.ReadUIDictionary("Constitution") + $": {current_character.GetStat(STATS.Constitution)}";
        infoList[5].text = LocalizationManager.Instance.ReadUIDictionary("Strength") + $": {current_character.GetStat(STATS.Strength)}";
        infoList[6].text = LocalizationManager.Instance.ReadUIDictionary("Dexterity") + $": {current_character.GetStat(STATS.Dexterity)}";
        infoList[7].text = LocalizationManager.Instance.ReadUIDictionary("Intellect") + $": {current_character.GetStat(STATS.Intellect)}";
        infoList[8].text = LocalizationManager.Instance.ReadUIDictionary("Wisdom") + $": {current_character.GetStat(STATS.Wisdom)}";
        infoList[9].text = LocalizationManager.Instance.ReadUIDictionary("Armor") + $": {current_character.CalculateFlatDefense(DamageType.Physical)}";
        infoList[10].text = LocalizationManager.Instance.ReadUIDictionary("Magic Resistance") + $": {current_character.CalculateFlatDefense(DamageType.Magical)}";
    }

}
