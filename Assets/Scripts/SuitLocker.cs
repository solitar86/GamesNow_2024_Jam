using System.Collections;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class SuitLocker : MonoBehaviour, Iinteractable
{
    [SerializeField] private float openAmount;
    private Vector3 closedPos;
    private Vector3 openPos;
    //[SerializeField] private AnimationCurve curve;
    [SerializeField] private float transitionDuration = 2f;
    float openYPos;
    Collider glassCollider;
    AudioSource _openAudioSource;

    public void Awake() {

        closedPos = transform.position;
        openYPos = closedPos.y - openAmount;
        openPos = new Vector3(closedPos.x, openYPos, closedPos.z);
        glassCollider = GetComponent<Collider>();
        _openAudioSource = GetComponent<AudioSource>();

        if(PersistantObjects.GameState.HasSuit == true)
        {
            transform.position = closedPos;
            glassCollider.enabled = false;
            return;
        }
    }

    void Iinteractable.Interact(Transform playerTransform)
    {

        if(PersistantObjects.GameState.HasSuit == false)
        {
            StartCoroutine(GlassOpenSequence());
        }
        else
        {
            AudioManager.PlayErrorBeep(transform);
        }
    }

    private IEnumerator GlassOpenSequence()
    {

        float elapsedTime = 0f;
        Vector3 startPosition = closedPos;
        Vector3 targetPosition = openPos;
        float percentComplete;
        float lerpValue;
        _openAudioSource.Play();

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            percentComplete = elapsedTime / transitionDuration;
            lerpValue = PersistantObjects.Instance.EvaluateSlowEndCurve(percentComplete);
            transform.position = Vector3.Lerp(startPosition, targetPosition, lerpValue);
            yield return null;
        }

        transform.position = targetPosition;
        glassCollider.enabled = false;
    }
}
