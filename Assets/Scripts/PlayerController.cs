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
    }

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
    }

    private void FixedUpdate()
    {
        rb.MovePosition(transform.position + movement * speed * Time.fixedDeltaTime);
    }

    private void AlInteractuar(InputAction.CallbackContext context)
    {
        Debug.Log("Intentando interactuar...");

        Collider[] cercanos = Physics.OverlapSphere(transform.position, radioInteraccion);
        
        foreach (var col in cercanos)
        {
            if (col.TryGetComponent(out Generic_NPC npc))
            {
                npc.Interactuar();
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