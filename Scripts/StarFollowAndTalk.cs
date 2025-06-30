using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // Para TextMeshPro

public class StarFollowAndTalk : MonoBehaviour
{
    [Header("Jugador a seguir")]
    public Transform player;
    public Vector3 offset = new Vector3(1f, 1f, 0f);
    public float followSpeed = 2f;

    [Header("Mensajes")]
    public string[] messages;          // Rellena en el Inspector
    public float displayTime  = 3f;    // Segundos que dura cada burbuja

    [Header("Burbuja UI")]
    public GameObject speechBubble;    // Arrastra aquí tu GameObject SpeechBubble
    
    // Soporte para ambos tipos de texto
    private Text bubbleText;           // Text legacy
    private TextMeshProUGUI bubbleTextTMP; // TextMeshPro
    private Coroutine bubbleRoutine;

    void Start()
    {
        // Verificar que speechBubble esté asignado
        if (speechBubble == null)
        {
            Debug.LogError("Speech Bubble no está asignado en " + gameObject.name);
            return;
        }

        // Intentar encontrar Text legacy primero
        bubbleText = speechBubble.GetComponentInChildren<Text>();
        
        // Si no hay Text legacy, buscar TextMeshPro
        if (bubbleText == null)
        {
            bubbleTextTMP = speechBubble.GetComponentInChildren<TextMeshProUGUI>();
        }

        // Verificar que se encontró algún componente de texto
        if (bubbleText == null && bubbleTextTMP == null)
        {
            Debug.LogError("No se encontró componente Text o TextMeshPro en " + speechBubble.name);
            return;
        }

        // Ocultar la burbuja al inicio
        speechBubble.SetActive(false);
    }

    void Update()
    {
        if (player == null) return;
        // Sigue suavemente al jugador
        Vector3 target = player.position + offset;
        transform.position = Vector3.Lerp(
            transform.position,
            target,
            followSpeed * Time.deltaTime
        );
    }

    /// <summary>
    /// Muestra el mensaje en messages[index] durante displayTime segundos.
    /// </summary>
    public void ShowMessage(int index)
    {
        if (index < 0 || index >= messages.Length) return;
        if (speechBubble == null) return; // Verificación adicional

        // Si ya estaba mostrando otro, deténlo
        if (bubbleRoutine != null)
            StopCoroutine(bubbleRoutine);

        bubbleRoutine = StartCoroutine(ShowBubble(messages[index]));
    }

    private IEnumerator ShowBubble(string msg)
    {
        // Asignar texto según el tipo de componente disponible
        if (bubbleText != null)
        {
            bubbleText.text = msg;
        }
        else if (bubbleTextTMP != null)
        {
            bubbleTextTMP.text = msg;
        }
        else
        {
            Debug.LogError("No hay componente de texto disponible");
            yield break;
        }

        speechBubble.SetActive(true);
        yield return new WaitForSeconds(displayTime);
        speechBubble.SetActive(false);
    }
}