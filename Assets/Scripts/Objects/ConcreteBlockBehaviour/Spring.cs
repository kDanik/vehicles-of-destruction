using UnityEngine;

public class Spring : MonoBehaviour
{
    [SerializeField]
    private GameObject leftSpring;

    [SerializeField]
    private GameObject rightSpring;

    [SerializeField]
    private GameObject middle;

    private BoxCollider2D springParentCollider;

    void Awake()
    {
        springParentCollider = GetComponent<BoxCollider2D>();


        Physics2D.IgnoreCollision(leftSpring.GetComponent<BoxCollider2D>(), springParentCollider);
        Physics2D.IgnoreCollision(rightSpring.GetComponent<BoxCollider2D>(), springParentCollider);
        Physics2D.IgnoreCollision(middle.GetComponent<BoxCollider2D>(), springParentCollider);
    }

    void Update()
    {
        rightSpring.transform.localPosition = new Vector3(rightSpring.transform.localPosition.x, 0);
        rightSpring.transform.localRotation = Quaternion.identity;
        leftSpring.transform.localPosition = new Vector3(leftSpring.transform.localPosition.x, 0);
        leftSpring.transform.localRotation = Quaternion.identity;

        middle.transform.localPosition = new Vector3(0, 0);
        middle.transform.localRotation = Quaternion.identity;

        UpdateMainColliderSize();
    }

    private void UpdateMainColliderSize()
    {
        float distanceBetweenSpringSides = Mathf.Abs(leftSpring.transform.localPosition.x - rightSpring.transform.localPosition.x);

        springParentCollider.size = new Vector2(0.1f + distanceBetweenSpringSides, springParentCollider.size.y);
    }
}
