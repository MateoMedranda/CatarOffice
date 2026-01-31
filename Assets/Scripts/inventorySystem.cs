using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance;

    // Evento para actualizar la UI
    public delegate void OnInventoryChangedEvent();
    public event OnInventoryChangedEvent onInventoryChangedCallback;

    // Lista simple de objetos. Si tienes 3 manzanas, habrá 3 elementos aquí.
    public List<InventoryItem> Inventory;

    // CONFIGURACIÓN: Límite de objetos (lo que pediste: 3)
    [SerializeField] public int maxDistinctItems = 3;

    public void Awake()
    {
        Inventory = new List<InventoryItem>();
        Instance = this;
    }

    // Verifica si cabe un objeto más (sin importar si ya tienes uno igual)
    public bool CanAdd(InventoryItemData itemData)
    {
        // Simplemente miramos si la lista tiene menos de 3 objetos
        if (Inventory.Count < maxDistinctItems)
        {
            return true;
        }

        Debug.Log("Inventario lleno: No caben más objetos.");
        return false;
    }

    public void Add(InventoryItemData itemData)
    {
        // Doble verificación de seguridad
        if (Inventory.Count >= maxDistinctItems) return;

        // AQUÍ ESTÁ EL CAMBIO CLAVE:
        // Siempre creamos un NUEVO item y lo agregamos a la lista.
        // No buscamos si ya existe para apilarlo.
        InventoryItem newItem = new InventoryItem(itemData);
        Inventory.Add(newItem);
        Debug.Log($"Agregado slot individual para: {itemData.itemName}");

        // Actualizamos la barra visual
        onInventoryChangedCallback?.Invoke();
    }

    public void Remove(InventoryItemData itemData)
    {
        // Como ahora los objetos pueden estar repetidos en distintos slots,
        // buscamos el PRIMERO que coincida y lo borramos.
        InventoryItem itemToRemove = null;

        foreach (InventoryItem item in Inventory)
        {
            if (item.data == itemData)
            {
                itemToRemove = item;
                break; // Encontramos uno, dejamos de buscar
            }
        }

        if (itemToRemove != null)
        {
            Inventory.Remove(itemToRemove);
            // Actualizamos la barra visual para que desaparezca ese slot
            onInventoryChangedCallback?.Invoke();
        }
    }
}