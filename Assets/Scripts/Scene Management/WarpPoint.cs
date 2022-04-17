using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpPoint : MonoBehaviour
{
    public void SpawnPlayerOnPoint(GameObject player)
    {
        player.GetComponent<Rigidbody2D>().MovePosition(transform.position);
    }
}
