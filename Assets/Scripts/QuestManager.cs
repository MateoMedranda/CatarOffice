using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    // Usamos un HashSet para búsqueda rápida. Guarda los "IDs" de las misiones completadas/activas.
    private HashSet<string> activeQuests = new HashSet<string>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Opcional: Para mantener estado entre escenas
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Activa una misión o marca un flag como verdadero.
    /// </summary>
    /// <param name="questName">ID único de la misión (ej: "BusquedaGrapadora")</param>
    public void ActivateQuest(string questName)
    {
        if (!activeQuests.Contains(questName))
        {
            activeQuests.Add(questName);
            Debug.Log($"[QuestManager] Misión activada: {questName}");
        }
    }

    /// <summary>
    /// Verifica si una misión está activa.
    /// </summary>
    public bool IsQuestActive(string questName)
    {
        return activeQuests.Contains(questName);
    }
    
    // Debug en inspector (opcional, básico)
    public List<string> debugActiveQuests;
    private void OnValidate()
    {
        // Solo para ver en inspector si queremos debuggear, no sync real en runtime bidireccional simple
    }
}
