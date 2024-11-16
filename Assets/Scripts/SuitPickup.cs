using UnityEngine;
using UnityEngine.Events;

public class SuitPickup : MonoBehaviour
{
    Collider trigger;

    void Start()
    {
       trigger = gameObject.GetComponent<Collider>(); 
    }

    private void OnTriggerEnter(Collider other) {
        if (other.GetComponentInParent<CharacterController>() != null)
        {
            other.GetComponentInChildren<Player_DimensionSwitcher>().EquippedSuit();
            other.GetComponentInParent<UI_PlayerInGameUI>().EnableHUD();
            gameObject.SetActive(false);
        }
    }
}
