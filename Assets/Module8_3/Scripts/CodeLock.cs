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
        startValue1 = Random.Range(0, 10);
        startValue2 = Random.Range(0, 10);
        startValue3 = Random.Range(0, 10);
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
                int randomButton = Random.Range(1, 4);

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
                            
                correctValue1 = NormalizeValue(correctValue1 + 1);
                correctValue2 = NormalizeValue(correctValue2 - 1);
                correctValue3 = NormalizeValue(correctValue3 + 2);
                break;

            case 2:

                correctValue1 = NormalizeValue(correctValue1 + 1);
                correctValue2 = NormalizeValue(correctValue2);
                correctValue3 = NormalizeValue(correctValue3 - 2);
                break;

            case 3:

                correctValue1 = NormalizeValue(correctValue1);
                correctValue2 = NormalizeValue(correctValue2 + 1);
                correctValue3 = NormalizeValue(correctValue3 - 1);
                break;
        }
    }

    private bool IsStartCombination()
    {
        return correctValue1 == startValue1 &&
               correctValue2 == startValue2 &&
               correctValue3 == startValue3;
    }

    private int NormalizeValue(int value)
    {
        return (value + 10) % 10;
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

    public void PressButton1()
    {
        if (isProcessing)
        {
            return;
        }

        disk1.ChangeValue(1);
        disk2.ChangeValue(-1);
        disk3.ChangeValue(2);

        StartCoroutine(CheckCombinationAfterAnimation());
    }

    public void PressButton2()
    {
        if (isProcessing) 
        { 
            return; 
        }
           
        disk1.ChangeValue(1);
        disk2.ChangeValue(0);
        disk3.ChangeValue(-2);

        StartCoroutine(CheckCombinationAfterAnimation());
    }

    public void PressButton3()
    {
        if (isProcessing)
        {
            return;
        }

        disk1.ChangeValue(0);
        disk2.ChangeValue(1);
        disk3.ChangeValue(-1);

        StartCoroutine(CheckCombinationAfterAnimation());
    }

    private IEnumerator CheckCombinationAfterAnimation()
    {
        isProcessing = true;

        while (disk1.IsAnimating || disk2.IsAnimating || disk3.IsAnimating)
        {
            yield return null;
        }

        CheckCombination();

        isProcessing = false;
    }

    private void CheckCombination()
    {
        if (disk1.CurrentValue == correctValue1 &&
            disk2.CurrentValue == correctValue2 &&
            disk3.CurrentValue == correctValue3)
        {
            GameManager.Instance.WinGame();
        }
    } 
}
