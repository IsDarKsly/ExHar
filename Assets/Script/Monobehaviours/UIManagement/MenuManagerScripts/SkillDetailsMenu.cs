using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillDetailsMenu : MonoBehaviour
{
    public GameObject DetailPanel;
    /// <summary>
    /// Displays the name of the skill
    /// </summary>
    public TMP_Text NameText;
    /// <summary>
    /// Should display level out of max IE 1/3
    /// </summary>
    public TMP_Text LevelText;
    /// <summary>
    /// The Description of the skill
    /// </summary>
    public TMP_Text DescriptionText;
    /// <summary>
    /// Technically total damage or total healing IE Damage: 5 or Healing: 5
    /// </summary>
    public TMP_Text DamageText;
    /// <summary>
    /// Cooldown Text will either display what cooldown will occur if the spell is used in White,
    /// or what the current cooldown is, in another color, maybe red
    /// </summary>
    public TMP_Text CooldownText;
    /// <summary>
    /// This displays the resource cost of a skill
    /// </summary>
    public TMP_Text CostText;
    /// <summary>
    /// The Image telling the user what type of element this skill has
    /// </summary>
    public Image SubTypeImage;

    /// <summary>
    /// The Image telling the user what type of damage this skill has
    /// </summary>
    public Image TypeImage;

    public TMP_Text StrengthScalingText;
    public TMP_Text DexScalingText;
    public TMP_Text IntScalingText;
    public TMP_Text WisScalingText;
    public TMP_Text ConstScalingText;
    public TMP_Text WeaponScalingText;

    /// <summary>
    /// Will enable the gameobject and show information about a given skill
    /// </summary>
    public void ActivateDetails(Talents skill, Humanoid humanoid) 
    {
        DetailPanel.transform.localPosition = MenuManager.Instance.mouse_is_left ? new Vector2(500, 18) : new Vector2(-500, 18);

        NameText.text = skill.GetNameText();
        DescriptionText.text = skill.GetDescriptionText();
        DamageText.text = skill.GetDamageText(humanoid);
        LevelText.text = skill.GetLevelText();
        CostText.text = skill.GetCostText();

        CooldownText.text = skill.GetCooldownText();
        if(skill.OffCooldown()) CooldownText.color = Color.white;
        else CooldownText.color = Color.red;

        SubTypeImage.sprite = SpriteManager.Instance.GetSubtypeSprite(skill.SubType);
        SubTypeImage.sprite = SpriteManager.Instance.GetSprite(skill.PrimaryType.ToString());

        StrengthScalingText.transform.parent.gameObject.SetActive(skill.GetAttributeScalingText(STATS.Strength) != null);
        DexScalingText.transform.parent.gameObject.SetActive(skill.GetAttributeScalingText(STATS.Dexterity) != null);
        IntScalingText.transform.parent.gameObject.SetActive(skill.GetAttributeScalingText(STATS.Intellect) != null);
        WisScalingText.transform.parent.gameObject.SetActive(skill.GetAttributeScalingText(STATS.Wisdom) != null);
        ConstScalingText.transform.parent.gameObject.SetActive(skill.GetAttributeScalingText(STATS.Constitution) != null);
        WeaponScalingText.transform.parent.gameObject.SetActive(skill.GetScalingText() != null);

        StrengthScalingText.text = skill.GetAttributeScalingText(STATS.Strength) + "%";
        DexScalingText.text = skill.GetAttributeScalingText(STATS.Dexterity) + "%";
        IntScalingText.text = skill.GetAttributeScalingText(STATS.Intellect) + "%";
        WisScalingText.text = skill.GetAttributeScalingText(STATS.Wisdom) + "%";
        ConstScalingText.text = skill.GetAttributeScalingText(STATS.Constitution) + "%";
        WeaponScalingText.text = skill.GetScalingText() + "%";

        gameObject.SetActive(true);
    }


    public void HideDetails() 
    {
        gameObject.SetActive(false);
    }

}
