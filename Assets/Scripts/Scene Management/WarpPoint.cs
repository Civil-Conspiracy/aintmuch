using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpPoint : MonoBehaviour
{
    public void SpawnPlayerOnPoint(GameObject player)
    {
        player.transform.position = gameObject.transform.position;
    }
}
