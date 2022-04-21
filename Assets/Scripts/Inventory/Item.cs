using UnityEngine;

[CreateAssetMenu(menuName = "Items/Item")]
public class Item : ScriptableObject
{
    public string ItemName;
    public Sprite ItemIcon;
    public int MaxStackSize;

    [Header("LEAVE AT 0")]
    public int CurrentStackSize = 0;
}
