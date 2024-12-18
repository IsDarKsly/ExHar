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
    /// Animation component
    /// </summary>
    public BattleAnimationManager animationManager;

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
        //Debug.Log($"Setting Active {humanoid?.Name}");
        activeCharacter = humanoid;
        OnActiveSelect?.Invoke(humanoid);
    }

    /// <summary>
    /// Sets the phase and previous phase
    /// </summary>
    /// <param name="phase"></param>
    public void SetPhase(BATTLEPHASE phase) 
    {
        //Debug.Log($"Setting Phase {phase.ToString()}, Previous: {PreviousPhase}, current {Phase}");
        if ((PreviousPhase == BATTLEPHASE.PLAYERTURN || PreviousPhase == BATTLEPHASE.ENEMYTURN) && Phase != BATTLEPHASE.PAUSE && phase == BATTLEPHASE.PLAYERTURN) //    We are changing the phase to the player phase after either an enemy turn or player animation
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
            enemy.FullRestore();
        }

        party.Add(DataManager.Instance.playerCharacter);
        foreach (var member in DataManager.Instance.roster.playerparty)
        {
            if (member == null) continue;
            party.Add(member);
        }



        battleUI.SetActive(true);
        battleUI.Initiate();
        StartPlayerTurn();  //  Player should be ready
    }

    /// <summary>
    /// Starts the enemy teams turn
    /// </summary>
    public void StartEnemyTurn() 
    {
        //Debug.Log("Starting enemy turn!");
        SetPhase(BATTLEPHASE.ENEMYTURN);
        SetActive(null);
        TickEnemyParty();

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
       //Debug.Log("Starting player turn!");
        TickParty();
        
        if (!IsPartyAlive() || !IsEnemyPartyAlive())
        {
            EndGame();
            return;
        }

        SetPartyTurns();    //  Set turns for everyone not stunned or dead

        if (!SetNextActive())   //  We have no turns 
        {
            StartEnemyTurn();
            return;
        }

        SetPhase(BATTLEPHASE.PLAYERTURN);
    }

    /// <summary>
    /// Ticks the entire player party
    /// </summary>
    public void TickParty() 
    {
        foreach (var member in party) 
        {
            member.Tick();
        }
    }

    /// <summary>
    /// Ticks the entire enemy party
    /// </summary>
    public void TickEnemyParty() 
    {
        foreach (var member in enemyParty)
        {
            member.Tick();
        }
    }

    /// <summary>
    /// Checks on the player and enemy team during the player phase.
    /// Certain strange conditions, like all the enemies or players dying
    /// during the player phase should be taken account of
    /// </summary>
    public void AfterAnimation() 
    {
        //Debug.Log($"Checking Phase, Previous: {PreviousPhase}, current {Phase}");
        if (!SetNextActive() && PreviousPhase == BATTLEPHASE.TARGETSELECTION)   //  If the previous phase was us targeting but we have no turns left
        {
            //Debug.Log("Assumption, previous phase was our phase but no active found");
            StartEnemyTurn();
        }
        else if (PreviousPhase == BATTLEPHASE.ENEMYTURN) //  Done animating the enemy turn phase
        {
            //Debug.Log("Assumption, Previous phase was enemy phase");
            StartPlayerTurn();
        }
        else // We still have a turn left, but don't reset the turns
        {
            //Debug.Log("Assumption, Previous phase was the player phase and we have turns");
            SetPhase(BATTLEPHASE.PLAYERTURN);
        }
    }
    
    /// <summary>
    /// Manages the end of the match
    /// </summary>
    public void EndGame() 
    {
        SetPhase(BATTLEPHASE.END);
        if (!IsEnemyPartyAlive())   //  The enemies died
        {
            return;
        }
        else if (!IsPartyAlive())   //  We lost
        {
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
       //Debug.Log($"Getting target! {target.Name}! Targets left: {targets}");
        if (target.GetHealth() <= 0) return;    //  No targeting dead people

       //Debug.Log($"Adding target to list");
        selectedTargets.Add(target);
        targets--;
        OnTargetSelect.Invoke(target);  //  Invokes listeners
        if (targets <= 0) //    We have selected all as necessary and not canceled
        {
           //Debug.Log($"Target count reached, casting skill");
            CastSkill(LoadedSkill);
            SetPhase(BATTLEPHASE.ANIMATING);
            selectedTargets.Clear();    //  Clear targets after usage
        }
    }

    /// <summary>
    /// Signals the battle manager to begin attempting to cast the given spell
    /// </summary>
    public void StartSkillCast(string name)
    {
       //Debug.Log("Starting skill cast");
        var skill = activeCharacter.ActiveTalents.Find(x => x.Name == name); //  Gets the skill if it exists (it should)
        if (skill == null)
        {
           //Debug.LogError($"The skill {name} was null!");
            return;
        }

        if (!activeCharacter.CanICast(name)) //  Only allow if character has resource required
        {
           //Debug.LogError($"{activeCharacter.Name} cant cast {name}, not enough resource!");
            return;
        }  

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
       //Debug.Log($"Battle Manager: {activeCharacter.Name} is casting {name}");
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
    /// GThis function will search for the first valid member on the party
    /// and set them as the active character
    /// It returns true only if that was successful
    /// </summary>
    public bool SetNextActive()
    {
        foreach (var member in party)
        {
            if (!member.IsStunned() && member.GetHealth() > 0 && member.turn)   //  You arent stunned, have health, and a turn
            {
               //Debug.Log("Found valid member");
                SetActive(member);
                return true;
            }
        }
       //Debug.Log("Did not find valid member");
        SetActive(null);
        return false;
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