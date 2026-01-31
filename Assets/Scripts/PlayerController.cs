using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private int speed;
    private Rigidbody rb;
    private Vector3 movement;
    private PlayerControls playerControls;
    [Header("Configuración de Soltar (Drop)")]
    public Transform dropPoint;
    public InventoryItemData currentItem;

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
    }

    void Update()
    {
        float x = playerControls.Player.Move.ReadValue<Vector2>().x;
        float z = playerControls.Player.Move.ReadValue<Vector2>().y;

        Debug.Log(x + "," + z);

        movement = new Vector3(x, 0, z).normalized;
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

    void DropItem()
    {
        // Chequeo 1: ¿Tienes un punto de salida?
        if (dropPoint == null)
        {
            Debug.Log("ERROR: ¡El 'Drop Point' se ha borrado o no está asignado!");
            return;
        }

        // Chequeo 2: ¿Tienes algo en la mano?
        if (currentItem == null)
        {
            Debug.Log("ERROR: Tu mano está vacía (Current Item es 'None'). Arrastra un ítem al Player para probar.");
            return;
        }

        // Chequeo 3: ¿El ítem sabe qué forma tiene?
        if (currentItem.itemPrefab == null)
        {
            Debug.Log("ERROR: El ítem '" + currentItem.name + "' no tiene un Prefab asignado en su archivo.");
            return;
        }

        // --- SI LLEGAMOS AQUÍ, TODO ESTÁ BIEN ---
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
            Debug.LogError("ERROR CRÍTICO: No se encuentra el 'InventorySystem' en la escena.");
        }

        currentItem = null; // Vaciamos la mano
        Debug.Log("¡ÉXITO! Objeto soltado y borrado.");
    }
}
