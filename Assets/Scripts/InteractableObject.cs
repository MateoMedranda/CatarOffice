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
    // Esta es la lista que ve vacía en el inspector. Aquí conectaremos la puerta.
    public UnityEvent onInteract;

    public void Interactuar()
    {
        // 1. CHEQUEO DE BLOQUEO (La Escoba)
        if (blockingObject != null && blockingObject.activeSelf)
        {
            Debug.Log("Interacción bloqueada por: " + blockingObject.name);
            
            // AGREGADO: Sonido de Error/Xray
            AudioManager.Instance.PlaySFX("Error"); 
            
            return; // Cortamos aquí, no dejamos pasar.
        }

        // 2. ÉXITO (Aquí se llama a lo que pongamos en la lista)
        onInteract.Invoke();

        if (disableAfterInteraction)
        {
            gameObject.SetActive(false);
        }
    }
}