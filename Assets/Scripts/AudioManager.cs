using System;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;


public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private AudioSource _darkDimensionAmbianceSource, _lightDimensionAmbianceSource;
    [Space(15)]
    [SerializeField] AudioMixerGroup _sfxGroup;

    private void Awake()
    {
        if (instance == null) { instance = this; }
        else Destroy(this.gameObject);
    }

    private void Start()
    {
        PlayMusicOnLoop();
    }

    private void PlayMusicOnLoop()
    {
        if(DimensionManager.Instance.CurrentDimension == Dimension.Light)
        {
            _darkDimensionAmbianceSource.volume = 0;
        }
        else
        {
            _lightDimensionAmbianceSource.volume = 0;
        }

        _lightDimensionAmbianceSource.loop = true;
        _darkDimensionAmbianceSource.loop = true;
        _lightDimensionAmbianceSource.Play();
        _darkDimensionAmbianceSource.Play();


    }

    public static void PlaySoundAtPoint(object sender, Sound soundToPlay, Vector3 point)
    {
        if(soundToPlay.Clip == null) return;

        GameObject tempGameObject = new GameObject(sender.ToString() + " : " + soundToPlay.Clip.ToString());
        tempGameObject.transform.position = point;
        AudioSource audioSource = (AudioSource)tempGameObject.AddComponent(typeof(AudioSource));
        audioSource.clip = soundToPlay.Clip;
        audioSource.outputAudioMixerGroup = soundToPlay.Mixergroup;
        audioSource.spatialBlend = 0.75f;
        audioSource.volume = soundToPlay.Volume;
        audioSource.Play();
        Destroy(tempGameObject,
            audioSource.clip.length);
    }

    public static void PlayClipAtPoint(object sender, AudioClip clipToPlay, Vector3 point, float volume = 1f)
    {
        Sound soundToPlay = new Sound();
        soundToPlay.Pitch = 1;
        soundToPlay.Volume = volume;
        soundToPlay.Clip = clipToPlay;
        soundToPlay.Mixergroup = instance._sfxGroup;

        PlaySoundAtPoint(sender, soundToPlay, point);
    }

    public static AudioClip GetRandomClipFromArray(AudioClip[] _clipArray, AudioClip previousFootStep = null)
    {
        AudioClip randomClip;
        do
        {
            randomClip = _clipArray[Random.Range(0, _clipArray.Length)];
        } while (randomClip == previousFootStep);

        return randomClip;
    }
}

[System.Serializable]
public class Sound
{
    [SerializeField] public AudioClip Clip;
    [SerializeField] public AudioMixerGroup Mixergroup;
    [SerializeField, Range(0f,1f)] public float Volume = 1f;
    [SerializeField, Range(-3, 3f)] public float Pitch = 1;
    
}
