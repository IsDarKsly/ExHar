using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Centralized management of all game sprites, including loading, updating, and applying optimizations
/// </summary>
public class SpriteManager : MonoBehaviour
{
    //  Public Variables

    /// <summary>
    /// The Instance that serves as our interface into this object
    /// </summary>
    public static SpriteManager Instance { get; private set; } //singleton

    public GameObject CharacterPrefab; //The prefab for our character

    [NonReorderable] public FemaleSprites femaleSprites; //Any and all female sprites
    [NonReorderable] public MaleSprites maleSprites; //Any and all male sprites

    public List<RacialColors> raceColor; //Each racial color

    /// <summary>
    /// An empty image, used to make sprite components invisible
    /// </summary>
    [SerializeField]private Sprite blankSprite;

    /// <summary>
    /// Animation prefabs logged here
    /// </summary>
    [SerializeField] private List<AnimationPrefab> AnimationPrefabs = new List<AnimationPrefab>();

    /// <summary>
    /// Miscellaneuos sprites are logged here, turned into dictionary on start.
    /// </summary>
    [SerializeField] private List<SpriteDict> spriteDicts = new List<SpriteDict>();

    /// <summary>
    /// Any male equipment sprites are preloaded into here, not to be accessed by other classes. Turned into dictionary on start.
    /// </summary>
    [SerializeField] private List<SpriteDict> MaleEquipmentSprites = new List<SpriteDict>(); //Equipment sprites for males

    /// <summary>
    /// Any female equipment sprites are preloaded into here, not to be accessed by other classes. Turned into dictionary on start.
    /// </summary>
    [SerializeField] private List<SpriteDict> FemaleEquipmentSprites = new List<SpriteDict>(); //Equipment sprites for females

    /// <summary>
    /// This list is used to represent any monster sprites and their corresponding injury status. It should not be used by any other classes as it is meant
    /// to be turned into a dictionary.
    /// </summary>
    [SerializeField] private List<Monsters> MonsterSprites = new List<Monsters>(); //Any Monster Sprites

    /// <summary>
    /// This serializable list is used to represent any unique character and their appearances. They should be identified by name.
    /// Turned into dictionary.
    /// </summary>
    [SerializeField] private List<PremadeCharacters> premadeSprites = new List<PremadeCharacters>();

    /// <summary>
    /// The sprite dictionary will help us easily search for relevant sprites to assign images
    /// accessed via functions
    /// </summary>
    private Dictionary<string, Sprite> SpriteDictionary = new Dictionary<string, Sprite>();

    /// <summary>
    /// The Equipment Dictionary will allow us to get equipment sprites 
    /// accessed via functions
    /// </summary>
    private Dictionary<bool, Dictionary<string, Sprite>> EquipmentDictionary = new Dictionary<bool, Dictionary<string, Sprite>>();

    /// <summary>
    /// The animation prefab dictionary, searchable via name
    /// accessed via functions
    /// </summary>
    private Dictionary<string, GameObject> AnimationPrefabsDictionary = new Dictionary<string, GameObject>();

    /// <summary>
    /// The Monster Sprite Dictionary
    /// accessed via functions
    /// </summary>
    private Dictionary<string, Dictionary<InjuredStatus, Sprite>> MonsterSpriteDictionary = new Dictionary<string, Dictionary<InjuredStatus, Sprite>>();

    /// <summary>
    /// The Premade Sprite Dictionary
    /// accessed via functions
    /// </summary>
    private Dictionary<string, Dictionary<Emotion, Sprite>> PremadeSpriteDictionary = new Dictionary<string, Dictionary<Emotion, Sprite>>();

    //  Private Variables

    //  Private Methods

    private void Awake() //A singleton will allow us to destroy other Instances of this object, since we only need one
    {
        if (Instance == null) //If we arent the Instance and Instance isnt null
        {
            Instance = this; //We set Instance to ourself
            SetupDictionary();  //  Setup the dictionary
            LoadSprites();  //  Setup the sprites
            DontDestroyOnLoad(this); //Mark this gameobject to remain after loading to new scenes
            return;
        }
        Destroy(this.gameObject); //We delete ourself
    }


