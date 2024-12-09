using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BattleSkill : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    /// <summary>
    /// This will be the skill
    /// </summary>
    public ActiveTalents Skill;

    /// <summary>
    /// This should only ever be called by the GameObjectPrefab
    /// </summary>
    public void Instantiate(ActiveTalents skill, Transform parent) 
    {
        var obj = Instantiate(gameObject, parent);
        obj.GetComponent<BattleSkill>().Skill = skill;
        obj.GetComponentInChildren<Image>().sprite = SpriteManager.Instance.GetSprite(skill.Name);
        if (!skill.OffCooldown()) obj.GetComponent<Button>().interactable = false;  // Disable button if not off cooldown
    }

    public void OnClick() 
    {
        BattleManager.Instance.StartSkillCast(Skill.Name);
        MenuManager.Instance.HideSkillDetails();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        MenuManager.Instance.ShowSkillDetails(Skill, BattleManager.Instance.activeCharacter);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        MenuManager.Instance.HideSkillDetails();
    }
}
