using StarterAssets;
using UnityEngine;

public class Audio_FootStepHandler : MonoBehaviour
{
    [SerializeField] StarterAssetsInputs _inputs;
    [SerializeField, Range(0f, 1f)] float _footStepInterval = 0.5f;
    [SerializeField, Range(0f,1f)] float footStepVolume = 1f;
    [Space(15)]
    [SerializeField] private AudioClip[] _footStepsDarkDimension, _footstepsLightDimension;
    private AudioClip _previousFootStep;
    private float _nextFootStepTime;
    

    private void Awake()
    {
        if (_inputs == null) _inputs = GetComponent<StarterAssetsInputs>();
    }

    private void Update()
    {
       if(Time.time > _nextFootStepTime)
        {
            _nextFootStepTime = Time.time + _footStepInterval;
            if(_inputs.move.magnitude > 0)
            {
                PlayPlayerFootStep();
            }
        }
    }

    public void PlayPlayerFootStep()
    {
        AudioClip footstep;

        if (DimensionManager.Instance.CurrentDimension == Dimension.Light)
        {
            footstep = AudioManager.GetRandomClipFromArray(_footstepsLightDimension, _previousFootStep);
        }
        else
        {
            footstep = AudioManager.GetRandomClipFromArray(_footStepsDarkDimension, _previousFootStep);
        }

        _previousFootStep = footstep;

        AudioManager.PlayClipAtPoint(this, footstep, transform.position, footStepVolume);

    }
}
