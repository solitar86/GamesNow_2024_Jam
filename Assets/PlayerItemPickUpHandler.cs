using UnityEngine;

public class PlayerItemPickUpHandler : MonoBehaviour
{
    [SerializeField] private KeyCode _itemPickUpKey;
    [SerializeField] private PlayerInteractor _interactor;

    private Item _currentItem;
    private void Start()
    {
        _interactor = GetComponent<PlayerInteractor>();   
    }
    private void Update()
    {
        if(Input.GetKeyDown(_itemPickUpKey))
        {
            if (_currentItem != null && _interactor.CanPlaceItem() == true)
            {
                _currentItem.PlaceDown(_interactor);
                _currentItem = null;
            }
            if(_interactor.SelectedItem != null)
            {
                _interactor.SelectedItem.PickUp(transform);
                _currentItem = _interactor.SelectedItem;
                _interactor.DeselectItem(_currentItem);
            }

        }
    }
}
