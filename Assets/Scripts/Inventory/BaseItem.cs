using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseItem
{
    [Header("Item")]
    public string itemName;
    public Sprite itemIcon;

    public abstract BaseItem GetItem();
    public abstract MaterialItem GetMaterial();
    public abstract SellableItem GetSellable();
}
