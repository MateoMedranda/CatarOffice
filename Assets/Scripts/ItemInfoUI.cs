using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemInfoUI : MonoBehaviour
{
    public static ItemInfoUI Instance;
    public GameObject infoPanel;
    public TextMeshProUGUI nameText;
    public Image iconImage;
    private ItemObject _currentItem;

    void Awake()
    {
        Instance = this;
        infoPanel.SetActive(false);
    }

    void Update()
    {
        // Si el panel está abierto, escuchamos la tecla L para cancelar
        if (infoPanel.activeSelf && Input.GetKeyDown(KeyCode.L))
        {
            ClosePanel();
        }
    }

    public void ShowItemInfo(ItemObject item)
    {
        _currentItem = item;
        nameText.text = item.itemData.itemName;
        iconImage.sprite = item.itemData.itemIcon;
        infoPanel.SetActive(true);

        // Opcional: Liberar mouse si es necesario, 
        // aunque ahora usas puro teclado (J y L)
    }

    public void OnSaveButtonClick()
    {
        if (_currentItem != null)
        {
            _currentItem.ConfirmPickup();
            ClosePanel();
        }
    }

    public void ClosePanel()
    {
        infoPanel.SetActive(false);
        _currentItem = null;
    }
}