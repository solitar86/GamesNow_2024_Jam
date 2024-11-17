using System;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerInGameUI : MonoBehaviour
{
    [SerializeField] Player_DimensionSwitcher _dimensionSwitcher;

    [SerializeField] Image _suitChargeImage_Light;
    [SerializeField] Image _suitChargeBG_Light;
    [SerializeField] Image _oxygenMeterImage_Light;
    [Space(15)]
    [SerializeField] Image _suitChargeImage_dark;
    [SerializeField] Image _suitChargeBG_dark;
    [SerializeField] Image _oxygenMeterImage_dark;

    Image _suitChargeImage_Current;
    Image _suitChargeBG_Current;
    Image _oxygenMeterImage_Current;

    private Color _normalChargeColor_Light;
    private Color _normalChargeColor_Dark;
    [SerializeField, ColorUsage(true, true)] private Color _notChargedSuitChargeColor;
    private Color _currentNormalColor;

    public GameObject HUD_Light;
    public GameObject HUD_Dark;

    private void Awake() 
    {
        //SwitchHud(DimensionManager.Instance.CurrentDimension);
        _suitChargeImage_Current = _suitChargeImage_Light;
        _suitChargeBG_Current = _suitChargeBG_Light;
        _oxygenMeterImage_Current = _oxygenMeterImage_Light;

        _normalChargeColor_Light = _suitChargeBG_Light.color;
        _normalChargeColor_Dark = _suitChargeBG_dark.color;

        _currentNormalColor = _normalChargeColor_Light;


        HUD_Light.SetActive(false);
        HUD_Dark.SetActive(false);

    }

    private void Start()
    {
        SceneLoader.Instance.OnSceneIsActivated += OnSwitchDimension;
    }

    private void Update()
    {
        _oxygenMeterImage_Current.fillAmount = _dimensionSwitcher.EmergencyTimerNormalized;
        _suitChargeImage_Current.fillAmount = _dimensionSwitcher.SuitChargeLevelNormalized;
        _suitChargeBG_Current.color = _suitChargeImage_Current.fillAmount > 0.99f ? _currentNormalColor : _notChargedSuitChargeColor;
    }

    public void EnableHUD() {
        print("Enabling HUD");
        HUD_Light.SetActive(true);
    }

    private void OnSwitchDimension(Dimension dimension)
    {
        SwitchHud(dimension);
    }


    public void SwitchHud(Dimension dimension)
    {
        switch (dimension)
        {
            case Dimension.Light:
                _suitChargeImage_Current = _suitChargeImage_Light;
                _suitChargeBG_Current = _suitChargeBG_Light;
                _oxygenMeterImage_Current = _oxygenMeterImage_Light;
                _currentNormalColor = _normalChargeColor_Light;
                HUD_Light.SetActive(true);
                HUD_Dark.SetActive(false);
                break;

            case Dimension.Dark:
                _suitChargeImage_Current = _suitChargeImage_dark;
                _suitChargeBG_Current = _suitChargeBG_dark;
                _oxygenMeterImage_Current = _oxygenMeterImage_dark;
                _currentNormalColor = _normalChargeColor_Dark;
                HUD_Light.SetActive(false);
                HUD_Dark.SetActive(true);
                break;

            default:
                // Error here
                break;
        }
    }
}
