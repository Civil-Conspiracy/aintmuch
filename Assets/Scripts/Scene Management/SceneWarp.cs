using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneWarp : MonoBehaviour, IInteractable
{
    [SerializeField] Scenes m_ToScene;
    [SerializeField] bool m_RequireActionKey;

    public void Interact(GameObject playerContext)
    {
        if (m_RequireActionKey)
        {
            Game.Instance.SceneManager.ChangeScene((int)m_ToScene, playerContext);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!m_RequireActionKey)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Game.Instance.SceneManager.ChangeScene((int)m_ToScene, collision.gameObject);
            }
        }
    }
}
