using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public InventoryItemData itemData;

    // --- EL CERROJO DE SEGURIDAD ---
    private bool yaFueRecogido = false;
    // -------------------------------

    private bool canPickup = false;

    private void Start()
    {
        StartCoroutine(EnablePickupDelay());
    }

    IEnumerator EnablePickupDelay()
    {
        yield return new WaitForSeconds(1.0f);
        canPickup = true;
    }

    public void OnHandlePickup(Player playerScript)
    {
        // 1. SI EL CERROJO ESTÁ CERRADO, PARAMOS AQUÍ
        if (yaFueRecogido == true)
        {
            return;
        }

        // 2. CERRAMOS EL CERROJO PARA SIEMPRE
        yaFueRecogido = true;

        // 3. Ahora sí, hacemos la lógica de inventario UNA SOLA VEZ
        if (InventorySystem.Instance != null)
        {
            InventorySystem.Instance.Add(itemData);

            if (playerScript != null)
            {
                playerScript.currentItem = itemData;
            }

            Debug.Log("¡ITEM RECOGIDO 1 VEZ!: " + itemData.itemName);

            // 4. Destruimos el objeto
            Destroy(gameObject);
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (!canPickup) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (player != null)
            {
                OnHandlePickup(player);
            }
        }
    }
}