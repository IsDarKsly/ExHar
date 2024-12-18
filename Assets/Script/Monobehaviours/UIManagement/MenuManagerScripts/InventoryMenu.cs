using TMPro;
using UnityEngine;

/// <summary>
/// The inventory system will display any item that a player has equipped along with the full list
/// of items in the players inventory. Items should be filtered by catagories of all, weapons, armor, jewellery,
/// and consumables. The player should be able to enter the name of the item to find what they are looking for within
/// those restrictions
/// </summary>
public class InventoryMenu : MonoBehaviour
{
    public GameObject InventoryObjectPrefab;
    /// <summary>
    /// The transform that inventory items will be assigned to
    /// </summary>
    public Transform ItemRosterParent;

    /// <summary>
    /// The input field that will be used to sort items by name
    /// </summary>
    public TMP_InputField input;

    /// <summary>
    /// The name text for this character
    /// </summary>
    public TMP_Text NameText;

    public AppearanceObj appearanceObj;

    /// <summary>
    /// Sets the gameobject to either active or not
    /// </summary>
    /// <param name="b"></param>
    public void SetActive(bool b) 
    {
        gameObject.SetActive(b);
    }

    /// <summary>
    /// Everytime this menu is enabled we should update the active character
    /// </summary>
    private void OnEnable()
    {
        UpdateActiveCharacter();
    }

    /// <summary>
    /// Destroys all possible gameobjects representing the players equipment
    /// also lel
    /// </summary>
    private void DestroyChildren() 
    {
        var EquipSlots = gameObject.transform.Find("InventoryPanel/EquipSlots");
        //Debug.Log(EquipSlots);
        foreach (Transform child in EquipSlots) 
        {
            //Debug.Log(child);
            foreach (Transform childrenschildren in child) 
            {
                //Debug.Log(childrenschildren);
                if (childrenschildren.childCount > 0) Destroy(childrenschildren.GetChild(0).gameObject);
            }
        }
    }

    /// <summary>
    /// When an item is clicked, it should Use
    /// </summary>
    public void ItemClick(ItemObject itemObject) 
    {
        if (itemObject.item.Use(MenuManager.Instance.GetCurrentCharacter()) && itemObject.item.ItemType != ItemType.REUSABLE) // The item was used successfully
        {
            Debug.Log("Item used");
        }
        UpdateActiveCharacter();
    }

    /// <summary>
    /// This will populate the inventory with all the items a character has
    /// Done after loading a save file
    /// </summary>
    public void Load() 
    {
        foreach (var item in DataManager.Instance.inventory.GetInventory()) 
        {
            CreateObjectForInventory(item);
        }

        UpdateActiveCharacter();
    }

    /// <summary>
    /// The create object for inventory function serves to create the visual component for
    /// any object added to the inventory through the datamanager
    /// </summary>
    public void CreateObjectForInventory(Item item) 
    {
        //Debug.Log("Creating object for inventory");
        var clone = Instantiate(InventoryObjectPrefab, ItemRosterParent);
        var itemObj = clone.GetComponent<ItemObject>();
        itemObj.Instantiate(item, () => ItemClick(itemObj), null, null, null);
    }

    /// <summary>
    /// The create object for inventory function serves to create the visual component for
    /// any object added to the inventory through the datamanager
    /// </summary>
    public void CreateObjectForEquipmentSlot(Item item, Transform transform)
    {
        //Debug.Log("Creating object for equipment slot");
        var clone = Instantiate(InventoryObjectPrefab, transform);
        var itemObj = clone.GetComponent<ItemObject>();
        itemObj.Instantiate(item, () => ItemClick(itemObj), null, null, null);
    }

    /// <summary>
    /// This function should really only be called from the datamanager to remind
    /// the inventory menu what should not be displayed
    /// </summary>
    public void RemoveObjectFromInventory(Item item) 
    {
        Debug.Log("Deleting visual object from inventory");
        foreach (var itemObj in ItemRosterParent.GetComponentsInChildren<ItemObject>())
        {
            if (itemObj.item == item) 
            {
                Debug.Log("Item found, Destroying now");
                Destroy(itemObj.gameObject);
                return;
            }
        }
    }

    /// <summary>
    /// Update Active Character will delete all gameobjects meant to represent a given current character,
    /// And then 
    /// </summary>
    public void UpdateActiveCharacter() 
    {
        DestroyChildren();

        var character = MenuManager.Instance.GetCurrentCharacter();
        appearanceObj.SetAppearance(character.appearance);
        NameText.text = character.GetName();

        var EquipSlots = gameObject.transform.Find("InventoryPanel/EquipSlots");

        if (character.Helmet != null) CreateObjectForEquipmentSlot(character.Helmet, EquipSlots.Find("ArmorSlots/HeadSlot"));
        if (character.Chest != null) CreateObjectForEquipmentSlot(character.Chest, EquipSlots.Find("ArmorSlots/ChestSlot"));
        if (character.Leggings != null) CreateObjectForEquipmentSlot(character.Leggings, EquipSlots.Find("ArmorSlots/LeggingSlot"));
        if (character.Boots != null) CreateObjectForEquipmentSlot(character.Boots, EquipSlots.Find("ArmorSlots/BootSlot"));
        if (character.Gloves != null) CreateObjectForEquipmentSlot(character.Gloves, EquipSlots.Find("ArmorSlots2/GloveSlot"));
        if (character.Amulet != null) CreateObjectForEquipmentSlot(character.Amulet, EquipSlots.Find("ArmorSlots2/AmuletSlot"));
        if (character.Ring1 != null) CreateObjectForEquipmentSlot(character.Ring1, EquipSlots.Find("ArmorSlots2/Ring1Slot"));
        if (character.Ring2 != null) CreateObjectForEquipmentSlot(character.Ring2, EquipSlots.Find("ArmorSlots2/Ring2Slot"));
        if (character.MainHand != null) CreateObjectForEquipmentSlot(character.MainHand, EquipSlots.Find("WeaponSlots/MainHandSlot"));
        if (character.OffHand != null) CreateObjectForEquipmentSlot(character.OffHand, EquipSlots.Find("WeaponSlots/OffHandSlot"));

    }

