using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelCompleteUI : MonoBehaviour
{
    [Header("Botones")]
    public Button nextLevelButton;
    public Button restartButton;
    public Button mainMenuButton;
    
    [Header("Nombres de Escenas")]
    [Tooltip("Ahora usaremos esta escena para volver al mapa estelar")]
    public string mapSceneName       = "Mapa";
    public string currentLevelSceneName = "Level1";
    public string mainMenuSceneName     = "MainMenu";
    
    void Start()
    {
        // Configurar los botones
        if (nextLevelButton != null)
            nextLevelButton.onClick.AddListener(ReturnToMap);
            
        if (restartButton != null)
            restartButton.onClick.AddListener(RestartLevel);
            
        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(LoadMainMenu);
    }
    
    /// <summary>
    /// Volver al mapa estelar
    /// </summary>
    private void ReturnToMap()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mapSceneName);
    }
    
    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(currentLevelSceneName);
    }
    
    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
