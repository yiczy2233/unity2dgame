using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData 
{
    public int currency;

    public SerializableDictionary<string, int> inventory;
    public List<string> equipmentId;

    public SerializableDictionary<string, bool> checkpoints;
    public string closestCheckpointId;

    public GameData() 
    {
        this.currency = 0;
        inventory = new SerializableDictionary<string, int>();
        equipmentId = new List<string>();

        closestCheckpointId = string.Empty;
        checkpoints = new SerializableDictionary<string, bool>();
    }
}