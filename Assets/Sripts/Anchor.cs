using UnityEngine;
using System.Collections.Generic;

public class Anchor : MonoBehaviour
{
    private List<(GameObject block, string hingeName)> connectedBlocks = new List<(GameObject, string)>();
    private bool hingesReleased = false;

    public void AddConnectedBlock(GameObject block, string hingeName)
    {
        if (block == null)
        {
            Debug.LogWarning($"Attempted to add a null block to anchor: {gameObject.name}");
            return;
        }

        connectedBlocks.Add((block, hingeName));
        // Debug.Log($"Block {block.name} connected to anchor {gameObject.name} with hinge {hingeName}");
    }
    public void ReleaseAllHinges()
    {
        if (hingesReleased)
        {
            // Debug.Log($"Hinges already released for anchor: {gameObject.name}");
            return;
        }

        if (connectedBlocks.Count == 0)
        {
            // Debug.LogWarning($"No blocks connected to anchor: {gameObject.name}");
            return;
        }

        foreach (var connection in connectedBlocks)
        {
            if (connection.block == null)
            {
                Debug.LogWarning($"Null block detected in connections for anchor: {gameObject.name}");
                continue;
            }

            HingeController hingeController = connection.block.GetComponent<HingeController>();
            if (hingeController != null)
            {
                hingeController.DeactivateHinge(connection.hingeName);
            }
            else
            {
                Debug.LogWarning($"HingeController not found on block: {connection.block.name}");
            }
        }

        hingesReleased = true;
        Debug.Log($"Released all hinges for anchor: {gameObject.name}");

        // Notify GameCanvasManager of the anchor trigger
        NotifyAnchorTriggered();
    }
    private void NotifyAnchorTriggered()
    {
        GameCanvasManager canvasManager = FindObjectOfType<GameCanvasManager>();
        if (canvasManager != null)
        {
            canvasManager.OnAnchorTriggered();
        }
    }
    public void ResetAnchor()
    {
        hingesReleased = false;
        connectedBlocks.Clear();
        Debug.Log($"Anchor {gameObject.name} has been reset.");
    }
    public int GetConnectedBlockCount()
    {
        return connectedBlocks.Count;
    }
}
