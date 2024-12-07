using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceAnimatingScript : MonoBehaviour, AnimationController
{
    public GameObject Owner;
    public int value;
    public DamageSubType subType;
    public RESOURCES resourceType;

    bool isComplete = false;

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

        switch (resourceValues.type)    //  Setting the image of the element type 
        {
            case DamageSubType.Crushing:
                image.sprite = SpriteManager.Instance.GetSprite("Crushing");
                break;
            case DamageSubType.Slashing:
                image.sprite = SpriteManager.Instance.GetSprite("Slashing");
                break;
            case DamageSubType.Stabbing:
                image.sprite = SpriteManager.Instance.GetSprite("Stabbing");
                break;
            case DamageSubType.Fire:
                image.sprite = SpriteManager.Instance.GetSprite("Fire");
                break;
            case DamageSubType.Water:
                image.sprite = SpriteManager.Instance.GetSprite("Water");
                break;
            case DamageSubType.Ice:
                image.sprite = SpriteManager.Instance.GetSprite("Ice");
                break;
            case DamageSubType.Lightning:
                image.sprite = SpriteManager.Instance.GetSprite("Lightning");
                break;
            case DamageSubType.Light:
                image.sprite = SpriteManager.Instance.GetSprite("Light");
                break;
            case DamageSubType.Dark:
                image.sprite = SpriteManager.Instance.GetSprite("Dark");
                break;
        }

        gameObject.SetActive(false);
    }

    public void SetUp(Humanoid caster, Humanoid target)
    {
        return;
    }
}