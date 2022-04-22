using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    [SerializeField] GameObject[] m_Rooms;
    //[SerializeField] GameObject m_StaticRoom;

    [SerializeField] private int m_LevelsPerFloor;

    GameObject[] m_CurrentFloorOrder;

    readonly float y_increase = 9.8f;
    int m_lastSpawnedRoomIndex = 0;

    private void Awake()
    {
        m_CurrentFloorOrder = new GameObject[m_LevelsPerFloor];
        DungeonInit();
    }

    private void DungeonInit()
    {
        m_CurrentFloorOrder = GenerateFloorOrder();

        SpawnFloor();
    }

    private GameObject[] GenerateFloorOrder()
    {
        GameObject[] floor = new GameObject[m_CurrentFloorOrder.Length];

        for (int i = 0; i < m_CurrentFloorOrder.Length; i++)
        {
            if (i == 0)
            {
                floor[i] = GetRandomRoom(GetRooms(RoomPattern.PATTERN3));
            }
            else
            {
                floor[i] = GetRandomRoom(GetRooms(floor[i - 1].GetComponent<Room>().Exit));
            }
        }

        return floor;
    }

    private List<GameObject> GetRooms(RoomPattern pattern)
    {
        var rooms = new List<GameObject>();

        foreach(var room in m_Rooms)
        {
            if (room.GetComponent<Room>().Enter == pattern)
            {
                rooms.Add(room);
            }
        }

        return rooms;
    }

    private GameObject GetRandomRoom(List<GameObject> rooms)
    {
        var r = rooms.ToArray();
        return r.Length == 0 ? null : r[Random.Range(0, r.Length)];
    }

    private void SpawnNextRoom()
    {
        SpawnRoom(m_lastSpawnedRoomIndex);
        m_lastSpawnedRoomIndex++;
    }

    private void SpawnFloor()
    {
        for (int i = 0; i < m_CurrentFloorOrder.Length; i++)
        {
            SpawnNextRoom();
        }
    }

    private GameObject SpawnRoom(int roomIndex)
    {
        return Instantiate(m_CurrentFloorOrder[roomIndex], new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + ((m_lastSpawnedRoomIndex + 1) * y_increase)), Quaternion.identity, gameObject.transform);
    }
}
