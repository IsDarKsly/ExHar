using UnityEngine;

public class BattleUI : MonoBehaviour
{
    public GameObject BattlePanel;
    public GameObject CancelButton;

    public GameObject BattleHumanoidPrefab;
    public GameObject EnemyPartyParent;
    public GameObject PlayerPartyParent;

    /// <summary>
    /// Adds listeners for certain events in the BattleManager
    /// </summary>
    private void OnEnable()
    {
        BattleManager.Instance.OnPhaseChange.AddListener(ToggleBattlePanel);
        BattleManager.Instance.OnPhaseChange.AddListener(ToggleCancelAttack);
    }

    /// <summary>
    /// Removes Listeners for certain events in battle manager
    /// </summary>
    private void OnDisable()
    {
        BattleManager.Instance.OnPhaseChange.RemoveListener(ToggleBattlePanel);
        BattleManager.Instance.OnPhaseChange.RemoveListener(ToggleCancelAttack);
    }

    /// <summary>
    /// Sets this game object to whatever boolean is recieved
    /// </summary>
    public void SetActive(bool b)
    {
        gameObject.SetActive(b);
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
