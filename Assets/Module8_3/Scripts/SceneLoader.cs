using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private GameObject losePanel;

    [Header("Game")]
    [SerializeField] private TimerUpdater timerUpdater;
    [SerializeField] private CodeLock codeLock;

    public void StartGame()
    {
        mainMenuPanel.SetActive(false);
        gamePanel.SetActive(true);
        victoryPanel.SetActive(false);
        losePanel.SetActive(false);

        timerUpdater.ResetTimer();
        codeLock.ResetLock();
    }

    public void LoadMainMenu()
    {
        mainMenuPanel.SetActive(true);
        gamePanel.SetActive(false);
        victoryPanel.SetActive(false);
        losePanel.SetActive(false);
    }

    public void ShowVictory()
    {
        mainMenuPanel.SetActive(false);
        gamePanel.SetActive(false);
        victoryPanel.SetActive(true);
        losePanel.SetActive(false);
    }

    public void ShowLose()
    {
        mainMenuPanel.SetActive(false);
        gamePanel.SetActive(false);
        victoryPanel.SetActive(false);
        losePanel.SetActive(true);
    }
}
