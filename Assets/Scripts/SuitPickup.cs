using UnityEngine;
using UnityEngine.Events;

public class SuitPickup : MonoBehaviour
{
    Collider trigger;
    public UnityEvent _tookSuit;
    void Start()
    {
       trigger = gameObject.GetComponent<Collider>(); 
    }

    private void OnTriggerEnter(Collider other) {
        if (other.GetComponentInParent<CharacterController>() != null)
        {
            _tookSuit.Invoke();
            gameObject.SetActive(false);
        }
    }
}
