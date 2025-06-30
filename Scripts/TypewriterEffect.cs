using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class TypewriterEffect : MonoBehaviour
{
    [Header("UI Text")]
    [Tooltip("El componente Text donde se mostrará la historia")]
    public Text uiText;

    [TextArea]
    [Header("Historia a mostrar")]
    [Tooltip("Texto completo de la introducción")]
    public string fullText;

    [Header("Velocidad de escritura")]
    [Tooltip("Tiempo en segundos entre cada carácter")]
    public float charDelay = 0.05f;

    [Header("Sonido de tecleo (opcional)")]
    [Tooltip("Sirve si quieres reproducir un clic por cada letra")]
    public AudioClip typeSound;

    [Header("Escena a cargar al terminar")]
    [Tooltip("Nombre exacto de tu escena del mapa estelar")]
    public string nextSceneName = "StarMap";

    private AudioSource audioSource;

    void Awake()
    {
        // Obtiene (o añade) el AudioSource en este GameObject
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;  // No suena solo al iniciar
    }

    void Start()
    {
        // Arranca con el texto vacío y lanza la corrutina
        uiText.text = "";
        StartCoroutine(TypeTextAndLoad());
    }

    private IEnumerator TypeTextAndLoad()
    {
        // Máquina de escribir: cada carácter aparece con retardo
        foreach (char c in fullText)
        {
            uiText.text += c;

            // Reproduce efecto de tecleo si tienes clip asignado
            if (typeSound != null)
                audioSource.PlayOneShot(typeSound);

            yield return new WaitForSeconds(charDelay);
        }

        // Pausa breve al acabar
        yield return new WaitForSeconds(1f);

        // Carga la escena del mapa estelar
        if (!string.IsNullOrEmpty(nextSceneName))
            SceneManager.LoadScene(nextSceneName);
    }
}
