using TMPro;
using UnityEngine;

public class TimerUpdater : MonoBehaviour
{
   [SerializeField] private TMP_Text timerText;
    [SerializeField] private float startTime = 30f;

    private float timeRemaining;
    private bool isRunning;

    private void Start()
    {
        ResetTimer();
    }

    private void Update()
    {
        if (!isRunning)
        {
            return;
        }
            

        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            isRunning = false;

            UpdateTimerText();          
            GameManager.Instance.LoseGame();
            
            return;
        }

        UpdateTimerText();
    }

    public void ResetTimer()
    {
        timeRemaining = startTime;
        isRunning = true;

        UpdateTimerText();
    }

    private void UpdateTimerText()
    {
        timerText.text = Mathf.CeilToInt(timeRemaining).ToString();
    }
}
