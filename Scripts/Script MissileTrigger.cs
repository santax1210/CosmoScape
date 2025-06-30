using UnityEngine;

public class MissileTrigger : MonoBehaviour
{
    [Header("Arrastra aquí tu misil de la escena")]
    public MissileMovement missile;
    [Header("¿Solo una vez?")]
    public bool spawnOnce = true;

    private bool used = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (used) return;
        if (other.CompareTag("Player"))
        {
            missile.Activate();
            if (spawnOnce) used = true;
        }
    }
}
