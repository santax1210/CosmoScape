using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class ShakingPlatform : MonoBehaviour
{
    [Header("Shake Settings")]
    [Tooltip("Duración total del temblor antes de caer (s)")]
    public float shakeDuration = 1f;
    [Tooltip("Magnitud máxima del temblor (unidades)")]
    public float shakeMagnitude = 0.1f;
    [Tooltip("Frecuencia de actualización del temblor (s)")]
    public float shakeInterval = 0.02f;

    [Header("Fall Settings")]
    [Tooltip("Delay tras el temblor antes de caer")]
    public float fallDelay = 0f;

    [Header("Tags")]
    [Tooltip("Tag del jugador que activa el temblor")]
    public string playerTag = "Player";

    private Rigidbody2D rb;
    private Collider2D col;
    private Vector3 originalPos;
    private bool triggered = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        // Para que no caiga hasta que termine el temblor
        rb.bodyType     = RigidbodyType2D.Kinematic;
        rb.gravityScale = 0f;
    }

    void Start()
    {
        originalPos = transform.position;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (triggered) return;
        if (collision.collider.CompareTag(playerTag))
        {
            triggered = true;
            // Deshabilita el collider si no quieres más colisiones
            // col.enabled = false;
            StartCoroutine(ShakeAndFall());
        }
    }

    private IEnumerator ShakeAndFall()
    {
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            elapsed += shakeInterval;
            // Aplica un pequeño offset aleatorio
            float offsetX = Random.Range(-shakeMagnitude, shakeMagnitude);
            float offsetY = Random.Range(-shakeMagnitude, shakeMagnitude);
            transform.position = originalPos + new Vector3(offsetX, offsetY, 0f);
            yield return new WaitForSeconds(shakeInterval);
        }

        // Vuelve a la posición original antes de caer
        transform.position = originalPos;
        // Pequeña pausa si se desea
        if (fallDelay > 0f)
            yield return new WaitForSeconds(fallDelay);

        // Activa la física para que caiga
        rb.bodyType     = RigidbodyType2D.Dynamic;
        rb.gravityScale = 1f;
    }

    void OnDrawGizmosSelected()
    {
        // Muestra el rango de temblor en la escena
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(shakeMagnitude * 2, shakeMagnitude * 2, 0));
    }
}
