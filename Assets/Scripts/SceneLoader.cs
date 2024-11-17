using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader: MonoBehaviour
{

    [SerializeField] SceneField _lightDimensionScene;
    [SerializeField] SceneField _darkDimensionScene;
    [SerializeField] float _sceneActivationDelay = 2.5f;

    public static SceneLoader Instance { get; private set; }

    public event Action<Dimension> OnStartDimensionLoad;
    public event Action<Dimension> OnDimensionReadyToActivate;
    public event Action<Dimension> OnDimensionLoaded;
    public event Action<Dimension> OnSceneIsActivated;

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
        if(SceneIsLoaded(sceneToUnload)) UnloadScene(sceneToUnload);
        OnDimensionLoaded?.Invoke(GetDimensionFromScene(sceneToLoad));

        yield return new WaitForSeconds(0.1f);

        OnSceneIsActivated?.Invoke(GetDimensionFromScene(sceneToLoad));

    }

    public Dimension GetDimensionFromScene(SceneField sceneField)
    {
        if(sceneField == _lightDimensionScene) return Dimension.Light;
        if(sceneField == _darkDimensionScene) return Dimension.Dark;

        Debug.Log("<color=#FF0000> Invalid Dimension From Scene. Returning Dimension.Light </color>");
        return Dimension.Light;
    }

    public Scene GetSceneFromDimension(Dimension dimensionToCheck)
    {
        if (dimensionToCheck == Dimension.Light) return SceneManager.GetSceneByName(_lightDimensionScene.SceneName);
        if(dimensionToCheck == Dimension.Dark) return SceneManager.GetSceneByName(_darkDimensionScene.SceneName);

        Debug.Log("Something went wrong with dimension loading");
        return SceneManager.GetSceneByName(_lightDimensionScene.SceneName);
    }

    public Scene GetCurrentDimensionScene()
    {
        if(DimensionManager.Instance.CurrentDimension == Dimension.Light)
        {
            return SceneManager.GetSceneByName(_lightDimensionScene);
        }

        return SceneManager.GetSceneByName(_darkDimensionScene);
    }

    private void UnloadScene(SceneField sceneToUnload)
    {
        SceneManager.UnloadSceneAsync(sceneToUnload);
    }

    private bool SceneIsLoaded(SceneField sceneToCheck)
    {
        return SceneManager.GetSceneByName(sceneToCheck.SceneName).isLoaded;
    }

    public bool IsDimensionSceneLoaded(Dimension dimensionToCheck)
    {
       Debug.Log(dimensionToCheck.ToString() + " is already loaded");
        var scene = GetSceneFromDimension(dimensionToCheck);
        return scene.isLoaded;

    }

    public void LoadLightDimensionInstant()
    {
        SceneManager.LoadScene(_lightDimensionScene, LoadSceneMode.Additive);
    }
}