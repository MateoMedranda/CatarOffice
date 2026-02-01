using UnityEngine;
using TMPro; 
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI textoPantalla;
    public GameObject panelDialogo;
    
    private bool isDialogueActive = false;
    public bool IsDialogueActive => isDialogueActive;

    // Variables para el efecto de escritura y nombre
    private string currentNPCName;
    private string currentNameColorHex;
    private Coroutine typingCoroutine;
    [SerializeField] private float typingSpeed = 0.05f; 

    private Coroutine animationCoroutine;
    private Queue<string> frases;
    public static DialogueManager instancia;

    private void Awake()
    {
        instancia = this;
        frases = new Queue<string>();
        panelDialogo.SetActive(false); 
        panelDialogo.transform.localScale = Vector3.zero; 
    }

    public void IniciarDialogo(DialogueData datos, string nombreNPC, Color colorNombre)
    {
        if (animationCoroutine != null) StopCoroutine(animationCoroutine);
        
        panelDialogo.SetActive(true);
        isDialogueActive = true;
        currentNPCName = nombreNPC;
        
        currentNameColorHex = "#" + ColorUtility.ToHtmlStringRGB(colorNombre);
        
        frases.Clear();
        textoPantalla.text = "";

        foreach (string frase in datos.frases)
        {
            frases.Enqueue(frase);
        }

        MostrarSiguienteFrase();
        
        animationCoroutine = StartCoroutine(AnimateScale(new Vector3(1.2f, 1.2f, 1.2f), Vector3.one, 0.3f));
    }

    public void OcultarDialogo()
    {
        if (animationCoroutine != null) StopCoroutine(animationCoroutine);
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);

        isDialogueActive = false;
        
        animationCoroutine = StartCoroutine(AnimateScale(Vector3.one, Vector3.zero, 0.3f, () => 
        {
            panelDialogo.SetActive(false);
        }));
    }

    public void MostrarSiguienteFrase()
    {
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);

        if (frases.Count == 0)
        {
            TerminarDialogo();
            return;
        }

        string fraseActual = frases.Dequeue();
        typingCoroutine = StartCoroutine(TypeSentence(fraseActual));
    }

    private System.Collections.IEnumerator TypeSentence(string sentence)
    {
        textoPantalla.text = ""; 
        
        string prefix = $"<color={currentNameColorHex}>{currentNPCName}:</color> ";
        textoPantalla.text = prefix;
        
        foreach (char letter in sentence.ToCharArray())
        {
            textoPantalla.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    private void TerminarDialogo()
    {
       OcultarDialogo();
    }

    private System.Collections.IEnumerator AnimateScale(Vector3 start, Vector3 end, float duration, System.Action onComplete = null)
    {
        float t = 0;
        panelDialogo.transform.localScale = start;
        
        while (t < 1)
        {
            t += Time.deltaTime / duration;
            float ease = t * t * (3f - 2f * t); 
            
            panelDialogo.transform.localScale = Vector3.Lerp(start, end, ease);
            yield return null;
        }
        
        panelDialogo.transform.localScale = end;
        onComplete?.Invoke();
    }
}