using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private int speed;
    private Rigidbody rb;
    private Vector3 movement;
    private PlayerControls playerControls;
    [Header("Configuraci�n de Soltar (Drop)")]
    public Transform dropPoint;
    public InventoryItemData currentItem;
    private int selectedIndex = -1; // -1 significa sin selección

    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }


    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        currentItem = null; // Comenzamos sin item en la mano
    }

    void Update()
    {
        float x = playerControls.Player.Move.ReadValue<Vector2>().x;
        float z = playerControls.Player.Move.ReadValue<Vector2>().y;

        Debug.Log(x + "," + z);

        movement = new Vector3(x, 0, z).normalized;
        
        // --- SELECCIONAR ITEM CON 1, 2, 3 ---
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectInventoryItem(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectInventoryItem(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SelectInventoryItem(2);
        }
        
        // --- SOLTAR ---
        if (Input.GetKeyDown(KeyCode.G))
        {
            Debug.Log("--- INICIANDO DROP ---");
            DropItem();
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(transform.position + movement * speed * Time.fixedDeltaTime);
    }

    void SelectInventoryItem(int index)
    {
        if (InventorySystem.Instance == null || InventorySystem.Instance.Inventory.Count == 0)
        {
            Debug.Log("ERROR: El inventario está vacío.");
            return;
        }

        if (index >= InventorySystem.Instance.Inventory.Count)
        {
            Debug.Log($"ERROR: No hay item en posición {index + 1}. Solo tienes {InventorySystem.Instance.Inventory.Count} item(s).");
            return;
        }

        // Deseleccionar el anterior si había
        if (selectedIndex >= 0 && selectedIndex < InventorySystem.Instance.Inventory.Count)
        {
            HighlightInventoryItem(selectedIndex, false);
        }

        // Obtener el item del inventario por índice
        selectedIndex = index;
        InventoryItem selectedItem = InventorySystem.Instance.Inventory[index];
        currentItem = selectedItem.data;
        
        // Resaltar el item seleccionado
        HighlightInventoryItem(index, true);
        
        Debug.Log($"Item seleccionado: {currentItem.itemName} (Posición {index + 1})");
    }

    void HighlightInventoryItem(int index, bool highlight)
    {
        InventoryUI inventoryUI = FindObjectOfType<InventoryUI>();
        if (inventoryUI == null) return;

        // Obtener el transform del contenedor de items
        Transform inventoryContainer = inventoryUI.transform.Find("InventoryContainer");
        if (inventoryContainer == null)
        {
            inventoryContainer = inventoryUI.transform;
        }

        // Contar solo los ItemSlot (no otros componentes)
        int slotCount = 0;
        foreach (Transform child in inventoryContainer)
        {
            ItemSlot slot = child.GetComponent<ItemSlot>();
            if (slot != null)
            {
                if (slotCount == index)
                {
                    slot.SetDarkened(!highlight);
                    return;
                }
                slotCount++;
            }
        }
    }

    void DropItem()
    {
        // Chequeo 1: �Tienes un punto de salida?
        if (dropPoint == null)
        {
            Debug.Log("ERROR: �El 'Drop Point' se ha borrado o no est� asignado!");
            return;
        }

        // Chequeo 2: �Tienes algo en la mano?
        if (currentItem == null)
        {
            Debug.Log("ERROR: Tu mano est� vac�a (Current Item es 'None'). Arrastra un �tem al Player para probar.");
            return;
        }

        // Chequeo 3: �El �tem sabe qu� forma tiene?
        if (currentItem.itemPrefab == null)
        {
            Debug.Log("ERROR: El �tem '" + currentItem.name + "' no tiene un Prefab asignado en su archivo.");
            return;
        }

        // --- SI LLEGAMOS AQU�, TODO EST� BIEN ---
        Debug.Log("Todo correcto. Creando objeto...");

        Instantiate(currentItem.itemPrefab, dropPoint.position, dropPoint.rotation);

        // BORRAR DEL INVENTARIO
        if (InventorySystem.Instance != null)
        {
            Debug.Log("Solicitando borrar del inventario...");
            InventorySystem.Instance.Remove(currentItem);
        }
        else
        {
            Debug.LogError("ERROR CR�TICO: No se encuentra el 'InventorySystem' en la escena.");
        }

        currentItem = null; // Vaciamos la mano
        Debug.Log("��XITO! Objeto soltado y borrado.");
    }
}
