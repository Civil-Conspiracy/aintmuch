using UnityEngine;
using TMPro;

public class FloorItem : MonoBehaviour
{
    Item thisItem;
    SpriteRenderer sr;
    TextMeshPro thisName;
    int thisStackCount;
    [SerializeField] LayerMask _playerMask;
    [SerializeField] LayerMask _itemMask;
    [SerializeField] int _startingItemCount;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        thisName = transform.Find("Name").GetComponent<TextMeshPro>();
        thisStackCount = _startingItemCount;
    }

    public int CurrentStackCount { get { return thisStackCount; } }

    public void SetInfo(Item item)
    {
        thisItem = item;
        sr.sprite = thisItem.ItemIcon;
        UpdateText();
    }
    private void UpdateText()
    {
        thisName.text = thisItem.ItemName + " x" + CurrentStackCount;
    }

    private void Update()
    {
        if (Physics2D.OverlapBox(transform.position, Vector2.one, 0, _playerMask))
            PlayerInventoryManager.instance.GetComponent<PlayerInventoryManager>().AddItem(thisItem, this);

        
        if(Physics2D.OverlapBox(transform.position, Vector2.one, 0, _itemMask))
        {
            FloorItem otherItem = Physics2D.OverlapBox(transform.position, Vector2.one, 0, _itemMask).GetComponent<FloorItem>();

            if (otherItem.thisItem == thisItem && otherItem != this)
            {
                int newStackCount = otherItem.CurrentStackCount + thisStackCount;
                if (newStackCount <= thisItem.MaxStackSize)
                {
                    thisStackCount = newStackCount;
                    Destroy(otherItem.gameObject);
                    UpdateText();
                }
            }
        }
    }
}
