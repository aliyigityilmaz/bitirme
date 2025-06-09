using System.Collections.Generic;
using UnityEngine;

public class PressurePlatePuzzleManager : MonoBehaviour
{
    public GameObject chest;
    private List<PressurePlate> pressurePlates = new List<PressurePlate>();
    private HashSet<PressurePlate> pressedPlates = new HashSet<PressurePlate>();
    private bool puzzleCompleted = false;

    public Color plateColor;

    private void Awake()
    {
        chest.SetActive(false); // Baþta kapalý
        pressurePlates.AddRange(GetComponentsInChildren<PressurePlate>());
        foreach (var plate in pressurePlates)
        {
            plate.AssignManager(this);
        }
    }

    public void PlateSteppedOn(PressurePlate plate)
    {
        if (puzzleCompleted || pressedPlates.Contains(plate))
            return;

        pressedPlates.Add(plate);
        plate.ChangeColor(plateColor);

        if (pressedPlates.Count == pressurePlates.Count)
        {
            puzzleCompleted = true;
            chest.SetActive(true);
            FloatingTextSpawner.Instance.ShowMessage("Puzzle Completed!", Color.white);
        }
    }

    public void SteppedOnInvalidSurface()
    {
        if (puzzleCompleted) return;
        ResetPuzzle();
    }

    public void ResetPuzzle()
    {
        foreach (var plate in pressedPlates)
            plate.ChangeColor(Color.white);

        pressedPlates.Clear();
    }
}
