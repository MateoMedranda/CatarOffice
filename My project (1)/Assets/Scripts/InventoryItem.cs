[System.Serializable]
public class InventoryItem
{
    public InventoryItemData data;
    public int stackSize;

    public InventoryItem(InventoryItemData itemData)
    {
        data = itemData;
        AddStack(); // Inicia en 1
    }

    public void AddStack()
    {
        stackSize++; // <--- ESTO ES LO IMPORTANTE (Suma 1)
        // stackSize += stackSize; <--- ESTO ESTABA MAL (Bórralo)
    }

    public void RemoveFromStack()
    {
        stackSize--; // <--- ESTO ES LO IMPORTANTE (Resta 1)
        // stackSize -= stackSize; <--- ESTO ESTABA MAL (Bórralo)
    }
}