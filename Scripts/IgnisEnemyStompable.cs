using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class IgnisEnemyStompable : MonoBehaviour
{
    [Header("Rebote y Stomp (Ignis)")]
    [Tooltip("Impulso vertical que recibe el jugador al rebotar sobre el enemigo")]
    public float bounceForce = 10f;
    [Tooltip("Margen vertical extra para considerar stomp (en unidades Unity)")]
    public float stompOffset = 0.1f;

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Sólo nos interesa colisión con el jugador
        if (!collision.collider.CompareTag("Player"))
            return;

        // Obtenemos el Rigidbody2D del jugador
        var playerRb = collision.collider.GetComponent<Rigidbody2D>();
        if (playerRb == null)
            return;

        // Calculamos el punto de contacto y el 'tope' del enemigo
        ContactPoint2D contact = collision.GetContact(0);
        float contactY = contact.point.y;
        float enemyTop = transform.position.y + stompOffset;

        Debug.Log($"[IgnisStomp] velY={playerRb.linearVelocity.y:F2}, contactoY={contactY:F2}, enemyTop={enemyTop:F2}");

        // Si el jugador cae (velY < 0) y pisa por encima del tope → stomp
        if (playerRb.linearVelocity.y < 0f && contactY > enemyTop)
        {
            // Rebote del jugador
            playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, bounceForce);
            Debug.Log("[IgnisStomp] STOMP! enemigo eliminado");
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("[IgnisStomp] Golpe lateral/abajo: jugador muere");
            var hud = FindObjectOfType<HUDManager>();
            if (hud != null)
                hud.LoseLifeAndRestart();
        }
    }
}
