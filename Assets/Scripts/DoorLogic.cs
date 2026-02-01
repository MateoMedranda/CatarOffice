using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; // <--- NECESARIO PARA LOS TEMPORIZADORES

public class DoorLogic : MonoBehaviour
{
    [Header("Configuración de Viaje")]
    public string escenaDestino;
    public string sfxPuerta;

    [Header("Configuración Visual")]
    [Tooltip("Arrasque aquí el objeto del Canvas que tiene el Animator (el Panel Negro).")]
    public Animator transitionAnimator; 

    [Header("Tiempos")]
    public float esperaSonido = 1.0f;     // Tiempo que esperamos oyendo el sonido antes de oscurecer
    public float duracionTransicion = 2.0f; // Tiempo que tarda el círculo en cerrarse

    // Esta es la función que llama el InteractableObject
    public void AbrirPuerta()
    {
        // En lugar de cambiar escena directo, iniciamos la secuencia
        StartCoroutine(SecuenciaDeSalida());
    }

    IEnumerator SecuenciaDeSalida()
    {
        Debug.Log("1. Reproduciendo sonido...");
        AudioManager.Instance.PlaySFX(sfxPuerta);

        // PASO 1: Esperar a que el jugador disfrute el sonido un momento
        yield return new WaitForSeconds(esperaSonido);

        Debug.Log("2. Iniciando transición visual...");
        // PASO 2: Activar la animación (Si asignó el Animator)
        if (transitionAnimator != null)
        {
            transitionAnimator.SetTrigger("Cerrar");
        }

        // PASO 3: Esperar a que termine la animación
        yield return new WaitForSeconds(duracionTransicion);

        Debug.Log("3. ¡Despegue! Cambiando de escena...");
        // PASO 4: Cambiar de escena
        if (!string.IsNullOrEmpty(escenaDestino))
        {
            SceneManager.LoadScene(escenaDestino);
        }
        else
        {
            Debug.LogError("Señor, falta el nombre de la escena en el Inspector.");
        }
    }
}