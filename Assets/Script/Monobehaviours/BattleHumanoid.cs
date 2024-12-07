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
    /// The appearance object representing this character
    /// </summary>
    public AppearanceObj appearanceObj;

    private Button button;
    /// <summary>
    /// The color for the base board
    /// </summary>
    private Color BaseColor = new Color(0.528f, 0.456f, 0.306f, 1f);

    /// <summary>
    /// The color for the active character board
    /// </summary>
    private Color ActiveColor = new Color(0.386f, 0.332f, 0.224f, 1f);

    /// <summary>
    /// The color for the selected target board
    /// </summary>
    private Color SelectedColor = new Color(0.858f, 0.733f, 0.482f, 1f);

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

        BattleManager.Instance.OnPhaseChange.AddListener(OnPhaseChange);
        BattleManager.Instance.OnActiveSelect.AddListener(OnActiveSelect);
        BattleManager.Instance.OnTargetSelect.AddListener(OnTargetSelect);
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
        var base_col = transform.Find("Base").GetComponent<Image>();

        if (person == character) 
        {
            base_col.color = SelectedColor;
        }

    }

    /// <summary>
    /// When this is called, if the active character isnt the one attached to this object, our base color reverts back to the natural color
    /// </summary>
    public void OnActiveSelect(Humanoid person) 
    {
        var base_col = transform.Find("Base").GetComponent<Image>();

        if (person != character)
        {
            base_col.color = BaseColor;
        }
        else 
        {
            base_col.color = ActiveColor;
        }
    }

    /// <summary>
    /// When this is called, all baseboards are set to the default base board color except the active character
    /// </summary>
    /// <param name="phase"></param>
    public void OnPhaseChange(BATTLEPHASE phase) 
    {
        var base_col = transform.Find("Base").GetComponent<Image>();

        if (BattleManager.Instance.activeCharacter != character)
        {
            base_col.color = BaseColor;
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
        string title = "";
        string desc = "";

        if (character.GetType().IsSubclassOf(typeof(Enemy)))
        {
            var c = (Enemy)character;
            title = c.GetName();
            desc = c.GetDescription();
        }
        else 
        {
            title = character.Name;
        }
        MenuManager.Instance.ShowDetails(title, desc);
    }

    /// <summary>
    /// When a pointer exits this character, The interfaces shown on pointerenter should be disabled
    /// </summary>
    /// <param name="eventData"></param>
    /// <exception cref="System.NotImplementedException"></exception>
    public void OnPointerExit(PointerEventData eventData)
    {
        MenuManager.Instance.HideDetails();
    }
}
