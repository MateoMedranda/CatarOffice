using UnityEngine;
using UnityEngine.Events;

public class InteractableObject : MonoBehaviour, IInteractable
{
    [Header("Bloqueo por Objeto")]
    [Tooltip("El objeto que debe desaparecer para poder usar esto (ej: La Escoba).")]
    public GameObject blockingObject;

    [Header("Requisito de Item")]
    [Tooltip("El item que debe tener el jugador para interactuar (ej: Tarjeta). Dejar vacío si no requiere nada.")]
    public InventoryItemData requiredItem;

    [Header("Conexiones")]
    public UnityEvent onInteract;

    public void Interactuar()
    {
        // 1. CHEQUEO DE BLOQUEO
        if (blockingObject != null && blockingObject.activeSelf)
        {
            Debug.Log("⛔ Interacción bloqueada por: " + blockingObject.name);
            if (AudioManager.Instance != null) AudioManager.Instance.PlaySFX("Error");
            return;
        }

        // 2. VERIFICAR SI SE REQUIERE UN ITEM
        if (requiredItem != null)
        {
            // Verificamos si lo tienes usando la función de abajo
            if (!TieneItem(requiredItem))
            {
                Debug.Log($"❌ No tienes {requiredItem.itemName}. No puedes hacer esto.");
                if (AudioManager.Instance != null) AudioManager.Instance.PlaySFX("Error");
                return;
            }

            // Si llegamos aquí, ES QUE SÍ LO TIENES.
            Debug.Log($"✅ Verificación correcta: Tienes {requiredItem.itemName}.");
        }

        // 3. ÉXITO (Aquí se llama a lo que pongamos en la lista)
        Debug.Log("✨ ¡ABRIENDO PUERTA!");
        onInteract.Invoke();
    }

    // Método auxiliar para verificar si el jugador tiene un item específico
    private bool TieneItem(InventoryItemData item)
    {
        if (InventorySystem.Instance == null)
        {
            Debug.LogWarning("InventorySystem no encontrado en la escena.");
            return false;
        }

        // Recorremos el inventario buscando el item
        foreach (InventoryItem inventoryItem in InventorySystem.Instance.Inventory)
        {
            // PROTECCIÓN IMPORTANTE:
            // Si hay un slot vacío o con error, lo saltamos (esto evita que el código se congele)
            if (inventoryItem == null || inventoryItem.data == null) continue;

            if (inventoryItem.data == item)
            {
                return true; // ¡Lo encontramos!
            }
        }

        return false; // Revisamos todo y no estaba
    }
}