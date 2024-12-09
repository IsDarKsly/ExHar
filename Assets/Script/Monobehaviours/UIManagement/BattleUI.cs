using UnityEngine;

public class BattleUI : MonoBehaviour
{
    public GameObject BattlePanel;
    public GameObject CancelButton;

    public GameObject BattleHumanoidPrefab;
    public GameObject EnemyPartyParent;
    public GameObject PlayerPartyParent;

    /// <summary>
    /// The panel to show or hide for skill
    /// </summary>
    public GameObject SkillPanel;
    /// <summary>
    /// The actual content parent of skills
    /// </summary>
    public GameObject SkillContents;
    /// <summary>
    /// The prefab for skill objects
    /// </summary>
    public GameObject BattleSkillPrefab;

    /// <summary>
    /// Adds listeners for certain events in the BattleManager
    /// </summary>
    private void OnEnable()
    {
        if (BattleManager.Instance == null) return;
        BattleManager.Instance.OnPhaseChange.AddListener(ToggleBattlePanel);
        BattleManager.Instance.OnPhaseChange.AddListener(ToggleSkillPanel);
        BattleManager.Instance.OnPhaseChange.AddListener(ToggleCancelAttack);
        BattleManager.Instance.OnActiveSelect.AddListener(OnSkillShow);
    }

    /// <summary>
    /// Removes Listeners for certain events in battle manager
    /// </summary>
    private void OnDisable()
    {
        BattleManager.Instance.OnPhaseChange.RemoveListener(ToggleBattlePanel);
        BattleManager.Instance.OnPhaseChange.RemoveListener(ToggleSkillPanel);
        BattleManager.Instance.OnPhaseChange.RemoveListener(ToggleCancelAttack);
        BattleManager.Instance.OnActiveSelect.RemoveListener(OnSkillShow);
    }

    /// <summary>
    /// Sets this game object to whatever boolean is recieved
    /// </summary>
    public void SetActive(bool b)
    {
        gameObject.SetActive(b);
    }

    /// <summary>
    /// When the Skill button is clicked or the active character changes, we need to destroy all the children of the skill content panel
    /// and instantiate new skill gameobject for whoeever the new active character is
    /// </summary>
    public void OnSkillShow(Humanoid humanoid) 
    {
        foreach (Transform child in SkillContents.transform) 
        {
            Destroy(child.gameObject);
        }

        if (humanoid == null) return;   //  The turn is over, no more skills

        foreach (var skill in humanoid.ActiveTalents) 
        {
            BattleSkillPrefab.GetComponent<BattleSkill>().Instantiate(skill, SkillContents.transform);
        }
    }

    /// <summary>
    /// When the player clicks the skill button, we should toggle
    /// </summary>
    public void SkillButtonClick() 
    {
        if (SkillPanel.activeSelf)  //Skill panel is already active 
        {
            SkillPanel.SetActive(false);
        }
        else 
        {
            Debug.Assert(BattleManager.Instance.activeCharacter != null, "The active character is null and we are trying to click the skill button!");
            OnSkillShow(BattleManager.Instance.activeCharacter);
            SkillPanel.SetActive(true);
        }
    }

    /// <summary>
    /// This should simply happen whenever we are in a mode that isnt the player mode
    /// </summary>
    public void ToggleSkillPanel(BATTLEPHASE phase) 
    {
        if (phase != BATTLEPHASE.PLAYERTURN) 
        {
            SkillPanel.SetActive(false);
        }
    }

    /// <summary>
    /// Sets the Cancel button to active if the 
    /// </summary>
    public void ToggleCancelAttack(BATTLEPHASE phase) 
    {
        if (phase == BATTLEPHASE.TARGETSELECTION)
        {
            CancelButton.SetActive(true);
        }
        else
        {
            CancelButton.SetActive(false);
        }
    }

    /// <summary>
    /// Cancels whatever attack was about to be cast
    /// </summary>
    public void CancelAttack() 
    {
        BattleManager.Instance.SetPhase(BATTLEPHASE.PLAYERTURN);
    }

    /// <summary>
    /// Sets the battle panel to active if the phase is in player phase mode
    /// </summary>
    public void ToggleBattlePanel(BATTLEPHASE phase)
    {
        if (phase == BATTLEPHASE.PLAYERTURN)
        {
            BattlePanel.SetActive(true);
        }
        else
        {
            BattlePanel.SetActive(false);
            SkillPanel.SetActive(false);
        }
    }

    /// <summary>
    /// When the Battle manager first starts loading, gameobjects are added to represent all the characters and enemies
    /// </summary>
    public void Initiate() 
    {
        foreach (var member in BattleManager.Instance.party) 
        {
            BattleHumanoidPrefab.GetComponent<BattleHumanoid>().Instantiate(PlayerPartyParent.transform,member);
        }

        foreach (var member in BattleManager.Instance.enemyParty)
        {
            BattleHumanoidPrefab.GetComponent<BattleHumanoid>().Instantiate(EnemyPartyParent.transform, member);
        }
    }

    /// <summary>
    /// When this function is run, it searches for the gameobject corresponding to the character
    /// </summary>
    /// <param name="person"></param>
    /// <returns></returns>
    public GameObject FindCharacterObject(Humanoid person) 
    {
        foreach (var member in PlayerPartyParent.GetComponentsInChildren<BattleHumanoid>()) 
        {
            if (member != null && member.character == person) return member.gameObject;
        }

        foreach (var member in EnemyPartyParent.GetComponentsInChildren<BattleHumanoid>())
        {
            if (member != null && member.character == person) return member.gameObject;
        }
        return null;
    }


    
    /// <summary>
    /// Signals the battle manager to begin attempting to cast the given spell, This is for the basic attack
    /// </summary>
    public void StartSkillCast(string name) 
    {
        BattleManager.Instance.StartSkillCast(name);
    }
}
