using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; 

public class DoorLogic : MonoBehaviour
{
    [Header("Configuración de Viaje")]
    public string escenaDestino;
    public string sfxPuerta;

    // --- AGREGADO: INICIO ---
    [Tooltip("Escribe aquí el ID del SpawnPoint al que quieres llegar (Ej: EntradaDesdePasillo)")]
    public string idSpawnDestino; // 1. Variable nueva para elegir destino

    // Memoria Global (Static): Aquí se anota el destino antes de irse
    public static string ProximoSpawnID; 
    // --- AGREGADO: FIN ---

    [Header("Configuración Visual")]
    [Tooltip("Arrastre aquí el objeto del Canvas que tiene el Animator (el Panel Negro).")]
    public Animator transitionAnimator; 

    [Header("Tiempos")]
    public float esperaSonido = 1.0f;    
    public float duracionTransicion = 2.0f; 

    public void AbrirPuerta()
    {
        StartCoroutine(SecuenciaDeSalida());
    }

    IEnumerator SecuenciaDeSalida()
    {
        Debug.Log("1. Reproduciendo sonido...");
        AudioManager.Instance.PlaySFX(sfxPuerta);

        // PASO 1: Esperar sonido
        yield return new WaitForSeconds(esperaSonido);

        // --- AGREGADO: INICIO ---
        // Justo antes de irnos, guardamos el dato en la memoria global
        Debug.Log("Guardando destino en memoria: " + idSpawnDestino);
        ProximoSpawnID = idSpawnDestino; 
        // --- AGREGADO: FIN ---

        Debug.Log("2. Iniciando transición visual...");
        // PASO 2: Activar animación
        if (transitionAnimator != null)
        {
            transitionAnimator.SetTrigger("Cerrar");
        }

        // PASO 3: Esperar animación
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