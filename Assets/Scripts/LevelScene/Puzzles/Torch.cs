using UnityEngine;

public class Torch : Interactable
{
    private TorchPuzzleManager puzzleManager;
    private Renderer rend;
    private bool isLit = false;

    private void Awake()
    {
        rend = GetComponent<Renderer>();
    }

    public void AssignManager(TorchPuzzleManager manager)
    {
        puzzleManager = manager;
    }

    public override void Interact()
    {
        if (!isLit)
        {
            puzzleManager.TorchActivated(this);
        }
    }

    public void SetLit(bool lit)
    {
        isLit = lit;
        rend.material.color = lit ? Color.yellow : Color.white;
    }
}
