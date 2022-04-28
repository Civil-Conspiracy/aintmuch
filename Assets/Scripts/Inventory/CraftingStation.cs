using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingStation : MonoBehaviour
{
    [SerializeField] private CraftingRecipe[] recipes;

    [SerializeField] private ItemSlot resultSlot;
    [SerializeField] private List<ItemSlot> materialSlots;

}
