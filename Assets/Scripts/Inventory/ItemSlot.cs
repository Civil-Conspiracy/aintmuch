using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] Image ItemIcon;
    [SerializeField] Text StackCountText;
    private int _currentStackCount;
    private Item _item;
    public Item Item
    {
        get { return _item; }
        set
        {
           _item = value;
            if (_item == null)
                ItemIcon.enabled = false;
            else
            {
                ItemIcon.sprite = _item.ItemIcon;
                ItemIcon.enabled = true;
            }
        }
    }
    public int CurrentStackCount
    {
        get { return _currentStackCount; }
        set
        {
            _currentStackCount = value;
            if (_currentStackCount < 1)
                StackCountText.enabled = false;
            else
            {
                StackCountText.text = _currentStackCount.ToString();
                StackCountText.enabled = true;
            }
        }
    }


    private void OnValidate()
    {
        if (ItemIcon == null)
        {
            ItemIcon = GetComponent<Image>();
        }
        if (StackCountText == null)
        {
            StackCountText = GetComponentInChildren<Text>();
        }
    }
}
