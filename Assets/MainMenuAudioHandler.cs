
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class MainMenuAudioHandler : MonoBehaviour

{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] float _fadeOutDuration = 3f;
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);
    }

    public void HandleGameStartAudio()
    {
        StartCoroutine(FadeOutAudio());
    }

    private IEnumerator FadeOutAudio()
    {
        float elapsedTime = 0f;
        float percentComplete;
        float startVolume = _audioSource.volume;
        while(elapsedTime < _fadeOutDuration)
        {
            elapsedTime += Time.deltaTime;
            percentComplete = elapsedTime / _fadeOutDuration;

            _audioSource.volume = Mathf.Lerp(startVolume, 0f, percentComplete);
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }


}
