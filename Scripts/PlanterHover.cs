using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider2D))]
public class PlanetHover : MonoBehaviour
{
    [Header("Escala al pasar ratón")]
    public float hoverScale = 1.2f;

    [Header("Descripción del nivel")]
    public string levelName;
    [TextArea]
    public string description;

    [Header("Referencias UI (Screen Space – Overlay)")]
    public GameObject tooltipPanel;   // Arrastra aquí tu TooltipPanel
    public Text tooltipText;          // Arrastra aquí tu TooltipText

    [Header("Offset en pantalla")]
    public Vector2 tooltipScreenOffset = new Vector2(0f, 80f);

    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
        if (tooltipPanel != null)
            tooltipPanel.SetActive(false);
    }

    void OnMouseEnter()
    {
        transform.localScale = originalScale * hoverScale;
        if (tooltipPanel != null && tooltipText != null)
        {
            tooltipText.text = $"<b>{levelName}</b>\n{description}";
            tooltipPanel.SetActive(true);
            Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
            tooltipPanel.transform.position = screenPos + (Vector3)tooltipScreenOffset;
        }
    }

    void OnMouseExit()
    {
        transform.localScale = originalScale;
        if (tooltipPanel != null)
            tooltipPanel.SetActive(false);
    }

    // ← NUEVO: al hacer clic, notificamos al controlador del mapa
    void OnMouseDown()
    {
        if (StarMapController.Instance != null)
            StarMapController.Instance.SelectPlanet(this);
    }
}
