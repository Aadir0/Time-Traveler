using UnityEngine;

public class StepPushBox : MonoBehaviour
{
    static StepPushBox activeBox;   // Global push owner

    public float pushSpeed = 2.5f;
    public float acceleration = 20f;
    public float stopDamping = 30f;
    public float minContactTime = 0.3f;
    Rigidbody2D rb;
    float contactTimer;
    bool beingPushed;
    Vector2 pushDir;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 0;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.linearDamping = 20f;
    }

    void OnCollisionStay2D(Collision2D col)
    {
        if (!col.gameObject.CompareTag("Player")) return;

        // Someone else already owns the push
        if (activeBox != null && activeBox != this) return;

        Vector2 dirToBox = (rb.position - col.rigidbody.position).normalized;
        Vector2 playerMoveDir = col.rigidbody.linearVelocity.normalized;

        if (Vector2.Dot(playerMoveDir, dirToBox) > 0.3f)
        {
            contactTimer += Time.deltaTime;
            pushDir = SnapToCardinal(dirToBox);

            if (contactTimer >= minContactTime && activeBox == null)
            {
                // Acquire exclusive ownership
                activeBox = this;
                beingPushed = true;
            }
        }
        else
        {
            contactTimer = 0f;
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (!col.gameObject.CompareTag("Player")) return;

        contactTimer = 0f;
        beingPushed = false;

        if (activeBox == this)
            activeBox = null;   // Release ownership
    }

    void FixedUpdate()
    {
        if (beingPushed && activeBox == this)
        {
            Vector2 targetVel = pushDir * pushSpeed;
            rb.linearVelocity = Vector2.MoveTowards(rb.linearVelocity, targetVel, acceleration * Time.fixedDeltaTime);
        }
        else
        {
            rb.linearVelocity = Vector2.MoveTowards(rb.linearVelocity, Vector2.zero, stopDamping * Time.fixedDeltaTime);
        }
    }

    Vector2 SnapToCardinal(Vector2 dir)
    {
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            return new Vector2(Mathf.Sign(dir.x), 0);
        else
            return new Vector2(0, Mathf.Sign(dir.y));
    }
}
