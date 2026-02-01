using UnityEngine;
using UnityEngine.Events;

public class NPC_RecibirObjeto : MonoBehaviour
{
    [Header("Requisito")]
    [Tooltip("El objeto que el jugador debe tener en el inventario para completar la misión.")]
    public InventoryItemData itemNecesario;

    [Header("Eventos")]
    [Tooltip("Qué pasa si SÍ tienes el objeto (se borrará del inventario).")]
    public UnityEvent alEntregar;

    [Tooltip("Qué pasa si NO tienes el objeto (opcional).")]
    public UnityEvent alFallar;

    public void VerificarYEntregar()
    {
        // 1. Seguridad: Verificar sistema
        if (InventorySystem.Instance == null) return;

        // 2. Buscar si el jugador tiene el item en su lista
        bool tieneItem = false;
        foreach (var slot in InventorySystem.Instance.Inventory)
        {
            if (slot.data == itemNecesario)
            {
                tieneItem = true;
                break;
            }
        }

        // 3. Lógica
        if (tieneItem)
        {
            // A) Quitar del inventario del jugador
            InventorySystem.Instance.Remove(itemNecesario);

            // B) Feedback (Sonido y Log)
            if (AudioManager.Instance != null) AudioManager.Instance.PlaySFX("Change"); // O sonido de éxito
            Debug.Log("¡Misión cumplida! Entregaste: " + itemNecesario.itemName);

            // C) Disparar evento de éxito (Aquí conectas lo que quieras que pase)
            alEntregar.Invoke();
        }
        else
        {
            // A) Feedback de error
            if (AudioManager.Instance != null) AudioManager.Instance.PlaySFX("Error");
            Debug.Log("Te falta el objeto: " + itemNecesario.itemName);

            // B) Disparar evento de fallo
            alFallar.Invoke();
        }
    }
}