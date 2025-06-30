using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StarMapController : MonoBehaviour
{
    public static StarMapController Instance;

    [Header("Nave espacial")]
    public Transform ship;
    public float travelSpeed = 5f;

    [Header("UI Confirmación")]
    public GameObject confirmationPanel;
    public Text confirmationText;

    [Header("UI Pre–Test")]
    [Tooltip("Panel que avisa de completar nivel de prueba")]
    public GameObject preTestPanel;
    [Tooltip("Texto en ese panel")]
    public Text preTestText;
    [Tooltip("Mensaje que se muestra si no se ha completado el nivel de prueba")]
    public string preTestMessage = "Antes de viajar a uno de los 4 planetas debes completar el nivel de prueba";

    [Header("Zoom Config (Planetas)")]
    public Camera mapCamera;
    public float zoomDuration = 1f;
    public float targetOrthoSize = 2f;

    [Header("Zoom Config (Nivel de prueba)")]
    [Tooltip("Tamaño ortográfico al hacer zoom sobre la nave")]
    public float shipOrthoSize = 1.5f;
    [Tooltip("Duración del zoom sobre la nave")]
    public float shipZoomDuration = 1f;

    [Header("Estado de progresión")]
    [Tooltip("Marca si ya completó el nivel de prueba")]
    public bool testLevelCompleted = false;

    private PlanetHover targetPlanet;
    private bool isMoving = false;
    private float stopThreshold = 0.1f;
    private float originalOrthoSize;
    private Vector3 originalCamPos;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // Recupera el progreso del test de PlayerPrefs
        testLevelCompleted = PlayerPrefs.GetInt("TestLevelCompleted", 0) == 1;

        // Ocultar paneles al inicio
        if (confirmationPanel != null) confirmationPanel.SetActive(false);
        if (preTestPanel     != null) preTestPanel    .SetActive(false);

        // Configurar cámara
        if (mapCamera == null) mapCamera = Camera.main;
        originalOrthoSize = mapCamera.orthographicSize;
        originalCamPos    = mapCamera.transform.position;
    }

    void Update()
    {
        if (!isMoving || targetPlanet == null) return;

        // Mover la nave hacia el planeta
        ship.position = Vector3.MoveTowards(
            ship.position,
            targetPlanet.transform.position,
            travelSpeed * Time.deltaTime
        );

        // Al llegar, mostrar confirmación
        if (Vector3.Distance(ship.position, targetPlanet.transform.position) <= stopThreshold)
        {
            isMoving = false;
            confirmationText.text = $"¿Quieres jugar el planeta {targetPlanet.levelName}?";
            confirmationPanel.SetActive(true);
        }
    }

    /// <summary>
    /// Llamado por PlanetHover.OnMouseDown
    /// </summary>
    public void SelectPlanet(PlanetHover planet)
    {
        if (!testLevelCompleted)
        {
            preTestText.text = preTestMessage;
            preTestPanel.SetActive(true);
            return;
        }

        targetPlanet = planet;
        isMoving     = true;
        confirmationPanel.SetActive(false);
        preTestPanel    .SetActive(false);
    }

    public void OnConfirmYes()
    {
        confirmationPanel.SetActive(false);
        StartCoroutine(ZoomIntoPlanet());
    }

    public void OnConfirmNo()
    {
        confirmationPanel.SetActive(false);
    }

    public void OnPreTestConfirm()
    {
        preTestPanel.SetActive(false);
        StartCoroutine(ZoomIntoShipAndLoad());
    }

    /// <summary>
    /// Marca test como completado y persiste el dato
    /// </summary>
    public void MarkTestLevelCompleted()
    {
        testLevelCompleted = true;
        PlayerPrefs.SetInt("TestLevelCompleted", 1);
        PlayerPrefs.Save();
    }

    private IEnumerator ZoomIntoPlanet()
    {
        Vector3 camStartPos = mapCamera.transform.position;
        Vector3 camEndPos   = new Vector3(
            targetPlanet.transform.position.x,
            targetPlanet.transform.position.y,
            camStartPos.z
        );

        float startSize = mapCamera.orthographicSize;
        float elapsed   = 0f;

        while (elapsed < zoomDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / zoomDuration);

            mapCamera.transform.position = Vector3.Lerp(camStartPos, camEndPos, t);
            mapCamera.orthographicSize   = Mathf.Lerp(startSize, targetOrthoSize, t);

            yield return null;
        }

        mapCamera.transform.position = camEndPos;
        mapCamera.orthographicSize   = targetOrthoSize;

        SceneManager.LoadScene(targetPlanet.levelName);
    }

    private IEnumerator ZoomIntoShipAndLoad()
    {
        Vector3 camStartPos = mapCamera.transform.position;
        Vector3 camEndPos   = new Vector3(ship.position.x, ship.position.y, camStartPos.z);

        float startSize = mapCamera.orthographicSize;
        float elapsed   = 0f;

        while (elapsed < shipZoomDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / shipZoomDuration);

            mapCamera.transform.position = Vector3.Lerp(camStartPos, camEndPos, t);
            mapCamera.orthographicSize   = Mathf.Lerp(startSize, shipOrthoSize, t);

            yield return null;
        }

        mapCamera.transform.position = camEndPos;
        mapCamera.orthographicSize   = shipOrthoSize;

        // Antes de regresar al mapa, aseguramos el progreso
        MarkTestLevelCompleted();

        SceneManager.LoadScene("Nivel_1");
    }

    public IEnumerator ResetZoom()
    {
        Vector3 camStartPos = mapCamera.transform.position;
        float sizeStart     = mapCamera.orthographicSize;
        float elapsed       = 0f;

        while (elapsed < zoomDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / zoomDuration);

            mapCamera.transform.position = Vector3.Lerp(camStartPos, originalCamPos, t);
            mapCamera.orthographicSize   = Mathf.Lerp(sizeStart, originalOrthoSize, t);

            yield return null;
        }

        mapCamera.transform.position = originalCamPos;
        mapCamera.orthographicSize   = originalOrthoSize;
    }
}
