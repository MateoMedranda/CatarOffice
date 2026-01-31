using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public InventoryItemData itemData;

    // Estado de interacción
    private bool _isPlayerNearby = false;
    private bool _canPickup = false; // Para el delay inicial

    // Referencia al script del jugador (para auto-equiparlo)
    private PlayerController _nearbyPlayerScript;

    private void Start()
    {
        // Del código 2: Espera 1 segundo antes de permitir interacción (evita clics accidentales al spawnear)
        StartCoroutine(EnablePickupDelay());
    }

    IEnumerator EnablePickupDelay()
    {
        yield return new WaitForSeconds(1.0f);
        _canPickup = true;
    }

    private void Update()
    {
        // Solo interactúa si: El jugador está cerca + Pasó el tiempo de espera + Presiona J
        if (_isPlayerNearby && _canPickup && Input.GetKeyDown(KeyCode.J))
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
                // Si está cerrada, J la abre para ver info
                ItemInfoUI.Instance.ShowItemInfo(this);
            }
        }
        else
        {
            // Bloqueo físico si no hay espacio en inventario
            GetComponent<Collider>().isTrigger = false;
            Debug.Log("Inventario lleno, el objeto se vuelve sólido.");
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerNearby = true;

            // Capturamos el script del Player al entrar en la zona para usarlo luego
            _nearbyPlayerScript = other.GetComponent<PlayerController>();

            // Aquí podrías mostrar un mensajito flotante: "Presiona J para interactuar"
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerNearby = false;
            _nearbyPlayerScript = null; // Limpiamos referencia
            ItemInfoUI.Instance.ClosePanel(); // Cerramos UI si se aleja
        }
    }

    // Esta función es llamada por el ItemInfoUI (botón Guardar) o al confirmar con J
    public void ConfirmPickup()
    {
        // 1. Guardar en el Inventario (Sistema General)
        InventorySystem.Instance.Add(itemData);

        // 2. ¡NUEVO! Poner en la mano del Player (Auto-equipar si es posible)
        // Usamos la referencia que capturamos en OnTriggerEnter
        if (_nearbyPlayerScript != null)
        {
            _nearbyPlayerScript.currentItem = itemData;
            Debug.Log($"¡Objeto {itemData.itemName} auto-equipado en la mano!");
        }

        // 3. Destruir el objeto del suelo
        Destroy(gameObject);
    }
}