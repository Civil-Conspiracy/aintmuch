using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private RoomPattern enter_pattern;
    [SerializeField] private RoomPattern exit_pattern;
    [SerializeField] private GameObject map;

    private void Awake()
    {
        Instantiate(map, transform);
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