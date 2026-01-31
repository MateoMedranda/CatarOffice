using UnityEngine;
using UnityEngine.Events;

public class Generic_NPC : MonoBehaviour {
    public DialogueData datos; 
    public UnityEvent alInteractuar; 

    public void Interactuar() {
        if (datos == null) return;

        foreach (string linea in datos.frases) {
            Debug.Log($"{gameObject.name} dice: {linea}");
        }
        
        alInteractuar.Invoke();
    }
}