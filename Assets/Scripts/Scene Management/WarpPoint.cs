using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpPoint : MonoBehaviour
{
    private void Awake()
    {
        Debug.Log("cum");
        Game.Instance.SceneManager.OnSceneChanged += SpawnPlayerOnPoint;
    }

    private void SpawnPlayerOnPoint(GameObject player)
    {
        player.GetComponent<Rigidbody2D>().MovePosition(transform.position);
    }
}
