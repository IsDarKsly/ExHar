using System.Collections;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public bool IsAnimationComplete { get; private set; }

    public void PlayAnimation()
    {
        // Trigger animation logic here
        IsAnimationComplete = false;
        StartCoroutine(AnimationRoutine());
    }

    private IEnumerator AnimationRoutine()
    {
        // Simulate animation duration
        yield return new WaitForSeconds(2.0f);
        IsAnimationComplete = true;
    }
}
