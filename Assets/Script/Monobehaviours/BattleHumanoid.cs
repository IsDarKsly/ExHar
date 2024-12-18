using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BattleHumanoid : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    /// <summary>
    /// The character which this Gameobject represents
    /// </summary>
    public Humanoid character;

    /// <summary>
    /// The Resource Bars for this group
    /// </summary>
    public GameObject ResourceBars;

    /// <summary>
    /// HealthBar will be updated each player and enemy phase
    /// </summary>
    public RectTransform HealthBar;
    /// <summary>
    /// StaminaBar will be updated each player and enemy phase
    /// </summary>
    public RectTransform StaminaBar;
    /// <summary>
    /// ManaBar will be updated each player and enemy phase
    /// </summary>
    public RectTransform ManaBar;

    /// <summary>
    /// The outline around the active character
    /// </summary>
    public Outline outline;

    /// <summary>
    /// The appearance object representing this character
    /// </summary>
    public AppearanceObj appearanceObj;

    private Button button;
    /// <summary>
    /// The inactive color for the outline
    /// </summary>
    private Color BaseColor = new Color(0f, 0f, 0f, 0f);

    /// <summary>
    /// The color for the active character outline
    /// </summary>
    private Color ActiveColor = Color.yellow;

    /// <summary>
    /// The color for the selected target outline
    /// </summary>
    private Color SelectedColor = Color.red;

    /// <summary>
    /// This function is called when the BattleUI is cleaning up after a battle. Will remove all listeners relative to this object
    /// </summary>
    private void OnDestroy()
    {
        BattleManager.Instance.OnPhaseChange.RemoveListener(OnPhaseChange);
        BattleManager.Instance.OnActiveSelect.RemoveListener(OnActiveSelect);
        BattleManager.Instance.OnTargetSelect.RemoveListener(OnTargetSelect);
    }

    /// <summary>
    /// An override of a regular instantiate call.
    /// Should only ever be called from the prefab script
    /// </summary>
    public GameObject Instantiate(Transform parent, Humanoid person) 
    {
        var obj = Instantiate(gameObject, parent);
        var script = obj.GetComponent<BattleHumanoid>();
        script.character = person;
        script.SetUp();

        return obj;
    }

    /// <summary>
    /// Set's up important values relvant to this object
    /// </summary>
    private void SetUp() 
    {
        if (character == null) return;  //  Object should have this assigned
        appearanceObj.SetAppearance(character.appearance);
        button = GetComponent<Button>();
        button.onClick.AddListener(ClickCharacter);

        if (character.GetType().IsSubclassOf(typeof(Enemy))) 
        {
            ResourceBars.transform.localPosition = new Vector2(0, -222);
        }

        BattleManager.Instance.OnPhaseChange.AddListener(OnPhaseChange);
        BattleManager.Instance.OnActiveSelect.AddListener(OnActiveSelect);
        BattleManager.Instance.OnTargetSelect.AddListener(OnTargetSelect);
    }

    /// <summary>
    /// Updates each of the resources relative to the character
    /// </summary>
    private void UpdateResourceVisuals()
    {
        HealthBar.sizeDelta = new Vector2((int)(200 * (character.GetHealth()/(float)character.GetMaxHealth())), 5);
        StaminaBar.sizeDelta = new Vector2((int)(200 * (character.GetStamina() / (float)character.GetMaxStamina())), 5);
        ManaBar.sizeDelta = new Vector2((int)(200 * (character.GetMana() / (float)character.GetMaxMana())), 5);
    }

    /// <summary>
    /// When executed, will update the character appearance attached to this object
    /// </summary>
    public void UpdateAppearance() 
    {
        appearanceObj.updateLooks();
    }

    /// <summary>
    /// When this function is called, it checks if the referenced humanoid belongs to this object
    /// and sets its colors accordingly
    /// </summary>
    public void OnTargetSelect(Humanoid person) 
    {
        if (person == character) 
        {
            outline.effectColor = SelectedColor;
        }
    }


    /// <summary>
    /// When this is called, if the active character isnt the one attached to this object, our base color reverts back to the natural color
    /// </summary>
    public void OnActiveSelect(Humanoid person) 
    {
        if (person != character)
        {
            outline.effectColor = BaseColor;
        }
        else 
        {
            outline.effectColor = ActiveColor;
        }
    }

    /// <summary>
    /// When this is called, all baseboards are set to the default base board color except the active character.
    /// Also, each resource for this character is set relative to the max resource
    /// </summary>
    /// <param name="phase"></param>
    public void OnPhaseChange(BATTLEPHASE phase) 
    {
        if (BattleManager.Instance.activeCharacter != character)
        {
            outline.effectColor = BaseColor;
        }

        if (phase == BATTLEPHASE.PLAYERTURN)
        {
            UpdateResourceVisuals();
        }
        else if (phase == BATTLEPHASE.ENEMYTURN) 
        {
            UpdateResourceVisuals();
        }
    }

    /// <summary>
    /// What happens if you click this given characters gameobject.
    /// This is dependant on the state of the Battle Manager
    /// </summary>
    public void ClickCharacter()
    {
        if (BattleManager.Instance.Phase == BATTLEPHASE.PLAYERTURN)  //  This battle phase lets us set active
        {
            if (character.GetType().IsSubclassOf(typeof(Enemy)))  //  Enemy cannot be active
            {
                return;
            }

            if(character.turn)  BattleManager.Instance.SetActive(character);

        }
        else if (BattleManager.Instance.Phase == BATTLEPHASE.TARGETSELECTION)
        {
            BattleManager.Instance.GetTarget(character);
        }
    }

    /// <summary>
    /// When a pointer enters this object, it should display some basic information about them
    /// along with displaying some health, mana, stamina values
    /// </summary>
    /// <param name="eventData"></param>
    /// <exception cref="System.NotImplementedException"></exception>
    public void OnPointerEnter(PointerEventData eventData)
    {
        MenuManager.Instance.ShowHumanoidDetails(character);
    }

    /// <summary>
    /// When a pointer exits this character, The interfaces shown on pointerenter should be disabled
    /// </summary>
    /// <param name="eventData"></param>
    /// <exception cref="System.NotImplementedException"></exception>
    public void OnPointerExit(PointerEventData eventData)
    {
        MenuManager.Instance.HideHumanoidDetails();
    }
}
