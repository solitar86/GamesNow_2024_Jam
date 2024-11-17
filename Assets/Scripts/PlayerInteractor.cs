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
    [SerializeField] GameObject _textPopUpCanvas;
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
        // Create a PointerEventData object
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition // Set to current mouse position
        };

        // Store raycast results
        List<RaycastResult> results = new List<RaycastResult>();

        // Perform a raycast
        EventSystem.current.RaycastAll(pointerEventData, results);

        // Check what the raycast hit
        if (results.Count > 0)
        {
            Debug.Log("Currently hovering over: " + results[0].gameObject.name);
        }
        else
        {
            Debug.Log("Not hovering over any UI element.");
        }

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
        Debug.DrawRay(mainCamera.transform.position, _interactionRay.GetPoint(_interactionDistance), Color.red, 0.001f);
#endif
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
            SelectInteractable(interactable);
        }
    }

    private void SelectInteractable(Iinteractable interactable)
    {
        DeSelectCurrentInteractable();
        _selectedInteractable = interactable;
        _textPopUpCanvas.SetActive(true);
        _textPopUpText.SetText("Interact\n(E)");
    }

    private void DeSelectCurrentInteractable()
    {
        _selectedInteractable = null;
        _textPopUpCanvas.SetActive(false);
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
