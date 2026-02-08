using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class InventoryUI : MonoBehaviour
{
    [Header("Referencias OBLIGATORIAS")]
    public GameObject itemSlotPrefab;
    public Transform container; // <--- ¡RECUPERAMOS ESTA REFERENCIA VITAL!

    [Header("Configuración Visual")]
    public Vector2 itemSize = new Vector2(85, 85);
    public float spacing = 20f;

    private Canvas _myCanvas;

    void Awake()
    {
        // Buscamos el Canvas en este objeto o en los padres
        _myCanvas = GetComponent<Canvas>();
        if (_myCanvas == null) _myCanvas = GetComponentInParent<Canvas>();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        if (InventorySystem.Instance != null) InventorySystem.Instance.onInventoryChangedCallback += OnUpdateInventory;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        if (InventorySystem.Instance != null) InventorySystem.Instance.onInventoryChangedCallback -= OnUpdateInventory;
    }

    void Start()
    {
        // 1. CORRECCIÓN DE TIEMPOS (Race Condition Fix)
        // Nos aseguramos de conectarnos al sistema si llegamos tarde
        if (InventorySystem.Instance != null)
        {
            InventorySystem.Instance.onInventoryChangedCallback -= OnUpdateInventory;
            InventorySystem.Instance.onInventoryChangedCallback += OnUpdateInventory;
        }

        // 2. Pintar lo que haya (Borré la línea 'InitializeUI();' porque sobraba)
        OnUpdateInventory();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (_myCanvas != null && _myCanvas.renderMode == RenderMode.ScreenSpaceCamera)
        {
            _myCanvas.worldCamera = Camera.main;
        }
        OnUpdateInventory();
    }

    public void OnUpdateInventory()
    {
        // SEGURIDAD: Si no asignaste el contenedor, no hagas nada (evita errores rojos)
        if (container == null) return;

        // 1. Limpieza: Borrar SOLO lo que esté DENTRO del contenedor
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }

        if (InventorySystem.Instance == null) return;

        // 2. Dibujar los nuevos
        foreach (InventoryItem item in InventorySystem.Instance.Inventory)
        {
            AddInventorySlot(item);
        }
    }

    public void AddInventorySlot(InventoryItem item)
    {
        // A. Crear el slot DENTRO del contenedor
        GameObject obj = Instantiate(itemSlotPrefab, container);

        // B. Resetear escala y posición local para que no salga gigante ni lejos
        obj.transform.localScale = Vector3.one;
        obj.transform.localPosition = Vector3.zero;

        // C. Configurar tamaño
        RectTransform rt = obj.GetComponent<RectTransform>();
        if (rt != null) rt.sizeDelta = itemSize;

        LayoutElement le = obj.GetComponent<LayoutElement>();
        if (le == null) le = obj.AddComponent<LayoutElement>();

        le.preferredWidth = itemSize.x;
        le.preferredHeight = itemSize.y;
        le.minWidth = itemSize.x;
        le.minHeight = itemSize.y;

        // D. Rellenar datos
        ItemSlot slot = obj.GetComponent<ItemSlot>();
        if (slot != null)
        {
            slot.Set(item);
        }
    }
}