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
    public List <PremadeCharacters> premadeSprites; //A list of every premades sprites
    public List<Sprite> MonsterSprites = new List<Sprite>(); //Any item sprites
    public List<Sprite> ItemSprites = new List<Sprite>(); //Any item sprites
    public List<Sprite> EquipmentSprites = new List<Sprite>(); //Any equipment sprites

    public Sprite blankSprite; //An empty image

    //  Private Variables

    //  Private Methods

    private void Awake() //A singleton will allow us to destroy other Instances of this object, since we only need one
    {
        if (Instance == null) //If we arent the Instance and Instance isnt null
        {
            Instance = this; //We set Instance to ourself
            DontDestroyOnLoad(this); //Mark this gameobject to remain after loading to new scenes
            return;
        }
        Destroy(this.gameObject); //We delete ourself
    }

    //  Public Methods

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
public class PremadeCharacters //Contains a list of a premade characters sprites
{
    public string name;
    public List<Sprite> characterSprites;
}
