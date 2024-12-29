using UnityEngine;

public class HingeController : MonoBehaviour
{
    public HingeJoint2D hingeLeft;
    public HingeJoint2D hingeRight;
    public void DeactivateHinge(string hingeName)
    {
        switch (hingeName)
        {
            case "Left":
                DeactivateSpecificHinge(hingeLeft, "Left");
                break;

            case "Right":
                DeactivateSpecificHinge(hingeRight, "Right");
                break;

            default:
                break;
        }
    }
    private void DeactivateSpecificHinge(HingeJoint2D hinge, string hingeLabel)
    {
        if (hinge != null)
        {
            Destroy(hinge);
        }
    }

}
