using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public InventoryItemData itemData;

    // Estado de interacci�n
    private bool _isPlayerNearby = false;
    private bool _canPickup = false; // Para el delay inicial

    // Referencia al script del jugador (para auto-equiparlo)
    private PlayerController _nearbyPlayerScript;

    private void Start()
    {
        // Del c�digo 2: Espera 1 segundo antes de permitir interacci�n (evita clics accidentales al spawnear)
        StartCoroutine(EnablePickupDelay());
    }

    IEnumerator EnablePickupDelay()
    {
        yield return new WaitForSeconds(1.0f);
        _canPickup = true;
    }

    private void Update()
    {
        // Solo interact�a si: El jugador est� cerca + Pas� el tiempo de espera + Presiona E
        if (_isPlayerNearby && _canPickup && Input.GetKeyDown(KeyCode.E))
        {
            HandleInteraction();
        }
    }

    private void HandleInteraction()
    {
        if (InventorySystem.Instance.CanAdd(itemData))
        {
            // Si la UI ya est� abierta, J sirve para confirmar
            if (ItemInfoUI.Instance.infoPanel.activeSelf)
            {
                ItemInfoUI.Instance.OnSaveButtonClick();
            }
            else
            {
                // Si est� cerrada, J la abre para ver info
                ItemInfoUI.Instance.ShowItemInfo(this);
            }
        }
        else
        {
            // Bloqueo f�sico si no hay espacio en inventario
            GetComponent<Collider>().isTrigger = false;
            AudioManager.Instance.PlaySFX("Error");
            UnityEngine.Debug.Log("Inventario lleno, el objeto se vuelve s�lido.");
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerNearby = true;

            // Capturamos el script del Player al entrar en la zona para usarlo luego
            _nearbyPlayerScript = other.GetComponent<PlayerController>();

            // Aqu� podr�as mostrar un mensajito flotante: "Presiona J para interactuar"
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

    // Esta funci�n es llamada por el ItemInfoUI (bot�n Guardar) o al confirmar con J
    public void ConfirmPickup()
    {
        // 1. Guardar en el Inventario (Sistema General)
        InventorySystem.Instance.Add(itemData);

        AudioManager.Instance.PlaySFX("Pickup");

        // 2. Poner en la mano del Player (Auto-equipar)
        if (_nearbyPlayerScript != null)
        {
            _nearbyPlayerScript.currentItem = itemData;
            UnityEngine.Debug.Log($"¡Objeto {itemData.itemName} auto-equipado en la mano!");
        }

        // --- BLOQUE NUEVO: AVISAR A LA MEMORIA ANTES DE MORIR ---
        ObjectPersistence memory = GetComponent<ObjectPersistence>();
        if (memory != null)
        {
            memory.SaveState(); // <--- ¡AQUÍ GUARDAMOS QUE YA SE RECOGIÓ!
        }
        // --------------------------------------------------------

        // 3. Destruir el objeto del suelo
        Destroy(gameObject);
    }
}