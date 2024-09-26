using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] private int possibleItemDrop;
    [SerializeField] private Itemdata[] possibleDrop;
    private List<Itemdata> dropList=new List<Itemdata>();


    [SerializeField] private GameObject dropPrefab;


    public virtual void GenerateDrop() 
    {
        for (int i = 0; i < possibleDrop.Length; i++)
        {
            if (Random.Range(0, 100) <= possibleDrop[i].dropChance)
                dropList.Add(possibleDrop[i]);
        }

        for (int i = 0; i < possibleItemDrop; i++)
        {
            Itemdata randomItem = dropList[Random.Range(0,dropList.Count - 1)];
            
            dropList.Remove(randomItem);
            DropItem(randomItem);
        }

    }
    protected void DropItem(Itemdata _itemData) 
    {
        GameObject newDrop = Instantiate(dropPrefab,transform.position,Quaternion.identity);

        Vector2 randomVelocity = new Vector2(Random.Range(-5,5),Random.Range(20,30));

        newDrop.GetComponent<ItemObject>().SetupItem(_itemData, randomVelocity);
    }
}
