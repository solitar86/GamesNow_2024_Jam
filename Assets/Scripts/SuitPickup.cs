using UnityEngine;
using UnityEngine.Events;

public class SuitPickup : MonoBehaviour
{
    Collider trigger;
    UI_PlayerInGameUI playerUI;
    Player_DimensionSwitcher dimensionShifter;
    [SerializeField] Sound _suitPickUpSound;

    private void Awake()
    {
        // Player has already collected suit;
        if(PersistantObjects.GameState.HasSuit == true)
        {
            gameObject.SetActive(false);
        }

    }
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
            PersistantObjects.GameState.SetPlayerHasSuit(true);
            AudioManager.PlaySoundAtPoint(this, _suitPickUpSound, transform.position);
            gameObject.SetActive(false);
        }
    }
}
