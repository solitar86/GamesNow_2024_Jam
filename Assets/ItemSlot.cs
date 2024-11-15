
using UnityEngine;
using UnityEngine.Events;

public class ItemSlot : MonoBehaviour, Iinteractable
{
    [SerializeField] ItemDataSO _keyItem;
    [SerializeField] Transform _keyItemPosition;

    public UnityEvent OnKeyItemPlaced;
    public UnityEvent OnIncorrectItemAttempted;
    void Iinteractable.Interact(Transform playerTransform)
    {
       var playerHand = playerTransform.GetComponent<PlayerItemPickUpHandler>();
        if (playerHand.CurrenItemInHand.GetItemDataSO() == _keyItem)
        {
            // Player inserted keyitem;
            playerHand.CurrenItemInHand.FreezeItem();
            playerHand.PlaceItemInKeyItemPosition(_keyItemPosition, OnItemPlacedCallBack);
        }
        else if(playerHand.CurrenItemInHand != _keyItem)
        {
            Debug.Log("Incorrect Item");
            OnIncorrectItemAttempted?.Invoke();
        }
    }

    public void OnItemPlacedCallBack()
    {
        OnKeyItemPlaced?.Invoke();
        Debug.Log("Item Placed CallBack Called");

    }


}
