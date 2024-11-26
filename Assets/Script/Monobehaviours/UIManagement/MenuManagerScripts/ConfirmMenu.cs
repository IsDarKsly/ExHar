using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

public class ConfirmMenu : MonoBehaviour
{
    //  Public variables
    public Button y_button;
    public Button n_button;
    //  Private variables

    //  Private functions
    /// <summary>
    /// Cleras any actions registered on the buttons and disables the gameobject
    /// </summary>
    private void DisableSelf() 
    {
        y_button.onClick.RemoveAllListeners();
        n_button.onClick.RemoveAllListeners();

        gameObject.SetActive(false);
    }

    //  Public functions

    /// <summary>
    /// Activates the GameObject and loads the actions into the yes button
    /// </summary>
    public void ActivateSelf(UnityAction action)
    {
        y_button.onClick.RemoveAllListeners();
        n_button.onClick.RemoveAllListeners();

        gameObject.SetActive(true);

        y_button.onClick.AddListener(action);
        y_button.onClick.AddListener(DisableSelf);

        n_button.onClick.AddListener(DisableSelf);
    }


}