    /// <summary>
    /// Sets up a dictionary for the animation prefab objects using 
    /// </summary>
    private void SetupDictionary() 
    {
        foreach (var kvp in AnimationPrefabs) 
        {
            AnimationPrefabsDictionary[kvp.Key] = kvp.Value;
        }

        EquipmentDictionary[true] = new Dictionary<string, Sprite>();   //  Male equipment dictionary
        EquipmentDictionary[false] = new Dictionary<string, Sprite>();  //  Female Equipment dictionary

        foreach (var kvp in MaleEquipmentSprites)   //  Setting all the Male equipments
        {
            EquipmentDictionary[true][kvp.Key] = kvp.Value;
        }

        foreach (var kvp in FemaleEquipmentSprites)     //  Setting all the female equipments
        {
            EquipmentDictionary[false][kvp.Key] = kvp.Value;
        }

        foreach (var monster in MonsterSprites)     //  Loading all the monster sprites into the corresponding dictionary
        {
            MonsterSpriteDictionary[monster.name] = new Dictionary<InjuredStatus, Sprite>();
            foreach (var injurysprite in monster.injuredSprites) 
            {
                MonsterSpriteDictionary[monster.name][injurysprite.injuredstatus] = injurysprite.injuredSprite;
            }
        }

        foreach (var character in premadeSprites)   //  Loading all the premade character into the 
        {
            PremadeSpriteDictionary[character.name] = new Dictionary<Emotion,Sprite>();
            foreach (var emotion in character.emotionSprites) 
            {
                PremadeSpriteDictionary[character.name][emotion.emotion] = emotion.emotionSprite;
            }
        }
    }

    /// <summary>
    /// Loads the inspector sprite combos into the dictionary
    /// </summary>
    private void LoadSprites() 
    {
        foreach (var kvp in spriteDicts)
        {
            SpriteDictionary[kvp.Key] = kvp.Value;
        }
    }

    /// <summary>
    /// Attempts to get a sprite from a string key
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public Sprite GetSprite(string key) 
    {
        return SpriteDictionary.ContainsKey(key) ? SpriteDictionary[key] : blankSprite;
    }

    /// <summary>
    /// The GetEquipmentSprite is used to determine what equipment should be loaded onto this character.
    /// Should the equipment not exist, the blank sprite is used instead
    /// </summary>
    /// <param name="gender"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public Sprite GetEquipmentSprite(bool gender, string key) 
    {
        return EquipmentDictionary[gender].ContainsKey(key) ? EquipmentDictionary[gender][key] : blankSprite;
    }

    /// <summary>
    /// The get monster sprite function attempts to retrieve the corresponding monster sprite from the monster sprite dictionary based on the name and injury status of the monster
    /// If no viable injury status is found, it assumes the healthy injury status.
    /// If the Healthy status does not exist, somebody messed up
    /// </summary>
    public Sprite GetMonsterSprite(string name, InjuredStatus status)
    {
        if (!MonsterSpriteDictionary.ContainsKey(name))   //  If this name doesnt exist in the database, logg it and return the 
        {
            Debug.LogWarning($"Warning, {name} does not exist within the monster dictionary");
            return blankSprite;
        }

        if (!MonsterSpriteDictionary[name].ContainsKey(status))
        {
            Debug.LogWarning($"Warning, {name} does not contain this status");
            return MonsterSpriteDictionary[name][InjuredStatus.HEALTHY];
        }
        else 
        {
            return MonsterSpriteDictionary[name][status];
        }
    }

    /// <summary>
    /// The Get Premade Sprite attempts to get the corresponding sprite for a premade character given an emotion.
    /// If the character does not exist in the dictionary, the blank sprite is returned. If the emotion does not exists, the neutral
    /// emotion is assumed to exist and returned. If that doesn't exist, then an error will be thrown.
    /// </summary>
    public Sprite GetPremadeSprite(string name, Emotion emotion)
    {
        if (!PremadeSpriteDictionary.ContainsKey(name))   //  If this name doesnt exist in the database, logg it and return the 
        {
            Debug.LogWarning($"Warning, {name} does not exist within the premade dictionary");
            return blankSprite;
        }

        if (!PremadeSpriteDictionary[name].ContainsKey(emotion))
        {
            Debug.LogWarning($"Warning, {name} does not contain the requested emotion");
            return PremadeSpriteDictionary[name][Emotion.NEUTRAL];
        }
        else
        {
            return PremadeSpriteDictionary[name][emotion];
        }
    }

