using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [Header("Identificación")]
    [Tooltip("Ponle un nombre único, ej: 'EntradaDesdePasillo'")]
    public string idSpawn;

    [Header("Configuración de Salida")]
    [Tooltip("Si marcas esto, el personaje mirará a la izquierda al aparecer")]
    public bool mirarIzquierda; // <--- LA NUEVA CASILLA

    void OnDrawGizmos()
    {
        // 1. Dibujamos la bolita azul (Posición)
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 0.5f);

        // 2. Dibujamos una FLECHA que indica hacia dónde mirará el jugador
        // Si 'mirarIzquierda' es true, la flecha apunta a la izquierda (-Vector3.right)
        Vector3 direccion = mirarIzquierda ? Vector3.left : Vector3.right;

        Gizmos.color = Color.yellow; // Flecha amarilla para dirección
        Gizmos.DrawRay(transform.position, direccion * 1.5f);

        // Punta de la flecha para que se vea bonito
        Gizmos.DrawRay(transform.position + direccion * 1.5f, (Vector3.up - direccion).normalized * 0.5f);
        Gizmos.DrawRay(transform.position + direccion * 1.5f, (-Vector3.up - direccion).normalized * 0.5f);
    }
}