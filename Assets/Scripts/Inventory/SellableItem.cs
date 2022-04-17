using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellableItem : BaseItem
{
    [Header("Sellable Item")]
    public int ItemSellValue;

    public override BaseItem GetItem()
    {
        return this;
    }

    public override MaterialItem GetMaterial()
    {
        return null;
    }

    public override SellableItem GetSellable()
    {
        return this;
    }
}
