using UnityEngine;
using UnityEngine.SceneManagement;

public class ComputerPower : MonoBehaviour
{
    Computer_Password _computerScript;
    Collider _collider;
    Light _computerLight;
    bool _computerPowerEnabled = false;
    

    private void Start() {
        if (PersistantObjects.GameState.PowerOnline == false)
        {
            _computerScript = GetComponent<Computer_Password>();
            _collider = GetComponent<Collider>();
            _collider.enabled = false;
            _computerLight = GetComponentInChildren<Light>();
            _computerLight.enabled = false;
            _computerScript.enabled = false;
        }
    }

    private void Update() {
        if(!_computerPowerEnabled && PersistantObjects.GameState.PowerOnline == true)
        {
            ComputerPowered();          
        }
    }

    public void ComputerPowered() {
        PersistantObjects.GameState.SetPowerOnline(true);
        _computerPowerEnabled = true;
        _collider.enabled = true;
        _computerScript.enabled = true;
        _computerLight.enabled = true;
    }

    public void LoggedIn() {
        SceneManager.LoadScene(6);
    }

}
