using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempScriptRespawnPlayer : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            collision.transform.position = _spawnPoint.position;
    }
}
