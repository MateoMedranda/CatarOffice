using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public GameObject itemSlotPrefab;

    [Header("Configuración Visual")]
    public Vector2 itemSize = new Vector2(85, 85); // Tu configuración de tamaño
    public float spacing = 20f; // <--- NUEVO: Espacio entre objetos
    
    private TextMeshProUGUI instructionText;

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
        
        // Buscar el texto de instrucciones en el Image/Canvas
        instructionText = GetComponentInChildren<TextMeshProUGUI>();
        
        // Actualizamos al inicio para que si está vacío, se oculte la barra negra
        OnUpdateInventory();
    }

    public void OnUpdateInventory()
    {
        // 1. Limpiamos los iconos viejos
        foreach (Transform child in transform)
        {
            // No destruir el texto de instrucciones
            if (child.GetComponent<TextMeshProUGUI>() == null)
            {
                Destroy(child.gameObject);
            }
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
        int index = 1;
        foreach (InventoryItem item in InventorySystem.Instance.Inventory)
        {
            AddInventorySlot(item, index);
            index++;
        }
    }

    public void AddInventorySlot(InventoryItem item, int index)
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
        
        // Crear un fondo para el número en la esquina superior derecha
        GameObject numberBgGO = new GameObject("NumberBg");
        numberBgGO.transform.SetParent(obj.transform, false);
        
        RectTransform numberBgRt = numberBgGO.AddComponent<RectTransform>();
        numberBgRt.anchorMin = Vector2.one;
        numberBgRt.anchorMax = Vector2.one;
        numberBgRt.pivot = Vector2.one;
        numberBgRt.sizeDelta = new Vector2(45, 45);
        numberBgRt.anchoredPosition = new Vector2(-5, -5);
        
        // Agregar imagen de fondo circular
        Image bgImage = numberBgGO.AddComponent<Image>();
        bgImage.color = new Color(1f, 0.2f, 0.2f, 0.95f); // Rojo vibrante
        
        // Agregar layout group
        LayoutElement bgLayout = numberBgGO.AddComponent<LayoutElement>();
        bgLayout.preferredWidth = 45;
        bgLayout.preferredHeight = 45;
        
        // Crear el texto del número
        GameObject numberTextGO = new GameObject("NumberText");
        numberTextGO.transform.SetParent(numberBgGO.transform, false);
        
        TextMeshProUGUI numberText = numberTextGO.AddComponent<TextMeshProUGUI>();
        numberText.text = index.ToString();
        numberText.alignment = TextAlignmentOptions.Center;
        numberText.fontSize = 32;
        numberText.fontStyle = FontStyles.Bold;
        numberText.color = Color.white;
        
        // Agregar outline al texto
        var outline = numberTextGO.AddComponent<Outline>();
        outline.effectColor = new Color(0.2f, 0.2f, 0.2f, 1f); // Gris oscuro
        outline.effectDistance = new Vector2(1, -1);
        
        RectTransform numberTextRt = numberTextGO.GetComponent<RectTransform>();
        numberTextRt.anchorMin = Vector2.zero;
        numberTextRt.anchorMax = Vector2.one;
        numberTextRt.offsetMin = Vector2.zero;
        numberTextRt.offsetMax = Vector2.zero;
    }
}