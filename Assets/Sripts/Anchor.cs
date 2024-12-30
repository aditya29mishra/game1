using UnityEngine;
using System.Collections.Generic;

public class Anchor : MonoBehaviour
{
    private List<(GameObject block, string hingeName)> connectedBlocks = new List<(GameObject, string)>();
    private bool hingesReleased = false;

    public void AddConnectedBlock(GameObject block, string hingeName)
    {
        connectedBlocks.Add((block, hingeName));
    }
    public void ReleaseAllHinges()
    {
        foreach (var connection in connectedBlocks)
        {
            if (connection.block == null)
            {
                continue;
            }

            HingeController hingeController = connection.block.GetComponent<HingeController>();
            if (hingeController != null)
            {
                hingeController.DeactivateHinge(connection.hingeName);
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
    }
    public int GetConnectedBlockCount()
    {
        return connectedBlocks.Count;
    }
}
