using System.Collections.Generic;
using UnityEngine;

public class RotatableStonePuzzleManager : MonoBehaviour
{
    public List<RotatableStone> stones;
    public GameObject chest; // prefab deðil, sahnedeki aktif olmayan objeyi referansla
    public bool puzzleCompleted = false;

    private void Awake()
    {
        chest.SetActive(false); // Baþta kapalý
        foreach (var stone in stones)
        {
            stone.AssignManager(this);
        }
    }

    public void CheckPuzzle()
    {
        if (puzzleCompleted) return;

        foreach (var stone in stones)
        {
            if (stone.IsRotating)
                return;
        }

        int firstIndex = stones[0].GetRotationIndex();
        foreach (var stone in stones)
        {
            if (stone.GetRotationIndex() != firstIndex)
                return;
        }

        puzzleCompleted = true;
        chest.SetActive(true);
        FloatingTextSpawner.Instance.ShowMessage("Rotation Puzzle Completed!", Color.cyan);
    }

}
