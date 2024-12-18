using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

/// <summary>
/// The Party menu will show the players active party, and will allow the player to select from the entire roster
/// of available party members. From here the player should be able to access the Talent menu, Skill menu, Inventory menu
/// for any character on either the party or the roster
/// </summary>
public class PartyMenu : MonoBehaviour
{

    /// <summary>
    /// The name of the current character
    /// </summary>
    public TMP_Text NameText;

    public GameObject[] partyslot = new GameObject[4];
    public GameObject rosterParent;
    public TMP_InputField input;

    public int SelectedSlot { get; private set; } = 4;

    private Humanoid current_character;

    /// <summary>
    /// The remove from roster button allows player to permanenty remove a character from the party
    /// </summary>
    public GameObject RemoveFromRosterButton;

    /// <summary>
    /// The SelectPlayerButton will be displayed whenever a character that is not the main character is selected
    /// </summary>
    public GameObject SelectPlayerButton;

    /// <summary>
    /// The sorting roster is specifically for when a character is added to the Datamanagers roster. We use it
    /// to sort through our characters based on relevant data.
    /// </summary>
    public List<CharacterObject> SortingRoster = new List<CharacterObject>();

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
        MenuManager.Instance.ShowHumanoidDetails(person);
    }

    /// <summary>
    /// Default hover exit action for any gameobjects in this menu
    /// </summary>
    /// <param name="person"></param>
    private void DefaultHoverExit()
    {
        MenuManager.Instance.HideHumanoidDetails();
    }

    /// <summary>
    /// Unsets this character from the party, back into the roster
    /// </summary>
    /// <param name="person"></param>
    private void UnSet(CharacterObject characterButton)
    {
        Debug.Log($"Attempting to unset {characterButton.character.Name}");

        DataManager.Instance.roster.rosterlist.Add(characterButton.character);  //Add character data back into the rosterlist
        SortingRoster.Add(characterButton); //  We add the characterButton back into the sorting roster, so that it may be sorted

        for (int i = 0; i < 4; i++)  //  Find character in party list 
        {
            if (DataManager.Instance.roster.playerparty[i] == characterButton.character)    //  This is our guy, we remove them from this data slot
            {
                DataManager.Instance.roster.playerparty[i] = null;
            }
        }
        
        characterButton.gameObject.transform.SetParent(rosterParent.transform); //  Set the gameobject to the corresponding
        if (GetCurrentCharacter() == characterButton.character) SetCurrentCharacter(characterButton.character);  //  We re-set the current character to update any changes to the character
    }

    /// <summary>
    /// When the set function is called, is adds the corresponding character into the relevant slot in the players party.
    /// </summary>
    /// <param name="person"></param>
    private void Set(CharacterObject characterButton)
    {
        //Debug.Log($"Attempting to set {characterButton.character.Name}, Slot: {SelectedSlot}");
        
        if (SelectedSlot == 4)  //  Should a player be clicked before selected slot is set, we attempt to find a relevant slot to add them to
        {
            //Debug.Log("There was no selected slot");
            for (int i = 0; i < 4; i++)
            {
                if (DataManager.Instance.roster.playerparty[i] == null) //    This slot is free
                {
                    //Debug.Log($"First open slot at {i}");
                    DataManager.Instance.roster.playerparty[i] = characterButton.character;
                    characterButton.gameObject.transform.SetParent(partyslot[i].transform);
                    break;
                }
            }
        }
        else 
        {
            if (DataManager.Instance.roster.playerparty[SelectedSlot] == null) //    This slot is free
            {
                //Debug.Log($"Adding character to slot {SelectedSlot}");
                DataManager.Instance.roster.playerparty[SelectedSlot] = characterButton.character;
                characterButton.gameObject.transform.SetParent(partyslot[SelectedSlot].transform);
            }

            RemoveSelectionOutline(SelectedSlot);   //  We remove the selection outline and reset the selected slot
            SelectedSlot = 4;
        }
        DataManager.Instance.roster.rosterlist.Remove(characterButton.character);   //  Remove the character from the roster list
        SortingRoster.Remove(characterButton);  //  We remove this character button from the sorting list, as it is in the party
        if(GetCurrentCharacter() == characterButton.character) SetCurrentCharacter(characterButton.character);  //  We re-set the current character to update any changes to the character
    }

    /// <summary>
    /// When a character object is pressed, we check to see if the character is already withing the party.
    /// If they are, we place them back in the Roster, otherwise, we attempt to add them into the party.
    /// </summary>
    public void Click(CharacterObject characterButton) 
    {

        if (DataManager.Instance.IsWithinParty(characterButton.character))   //  The character this button represents is already within the party. We will add them back into the roster
        {
            UnSet(characterButton);
        }
        else    //  The character is not within the party, we attempt to add them
        {
            Set(characterButton);
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
    /// <param name="i">Valid numbers are 0 through 3, represent a party slot</param>
    public void SelectSlot(int i)
    {
        if (i >= partyslot.Length) return;

        if (SelectedSlot == i)  //  We selected the already selected slot
        {
            RemoveSelectionOutline(SelectedSlot);   //  Remove selection outline
            SelectedSlot = 4;   //  Set to out of bounds number (effectively cancels selection)
        }
        else // otherwise
        {
            if (SelectedSlot != 4) RemoveSelectionOutline(SelectedSlot);    //  Remove the selection outline around the last valid selected slot
            SelectedSlot = i;
            AddSelectionOutline(i);
        }
    }

    /// <summary>
    /// Removes selection outline around the i'th party slot object
    /// </summary>
    public void RemoveSelectionOutline(int i) 
    {
        var color = partyslot[i].GetComponent<Outline>().effectColor;
        partyslot[i].GetComponent<Outline>().effectColor = new Color(color.r, color.g, color.b, 0f);
    }

    /// <summary>
    /// Shows selection outline around the i'th party slot object
    /// </summary>
    public void AddSelectionOutline(int i)
    {
        var color = partyslot[i].GetComponent<Outline>().effectColor;
        partyslot[i].GetComponent<Outline>().effectColor = new Color(color.r, color.g, color.b, 1f);
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
        foreach (CharacterObject obj in SortingRoster)
        {
            obj.gameObject.SetActive(false);
            if (obj.character.GetName().ToLower().Contains(input.text.ToLower()))
            {
                obj.gameObject.SetActive(true);
            }
        }
    }

    /// <summary>
    /// When a character is hovered over, this shows important information about them
    /// </summary>
    public void ShowCharacterDetails()
    {
        MenuManager.Instance.ShowHumanoidDetails(current_character);
    }

    /// <summary>
    /// When a pointer leaves the character portrait, we no longer show pertanent information (whatever that means)
    /// </summary>
    public void HideCharacterDetails()
    {
        MenuManager.Instance.HideHumanoidDetails();
    }

    /// <summary>
    /// Sorts the roster list based on class
    /// </summary>
    public void SortByClass(int i)
    {
        foreach (CharacterObject obj in SortingRoster)
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
        foreach (CharacterObject obj in SortingRoster)
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
        

        for (int i = 0; i < 4; i++) //  We set every existing party member
        {
            if (DataManager.Instance.roster.playerparty[i] != null)
            {
                CreateCharacterObject(DataManager.Instance.roster.playerparty[i], partyslot[i].transform);
            }
        }

        foreach (Humanoid person in DataManager.Instance.roster.rosterlist)     //  We set every existing roster member
        {
            var clone = CreateCharacterObject(person, rosterParent.transform);
            SortingRoster.Add(clone.GetComponent<CharacterObject>());   //  This includes adding them to the sorting list
        }

        SelectPlayerButton.GetComponentInChildren<TMP_Text>().text = DataManager.Instance.playerCharacter.GetName();

        SetCurrentCharacter(DataManager.Instance.playerCharacter);
    }

    /// <summary>
    /// This function creates a character object from the prefab. It returns the corresponding gameobject after it finishes
    /// </summary>
    /// <param name="person"></param>
    /// <param name="parent"></param>
    public GameObject CreateCharacterObject(Humanoid person, Transform parent) 
    {
        GameObject clone = Instantiate(characterPrefab, parent);
        clone.GetComponent<CharacterObject>().Initiate(person,
                () => Click(clone.GetComponent<CharacterObject>()),
                () => SetCurrentCharacter(person),
                () => DefaultHoverEnter(person),
                () => DefaultHoverExit()
                );
        return clone;
    }

    /// <summary>
    /// When a new character is added to the party, This function should be called to create this characters object
    /// </summary>
    public void AddRosterMember(Humanoid person) 
    {
        var clone = CreateCharacterObject(person, rosterParent.transform);
        SortingRoster.Add(clone.GetComponent<CharacterObject>());
    }

    /// <summary>
    /// Should the player click the permanent remove button. We remove all references of the character, and we set the current character to the player, as we know they exist.
    /// </summary>
    public void ClickPermanentRemove() 
    {
        MenuManager.Instance.ActivateConfirmMenu(() => { DataManager.Instance.PermanentlyRemove(current_character); SetCurrentCharacter(DataManager.Instance.playerCharacter); });
    }

    /// <summary>
    /// Sets the current character that the party manager should be focusing on
    /// </summary>
    /// <param name="person"></param>
    public void SetCurrentCharacter(Humanoid person) 
    {
        RemoveFromRosterButton.SetActive(false);
        SelectPlayerButton.SetActive(false);
        current_character = person;
        UpdateCurrentCharacter();
        if (person != DataManager.Instance.playerCharacter) SelectPlayerButton.SetActive(true);
        if (!person.IsCustom && !DataManager.Instance.IsWithinParty(person) && person != DataManager.Instance.playerCharacter) RemoveFromRosterButton.SetActive(true);
    }

    /// <summary>
    /// This simply sets the current character to the maincharacter
    /// </summary>
    public void SelectPlayerClick() 
    {
        SetCurrentCharacter(DataManager.Instance.playerCharacter);
    }

    /// <summary>
    /// Updates the details about the current character being displayed
    /// </summary>
    public void UpdateCurrentCharacter() 
    {
        appearnceObj.SetAppearance(current_character.appearance);

        NameText.text = current_character.GetName();
    }

    /// <summary>
    /// Destroys a roster character gameobject
    /// </summary>
    /// <param name="person"></param>
    public void DestroyRosterObject(Humanoid person) 
    {
        var CharacterObject = SortingRoster.Find(c=>c.character==person);
        SortingRoster.Remove(CharacterObject);
        Destroy(CharacterObject.gameObject);
    }

}
