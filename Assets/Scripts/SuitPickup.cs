using UnityEngine;
using UnityEngine.Events;

public class SuitPickup : MonoBehaviour
{
    Collider trigger;
    UI_PlayerInGameUI playerUI;
    Player_DimensionSwitcher dimensionShifter;

    void Start()
    {
       trigger = gameObject.GetComponent<Collider>();
       playerUI = FindAnyObjectByType<UI_PlayerInGameUI>();
       dimensionShifter = FindAnyObjectByType<Player_DimensionSwitcher>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.GetComponentInParent<CharacterController>() != null)
        {
            playerUI.EnableHUD();
            dimensionShifter.EquippedSuit();
            gameObject.SetActive(false);
        }
    }
}
