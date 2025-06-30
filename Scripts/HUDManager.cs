using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class HUDManager : MonoBehaviour
{
    // Singleton
    public static HUDManager instance;

    [Header("Vidas")]
    public Image[] heartIcons;            // Asigna aquí tus 3 corazones

    [Header("Monedas")]
    public Image coinIcon;                // Asigna aquí la Image de tu icono de moneda
    public TextMeshProUGUI coinsText;     // Asigna aquí tu TMP para el contador

    [Header("Valores iniciales")]
    public int lives = 3;
    public int coins = 0;

    void Awake()
    {
        // Asegura única instancia y persiste entre escenas
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // Inicializa HUD
        UpdateLivesUI(lives);
        UpdateCoinsUI(coins);
    }

    /// <summary>
    /// Conserva la firma antigua para no romper llamadas existentes.
    /// </summary>
    public void LoseLifeAndRestart()
    {
        LoseLife();
    }

    /// <summary>
    /// Disminuye vidas, actualiza HUD y recarga o hace Game Over.
    /// </summary>
    public void LoseLife()
    {
        lives--;
        UpdateLivesUI(lives);

        if (lives <= 0)
        {
            Debug.Log("Game Over!");
            lives = 3; // Reinicia vidas para la próxima partida
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            Debug.Log("Vida perdida. Reiniciando nivel...");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    /// <summary>
    /// Suma monedas y actualiza el contador.
    /// </summary>
    public void AddCoin(int amount)
    {
        coins += amount;
        UpdateCoinsUI(coins);

        // Ejemplo de cómo podrías animar el icono:
        // StartCoroutine(PunchIcon());
    }

    /// <summary>
    /// Refresca el texto de monedas.
    /// </summary>
    void UpdateCoinsUI(int currentCoins)
    {
        coinsText.text = currentCoins.ToString("D3");
    }

    /// <summary>
    /// Actualiza la visibilidad de los iconos de vida.
    /// </summary>
    void UpdateLivesUI(int currentLives)
    {
        for (int i = 0; i < heartIcons.Length; i++)
            heartIcons[i].enabled = (i < currentLives);
    }

    /// <summary>
    /// (Opcional) Un ejemplo simple de animación sin DOTween:
    /// </summary>
    // private IEnumerator PunchIcon()
    // {
    //     Vector3 original = coinIcon.rectTransform.localScale;
    //     coinIcon.rectTransform.localScale = original * 1.2f;
    //     yield return new WaitForSeconds(0.1f);
    //     coinIcon.rectTransform.localScale = original;
    // }

    /// <summary>
    /// (Opcional) Resetea completamente el juego.
    /// </summary>
    public void ResetGame()
    {
        lives = 3;
        coins = 0;
        UpdateLivesUI(lives);
        UpdateCoinsUI(coins);
    }
}
