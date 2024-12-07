using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BattleAnimationManager : MonoBehaviour
{
    /// <summary>
    /// The animations that should be played
    /// </summary>
    public Queue<GameObject> Animations;

    /// <summary>
    /// This Gameobject will display a Resource number descending or ascending from the target.
    /// Then it will disappear after a given time
    /// </summary>
    public GameObject ResourceNumberPrefab;

    /// <summary>
    /// Initilizes some values
    /// </summary>
    private void Awake()
    {
        Animations = new Queue<GameObject>();
    }

    /// <summary>
    /// Adds listeners for certain events in the BattleManager
    /// </summary>
    private void OnEnable()
    {
        BattleManager.Instance.OnPhaseChange.AddListener(AnimationPhase);
    }

    /// <summary>
    /// Removes Listeners for certain events in battle manager
    /// </summary>
    private void OnDisable()
    {
        BattleManager.Instance.OnPhaseChange.RemoveListener(AnimationPhase);
    }

    /// <summary>
    /// This function will create a new resource number prefab and add it to the Queue
    /// </summary>
    public void QueueAnimationResourceChange(AnimatableResourceChange resourceValue) 
    {
        Debug.Log($"An animation is being added to the Queue!");
        var obj = Instantiate(ResourceNumberPrefab);
        var script = obj.GetComponent<AnimationController>();
        if (script != null)
        {
            Debug.Log($"{script.GetType().Name}");
            script.SetUp(resourceValue);
            Animations.Enqueue(obj);
        }
        else
        {
            Debug.LogError("The instantiated prefab does not implement the AnimationController interface.");
            Destroy(obj); // Clean up the instantiated object if it doesn’t have the correct interface
        }
    }

    /// <summary>
    /// This function will create a new animation and place it into the Animations Queue
    /// </summary>
    public void QueueAnimation(int skillID, Humanoid caster, Humanoid target = null) 
    {
        var obj = SpriteManager.Instance.GetAnimationPrefab(skillID);
        if (obj == null) return;
        var script = obj.GetComponent<MonoBehaviour>() as AnimationController;

        if (script != null)
        {
            script.SetUp(caster, target);
            Animations.Enqueue(obj);
        }
        else
        {
            Debug.LogError("The instantiated prefab does not implement the AnimationController interface.");
            Destroy(obj); // Clean up the instantiated object if it doesn’t have the correct interface
        }
    }

    /// <summary>
    /// When the battle manager phase is set to animate, we animate every queued animation
    /// </summary>
    /// <param name="phase"></param>
    public void AnimationPhase(BATTLEPHASE phase)
    {
        if (phase != BATTLEPHASE.ANIMATING) return;
        Debug.Log($"Animation phase trigger: {Animations.Count}");
        StartCoroutine(PlayAnimations());
    }

    /// <summary>
    /// Plays all the animations in the Cue, waiting for each to finish before changing the phase back to the player phase
    /// </summary>
    /// <returns></returns>
    private IEnumerator PlayAnimations()
    {
        while (Animations.Count > 0)
        {
            GameObject animationObject = Animations.Dequeue();
            Debug.Log("Animating object");
            // Assume animations have an "AnimationController" script that plays them
            if (animationObject.TryGetComponent<AnimationController>(out var controller))
            {
                Debug.Log("Controller component obtained");
                controller.PlayAnimation();
                yield return new WaitUntil(() => controller.IsComplete() || animationObject == null);
            }

        }

        BattleManager.Instance.AfterAnimation();
    }
}

public struct AnimatableResourceChange
{
    /// <summary>
    /// 
    /// </summary>
    public int resourceAmount;

    /// <summary>
    /// This only effects the symbol next to the damage
    /// </summary>
    public DamageSubType type;
    
    /// <summary>
    /// This displays whether
    /// </summary>
    public RESOURCES resourceType;

    /// <summary>
    /// Whether or not this value was critical
    /// </summary>
    public bool isCrit;

    /// <summary>
    /// Whoever gained or lost resource
    /// </summary>
    public Humanoid target;

    /// <summary>
    /// Constructor for the struct
    /// </summary>
    /// <param name="resourceAmount"></param>
    /// <param name="type"></param>
    /// <param name="resoureType"></param>
    /// <param name="isCrit"></param>
    public AnimatableResourceChange(int resourceAmount, DamageSubType type, RESOURCES resourceType, bool isCrit, Humanoid target) 
    {
        this.resourceAmount = resourceAmount;
        this.type = type;
        this.resourceType = resourceType;
        this.isCrit = isCrit;
        this.target = target;
    }

}

