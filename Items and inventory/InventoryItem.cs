using System;
[Serializable]
public class InventoryItem
{
    public Itemdata data;
    public int stackSize;

    public InventoryItem(Itemdata _newItemData)
    {
        data = _newItemData;
        //add to stack
        AddStack();

    }
    public void AddStack() => stackSize++;
    public void RemoveStack() => stackSize--;
}
