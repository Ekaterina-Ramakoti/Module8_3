using System.Collections;
using TMPro;
using UnityEngine;

public class CodeLock : MonoBehaviour
{

    [Header("Disks")]
    [SerializeField] private CodeLockDisk disk1;
    [SerializeField] private CodeLockDisk disk2;
    [SerializeField] private CodeLockDisk disk3;

    [Header("Hint")]
    [SerializeField] private TMP_Text combinationHint;

    [Header("Random solution length")]
    [SerializeField] private int minSteps = 4;
    [SerializeField] private int maxSteps = 8;

    private const int MIN_DISK_VALUE = 0;
    private const int MAX_DISK_VALUE = 10;

    private const int BUTTON_1 = 1;
    private const int BUTTON_2 = 2;
    private const int BUTTON_3 = 3;
    private const int BUTTON_1_DISK_1_CHANGE = 1;
    private const int BUTTON_1_DISK_2_CHANGE = -1;
    private const int BUTTON_1_DISK_3_CHANGE = 2;
    private const int BUTTON_2_DISK_1_CHANGE = 1;
    private const int BUTTON_2_DISK_2_CHANGE = 0;
    private const int BUTTON_2_DISK_3_CHANGE = -2;
    private const int BUTTON_3_DISK_1_CHANGE = 0;
    private const int BUTTON_3_DISK_2_CHANGE = 1;
    private const int BUTTON_3_DISK_3_CHANGE = -1;



    private int startValue1;
    private int startValue2;
    private int startValue3;

    private int correctValue1;
    private int correctValue2;
    private int correctValue3;

    private bool isProcessing = false;

    private void Start()
    {
        ResetLock();
    }

    public void ResetLock()
    {
         StopAllCoroutines();

        isProcessing = false;

        GenerateRandomStartCombination();

        GenerateReachableCombination();

        disk1.ResetDisk(startValue1);
        disk2.ResetDisk(startValue2);
        disk3.ResetDisk(startValue3);

        UpdateHint();

    }

    private void GenerateRandomStartCombination()
    {
        startValue1 = Random.Range(MIN_DISK_VALUE, MAX_DISK_VALUE);
        startValue2 = Random.Range(MIN_DISK_VALUE, MAX_DISK_VALUE);
        startValue3 = Random.Range(MIN_DISK_VALUE, MAX_DISK_VALUE);
    }

    private void GenerateReachableCombination()
    {
        do
        {
            correctValue1 = startValue1;
            correctValue2 = startValue2;
            correctValue3 = startValue3;

            int steps = Random.Range(minSteps, maxSteps + 1);

            for (int i = 0; i < steps; i++)
            {
                int randomButton = Random.Range(BUTTON_1, BUTTON_3 + 1);

                ApplyButtonToCombination(randomButton);
            }
        }
        while (IsStartCombination());
    }

    private void ApplyButtonToCombination(int buttonNumber)
    {
        switch (buttonNumber)
        {
            case 1:

                correctValue1 = NormalizeValue(correctValue1 + BUTTON_1_DISK_1_CHANGE);
                correctValue2 = NormalizeValue(correctValue2 + BUTTON_1_DISK_2_CHANGE);
                correctValue3 = NormalizeValue(correctValue3 + BUTTON_1_DISK_3_CHANGE);
                break;

            case 2:

                correctValue1 = NormalizeValue(correctValue1 + BUTTON_2_DISK_1_CHANGE);
                correctValue2 = NormalizeValue(correctValue2 + BUTTON_2_DISK_2_CHANGE);
                correctValue3 = NormalizeValue(correctValue3 + BUTTON_2_DISK_3_CHANGE);
                break;

            case 3:

                correctValue1 = NormalizeValue(correctValue1 + BUTTON_3_DISK_1_CHANGE);
                correctValue2 = NormalizeValue(correctValue2 + BUTTON_3_DISK_2_CHANGE);
                correctValue3 = NormalizeValue(correctValue3 + BUTTON_3_DISK_3_CHANGE);
                break;
        }
    }

    private void ApplyButtonToDisks(int buttonNumber)
    {
        switch (buttonNumber)
        {
            case BUTTON_1:
                disk1.ChangeValue(BUTTON_1_DISK_1_CHANGE);
                disk2.ChangeValue(BUTTON_1_DISK_2_CHANGE);
                disk3.ChangeValue(BUTTON_1_DISK_3_CHANGE);
                break;

            case BUTTON_2:
                disk1.ChangeValue(BUTTON_2_DISK_1_CHANGE);
                disk2.ChangeValue(BUTTON_2_DISK_2_CHANGE);
                disk3.ChangeValue(BUTTON_2_DISK_3_CHANGE);
                break;

            case BUTTON_3:
                disk1.ChangeValue(BUTTON_3_DISK_1_CHANGE);
                disk2.ChangeValue(BUTTON_3_DISK_2_CHANGE);
                disk3.ChangeValue(BUTTON_3_DISK_3_CHANGE);
                break;
        }
    }

    public void PressButton1()
    {
        PressButton(BUTTON_1);
    }

    public void PressButton2()
    {
        PressButton(BUTTON_2);
    }

    public void PressButton3()
    {
        PressButton(BUTTON_3);
    }

    private void PressButton(int buttonNumber)
    {
        if (isProcessing)
        {
            return;
        }

        ApplyButtonToDisks(buttonNumber);
        StartCoroutine(CheckCombinationAfterAnimation());
    }

    private IEnumerator CheckCombinationAfterAnimation()
    {
        isProcessing = true;

        yield return new WaitUntil(() =>
            !disk1.IsAnimating &&
            !disk2.IsAnimating &&
            !disk3.IsAnimating
        );

        CheckCombination();

        isProcessing = false;
    }

    private void CheckCombination()
    {
        if (disk1.CurrentValue == correctValue1 && disk2.CurrentValue == correctValue2 && disk3.CurrentValue == correctValue3)
        {
            GameManager.Instance.WinGame();
        }
    }

    private bool IsStartCombination()
    {
        return correctValue1 == startValue1 && correctValue2 == startValue2 && correctValue3 == startValue3;
    }

    private int NormalizeValue(int value)
    {
        return (value + MAX_DISK_VALUE) % MAX_DISK_VALUE;
    }

    private void UpdateHint()
    {
        if (combinationHint != null)
        {
            combinationHint.text =
                "Код замка: " +
                correctValue1 + " — " +
                correctValue2 + " — " +
                correctValue3;
        }
    }
}
