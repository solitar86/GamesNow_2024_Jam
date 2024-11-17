using UnityEngine;
using UnityEngine.SceneManagement;

public class ComputerPower : MonoBehaviour
{
    Computer_Password _computerScript;
    Collider _collider;
    Light _computerLight;
    

    private void Start() {
        _computerScript = GetComponent<Computer_Password>();
        _collider = GetComponent<Collider>();
        _collider.enabled = false;
        _computerLight = GetComponentInChildren<Light>();
        _computerLight.enabled = false;
        _computerScript.enabled = false;
    }

    public void ComputerPowered() {
        _collider.enabled = true;
        _computerScript.enabled = true;
        _computerLight.enabled = true;
    }

    public void LoggedIn() {
        SceneManager.LoadScene(7);
    }

}
