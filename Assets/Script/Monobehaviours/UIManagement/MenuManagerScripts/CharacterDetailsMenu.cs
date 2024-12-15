using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterDetailsMenu : MonoBehaviour
{
    public GameObject DetailPanel;

    //Top of descriptions
    public TMP_Text NameText;
    public TMP_Text LevelText;
    public TMP_Text DescriptionText;
    public Image ClassImage;

    //Resources
    public TMP_Text HealthText;
    public TMP_Text ManaText;
    public TMP_Text StaminaText;

    //Stats
    public TMP_Text ConstitutionText;
    public TMP_Text StrengthText;
    public TMP_Text DexterityText;
    public TMP_Text IntelligenceText;
    public TMP_Text WisdomText;

    //Flat defenses
    public TMP_Text ArmorText;
    public TMP_Text MagicArmorText;

    //Resitances
    public TMP_Text FireResText;
    public TMP_Text IceResText;
    public TMP_Text LightningResText;
    public TMP_Text WaterResText;
    public TMP_Text LightResText;
    public TMP_Text DarkResText;
    public TMP_Text BloodResText;
    public TMP_Text PoisonResText;
    public TMP_Text SmashResText;
    public TMP_Text StabResText;
    public TMP_Text CutResText;


    /// <summary>
    /// Activate details will show all relevant information to a character.
    /// For the Description, if the character is not an enemy, It will simply display the race and gender of the character
    /// </summary>
    /// <param name="person"></param>
    public void ActivateDetails(Humanoid person) 
    {
        DetailPanel.transform.localPosition = MenuManager.Instance.mouse_is_left ? new Vector2(500, 18) : new Vector2(-500, 18);

        //Top of descriptions
        NameText.text = person.Name;
        LevelText.text = person.Level.ToString();
        DescriptionText.text = (person.GetType().IsSubclassOf(typeof(Enemy)) ? ((Enemy)person).GetDescription() : (person.race.ToString() + "\n" + (person.gender?"Male":"Female")));
        ClassImage.sprite = SpriteManager.Instance.GetSprite(person.spec.ToString());

        //Resources
        HealthText.text = person.GetHealth().ToString() + "/" + person.GetMaxHealth().ToString();
        ManaText.text = person.GetMana().ToString() + "/" + person.GetMaxMana().ToString();
        StaminaText.text = person.GetStamina().ToString() + "/" + person.GetMaxStamina().ToString();

        //Stats
        ConstitutionText.text = person.GetStat(STATS.Constitution).ToString();
        StrengthText.text = person.GetStat(STATS.Strength).ToString();
        DexterityText.text = person.GetStat(STATS.Dexterity).ToString();
        IntelligenceText.text = person.GetStat(STATS.Intellect).ToString();
        WisdomText.text = person.GetStat(STATS.Wisdom).ToString();

        //Flat defenses
        ArmorText.text = person.CalculateFlatDefense(DamageType.Physical).ToString();
        MagicArmorText.text = person.CalculateFlatDefense(DamageType.Magical).ToString();

        //Resitances
        FireResText.text = person.CalculateResistance(DamageSubType.Fire).ToString() + "%";
        IceResText.text = person.CalculateResistance(DamageSubType.Ice).ToString() + "%";
        LightningResText.text = person.CalculateResistance(DamageSubType.Lightning).ToString() + "%";
        WaterResText.text = person.CalculateResistance(DamageSubType.Water).ToString() + "%";
        LightResText.text = person.CalculateResistance(DamageSubType.Light).ToString() + "%";
        DarkResText.text = person.CalculateResistance(DamageSubType.Dark).ToString() + "%";
        BloodResText.text = person.CalculateResistance(DamageSubType.Bleeding).ToString() + "%";
        PoisonResText.text = person.CalculateResistance(DamageSubType.Poison).ToString() + "%";
        SmashResText.text = person.CalculateResistance(DamageSubType.Crushing).ToString() + "%";
        StabResText.text = person.CalculateResistance(DamageSubType.Stabbing).ToString() + "%";
        CutResText.text = person.CalculateResistance(DamageSubType.Slashing).ToString() + "%";

        gameObject.SetActive(true);
    }

    public void HideDetails()
    {
        gameObject.SetActive(false);
    }

}
