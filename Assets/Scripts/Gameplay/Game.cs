using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    #region Singleton
    private static Game instance;

    public static Game Instance
    {
        get => instance;
        set
        {
            if (instance == null)
            {
                instance = value;
            }
            else if (instance != value)
            {
                Debug.Log($"{nameof(Game)} already exists. Deleting now...");
                Destroy(value);
            }
        }
    }
    #endregion

    SceneManager m_sceneManager;
    //public GameObject player;

    public SceneManager SceneManager
    {
        get => m_sceneManager;
    }

    private void Awake()
    {
        instance = this;
        m_sceneManager = GetComponent<SceneManager>();
        DontDestroyOnLoad(gameObject);
    }
}
