using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("MainGame");
    }

    public void HowTo()
    {
        SceneManager.LoadScene("HowTo");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
