using UnityEngine;

[CreateAssetMenu(fileName = "New Dialoque", menuName = "ScriptableObjects/Dialoque")]
public class DialoqueSO : ScriptableObject
{
    [SerializeField, TextArea(2, 3)] public string dialoqueLine;
    [SerializeField] public AudioClip _dialoqueAudio;
    [SerializeField] public float _timeToShowOnScreen = 3f;
}