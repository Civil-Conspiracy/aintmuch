using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInventoryManager : MonoBehaviour
{
    [SerializeField] Item offhandItem;
    [SerializeField] Transform offhandParent;
    [SerializeField] ItemSlot offhandSlot;

    [SerializeField] List<Item> bagItems;
    [SerializeField] Transform bagParent;
    [SerializeField] List<ItemSlot> bagSlots;


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
            if(bagItems.Count > bagSlots.Count)
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
            offhandSlot.Item = offhandItem;
    }

    //public bool AddItemCheck(Item item)
    //{
    //    //check if item can stack in offhand
    //    //check if item can stack in bag
    //
    //    //if offhand is empty, and item can't stack in bag
    //        //add item to offhandItems and destroy item.gameobject
    //    //else, if bag isn't full, and item can't stack in offhand
    //        //add item to bagItems and destroy item.gameobject
    //    //else, if offhand is full and item can't stack in bag
    //        //if item can stack, existing item's currentstack++ and destroy item.gameobject
    //    //else, if bag is full
    //        //if item can stack, existing item's currentstack++ and destroy item.gameobject
    //}

}
