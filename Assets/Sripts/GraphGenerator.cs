using UnityEngine;

public class GraphGenerator : MonoBehaviour
{
    public GameObject blockPrefab;
    public GameObject anchorPrefab;
    public Vector3 gridOffset = Vector3.zero;
    private int gridRows;
    private int gridColumns;

    public void GenerateGraphWithSize(int rows, int columns)
    {
        // Clear the previous grid (if any)
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        gridRows = rows;
        gridColumns = columns;
        float xOffset = gridColumns;
        float yOffset = -gridRows;

        gridOffset = new Vector3(-xOffset, -yOffset, 0);
        GameObject[,] anchors = new GameObject[gridRows + 1, gridColumns + 1];

        for (int i = 0; i <= gridRows; i++)
        {
            for (int j = 0; j <= gridColumns; j++)
            {
                Vector3 anchorPosition = new Vector3(j * 2, -i * 2, 0) + gridOffset; // Adjust spacing and apply offset
                anchors[i, j] = Instantiate(anchorPrefab, anchorPosition, Quaternion.identity, transform);
                anchors[i, j].name = $"Anchor ({i},{j})";
            }
        }

        int totalBlocks = gridRows * gridColumns * 2;
        int blockIndex = 0;

        for (int i = 0; i <= gridRows; i++)
        {
            for (int j = 0; j <= gridColumns; j++)
            {
                // Create horizontal block if not in the last column
                if (j < gridColumns)
                {
                    float colorFactor = (float)blockIndex / totalBlocks; // Calculate color gradient
                    CreateBlock(anchors[i, j], anchors[i, j + 1], colorFactor);
                    blockIndex++;
                }
                // Create vertical block if not in the last row
                if (i < gridRows)
                {
                    float colorFactor = (float)blockIndex / totalBlocks; // Calculate color gradient
                    CreateBlock(anchors[i, j], anchors[i + 1, j], colorFactor);
                    blockIndex++;
                }
            }
        }
    }

    void CreateBlock(GameObject anchorLeft, GameObject anchorRight, float colorFactor)
    {
        Vector3 blockPosition = (anchorLeft.transform.position + anchorRight.transform.position) / 2;
        GameObject block = Instantiate(blockPrefab, blockPosition, Quaternion.identity, transform);

        Vector3 direction = (anchorRight.transform.position - anchorLeft.transform.position).normalized;
        block.transform.right = direction;

        SpriteRenderer blockRenderer = block.GetComponent<SpriteRenderer>();
        if (blockRenderer != null)
        {
            Color baseColor = Color.blue;
            blockRenderer.color = Color.Lerp(baseColor * 0.5f, baseColor, colorFactor);
        }

        HingeJoint2D[] hinges = block.GetComponents<HingeJoint2D>();
        HingeJoint2D leftHinge = hinges[0];
        leftHinge.connectedBody = anchorLeft.GetComponent<Rigidbody2D>();

        HingeJoint2D rightHinge = hinges[1];
        rightHinge.connectedBody = anchorRight.GetComponent<Rigidbody2D>();

        Anchor leftAnchorScript = anchorLeft.GetComponent<Anchor>();
        Anchor rightAnchorScript = anchorRight.GetComponent<Anchor>();

        leftAnchorScript.AddConnectedBlock(block, "Left");
        rightAnchorScript.AddConnectedBlock(block, "Right");
    }

    public int GetTotalClickableAnchors()
    {
        return (gridRows + 1) * (gridColumns + 1);
    }
}