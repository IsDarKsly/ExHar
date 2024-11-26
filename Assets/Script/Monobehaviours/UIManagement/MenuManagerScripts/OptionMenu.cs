using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The option menu monobehaviour adds simple functionality to the options
/// </summary>
public class OptionMenu : MonoBehaviour
{
    //  Public Variables

    public TMP_Dropdown res;

    public Toggle tog;

    public Slider mas_slide;

    public Slider mus_slide;

    public Slider eff_slide;

    public Slider voice_slide;

    public TMP_Dropdown lan;

    // Private Variables

    //  Private Functions
    private void Awake()
    {

    }


    //  Public Functions



    /// <summary>
    /// Toggles this scripts gameobject on or off
    /// </summary>
    public void ToggleSelf() 
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
    /// <summary>
    /// After DataManager sets up options, we set all the selected values on the gameobjects
    /// </summary>
    public void SetUp()
    {
        switch (DataManager.Instance.options.resolution.y)
        {
            case 1920:
                res.value = 0;
                break;
            case 720:
                res.value = 1;
                break;
            case 2160:
                res.value = 2;
                break;
        }

        mas_slide.value = DataManager.Instance.options.masterVolume;
        mus_slide.value = DataManager.Instance.options.musicVolume;
        eff_slide.value = DataManager.Instance.options.effectsVolume;
        voice_slide.value = DataManager.Instance.options.voiceVolume;

        //  Lord forgive me
        switch (DataManager.Instance.options.language)
        {
            case "English":
                lan.value = 0;
                break;
            case "Chinese":
                lan.value = 1;
                break;
            case "Hindi":
                lan.value = 2;
                break;
            case "Spanish":
                lan.value = 3;
                break;
            case "Arabic":
                lan.value = 4;
                break;
            case "Bengali":
                lan.value = 5;
                break;
            case "Portuguese":
                lan.value = 6;
                break;
            case "Russian":
                lan.value = 7;
                break;
            case "Japanese":
                lan.value = 8;
                break;
            case "Indonesian":
                lan.value = 9;
                break;
            case "French":
                lan.value = 10;
                break;
            case "Turkish":
                lan.value = 11;
                break;
            case "Vietnamese":
                lan.value = 12;
                break;
            case "Urdu":
                lan.value = 13;
                break;
            case "Tamil":
                lan.value = 14;
                break;
            case "Telugu":
                lan.value = 15;
                break;
            case "Korean":
                lan.value = 16;
                break;
            case "Pashto":
                lan.value = 17;
                break;
            case "Italian":
                lan.value = 18;
                break;
            case "Thai":
                lan.value = 19;
                break;
            case "Persian":
                lan.value = 20;
                break;
            case "German":
                lan.value = 21;
                break;
            case "Swedish":
                lan.value = 22;
                break;
            case "Dutch":
                lan.value = 23;
                break;
            case "Greek":
                lan.value = 24;
                break;
            case "Danish":
                lan.value = 25;
                break;
            case "Finnish":
                lan.value = 26;
                break;
            case "Norwegian":
                lan.value = 27;
                break;
            case "Hebrew":
                lan.value = 28;
                break;
            case "Yiddish":
                lan.value = 29;
                break;
            case "Divehi":
                lan.value = 30;
                break;
            case "Syriac":
                lan.value = 31;
                break;
            default:
                Debug.LogWarning($"Language '{DataManager.Instance.options.language}' not recognized.");
                break;

        }

    }
    public void ChangeResolution() 
    {
        switch (res.value) 
        {
            case 0:
                DataManager.Instance.options.resolution = new Vector2Int(1920,1080);
                break;
            case 1:
                DataManager.Instance.options.resolution = new Vector2Int(1920, 1080);
                break;
            case 2:
                DataManager.Instance.options.resolution = new Vector2Int(1920, 1080);
                break;
        }
        MenuManager.Instance.UpdateFromOptions();
    }

    public void ToggleFullscreen() 
    {
        DataManager.Instance.options.isFullscreen = tog.enabled;
        MenuManager.Instance.UpdateFromOptions();
    }

    public void SetMasterVolume() 
    {
        DataManager.Instance.options.masterVolume = mas_slide.value;
    }

    public void SetMusicVolume()
    {
        DataManager.Instance.options.musicVolume = mus_slide.value;
    }

    public void SetEffectsVolume()
    {
        DataManager.Instance.options.effectsVolume = eff_slide.value;
    }

    public void SetVoiceVolume()
    {
        DataManager.Instance.options.voiceVolume = voice_slide.value;
    }

    public void Language()
    {
        switch (lan.value)
        {
            case 0:
                DataManager.Instance.options.language = "English";
                break;
            case 1:
                DataManager.Instance.options.language = "Chinese";
                break;
            case 2:
                DataManager.Instance.options.language = "Hindi";
                break;
            case 3:
                DataManager.Instance.options.language = "Spanish";
                break;
            case 4:
                DataManager.Instance.options.language = "Arabic";
                break;
            case 5:
                DataManager.Instance.options.language = "Bengali";
                break;
            case 6:
                DataManager.Instance.options.language = "Portuguese";
                break;
            case 7:
                DataManager.Instance.options.language = "Russian";
                break;
            case 8:
                DataManager.Instance.options.language = "Japanese";
                break;
            case 9:
                DataManager.Instance.options.language = "Indonesian";
                break;
            case 10:
                DataManager.Instance.options.language = "French";
                break;
            case 11:
                DataManager.Instance.options.language = "Turkish";
                break;
            case 12:
                DataManager.Instance.options.language = "Vietnamese";
                break;
            case 13:
                DataManager.Instance.options.language = "Urdu";
                break;
            case 14:
                DataManager.Instance.options.language = "Tamil";
                break;
            case 15:
                DataManager.Instance.options.language = "Telugu";
                break;
            case 16:
                DataManager.Instance.options.language = "Korean";
                break;
            case 17:
                DataManager.Instance.options.language = "Pashto";
                break;
            case 18:
                DataManager.Instance.options.language = "Italian";
                break;
            case 19:
                DataManager.Instance.options.language = "Thai";
                break;
            case 20:
                DataManager.Instance.options.language = "Persian";
                break;
            case 21:
                DataManager.Instance.options.language = "German";
                break;
            case 22:
                DataManager.Instance.options.language = "Swedish";
                break;
            case 23:
                DataManager.Instance.options.language = "Dutch";
                break;
            case 24:
                DataManager.Instance.options.language = "Greek";
                break;
            case 25:
                DataManager.Instance.options.language = "Danish";
                break;
            case 26:
                DataManager.Instance.options.language = "Finnish";
                break;
            case 27:
                DataManager.Instance.options.language = "Norwegian";
                break;
            case 28:
                DataManager.Instance.options.language = "Hebrew";
                break;
            case 29:
                DataManager.Instance.options.language = "Yiddish";
                break;
            case 30:
                DataManager.Instance.options.language = "Divehi";
                break;
            case 31:
                DataManager.Instance.options.language = "Syriac";
                break;
            default:
                Debug.LogWarning("Invalid language index.");
                break;
        }
        MenuManager.Instance.UpdateFromOptions();
    }
}
