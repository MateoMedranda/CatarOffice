using UnityEngine;
using UnityEngine.Events;

public class Generic_NPC : MonoBehaviour, IInteractable {
    public DialogueData datos; 
    public string npcName = "NPC"; 
    public Color npcNameColor = Color.blue; // Default blue
    public UnityEvent alInteractuar; 

    public void Interactuar() {
        if (datos != null)
        {
            if (DialogueManager.instancia.IsDialogueActive)
            {
                DialogueManager.instancia.OcultarDialogo();
            }
            else
            {
                DialogueManager.instancia.IniciarDialogo(datos, npcName, npcNameColor);
                alInteractuar.Invoke();
            }
        }
    }
}