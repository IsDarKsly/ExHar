using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class TransitionManager : MonoBehaviour
{
    //  Public variables
    public static TransitionManager Instance;
    //  Private Variables
    [SerializeField] private GameObject fade_canvas;

    private Action act;  //  act will be set and used by any specific animations
    //  Private functions
    private void Awake()
    {
        if (Instance == null) 
        {
            Instance = this;
            DontDestroyOnLoad(this);
            return;
        }
        Destroy(this.gameObject);
    }

    /// <summary>
    /// Sets the action to be taken by specific animations
    /// </summary>
    /// <param name="action"></param>
    private void SetAction(Action action)
    {
        act = action;
    }



    /// <summary>
    /// Animates the fade_canvas to black and back in a varient amount of time.
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    private IEnumerator FadeAnimation(float time, float wait_time) 
    {          
        CanvasGroup canvasgroup = fade_canvas.GetComponent<CanvasGroup>();
        // Animating accounting for each frame over this duration of time
        for (float i = 0; i < (60.0f*time); i++) 
        {
            canvasgroup.alpha = i/(60.0f * time); //  Setting the alpha value
            yield return new WaitForSecondsRealtime(1.0f/(60.0f));    //  Seconds per frame
        }
        canvasgroup.alpha = 1.0f;

        if (act!=null) yield return new WaitUntil(()=>TakeAction());
        yield return new WaitForSecondsRealtime(wait_time);
        //  Fading back
        for (float i = (60.0f * time); i > 0; i--)
        {
            canvasgroup.alpha = i/(60.0f * time); //  Setting the alpha value
            yield return new WaitForSecondsRealtime(1.0f/(60.0f));    //  Seconds per frame
        }

        canvasgroup.alpha = 0f;
        fade_canvas.SetActive(false);
    }

    /// <summary>
    /// Fades starting from black, usage should be rare
    /// </summary>
    /// <returns></returns>
    private IEnumerator INSTANT_Fade(float time, float wait_time)
    {
        CanvasGroup canvasgroup = fade_canvas.GetComponent<CanvasGroup>();
        canvasgroup.alpha = 1.0f;
        yield return new WaitForSecondsRealtime(wait_time);
        //  Fading back
        for (float i = (60.0f * time); i > 0; i--)
        {
            canvasgroup.alpha = i / (60.0f * time); //  Setting the alpha value
            yield return new WaitForSecondsRealtime(1.0f / (60.0f));    //  Seconds per frame
        }
        if (act != null) act();

        fade_canvas.SetActive(false);
    }

    //  Public functions

    /// <summary>
    /// The FadeToBlack function enables the fade canvas, and fades to black in a set amount of time.
    /// If a parameter for action is given, that action will be performed at the peak of the animation
    /// </summary>
    /// <param name="time">Time to reach full fade to black</param>
    /// <param name="wait_time">Time to wait at complete darkness</param>
    /// <param name="action">Defaults to null, will perform action after set time</param>
    public void FadeToBlack(float time, Action action = null, float wait_time = 1.0f) 
    {
        fade_canvas.SetActive(true);
        SetAction(action);
        StartCoroutine(FadeAnimation(time, wait_time));
    }

    /// <summary>
    /// Fades from black, usage should be rare, as it immediatly sets the value of alpha to its max
    /// </summary>
    public void IMMEDIATE_FadeFromBlack(float time, Action action = null, float wait_time = 1.0f) 
    {
        fade_canvas.SetActive(true);
        SetAction(action);
        StartCoroutine(INSTANT_Fade(time, wait_time));
    }

    /// <summary>
    /// Transition is meant to be a sort of "catch all" function that simplifies transitions.
    /// Instead of having to remember the specific name of the transition function, the enum can be passed instead
    /// </summary>
    /// <param name="type"></param>
    /// <param name="time"></param>
    /// <param name="action"></param>
    /// <param name="wait_time"></param>
    public void Transition(TRANSITIONTYPE type, float time, Action action = null, float wait_time = 1.0f) 
    {
        switch (type) 
        {
            case TRANSITIONTYPE.Fade:
                FadeToBlack(time, action, wait_time);
                break;
            case TRANSITIONTYPE.ImmediateFade:
                IMMEDIATE_FadeFromBlack(time, action, wait_time);
                break;
            default:
               //Debug.LogError($"{type.ToString()} not implemented");
                break;
        }
    }

    /// <summary>
    /// If action is true, we take it. Called by various animations
    /// </summary>
    /// <returns></returns>
    private bool TakeAction()
    {
        Debug.Assert(act != null, "action_to_take is null!");
        if (act != null)
        {
            act();
            act = null;
            return true;
        }
        return false;
    }
}

public enum TRANSITIONTYPE { Fade, ImmediateFade }