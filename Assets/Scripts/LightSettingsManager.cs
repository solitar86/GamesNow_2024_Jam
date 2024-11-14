
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Overrides lighting settings on sceneload
/// </summary>
public class LightSettingsManager : MonoBehaviour
{
    [SerializeField] LightSettingsSO _darkDimensionLightSettings;
    [SerializeField] LightSettingsSO _lightimensionLightSettings;
    //[SerializeField] Light _directionalLight;

    private LightSettingsSO _currentSettings;
    private void Awake()
    {
        _currentSettings = _lightimensionLightSettings;
        _currentSettings.OnLightSettingsUpdated += OnLightSettingsUpdated;
        SceneManager.sceneLoaded += OnSceneLoaded;
        //_lightSettingsSO.LightSettingsUpdated += OnLightSettingsUpdated;
        ApplyLightSettings(_currentSettings);
    }

    private void OnLightSettingsUpdated()
    {
        ApplyLightSettings(_currentSettings);
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        _currentSettings.OnLightSettingsUpdated -= OnLightSettingsUpdated;
        if (DimensionManager.Instance.CurrentDimension == Dimension.Light) _currentSettings = _lightimensionLightSettings;
        if (DimensionManager.Instance.CurrentDimension == Dimension.Dark) _currentSettings = _darkDimensionLightSettings;
        _currentSettings.OnLightSettingsUpdated += OnLightSettingsUpdated;
        ApplyLightSettings(_currentSettings);
    }

    private void ApplyLightSettings(LightSettingsSO lightSettingsSO)
    {
        RenderSettings.skybox = lightSettingsSO.skybox;
        RenderSettings.ambientMode = lightSettingsSO.ambientMode;
        RenderSettings.ambientSkyColor = lightSettingsSO.ambientSkyColor;
        //RenderSettings.ambientEquatorColor = lightSettingsSO.ambientEquatorColor;
        //RenderSettings.ambientGroundColor = lightSettingsSO.ambientGroundColor;
        RenderSettings.ambientIntensity = lightSettingsSO.ambientIntensity;
        //RenderSettings.ambientLight = lightSettingsSO.ambientLight;
        //RenderSettings.subtractiveShadowColor = lightSettingsSO.realTimeShadowColor;
        RenderSettings.fog = lightSettingsSO.fog;
        RenderSettings.fogMode = lightSettingsSO.fogMode;
        RenderSettings.fogColor = lightSettingsSO.fogColor;
        RenderSettings.fogDensity = lightSettingsSO.fogDensity;

        //_directionalLight.color = lightSettingsSO.lightColor;
        //_directionalLight.intensity = lightSettingsSO.lightIntensity;
        //_directionalLight.shadowStrength = lightSettingsSO.lightShadowStrength;
        //_directionalLight.transform.rotation = lightSettingsSO.lightRotation;
    }

    private void OnDestroy()
    {
        _currentSettings.OnLightSettingsUpdated -= OnLightSettingsUpdated;
    }
}
