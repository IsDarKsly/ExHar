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
    /// This unity event will be invoked whenever a skill is used.
    /// The First humanoid represents who is using the skill
    /// the ActiveTalent represents the skill
    /// The Second Humanoid represents the target of the skill
    /// </summary>
    public UnityEvent<Humanoid, ActiveTalents, Humanoid> AnimationEvent = new UnityEvent<Humanoid, ActiveTalents, Humanoid>();

    /// <summary>
    /// Adds listeners for certain events in the BattleManager
    /// </summary>
    private void OnEnable()
    {
        BattleManager.Instance.OnPhaseChange.AddListener(AnimationPhase);
        AnimationEvent.AddListener(QueueAnimation);
    }

    /// <summary>
    /// Removes Listeners for certain events in battle manager
    /// </summary>
    private void OnDisable()
    {
        BattleManager.Instance.OnPhaseChange.RemoveListener(AnimationPhase);
        AnimationEvent.RemoveListener(QueueAnimation);
    }

    /// <summary>
    /// This function will create a new animation and place it into the Animations Queue
    /// </summary>
    public void QueueAnimation(Humanoid caster, ActiveTalents skill, Humanoid target) 
    {
    
    }

    public void AnimationPhase(BATTLEPHASE phase)
    {
        if (phase != BATTLEPHASE.ANIMATING) return;

        StartCoroutine(PlayAnimations());
    }

    private IEnumerator PlayAnimations()
    {
        while (Animations.Count > 0)
        {
            GameObject animationObject = Animations.Dequeue();

            // Assume animations have an "AnimationController" script that plays them
            if (animationObject.TryGetComponent<AnimationController>(out var controller))
            {
                controller.PlayAnimation();
                yield return new WaitUntil(() => controller.IsAnimationComplete);
            }

            Destroy(animationObject); // Clean up animation object after it's finished
        }

        BattleManager.Instance.SetPhase(BATTLEPHASE.PLAYERTURN);
    }
}
