using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 2f;

    private Vector3 nextPoint;

    void Start()
    {
        if (pointA == null || pointB == null)
        {
            Debug.LogError("PointA o PointB no asignados");
            return;
        }

        nextPoint = pointB.position;
    }

    void Update()
    {
        if (pointA == null || pointB == null) return;

        // Movimiento hacia el punto
        transform.position = Vector2.MoveTowards(transform.position, nextPoint, speed * Time.deltaTime);

        // ¿Llegó cerca del punto?
        if (Vector2.Distance(transform.position, nextPoint) < 0.1f)
        {
            // Cambia el siguiente punto
            nextPoint = (nextPoint == pointA.position) ? pointB.position : pointA.position;
            Flip();
        }
    }

    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void OnDrawGizmosSelected()
    {
        if (pointA != null && pointB != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(pointA.position, pointB.position);
        }
    }
}
