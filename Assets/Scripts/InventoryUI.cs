using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public GameObject itemSlotPrefab;
    private CanvasGroup canvasGroup;

    void Start()
    {
        InventorySystem.Instance.onInventoryChangedCallback += OnUpdateInventory;
        // Añadir CanvasGroup para controlar visibility
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
        // Inicialmente invisible si el inventario está vacío
        canvasGroup.alpha = InventorySystem.Instance.Inventory.Count > 0 ? 1f : 0f;
        
        // Eliminar LayoutGroup anterior si existe
        var existingLayout = GetComponent<LayoutGroup>();
        if (existingLayout != null)
        {
            DestroyImmediate(existingLayout);
        }
        
        // Crear nuevo GridLayoutGroup
        if (itemSlotPrefab != null)
        {
            var grid = gameObject.AddComponent<GridLayoutGroup>();
            grid.cellSize = new Vector2(160f, 160f); // cuadrado perfecto
            grid.spacing = new Vector2(20f, 20f); // espaciado
            grid.startCorner = GridLayoutGroup.Corner.UpperLeft;
            grid.startAxis = GridLayoutGroup.Axis.Horizontal; // items lado a lado
            grid.childAlignment = TextAnchor.UpperLeft;
            grid.constraint = GridLayoutGroup.Constraint.FixedRowCount;
            grid.constraintCount = 1; // Una fila: items se expanden horizontalmente
            grid.padding = new RectOffset(20, 20, 20, 20); // padding interno

            // ContentSizeFitter para expandir contenedor dinámicamente según items
            if (GetComponent<ContentSizeFitter>() == null)
            {
                var fitter = gameObject.AddComponent<ContentSizeFitter>();
                fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
                fitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize; // Crece en ambos ejes
            }
        }
    }

    public void OnUpdateInventory()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.transform.gameObject);
        }
        DrawInventory();
        // Mostrar/ocultar el contenedor según si hay items
        if (canvasGroup != null)
        {
            canvasGroup.alpha = InventorySystem.Instance.Inventory.Count > 0 ? 1f : 0f;
            canvasGroup.blocksRaycasts = InventorySystem.Instance.Inventory.Count > 0;
        }

        // Asegurar que el contenedor tenga tamaño mínimo adecuado
        var rt = GetComponent<RectTransform>();
        if (rt != null)
        {
            // Tamaño mínimo del contenedor
            if (rt.sizeDelta.x < 350f || rt.sizeDelta.y < 300f)
            {
                rt.sizeDelta = new Vector2(Mathf.Max(rt.sizeDelta.x, 350f), Mathf.Max(rt.sizeDelta.y, 300f));
            }
        }

        // Forzamos la reconstrucción del layout para asegurar posiciones correctas
        if (rt != null)
        {
            UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(rt);
        }
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
        // Asegurarnos de que la escala y el RectTransform sean correctos
        obj.transform.localScale = Vector3.one;
        var rt = obj.GetComponent<RectTransform>();
        if (rt != null)
        {
            rt.localScale = Vector3.one;
            // Configurar para que LayoutGroup controle completamente el tamaño
            rt.anchorMin = new Vector2(0.5f, 0.5f);
            rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.pivot = new Vector2(0f, 0f);
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
        }

        // Aplicar LayoutElement con tamaño fijo igual a cellSize del GridLayoutGroup
        var grid = GetComponent<GridLayoutGroup>();
        if (grid != null)
        {
            var le = obj.GetComponent<LayoutElement>();
            if (le == null)
            {
                le = obj.AddComponent<LayoutElement>();
            }
            le.preferredWidth = grid.cellSize.x;
            le.preferredHeight = grid.cellSize.y;
            le.minWidth = grid.cellSize.x;
            le.minHeight = grid.cellSize.y;
            le.flexibleWidth = 0;
            le.flexibleHeight = 0;

            // Asegurar que el RectTransform tenga el tamaño del cell
            if (rt != null)
            {
                rt.sizeDelta = grid.cellSize;
            }
        }

        ItemSlot slot = obj.GetComponent<ItemSlot>();
        slot.Set(item);
    }
}
