using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Unity.VisualScripting;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance { get; private set; }
    [SerializeField] private float speed = 4f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject diePrefab;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject torchGameObject;
    private Vector3 moveDirection;
    [SerializeField] private AudioSource dieSound;
    private bool facingRight = true; // Track which direction player is facing

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

        // Track facing direction for torch (blend tree handles visual flip)
        if (inputX > 0)
        {
            facingRight = true;
        }
        else if (inputX < 0)
        {
            facingRight = false;
        }

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
        dieSound.Play();

        speed = 0f;
        
        // Spawn death effect
        if (diePrefab != null)
        {
            Instantiate(diePrefab, transform.position, transform.rotation);
            spriteRenderer.enabled = false; // Hide player sprite
            torchGameObject.SetActive(false); // Hide torch GameObject
        }
        
        //Wait for 1 second
        yield return new WaitForSeconds(1f);
        
        //Reload current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public bool IsFacingRight()
    {
        return facingRight;
    }
}
