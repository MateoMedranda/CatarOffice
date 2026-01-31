using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public GameObject itemSlotPrefab;

    [Header("Configuración Visual")]
    public Vector2 itemSize = new Vector2(85, 85); // Tu configuración de tamaño
    public float spacing = 20f; // <--- NUEVO: Espacio entre objetos

    void Start()
    {
        InventorySystem.Instance.onInventoryChangedCallback += OnUpdateInventory;

        // --- APLICAR ESPACIADO ---
        // Buscamos el componente que ordena (Horizontal Layout Group) y le ponemos tu valor
        HorizontalLayoutGroup layout = GetComponent<HorizontalLayoutGroup>();
        if (layout != null)
        {
            layout.spacing = spacing; 
        }
        
        // Actualizamos al inicio para que si está vacío, se oculte la barra negra
        OnUpdateInventory();
    }

    public void OnUpdateInventory()
    {
        // 1. Limpiamos los iconos viejos
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // 2. Controlar si se ve la barra negra de fondo
        // Si hay 0 items, apagamos la imagen de fondo. Si hay items, la encendemos.
        Image fondo = GetComponent<Image>();
        if (fondo != null)
        {
            fondo.enabled = (InventorySystem.Instance.Inventory.Count > 0);
        }

        // 3. Dibujamos los nuevos
        DrawInventory();
    }

    public void DrawInventory()
    {
        foreach (InventoryItem item in InventorySystem.Instance.Inventory)
        {
            AddInventorySlot(item);
        }
    }

    public void AddInventorySlot(InventoryItem item)
    {
        GameObject obj = Instantiate(itemSlotPrefab);
        obj.transform.SetParent(transform, false);

        // --- AQUÍ ESTÁ EL TRUCO PARA EL TAMAÑO (TU CÓDIGO ORIGINAL) ---
        RectTransform rt = obj.GetComponent<RectTransform>();
        if (rt != null)
        {
            rt.sizeDelta = itemSize; 
        }

        LayoutElement le = obj.GetComponent<LayoutElement>();
        if (le == null) le = obj.AddComponent<LayoutElement>();
        
        le.preferredWidth = itemSize.x;
        le.preferredHeight = itemSize.y;
        le.minWidth = itemSize.x;
        le.minHeight = itemSize.y;

        // Configuramos los datos del item
        ItemSlot slot = obj.GetComponent<ItemSlot>();
        slot.Set(item);
    }
}