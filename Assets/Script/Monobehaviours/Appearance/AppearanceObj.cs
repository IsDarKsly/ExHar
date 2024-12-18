using UnityEngine;
using UnityEngine.UI;

public class AppearanceObj : MonoBehaviour
{
    private Appearance appearance = new Appearance();

    /// <summary>
    /// Gets the Appearance class from the Appearance Obj
    /// </summary>
    /// <returns>Non-null appearance</returns>
    public Appearance GetAppearance() 
    {
        if (appearance == null) 
        {
            appearance = new Appearance();
        }
        return appearance;
    }

    /// <summary>
    /// Sets the appearance on the gameobject to the supplied appearance and updates the looks
    /// </summary>
    /// <param name="appearance">Appearance of a character</param>
    public void SetAppearance(Appearance appearance) 
    {
        GetAppearance();
        this.appearance = appearance;
        updateLooks();
    }

    /// <summary>
    /// Goes through the appearance gameobject, setting each componenet to accurately represent a character appearance
    /// </summary>
    public void updateLooks()
    {


        //Sets our image variables, Headed with an I
        var skinColorI = transform.Find("SkinColor").GetComponent<Image>();
        var SkinOutlineI = transform.Find("SkinOutline").GetComponent<Image>();
        var NoseI = transform.Find("Nose").GetComponent<Image>();
        var MouthI = transform.Find("Mouth").GetComponent<Image>();
        var EyebrowI = transform.Find("Eyebrow").GetComponent<Image>();
        var EyeOutlineI = transform.Find("EyeOutline").GetComponent<Image>();
        var EyeColorI = transform.Find("EyeColor").GetComponent<Image>();
        var ExtraColorI = transform.Find("ExtraColor").GetComponent<Image>();
        var ExtraOutlineI = transform.Find("ExtraOutline").GetComponent<Image>();
        var HairColorI = transform.Find("HairColor").GetComponent<Image>();
        var HairOutlineI = transform.Find("HairOutline").GetComponent<Image>();
        var PremadeI = transform.Find("PreMade").GetComponent<Image>();
        var Armor_OutlineI = transform.Find("Armor_Outline").GetComponent<Image>();
        var Armor_ColorI = transform.Find("Armor_Color").GetComponent<Image>();
        //Finished Image sets

        blankLooks(); //Resets the looks in case of errors

        if (appearance.PRESET != PRESETAPPEARANCE.CUSTOM)
        {
            if (appearance.PRESET == PRESETAPPEARANCE.MONSTER) 
            {
                PremadeI.sprite = SpriteManager.Instance.GetMonsterSprite(appearance.Name, InjuredStatus.HEALTHY);
                return;
            }

            PremadeI.sprite = SpriteManager.Instance.GetPremadeSprite(appearance.Name, Emotion.NEUTRAL);
            return;
        }


        //First we adjust the images to match the correct variables, note that color varibales are represented by Colors
        skinColorI.sprite = appearance.Male ? SpriteManager.Instance.maleSprites.skincolor[appearance.SkinShape] : SpriteManager.Instance.femaleSprites.skincolor[appearance.SkinShape];
        SkinOutlineI.sprite = appearance.Male ? SpriteManager.Instance.maleSprites.skinshape[appearance.SkinShape] : SpriteManager.Instance.femaleSprites.skinshape[appearance.SkinShape];
        NoseI.sprite = appearance.Male ? SpriteManager.Instance.maleSprites.nose[appearance.Nose] : SpriteManager.Instance.femaleSprites.nose[appearance.Nose];
        MouthI.sprite = appearance.Male ? SpriteManager.Instance.maleSprites.mouth[appearance.Mouth] : SpriteManager.Instance.femaleSprites.mouth[appearance.Mouth];
        EyebrowI.sprite = appearance.Male ? SpriteManager.Instance.maleSprites.eyebrow[appearance.Eyebrow] : SpriteManager.Instance.femaleSprites.eyebrow[appearance.Eyebrow];
        EyeOutlineI.sprite = appearance.Male ? SpriteManager.Instance.maleSprites.eye[appearance.Eye] : SpriteManager.Instance.femaleSprites.eye[appearance.Eye];
        EyeColorI.sprite = appearance.Male ? SpriteManager.Instance.maleSprites.eyecolor[appearance.Eye] : SpriteManager.Instance.femaleSprites.eyecolor[appearance.Eye];
        ExtraColorI.sprite = appearance.Male ? SpriteManager.Instance.maleSprites.extracolor[appearance.Extra] : SpriteManager.Instance.femaleSprites.extracolor[appearance.Extra];
        ExtraOutlineI.sprite = appearance.Male ? SpriteManager.Instance.maleSprites.extra[appearance.Extra] : SpriteManager.Instance.femaleSprites.extra[appearance.Extra];
        HairColorI.sprite = appearance.Male ? SpriteManager.Instance.maleSprites.haircolor[appearance.Hair] : SpriteManager.Instance.femaleSprites.haircolor[appearance.Hair];
        HairOutlineI.sprite = appearance.Male ? SpriteManager.Instance.maleSprites.hair[appearance.Hair] : SpriteManager.Instance.femaleSprites.hair[appearance.Hair];

        //Now we can change tones based on Colors
        skinColorI.color = SpriteManager.Instance.raceColor[(int)appearance.race].skincolors[appearance.SkinColor];
        EyebrowI.color = SpriteManager.Instance.raceColor[(int)appearance.race].haircolors[appearance.EyebrowColor];
        EyeColorI.color = SpriteManager.Instance.raceColor[(int)appearance.race].eyecolors[appearance.EyeColor];
        ExtraColorI.color = SpriteManager.Instance.raceColor[(int)appearance.race].extracolors[appearance.ExtraColor];
        HairColorI.color = SpriteManager.Instance.raceColor[(int)appearance.race].haircolors[appearance.HairColor];

        //In case of beard option
        ExtraColorI.color = (appearance.Male && appearance.Extra == 3) ? HairColorI.color : SpriteManager.Instance.raceColor[(int)appearance.race].extracolors[appearance.ExtraColor];




    }

