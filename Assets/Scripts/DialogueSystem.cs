using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Displayes dialoque on screen.
/// </summary>
public class DialoqueSystem : MonoBehaviour
{
    [HideInInspector] public static DialoqueSystem instance;
    private Queue<DialoqueSO> _dialoqueQueue = new();
    [SerializeField] private GameObject _dialoqueCanvas;
    [SerializeField] private TextMeshProUGUI _dialoqueTextField;
    [SerializeField] private RectTransform _dialoquePanel;
    private Vector2 _screenPositionOfSpeaker;

    Transform player;
    private void Awake()
    {
        if (instance == null) instance = this;
        _dialoqueCanvas.SetActive(false);
    }


    //private void Update()
    //{
    //    PositionDialoqueBox();
    //}

    //private void PositionDialoqueBox()
    //{
    //    if (_dialoqueCanvas.activeSelf == false) return;
    //    _screenPositionOfSpeaker = Camera.main.WorldToScreenPoint(player.position);
    //    var offset = _dialoquePanel.sizeDelta;
    //    _dialoquePanel.position = _screenPositionOfSpeaker + offset;
    //}

    public void DisplayDialoque(DialoqueSO[] _dialoqueSOArray)
    {
        ResetDialoqueBox();
        for (var i = 0; i < _dialoqueSOArray.Length; i++)
        {
            _dialoqueQueue.Enqueue(_dialoqueSOArray[i]);
        }

        StartCoroutine(DisplayDialoqueCoroutine());
    }

    private void ResetDialoqueBox()
    {
        StopAllCoroutines();
        _dialoqueQueue.Clear();
        _dialoqueCanvas.SetActive(false);
    }

    private IEnumerator DisplayDialoqueCoroutine()
    {
        _dialoqueCanvas.SetActive(true);
        //PositionDialoqueBox();

        while (_dialoqueQueue.Count != 0)
        {
            DialoqueSO currentDialogue = _dialoqueQueue.Dequeue();
            string dialoqueToShow = currentDialogue.dialoqueLine;

            // TODO: Add support to play audio:
            if (currentDialogue._dialoqueAudio != null)
            {
                AudioSource.PlayClipAtPoint(currentDialogue._dialoqueAudio, transform.position);
            }
            yield return StartCoroutine(TypeOutDialogueText(dialoqueToShow));

            float timeToShowOnScreen;
            //_dialoqueTextField.SetText(dialoqueToShow);
            if (currentDialogue._dialoqueAudio != null)
            {
            timeToShowOnScreen =
                    currentDialogue._timeToShowOnScreen > currentDialogue._dialoqueAudio.length ?
                    currentDialogue._timeToShowOnScreen : currentDialogue._dialoqueAudio.length;
            }
            else
            {
                timeToShowOnScreen = currentDialogue._timeToShowOnScreen;
            }

            yield return new WaitForSeconds(timeToShowOnScreen);
        }
        _dialoqueCanvas.SetActive(false);
    }

    private IEnumerator TypeOutDialogueText(string dialogueToType)
    {
        WaitForSeconds delayBetweenCharacters = new WaitForSeconds(0.01f);
        int characterIndex = 0;

        // Set full text so panel resizes accordingly.
        _dialoqueTextField.SetText("<color=#00000000>" + dialogueToType + "</color>");

        while (characterIndex <= dialogueToType.Length)
        {
            var text = dialogueToType.Substring(0, characterIndex);
            text += "<color=#00000000>" + dialogueToType.Substring(characterIndex) + "</color>";
            _dialoqueTextField.SetText(text);
            characterIndex++;

            yield return delayBetweenCharacters;
        }

    }

}
