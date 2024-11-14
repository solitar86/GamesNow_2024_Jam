
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_DimensionShiftCanvasHandler : MonoBehaviour
{
    [SerializeField] private CanvasGroup _transitionCanvas;
    [SerializeField] private Image _transitionImage;
    [SerializeField] private float _transitionDuration;
    [SerializeField] private AnimationCurve _curve;
    [SerializeField] private LightSettingsSO _lightDimensionLightSettings, _darkDimensionLightSettings;

    #region Setup
    private void Awake()
    {
        if (_transitionCanvas == null) _transitionCanvas = GetComponentInChildren<CanvasGroup>();
        if(_transitionImage == null) _transitionImage = GetComponentInChildren<Image>();
       _transitionCanvas.alpha = 0f;
    }

    private void Start()
    {
        SceneLoader.Instance.OnDimensionReadyToActivate += OnStartDimensionLoad_StartDimensionSwitchAnimation;
        SceneLoader.Instance.OnDimensionLoaded += OnDimensionLoaded_FadeOutTransitionCanvas;
    }

    #endregion

    private void OnStartDimensionLoad_StartDimensionSwitchAnimation(Dimension dimension)
    {

        LightSettingsSO _currentSettings = dimension == Dimension.Light ? _lightDimensionLightSettings : _darkDimensionLightSettings;
        _transitionImage.color = _currentSettings.fogColor;
    }

    private void OnDimensionLoaded_FadeOutTransitionCanvas(Dimension dimension)
    {
        StopAllCoroutines();
        StartCoroutine(HandleTransitionCanvasAnimation(dimension));
    }

    private IEnumerator HandleTransitionCanvasAnimation(Dimension _dimensionToSwitchTo)
    {
        float elapsedTime = 0;
        float percentComplete;
        _transitionCanvas.alpha = 1f;

        while (elapsedTime < _transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            percentComplete = elapsedTime / _transitionDuration;
            _transitionCanvas.alpha = Mathf.Lerp(1, 0, _curve.Evaluate(percentComplete));
            yield return null;
        }

        _transitionCanvas.alpha = 0;
    }

    private void OnDestroy()
    {
        SceneLoader.Instance.OnStartDimensionLoad -= OnStartDimensionLoad_StartDimensionSwitchAnimation;
        SceneLoader.Instance.OnDimensionLoaded -= OnDimensionLoaded_FadeOutTransitionCanvas;
    }
}
