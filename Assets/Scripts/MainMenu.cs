using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] SceneField _startScene;
    public void PlayGame ()
    {
        SceneManager.LoadScene(_startScene);
    }

    public void QuitGame ()
    {
        Application.Quit();
    }
}


