using UnityEngine;

[CreateAssetMenu(fileName = "NewItemData", menuName = "ScriptableObjects/ItemDataSO")]
public class ItemDataSO : ScriptableObject
{
    public string ItemName = "UnnamedItem";
    public Vector3 Position;
    public Dimension CurrentDimension;
    public bool isDestroyed = false;
    [Header("Set this manually or nothing workds :D")]
    [SerializeField] private Dimension _startingDimension;
    private bool _isHeldByPlayer = false;
    public bool IsHeldByPlayer { get { return _isHeldByPlayer;} }

    public void UpdateItemData(Vector3 position, Dimension currentDimension, bool isDestroyed)
    {
        this.Position = position;
        this.CurrentDimension = currentDimension;
        this.isDestroyed = isDestroyed;
    }

    public Dimension StartingDimension
    {
        get { return _startingDimension; }
    }

    public void Reset()
    {
        CurrentDimension = StartingDimension;
        isDestroyed = false;
    }

    public void SetIsHeldByPlayer(bool isHeldByPlayer)
    {
        this._isHeldByPlayer = isHeldByPlayer;
    }

}
