using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class SuitLocker : MonoBehaviour, Iinteractable
{
    [SerializeField] private float openAmount;
    private Vector3 closedPos;
    [SerializeField] private AnimationCurve curve;
    [SerializeField] private float transitionDuration;
    public bool opening = false;
    public bool used = false;
    float openYPos;
    float lerp = 0;
    Collider glassCollider;

    public void Awake() {
        curve = curve.length == 0 ? null : curve;
        closedPos = transform.position;
        openYPos = closedPos.y - openAmount;
        glassCollider = GetComponent<Collider>();
    }

// jos on aikaa, tehd‰‰n smoothimpi avautuminen

    void Iinteractable.Interact(Transform playerTransform) {
        if(!used)
        {
            glassCollider.enabled = false;
            transform.position = new Vector3(closedPos.x, openYPos, closedPos.z);
            used = true;
        }
        
       
    }
}
