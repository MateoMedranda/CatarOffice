using UnityEngine;
using UnityEngine.Events;

public class Generic_NPC : MonoBehaviour, IInteractable {
    public DialogueData[] dialogos;
    public string npcName = "NPC"; 
    public Color npcNameColor = Color.blue;
    public UnityEvent alInteractuar; 

    [System.Serializable]
    public struct QuestDependency
    {
        [Tooltip("El índice de la LISTA DE DIALOGOS (Archivos) que requiere una misión. No es el índice de la frase.")]
        public int dialogueIndex;
        [Tooltip("El nombre exacto de la misión (String) que debe estar activa.")]
        public string requiredQuest;
    }

    [Header("Condiciones de Misión")]
    public System.Collections.Generic.List<QuestDependency> questDependencies;

    private int indiceDialogoActual = 0;

    public void Interactuar() {
        if (dialogos != null && dialogos.Length > 0)
        {
            if (DialogueManager.instancia.IsDialogueActive)
            {
                DialogueManager.instancia.MostrarSiguienteFrase();
            }
            else
            {
                // ACTUALIZACIÓN AUTOMÁTICA: Si ya tenemos la misión, saltamos al diálogo correspondiente.
                UpdateIndexBasedOnQuests();

                // Usar el índice actual
                int indiceAUsar = Mathf.Min(indiceDialogoActual, dialogos.Length - 1);
                
                DialogueManager.instancia.IniciarDialogo(dialogos[indiceAUsar], npcName, npcNameColor);
                
                // Lógica de avance condicional (para el siguiente)
                if (indiceDialogoActual < dialogos.Length - 1)
                {
                    // Revisamos si el SIGUIENTE índice (current + 1) tiene algún bloqueo
                    int nextIndex = indiceDialogoActual + 1;
                    if (CanAdvanceTo(nextIndex))
                    {
                        indiceDialogoActual++;
                    }
                    else
                    {
                        Debug.Log($"NPC {npcName}: No se puede avanzar al diálogo {nextIndex} por falta de misión.");
                    }
                }
                
                alInteractuar.Invoke();
            }
        }
    }

    // Verifica si se cumple la condición para un índice específico
    private bool CanAdvanceTo(int targetIndex)
    {
        if (questDependencies == null) return true;

        foreach (var dep in questDependencies)
        {
            if (dep.dialogueIndex == targetIndex)
            {
                // Este índice tiene un requisito. Chequeamos el manager.
                if (QuestManager.Instance == null) 
                {
                    Debug.LogWarning("QuestManager no existe en la escena!");
                    return false; 
                }

                if (!QuestManager.Instance.IsQuestActive(dep.requiredQuest))
                {
                    return false; // Bloqueado, falta la quest
                }
            }
        }
        return true; // No tiene bloqueos o ya se cumplieron
    }

    // Método público para avanzar al siguiente diálogo (lo llamas cuando entregue los archivos, o manual)
    public void AvanzarAlSiguienteDialogo()
    {
        if (indiceDialogoActual < dialogos.Length - 1)
        {
            int nextIndex = indiceDialogoActual + 1;
            // Opcional: ¿Queremos que este método fuerce el avance ignorando quests? 
            // Por ahora asumimos que si se llama manual, se quiere forzar. 
            // O podemos usar CanAdvanceTo(nextIndex) si queremos ser estrictos.
            indiceDialogoActual++; 
        }
    }

    // Revisa todas las dependencias y si alguna misión está activa, salta a ese índice.
    private void UpdateIndexBasedOnQuests()
    {
        if (questDependencies == null) return;

        foreach (var dep in questDependencies)
        {
            // Si la misión requerida está activa...
            if (QuestManager.Instance != null && QuestManager.Instance.IsQuestActive(dep.requiredQuest))
            {
                // ...y ese índice es mayor al actual...
                if (dep.dialogueIndex > indiceDialogoActual)
                {
                    // ¡Saltamos directamente a ese diálogo!
                    indiceDialogoActual = dep.dialogueIndex;
                }
            }
        }
    }
}