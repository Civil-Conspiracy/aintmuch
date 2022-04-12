using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    [SerializeField] GameObject obj;

    private void Awake()
    {
        Instantiate(obj, transform.position, Quaternion.identity);
        Instantiate(obj, new Vector2(transform.position.x, transform.position.y + 11f), Quaternion.identity);
        Instantiate(obj, new Vector2(transform.position.x, transform.position.y + 22f), Quaternion.identity);
    }
}
