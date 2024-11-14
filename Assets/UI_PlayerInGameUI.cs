using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerInGameUI : MonoBehaviour
{
    [SerializeField] Image _isReadyImage;
    [SerializeField] Player_DimensionSwitcher _dimensionSwitcher;
    private void Update()
    {
       _isReadyImage.color = _dimensionSwitcher.CanSwitchDimensions ? Color.green : Color.red;
    }
}
