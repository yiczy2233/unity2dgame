using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Inventory : MonoBehaviour, ISaveManager
{
    public static Inventory instance;


    public List<Itemdata> startingItems;

    public List<InventoryItem> inventory;
    public Dictionary<Itemdata, InventoryItem> inventoryDictionary;

    public List<InventoryItem> equipment;
    public Dictionary<ItemData_Equipment, InventoryItem> equipmentDictionary;

    public List<InventoryItem> stash;
    public Dictionary<Itemdata, InventoryItem> stashDictionary;

    [Header("Inventory UI")]
    [SerializeField] private Transform inventorySlotParent;
    [SerializeField] private Transform stashSlotParent;
    [SerializeField] private Transform equipmentSlotParent;
    [SerializeField] private Transform statSlotParent;


    private UI_ItemSlot[] stashItemSlot;
    private UI_ItemSlot[] inventoryItemSlot;
    private UI_EquipmentSlot[] equipmentSlot;

    [Header("Items Cooldown")]
    private float flaskCoolDown;
    private float armorCooldown;

    private float lastTimeUsedFlask;
    private float lastTimeUseArmor;


    [Header("Data base")]
    private List<Itemdata> itemDataBase;
    public List<ItemData_Equipment> loadedEquipment;
    public List<InventoryItem> loadedItems;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    private void Start()
    {
        inventory = new List<InventoryItem>();
        inventoryDictionary = new Dictionary<Itemdata, InventoryItem>();

        stash = new List<InventoryItem>();
        stashDictionary = new Dictionary<Itemdata, InventoryItem>();

        equipment = new List<InventoryItem>();
        equipmentDictionary = new Dictionary<ItemData_Equipment, InventoryItem>();


        inventoryItemSlot = inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>();
        stashItemSlot = stashSlotParent.GetComponentsInChildren<UI_ItemSlot>();
        equipmentSlot = equipmentSlotParent.GetComponentsInChildren<UI_EquipmentSlot>();


        AddStartingItems();
    }

    private void AddStartingItems()
    {
        foreach (ItemData_Equipment item in loadedEquipment) 
        {
            EquipItem(item);
        }

        if (loadedItems.Count > 0) 
        {
            foreach (InventoryItem item in loadedItems)
            {
                for (int i = 0; i < item.stackSize; i++) 
                {
                    AddItem(item.data);
                }
            }
            return;
        }

        for (int i = 0; i < startingItems.Count; i++)
        {
            if (startingItems[i]!=null)
                AddItem(startingItems[i]);
        }
    }

    public void EquipItem(Itemdata _item)
    {
        ItemData_Equipment newEquipment = _item as ItemData_Equipment;
        InventoryItem newItem = new InventoryItem(newEquipment);
        ItemData_Equipment oldEquipment = null;

        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
        {
            if (item.Key.equipmentType == newEquipment.equipmentType)
                oldEquipment = item.Key;
        }
        if (oldEquipment != null)
        {
            UnequipItem(oldEquipment);
            AddItem(oldEquipment);
        }


        equipment.Add(newItem);
        equipmentDictionary.Add(newEquipment, newItem);
        newEquipment.AddModifiers();


        RemoveItem(_item);
        UpdateSlotUI();
    }

    public void UnequipItem(ItemData_Equipment itemToRemove)
    {
        if (equipmentDictionary.TryGetValue(itemToRemove, out InventoryItem value))
        {
            equipment.Remove(value);
            equipmentDictionary.Remove(itemToRemove);
            itemToRemove.RemoveModifiers();
        }
    }

    private void UpdateSlotUI()
    {
        for (int i = 0; i < equipmentSlot.Length; i++)
        {
            foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
            {
                if (item.Key.equipmentType == equipmentSlot[i].slotType)
                    equipmentSlot[i].UpdateSlot(item.Value);
            }
        }

        for (int i = 0; i < inventoryItemSlot.Length; i++)
        {
            inventoryItemSlot[i].CleanUpSlot();
        }
        for (int i = 0; i < stashItemSlot.Length; i++)
        {
            stashItemSlot[i].CleanUpSlot();
        }



        for (int i = 0; i < inventory.Count; i++)
        {
            inventoryItemSlot[i].UpdateSlot(inventory[i]);
        }
        for (int i = 0; i < stash.Count; i++)
        {
            stashItemSlot[i].UpdateSlot(stash[i]);
        }


    }
    public bool CanAddItem() 
    {
        if (inventory.Count >= inventoryItemSlot.Length) 
        {
            Debug.Log("no space");
             return false;
        }
        return true;
    }

    public void AddItem(Itemdata _item)
    {
        if (_item.itemType == ItemType.Equipment && CanAddItem())
        {
            AddToInventory(_item);
        }
        else if (_item.itemType == ItemType.Material)
        {
            AddToStash(_item);
        }

        UpdateSlotUI();
    }

    private void AddToStash(Itemdata _item)
    {
        if (stashDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            stash.Add(newItem);
            stashDictionary.Add(_item, newItem);
        }
    }

    private void AddToInventory(Itemdata _item)
    {
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            inventory.Add(newItem);
            inventoryDictionary.Add(_item, newItem);
        }
    }

    public void RemoveItem(Itemdata _item)
    {
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            if (value.stackSize <= 1)
            {
                inventory.Remove(value);
                inventoryDictionary.Remove(_item);
            }
            else
                value.RemoveStack();
        }
        if (stashDictionary.TryGetValue(_item, out InventoryItem stashValue))
        {
            if (stashValue.stackSize <= 1)
            {
                stash.Remove(stashValue);
                stashDictionary.Remove(_item);
            }
            else
                stashValue.RemoveStack();
        }
        UpdateSlotUI();
    }
    public List<InventoryItem> GetEqiupmentList() => equipment;
    public List<InventoryItem> GetStashList() => stash;
    public ItemData_Equipment GetEquipement(EquipmentType _type)
    {
        ItemData_Equipment equipedItem = null;
        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
        {
            if (item.Key.equipmentType == _type)
                equipedItem = item.Key;
        }
        return equipedItem;
    }

    public void UseFlask()
    {
        ItemData_Equipment currentFlask = GetEquipement(EquipmentType.Flask);

        if (currentFlask == null)
            return;

        bool canUseFlask = Time.time > lastTimeUsedFlask + flaskCoolDown;
        if (canUseFlask)
        {
            flaskCoolDown = currentFlask.itemCoolDwon;
            currentFlask.Effect(null);
            lastTimeUsedFlask = Time.time;
        }
        else
            Debug.Log("flask cooldown");
    }
    public bool CanUseArmor() 
    {
        ItemData_Equipment currentArmor = GetEquipement(EquipmentType.Armor);
        if (Time.time > lastTimeUseArmor + armorCooldown)
        {
            armorCooldown = currentArmor.itemCoolDwon;
            lastTimeUseArmor = Time.time;
            return true;
        }
        
            Debug.Log("Armor cooldowen");
            return false;
    }

    public void LoadData(GameData _data)
    {
        foreach (KeyValuePair<string,int> pair in _data.inventory) 
        {
            foreach (var item in GetItemDataBase()) 
            {
                if (item != null && item.itemId == pair.Key) 
                {
                    InventoryItem itemToLoad = new InventoryItem(item);
                    itemToLoad.stackSize = pair.Value;

                    loadedItems.Add(itemToLoad);
                }
            }
        }
        foreach (string loadedItemId in _data.equipmentId) 
        {
            foreach (var item in GetItemDataBase()) 
            {
                if (item != null && loadedItemId == item.itemId) 
                {
                    loadedEquipment.Add(item as ItemData_Equipment);
                }
            }
        }
    }

    public void SaveData(ref GameData _data)
    {
        _data.inventory.Clear();
        _data.equipmentId.Clear();

        foreach (KeyValuePair<Itemdata, InventoryItem> pair in inventoryDictionary) 
        {
            _data.inventory.Add(pair.Key.itemId, pair.Value.stackSize);
        }
        foreach (KeyValuePair<Itemdata, InventoryItem> pair in stashDictionary) 
        {
            _data.inventory.Add(pair.Key.itemId, pair.Value.stackSize);
        }
        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> pair in equipmentDictionary) 
        {
            _data.equipmentId.Add(pair.Key.itemId);
        
        }


    }

    private List<Itemdata> GetItemDataBase() 
    {
        itemDataBase = new List<Itemdata>();
        string[] assetName = AssetDatabase.FindAssets("", new[] { "Assets/Item/Items" });

        foreach (string SOName in assetName)
        {
            var SOpath = AssetDatabase.GUIDToAssetPath(SOName);
            var itemData = AssetDatabase.LoadAssetAtPath<Itemdata>(SOpath);
            itemDataBase.Add(itemData);
        }
        return itemDataBase;
    }
}
