using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchPuzzleManager : MonoBehaviour
{
    public List<Torch> torchesInOrder;
    public GameObject chest;
    private int currentIndex = 0;
    private bool puzzleCompleted = false;

    private void Awake()
    {
        chest.SetActive(false); // Baþta kapalý
        foreach (var torch in torchesInOrder)
        {
            torch.AssignManager(this);
        }
    }

    public void TorchActivated(Torch torch)
    {
        if (puzzleCompleted) return;

        if (torch == torchesInOrder[currentIndex])
        {
            torch.SetLit(true);
            currentIndex++;

            if (currentIndex >= torchesInOrder.Count)
            {
                puzzleCompleted = true;
                chest.SetActive(true);
                FloatingTextSpawner.Instance.ShowMessage("Torch Puzzle Completed!", Color.yellow);
            }
        }
        else
        {
            torch.SetLit(true);
            StartCoroutine(ResetPuzzle());
        }
    }

    private IEnumerator ResetPuzzle()
    {
        yield return new WaitForSeconds(0.5f);
        foreach (Torch torch in torchesInOrder)
        {
            torch.SetLit(false);
        }
        currentIndex = 0;
    }
}
