using UnityEngine;

public class DimensionManager : MonoBehaviour
{
    [SerializeField] private KeyCode _dimensionShiftKey;
    public static DimensionManager Instance { get; private set; }

    public Dimension CurrentDimension { get; private set; }

    #region Setup
    private void Awake()

    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }

    private void Start()
    {
        // Load initial Dimension
        SwitchDimensionTo(Dimension.Light);
    }

    #endregion

    #region Update
    private void Update()
    {
        if (Input.GetKeyDown(_dimensionShiftKey))
        {
            SwitchToOtherDimension();
        }

    }
    #endregion

    public void SwitchDimensionTo(Dimension dimensionToSwitchTo)
    {
        CurrentDimension = dimensionToSwitchTo;
        SceneLoader.Instance.LoadDimensionScene(dimensionToSwitchTo, false);
    }

    public void SwitchToOtherDimension()
    {
        CurrentDimension = CurrentDimension == Dimension.Light ?  Dimension.Dark : Dimension.Light;
        SwitchDimensionTo(CurrentDimension);
    }
}




