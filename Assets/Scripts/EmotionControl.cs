using UnityEngine;

public class EmotionControl : MonoBehaviour
{
    [Header("Referencias")]
    public Material materialParedes; // Arrastre aquí el material "ParedBase"
    public Camera camaraJuego;       // Arrastre aquí su Main Camera

    [Header("Colores de Estados (Orden: 0=Aburrido, 1=Alegre, 2=Enojado)")]
    public Color[] coloresPared;     // Lista de colores para el suelo/paredes
    public Color[] coloresFondo;     // Lista de colores para el vacío

    private int indiceActual = 0;

    void Start()
    {
        // Aplicar el primer color al iniciar el juego
        CambiarColor();
    }

    void Update()
    {
        // Detectar tecla Q (funciona porque activó "Both" en inputs)
        if (Input.GetKeyDown(KeyCode.Q))
        {
            // Avanzar al siguiente (0 -> 1 -> 2 -> 0...)
            indiceActual++;
            if (indiceActual >= coloresPared.Length)
            {
                indiceActual = 0;
            }
            CambiarColor();
        }
    }

    void CambiarColor()
    {
        // 1. Cambiar color del material (afecta a todos los objetos que lo usan)
        if (materialParedes != null)
            materialParedes.color = coloresPared[indiceActual];

        // 2. Cambiar color de fondo de la cámara
        if (camaraJuego != null)
            camaraJuego.backgroundColor = coloresFondo[indiceActual];
    }
}