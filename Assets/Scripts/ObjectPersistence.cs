using UnityEngine;

public class ObjectPersistence : MonoBehaviour
{
    [Header("Identificación")]
    [Tooltip("Pon un ID ÚNICO para este objeto. Ej: 'Escoba_Nivel1' o 'Luz_Pasillo'.")]
    public string objectID;

    [Header("Configuración")]
    [Tooltip("Si es TRUE, este objeto desaparecerá al iniciar si ya fue guardado antes.")]
    public bool disableOnStart = true;

    void Start()
    {
        // 1. Al iniciar la escena, preguntamos: "¿Este objeto ya fue usado?"
        if (QuestManager.Instance != null && QuestManager.Instance.IsQuestActive(objectID))
        {
            // CORREGIDO AQUÍ: UnityEngine.Debug
            UnityEngine.Debug.Log($"[MEMORIA] El objeto '{objectID}' ya fue guardado. Eliminando...");

            if (disableOnStart)
            {
                gameObject.SetActive(false); // ¡Puf! Desaparece
            }
        }
    }

    // Esta función la llamamos cuando recogemos el objeto o tocamos el interruptor
    public void SaveState()
    {
        if (QuestManager.Instance != null)
        {
            QuestManager.Instance.ActivateQuest(objectID);
            // CORREGIDO AQUÍ TAMBIÉN: UnityEngine.Debug
            UnityEngine.Debug.Log($"[MEMORIA] Estado guardado para: {objectID}");
        }
    }
}