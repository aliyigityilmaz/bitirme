using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatableStone : Interactable
{
    [SerializeField] private List<RotatableStone> linkedStones;
    private int currentRotationIndex = 0;
    private static readonly Vector3[] directions = { Vector3.forward, Vector3.right, Vector3.back, Vector3.left };
    private RotatableStonePuzzleManager manager;
    private bool isRotating = false;
    private float rotationDuration = 0.4f;

    public bool IsRotating => isRotating;

    private void Start()
    {
        currentRotationIndex = GetClosestDirectionIndex(transform.forward);
    }

    private int GetClosestDirectionIndex(Vector3 forward)
    {
        int bestIndex = 0;
        float bestDot = -1f;

        for (int i = 0; i < directions.Length; i++)
        {
            float dot = Vector3.Dot(forward.normalized, directions[i]);
            if (dot > bestDot)
            {
                bestDot = dot;
                bestIndex = i;
            }
        }

        return bestIndex;
    }


    public void AssignManager(RotatableStonePuzzleManager mgr)
    {
        manager = mgr;
    }

    public override void Interact()
    {
        if (manager.puzzleCompleted || isRotating) return;

        StartCoroutine(RotateSmooth(true));
    }

    private IEnumerator RotateSmooth(bool triggeredByInteraction = true)
    {
        isRotating = true;

        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, 90f, 0));

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / rotationDuration;
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, t);
            yield return null;
        }

        currentRotationIndex = (currentRotationIndex + 1) % directions.Length;
        transform.forward = directions[currentRotationIndex]; // Snap yön

        // Baðlý taþlarý da döndür (ama zincirleme olduðunu belirterek)
        foreach (var linked in linkedStones)
        {
            if (!linked.isRotating)
                linked.StartCoroutine(linked.RotateSmooth(false));
        }

        isRotating = false;

        if (triggeredByInteraction)
            manager.CheckPuzzle();
    }

    private void ApplyRotationImmediate()
    {
        transform.forward = directions[currentRotationIndex];
    }

    public Vector3 GetFacingDirection()
    {
        return directions[currentRotationIndex];
    }
}
