using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader: MonoBehaviour
{

    [SerializeField] SceneField _lightDimensionScene;
    [SerializeField] SceneField _darkDimensionScene;
    [SerializeField] float _sceneActivationDelay = 1.9f;

    public static SceneLoader Instance { get; private set; }

    public event Action<Dimension> OnStartDimensionLoad;
    public event Action<Dimension> OnDimensionReadyToActivate;
    public event Action<Dimension> OnDimensionLoaded;

    #region Setup
    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }
    #endregion

    public void LoadDimensionScene(Dimension dimensionToLoad, bool useSceneLoadDelay = true)
    {
        switch (dimensionToLoad)
        {
            case Dimension.Light:
                if (SceneIsLoaded(_lightDimensionScene)) return;
                OnStartDimensionLoad?.Invoke(Dimension.Light);
                LoadSceneAdditive(_lightDimensionScene, _darkDimensionScene, useSceneLoadDelay);
                break;

            case Dimension.Dark:
                if (SceneIsLoaded(_darkDimensionScene)) return;
                OnStartDimensionLoad?.Invoke(Dimension.Dark);
                LoadSceneAdditive(_darkDimensionScene, _lightDimensionScene, useSceneLoadDelay);
                break;

            default:
                // Error here
                break;
        }
    }

    private void LoadSceneAdditive(SceneField sceneToLoad, SceneField sceneToUnload, bool useSceneLoadDelay = true)
    {
       OnStartDimensionLoad?.Invoke(GetDimensionFromScene(sceneToLoad));
       StartCoroutine(LoadSceneAdditiveCoroutine(sceneToLoad, sceneToUnload));

    }

    private IEnumerator LoadSceneAdditiveCoroutine(SceneField sceneToLoad, SceneField sceneToUnload, bool useSceneLoadDelay = true)
    {
        AsyncOperation sceneLoadProgress = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);

        if(useSceneLoadDelay == true) sceneLoadProgress.allowSceneActivation = false;

        while(sceneLoadProgress.progress < 0.9f)
        {
            yield return null;
        }

        OnDimensionReadyToActivate?.Invoke(GetDimensionFromScene(sceneToLoad));

        if (useSceneLoadDelay == true) yield return new WaitForSeconds(_sceneActivationDelay);

        sceneLoadProgress.allowSceneActivation = true;
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