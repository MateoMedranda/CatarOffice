using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
        // SINGLETON CON INMORTALIDAD
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // <--- ¡ESTA ES LA LÍNEA MÁGICA!
            Inventory = new List<InventoryItem>();
        }
        else
        {
            // Si ya existe uno (porque venimos de otra escena), nos destruimos para no duplicar
            Destroy(gameObject);
        }
    }

    // Verifica si cabe un objeto más (sin importar si ya tienes uno igual)
    public bool CanAdd(InventoryItemData itemData)
    {
        // Simplemente miramos si la lista tiene menos de 3 objetos
        if (Inventory.Count < maxDistinctItems)
        {
            return true;
        }

        UnityEngine.Debug.Log("Inventario lleno: No caben más objetos.");
        return false;
    }

    public void Add(InventoryItemData itemData)
    {
        // 1. PRIMERO BUSCAMOS SI YA TENEMOS ESE OBJETO
        InventoryItem value = null;

        foreach (InventoryItem i in Inventory)
        {
            if (i.data == itemData)
            {
                value = i;
                break;
            }
        }

        // 2. DECISIÓN: ¿SUMAR O CREAR?
        if (value != null)
        {
            // YA EXISTE -> Solo aumentamos el contador
            value.stackSize++;
            UnityEngine.Debug.Log($"[SISTEMA] Se sumó 1 a {itemData.itemName}. Total: {value.stackSize}");
        }
        else
        {
            // ES NUEVO -> Verificamos espacio y creamos
            if (Inventory.Count >= maxDistinctItems)
            {
                UnityEngine.Debug.Log("[SISTEMA] Inventario lleno, no cabe slot nuevo.");
                return;
            }

            InventoryItem newItem = new InventoryItem(itemData);
            Inventory.Add(newItem);
            UnityEngine.Debug.Log($"[SISTEMA] Creado slot nuevo para: {itemData.itemName}");
        }

        // 3. AVISAR A LA UI
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