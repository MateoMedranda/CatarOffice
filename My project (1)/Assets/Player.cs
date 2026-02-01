using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 10f;

    [Header("Configuración de Soltar (Drop)")]
    public Transform dropPoint;
    public InventoryItemData currentItem;

    void Update()
    {
        // --- MOVIMIENTO (Tu código original intacto) ---
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 input = new Vector3(h, 0f, v);

        if (input.sqrMagnitude > 0.0001f)
        {
            Vector3 direction = input.normalized;
            Vector3 displacement = direction * speed * Time.deltaTime;
            transform.Translate(displacement, Space.World);
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // --- SOLTAR (Tu código original intacto) ---
        if (Input.GetKeyDown(KeyCode.G))
        {
            Debug.Log("--- INICIANDO DROP ---");
            DropItem();
        }

        // --- AGREGADO: SELECCIÓN DE ARMAS ---
        // Tecla 1 para Candy
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            EquipItemByName("Candy");
        }
        // Tecla 2 para Pizza
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            EquipItemByName("Pizza");
        }
    }

    // --- AGREGADO: FUNCIÓN PARA BUSCAR Y EQUIPAR ---
    void EquipItemByName(string nameToFind)
    {
        // Verificamos que el sistema exista
        if (InventorySystem.Instance != null)
        {
            // Buscamos en la lista del inventario
            foreach (InventoryItem item in InventorySystem.Instance.Inventory)
            {
                // Comparamos el nombre del item guardado con lo que buscamos
                if (item.data.itemName == nameToFind)
                {
                    currentItem = item.data; // Lo ponemos en la mano
                    Debug.Log("¡Equipado: " + nameToFind + "!");
                    return; // Terminamos porque ya lo encontramos
                }
            }
            // Si el bucle termina y no retornamos, es que no lo tienes
            Debug.Log("No tienes '" + nameToFind + "' en tu inventario.");
        }
    }

    // --- TU FUNCIÓN DROP ORIGINAL (INTACTA) ---
    void DropItem()
    {
        // Chequeo 1: ¿Tienes un punto de salida?
        if (dropPoint == null)
        {
            Debug.Log("ERROR: ¡El 'Drop Point' se ha borrado o no está asignado!");
            return;
        }

        // Chequeo 2: ¿Tienes algo en la mano?
        if (currentItem == null)
        {
            Debug.Log("ERROR: Tu mano está vacía (Current Item es 'None'). Arrastra un ítem al Player para probar.");
            return;
        }

        // Chequeo 3: ¿El ítem sabe qué forma tiene?
        if (currentItem.itemPrefab == null)
        {
            Debug.Log("ERROR: El ítem '" + currentItem.name + "' no tiene un Prefab asignado en su archivo.");
            return;
        }

        // --- SI LLEGAMOS AQUÍ, TODO ESTÁ BIEN ---
        Debug.Log("Todo correcto. Creando objeto...");

        Instantiate(currentItem.itemPrefab, dropPoint.position, dropPoint.rotation);

        // BORRAR DEL INVENTARIO
        if (InventorySystem.Instance != null)
        {
            Debug.Log("Solicitando borrar del inventario...");
            InventorySystem.Instance.Remove(currentItem);
        }
        else
        {
            Debug.LogError("ERROR CRÍTICO: No se encuentra el 'InventorySystem' en la escena.");
        }

        currentItem = null; // Vaciamos la mano
        Debug.Log("¡ÉXITO! Objeto soltado y borrado.");
    }
}