    /// <summary>
    /// Quickly gets the sprite for a given damage subtype
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public Sprite GetSubtypeSprite(DamageSubType type) 
    {
        switch (type)    //  Setting the image of the element type 
        {
            case DamageSubType.Crushing:
                return GetSprite("Crushing");
            case DamageSubType.Slashing:
                return GetSprite("Slashing");
            case DamageSubType.Stabbing:
                return GetSprite("Stabbing");
            case DamageSubType.Fire:
                return GetSprite("Fire");
            case DamageSubType.Water:
                return GetSprite("Water");
            case DamageSubType.Ice:
                return GetSprite("Ice");
            case DamageSubType.Lightning:
                return GetSprite("Lightning");
            case DamageSubType.Light:
                return GetSprite("Light");
            case DamageSubType.Dark:
                return GetSprite("Dark");
            case DamageSubType.Bleeding:
                return GetSprite("Blood");
            case DamageSubType.Poison:
                return GetSprite("Poison");
        }
        return null;
    }

    //  Public Methods
    /// <summary>
    /// Get animation Prefab searches the animation prefab dictionary and returns a corresponding gameobject value if it exists.
    /// Null if it does not
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public GameObject GetAnimationPrefab(string id) 
    {
        return (AnimationPrefabsDictionary.ContainsKey(id)) ? AnimationPrefabsDictionary[id] : null;
    }

    /// <summary>
    /// Returns the blank sprite
    /// </summary>
    /// <returns></returns>
    public Sprite GetBlankSprite() 
    {
        return blankSprite;
    }

}



[System.Serializable]
public class FemaleSprites
{
    public List<Sprite> eyebrow = new List<Sprite>();
    public List<Sprite> eye = new List<Sprite>();
    public List<Sprite> eyecolor = new List<Sprite>();
    public List<Sprite> hair = new List<Sprite>();
    public List<Sprite> haircolor = new List<Sprite>();
    public List<Sprite> mouth = new List<Sprite>();
    public List<Sprite> nose = new List<Sprite>();
    public List<Sprite> skincolor = new List<Sprite>();
    public List<Sprite> skinshape = new List<Sprite>();
    public List<Sprite> extracolor = new List<Sprite>();
    public List<Sprite> extra = new List<Sprite>();
    public List<InjurySprites> injuries = new List<InjurySprites>();

} //Female sprites in unique class

[System.Serializable]
public class MaleSprites
{
    public List<Sprite> eyebrow = new List<Sprite>();
    public List<Sprite> eye = new List<Sprite>();
    public List<Sprite> eyecolor = new List<Sprite>();
    public List<Sprite> hair = new List<Sprite>();
    public List<Sprite> haircolor = new List<Sprite>();
    public List<Sprite> mouth = new List<Sprite>();
    public List<Sprite> nose = new List<Sprite>();
    public List<Sprite> skincolor = new List<Sprite>();
    public List<Sprite> skinshape = new List<Sprite>();
    public List<Sprite> extracolor = new List<Sprite>();
    public List<Sprite> extra = new List<Sprite>();
    public List<InjurySprites> injuries = new List<InjurySprites>();
} //Male sprites in unique class

[System.Serializable]
public class RacialColors //Contains a list of each races skin tones
{
    public RACE race;
    public List<Color> skincolors = new List<Color>();
    public List<Color> eyecolors = new List<Color>();
    public List<Color> haircolors = new List<Color>();
    public List<Color> extracolors = new List<Color>();
}

[System.Serializable]
public class PremadeCharacters
{
    public string name;
    public List<EmotionSprites> emotionSprites;
}

[System.Serializable]
public class Monsters
{
    public string name;
    public List<InjurySprites> injuredSprites;
}

/// <summary>
/// The Injury Sprite class represents what certain characters look like when injured
/// </summary>
[System.Serializable]
public class InjurySprites 
{
    public InjuredStatus injuredstatus;
    public Sprite injuredSprite;
}

/// <summary>
/// The Emotion Sprite class represents what certain characters look like when expressing a different emotion
/// </summary>
[System.Serializable]
public class EmotionSprites
{
    public Emotion emotion;
    public Sprite emotionSprite;
}

/// <summary>
/// Every Animation prefab will contain a specific correspondinhg int relative to the talent id,
/// The value will be a gameobject prefab with an inherited Animation Controller script on it
/// </summary>
[System.Serializable]
public class AnimationPrefab 
{
    public string Key;
    public GameObject Value;
}

[System.Serializable]
public class SpriteDict 
{
    public string Key;
    public Sprite Value;
}

