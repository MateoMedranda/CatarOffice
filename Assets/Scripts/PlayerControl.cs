using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float velocidad = 5f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Al poner "Both" en los settings, esto volverá a funcionar:
        float moverX = Input.GetAxis("Horizontal");
        float moverZ = Input.GetAxis("Vertical");

        Vector3 movimiento = new Vector3(moverX, 0, moverZ) * velocidad;

        // Su corrección para Unity 6 es correcta:
        rb.linearVelocity = new Vector3(movimiento.x, rb.linearVelocity.y, movimiento.z);
    }
}