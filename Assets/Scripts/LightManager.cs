using UnityEngine;

public class LightManager : MonoBehaviour
{
    [Header("MEMORIA (¡Importante!)")]
    [Tooltip("Escribe AQUÍ el mismo ID que pusiste en el ObjectPersistence del interruptor. Ej: Luz_Principal_Oficina")]
    public string memoryID;

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

    [Header("Objetos a eliminar")]
    [Tooltip("Objeto que se destruirá al encender la luz (opcional).")]
    public GameObject objectToDestroy;

    private bool isLightsOn = false;
    private float transitionTimer = 0f;

    private void Start()
    {
        // 1. PREGUNTAR A LA MEMORIA: ¿Ya prendieron esta luz antes?
        if (!string.IsNullOrEmpty(memoryID) && QuestManager.Instance != null && QuestManager.Instance.IsQuestActive(memoryID))
        {
            // SI YA ESTABA PRENDIDA: Forzamos el estado de luz INMEDIATAMENTE
            // CORRECCIÓN AQUÍ: UnityEngine.Debug
            UnityEngine.Debug.Log("[LightManager] Recordando que la luz estaba prendida. Restaurando...");
            SetLightsInstantOn();
        }
        else
        {
            // SI NO: Iniciamos en oscuridad normal
            SetLightsInstantOff();
        }
    }

    private void SetLightsInstantOff()
    {
        isLightsOn = false;
        if (directionalLight != null) directionalLight.intensity = dimIntensity;
        RenderSettings.ambientIntensity = dimAmbientIntensity;
        if (playerLight != null) playerLight.gameObject.SetActive(true);
    }

    private void SetLightsInstantOn()
    {
        isLightsOn = true;

        // Luces al máximo
        if (directionalLight != null) directionalLight.intensity = normalIntensity;
        RenderSettings.ambientIntensity = normalAmbientIntensity;

        // Cambiar sprite del interruptor
        if (switchRenderer != null && onSprite != null)
        {
            switchRenderer.sprite = onSprite;
        }

        // Eliminar monstruo/sombra si aún existe
        if (objectToDestroy != null)
        {
            Destroy(objectToDestroy);
        }
    }

    private void Update()
    {
        // Animación suave SOLO si estamos transicionando en tiempo real
        if (isLightsOn && directionalLight != null && (directionalLight.intensity < normalIntensity || RenderSettings.ambientIntensity < normalAmbientIntensity))
        {
            transitionTimer += Time.deltaTime;
            float t = transitionTimer / transitionDuration;

            directionalLight.intensity = Mathf.Lerp(dimIntensity, normalIntensity, t);
            RenderSettings.ambientIntensity = Mathf.Lerp(dimAmbientIntensity, normalAmbientIntensity, t);
        }
    }

    public void TurnOnLights()
    {
        if (isLightsOn) return;

        // CORRECCIÓN AQUÍ TAMBIÉN: UnityEngine.Debug
        UnityEngine.Debug.Log("Encendiendo luces (Evento)...");
        isLightsOn = true;
        transitionTimer = 0f;

        if (switchRenderer != null && onSprite != null) switchRenderer.sprite = onSprite;

        if (objectToDestroy != null) Destroy(objectToDestroy);

        AudioManager.Instance.PlaySFX("Switch");
    }
}