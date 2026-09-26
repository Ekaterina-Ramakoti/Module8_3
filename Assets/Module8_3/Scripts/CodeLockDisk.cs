using System.Collections;
using TMPro;
using UnityEngine;

public class CodeLockDisk : MonoBehaviour
{
    [Header("Numbers")]
    [SerializeField] private TMP_Text numberTop;
    [SerializeField] private TMP_Text numberCurrent;
    [SerializeField] private TMP_Text numberBottom;

    [Header("Animation")]
    [SerializeField] private RectTransform numbers;
    [SerializeField] private float stepDistance = 60f;
    [SerializeField] private float animationDuration = 0.2f;

    private int currentValue = 0;
    private bool isAnimating = false;

    public bool IsAnimating => isAnimating;
    public int CurrentValue => currentValue;

    private void Start()
    {
        UpdateNumbers();
    }

    public void SetValue(int value)
    {
        currentValue = NormalizeValue(value);
        UpdateNumbers();
    }

    public void ChangeValue(int change)
    {
        if (change == 0 || isAnimating)
        {
            return;
        }
           
        StartCoroutine(AnimateChange(change));
    }

    private IEnumerator AnimateChange(int change)
    {
        isAnimating = true;

        int direction = change > 0 ? 1 : -1;
        int steps = Mathf.Abs(change);

        for (int i = 0; i < steps; i++)
        {
            yield return StartCoroutine(AnimateOneStep(direction));

            currentValue = NormalizeValue(currentValue + direction);

            UpdateNumbers();
        }

        isAnimating = false;
    }

    private IEnumerator AnimateOneStep(int direction)
    {
        Vector2 startPosition = numbers.anchoredPosition;

        float visualDirection = direction > 0 ? 1f : -1f;

        Vector2 targetPosition = startPosition + Vector2.up * stepDistance * visualDirection;

        float elapsed = 0f;

        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;

            float progress = elapsed / animationDuration;

            progress = Mathf.SmoothStep(0f, 1f, progress);

            numbers.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, progress);

            yield return null;
        }

        numbers.anchoredPosition = startPosition;
    }

    private void UpdateNumbers()
    {
        numberCurrent.text = currentValue.ToString();

        numberTop.text = NormalizeValue(currentValue - 1).ToString();

        numberBottom.text = NormalizeValue(currentValue + 1).ToString();
    }

    private int NormalizeValue(int value)
    {
        return (value + 10) % 10;
    }

    public void ResetDisk(int value)
    {
        StopAllCoroutines();

        isAnimating = false;

        numbers.anchoredPosition = Vector2.zero;

        currentValue = NormalizeValue(value);

        UpdateNumbers();
    }
}
