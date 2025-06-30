using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovingPlatform : MonoBehaviour
{
    public enum Axis { Horizontal, Vertical }

    [Header("Movimiento")]
    public Axis moveAxis = Axis.Horizontal;

    [Tooltip("Distancia total (hacia cada extremo) desde la posición inicial")]
    public float distance = 3f;

    [Tooltip("Velocidad de desplazamiento")]
    public float speed = 2f;

    [Tooltip("¿Comenzar moviéndose en la dirección positiva?")]
    public bool startPositiveDirection = true;

    private Rigidbody2D _rb;
    private Vector2 startPos;
    private Vector2 posPositive;
    private Vector2 posNegative;
    private bool movingPositive;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.bodyType = RigidbodyType2D.Kinematic;
        _rb.simulated = true;
        movingPositive = startPositiveDirection;
    }

    void Start()
    {
        startPos = transform.position;

        if (moveAxis == Axis.Horizontal)
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
        Vector2 current = _rb.position;
        Vector2 target = movingPositive ? posPositive : posNegative;

        Vector2 next = Vector2.MoveTowards(current, target, speed * Time.fixedDeltaTime);
        _rb.MovePosition(next);

        if (Vector2.Distance(next, target) < 0.01f)
            movingPositive = !movingPositive;
    }

    void OnDrawGizmosSelected()
    {
        // Dibuja la trayectoria en la Scene View
        Vector2 basePos = Application.isPlaying ? startPos : (Vector2)transform.position;
        Vector2 pPos = moveAxis == Axis.Horizontal
            ? basePos + Vector2.right * distance
            : basePos + Vector2.up * distance;
        Vector2 pNeg = moveAxis == Axis.Horizontal
            ? basePos - Vector2.right * distance
            : basePos - Vector2.up * distance;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(pNeg, pPos);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(pNeg, 0.2f);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(pPos, 0.2f);
    }
}
