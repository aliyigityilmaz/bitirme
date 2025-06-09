using UnityEngine;

public class PuzzleZone : MonoBehaviour
{
    public PressurePlatePuzzleManager puzzleManager;

    private void Start()
    {
        puzzleManager = GetComponentInParent<PressurePlatePuzzleManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            puzzleManager.SteppedOnInvalidSurface();
        }
    }
}

