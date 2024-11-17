using UnityEngine;
using UnityEngine.EventSystems;
[RequireComponent(typeof(AudioSource))]
public class ButtonAudioHandler : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private AudioSource _audioSource;
    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        _audioSource.pitch = 1f + Random.Range(-0.1f, 0.1f);
        _audioSource.Play();
    }
}
