using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInventoryManager : MonoBehaviour
{
    public static PlayerInventoryManager instance;
    [SerializeField] GameObject floorItem;
    [SerializeField] Item offhandItem;
    [SerializeField] Transform offhandParent;
    [SerializeField] ItemSlot offhandSlot;

    [SerializeField] List<Item> bagItems;
    [SerializeField] Transform bagParent;
    [SerializeField] List<ItemSlot> bagSlots;

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

    private void Update()
    {
        if (offhandItem == null && bagItems.Count > 0)
        {
            ScrollForward();
        }
    }

    private void Start()
    {
        InputManager.instance.OnScrollForward += args => OnScrollForward(args);
        InputManager.instance.OnScrollBackward += args => OnScrollBackward(args);
    }

    public void OnScrollForward(InputManager.InputArgs args)
    {
        bool pressed = args.context.ReadValueAsButton();
        if (pressed)
            ScrollForward();
    }    
    public void OnScrollBackward(InputManager.InputArgs args)
    {
        bool pressed = args.context.ReadValueAsButton();
        if (pressed)
            ScrollBackward();
    }

    private void OnValidate()
    {
        GetHUD();

        RefreshHUD();
    }

    #region HUD DISPLAY
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
            offhandSlot.Item = offhandItem;
    }
    #endregion

    #region ADD ITEMS
    public void AddItem(Item item, FloorItem floorItem)
    {
        if (IsFull())
            return;
        Item bagItemToStack = Contains(bagItems, item);
        bool canStackOffhand = AddToOffhandItemCheck(item);

        //if item can stack in offhand (item matches offhand item && offhand isn't full)
        if (canStackOffhand)
        {
            int newOffhandStackSize = offhandItem.CurrentStackSize + item.CurrentStackSize;
            //if entire stack can be picked up
            if (newOffhandStackSize <= offhandItem.MaxStackSize)
            {
                offhandItem.CurrentStackSize = newOffhandStackSize;
                Destroy(floorItem.gameObject);
            }
            //if only some of it can be picked up
            else
            {
                int amountAvailable = offhandItem.MaxStackSize - offhandItem.CurrentStackSize;
                int amountToAdd = Mathf.Min(item.CurrentStackSize, amountAvailable);
                offhandItem.CurrentStackSize += amountToAdd;
                item.CurrentStackSize -= amountToAdd;
                floorItem.UpdateText();
            }
        }
        //if item can stack in bag (item matches bag item && bag item isn't fully stacked)
        else if (bagItemToStack != null)
        {
            int newBagStackSize = bagItemToStack.CurrentStackSize + item.CurrentStackSize;
            //if entire stack can be picked up
            if(newBagStackSize <= bagItemToStack.MaxStackSize)
            {
                bagItemToStack.CurrentStackSize += item.CurrentStackSize;
                Destroy(floorItem.gameObject);
            }
            //if only some of it can be picked up
            else
            {
                int amountAvailable = bagItemToStack.MaxStackSize - bagItemToStack.CurrentStackSize;
                int amountToAdd = Mathf.Min(item.CurrentStackSize, amountAvailable);
                bagItemToStack.CurrentStackSize += amountToAdd;
                item.CurrentStackSize -= amountToAdd;
                floorItem.UpdateText();
            }
        }
        //if offhand isn't full, and item can't stack in bag
        else if (offhandItem == null)
        {
            offhandItem = Instantiate(item);
            Destroy(floorItem.gameObject);
        }
        //if bag isn't full, and item can't stack in offhand
        else if (bagItems.Count < bagSlots.Count)
        {
            bagItems.Add(Instantiate(item));
            Destroy(floorItem.gameObject);
        }
        else
            Debug.Log("couldn't add item to inventory.");

        RefreshHUD();
    }

    private bool AddToOffhandItemCheck(Item item)
    {
        if(offhandItem != null)
        {
            if (offhandItem.ItemName == item.ItemName && offhandItem.CurrentStackSize < offhandItem.MaxStackSize)
                return true;
        }

        return false;
    }
    #endregion

    #region REMOVE ITEMS
    public bool RemoveItemFromOffhand(int quantity)
    {
        int newQuantity = Mathf.Min(quantity, offhandItem.MaxStackSize);
        if (offhandItem != null)
        {
            if (offhandItem.CurrentStackSize > 1)
                offhandItem.CurrentStackSize -= newQuantity;
            else
            {
                offhandItem = null;
            }
        }
        else
        {
            return false;
        }

        RefreshHUD();
        return true;
    }

    public bool DropItemFromOffhand(bool isFacingRight, Vector2 spawnPos)
    {
        Item temp = offhandItem;
        bool removed = RemoveItemFromOffhand(1);
        if (removed)
        {
            GameObject lootDrop = Instantiate(floorItem, spawnPos, Quaternion.identity);
            lootDrop.GetComponent<FloorItem>().SetInfo(Instantiate(temp), 1);
            lootDrop.GetComponent<FloorItem>().WasDropped();
            return true;
        }
        return false;
    }

    //this works, just no reason to use it from what I can see (so far).  but I made it so I'm leaving it here.
    public bool RemoveItemFromBag(Item item, int quantity)
    {
        int newQuantity = Mathf.Min(quantity, item.MaxStackSize);
        Item slotToRemove = Contains(bagItems, item, true);
        if (slotToRemove != null)
        {
            if (slotToRemove.CurrentStackSize > 1)
                slotToRemove.CurrentStackSize -= newQuantity;
            else
            {
                int itemToRemove = bagItems.LastIndexOf(slotToRemove);
                bagItems.RemoveAt(itemToRemove);
            }
        }
        else
        {
            return false;
        }

        RefreshHUD();
        return true;
    }
    #endregion

    #region SCROLL ITEMS
    public void ScrollForward()
    {
        Item toSwapFromBag = bagItems[0];
        Item toSwapFromOffhand = offhandItem;
        // if there is an item to swap from the bag, and an item to swap from the offhand
        if (toSwapFromBag != null && toSwapFromOffhand != null)
        {
            offhandItem = toSwapFromBag;
            bagItems.RemoveAt(0);
            bagItems.Add(toSwapFromOffhand);

        }
        // if there is an item to swap from the bag, but the offhand is empty
        else if (toSwapFromBag != null && toSwapFromOffhand == null)
        {
            offhandItem = toSwapFromBag;
            bagItems.RemoveAt(0);
        }

        RefreshHUD();
    }

    public void ScrollBackward()
    {
        Item toSwapFromBag = bagItems[bagItems.Count - 1];
        Item toSwapFromOffhand = offhandItem;
        // if there is an item to swap from the bag, and an item to swap from the offhand
        if (toSwapFromBag != null && toSwapFromOffhand != null)
        {
            offhandItem = toSwapFromBag;
            bagItems.RemoveAt(bagItems.Count - 1);
            bagItems.Insert(0, toSwapFromOffhand);

        }
        // if there is an item to swap from the bag, but the offhand is empty
        else if (toSwapFromBag != null && toSwapFromOffhand == null)
        {
            offhandItem = toSwapFromBag;
            bagItems.RemoveAt(bagItems.Count - 1);
        }

        RefreshHUD();
    }

    #endregion
    public Item Contains(List<Item> list, Item item)
    {
        foreach(Item slot in list)
        {
            if (slot.ItemName == item.ItemName && slot.CurrentStackSize < item.MaxStackSize)
                return slot;
        }

        return null;
    }

    public Item Contains(List<Item> list, Item item, bool toRemove)
    {
        foreach (Item slot in list)
        {
            if (slot.ItemName == item.ItemName && slot.CurrentStackSize < slot.MaxStackSize)
                return slot;
        }

        return null;
    }

    public bool IsFull()
    {
        if (offhandItem == null || offhandItem.CurrentStackSize < offhandItem.MaxStackSize)
            return false;
        foreach (Item slot in bagItems)
        {
            if (slot.CurrentStackSize < slot.MaxStackSize)
                return false;
        }
        return bagItems.Count >= bagSlots.Count && offhandItem != null;
    }

}
