using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using TMPro;

public class LocalizationManager : MonoBehaviour
{
    //  Public variables
    public static string LOCALIZATIONPATH { get { return Application.streamingAssetsPath + @"\Localization\"; } }
    public static string LANGUAGEPATH { get { return LOCALIZATIONPATH + $"{DataManager.Instance.options.language}\\"; } }



    /// <summary>
    /// The Instance that serves as our interface into this object
    /// </summary>
    public static LocalizationManager Instance { get; private set; }

    //  Private variables

    /// <summary>
    /// _initialized will stop us from executing any functions we still have dependencies for
    /// </summary>
    private bool _initialized { get { return (DataManager.Instance && DataManager.Instance.options != null); } }

    /// <summary>
    /// Will be used by text assets to update wording
    /// </summary>

    private Dictionary<string, string> UI_Dictionary;

    /// <summary>
    /// Will be used to attempt getting a translated name
    /// </summary>

    private Dictionary<string, string> Name_Dictionary;

    /// <summary>
    /// This Dictionary will require some tuning, but represents every race and gender name combination
    /// </summary>
    private Dictionary<bool, Dictionary<RACE, List<string>>> Specific_Name_Dictionary;


    /// <summary>
    /// Will be used by text assets to update font
    /// </summary>
    private Dictionary<string, TMP_FontAsset> Font_Dictionary;

    //  Private Methods
    private void Awake() //A singleton will allow us to destroy other Instances of this object, since we only need one
    {
        if (Instance == null) //If we arent the Instance and Instance isnt null
        {
            Instance = this; //We set Instance to ourself
            LoadFonts();    //  Load the existing fonts into the dictionary

            InitializeNameDictionaries();

            DontDestroyOnLoad(this); //Mark this gameobject to remain after loading to new scenes
            return;
        }
        Destroy(this.gameObject); //We delete ourself
    }

    /// <summary>
    /// Sets all the possible name dictionaries
    /// </summary>
    private void InitializeNameDictionaries() 
    {
        Name_Dictionary = new Dictionary<string, string>();

        Specific_Name_Dictionary = new Dictionary<bool, Dictionary<RACE, List<string>>>();
        Specific_Name_Dictionary[true] = new Dictionary<RACE, List<string>>();
        Specific_Name_Dictionary[true][RACE.Wild_One] = new List<string>();
        Specific_Name_Dictionary[true][RACE.Arenaen] = new List<string>();
        Specific_Name_Dictionary[true][RACE.Westerner] = new List<string>();
        Specific_Name_Dictionary[true][RACE.Avition] = new List<string>();
        Specific_Name_Dictionary[true][RACE.Novun] = new List<string>();
        Specific_Name_Dictionary[true][RACE.Umbran] = new List<string>();
        Specific_Name_Dictionary[false] = new Dictionary<RACE, List<string>>();
        Specific_Name_Dictionary[false][RACE.Wild_One] = new List<string>();
        Specific_Name_Dictionary[false][RACE.Arenaen] = new List<string>();
        Specific_Name_Dictionary[false][RACE.Westerner] = new List<string>();
        Specific_Name_Dictionary[false][RACE.Avition] = new List<string>();
        Specific_Name_Dictionary[false][RACE.Novun] = new List<string>();
        Specific_Name_Dictionary[false][RACE.Umbran] = new List<string>();
    }

    /// <summary>
    /// Loads all fonts into
    /// </summary>
    private void LoadFonts() 
    {
        Font_Dictionary = new Dictionary<string, TMP_FontAsset>();

        // Right-to-left languages
        Font_Dictionary["Arabic"] = Resources.Load<TMP_FontAsset>("Fonts & Materials/Arabic");
        Font_Dictionary["Hebrew"] = Resources.Load<TMP_FontAsset>("Fonts & Materials/Hebrew");
        Font_Dictionary["Persian"] = Resources.Load<TMP_FontAsset>("Fonts & Materials/Arabic");
        Font_Dictionary["Urdu"] = Resources.Load<TMP_FontAsset>("Fonts & Materials/Urdu");
        Font_Dictionary["Pashto"] = Resources.Load<TMP_FontAsset>("Fonts & Materials/Urdu");
        Font_Dictionary["Syriac"] = Resources.Load<TMP_FontAsset>("Fonts & Materials/Syriac");
        Font_Dictionary["Divehi"] = Resources.Load<TMP_FontAsset>("Fonts & Materials/Divehi");
        Font_Dictionary["Yiddish"] = Resources.Load<TMP_FontAsset>("Fonts & Materials/Yiddish");

        // Left-to-right languages
        Font_Dictionary["English"] = Resources.Load<TMP_FontAsset>("Fonts & Materials/English");
        Font_Dictionary["Spanish"] = Resources.Load<TMP_FontAsset>("Fonts & Materials/Sans");
        Font_Dictionary["French"] = Resources.Load<TMP_FontAsset>("Fonts & Materials/Sans");
        Font_Dictionary["German"] = Resources.Load<TMP_FontAsset>("Fonts & Materials/Sans");
        Font_Dictionary["Italian"] = Resources.Load<TMP_FontAsset>("Fonts & Materials/Sans");
        Font_Dictionary["Portuguese"] = Resources.Load<TMP_FontAsset>("Fonts & Materials/Sans");
        Font_Dictionary["Dutch"] = Resources.Load<TMP_FontAsset>("Fonts & Materials/Sans");
        Font_Dictionary["Swedish"] = Resources.Load<TMP_FontAsset>("Fonts & Materials/Sans");
        Font_Dictionary["Norwegian"] = Resources.Load<TMP_FontAsset>("Fonts & Materials/Sans");
        Font_Dictionary["Danish"] = Resources.Load<TMP_FontAsset>("Fonts & Materials/Sans");
        Font_Dictionary["Finnish"] = Resources.Load<TMP_FontAsset>("Fonts & Materials/Sans");

        Font_Dictionary["Russian"] = Resources.Load<TMP_FontAsset>("Fonts & Materials/Sans");
        Font_Dictionary["Greek"] = Resources.Load<TMP_FontAsset>("Fonts & Materials/Sans");

        Font_Dictionary["Chinese"] = Resources.Load<TMP_FontAsset>("Fonts & Materials/Chinese");
        Font_Dictionary["Japanese"] = Resources.Load<TMP_FontAsset>("Fonts & Materials/Japanese");
        Font_Dictionary["Korean"] = Resources.Load<TMP_FontAsset>("Fonts & Materials/Korean");

        Font_Dictionary["Thai"] = Resources.Load<TMP_FontAsset>("Fonts & Materials/Thai");
        Font_Dictionary["Vietnamese"] = Resources.Load<TMP_FontAsset>("Fonts & Materials/Thai");

        Font_Dictionary["Hindi"] = Resources.Load<TMP_FontAsset>("Fonts & Materials/Hindi");
        Font_Dictionary["Bengali"] = Resources.Load<TMP_FontAsset>("Fonts & Materials/Bengali");
        Font_Dictionary["Tamil"] = Resources.Load<TMP_FontAsset>("Fonts & Materials/Tamil");
        Font_Dictionary["Telugu"] = Resources.Load<TMP_FontAsset>("Fonts & Materials/Telugu");

        Font_Dictionary["Malay"] = Resources.Load<TMP_FontAsset>("Fonts & Materials/Sans"); // Covered by English
        Font_Dictionary["Indonesian"] = Resources.Load<TMP_FontAsset>("Fonts & Materials/Sans"); // Covered by English
        Font_Dictionary["Turkish"] = Resources.Load<TMP_FontAsset>("Fonts & Materials/Sans"); // Covered by English
    }

    /// <summary>
    /// Takes a dictionary and loads the values from the path into it. Dependant on DataManager options being loaded
    /// </summary>
    /// <param name="dictionary">The dictionary to be loaded</param>
    /// <param name="PATH">The path to load the dictionary from</param>
    private void LoadDictionary(out Dictionary<string, string> dictionary, string PATH)
    {
        dictionary = null;
        if (!_initialized) return;
        Debug.Assert(File.Exists(PATH), $"{PATH} does not exist");
        dictionary = LoadClass.Load<Dictionary<string, string>>(PATH);
    }

    /// <summary>
    /// Load names attempts to load every name combination
    /// </summary>
    /// <returns></returns>
    private IEnumerator LoadNames() 
    {
        yield return new WaitUntil(() => _initialized == true);
        StartCoroutine(LoadName(LANGUAGEPATH + @"Names\Arenaen" + "Male" + "Names.json", Specific_Name_Dictionary[true][RACE.Arenaen]));
        StartCoroutine(LoadName(LANGUAGEPATH + @"Names\Arenaen" + "Female" + "Names.json", Specific_Name_Dictionary[false][RACE.Arenaen]));
        StartCoroutine(LoadName(LANGUAGEPATH + @"Names\Avition" + "Male" + "Names.json", Specific_Name_Dictionary[true][RACE.Avition]));
        StartCoroutine(LoadName(LANGUAGEPATH + @"Names\Avition" + "Female" + "Names.json", Specific_Name_Dictionary[false][RACE.Avition]));
        StartCoroutine(LoadName(LANGUAGEPATH + @"Names\Novun" + "Male" + "Names.json", Specific_Name_Dictionary[true][RACE.Novun]));
        StartCoroutine(LoadName(LANGUAGEPATH + @"Names\Novun" + "Female" + "Names.json", Specific_Name_Dictionary[false][RACE.Novun]));
        StartCoroutine(LoadName(LANGUAGEPATH + @"Names\Umbran" + "Male" + "Names.json", Specific_Name_Dictionary[true][RACE.Umbran]));
        StartCoroutine(LoadName(LANGUAGEPATH + @"Names\Umbran" + "Female" + "Names.json", Specific_Name_Dictionary[false][RACE.Umbran]));
        StartCoroutine(LoadName(LANGUAGEPATH + @"Names\Western" + "Male" + "Names.json", Specific_Name_Dictionary[true][RACE.Westerner]));
        StartCoroutine(LoadName(LANGUAGEPATH + @"Names\Western" + "Female" + "Names.json", Specific_Name_Dictionary[false][RACE.Westerner]));
        StartCoroutine(LoadName(LANGUAGEPATH + @"Names\WildOne" + "Male" + "Names.json", Specific_Name_Dictionary[true][RACE.Wild_One]));
        StartCoroutine(LoadName(LANGUAGEPATH + @"Names\WildOne" + "Female" + "Names.json", Specific_Name_Dictionary[false][RACE.Wild_One]));
    }

    /// <summary>
    /// This sets up a temp dictionary that loads each kvp into the seperated list structure and the general translation structure
    /// </summary>
    /// <param name="PATH"></param>
    /// <returns></returns>
    private IEnumerator LoadName(string PATH, List<string> list) 
    {
        yield return new WaitUntil(() => _initialized == true);
        Dictionary<string, string> dictionary = null;
        Debug.Assert(File.Exists(PATH), $"{PATH} does not exist");
        dictionary = LoadClass.Load<Dictionary<string, string>>(PATH);

        foreach (var kvp in dictionary) 
        {
            list.Add(kvp.Key);
            Name_Dictionary[kvp.Key] = kvp.Value;
        }

    }

    //  Public methods

    /// <summary>
    /// Reloads the UI Dictionary with the newest selected language
    /// </summary>
    public void ReloadUI() 
    {
        LoadDictionary(out UI_Dictionary, LANGUAGEPATH + "UI.json");
        StartCoroutine(LoadNames());
    }

    /// <summary>
    /// UpdateUI will check every single text based object with a LocalizableText GameObject, and force the text to update itself
    /// dependent on the dictionary being loaded
    /// </summary>
    public void UpdateUI() 
    {
        if (!_initialized) return;

        foreach (var localizabletext in Resources.FindObjectsOfTypeAll(typeof(LocalizableText)) as LocalizableText[]) 
        {
            localizabletext.UpdateText();
        }
    }
    /// <summary>
    /// This function checks if a given language is printed right to left
    /// </summary>
    /// <returns>True if the language is right to left</returns>
    public bool IsRightToLeft() 
    {
        switch (DataManager.Instance.options.language)
        {
            // Right-to-left languages
            case "Arabic":
            case "Hebrew":
            case "Persian":
            case "Urdu":
            case "Pashto":
            case "Syriac":
            case "Divehi":
            case "Yiddish":
                return true;

            // Left-to-right languages
            case "English":
            case "Spanish":
            case "French":
            case "German":
            case "Italian":
            case "Portuguese":
            case "Russian":
            case "Chinese":
            case "Japanese":
            case "Korean":
            case "Dutch":
            case "Swedish":
            case "Norwegian":
            case "Danish":
            case "Finnish":
            case "Greek":
            case "Thai":
            case "Vietnamese":
            case "Hindi":
            case "Bengali":
            case "Tamil":
            case "Telugu":
            case "Malay":
            case "Indonesian":
            case "Turkish":
                return false;

            // Default to LTR if the language is not explicitly listed
            default:
                Debug.LogWarning($"Language '{DataManager.Instance.options.language}' not recognized. Assuming LTR.");
                return false;
        }
    }

    /// <summary>
    /// Reads the value from the UI Dictionary
    /// </summary>
    /// <param name="key">The key to the Dictionary</param>
    /// <returns>The Value at that location</returns>
    public string ReadUIDictionary(string key) 
    {
        if (!_initialized) return null;
        Debug.Assert(UI_Dictionary.ContainsKey(key), $"{key} is not in Dictionary");
        return UI_Dictionary.ContainsKey(key) ? UI_Dictionary[key] : key;
    }

    /// <summary>
    /// Returns whether the UI is ready yet for reading
    /// </summary>
    /// <returns></returns>
    public bool UIReady() 
    {
        return _initialized;
    }

    /// <summary>
    /// Gets the font for a given language
    /// </summary>
    /// <returns></returns>
    public TMP_FontAsset GetFont() 
    {
        Debug.Assert(Font_Dictionary.ContainsKey(DataManager.Instance.options.language), "No Font associated with this language!");
        return Font_Dictionary[DataManager.Instance.options.language];
    }


}

/// <summary>
/// This serves as our entry point into the Language Localization file
/// </summary>
[System.Serializable]
public class LCLZ_Language 
{
    public List<LCLZ_LanguageListElements> entries;

    /// <summary>
    /// Converts the JSON file conversion into dictionary format
    /// </summary>
    /// <returns>A Dictionary</returns>
    public Dictionary<string, string> ToDictionary() 
    {
        Dictionary<string, string> result = new Dictionary<string,string>();

        foreach (LCLZ_LanguageListElements e in entries) 
        {
            result[e.key] = e.value;
        }

        return result;
    }
}

/// <summary>
/// LCLZ_LanguageListElements are string key value pairs
/// </summary>
[System.Serializable]
public class LCLZ_LanguageListElements 
{
    public string key;
    public string value;
}
