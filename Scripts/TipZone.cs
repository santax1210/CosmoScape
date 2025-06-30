using UnityEngine;

public class TipZone : MonoBehaviour
{
    [Tooltip("Índice en el array de mensajes de StarFollowAndTalk")]
    public int messageIndex;

    private StarFollowAndTalk star;

    void Start()
    {
        // Busca la estrella en la escena
        star = FindObjectOfType<StarFollowAndTalk>();
        if (star == null)
            Debug.LogError("TipZone: no se encontró ninguna StarFollowAndTalk en la escena.");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && star != null)
        {
            // Muestra el mensaje correspondiente
            star.ShowMessage(messageIndex);
        }
    }
}
