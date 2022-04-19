using UnityEngine;
using TMPro;

public class FloorItem : MonoBehaviour
{
    Item thisItem;
    SpriteRenderer sr;
    TextMeshPro thisName;
    [SerializeField] LayerMask _playerMask;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        thisName = transform.Find("Name").GetComponent<TextMeshPro>();
    }

    public void SetInfo(Item item)
    {
        thisItem = item;
        sr.sprite = thisItem.ItemIcon;
        thisName.text = thisItem.ItemName;
    }

    private void Update()
    {
        if (Physics2D.OverlapBox(transform.position, Vector2.one, 0, _playerMask))
            PlayerInventoryManager.instance.GetComponent<PlayerInventoryManager>().AddItem(thisItem, this);
    }
}
