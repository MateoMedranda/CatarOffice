using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem; 

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float radioInteraccion = 2f;
    [SerializeField] private Animator anim;
    [SerializeField] private SpriteRenderer playerSprite;
    
    private Rigidbody rb;
    private Vector3 movement;
    private PlayerControls playerControls;

    private const  string IS_WALK_PARAM = "IsWalk";

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
        playerControls.Player.interactuar.performed += AlInteractuar;
    }

    private void OnDisable()
    {
        playerControls.Player.interactuar.performed -= AlInteractuar;
        playerControls.Disable();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentItem = null; // Comenzamos sin item en la mano
        if (rb != null)
        {
            rb.useGravity = false;
            rb.constraints |= RigidbodyConstraints.FreezePositionY;
        }
    }

    private InteractionPrompt currentPrompt;

    void Update()
    {
        Vector2 inputVector = playerControls.Player.Move.ReadValue<Vector2>();
        movement = new Vector3(inputVector.x, 0, inputVector.y).normalized;

        anim.SetBool(IS_WALK_PARAM, movement != Vector3.zero);

        if (inputVector.x != 0 && inputVector.x < 0)
        {
            playerSprite.flipX = true;
        }

        if (inputVector.x != 0 && inputVector.x > 0)
        {
            playerSprite.flipX = false;
        }

        CheckForInteractionPrompts();
    }

    private void CheckForInteractionPrompts()
    {
        Collider[] cercanos = Physics.OverlapSphere(transform.position, radioInteraccion);
        InteractionPrompt closestPrompt = null;
        float closestDistance = float.MaxValue;

        foreach (var col in cercanos)
        {
            // Check if it's interactable AND has a prompt
            if (col.GetComponent<IInteractable>() != null)
            {
                if (col.TryGetComponent(out InteractionPrompt prompt))
                {
                    float d = Vector3.Distance(transform.position, col.transform.position);
                    if (d < closestDistance)
                    {
                        closestDistance = d;
                        closestPrompt = prompt;
                    }
                }
            }
        }

        // State machine for prompts
        if (closestPrompt != currentPrompt)
        {
            if (currentPrompt != null) currentPrompt.Hide();
            
            currentPrompt = closestPrompt;
            
            if (currentPrompt != null) currentPrompt.Show();
        }        
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
        if (Input.GetKeyDown(KeyCode.T))
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

        Vector3 dropPosition = dropPoint.position;
        dropPosition.y -= 0.75f; // Bajar el objeto 0.5 unidades m�s en el suelo
        Instantiate(currentItem.itemPrefab, dropPosition, dropPoint.rotation);

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

    private void AlInteractuar(InputAction.CallbackContext context)
    {
        Debug.Log("Intentando interactuar...");

        Collider[] cercanos = Physics.OverlapSphere(transform.position, radioInteraccion);
        
        foreach (var col in cercanos)
        {
            if (col.TryGetComponent(out IInteractable interactable))
            {
                interactable.Interactuar();
                break; 
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, radioInteraccion);
    }
}