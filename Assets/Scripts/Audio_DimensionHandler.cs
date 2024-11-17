using System;
using System.Collections;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Audio_DimensionHandler : MonoBehaviour
{
    [SerializeField] private AudioSource _darkDimensionBreathSource, _lightDimensionBreathSource;
    [Space(15)]
    [SerializeField] private AudioSource _playerBreathAudioSource;
    [SerializeField] private float _crossfadeDuration = 0.5f;

    private float _darkDimensionAudioVolume, _lightDimensionAudioVolume, _playerBreathVolume;
    private void Start()
    {
        SceneLoader.Instance.OnStartDimensionLoad += OnDimensionLoadStarted;
        SceneLoader.Instance.OnDimensionLoaded += OnDimensionLoaded;
        _darkDimensionAudioVolume = 1f;
        _lightDimensionAudioVolume  = _lightDimensionBreathSource.volume;
        _playerBreathVolume = _playerBreathAudioSource.volume;
        _playerBreathAudioSource.loop = true;

        // Set initial volumes based on starting dimension (should be LIGHT dimension)
        OnDimensionLoaded(DimensionManager.Instance.CurrentDimension);
    }

    private void OnDimensionLoadStarted(Dimension dimension)
    {
        PlayDimensionShiftAudio();
    }
    private void OnDimensionLoaded(Dimension dimension)
    {
        StopAllCoroutines();

        if (dimension == Dimension.Dark)
        {
            StartCoroutine(CrossFadeToTargetVolume(_lightDimensionBreathSource, _darkDimensionBreathSource, _darkDimensionAudioVolume));
            StartCoroutine(FadeSourceAudioToTargetVolume(_playerBreathAudioSource, _playerBreathVolume));
        }
        else if (dimension == Dimension.Light)
        {
            StartCoroutine(CrossFadeToTargetVolume(_darkDimensionBreathSource, _lightDimensionBreathSource, _lightDimensionAudioVolume));
            StartCoroutine(FadeSourceAudioToTargetVolume(_playerBreathAudioSource, 0f));
        }
    }

    private IEnumerator CrossFadeToTargetVolume(AudioSource sourceToSilence, AudioSource sourceTofadeUp, float targetVolume)
    {
        float elapsedTime = 0;
        float percentComplete;
        float silencingStartVolume = sourceToSilence.volume;

        while(elapsedTime < _crossfadeDuration)
        {
            elapsedTime += Time.deltaTime;
            percentComplete = elapsedTime / _crossfadeDuration;

            sourceToSilence.volume = Mathf.Lerp(silencingStartVolume, 0, percentComplete);
            sourceTofadeUp.volume = Mathf.Lerp(0, targetVolume, percentComplete);

            yield return null;
        }

        sourceToSilence.volume = 0;
        sourceTofadeUp.volume = targetVolume;
    }

    private IEnumerator FadeSourceAudioToTargetVolume(AudioSource audioSource, float targetVolume)
    {
        float elapsedTime = 0;
        float percentComplete;

        while (elapsedTime < _crossfadeDuration)
        {
            elapsedTime += Time.deltaTime;
            percentComplete = elapsedTime / _crossfadeDuration;
            audioSource.volume = Mathf.Lerp(0, targetVolume, percentComplete);

            yield return null;
        }

        audioSource.volume = targetVolume;
    }

    private void PlayDimensionShiftAudio()
    {
       
    }

    private void OnDestroy()
    {
        SceneLoader.Instance.OnStartDimensionLoad -= OnDimensionLoadStarted;
        SceneLoader.Instance.OnDimensionLoaded -= OnDimensionLoaded;
    }
}
