using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance;
    private Dictionary<InventoryItemData, InventoryItem> _itemDictionary = new Dictionary<InventoryItemData, InventoryItem>();
    public List<InventoryItem> Inventory;

    public void Awake()
    {
        Inventory = new List<InventoryItem>();
        _itemDictionary = new Dictionary<InventoryItemData, InventoryItem>();
        Instance = this;
    }

    public void Add(InventoryItemData itemData)
    {
        if (_itemDictionary.TryGetValue(itemData, out InventoryItem value))
        {
            Debug.Log("Sumar stack item");
            value.AddStack();
           // onInventoryChangedEventCallback.Invoke();
        }
        else
        {
            Debug.Log("Agregar un nuevo item");
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