using UnityEngine;
using UnityEngine.Events;

public class CombinationLockManager : MonoBehaviour
{
    [Header("Lock Settings")]
    [Tooltip("The correct four-digit code (e.g., 1234)")]
    public int[] correctCode = new int[4];

    [Tooltip("The current numbers displayed on the tumblers")]
    public int[] currentCode = new int[4];

    [Header("Events")]
    public UnityEvent onLockOpened;
    public UnityEvent onIncorrectAttempt;

    // Call this from the tumbler buttons to update the code
    public void UpdateTumbler(int tumblerIndex, int direction)
    {
        // tumblerIndex: 0, 1, 2, or 3 (for the four tumblers)
        // direction: +1 for Up, -1 for Down

        // 1. Update the number based on the direction
        currentCode[tumblerIndex] += direction;

        // 2. Handle wrapping (0-9 range)
        if (currentCode[tumblerIndex] > 9)
        {
            currentCode[tumblerIndex] = 0;
        }
        else if (currentCode[tumblerIndex] < 0)
        {
            currentCode[tumblerIndex] = 9;
        }

        // 3. Optional: Trigger a visual update on the tumbler (e.g., update TextMeshPro)
        // You'll need a way to reference the actual tumbler object here.
        // For now, we'll focus on the logic.

        // 4. Check the combination after every change
        CheckCombination();
    }

    private void CheckCombination()
    {
        for (int i = 0; i < 4; i++)
        {
            // If any number is wrong, the combination is incorrect
            if (currentCode[i] != correctCode[i])
            {
                // If it was previously correct, but now it's wrong, we might fire an event here.
                // For simplicity, we just stop checking.
                return;
            }
        }

        // If the loop completes, all four digits match!
        onLockOpened.Invoke();
    }
}
