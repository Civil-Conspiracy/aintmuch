using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneBorder : MonoBehaviour
{
    [SerializeField] Scenes toScene;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Game.Instance.SceneManager.ChangeScene((int)toScene);
        }
    }
}
