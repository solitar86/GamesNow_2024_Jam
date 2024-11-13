using UnityEngine;

public class DimensionManager : MonoBehaviour
{
    public static DimensionManager Instance { get; private set; }
    public Dimension CurrentDimension { get; private set; }

    #region Setup
    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }

    #endregion

    public void SwitchDimensionTo(Dimension dimensionToSwitchTo)
    {
        CurrentDimension = dimensionToSwitchTo;
        SceneLoader.Instance.LoadDimensionScene(dimensionToSwitchTo);
    }
}




