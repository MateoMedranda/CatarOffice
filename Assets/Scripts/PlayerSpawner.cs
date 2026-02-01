using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    void Awake()
    {
        // 1. Preguntar: ¿Hay algún destino pendiente en la memoria?
        string idDestino = DoorLogic.ProximoSpawnID;

        if (string.IsNullOrEmpty(idDestino))
        {
            // Si no hay destino (es la primera vez que jugamos), no hacemos nada.
            return; 
        }

        // 2. Buscar todos los SpawnPoints de la escena nueva
        SpawnPoint[] puntos = FindObjectsByType<SpawnPoint>(FindObjectsSortMode.None);
        SpawnPoint puntoCorrecto = null;

        // 3. Encontrar el que coincida con el ID
        foreach (SpawnPoint p in puntos)
        {
            if (p.idSpawn == idDestino)
            {
                puntoCorrecto = p;
                break;
            }
        }

        // 4. Mover al jugador
        if (puntoCorrecto != null)
        {
            // Desactivar CharacterController momentáneamente si lo usa (para evitar conflictos)
            CharacterController cc = GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false;

            // EL TELETRANSPORTE
            transform.position = puntoCorrecto.transform.position;
            transform.rotation = puntoCorrecto.transform.rotation; // Para que mire al lado correcto

            if (cc != null) cc.enabled = true;

            Debug.Log("¡Jugador movido al Spawn: " + idDestino + "!");
        }
        else
        {
            Debug.LogWarning("No encontré ningún SpawnPoint con el ID: " + idDestino);
        }
    }
}