using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceAnimatingScript : MonoBehaviour, AnimationController
{
    public GameObject Owner;
    public int value;
    public DamageSubType subType;
    public RESOURCES resourceType;

    public GameObject CriticalIcon;

    /// <summary>
    /// I will set this to true a short while after it starts. This is to support multiple target animations
    /// </summary>
    bool isComplete = true;

    TMP_Text text;
    Image image;



    public bool IsComplete()
    {
        return isComplete;
    }

    public void PlayAnimation()
    {
        gameObject.SetActive(true);
        StartCoroutine(Animate());
    }

    public IEnumerator Animate() 
    {
        // Get starting position
        Vector3 startPosition = transform.position;
        Vector3 endPosition;

        if (transform.position.y < Screen.height / 2) // Bottom side of the screen
        {
            endPosition = startPosition + new Vector3(0, 50, 0); // Move up 50 pixels
        }
        else
        {
            endPosition = startPosition - new Vector3(0, 50, 0); // Move down 50 pixels
        }

        // Move the text over 2 seconds
        float duration = 1f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = endPosition;

        // Fade out over 1 second
        float fadeDuration = 0.5f;
        float fadeElapsedTime = 0f;

        Color originalColor = text.color;

        while (fadeElapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, fadeElapsedTime / fadeDuration);
            text.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            if (image != null)
            {
                image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
            }
            fadeElapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure text and image are fully transparent
        text.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        if (image != null)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, 0f);
        }

        // Mark the animation as complete and destroy the GameObject
        isComplete = true;
        Destroy(gameObject);

    }

    /// <summary>
    /// The setup function sets all the corresponding values to be animated
    /// </summary>
    /// <param name="resourceValues"></param>
    public void SetUp(AnimatableResourceChange resourceValues)
    {
        Owner = BattleManager.Instance.battleUI.FindCharacterObject(resourceValues.target);
        transform.SetParent(Owner.transform);
        value = resourceValues.resourceAmount;
        subType = resourceValues.type;
        resourceType = resourceValues.resourceType;
        text = GetComponent<TMP_Text>();
        if(value > 0) text.text = "+" + value.ToString();
        else text.text = value.ToString();

        if(!resourceValues.isCrit) CriticalIcon.SetActive(false);

        image = GetComponentInChildren<Image>();
        transform.position = Owner.transform.position;

        if (transform.position.y < Screen.height / 2) //Bottom side of screen
        {
            transform.Translate(new Vector3(0, 125, 0));  //  Move up 125 pixels
        }
        else 
        {
            transform.Translate(new Vector3(0, -125, 0));  //  Move up 125 pixels
        }

        switch (resourceValues.resourceType) // Whether we change the color of text
        {
            case RESOURCES.Health:
                text.color = Color.red;
                break;
            case RESOURCES.Stamina:
                text.color = Color.yellow;
                break;
            case RESOURCES.Mana:
                text.color = Color.blue;
                break;
        }

        image.sprite = SpriteManager.Instance.GetSubtypeSprite(subType);

        gameObject.SetActive(false);
    }

    public void SetUp(Humanoid caster, List<Humanoid> target)
    {
        return;
    }
}
