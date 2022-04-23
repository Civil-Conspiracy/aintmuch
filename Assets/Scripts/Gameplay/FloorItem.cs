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
    float _timeSinceDropped = 0;
    readonly float _pickupCooldown = 5f;

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

    public void WasDropped()
    {
        _timeSinceDropped += _pickupCooldown;
    }

    public void SetInfo(Item item, int stackCount)
    {
        thisItem = item;
        thisItem.CurrentStackSize = Mathf.Clamp(stackCount, 0, thisItem.MaxStackSize);
        sr.sprite = thisItem.ItemIcon;
        sr.color = thisItem.ItemColor;
        UpdateText();
    }
    public void UpdateText()
    {
        thisName.text = thisItem.ItemName + " x" + thisItem.CurrentStackSize;
    }

    private void Update()
    {
        _timeSinceDropped -= Time.deltaTime;
        if(_timeSinceDropped < 0)
        {
            if (Physics2D.OverlapBox(transform.position, Vector2.one, 0, _playerMask)) 
                PlayerInventoryManager.instance.AddItem(thisItem, this);
        }
        
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
