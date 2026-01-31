
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI _itemName;

    [SerializeField] Image _itemIcon;

    [SerializeField] GameObject _stackObj;

    [SerializeField] TextMeshProUGUI _stackNumber;

    private InventoryItem currentItem;

    public void Set(InventoryItem item)
    {
        currentItem = item;
        _itemName.text = item.data.itemName;
        _itemIcon.sprite = item.data.itemIcon;

        if (item.stackSize <= 1)
        {
            _stackObj.SetActive(false);
        }
        else
        {
            _stackNumber.text = item.stackSize.ToString();
        }
        
        // Oscurecer el item al iniciar
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

