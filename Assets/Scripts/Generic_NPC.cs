using UnityEngine;
using UnityEngine.Events;

public class Generic_NPC : MonoBehaviour, IInteractable {
    public DialogueData[] dialogos;
    public string npcName = "NPC"; 
    public Color npcNameColor = Color.blue;
    public UnityEvent alInteractuar; 

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
                // Usar el índice actual, pero no pasar del último
                int indiceAUsar = Mathf.Min(indiceDialogoActual, dialogos.Length - 1);
                
                DialogueManager.instancia.IniciarDialogo(dialogos[indiceAUsar], npcName, npcNameColor);
                
                // Solo avanzar si no estamos en el último diálogo
                if (indiceDialogoActual < dialogos.Length - 1)
                {
                    indiceDialogoActual++;
                }
                
                alInteractuar.Invoke();
            }
        }
    }

    // Método público para avanzar al siguiente diálogo (lo llamas cuando entregue los archivos)
    public void AvanzarAlSiguienteDialogo()
    {
        if (indiceDialogoActual < dialogos.Length - 1)
        {
            indiceDialogoActual++;
        }
    }
}