    /// <summary>
    /// When a character is hovered over, this shows important information about them
    /// </summary>
    public void ShowCharacterDetails() 
    {
        MenuManager.Instance.ShowHumanoidDetails(MenuManager.Instance.GetCurrentCharacter());
    }

    /// <summary>
    /// When a pointer leaves the character portrait, we no longer show pertanent information (whatever that means)
    /// </summary>
    public void HideCharacterDetails() 
    {
        MenuManager.Instance.HideHumanoidDetails();
    }

    /// <summary>
    /// Sorts the item list based on the name input
    /// </summary>
    public void SortByName()
    {
        if (input.text == "")
        {
            SortItemList((int)ItemSort.UNSORT);
            return;
        }
        foreach (var itemObj in ItemRosterParent.GetComponentsInChildren<ItemObject>())
        {
            itemObj.gameObject.SetActive(false);
            if (itemObj.item.GetName().ToLower().Contains(input.text.ToLower()))
            {
                itemObj.gameObject.SetActive(true);
            }
        }
    }

    /// <summary>
    /// Given an enum, we search through the itemroster parent. This should save me time making a sorting list
    /// </summary>
    /// 
    public void SortItemList( int itemSort ) 
    {
        //Debug.Log("Starting Sort");
        foreach (var itemObj in ItemRosterParent.GetComponentsInChildren<ItemObject>(includeInactive: true)) 
        {
            //Debug.Log($"{itemObj.name} found, sorting now");
            itemObj.gameObject.SetActive(true);
            switch ((ItemSort)itemSort) 
            {
                case ItemSort.UNSORT:   //  We let unsort do nothing as the gameobject is already set true at start of function
                    break;
                case ItemSort.WEAPONS:
                    if (!itemObj.item.GetType().IsSubclassOf(typeof(Weapon))) // This item isnt a weapon
                    {
                        itemObj.gameObject.SetActive(false);
                    }
                    break;
                case ItemSort.HELMET:
                    if (!itemObj.item.GetType().IsSubclassOf(typeof(Equipment))) // This item isnt equipment
                    {
                        itemObj.gameObject.SetActive(false);
                    }
                    else if (((Equipment)itemObj.item).equipmentslot != EQUIPMENTSLOT.Helm)   //  This item is equipment but is not the defined sort
                    {
                        itemObj.gameObject.SetActive(false);
                    }
                    break;
                case ItemSort.CHEST:
                    if (!itemObj.item.GetType().IsSubclassOf(typeof(Equipment))) // This item isnt equipment
                    {
                        itemObj.gameObject.SetActive(false);
                    }
                    else if (((Equipment)itemObj.item).equipmentslot != EQUIPMENTSLOT.Chest)   //  This item is equipment but is not the defined sort
                    {
                        itemObj.gameObject.SetActive(false);
                    }
                    break;
                case ItemSort.LEGGINGS:
                    if (!itemObj.item.GetType().IsSubclassOf(typeof(Equipment))) // This item isnt equipment
                    {
                        itemObj.gameObject.SetActive(false);
                    }
                    else if (((Equipment)itemObj.item).equipmentslot != EQUIPMENTSLOT.Legs)   //  This item is equipment but is not the defined sort
                    {
                        itemObj.gameObject.SetActive(false);
                    }
                    break;
                case ItemSort.BOOTS:
                    if (!itemObj.item.GetType().IsSubclassOf(typeof(Equipment))) // This item isnt equipment
                    {
                        itemObj.gameObject.SetActive(false);
                    }
                    else if (((Equipment)itemObj.item).equipmentslot != EQUIPMENTSLOT.Boots)   //  This item is equipment but is not the defined sort
                    {
                        itemObj.gameObject.SetActive(false);
                    }
                    break;
                case ItemSort.GLOVES:
                    if (!itemObj.item.GetType().IsSubclassOf(typeof(Equipment))) // This item isnt equipment
                    {
                        itemObj.gameObject.SetActive(false);
                    }
                    else if (((Equipment)itemObj.item).equipmentslot != EQUIPMENTSLOT.Gloves)   //  This item is equipment but is not the defined sort
                    {
                        itemObj.gameObject.SetActive(false);
                    }
                    break;
                case ItemSort.JEWELRY:
                    if (!itemObj.item.GetType().IsSubclassOf(typeof(Jewelry))) // This item isnt jewelry
                    {
                        itemObj.gameObject.SetActive(false);
                    }
                    break;
                case ItemSort.CONSUMABLE:
                    if(!itemObj.item.GetType().IsSubclassOf(typeof(Consumable))) itemObj.gameObject.SetActive(false);
                    break;
                case ItemSort.QUEST:
                    if(itemObj.item.ItemType != ItemType.REUSABLE) itemObj.gameObject.SetActive(false);
                    break;
            }
        }
    }

}

public enum ItemSort { UNSORT, WEAPONS, HELMET, CHEST, LEGGINGS, BOOTS, GLOVES, JEWELRY, CONSUMABLE, QUEST}
