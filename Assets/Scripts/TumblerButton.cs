using UnityEngine;
using TMPro;

public class TumblerButton : MonoBehaviour
{
    [Header("Setup")]
    [Tooltip("Which tumbler index is this? (0, 1, 2, or 3)")]
    public int tumblerIndex;

    [Tooltip("Reference to the Lock Manager")]
    public CombinationLockManager lockManager;

    [Tooltip("Reference to the TextMeshPro component displaying the number")]
    public TextMeshPro textDisplay;

    void Start()
    {
        if (lockManager == null || textDisplay == null)
        {
            Debug.LogError("TumblerButton requires Lock Manager and Text Display assigned in the Inspector.");
            enabled = false;
        }

        
        textDisplay.text = lockManager.currentCode[tumblerIndex].ToString();
    }

    
    public void PressUp()
    {
        lockManager.UpdateTumbler(tumblerIndex, 1);
        UpdateDisplay();
    }

    
    public void PressDown()
    {
        lockManager.UpdateTumbler(tumblerIndex, -1);
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        
        textDisplay.text = lockManager.currentCode[tumblerIndex].ToString();
    }
}
