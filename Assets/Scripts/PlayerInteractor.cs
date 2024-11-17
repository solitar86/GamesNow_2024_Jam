using System;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

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

    [Space(15), Header("TEXT POP-UP")]
    [SerializeField] GameObject _textPopUp;
    [SerializeField] TextMeshProUGUI _textPopUpText;

    [SerializeField, ReadOnly(true)] Item _selectedItem;
    [SerializeField, ReadOnly(true)] Iinteractable _selectedInteractable;
    public Item SelectedItem { get { return _selectedItem; } }
    public Iinteractable Interactable { get { return _selectedInteractable;} }

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
        }

        HandleItemSelection(hitInfo);
    }

    private void HandleItemSelection(RaycastHit hitInfo)
    {
        if(hitInfo.collider == null)
        {
            // We didn't hit anything
            DeselectCurrentItem();
            DeSelectCurrentInteractable();
            return;
        }
        if (hitInfo.collider.TryGetComponent<Item>(out Item item))
        {
            if(item.IsFrozen == false)
            {
                DeSelectCurrentInteractable();
                SelectThisItem(item);
                return;
            }
        }
        else
        {
            DeselectCurrentItem();
        }

        if(hitInfo.collider.TryGetComponent<Iinteractable>(out Iinteractable interactable))
        {
            DeSelectCurrentInteractable();
            SelectInteractable(interactable);
        }
        else
        {
            DeselectCurrentItem();
            DeSelectCurrentInteractable();
        }

    }

    private void SelectInteractable(Iinteractable interactable)
    {
        DeSelectCurrentInteractable();
        _selectedInteractable = interactable;
        _textPopUp.SetActive(true);
        _textPopUpText.SetText("Interact\n(E)");
    }

    private void DeSelectCurrentInteractable()
    {
        _selectedInteractable = null;
        _textPopUp.SetActive(false);
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

    public void DisAbleSelecting()
    {
        DeselectCurrentItem();
        this.enabled = false;
    }

    public void DeselectItem(Item itemBeingPickedUp)
    {
        if(_selectedItem == itemBeingPickedUp)
        {
            DeselectCurrentItem();
        }
    }
}
