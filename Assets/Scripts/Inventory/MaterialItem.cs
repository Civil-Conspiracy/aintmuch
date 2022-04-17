using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialItem : BaseItem
{
    public override BaseItem GetItem()
    {
        return this;
    }

    public override MaterialItem GetMaterial()
    {
        return this;
    }

    public override SellableItem GetSellable()
    {
        return null;
    }
}
