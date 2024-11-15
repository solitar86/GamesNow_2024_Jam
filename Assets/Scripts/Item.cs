
using System;
using System.Collections;

using UnityEngine;


public class Item: MonoBehaviour
{
    Material _defaultMaterial;
    MeshRenderer _meshRenderer;
    Collider _collider;

    [SerializeField] ItemDataSO _itemData;
    private static float _moveDuration = 0.2f;
    public bool IsFrozen { get { return _isFrozen; } }
    private bool _isFrozen;
   

    private void Awake()
    {
        _defaultMaterial = GetComponent<MeshRenderer>().material;
        _meshRenderer = GetComponent<MeshRenderer>();
        _collider = GetComponent<Collider>();
        gameObject.name = _itemData.name;
        PersistantObjects.Instance.AddItemToDictionary(this);
    }

    private void Start()
    {
        transform.position = _itemData.Position;
    }


    public string GetItemName()
    {
        return _itemData.name;
    }

    public void SetMaterial(Material material)
    {
        _meshRenderer.material = material;
    }
    public void ResetMaterial()
    {
        _meshRenderer.material = _defaultMaterial;
    }

    public void PickUpAndParentToPlayer(Transform playerTransform)
    {
        StopAllCoroutines();
        _collider.enabled = false;
        transform.SetParent(playerTransform, true);
        StartCoroutine(PickUpItem(playerTransform));
    }

    public void PlaceDownAndUnParent(PlayerInteractor interactor)
    {
        StopAllCoroutines();
        gameObject.SetActive(true);
        transform.SetParent(null, true);
        Vector3 point = interactor.GetInteractionPosition();
        StartCoroutine(PlaceItemOnPoint(point, _moveDuration, Quaternion.identity));
        UpdateItemData(point);
    }

    public void PlaceOnKeyItemPosition(Transform keyItemTransform, Action callback)
    {
        StopAllCoroutines();
        gameObject.SetActive(true);
        transform.SetParent(null, true);
        StartCoroutine(PlaceItemOnPoint(keyItemTransform.position, _moveDuration * 10, keyItemTransform.rotation, true, callback));
        UpdateItemData(keyItemTransform.position);

    }

    public void UpdateItemData(Vector3 position)
    {
        _itemData.UpdateItemData(position, DimensionManager.Instance.CurrentDimension, _itemData.isDestroyed);
    }

    private IEnumerator PickUpItem(Transform player)
    {
        float elapsedTime = 0f;
        float duration = _moveDuration;
        float percentComplete;
        Vector3 startPosition = transform.position;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            percentComplete = elapsedTime / duration;
            transform.position = Vector3.Lerp(startPosition, player.position, percentComplete);
            yield return null;
        }

        gameObject.SetActive(false);
    }
    private IEnumerator PlaceItemOnPoint(Vector3 point, float moveDuration, Quaternion rotation, bool includeRotation = false, Action OnCompleteCallback = null)
    {
        float elapsedTime = 0f;
        _collider.enabled = true;
        Vector3 startPosition = transform.position;
        float percentComplete;
        float lerpValue;

        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = rotation;

        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;
            percentComplete = elapsedTime / moveDuration;
            lerpValue = PersistantObjects.Instance.EvaluateSlowEndCurve(percentComplete);
            transform.position = Vector3.Lerp(startPosition, point, lerpValue);

            if(includeRotation )
            {
                transform.rotation = Quaternion.Slerp(startRotation, targetRotation, lerpValue);
            }
            yield return null;
        }

        transform.position = point;

        OnCompleteCallback?.Invoke();

    }

    private void OnValidate()
    {
        if (Application.isPlaying) return;
        if(_itemData == null)
        {
            Debug.LogWarning("<color=#ff0000> Item " + transform.gameObject.name + " has no itemData. ", gameObject);
        }

        _itemData.Position = transform.position;
        Debug.Log("On validate was called for " + gameObject.name);
    }

    private void OnDestroy()
    {
        PersistantObjects.Instance.RemoveItemFromDictionary(this);
    }

    public void SetPosition()
    {
        transform.position = _itemData.Position;
    }


    public ItemDataSO GetItemDataSO() { return _itemData; }
    public void SetItemDataSO(ItemDataSO itemData) { _itemData = itemData; }

    public void SavePosition()
    {
        _itemData.Position = transform.position;
    }

    public void FreezeItem()
    {
        _isFrozen = true;
    }
}