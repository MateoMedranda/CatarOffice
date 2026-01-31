using UnityEngine;
using UnityEngine.Events;

public class InteractableObject : MonoBehaviour, IInteractable
{
    [Tooltip("Object that must be inactive/removed before this can be interacted with.")]
    public GameObject blockingObject;

    [Tooltip("If true, this object disables itself after interaction (e.g., a broom being picked up).")]
    public bool disableAfterInteraction;

    public UnityEvent onInteract;

    public void Interactuar()
    {
        // Check if blocked
        if (blockingObject != null && blockingObject.activeSelf)
        {
            Debug.Log("Interacción bloqueada por: " + blockingObject.name);
            return;
        }

        // Perform interaction
        onInteract.Invoke();

        if (disableAfterInteraction)
        {
            gameObject.SetActive(false);
        }
    }
}
