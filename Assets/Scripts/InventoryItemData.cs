using UnityEngine;


[CreateAssetMenu(fileName = "Inventory Item Data", menuName = "Inventory System/Create Item", order = 0)]
public class InventoryItemData : ScriptableObject
{
    public int maxStackSize = 3;
    public string id;
    public string itemName;
    public Sprite itemIcon;
    public GameObject itemPrefab;
}