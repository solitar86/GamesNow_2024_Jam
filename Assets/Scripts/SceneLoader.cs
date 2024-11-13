using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader: MonoBehaviour
{

    [SerializeField] SceneField _lightDimensionScene;
    [SerializeField] SceneField _darkDimensionScene;

    public static SceneLoader Instance { get; private set; }

    public event Action<Dimension> OnDimensionLoaded;
    public event Action<Dimension> OnStartDimensionLoad;

    #region Setup
    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }
    #endregion

    public void LoadDimensionScene(Dimension dimensionToLoad)
    {
        switch (dimensionToLoad)
        {
            case Dimension.Light:
                if (SceneIsLoaded(_lightDimensionScene)) return;
                OnStartDimensionLoad?.Invoke(Dimension.Light);
                LoadSceneAdditive(_lightDimensionScene, _darkDimensionScene);
                break;

            case Dimension.Dark:
                if (SceneIsLoaded(_darkDimensionScene)) return;
                OnStartDimensionLoad?.Invoke(Dimension.Dark);
                LoadSceneAdditive(_darkDimensionScene, _lightDimensionScene);
                break;

            default:
                // Error here
                break;
        }
    }

    private void LoadSceneAdditive(SceneField sceneToLoad, SceneField sceneToUnload)
    {
       OnStartDimensionLoad?.Invoke(GetDimensionFromScene(sceneToLoad));
       StartCoroutine(LoadSceneAdditiveCoroutine(sceneToLoad, sceneToUnload));

    }

    private IEnumerator LoadSceneAdditiveCoroutine(SceneField sceneToLoad, SceneField sceneToUnload)
    {
        AsyncOperation sceneLoadProgress = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);

        while(sceneLoadProgress.isDone == false)
        {
            yield return null;
        }
        OnDimensionLoaded?.Invoke(GetDimensionFromScene(sceneToLoad));

        if(SceneIsLoaded(sceneToUnload)) UnloadScene(sceneToUnload);
    }

    private Dimension GetDimensionFromScene(SceneField scene)
    {
        if(scene == _lightDimensionScene) return Dimension.Light;
        if(scene == _darkDimensionScene) return Dimension.Dark;

        Debug.Log("<color=#FF0000> Invalid Dimension From Scene. Returning Dimension.Light </color>");
        return Dimension.Light;
    }

    private void UnloadScene(SceneField sceneToUnload)
    {
        SceneManager.UnloadSceneAsync(sceneToUnload);
    }

    private bool SceneIsLoaded(SceneField sceneToCheck)
    {
        return SceneManager.GetSceneByName(sceneToCheck.SceneName).isLoaded;
    }
}