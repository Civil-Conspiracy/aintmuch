using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityScenes = UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    public SceneWrapper CurrentScene;

    private void OpenLoadingAndTarget(int targetIndex)
    {
        UnityScenes.SceneManager.LoadScene("Loading", UnityScenes.LoadSceneMode.Additive);
        UnityScenes.SceneManager.LoadScene(targetIndex, UnityScenes.LoadSceneMode.Additive);
    }

    private void CloseLoadingAndOriginal(int original)
    {
        UnityScenes.SceneManager.UnloadSceneAsync("Loading");
        UnityScenes.SceneManager.UnloadSceneAsync(original);
    }

    public IEnumerator LoadingScreenCoroutine(int targetScene, Action loadingAction)
    {
        int original = UnityScenes.SceneManager.GetActiveScene().buildIndex;

        OpenLoadingAndTarget(targetScene);

        yield return new WaitUntil(() => UnityScenes.SceneManager.GetSceneByBuildIndex((int)Scenes.LOADING).isLoaded);
        UnityScenes.SceneManager.SetActiveScene(UnityScenes.SceneManager.GetSceneByBuildIndex((int)Scenes.LOADING));

        yield return new WaitUntil(() => UnityScenes.SceneManager.GetSceneByBuildIndex(targetScene).isLoaded);

        loadingAction();

        CloseLoadingAndOriginal(original);
    }


}

public enum Scenes
{
    HUB,
    FOREST,
    VILLAGE,
    HUBINSIDE,
    LOADING
}
