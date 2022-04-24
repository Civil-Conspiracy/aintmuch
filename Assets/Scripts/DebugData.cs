using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player/Debug/DebugData", fileName = "DebugData")]
public class DebugData : ScriptableObject
{
    protected PlayerData data;
    public DebugData(PlayerData _data)
    {
        data = _data;
    }

    [Header("Inventory Debug")]
    public Item DEBUGITEM;
    public GameObject go_defaultItem;
}
