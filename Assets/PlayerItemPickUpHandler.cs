using System;
using System.Globalization;
using UnityEngine;

public class PlayerItemPickUpHandler : MonoBehaviour
{
    [SerializeField] private KeyCode _itemPickUpKey;
    [SerializeField] private PlayerInteractor _interactor;

    private Item _currentItemInHand;
    public Item CurrenItemInHand { get { return _currentItemInHand; } }
    private void Start()
    {
        _interactor = GetComponent<PlayerInteractor>();
    }
    private void Update()
    {
        if(Input.GetKeyDown(_itemPickUpKey))
        {
            if (_currentItemInHand != null && _interactor.CanPlaceItem() == true)
            {
                // Place Item Down
                PersistantObjects.Instance.SwitchObjectToActiveDimensionScene(_currentItemInHand);
                _currentItemInHand.PlaceDownAndUnParent(_interactor);
                _currentItemInHand.GetItemDataSO().SetIsHeldByPlayer(false);
                _currentItemInHand = null;
            }
            if(_interactor.SelectedItem != null)
            {
                // Pick Up Item
                PersistantObjects.Instance.SwitchObjectToBootStrapScene(_interactor.SelectedItem);
                _interactor.SelectedItem.PickUpAndParentToPlayer(transform);
                _currentItemInHand = _interactor.SelectedItem;
                _currentItemInHand.GetItemDataSO().SetIsHeldByPlayer(true);
                _interactor.DeselectItem(_currentItemInHand); // I don't remember why is this important
            }

            if(_interactor.Interactable != null)
            {
                _interactor.Interactable.Interact(transform);
            }

        }
    }

    public void PlaceItemInKeyItemPosition(Transform keyItemSlotTransform, Action callback)
    {
        PersistantObjects.Instance.SwitchObjectToActiveDimensionScene(_currentItemInHand);
        _currentItemInHand.PlaceOnKeyItemPosition(keyItemSlotTransform, callback);
        _currentItemInHand.GetItemDataSO().SetIsHeldByPlayer(false);
        _currentItemInHand = null;
    }
}
