using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Llamado al presionar "Jugar"
    public void PlayGame()
    {
        // Reinicia el flag de prueba completada
        PlayerPrefs.DeleteKey("TestLevelCompleted");
        PlayerPrefs.Save();
        
        // Carga tu escena de introducción (o nivel de prueba)
        SceneManager.LoadScene("Intro");
    }

    // Llamado al presionar "Salir"
    public void QuitGame()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}
