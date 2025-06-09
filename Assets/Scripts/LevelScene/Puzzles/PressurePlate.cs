using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    private PressurePlatePuzzleManager puzzleManager;

    [SerializeField]    
    private bool playerOnTop = false;

    public void AssignManager(PressurePlatePuzzleManager manager)
    {
        puzzleManager = manager;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !playerOnTop)
        {
            playerOnTop = true;
            puzzleManager.PlateSteppedOn(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerOnTop = false;
    }

    public void ChangeColor(Color color)
    {
        GetComponent<Renderer>().material.color = color;
    }
}
