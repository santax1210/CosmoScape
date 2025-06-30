using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EnemyStompable : MonoBehaviour
{
    [Header("Rebote y Stomp")]
    public float bounceForce = 10f;
    public float stompOffset = 0.1f;

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Solo nos interesa colisión contra el jugador
        if (!collision.collider.CompareTag("Player")) return;

        // Recogemos el Rigidbody2D del jugador
        var playerRb = collision.collider.GetComponent<Rigidbody2D>();
        if (playerRb == null) return;

        // Punto de contacto
        ContactPoint2D contact = collision.GetContact(0);
        float contactY = contact.point.y;
        float enemyTop = transform.position.y + stompOffset;

        Debug.Log($"Jugador velY={playerRb.linearVelocity.y:F2}, contactoY={contactY:F2}, enemyTop={enemyTop:F2}");

        // Si viene desde arriba (velocidad negativa) y contacta por encima del tope
        if (playerRb.linearVelocity.y < 0f && contactY > enemyTop)
        {
            // Rebote del jugador
            playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, bounceForce);
            Debug.Log("STOMP: enemigo eliminado");
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("COLLISION: jugador muere");
            var hud = FindObjectOfType<HUDManager>();
            if (hud != null)
                hud.LoseLifeAndRestart();
        }
    }
}










