[System.Serializable]
public class InventoryItem
{
    public InventoryItemData data;
    public int stackSize;

    public InventoryItem(InventoryItemData itemData)
    {
        data = itemData;
        stackSize = 1; // Inicializamos en 1 al crear el objeto
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