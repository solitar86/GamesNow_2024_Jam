using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerInGameUI : MonoBehaviour
{
    [SerializeField] Image _isReadyImage;
    [SerializeField] Player_DimensionSwitcher _dimensionSwitcher;

    [SerializeField] Image _suitChargeImage;
    [SerializeField] Image _oxygenMeterImage;
    private void Update()
    {
       _isReadyImage.color = _dimensionSwitcher.CanSwitchDimensions ? Color.green : Color.red;
        _oxygenMeterImage.fillAmount = _dimensionSwitcher.EmergencyTimerNormalized;
        _suitChargeImage.fillAmount = _dimensionSwitcher.SuitChargeLevelNormalized;
    }
}
