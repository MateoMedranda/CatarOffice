using UnityEngine;

[CreateAssetMenu(fileName = "NuevoDialogo", menuName = "NPC/Dialogo")]
public class DialogueData : ScriptableObject {
    [TextArea(3, 10)]
    public string[] frases;
}