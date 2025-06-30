using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class OneShotPlatform : MonoBehaviour
{
    [Header("Movimiento")]
    public Transform pointA;
    public Transform pointB;
    public float speed = 2f;        // Unidades/segundo

    [Header("Salto al finalizar")]
    public float jumpForce = 5f;    // Impulso vertical

    private Rigidbody2D _rb;
    private bool _moving = false;
    private Rigidbody2D _playerRb;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.bodyType = RigidbodyType2D.Kinematic;  // Kinematic para MovePosition
        _rb.simulated = true;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (!_moving && col.collider.CompareTag("Player"))
        {
            _playerRb = col.collider.attachedRigidbody;
            StartCoroutine(MoveUp());
        }
    }

    private IEnumerator MoveUp()
    {
        _moving = true;

        Vector2 startPos = _rb.position;
        Vector2 endPos   = pointB.position;
        float  distance  = Vector2.Distance(startPos, endPos);
        float  duration  = distance / speed;
        float  elapsed   = 0f;

        // Subida
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            Vector2 nextPos = Vector2.Lerp(startPos, endPos, t);
            _rb.MovePosition(nextPos);
            yield return new WaitForFixedUpdate();
        }

        // Asegura posición exacta
        _rb.MovePosition(endPos);

        // Aplica el salto al jugador justo al llegar
        if (_playerRb != null)
        {
            _playerRb.linearVelocity = new Vector2(_playerRb.linearVelocity.x, 0f);
            _playerRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }
}
