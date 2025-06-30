using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FlyingEnemyPatrol : MonoBehaviour
{
    public enum Axis { Horizontal, Vertical }

    [Header("Patrol Settings")]
    [Tooltip("Eje en el que patrulla el enemigo")]
    public Axis patrolAxis = Axis.Horizontal;

    [Tooltip("Distancia desde su posición inicial hacia cada extremo")]
    public float distance = 3f;

    [Tooltip("Velocidad de patrulla")]
    public float speed = 2f;

    [Tooltip("¿Comenzar moviéndose en dirección positiva (derecha/arriba)?")]
    public bool startPositive = true;

    // Posiciones objetivo
    private Vector2 startPos;
    private Vector2 posPositive;
    private Vector2 posNegative;

    // Rigidbody para moverlo
    private Rigidbody2D rb;
    private bool movingPositive;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic; // para evitar física estándar
        movingPositive = startPositive;
    }

    void Start()
    {
        startPos = rb.position;

        if (patrolAxis == Axis.Horizontal)
        {
            posPositive = startPos + Vector2.right * distance;
            posNegative = startPos - Vector2.right * distance;
        }
        else // Vertical
        {
            posPositive = startPos + Vector2.up * distance;
            posNegative = startPos - Vector2.up * distance;
        }
    }

    void FixedUpdate()
    {
        Vector2 current = rb.position;
        Vector2 target  = movingPositive ? posPositive : posNegative;

        // Mueve suavemente hacia el punto target
        Vector2 next = Vector2.MoveTowards(current, target, speed * Time.fixedDeltaTime);
        rb.MovePosition(next);

        // Al alcanzar el target, invierte dirección
        if (Vector2.Distance(next, target) < 0.01f)
            movingPositive = !movingPositive;
    }

    void OnDrawGizmosSelected()
    {
        // Dibuja en la escena la línea de patrulla y los extremos
        Vector2 basePos = Application.isPlaying ? startPos : (Vector2)transform.position;
        Vector2 pPos = (patrolAxis == Axis.Horizontal)
            ? basePos + Vector2.right * distance
            : basePos + Vector2.up * distance;
        Vector2 pNeg = (patrolAxis == Axis.Horizontal)
            ? basePos - Vector2.right * distance
            : basePos - Vector2.up * distance;

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(pNeg, pPos);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(pNeg, 0.2f);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(pPos, 0.2f);
    }
}
