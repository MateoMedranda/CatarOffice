using UnityEngine;

public class LightManager : MonoBehaviour
{
    [Header("Referencias de Luces")]
    [Tooltip("La luz direccional principal de la escena.")]
    public Light directionalLight;

    [Tooltip("La luz pequeña que acompaña al jugador (puede ser null si no existe).")]
    public Light playerLight;

    [Header("Configuración")]
    [Tooltip("Intensidad de la luz direccional al inicio (Modo Oscuro).")]
    public float dimIntensity = 0.1f;

    [Tooltip("Intensidad de la luz direccional al encenderse (Modo Normal).")]
    public float normalIntensity = 1.0f;

    [Tooltip("Intensidad de la luz ambiente al inicio (Modo Oscuro).")]
    public float dimAmbientIntensity = 0f;

    [Tooltip("Intensidad de la luz ambiente al encenderse (Modo Normal).")]
    public float normalAmbientIntensity = 1.0f;

    [Tooltip("Duración de la transición de luz (en segundos).")]
    public float transitionDuration = 2.0f;

    [Header("Visuales del Interruptor")]
    [Tooltip("El SpriteRenderer del objeto interruptor (opcional).")]
    public SpriteRenderer switchRenderer;
    [Tooltip("El sprite que se pondrá cuando la luz esté ENCENDIDA.")]
    public Sprite onSprite;

    private bool isLightsOn = false;
    private float targetIntensity;
    private float initialIntensity;
    private float transitionTimer = 0f;

    private void Start()
    {
        // Estado Inicial: Oscuridad
        if (directionalLight != null)
        {
            directionalLight.intensity = dimIntensity;
        }

        // Controlar la luz ambiente también para que sea oscuro de verdad
        RenderSettings.ambientIntensity = dimAmbientIntensity;

        if (playerLight != null)
        {
            playerLight.gameObject.SetActive(true); // Aseguramos que la luz del jugador esté prendida
        }
    }

    private void Update()
    {
        // Animación simple de intensidad si estamos en transición
        if (isLightsOn && directionalLight != null && (directionalLight.intensity < normalIntensity || RenderSettings.ambientIntensity < normalAmbientIntensity))
        {
            transitionTimer += Time.deltaTime;
            float t = transitionTimer / transitionDuration;
            
            // Lerp de luz direccional
            directionalLight.intensity = Mathf.Lerp(dimIntensity, normalIntensity, t);
            
            // Lerp de luz ambiente
            RenderSettings.ambientIntensity = Mathf.Lerp(dimAmbientIntensity, normalAmbientIntensity, t);
        }
    }

    /// <summary>
    /// Función pública para llamar desde el InteractableObject (UnityEvent).
    /// </summary>
    public void TurnOnLights()
    {
        if (isLightsOn) return; // Ya está encendida

        Debug.Log("Encendiendo luces...");
        isLightsOn = true;
        transitionTimer = 0f;

        // Opcional: Apagar la luz del jugador cuando se enciende la luz principal
        // if (playerLight != null) playerLight.gameObject.SetActive(false);
        
        // Cambiar el sprite del interruptor si está asignado
        if (switchRenderer != null && onSprite != null)
        {
            switchRenderer.sprite = onSprite;
        }

        AudioManager.Instance.PlaySFX("Switch"); // Asumiendo que existe un sonido "Switch" o genérico
    }
}
