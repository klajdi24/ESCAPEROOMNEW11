using UnityEngine;
using TMPro;

public class GazeLockTumbler : MonoBehaviour
{
    [Tooltip("The text display for this single digit")]
    public TMP_Text digitDisplay;

    [Tooltip("The 'manager' script for the whole lock")]
    public CombinationLock lockManager;

    private int currentValue = 0;

    private void Start()
    {
        UpdateText();
    }

    
    public void Increment()
    {
        currentValue++;
        if (currentValue > 9)
            currentValue = 0;

        UpdateText();
        lockManager?.CheckCode(); 
    }

    
    public void Decrement()
    {
        currentValue--;
        if (currentValue < 0)
            currentValue = 9;

        UpdateText();
        lockManager?.CheckCode(); 
    }

    public int GetValue()
    {
        return currentValue;
    }

    private void UpdateText()
    {
        if (digitDisplay != null)
            digitDisplay.text = currentValue.ToString();
    }
}
