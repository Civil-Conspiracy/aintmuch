using UnityEngine;
using TMPro;

public class FloorItem : MonoBehaviour
{
    Item thisItem;
    SpriteRenderer sr;
    TextMeshPro thisName;

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
}
