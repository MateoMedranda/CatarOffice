using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para detectar cambio de escena

public class EmotionControl : MonoBehaviour
{
    public static EmotionControl Instance;

    [Header("Referencias Globales")]
    public Material materialParedes; // El material persiste solo (es un asset)

    [Header("Configuración de Emociones")]
    // Orden: 0=Aburrido, 1=Alegre, 2=Enojado
    public Color[] coloresPared;
    public Color[] coloresFondo; // Color de la cámara
    public string[] nombresMusica; // Ej: "Musica_Aburrido", "Musica_Alegre", "Musica_Enojado"

    private int indiceActual = 0;
    private Camera camaraActual;

    private void Awake()
    {
        // Singleton para mantener la emoción entre escenas
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnEnable()
    {
        // Nos suscribimos al evento de "Cargó una nueva escena"
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Se llama cada vez que entramos a una escena (Nivel 1, Nivel 2...)
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 1. Buscar la cámara nueva
        camaraActual = Camera.main;

        // 2. Re-aplicar la emoción que teníamos guardada
        AplicarEmocion(false); // false = cambio instantáneo de visuales al cargar
    }

    void Start()
    {
        // Arrancar la primera vez con música
        AplicarEmocion(true); // true = con transición de música
    }

    void Update()
    {
        // Detectar tecla Q para cambiar emoción
        if (Input.GetKeyDown(KeyCode.Q))
        {
            indiceActual++;
            if (indiceActual >= coloresPared.Length)
            {
                indiceActual = 0;
            }
            AplicarEmocion(true);
        }
    }

    void AplicarEmocion(bool cambiarMusica)
    {
        // 1. Cambiar Paredes (Material Global)
        if (materialParedes != null && coloresPared.Length > indiceActual)
            materialParedes.color = coloresPared[indiceActual];

        // 2. Cambiar Fondo Cámara (Local de la escena)
        if (camaraActual != null && coloresFondo.Length > indiceActual)
            camaraActual.backgroundColor = coloresFondo[indiceActual];
        else if (camaraActual == null)
            camaraActual = Camera.main; // Intento de rescate si se perdió

        // 3. Cambiar Música (Solo si se pide, para evitar reinicios innecesarios)
        if (cambiarMusica && nombresMusica.Length > indiceActual)
        {
            // Llama al AudioManager para que haga el Fade
            AudioManager.Instance.PlayMusic(nombresMusica[indiceActual]);
        }
    }
}