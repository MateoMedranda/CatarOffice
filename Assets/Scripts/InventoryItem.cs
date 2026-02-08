using UnityEngine;
using System;

[Serializable] // Importante para que se vea en el Inspector
public class InventoryItem
{
    public InventoryItemData data;
    public int stackSize; // <--- ESTO ES LO QUE NECESITAMOS PARA APILAR

    // Constructor que pide su sistema antiguo
    public InventoryItem(InventoryItemData source)
    {
        data = source;
        stackSize = 1; // Empieza con 1
    }

    public void AddStack()
    {
        stackSize++; // Suma 1 de forma correcta
    }

    public void RemoveFromStack()
    {
        stackSize--; // Resta 1 de forma correcta
    }
}