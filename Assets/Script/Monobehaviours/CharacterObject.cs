using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public AppearanceObj appearanceObj;
    public Humanoid character;

    private Action onpointerenter;
    private Action onpointerexit;
    private Action leftClick;
    private Action rightClick;

    /// <summary>
    /// After the prefab is instantiated, this will set important information
    /// like the what the character looks like, what happens if the button is clicked,
    /// what happens if the mouse hovers over the gameobject, and when a mouse exits the gameobject.
    /// </summary>
    /// <param name="targ"></param>
    public void Initiate(Humanoid targ, Action onleftclick, Action onrightclick, Action onenter, Action onexit) 
    {
        if(appearanceObj == null) appearanceObj = GetComponentInChildren<AppearanceObj>();
        character = targ;
        //Debug.Log($"{character}");
        appearanceObj.SetAppearance(character.appearance);

        leftClick = onleftclick;
        rightClick = onrightclick;

        onpointerenter = onenter;
        onpointerexit = onexit;
    }

    /// <summary>
    /// Sets a new event for clicking
    /// </summary>
    /// <param name="onclick"></param>
    public void SetButtonEvent(Action onleftclick) 
    {
        leftClick = onleftclick;
    }

    /// <summary>
    /// Sets a new event for clicking
    /// </summary>
    /// <param name="onclick"></param>
    public void SetRightClickButtonEvent(Action onrightclick)
    {
        rightClick = onrightclick;
    }

    /// <summary>
    /// Sets a new event for pointer enter
    /// </summary>
    /// <param name="onclick"></param>
    public void SetPointerEnterEvent(Action _event)
    {
        onpointerenter = _event;
    }

    /// <summary>
    /// Sets a new event for pointer exit
    /// </summary>
    /// <param name="onclick"></param>
    public void SetPointerExitEvent(Action _event)
    {
        onpointerexit = _event;
    }

    /// <summary>
    /// What will happen if a pointer enters this object
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        onpointerenter?.Invoke();
    }

    /// <summary>
    /// What will happen if a pointer exits this object
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData) 
    {
        onpointerexit?.Invoke();
    }

    /// <summary>
    /// What will happen if this 
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            leftClick?.Invoke();
        }
        else if (eventData.button == PointerEventData.InputButton.Right) 
        {
            rightClick?.Invoke();
        }
    }
}
