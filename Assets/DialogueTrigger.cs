using UnityEngine;

/// <summary>
/// Triggers a Dialoque when player enters volume
/// </summary>
public class DialoqueTrigger : MonoBehaviour
{
    [SerializeField] string PlayerTag;
    [SerializeField] DialoqueSO[] _dialoqueLines;

    private void Start()
    {
        PersistantObjects.Instance.HandleDuplicateDialoque(_dialoqueLines[0], gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == PlayerTag)
        {
            TriggerDialoque();
        }
    }

    private void TriggerDialoque()
    {
        DialoqueSystem.instance.DisplayDialoque(_dialoqueLines);
        PersistantObjects.Instance.RegisterDialoqueTrigger(_dialoqueLines[0]);
        Destroy(gameObject);
    }
}
