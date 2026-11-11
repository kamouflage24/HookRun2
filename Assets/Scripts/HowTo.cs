using UnityEngine;
using UnityEngine.SceneManagement;

public class HowTo : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void LoadMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
