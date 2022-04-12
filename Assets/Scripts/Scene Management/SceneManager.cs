using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityScenes = UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    public void ChangeScene(int sceneIndex)
    {
        UnityScenes.SceneManager.LoadScene(sceneIndex, UnityScenes.LoadSceneMode.Single);
    }
}

public enum Scenes
{
    HUB,
    FOREST,
    VILLAGE,
    HUBINSIDE
}
