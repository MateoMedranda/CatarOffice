using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public static  InventorySystem Instance;
    private Dictionary<InventoryItemData, InventoryItem> _itemDictionary = new Dictionary<InventoryItemData, InventoryItem>();
    public List<InventoryItem> Inventory;

    // Limitación máxima de objetos distintos (configurable en el inspector)
    [SerializeField]
    public int maxDistinctItems = 1;

    public void Awake()
    {
        Inventory = new List<InventoryItem>();
        _itemDictionary = new Dictionary<InventoryItemData, InventoryItem>();
        Instance = this;
    }
    public bool CanAdd(InventoryItemData itemData)
    {
        // CASO A: Ya tienes este tipo de objeto en el inventario
        if (_itemDictionary.TryGetValue(itemData, out InventoryItem value))
        {
            // Solo puedes recogerlo si el stack (montón) no está lleno
            return value.stackSize < itemData.maxStackSize;
        }

        // CASO B: Es un objeto NUEVO que no tienes
        // Solo puedes recogerlo si NO has superado el límite de objetos distintos (slots)
        if (Inventory.Count < maxDistinctItems)
        {
            return true;
        }

        // Si no entra en ninguna, es que el inventario está lleno "por objetos"
        Debug.Log("No hay slots disponibles para un nuevo tipo de objeto.");
        return false;
    }

    public void Add(InventoryItemData itemData)
    {
        // 1. Intentar encontrar el item en el diccionario
        if (_itemDictionary.TryGetValue(itemData, out InventoryItem value))
        {
            // LIMITACIÓN: Verificar si el stack ya está lleno
            // Asumiendo que itemData tiene una variable 'maxStackSize'
            if (value.stackSize < itemData.maxStackSize)
            {
                Debug.Log($"Sumando al stack de {itemData.itemName}");
                value.AddStack();
            }
            else
            {
                Debug.Log($"El stack de {itemData.itemName} está lleno.");
                // Aquí podrías decidir si crear un nuevo slot o simplemente no agregar más
            }
        }
        else
        {
            // 2. Si no existe, verificar límite de slots (objetos distintos)
            if (Inventory.Count >= maxDistinctItems)
            {
                Debug.Log($"Inventario lleno: límite de {maxDistinctItems} slots alcanzado.");
                return;
            }

            Debug.Log($"Agregando nuevo item: {itemData.itemName}");
            InventoryItem newItem = new InventoryItem(itemData);
            Inventory.Add(newItem);
            _itemDictionary.Add(itemData, newItem);
        }
    }

    public void Remove(InventoryItemData itemData)
    {
        if (_itemDictionary.TryGetValue(itemData, out InventoryItem value))
        {
            value.RemoveFromStack();
            if (value.stackSize == 0)
            {
                Inventory.Remove(value);
                _itemDictionary.Remove(itemData);
            }
        }
        // onInventoryChangedEventCallback.Invoke();
    }
}