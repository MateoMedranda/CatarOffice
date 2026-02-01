using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [Header("Identificación")]
    [Tooltip("Ponle un nombre único, ej: 'EntradaDesdePasillo'")]
    public string idSpawn; 

    void OnDrawGizmos()
    {
        // Esto es solo para que usted vea una bolita azul en el editor y sepa dónde está el punto
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 1.0f); // Flecha de dirección
    }
}