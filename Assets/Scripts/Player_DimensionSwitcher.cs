using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Player_DimensionSwitcher : MonoBehaviour
{
    [SerializeField] float _emergencyDuration = 30f;
    [SerializeField] float _suitChargeDuration = 3.6f;
    [SerializeField] KeyCode _keyToSwitchDimension;
    private bool _canSwitchDimensions = true;
    [SerializeField] Sound _triggerSound;
    [SerializeField] Sound _switchSound;
    [SerializeField] Sound _reloadSound;
    [Space(15)]
    [SerializeField] Sound _heavyBreathSound;
    public bool _suitOn = true;
    public UnityEvent OnStartSceneSwitch;
    public UnityEvent OnFinishSceneSwitch;

    private bool _wasEmergencySwitch = false;

    private float _emergencyTimer;
    private bool _hasReachedEmergencyDuration = false;
    public float EmergencyTimerNormalized { get { return 1f-(_emergencyTimer / _emergencyDuration); } }

    private float _suitChargeLevelForUi = 1f;
    public float SuitChargeLevelNormalized { get { return _suitChargeLevelForUi; } }
    public bool CanSwitchDimensions { get { return _canSwitchDimensions; } }

    private void Start()
    {
        SceneLoader.Instance.OnDimensionReadyToActivate += PlaySwitchingAudioAndEffects;
        if (!_suitOn)
        {
            _canSwitchDimensions = false;
        }
    }
    public void EquippedSuit() {
        _suitOn = true;
        //play equip sound etc.
    }
    #region Update
    private void Update()
    {
        if(DimensionManager.Instance.CurrentDimension == Dimension.Light)
        {
            _emergencyTimer -= Time.deltaTime;
            if(_emergencyTimer < 0) _emergencyTimer = 0;
            _hasReachedEmergencyDuration = false;
        }
        else if (DimensionManager.Instance.CurrentDimension == Dimension.Dark) 
        {
            _emergencyTimer += Time.deltaTime;
            if(_emergencyTimer > _emergencyDuration && _hasReachedEmergencyDuration == false) 
            {
                _emergencyTimer = _emergencyDuration;
                _hasReachedEmergencyDuration = true;
                _canSwitchDimensions = false;
                _wasEmergencySwitch = true;
                StartDimensionSwitchSequence();
            }
        }


        if (_canSwitchDimensions == true && Input.GetKeyDown(_keyToSwitchDimension))
        {
            _canSwitchDimensions = false;
            StartDimensionSwitchSequence();

        }

    }

    #endregion
    private void StartDimensionSwitchSequence()
    {
        AudioManager.PlaySoundAtPoint(this, _triggerSound, transform.position);
        DimensionManager.Instance.SwitchToOtherDimension();
        OnStartSceneSwitch.Invoke();
        _suitChargeLevelForUi = 0f;
    }

    private void PlaySwitchingAudioAndEffects(Dimension dimension)
    {
        AudioManager.PlaySoundAtPoint(this, _switchSound, transform.position);
        float delay = _switchSound.Clip.length;
        Invoke(nameof(StartReloadSequence), delay);

        if(_wasEmergencySwitch)
        {
            AudioManager.PlaySoundAtPoint(this, _heavyBreathSound, transform.position);
            _wasEmergencySwitch = false;
        }
    }

    private void StartReloadSequence()
    {
        AudioManager.PlaySoundAtPoint(this, _reloadSound, transform.position);
        float delay = _reloadSound.Clip.length;
        Invoke(nameof(AllowDimensionSwitch), delay - 1.4f);
        OnFinishSceneSwitch.Invoke();
        StartCoroutine(ChargeMeterLoadNumberNormalized());

    }


    private IEnumerator ChargeMeterLoadNumberNormalized()
    {
        float elapsedTime = 0f;
        float percentComplete;
        while (_suitChargeDuration > elapsedTime)
        {
            elapsedTime += Time.deltaTime;
            percentComplete = elapsedTime / _suitChargeDuration;
            _suitChargeLevelForUi = percentComplete;
            yield return null;
        }

        _suitChargeLevelForUi = 1f;
    }

    private void AllowDimensionSwitch()
    {
        _canSwitchDimensions = true;
    }

    private void OnDestroy()
    {
        SceneLoader.Instance.OnDimensionReadyToActivate -= PlaySwitchingAudioAndEffects;
    }
}
