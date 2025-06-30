// Script para detectar la finalización del nivel (mejorado)
using UnityEngine;

public class LevelFinish : MonoBehaviour
{
    [Header("Configuración del Final")]
    public GameObject levelCompleteUI;
    public AudioClip victorySound;
    
    [Header("Efectos Visuales (Opcional)")]
    public ParticleSystem confettiEffect;
    
    private AudioSource audioSource;
    private bool levelCompleted = false;
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        
        // Asegurarse de que el UI esté desactivado al inicio
        if (levelCompleteUI != null)
            levelCompleteUI.SetActive(false);
    }
    
    // Método público para ser llamado desde OneShotPlatform
    public void CompleteLevel()
    {
        if (levelCompleted) return;
        
        levelCompleted = true;
        
        // Reproducir sonido de victoria
        if (audioSource != null && victorySound != null)
        {
            audioSource.PlayOneShot(victorySound);
        }
        
        // Efectos de partículas
        if (confettiEffect != null)
        {
            confettiEffect.Play();
        }
        
        // Mostrar UI de nivel completado
        if (levelCompleteUI != null)
        {
            levelCompleteUI.SetActive(true);
        }
        
        // Pausar el juego después de un pequeño delay
        Invoke("PauseGame", 1f);
    }
    
    private void PauseGame()
    {
        Time.timeScale = 0f;
    }
    
    // Método para cargar el siguiente nivel (opcional)
    public void LoadNextLevel()
    {
        Time.timeScale = 1f;
        // UnityEngine.SceneManagement.SceneManager.LoadScene("NextLevelName");
    }
}