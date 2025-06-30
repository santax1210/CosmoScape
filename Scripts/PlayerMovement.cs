using System.Collections;           // Necesario para IEnumerator
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 5f;

    [Header("Salto")]
    public float jumpForce = 10f;
    public float doubleJumpForce = 15f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float checkRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("Power-up UI (World Space)")]
    public GameObject doubleJumpLabel;   // Asigna aquí tu GameObject “DoubleJumpLabel”
    public float labelDuration = 2f;     // Segundos que se muestra

    // Estado de doble salto
    private bool doubleJumpEnabled = false;
    private bool hasDoubleJumped   = false;
    private float doubleJumpTimer  = 0f;
    private float doubleJumpDuration = 0f;

    private Rigidbody2D rb;
    private bool isGrounded;
    private Coroutine labelRoutine;      // Para manejar la corrutina del texto

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Asegúrate de que el label esté oculto al inicio
        if (doubleJumpLabel != null)
            doubleJumpLabel.SetActive(false);
    }

    void Update()
    {
        // 1) Caducar power-up si tiene duración limitada
        if (doubleJumpEnabled && doubleJumpDuration > 0f)
        {
            doubleJumpTimer += Time.deltaTime;
            if (doubleJumpTimer >= doubleJumpDuration)
                doubleJumpEnabled = false;
        }

        // 2) Movimiento horizontal
        float moveX = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveX * moveSpeed, rb.linearVelocity.y);

        // 3) Comprobar suelo
        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            checkRadius,
            groundLayer
        );
        if (isGrounded)
            hasDoubleJumped = false;

        // 4) Salto y doble salto
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                // Salto normal
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            }
            else if (doubleJumpEnabled && !hasDoubleJumped)
            {
                // Salto en el aire
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
                rb.AddForce(Vector2.up * doubleJumpForce, ForceMode2D.Impulse);
                hasDoubleJumped = true;
            }
        }
    }

    /// <summary>
    /// Activa el doble salto y muestra el label al lado del jugador.
    /// </summary>
    public void EnableDoubleJump(float duration)
    {
        Debug.Log("Recolectó power-up de doble salto. Duración: " + duration + "s");
        doubleJumpEnabled   = true;
        hasDoubleJumped     = false;
        doubleJumpTimer     = 0f;
        doubleJumpDuration  = duration;

        // Muestra el texto en World Space
        if (doubleJumpLabel != null)
        {
            if (labelRoutine != null)
                StopCoroutine(labelRoutine);
            labelRoutine = StartCoroutine(ShowLabel());
        }
    }

    private IEnumerator ShowLabel()
    {
        Debug.Log("Mostrando label de doble salto");
        doubleJumpLabel.SetActive(true);
        yield return new WaitForSeconds(labelDuration);
        Debug.Log("Ocultando label de doble salto");
        doubleJumpLabel.SetActive(false);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (collision.contacts[0].normal.y > 0.5f)
            {
                Destroy(collision.gameObject);
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce * 0.75f);
            }
            else
            {
                FindObjectOfType<HUDManager>().LoseLifeAndRestart();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("JumpZone"))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce * 1.5f);
        }

        if (other.CompareTag("DeadZone"))
        {
            FindObjectOfType<HUDManager>().LoseLifeAndRestart();
        }

        if (other.CompareTag("DoubleJump"))
        {
            EnableDoubleJump(
                other.GetComponent<DoubleJump>()?.duration ?? 0f
            );
            Destroy(other.gameObject);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        }
    }
}
