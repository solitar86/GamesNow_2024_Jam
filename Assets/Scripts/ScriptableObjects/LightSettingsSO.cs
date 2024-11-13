
using System;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "NewLightSettingsSO", menuName = "ScriptableObjects/LightSettingsSO")]
public class LightSettingsSO: ScriptableObject
{
    public event Action OnLightSettingsUpdated;

    [Header("Render Settings")]
    public Material skybox;
    public AmbientMode ambientMode = AmbientMode.Flat;
    [ColorUsage(true, true)] public Color ambientSkyColor = Color.white;
    //[ColorUsage(true, true)] public Color ambientEquatorColor = Color.gray;
    //[ColorUsage(true, true)] public Color ambientGroundColor = Color.black; 
    [Range(0f,1f)] public float ambientIntensity = 1.0f;
    [Space(15)]
    [Header("Fog Settings")]
    public bool fog;
    public FogMode fogMode;
    [ColorUsage(true, true)] public Color fogColor;
    [Range(0f,1f)] public float fogDensity;

    //[ColorUsage(true, true)] public Color ambientLight = Color.gray;

    //[Header("Sun light Settings")]
    //[ColorUsage(true, true)] public Color lightColor;
    //public float lightIntensity = 1.0f;
    //[Range(0f,1f)] public float lightShadowStrength = 1.0f;
    //public Quaternion lightRotation;
    //[ColorUsage(true, true)] public Color realTimeShadowColor;

    private void OnValidate()
    {
        OnLightSettingsUpdated?.Invoke();
    }
}
