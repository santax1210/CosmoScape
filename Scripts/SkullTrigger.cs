using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class SkullTrigger : MonoBehaviour
{
    [Header("Salto de la calavera")]
    public float jumpForce = 8f;

    [Header("Configuración de pilares")]
    [Tooltip("Para cada pilar, indica qué GameObject es y el delay antes de soltarlo")]
    public PillarInfo[] pillarsToDrop;

    [Header("Tag del jugador")]
    public string playerTag = "Player";

    Rigidbody2D rb;
    Collider2D col;

    [System.Serializable]
    public struct PillarInfo
    {
        public GameObject pillar;   // El pilar a soltar
        public float   delay;       // Retraso (en segundos) tras el trigger
    }

    void Awake()
    {
        rb  = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        // Un solo collider en modo trigger para detectar al jugador
        col.isTrigger    = true;
        // Arrancamos como kinematic para que no quede flotando
        rb.bodyType      = RigidbodyType2D.Kinematic;
        rb.gravityScale  = 0f;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!col.enabled) return;
        if (other.CompareTag(playerTag))
        {
            // 1) La calavera salta y luego cae
            rb.bodyType     = RigidbodyType2D.Dynamic;
            rb.gravityScale = 1f;
            rb.linearVelocity     = Vector2.up * jumpForce;

            // 2) Desactivamos el trigger para no volver a dispararlo
            col.enabled     = false;

            // 3) Para cada pilar, lanza su corrutina con su propio delay
            foreach (var info in pillarsToDrop)
            {
                if (info.pillar != null)
                    StartCoroutine(DropPillarWithDelay(info.pillar, info.delay));
            }
        }
    }

    private IEnumerator DropPillarWithDelay(GameObject pillar, float delay)
{
    yield return new WaitForSeconds(delay);

    var prb = pillar.GetComponent<Rigidbody2D>();
    if (prb != null)
    {
        // 1) Activamos la física
        prb.bodyType     = RigidbodyType2D.Dynamic;
        // 2) Ajustamos la velocidad de caída:
        prb.gravityScale = 1.5f;   // <— prueba valores entre 0 (casi flota) y 1 (gravedad normal)
        // 3) (Opcional) Arrastrón contra el aire:
        prb.linearDamping         = 1f;     // <— mayor drag = más frenado al caer
    }
}
}
