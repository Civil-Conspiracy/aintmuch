using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityScenes = UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    public delegate void SceneManagerDelegate(GameObject go);

    public SceneManagerDelegate OnSceneChanged;

    public SceneWrapper CurrentScene;

    public void ChangeScene(int sceneIndex, GameObject player)
    {
        UnityScenes.SceneManager.LoadScene(sceneIndex, UnityScenes.LoadSceneMode.Single);
        
        var roots = UnityScenes.SceneManager.GetActiveScene().GetRootGameObjects();
        
        foreach (var root in roots)
        {
            if (root.GetComponent<SceneWrapper>() != null)
            {
                root.GetComponent<SceneWrapper>().Init();
                CurrentScene.DeInit();
                CurrentScene = root.GetComponent<SceneWrapper>();
            }
        }
        
        OnSceneChanged(player);
    }
}

public enum Scenes
{
    HUB,
    FOREST,
    VILLAGE,
    HUBINSIDE
}
