using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class HorizontalFinishPlatform : MonoBehaviour
{
    [Header("Movimiento Horizontal")]
    public float leftDistance = 3f;    // Distancia hacia la izquierda desde la posición inicial
    public float rightDistance = 3f;   // Distancia hacia la derecha desde la posición inicial
    public float speed = 2f;           // Velocidad de movimiento
    public bool startMovingRight = true;  // Dirección inicial
    
    [Header("Finalización de Nivel")]
    public LevelFinish levelFinishScript;  // Referencia al script de finalización
    
    private Rigidbody2D _rb;
    private Vector2 startPosition;     // Posición inicial de la plataforma
    private Vector2 leftPoint;        // Punto calculado izquierdo
    private Vector2 rightPoint;       // Punto calculado derecho
    private bool movingRight = true;   // true = hacia derecha, false = hacia izquierda
    private bool levelCompleted = false;
    
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.bodyType = RigidbodyType2D.Kinematic;
        _rb.simulated = true;
        
        // Configurar dirección inicial
        movingRight = startMovingRight;
    }
    
    void Start()
    {
        // Guardar posición inicial y calcular puntos
        startPosition = transform.position;
        leftPoint = startPosition - Vector2.right * leftDistance;
        rightPoint = startPosition + Vector2.right * rightDistance;
        
        // Mostrar los puntos en la consola para debug
        Debug.Log($"Plataforma: Punto Izquierdo: {leftPoint}, Punto Derecho: {rightPoint}");
    }
    
    void FixedUpdate()
    {
        MoveHorizontally();
    }
    
    void MoveHorizontally()
    {
        Vector2 currentPos = _rb.position;
        Vector2 targetPos = movingRight ? rightPoint : leftPoint;
        
        // Mover hacia el objetivo
        Vector2 newPos = Vector2.MoveTowards(currentPos, targetPos, speed * Time.fixedDeltaTime);
        _rb.MovePosition(newPos);
        
        // Cambiar dirección al llegar al punto
        if (Vector2.Distance(newPos, targetPos) < 0.01f)
        {
            movingRight = !movingRight;
        }
    }
    
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.CompareTag("Player") && !levelCompleted)
        {
            FinishLevel();
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !levelCompleted)
        {
            FinishLevel();
        }
    }
    
    void FinishLevel()
    {
        levelCompleted = true;
        
        if (levelFinishScript != null)
        {
            levelFinishScript.CompleteLevel();
        }
        else
        {
            Debug.LogWarning("LevelFinish script no asignado en HorizontalFinishPlatform");
        }
    }
    
    // Visualizar los puntos en el editor
    void OnDrawGizmos()
    {
        Vector2 center = Application.isPlaying ? startPosition : (Vector2)transform.position;
        Vector2 left = center - Vector2.right * leftDistance;
        Vector2 right = center + Vector2.right * rightDistance;
        
        // Dibujar línea de movimiento
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(left, right);
        
        // Dibujar puntos
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(left, 0.3f);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(right, 0.3f);
    }
}