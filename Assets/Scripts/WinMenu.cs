using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class WinMenu : MonoBehaviour
{
    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
