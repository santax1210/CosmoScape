using UnityEngine;

public class MissileMovement : MonoBehaviour
{
    [Header("Velocidad y duración")]
    public float speed    = 6f;
    public float lifetime = 5f;

    private Vector2 direction;
    private bool   active = false;

    void Start()
    {
        // No movemos el misil hasta que nos digas
    }

    void Update()
    {
        if (!active) return;
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    // Llama a este método cuando quieras que el misil «despegue»
    public void Activate()
    {
        // Calcula la dirección al jugador en el momento de activación
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
            direction = (player.transform.position - transform.position).normalized;
        else
            direction = Vector2.right;

        active = true;
        Destroy(gameObject, lifetime);  // se autodestruye pasado el tiempo
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!active) return;
        if (other.CompareTag("Player"))
        {
            HUDManager.instance.LoseLifeAndRestart();
            Destroy(gameObject);
        }
    }
}
