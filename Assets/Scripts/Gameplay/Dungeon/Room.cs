using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private RoomPattern m_enter_pattern;
    [SerializeField] private RoomPattern m_exit_pattern;
    [SerializeField] private GameObject m_map;

    public RoomPattern Enter
    {
        get => m_enter_pattern;
    }

    public RoomPattern Exit
    {
        get => m_exit_pattern;
    }

    private void Awake()
    {
        Instantiate(m_map, transform);
    }
}


public enum RoomPattern
{
    PATTERN1,
    PATTERN2,
    PATTERN3,
    PATTERN4,
    PATTERN5,
    PATTERN6,
    START
}