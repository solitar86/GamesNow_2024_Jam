using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerInGameUI : MonoBehaviour
{
    [SerializeField] Image _isReadyImage;
    [SerializeField] Player_DimensionSwitcher _dimensionSwitcher;

    [SerializeField] Image _suitChargeImage;
    [SerializeField] Image _suitChargeBG;
    [SerializeField] Image _oxygenMeterImage;

    private  Color _normalSuitChargeColor;
    [SerializeField] private Color _notChargedSuitChargeColor;
    public GameObject HUD; 

    private void Awake()
    {
        _normalSuitChargeColor = _suitChargeBG.color;
    }
    private void Update()
    {
       _isReadyImage.color = _dimensionSwitcher.CanSwitchDimensions ? Color.green : Color.red;
        _oxygenMeterImage.fillAmount = _dimensionSwitcher.EmergencyTimerNormalized;
        _suitChargeImage.fillAmount = _dimensionSwitcher.SuitChargeLevelNormalized;

        _suitChargeBG.color = _suitChargeImage.fillAmount > 0.99f ? _normalSuitChargeColor : _notChargedSuitChargeColor;
    }

    public void EnableHUD() {
        print("Enabling HUD");
        HUD.SetActive(true);
    }
}
