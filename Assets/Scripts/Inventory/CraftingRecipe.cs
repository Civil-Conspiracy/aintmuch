using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Crafting Recipe", fileName = "Recipe")]
public class CraftingRecipe : ScriptableObject
{
    public Item Result;
    public Item[] Materials;

    private Item[] materialList;
    public int[] MaterialCount;

    [ReadOnly] public Item match1;
    [ReadOnly] public Item match2;
    [ReadOnly] public Item match3;
    [ReadOnly] public Item match4;
    [ReadOnly] public Item match5;

    private void OnValidate()
    {
        RefreshRecipe();
    }

    private void RefreshRecipe()
    {
        if (materialList != null)
            materialList = null;
        List<Item> newMaterialList = new List<Item>();
        foreach (Item item in Materials)
        {
            Item newItem;
            newItem = item;
            newMaterialList.Add(newItem);
        }
        materialList = newMaterialList.ToArray();
        for (int i = 0; i < materialList.Length; i++)
        {
            if (materialList[i] != null && materialList[i].CurrentStackSize > 0)
                materialList[i].CurrentStackSize = MaterialCount[i];
        }
    }

    public void CraftItem(List<Item> bagItems, Item offhandItem)
    {

    }

    public bool CanCraft(List<Item> bagItems, Item offhandItem)
    {
        int numberOfMats = materialList.Length;
        int numberOfMatches = 0;
        bool matches1 = false;
        bool matches2 = false;
        bool matches3 = false;
        bool matches4 = false;
        bool matches5 = false;
        foreach (Item item in bagItems)
        {
            int matchValue = (Match(item));
            switch (matchValue)
            {
                case 1:
                    matches1 = true;
                    break;
                case 2:
                    matches2 = true;
                    break;
                case 3:
                    matches3 = true;
                    break;
                case 4:
                    matches4 = true;
                    break;
                case 5:
                    matches5 = true;
                    break;
                default:
                    break;
            }
        }
        if (matches1 && numberOfMats >= 1)
            numberOfMatches++;
        if (matches2 && numberOfMats >= 2)
            numberOfMatches++;
        if (matches3 && numberOfMats >= 3)
            numberOfMatches++;
        if (matches4 && numberOfMats >= 4)
            numberOfMatches++;
        if (matches5 && numberOfMats >= 5)
            numberOfMatches++;

        if (numberOfMatches == numberOfMats)
        {
            return true;
        }
        else
            return false;
        
    }

    private int Match(Item item)
    {
        int numberOfMats = materialList.Length;
        int matchHash = -1;
        for (int i = 0; i < materialList.Length; i++)
        {
            if (materialList[i] != null && item.ItemName == materialList[i].ItemName && item.CurrentStackSize >= materialList[i].CurrentStackSize)
            {
                matchHash = i;
                switch (matchHash)
                {
                    case 0:
                        if (numberOfMats >= 1)
                        {
                            match1 = item;
                            match1.CurrentStackSize = materialList[i].CurrentStackSize;
                            return 1;
                        }
                        else break;
                    case 1:
                        if (numberOfMats >= 2)
                        {
                            match2 = item;
                            match2.CurrentStackSize = materialList[i].CurrentStackSize;
                            return 2;
                        }
                        else break;
                    case 2:
                        if (numberOfMats >= 3)
                        {
                            match3 = item;
                            match3.CurrentStackSize = materialList[i].CurrentStackSize;
                            return 3;
                        }
                        else break;
                    case 3:
                        if (numberOfMats >= 4)
                        {
                            match4 = item;
                            match4.CurrentStackSize = materialList[i].CurrentStackSize;
                            return 4;
                        }
                        else break;
                    case 4:
                        if (numberOfMats >= 1)
                        {
                            match5 = item;
                            match5.CurrentStackSize = materialList[i].CurrentStackSize;
                            return 5;
                        }
                        else break;
                    default:
                        return 0;
                }
            }         

        }
        return 0;
    }
}