    public void blankLooks() //Sets every image to a preset tranparent image
    {
        //Sets our image variables, Headed with an I
        var skinColorI = transform.Find("SkinColor").GetComponent<Image>();
        var SkinOutlineI = transform.Find("SkinOutline").GetComponent<Image>();
        var NoseI = transform.Find("Nose").GetComponent<Image>();
        var MouthI = transform.Find("Mouth").GetComponent<Image>();
        var EyebrowI = transform.Find("Eyebrow").GetComponent<Image>();
        var EyeOutlineI = transform.Find("EyeOutline").GetComponent<Image>();
        var EyeColorI = transform.Find("EyeColor").GetComponent<Image>();
        var ExtraColorI = transform.Find("ExtraColor").GetComponent<Image>();
        var ExtraOutlineI = transform.Find("ExtraOutline").GetComponent<Image>();
        var HairColorI = transform.Find("HairColor").GetComponent<Image>();
        var HairOutlineI = transform.Find("HairOutline").GetComponent<Image>();
        var PremadeI = transform.Find("PreMade").GetComponent<Image>();
        var Armor_OutlineI = transform.Find("Armor_Outline").GetComponent<Image>();
        var Armor_ColorI = transform.Find("Armor_Color").GetComponent<Image>();

        //First we adjust the images to match the correct variables, note that color varibales are represented by Colors
        skinColorI.sprite = SpriteManager.Instance.GetBlankSprite();
        SkinOutlineI.sprite = SpriteManager.Instance.GetBlankSprite();
        NoseI.sprite = SpriteManager.Instance.GetBlankSprite();
        MouthI.sprite = SpriteManager.Instance.GetBlankSprite();
        EyebrowI.sprite = SpriteManager.Instance.GetBlankSprite();
        EyeOutlineI.sprite = SpriteManager.Instance.GetBlankSprite();
        EyeColorI.sprite = SpriteManager.Instance.GetBlankSprite();
        ExtraColorI.sprite = SpriteManager.Instance.GetBlankSprite();
        ExtraOutlineI.sprite = SpriteManager.Instance.GetBlankSprite();
        HairColorI.sprite = SpriteManager.Instance.GetBlankSprite();
        HairOutlineI.sprite = SpriteManager.Instance.GetBlankSprite();
        PremadeI.sprite = SpriteManager.Instance.GetBlankSprite();
        Armor_ColorI.sprite = SpriteManager.Instance.GetBlankSprite();
        Armor_OutlineI.sprite = SpriteManager.Instance.GetBlankSprite();

    }
}
