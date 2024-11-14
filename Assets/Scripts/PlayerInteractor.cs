using System;
using System.ComponentModel;
using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    Camera mainCamera;
    Ray _interactionRay;
    [SerializeField] RectTransform _crosshair;
    [SerializeField] float _interactionDistance;
    [SerializeField] LayerMask _interactWithLayers;

    [Space(15), Header("Interaction Settings")]
    [SerializeField] Material _selectedMaterial;

    [Space(15), Header("Debug Settings")]
    [SerializeField] bool _showDebugSphere;
    [SerializeField] Transform _debugSphere;

    [SerializeField, ReadOnly(true)] Item _selectedItem;
    public Item SelectedItem { get { return _selectedItem; } }

    private void Awake()
    {
        mainCamera = Camera.main;
        _debugSphere.gameObject.SetActive(false);
    }

    private void Update()
    {
        _interactionRay = mainCamera.ScreenPointToRay(_crosshair.position);
        if (Physics.Raycast(_interactionRay, out RaycastHit hitInfo, _interactionDistance, _interactWithLayers))
        {
            HandleItemSelection(hitInfo);
#if UNITY_EDITOR
            if (_showDebugSphere)
            {
                _debugSphere.gameObject.SetActive(true);
                _debugSphere.position = hitInfo.point;
            }
        }
        else
        {
            _debugSphere.gameObject.SetActive(false);
        }
        HandleItemSelection(hitInfo);
        Debug.DrawRay(mainCamera.transform.position, _interactionRay.GetPoint(_interactionDistance), Color.red, 0.001f);
#endif
    }

    private void HandleItemSelection(RaycastHit hitInfo)
    {
        if(hitInfo.collider == null)
        {
            // We didn't hit anything
            DeselectCurrentItem();
            return;
        }
        if (hitInfo.collider.TryGetComponent<Item>(out Item item))
        {
            SelectThisItem(item);
        }
        else
        {
            DeselectCurrentItem();
        }
    }

    private void SelectThisItem(Item item)
    {
        DeselectCurrentItem();
        _selectedItem = item;
        _selectedItem.SetMaterial(_selectedMaterial);
    }

    private void DeselectCurrentItem()
    {
        _selectedItem?.ResetMaterial();
        _selectedItem = null;
    }

    public bool CanPlaceItem()
    {
        return SelectedItem != null;
    }

    public Vector3 GetInteractionPosition()
    {
        return SelectedItem.transform.position;
    }

    public bool CanPickUpItem()
    {
       return SelectedItem != null;
    }

    public void DeselectItem(Item itemBeingPickedUp)
    {
        if(_selectedItem == itemBeingPickedUp)
        {
            DeselectCurrentItem();
        }
    }
}
