

using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class ItemSlot : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI _itemName;

    [SerializeField] UnityEngine.UI.Image _itemIcon;

    [SerializeField] GameObject _stackObj;

    [SerializeField] TextMeshProUGUI _stackNumber;

    private InventoryItem currentItem;

    public void Set(InventoryItem item)
    {
        currentItem = item;

        // VERIFICACIÓN DE SEGURIDAD (Esto evita el error rojo si la casilla está vacía)
        if (_itemName != null)
        {
            _itemName.text = item.data.itemName;
        }

        if (_itemIcon != null)
        {
            _itemIcon.sprite = item.data.itemIcon;
        }

        // CAMBIO: Ahora solo se oculta si es 0 o menos. Si es 1, SE MUESTRA.
        if (item.stackSize <= 0)
        {
            if (_stackObj != null) _stackObj.SetActive(false);
        }
        else
        {
            if (_stackObj != null) _stackObj.SetActive(true);
            if (_stackNumber != null) _stackNumber.text = item.stackSize.ToString();
        }

        SetDarkened(true);
    }

    public void SetDarkened(bool isDarkened)
    {
        // Oscurecer el icon
        if (_itemIcon != null)
        {
            if (isDarkened)
            {
                _itemIcon.color = new Color(0.5f, 0.5f, 0.5f, 1f); // Gris oscuro
            }
            else
            {
                _itemIcon.color = Color.white; // Color normal
            }
        }

        // Oscurecer el nombre
        if (_itemName != null)
        {
            if (isDarkened)
            {
                _itemName.color = new Color(0.6f, 0.6f, 0.6f, 1f); // Gris oscuro
            }
            else
            {
                _itemName.color = Color.white; // Color normal
            }
        }
    }

    public InventoryItem GetItem()
    {
        return currentItem;
    }
}
