using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public List<Item> Items = new List<Item>();

    public Transform ItemContent;
    public GameObject InventoryItem;
    public InventoryItemController[] InventoryItems;
    
    private void Awake()
    {
        Instance = this;
    }
    public void Add(Item item)
    {
        Items.Add(item);
    }

    public void Remove(Item item)
    {
        Items.Remove(item);
    }

    public int Count()
    {
        return Items.Count;
    }

    public void ListItems()
    {
        // Clear existing items first
        Clean();

        // Create new inventory items and set them up immediately
        for (int i = 0; i < Items.Count; i++)
        {
            GameObject obj = Instantiate(InventoryItem, ItemContent);
            var itemIcon = obj.transform.Find("ItemIcon").GetComponent<Image>();
            itemIcon.sprite = Items[i].icon;
            
            // Set up the controller right away
            var controller = obj.GetComponent<InventoryItemController>();
            if (controller != null)
            {
                controller.AddItem(Items[i]);
            }
        }

        // Update the InventoryItems array
        InventoryItems = ItemContent.GetComponentsInChildren<InventoryItemController>();
    }


    public void Clean()
    {
        foreach (Transform item in ItemContent)
        {
            Destroy(item.gameObject);
        }
    }

}
