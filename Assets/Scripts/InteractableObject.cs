using UnityEngine;
using UnityEngine.Events;

public class InteractableObject : MonoBehaviour, IInteractable
{
    [Header("Bloqueo")]
    [Tooltip("El objeto que debe desaparecer para poder usar esto (ej: La Escoba).")]
    public GameObject blockingObject;

    [Tooltip("Si es true, este objeto se apaga al interactuar.")]
    public bool disableAfterInteraction;

    [Header("Conexiones")]
    public UnityEvent onInteract;

    public void Interactuar()
    {
        // 1. CHEQUEO DE BLOQUEO
        if (blockingObject != null && blockingObject.activeSelf)
        {
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX("Error");
            }
            return;
        }

        // --- SOLUCIÓN DEFINITIVA PARA LA LUPA ---
        
        // A) Ocultamos la lupa visualmente
        InteractionPrompt prompt = GetComponent<InteractionPrompt>();
        if (prompt != null)
        {
            prompt.Hide();
            prompt.enabled = false; // Apagar el script de la lupa
        }

        // B) ¡IMPORTANTE! Apagamos el Collider para que el Player deje de detectarlo
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false; 
        }
        // ---------------------------------------

        // 2. ÉXITO
        onInteract.Invoke();

        if (disableAfterInteraction)
        {
            gameObject.SetActive(false);
        }
    }
}