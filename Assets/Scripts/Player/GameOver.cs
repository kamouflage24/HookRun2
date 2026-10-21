using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class GameOver : MonoBehaviour
{
    public static GameOver Instance{ get; private set; }
    [SerializeField] public GameObject gameOverUI;
    [SerializeField] private TMP_Text gameOverMessage;

    public bool IsGameOver {get; private set; }

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            enabled = false;
            return;
        }
        Instance = this;
        Time.timeScale = 1f;
        if(gameOverUI != null)
        {
            gameOverUI.SetActive(false);
        }
       
    }

    void OnDestroy()
    {
        if(Instance == this)
        {
            Instance = null;
            Time.timeScale = 1f;
        }
        
    }
    public static void TriggerGameOver(string message)
    {
        Instance.ShowGameOver(message);
    }

    private void ShowGameOver(string message)
    {
        if (IsGameOver)
        {
            return;
        }
        IsGameOver = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if(gameOverUI != null)
        {
            gameOverUI.SetActive(true);
        }
        if(gameOverMessage != null)
        {
            gameOverMessage.text = message;
        }
    }
    public void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
    public void QuitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }
}
