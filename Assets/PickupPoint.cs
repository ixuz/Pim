using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupPoint : MonoBehaviour {

    public GameObject pickupItemInstance;

    [Header("Gizmos")]
    public Sprite unoccupiedIcon;
    public Sprite occupiedIcon;

    public bool AddItem(GameObject item) {
        if (item == null) return false;
        if (GetItem() != null) return false;

        item.transform.SetParent(transform);
        item.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
        item.transform.localScale = Vector3.one;
        pickupItemInstance = item;

        return true;
    }

    public bool RemoveItem() {
        if (pickupItemInstance != null) {
            pickupItemInstance = null;
            return true;
        }

        return false;
    }

    public GameObject GetItem() {
        return pickupItemInstance;
    }

    void OnDrawGizmos() {
        if (pickupItemInstance == null && unoccupiedIcon) {
            Gizmos.DrawIcon(transform.position, unoccupiedIcon.name);
        }
        if (pickupItemInstance != null && occupiedIcon) {
            Gizmos.DrawIcon(transform.position, occupiedIcon.name);
        }
    }
}
