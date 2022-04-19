using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempScriptRespawnPlayer : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private GameObject _player;

    private void Update()
    {
        if (_player != null)
        {
            if (_player.transform.position.y <= -20f)
            {
                _player.transform.position = _spawnPoint.position;
            }
        }
    }
}
