using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Thunder strike effect", menuName = "Data/Item effect/Thunder strike")]

public class ThunderStrike_Effect : ItemEffect
{
    [SerializeField] private GameObject thunderStrikePrefeb;
    public override void ExecuteEffect(Transform _enemyPosition)
    {
        GameObject newThunderStrike =  Instantiate(thunderStrikePrefeb,_enemyPosition.position,Quaternion.identity);
        Destroy(newThunderStrike,0.5f);
    }
}
