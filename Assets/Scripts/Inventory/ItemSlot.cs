using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] Image ItemIcon;
    [SerializeField] TextMeshProUGUI StackCountText;
    private int _currentStackCount;
    private int _slotID;
    [SerializeField]private Item _item;

    public Item Item
    {
        get { return _item; }
        set
        {
           _item = value;
            if (_item == null)
            {
                ItemIcon.enabled = false;
                CurrentStackCount = 0;
            }
            else
            {
                ItemIcon.sprite = _item.ItemIcon;
                ItemIcon.enabled = true;
                CurrentStackCount = _item.CurrentStackSize;
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

    public int SlotID
    {
        get { return _slotID; }

        set
        {
            _slotID = value;
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
            StackCountText = GetComponentInChildren<TextMeshProUGUI>();
        }
    }
}
