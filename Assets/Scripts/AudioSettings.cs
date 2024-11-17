using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    AudioMixer _mainMixer;
    [SerializeField] private GameObject _audioPanel;
    [SerializeField] Slider _masterVolumeSlider, _musicVolumeSlider, _sfxVolumeSlider;

    private bool _recordVolumeValues = false;
    private bool _slidersAreReady = false;
    private const string MASTERVOLUME = "MasterVolume";
    private const string MUSICVOLUME = "MusicVolume";
    private const string SFXVOLUME = "SFXVolume";

    private void Awake()
    {
        _masterVolumeSlider.onValueChanged.AddListener((delegate { SetMasterVolume(_masterVolumeSlider.value); }));
        //_musicVolumeSlider.onValueChanged.AddListener((delegate { SetMusicVolume(_musicVolumeSlider.value); }));
        //_sfxVolumeSlider.onValueChanged.AddListener((delegate { SetSFXVolume(_sfxVolumeSlider.value); }));

    }
    void Start()
    {
        InitializeAudioSettings();
    }

    public void InitializeAudioSettings()
    {
        _mainMixer = Resources.Load<AudioMixer>("MainMixer") as AudioMixer;
        _recordVolumeValues = false;
        _slidersAreReady = true;
        GetSettingsFromPlayerPrefs();
        _recordVolumeValues = true;
        //_audioPanel.SetActive(false);
    }
  
  public void SetMasterVolume(float decimalVolume)
    {
        if (_slidersAreReady == false) return;

        float dbVolume = Mathf.Log10(decimalVolume) * 20;
        if (decimalVolume == 0.0f)
        {
            dbVolume = -80.0f;
        }
        _mainMixer.SetFloat(MASTERVOLUME, dbVolume);
        RecordVolumeValue(MASTERVOLUME, decimalVolume);
    }

    public void SetMusicVolume(float decimalVolume)
    {
        if (_slidersAreReady == false) return;

        float dbVolume = Mathf.Log10(decimalVolume) * 20;
        if (decimalVolume == 0.0f)
        {
            dbVolume = -80.0f;
        }
        _mainMixer.SetFloat(MUSICVOLUME, dbVolume);
        RecordVolumeValue(MUSICVOLUME, decimalVolume);
    }

    public void SetSFXVolume(float decimalVolume)
    {
        if (_slidersAreReady == false) return;

        float dbVolume = Mathf.Log10(decimalVolume) * 20;
        if (decimalVolume == 0.0f)
        {
            dbVolume = -80.0f;
        }
        _mainMixer.SetFloat(SFXVOLUME, dbVolume);
        RecordVolumeValue(SFXVOLUME, decimalVolume);
    }



    void GetSettingsFromPlayerPrefs()
    {
        SetMasterVolume(PlayerPrefs.GetFloat(MASTERVOLUME, 1f));
        //SetMusicVolume(PlayerPrefs.GetFloat(MUSICVOLUME, 1f));
        //SetSFXVolume(PlayerPrefs.GetFloat(SFXVOLUME, 1f));

        SetSliderValuesToMatchRecordedValue(PlayerPrefs.GetFloat(MASTERVOLUME, 1), _masterVolumeSlider);
        //SetSliderValuesToMatchRecordedValue(PlayerPrefs.GetFloat(MUSICVOLUME, 1), _musicVolumeSlider);
        //SetSliderValuesToMatchRecordedValue(PlayerPrefs.GetFloat(SFXVOLUME, 1), _sfxVolumeSlider);


    }

    void SetSliderValuesToMatchRecordedValue(float value, Slider slider)
    {
        slider.value = value;
    }

    void RecordVolumeValue(string prefsKey, float value)
    {
        if (_recordVolumeValues == false) return;
        PlayerPrefs.SetFloat(prefsKey, value);
    }

}
