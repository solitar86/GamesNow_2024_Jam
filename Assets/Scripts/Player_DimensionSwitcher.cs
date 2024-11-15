using System;
using UnityEngine;
using UnityEngine.Events;

public class Player_DimensionSwitcher : MonoBehaviour
{
    [SerializeField] KeyCode _keyToSwitchDimension;
    private bool _canSwitchDimensions = true;
    [SerializeField] Sound _triggerSound;
    [SerializeField] Sound _switchSound;
    [SerializeField] Sound _reloadSound;

    public UnityEvent OnStartSceneSwitch;
    public UnityEvent OnFinishSceneSwitch;

    public bool CanSwitchDimensions { get { return _canSwitchDimensions; } }

    private void Start()
    {
        SceneLoader.Instance.OnDimensionReadyToActivate += PlaySwitchingAudioAndEffects;
    }

    #region Update
    private void Update()
    {
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
    }

    private void PlaySwitchingAudioAndEffects(Dimension dimension)
    {
        AudioManager.PlaySoundAtPoint(this, _switchSound, transform.position);
        float delay = _switchSound.Clip.length;
        Invoke(nameof(StartReloadSequence), delay);
    }

    private void StartReloadSequence()
    {
        AudioManager.PlaySoundAtPoint(this, _reloadSound, transform.position);
        float delay = _reloadSound.Clip.length;
        Invoke(nameof(AllowDimensionSwitch), delay);
        OnFinishSceneSwitch.Invoke();

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
