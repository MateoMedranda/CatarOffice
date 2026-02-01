using UnityEngine;
using System.Collections; // <--- IMPRESCINDIBLE para que funcione la espera

public class NPC_DarObjeto : MonoBehaviour
{
    [Header("Configuración del Regalo")]
    [Tooltip("Arrastra aquí el InventoryItemData (ScriptableObject) que quieres que dé.")]
    public InventoryItemData itemParaRegalar;

    [Tooltip("Si es true, solo dará el objeto una vez.")]
    public bool esUnico = true;

    [Header("Opcional: Esperar Diálogo")]
    [Tooltip("Arrastra aquí el Panel o Canvas del diálogo (el objeto que aparece y desaparece).")]
    public GameObject cajaDeDialogoUI;

    private bool yaEntregado = false;

    // Este es el método que conectaremos en el Inspector
    public void EntregarItem()
    {
        // 1. Verificamos si ya se entregó
        if (esUnico && yaEntregado)
        {
            Debug.Log("El NPC dice: 'Ya te di esto, no seas codicioso'.");
            return; 
        }

        // --- BLOQUE DE ESPERA ---
        // Si asignaste la caja de diálogo, iniciamos la espera
        if (cajaDeDialogoUI != null)
        {
            StartCoroutine(EsperarCierreDialogo());
            return; // Cortamos aquí para que no te lo dé inmediatamente, sino después
        }
        // ------------------------

        // Si no hay caja de diálogo asignada, te lo da al instante
        DarObjetoAhora();
    }

    // --- ESTA ES LA PARTE QUE TE FALTABA ---
    IEnumerator EsperarCierreDialogo()
    {
        // Esperamos un momento para asegurar que el diálogo se haya activado primero
        yield return new WaitForSeconds(0.1f);
        Debug.Log("Esperando a que cierres el diálogo...");
        // Esperamos MIENTRAS la caja esté activa (visible)
        // Se quedará pausado aquí hasta que cierres el diálogo
        yield return new WaitUntil(() => !cajaDeDialogoUI.activeSelf);

        // Una vez cerrado, damos el objeto
        DarObjetoAhora();
        Debug.Log("Espera terminada, dando el objeto.");
    }

    private void DarObjetoAhora()
    {
        // 2. Verificamos que tengamos el sistema de inventario
        if (InventorySystem.Instance == null)
        {
            Debug.LogError("No se encontró el InventorySystem en la escena.");
            return;
        }

        // 3. Verificamos que haya un item asignado
        if (itemParaRegalar == null)
        {
            Debug.LogError("Se te olvidó asignar el Item en el inspector del NPC.");
            return;
        }

        // 4. AÑADIR AL INVENTARIO
        InventorySystem.Instance.Add(itemParaRegalar); 

        // 5. Marcar como entregado y Sonido
        yaEntregado = true;

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX("Change"); 
        }
        
        Debug.Log("¡Has recibido: " + itemParaRegalar.itemName + "!");
    }
}