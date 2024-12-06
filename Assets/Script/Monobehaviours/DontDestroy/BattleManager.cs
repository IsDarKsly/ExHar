using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BattleManager : MonoBehaviour
{
    /// <summary>
    /// This will be a singleton
    /// </summary>
    public static BattleManager Instance;

    /// <summary>
    /// Who the current active character is
    /// </summary>
    public Humanoid activeCharacter;
    /// <summary>
    /// The player party
    /// </summary>
    public List<Humanoid> party;
    /// <summary>
    /// The enemy party
    /// </summary>
    public List<Enemy> enemyParty;
    /// <summary>
    /// For target selection
    /// </summary>
    public List<Humanoid> selectedTargets;
    /// <summary>
    /// The Enemies turns as a queue
    /// </summary>
    public Queue<Enemy> enemyTurnQueue;
    /// <summary>
    /// The Mode variable will help us keep track of which phase of battle we are meant to be in
    /// </summary>
    public BATTLEPHASE Phase = BATTLEPHASE.SETUP;
    /// <summary>
    /// The Mode variable will help us keep track of the previous phase. Might be useful for pausing and unpausing
    /// </summary>
    public BATTLEPHASE PreviousPhase = BATTLEPHASE.SETUP;
    /// <summary>
    /// UI component
    /// </summary>
    public BattleUI battleUI;

    /// <summary>
    /// Unity event for phase changes
    /// </summary>
    public UnityEvent<BATTLEPHASE> OnPhaseChange = new UnityEvent<BATTLEPHASE>();

    /// <summary>
    /// Unity event for choosing a target
    /// </summary>
    public UnityEvent<Humanoid> OnTargetSelect = new UnityEvent<Humanoid>();

    /// <summary>
    /// Unity event for updating statuses
    /// </summary>
    public UnityEvent<Humanoid> OnActiveSelect = new UnityEvent<Humanoid>();

    /// <summary>
    /// targets is used to keep track of how many people have been selected
    /// </summary>
    private int targets = 0;

    /// <summary>
    /// The skill that will be used should the selectedtargets goal be reached
    /// </summary>
    private string LoadedSkill;

    private void Awake()
    {
        if (Instance == null) 
        {
            Instance = this;
            party = new List<Humanoid>();
            enemyParty = new List<Enemy>();
            selectedTargets = new List<Humanoid>();
            enemyTurnQueue = new Queue<Enemy>();
            DontDestroyOnLoad(gameObject);
            return;
        }
        Destroy(gameObject);
    }

    /// <summary>
    /// Sets up any listeners at the stat of the match
    /// </summary>
    private void SetListeners() 
    {
        OnPhaseChange.AddListener(CheckPlayerPhase);
    }
    /// <summary>
    /// Destroys any listeners
    /// </summary>
    private void DestroyListeners() 
    {
        OnPhaseChange.RemoveAllListeners();
        OnTargetSelect.RemoveAllListeners();
        OnActiveSelect.RemoveAllListeners();
    }
    /// <summary>
    /// Sets the active character to the provided humanoid
    /// </summary>
    /// <param name="humanoid"></param>
    public void SetActive(Humanoid humanoid) 
    {
        Debug.Log($"Setting Active {humanoid.Name}");
        activeCharacter = humanoid;
        OnActiveSelect?.Invoke(humanoid);
    }

    /// <summary>
    /// Sets the phase and previous phase
    /// </summary>
    /// <param name="phase"></param>
    public void SetPhase(BATTLEPHASE phase) 
    {
        Debug.Log($"Setting Phase {phase.ToString()}");
        if (PreviousPhase == BATTLEPHASE.PLAYERTURN && Phase != BATTLEPHASE.PAUSE && phase == BATTLEPHASE.PLAYERTURN) //    We are changing the phase to the player phase after either an enemy turn or player animation
        { 
            ResetTimer();
        }

        PreviousPhase = Phase;
        Phase = phase;
        OnPhaseChange?.Invoke(phase);
    }

    /// <summary>
    /// Only applies to the gamemode with the timer
    /// not implemented yet
    /// </summary>
    public void ResetTimer() 
    {
    
    }

    /// <summary>
    /// When Called, will command the transition manager to load us into the battle Scene
    /// </summary>
    /// <param name="enemies"></param>
    public void StartMatch(TRANSITIONTYPE type, float time, List<Enemy> enemies) 
    {
        TransitionManager.Instance.Transition(type, time, ()=> { GameManager.Instance.LoadScene("Battle"); SetUpMatch(enemies); });
    }
    /// <summary>
    /// Initializes the battle field, sets enemy healths to their maxes if not already
    /// </summary>
    public void SetUpMatch(List<Enemy> enemies) 
    {
        SetPhase(BATTLEPHASE.SETUP);
        enemyParty = enemies;
        foreach (var enemy in enemyParty) //    Setting health for all enemies
        {
            enemy.ChangeHealth(enemy.GetMaxHealth());
        }

        party.Add(DataManager.Instance.playerCharacter);
        foreach (var member in DataManager.Instance.roster.playerparty)
        {
            if (member == null) continue;
            party.Add(member);
        }



        battleUI.SetActive(true);
        battleUI.Initiate();
        SetActive(party[0]);    //  Sets the player active
        StartPlayerTurn();  //  Player should be ready
    }

    /// <summary>
    /// Starts the enemy teams turn
    /// </summary>
    public void StartEnemyTurn() 
    {
        SetPhase(BATTLEPHASE.ENEMYTURN);
        SetActive(null);

        if (!IsPartyAlive() || !IsEnemyPartyAlive())
        {
            EndGame();
            return;
        }

        SetEnemyPartyTurns();
        foreach (var enemy in enemyParty) 
        {
            if(enemy.turn) enemyTurnQueue.Enqueue(enemy);
        }

        while (enemyTurnQueue.Count>0) 
        {
            EnemyAction(enemyTurnQueue.Dequeue());
        }

        SetPhase(BATTLEPHASE.ANIMATING);
    }

    /// <summary>
    /// Start of the player turn
    /// </summary>
    public void StartPlayerTurn() 
    {
        
        
        if (!IsPartyAlive() || !IsEnemyPartyAlive())
        {
            EndGame();
            return;
        }

        SetPartyTurns();

        foreach (var member in party)   //  Set the first available character as active
        {
            if (member.turn) 
            {
                SetActive(member);
                return;
            }
        }

        SetPhase(BATTLEPHASE.PLAYERTURN);
    }

    /// <summary>
    /// Checks on the player and enemy team during the player phase.
    /// Certain strange conditions, like all the enemies or players dying
    /// during the player phase should be taken account of
    /// </summary>
    public void CheckPlayerPhase(BATTLEPHASE phase) 
    {
        if (phase == BATTLEPHASE.PLAYERTURN)
        {
            if (!IsPartyAlive() || !IsEnemyPartyAlive())
            {
                EndGame();
                return;
            }

            if (!GetPartyTurns()) //    Player has no moves left, start enemy turn
            {
                StartEnemyTurn();
            }
        }
    }
    
    /// <summary>
    /// Manages the end of the match
    /// </summary>
    public void EndGame() 
    {
        SetPhase(BATTLEPHASE.END);
        if (!IsPartyAlive() || IsEnemyPartyAlive())
        {
            SetPhase(BATTLEPHASE.END);
            return;
        }
    }

    /// <summary>
    /// This will decide what the enemy does, likely involving choosing a skill to use
    /// </summary>
    /// <param name="enemy"></param>
    public void EnemyAction(Enemy enemy) 
    {
        selectedTargets.Clear();
        enemy.turn = false;
        enemy.TakeAction();
    }

    /// <summary>
    /// This function, when called, sets the battle manager into a state that will wait
    /// for the player to select however many targets as needed by a skill.
    /// To clarify, this function is run if a skill is selected that demands multiple target selection
    /// </summary>
    public void SetTargetMode(int targets, string Skill) 
    {
        selectedTargets.Clear();
        this.targets = targets;
        LoadedSkill = Skill;
        SetPhase(BATTLEPHASE.TARGETSELECTION);
    }

    /// <summary>
    /// Gets the target and decrements the target counter.
    /// If the target counter reaches 0, the ability is used and
    /// </summary>
    /// <param name="target"></param>
    public void GetTarget(Humanoid target) 
    {
        if (target.GetHealth() <= 0) return;    //  No targeting dead people


        selectedTargets.Add(target);
        targets--;
        OnTargetSelect.Invoke(target);  //  Invokes listeners
        if (targets <= 0) //    We have selected all as necessary and not canceled
        {
            CastSkill(LoadedSkill);
            SetPhase(BATTLEPHASE.ANIMATING);
        }
        selectedTargets.Clear();    //  Clear targets after usage
    }

    /// <summary>
    /// Signals the battle manager to begin attempting to cast the given spell
    /// </summary>
    public void StartSkillCast(string name)
    {
        Debug.Log("Starting skill cast");
        var skill = activeCharacter.ActiveTalents.Find(x => x.Name == name); //  Gets the skill if it exists (it should)
        if (skill == null)
        {
            Debug.LogError($"The skill {name} was null!");
            return;
        }

        if (!activeCharacter.CanICast(name)) return;  //  Only allow if character has resource required

        if (skill.TargetType == TARGETTYPE.SINGLE || skill.TargetType == TARGETTYPE.MULTIPLE) //    If our skill can target a character
        {
            SetTargetMode(skill.Targets, name);
        }
        else 
        {
            CastSkill(name);
        }
    }

    /// <summary>
    /// When this function is called, a skill will attempt to be cast
    /// </summary>
    public void CastSkill(string name) 
    {
        activeCharacter.turn = false;
        activeCharacter.UseSkill(name, GetTargets());
    }

    /// <summary>
    /// Returns the enemy party
    /// </summary>
    /// <returns></returns>
    public List<Enemy> GetEnemies()
    {
        return enemyParty;
    }
    /// <summary>
    /// Returns the player party
    /// </summary>
    /// <returns></returns>
    public List<Humanoid> GetParty()
    {
        return party;
    }
    /// <summary>
    /// Returns the selected targets list
    /// </summary>
    /// <returns></returns>
    public List<Humanoid> GetTargets() 
    {
        return selectedTargets;
    }

    /// <summary>
    /// Checks if anyone on the party is stil alive
    /// </summary>
    /// <returns></returns>
    public bool IsPartyAlive() 
    {
        bool b = false;

        foreach (var member in party) 
        {
            if (member.GetHealth() > 0) b = true;
        }

        return b;
    }

    /// <summary>
    /// Checks if anyone on the enemy party is stil alive
    /// </summary>
    /// <returns></returns>
    public bool IsEnemyPartyAlive()
    {
        bool b = false;

        foreach (var member in enemyParty)
        {
            if (member.GetHealth() > 0) b = true;
        }

        return b;
    }

    /// <summary>
    /// Sets each party member turn status to true if they arent stunned or dead
    /// </summary>
    public void SetPartyTurns() 
    {
        foreach (var member in party) 
        {
            if (member.IsStunned() || member.GetHealth() == 0) continue;
            member.turn = true;
        }
    }

    /// <summary>
    /// Gets whether anyone in the party has any turns left
    /// </summary>
    public bool GetPartyTurns()
    {
        bool turn = false;
        foreach (var member in party)
        {
            if (member.IsStunned() || member.GetHealth() == 0) continue;
            turn = true;
        }
        return turn;
    }

    /// <summary>
    /// Sets each enemy party member turn status to true if they arent stunned or dead
    /// </summary>
    public void SetEnemyPartyTurns()
    {
        foreach (var member in enemyParty)
        {
            if (member.IsStunned() || member.GetHealth() == 0) continue;
            member.turn = true;
        }
    }

    /// <summary>
    /// Returns the first party member alive, necessary for some targeting
    /// </summary>
    /// <returns></returns>
    public Humanoid GetFirstAlive() 
    {
        foreach(var member in party)
        {
            if (member.GetHealth() > 0) return member;
        }
        return null;
    }

    /// <summary>
    /// Returns the person with the highest threat in the party
    /// </summary>
    /// <returns></returns>
    public Humanoid GetHighestThreat() 
    {
        var target = GetFirstAlive();

        foreach (var member in party) 
        {
            if(member.GetThreat() > target.GetThreat()) target = member;
        }

        return target;
    }

    /// <summary>
    /// This returns a list of every live person in the party ordered by highest threat first to lowest threat.
    /// </summary>
    /// <returns>A sorted list of living party members based on threat level.</returns>
    public List<Humanoid> PartyByThreat()
    {
        // Create a list to hold the living members of the party
        List<Humanoid> threatlist = new List<Humanoid>();

        // Filter for living members
        foreach (var member in party)
        {
            if (member.GetHealth() > 0) // Assuming IsAlive() checks if the member's health > 0
            {
                threatlist.Add(member);
            }
        }

        // Sort the list by threat level in descending order
        threatlist.Sort((x, y) => y.GetThreat().CompareTo(x.GetThreat()));

        return threatlist;
    }

}

public enum BATTLEPHASE { SETUP, PLAYERTURN, TARGETSELECTION, ENEMYTURN, ANIMATING, PAUSE, END}