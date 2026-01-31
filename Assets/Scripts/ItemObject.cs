using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public InventoryItemData itemData;
    private bool _isPlayerNearby = false;

    private void Update()
    {
        // Si el jugador está cerca y presiona J
        if (_isPlayerNearby && Input.GetKeyDown(KeyCode.J))
        {
            HandleInteraction();
        }
    }

    private void HandleInteraction()
    {
        if (InventorySystem.Instance.CanAdd(itemData))
        {
            // Si la UI ya está abierta, J sirve para confirmar
            if (ItemInfoUI.Instance.infoPanel.activeSelf)
            {
                ItemInfoUI.Instance.OnSaveButtonClick();
            }
            else
            {
                // Si está cerrada, J la abre
                ItemInfoUI.Instance.ShowItemInfo(this);
            }
        }
        else
        {
            // Bloqueo físico si no hay espacio
            GetComponent<Collider>().isTrigger = false;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerNearby = true;
            // Aquí podrías mostrar un mensajito flotante: "Presiona J para interactuar"
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerNearby = false;
            ItemInfoUI.Instance.ClosePanel(); // Cerramos si se aleja
        }
    }

    public void ConfirmPickup()
    {
        InventorySystem.Instance.Add(itemData);
        Destroy(gameObject);
    }
}