using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public string itemName; // Nombre del objeto
    public Sprite icon;     // La imagen (escoba)
}