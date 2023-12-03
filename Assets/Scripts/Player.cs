using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float SpeedFactor = 5;
    [SerializeField] private float JumpForce;
    [SerializeField] private LayerMask GroundLayers;
    [SerializeField] private Boss Boss;

    private Rigidbody2D Rb;
    private float Depth;
    private bool IsGrounded;
    private Vector3 NewPos;
    private bool inFight;

    void Start()
    {
        ImputManager.Init(this);
        ImputManager.GameMode();

        Rb = GetComponent<Rigidbody2D>();
        Depth = GetComponent<Collider2D>().bounds.size.y;

        NewPos = Vector3.right / SpeedFactor;
    }
    void FixedUpdate()
    {
        if (!inFight)
        {
            transform.position += NewPos;
        }
        CheckGrounded();
    }
    public void Jump()
    {
        if (IsGrounded)
        {
            Rb.AddForce(Vector3.up * JumpForce, ForceMode2D.Impulse);
        }
    }
    public void Fly()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Rb.gravityScale = 0;
            Rb.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
        }
        else
        {
            Rb.gravityScale = 1;
        }
    }

    private void CheckGrounded()
    {
        IsGrounded = Physics2D.Raycast(transform.position, Vector3.down, Depth, GroundLayers);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Enter Fight"))
        {
            inFight = !inFight;
            Boss.inBossFight = !Boss.inBossFight;
            ImputManager.inFight = !ImputManager.inFight;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Side Block"))
        {
            DetachChild();
            Destroy(gameObject);
        }
    }
    public void DetachChild()
    {
        if (transform.childCount > 0)
        {
            // Create a temporary list to store children
            Transform[] children = new Transform[transform.childCount];
            int i = 0;

            // Store children in the list
            foreach (Transform child in transform)
            {
                children[i] = child;
                i++;
            }

            // Detach children from the parent
            foreach (Transform child in children)
            {
                child.parent = null;
            }
        }
    }
}
