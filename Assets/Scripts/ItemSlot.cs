
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI _itemName;

    [SerializeField] Image _itemIcon;

    [SerializeField] GameObject _stackObj;

 [SerializeField] TextMeshProUGUI _stackNumber;

    public void Set(InventoryItem item)
    {
        _itemName.text = item.data.itemName;
        _itemIcon.sprite = item.data.itemIcon;

        if (item.stackSize <= 1)
        {
            _stackObj.SetActive(false);
            return;

        }
        _stackNumber.text = item.stackSize.ToString();
    }
}
