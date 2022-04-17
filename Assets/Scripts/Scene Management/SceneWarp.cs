using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityScenes = UnityEngine.SceneManagement;

public class SceneWarp : MonoBehaviour, IInteractable
{
    [SerializeField] Scenes m_ToScene;
    [SerializeField] bool m_RequireActionKey;

    public void Interact(GameObject playerContext)
    {
        if (m_RequireActionKey)
        {
            StartCoroutine(Game.Instance.SceneManager.LoadingScreenCoroutine((int)m_ToScene, () =>
            {
                WarpPlayer(playerContext);
            }));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!m_RequireActionKey)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                StartCoroutine(Game.Instance.SceneManager.LoadingScreenCoroutine((int)m_ToScene, () =>
                {
                    WarpPlayer(collision.gameObject);
                }));
            }
        }
    }

    private void WarpPlayer(GameObject player)
    {
        GameObject[] roots = UnityScenes.SceneManager.GetSceneByBuildIndex((int)m_ToScene).GetRootGameObjects();
        foreach (GameObject go in roots)
        {
            if (go.GetComponent<WarpPoint>() != null)
            {
                go.GetComponent<WarpPoint>().SpawnPlayerOnPoint(player);
            }
        }
    }
}
