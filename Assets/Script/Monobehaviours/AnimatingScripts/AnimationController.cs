using System.Collections;
using UnityEngine;

/// <summary>
/// The animation controller for any given inheritor
/// </summary>
public interface AnimationController
{
    public void PlayAnimation();
    public bool IsComplete();

    public void SetUp(AnimatableResourceChange resourceValues);

    public void SetUp(Humanoid caster, Humanoid target);
}