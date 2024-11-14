using System;
using System.Collections;
using UnityEngine;

public class Item: MonoBehaviour
{
    Material _defaultMaterial;
    MeshRenderer _meshRenderer;
    Collider _collider;

    private void Awake()
    {
        _defaultMaterial = GetComponent<MeshRenderer>().material;
        _meshRenderer = GetComponent<MeshRenderer>();
        _collider = GetComponent<Collider>();
    }

    public void SetMaterial(Material material)
    {
        _meshRenderer.material = material;
    }
    public void ResetMaterial()
    {
        _meshRenderer.material = _defaultMaterial;
    }

    public void PickUp(Transform transform)
    {
        StopAllCoroutines();
        _collider.enabled = false;
        StartCoroutine(PickUpItem(transform));
    }

    public void PlaceDown(PlayerInteractor interactor)
    {
        StopAllCoroutines();
        gameObject.SetActive(true);
        Vector3 point = interactor.GetInteractionPosition();
        StartCoroutine(PlaceItemOnPoint(point));
        
    }

    private IEnumerator PickUpItem(Transform player)
    {
        float elapsedTime = 0f;
        float duration = 2f;

        while(elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, player.position, elapsedTime / duration);
            yield return null;
        }

        gameObject.SetActive(false);
    }
    private IEnumerator PlaceItemOnPoint(Vector3 point)
    {
        float elapsedTime = 0f;
        float duration = 2f;
        _collider.enabled = true;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, point, elapsedTime / duration);
            yield return null;
        }

        transform.position = point;

    }
}