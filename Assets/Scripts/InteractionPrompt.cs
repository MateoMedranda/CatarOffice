using UnityEngine;
using TMPro;

public class InteractionPrompt : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("Assign the Canvas or Panel containing the UI")]
    public GameObject uiPanel;
    [Tooltip("Optional: Assign a TextMeshPro component for the label")]
    public TextMeshProUGUI label;

    [Header("Settings")]
    public string promptText = "Interactuar";
    public float floatSpeed = 2f;
    public float floatAmplitude = 0.1f;

    private Vector3 startPos;
    private bool isShown = false;

    private void Start()
    {
        if (uiPanel != null)
        {
            // Cache starting position relative to parent or just local
            startPos = uiPanel.transform.localPosition;
            uiPanel.SetActive(false);
        }
        
        // If label is assigned, set initial text
        if (label != null) label.text = promptText;
    }

    private void Update()
    {
        if (isShown && uiPanel != null)
        {
            // Floating animation (Sine wave on Y axis)
            float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
            uiPanel.transform.localPosition = new Vector3(startPos.x, newY, startPos.z);
            
            // Optional: Face camera if needed (Billboard)
            // transform.LookAt(Camera.main.transform); 
        }
    }

    public void Show()
    {
        if (uiPanel != null)
        {
            uiPanel.SetActive(true);
            if (label != null) label.text = promptText; 
            isShown = true;
        }
    }

    public void Hide()
    {
        if (uiPanel != null)
        {
            uiPanel.SetActive(false);
            isShown = false;
        }
    }
}
