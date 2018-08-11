using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ItemType", menuName = "ItemType")]
public class ItemType : ScriptableObject {

    [Header("General properties")]
    public string itemName = "Unnamed ItemType";
    public Sprite sprite = null;
    public int value = 0;
}
