using UnityEditor;
using UnityEngine;

public class DebugMenuFunctions
{
    [MenuItem("Testing / Reset ItemData", false, 10)]
    public static void EnableAllProps()
    {
        ItemDataSO[] SOs = Resources.LoadAll<ItemDataSO>("ItemData");

        foreach (var item in SOs)
        {
            item.Reset();
        }

        var allitemsInScene = MonoBehaviour.FindObjectsByType<Item>(FindObjectsSortMode.None);
        foreach (var item in allitemsInScene)
        {
            item.SavePosition();
        }
    }
}
