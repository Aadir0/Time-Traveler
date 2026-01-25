using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance { get; private set; }
    [SerializeField] private float speed = 4f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject diePrefab;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private Vector3 moveDirection;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    void Update()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector3(inputX, inputY).normalized;

        anim.SetFloat("MoveX", inputX);
        anim.SetFloat("MoveY", inputY);

        if (moveDirection == Vector3.zero)
        {
            anim.SetBool("moving", false);
        }
        else
        {
            anim.SetBool("moving", true);
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector3(moveDirection.x * speed, moveDirection.y * speed);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            StartCoroutine(Die());
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            StartCoroutine(Die());
        }
    }

    private IEnumerator Die()
    {
        //Disable player movement
        speed = 0f;
        
        // Spawn death effect
        if (diePrefab != null)
        {
            Instantiate(diePrefab, transform.position, transform.rotation);
            spriteRenderer.enabled = false; // Hide player sprite
        }
        
        //Wait for 1 second
        yield return new WaitForSeconds(1f);
        
        //Reload current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
