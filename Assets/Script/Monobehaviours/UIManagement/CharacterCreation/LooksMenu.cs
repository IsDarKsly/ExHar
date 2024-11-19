using System.Collections;
using UnityEngine;

public class LooksMenu : MonoBehaviour
{
    //  Public Variables
    public TMPro.TMP_Text[] appearanceText = new TMPro.TMP_Text[11]; //Stat texts, last one represents points remaining

    //  Private Variables

    //  Private Functions
    private void Start()
    {

    }

    private void OnEnable()
    {
        CharacterCreator.Instance.AppearanceObj.updateLooks();
    }

    //  Public Functions
    /// <summary>
    /// Sets the GameObject to active or not
    /// </summary>
    public void SetActive(bool a)
    {
        gameObject.SetActive(a);
    }

    public void increaseappearance(int i) //Will increase an CharacterCreator.Instance.appearance characteristic
    {
        switch (i)
        {
            case 0:
                CharacterCreator.Instance.AppearanceObj.GetAppearance().Eye++;

                break;
            case 1:
                CharacterCreator.Instance.AppearanceObj.GetAppearance().EyeColor++;

                break;
            case 2:
                CharacterCreator.Instance.AppearanceObj.GetAppearance().Hair++;

                break;
            case 3:
                CharacterCreator.Instance.AppearanceObj.GetAppearance().HairColor++;

                break;
            case 4:
                CharacterCreator.Instance.AppearanceObj.GetAppearance().Eyebrow++;

                break;
            case 5:
                CharacterCreator.Instance.AppearanceObj.GetAppearance().EyebrowColor++;

                break;
            case 6:
                CharacterCreator.Instance.AppearanceObj.GetAppearance().Mouth++;

                break;
            case 7:
                CharacterCreator.Instance.AppearanceObj.GetAppearance().Nose++;

                break;
            case 8:
                CharacterCreator.Instance.AppearanceObj.GetAppearance().SkinColor++;

                break;
            case 9:
                CharacterCreator.Instance.AppearanceObj.GetAppearance().Extra++;

                break;
            case 10:
                CharacterCreator.Instance.AppearanceObj.GetAppearance().ExtraColor++;

                break;
        }

        updateappearanceText(i);
    }

    public void decreaseappearance(int i) //Will decrease an appearance characteristic
    {
        switch (i)
        {
            case 0:
                CharacterCreator.Instance.AppearanceObj.GetAppearance().Eye--;
                break;
            case 1:
                CharacterCreator.Instance.AppearanceObj.GetAppearance().EyeColor--;
                break;
            case 2:
                CharacterCreator.Instance.AppearanceObj.GetAppearance().Hair--;
                break;
            case 3:
                CharacterCreator.Instance.AppearanceObj.GetAppearance().HairColor--;
                break;
            case 4:
                CharacterCreator.Instance.AppearanceObj.GetAppearance().Eyebrow--;
                break;
            case 5:
                CharacterCreator.Instance.AppearanceObj.GetAppearance().EyebrowColor--;
                break;
            case 6:
                CharacterCreator.Instance.AppearanceObj.GetAppearance().Mouth--;
                break;
            case 7:
                CharacterCreator.Instance.AppearanceObj.GetAppearance().Nose--;
                break;
            case 8:
                CharacterCreator.Instance.AppearanceObj.GetAppearance().SkinColor--;
                break;
            case 9:
                CharacterCreator.Instance.AppearanceObj.GetAppearance().Extra--;
                break;
            case 10:
                CharacterCreator.Instance.AppearanceObj.GetAppearance().ExtraColor--;
                break;
        }

        updateappearanceText(i);
    }

    public void updateappearanceText(int i) //Updates appearance text
    {
        switch (i)
        {
            case 0:

                appearanceText[i].text = (CharacterCreator.Instance.AppearanceObj.GetAppearance().Eye + 1).ToString();
                break;
            case 1:

                appearanceText[i].text = (CharacterCreator.Instance.AppearanceObj.GetAppearance().EyeColor + 1).ToString();
                break;
            case 2:

                appearanceText[i].text = (CharacterCreator.Instance.AppearanceObj.GetAppearance().Hair + 1).ToString();
                break;
            case 3:

                appearanceText[i].text = (CharacterCreator.Instance.AppearanceObj.GetAppearance().HairColor + 1).ToString();
                break;
            case 4:

                appearanceText[i].text = (CharacterCreator.Instance.AppearanceObj.GetAppearance().Eyebrow + 1).ToString();
                break;
            case 5:

                appearanceText[i].text = (CharacterCreator.Instance.AppearanceObj.GetAppearance().EyebrowColor + 1).ToString();
                break;
            case 6:

                appearanceText[i].text = (CharacterCreator.Instance.AppearanceObj.GetAppearance().Mouth + 1).ToString();
                break;
            case 7:

                appearanceText[i].text = (CharacterCreator.Instance.AppearanceObj.GetAppearance().Nose + 1).ToString();
                break;
            case 8:

                appearanceText[i].text = (CharacterCreator.Instance.AppearanceObj.GetAppearance().SkinColor + 1).ToString();
                break;
            case 9:

                appearanceText[i].text = (CharacterCreator.Instance.AppearanceObj.GetAppearance().Extra + 1).ToString();
                break;
            case 10:
                appearanceText[i].text = (CharacterCreator.Instance.AppearanceObj.GetAppearance().ExtraColor + 1).ToString();
                break;
        }

        CharacterCreator.Instance.AppearanceObj.updateLooks();
    }
}
