using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DetailsMenu : MonoBehaviour
{
    public GameObject DetailPanel;

    public TMP_Text TitleText;
    public TMP_Text DetailText;

    public Button y_button;
    public Button n_button;

    private void DisableSelf()
    {
        y_button.onClick.RemoveAllListeners();
        n_button.onClick.RemoveAllListeners();

        gameObject.SetActive(false);
    }

    //  public functions
    /// <summary>
    /// Activates the Details panel, and sets the transform to either the left or right side of the screen dependant on mouse position
    /// </summary>
    /// <param name="title"></param>
    /// <param name="details"></param>
    public void ActiveDetails(string title, string details) 
    {
        y_button.gameObject.SetActive(false);
        n_button.gameObject.SetActive(false);
        DetailPanel.transform.localPosition = MenuManager.Instance.mouse_is_left ? new Vector2(500, -50) : new Vector2(-500, -50);

        gameObject.SetActive(true);

        TitleText.text = title;
        DetailText.text = details;

    }

    /// <summary>
    /// Activates the Details panel, and sets the transform to either the left or right side of the screen dependant on mouse position
    /// Additionally sets functions for the optional accept button.
    /// </summary>
    public void ActiveDetails(string title, string details, UnityAction action)
    {
        y_button.onClick.RemoveAllListeners();
        n_button.onClick.RemoveAllListeners();
        gameObject.SetActive(true);
        y_button.gameObject.SetActive(true);
        n_button.gameObject.SetActive(true);

        y_button.onClick.AddListener(action);
        y_button.onClick.AddListener(DisableSelf);

        n_button.onClick.AddListener(DisableSelf);

        DetailPanel.transform.localPosition = MenuManager.Instance.mouse_is_left ? new Vector2(500, -50) : new Vector2(-500, -50);

        gameObject.SetActive(true);

        TitleText.text = title;
        DetailText.text = details;
    }

    /// <summary>
    /// Hides this gameobject
    /// </summary>
    public void SetActive(bool t) 
    {
        gameObject.SetActive(t);
    }

}
