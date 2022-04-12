using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    [SerializeField] GameObject[] m_Rooms;
    [SerializeField] GameObject m_StaticRoom;

    GameObject[] m_randomizedRooms;

    readonly float y_increase = 11f;
    int m_lastSpawnedRoomIndex = 1;

    private void Awake()
    {
        DungeonInit();
        SpawnRoom(m_StaticRoom);
        SpawnNextRoom();
    }

    private void DungeonInit()
    {
        m_randomizedRooms = m_Rooms;
        m_randomizedRooms.Shuffle();
    }

    private void SpawnNextRoom()
    {
        SpawnRoom(m_lastSpawnedRoomIndex++);
    }

    private GameObject SpawnRoom(GameObject room)
    {
        return Instantiate(room, gameObject.transform);
    }

    private GameObject SpawnRoom(int roomIndex)
    {
        return Instantiate(m_randomizedRooms[roomIndex], new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + (m_lastSpawnedRoomIndex * y_increase)), Quaternion.identity, gameObject.transform);
    }
}
