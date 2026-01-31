using UnityEngine;
using TMPro; 
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI textoPantalla;
    public GameObject panelDialogo;
    
    private Queue<string> frases;
    public static DialogueManager instancia;

    private void Awake()
    {
        instancia = this;
        frases = new Queue<string>();
        panelDialogo.SetActive(false); 
    }

    public void IniciarDialogo(DialogueData datos)
    {
        panelDialogo.SetActive(true);
        frases.Clear();

        foreach (string frase in datos.frases)
        {
            frases.Enqueue(frase);
        }

        MostrarSiguienteFrase();
    }

    public void MostrarSiguienteFrase()
    {
        if (frases.Count == 0)
        {
            TerminarDialogo();
            return;
        }

        string fraseActual = frases.Dequeue();
        textoPantalla.text = fraseActual;
    }

    private void TerminarDialogo()
    {
        panelDialogo.SetActive(false);
    }
}