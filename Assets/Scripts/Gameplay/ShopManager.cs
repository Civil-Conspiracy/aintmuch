using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private GameObject shopDisplay;
    private ItemSlot[] displaySlots;

    private void Awake()
    {
        displaySlots = shopDisplay.GetComponentsInChildren<ItemSlot>();

        foreach(ItemSlot slot in displaySlots)
        {
            slot.GetComponent<Button>().onClick.AddListener(delegate { DisplaySlotClicked(); });
        }
    }

    public void DisplaySlotClicked()
    {
        // check to see if the player has enough money
        // if true
        // remove price from player's money
        // add item to player's inventory or whereever its going
        // else
        // no buying allowed, maybe an animation
    }
}
