using StarterAssets;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Computer_Password : MonoBehaviour, Iinteractable
{
    [SerializeField] Canvas _computerCanvas;
    [SerializeField] Button _exitButton;
    [SerializeField] TMP_InputField _inputField;
    [SerializeField] string _correctPassword;

    public UnityEvent _OnComputerInteractedWith;
    public UnityEvent _OnPassWordCorrect;
    public UnityEvent _OnPassWordIncorrect;
    public UnityEvent _OnExitComputerScreen;

    private Transform _playerTransform;

    private string _previousInputFieldText;

    [SerializeField] private AudioSource _keyboardSource;

    private void Awake()
    {
        _previousInputFieldText = _inputField.text;
        _computerCanvas.enabled = false;
        if (string.IsNullOrEmpty(_correctPassword))
        {
            _correctPassword = "password";
        }
        if(_keyboardSource == null) _keyboardSource = GetComponent<AudioSource>();
    }
    void Iinteractable.Interact(Transform playerTransform)
    {
        _OnComputerInteractedWith?.Invoke();
        _computerCanvas.enabled = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        this._playerTransform = playerTransform;

        _playerTransform.GetComponentInChildren<FirstPersonController>().enabled = false;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            ExitMenu();
        }
        CheckPassword();
    }

    private void CheckPassword()
    {
        if(_previousInputFieldText != _inputField.text)
        {
            _keyboardSource.Play();
            _previousInputFieldText = _inputField.text;
        }

        if (_inputField.text.ToLower() == _correctPassword.ToLower())
        {
            ExitMenu();
            _OnPassWordCorrect?.Invoke();
        }
    }

    public void ExitMenu()
    {
        _computerCanvas.enabled = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if(_playerTransform != null) 
        {
            _playerTransform?.gameObject.SetActive(true);
            _playerTransform.GetComponentInChildren<FirstPersonController>().enabled = true;
        }
    }
}
