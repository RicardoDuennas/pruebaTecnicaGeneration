using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class InventoryItemController : MonoBehaviour
{
    Item item;
    // Start is called before the first frame update
    public void UseItem()
    {
        Vector3 position  = Grid3DPlayerController.Instance.GetPlayerPosition();
        switch (item.itemType)
        {
            case Item.ItemType.GreenPotion:
                FireManager.Instance.deleteAroundFires((int)position.x, (int)position.z);
                break;
            case Item.ItemType.RedPotion:
                FireManager.Instance.deleteAdjacentFires((int)position.x, (int)position.z);
                break;
            case Item.ItemType.BluePotion:
                FireManager.Instance.deleteFire((int)position.x, (int)position.z);
                break;
        }
        RemoveItem();
    }

    public void AddItem(Item newItem)
    {
        item = newItem;
    }

    public void RemoveItem()
    {
        InventoryManager.Instance.Remove(item);
        Destroy(gameObject);
    }
}
