using System.Diagnostics;
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
        // 4. Mover al jugador
        if (puntoCorrecto != null)
        {
            // Desactivar CharacterController momentáneamente si lo usa
            CharacterController cc = GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false;

            // A. POSICIÓN (Esto ya lo tenía bien)
            transform.position = puntoCorrecto.transform.position;

            // B. ROTACIÓN (¡BORRE ESTO! Causaba el error de la sombra)
            // transform.rotation = puntoCorrecto.transform.rotation; 

            // C. ORIENTACIÓN (¡NUEVO!) 
            // Buscamos el SpriteRenderer en los hijos (el dibujo del personaje)
            SpriteRenderer spriteVisual = GetComponentInChildren<SpriteRenderer>();

            if (spriteVisual != null)
            {
                // Le aplicamos el flip directamente según lo que diga la casilla del SpawnPoint
                spriteVisual.flipX = puntoCorrecto.mirarIzquierda;
                UnityEngine.Debug.Log($"[SPAWNER] Acomodando sprite. Mirar Izquierda: {puntoCorrecto.mirarIzquierda}");
            }

            if (cc != null) cc.enabled = true;

            UnityEngine.Debug.Log("¡Jugador movido al Spawn: " + idDestino + "!");
        }
        else
        {
            UnityEngine.Debug.LogWarning("No encontré ningún SpawnPoint con el ID: " + idDestino);
        }
    }
}