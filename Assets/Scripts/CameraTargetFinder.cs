using UnityEngine;
using Unity.Cinemachine; // OJO: Si usa Unity 6 o Cinemachine 3.x es Unity.Cinemachine. 
// Si usa versión vieja es 'using Cinemachine;'

public class CameraTargetFinder : MonoBehaviour
{
    [Tooltip("El Tag que debe buscar (normalmente 'Player')")]
    public string targetTag = "Player";

    void Start()
    {
        // 1. Buscamos al jugador por su etiqueta
        GameObject player = GameObject.FindGameObjectWithTag(targetTag);

        if (player != null)
        {
            // 2. Intentamos obtener el componente de Cinemachine
            // NOTA TÉCNICA: Dependiendo de su versión de Unity/Cinemachine, 
            // el componente se llama 'CinemachineVirtualCamera' o 'CinemachineCamera'.

            var cam = GetComponent<CinemachineCamera>(); // Para Unity 6 / CM 3.x
            // var cam = GetComponent<CinemachineVirtualCamera>(); // Para versiones viejas

            if (cam != null)
            {
                cam.Follow = player.transform;
                cam.LookAt = player.transform; // Opcional, si quiere que rote mirando al player
                UnityEngine.Debug.Log($"[CÁMARA] Objetivo encontrado y asignado: {player.name}");
            }
            else
            {
                UnityEngine.Debug.LogError("[CÁMARA] No se encontró el componente Cinemachine en este objeto.");
            }
        }
        else
        {
            UnityEngine.Debug.LogWarning($"[CÁMARA] No se encontró ningún objeto con el tag '{targetTag}' en la escena.");
        }
    }
}