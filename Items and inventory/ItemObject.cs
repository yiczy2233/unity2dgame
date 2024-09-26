using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Vector2 velocity;
    [SerializeField] private Itemdata itemdata;

    private void SetupVisuals()
    {
        if (itemdata == null)
        {
            return;
        }

        GetComponent<SpriteRenderer>().sprite = itemdata.icon;
        gameObject.name = "Item object -" + itemdata.name;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            rb.velocity = velocity;
        }
    }
    public void SetupItem(Itemdata _itemData, Vector2 _velocity)
    {
        itemdata = _itemData;
        rb.velocity = _velocity;
        SetupVisuals();
    }


    public void PickupItem()
    {
        if (!Inventory.instance.CanAddItem() && itemdata.itemType == ItemType.Equipment) 
        {
            rb.velocity = new Vector2(0, 5);
          return;
        }
          
        Inventory.instance.AddItem(itemdata);
        rb.velocity = new Vector2(0, 5);
        Destroy(gameObject);
    }
}
