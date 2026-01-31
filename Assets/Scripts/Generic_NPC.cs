using UnityEngine;
using UnityEngine.Events;

public class Generic_NPC : MonoBehaviour {
    public DialogueData datos; 
    public UnityEvent alInteractuar; 

    public void Interactuar() {
        if (datos != null)
    {
        DialogueManager.instancia.IniciarDialogo(datos);
        
        alInteractuar.Invoke();
    }
    }
}