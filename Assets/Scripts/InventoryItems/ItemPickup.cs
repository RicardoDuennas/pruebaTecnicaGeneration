using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ItemPickup : MonoBehaviour
{
    public Item Item;

    void Pickup()
    {
        Destroy(gameObject);
        if (InventoryManager.Instance.Count() < 10)
        {
            // Clean content before update list
            InventoryManager.Instance.Add(Item);
            InventoryManager.Instance.ListItems();            
        }
    }
    
    void OnCollisionEnter(Collision collision)
    {        
        if (collision.gameObject.CompareTag("Player"))
        {
            Pickup();
        }
    }
}
