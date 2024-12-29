using UnityEngine;

public class ClickHandler : MonoBehaviour
{
    private Anchor parentAnchor;
    public LayerMask clickLayer;

    void Start()
    {
        parentAnchor = GetComponentInParent<Anchor>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hitCollider = Physics2D.OverlapPoint(mousePosition, clickLayer);

            if (hitCollider != null)
            {
                if (hitCollider.gameObject == gameObject)
                {
                    parentAnchor?.ReleaseAllHinges();
                    Destroy(gameObject);
                }
            }
        }
    }
}
