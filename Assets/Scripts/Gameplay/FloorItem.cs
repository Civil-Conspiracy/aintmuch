using UnityEngine;
using Unity.Collections;
using TMPro;

public class FloorItem : MonoBehaviour
{
    [SerializeField, ReadOnly] Item thisItem;
    SpriteRenderer sr;
    TextMeshPro thisName;
    //[SerializeField] bool _requiresInteract;
    [SerializeField] LayerMask _playerMask;
    [SerializeField] LayerMask _itemMask;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        thisName = transform.Find("Name").GetComponent<TextMeshPro>();
    }

    public int CurrentStackCount
    {
        get { return thisItem.CurrentStackSize; }
        set
        {
            thisItem.CurrentStackSize = Mathf.Max(value, thisItem.MaxStackSize);
        }
    }

    public void SetInfo(Item item, int stackCount)
    {
        thisItem = item;
        thisItem.CurrentStackSize = Mathf.Clamp(stackCount, 0, thisItem.MaxStackSize);
        sr.sprite = thisItem.ItemIcon;
        UpdateText();
    }
    private void UpdateText()
    {
        thisName.text = thisItem.ItemName + " x" + thisItem.CurrentStackSize;
    }

    private void Update()
    {
        if (Physics2D.OverlapBox(transform.position, Vector2.one, 0, _playerMask)) 
            PlayerInventoryManager.instance.AddItem(thisItem, this);

        
        if(Physics2D.OverlapBox(transform.position, Vector2.one, 0, _itemMask))
        {
            FloorItem otherItem = Physics2D.OverlapBox(transform.position, Vector2.one, 0, _itemMask).GetComponent<FloorItem>();

            if (otherItem.thisItem.ItemName == thisItem.ItemName && otherItem != this)
            {
                int newStackCount = otherItem.thisItem.CurrentStackSize + CurrentStackCount;
                if (newStackCount <= thisItem.MaxStackSize)
                {
                    thisItem.CurrentStackSize = newStackCount;
                    Destroy(otherItem.gameObject);
                    UpdateText();
                }
            }
        }
    }
}
