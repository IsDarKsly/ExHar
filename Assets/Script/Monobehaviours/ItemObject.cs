using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemObject : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    /// <summary>
    /// The image displayed on the item
    /// </summary>
    public Image image;

    /// <summary>
    /// Outline of an item object. Determined by the items rarity
    /// </summary>
    public Outline outline;

    public Action leftClick;
    public Action rightClick;
    public Action mouseEnter;
    public Action mouseExit;

    /// <summary>
    /// The item this gameobject will represent
    /// </summary>
    public Item item;

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

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseEnter?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseExit?.Invoke();
    }

    /// <summary>
    /// Instantiate will be used to instantiate this gameobject. This should only ever be called from the prefab of this object
    /// </summary>
    /// <returns></returns>
    public void Instantiate(Item item, Action leftClick, Action rightClick, Action mouseEnter, Action mouseExit) 
    {
        this.leftClick = leftClick;
        this.rightClick = rightClick;
        this.mouseEnter = mouseEnter;
        this.mouseExit = mouseExit;
        this.item = item;

        image.sprite = SpriteManager.Instance.GetSprite(item.Name);

        switch (item.RARITY) 
        {
            case RARITY.Common:
                outline.effectColor = Color.clear;
                break;
            case RARITY.Uncommon:
                outline.effectColor = Color.green;
                break;
            case RARITY.Rare:
                outline.effectColor = Color.blue;
                break;
            case RARITY.Epic:
                outline.effectColor = Color.magenta;
                break;
            case RARITY.Legendary:
                outline.effectColor = Color.yellow;
                break;
        }
    }

}
