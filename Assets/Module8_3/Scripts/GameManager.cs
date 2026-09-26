using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private SceneLoader sceneLoader;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void LoseGame()
    {
        sceneLoader.ShowLose();
    }

    public void WinGame()
    {
        sceneLoader.ShowVictory();
    }
}
