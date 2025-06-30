using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PillarDeath : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Usamos collision.collider para acceder al GameObject que colisionó
        if (collision.collider.CompareTag("Player"))
        {
            // Llama a tu HUDManager para perder vida y reiniciar
            var hud = FindObjectOfType<HUDManager>();
            if (hud != null)
                hud.LoseLifeAndRestart();
            else
                Debug.LogWarning("No se encontró HUDManager para reiniciar tras colisión con pilar.");
        }
    }
}
