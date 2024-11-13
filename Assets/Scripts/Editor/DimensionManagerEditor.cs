using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DimensionManager))]
public class DimensionManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        DimensionManager dimensionManager = (DimensionManager)target;
        GUILayout.Space(10);

        if (GUILayout.Button("Light Dimension"))
        {
            dimensionManager.SwitchDimensionTo(Dimension.Light);
        }

        if (GUILayout.Button("Dark Dimension"))
        {
            dimensionManager.SwitchDimensionTo(Dimension.Dark);
        }
    }
}
