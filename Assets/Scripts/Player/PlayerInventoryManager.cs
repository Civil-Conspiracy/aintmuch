using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInventoryManager : MonoBehaviour
{
    public static PlayerInventoryManager instance;
    [SerializeField] Item offhandItem;
    [SerializeField] Transform offhandParent;
    [SerializeField] ItemSlot offhandSlot;

    [SerializeField] List<Item> bagItems;
    [SerializeField] Transform bagParent;
    [SerializeField] List<ItemSlot> bagSlots;
    [SerializeField] int maxBagSlots = 5;

    private void Awake()
    {
        #region Singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        #endregion

        GetHUD();
    }

    private void OnValidate()
    {
        GetHUD();

        RefreshHUD();
    }

    private void GetHUD()
    {
        //offhand slot
        if (offhandParent != null)
        {
            offhandSlot = offhandParent.GetComponentInChildren<ItemSlot>();
        }

        //bag slots
        if (bagParent != null)
        {
            bagSlots = bagParent.GetComponentsInChildren<ItemSlot>().ToList();
            if(bagItems.Count > bagSlots.Count && bagSlots.Count < maxBagSlots)
            {
                //instantiate a new bag slot until there are as many slots as items
                
            }
        }
    }
    private void RefreshHUD()
    {
        int i = 0;
        for (; i < bagItems.Count && i < bagSlots.Count; i++)
        {
            bagSlots[i].Item = bagItems[i];
        }
        for (; i < bagSlots.Count; i++)
        {
            bagSlots[i].Item = null;
        }
        if (offhandItem != null)
        {
            offhandSlot.Item = offhandItem;
        }
    }

    public void AddItem(Item item, FloorItem floorItem)
    {
        ItemSlot bagSlotToStack = Contains(bagSlots, item);
        bool canStackOffhand = AddToOffhandItemCheck(item);

        //if item can stack in offhand
        if (canStackOffhand && offhandSlot.CurrentStackCount < offhandItem.MaxStackSize)
        {
            offhandSlot.CurrentStackCount++;
            Destroy(floorItem.gameObject);
        }
        //if item can stack in bag
        else if (bagSlotToStack != null && bagSlotToStack.CurrentStackCount < item.MaxStackSize)
        {
            bagSlotToStack.CurrentStackCount++;
            Destroy(floorItem.gameObject);
        }
        //if offhand is empty, and item can't stack in bag
        else if (offhandItem == null)
        {
            offhandItem = item;
            offhandSlot.CurrentStackCount = 1;
            Destroy(floorItem.gameObject);
        }
        //if bag isn't full, and item can't stack in offhand
        else if (bagItems.Count < maxBagSlots)
        {
            bagItems.Add(item);
            ItemSlot bagSlotToAdd = bagSlots[bagItems.Count - 1];
            bagSlotToAdd.CurrentStackCount = 1;
            Destroy(floorItem.gameObject);
        }
        else
            Debug.Log("couldn't add item to inventory.");

        RefreshHUD();
    }

    public bool RemoveItem(Item item, int quantity)
    {
        ItemSlot temp = Contains(bagSlots, item);
        if (temp != null)
        {
            if (temp.CurrentStackCount > 1)
                temp.CurrentStackCount -= quantity;
            else
            {

                ItemSlot slotToRemove;
                foreach (ItemSlot slot in bagSlots)
                {
                    if (slot.Item.ItemName == item.ItemName)
                    {
                        slotToRemove = slot;
                        slotToRemove.Item = null;
                        break;
                    }
                }
            }
        }
        else
        {
            return false;
        }

        RefreshHUD();
        return true;
    }
    private bool AddToOffhandItemCheck(Item item)
    {
        if (offhandItem == item)
            return true;
        else
            return false;
    }
    public ItemSlot Contains(List<ItemSlot> list, Item item)
    {
        foreach(ItemSlot slot in list)
        {
            if (slot.Item == item && slot.CurrentStackCount < item.MaxStackSize)
                return slot;
        }

        return null;
    }
}
