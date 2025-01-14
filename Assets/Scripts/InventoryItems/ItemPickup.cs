using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{

    public Item Item;

    void Pickup()
    {
        InventoryManager.Instance.Add(Item);
        Destroy(gameObject);
    }

    private void OnMouseDown() 
    {
        Debug.Log("Down");
        Pickup();    
    }
    
    void OnCollisionEnter(Collision collision)
    {
        // Code here executes when this object's collider 
        // first makes contact with another collider
        
        // You can access the other object through collision.gameObject
        Debug.Log("Collided with: " + collision.gameObject.name);
        
        // You can check tags like this:
        if (collision.gameObject.CompareTag("Player"))
        {
            // Do something when colliding with an object tagged as "Player"
            Debug.Log("Down");
            Pickup();            }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
